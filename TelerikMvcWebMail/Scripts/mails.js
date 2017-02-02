var navigated = false;
var marked = false;
var markedAsUnread = false;
var savedScroll = 0;

$(document).ready(function () {
    // Preselect Mails category
    var treeview = $("#navigationTreeView").data("kendoTreeView");
    if (!Cookies.get('selected')) {
        Cookies.remove('selectedNodeText');
        setMenuItemsAvailability(false, "noselection");
    }

    if (Cookies.get('selectedNodeText')) {
        filterGrid(Cookies.get('selectedNodeText'));
    }
    else if (treeview.select().length == 0) {
        filterGrid("Inbox");
    }

    $('.new-Mail').on('click', function (e) {
        $(".main-section").load(location.protocol + '//' + location.host + '/Home/NewMail');
        $(".main-section").removeClass("horizontal").removeClass("vertical");
    });

    // Attach new mails handler
    $("#mainWidget").on("mousedown", "tr[role='row']", function (e) {
        if (e.which === 3) {
            if (!$(this).hasClass("k-state-selected")) {
                $("#mainWidget tbody tr").removeClass("k-state-selected");
                var mailGrid = $("#mainWidget").data("kendoGrid");
                mailGrid.select($(this));
            }
        }
    });

    // Attach search textbox handler
    $(".search-textbox").on('keyup', function (e) {
        var text = $(e.target).val().toLowerCase();
        var mailGrid = $("#mainWidget").data("kendoGrid");

        var dataInView = mailGrid.dataSource.view();
        dataInView.forEach(function (item) {
            var row = $('tr[data-uid="' + item.uid + '"]');

            if (item.Subject.toLowerCase().indexOf(text) > -1) {
                row.show();
            } else {
                row.hide();
            }
        });
    });

    // Attach master checkbox click handler
    $('.master-checkbox').on('click', function (e) {
        var grid = $("#mainWidget").data("kendoGrid");
        var dataInView = grid.dataSource.view();
        if (dataInView.length == 0) {
            e.preventDefault();
        }
    });

    // Attach master checkbox handler
    $('.master-checkbox').on('change', function (e) {
        var checked = e.target.checked;
        var grid = $("#mainWidget").data("kendoGrid");

        if (checked) {
            grid.select('tr');
            setMenuItemsAvailability(false, "multiselection");

        } else {
            grid.clearSelection();
            setMenuItemsAvailability(true);
            setMenuItemsAvailability(false, "noselection");
        }
    });
});

// Menu select item handler and its functions
function mailMenuSelect(e) {
    switch (e.item.getAttribute("operation")) {
        case "reply":
            mailReply(e.item.id);
            break;
        case "forward":
            mailForward(e.item.id);
            break;
        case "moveDelete":
            mailMoveDelete(e.item.id);
            break;
        case "read":
            mailMarkAsReadUnread("read");
            break;
        case "unread":
            mailMarkAsReadUnread("unread");
            break;
        case "print":
            mailPrint();
            break;
    }
}

