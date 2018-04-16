import React from 'react';
import CountryScreen from "./CountryScreen.js";
import GradeScreen from "./GradeScreen.js";
import TopicScreen from "./TopicScreen.js";
import LessonScreen from "./LessonScreen.js";
import PlayerScreen from "./PlayerScreen.js";
import MenuIcon from "./MenuIcon.js";
import { StackNavigator } from 'react-navigation';
import { MenuProvider } from 'react-native-popup-menu';
	
const RootNavigator = StackNavigator({
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
});

export default class App extends React.Component {
  render() {
    return (
      <MenuProvider>
        <RootNavigator />
      </MenuProvider>
    );
  }
}
