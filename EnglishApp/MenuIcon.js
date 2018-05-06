//NOT SURE IF THE TRY CATCH WILL WORK WHILE OFFLINE (LINE 27, 28)
//NEED TO FILL IN THE "ABOUT" PART.

import React from 'react';
import { View, Text, Button, Image, ScrollView, Linking } from 'react-native';
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
						email('reachingforenglish@gmail.com', {subject: 'Report A Bug', body: 'Please report your bug here.'}).catch(console.error);
					else if(value == "survey"){
						try{ Linking.openURL('http://lambejor-001-site1.htempurl.com/PDFViewer.aspx'); }
						catch(e){ alert('Cannot open survey');}
					}
				}}>
					<MenuTrigger > 
						<Image source={require('./assets/images/menu_button.png')} />
					</MenuTrigger>
					<MenuOptions>
						<MenuOption value={"about"} text='About' />
						<MenuOption value={"survey"} text='Survey' />
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