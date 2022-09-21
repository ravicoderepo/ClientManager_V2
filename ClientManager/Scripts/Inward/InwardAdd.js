$('#inward').addClass('active');
$(document).ready(function () {

    $("#MaterialId").change(function () {
        var selMaterial = $('option:selected', this).val();
        if (selMaterial != "" && selMaterial != "Select") {
            $.ajax({
                url: relativepath + '/Inward/GetTypelist?id=' + selMaterial,
                type: "POST",
                success: function (result) {
                    var typeselect = $('#TypeId');

                    typeselect.empty();
                    typeselect.append($('<option></option>').html("Select"));

                    var itemselect = $('#ItemId');
                    itemselect.empty();
                    itemselect.append($('<option></option>').html("Select"));


                    $.each(result, function (key, value) {
                        typeselect.append($('<option></option>').val(value.Value).html(value.Text));
                    });

                },

            });
        }
    });

    $("#TypeId").change(function () {
        var selType = $('option:selected', this).val();
        console.log(selType);
        if (selType != "" && selType != "Select") {
            $.ajax({
                url: relativepath + '/Inward/GetItemlist?id=' + selType,
                type: "POST",
                success: function (result) {
                    var typeselect = $('#ItemId');
                    typeselect.empty();
                    typeselect.append($('<option></option>').html("Select"));
                    $.each(result, function (key, value) {
                        typeselect.append($('<option></option>').val(value.Value).html(value.Text));
                    });

                },

            });
        }
    });
});