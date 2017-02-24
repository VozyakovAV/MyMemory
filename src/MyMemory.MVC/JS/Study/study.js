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
                console.log(response);
                ShowQuestion(response.Question.Text);
            }
        });
    }

    function NextStep() {
        $.ajax({
            type: "POST", url: _urlNextStep, data: { answer: _inpAnswer.val() },
            success: function (response) {
                console.log(response);
                ShowResult(response);
                setTimeout(function () {
                    ShowQuestion(response.Question.Text);
                }, 2000)
            }
        });
    }

    function ShowQuestion(question) {
        _inpCorrectAnswer.hide();
        _inpAnswer.val("");
        _txtMessage.html("");
        _txtQuestion.text(question);
    }

    function ShowResult(response) {
        if (response.PrevAnswer != null) {
            if (response.PrevAnswer.IsCorrectAnswer) {
                ShowSuccessResult();
            }
            else {
                ShowErrorResult(response.PrevAnswer.CorrectAnswer)
            }
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
        StartStudy();
    }

    _btnSubmit.click(function () {
        NextStep();
    });

}