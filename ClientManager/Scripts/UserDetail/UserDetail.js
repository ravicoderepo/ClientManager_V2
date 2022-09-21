
$(document).ready(function () {

    var grid = "#jqUserDetailGrid";
    var gridpager = "#jqUserDetailGridPager";

    var bodyElem = $('.container');
    new ResizeSensor(bodyElem, function () {
        var bodyElemWidth = Math.round($('.container').width());
        var newGridWidth = bodyElemWidth - 25;
        $(grid).jqGrid("setGridWidth", newGridWidth, true);
        $('.ui-jqgrid-bdiv').css('overflow', 'hidden');
    });


    $(grid).jqGrid({
        url: relativepath + "UserDetail/GetUserList",
        mtype: "POST",
        styleUI: 'Bootstrap',
        datatype: "json",
        height: 'auto',
        jsonReader: jsonreader,
        colModel: [

            { label: 'UserDetailId', name: 'UserDetailId', key: true, width: 100, hidden: true, },
            { label: 'EncryptUserDetailId', name: 'EncryptUserDetailId', width: 100, hidden: true, },
            {
                label: 'Profile',
                name: 'ProfileImage',
                width: 50,
                search: false,
                align: 'center',
                formatter: function (cellvalue, options, rowObject) {
                    var img = "<img src='/Images/default_profile.jpg' alt='Profile Image' class='rounded-circle' width='36' >"
                    if (cellvalue.length > 0) {
                        img=  "<img src='" + cellvalue + "' alt='Profile Image' class='rounded-circle' width='36' />";
                    }
                    return img;
                }
            },
            { label: 'FirstName', name: 'FirstName', width: 200, },
            { label: 'LastName', name: 'LastName', width: 200, },
            { label: 'Email', name: 'Email', width: 200, },
            { label: 'RoleDesc', name: 'RoleDesc', width: 200, search: false, },

        ],
        rownumbers: true,
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30, 40, 50],
        //editurl: relativepath + "Material/MaterialManipulation",
        pager: gridpager,
        caption: "User List"

    });
    $(grid).jqGrid('navGrid', gridpager, { edit: false, add: false, del: true, refresh: true, search: false },

        {},
        {},
        {
            url: relativepath + "UserDetail/UserDeletion",
            closeOnEscape: true,
            reloadAfterSubmit: true,
            closeAfterDel: true,
            recreateFrom: true,

            delData: {
                UserDetailId: function () {
                    var selRowId = $(grid).jqGrid('getGridParam', 'selrow');
                    var rowData = $(grid).jqGrid("getRowData", selRowId);
                    return rowData.UserDetailId;
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
                window.location = relativepath + "UserDetail/Register";
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
                    window.location = relativepath + "UserDetail/UpdateUserDetail?id=" + rowData.EncryptUserDetailId;
                }
            }
        });

    $(grid).jqGrid('navButtonAdd', gridpager,
        {
            caption: "", buttonicon: "glyphicon glyphicon-lock", title: "Reset password selected row",
            onClickButton: function () {
                var selRowId = $(grid).jqGrid('getGridParam', 'selrow');
                if (selRowId == null) {
                    $.jgrid.info_dialog('Warning', 'Please, select row', '', { styleUI: 'Bootstrap' });
                } else {
                    var rowData = $(grid).jqGrid("getRowData", selRowId);
                    window.location = relativepath + "UserDetail/ResetPassword?id=" + rowData.EncryptUserDetailId;
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
            url: relativepath + '/Material/MaterialManipulation',
            type: "POST",
            data: fileData,
            success: function (result) {
              //  alert(result);
            },
            error: function (err) {
                Notify_Validation(err.statusText);
            }
        });

    });
});

