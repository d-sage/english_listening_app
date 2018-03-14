import React, { Component } from 'react';
import { View, TouchableNativeFeedback, Text,Button, ListView  } from 'react-native';
import { Audio, SQLite, FileSystem } from 'expo';
import styles from "./Styles.js";

const db = SQLite.openDatabase('db.db');

class AudioPlayer extends Component {

	constructor(props) {
		super(props);
		this.state = { isPlaying: false };
		this.loadAudio = this.loadAudio.bind(this);
		this.toggleAudioPlayback = this.toggleAudioPlayback.bind(this);
	}

	componentDidMount() {
		this.loadAudio();
		//alert(this.props.navigation.state.params.path);
	}

	componentWillUnmount() {
		this.soundObject.stopAsync();
	}

	async loadAudio() {
		this.soundObject = new Audio.Sound();
		try {
				await this.soundObject.loadAsync({ uri: this.props.navigation.state.params.path });
		} catch (e) { alert('ERROR Loading Audio: ' + e); }
	}

	stopAudioPlayback(){
		this.soundObject.stopAsync();
		this.setState({isPlaying: false});
	}

  toggleAudioPlayback() {
    this.setState({
      isPlaying: !this.state.isPlaying,
    }, () => (this.state.isPlaying
      ? this.soundObject.playAsync()
      : this.soundObject.pauseAsync()));
  }

	render(){
		if(this.props.navigation.state.params.connected){//ONLINE
			return(
				<View style={styles.mainContainer}>
					<View style={styles.headerContainer}>
						<Text>Player Screen</Text>
					</View>
					<View style={styles.buttonContainer}>
						<Button
							onPress={async() => {
								this.toggleAudioPlayback();  }
							}
							title="Play/Pause Sound"
						/>
					</View>
					<View style={styles.buttonContainer}>
						<Button
							onPress={async() => {
								this.stopAudioPlayback(); }
							}
							title="Stop Sound"
						/>
					</View>
					<View style={styles.buttonContainer}>
						<Button
							onPress={() => {
								db.transaction(tx => {
									tx.executeSql('INSERT OR IGNORE INTO lessons (cid, gid, tid, lid, text, path) values (?, ?, ?, ?, ?, ?)', [this.props.navigation.state.params.country, this.props.navigation.state.params.grade, this.props.navigation.state.params.topic, this.props.navigation.state.params.lid, this.props.navigation.state.params.textSubs, FileSystem.documentDirectory + this.props.navigation.state.params.name]);
								});
								FileSystem.downloadAsync(this.props.navigation.state.params.path,
									FileSystem.documentDirectory + this.props.navigation.state.params.name ).then(({ uri }) => {
										alert('Finished Downloading To Directory:\n ' + uri);
									}).catch(error => { alert("ERROR Downloading Audio: " + error); });
							}}
							title="Download"
						/>
					</View>
					<View style={styles.buttonContainer}>
						<Button
							onPress={() => {this.props.navigation.navigate('Country'),
											this.stopAudioPlayback()
							}}
							title = "Back to CountryScreen"
						/>
					</View>
				</View>
			)
		}
		else{//OFFLINE
			return(
				<View style={styles.mainContainer}>
				<View style={styles.headerContainer}>
					<Text>Player Screen</Text>
				</View>
				<View style={styles.buttonContainer}>
					<Button
						onPress={async() => {
							this.toggleAudioPlayback();  }
						}
						title="Play/Pause Sound"
					/>
				</View>
				<View style={styles.buttonContainer}>
					<Button
						onPress={async() => {
							this.stopAudioPlayback(); }
						}
						title="Stop Sound"
					/>
				</View>
				<View style={styles.buttonContainer}>
						<Button
							onPress={() => {
								this.stopAudioPlayback();
								db.transaction(tx => {
									tx.executeSql('DELETE FROM lessons WHERE cid = ? AND gid = ? AND tid = ? AND lid = ? AND path = ?;', [this.props.navigation.state.params.country, this.props.navigation.state.params.grade, this.props.navigation.state.params.topic, this.props.navigation.state.params.lid, this.props.navigation.state.params.path]);
								});
								FileSystem.deleteAsync( FileSystem.documentDirectory + this.props.navigation.state.params.name, {idempotent: true} );
								this.props.navigation.navigate('Country');
								alert('Finished Deleting Audio File');
							}}
							title="Delete"
						/>
					</View>
				<View style={styles.buttonContainer}>
					<Button
						onPress={() => {this.props.navigation.navigate('Country'),
										this.stopAudioPlayback()
							}}
						title = "Back to CountryScreen"
					/>
				</View>
			</View>
			)
		}
	}
}

export default AudioPlayer;
