var baseUrl = $("#BaseUrl").data("baseurl");

function onClick(e) {
    location.href = baseUrl + '/SignIn/Index';
    return false;
}
