<?php

	/*
	Credit to: https://stackoverflow.com/questions/18217964/upload-video-files-via-php-and-save-them-in-appropriate-folder-and-have-a-databa
	Used for knowledge and implementation of uploading files in PHP
	*/

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
			
			$statementString = "SELECT cid, gid, tid, lid, text, path FROM lessons";
			
			try
			{
				$stmt = executeDbQuery_noVars($pdo, $statementString);
			}
			catch(PDOException $e)
			{
				$stmt = false;
				$response['Response'] = "Lessons: get all failed";
			}
		}
		else
		{
			//Paramter array used to check that all are present
			$requiredParams = ["cid","gid","tid"];
			
			$httpCode = checkAllParamsPresent($requiredParams, $givenParams, $response);
				
			if($httpCode == 200)
			{
				$statementString = "SELECT cid, gid, tid, lid, text, path FROM lessons WHERE cid = :cid AND gid = :gid AND tid = :tid";
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
					$response['Response'] = "Lessons: get specific failed";
				}
			}
			else
			{
				$response['Response'] = "Lessons: no valid parameter";
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
		$requiredParams = ["cid","gid","tid","lid","text"];
		
		$httpCode = checkAllParamsPresent($requiredParams, $givenParams, $response);
		
		if(!isset($_FILES["file"]["name"]))
		{
			$response['Missing']['file'] = "not present";
			$httpCode = 202;
		}
		
		if($httpCode == 200)
		{
			
			//TODO: scrub for malicious naming?
			$path = "../Audio/" . $_FILES["file"]["name"];
			
			$statementString = "INSERT INTO lessons (cid,gid,tid,lid,text,path) VALUES (:cid,:gid,:tid,:lid,:text,:path)";
			$variables = ['cid' => $givenParams['cid'],
						'gid' => $givenParams['gid'],
						'tid' => $givenParams['tid'],
						'lid' => $givenParams['lid'],
						'text' => $givenParams['text'],
						'path' => $path
			];
			
			
			$canAddToDB = false;
			
			try
			{
				
				if (!file_exists("../Audio/" . $_FILES["file"]["name"]))
				{
					if(move_uploaded_file($_FILES["file"]["tmp_name"], "../Audio/" . $_FILES["file"]["name"]))
					{
						$canAddToDB = true;
					}
					else
					{
						$canAddToDB = false;
					}
				}
				else
				{
					//TODO: inform there was already a file there????
					$canAddToDB = true;
				}
				
				if($canAddToDB)
				{
					$stmt = executeDbCall($pdo, $statementString, $variables);
				}
				else
				{
					$response['Response'] = "Lessons: insert failed, could not upload file";
				}
				
			}
			catch(PDOException $e)
			{
				$stmt = false;
				$response['Response'] = "Lessons: insert failed";
				//$response['Response'] = $e->getMessage();
			}
		}
		else
		{
			$response['Response'] = "Lessons: insert failed";
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
			
			//if statement failed but an audio was uploaded, then remove the file
			if(canAddToDB)
			{
				if(!removeFile("../Audio/" . $_FILES["file"]["name"]))
				{
					$response['Response'] .= " | Critical: could not remove file or file did not exist, contact help.";
				}
			}
			
			$response['Data'] = [];
		}
		
	}
	else if($verb == 'PUT')																	//PUT
	{
		//Paramter array used to check that all are present
		$requiredParams = ["oldCid", "newCid", "oldGid", "newGid", "oldTid", "newTid", "oldLid", "newLid"];
		
		$httpCode = checkAllParamsPresent($requiredParams, $givenParams, $response);
		
		if($httpCode == 200)
		{
			
			$statementString = "UPDATE lessons SET cid = :newCid, gid = :newGid, tid = :newTid, lid = :newLid "
								. "WHERE cid = :oldCid AND gid = :oldGid AND tid = :oldTid AND lid =:oldLid";
			$variables = ['newCid' => $givenParams["newCid"],
						'oldCid' => $givenParams["oldCid"],
						'newGid' => $givenParams["newGid"],
						'oldGid' => $givenParams["oldGid"],
						'newTid' => $givenParams["newTid"],
						'oldTid' => $givenParams["oldTid"],
						'newLid' => $givenParams["newLid"],
						'oldLid' => $givenParams["oldLid"]
			];
			
			try
			{
				$stmt = executeDbCall($pdo, $statementString, $variables);
			}
			catch(PDOException $e)
			{
				$stmt = false;
				$response['Response'] = "Lessons: update failed";
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
		$requiredParams = ["cid", "gid", "tid", "lid"];
		
		$httpCode = checkAllParamsPresent($requiredParams, $givenParams, $response);
		
		if($httpCode == 200)
		{
			if(removePaths($pdo, $response, $givenParams))
			{
				//If the removal of the path/file is successful then we can delete the record
				$statementString = "DELETE FROM lessons WHERE cid = :cid AND gid = :gid AND tid = :tid AND lid = :lid";
				$variables = ['cid' => $givenParams['cid'],
					'gid' => $givenParams['gid'],
					'tid' => $givenParams['tid'],
					'lid' => $givenParams['lid']
				];
				
				try
				{
					$stmt = executeDbCall($pdo, $statementString, $variables);
				}
				catch(PDOException $e)
				{
					$stmt = false;
					$response['Response'] = "Lessons: delete failed";
				}
			}
			else
			{
				$response['Response'] = "Lessons: delete failed, removal of paths failed | contact help";
			}
		}
		else
		{
			$response['Response'] = "Lessons: delete failed";
		}
		
		closeDB($pdo);
		
		if($stmt == true)
		{
			//$response['Error'] = "false";
			$response['Response'] .= " | OK";
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