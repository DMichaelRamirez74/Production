var HostedDir = "";
function validateEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}
function Password(password) {
    // var re = /^(\w*(\d+[A-Z]|[a-z]+\d)\w*)+$/;
    var re = /^(?=.*\d)(?=.*[A-Z])[0-9a-zA-Z\d$@$!%*#?&]{7,}$/;
    return re.test(password);
}

function isSpclChar(el) {
    var iChars = "!@#$%^&*()+=-[]\\\';,./{}|\":?";
    for (var i = 0; i < el.value.length; i++) {
        if (iChars.indexOf(el.value.charAt(i)) != -1) {
            el.value = "";
            return false;
        }
    }
    el.value = el.value.toUpperCase();
    //var ex = /([0-9a-zA-Z])$/;
    //if (ex.test(el.value) == false) {
    //    el.value = "";
    //}
}
function checkIfNumeric(s) {
    //This will check value is numeric or not
    if (isNaN(s.value)) {
        s.value = "";
        //alert("Not a number!");
    }
}

function PhoneValidate(el)
{
  
    var ex = /^[+]?[0-9]?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.length - 1);
    }
}

function checkDecimal(el) {
   
    var ex = /^[0-9]+\.?[0-9]*$/;
    if (ex.test(el.value) == false) {
        el.value = "";
    }
    var ex = /^\d*\.?\d{0,2}$/;
    if (ex.test(el.value) == false) {
        el.value = el.value.substring(0, el.value.indexOf('.') + 3);
    }
    var lastChar = el.value.substring(el.value.length - 1);
    if (lastChar == ".") {        
        el.value = el.value.substring(0,el.value.length - 1);
    }
}



    function checkDec(el) {
        var ex = /^[0-9]+\.?[0-9]*$/;
        if (ex.test(el.value) == false) {
            el.value = el.value.substring(0, el.value.length - 1);
        }
    }
    function checkIfLetters(s) {
        //This will check value is numeric or not
        if (!/^[a-zA-Z ]*$/g.test(s.value)) {
            s.value = "";
        }
    }
    function CheckFirstCharcter(s) {
        var text = s.value;
        var fistchar = text.charAt(0)
        if (!/[a-zA-Z]/.test(fistchar))
            s.value = "";
    }
    function customAlert(MessageText) {
       
        if (MessageText.indexOf("added") != -1 || MessageText.indexOf("updated") != -1 || MessageText.indexOf("successfully") != -1 || MessageText.indexOf("Invatation") != -1) {
            $("#noty_topRight_layout_container").css("background-color", "green");
        }
        else {
            $("#noty_topRight_layout_container").css("background-color", "#a94442");
        }
        $("#noty_topRight_layout_container")[0].children[0].children[0].children[0].innerHTML = MessageText;
        $("#noty_topRight_layout_container").slideToggle();
        setTimeout(function () { $("#noty_topRight_layout_container").slideToggle(); }, 4000);
    }
    function customAlertSuccess(MessageText) {
        $("#noty_topRight_layout_container").css("background-color", "green");
        $("#noty_topRight_layout_container")[0].children[0].children[0].children[0].innerHTML = MessageText;
        $("#noty_topRight_layout_container").slideToggle();
        setTimeout(function () { $("#noty_topRight_layout_container").slideToggle(); }, 4000);
    }
    function customAlertforlongtime(MessageText) {

        if (MessageText.indexOf("added") != -1 || MessageText.indexOf("updated") != -1 || MessageText.indexOf("successfully") != -1) {
            $("#noty_topRight_layout_container").css("background-color", "green");
        }
        else {
            $("#noty_topRight_layout_container").css("background-color", "#a94442");
        }
        $("#noty_topRight_layout_container")[0].children[0].children[0].children[0].innerHTML = MessageText;
        $("#noty_topRight_layout_container").slideToggle();
        setTimeout(function () { $("#noty_topRight_layout_container").slideToggle(); }, 6000);
    }
    function plainValidation(id) {
        // $(id).addClass('backPink');
        $(id).focus();
        $(id).css("background-color", "pink");
    }
    function cleanValidation() {
        $('input,textarea,select').each(function () {
            // $(this).removeClass('backPink');
            $(this).css("background-color", "");
        });
    }
    function Converttimeformat(time1) {
        
        var time = time1;
        var hrs = Number(time.match(/^(\d+)/)[1]);
        var mnts = Number(time.match(/:(\d+)/)[1]);
        var format = time.match(/\s(.*)$/)[1];
        if (format == "PM" && hrs < 12) hrs = hrs + 12;
        if (format == "AM" && hrs == 12) hrs = hrs - 12;
        var hours = hrs.toString();
        var minutes = mnts.toString();
        if (hrs < 10) hours = "0" + hours;
        if (mnts < 10) minutes = "0" + minutes;
          return hours + ":" + minutes;
    }
    function commaSeparateNumber(val) {
        while (/(\d+)(\d{3})/.test(val.toString())) {
            val = val.toString().replace(/(\d+)(\d{3})/, '$1' + ',' + '$2');
        }
        return val;
    }