import { StyleSheet } from 'react-native';

const LIVE_COLOR = '#FF0000';

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
})

export default styles; 