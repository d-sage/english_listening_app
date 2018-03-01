import React from 'react';
import Expo, { SQLite } from 'expo';
import styles from "./Styles.js";
import { Text, View, Button } from 'react-native';

const db = SQLite.openDatabase('db.db');

export default class SQLTestScreen extends React.Component {

	state = {
		items: [],
	};

	componentDidMount() {
		db.transaction(tx => {
			//tx.executeSql('DROP TABLE IF EXISTS lessons;');

			tx.executeSql('CREATE TABLE IF NOT EXISTS lessons (cid varchar(30) NOT NULL, gid tinyint(4) NOT NULL, tid varchar(30) NOT NULL, lid varchar(30) NOT NULL, text varchar(500) NOT NULL, path varchar(260) NOT NULL, PRIMARY KEY (cid, gid, tid, lid));');

			tx.executeSql('insert OR IGNORE into lessons (cid, gid, tid, lid, text, path) values (?, ?, ?, ?, ?, ?)', [this.props.navigation.state.params.country, this.props.navigation.state.params.grade, this.props.navigation.state.params.topic, this.props.navigation.state.params.lid, "poop", this.props.navigation.state.params.path]);

			//tx.executeSql('select * from lessons', [], (_, { rows }) =>
			//	console.log(JSON.stringify(rows))
			//);
			tx.executeSql('select * from lessons;', [], (_, { rows: { _array } }) => this.setState({ items: _array }));
		});
	}

	render() {
		if (this.state.items === null || this.state.items.length === 0) {
			return null;
		}
		return (
			<View style={styles.buttonContainer}>
				{this.state.items.map(({ cid, gid, tid, lid, text, path }) => (
					<Button
						key = {cid}
						onPress={() => alert(cid + " " + gid + " " + tid + " " + lid)}
						title = {path}
					/>
				))}
			</View>
		);
	}
}
