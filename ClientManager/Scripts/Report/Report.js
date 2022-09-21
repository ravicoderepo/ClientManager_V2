$('#report').addClass('active');
$(document).ready(function () {

    GridBind();

    $("input").attr('autocomplete', 'off');



    $(document).on("click", "#btnSearch", function () {
        if (DateValidationSearch()) {
            $('#reportGridDiv').empty();
            var material = $('#Material').val();
            var type = $('#Type').val();
            var item = $('#Item').val();
            var fromDate = $('#FromDate').val();
            var toDate = $('#ToDate').val();

            GridBind(material, type, item, fromDate, toDate);
        }

    });

    $(document).on("click", "#btnDownloadReport", function () {
        var material = $('#Material').val();
        var type = $('#Type').val();
        var item = $('#Item').val();
        var fromDate = $('#FromDate').val();
        var toDate = $('#ToDate').val();
        window.location = relativepath + "Report/DownloadStockReport?material=" + material + "&type=" + type + "&item=" + item + "&fromDate=" + fromDate + "&toDate=" + toDate


    });


});

function GridBind(material, type, item, fromDate, toDate) {

    $("#reportGridDiv").append('<table id="jqReportGrid"></table>');
    $("#reportGridDiv").append('<div id="jqReportGridPager"></div>');

    var grid = "#jqReportGrid";
    var gridpager = "#jqReportGridPager";

    var bodyElem = $('.container');
    new ResizeSensor(bodyElem,
        function () {
            var bodyElemWidth = Math.round($('.container').width());
            var newGridWidth = bodyElemWidth - 25;
            $(grid).jqGrid("setGridWidth", newGridWidth, true);
            $('.ui-jqgrid-bdiv').css('overflow', 'hidden');
        });


    $(grid).jqGrid({
        url: relativepath + "Report/GetStockReportList?material=" + material + "&type=" + type + "&item=" + item + "&fromDate=" + fromDate + "&toDate=" + toDate,
        mtype: "POST",
        styleUI: 'Bootstrap',
        datatype: "json",
        height: 'auto',
        jsonReader: jsonreader,
        colModel: [
            { label: 'StockId', name: 'StockId', key: true, width: 100, hidden: true, },
            {
                label: 'Material', name: 'Material', width: 100, formatter: 'select',
                editoptions: { value: jqMaterialList }, stype: 'select', searchoptions: { value: jqMaterialList, }, sortable: false, search: false
            },
            {
                label: 'Type', name: 'Type', width: 100, formatter: 'select', editoptions: { value: jqTypeList },
                searchoptions: { value: ": Select", }, stype: 'select', sortable: false, search: false
            },
            {
                label: 'Item', name: 'Item', width: 200, formatter: 'select', editoptions: { value: jqItemList },
                searchoptions: { value: ": Select", }, stype: 'select', sortable: false, search: false
            },
            { label: 'Min Avail Quantity', name: 'MinimumAvailableQuantity', width: 100, sortable: false, search: false },
            { label: 'Available', name: 'AvailableQuantity', width: 100, search: false },
            { label: 'Inward', name: 'Inward', width: 100, sortable: false, search: false },
            { label: 'Outward', name: 'Outward', width: 100, sortable: false, search: false },
            {
                label: 'Date', name: 'Date', width: 100, formatter: 'date', formatoptions: { newformat: jqgridDateFormat }, sorttype: "date", sortable: false, search: false,
                searchoptions: {
                    dataInit: function (elem) {
                        $(elem).datepicker({
                            dateFormat: calendardateformat,
                            changeYear: true,
                            changeMonth: true,
                            showWeek: true,
                            onSelect: function (dateText, inst) {
                                setTimeout(function () {
                                    $(grid)[0].triggerToolbar();
                                },
                                    100);
                            }
                        });
                    }
                }
            },

        ],
        rownumbers: true,
        viewrecords: true,
        rowNum: 50,
        rowList: [50, 100, 150, 200, 250],
        //editurl: relativepath + "Inward/StackManipulation",
        pager: gridpager,
        caption: "Report",
        //footerrow: true,
        //gridComplete: function () {
        //    var colSum = $(grid).jqGrid('getCol', 'Inward', false, 'sum');
        //    $(grid).jqGrid('footerData', 'set', {
        //        'Inward': colSum
        //    });
        //}

    });


    $(grid).jqGrid('navGrid', gridpager, { edit: false, add: false, del: false, refresh: true, search: false },

        {},
        {},
        {},
        {},
        {}
    );

    $(grid).jqGrid('filterToolbar', {
        stringResult: true,
        searchOnEnter: false,
        defaultSearch: "cn",
        beforeSearch: function () {
            modifySearchingFilter.call(this, ' ');
        }
    });


    //$("#Type").prop("disabled", true);
    //$("#Item").prop("disabled", true);

    $("#Material").change(function () {
    
        $.ajax({
            url: relativepath + '/Report/GetTypelist?materialId=' + $(this).val(),
            type: "GET",
            success: function (result) {
                $('#Type').empty();
                $('#Item').empty();
                $("#Type").append(new Option("Select", ""));
                $("#Item").append(new Option("Select", ""));
                $.each(result, function (i, item) {
                    $("#Type").append(new Option(item.TypeName, item.TypeId));
                });
            },
            error: function (err) {
                alert(err.statusText);
            }
        });

    });


    $("#Type").change(function () {
        
        $.ajax({
            url: relativepath + '/Report/GetItemlist?typeId=' + $(this).val(),
            type: "GET",
            success: function (result) {
                $('#Item').empty();
                $("#Item").append(new Option("Select", ""));
                $.each(result, function (i, item) {
                    $("#Item").append(new Option(item.ItemName, item.ItemId));
                });
            },
            error: function (err) {
                alert(err.statusText);
            }
        });
    });

}
