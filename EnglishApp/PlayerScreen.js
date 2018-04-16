//TODO
//make text box half screen and adjust the rest of screen accordingly
//see if its possible to have text follow audio
//speed up and slow down rate of audio along with a default button.

import React from 'react';
import {
  Image,
  Slider,
  StyleSheet,
  Text,
  TouchableHighlight,
  View,
  ScrollView,
  Platform,
} from 'react-native';
import { Audio, SQLite, FileSystem, Asset } from 'expo';
import { NavigationActions } from 'react-navigation';
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
			rate: 1.0,
		};
		this.audioPlayPause = this.audioPlayPause.bind(this);
		this.audioStop = this.audioStop.bind(this);
		this.audioDownload = this.audioDownload.bind(this);
		this.audioDelete = this.audioDelete.bind(this);
		this.recordingPressed = this.recordingPressed.bind(this);
		this.setRateUp = this.setRateUp.bind(this);
		this.setRateDown = this.setRateDown.bind(this);
		this.setRateDefault = this.setRateDefault.bind(this);
		this.recordingSettings = JSON.parse(JSON.stringify(Audio.RECORDING_OPTIONS_PRESET_HIGH_QUALITY));
	}
	
	componentDidMount() {
		this.mounted = true;
		if(this.audio == null)
			this.loadAudio();
		Expo.ScreenOrientation.allow(Expo.ScreenOrientation.Orientation.PORTRAIT_UP);
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
	
	setRate = async (rate, shouldCorrectPitch) => {
		if (this.audio != null) {
			try { await this.audio.setRateAsync(rate, shouldCorrectPitch); } 
			catch (error) {}// Rate changing could not be performed, possibly because the client's Android API is too old.
		}
	};
	
	setRateUp(){
		if(Platform.Version < 23)
			alert('Your version of android is not supported to change the rate.');
		else if(this.state.rate < 5.0){
			this.setState({rate: this.state.rate+0.2,});
			this.setRate(this.state.rate, true);	
		}
		else
			alert('Can\'t set rate any higher.');
	}
	
	setRateDown(){
		if(Platform.Version < 23)
			alert('Your version of android is not supported to change the rate.');
		else if(this.state.rate > 0.0){
			this.setState({rate: this.state.rate-0.2,});
			this.setRate(this.state.rate, true);
		}
		else
			alert('Can\'t set rate any lower.');
	}
	
	setRateDefault(){
		if(Platform.Version < 23)
			alert('Your version of android is not supported to change the rate.');
		else{
			this.setState({rate: 1.0,});
			this.setRate(1.0, true);
		}
	}
	
	render(){
		var texts;
		if(this.props.navigation.state.params.textSubs == 'undefined') 
			texts = "No Text To Diaplay";
		else
			texts = this.props.navigation.state.params.textSubs;
		return(
			<View>
				<View style={styles.textContainer}>
					<ScrollView contentContainerStyle={{flexGrow:1}}>
						<Text style={styles.textBox}>{texts}</Text>
					</ScrollView>
				</View>
				<View style={styles.playerContainer}>
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
						<View style={styles.fifthButtonsContainer}>
							<TouchableHighlight
								underlayColor={BACKGROUND_COLOR}
								style={styles.wrapper}
								onPress={this.props.navigation.state.params.connected ? this.audioDownload : this.audioDelete}
								disabled={this.state.isLoading}>
								<Image style={styles.image} source={this.props.navigation.state.params.connected ? ICON_DOWNLOAD_BUTTON : ICON_DELETE_BUTTON} />
							</TouchableHighlight>
							<Text>{this.props.navigation.state.params.connected ? 'Download' : 'Delete'}</Text>
						</View>
						<View style={styles.fifthButtonsContainer }>
							<TouchableHighlight
								underlayColor={BACKGROUND_COLOR}
								style={styles.wrapper}
								onPress={this.setRateUp}
								disabled={this.state.isLoading}>
								<Image style={styles.image} source={ICON_UP_RATE_BUTTON} />
							</TouchableHighlight>
								<TouchableHighlight
								underlayColor={BACKGROUND_COLOR}
								style={styles.wrapper}
								onPress={this.setRateDown}
								disabled={this.state.isLoading}>
								<Image style={styles.image} source={ICON_DOWN_RATE_BUTTON} />
							</TouchableHighlight>
							<Text>{'Rate'}</Text>
						</View>
						<View style={styles.fifthButtonsContainer}>
							<View style={styles.eighthButtonsContainer}>
								<TouchableHighlight
									underlayColor={BACKGROUND_COLOR}
									style={styles.wrapper}
									onPress={this.setRateDefault}
									disabled={this.state.isLoading}>
									<Image style={styles.image} source={ICON_RESET_RATE_BUTTON} />
								</TouchableHighlight>
								<Text>{'Default'}</Text>
							</View>
						</View>
						<View style={styles.fifthButtonsContainer}>
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
						<View style={styles.fifthButtonsContainer}>
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
				<View style={styles.playerContainer}>
					<View style={styles.sliderContainer}>
						<Slider
							style={styles.playbackSlider}
							value={this.recordingFinishedSeekSliderPosition()}
							onValueChange={this.recordingSeekSliderValueChange}
							onSlidingComplete={this.recordingFinishedSeekSlidingComplete}
							disabled={!this.state.isPlaybackAllowed || this.state.isLoading}
						/>
						<Text style={{opacity: this.recordingFinished != null ? 1.0 : 0.0 }}>{this.recordingRemaining()}</Text>
					</View>
					<View style={styles.buttonsContainer}>
						<View style={styles.halfButtonsContainer}>
							<View style={styles.fourthButtonsContainer}>
								<TouchableHighlight
									underlayColor={BACKGROUND_COLOR}
									style={styles.wrapper}
									onPress={this.recordingPressed}>
									<Image style={styles.image} source={this.state.isRecording ? ICON_RECORDING : ICON_RECORD_BUTTON }/>
								</TouchableHighlight>
								<Text>{this.state.isRecording ? 'Stop' : 'Record'}</Text>
							</View>
							<View style={styles.fourthButtonsContainer}>
								<Text style={{fontSize: 13, color: 'red'}}> {this.state.isRecording ? 'RECORDING:' : ''} </Text>
								<Text style={{opacity: this.state.isRecording ? 1.0 : 0.0 }}>{this.getRecordingTimestamp()}</Text>
							</View>
						</View>
						<View style={[styles.halfButtonsContainer, { opacity: !this.state.isPlaybackAllowed || this.state.isLoading ? DISABLED_OPACITY : 1.0, },]}>
							<View style={styles.fourthButtonsContainer}>
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
							<View style={styles.fourthButtonsContainer}>
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
			</View>
		)
	}
}