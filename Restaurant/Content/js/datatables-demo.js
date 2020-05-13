// Call the dataTables jQuery plugin
$(document).ready(function() {
    $("#dataTable").DataTable({
        "language": {
            "lengthMenu": "Hiển thị _MENU_ người dùng trên 1 trang",
            "zeroRecords": "No Data",
            "info": "Hiển thị _PAGE_ trên _PAGES_ trang",
            "infoEmpty": "No Data",
            "paginate": {
                "first": "First",
                "last": "Last",
                "next": ">",
                "previous": "<"
            },
            "search": "Search:",
        },
        "bLengthChange": false,
    });
});
