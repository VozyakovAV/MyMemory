var as = as || {};

as.sys = {   
    init: function () {
        $(document).delegate('.fancy-close', 'click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            as.sys.closeDialog2();
        });
    },
    showDialog: function (title, html, buttonText, callback, callbackAfterLoadingWindow, callbackClose, window) {
        if (!window) window = $("#asModal");
        if ($('.modal:visible').length > 0) {
            window.css('zIndex', parseInt($('.modal:visible').css('zIndex')) + 5);
        }

        $('.modal-title', window).html(title);
        $('.modal-body', window).html(html);
        var btn = $('.modal-footer .btn-primary', window);
        var btnClose = $('.modal-footer button.btn-default', window);
        btn.html(buttonText);       
        if (!buttonText) btn.hide();
        else btn.show();
        if (callback) btn.off().click(callback);
        if (callbackClose) btnClose.off().click(callbackClose);

        window.modal({ show: true, backdrop: false });

        if (callbackAfterLoadingWindow) callbackAfterLoadingWindow();
    },
    closeDialog: function (window) {
        if (!window) window = $('#asModal');
        window.modal('hide');
    },
    ajaxSend: function (url, data, callback, btn) {
        var params = data;
        var txt = "";
        if (btn) {
            btn.addClass('disabled');
            btn.attr('disabled', 'disabled');
        }
        $.ajax({
            type: 'POST',
            url: url,
            cache: false,
            traditional: true,
            //dataType: "json",
            contentType: 'application/json; charset=utf-8',
            //data: JSON.stringify(params),
            data: params,
            success: function (data, status) {
                var response = data;
                if (data.d != undefined) response = data.d;
                if (typeof (response) != "object") response = eval('(' + response + ')');
                if (callback) callback(response);
            },
            complete: function () {
                if (btn) {
                    btn.removeClass('disabled');
                    btn.removeAttr('disabled');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (as.sys.logJSError) {
                    as.sys.logJSError(url + JSON.stringify(params) + ": " + textStatus + ", " + errorThrown);
                }
            },
            beforeSend: function (xhr) {
                xhr.setRequestHeader('Content-type', 'application/json; charset=utf-8');
            }
        });
    },
    logJSError: function (str) {
        console.log(as.sys.jsErrorPeriod);
        as.sys.jsErrorPeriod = as.sys.jsErrorPeriod || 500;
        as.sys.jsErrorPeriod = as.sys.jsErrorPeriod * 4;
        setTimeout(function () {
            as.sys.ajaxSend("/Plan/jsError", { s: str }, function () { });
        }, as.sys.jsErrorPeriod);
    },
    htmlEncode(value) {
        return $('<div/>').text(value).html();
},
htmlDecode(value) {
    return $('<div/>').html(value).text();
}
};