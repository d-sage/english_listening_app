import React from 'react';
import { View, Text, Button } from 'react-native';
import styles from "./Styles.js";

const TopicScreen = ({ navigation }) => (
	<View style={styles.mainContainer}>
		<View style={styles.headerContainer}>
			<Text>Topic Screen</Text>
		</View>	
		<View style={styles.buttonContainer}>
			<Button
				onPress={() => navigation.navigate('Lesson')}
				title="Select Lesson"
			/>
		</View>
	</View>
);

export default TopicScreen;