
$(document).ready(function () {
    $('.fieldToToggle').hide();
    $('.otherLocation').hide();
    $('.sapid-error-msg1').hide();
    $('.sapid-error-msg2').hide();
    $('.sapid-error-msg3').hide();
    $('.sapid-error-msg4').hide();
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
    $('.sapID3').on('focusout', function () {
        v = parseInt($(this).val());
        min = parseInt($(this).attr('min'));
        max = parseInt($(this).attr('max'));

        if (v < min) {
            $(this).val(null);
            $('.sapid-error-msg3').show();
        } else if (v > max) {
            $(this).val(null);
            $('.sapid-error-msg3').show();
        }
        else {
            $('.sapid-error-msg3').hide();
        }
    })
    $('.sapID4').on('focusout', function () {
        v = parseInt($(this).val());
        min = parseInt($(this).attr('min'));
        max = parseInt($(this).attr('max'));

        if (v < min) {
            $(this).val(null);
            $('.sapid-error-msg4').show();
        } else if (v > max) {
            $(this).val(null);
            $('.sapid-error-msg4').show();
        }
        else {
            $('.sapid-error-msg4').hide();
        }
    })
});

function test() {
    var element = $("#condition option:selected").text();
    if (element == "Contract") {
        $('.fieldToToggle').show();
    }
    else if (element == "Full Time") {
        $('.fieldToToggle').hide();
    }
}

function toggleSAPID() {
    var element = $("#reqsource option:selected").text();
    if (element == "New") {
        $('#sapid').hide();
        $('#sapidInput').prop('required', false);
    }
    else {
        $('#sapid').show();
        $('#sapidInput').prop('required', true);
    }
}

function toggleLocation() {
    var element = $("#location option:selected").text();
    if (element == "Other") {
        $('.otherLocation').show();
    }
    else {
        $('.otherLocation').hide();
    }
}

function updateOnboardingDate(e) {
    $("#onboardingDate").attr("min", e.target.value);
    $("#onboardingDate").attr("value", e.target.value);
}

function updateBillLossStartDate(e) {
    $("#billingLossStartDate").attr("min", e.target.value);
    $("#billingLossStartDate").attr("value", e.target.value);
}

function updateContractToDate() {
    $("#contractToDate").attr("min", document.getElementById('contractFromDate').value);
    $("#contractToDate").attr("value", document.getElementById('contractFromDate').value);
}

function updateTextInput(val) {
    document.getElementById('textInput').value = val;
}