const form = document.getElementById("frmCreateOrUpdate");
$(document).ready(function () {
    //open choose file dialog
    $("#btnChooseFile").on("click", (e) => {
        $("#image").trigger("click");
    })
    $("#image").on("change", (e) => {
        const image = e.target.files[0];
        if (image != undefined) {
            $("#image-preview").attr("src", URL.createObjectURL(image));
        }
    });
    // create
    $("#btnAdd").on("click", (event) => {
        event.preventDefault();
        $("#create-or-update-title").text("Add New Product");
        $("#menuId").val("")
        $("#menuModal").modal({
            show: true
        });
    });

    const menuSize = $("#menuSize").val();
    if (menuSize > 0) {
        for (let i = 0; i < menuSize; i++) {
            // update
            $("#dataTable tbody").on("click", `#updateId${i}`, function () {
                const menuId = $(`#menuId${i}`).val();
                const menuName = $(`#menuName${i}`).val();
                const typeId = $(`#typeId${i}`).val();
                const unitPrice = $(`#unitPrice${i}`).val();
                const imageUrl = $(`#imageUrl${i}`).val();
                $("#create-or-update-title").text("Update Product");
                $("#menuId").val(menuId);
                $("#menuName").val(menuName);
                $("#unitPrice").val(unitPrice);
                $("#image-preview").attr("src", imageUrl);
                $("#select-type-item").val(typeId);
                $("#menuModal").modal({
                    show: true
                });
            });

            // delete
            $("#dataTable tbody").on("click", `#deleteId${i}`, function () {
                const menuDeleteId = $(`#menuId${i}`).val();
                $("#menuDeleteId").val(menuDeleteId);
                $("#deleteModal").modal({
                    show: true
                });
            });
        }
    };

    // createOrUpdate Form handle
    $(form).on("submit", (event) => {
        $(".text-danger").text("");
        event.preventDefault();
        const menuId = $("#menuId").val();
        const menuName = $("#menuName").val();
        const typeId = $("#select-type-item").val();
        const unitPrice = $("#unitPrice").val();
        const file = document.getElementById("image").files[0];
        if (!form.checkValidity()) {
            event.stopPropagation();
            if (menuName === "") {
                $("#menuNameInvalid").text("Vui lòng nhập Tên món!");
            }
            if (unitPrice === "") {
                $("#unitPriceInvalid").text("Vui lòng nhập Đơn giá!");
                return;
            }
            if (unitPrice === "0") {
                $("#unitPriceInvalid").text("Đơn giá phải lớn hơn 0!");
            }
        } else {
            if (file != null) {
                const formdata = new FormData(); //FormData object
                formdata.append(file.name, file);
                $.ajax(
                {
                    url: "/Upload/Upload",
                    method: "post",
                    processData: false,
                    contentType: false,
                    data: formdata,
                    complete: (data) => {
                        $.ajax({
                            url: "/Admin/MenuManagement",
                            method: "post",
                            data: {
                                menuId,
                                menuName,
                                typeId,
                                unitPrice,
                                fileUrl: data.responseText
                            },
                            complete: () => {
                                $("#menuModal").modal({
                                    show: false
                                });
                                location.reload();
                            }
                        });
                    }
                });
            }
            else {
                $.ajax(
                {
                    url: "/Admin/MenuManagement",
                    method: "post",
                    data: {
                        menuId,
                        menuName,
                        typeId,
                        unitPrice,
                        fileUrl: ""
                    },
                    complete: () => {
                        $("#menuModal").modal({
                            show: false
                        });
                        location.reload();
                    }
                }
            );
            }

        }
    });

    // confirm delete
    $("#btnConfirmDelete").on("click", (e) => {
        const menuId = $("#menuDeleteId").val();
        $.ajax(
                {
                    url: "/Admin/DeleteMenu",
                    method: "post",
                    data: {
                        menuId,
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

    // validate for unitPrice
    $("#unitPrice").on("keydown", (e) => {
        if (!((e.keyCode > 95 && e.keyCode < 106)
      || (e.keyCode > 47 && e.keyCode < 58)
      || e.keyCode == 8)) {
            return false;
        }
    });

    $(".btnCancel").on("click", (e) => {
        $(".text-danger").text("");
        $("#menuId").val("");
        $("#select-type-item").val("MA");
        $("#menuName").val("");
        $("#unitPrice").val("");
        $("#image-preview").attr("src", "/Content/Images/Menu/noImage.jpg");
    });
})