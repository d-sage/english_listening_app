import React from 'react';
import styles from "./Styles.js";
import Expo, { SQLite } from 'expo';
import { View, Text, Button, ListView } from 'react-native';


var ds = new ListView.DataSource({ rowHasChanged: (row1, row2) => row1 !== row2 });
const db = SQLite.openDatabase('db.db');

class LocalTopicScreen extends React.Component {

	state = {
		items: [],
	};

	componentDidMount() {
		db.transaction(tx => {
			//tx.executeSql('CREATE TABLE IF NOT EXISTS lessons (cid varchar(30) NOT NULL, gid tinyint(4) NOT NULL, tid varchar(30) NOT NULL, lid varchar(30) NOT NULL, text varchar(500) NOT NULL, path varchar(260) NOT NULL, PRIMARY KEY (cid, gid, tid, lid));');
			tx.executeSql('select cid,gid,tid from lessons where cid = ? and gid = ?;', [this.props.navigation.state.params.country,this.props.navigation.state.params.grade], (_, { rows: { _array } }) => this.setState({ items: _array }));
		});
	}

	render() {
		if (this.state.items === null || this.state.items.length === 0) {
			return null;
		}
		return (
			<View style={styles.buttonContainer}>
				{this.state.items.map(({cid,gid,tid}) => (
					<Button
						//key = {cid}
						onPress={() => this.props.navigation.navigate('LocalLesson',
							{country: cid,
							 grade: gid+"",
						 	 topic: tid})}
						title = {tid}
					/>
				))}
			</View>
		);
	}
}

export default LocalTopicScreen;
