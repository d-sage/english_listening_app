import React from 'react';
import { View, Text, Button } from 'react-native'; 
import styles from "./Styles.js";
import backend from "./BackendCalls.js";

const HomeScreen = ({ navigation }) => (
	<View style={styles.mainContainer}>
		<View style={styles.headerContainer}>
			<Text>Home Screen</Text>
		</View>
		<View style={styles.buttonContainer}>
			<Button
				onPress={() => navigation.navigate('Grade')}
				title="Select Grade"
			/>
		</View>
		<View style={styles.buttonContainer}>
			<Button
				onPress={backend}
				title="Get a message from url php backend" 
			/>
		</View>
		<View style={styles.buttonContainer}>
			<Button
                onPress={() => navigation.navigate('Player')}
				title="Player Screen"
			/>
		</View>  
	</View>
);

export default HomeScreen;