import { StyleSheet, Dimensions, Platform } from 'react-native';

const { width: DEVICE_WIDTH, height: DEVICE_HEIGHT } = Dimensions.get('window');
const BACKGROUND_COLOR = '#87ceeb';

const styles = StyleSheet.create({ 
	mainContainer: {
		flex: 1,
		justifyContent: 'center',
		paddingTop: (Platform.OS === 'ios') ? 20 : 0, 
	},
	buttonContainer: {
		margin: 15
	},
	headerContainer: {
		margin: 20,
		alignItems: 'center',
	},
	dataContainer: {
		flex: 1,
		marginTop: 20,
	},	
	text: {
		textAlign: 'center',
        fontSize: 15,
        color: 'black',
        padding: 10,
		backgroundColor: BACKGROUND_COLOR,
    },	
	textBox: {
		fontSize: 20,
		margin: 10,
		textAlign: 'left',
		backgroundColor: 'whitesmoke',
	},
	image: {
		backgroundColor: BACKGROUND_COLOR, 
	},
	wrapper: {},
	playbackSlider: {
		alignSelf: 'stretch',
	},
	playerContainer: {
		flex: 1,
		flexDirection: 'column',
		justifyContent: 'space-between',
		alignItems: 'center',
		alignSelf: 'stretch',
		minHeight: DEVICE_HEIGHT / 4.3,
		maxHeight: DEVICE_HEIGHT / 4.3,
		backgroundColor: BACKGROUND_COLOR,
		borderWidth: 1.0,
		borderColor: 'black'
	},
	textContainer: {
		flex: 1,
		flexDirection: 'column',
		alignItems: 'center',
		alignSelf: 'stretch',
		backgroundColor: 'whitesmoke',
		borderWidth: 1.0,
		borderColor: 'black',
	}, 
	buttonsContainer: {
		flex: 1,
		flexDirection: 'row',
		width: DEVICE_WIDTH,
		height: (DEVICE_HEIGHT / 2) / 2,
	},
	buttonsContainers: {
		flex: 1,
		flexDirection: 'column',
		alignItems: 'center',
	},
	sliderContainer: {
		flex: 1,
		flexDirection: 'column',
		alignItems: 'center',
		width: DEVICE_WIDTH,
	},
	thinContainer: {
		flex: 1,
		flexDirection: 'row',
		alignItems: 'center',
	},
},{ headerMode: 'none' })

export default styles; 