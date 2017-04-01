function StudyManager(urlStartStudy, urlNextStep) {

    var _urlStartStudy = urlStartStudy;
    var _urlNextStep = urlNextStep;
    var _groupId = 0;

    var _statCorrect = $("#statCorrect");
    var _statIncorrect = $("#statIncorrect");
    var _statStepNumber = $("#statStepNumber");

    var _blockMessage = $("#blockMessage");
    var _blockTestWords = $("#blockTestWords");
    var _blockTestLetters = $("#blockTestLetter");
    
    var _studyTestWords;
    var _studyTestLetters;

    // ----------------------------

    this.Start = function () {
        document.addEventListener('answerEvent', function (e) {
            NextStep(e.detail);
        }, false);

        _studyTestWords = new StudyTestWords();
        _studyTestLetters = new StudyTestLetters();

        StartStudy();
    }

    function StartStudy() {
        $.ajax({
            type: "POST", url: _urlStartStudy, data: { groupId: _groupId },
            success: function (response) {
                ShowResult(response);
            }
        });
    }

    function NextStep(textAnswer) {
        $.ajax({
            type: "POST", url: _urlNextStep, data: { answer: textAnswer },
            success: function (response) {
                ShowResult(response);
            }
        });
    }

    function ShowResult(response) {
        console.log(response);
        HideBlocks();

        if (response.Message != null) {
            ShowViewMessage(response);
        }
        else {
            ShowStatistic(response);
            ShowBlockTestWords(response);
            //ShowBlockTestLetters(response);
        }
    }

    function ShowStatistic(response) {
        _statCorrect.html(response.Statistic.NumberOfCorrect);
        _statIncorrect.html(response.Statistic.NumberOfIncorrect);
        _statStepNumber.html(response.Step.Question.StepNumber);
    }

    function ShowViewMessage(response) {
        _blockMessage.html(response.Message).css("color", "black").show();
    }
    
    function HideBlocks() {
        _blockMessage.hide();
        _blockTestWords.hide();
    }

    function ShowBlockTestWords(response) {
        _blockTestWords.show();
        _studyTestWords.Show(response);
    }

    function ShowBlockTestLetters(response) {
        _blockTestLetters.show();
        _studyTestLetters.Show(response);
    }
}