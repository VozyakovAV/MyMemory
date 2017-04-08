function StudyTestLetters() {

    var _content = $("#blockTestLetters");
    var _txtQuestion = _content.find(".txtQuestion");
    var _inpAnswer = _content.find(".inpAnswer");
    var _inpCorrectAnswer = _content.find(".inpCorrectAnswer");
    var _txtMessage = _content.find(".txtMessage");
    var _btnSubmit = _content.find(".btnSubmit");
    var _divGroupName = _content.find(".divGroupName");
    var _currentAnswerMD5;

    // ----------------------------------

    _inpAnswer.keyup(function (e) {
        if (e.keyCode == 13) {
            _btnSubmit.click();
        }
    });

    this.Show = function (response) {
        ShowResult(response);
    }

    function ShowResult(response) {
        if (response.PrevStep.Answer != null) {
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

    function NextStep() {
        StylesForNextStep();
        var event = new CustomEvent('answerEvent', { 'detail': _inpAnswer.val() });
        document.dispatchEvent(event);
    }

    function ShowViewMessage(response) {
        ResetStylesControls();
        _txtMessage.html(response.Message).css("color", "black").show();
    }

    function ShowViewQuestion(response) {
        ResetStylesControls();
        _currentAnswerMD5 = response.Step.Answer.CorrectAnswerMD5;
        _txtQuestion.text(response.Step.Question.Text).show();
        _inpAnswer.val("").show().focus();
        _divGroupName.html(response.Step.Question.GroupName).show();
        _btnSubmit.html("Проверить").show().click(function () {
            NextStep();
        });
        _inpAnswer.on('input', function () {
            CheckAnswerHash();
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

    function StylesForNextStep() {
        _txtQuestion.prop("disabled", true);
        _inpAnswer.prop("disabled", true).unbind();
        _inpCorrectAnswer.prop("disabled", true);
        _btnSubmit.prop("disabled", true).unbind();
    }

    function ResetStylesControls() {
        _txtQuestion.prop("disabled", false).hide();
        _inpAnswer.prop("disabled", false).hide();
        _inpCorrectAnswer.prop("disabled", false).hide();
        _txtMessage.prop("disabled", false).css("color", "black").hide();
        _btnSubmit.prop("disabled", false).unbind().hide();
        _inpAnswer.parent().removeClass("has-success").removeClass("has-error").unbind();
        _inpCorrectAnswer.parent().removeClass("has-success").removeClass("has-error");
    }

    function CheckAnswerHash() {
        var text = _inpAnswer.val().toString().trim().toLowerCase();
        var MD5 = CryptoJS.MD5(text).toString(CryptoJS.enc.Hex);

        //_txtMessage.show().html(MD5 + ", " + _currentAnswerMD5 + ", " + (MD5 == _currentAnswerMD5).toString())

        if (MD5 == _currentAnswerMD5) {
            NextStep();
        }
    }
}