function mailReply(id) {
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

function mailForward(id) {
    var grid = $("#mainWidget").data("kendoGrid");
    var selected = grid.dataItem(grid.select());

    if (!selected) {
        $(".main-section").load(location.protocol + '//' + location.host + '/Home/NewMail?id=' + id);
    }
    else {
        var subject = selected.Subject.replace(/ /g, '%20');
        $(".main-section").load(location.protocol + '//' + location.host + '/Home/NewMail?id=' + id + '&subject=' + subject);
    }
}

function mailMoveDelete(id) {
    var grid = $("#mainWidget").data("kendoGrid");

    for (var i = 0; i < grid.select().length; i++) {
        var selectedItem = grid.dataItem(grid.select()[i]);
        selectedItem.Category = id;
        selectedItem.dirty = true;
    }

    grid.dataSource.sync();
}

function mailMarkAsReadUnread(id) {
    var grid = $("#mainWidget").data("kendoGrid");
    var selectedRows = grid.select();
    var read;

    if (id === "read") {
        read = true;
    } else {
        read = false;
    }

    for (var i = 0; i < selectedRows.length; i++) {
        var item = grid.dataItem(selectedRows[i]);
        item.IsRead = read;
        item.dirty = true;
        read === true ? $(selectedRows[i]).removeClass("unread") : $(selectedRows[i]).addClass("unread");;
    }

    if (!read) {
        markedAsUnread = true;
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

// Switch between vertical / horizontal panes view
function changeToVerticalPanes(e) {
    var grid = $("#mainWidget").data("kendoGrid");
    grid.hideColumn(1);
    grid.hideColumn(2);
    grid.showColumn(3);
    updateSelectedClasses(e);
    $('.main-section').addClass("vertical");
    $('.main-section').removeClass("horizontal");
    grid.refresh();
}

function changeToHorizontalPanes(e) {
    var grid = $("#mainWidget").data("kendoGrid");
    grid.hideColumn(3);
    grid.showColumn(1);
    grid.showColumn(2);
    updateSelectedClasses(e);
    $('.main-section').addClass("horizontal");
    $('.main-section').removeClass("vertical");
    grid.refresh();
}

function updateSelectedClasses(element) {
    var selectedClass = "selected";

    $(".toolbar ." + selectedClass).removeClass(selectedClass);
    $(element).addClass(selectedClass);
}

// Select category handler in the sidebar navigation
function selectCategory(e) {
    var dataItem = this.dataItem(e.node);
    var selectedText = e.sender.dataItem(e.node).value;

    setMenuItemsAvailability(false, "noselection");
    $(".search-textbox").val('');
    $('input.master-checkbox').prop('checked', false);

    Cookies.set('selected', kendo.stringify(dataItem.index));
    Cookies.set('selectedNodeText', selectedText);

    selected = Cookies.get('selected');
    selectedNodeText = Cookies.get('selectedNodeText');

    navigated = true;

    filterGrid(selectedText);
}

// Filter grid according to the currently selected mails category
function filterGrid(selectedText) {
    var mailsGrid = $("#mainWidget").data("kendoGrid");
    if (!mailsGrid) {
        window.location.href = location.protocol + '//' + location.host + '/Home/Index';
    } else {
        mailsGrid.dataSource.filter({ field: "Category", operator: "contains", value: selectedText });
    }
}

// Get the number of mails in each category
function getinitialNumberOfItems(gridData) {
    var numbers = { Inbox: 0, Junk: 0, Drafts: 0, Deleted: 0, NativeScript: 0, KendoUI: 0, Sitefinity: 0 };
    for (var i = 0; i < gridData.length; i++) {
        var currentItemCategory = gridData[i].Category;
        numbers[currentItemCategory] += 1;
    }

    return numbers;
}

function dataSourceChange(e) {
    var grid = $("#mainWidget").data("kendoGrid");
    var treeview = $("#navigationTreeView").data("kendoTreeView");
    if (e.action === "sync") {
        var dataItem = treeview.dataItem(treeview.select());

        var data = grid.dataSource.data();
        var dataLength = data.length;

        for (var i = 0; i < dataLength; i++) {
            var currntItem = data[i];
            if (currntItem.Category !== dataItem.value) {
                i -= 1;
                dataLength -= 1;
                grid.dataSource.pushDestroy(currntItem);
            }
        }
    }
}

function dataSourceRequestEnd(e) {
    setTimeout(function () {
        var grid = $("#mainWidget").data("kendoGrid");
        if (grid.dataSource.view().length == 0) {
            setMenuItemsAvailability(false, "noselection");
        }
    }, 100)
}

// Attach mails grid events
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
                text: "Inbox ",
                value: "Inbox",
                number: numbers.Inbox
            }, {
                text: "Junk ",
                value: "Junk",
                number: numbers.Junk
            }, {
                text: "Drafts ",
                value: "Drafts",
                number: numbers.Drafts
            }, {
                text: "Deleted ",
                value: "Deleted",
                number: numbers.Deleted
            }, {
                text: "NativeScript ",
                value: "NativeScript",
                number: numbers.NativeScript
            }, {
                text: "KendoUI ",
                value: "KendoUI",
                number: numbers.KendoUI
            }, {
                text: "Sitefinity ",
                value: "Sitefinity",
                number: numbers.Sitefinity
            }];

            populateNavigationTree(data);
        }
    });
    if (grid.select().length === 1) {
        $(".mail-details-wrapper").removeClass("empty");
    }
    else {
        $(".mail-details-wrapper").addClass("empty");
    }

    if (grid.dataSource.view().length === 0) {
        $('input.master-checkbox').prop('checked', false);
    };
}

function mailContextMenuOpen(e) {
    var mailsGrid = $('#mainWidget').data('kendoGrid');
    var mailsInView = mailsGrid.dataSource.view().length;

    if (mailsInView == 0) {
        e.preventDefault();
    }
}

function mailSelectionChanged(e) {
    var selectedRows = this.select();

    selectionChanged(e.sender, 'mailsSelectedRow');
    checkSelectedCheckbox(selectedRows);

    if (selectedRows.length === 1) {
        var dataItem = this.dataItem(selectedRows[0]);
        populateDetailsView(dataItem);
        $(".mail-details-wrapper").removeClass("empty");

        setMenuItemsAvailability(true);
    } else {
        $(".mail-details-wrapper").addClass("empty");
        setMenuItemsAvailability(false, "multiselection");
    }
}

