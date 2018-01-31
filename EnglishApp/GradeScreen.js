import React from 'react';
import { View, Text, Button } from 'react-native';
import styles from "./Styles.js";

const GradeScreen = ({ navigation }) => (
	<View style={styles.mainContainer}>
		<View style={styles.headerContainer}>
			<Text>Grade Screen</Text>
		</View>	
		<View style={styles.buttonContainer}>
			<Button
				onPress={() => navigation.navigate('Topic')}
				title="Select Topic"
			/>
		</View>
	</View>
);

export default GradeScreen;