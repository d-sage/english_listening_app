import React from 'react';
import { View, Text, Button, ListView } from 'react-native';
import styles from "./Styles.js";

var ds = new ListView.DataSource({ rowHasChanged: (row1, row2) => row1 !== row2 });

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
									 grade: rowData.gid + ""})}
								title = {rowData.gid + ""}
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
		return fetch('http://jordanlambertonline.com/EnglishApp/Grades/gradeQuery.php?cid=' + this.props.navigation.state.params.country)
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

export default GradeScreen;
