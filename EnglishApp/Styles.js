import { StyleSheet, Dimensions, } from 'react-native';

const { width: DEVICE_WIDTH, height: DEVICE_HEIGHT } = Dimensions.get('window');
const BACKGROUND_COLOR = '#87ceeb';

const styles = StyleSheet.create({ 
	mainContainer: {
		flex: 1,
		justifyContent: 'center',
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
	textBox: {
		fontSize: 20,
		margin: 10,
		textAlign: 'center',
		backgroundColor: 'white',
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
		minHeight: DEVICE_HEIGHT / 2.5,
		maxHeight: DEVICE_HEIGHT / 2.5,
		backgroundColor: 'whitesmoke',
		borderWidth: 1.0,
		borderColor: 'black'
	}, 
	buttonsContainer: {
		flex: 1,
		flexDirection: 'row',
		width: DEVICE_WIDTH,
		height: (DEVICE_HEIGHT / 2) / 2,
	},
	halfButtonsContainer: {
		flex: 1,
		flexDirection: 'row',
		width: DEVICE_WIDTH / 2,
		height: (DEVICE_HEIGHT / 2) / 2,
	},
	fourthButtonsContainer: {
		flex: 1,
		flexDirection: 'column',
		alignItems: 'center',
		width: DEVICE_WIDTH / 4,
		height: (DEVICE_HEIGHT / 4) / 2,
	},
	fifthButtonsContainer: {
		flex: 1,
		flexDirection: 'column',
		alignItems: 'center',
		width: DEVICE_WIDTH / 5,
		height: (DEVICE_HEIGHT / 4) / 2,
	},
	sliderContainer: {
		flex: 1,
		flexDirection: 'column',
		alignItems: 'center',
		width: DEVICE_WIDTH,
		height: (DEVICE_HEIGHT / 4) / 2,
	},
})

export default styles; 