$(document).ready(function () {

    var grid = "#jqMaterialGrid";
    var gridpager = "#jqMaterialGridPager";

    var bodyElem = $('.container');
    new ResizeSensor(bodyElem, function () {
        var bodyElemWidth = Math.round($('.container').width());
        var newGridWidth = bodyElemWidth - 25;
        $(grid).jqGrid("setGridWidth", newGridWidth, true);
        $('.ui-jqgrid-bdiv').css('overflow', 'hidden');
    });


    $(grid).jqGrid({
        url: relativepath + "Material/GetMaterialList",
        mtype: "POST",
        styleUI: 'Bootstrap',
        datatype: "json",
        height: 'auto',
        jsonReader: jsonreader,
        colModel: [

            { label: 'MaterialId', name: 'MaterialId', key: true, width: 100, hidden: true, },
            { label: 'EncryptMaterialId', name: 'EncryptMaterialId', width: 100, hidden: true, },
            { label: 'Material', name: 'MaterialName', width: 400, },
            { label: 'Description', name: 'Description', width: 400, },

        ],
        rownumbers: true,
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30, 40, 50],
        //editurl: relativepath + "Material/MaterialManipulation",
        pager: gridpager,
        caption: "Material List"

    });
    $(grid).jqGrid('navGrid', gridpager, { edit: false, add: false, del: true, refresh: true, search: false },

        {},
        {},
        {
            url: relativepath + "Material/MaterialDeletion",
            closeOnEscape: true,
            reloadAfterSubmit: true,
            closeAfterDel: true,
            recreateFrom: true,

            delData: {
                MaterialId: function () {
                    var selRowId = $(grid).jqGrid('getGridParam', 'selrow');
                    var rowData = $(grid).jqGrid("getRowData", selRowId);
                    return rowData.MaterialId;
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
                window.location = relativepath + "Material/Add";
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
                    console.log(rowData.EncryptMaterialId)
                    window.location = relativepath + "Material/Add?id=" + rowData.EncryptMaterialId;

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

