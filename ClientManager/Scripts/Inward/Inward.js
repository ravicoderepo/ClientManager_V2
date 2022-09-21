
$(document).ready(function () {

    var deleterow = false;
    GridBind(null, null);

    $("input").attr('autocomplete', 'off');

    $(document).on("click", "#btnSearch", function () {
        if (DateValidationSearch()) {
            var fromDate = $('#FromDate').val();
            var toDate = $('#ToDate').val();

            StockTransactionDetails(null, null, null, fromDate, toDate)
        }
    });
    $('#btnAdd').click(function () {
        window.location = relativepath + "Inward/Add";
    });
});


function GridBind(fromDate, toDate) {

    $("#stackGridDiv").append('<table id="jqStackGrid"></table>');
    $("#stackGridDiv").append('<div id="jqStackGridPager"></div>');
    var grid = "#jqStackGrid";
    var gridpager = "#jqStackGridPager";

    var bodyElem = $('.container');
    new ResizeSensor(bodyElem,
        function () {
            var bodyElemWidth = Math.round($('.container').width());
            var newGridWidth = bodyElemWidth - 25;
            $(grid).jqGrid("setGridWidth", newGridWidth, true);
            $('.ui-jqgrid-bdiv').css('overflow', 'hidden');
        });


    $(grid).jqGrid({
        url: relativepath + "Inward/GetStockList?fromDate=" + fromDate + "&toDate=" + toDate,
        mtype: "POST",
        styleUI: 'Bootstrap',
        datatype: "json",
        height: 'auto',
        jsonReader: jsonreader,
        colModel: [
            { label: 'StockId', name: 'StockId', width: 100, hidden: true, },
            { label: 'EncryptStockId', name: 'EncryptStockId', width: 100, hidden: true, },
            { label: 'StockransactionId', name: 'StockransactionId', width: 100, hidden: true, },
            { label: 'EncryptStockransactionId', name: 'EncryptStockransactionId', width: 100, hidden: true, },
            {
                label: 'Material', name: 'MaterialId', width: 100, formatter: 'select', stype: 'select', edittype: "select", editoptions: { value: jqMaterialList }, searchoptions: { value: jqMaterialList, }, sortable: false
            },
            {
                label: 'Type', name: 'TypeId', width: 100, formatter: 'select', search: true, stype: 'select', edittype: "select", editoptions: { value: jqTypeList }, searchoptions: { value: jqTypeList, }, sortable: false
            },
            {
                label: 'Item', name: 'ItemId', width: 150, formatter: 'select', search: true, stype: 'select', edittype: "select", editoptions: { value: jqItemList }, searchoptions: { value: jqItemList, }, sortable: false
            },
            {
                label: 'Min Avail Quantity', name: 'MinimumAvailableQuantity', width: 50, search: true, sortable: false,

            },
            {
                label: 'Avail Quantity', name: 'AvailableQuantity', width: 50, search: true, sortable: false,

            },
            {
                label: '', name: '', width: 20, search: false, sortable: false, align: 'center',
                formatter: function (cellvalue, options, rowObject) {

                    return '<a href=\"#\" onclick=\"StockTransactionDetails(\''+ rowObject.StockId + '\',\'' + rowObject.EncryptStockId +'\',\'' + rowObject.ItemId +'\', null, null);\"><i class="fa fa-eye"></i></a>';
                }

            },




        ],
        rownumbers: true,
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30, 40, 50],
        //editurl: relativepath + "Inward/StackManipulation",
        pager: gridpager,
        caption: "Material Inward List",
        ondblClickRow: function (rowId) {
            var rowData = $(grid).jqGrid("getRowData", rowId);
            StockTransactionDetails(rowData.StockId, rowData.EncryptStockId, rowData.ItemId, null, null)
        }

    });

    $(grid).jqGrid('navGrid', gridpager, { edit: false, add: false, del: false, refresh: true, search: false },

        {},
        {},
        {},
        {},
        {}
    );

    //$(grid).jqGrid('inlineNav', gridpager, {
    //    addParams: {
    //        position: 'last',
    //        addRowParams: {
    //            keys: true,
    //            successfunc: function () {
    //                $(this).trigger("reloadGrid");
    //            }
    //        }
    //    }, editParams: {
    //        keys: true,
    //        successfunc: function () {
    //            $(this).trigger("reloadGrid");
    //        }
    //    }
    //});
    $(grid).jqGrid('navButtonAdd', gridpager,
        {
            caption: "", buttonicon: "glyphicon glyphicon-plus", title: "Add new row",
            onClickButton: function () {
                window.location = relativepath + "Inward/Add";
            },

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



function StockTransactionDetails(stockId, encryptStockId, itemId, fromDate, toDate) {

    $('#stackTransactionModal').modal();
    if (itemId != null) {
        var jqItemListarray = jqItemList.split(';');
        $.each(jqItemListarray, function (index, value) {
            var valuesplit = value.split(':');
            if (valuesplit[0] == itemId) {
                $('#gridTitle').text('Item - ' + valuesplit[1]);
            }
        });
    }


    var content = "<table id='jqStackTransactionGrid'> </table> <div id='jqStackTransactionGridPager'></div>"
    $('#stackTransactionElement').append(content);
    var stackTransactionGrid = "#jqStackTransactionGrid";
    var stackTransactionGridPager = "#jqStackTransactionGridPager";

    $(stackTransactionGrid).jqGrid({
        url: relativepath + "Inward/GetStockTransactionList?fromDate=" + fromDate + "&toDate=" + toDate + "&stockId=" + stockId,
        mtype: "POST",
        styleUI: 'Bootstrap',
        datatype: "json",
        height: 'auto',
        width: Math.round($('.container').width()),
        jsonReader: jsonreader,
        colModel: [
            { label: 'StockransactionId', name: 'StockransactionId', key: true, width: 100, hidden: true, },
            { label: 'EncryptStockransactionId', name: 'EncryptStockransactionId', width: 100, hidden: true, },
            { label: 'StockId', name: 'StockId', width: 100, hidden: true, },
            { label: 'EncryptStockId', name: 'EncryptStockId', width: 100, hidden: true, },
            {
                label: 'Item', name: 'ItemId', width: 150, formatter: 'select', search: true, stype: 'select', edittype: "select", editoptions: { value: jqItemList }, searchoptions: { value: jqItemList, }, sortable: false
            },
            { label: 'Quantity', name: 'Quantity', width: 70, search: true, sortable: false },
            { label: 'PONumber', name: 'PONumber', width: 100, search: true, sortable: false },
            {
                label: 'Received From', name: 'ReceivedFrom', width: 150, formatter: 'select', search: true, stype: 'select', edittype: "select", editoptions: { value: jqCompanyList }, searchoptions: { value: jqCompanyList, }, sortable: false,
            },
            { label: 'Received By', name: 'ReceivedBy', width: 100, search: true, sortable: false },
            {
                label: 'Received Date', name: 'ReceivedDate', width: 100, formatter: 'date', sortable: false, formatoptions: { newformat: jqgridDateFormat }, sorttype: "date",
                searchoptions: {
                    dataInit: function (elem) {
                        $(elem).datepicker({
                            dateFormat: calendardateformat,
                            changeYear: true,
                            changeMonth: true,
                            showWeek: true,
                            onSelect: function (dateText, inst) {
                                setTimeout(function () {
                                    $(stackTransactionGrid)[0].triggerToolbar();
                                },
                                    100);
                            }
                        });
                    }
                }
            },
            { label: 'GRN Number', name: 'GRNNumber', width: 100, search: true, sortable: false },
            { label: 'Comments', name: 'Description', width: 150, search: true, sortable: false },

        ],
        rownumbers: true,
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30, 40, 50],
        pager: stackTransactionGridPager,
        caption: "Stock Transaction" + $('#gs_ReceivedFrom').find('option[value=1]').text(),

    });

    $(stackTransactionGrid).jqGrid('navGrid', stackTransactionGridPager, { edit: false, add: false, del: true, refresh: true, search: false },

        {},
        {},
        {
            url: relativepath + "Inward/StockTransactionDeletion",
            closeOnEscape: true,
            reloadAfterSubmit: true,
            closeAfterDel: true,
            recreateFrom: true,

            delData: {
                stockTranId: function () {
                    var selRowId = $(stackTransactionGrid).jqGrid('getGridParam', 'selrow');
                    var rowData = $(stackTransactionGrid).jqGrid("getRowData", selRowId);
                    deleterow = true;
                    return rowData.StockransactionId;
                }
            }

        },
        {},
        {}
    );

    $(stackTransactionGrid).jqGrid('navButtonAdd', stackTransactionGridPager,
        {
            caption: "", buttonicon: "glyphicon glyphicon-plus", title: "Add new row",
            onClickButton: function () {
                window.location = relativepath + "Inward/Add?id=" + encryptStockId;

            },

        });

    $(stackTransactionGrid).jqGrid('navButtonAdd', stackTransactionGridPager,
        {
            caption: "", buttonicon: "glyphicon glyphicon-edit", title: "Edit selected row",
            onClickButton: function () {
                var selRowId = $(stackTransactionGrid).jqGrid('getGridParam', 'selrow');
                if (selRowId == null) {
                    $.jgrid.info_dialog('Warning', 'Please, select row', '', { styleUI: 'Bootstrap' });
                } else {
                    var rowData = $(stackTransactionGrid).jqGrid("getRowData", selRowId);
                    window.location = relativepath + "Inward/Add?tranId=" + rowData.EncryptStockransactionId;
                }
            }
        });

    $(stackTransactionGrid).jqGrid('filterToolbar', {
        stringResult: true,
        searchOnEnter: false,
        defaultSearch: "cn",
        beforeSearch: function () {
            modifySearchingFilter.call(this, ' ');
        }
    });

    $('#btnstockpopupclose').click(function () {
        $('#stackTransactionElement').empty();
        if (deleterow) {
            $('#jqStackGrid').trigger("reloadGrid");
        }
    });


}
