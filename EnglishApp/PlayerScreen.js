import React from 'react';
import { View, Text, Button, ListView } from 'react-native';
import styles from "./Styles.js";
import Expo from 'expo';

var ds = new ListView.DataSource({ rowHasChanged: (row1, row2) => row1 !== row2 });
const soundObject = new Expo.Audio.Sound();
loadMp3();

class PlayerScreen extends React.Component {

	render(){
		return(
			<View style={styles.mainContainer}>
				<View style={styles.headerContainer}>
					<Text>Player Screen</Text>
				</View>
				<View style={styles.buttonContainer}>
					<Button
						onPress={async() => {
							try{ await soundObject.playAsync(); }
							catch(error){ console.log("Error occurred!!!"); }
						}}
						title="Play Sound"
					/>
				</View>
				<View style={styles.buttonContainer}>
					<Button
						onPress={async() => {
							try{ await soundObject.pauseAsync(); }
							catch(error){ console.log("Error occurred!!!"); }
						}}
						title="Pause Sound"
					/>
				</View>
				<View style={styles.buttonContainer}>
					<Button
						onPress={async() => {
							try{ await soundObject.stopAsync(); }
							catch(error){ console.log("Error occurred!!!"); }
						}}
						title="Stop Sound"
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

	constructor(props){
        super(props);
        this.state = {
          dataSource: ds.cloneWithRows([]),
        };
		alert(this.props.navigation.state.params.path);
	}

	componentWillMount() {
		this.fetchData();
	}

	fetchData(){
		return;
	}
}

async function loadMp3() {
	//var path = this.props.navigation.state.params.path;
	try{ await soundObject.loadAsync(require('./media/SampleAudio_0.4mb.mp3')); }
	catch(error){ console.log("Error occurred!!!"); }
}

export default PlayerScreen;
