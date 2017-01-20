$(document).ready(function () {
    $('.new-Contact').on('click', function (e) {
        var listView = $("#mainWidget").data("kendoListView");
        listView.add();
    });
});

function exportPeople() {
    kendo.drawing.drawDOM($(".inner-section .k-listview"))
        .then(function (group) {
            return kendo.drawing.exportPDF(group, {
                paperSize: "auto",
                margin: { left: "1cm", top: "1cm", right: "1cm", bottom: "1cm" }
            });
        })
        .done(function (data) {
            kendo.saveAs({
                dataURI: data,
                fileName: "People.pdf"
            });
        });
}

function changeToListDetails(e) {
    var contactsElement = $('.inner-section');
    contactsElement.removeClass("card-view");
    contactsElement.addClass("list-view");

    updateSelectedViewButton(e);
}

function changeToContactsCards(e) {
    var contactsElement = $('.inner-section');
    contactsElement.removeClass("list-view");
    contactsElement.addClass("card-view");

    updateSelectedViewButton(e);
}

function updateSelectedViewButton(element) {
    var selectedClass = "selected";

    $(".toolbar ." + selectedClass).removeClass(selectedClass);
    $(element).addClass(selectedClass);
}

function getInitialNumberOfItems(listViewData) {
    var numbers = { Favorites: 0, Friends: 0, Work: 0 };
    for (var i = 0; i < listViewData.length; i++) {
        var currentItemCategory = listViewData[i].Folder;
        numbers[currentItemCategory] += 1;
    }

    numbers.All = numbers.Favorites + numbers.Friends + numbers.Work;

    return numbers;
}

function onListViewDataBound(e) {
    var listView = e.sender;
    var selectedItem = listView.select();

    if (selectedItem.length === 0) {
        listView.select(listView.element.children().first());
    }

    $.ajax({
        url: location.protocol + '//' + location.host + '/Contacts/Contacts_Read',
        success: function (listViewData) {
            var numbers = getInitialNumberOfItems(listViewData.Data);
            var data = [{
                text: "All ",
                value: "All",
                number: numbers.All
            }, {
                text: "Favorites ",
                value: "Favorites",
                number: numbers.Favorites
            }, {
                text: "Friends ",
                value: "Friends",
                number: numbers.Friends
            }, {
                text: "Work ",
                value: "Work",
                number: numbers.Work
            }];

            populateNavigationTree(data);
        }
    });

    $('.k-email-button').on('click', createNewMail);
    $('.search-textbox').on('keyup', searchContacts);
}

function selectFolder(e) {
    var dataItem = this.dataItem(e.node);
    var selectedText = e.sender.dataItem(e.node).value;
    Cookies.set('selected', kendo.stringify(dataItem.index));
    selected = Cookies.get('selected');

    $(".search-textbox").val('');

    var contactsListView = $("#mainWidget").data("kendoListView");

    if (!contactsListView) {
        window.location.href = location.protocol + '//' + location.host + '/Contacts/Index';
    } else if (selectedText === "All") {
        contactsListView.dataSource.filter({});
    } else {
        contactsListView.dataSource.filter({ field: "Folder", operator: "contains", value: selectedText });
    }
}

function searchContacts(e) {
    var text = $(e.target).val().toLowerCase();
    var listView = $("#mainWidget").data("kendoListView");

    var dataInView = listView.dataSource.view();
    dataInView.forEach(function (item) {
        var row = $('div[data-uid="' + item.uid + '"]');

        if (item.Name.toLowerCase().indexOf(text) > -1) {
            row.show();
        } else {
            row.hide();
        }
    });
}

function createNewMail(e) {
    var target = $(e.target);
    var listVewItem = target.parents('.contact-list, .contact-card');
    var listView = $('#mainWidget').data('kendoListView');
    var dataItem = listView.dataItem(listVewItem);
    var email = dataItem.Email;

    openNewMailView(email);
}

function singleCreateNewMail(e) {
    var target = $(e.target);
    var singleItem = target.parents('.contact-view');
    var email = singleItem.find('.hidden-email').text();

    openNewMailView(email);
}

function openNewMailView(email) {
    $(".main-section").load(location.protocol + '//' + location.host + '/Home/NewMail?mailTo=' + email);
}

function singleEditClick(e) {
    var uid = getItemUid(e);
    var listView = $("#mainWidget").data("kendoListView");
    listView.edit(listView.element.find('[data-uid="' + uid + '"]'));
    $(".form-title .action").html("Edit");
}

function singleDeleteClick(e) {
    var uid = getItemUid(e);
    var listView = $("#mainWidget").data("kendoListView");
    listView.remove(listView.element.find('[data-uid="' + uid + '"]'));
}

function getItemUid(e) {
    var target = $(e.target);
    var singleItem = target.parents('.contact-view');
    return singleItem.attr('uid');
}

function onListViewSelectionChange(e) {
    var selecteditem = e.sender.select();
    var dataItem = e.sender.dataItem(selecteditem);

    if (!dataItem) {
        return;
    }

    var template = kendo.template($('#single-contact-template').html());
    var result = template(dataItem);
    $(".single-contact-details").html(result);

    attachButtonHandlers();
}

function attachButtonHandlers() {
    $('.k-single-email-button').on('click', singleCreateNewMail);
    $('.k-single-edit-button').on('click', singleEditClick);
    $('.k-single-delete-button').on('click', singleDeleteClick);
}

function onListViewEdit(e) {
    if (!e.model.EmployeeID) {
        e.model.EmployeeID = getId();
        $('#EmployeeID').val(e.model.EmployeeID);
    }
}

function onListViewCancel(e) {
    setTimeout(function (e) {
        attachButtonHandlers();
    }, 0);
}

function getId() {
    var text = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    for (var i = 0; i < 5; i++) {
        text += possible.charAt(Math.floor(Math.random() * possible.length));
    }

    return text;
}

function onImageSelect(e) {
    var fileInfo = e.files[0];

    if (fileInfo.validationErrors && fileInfo.validationErrors.length > 0) {
        return;
    }

    setTimeout(function () {
        addPreview(fileInfo);
    });
}

function addPreview(file) {
    var raw = file.rawFile;
    var reader = new FileReader();

    if (raw) {
        reader.onloadend = function () {
            var image = $('<img class="image-preview">').attr('src', this.result);

            $('.upload-image-wrapper').html(image);

            var employeeId = $('#EmployeeID').val();

            setTimeout(function () {
                var imgData = getBase64Image(image[0]);
                sessionStorage.setItem(employeeId, imgData);
            }, 100);            
        };

        reader.readAsDataURL(raw);
    }
}

function getBase64Image(img) {
    var canvas = document.createElement("canvas");

    var ctx = canvas.getContext("2d");
    ctx.drawImage(img, 0, 0);

    var dataURL = canvas.toDataURL("image/png");
    console.log(dataURL);

    return dataURL.replace(/^data:image\/(png|jpg)\;base64,/, "");
}