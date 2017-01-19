function newMailMenuSelect(e) {
    var dialog = $("#newMailDialog").data("kendoDialog");
    var itemText = $(e.item).children(".k-link").text();

    if (itemText == "Send") {
        dialog.content("This is a Demo Application and your mail will not be send successfully. To develop further this functionality, please visit a help link SmptClient Class.");
        dialog.open();
    }
    else if (itemText == "Save") {
        dialog.content("This is a Demo Application and mail saving functionality is not available.");
        dialog.open();
    }
    else if (itemText == "Cancel") {
        var currentLocation = window.location.href;

        if (currentLocation.indexOf("Contacts") > -1) {
            window.location.href = "/Contacts/Index/";
        } else {
            window.location.href = "/Home/Index/";
        }
    }
}