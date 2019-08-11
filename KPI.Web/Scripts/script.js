$(document).ready(function () {
    $('.monthly').datetimepicker({
        format: 'MM/DD/YYYY'
    });
    $('.quaterly').datetimepicker({
        format: 'MM/DD/YYYY'
    });
    $('.yearly').datetimepicker({
        format: 'MM/DD/YYYY'
    });
    $('#ChangePassword .username').val($('#user').text().trim());
    $('#btnChangePassword').off('click').on('click', function () {
        if ($('#ChangePassword .username').val().trim() === "") {
            Swal.fire({
                title: 'Warning!',
                text: 'Please enter password!',
                type: 'warning'
            });
            return;
        }
            var  username= $('#ChangePassword .username').val();
            var  password= $('#ChangePassword .password').val();
        $.post("/AdminUser/ChangePassword", { username: username, password: password }, function (result) {
            if (result) {
                Swal.fire({
                    title: 'Success!',
                    text: 'Change password successfully!',
                    type: 'success'
                });
                $('#modal-group-change-password').modal('hide');
            }
            else {
                Swal.fire({
                    title: 'Error!',
                    text: 'Error!',
                    type: 'error'
                });
            }
        });


    });
});



