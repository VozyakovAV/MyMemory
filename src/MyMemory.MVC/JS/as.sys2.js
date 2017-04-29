var as = as || {};

as.sys2 = {
    init: function () {
        $.ajaxSetup({ cache: false });
        as.sys2.initDialogCreate();
        as.sys2.initDialogEdit();
        as.sys2.initDialogDelete();
        as.sys2.initDialogConfirm();
    },
    initDialogCreate: function () {
        $(".dialog-create").on("click", function (e) {
            e.preventDefault();
            var url = this.href;

            $.get(url, function (data) {
                var html = $(data);
                as.sys.showDialog("Создание", html, "Ok", function (e) {
                    var params = $("form", html).serialize();
                    $.ajax({
                        type: "POST",
                        url: url,
                        data: params,
                        success: function (response) {
                            console.log(response);
                            as.sys.closeDialog();
                            location.reload();
                        }
                    });
                });

            });
        });
    },
    initDialogEdit: function () {
        $(".dialog-edit").on("click", function (e) {
            e.preventDefault();
            var url = this.href;
            var taskID = $(this).data("id");

            $.get(url, { id: taskID }, function (data) {
                var html = $(data);
                as.sys.showDialog("Редактирование", html, "Ok", function (e) {
                    var params = $("form", html).serialize();
                    $.ajax({
                        type: "POST",
                        url: url,
                        data: params,
                        success: function (response) {
                            console.log(response);
                            as.sys.closeDialog();
                            location.reload();
                        }
                    });
                });

            });
        });
    },
    initDialogDelete: function () {
        $(".dialog-delete").on("click", function (e) {
            e.preventDefault();
            var url = this.href;
            var text = this.text;
            var taskID = $(this).data("id");

            as.sys.showDialog("Удаление", "Удалить?", "Ok", function (e) {
                var params = { id: taskID };
                $.ajax({
                    type: "POST",
                    url: url,
                    data: params,
                    success: function (response) {
                        console.log(response);
                        as.sys.closeDialog();
                        location.reload();
                    }
                });
            });
        });
    },
    initDialogConfirm: function () {
        $(".dialog-confirm").on("click", function (e) {
            e.preventDefault();
            if (confirm("Вы уверены?")) {
                var url = this.href;
                var text = this.text;
                var taskID = $(this).data("id");

                var params = { id: taskID };
                $.ajax({
                    type: "POST",
                    url: url,
                    data: params,
                    success: function (response) {
                        console.log(response);
                        as.sys.closeDialog();
                        location.reload();
                    }
                });
            }
        });
    }
};