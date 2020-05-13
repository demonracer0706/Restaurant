const length = $("#tableContainer .tableId").length;
$(document).ready(function () {
    for (let i = 0; i < length; i++) {
        $(`#table${i + 1}`).on("click", (event) => {
            //if ($(`#status${i + 1}`).val() === "true") {
            //    location.href = "/Counter/Detail?tableId=" + (i + 1);
            //}
            location.href = "/Counter/Detail?tableId=" + (i + 1);
        })
    }
    $("#btn-payment").on("click", (event) => {
        const tableId = $("#tableId").val();
        window.open("/Counter/PrintBill?tableId=" + tableId, "_blank");
        //location.href = "/Counter/PrintBill?tableId=" + tableId;
    });
})