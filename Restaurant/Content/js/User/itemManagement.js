
const length = $("#tableContainer .tableId").length;
$(document).ready(function () {
    for (let i = 0; i < length; i++) {
        $(`#table${i + 1}`).on("click", (event) => {
            //if ($(`#status${i + 1}`).val() === "true") {
            //    location.href = "/User/Detail?tableId=" + (i + 1);
            //}
            location.href = "/User/Detail?tableId=" + (i + 1);
        })
    }
    
})


