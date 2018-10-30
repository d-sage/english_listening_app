import React from 'react';
import styles from "./Styles.js";
import { SQLite, Permissions } from 'expo';
import { View, Text, Button, ListView, NetInfo, Platform, Image, TouchableHighlight } from 'react-native';
import { NavigationActions, StackActions } from 'react-navigation';

const db = SQLite.openDatabase('db.db');
const ICON_LOGO_BUTTON = require('./assets/images/logo.png');

class HomeScreen extends React.Component {
	
	render(){
		return(		
			<View style={styles.homeContainer}>
				<TouchableHighlight
					onPress={this.handlePermissions}
					underlayColor="#fff">
					<Image source={ICON_LOGO_BUTTON}/>
				</TouchableHighlight>
				<Text style={{fontSize: 40, color: '#4272b8', fontWeight: 'bold'}}>Reaching</Text>
				<Text style={{fontSize: 40, color: '#4272b8', fontWeight: 'bold'}}>for English</Text>
			</View>
		);
	}

	constructor(props){
		super(props);
		this.state = {
			haveRecordingPermissions: false,
		};
		this.handlePermissions = this.handlePermissions.bind(this);
	}

	componentDidMount() {
		this.askForPermissions();
		db.transaction(tx => {
			//tx.executeSql('DROP TABLE IF EXISTS lessons;');
			tx.executeSql('CREATE TABLE IF NOT EXISTS lessons (userType varchar(30) NOT NULL, env varchar(30) NOT NULL, tid varchar(50) NOT NULL, lid varchar(100) NOT NULL, filename varchar(100) NOT NULL, text varchar(2500), path varchar(260) NOT NULL, ext varchar(5) NOT NULL, PRIMARY KEY (userType, env, tid, lid));');
		}, () => { alert("Error Creating Table"); }, () => { });
	}

	handlePermissions(){
		if(this.state.haveRecordingPermissions){
			this.props.navigation.navigate('User');
		}
		else{
			this.askForPermissions();
		}
	}
		
	askForPermissions = async () => {
		const response = await Permissions.askAsync(Permissions.AUDIO_RECORDING);
		this.setState({
			haveRecordingPermissions: response.status === 'granted',
		});
	};

}
export default HomeScreen;