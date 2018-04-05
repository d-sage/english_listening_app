import React from 'react';
import {
  Image,
  Slider,
  StyleSheet,
  Text,
  TouchableHighlight,
  View,
  ScrollView,
} from 'react-native';
import { Audio, SQLite, FileSystem, Asset, Permissions } from 'expo';
import { NavigationActions } from 'react-navigation';
import styles from "./Styles.js";

class Icon {
  constructor(module, width, height) {
    this.module = module;
    this.width = width;
    this.height = height;
    Asset.fromModule(this.module).downloadAsync();
  }
}

const ICON_RECORD_BUTTON = new Icon(require('./assets/images/record_button.png'), 70, 119);
const ICON_RECORDING = new Icon(require('./assets/images/record_icon.png'), 20, 14);
const ICON_PLAY_BUTTON = new Icon(require('./assets/images/play_button.png'), 34, 51);
const ICON_PAUSE_BUTTON = new Icon(require('./assets/images/pause_button.png'), 34, 51);
const ICON_STOP_BUTTON = new Icon(require('./assets/images/stop_button.png'), 22, 22);
const ICON_DOWNLOAD_BUTTON = new Icon(require('./assets/images/download_button.png'), 10, 10);
const ICON_DELETE_BUTTON = new Icon(require('./assets/images/delete_button.png'), 10, 10);

const BACKGROUND_COLOR = '#87ceeb';
const DISABLED_OPACITY = 0.5;
const db = SQLite.openDatabase('db.db');
const resetAction = NavigationActions.reset({
  index: 0,
  actions: [NavigationActions.navigate({ routeName: 'Country' })],
});

export default class AudioPlayer extends React.Component{
	constructor(props){
		super(props);
		this.mounted = false;
		this.audio = null;
		this.recordingFinished = null;
		this.recording = null;
		this.audioIsSeeking = false;
		this.recordingIsSeeking = false;
		this.audioShouldPlayAtEndOfSeek = false;
		this.recordingShouldPlayAtEndOfSeek = false; 
		this.state = {
			isAudioPlaying: false,
			isRecordingPlaying: false,
			isLoading: false,
			isRecording: null,
			recordingDuration: null,
			recordingPosition: null,
			audioDuration: null,
			audioPosition: null,
			isPlaybackAllowed: false,
			audioShouldPlay: false,
			recordingShouldPlay: false,
			haveRecordingPermissions: false,
		};
		this.audioPlayPause = this.audioPlayPause.bind(this);
		this.audioStop = this.audioStop.bind(this);
		this.audioDownload = this.audioDownload.bind(this);
		this.audioDelete = this.audioDelete.bind(this);
		this.recordingPressed = this.recordingPressed.bind(this);
		this.recordingSettings = JSON.parse(JSON.stringify(Audio.RECORDING_OPTIONS_PRESET_HIGH_QUALITY));
	}
	
	componentDidMount() {
		this.mounted = true;
		if(this.audio == null)
			this.loadAudio();
		Expo.ScreenOrientation.allow(Expo.ScreenOrientation.Orientation.PORTRAIT_UP);
		this.askForPermissions();
	}
	
	componentWillUnmount() {
		this.mounted = false;
		if(this.audio != null){
			this.audio.stopAsync();
			this.audio.unloadAsync();
			this.audio = null;
		}
		if(this.recording != null){
			this.recording.stopAndUnloadAsync();
			this.recording = null;
		}
		if(this.recordingFinished != null){
			this.recordingFinished.stopAsync();
			this.recordingFinished.unloadAsync();
			this.recordingFinished = null;
		}
	}	

	askForPermissions = async () => {
		const response = await Permissions.askAsync(Permissions.AUDIO_RECORDING);
		this.setState({
			haveRecordingPermissions: response.status === 'granted',
		});
	};
  
	async loadAudio() {	
		this.setState({ isLoading: true, });
		this.audio = new Audio.Sound();
		try { 
			await this.audio.loadAsync({uri: this.props.navigation.state.params.path });
			await this.audio.setIsLoopingAsync(true);			
		} 
		catch (e) { alert('Error Loading Audio: ' + e); }
		this.setState({ isLoading: false, });
	}
	
	audioPlayPause() {
		if(this.audio != null){
			if(this.state.isAudioPlaying){
				this.audio.pauseAsync();
				this.setState({ isAudioPlaying: false, });
			}
			else{
				this.audio.playAsync();
				this.setState({ isAudioPlaying: true, });
			}
			this.audio.setOnPlaybackStatusUpdate(this.updateAudioStatus);
		}
	}
	
	updateAudioStatus = status => {
		if(this.mounted){
			this.setState({
				audioDuration: status.durationMillis,
				audioPosition: status.positionMillis,
				audioShouldPlay: status.shouldPlay,
			});
		}
	}
	
