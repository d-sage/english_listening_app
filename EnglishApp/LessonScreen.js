import React from 'react';
import { View, Text, Button, NetInfo, Platform, Image, ListView } from 'react-native';
import Expo, { SQLite } from 'expo';
import styles from "./Styles.js";
import { NavigationActions, StackActions } from 'react-navigation';

var ds = new ListView.DataSource({ rowHasChanged: (row1, row2) => row1 !== row2 });
const db = SQLite.openDatabase('db.db');
const ICON_LESSONS_BUTTON = require('./assets/images/lessons_button.png');
const resetActionUser = StackActions.reset({
  index: 0,
  actions: [NavigationActions.navigate({ routeName: 'User' })],
}); 

class LessonScreen extends React.Component {

	render(){
		return(
			<View style={styles.mainContainer}>
				<View style={styles.headerContainer}>
					<Image source={ICON_LESSONS_BUTTON}/>
					<Text style={{fontSize: 20}}>Lessons</Text>
				</View>
				<ListView
					enableEmptySections
					style={styles.dataContainer}
					dataSource={this.state.dataSource}
					renderRow={(rowData) =>
						<View style={styles.buttonContainer}>
							<Button color='#396FB6' 
								onPress={() => {
									if(rowData.ext == "pdf"){
										Linking.openURL(rowData.path);
										this.componentWillUnmount();
									}
									else if(rowData.ext == "mp3"){
										this.props.navigation.navigate('Player',
											{user: this.props.navigation.state.params.user,
											 environment: this.props.navigation.state.params.environment,
											 topic: this.props.navigation.state.params.topic,
											 lid: rowData.lid,
											 textSubs: rowData.text+"",
											 path: rowData.path,
											 name: rowData.filename,
											 connected: this.props.navigation.state.params.connected,
											 fromRecoring: false,
											 ext: rowData.ext+"",}
										);
										this.componentWillUnmount();
									}
									else
										alert("File type not supported");
								}}
								title = {rowData.lid+""}
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
		return fetch('http://reaching4english-001-site1.itempurl.com/Lessons/lessonQuery.php?userType=' + this.props.navigation.state.params.user + 
			' &env=' + this.props.navigation.state.params.environment + 
			' &tid=' + this.props.navigation.state.params.topic)
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
			tx.executeSql('SELECT DISTINCT userType,env,tid,lid,filename,text,path,ext FROM lessons WHERE userType = ? AND env = ? AND tid = ?;', 
			[this.props.navigation.state.params.user,this.props.navigation.state.params.environment,this.props.navigation.state.params.topic], 
			(_, { rows: { _array } }) => this.setState({ dataSource: ds.cloneWithRows(_array) }));
		});
	}
}

export default LessonScreen;
