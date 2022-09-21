$('#home').addClass('active');
var chartOption = {
    responsive: true,
    title: {
        display: true,
        text: 'Stock'
    },
    tooltips: {
        mode: 'index',
    },
    hover: {
        mode: 'index'
    },
    scales: {
        xAxes: [{
            scaleLabel: {
                display: true,
                labelString: 'Products'
            }
        }],
        yAxes: [{
            stacked: false,
            ticks: {
                beginAtZero: true,
                min: 0,
            },
            scaleLabel: {
                display: true,
                labelString: 'Value'
            },
            gridLines: {
                offsetGridLines: true
            }
        }]
    }
}
$(document).ready(function () {
    BindBalanceChart();
    $('input[type=radio][name=chartType]').change(function () {
        if (this.value == 'Available') {
            BindBalanceChart();
        }
        else if (this.value == 'Transaction') {
            $('#chartfilterdive').show();
            BindTransactionChart(null, null)
        }
    });
    $('#btnChartSearch').click(function () {
        var fromDate = $('#FromDate').val();
        var toDate = $('#ToDate').val();


        if (DateValidationSearch()) {
            BindTransactionChart(fromDate, toDate);
        }

    });

});

function BindBalanceChart() {
    Chartdivcreation();
    $('#chartfilterdive').hide();
    $.ajax({
        url: relativepath + '/Home/GetAvailableChartDetail',
        type: "GET",
        success: function (result) {
            console.log(result.Label);
            console.log(result.Data[0]);

            var config = {
                type: 'line',
                data: {
                    labels: result.Label,
                    datasets: [{
                        label: 'Available',
                        borderColor: "#3b81aa",
                        backgroundColor: "#4b94bf",
                        data: result.Data[0],

                    },

                    ]
                },
                options: chartOption,
            };
            var ctx = document.getElementById('canvas').getContext('2d');
            window.myLine = new Chart(ctx, config);

        },
        error: function (err) {
            alert(err.statusText);
        }
    });



}

function BindTransactionChart(fromDate, toDate) {
    Chartdivcreation();
    $.ajax({
        url: relativepath + '/Home/GetInandOutwardChartDetail?fromDate=' + fromDate + '&toDate=' + toDate,
        type: "GET",
        success: function (result) {
            console.log(result)
            var config = {
                type: 'line',
                data: {
                    labels: result.Label,
                    datasets: [
                        {
                            label: 'Inward',
                            borderColor: "#3b81aa",
                            backgroundColor: "#4b94bf",
                            data: result.Data[0],

                        },
                        {
                            label: 'Outward',
                            borderColor: "#b6bbc4",
                            backgroundColor: "#d2d6de",
                            data: result.Data[1],

                        },

                    ]
                },
                options: chartOption,
            };
            var ctx = document.getElementById('canvas').getContext('2d');
            window.myLine = new Chart(ctx, config);

        },
        error: function (err) {
            alert(err.statusText);
        }
    });
}

function Chartdivcreation() {
    $("#canvasdiv").empty();
    var content = '<canvas id="canvas"></canvas>';
    $("#canvasdiv").append(content);

}