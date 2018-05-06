import React from 'react';
import { View, Text, Button, ListView } from 'react-native';
import styles from "./Styles.js";
import Expo, { SQLite } from 'expo';

var ds = new ListView.DataSource({ rowHasChanged: (row1, row2) => row1 !== row2 });
const db = SQLite.openDatabase('db.db');

class TopicScreen extends React.Component {

	render(){
		return(
			<View style={styles.mainContainer}>
				<View style={styles.headerContainer}>
					<Text style={{fontSize: 20}}>Select Topic</Text>
				</View>
				<ListView
					enableEmptySections
					style={styles.dataContainer}
					dataSource={this.state.dataSource}
					renderRow={(rowData) =>
						<View style={styles.buttonContainer}>
							<Button
								onPress={() => this.props.navigation.navigate('Lesson',
									{country: this.props.navigation.state.params.country,
									 grade: this.props.navigation.state.params.grade,
									 topic: rowData.tid,
									 connected: this.props.navigation.state.params.connected,})}
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

	componentWillMount() {
		if(this.props.navigation.state.params.connected)
			this.fetchOnlineData();
		else
			this.fetchOfflineData();
	}

	fetchOnlineData(){
		return fetch('http://lambejor-001-site1.htempurl.com/Topics/topicQuery.php?cid=' + this.props.navigation.state.params.country + 
			' &gid=' + this.props.navigation.state.params.grade)
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
			tx.executeSql('SELECT DISTINCT cid,gid,tid FROM lessons WHERE cid = ? AND gid = ?;', 
			[this.props.navigation.state.params.country,this.props.navigation.state.params.grade], (_, { rows: { _array } }) => 
				this.setState({ dataSource: ds.cloneWithRows(_array) }));
		});
	}
}

export default TopicScreen;
