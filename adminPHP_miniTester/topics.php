<?php


	include_once "./icons/smileyFace.php";
	include_once "./libraries/databases.php";
	
	
	$verb = $_SERVER["REQUEST_METHOD"];
	$givenParams = array();
	parse_str(file_get_contents('PHP://input'), $givenParams);
	
	$response = array();
	$httpCode = 200;
	
	$statementString = "";
	$variables = array();
	$stmt = false;
	

	if($verb == 'GET')																		//GET
	{
		$givenParams = $_GET;
		
		if(sizeof($givenParams) == 0)
		{
			/*this is a get all*/
			
			$statementString = "SELECT tid FROM topics";
			
			try
			{
				$stmt = executeDbQuery_noVars($pdo, $statementString);
			}
			catch(PDOException $e)
			{
				$stmt = false;
				$response['Response'] = "Topics: get all failed";
			}
		}
		else if(isset($givenParams['tid']))
		{
			$tid = $givenParams['tid'];
			
			$statementString = "SELECT tid FROM topics WHERE tid = :tid";
			$variables = ['tid' => $tid];
			
			try
			{
				$stmt = executeDbCall($pdo, $statementString, $variables);
			}
			catch(PDOException $e)
			{
				$stmt = false;
				$response['Response'] = "Topics: get specific failed";
			}
		}
		else
		{
			$response['Response'] = "Topics: no valid parameter";
		}
		
		closeDB($pdo);
		
		if($stmt == true)
		{
			//$response['Error'] = "false";
			$response['Response'] = "OK";
			$response['Data'] = $stmt->fetchAll(PDO::FETCH_ASSOC);
		}
		else
		{
			$httpCode = 500;
			//$response['Error'] = "true";
			//if(!isset($response['Response']))
			//{
			//	$response['Response'] = "'GET' unnsuccessful";
			//}
			$response['Data'] = [];
		}
		
	}
	else if($verb == 'POST')																//POST
	{
		$givenParams = $_POST;
		
		//Paramter array used to check that all are present
		$requiredParams = ["tid"];
		
		$httpCode = checkAllParamsPresent($requiredParams, $givenParams, $response);
		
		if($httpCode == 200)
		{
			$statementString = "INSERT INTO topics (tid) VALUES (:tid)";
			$variables = ['tid' => $givenParams['tid']];
			
			try
			{
				$stmt = executeDbCall($pdo, $statementString, $variables);
			}
			catch(PDOException $e)
			{
				$stmt = false;
				$response['Response'] = "Topics: insert failed";
				//$response['Response'] = $e->getMessage();
			}
		}
		else
		{
			$response['Response'] = "Topics: insert failed";
		}
		
		closeDB($pdo);
		
		if($stmt == true)
		{
			//$response['Error'] = "false";
			$response['Response'] = "OK";
			$response['Data'] = [];
		}
		else
		{
			//$response['Error'] = "true";
			//if(!isset($response['Response']))
			//{
			//	$response['Response'] = "'POST' unnsuccessful";
			//}
			$response['Data'] = [];
		}
		
	}
	else if($verb == 'PUT')																	//PUT
	{
		//Paramter array used to check that all are present
		$requiredParams = ["oldTid", "newTid"];
		
		$httpCode = checkAllParamsPresent($requiredParams, $givenParams, $response);
		
		if($httpCode == 200)
		{
			
			$statementString = "UPDATE topics SET tid = :newTid "
								. "WHERE tid = :oldTid";
			$variables = ['newTid' => $givenParams["newTid"],
						'oldTid' => $givenParams["oldTid"]
			];
			
			try
			{
				$stmt = executeDbCall($pdo, $statementString, $variables);
			}
			catch(PDOException $e)
			{
				$stmt = false;
				$response['Response'] = "Topics: update failed";
			}
		}
		
		closeDB($pdo);
		
		if($stmt == true)
		{
			//$response['Error'] = "false";
			$response['Response'] = "OK";
			$response['Data'] = [];
		}
		else
		{
			//$response['Error'] = "true";
			//if(!isset($response['Response']))
			//{
			//	$response['Response'] = "'PUT' unnsuccessful";
			//}
			$response['Data'] = [];
		}
	}
	else if($verb == 'DELETE')																//DELETE
	{
		if(isset($givenParams["tid"]))
		{
			$tid = $givenParams["tid"];
			
			$statementString = "DELETE FROM topics WHERE tid = :tid";
			$variables = ['tid' => $tid];
			
			try
			{
				$stmt = executeDbCall($pdo, $statementString, $variables);
			}
			catch(PDOException $e)
			{
				$stmt = false;
				$response['Response'] = "Topics: delete failed";
			}
		}
		else
		{
			$response['Response'] = "Topics: no valid parameter";
		}
		
		closeDB($pdo);
		
		if($stmt == true)
		{
			//$response['Error'] = "false";
			$response['Response'] = "OK";
			$response['Data'] = [];
		}
		else
		{
			//$response['Error'] = "true";
			//if(!isset($response['Response']))
			//{
			//	$response['Missing']['id'] = "not present";
			//	$response['Response'] = "'DELETE' unnsuccessful";
			//}
			$response['Data'] = [];
		}
	}
	else
	{
		$response['Response'] = "Invalid Request";
		$response['Data'] = [];
	}


	http_response_code($httpCode);
	echo json_encode($response);

?>