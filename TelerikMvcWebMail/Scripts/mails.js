$(document).ready(function () {
    var treeview = $("#navigationTreeView").data("kendoTreeView");
    if (!Cookies.get('selected')) {
        Cookies.remove('selectedNodeText');
    }

    if (Cookies.get('selectedNodeText')) {
        filterGrid(Cookies.get('selectedNodeText'));
    }
    else if (treeview.select().length == 0) {
        filterGrid("Inbox");
    }

    $('.new-Mail').on('click', function (e) {
        $(".main-section").load(location.protocol + '//' + location.host + '/Home/NewMail');
    });

    $(".search-textbox").on('keyup', function (e) {
        var text = $(e.target).val();
        var mailGrid = $("#mainWidget").data("kendoGrid");

        if (text === null || text == '') {
            mailGrid.dataSource.filter({});
        } else {
            mailGrid.dataSource.filter({ field: "Subject", operator: "contains", value: text });
        }
    });

    $('.master-checkbox').on('change', function (e) {
        var checked = e.target.checked;
        var tasksGrid = $("#mainWidget").data("kendoGrid");

        if (checked) {
            tasksGrid.select('tr');
        } else {
            tasksGrid.clearSelection();
        }
    });
});

function mailMenuSelect(e) {
    switch (e.item.getAttribute("operation")) {
        case "replyForward":
            mailReplyForward(e.item.id);
            break;
        case "moveDelete":
            mailMoveDelete(e.item.id);
            break;
        case "readUnread":
            mailMarkAsReadUnread(e.item.id);
            break;
        case "print":
            mailPrint();
            break;
        case "changeView":
            e.item.id == "verticalPanes" ? changeToVerticalPanes(e) : changeToHorizontalPanes(e);
            break;
    }
}

function mailMoveDelete(id) {
    var grid = $("#mainWidget").data("kendoGrid");

    for (var i = 0; i < grid.select().length; i++) {
        var selectedItem = grid.dataItem(grid.select()[i]);
        selectedItem.Folder = id;
        selectedItem.dirty = true;
    }

    grid.dataSource.sync();

    var treeview = $("#navigationTreeView").data("kendoTreeView");
    var dataItem = treeview.dataItem(treeview.select());

    if (dataItem) {
        grid.dataSource.filter({ field: "Folder", operator: "contains", value: dataItem.value });
    }
}

function mailReplyForward(id) {
    var grid = $("#mainWidget").data("kendoGrid");
    var selected = grid.dataItem(grid.select());

    if (!selected) {
        $(".main-section").load(location.protocol + '//' + location.host + '/Home/NewMail?id=' + id);
    }
    else {
        var subject = selected.Subject.replace(/ /g, '%20');
        var mailTo = selected.Email;

        $(".main-section").load(location.protocol + '//' + location.host + '/Home/NewMail?id=' + id + '&mailTo=' + mailTo + '&subject=' + subject);
    }
}

function mailMarkAsReadUnread(id) {
    var grid = $("#mainWidget").data("kendoGrid");
    var selectedRows = grid.select();

    for (var i = 0; i < selectedRows.length; i++) {
        var item = grid.dataItem(selectedRows[i]);
        id == "read" ? item.IsRead = true : item.IsRead = false;
        item.dirty = true;
        id == "read" ? $(selectedRows[i]).removeClass("unread") : $(selectedRows[i]).addClass("unread");;
    }

    if (id == "unread") {
        Cookies.set('markedAsUnread', 'marked');
    }

    grid.dataSource.sync();
}

function mailPrint() {
    var grid = $("#mainWidget").data("kendoGrid");
    if (grid.select().length != 0) {
        kendo.drawing.drawDOM($(".mail-details"))
        .then(function (group) {
            return kendo.drawing.exportPDF(group, {
                paperSize: "auto",
                margin: { left: "1cm", top: "1cm", right: "1cm", bottom: "1cm" }
            });
        })
        .done(function (data) {
            kendo.saveAs({
                dataURI: data,
                fileName: "Mail.pdf"
            });
        });
    }
}

function changeToVerticalPanes(e) {
    var grid = $("#mainWidget").data("kendoGrid");
    grid.hideColumn(1);
    grid.hideColumn(2);
    grid.showColumn(3);
    updateSelectedClasses(e.item);
    $('.main-section').addClass("vertical");
    $('.main-section').removeClass("horizontal");
    grid.refresh();
}

function changeToHorizontalPanes(e) {
    var grid = $("#mainWidget").data("kendoGrid");
    grid.hideColumn(3);
    grid.showColumn(1);
    grid.showColumn(2);
    updateSelectedClasses(e.item);
    $('.main-section').addClass("horizontal");
    $('.main-section').removeClass("vertical");
    grid.refresh();
}

