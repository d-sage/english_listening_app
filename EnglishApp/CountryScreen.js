import React from 'react';
import styles from "./Styles.js";
import { SQLite, Permissions } from 'expo';
import { View, Text, Button, ListView, NetInfo, Platform, Image, TouchableHighlight } from 'react-native';
import { NavigationActions } from 'react-navigation';

var ds = new ListView.DataSource({ rowHasChanged: (row1, row2) => row1 !== row2 });
const db = SQLite.openDatabase('db.db');
const ICON_REFRESH_BUTTON = require('./assets/images/refresh_button.png');
const resetActionCountry = NavigationActions.reset({
  index: 0,
  actions: [NavigationActions.navigate({ routeName: 'Country' })],
}); 

class CountryScreen extends React.Component {
	
	render(){
		if(this.state.haveRecordingPermissions){
			return(		
				<View style={styles.mainContainer}>
					<View style={styles.headerContainer}>
						<Text style={{fontSize: 20}}>Select Country</Text>
					</View>
					<ListView
						enableEmptySections
						style={styles.dataContainer}
						dataSource={this.state.dataSource}
						renderRow={(rowData) =>
							<View style={styles.buttonContainer}>
								<Button
									onPress={() => {this.props.navigation.navigate('Grade',
										{country: rowData.cid,
										connected: this.state.connected });
										this.componentWillUnmount();
									}}
									title = {rowData.cid+""}
								/>
							</View>
						}
					/>
					<View style={{ alignItems:'center', bottom: 1, }}>
						<TouchableHighlight
							onPress={() => {
								this.props.navigation.dispatch(resetActionCountry);
								this.componentWillUnmount();
							}}>
							<Image source={ICON_REFRESH_BUTTON}/>
						</TouchableHighlight>
						<Text>{'Refresh'}</Text>
					</View>
				</View>
			);
		}
		else
			return(<View><Text style={{fontSize: 20, color: 'red'}}>You must enable permissions to use this app.</Text></View>);
	}

	constructor(props){
        super(props);
        this.state = {
          dataSource: ds.cloneWithRows([]),
		  connected: false,
		  haveRecordingPermissions: false,
        };
	}

	componentDidMount() {
		this.askForPermissions();
		db.transaction(tx => {
			//tx.executeSql('DROP TABLE IF EXISTS lessons;');
			tx.executeSql('CREATE TABLE IF NOT EXISTS lessons (cid varchar(30) NOT NULL, gid tinyint(4) NOT NULL, tid varchar(50) NOT NULL, lid varchar(100) NOT NULL, filename varchar(100) NOT NULL, text varchar(2500) NOT NULL, path varchar(260) NOT NULL, ext varchar(5) NOT NULL, PRIMARY KEY (cid, gid, tid, lid));');
		});
		if(Platform.OS == 'ios'){		
			NetInfo.isConnected.addEventListener('connectionChange', this.handleConnectionChange);
		}
		else if(Platform.OS == 'android'){
			NetInfo.isConnected.fetch().done(
				(isConnected) => { this.getData(isConnected); }
			);
		}
		else
			alert('Only Android and IOS are supported');
	}
	
	componentWillUnmount() {
		if(Platform.OS == 'ios')
			NetInfo.isConnected.removeEventListener('connectionChange', this.handleConnectionChange);
	}
	
	handleConnectionChange = (isConnected) => {
		this.getData(isConnected);
	}
	
	askForPermissions = async () => {
		const response = await Permissions.askAsync(Permissions.AUDIO_RECORDING);
		this.setState({
			haveRecordingPermissions: response.status === 'granted',
		});
	};

	getData(isConnected){
		this.setState({ connected: isConnected });
		if(isConnected)
			this.fetchOnlineData();
		else
			this.fetchOfflineData();
	}

	fetchOnlineData(){
		return fetch('http://reaching4english-001-site1.itempurl.com/Countries/countryQuery.php')
		.then((response) => response.json())
		.then((responseJson) => {
			if(responseJson){
				this.setState({
					dataSource: ds.cloneWithRows(responseJson),
				});
			}
		}).done();
	}

	fetchOfflineData(){
		db.transaction(tx => {
			tx.executeSql('SELECT DISTINCT cid FROM lessons;', [], (_, { rows: { _array } }) => this.setState({ dataSource: ds.cloneWithRows(_array) }));
		});
	}
}

export default CountryScreen;
