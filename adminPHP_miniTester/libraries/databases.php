<?php

	$dsn = "mysql:host=$host;dbname=$db;charset=$charset";
		
	$opt = [
		PDO::ATTR_ERRMODE            => PDO::ERRMODE_EXCEPTION,
		PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,
		PDO::ATTR_EMULATE_PREPARES   => false
	];
		
	$pdo = new PDO($dsn, $user, $pass, $opt);
		
	function executeDbCall($pdo, $statementString, $variables)
	{
		$stmt = $pdo->prepare($statementString);
		
		try
		{
			$stmt->execute($variables);
		}
		catch(PDOException $e)
		{
			throw $e;
		}
		
		return $stmt;
	}

	function executeDbQuery_noVars($pdo, $statementString)
	{
		try
		{
			$stmt = $pdo->query($statementString);
		}
		catch(PDOException $e)
		{
			throw $e;
		}
		
		return $stmt;
	}

	function closeDB($pdo)
	{
		$pdo = null;
	}
		

//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	
	/*
	~Takes a partially constructed statementString and a variableArray and a parameters array and an input array
	~It iterates through the paramters and sees if it is in the input assoc array. It will add to the statement string
	and variable array.
	~The varaible array is passed by reference so that is modified and the new statementString is returned.
	*/
	function genLikeQueryVarsAndStatement($statementString, &$variablesArray, $parameters, $inputData)
	{
		//Determine which parameters are used for the Query
		foreach ($parameters as $param)
		{
			if(isset($inputData[$param]))
			{
				if(sizeof($variablesArray) > 0)
				{
					$statementString .= " AND ";
				}
				$statementString .= " " . $param . " LIKE ?";
				array_push($variablesArray, "%" . $inputData[$param] . "%");
			}
		}
		
		return $statementString;
	}
	
	/*
	~Takes a parameters array and an input array and a response assoc array passed by reference
	~It iterates through the paramters and sees if it is in the input assoc array. If it is not present it
	adds to the response assoc array as a 'missing' param and changes httpCode to fail.
	~The response array is passed by reference so that is modified and the httpCode is returned to show success or fail.
	*/
	function checkAllParamsPresent($parameters, $input, &$response)
	{
		$httpCode = 200;
		
		//Check all parameters are present
		foreach ($parameters as $param)
		{
			if(!isset($input[$param]))
			{
				$response['Missing'][$param] = "not present";
				$httpCode = 202;
			}
			else if(empty($input[$param]))
			{
				$response['Missing'][$param] = "not present";
				$httpCode = 202;
			}
		}
		
		return $httpCode;
	}
	
	
?>