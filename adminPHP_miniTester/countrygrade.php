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
			
			$statementString = "SELECT cid, gid FROM country_grade_relationship";
			
			try
			{
				$stmt = executeDbQuery_noVars($pdo, $statementString);
			}
			catch(PDOException $e)
			{
				$stmt = false;
				$response['Response'] = "Country-Grade: get all failed";
			}
		}
		else if(isset($givenParams['cid']))
		{
			$cid = $givenParams['cid'];
			
			$statementString = "SELECT cid, gid FROM country_grade_relationship WHERE cid = :cid";
			$variables = ['cid' => $cid];
			
			try
			{
				$stmt = executeDbCall($pdo, $statementString, $variables);
			}
			catch(PDOException $e)
			{
				$stmt = false;
				$response['Response'] = "Country-Grade: get specific failed";
			}
		}
		else
		{
			$response['Response'] = "Country-Grade: no valid parameter";
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
		$requiredParams = ["cid","gid"];
		
		$httpCode = checkAllParamsPresent($requiredParams, $givenParams, $response);
		
		if($httpCode == 200)
		{
			$statementString = "INSERT INTO country_grade_relationship (cid,gid) VALUES (:cid,:gid)";
			$variables = ['cid' => $givenParams['cid'],
						'gid' => $givenParams['gid'],
			];
			
			try
			{
				$stmt = executeDbCall($pdo, $statementString, $variables);
			}
			catch(PDOException $e)
			{
				$stmt = false;
				$response['Response'] = "Country-Grade: insert failed";
				//$response['Response'] = $e->getMessage();
			}
		}
		else
		{
			$response['Response'] = "Country-Grade: insert failed";
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
		$requiredParams = ["oldCid", "newCid", "oldGid", "newGid"];
		
		$httpCode = checkAllParamsPresent($requiredParams, $givenParams, $response);
		
		if($httpCode == 200)
		{
			
			$statementString = "UPDATE country_grade_relationship SET cid = :newCid, gid = :newGid "
								. "WHERE cid = :oldCid AND gid = :oldGid";
			$variables = ['newCid' => $givenParams["newCid"],
						'oldCid' => $givenParams["oldCid"],
						'newGid' => $givenParams["newGid"],
						'oldGid' => $givenParams["oldGid"]
			];
			
			try
			{
				$stmt = executeDbCall($pdo, $statementString, $variables);
			}
			catch(PDOException $e)
			{
				$stmt = false;
				$response['Response'] = "Country-Grade: update failed";
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
		
		//Paramter array used to check that all are present
		$requiredParams = ["cid", "gid"];
		
		$httpCode = checkAllParamsPresent($requiredParams, $givenParams, $response);
		
		if($httpCode == 200)
		{
			
			$statementString = "DELETE FROM country_grade_relationship WHERE cid = :cid AND gid = :gid";
			$variables = ['cid' => $givenParams['cid'],
						'gid' => $givenParams['gid']
			];
			
			try
			{
				$stmt = executeDbCall($pdo, $statementString, $variables);
			}
			catch(PDOException $e)
			{
				$stmt = false;
				$response['Response'] = "Country-Grade: delete failed";
			}
		}
		else
		{
			$response['Response'] = "Country-Grade: delete failed";
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