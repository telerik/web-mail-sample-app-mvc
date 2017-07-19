$(document).ready(function () {
    $('.resource').on("click", function () {
        $(this).toggleClass('selected');
    });

    $('.new-Event').on('click', function (e) {
        var scheduler = $("#scheduler").data("kendoScheduler");
        scheduler.addEvent();
    });

    $(".calendar-resources-list > div").click(function (e) {
        var selected = $.map($(".calendar-resources-list > div.selected"), function (div) {
            return parseInt($(div).attr("data-resource-value"));
        });

        var filter = {
            logic: "or",
            filters: $.map(selected, function (value) {
                return {
                    operator: "eq",
                    field: "Category",
                    value: value
                };
            })
        };

        if (filter.filters.length === 0) {
            filter = { field: "Category", operator: "eq", value: "7" };
        }

        var scheduler = $("#scheduler").data("kendoScheduler");

        scheduler.dataSource.filter(filter);
    });
});

function onExportBtnClick(e) {
    if (e === "exportPdf") {
        exportSchedulerToPdf($("#scheduler"), "Calendar.pdf");
    }
    else if (e === "exportImage") {
        exportSchedulerToImage($("#scheduler"), "Calendar.png");
    }
}

function exportSchedulerToPdf(content, fileName) {
    kendo.drawing.drawDOM(content)
    .then(function (group) {
        return kendo.drawing.exportPDF(group, {
            paperSize: "auto",
            margin: { left: "1cm", top: "1cm", right: "1cm", bottom: "1cm" }
        });
    })
    .done(function (data) {
        kendo.saveAs({
            dataURI: data,
            fileName: fileName
        });
    });
}

function exportSchedulerToImage(content, fileName) {
    kendo.drawing.drawDOM(content)
        .then(function (group) {
            return kendo.drawing.exportImage(group);
        })
        .done(function (data) {
            kendo.saveAs({
                dataURI: data,
                fileName: fileName
            });
        });
}

function onCalenderChange(e) {
    var scheduler = $("#scheduler").data("kendoScheduler");
    scheduler.date(e.sender.current());
}

function onSchedulerChange(e) {
    var calendar = $('#calendar').data('kendoCalendar');
    var selectedSlot = e.slots[0];

    if (selectedSlot) {
        calendar.value(selectedSlot.start);
    }
}

function onSchedulerEdit(e) {
    var scheduler = e.sender;
    var selectedSlot = scheduler.select();

    if (selectedSlot && selectedSlot.start && selectedSlot.end && e.event.isNew()) {
        populateDateTimePickers(e.container, selectedSlot);
    }
}

function populateDateTimePickers(container, selectedSlot) {
    var startInputs = container.find("[data-container-for=start]").find("input");
    var endInputs = container.find("[data-container-for=end]").find("input");

    var startDate = new Date(selectedSlot.start);

    startInputs.each(function () {
        var element = $(this);
        var widgetType = element.is("[data-role=datepicker]") ? "kendoDatePicker" : "kendoDateTimePicker";

        element.data(widgetType).value(startDate);
        element.data(widgetType).trigger("change");
    });

    var endDate = new Date(selectedSlot.end);

    endInputs.each(function () {
        var element = $(this);
        var widgetType = element.is("[data-role=datepicker]") ? "kendoDatePicker" : "kendoDateTimePicker";

        element.data(widgetType).value(endDate);
        element.data(widgetType).trigger("change");
    });
}