function updateSelectedClasses(btnElement) {
    var selectedClass = "selected";

    $(".k-item." + selectedClass).removeClass(selectedClass);
    $(btnElement).addClass(selectedClass);
}

function selectFolder(e) {
    var dataItem = this.dataItem(e.node);
    var selectedText = e.sender.dataItem(e.node).value;
    Cookies.set('selected', kendo.stringify(dataItem.index));
    Cookies.set('selectedNodeText', selectedText);

    selected = Cookies.get('selected');
    selectedNodeText = Cookies.get('selectedNodeText');

    filterGrid(selectedText);
}

function filterGrid(selectedText) {
    var mailsGrid = $("#mainWidget").data("kendoGrid");
    if (!mailsGrid) {
        window.location.href = location.protocol + '//' + location.host + '/Home/Index';
    } else {
        mailsGrid.dataSource.filter({ field: "Folder", operator: "contains", value: selectedText });
    }
}

function getinitialNumberOfItems(gridData) {
    var numbers = { Inbox: 0, Junk: 0, Drafts: 0, Deleted: 0, NativeScript: 0, KendoUI: 0, Sitefinity: 0 };
    for (var i = 0; i < gridData.length; i++) {
        var currentItemFolder = gridData[i].Folder;
        numbers[currentItemFolder] += 1;
    }

    return numbers;
}

function mailGridDataBound(e) {
    var grid = e.sender;

    for (var i = 0; i < grid.tbody.find(">tr").length; i++) {
        var item = grid.dataItem(grid.tbody.find(">tr")[i]);
        if (item.IsRead == false) {
            $(grid.tbody.find(">tr")[i]).addClass("unread")
        }
    }

    bindCheckboxes();
    polulateSelectedRows(grid);

    $.ajax({
        url: location.protocol + '//' + location.host + '/Home/Read',
        success: function (gridData) {
            var numbers = getinitialNumberOfItems(gridData.Data);
            var data = [{
                text: "Inbox " + numbers.Inbox,
                value: "Inbox"
            }, {
                text: "Junk " + numbers.Junk,
                value: "Junk"
            }, {
                text: "Drafts " + numbers.Drafts,
                value: "Drafts"
            }, {
                text: "Deleted " + numbers.Deleted,
                value: "Deleted"
            }, {
                text: "NativeScript " + numbers.NativeScript,
                value: "NativeScript"
            }, {
                text: "KendoUI " + numbers.KendoUI,
                value: "KendoUI"
            }, {
                text: "Sitefinity " + numbers.Sitefinity,
                value: "Sitefinity"
            }];

            populateNavigationTree(data);
        }
    });
    if (grid.select().length == 0) {
        $(".noData-message").show();
        $(".mail-details").hide();
    }
    else {
        $(".noData-message").hide();
        $(".mail-details").show();
    }
}

function bindCheckboxes() {
    $('.chkbx').on('change', function (e) {
        var target = $(e.target);
        var checked = e.target.checked;
        var tasksGrid = $("#mainWidget").data("kendoGrid");
        var selectedRows = tasksGrid.select();
        var checkedRow = $(target).parents('tr');

        if (checked) {
            checkedRow.addClass('k-state-selected');
            selectedRows.add(checkedRow);
            tasksGrid.select(selectedRows);
        } else {
            $('.master-checkbox').prop('checked', false);

            var resultSelection = $.map(selectedRows, function (row) {
                if ($(row).attr('data-uid') !== checkedRow.attr('data-uid')) {
                    return row;
                }
            });

            checkedRow.removeClass('k-state-selected');
            tasksGrid.select(resultSelection);
        }
    });
}

function mailSelectionChanged(e) {
    var selectedRows = this.select();

    selectionChanged(e.sender, 'mailsSelectedRow');
    checkSelectedCheckbox(selectedRows);

    if (selectedRows.length === 1) {
        var dataItem = this.dataItem(selectedRows[0]);
        populateDetailsView(dataItem);
    }

    $(".noData-message").hide();
    $(".mail-details").show();
}

function checkSelectedCheckbox(selectedRows) {
    var mailsGrid = $('#mainWidget').data('kendoGrid');
    var mailsInView = mailsGrid.dataSource.view().length;

    $('input.chkbx').prop('checked', false);

    if (mailsInView > selectedRows.length) {
        $('input.master-checkbox').prop('checked', false);
    } else {
        $('input.master-checkbox').prop('checked', true);
    }

    var checkboxes = selectedRows.find('.chkbx');
    checkboxes.prop('checked', true);
}

function populateDetailsView(item) {
    $('.mail-subject').text(item.Subject);
    $('.mail-sender').text(item.To);
    $('.mail-date').text(item.Date);
    $('.mail-text').html(item.Text);
}