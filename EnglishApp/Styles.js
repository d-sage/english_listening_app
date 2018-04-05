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
		minHeight: DEVICE_HEIGHT / 3.2,
		maxHeight: DEVICE_HEIGHT / 3.2,
		backgroundColor: BACKGROUND_COLOR,
		borderWidth: 1.0,
		borderColor: 'black'
	},
	playStopContainer: {
		flex: 1,
		flexDirection: 'row',
		alignItems: 'flex-end',
		justifyContent: 'space-around',  
	},
	downloadRecordContainer: {
		flex: 1,
		flexDirection: 'row',
		alignItems: 'flex-start',
	},
	buttonsContainer: {
		flex: 1,
		flexDirection: 'row',
		alignItems: 'center',
	},
	textContainer: {
		flex: 1,
		flexDirection: 'column',
		alignItems: 'center',
		alignSelf: 'stretch',
		minHeight: DEVICE_HEIGHT / 4.0,
		maxHeight: DEVICE_HEIGHT / 4.0,
		backgroundColor: 'whitesmoke',
		borderWidth: 1.0,
		borderColor: 'black'
	}, 
	textBox: {
		fontSize: 20,
		margin: 10,
		textAlign: 'center',
   },
	recordingContainer: {
		flexDirection: 'column',
		flex: 1,
		alignItems: 'center',
	},
	downloadContainer: {
		flex: 1,
		alignItems: 'flex-start',
		paddingLeft: 20,
		paddingTop: 32,
	},
	buttonPlayerContainer: {
		flex: 1,
		flexDirection: 'column',
		alignItems: 'center',
	},
   
})

export default styles; 