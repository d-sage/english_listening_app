import React from 'react';
import {
  Image,
  Slider,
  Text,
  TouchableHighlight,
  View,
  ScrollView,
  Platform,
  LayoutAnimation, 
  UIManager, 
  TouchableOpacity,
  Button,
  NetInfo,
} from 'react-native';
import { Audio, SQLite, FileSystem } from 'expo';
import { NavigationActions, StackActions } from 'react-navigation';
import styles from "./Styles.js";

const ICON_RECORD_BUTTON = require('./assets/images/record_button.png');
const ICON_RECORDING = require('./assets/images/record_icon.png');
const ICON_PLAY_BUTTON = require('./assets/images/play_button.png');
const ICON_PAUSE_BUTTON = require('./assets/images/pause_button.png');
const ICON_STOP_BUTTON = require('./assets/images/stop_button.png');
const ICON_DOWNLOAD_BUTTON = require('./assets/images/download_button.png');
const ICON_DELETE_BUTTON = require('./assets/images/delete_button.png');
const ICON_UP_RATE_BUTTON = require('./assets/images/rateUp_button.png');
const ICON_DOWN_RATE_BUTTON = require('./assets/images/rateDown_button.png');
const ICON_RESET_RATE_BUTTON = require('./assets/images/rateReset_button.png');
const ICON_SAVE_BUTTON = require('./assets/images/save_button.png');
const ICON_OPEN_BUTTON = require('./assets/images/open_button.png');
const ICON_REFRESH_BUTTON = require('./assets/images/refresh_button.png');