	audioStop(){
		if(this.audio != null){
			this.audio.stopAsync();
			this.setState({ isAudioPlaying: false, });
		}
	}
	
	audioDownload(){
		db.transaction(tx => {
			tx.executeSql('INSERT OR IGNORE INTO lessons (cid, gid, tid, lid, text, path) values (?, ?, ?, ?, ?, ?)', [this.props.navigation.state.params.country, this.props.navigation.state.params.grade, this.props.navigation.state.params.topic, this.props.navigation.state.params.lid, this.props.navigation.state.params.textSubs, FileSystem.documentDirectory + this.props.navigation.state.params.name]);
		});
		alert('Downloading File, Please wait. You will get an alert when download has finished.');
		FileSystem.downloadAsync(this.props.navigation.state.params.path,
			FileSystem.documentDirectory + this.props.navigation.state.params.name ).then(({ uri }) => {
				alert('Finished Downloading To Directory:\n ' + uri);
		}).catch(error => { alert("ERROR Downloading Audio: " + error); });
	}
	
	audioDelete() {
		this.audioStop();
		db.transaction(tx => {
			tx.executeSql('DELETE FROM lessons WHERE cid = ? AND gid = ? AND tid = ? AND lid = ? AND path = ?;', [this.props.navigation.state.params.country, this.props.navigation.state.params.grade, this.props.navigation.state.params.topic, this.props.navigation.state.params.lid, this.props.navigation.state.params.path]);
		});
		FileSystem.deleteAsync( FileSystem.documentDirectory + this.props.navigation.state.params.name, {idempotent: true} );
		this.props.navigation.dispatch(resetAction);
		alert('Finished Deleting Audio File');
	}
	
	audioRemaining(){
		if(this.audio != null)
			return this.getMMSSFromMillis(this.state.audioDuration - this.state.audioPosition);
		return this.getMMSSFromMillis(0);
	}
	
	audioSeekSliderPosition() {
		if ( this.audio != null && this.state.audioPosition != null)
			return this.state.audioPosition / this.state.audioDuration;
		return 0;
	}
	
	audioSeekSliderValueChange = value => {
		if (this.audio != null && !this.audioIsSeeking) {
			this.audioIsSeeking = true;
			this.audioShouldPlayAtEndOfSeek = this.state.audioShouldPlay;
			this.audio.pauseAsync();
		}
	};

	audioSeekSlidingComplete = async value => {
		if (this.audio != null) {
			this.audioIsSeeking = false;
			const seekPosition = value * this.state.audioDuration;
			if (this.audioShouldPlayAtEndOfSeek)
				this.audio.playFromPositionAsync(seekPosition);
			else
				this.audio.setPositionAsync(seekPosition);
		}
	};
	
	recordingSeekSliderValueChange = value => {
		if (this.recordingFinished != null && !this.recordingIsSeeking) {
			this.recordingIsSeeking = true;
			this.recordingShouldPlayAtEndOfSeek = this.state.recordingShouldPlay;
			this.recordingFinished.pauseAsync();
		}
	};
	
	recordingFinishedSeekSliderPosition() {
		if ( this.recordingFinished != null && this.state.recordingPosition != null)
			return this.state.recordingPosition / this.state.recordingDuration;
		return 0;
	}
	
	recordingFinishedSeekSlidingComplete = async value => {
		if (this.recordingFinished != null) {
			this.recordingIsSeeking = false;
			const seekPosition = value * this.state.recordingDuration;
			if (this.recordingShouldPlayAtEndOfSeek)
				this.recordingFinished.playFromPositionAsync(seekPosition);
			else
				this.recordingFinished.setPositionAsync(seekPosition);
		}
	};
	
	async recordingStart() {
		this.setState({ isLoading: true, });
		if (this.recordingFinished !== null) {
			await this.recordingFinished.unloadAsync();
			this.recordingFinished.setOnPlaybackStatusUpdate(null);
			this.recordingFinished = null;
		}
		await Audio.setAudioModeAsync({
			allowsRecordingIOS: true,
			interruptionModeIOS: Audio.INTERRUPTION_MODE_IOS_DO_NOT_MIX,
			playsInSilentModeIOS: true,
			shouldDuckAndroid: true,
			interruptionModeAndroid: Audio.INTERRUPTION_MODE_ANDROID_DO_NOT_MIX,
		});
		if (this.recording !== null) {
			this.recording.setOnRecordingStatusUpdate(null);
			this.recording = null;
		}

		const recording = new Audio.Recording();
		await recording.prepareToRecordAsync(this.recordingSettings);
		recording.setOnRecordingStatusUpdate(this.updateRecordingStatus);

		this.recording = recording;
		await this.recording.startAsync(); // Will call this.updateRecordingStatus to update the screen.
		this.setState({ isLoading: false, });
	}
  
