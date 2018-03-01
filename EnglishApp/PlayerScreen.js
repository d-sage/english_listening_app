import React, { Component } from 'react';
import { View, TouchableNativeFeedback,Text,Button, ListView  } from 'react-native';
import { Audio } from 'expo';
import styles from "./Styles.js";

class AudioPlayer extends Component {
  constructor(props) {
    super(props);
    this.state = { isPlaying: false };

    this.loadAudio = this.loadAudio.bind(this);
    this.toggleAudioPlayback = this.toggleAudioPlayback.bind(this);
  }

  componentDidMount() {
    this.loadAudio();
  }

  componentWillUnmount() {
    this.soundObject.stopAsync();
  }

  async loadAudio() {
    this.soundObject = new Audio.Sound();
    try {
      await this.soundObject.loadAsync({ uri: this.props.navigation.state.params.path /* url for your audio file */ });
    } catch (e) {
      console.log('ERROR Loading Audio', e);
    }
  }

	stopAudioPlayback()
	{
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
						onPress={() => this.props.navigation.navigate('Test',
            {country: this.props.navigation.state.params.country,
             grade: this.props.navigation.state.params.grade,
             topic: this.props.navigation.state.params.topic,
             lid: this.props.navigation.state.params.lid,
             path: this.props.navigation.state.params.path+""})}
						title="Download"
					/>
				</View>
				<View style={styles.buttonContainer}>
					<Button
						onPress={() => this.props.navigation.navigate('Country')}
						title = "Back to CountryScreen"
					/>
				</View>
			</View>
		)
	}
}

export default AudioPlayer;
