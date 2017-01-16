function onClick(e) {
    location.href = location.protocol + '//' + location.host + '/SignIn/Index';
    return false;
}

function closeToolTip(e) {
    var tooltip = $("#mainMenu > li:last").data("kendoTooltip");
    tooltip.hide();
}

function tooltipShow(e) {
    document.body.addEventListener("mouseup", closeToolTip);
}

function tooltipHide(e) {
    document.body.removeEventListener("mouseup", closeToolTip);
}