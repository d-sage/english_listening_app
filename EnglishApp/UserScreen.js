import React from 'react';
import styles from "./Styles.js";
import { SQLite, Permissions } from 'expo';
import { View, Text, Button, ListView, NetInfo, Platform, Image, TouchableHighlight } from 'react-native';
import { NavigationActions, StackActions } from 'react-navigation';

var ds = new ListView.DataSource({ rowHasChanged: (row1, row2) => row1 !== row2 });
const db = SQLite.openDatabase('db.db');
const ICON_REFRESH_BUTTON = require('./assets/images/refresh_button.png');
const ICON_TEACHER_BUTTON = require('./assets/images/teacher_button.png');
const ICON_STUDENT_BUTTON = require('./assets/images/student_button.png');
const resetActionUser = StackActions.reset({
  index: 0,
  actions: [NavigationActions.navigate({ routeName: 'Home' })],
}); 

class UserScreen extends React.Component {
	
	render(){
			return(		
				<View style={styles.mainContainer}>
					<ListView
						enableEmptySections
						style={styles.usersContainer}
						dataSource={this.state.dataSource}
						renderRow={(rowData) => {	
							if(rowData.userType == 'Teacher'){
								return(
									<View style={styles.usersContainer}>
										<TouchableHighlight
											underlayColor='transparent'
											onPress={() => {this.props.navigation.navigate('Environment',
												{user: rowData.userType,
												connected: this.state.connected});
												this.componentWillUnmount();
											}}>
											<Image source={ICON_TEACHER_BUTTON}/>
										</TouchableHighlight>
										<Text style={{fontSize: 20}}>{rowData.userType+""}</Text>
									</View>	
								);
							}
							else if(rowData.userType == 'Student'){
								return(
									<View style={styles.usersContainer}>
										<TouchableHighlight
											underlayColor='transparent'
											onPress={() => {this.props.navigation.navigate('Environment',
												{user: rowData.userType,
												connected: this.state.connected});
												this.componentWillUnmount();
											}}>
											<Image source={ICON_STUDENT_BUTTON}/>
										</TouchableHighlight>
							<Text style={{fontSize: 20}}>{rowData.userType+""}</Text>
									</View>	
								);
							}
							else{
								return(
									<View style={styles.headerContainer}>
										<Text style={{fontSize: 20}}>Unknown User</Text>
									</View>
								);
							}
						}}
					/>
					<View style={{ alignItems:'center', bottom: 1, }}>
						<TouchableHighlight
							onPress={() => {
								this.props.navigation.dispatch(resetActionUser);
								this.componentWillUnmount();
							}}>
							<Image source={ICON_REFRESH_BUTTON}/>
						</TouchableHighlight>
						<Text>{'Refresh'}</Text>
					</View>
				</View>
			);
	}

	constructor(props){
        super(props);
        this.state = {
          dataSource: ds.cloneWithRows([]),
		  connected: false,
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
		this.setState({ connected: isConnected });
		if(isConnected)
			this.fetchOnlineData();
		else
			this.fetchOfflineData();
	}

	fetchOnlineData(){
		return fetch('http://reaching4english-001-site1.itempurl.com/Users/userQuery.php')
		//return fetch('http://justinoneel.com/ReachingForEnglish/userQuery.php')
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
			tx.executeSql('SELECT DISTINCT userType FROM lessons;', [], (_, { rows: { _array } }) => this.setState({ dataSource: ds.cloneWithRows(_array) }));
		});
	}
}

export default UserScreen;