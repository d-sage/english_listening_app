import React from 'react';
import { View, Text, Button, ListView } from 'react-native';
import styles from "./Styles.js";

var ds = new ListView.DataSource({ rowHasChanged: (row1, row2) => row1 !== row2 });

class TopicScreen extends React.Component {

	render(){
		return(
			<View style={styles.mainContainer}>
				<View style={styles.headerContainer}>
					<Text>Topic Screen</Text>
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
									 topic: rowData.tid+""})}
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
		this.fetchData();
	}

	fetchData(){
		return fetch('http://jordanlambertonline.com/EnglishApp/Topics/topicQuery.php?cid=' + this.props.navigation.state.params.country + ' &gid=' + this.props.navigation.state.params.grade)
		.then((response) => response.json())
		.then((responseJson) => {
			if(responseJson){
				this.setState({
					dataSource: ds.cloneWithRows(responseJson),
				});
			}
		}).done();
	}
}

export default TopicScreen;
