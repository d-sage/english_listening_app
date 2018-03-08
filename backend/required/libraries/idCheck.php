<?php

	if (!isset($_SERVER['PHP_AUTH_USER']))
	{
		die("Unauthorized");
	}
	
	//The id was set so grab the id in to '$requestID'
	$requestID = $_SERVER['PHP_AUTH_USER'];
	
	//query for the ID info from the database table
	$statementString = "SELECT id, time FROM session";
	try
	{
		$stmt = executeDbQuery_noVars($pdo, $statementString);
	}
	catch(PDOException $e)
	{
		die("Contact Help");
	}
	
	//grab each piece of database
	$data = $stmt->fetchAll(PDO::FETCH_ASSOC);
	
	if(sizeof($data) != 1)
	{
		die("Unauthorized");
	}
	
	$storedId = (string)($data[0]['id']);
	$storedTime = $data[0]['time'];
	
	
	//if the 'created' time for the randomID has been within 15 minutes then continue
	//otherwise 'die'
	if($requestID !== $storedId)
	{
		die("Unauthorized");
	}
	
	$timeDifference = time() - $storedTime;
	$fifteenMins = 900;
	
	if($timeDifference > $fifteenMins)
	{
		die("Session Timeout");
		//header to redirect to Login page???
		//delete record??
	}

?>