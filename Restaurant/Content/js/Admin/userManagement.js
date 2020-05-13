const form = document.getElementById("frmCreateOrUpdate");
// datettimepicker
$("#datepicker").datetimepicker({
    format: "DD/MM/YYYY",
    locale: "vi"
});
// fixed format dd/mm/yyyy
function formatDate(string) {
    const fullDateTime = string.split(" ");
    const fullDate = fullDateTime[0].split("/", 3);
    const date = new Date(fullDate[2], fullDate[0], fullDate[1]);
    var mm = date.getMonth();
    var dd = date.getDate();
    return (dd > 9 ? '' : '0') + dd + "/" + (mm > 9 ? '' : '0') + mm + "/" + date.getFullYear();
}

$(document).ready(function () {
    // create
    $("#btnAdd").on("click", (event) => {
        event.preventDefault();
        $("#create-or-update-title").text("Add New User");
        $("#userModal").modal({
            show: true
        });
    });

    const userSize = $("#userSize").val();
    if (userSize > 0) {
        for (let i = 0; i < userSize; i++) {
            // update
            $("#dataTable tbody").on("click", `#updateId${i}`, function () {
                const fullname = $(`#fullname${i}`).val();
                const roleId = $(`#roleId${i}`).val();
                const gender = $(`#gender${i}`).val();
                const birthday = $(`#birthday${i}`).val();
                const phone = $(`#phone${i}`).val();
                const address = $(`#address${i}`).val();
                const username = $(`#username${i}`).val();
                const password = $(`#password${i}`).val();
                $("#create-or-update-title").text("Update User");
                $("#fullname").val(fullname);
                $("#select-role-item").val(roleId);
                $("#gender").val(gender);
                $("#birthday").val(birthday);
                $("#phone").val(phone);
                $("#address").val(address);
                $("#username").val(username);
                $("#username").attr("disabled", "disabled");
                $("#password").val(password);
                $("#userModal").modal({
                    show: true
                });
            });

            // delete
            $("#dataTable tbody").on("click", `#deleteId${i}`, function () {
                const userDeleteId = $(`#username${i}`).val();
                $("#userDeleteId").val(userDeleteId);
                $("#deleteModal").modal({
                    show: true
                });
            });

            const date = $(`#dateFormat${i}`).text();
            const dateFormat = formatDate(date);
            $(`#dateFormat${i}`).text(dateFormat);
            $(`#birthday${i}`).val(dateFormat);
        }
    };


    // confirm delete
    $("#btnConfirmDelete").on("click", (e) => {
        const username = $("#userDeleteId").val();
        $.ajax(
                {
                    url: "/Admin/DeleteUser",
                    method: "post",
                    data: {
                        username,
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
        const fullname = $("#fullname").val();
        const roleId = $("#select-role-item").val();
        const gender = $("#gender").val();
        const birthday = $("#birthday").val();
        const phone = $("#phone").val();
        const address = $("#address").val();
        const username = $("#username").val();
        const password = $("#password").val();
        if (!form.checkValidity()) {
            event.stopPropagation();
            if (fullname === "") {
                $("#fullnameInvalid").text("Vui lòng nhập Họ tên!");
            }
            if (gender === "") {
                $("#genderInvalid").text("Vui lòng nhập Giới tính!");
            }
            if (birthday === "") {
                $("#birthdayInvalid").text("Vui lòng nhập Ngày sinh!");
            }
            if (phone === "") {
                $("#phoneInvalid").text("Vui lòng nhập Số điện thoại!");
            }
            if (address === "") {
                $("#addressInvalid").text("Vui lòng nhập Địa chỉ!");
            }
            if (username === "") {
                $("#usernameInvalid").text("Vui lòng nhập tên đăng nhập!");
            }
            if (password === "") {
                $("#passwordInvalid").text("Vui lòng nhập Mật khẩu!");
            }
        } else {
            $.ajax(
                {
                    url: "/Admin/UserManagement",
                    method: "post",
                    data: {
                        fullname,
                        roleId,
                        gender,
                        birthday,
                        phone,
                        address,
                        username,
                        password
                        },
                    complete: () => {
                        $("#userModal").modal({
                            show: false
                        });
                        location.reload();
                    }
                }
            );
        }
    });

    // validate for phone number
    $("#phone").on("keydown", (e) => {
        if (!((e.keyCode > 95 && e.keyCode < 106)
      || (e.keyCode > 47 && e.keyCode < 58)
      || e.keyCode == 8)) {
            return false;
        }
    });

    $(".btnCancel").on("click", (e) => {
        $(".text-danger").text("");
        $("#fullname").val("");
        $("#select-role-item").val("NV");
        $("#gender").val("");
        $("#birthday").val("");
        $("#phone").val("");
        $("#address").val("");
        $("#username").val("");
        $("#username").removeAttr("disabled");
        $("#password").val("");
    });
})

