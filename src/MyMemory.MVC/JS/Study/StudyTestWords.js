function StudyTestWords() {

    var _content = $("#blockTestWords");
    var _txtQuestion = _content.find(".txtQuestion");
    var _inpAnswer = _content.find(".inpAnswer");
    var _inpCorrectAnswer = _content.find(".inpCorrectAnswer");
    var _btnSubmit = _content.find(".btnSubmit");
    var _groupVariants = _content.find(".groupVariants");
    var _btnRemove = _content.find(".btnRemove");
    var _divGroupName = _content.find(".divGroupName");
    var _txtMessage = _content.find(".txtMessage");
    var _variantItems = [];
    var _variantItemsAll;
    var _currentAnswerMD5;

    _inpAnswer.keyup(function (e) {
        if (e.keyCode == 13) {
            _btnSubmit.click();
        }
    });

    _btnRemove.on("click", function () {
        RemoveLastVariantItem();
    });

    _btnSubmit.on("click", function () {
        NextStep();
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
        var event = new CustomEvent('answerEvent', { 'detail': _inpAnswer.val() });
        document.dispatchEvent(event);
    }

    function ShowViewQuestion(response) {
        ResetStylesControls();
        _currentAnswerMD5 = response.Step.Answer.CorrectAnswerMD5;
        _txtQuestion.text(response.Step.Question.Text).show();
        _inpAnswer.val("").show().prop("disabled", true).focus();
        _divGroupName.html(response.Step.Question.GroupName).show();
        _btnSubmit.html("Проверить").show().click(function () {
            NextStep();
        });

        _variantItems = [];
        _variantItemsAll = response.Step.Question.GroupVariants;
        ShowVariants();
    }

    function ShowVariants() {
        _groupVariants.empty();
        var num = _variantItems.length;

        if (_variantItemsAll.length > num) {

            $.each(_variantItemsAll[num].Variants, function (key, value) {
                var link = $("<a href='#' class='linkVariant btn btn-primary'>" + value.trim() + "</a>");
                link.data("variantData", value);
                var div = $("<div class='col-xs-6'></div>");
                div.append(link);
                _groupVariants.append(div);
            });

            $(".linkVariant").on("click", function () {
                AddVariantItem($(this).data("variantData"));
            });
        }
    }

    function AddVariantItem(variantItem) {
        _variantItems.push(variantItem);
        ShowVariantText();
        ShowVariants();
        CheckAnswerHash();
    }

    function RemoveLastVariantItem() {
        if (_variantItems.length > 0) {
            _variantItems.splice(_variantItems.length - 1, 1);
            ShowVariantText();
            ShowVariants();
        }
    }

    function CheckAnswerHash() {
        var text = _inpAnswer.val().toString().trim().toLowerCase();
        var MD5 = CryptoJS.MD5(text).toString(CryptoJS.enc.Hex);

        if (MD5 == _currentAnswerMD5) {
            NextStep();
        }
    }

    function ShowVariantText() {
        var st = "";
        $.each(_variantItems, function (key, value) {
            st += value;
        });
        _inpAnswer.val(st);
    }


    function ShowViewCorrectAnswer(response) {
        ResetStylesControls();
        _txtQuestion.show();
        _inpAnswer.prop("disabled", true).show();
        _inpAnswer.parent().addClass("has-success");
        _txtMessage.html("Верно!").css("color", "green").show();
        _btnSubmit.prop("disabled", true).show();
        _divGroupName.show();
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
        _divGroupName.show();
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
        _divGroupName.hide();
    }
}