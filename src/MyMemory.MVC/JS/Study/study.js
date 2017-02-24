function StudyTest(urlStartStudy, urlNextStep) {
    var _urlStartStudy = urlStartStudy;
    var _urlNextStep = urlNextStep;
    var _txtQuestion = $("#txtQuestion");
    var _inpAnswer = $("#inpAnswer");
    var _btnSubmit = $("#btnSubmit");

    function StartStudy() {
        $.ajax({
            type: "POST", url: _urlStartStudy, data: { groupId: 0 },
            success: function (response) {
                console.log(response);
                _txtQuestion.text(response.Question.Text);
            }
        });
    }

    function NextStep() {
        $.ajax({
            type: "POST", url: _urlNextStep, data: { answer: _inpAnswer.val() },
            success: function (response) {
                console.log(response);
                _txtQuestion.text(response.Question.Text);
            }
        });
    }

    this.Start = function () {
        StartStudy();
    }

    _btnSubmit.click(function () {
        NextStep();
    });

}