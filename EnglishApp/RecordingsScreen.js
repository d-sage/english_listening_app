import React from 'react';
import { View, Text, Button, ListView } from 'react-native';
import styles from "./Styles.js";
import { FileSystem } from 'expo';
import { NavigationActions, StackActions } from 'react-navigation';

var ds = new ListView.DataSource({ rowHasChanged: (row1, row2) => row1 !== row2 });

class RecordingsScreen extends React.Component {

	render(){
		return(
			<View style={styles.mainContainer}>
				<View style={styles.headerContainer}>
					<Text style={{fontSize: 20}}>Select Recording</Text>
				</View>
				<ListView
					enableEmptySections
					style={styles.dataContainer}
					dataSource={this.state.dataSource}
					renderRow={(rowData) =>
						<View style={styles.buttonContainer}>
							<Button color='#396FB6'
								onPress={ () => { this.getInfo(rowData+"") }}
								title = {rowData+""}
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
			resetActionPlayer: null,
        };
	}
	
	async getInfo(rowData){
		await this.setState({
			resetActionPlayer: StackActions.reset({
				index: 0,
				actions: [
					NavigationActions.navigate({ routeName: 'Player', 
						params: {
							user: this.props.navigation.state.params.user,
							environment: this.props.navigation.state.params.environment,
							topic: this.props.navigation.state.params.topic,
							lid: this.props.navigation.state.params.lid,
							textSubs: 'No Text',
							path: FileSystem.documentDirectory + 'recordings/' + rowData,
							name: rowData,
							connected: this.props.navigation.state.params.connected,
							fromRecording: true, 
						}
					}),
				],
			}
			), 
		});
		this.props.navigation.dispatch(this.state.resetActionPlayer);
	}
	
	componentDidMount() {
		this.setState({ dataSource: ds.cloneWithRows(this.props.navigation.state.params.recordings)});
	}
}

export default RecordingsScreen;
