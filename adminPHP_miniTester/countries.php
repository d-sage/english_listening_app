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
			
			$statementString = "SELECT cid FROM countries";
			
			try
			{
				$stmt = executeDbQuery_noVars($pdo, $statementString);
			}
			catch(PDOException $e)
			{
				$stmt = false;
				$response['Response'] = "Countries: get all failed";
			}
		}
		else if(isset($givenParams['cid']))
		{
			$cid = $givenParams['cid'];
			
			$statementString = "SELECT cid FROM countries WHERE cid = :cid";
			$variables = ['cid' => $cid];
			
			try
			{
				$stmt = executeDbCall($pdo, $statementString, $variables);
			}
			catch(PDOException $e)
			{
				$stmt = false;
				$response['Response'] = "Countries: get specific failed";
			}
		}
		else
		{
			$response['Response'] = "Countries: no valid parameter";
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
		$requiredParams = ["cid"];
		
		$httpCode = checkAllParamsPresent($requiredParams, $givenParams, $response);
		
		if($httpCode == 200)
		{
			$statementString = "INSERT INTO countries (cid) VALUES (:cid)";
			$variables = ['cid' => $givenParams['cid']];
			
			try
			{
				$stmt = executeDbCall($pdo, $statementString, $variables);
			}
			catch(PDOException $e)
			{
				$stmt = false;
				$response['Response'] = "Countries: insert failed";
				//$response['Response'] = $e->getMessage();
			}
		}
		else
		{
			$response['Response'] = "Countries: insert failed";
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
		$requiredParams = ["oldCid", "newCid"];
		
		$httpCode = checkAllParamsPresent($requiredParams, $givenParams, $response);
		
		if($httpCode == 200)
		{
			
			$statementString = "UPDATE countries SET cid = :newCid "
								. "WHERE cid = :oldCid";
			$variables = ['newCid' => $givenParams["newCid"],
						'oldCid' => $givenParams["oldCid"]
			];
			
			try
			{
				$stmt = executeDbCall($pdo, $statementString, $variables);
			}
			catch(PDOException $e)
			{
				$stmt = false;
				$response['Response'] = "Countries: update failed";
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
		if(isset($givenParams["cid"]))
		{
			$cid = $givenParams["cid"];
			
			$statementString = "DELETE FROM countries WHERE cid = :cid";
			$variables = ['cid' => $cid];
			
			try
			{
				$stmt = executeDbCall($pdo, $statementString, $variables);
			}
			catch(PDOException $e)
			{
				$stmt = false;
				$response['Response'] = "Countries: delete failed";
			}
		}
		else
		{
			$response['Response'] = "Countries: no valid parameter";
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