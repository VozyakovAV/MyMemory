function StudyTest(urlStartStudy, urlNextStep) {
    var _urlStartStudy = urlStartStudy;
    var _urlNextStep = urlNextStep;
    var _txtQuestion = $("#txtQuestion");
    var _inpAnswer = $("#inpAnswer");
    var _inpCorrectAnswer = $("#inpCorrectAnswer");
    var _txtMessage = $("#txtMessage");
    var _btnSubmit = $("#btnSubmit");
    var _groupId = 0;

    function StartStudy() {
        $.ajax({
            type: "POST", url: _urlStartStudy, data: { groupId: _groupId },
            success: function (response) {
                ShowResult(response);
            }
        });
    }

    function NextStep() {
        $.ajax({
            type: "POST", url: _urlNextStep, data: { answer: _inpAnswer.val() },
            success: function (response) {
                ShowResult(response);
            }
        });
    }

    function ShowResult(response) {
        console.log(response);

        if (response.Message != null) {
            ShowViewMessage(response);
        }
        else if (response.PrevAnswer != null) {
            if (response.PrevAnswer.IsCorrectAnswer) {
                ShowViewCorrectAnswer(response);
            }
            else {
                ShowViewIncorrectAnswer(response);
            }
        }
        else {
            ShowViewQuestion(response);
        }
    }

    this.Start = function () {
        //ShowViewQuestion();
        //_inpCorrectAnswer.prop("disabled", true);
        _inpAnswer.keyup(function (e) {
            if (e.keyCode == 13) {
                _btnSubmit.click();
            }
        });

        StartStudy();
    }

    _btnSubmit.click(function () {
        NextStep();
    });

    function ShowViewMessage(responce) {
        _txtQuestion.hide();
        _inpCorrectAnswer.hide();
        _inpAnswer.hide();
        _btnSubmit.hide();
        _txtMessage.html(response.Message).css("color", "black").show();
    }

    function ShowViewQuestion(response) {
        _txtQuestion.text(response.Question.Text);
        _inpCorrectAnswer.hide();
        _txtMessage.hide();
        _inpAnswer.prop("disabled", false).val("").focus();
        _btnSubmit.prop("disabled", false).html("Проверить").unbind().click(function () {
            NextStep();
        });
    }

    function ShowViewCorrectAnswer(response) {
        _txtQuestion.prop("disabled", true);
        _inpCorrectAnswer.hide();
        _txtMessage.html("Верно!").css("color", "green").show();
        _inpAnswer.prop("disabled", true);
        //_inpAnswer.parent().addClass("has-success");
        _btnSubmit.prop("disabled", true);
        /*_btnSubmit.html("Дальше").unbind().click(function () {
            ShowViewQuestion(response);
        });*/
        setTimeout(function () {
            ShowViewQuestion(response);
        }, 2000)
    }

    function ShowViewIncorrectAnswer(response) {
        _inpAnswer.prop("disabled", true);
        _inpCorrectAnswer.prop("disabled", true).val(response.PrevAnswer.CorrectAnswer).show();
        _txtMessage.html("Упс!").css("color", "red").show();
        _btnSubmit.prop("disabled", false).html("Дальше").unbind().click(function () {
            ShowViewQuestion(response);
        });
    }
}