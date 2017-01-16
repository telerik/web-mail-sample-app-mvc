$(document).ready(function () {
    $("#btn-about").on("click", function () {
        var window = $("#aboutWindow").data("kendoWindow");
        window.open();
        window.center();
    });
});