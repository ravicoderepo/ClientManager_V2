var deleterow = false;
$(document).ready(function () {
    deleterow = false;

    GridBind(null, null);
    $("input").attr('autocomplete', 'off');

    $(document).on("click", "#btnSearch", function () {
        if (DateValidationSearch()) {

            var fromDate = $('#FromDate').val();
            var toDate = $('#ToDate').val();
            OutwardTransactionDetails(null, fromDate, toDate, false);
            $('#invoiceNumberHeader').text('')
        }
    });


    $('#btnOutwardAdd').click(function () {
        window.location = relativepath + "Outward/Add";
    });

});

function GridBind(fromDate, toDate) {

    $("#OutwardGridDiv").append('<table id="jqOutwardGrid"></table>');
    $("#OutwardGridDiv").append('<div id="jqOutwardGridPager"></div>');

    var grid = "#jqOutwardGrid";
    var gridpager = "#jqOutwardGridPager";

    var bodyElem = $('.container');
    new ResizeSensor(bodyElem, function () {
        var bodyElemWidth = Math.round($('.container').width());
        var newGridWidth = bodyElemWidth - 25;
        $(grid).jqGrid("setGridWidth", newGridWidth, true);
        $('.ui-jqgrid-bdiv').css('overflow', 'hidden');
    });


    $(grid).jqGrid({
        url: relativepath + "Outward/GetOutwardStockList?fromDate=" + fromDate + "&toDate=" + toDate,
        mtype: "POST",
        styleUI: 'Bootstrap',
        datatype: "json",
        height: 'auto',
        jsonReader: jsonreader,
        colModel: [

            { label: 'OutwardId', name: 'OutwardId', width: 100, hidden: true },
            { label: 'EncryptOutwardId', name: 'EncryptOutwardId', width: 100, hidden: true },
            { label: 'StockId', name: 'StockId', width: 100, hidden: true, },
            { label: 'Invoice Number', name: 'InvoiceNumber', width: 150, sortable: true, },
            {
                label: 'Date', name: 'OutwardDate', width: 120, formatter: 'date', sorttype: "date", sortable: false, search: true,
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

            { label: 'Customer Information', name: 'CustomerInformation', width: 200, sortable: false, search: true, },
            {
                label: 'Attachments', name: 'Attachments', width: 200, search: false, sortable: false, formatter: function (cellvalue, options, rowObject) {
                    return '<a target=”_blank” href="' + relativepath + '/Outward/DownloadAttachment?id=' + rowObject.OutwardId + '">' +
                        cellvalue + '</a>';
                }
            },
            //{ label: 'Total Quantity', name: 'TotalQuantity', width: 100, sortable: false, search: false, },
            {
                label: '', name: '', width: 20, search: false, sortable: false, align: 'center',
                formatter: function (cellvalue, options, rowObject) {

                    return '<a href=\"#\" onclick=\"OutwardTransactionDetails(' + rowObject.OutwardId + ', null, null);\"><i class="fa fa-eye"></i></a>';
                }

            },
        ],
        rownumbers: true,
        viewrecords: true,
        rowNum: 50,
        rowList: [50, 100, 150, 200, 250],
        editurl: relativepath + "Outward/OutwardStackManipulation",
        pager: gridpager,
        caption: "Material Outward Details",
        ondblClickRow: function (rowId) {
            var rowData = $(grid).jqGrid("getRowData", rowId);
            OutwardTransactionDetails(rowData.OutwardId, null, null, true);
            $('#invoiceNumberHeader').text(' - Invoice Number: ' + rowData.InvoiceNumber);
        }

    });

    $(grid).jqGrid('navGrid', gridpager, { edit: false, add: false, del: true, refresh: true, search: false },

        {},
        {},
        {
            url: relativepath + "Outward/OutwardDeletion",
            closeOnEscape: true,
            reloadAfterSubmit: true,
            closeAfterDel: true,
            recreateFrom: true,

            delData: {
                outwardId: function () {
                    var selRowId = $(grid).jqGrid('getGridParam', 'selrow');
                    var rowData = $(grid).jqGrid("getRowData", selRowId);
                    deleterow = true;

                    return rowData.OutwardId;
                }
            }
        },
        {},
        {}
    );
    $(grid).jqGrid('navButtonAdd', gridpager,
        {
            caption: "", buttonicon: "glyphicon glyphicon-plus", title: "Add new row",
            onClickButton: function () {
                window.location = relativepath + "Outward/Add";
            },

        });
    $(grid).jqGrid('navButtonAdd', gridpager,
        {
            caption: "", buttonicon: "glyphicon glyphicon-edit", title: "Edit selected row",
            onClickButton: function () {
                var selRowId = $(grid).jqGrid('getGridParam', 'selrow');
                var rowData = $(grid).jqGrid("getRowData", selRowId);
                if (selRowId == null) {
                    $.jgrid.info_dialog('Warning', 'Please, select row', '', { styleUI: 'Bootstrap' });
                } else {
                    var rowData = $(grid).jqGrid("getRowData", selRowId);
                    window.location = relativepath + "Outward/Add?id=" + rowData.EncryptOutwardId;
                }

            }
        });


    $(grid).jqGrid('filterToolbar', {
        stringResult: true,
        searchOnEnter: false,
        defaultSearch: "cn",
        beforeSearch: function () {
            modifySearchingFilter.call(this, ' ');
        }
    });


}

function OutwardTransactionDetails(outwardId, fromDate, toDate, isHideInvoice) {

    $('#outwardTransactionModal').modal();

    var content = "<table id='jqOutwardTransactionGrid'> </table> <div id='jqOutwardTransactionGridPager'></div>"
    $('#outwardTransactionElement').append(content);
    var outwardTransactionGrid = "#jqOutwardTransactionGrid";
    var outwardTransactionGridPager = "#jqOutwardTransactionGridPager";


    $(outwardTransactionGrid).jqGrid({
        url: relativepath + "Outward/GetOutwardStockTransactionList?fromDate=" + fromDate + "&toDate=" + toDate + "&outwardId=" + outwardId,
        mtype: "POST",
        styleUI: 'Bootstrap',
        datatype: "json",
        height: 'auto',
        width: Math.round($('.container').width()),
        jsonReader: jsonreader,
        colModel: [

            { label: 'OutwardStackDetailId', name: 'OutwardStackDetailId', key: true, hidden: true, },
            { label: 'EncryptOutwardStackDetailId', name: 'EncryptOutwardStackDetailId', hidden: true, },
            { label: 'OutwardId', name: 'OutwardId', width: 100, hidden: true },
            { label: 'EncryptOutwardId', name: 'EncryptOutwardId', width: 100, hidden: true },
            { label: 'StockId', name: 'StockId', width: 100, hidden: true, },
            { label: 'Invoice Number', name: 'InvoiceNumber', width: 100, sortable: false, hidden: isHideInvoice  },
            { label: 'Material', name: 'MaterialId', width: 170, formatter: 'select', stype: 'select', edittype: "select", editoptions: { value: jqMaterialList }, searchoptions: { value: jqMaterialList, }, search: true, sortable: false },
            { label: 'Type', name: 'TypeId', width: 170, formatter: 'select', stype: 'select', edittype: "select", editoptions: { value: jqTypeList }, searchoptions: { value: jqTypeList, }, sortable: false, search: true },
            { label: 'Item', name: 'ItemId', width: 200, formatter: 'select', stype: 'select', edittype: "select", editoptions: { value: jqItemList }, searchoptions: { value: jqItemList, }, sortable: false, search: true},
            { label: 'Quantity', name: 'OutwardQuantity', width: 90, sortable: false, search: true, align: 'center' },

            {
                label: 'Date', name: 'OutwardDate', width: 120, formatter: 'date', sorttype: "date", sortable: true, search:false,
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
        editurl: relativepath + "Outward/OutwardStackManipulation",
        pager: outwardTransactionGridPager,
        caption: "Outward Details",

    });

    $(outwardTransactionGrid).jqGrid('navGrid', outwardTransactionGridPager, { edit: false, add: false, del: true, refresh: true, search: false },

        {},
        {},
        {
            url: relativepath + "Outward/OutwardTransactionDeletion",
            closeOnEscape: true,
            reloadAfterSubmit: true,
            closeAfterDel: true,
            recreateFrom: true,

            delData: {
                outwardStackDetailId: function () {
                    var selRowId = $(outwardTransactionGrid).jqGrid('getGridParam', 'selrow');
                    var rowData = $(outwardTransactionGrid).jqGrid("getRowData", selRowId);
                    deleterow = true;

                    return rowData.OutwardStackDetailId;
                }
            }
        },
        {},
        {}
    );

    //$(outwardTransactionGrid).jqGrid('navButtonAdd', outwardTransactionGridPager,
    //    {
    //        caption: "", buttonicon: "glyphicon glyphicon-plus", title: "Add new row",
    //        onClickButton: function () {
    //            window.location = relativepath + "Outward/Add";
    //        },

    //    });
    $(outwardTransactionGrid).jqGrid('navButtonAdd', outwardTransactionGridPager,
        {
            caption: "", buttonicon: "glyphicon glyphicon-edit", title: "Edit selected row",
            onClickButton: function () {
               
                var selRowId = $(outwardTransactionGrid).jqGrid('getGridParam', 'selrow');
                console.log(selRowId);
                if (selRowId == null) {
                    $.jgrid.info_dialog('Warning', 'Please, select row', '', { styleUI: 'Bootstrap' });
                } else {
                    var rowData = $(outwardTransactionGrid).jqGrid("getRowData", selRowId);
                    window.location = relativepath + "Outward/Add?id=" + rowData.EncryptOutwardId;
                }
            }
        });


    $(outwardTransactionGrid).jqGrid('filterToolbar', {
        stringResult: true,
        searchOnEnter: false,
        defaultSearch: "cn",
        beforeSearch: function () {
            modifySearchingFilter.call(this, ' ');
        }
    });

    $('#btnoutwardpopupclose').click(function () {
        $('#outwardTransactionElement').empty();
        if (deleterow) {
            $('#jqOutwardGrid').trigger("reloadGrid");
        }
    });

}