const BACKGROUND_COLOR = '#87ceeb';
const DISABLED_OPACITY = 0.5;
const RATE_MAX = 2.0;
const RATE_MIN = 0.5;
const MAX_SAVES = 15;
const MAX_RECORD_TIME = 5 * (60*1000);//minutes to milliseconds.
const db = SQLite.openDatabase('db.db');
const resetActionCountry = StackActions.reset({
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
			rate: 1.0,
			onLayoutPlayerHeight: 0,
			onLayoutRecorderHeight: 0,			
			modifiedRecorderHeight: 0,
			modifiedPlayerHeight: 0,
			expandedRecorder: false,
			expandedPlayer: false,
			recordings: [],
		};
		this.audioPlayPause = this.audioPlayPause.bind(this);
		this.audioStop = this.audioStop.bind(this);
		this.audioDownload = this.audioDownload.bind(this);
		this.audioDelete = this.audioDelete.bind(this);
		this.recordingPressed = this.recordingPressed.bind(this);
		this.setRateUp = this.setRateUp.bind(this);
		this.setRateDown = this.setRateDown.bind(this);
		this.setRateDefault = this.setRateDefault.bind(this);
		this.openRecordings = this.openRecordings.bind(this);
		this.recordingSave = this.recordingSave.bind(this);
		this.recordingSettings = JSON.parse(JSON.stringify(Audio.RECORDING_OPTIONS_PRESET_HIGH_QUALITY));
		if( Platform.OS === 'android' )
			UIManager.setLayoutAnimationEnabledExperimental( true )
	}
	
	componentDidMount() {
		this.mounted = true;
		if(Platform.OS == 'ios'){		
			NetInfo.isConnected.addEventListener('connectionChange', this.handleConnectionChange);
		}
		else if(Platform.OS == 'android'){
			NetInfo.isConnected.fetch().done(
				(isConnected) => { 
					if(isConnected != this.props.navigation.state.params.connected){
						if(isConnected)
							alert("Data Connected");
						else
							alert("Data Not Connected");
						this.props.navigation.dispatch(resetActionCountry);
						this.componentWillUnmount();
					}			
				}
			);
		}
		else
			alert('Only Android and IOS are supported');
		this.loadAudio();
		Expo.ScreenOrientation.allow(Expo.ScreenOrientation.Orientation.PORTRAIT_UP);
		this.openRecordings();
	}
	
	componentWillUnmount() {
		this.mounted = false;
		if(this.audio != null){
			this.audioStop();
			this.audio.unloadAsync();
			this.audio = null;
		}
		if(this.recording != null){
			this.recording = null;
		}
		if(this.recordingFinished != null){
			this.recordingFinished.stopAsync();
			this.recordingFinished.unloadAsync();
			this.recordingFinished = null; 
		}
		if(Platform.OS == 'ios')
			NetInfo.isConnected.removeEventListener('connectionChange', this.handleConnectionChange);
	}	
	
	handleConnectionChange = (isConnected) => {
		if(isConnected != this.props.navigation.state.params.connected && !this.props.navigation.state.params.fromRecording){
			if(isConnected)
				alert("Online");
			else
				alert("Offline");
			this.props.navigation.dispatch(resetActionCountry);
			this.componentWillUnmount();
			
		}
	}
  
	async loadAudio() {	
		this.setState({ isLoading: true, });
		this.audio = new Audio.Sound();
		try { 
			await this.audio.loadAsync({uri: this.props.navigation.state.params.path });
			await this.audio.setIsLoopingAsync(true);			
		} 
		catch (e) {  }
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
			
			if(Platform.OS == 'android'){
				NetInfo.isConnected.fetch().done(
					(isConnected) => { 
						if(isConnected != this.props.navigation.state.params.connected && !this.props.navigation.state.params.fromRecording){
							if(isConnected)
								alert("Online");
							else
								alert("Offline");
							this.props.navigation.dispatch(resetActionCountry);
							this.componentWillUnmount();
						}			
					}
				);
			}
			
		}
	}
	
	audioStop(){
		if(this.audio != null){
			this.audio.stopAsync();
			this.setState({ isAudioPlaying: false, });
		}
	}
	
	audioDownload(){
		if(!this.props.navigation.state.params.fromRecording){
			db.transaction(tx => {
				tx.executeSql('INSERT OR IGNORE INTO lessons (cid, gid, tid, lid, filename, text, path, ext) values (?, ?, ?, ?, ?, ?, ?, ?)', 
				[this.props.navigation.state.params.country, this.props.navigation.state.params.grade, this.props.navigation.state.params.topic, 
				this.props.navigation.state.params.lid, this.props.navigation.state.params.name, this.props.navigation.state.params.textSubs, 
				FileSystem.documentDirectory + this.props.navigation.state.params.name, this.props.navigation.state.params.ext]);
			});
			FileSystem.downloadAsync(this.props.navigation.state.params.path,
				FileSystem.documentDirectory + this.props.navigation.state.params.name )
			.then(alert('Lesson Downloaded'))
			.catch(error => { alert(error); });
		}
	}
	
	async audioDelete() {
		if(this.audio != null){
			if(!this.props.navigation.state.params.fromRecording){			
				db.transaction(tx => {
					tx.executeSql('DELETE FROM lessons WHERE cid = ? AND gid = ? AND tid = ? AND lid = ? AND path = ?;', 
					[this.props.navigation.state.params.country, this.props.navigation.state.params.grade, this.props.navigation.state.params.topic, 
					this.props.navigation.state.params.lid, this.props.navigation.state.params.path]);
				});
				db.transaction(tx => {//only delete if no other references to that mp3 path.
					tx.executeSql('SELECT * FROM lessons WHERE path = ?;', 
						[this.props.navigation.state.params.path], 
						(_, { rows: { _array } }) => {
							if(_array.length == 0){
								FileSystem.deleteAsync( FileSystem.documentDirectory + this.props.navigation.state.params.name, {idempotent: true} )
								.then(alert('Lesson Deleted'));
							}
						}
					);
				});
				this.props.navigation.dispatch(resetActionCountry);
			}
			else{
				FileSystem.deleteAsync( FileSystem.documentDirectory +'recordings/' + this.props.navigation.state.params.name, {idempotent: true} )
				.then(alert('Recording Deleted'));
				await this.openRecordings();
				if(this.state.recordings.length == 0){
					alert('Select New Lesson');
					this.props.navigation.dispatch(resetActionCountry); 
				}
				else{
					resetActionPlayer = StackActions.reset({
											index: 0,
											actions: [
												NavigationActions.navigate({ routeName: 'Recordings', 
													params:{
														country: this.props.navigation.state.params.country,
														grade: this.props.navigation.state.params.grade,
														topic: this.props.navigation.state.params.topic,
														lid: this.props.navigation.state.params.lid,
														textSubs: this.props.navigation.state.params.textSubs,
														path: this.props.navigation.state.params.path,
														name: this.props.navigation.state.params.name,
														connected: this.props.navigation.state.params.connected,
														recordings: this.state.recordings
													}
												}),
											],
										}), 
					this.props.navigation.dispatch(resetActionPlayer);
				}
			}
		}
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
	
	async recordingSave(){
		if(this.recording != null){
			try{
				const files = await FileSystem.readDirectoryAsync(FileSystem.documentDirectory+'recordings');
				if(files.length < MAX_SAVES){
					const date = new Date();
					const recStatus = await this.recording.getURI();
					FileSystem.copyAsync({ 
						from: recStatus, 
						to: FileSystem.documentDirectory + 'recordings/' + this.props.navigation.state.params.lid.split(' ').join('_') + 
							'_['+ date.getMonth()+ "-" + date.getDay()+ "-" + date.getFullYear()+ "_" + date.getHours()+ ":" + 
							date.getMinutes()+ ":" + date.getSeconds() + "].mp3"
					}).then(alert('Recording saved'));
					this.openRecordings();
					this.recording = null;
				}
				else
					alert('Delete recordings to save more.');
			}catch(e){alert(e);}
		}
	}
	
	async openRecordings(){
		try{ 
			this.setState({
				recordings: await FileSystem.readDirectoryAsync(FileSystem.documentDirectory+'recordings'),
			});
		}
		catch(e){alert(e);}
	}
  
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
			if(this.state.recordingDuration >= MAX_RECORD_TIME)
				this.recordingStop();
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
	
	setRate = async (rate, shouldCorrectPitch) => {
		if (this.audio != null) {
			try { await this.audio.setRateAsync(rate, shouldCorrectPitch); } 
			catch (error) {}// Rate changing could not be performed, possibly because the client's Android API is too old.
		}
	};
	
	setRateUp(){
		if(Platform.OS == 'android' && Platform.Version < 23)
			alert('Cannot change the rate.');
		else{
			this.setState({rate: this.state.rate+0.1});
			this.setRate(this.state.rate, true);
		}
	}
	
	setRateDown(){
		if(Platform.OS == 'android' && Platform.Version < 23)
			alert('Cannot change the rate.');
		else{		
			this.setState({rate: this.state.rate-0.1});
			this.setRate(this.state.rate, true);
		}
	}
	
	setRateDefault(){
		if(Platform.OS == 'android' && Platform.Version < 23)
			alert('Cannot change the rate.');
		else{
			this.setState({rate: 1.0,});
			this.setRate(1.0, true);
		}
	}
	
	changePlayerLayout = () => {
		LayoutAnimation.configureNext( LayoutAnimation.Presets.easeInEaseOut );
		if( this.state.expandedPlayer === false )
			this.setState({ modifiedPlayerHeight: this.state.onLayoutPlayerHeight, expandedPlayer: true });
		else
			this.setState({ modifiedPlayerHeight: 0, expandedPlayer: false });
	}
	changeRecorderLayout = () => {
		LayoutAnimation.configureNext( LayoutAnimation.Presets.easeInEaseOut );
		if( this.state.expandedRecorder === false )
			this.setState({ modifiedRecorderHeight: this.state.onLayoutRecorderHeight, expandedRecorder: true });
		else
			this.setState({ modifiedRecorderHeight: 0, expandedRecorder: false });
	}
	getPlayerHeight( height ) {
		this.setState({ onLayoutPlayerHeight: height });
	}
	getRecorderHeight( height ) {
		this.setState({ onLayoutRecorderHeight: height });
	}
	
	render(){
		var texts = "No Text";
		if(this.props.navigation.state.params.textSubs != '' && this.props.navigation.state.params.textSubs != 'undefined') 
			texts = this.props.navigation.state.params.textSubs;
		return(
			<View style={styles.mainContainer}>
				<View style={styles.textContainer}>
					<ScrollView contentContainerStyle={{flexGrow:1}}>
						<Text style={styles.textBox}>{texts}</Text>
					</ScrollView>
				</View>							
				<TouchableOpacity activeOpacity = { 0.8 } onPress = { this.changePlayerLayout } >
                    <Text style = { styles.text }>{this.state.expandedPlayer ? 'Hide ' : ''}Player</Text>
                </TouchableOpacity>
                <View style = {{ height: this.state.modifiedPlayerHeight, overflow: 'hidden' }}>				
					<View style={styles.playerContainer} onLayout = {( event ) => this.getPlayerHeight( event.nativeEvent.layout.height )}>
						<View style={styles.sliderContainer}>
							<Slider
								style={styles.playbackSlider}
								value={this.audioSeekSliderPosition()}
								onValueChange={this.audioSeekSliderValueChange}
								onSlidingComplete={this.audioSeekSlidingComplete}
							/>  
							<Text>{this.audioRemaining()}</Text>
						</View>
						<View style={styles.buttonsContainer}>
							<View style={styles.buttonsContainers}>
								<TouchableHighlight
									underlayColor={BACKGROUND_COLOR}
									style={styles.wrapper}
									onPress={this.props.navigation.state.params.connected && !this.props.navigation.state.params.fromRecording ? this.audioDownload : this.audioDelete}
									disabled={this.state.isLoading || this.state.isRecording}>
									<Image style={styles.image} source={this.props.navigation.state.params.connected && !this.props.navigation.state.params.fromRecording ? ICON_DOWNLOAD_BUTTON : ICON_DELETE_BUTTON} />
								</TouchableHighlight>
								<Text>{this.props.navigation.state.params.connected && !this.props.navigation.state.params.fromRecording ? 'Download' : 'Delete'}</Text>
							</View>
							<View style={styles.buttonsContainers}>
								<TouchableHighlight
									underlayColor={BACKGROUND_COLOR}
									style={styles.wrapper}
									onPress={this.setRateUp}
									disabled={this.state.isLoading || this.state.rate > RATE_MAX}>
									<Image style={styles.image} source={ICON_UP_RATE_BUTTON} />
								</TouchableHighlight>
									<TouchableHighlight
									underlayColor={BACKGROUND_COLOR}
									style={styles.wrapper}
									onPress={this.setRateDown}
									disabled={this.state.isLoading || this.state.rate < RATE_MIN}>
									<Image style={styles.image} source={ICON_DOWN_RATE_BUTTON} />
								</TouchableHighlight>
								<Text>{'Rate: '+this.state.rate.toFixed(1)}</Text>
							</View>
							<View style={styles.buttonsContainers}>
								<View style={styles.eighthButtonsContainer}>
									<TouchableHighlight
										underlayColor={BACKGROUND_COLOR}
										style={styles.wrapper}
										onPress={this.setRateDefault}
										disabled={this.state.isLoading}>
										<Image style={styles.image} source={ICON_RESET_RATE_BUTTON} />
									</TouchableHighlight>
									<Text>{'Reset'}</Text>
								</View>
							</View>
							<View style={styles.buttonsContainers}>
								<TouchableHighlight
									underlayColor={BACKGROUND_COLOR}
									style={styles.wrapper}
									onPress={this.audioPlayPause}
									disabled={this.state.isLoading}>
									<Image style={styles.image}
										source={ this.state.isAudioPlaying ? ICON_PAUSE_BUTTON : ICON_PLAY_BUTTON }
									/>
								</TouchableHighlight>
								<Text>{this.state.isAudioPlaying ? 'Pause' : 'Play'}</Text>
							</View>
							<View style={styles.buttonsContainers}>
								<TouchableHighlight
									underlayColor={BACKGROUND_COLOR}
									style={styles.wrapper}
									onPress={this.audioStop}
									disabled={this.state.isLoading}>
									<Image style={styles.image} source={ICON_STOP_BUTTON}/>
								</TouchableHighlight>
								<Text>{'Stop'}</Text>
							</View>
						</View>
					</View>
				</View>
				<TouchableOpacity activeOpacity = { 0.8 } onPress = { this.changeRecorderLayout } >
                    <Text style = { styles.text }>{this.state.expandedRecorder ? 'Hide ' : ''}Recorder</Text>
                </TouchableOpacity>				
                <View style = {{ height: this.state.modifiedRecorderHeight, overflow: 'hidden' }}>				
					<View style={styles.playerContainer} onLayout = {( event ) => this.getRecorderHeight( event.nativeEvent.layout.height )}>
						<View style={styles.sliderContainer}>
							<Slider
								style={styles.playbackSlider}
								value={this.recordingFinishedSeekSliderPosition()}
								onValueChange={this.recordingSeekSliderValueChange}
								onSlidingComplete={this.recordingFinishedSeekSlidingComplete}
								disabled={!this.state.isPlaybackAllowed || this.state.isLoading}
							/>
							<View style={styles.thinContainer}>
								<Text style={{fontSize: 13, color: 'red'}}> {this.state.isRecording ? 'RECORDING: '+this.getRecordingTimestamp() : ''} </Text>
								<Text style={{opacity: this.recordingFinished != null ? 1.0 : 0.0 }}>{this.recordingRemaining()}</Text>
							</View>	
						</View>
						<View style={styles.buttonsContainer}>
								<View style={styles.buttonsContainers}>
									<TouchableHighlight
										underlayColor={BACKGROUND_COLOR}
										style={styles.wrapper}
										onPress={this.recordingPressed}>
										<Image style={styles.image} source={ICON_RECORD_BUTTON }/>
									</TouchableHighlight>
									<Text>{this.state.isRecording ? 'Stop' : 'Record'}</Text>
								</View>
								<View style={[styles.buttonsContainers, { opacity: !this.state.isPlaybackAllowed || this.state.isLoading || this.recording == null ? DISABLED_OPACITY : 1.0, },]}>
									<TouchableHighlight
										underlayColor={BACKGROUND_COLOR}
										style={styles.wrapper}
										onPress={this.recordingSave}
										disabled={!this.state.isPlaybackAllowed || this.state.isLoading || this.recording == null }>
										<Image style={styles.image} source={ICON_SAVE_BUTTON}/>
									</TouchableHighlight>
									<Text>{'Save'}</Text>
								</View>
								<View style={[styles.buttonsContainers, { opacity: this.state.recordings.length == 0 || this.state.isRecording ? DISABLED_OPACITY : 1.0, }]}>
									<TouchableHighlight
										underlayColor={BACKGROUND_COLOR}
										style={styles.wrapper}
										onPress={() => {
											resetActionPlayer = StackActions.reset({
												index: 0,
												actions: [
													NavigationActions.navigate({ routeName: 'Recordings', 
														params:{
															country: this.props.navigation.state.params.country,
															grade: this.props.navigation.state.params.grade,
															topic: this.props.navigation.state.params.topic,
															lid: this.props.navigation.state.params.lid,
															textSubs: this.props.navigation.state.params.textSubs,
															path: this.props.navigation.state.params.path,
															name: this.props.navigation.state.params.name,
															connected: this.props.navigation.state.params.connected,
															recordings: this.state.recordings
														}
													}),
												],
											}), 
											this.props.navigation.dispatch(resetActionPlayer);
										}}
										disabled={this.state.recordings.length == 0 || this.state.isRecording}>
										<Image style={styles.image} source={ICON_OPEN_BUTTON }/>
									</TouchableHighlight>
									<Text>{'Open'}</Text>
								</View>								
								<View style={[styles.buttonsContainers, { opacity: !this.state.isPlaybackAllowed || this.state.isLoading ? DISABLED_OPACITY : 1.0, },]}>
									<TouchableHighlight
										underlayColor={BACKGROUND_COLOR}
										style={styles.wrapper}
										onPress={this.recordingFinishedPlayPause}
										disabled={!this.state.isPlaybackAllowed || this.state.isLoading}>
										<Image style={styles.image}
											source={ this.state.isRecordingPlaying ? ICON_PAUSE_BUTTON : ICON_PLAY_BUTTON }
										/>
									</TouchableHighlight>
									<Text>{this.state.isRecordingPlaying ? 'Pause' : 'Play'}</Text>
								</View>
								<View style={[styles.buttonsContainers, { opacity: !this.state.isPlaybackAllowed || this.state.isLoading ? DISABLED_OPACITY : 1.0, },]}>
									<TouchableHighlight
										underlayColor={BACKGROUND_COLOR}
										style={styles.wrapper}
										onPress={this.recordingFinishedStop}
										disabled={!this.state.isPlaybackAllowed || this.state.isLoading}>
										<Image style={styles.image} source={ICON_STOP_BUTTON}/>
									</TouchableHighlight>
									<Text>{'Stop'}</Text>
								</View>
						</View>
					</View>
                </View>	
				<View style={{ alignItems:'center', bottom: 1, backgroundColor: BACKGROUND_COLOR }}>
					{this.props.navigation.state.params.fromRecording &&  
						<TouchableHighlight
							onPress={() => {this.props.navigation.dispatch(resetActionCountry)}}>
							<Image source={ICON_REFRESH_BUTTON}/>
						</TouchableHighlight>
					}
				</View>
				<View style={{ alignItems:'center', bottom: 1, backgroundColor: BACKGROUND_COLOR }}>
					{this.props.navigation.state.params.fromRecording && <Text>{'New Lesson'}</Text>}
				</View>
			</View>
		)
	}
}