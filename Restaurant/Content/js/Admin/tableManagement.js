const form = document.getElementById("frmCreateOrUpdate");
$(document).ready(function () {
    // create
    $("#btnAdd").on("click", (event) => {
        event.preventDefault();
        $("#create-or-update-title").text("Add New Table");
        $("#tableModal").modal({
            show: true
        });
    });

    // create
    $("#btnAddZone").on("click", (event) => {
        event.preventDefault();
        $("#create-zone-title").text("Add New Zone");
        $("#zoneModal").modal({
            show: true
        });
    });
    
    const tableSize = $("#tableSize").val();
    if (tableSize > 0) {
        for (let i = 0; i < tableSize; i++) {
            // update
            $("#dataTable tbody").on("click", `#updateId${i}`, function () {
                const tableId = $(`#tableId${i}`).val();
                const tableName = $(`#tableName${i}`).val();
                const zoneId = $(`#zoneId${i}`).val();
                $("#create-or-update-title").text("Update Table");
                $("#tableId").val(tableId);
                $("#tableName").val(tableName);
                $("#select-zone-item").val(zoneId);
                $("#tableModal").modal({
                    show: true
                });
            });

            // delete
            $("#dataTable tbody").on("click", `#deleteId${i}`, function () {
                const tableDeleteId = $(`#tableId${i}`).val();
                $("#tableDeleteId").val(tableDeleteId);
                $("#deleteModal").modal({
                    show: true
                });
            });
        }
    };


    // confirm delete
    $("#btnConfirmDelete").on("click", (e) => {
        const tableId = $("#tableDeleteId").val();
        $.ajax(
                {
                    url: "/Admin/DeleteTable",
                    method: "post",
                    data: {
                        tableId,
                    },
                    complete: () => {
                        $("#deleteModal").modal({
                            show: false
                        });
                        location.reload();
                    }
                }
            );
    });
    

    // createOrUpdate Form handle
    $(form).on("submit", (event) => {
        $(".text-danger").text("");
        event.preventDefault();
        const tableId = $("#tableId").val();
        const tableName = $("#tableName").val();
        const zoneId = $("#select-zone-item").val();
        if (!form.checkValidity()) {
            event.stopPropagation();
            if (tableName === "") {
                $("#tableNameInvalid").text("Vui lòng nhập Tên bàn!");
            }
        } else {
            $.ajax(
                {
                    url: "/Admin/TableManagement",
                    method: "post",
                    data: {
                        tableId,
                        tableName,
                        zoneId
                    },
                    complete: () => {
                        $("#tableModal").modal({
                            show: false
                        });
                        location.reload();
                    }
                }
            );
        }
    });

    // createZone Form handle
    $(form).on("submit", (event) => {
        $(".text-danger").text("");
        event.preventDefault();
        const zoneName = $("#zoneName").val();
        const zoneCode = $("zoneCode").val();
        $.ajax(
            {
                url: "/Admin/ZoneManagement",
                method: "post",
                data: {
                    zoneName,
                    zoneCode
                },
                complete: () => {
                    $("#zoneModal").modal({
                        show: false
                    });
                    location.reload();
                }
            });
    });

    $(".btnCancel").on("click", (e) => {
        $(".text-danger").text("");
        $("#tableId").val("");
        $("#tableName").val("");
        $("#select-zone-item").val("KV1");
    });
})