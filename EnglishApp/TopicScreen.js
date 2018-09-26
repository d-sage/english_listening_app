import React from 'react';
import { View, Text, Button, NetInfo, Platform, Image, ListView } from 'react-native';
import Expo, { SQLite } from 'expo';
import styles from "./Styles.js";
import { NavigationActions, StackActions } from 'react-navigation';

var ds = new ListView.DataSource({ rowHasChanged: (row1, row2) => row1 !== row2 });
const db = SQLite.openDatabase('db.db');
const ICON_TOPICS_BUTTON = require('./assets/images/topics_button.png');
const resetActionUser = StackActions.reset({
  index: 0,
  actions: [NavigationActions.navigate({ routeName: 'User' })],
}); 

class TopicScreen extends React.Component {

	render(){
		return(
			<View style={styles.mainContainer}>
				<View style={styles.headerContainer}>
					<Image source={ICON_TOPICS_BUTTON}/>
					<Text style={{fontSize: 20}}>Select Topic</Text>
				</View>
				<ListView
					enableEmptySections
					style={styles.dataContainer}
					dataSource={this.state.dataSource}
					renderRow={(rowData) =>
						<View style={styles.buttonContainer}>
							<Button color='#396FB6' 
								onPress={() => {this.props.navigation.navigate('Lesson',
									{user: this.props.navigation.state.params.user,
									 environment: this.props.navigation.state.params.environment,
									 topic: rowData.tid,
									 connected: this.props.navigation.state.params.connected});
									 this.componentWillUnmount();
								}}
								title = {rowData.tid+""}
							/>
						</View>
					}
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
		return fetch('http://reaching4english-001-site1.itempurl.com/Topics/topicQuery.php?userType=' + this.props.navigation.state.params.user + 
			' &env=' + this.props.navigation.state.params.environment)
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
			tx.executeSql('SELECT DISTINCT userType,env,tid FROM lessons WHERE userType = ? AND env = ?;', 
			[this.props.navigation.state.params.user,this.props.navigation.state.params.environment], (_, { rows: { _array } }) => 
				this.setState({ dataSource: ds.cloneWithRows(_array) }));
		});
	}
}

export default TopicScreen;
