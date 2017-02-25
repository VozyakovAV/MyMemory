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
                var next = ShowResult(response);
                if (next) {
                    ShowQuestion(response);
                }
            }
        });
    }

    function NextStep() {
        $.ajax({
            type: "POST", url: _urlNextStep, data: { answer: _inpAnswer.val() },
            success: function (response) {
                var next = ShowResult(response);
                if (next) {
                    setTimeout(function () {
                        ShowQuestion(response);
                    }, 2000)
                }
            }
        });
    }

    function ShowResult(response) {
        console.log(response);

        if (response.Message != null) {
            _txtMessage.html(response.Message).css("color", "black");
            _btnSubmit.hide();
            _inpAnswer.hide();
            _inpCorrectAnswer.hide();
            return false;
        }

        if (response.PrevAnswer != null) {
            if (response.PrevAnswer.IsCorrectAnswer) {
                ShowSuccessResult();
                return true;
            }
            else {
                ShowErrorResult(response.PrevAnswer.CorrectAnswer)
                return false;
            }
        }

        return true;
    }

    function ShowQuestion(response) {
        _inpCorrectAnswer.hide();
        _inpAnswer.val("");
        _txtMessage.html("");
        if (response.Message == null) {
            _txtQuestion.text(response.Question.Text);
            _inpAnswer.focus();
            _inpAnswer.keyup(function (e) {
                if (e.keyCode == 13) {
                    NextStep();
                }
            });
        }
        else {
            _txtMessage.html(response.Message).css("color", "black");
            _btnSubmit.hide();
        }
    }

    function ShowSuccessResult() {
        _txtMessage.html("Верно!").css("color", "green");
    };
    function ShowErrorResult(correctAnswer) {
        _txtMessage.html("Упс!").css("color", "red");
        _inpAnswer.addClass("")
        _inpCorrectAnswer.show();
        _inpCorrectAnswer.val(correctAnswer);
    };

    this.Start = function () {
        _inpCorrectAnswer.prop("disabled", true);
        StartStudy();
    }

    _btnSubmit.click(function () {
        NextStep();
    });

}