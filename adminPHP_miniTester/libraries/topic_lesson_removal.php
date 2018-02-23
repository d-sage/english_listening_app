<?php


	/*
	This checks that the $filepath is not null, then checks to see
	if the file is present, then removes / unlinks it
	
	returns true if successful removal
	returns false if filepath is null, not in Audio directory, or removal failed
	returns 0 if the file did not exist
	*/
	function removeFile($filepath)
	{
		if(is_null($filepath))
		{
			return false;
		}
		
		if(strpos($filepath, '../Audio') === false)
		{
			return false;
		}
		
		if(file_exists($filepath))
		{
			return unlink($filepath);
		}
		return 0;
	}
	
	
	/*
	This function determines if any lessons are sharing the same path as the lesson
	that is being deleted. So it checks to see if any other lessons have the path, if
	so then we do not remove the mp3 file. If there is no other lessons associated with
	the same file, then we remove the file.
	*/
	function removePaths($pdo, &$response, $givenParams)
	{
		
		$stmt = true;
		
		/*
		The following section receives the count of the number of lessons
		that share the same path that is wanting to be deleted, that way we can
		determine if the file should be removed or not
		*/
		$statementString = "SELECT COUNT(*) as count, path
							FROM lessons
							WHERE path = (SELECT path
										FROM lessons
										WHERE cid = :cid AND gid = :gid AND tid = :tid AND lid = :lid);";
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
			$response['Response'] .= "Lessons: delete failed | Could not get path count, contact help.";
			return $stmt;
		}
		
		$data = $stmt->fetchAll(PDO::FETCH_ASSOC);
		$countOfPaths = $data[0]['count'];
		$path = $data[0]['path'];
		
		
		/*
		There is only one path count(the one we want to remove), so remove the file
		*/
		if($countOfPaths == 1)
		{
			$result = true;
			if(($result = removeFile($path)) === false)
			{
				$response['Response'] .= "failed to remove file, contact help";
				return false;
			}
			else if($result === 0)
			{
				$response['Response'] = "File did not exist";
			}
		}
		
		return true;		
	}
	
	
	//TODO
	function removeTopic($pdo, &$response, $givenParams)
	{
		
		$stmt = true;
		
		/*
		The following section receives the lids associated with the given country,grade,topic.
		This will allow us to loop through each and pass to 'removePaths'.
		*/
		$statementString = "SELECT lid
							FROM lessons
							WHERE cid = :cid AND gid = :gid AND tid = :tid;";
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
			$response['Response'] .= "Topics: delete failed | Could not get lids assoc. with this country-grade-topic, contact help.";
			return $stmt;
		}

		//Get the lids from the previous query
		$lids = $stmt->fetchAll(PDO::FETCH_ASSOC);
		
		$newParams = $givenParams;
		
		//loop through and fill the associated array 'lid' in with the new lid each time
		//and call the 'removePaths' function
		foreach($lids as $nextLid)
		{
			$newParams['lid'] = $nextLid['lid'];
			
			if(!removePaths($pdo, $response, $newParams))
			{
				return false;
			}
			
		}
		
		return true;
	}
	


?>