function selectionChanged(widget, selectedRowPrefix) {
    var navigationTreeView = $('#navigationTreeView').data('kendoTreeView');
    var selectedNode = navigationTreeView.select();
    var selectedRows = widget.select();

    if (!selectedNode) {
        return;
    }

    var selectedNodeData = navigationTreeView.dataItem(selectedNode);
    var selectedNodeValue = selectedNodeData.value;

    if (selectedRows.length === 1) {
        var dataItem = widget.dataItem(selectedRows);

        if (markedAsUnread) {
            markedAsUnread = false;
        } else if (!dataItem.IsRead) {
            marked = true;
            savedScroll = widget.content.scrollTop();

            dataItem.IsRead = true;
            dataItem.dirty = true;
            selectedRows.removeClass("unread");
            widget.dataSource.sync();
        }

        Cookies.set(selectedRowPrefix + selectedNodeValue, dataItem.ID);
    } else {
        var selectedRowsIds = [];

        for (var i = 0; i < selectedRows.length; i++) {
            var selectedRow = selectedRows[i];
            var dataItem = widget.dataItem(selectedRow);

            selectedRowsIds.push(dataItem.ID);
        }

        Cookies.set(selectedRowPrefix + selectedNodeValue, selectedRowsIds.join());
    }
}

// Populated selected rows, based on previously saved selection
function polulateSelectedRows(widget) {
    var navigationTreeView = $('#navigationTreeView').data('kendoTreeView');
    var treeViewSelectedItem = navigationTreeView.select();
    var widgetDataSource = widget.dataSource;

    if (treeViewSelectedItem.length === 1) {
        var treeViewDataItem = navigationTreeView.dataItem(treeViewSelectedItem);
        var treeViewItemValue = treeViewDataItem.value;
        var selectedRowsFromCoockie = Cookies.get('mailsSelectedRow' + treeViewItemValue);

        if (selectedRowsFromCoockie) {
            var selectedRowsArray = selectedRowsFromCoockie.split(',');

            for (var i = 0; i < selectedRowsArray.length; i++) {
                currentRowId = selectedRowsArray[i];
                var dataItem = widgetDataSource.get(currentRowId);
                if (dataItem) {
                    var row = widget.tbody.find("tr[data-uid='" + dataItem.uid + "']");
                    widget.select(row);

                    if (!marked && navigated) {
                        widget.content.scrollTop(row.offset().top - widget.content.offset().top);
                    } else if (marked && navigated) {
                        marked = false;
                        widget.content.scrollTop(savedScroll);
                    }

                    navigated = false;
                }
            }
        }
    }
}

// Bind mail ceckboxes
function bindCheckboxes() {
    $('.chkbx').on('change', function (e) {
        var target = $(e.target);
        var checked = e.target.checked;
        var mailsGrid = $("#mainWidget").data("kendoGrid");
        var selectedRows = mailsGrid.select();
        var checkedRow = $(target).parents('tr');

        if (checked) {
            checkedRow.addClass('k-state-selected');
            selectedRows = mailsGrid.select();
            mailsGrid.select(selectedRows);
        } else {
            $('.master-checkbox').prop('checked', false);

            var resultSelection = $.map(selectedRows, function (row) {
                if ($(row).attr('data-uid') !== checkedRow.attr('data-uid')) {
                    return row;
                }
            });

            checkedRow.removeClass('k-state-selected');
            mailsGrid.select(resultSelection);

            if (resultSelection.length === 0) {
                $('input.master-checkbox').prop('checked', false);
                $(".mail-details-wrapper").addClass("empty");
                setMenuItemsAvailability(false, "noselection");
            }
        }
    });
}

// Configure menu items availability
function setMenuItemsAvailability(isEnabled, selection) {
    var menu = $('#mailMenu').data('kendoMenu');
    var contextMenu = $('#mailContextMenu').data('kendoContextMenu');

    if (isEnabled) {
        toggleEnableMenuItems(menu, "mailMenu", isEnabled);
        toggleEnableMenuItems(contextMenu, "mailContextMenu", isEnabled);
    } else if (!isEnabled && selection == "noselection") {
        toggleEnableMenuItems(menu, "mailMenu", isEnabled);
    } else if (!isEnabled && selection == "multiselection") {
        var itemsIds = ["RE", "RE_ALL", "FW", "print"];

        toggleEnableMenuItems(menu, "mailMenu", true);

        itemsIds.forEach(function (itemID) {
            $("#mailMenu").find(".k-item[id=" + itemID + "]").each(function (index) {
                menu.enable($(this), isEnabled);
            });
            $("#mailContextMenu").find(".k-item[id=" + itemID + "]").each(function (index) {
                contextMenu.enable($(this), isEnabled);
            });
        });
    }
}

function toggleEnableMenuItems(widget, widgetId, isEnabled) {
    $("#" + widgetId).find(".k-item").each(function (index) {
        widget.enable($(this), isEnabled);
    });
}

// Check checkbox on mail selection
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

// Populate the details view with the selected mail
function populateDetailsView(item) {
    $('.mail-subject').text(item.Subject);
    $('.mail-sender').text(item.To);
    $('.mail-date').text(item.Date);
    $('.mail-text').html(item.Text);
}