import React from 'react';
import { View, Text, Image, Linking } from 'react-native';
import email from 'react-native-email';
import styles from "./Styles.js";
import {
  Menu,
  MenuOptions,
  MenuOption,
  MenuTrigger,
} from 'react-native-popup-menu';

const about =   "Version: 1.0.0 \n"+
				"Owner: Stan Pichinevskiy \n"+
				"Developed by: \n"+
				"Jordan Lambert \n"+
				"Justin O'Neel \n"+
				"Daric Sage \n"+
				"Jared Regan \n\n"+

				"English listening assistant for Educative English Material. \n"+
				"For information on how to use the app please visit: (Enter URL to website with info on using the app since the apple store requires one?) \n";

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
						try{ Linking.openURL('http://reaching4english-001-site1.itempurl.com/PDFViewer.aspx'); }
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