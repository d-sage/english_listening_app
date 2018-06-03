/*
*
* Use a StackNavigator for all the screens and wrap the 
* stack navigator within a MenuProvider from react-native-popup-menu
* so the menu will be on each screen. In ComponentWillMount
* we create the directory for saved recordings.
*
*/


import React from 'react';
import CountryScreen from "./CountryScreen.js";
import GradeScreen from "./GradeScreen.js";
import TopicScreen from "./TopicScreen.js";
import LessonScreen from "./LessonScreen.js";
import PlayerScreen from "./PlayerScreen.js";
import RecordingsScreen from "./RecordingsScreen.js";
import MenuIcon from "./MenuIcon.js";
import { FileSystem } from 'expo';
import { createStackNavigator } from 'react-navigation';
import { MenuProvider } from 'react-native-popup-menu';
	
const RootNavigator = createStackNavigator({
	Country: {
		screen: CountryScreen,
		navigationOptions: {
			headerTitle: 'Country',
			headerRight: <MenuIcon />
		},
	},
	Grade: {
		screen: GradeScreen,
		navigationOptions: {
			headerTitle: 'Grade',
			headerRight: <MenuIcon />
		},
	},
	Topic: {
		screen: TopicScreen,
		navigationOptions: {
			headerTitle: 'Topic',
			headerRight: <MenuIcon />
		},
	},
	Lesson: {
		screen: LessonScreen,
		navigationOptions: {
			headerTitle: 'Lesson',
			headerRight: <MenuIcon />
		},
	},
	Player: {
    	screen: PlayerScreen,
    	navigationOptions: {
    		headerTitle: 'Player',
			headerRight: <MenuIcon />
    	},
    },
	Recordings: {
    	screen: RecordingsScreen,
    	navigationOptions: {
    		headerTitle: 'Recordings',
			headerRight: <MenuIcon />
    	},
    },
});

export default class App extends React.Component {
	
	async componentWillMount(){
		try{ 
			//await FileSystem.deleteAsync(FileSystem.documentDirectory+'recordings')
			await FileSystem.makeDirectoryAsync(FileSystem.documentDirectory+'recordings');
		}
		catch(e){}//do nothing, directory already exists.
	}
	
	render() {
		return (
			<MenuProvider>
				<RootNavigator />
			</MenuProvider>
		);
	}
}
