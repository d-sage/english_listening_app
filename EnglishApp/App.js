import CountryScreen from "./CountryScreen.js";
import GradeScreen from "./GradeScreen.js";
import TopicScreen from "./TopicScreen.js";
import LessonScreen from "./LessonScreen.js";
import PlayerScreen from "./PlayerScreen.js";
import LocalCountryScreen from "./LocalCountryScreen.js";
import LocalGradeScreen from "./LocalGradeScreen.js";
import LocalTopicScreen from "./LocalTopicScreen.js";
import LocalLessonScreen from "./LocalLessonScreen.js";
import LocalPlayerScreen from "./LocalPlayerScreen.js";
import SQLTestScreen from "./SQLTestScreen.js";
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
		LocalCountry: {
			screen: LocalCountryScreen,
			navigationOptions: {
				headerTitleStyle: { alignSelf: 'center' },
				title: 'LocalCountry',
			},
		},
		LocalGrade: {
			screen: LocalGradeScreen,
			navigationOptions: {
				headerTitle: 'LocalGrade',
			},
		},
		LocalTopic: {
			screen: LocalTopicScreen,
			navigationOptions: {
				headerTitle: 'LocalTopic',
			},
		},
		LocalLesson: {
			screen: LocalLessonScreen,
			navigationOptions: {
				headerTitle: 'LocalLesson',
			},
		},
		LocalPlayer: {
	    	screen: LocalPlayerScreen,
	    	navigationOptions: {
	    		headerTitle: 'LocalPlayer',
	    	},
	    },
		Test: {
	 		 screen: SQLTestScreen,
	 		 navigationOptions: {
	 			 headerTitle: 'Test',
	 		 },
	 	 },
});

export default RootNavigator;
