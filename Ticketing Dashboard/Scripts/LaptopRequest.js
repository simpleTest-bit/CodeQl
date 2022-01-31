$(document).ready(function () {
    $('.otherLocation').hide();
    $('.otherLocation2').hide();
    $('.otherLocation3').hide();
    $('.sapid-error-msg1').hide();
    $('.sapid-error-msg2').hide();
    $('.sapID1').on('focusout', function () {
        v = parseInt($(this).val());
        min = parseInt($(this).attr('min'));
        max = parseInt($(this).attr('max'));

        if (v < min) {
            $(this).val(null);
            $('.sapid-error-msg1').show();
        } else if (v > max) {
            $(this).val(null);
            $('.sapid-error-msg1').show();
        }
        else {
            $('.sapid-error-msg1').hide();
        }
    })
    $('.sapID2').on('focusout', function () {
        v = parseInt($(this).val());
        min = parseInt($(this).attr('min'));
        max = parseInt($(this).attr('max'));

        if (v < min) {
            $(this).val(null);
            $('.sapid-error-msg2').show();
        } else if (v > max) {
            $(this).val(null);
            $('.sapid-error-msg2').show();
        }
        else {
            $('.sapid-error-msg2').hide();
        }
    })
    $("#offshoreForm").submit(function (e) {


        //disable the submit button
        $("#formSubmitBtn").attr("disabled", true);

        return true;

    });
    $("#breakFix").submit(function (e) {


        //disable the submit button
        $("#formSubmitBtn").attr("disabled", true);

        return true;

    });
});

function toggleLocation() {
    var element = $("#location option:selected").text();
    if (element == "Other") {
        $('.otherLocation').show();
    }
    else {
        $('.otherLocation').hide();
    }
}

function toggleLocation2() {
    var element = $("#location2 option:selected").text();
    if (element == "Other") {
        $('.otherLocation2').show();
    }
    else {
        $('.otherLocation2').hide();
    }
}

function toggleLocation3() {
    var element = $("#location3 option:selected").text();
    if (element == "Other") {
        $('.otherLocation3').show();
    }
    else {
        $('.otherLocation3').hide();
    }
}