import { StackNavigator } from 'react-navigation';
import {
    Player,
    Recorder,
    MediaStates
} from 'react-native-audio-toolkit';
import HomeScreen from "./HomeScreen.js";
import GradeScreen from "./GradeScreen.js";
import TopicScreen from "./TopicScreen.js";
import LessonScreen from "./LessonScreen.js"; 
//import FileSystem from 'react-native-filesystem';

const RootNavigator = StackNavigator({
	Home: {
		screen: HomeScreen,
		navigationOptions: {
			headerTitleStyle: { alignSelf: 'center' },
			title: 'Home',
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
});

export default RootNavigator; 
