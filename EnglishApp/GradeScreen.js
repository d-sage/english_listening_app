import React from 'react';
import { View, Text, Button, ListView } from 'react-native';
import Expo, { SQLite } from 'expo';
import styles from "./Styles.js";

var ds = new ListView.DataSource({ rowHasChanged: (row1, row2) => row1 !== row2 });
const db = SQLite.openDatabase('db.db');

class GradeScreen extends React.Component {

	render(){
		return(
			<View style={styles.mainContainer}>
				<View style={styles.headerContainer}>
					<Text>Grade Screen</Text>
				</View>
				<ListView
					enableEmptySections
					style={styles.dataContainer}
					dataSource={this.state.dataSource}
					renderRow={(rowData) =>
						<View style={styles.buttonContainer}>
							<Button
								onPress={() => this.props.navigation.navigate('Topic',
									{country: this.props.navigation.state.params.country,
									 grade: rowData.gid,
									 connected: this.props.navigation.state.params.connected})}
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

	componentWillMount() {
		if(this.props.navigation.state.params.connected)
			this.fetchOnlineData();
		else
			this.fetchOfflineData();
	}

	fetchOnlineData(){
		return fetch('http://lambejor-001-site1.htempurl.com/Grades/gradeQuery.php?cid=' + this.props.navigation.state.params.country)
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
			tx.executeSql('SELECT DISTINCT cid, gid FROM lessons WHERE cid = ?;', [this.props.navigation.state.params.country], (_, { rows: { _array } }) => this.setState({ dataSource: ds.cloneWithRows(_array) }));
		});
	}
}

export default GradeScreen;
