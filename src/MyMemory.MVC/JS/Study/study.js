function StudyTest(urlStartStudy, urlNextStep) {
    var _urlStartStudy = urlStartStudy;
    var _urlNextStep = urlNextStep;
    var _txtQuestion = $("#txtQuestion");
    var _inpAnswer = $("#inpAnswer");
    var _inpCorrectAnswer = $("#inpCorrectAnswer");
    var _txtMessage = $("#txtMessage");
    var _btnSubmit = $("#btnSubmit");
    var _statCorrect = $("#statCorrect");
    var _statIncorrect = $("#statIncorrect");
    var _statStepNumber = $("#statStepNumber");
    var _groupId = 0;
    var _currentAnswerMD5;

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
        else if (response.PrevStep.Answer != null) {
            if (response.PrevStep.Answer.IsCorrect) {
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
        _inpAnswer.on('input', function () {
            CheckAnswerHash();
        });

        StartStudy();
    }

    _btnSubmit.click(function () {
        NextStep();
    });

    function ShowViewMessage(response) {
        ResetStylesControls();
        _txtMessage.html(response.Message).css("color", "black").show();
    }

    function ShowViewQuestion(response) {
        _currentAnswerMD5 = response.Step.Answer.CorrectAnswerMD5;
        ResetStylesControls();
        _txtQuestion.text(response.Step.Question.Text).show();
        _inpAnswer.val("").show().focus();
        _statCorrect.html(response.Statistic.NumberOfCorrect);
        _statIncorrect.html(response.Statistic.NumberOfIncorrect);
        _statStepNumber.html(response.Step.Question.StepNumber);
        _btnSubmit.html("Проверить").show().click(function () {
            NextStep();
        });
    }

    function ShowViewCorrectAnswer(response) {
        ResetStylesControls();
        _txtQuestion.show();
        _inpAnswer.prop("disabled", true).show();
        _inpAnswer.parent().addClass("has-success");
        _txtMessage.html("Верно!").css("color", "green").show();
        _btnSubmit.prop("disabled", true).show();
        setTimeout(function () {
            ShowViewQuestion(response);
        }, 1000)
    }

    function ShowViewIncorrectAnswer(response) {
        ResetStylesControls();
        _inpAnswer.prop("disabled", true).show();
        _inpAnswer.parent().addClass("has-error");
        _inpCorrectAnswer.prop("disabled", true).val(response.PrevStep.Answer.CorrectAnswer).show();
        _inpCorrectAnswer.parent().addClass("has-success");
        _txtMessage.html("Упс!").css("color", "red").show();
        _btnSubmit.html("Дальше").show().click(function () {
            ShowViewQuestion(response);
        });
    }

    function ResetStylesControls() {
        _txtQuestion.prop("disabled", false).hide();
        _inpAnswer.prop("disabled", false).hide();
        _inpCorrectAnswer.prop("disabled", false).hide();
        _txtMessage.prop("disabled", false).css("color", "black").hide();
        _btnSubmit.prop("disabled", false).unbind().hide();
        _inpAnswer.parent().removeClass("has-success").removeClass("has-error");
        _inpCorrectAnswer.parent().removeClass("has-success").removeClass("has-error");
    }

    function CheckAnswerHash() {
        var text = _inpAnswer.val();
        var MD5 = CryptoJS.MD5(text).toString(CryptoJS.enc.Hex);

        if (MD5 == _currentAnswerMD5) {
            NextStep();
        }
    }
}