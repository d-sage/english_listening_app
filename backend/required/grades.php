<?php


	/**
	
	
	Placeholder for an admin 'grades' database access.
	
	I thougth about it and realized that the admin may
	not have any use for get/post/put/delete in regards
	to grades	
	
	
	**/











	include_once "./icons/smileyFace.php";
	include_once "./libraries/databases.php";
	
	
	$verb = $_SERVER["REQUEST_METHOD"];
	$parameters = array();
	parse_str(file_get_contents('PHP://input'), $input);
	
	$response = array();
	
	$statementString = "";
	$variables = array();
	$stmt = false;
	

	if($verb == 'GET')																		//GET
	{
		
	}
	else if($verb == 'POST')																//POST
	{
		
	}
	else if($verb == 'PUT')																	//PUT
	{
		
	}
	else if($verb == 'DELETE')																//DELETE
	{
		
	}
	else
	{
		
	}



?>