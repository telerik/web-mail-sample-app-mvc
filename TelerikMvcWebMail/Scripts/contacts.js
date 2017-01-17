$(document).ready(function () {
    $('.new-Contact').on('click', function (e) {
        var listView = $("#mainWidget").data("kendoListView");
        listView.add();
    });
});

function exportPeople() {
    kendo.drawing.drawDOM($(".inner-section > .list-view-inner"))
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
                text: "All " + numbers.All,
                value: "All"
            }, {
                text: "Favorites " + numbers.Favorites,
                value: "Favorites"
            }, {
                text: "Friends " + numbers.Friends,
                value: "Friends"
            }, {
                text: "Work " + numbers.Work,
                value: "Work"
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
    var text = $(e.target).val();

    var listView = $("#mainWidget").data("kendoListView");
    listView.dataSource.filter({ field: "Name", operator: "contains", value: text });
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
    $(".main-section").load(location.protocol + '//' + location.host + '/Home/NewMail?mailTo=' + email + '&fromView=Contacts');
}

function singleEditClick(e) {
    var uid = getItemUid(e);
    var listView = $("#mainWidget").data("kendoListView");
    listView.edit(listView.element.find('[data-uid="' + uid + '"]'));
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

    $('.k-single-email-button').on('click', singleCreateNewMail);
    $('.k-single-edit-button').on('click', singleEditClick);
    $('.k-single-delete-button').on('click', singleDeleteClick);
}

function onListViewEdit(e) {
    //debugger;
    if (!e.model.EmployeeID) {
        e.model.EmployeeID = getId();
        $('#EmployeeID').val(e.model.EmployeeID);
    }
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
            var imageWrapper = $('<span class="image-wrapper"></span>').html(image);

            $('.new-contact-upload-wrapper').html(imageWrapper);

            var employeeId = $('#EmployeeID').val();
            var imgData = getBase64Image(image[0]);
            sessionStorage.setItem(employeeId, imgData);
        };

        reader.readAsDataURL(raw);
    }
}

function getBase64Image(img) {
    var canvas = document.createElement("canvas");
    canvas.width = img.width;
    canvas.height = img.height;

    var ctx = canvas.getContext("2d");
    ctx.drawImage(img, 0, 0);

    var dataURL = canvas.toDataURL("image/png");

    return dataURL.replace(/^data:image\/(png|jpg)\;base64,/, "");
}