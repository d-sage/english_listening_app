$(document).ready($init);

function $init($evt) {

    //setup buttons and events
    $("#btnTestAjax").click($testAjaxButton_click);

}

function $testAjaxButton_click($evt) {

    var value = $("#ValueHiddenField").val();

    //cache = false;???
    ////If there is a '_' variable, used in ajax to avoid caching, then unset it
    ////unset($input['_']);

    $.ajax({
        url: "http://localhost/seniorProj_testing/backend/indexes/countrygradeIndex.php",
        type: "GET",
        headers: {
            'Authorization': 'Basic ' + btoa(value.toString() + ":none")
        },
        success: $success,
        error: $error,
        complete: $complete,
        dataType: "json"
    });
    
}

function $success($data) {

    //just as a test
    console.log($data);

    /*
    $.ajax({
        type: "POST",
        url: 'Manage.aspx/AjaxSendTest',
        data: JSON.stringify({data: $data }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        error: $error,
        complete: $complete,
    });
    */


}
function $error($data) {

    //just as a test
    console.log($data);

}
function $complete($data) {

    //just as a test
    console.log($data);

}



