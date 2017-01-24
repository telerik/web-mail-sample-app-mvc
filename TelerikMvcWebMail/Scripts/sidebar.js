var selected = Cookies.get('selected');
var marked = false;
var navigated = false;
var savedScroll = 0;

function setSelectedNode(selected) {
    var treeview = $("#navigationTreeView").data("kendoTreeView");
    if (selected) {
        Cookies.remove('selected');
        var node = $("#navigationTreeView").find('li').eq(selected);
        treeview.select(node);
    }
}

function populateNavigationTree(data) {
    var newDataSource = new kendo.data.HierarchicalDataSource({ data: data });
    var navigationTreeView = $('#navigationTreeView').data('kendoTreeView');
    navigationTreeView.setDataSource(newDataSource);

    if (selected) {
        setSelectedNode(selected);
    }
    else {
        setSelectedNode("0");
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

        if (Cookies.get('markedAsUnread') === 'marked') {
            Cookies.set('markedAsUnread', '');
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