
$(document).ready(function () {

    var grid = "#jqStackGrid";
    var gridpager = "#jqStackGridPager";
    var lastSelection;

    var bodyElem = $('.container');
    new ResizeSensor(bodyElem, function () {
        var bodyElemWidth = Math.round($('.container').width());
        var newGridWidth = bodyElemWidth - 20;
        $(grid).jqGrid("setGridWidth", newGridWidth, true);
        $('.ui-jqgrid-bdiv').css('overflow', 'hidden');
    });


    $(grid).jqGrid({
        url: relativepath + "Outward/GetStocks",
        mtype: "POST",
        styleUI: 'Bootstrap',
        datatype: "json",
        height: 'auto',
        jsonReader: jsonreader,
        colModel: [
            { label: 'StockId', name: 'StockId', key: true, width: 100, hidden: true, },
            {
                label: 'Material', name: 'MaterialId', width: 150, formatter: 'select', stype: 'select', edittype: "select", editoptions: { value: jqMaterialList }, searchoptions: { value: jqMaterialList, }
            },
            {
                label: 'Type', name: 'TypeId', width: 150, formatter: 'select', stype: 'select', edittype: "select", editoptions: { value: jqTypeList }, searchoptions: { value: jqTypeList, }
            },
            {
                label: 'Item', name: 'ItemId', width: 150, formatter: 'select', stype: 'select', edittype: "select", editoptions: { value: jqItemList }, searchoptions: { value: jqItemList, }
            },
            { label: 'Avail Quantity', name: 'AvailableQuantity', width: 110, },
            { label: 'Outward Quantity', name: 'OutwardQuantity', width: 110, editable: true, search: false, },
            //{ label: 'Description', name: 'Description', width: 150, editable: true, search: false },
            //{ label: 'Sent To', name: 'SentTo', width: 150, editable: true, search: false },

        ],
        multiselect: true,
        multiboxonly: true,
        rownumbers: true,
        viewrecords: true,
        rowNum: 10,
        rowList: [10, 20, 30, 40, 50],
        editurl: relativepath + "Inward/StackManipulation",
        pager: gridpager,
        caption: "Availabe Stock    <small class='text-danger' style='padding-left:500px'> (Outward quantity is required also should be less than or equal to available quantity.)</small>",

        loadComplete: function () {
            var $this = $(this), ids = $this.jqGrid('getDataIDs'), i, l = ids.length;
            for (i = 0; i < l; i++) {
                $this.jqGrid('editRow', ids[i], true);
            }
            $('#cb_jqStackGrid').hide();
        },

        beforeSelectRow: function (rowid, e) {
            return false;

        },


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

    $("input").attr('autocomplete', 'off');


    $(document).on("click", "#AddMaterial", function () {

        var selrows = $(grid).jqGrid('getGridParam', 'selarrrow');
        if (selrows.length == 0) {
            Notify_Validation("Material is required to select");

            return;
        }
        $.each(selrows, function (key, value) {

            var rowobj = $(grid).jqGrid('getRowData', value);
            var outwardQuantity = $('#' + value + '_OutwardQuantity').val();
            var description = $('#' + value + '_Description').val();
            var sentto = $('#' + value + '_SentTo').val();
            var selrowobj = {
                OutwardStackDetailId: 0,
                OutwardId: 0,
                StockId: rowobj.StockId,
                MaterialId: rowobj.MaterialId,
                TypeId: rowobj.TypeId,
                ItemId: rowobj.ItemId,
                AvailableQuantity: rowobj.AvailableQuantity,
                OutwardQuantity: outwardQuantity,
                Description: description,
                SentTo: sentto,
            };

            //var existRow = $.grep(outwardgridlist, function (e) {
            //    if (e.StockId == rowobj.StockId) {
            //        e.OutwardQuantity = outwardQuantity;
            //    }
            //    return e.StockId == rowobj.StockId;
            //});
            
            //if (!existRow.length) {
            //    outwardgridlist.push(selrowobj);
            //}
            outwardgridlist.push(selrowobj);
           

        });
        Notify_Message("Stocks are added successfully");
       // $('#stackpopupModal').modal('hide');
        jQuery("#jqOutwardAddGrid").jqGrid('setGridParam', { datatype: 'local', data: outwardgridlist }).trigger("reloadGrid");

    });

    $(document).on("click", "#closeMaterial", function () {
        $('#stackpopupModal').modal('hide');
    });

    $(document).on("change", "#jqStackGrid td > .checkbox", function () {

        var rowid = $(this).closest('tr').attr('id');
        if ($(this).is(':checked')) {
            var rowData = $(grid).jqGrid('getRowData', rowid);
            var outwardQuantityVal = $('#' + rowid + '_OutwardQuantity').val();
            var outwardQuantity = parseInt(outwardQuantityVal);
            var availquantity = parseInt(rowData.AvailableQuantity);
            if (outwardQuantityVal != "") {
                if (!isNaN(outwardQuantityVal)) {
                    if (outwardQuantity == 0 || availquantity < outwardQuantity) {
                        Notify_Validation('Outward quantity is required also should be less than of available quantity.');
                        $(this).prop("checked", false);
                        return false;
                    } else {
                        $(grid).jqGrid('setSelection', rowid);
                        return true;
                    }
                }
                else {
                    $(this).prop("checked", false);
                    Notify_Validation('Outward quantity should be a mumeric.');
                    return true;

                }
            } else {
                $(this).prop("checked", false);
                Notify_Validation('Outward quantity is required also should be less than of available quantity.');
            }
        } else {
            $(grid).jqGrid('setSelection', rowid);
        }

    });

    $(document).on("blur", "#jqStackGrid td > input[name='OutwardQuantity']", function () {

        var rowid = $(this).closest('tr').attr('id');
        var checkbox = '#jqg_jqStackGrid_' + rowid;
        var rowData = $(grid).jqGrid('getRowData', rowid);
        var outwardQuantityVal = $('#' + rowid + '_OutwardQuantity').val();
        var availquantity = parseInt(rowData.AvailableQuantity);
        if (outwardQuantityVal != "") {
            if (!isNaN(outwardQuantityVal)) {
                var outwardQuantity = parseInt(outwardQuantityVal);
                if (outwardQuantity == 0 || availquantity < outwardQuantity) {

                    if ($(checkbox).is(':checked')) {
                        $(checkbox).prop("checked", false);
                        $(grid).jqGrid('setSelection', rowid);
                    }
                    Notify_Validation('Outward quantity should be less than of available quantity.');
                    return false;
                } else {
                    // $(grid).jqGrid('setSelection', rowid);
                    return true;
                }
            } else {
                if ($(checkbox).is(':checked')) {
                    $(grid).jqGrid('setSelection', rowid);
                    $(checkbox).prop("checked", false);
                }
                Notify_Validation('Outward quantity should be a Numeric.');
                return true;
            }
        } else {
            if ($(checkbox).is(':checked')) {
                $(grid).jqGrid('setSelection', rowid);
                $(checkbox).prop("checked", false);
            }
        }

    });
});

