import React from 'react';
import { View, Text, Button, NetInfo, Platform, ListView, Image, TouchableHighlight } from 'react-native';
import Expo, { SQLite } from 'expo';
import styles from "./Styles.js";
import { NavigationActions, StackActions } from 'react-navigation';

var ds = new ListView.DataSource({ rowHasChanged: (row1, row2) => row1 !== row2 });
const db = SQLite.openDatabase('db.db');
const ICON_INDEPENDENT_BUTTON = require('./assets/images/independent_button.png');
const ICON_CLASSROOM_BUTTON = require('./assets/images/classroom_button.png');
const resetActionUser = StackActions.reset({
  index: 0,
  actions: [NavigationActions.navigate({ routeName: 'User' })],
}); 

class EnvironmentScreen extends React.Component {

	render(){
		return(
			<View style={styles.mainContainer}>
				<ListView
						enableEmptySections
						style={styles.usersContainer}
						dataSource={this.state.dataSource}
						renderRow={(rowData) => {	
							if(rowData.env == 'Independent'){
								return(
									<View style={styles.usersContainer}>
										<TouchableHighlight
											underlayColor='transparent'
											onPress={() => {this.props.navigation.navigate('Topic',
												{user: this.props.navigation.state.params.user,
												environment: rowData.env,
												connected: this.props.navigation.state.params.connected});
												this.componentWillUnmount();
											}}>
											<Image source={ICON_INDEPENDENT_BUTTON}/>
										</TouchableHighlight>
										<Text style={{fontSize: 20}}>{rowData.env+""}</Text>
									</View>	
								);
							}
							else if(rowData.env == 'Classroom'){
								return(
									<View style={styles.usersContainer}>
										<TouchableHighlight
											underlayColor='transparent'
											onPress={() => {this.props.navigation.navigate('Topic',
												{user: this.props.navigation.state.params.user,
												environment: rowData.env,
												connected: this.props.navigation.state.params.connected});
												this.componentWillUnmount();
											}}>
											<Image source={ICON_CLASSROOM_BUTTON}/>
										</TouchableHighlight>
										<Text style={{fontSize: 20}}>{rowData.env+""}</Text>
									</View>	
								);
							}
							else{
								return(
									<View style={styles.headerContainer}>
										<Text style={{fontSize: 20}}>Unknown Environment</Text>
									</View>
								);
							}
						}}
					/>
			</View>
		)
	}

	constructor(props){
        super(props);
        this.state = {
          dataSource: ds.cloneWithRows([]),
        };
	}

	componentDidMount() {
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
	
	getData(isConnected){
		if(isConnected != this.props.navigation.state.params.connected){
			if(isConnected)
				alert("Online");
			else
				alert("Offline");
			this.props.navigation.dispatch(resetActionUser);
			this.componentWillUnmount();
		}
		else if(isConnected)
			this.fetchOnlineData();
		else
			this.fetchOfflineData();
	}
	
	fetchOnlineData(){
		return fetch('http://reaching4english-001-site1.itempurl.com/Environment/envQuery.php?userType=' + this.props.navigation.state.params.user)
		//return fetch('http://justinoneel.com/ReachingForEnglish/environmentQuery.php?user=' + this.props.navigation.state.params.user)
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
			tx.executeSql('SELECT DISTINCT env FROM lessons WHERE userType = ?;', 
			[this.props.navigation.state.params.user], (_, { rows: { _array } }) => 
				this.setState({ dataSource: ds.cloneWithRows(_array) }));
		});
	}
}

export default EnvironmentScreen;
