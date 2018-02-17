$(document).ready($init);


function $init($evt)
{
	
	$('#inputButton1').click($upload);
	$('#inputButton2').click($update);
	
}

function $upload($evt)
{
	
	var name= document.getElementById('inputFile1');
                 var alpha=name.files[0];
                console.log(alpha.name);
                 var data= new FormData();
                 data.append('file',alpha);
				 data.append('cid', 'hon');
				 data.append('gid', '1');
				 data.append('tid', 'job');
				 data.append('lid', 'j1.3');
				 data.append('text', 'This is a test');
                 $.ajax({
                 url:'./php/lessons.php',
                 data:data,
                 processData:false,
                 contentType:false,
                 type:'POST',
                 success:function($msg){
					alert("s: " + $msg);
                 },
				 complete:function($msg){
					alert("c: "+ $msg);
                 }
                 });
	
}

function $update($evt)
{
	
	
                 var data= {};
				 data['oldCid'] = 'hon';
				 data['newCid'] = 'nic';
				 data['newGid'] = '2';
				 data['oldGid'] = '1';
				 data['newTid'] = 'school';
				 data['oldTid'] = 'job';
				 data['newLid'] = 's1.4';
				 data['oldLid'] = 'j1.3';
				 
                 $.ajax({
                 url:'./php/lessons.php',
				 cache: false,
                 data:data,
                 type:'PUT',
                 success:function($msg){
					alert("s: " + $msg);
                 },
				 complete:function($msg){
					alert("c: "+ $msg);
                 }
                 });
	
}




























