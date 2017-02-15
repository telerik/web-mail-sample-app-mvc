function addClick() {
    var selectedCategory = $('#newTaskCategory').data('kendoDropDownList').value();
    var subject = $('.subject-textbox').val();
    var content = $('#newTask').data('kendoEditor').textarea.val();

    $.ajax({
        method: "POST",
        url: baseUrl + "/Tasks/Tasks_Create",
        data: {
            task: {
                Category: selectedCategory,
                Subject: subject,
                Content: content
            }
        }
    }).done(function (msg) {
        window.location.href = baseUrl + 'Tasks';
    });
}

function cancelClick() {
    window.location.href = baseUrl + 'Tasks';
}