	async recordingStop() {
		this.setState({ isLoading: true, });
		try { await this.recording.stopAndUnloadAsync(); } 
		catch (error) {}// Do nothing -- we are already unloaded.
		await Audio.setAudioModeAsync({
			allowsRecordingIOS: false,
			interruptionModeIOS: Audio.INTERRUPTION_MODE_IOS_DO_NOT_MIX,
			playsInSilentModeIOS: true,
			playsInSilentLockedModeIOS: true,
			shouldDuckAndroid: true,
			interruptionModeAndroid: Audio.INTERRUPTION_MODE_ANDROID_DO_NOT_MIX,
		});
		const { sound, status } = await this.recording.createNewLoadedSound(
			{ isLooping: true, },
			this.updateRecordingFinishedStatus
		);
		this.recordingFinished = sound;
		this.setState({ isLoading: false, });
		this.recording = null;
	}
  
	recordingPressed(){
		if (this.state.isRecording)
			this.recordingStop();
     	else
			this.recordingStart();
	}
	
	recordingFinishedPlayPause = () => {
		if (this.recordingFinished != null) {
			if (this.state.isRecordingPlaying)
				this.recordingFinished.pauseAsync();
			else
				this.recordingFinished.playAsync();  
		}
	};
	
	recordingFinishedStop = () => {
		if (this.recordingFinished != null)
			this.recordingFinished.stopAsync();
	};
  
    updateRecordingStatus = status => {
		if(this.mounted){
			if (status.canRecord) {
				this.setState({
					isRecording: status.isRecording,
					recordingDuration: status.durationMillis,
				});
			} 
			else if (status.isDoneRecording) {
				this.setState({
					isRecording: false,
					recordingDuration: status.durationMillis,
				});
				if (!this.state.isLoading) {
					this.recordingStop();
				}
			}
		}
	};
	
	updateRecordingFinishedStatus = status => {
		if(this.mounted){
			if (status.isLoaded) {
				this.setState({
					recordingPosition: status.positionMillis, 
					recordingDuration: status.durationMillis,
					recordingShouldPlay: status.shouldPlay,
					isRecordingPlaying: status.isPlaying,  
					isPlaybackAllowed: true,
				});
			} 
			else {
				this.setState({
					recordingDuration: null,
					recordingPosition: null,
					isPlaybackAllowed: false,
				});
				if (status.error)
					console.log('FATAL PLAYER ERROR: ', status.error);
			}
		}
	};
	
	recordingRemaining(){
		if(this.recordingFinished != null)
			return this.getMMSSFromMillis(this.state.recordingDuration - this.state.recordingPosition);
		return this.getMMSSFromMillis(0);
	}
	
	getRecordingTimestamp() {
		if (this.state.recordingDuration != null && this.state.isRecording)
			return this.getMMSSFromMillis(this.state.recordingDuration);
		return this.getMMSSFromMillis(0);
	}
	
	getMMSSFromMillis(millis) {
		const totalSeconds = millis / 1000;
		const seconds = Math.floor(totalSeconds % 60);
		const minutes = Math.floor(totalSeconds / 60);

		const padWithZero = number => {
			const string = number.toString();
			if (number < 10)
				return '0' + string;
			return string;
		};
		return padWithZero(minutes) + ':' + padWithZero(seconds); 
	}
	
