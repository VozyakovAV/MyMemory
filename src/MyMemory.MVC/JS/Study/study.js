function StudyTest(urlStartStudy, urlNextStep) {
    var _urlStartStudy = urlStartStudy;
    var _urlNextStep = urlNextStep;

    function StartStudy() {
        $.ajax({ type: "POST", url: _urlStartStudy, data: { groupId: 0},
            success: function (response) {
                console.log(response);
                $("#txtQuestion").text(response.Question.Text);
            }
        });
    }

    function NextStep() {
        $.ajax({ type: "POST", url: _urlNextStep, data: { answer: $("#inpAnswer").val() },
            success: function (response) {
                console.log(response);
                $("#txtQuestion").text(response.Question.Text);
            }
        });
    }

    this.Start = function()
    {
        StartStudy();
    }

    $("#btnSubmit").click(function () {
         NextStep();
    });

}