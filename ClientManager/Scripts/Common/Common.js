//var jsonreader = {
//    root: "rows",
//    page: "page",
//    total: "total",
//    records: "records",
//    repeatitems: true,
//    cell: "cell",
//    id: "id",
//    userdata: "userdata",
//    subgrid: {
//        root: "rows",
//        repeatitems: true,
//        cell: "cell"
//    }
//}; 


var jqgridDateFormat = "m/d/Y";
var calendardateformat = "yy-mm-dd";


var jsonreader = {
    root: "rows",
    page: "page",
    total: "total",
    records: "records",
    repeatitems: false,
    userdata: "userdata"
};

var modifySearchingFilter = function (separator) {
    var i, l, rules, rule, parts, j, group, str,
        filters = $.parseJSON(this.p.postData.filters);
    if (filters && typeof filters.rules !== 'undefined' && filters.rules.length > 0) {
        rules = filters.rules;
        for (i = 0; i < rules.length; i++) {
            rule = rules[i];
            if (rule.op === 'cn') {
                // make modifications only for the 'contains' operation
                parts = rule.data.split(separator);
                if (parts.length > 1) {
                    if (typeof filters.groups === 'undefined') {
                        filters.groups = [];
                    }
                    group = {
                        groupOp: 'OR',
                        groups: [],
                        rules: []
                    };
                    filters.groups.push(group);
                    for (j = 0, l = parts.length; j < l; j++) {
                        str = parts[j];
                        if (str) {
                            // skip empty '', which exist in case of two separaters of once
                            group.rules.push({
                                data: parts[j],
                                op: rule.op,
                                field: rule.field
                            });
                        }
                    }
                    rules.splice(i, 1);
                    i--; // to skip i++
                }
            }
        }
        console.log(JSON.stringify(filters));
        this.p.postData.filters = JSON.stringify(filters);
    }
};

$(document).ready(function () {
    $('form').submit(function () {
        if ($(this).valid()) {
            loadingshow();
        }
    });
});

$(document).ajaxStart(function () {
    loadingshow();
});
$(document).ajaxComplete(function () {
    loadinghide();
});

function loadingshow() {
    $('body').waitMe({
        effect: 'win8_linear',
        text: 'Please wait...',
        bg: 'rgba(255,255,255,0.7)',
        color: '#000'
    });
}
function loadinghide() {
    $('body').waitMe('hide');
}

function DateValidationSearch() {

    if ($('#FomDate').val() != "" && $('#ToDate').val() != "") {
        if ($('#FromDate').val() <= $('#ToDate').val()) {
            return true;
        } else {
            Notify_Validation('To Date should be Greater than From Date!');
            return false;
        }
    }
    return true;
}

function Notify_Success() {
    $.notify({
        title: '<strong>Success!</strong>',
        message: 'Record saved successfully.'
    }, {
        type: 'success',
        z_index: 1050,
        placement: {
            from: 'top',
            align: 'right'
        }

    });
}

function Notify_Message(msg) {
    $.notify({
        title: '<strong>Success!</strong>',
        message: msg,
    }, {
        type: 'success',
        z_index: 1050,
        placement: {
            from: 'top',
            align: 'right'
        }

    });
}

function Notify_Validation(msg) {
    $.notify({
        title: '<strong>Warning!</strong>',
        message: msg,
    }, {
        type: 'danger',
        z_index: 1050,
        placement: {
            from: 'top',
            align: 'right',

        }

    });
}