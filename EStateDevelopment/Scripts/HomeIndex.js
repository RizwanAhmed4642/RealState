function Fromcountry() {
    var ddlFromcountry = $("#ddlFromcountry").val();
    if (ddlFromcountry.trim() == '') {
        if (ddlFromcountry.charAt(0) == ' ') {
            $("#errorFromcountry").html("Please remove space");
        }
        $("#errorFromcountry").html("This field is must required");
    }
    else {
        if (ddlFromcountry.charAt(0) != ' ') {
            $("#errorFromcountry").html("");
            return true;
        }
        else {
            $("#errorFromcountry").html("Please remove space");
        }
    }
}
function Tocountry() {
    var ddlTocountry = $("#ddlTocountry").val();
    if (ddlTocountry.trim() == '') {
        if (ddlTocountry.charAt(0) == ' ') {
            $("#errorTocountry").html("Please remove space");
        }
        $("#errorTocountry").html("This field is must required");
    }
    else {
        if (ddlTocountry.charAt(0) != ' ') {
            $("#errorTocountry").html("");
            return true;
        }
        else {
            $("#errorTocountry").html("Please remove space");
        }
    }
}
//function fromcity() {
//    var txtfromcity = $("#txtfromcity").val();
//    var regex = /^[ a-zA-Z\-\’]+$/;
//    if (txtfromcity.trim() == '') {
//        if (txtfromcity.indexOf(' ') > -1) {
//            $("#errorfromcity").html("Please remove space");
//            return false;
//        }
//        $("#errorfromcity").html("This field is must required");
//        return false;
//    }
//    else {
//        if (txtfromcity.charAt(0) != ' ') {
//            if (regex.test(txtfromcity)) {
//                if ($.trim(txtfromcity)) {
//                    $("#errorfromcity").html("");
//                    return true;
//                }
//            }
//            else {
//                $("#errorfromcity").html("Just alphabet are allow.");
//                return false;
//            }
//        }
//        else {
//            $("#errorfromcity").html("whitespace are not allowed.");
//            return false;
//        }
//    }
//}


