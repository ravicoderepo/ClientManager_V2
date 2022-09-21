
$(document).ready(function () {

    var grid = "#jqItemGrid";
    var gridpager = "#jqItemGridPager";

    var bodyElem = $('.container');
    new ResizeSensor(bodyElem, function () {
        var bodyElemWidth = Math.round($('.container').width());
        var newGridWidth = bodyElemWidth - 25;
        $(grid).jqGrid("setGridWidth", newGridWidth, true);
        $('.ui-jqgrid-bdiv').css('overflow', 'hidden');
    });

    $(grid).jqGrid({
        url: relativepath + "Item/GetItemList",
        mtype: "POST",
        styleUI: 'Bootstrap',
        datatype: "json",
        height: 'auto',
        jsonReader: jsonreader,
        colModel: [

            { label: 'ItemId', name: 'ItemId', key: true, width: 100, hidden: true, },
            { label: 'EncryptItemId', name: 'EncryptItemId', width: 100, hidden: true, },
            {
                label: 'Material',
                name: 'MaterialId',
                width: 200,
                editable: true,
                editrules: { required: true, },
                formatter: 'select',
                stype: 'select',
                edittype: "select",
                editoptions: { value: jqMaterialList },
                searchoptions: { value: jqMaterialList, }
            }, {
                label: 'Type',
                name: 'TypeId',
                width: 200,
                editable: true,
                editrules: { required: true, },
                formatter: 'select',
                stype: 'select',
                edittype: "select",
                editoptions: { value: jqTypeList },
                searchoptions: { value: jqTypeList, },
              
            },
            { label: 'Item', name: 'ItemName', width: 200, editable: true, editrules: { required: true, } },
            { label: 'Description', name: 'Description', width: 200, editable: true, editrules: { required: true, } },
            { label: 'Min Avail Quantity', name: 'MinimunAvailableQuantity', width: 100, editable: true, editrules: { required: true, } },

        ],
        rownumbers: true,
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30, 40, 50],
        //editurl: relativepath + "Item/ItemManipulation",
        pager: gridpager,
        caption: "Item List"

    });
    $(grid).jqGrid('navGrid', gridpager, { edit: false, add: false, del: true, refresh: true, search: false },

        {},
        {},
        {
            url: relativepath + "Item/ItemDeletion",
            closeOnEscape: true,
            reloadAfterSubmit: true,
            closeAfterDel: true,
            recreateFrom: true,

            delData: {
                itemId: function () {
                    var selRowId = $(grid).jqGrid('getGridParam', 'selrow');
                    var rowData = $(grid).jqGrid("getRowData", selRowId);
                    return rowData.ItemId;
                }
            }
        },
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
                window.location = relativepath + "Item/Add";
            },

        });
    $(grid).jqGrid('navButtonAdd', gridpager,
        {
            caption: "", buttonicon: "glyphicon glyphicon-edit", title: "Edit selected row",
            onClickButton: function () {
                var selRowId = $(grid).jqGrid('getGridParam', 'selrow');
                if (selRowId == null) {
                    $.jgrid.info_dialog('Warning', 'Please, select row', '', { styleUI: 'Bootstrap' });
                } else {
                    var rowData = $(grid).jqGrid("getRowData", selRowId);
                    window.location = relativepath + "Item/Add?id=" + rowData.EncryptItemId;
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





});

