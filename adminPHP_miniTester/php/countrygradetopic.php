<?php


	include_once "../icons/smileyFace.php";
	include_once "../libraries/databases.php";
	include_once "../libraries/topic_lesson_removal.php";
	
	
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
			
			$statementString = "SELECT cid, gid, tid FROM country_grade_topic_relation";
			
			try
			{
				$stmt = executeDbQuery_noVars($pdo, $statementString);
			}
			catch(PDOException $e)
			{
				$stmt = false;
				$response['Response'] = "Country-Grade-Topic: get all failed";
			}
		}
		else
		{
			//Paramter array used to check that all are present
			$requiredParams = ["cid","gid"];
			
			$httpCode = checkAllParamsPresent($requiredParams, $givenParams, $response);
				
			if($httpCode == 200)
			{
				$statementString = "SELECT cid, gid, tid FROM country_grade_topic_relation WHERE cid = :cid AND gid = :gid";
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
					$response['Response'] = "Country-Grade-Topic: get specific failed";
				}
			}
			else
			{
				$response['Response'] = "Country-Grade-Topic: no valid parameter";
			}
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
		$requiredParams = ["cid","gid","tid"];
		
		$httpCode = checkAllParamsPresent($requiredParams, $givenParams, $response);
		
		if($httpCode == 200)
		{
			$statementString = "INSERT INTO country_grade_topic_relation (cid,gid,tid) VALUES (:cid,:gid,:tid)";
			$variables = ['cid' => $givenParams['cid'],
						'gid' => $givenParams['gid'],
						'tid' => $givenParams['tid']
			];
			
			try
			{
				$stmt = executeDbCall($pdo, $statementString, $variables);
			}
			catch(PDOException $e)
			{
				$stmt = false;
				$response['Response'] = "Country-Grade-Topic: insert failed";
				//$response['Response'] = $e->getMessage();
			}
		}
		else
		{
			$response['Response'] = "Country-Grade-Topic: insert failed";
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
		$requiredParams = ["oldCid", "newCid", "oldGid", "newGid", "oldTid", "newTid"];
		
		$httpCode = checkAllParamsPresent($requiredParams, $givenParams, $response);
		
		if($httpCode == 200)
		{
			
			$statementString = "UPDATE country_grade_topic_relation SET cid = :newCid, gid = :newGid, tid = :newTid "
								. "WHERE cid = :oldCid AND gid = :oldGid AND tid = :oldTid";
			$variables = ['newCid' => $givenParams["newCid"],
						'oldCid' => $givenParams["oldCid"],
						'newGid' => $givenParams["newGid"],
						'oldGid' => $givenParams["oldGid"],
						'newTid' => $givenParams["newTid"],
						'oldTid' => $givenParams["oldTid"]
			];
			
			try
			{
				$stmt = executeDbCall($pdo, $statementString, $variables);
			}
			catch(PDOException $e)
			{
				$stmt = false;
				$response['Response'] = "Country-Grade-Topic: update failed";
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
		$requiredParams = ["cid", "gid", "tid"];
		
		$httpCode = checkAllParamsPresent($requiredParams, $givenParams, $response);
		
		if($httpCode == 200)
		{
			if(removeTopic($pdo, $response, $givenParams))
			{
				//If removeal of all lessons from the topic, then delete the actual record
				$statementString = "DELETE FROM country_grade_topic_relation WHERE cid = :cid AND gid = :gid AND tid = :tid";
				$variables = ['cid' => $givenParams['cid'],
							'gid' => $givenParams['gid'],
							'tid' => $givenParams['tid']
				];
				
				try
				{
					$stmt = executeDbCall($pdo, $statementString, $variables);
				}
				catch(PDOException $e)
				{
					$stmt = false;
					$response['Response'] = "Country-Grade-Topic: delete failed";
				}
			}
			else
			{
				$response['Response'] = "Country-Grade-Topic: delete failed, removal of all lessons failed, contact help";
			}
		}
		else
		{
			$response['Response'] = "Country-Grade-Topic: delete failed";
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