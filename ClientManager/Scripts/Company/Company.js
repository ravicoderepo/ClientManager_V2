
$(document).ready(function () {

    var grid = "#jqCompanyGrid";
    var gridpager = "#jqCompanyGridPager";

    var bodyElem = $('.container');
    new ResizeSensor(bodyElem, function () {
        var bodyElemWidth = Math.round($('.container').width());
        var newGridWidth = bodyElemWidth - 25;
        $(grid).jqGrid("setGridWidth", newGridWidth, true);
        $('.ui-jqgrid-bdiv').css('overflow', 'hidden');
    });


    $(grid).jqGrid({
        url: relativepath + "Company/GetCompanyList",
        mtype: "POST",
        styleUI: 'Bootstrap',
        datatype: "json",
        height: 'auto',
        jsonReader: jsonreader,
        colModel: [

            { label: 'CompanyId', name: 'CompanyId', key: true, width: 100, hidden: true, },
            { label: 'EncryptCompanyId', name: 'EncryptCompanyId',width: 100, hidden: true, }, 
            { label: 'Name', name: 'Name', width: 400, },
            { label: 'Description', name: 'Description', width: 400, },

        ],
        rownumbers: true,
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30, 40, 50],
        //editurl: relativepath + "Material/MaterialManipulation",
        pager: gridpager,
        caption: "Company List"

    });
    $(grid).jqGrid('navGrid', gridpager, { edit: false, add: false, del: true, refresh: true, search: false },

        {},
        {},
        {
            url: relativepath + "Company/CompanyDeletion",
            closeOnEscape: true,
            reloadAfterSubmit: true,
            closeAfterDel: true,
            recreateFrom: true,

            delData: {
                companyId: function () {
                    var selRowId = $(grid).jqGrid('getGridParam', 'selrow');
                    var rowData = $(grid).jqGrid("getRowData", selRowId);
                    return rowData.CompanyId;
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
                window.location = relativepath + "Company/Add";
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
                    window.location = relativepath + "Company/Add?id=" + rowData.EncryptCompanyId;
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

    $(document).on("click", "#btnsave", function () {


        $.ajax({
            url: relativepath + '/Company/CompanyManipulation',
            type: "POST",
            data: fileData,
            success: function (result) {
               // alert(result);
            },
            error: function (err) {
                Notify_Validation(err.statusText);
            }
        });

    });
});

