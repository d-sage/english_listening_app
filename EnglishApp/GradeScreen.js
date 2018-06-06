import React from 'react';
import { View, Text, Button, NetInfo, Platform, ListView } from 'react-native';
import Expo, { SQLite } from 'expo';
import styles from "./Styles.js";
import { NavigationActions } from 'react-navigation';

var ds = new ListView.DataSource({ rowHasChanged: (row1, row2) => row1 !== row2 });
const db = SQLite.openDatabase('db.db');
const resetActionCountry = NavigationActions.reset({
  index: 0,
  actions: [NavigationActions.navigate({ routeName: 'Country' })],
}); 

class GradeScreen extends React.Component {

	render(){
		return(
			<View style={styles.mainContainer}>
				<View style={styles.headerContainer}>
					<Text style={{fontSize: 20}}>Select Grade</Text>
				</View>
				<ListView
					enableEmptySections
					style={styles.dataContainer}
					dataSource={this.state.dataSource}
					renderRow={(rowData) =>
						<View style={styles.buttonContainer}>
							<Button
								onPress={() => {this.props.navigation.navigate('Topic',
									{country: this.props.navigation.state.params.country,
									 grade: rowData.gid,
									 connected: this.props.navigation.state.params.connected});
									 this.componentWillUnmount();
								}}
								title = {rowData.gid+""}
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
			this.props.navigation.dispatch(resetActionCountry);
			this.componentWillUnmount();
		}
		else if(isConnected)
			this.fetchOnlineData();
		else
			this.fetchOfflineData();
	}
	
	fetchOnlineData(){
		return fetch('http://reaching4english-001-site1.itempurl.com/Grades/gradeQuery.php?cid=' + this.props.navigation.state.params.country)
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
			tx.executeSql('SELECT DISTINCT cid, gid FROM lessons WHERE cid = ?;', 
			[this.props.navigation.state.params.country], (_, { rows: { _array } }) => 
				this.setState({ dataSource: ds.cloneWithRows(_array) }));
		});
	}
}

export default GradeScreen;
