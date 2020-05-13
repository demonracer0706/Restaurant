var start = moment();
var end = moment();
// Call the dataTables jQuery plugin
var oTable = $("#dataTable").DataTable({
    "language": {
        "zeroRecords": "Chưa có dữ liệu",
        "info": "Hiển thị _PAGE_ trên _PAGES_ trang",
        "infoEmpty": "Chưa có dữ liệu",
        "paginate": {
            "first": "First",
            "last": "Last",
            "next": ">",
            "previous": "<"
        },
        "search": "Tìm kiếm:",
    },
    "bLengthChange": false,
    "processing": true,
    "serverSide": true,
    "ajax": {
        "url": "/Admin/Index",
        "type": "POST",
        "datatype": "json",
        "data": function (data) {
            var startDate = $("#startDate").val() || "";
            var endDate = $("#endDate").val() || "";
            if (startDate === "") {
                startDate = start.format("DD/MM/YYYY");
            }
            if (endDate === "") {
                endDate = end.format("DD/MM/YYYY");
            }
            data.start = startDate;
            data.end = endDate;
        }
    },
    "columns": [
        {
            "data": "ID",
            "defaultContent": ""
        },
        {
            "data": "TEN_THUCPHAM",
            "defaultContent": ""
        },
        {
            "data": "LOAI",
            "defaultContent": ""
        },
        {
            "data": "SOLUONG",
            "defaultContent": ""
        },
        {
            "data": "DONGIA",
            "defaultContent": ""
        },
        {
            "data": "DOANHTHU",
            "defaultContent": ""
        },
    ]
});
function cb(start, end) {
    const startFormat = start.format("DD/MM/YYYY");
    const endFormat = end.format("DD/MM/YYYY");
    if (startFormat === moment().format("DD/MM/YYYY") && endFormat === moment().format("DD/MM/YYYY"))
    {
        $("#reportrange span").html("Hôm nay");
        return;
    }
    if (startFormat === moment().subtract(1, 'days').format("DD/MM/YYYY") && moment().subtract(1, 'days').format("DD/MM/YYYY")) {
        $("#reportrange span").html("Hôm qua");
        return;
    }
    if (startFormat === moment().subtract(6, 'days').format("DD/MM/YYYY") && moment().format("DD/MM/YYYY")) {
        $("#reportrange span").html("7 ngày qua");
        return;
    }
    if (startFormat === moment().startOf('month').format("DD/MM/YYYY") && moment().endOf('month').format("DD/MM/YYYY")) {
        $("#reportrange span").html("Tháng này");
        return;
    }
    if (startFormat === moment().subtract(1, 'month').startOf('month').format("DD/MM/YYYY")
        && moment().subtract(1, 'month').endOf('month').format("DD/MM/YYYY")) {
        $("#reportrange span").html("Tháng trước");
        return;
    }
    $("#reportrange span").html(start.format("DD/MM/YYYY") + ' - ' + end.format("DD/MM/YYYY"));
}
$('#reportrange').daterangepicker({
    startDate: start,
    endDate: end,
    "locale": {
        "format": "DD/MM/YYYY",
        "separator": " - ",
        "applyLabel": "Áp dụng",
        "cancelLabel": "Hủy",
        "fromLabel": "Từ",
        "toLabel": "Đến",
        "customRangeLabel": "Tùy chỉnh",
        "daysOfWeek": [
            "CN",
            "T2",
            "T3",
            "T4",
            "T5",
            "T6",
            "T7"
        ],
        "monthNames": [
            "Tháng 1",
            "Tháng 2",
            "Tháng 3",
            "Tháng 4",
            "Tháng 5",
            "Tháng 6",
            "Tháng 7",
            "Tháng 8",
            "Tháng 9",
            "Tháng 10",
            "Tháng 11",
            "Tháng 12",
        ],
        "firstDay": 1
    },
    ranges: {
        'Hôm nay': [moment(), moment()],
        'Hôm qua': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
        '7 ngày qua': [moment().subtract(6, 'days'), moment()],
        'Tháng này': [moment().startOf('month'), moment().endOf('month')],
        'Tháng trước': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
    }
}, cb);

cb(start, end);
$('#reportrange').on('apply.daterangepicker', function (ev, picker) {
    $("#startDate").val(picker.startDate.format("DD/MM/YYYY") + "");
    $("#endDate").val(picker.endDate.format("DD/MM/YYYY") + "");
    oTable.ajax.reload();
});

$("#reportrange").on('cancel.daterangepicker', function (ev, picker) {
    //do something, like clearing an input
    $("#reportrange").val("");
});

