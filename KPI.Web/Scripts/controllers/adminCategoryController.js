
$(document).ready(function () {
    adminCategoryController.init();
    $("#addKPI .parent-search .search-box").hide();
});
var config = {
    pageSize: 3,
    pageIndex: 1
};
var adminCategoryController = {
    init: function () {
        adminCategoryController.loadData();
        adminCategoryController.registerEvent();
    },
    registerEvent: function () {
        $('#search').off('keypress').on('keypress', function (e) {
            if (e.which === 13) {
                adminCategoryController.loadData(true);
            }
        });
        $('#table-group select').off('change').on('change', function (e) {
            adminCategoryController.loadData(true);
        });
        //Save KPI
        $('#btnAdd').off('click').on('click', function () {
            adminCategoryController.addData();

        });
        //Delete
        $('#btnDelete').off('click').on('click', function () {

        });
        //Update
        $('.btnUpdate').off('click').on('click', function () {
            //load detail
            adminCategoryController.loadDetail(Number($(this).data('id')));
        });
        //save
        $('#btnSaveUpdateModal').off('click').on('click', function () {
            adminCategoryController.updateData();
        });
        //delete
        $('.btnDelete').off('click').on('click', function () {
            adminCategoryController.deleteData(Number($(this).data('id')));
        });
        //$('#addKPI #addKPI').typeahead({
        //    ajax: '/AdminCategory/Autocomplete'
        //});
        $('#addKPI .search').focusout(function () {
            $("#addKPI .parent-search .search-box").hide();
        });
        $("#addKPI .search").off('keyup').on('keyup', function (e) {

            $.ajax({
                type: "POST",
                url: "/AdminCategory/Autocomplete",
                data: 'name=' + $(this).val(),
                success: function (data) {
                    console.log(data);
                    if (data.length !== 0) {
                        $("#addKPI .parent-search .search-box").show();
                        $("#addKPI .parent-search .search-box ul").empty();
                        $.each(data, function (i, item) {
                            $("#addKPI .parent-search .search-box ul").append("<li>" + item + "</li>");

                        });
                    }
                }

            });

        });
    },
    log: function (message) {
        $("<div>").text(message).prependTo("#log");
        $("#log").scrollTop(0);
    },
    addData: function () {
        var res = adminCategoryController.validate();
        if (res === false) {
            return false;
        }
        var mObj = {
            Code: $('#addKPI .Code').val(),
            Name: $('#addKPI .Name').val(),
            LevelID: $('#addKPI .LevelID').val(),
            CategoryID: $('#addKPI .Category').val()
        };
        $.ajax({
            url: "/AdminCategory/Add",
            data: JSON.stringify(mObj),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result) {
                    Swal.fire({
                        title: 'Success!',
                        text: 'Add successfully!',
                        type: 'success',
                        confirmButtonText: 'OK'
                    });
                    $("#modal-group").modal('hide');
                    adminCategoryController.loadData(true);
                    adminCategoryController.resetForm();
                }
                else {
                    Swal.fire({
                        title: 'Error!',
                        text: 'This code has already existed!',
                        type: 'error'
                    });
                }

            },
            error: function (errormessage) {
                console.log(errormessage.responseText);
            }
        });
    },
    deleteData: function (id) {
        var ID = id;
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.value) {
                $.ajax({
                    url: "/AdminCategory/Delete/" + ID,
                    type: "POST",
                    contentType: "application/json;charset=UTF-8",
                    dataType: "json",
                    success: function (result) {
                        Swal.fire({
                            title: 'Success!',
                            text: 'Delete successfully.',
                            type: 'success'
                        });
                        adminCategoryController.loadData();
                        $("#modal-group").modal('hide');
                    },
                    error: function (errormessage) {
                        console.log(errormessage.responseText);
                    }
                });
                Swal.fire(
                    'Deleted!',
                    'Reacord has been deleted.',
                    'success'
                );
            }
        });

    },
    resetForm: function () {
        $('.ID').val("");
        $('.Name').val("");
        $('.Code').val("");
        $('.LevelID').val("");
        $('.Category').val("");
        $('.Name').css('border-color', 'lightgrey');
        $('.Code').css('border-color', 'lightgrey');
        $('.LevelID').css('border-color', 'lightgrey');
    },
    updateData: function () {
        var mObj = {
            ID: $('#modal-group2 .ID').val(),
            Name: $('#modal-group2 .Name').val(),
            Code: $('#modal-group2 .Code').val(),
            LevelID: $('#modal-group2 .LevelID').val()
        };

        $.ajax({
            url: "/AdminCategory/Update",
            data: JSON.stringify(mObj),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {
                if (result) {
                    Swal.fire({
                        title: 'Success!',
                        text: 'Update successfully!',
                        type: 'success'
                    });
                    adminCategoryController.resetForm();
                    $("#modal-group2").modal('hide');
                    adminCategoryController.loadData();

                }

            },
            error: function (error) {
                console.log(error);
            }
        });
    },
    loadDetail: function (id) {
        var value = id;
        $.ajax({
            url: "/AdminCategory/GetbyID/",
            data: { ID: JSON.stringify(value) },
            type: "GET",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                $('#modal-group2 .ID').val(result.ID);
                $('#modal-group2 .Name').val(result.Name);
                $('#modal-group2 .Code').val(result.Code);
                $('#modal-group2 .LevelID').val(result.LevelID);
                $('#modal-group2').modal('show');

            },
            error: function (err) {
                console.log(err);
            }
        });
    },

    validate: function () {
        var isValid = true;
        if ($('.Name').val().trim() === "") {
            $('.Name').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('.Name').css('border-color', 'lightgrey');
        }
        if ($('.Code').val().trim() === "") {
            $('.Code').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('.Code').css('border-color', 'lightgrey');
        }
        if ($('.LevelID').val().trim() === "") {
            $('.LevelID').css('border-color', 'Red');
            isValid = false;
        }
        else {
            $('.LevelID').css('border-color', 'lightgrey');
        }
        return isValid;
    },
    loadData: function (changePageSize) {
        var name = $('#search').val();
        $.ajax({
            url: '/AdminCategory/LoadData',
            type: 'GET',
            data: {
                name: name,
                page: config.pageIndex,
                pageSize: config.pageSize
            },
            dataType: 'json',
            beforeSend: function () {
                $("#main-loading-delay").show();
            },
            success: function (response) {
                if (response.status) {
                    $("#main-loading-delay").hide();
                    var count = 1;
                    //console.log(response.data);
                    var data = response.data;
                    var html = '';
                    var template = $('#tblkpi-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            No: count,
                            Name: item.Name,
                            Code: item.Code,
                            Level: item.LevelID,
                            ID: item.ID,
                            IDDelete: item.ID
                        });
                        count++;
                    });
                    $('#tblkpi').html(html);
                    adminCategoryController.paging(response.total, function () {
                        adminCategoryController.loadData();
                    }, changePageSize);
                    adminCategoryController.registerEvent();
                }
            },
            error: function (err) {
                console.log(err);
            }
        });
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / config.pageSize);

        //Unbind pagination if it existed or click change pagesize
        if ($('#pagination a').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData("twbs-pagination");
            $('#pagination').unbind("page");
        }

        $('#pagination').twbsPagination({
            totalPages: totalPage === 0 ? 1 : totalPage,
            first: "First",
            next: "Next",
            last: "Last",
            prev: "Previous",
            visiblePages: 10,
            onPageClick: function (event, page) {
                config.pageIndex = page;
                setTimeout(callback, 500);
            }
        });
    }
};
