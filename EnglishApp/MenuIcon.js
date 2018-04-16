//NEED TO CHANGE WHO WE EMAIL THE BUG TO (LINE 23)
//NEED TO FILL IN THE "ABOUT" PART.

import React from 'react';
import { View, Text, Button, Image, ScrollView } from 'react-native';
import email from 'react-native-email';
import styles from "./Styles.js";
import {
  Menu,
  MenuOptions,
  MenuOption,
  MenuTrigger,
} from 'react-native-popup-menu';

const about = "This is where we put \"ABOUT\" stuff!";

export default class MenuIcon extends React.Component {	
	render(){
		return(
			<View>
				<Menu onSelect={value => {
					if(value == "about")
						alert(about);
					else if(value == "report")
						email('oneel3403@gmail.com', {subject: 'Report A Bug', body: 'Please report your bug here.'}).catch(console.error);
				}}>
					<MenuTrigger > 
						<Image source={require('./assets/images/menu_button.png')} />
					</MenuTrigger>
					<MenuOptions>
						<MenuOption value={"about"} text='About' />
						<MenuOption value={"report"}>
							<Text style={{color: 'red'}}>Report Error</Text>
						</MenuOption>
					</MenuOptions>
				</Menu>
			</View>
		);
	}
	
	constructor(props){
		super(props);
	}
}