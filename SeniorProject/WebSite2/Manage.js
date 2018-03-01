﻿$(document).ready($init);

function $init($evt) {
    //setup buttons and events
    $("#btnTestAjax").click($testAjaxButton_click);
}

function $testAjaxButton_click($evt) {

    $.ajax({
        url: "http://daricsage.com/movies/services/get.php",
        cache: false,
        type: "GET",
        success: $gotQuery,
        error: $error,
        complete: $complete,
        dataType: "json"
    });

}

function $gotQuery($data) {
    //just as a test
    //alert($data[0].name);
    console.log($data);
}
function $error($data) {
    //just as a test
    //alert($data[0].name);
    console.log($data);
}
function $complete($data) {
    //just as a test
    //alert($data[0].name);
    console.log($data);
}