	render(){
		var texts;
		if(this.props.navigation.state.params.textSubs == 'undefined') 
			texts = "No Text To Diaplay, so here's some advice!\n" +
			"10 REASONS YOU KNOW YOU BOUGHT A BAD COMPUTER\n"+
			"1. Lower corner of screen has the words Etch-a-sketch on it. \n"+
			"2. It's celebrity spokesman is that Hey Vern! guy. \n"+
			"3. In order to start it you need some jumper cables and a friend's car. \n"+
			"4. It's slogan is Pentium: redefining mathematics. \n"+
			"5. The quick reference manual is 120 pages long. \n"+
			"6. Whenever you turn it on, all the dogs in your neighborhood start howling. \n"+
			"7. The screen often displays the message, Ain't it break time yet? \n"+
			"8. The manual contains only one sentence: Good Luck! \n"+
			"9. The only chip inside is a Dorito. \n"+
			"10. You've decided that your computer is an excellent addition to your fabulous paperweight collection.";
		else
			texts = this.props.navigation.state.params.textSubs;
		if(!this.state.haveRecordingPermissions)
			return(<View><Text>You must enable permissions to use this app.</Text></View>);
		return(
			<View>
				<View style={styles.textContainer}>
					<ScrollView contentContainerStyle={{flexGrow:1}}>
						<Text style={styles.textBox}>{texts}</Text>
					</ScrollView>
				</View>
				<View style={styles.playerContainer}>
					<Slider
						style={styles.playbackSlider}
						value={this.audioSeekSliderPosition()}
						onValueChange={this.audioSeekSliderValueChange}
						onSlidingComplete={this.audioSeekSlidingComplete}
					/>  
					<Text>{this.audioRemaining()}</Text>
					<View style={styles.buttonsContainer}>
						<View style={styles.downloadContainer}>
							<View style={styles.buttonPlayerContainer}>
								<TouchableHighlight
									underlayColor={BACKGROUND_COLOR}
									style={styles.wrapper}
									onPress={this.props.navigation.state.params.connected ? this.audioDownload : this.audioDelete}
									disabled={this.state.isLoading}>
									<Image style={styles.image} source={this.props.navigation.state.params.connected ? ICON_DOWNLOAD_BUTTON.module : ICON_DELETE_BUTTON.module} />
								</TouchableHighlight>
								<Text>{this.props.navigation.state.params.connected ? 'Download' : 'Delete'}</Text>
							</View>
						</View>
						<View style={styles.playStopContainer}>
							<View style={styles.buttonPlayerContainer}>
								<TouchableHighlight
									underlayColor={BACKGROUND_COLOR}
									style={styles.wrapper}
									onPress={this.audioPlayPause}
									disabled={this.state.isLoading}>
									<Image style={styles.image}
										source={ this.state.isAudioPlaying ? ICON_PAUSE_BUTTON.module : ICON_PLAY_BUTTON.module }
									/>
								</TouchableHighlight>
								<Text>{this.state.isAudioPlaying ? 'Pause' : 'Play'}</Text>
							</View>
							<View style={styles.buttonPlayerContainer}>
								<TouchableHighlight
									underlayColor={BACKGROUND_COLOR}
									style={styles.wrapper}
									onPress={this.audioStop}
									disabled={this.state.isLoading}>
									<Image style={styles.image} source={ICON_STOP_BUTTON.module}/>
								</TouchableHighlight>
								<Text>{'Stop'}</Text>
							</View>
						</View>
					</View>
				</View>
				<View style={styles.playerContainer}>
					<Slider
						style={styles.playbackSlider}
						value={this.recordingFinishedSeekSliderPosition()}
						onValueChange={this.recordingSeekSliderValueChange}
						onSlidingComplete={this.recordingFinishedSeekSlidingComplete}
						disabled={!this.state.isPlaybackAllowed || this.state.isLoading}
					/>
					<Text style={{opacity: this.recordingFinished != null ? 1.0 : 0.0 }}>{this.recordingRemaining()}</Text>
					<View style={styles.buttonsContainer}>
						<View style={styles.downloadRecordContainer}>
							<View style={styles.buttonPlayerContainer}>
								<TouchableHighlight
									underlayColor={BACKGROUND_COLOR}
									style={styles.wrapper}
									onPress={this.recordingPressed}>
									<Image style={styles.image} source={this.state.isRecording ? ICON_RECORDING.module : ICON_RECORD_BUTTON.module }/>
								</TouchableHighlight>
								<Text>{this.state.isRecording ? 'Stop' : 'Record'}</Text>
							</View>
							<View style={styles.recordingContainer}>
								<Text> {this.state.isRecording ? 'RECORDING:' : ''} </Text>
								<Text style={{opacity: this.state.isRecording ? 1.0 : 0.0 }}>{this.getRecordingTimestamp()}</Text>
							</View>
						</View>
						<View style={[styles.playStopContainer, { opacity: !this.state.isPlaybackAllowed || this.state.isLoading ? DISABLED_OPACITY : 1.0, },]}>
							<View style={styles.buttonPlayerContainer}>
								<TouchableHighlight
									underlayColor={BACKGROUND_COLOR}
									style={styles.wrapper}
									onPress={this.recordingFinishedPlayPause}
									disabled={!this.state.isPlaybackAllowed || this.state.isLoading}>
									<Image style={styles.image}
										source={ this.state.isRecordingPlaying ? ICON_PAUSE_BUTTON.module : ICON_PLAY_BUTTON.module }
									/>
								</TouchableHighlight>
								<Text>{this.state.isRecordingPlaying ? 'Pause' : 'Play'}</Text>
							</View>
							<View style={styles.buttonPlayerContainer}>
								<TouchableHighlight
									underlayColor={BACKGROUND_COLOR}
									style={styles.wrapper}
									onPress={this.recordingFinishedStop}
									disabled={!this.state.isPlaybackAllowed || this.state.isLoading}>
									<Image style={styles.image} source={ICON_STOP_BUTTON.module}/>
								</TouchableHighlight>
								<Text>{'Stop'}</Text>
							</View>
						</View>
					</View>
				</View>
			</View>
		)
	}
}