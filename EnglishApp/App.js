import CountryScreen from "./CountryScreen.js";
import GradeScreen from "./GradeScreen.js";
import TopicScreen from "./TopicScreen.js";
import LessonScreen from "./LessonScreen.js";
import PlayerScreen from "./PlayerScreen.js";
import { StackNavigator } from 'react-navigation';

const RootNavigator = StackNavigator({  
	Country: {
		screen: CountryScreen,
		navigationOptions: {
			headerTitleStyle: { alignSelf: 'center' },
			title: 'Country',
		},
	},
	Grade: { 
		screen: GradeScreen, 
		navigationOptions: {
			headerTitle: 'Grade',
		},
	},
	Topic: {
		screen: TopicScreen,
		navigationOptions: { 
			headerTitle: 'Topic', 
		},
	}, 
	Lesson: {
		screen: LessonScreen,
		navigationOptions: { 
			headerTitle: 'Lesson',
		},
	},
	Player: {
    	screen: PlayerScreen,
    	navigationOptions: {
    		headerTitle: 'Player',
    	},
    },
});

export default RootNavigator; 
