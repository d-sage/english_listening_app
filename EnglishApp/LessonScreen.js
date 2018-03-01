import React from 'react';
import { View, Text, Button, ListView } from 'react-native';
import styles from "./Styles.js";

var ds = new ListView.DataSource({ rowHasChanged: (row1, row2) => row1 !== row2 });

class LessonScreen extends React.Component {

	render(){
		return(
			<View style={styles.mainContainer}>
				<View style={styles.headerContainer}>
					<Text>Lesson Screen</Text>
				</View>
				<ListView
					enableEmptySections
					style={styles.dataContainer}
					dataSource={this.state.dataSource}
					renderRow={(rowData) =>
						<View style={styles.buttonContainer}>
							<Button
								onPress={() => this.props.navigation.navigate('Player',
									{country: this.props.navigation.state.params.country,
									 grade: this.props.navigation.state.params.grade,
									 topic: this.props.navigation.state.params.topic,
									 lid: rowData.lid,
									 path: rowData.path+""})}
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

	componentWillMount() {
		this.fetchData();
	}

	fetchData(){
		return fetch('http://jordanlambertonline.com/EnglishApp/Lessons/lessonQuery.php?cid=' + this.props.navigation.state.params.country + ' &gid=' + this.props.navigation.state.params.grade + ' &tid=' + this.props.navigation.state.params.topic)
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

export default LessonScreen;
