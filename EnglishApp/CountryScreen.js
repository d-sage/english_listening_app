import React from 'react';
import styles from "./Styles.js";
import Expo, { SQLite } from 'expo';
import { View, Text, Button, ListView, NetInfo } from 'react-native';

var ds = new ListView.DataSource({ rowHasChanged: (row1, row2) => row1 !== row2 });
const db = SQLite.openDatabase('db.db');

class CountryScreen extends React.Component {
	render(){
		return(
			<View style={styles.mainContainer}>
				<View style={styles.headerContainer}>
					<Text>Country Screen</Text>
				</View>
				<ListView
					enableEmptySections
					style={styles.dataContainer}
					dataSource={this.state.dataSource}
					renderRow={(rowData) =>
						<View style={styles.buttonContainer}>
							<Button
								onPress={() => this.props.navigation.navigate('Grade',
									{country: rowData.cid,
									 connected: this.state.connected})}
								title = {rowData.cid+""}
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
		  connected: false,
        };
	}

	componentDidMount() {
		db.transaction(tx => {
			//tx.executeSql('DROP TABLE IF EXISTS lessons;');
			tx.executeSql('CREATE TABLE IF NOT EXISTS lessons (cid varchar(30) NOT NULL, gid tinyint(4) NOT NULL, tid varchar(30) NOT NULL, lid varchar(30) NOT NULL, text varchar(500) NOT NULL, path varchar(260) NOT NULL, PRIMARY KEY (cid, gid, tid, lid));');
		});
	}

	componentWillMount() {
		NetInfo.isConnected.fetch().then(isConnected => {
			this.setState({connected: isConnected}, () => this.getData());
		});
	}

	getData(){
		if(this.state.connected)
			this.fetchOnlineData();
		else
			this.fetchOfflineData();
	}

	fetchOnlineData(){
		return fetch('http://jordanlambertonline.com/EnglishApp/Countries/countryQuery.php')
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
