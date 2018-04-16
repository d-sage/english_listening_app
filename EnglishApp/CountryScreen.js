import React from 'react';
import styles from "./Styles.js";
import Expo, { SQLite, Permissions, Asset } from 'expo';
import { View, Text, Button, ListView, NetInfo, Platform } from 'react-native';

var ds = new ListView.DataSource({ rowHasChanged: (row1, row2) => row1 !== row2 });
const db = SQLite.openDatabase('db.db');

class CountryScreen extends React.Component {
	
	render(){
		if(this.state.haveRecordingPermissions){
			return(		
				<View style={styles.mainContainer}>
					<View style={styles.headerContainer}>
						<Text style={{fontSize: 20}}>Please select a Country</Text>
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
			);
		}
		else
			return(<View><Text style={{fontSize: 20, color: 'red'}}>You must enable permissions to use this app.</Text></View>);
	}

	constructor(props){
        super(props);
        this.state = {
          dataSource: ds.cloneWithRows([]),
		  connected: false,
		  haveRecordingPermissions: false,
        };
	}

	componentDidMount() {
		this.askForPermissions();
		db.transaction(tx => {
			//tx.executeSql('DROP TABLE IF EXISTS lessons;');
			tx.executeSql('CREATE TABLE IF NOT EXISTS lessons (cid varchar(30) NOT NULL, gid tinyint(4) NOT NULL, tid varchar(30) NOT NULL, lid varchar(30) NOT NULL, text varchar(500) NOT NULL, path varchar(260) NOT NULL, PRIMARY KEY (cid, gid, tid, lid));');
		});
	}

	componentWillMount() {
		if (Platform.OS === 'ios'){
             NetInfo.isConnected.addEventListener('connectionChange', isConnected => {
                    this.setState({connected: isConnected}, () => this.getData());
             }); 
        }
        else{
			NetInfo.isConnected.fetch().then(isConnected => {
				this.setState({connected: isConnected}, () => this.getData());
			});
		}
	}
	
	askForPermissions = async () => {
		const response = await Permissions.askAsync(Permissions.AUDIO_RECORDING);
		this.setState({
			haveRecordingPermissions: response.status === 'granted',
		});
	};

	getData(){
		if(this.state.connected)
			this.fetchOnlineData();
		else
			this.fetchOfflineData();
	}

	fetchOnlineData(){
		return fetch('http://lambejor-001-site1.htempurl.com/Countries/countryQuery.php')
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
