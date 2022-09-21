var outwardgridlist = [];

$(document).ready(function () {

    var outwardgrid = "#jqOutwardAddGrid";
    var outwardgridpager = "#jqOutwardAddGridPager";
    var lastSelection;

    var bodyElem = $('.container');
    new ResizeSensor(bodyElem, function () {
        var bodyElemWidth = Math.round($('.container').width());
        var newGridWidth = bodyElemWidth - 25;
        $(outwardgrid).jqGrid("setGridWidth", newGridWidth, true);
        $('.ui-jqgrid-bdiv').css('overflow', 'hidden');
    });


    $(outwardgrid).jqGrid({
        styleUI: 'Bootstrap',
        datatype: "local",
        height: 'auto',
        data: outwardgridlist,
        colModel: [

            { label: 'OutwardStackDetailId', name: 'OutwardStackDetailId', hidden: true, },
            { label: 'OutwardId', name: 'OutwardId', hidden: true, },
            { label: 'StockId', name: 'StockId', hidden: true, key: true },
            { label: 'Material', name: 'MaterialId', width: 100, formatter: 'select', stype: 'select', edittype: "select", editoptions: { value: jqMaterialList }, searchoptions: { value: jqMaterialList, } },
            { label: 'Type', name: 'TypeId', width: 120, formatter: 'select', stype: 'select', edittype: "select", editoptions: { value: jqTypeList }, searchoptions: { value: jqTypeList, } },
            { label: 'Item', name: 'ItemId', width: 150, formatter: 'select', stype: 'select', edittype: "select", editoptions: { value: jqItemList }, searchoptions: { value: jqItemList, } },
            //{ label: 'Avail Quantity', name: 'AvailableQuantity', width: 100, },
            { label: 'Outward Quantity', name: 'OutwardQuantity', width: 100, editable: false, },

        ],
        rownumbers: true,
        viewrecords: true,
        rowNum: 50,
        rowList: [50, 100, 150, 200, 250],
        pager: outwardgridpager,
        caption: "Material Outward List",
        editurl: "clientArray",
        loadComplete: function () {
            var $this = $(this), ids = $this.jqGrid('getDataIDs'), i, l = ids.length;
            for (i = 0; i < l; i++) {
                $this.jqGrid('editRow', ids[i], true);
            }
        }

    });

    $(outwardgrid).jqGrid('navGrid', outwardgridpager, { edit: false, add: false, del: true, refresh: false, search: false },

        {},
        {},
        {
            closeOnEscape: true,
            reloadAfterSubmit: true,
            closeAfterDel: true,
            recreateFrom: true,
            onclickSubmit: function (options, rowid) {

            },
        },
        {},
        {}
    );
    $(outwardgrid).jqGrid('navButtonAdd', outwardgridpager,
        {
            caption: "", buttonicon: "glyphicon glyphicon-plus", title: "Add new row",
            onClickButton: function () {
                $("#jqStackGrid").jqGrid('setGridParam', { search: false, postData: { "filters": "" } }).trigger("reloadGrid");
                //$('#jqStackGrid').trigger('reloadGrid');
                $('#stackpopupModal').modal();
            },

        });



    $("input").attr('autocomplete', 'off');

    //Edit 
    if (outward != "") {
        var outwardData = JSON.parse(outward);
        $('#outwardId').val(outwardData.OutwardId);
        $('#invoiceNumber').val(outwardData.InvoiceNumber);
        $('#customerInformation').val(outwardData.CustomerInformation);
        var _date = new Date(parseInt(outwardData.OutwardDate.substr(6)));
        console.log(_date);
        var month = ("0" + (_date.getMonth() + 1)).slice(-2);
        var day = ("0" + _date.getDate()).slice(-2);
        var year = _date.getFullYear();
        console.log(year + "-" + month + "-" + day);
        $('#invoiceDate').val(year + "-" + month + "-" + day);

    }
    if (outwardstock != "") {
        var outwardstocklist = JSON.parse(outwardstock);
        outwardgridlist = outwardstocklist;
        jQuery("#jqOutwardAddGrid").jqGrid('setGridParam', { datatype: 'local', data: outwardgridlist }).trigger("reloadGrid");

    }
    if (attachment != "") {
        var attachmentlist = JSON.parse(attachment);
        $.each(attachmentlist, function (key, obj) {
            $("#divattachment").append('<div id="div_' + obj.AttachmentId + '"> <a href=' + relativepath + '/Outward/DownloadAttachmentByAttachmentId?id=' + obj.AttachmentId + '>' + obj.FileName + '</a> <a href="#" class="text-info" onclick=DeleteAttachment(' + obj.AttachmentId + ')><i class="fa fa-window-close" style="padding:5px" aria-hidden="true"></i></a></br></div>');
        });

    }



    $(document).on("click", "#btnSubmit", function () {
        var outwardresultist = [];
        var outwardId = $('#outwardId').val();
        if ($('#outwardId').val() == "") {
            outwardId = 0;
        }
        var invoiceNo = $('#invoiceNumber').val();
        var customerInformation = $('#customerInformation').val();
        var invoiceDate = $('#invoiceDate').val();
        var gridrows = $(outwardgrid).jqGrid('getGridParam', 'data');

        if (invoiceNo == "") {
            Notify_Validation("Invoice number is required.");
            return;
        }
        if (customerInformation == "") {
            Notify_Validation("Customer Information is required.");
            return;
        }
        if (invoiceDate == "") {
            Notify_Validation("Invoice date is required.");
            return;
        }
      
       
        if (gridrows.length == 0) {
            Notify_Validation("Outward material is required to add.");
            return;
        }

        $.ajax({
            url: relativepath + '/Outward/InvoiceNumberExist?invoiceNumber=' + invoiceNo + "&outwardId=" + outwardId,
            type: "GET",
            success: function (result) {
                if (result.response) {
                    Notify_Validation("Invoice number is already exists.");
                }
                else {
                    outwardSave();
                }
            }
        });

        function outwardSave() {
        
            $.each(gridrows, function (key, rowobj) {

                var selrowobj = {
                    OutwardStackDetailId: rowobj.OutwardStackDetailId,
                    OutwardId: rowobj.OutwardId,
                    StockId: rowobj.StockId,
                    MaterialId: rowobj.MaterialId,
                    TypeId: rowobj.TypeId,
                    ItemId: rowobj.ItemId,
                    InwardAvailableQuantity: rowobj.AvailableQuantity,
                    OutwardQuantity: rowobj.OutwardQuantity,

                };
                outwardresultist.push(selrowobj);


            });

            var fileUpload = $("#lrcopyattachment").get(0);
            var files = fileUpload.files;

            // Create FormData object  
            var fileData = new FormData();

            // Looping over all files and add it to FormData object  
            for (var i = 0; i < files.length; i++) {
                fileData.append(files[i].name, files[i]);
            }
            fileData.append("outwardId", outwardId);
            fileData.append("InvoiceNumber", invoiceNo);
            fileData.append("InvoiceDate", invoiceDate);
            fileData.append("CustomerInformation", customerInformation);
            fileData.append("Gridlist", JSON.stringify(outwardresultist));

            $.ajax({
                url: relativepath + '/Outward/SaveOutwardStack',
                type: "POST",
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fileData,
                success: function (result) {
                    $("#lrcopyattachment").val('');
                    $("#invoiceNumber").val('');
                    $("#invoiceDate").val('');
                    $('#customerInformation').val('')
                    $('#divattachment').empty();
                    outwardgridlist = [];
                    $(outwardgrid).clearGridData();
                    $(outwardgrid).trigger('reloadGrid');

                    Notify_Success();

                    if (outwardId > 0) {
                        setTimeout(function () {
                            window.location = relativepath + "Outward/Index";
                        }, 2000);

                    }


                },
                error: function (err) {
                    Notify_Validation(err.statusText);
                }
            });
        }


    });


});

function DeleteAttachment(id) {


    var cfm = confirm("Are you sure you want to delete this file ?");
    if (cfm == true) {

        $.ajax({
            url: relativepath + '/Outward/DeleteAttachmentByAttachmentId',
            type: "POST",
            data: { "id": id },
            success: function (result) {
                if (result.success) {
                    $("#div_" + id).remove();
                    Notify_Message('Attachment deleted successfully.')
                }

            },

        });
    }

}



function InvoiceNumberValidation(invoiceNumber) {

    $.ajax({
        url: relativepath + '/Outward/InvoiceNumberExist?invoiceNumber=' + invoiceNumber,
        type: "GET",
        success: function (result) {
            if (result.response) {
                Notify_Validation("Invoice number is already exists.");
                return true;
            }
            else {
                return false;
            }
        }
    });


}