function tocity() {
    var txttocity = $("#txttocity").val();
    var regex = /^[ a-zA-Z\-\’]+$/;
    if (txttocity.trim() == '') {
        if (txttocity.indexOf(' ') > -1) {
            $("#errortocity").html("Please remove space");
            return false;
        }
        $("#errortocity").html("This field is must required");
        return false;
    }
    else {
        if (txttocity.charAt(0) != ' ') {
            if (regex.test(txttocity)) {
                if ($.trim(txttocity)) {
                    $("#errortocity").html("");
                    return true;
                }
            }
            else {
                $("#errortocity").html("Just alphabet are allow.");
                return false;
            }
        }
        else {
            $("#errortocity").html("whitespace are not allowed.");
            return false;
        }
    }
}
function fromzipCode() {
    var txtfromzipCode = $("#txtfromzipCode").val();
    var regex = /^\d+$/;
    if (txtfromzipCode.trim() == '') {
        if (txtfromzipCode.indexOf(' ') > -1) {
            $("#errorfromzipCode").html("Please remove space");
            return false;
        }
        $("#errorfromzipCode").html("This field is must required");
        return false;
    }
    else {
        if (txtfromzipCode.charAt(0) != ' ') {
            if (regex.test(txtfromzipCode)) {
                if ($.trim(txtfromzipCode)) {
                    $("#errorfromzipCode").html("");
                    return true;
                }
            }
            else {
                $("#errorfromzipCode").html("Special characters and alphabet are not allowed.");
                return false;
            }
        }
        else {
            $("#errorfromzipCode").html("whitespace are not allowed.");
            return false;
        }
    }
}
function tozipCode() {
    var txttozipCode = $("#txttozipCode").val();
    var regex = /^\d+$/;
    if (txttozipCode.trim() == '') {
        if (txttozipCode.indexOf(' ') > -1) {
            $("#errortozipCode").html("Please remove space");
            return false;
        }
        $("#errortozipCode").html("This field is must required");
        return false;
    }
    else {
        if (txttozipCode.charAt(0) != ' ') {
            if (regex.test(txttozipCode)) {
                if ($.trim(txttozipCode)) {
                    $("#errortozipCode").html("");
                    return true;
                }
            }
            else {
                $("#errortozipCode").html("Special characters and alphabet are not allowed.");
                return false;
            }
        }
        else {
            $("#errortozipCode").html("whitespace are not allowed.");
            return false;
        }
    }
}
function fntype() {
    var ddltype = $("#ddltype").val();
    if (ddltype.trim() == '') {
        if (ddltype.charAt(0) == ' ') {
            $("#errortype").html("Please remove space");
        }
        $("#errortype").html("This field is must required");
    }
    else {
        if (ddltype.charAt(0) != ' ') {
            $("#errortype").html("");
            return true;
        }
        else {
            $("#errortype").html("Please remove space");
        }
    }
}
function packaging() {
    var ddlpackaging = $("#ddlpackaging").val();
    if (ddlpackaging.trim() == '') {
        if (ddlpackaging.charAt(0) == ' ') {
            $("#errorpackaging").html("Please remove space");
        }
        $("#errorpackaging").html("This field is must required");
    }
    else {
        if (ddlpackaging.charAt(0) != ' ') {
            $("#errorpackaging").html("");
            return true;
        }
        else {
            $("#errorpackaging").html("Please remove space");
        }
    }
}
function length() {
    debugger;
    var txtlength = $("#txtlength").val();
    var regex = /^\-?([0-9]+(\.[0-9]+)?|Infinity)$/;
    if (txtlength.trim() == '') {
        if (txtlength.indexOf(' ') > -1) {
            $("#errorlength").html("Please remove space");
            return false;
        }
        $("#errorlength").html("This field is must required");
        return false;
    }
    else {
        if (txtlength.charAt(0) != ' ') {
            if (regex.test(txtlength)) {
                if ($.trim(txtlength)) {
                    $("#errorlength").html("");
                    return true;
                }
            }
            else {
                $("#errorlength").html("Special characters and alphabet are not allowed.");
                return false;
            }
        }
        else {
            $("#errorlength").html("whitespace are not allowed.");
            return false;
        }
    }
}
function mwidth() {
    debugger;
    var txtwidth = $("#txtwidth").val();
    var regex = /^\-?([0-9]+(\.[0-9]+)?|Infinity)$/;
    if (txtwidth.trim() == '') {
        if (txtwidth.indexOf(' ') > -1) {
            $("#errorwidth").html("Please remove space");
            return false;
        }
        $("#errorwidth").html("This field is must required");
        return false;
    }
    else {
        if (txtwidth.charAt(0) != ' ') {
            if (regex.test(txtwidth)) {
                if ($.trim(txtwidth)) {
                    $("#errorwidth").html("");
                    return true;
                }
            }
            else {
                $("#errorwidth").html("Special characters and alphabet are not allowed.");
                return false;
            }
        }
        else {
            $("#errorwidth").html("whitespace are not allowed.");
            return false;
        }
    }
}
function mheight() {
    debugger;
    var txtheight = $("#txtheight").val();
    var regex = /^\-?([0-9]+(\.[0-9]+)?|Infinity)$/;
    if (txtheight.trim() == '') {
        if (txtheight.indexOf(' ') > -1) {
            $("#errorheight").html("Please remove space");
            return false;
        }
        $("#errorheight").html("This field is must required");
        return false;
    }
    else {
        if (txtheight.charAt(0) != ' ') {
            if (regex.test(txtheight)) {
                if ($.trim(txtheight)) {
                    $("#errorheight").html("");
                    return true;
                }
            }
            else {
                $("#errorheight").html("Special characters and alphabet are not allowed.");
                return false;
            }
        }
        else {
            $("#errorheight").html("whitespace are not allowed.");
            return false;
        }
    }
}
function Result() {
    debugger;
    var txtResult = $("#txtResult").val();
    var regex = /^\-?([0-9]+(\.[0-9]+)?|Infinity)$/;
    if (txtResult.trim() == '') {
        if (txtResult.indexOf(' ') > -1) {
            $("#errorResult").html("Please remove space");
            return false;
        }
        $("#errorResult").html("This field is must required");
        return false;
    }
    else {
        if (txtResult.charAt(0) != ' ') {
            if (regex.test(txtResult)) {
                if ($.trim(txtResult)) {
                    $("#errorResult").html("");
                    return true;
                }
            }
            else {
                $("#errorResult").html("Special characters and alphabet are not allowed.");
                return false;
            }
        }
        else {
            $("#errorResult").html("whitespace are not allowed.");
            return false;
        }
    }
}
