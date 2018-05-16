 var sortOrder = '';
var sortDirection = '';
var imgID = '';
var direction = '';
var pageSize = 50;
var requestedPage = 0;
var pageLoadedFirst = true;
var totalRecords = 0;
var numOfPages = 0;
var StartIndex = 0;
var LastIndex = 0;
var search = '';
var listAgency = null;
var isUpdate = false;
$(document).ready(function () {

    var value = 0;

    $.ajax({
        url: "/Matrix/GetAnnualAssessment",
        type: "POST",
        dataType: "json",
        secureuri: false,
        async: false,
        success: function (data) {
            var assessmentValue = parseInt(data.AnnualAssessmentType);
            var annualAssessmentId = parseInt(data.AnnualAssessmentId);
            isUpdate = (assessmentValue > 0) ? true : false;
            value = assessmentValue;
            var username = data.UserName;
            $('input[name=assesmentradio][value=' + assessmentValue + ']').prop('checked', true);
            $('#agencyName').html(username);
            var assessmentname = $('input[name="assesmentradio"]:checked').attr("data-text");
            $('#assessmentName').text(assessmentname);
            $('#assessment1Fromdate').val(data.Assessment1From);
            $('#assessment1Todate').val(data.Assessment1To);
            $('#assessment2Fromdate').val(data.Assessment2From);
            $('#assessment2Todate').val(data.Assessment2To);
            $('#assessment3Fromdate').val(data.Assessment3From);
            $('#assessment3Todate').val(data.Assessment3To);


            switch (assessmentValue) {
                case 1:
                    $('.assessment1-date').removeClass('hidden');
                    $('.assessment2-date, .assessment3-date').addClass('hidden');
                    break;
                case 2:
                    $('.assessment1-date, .assessment2-date').removeClass('hidden');
                    $('.assessment3-date').addClass('hidden');
                    break;
                case 3:
                    $('.assessment1-date, .assessment2-date,.assessment3-date').removeClass('hidden');
                    break;
            }
        }
        , error: function (response) {
            //  customAlert("Session Ended Log Onto The System Again.");setTimeout(function () {window.location.href= HostedDir + '/login/Loginagency';   }, 2000);
        }
    });


    $('#btnadd').click(function () {
        cleanValidation();
        var GetDate = new Date();
        var month = GetDate.getMonth() + 1;
        var day = GetDate.getDate();
        var CurrentDate =new Date( (month < 10 ? '0' : '') + month + '/' + (day < 10 ? '0' : '') + day + '/' + GetDate.getFullYear());
        if (!($("input:radio[name='assesmentradio']").is(":checked"))) {
            customAlert("Please select assessment type");
            plainValidation('assesmentradio');
            return false;
        }
        var assessmentValue = $("input[name='assesmentradio']:checked").val();
        if (assessmentValue == 1) {
            if ($('#assessment1Fromdate').val().trim() == "") {
                customAlert("Please select From Date");
                plainValidation('#assessment1Fromdate');
                return false;
            }
            else if (!isDate($('#assessment1Fromdate').val())) {
                cleanValidation();
                customAlert("Invalid Date");
                plainValidation('#assessment1Fromdate');
                return false;
            }

            else if ($('#assessment1Todate').val().trim() == "") {
                customAlert("Please select To Date");
                plainValidation('#assessment1Todate');
                return false;
            }
            else if (!isDate($('#assessment1Todate').val())) {
                cleanValidation();
                customAlert("Invalid Date");
                plainValidation('#assessment1Todate');
                return false;
            }
        }
        if (assessmentValue == 2) {
            if ($('#assessment1Fromdate').val().trim() == "") {
                customAlert("Please select From Date");
                plainValidation('#assessment1Fromdate');
                return false;
            }
            else if (!isDate($('#assessment1Fromdate').val())) {
                cleanValidation();
                customAlert("Invalid Date");
                plainValidation('#assessment1Fromdate');
                return false;

            }

            else if ($('#assessment1Todate').val().trim() == "") {
                customAlert("Please select To Date");
                plainValidation('#assessment1Todate');
                return false;
            }
            else if (!isDate($('#assessment1Todate').val())) {
                cleanValidation();
                customAlert("Invalid Date");
                plainValidation('#assessment1Todate');
                return false;

            }
                    
            else if ($('#assessment2Fromdate').val().trim() == "") {
                customAlert("Please select From Date for Assessment 2");
                plainValidation('#assessment2Fromdate');
                return false;
            }
            else if (!isDate($('#assessment2Fromdate').val())) {
                cleanValidation();
                customAlert("Invalid Date");
                plainValidation('#assessment2Fromdate');
                return false;

            }
                    
            else if ($('#assessment2Todate').val().trim() == "") {
                customAlert("Please select To Date");
                plainValidation('#assessment2Todate');
                return false;
            }
            else if (!isDate($('#assessment2Todate').val())) {
                cleanValidation();
                customAlert("Invalid Date");
                plainValidation('#assessment2Todate');
                return false;

            }
        }
        if (assessmentValue == 3) {
            if ($('#assessment1Fromdate').val().trim() == "") {
                customAlert("Please select From Date");
                plainValidation('#assessment1Fromdate');
                return false;
            }
            else if (!isDate($('#assessment1Fromdate').val())) {
                cleanValidation();
                customAlert("Invalid Date");
                plainValidation('#assessment1Fromdate');
                return false;

            }
            else if ($('#assessment1Todate').val().trim() == "") {
                customAlert("Please select To Date for Assessment 1");
                plainValidation('#assessment1Todate');
                return false;
            }
            else if (!isDate($('#assessment1Todate').val())) {
                cleanValidation();
                customAlert("Invalid Date");
                plainValidation('#assessment1Todate');
                return false;

            }
            else if ($('#assessment2Fromdate').val().trim() == "") {
                customAlert("Please select From Date for Assessment 2");
                plainValidation('#assessment2Fromdate');
                return false;
            }
            else if (!isDate($('#assessment2Fromdate').val())) {
                cleanValidation();
                customAlert("Invalid Date");
                plainValidation('#assessment2Fromdate');
                return false;

            }
            else if ($('#assessment2Todate').val().trim() == "") {
                customAlert("Please select To Date for Assessment 2");
                plainValidation('#assessment2Todate');
                return false;
            }
            else if (!isDate($('#assessment2Todate').val())) {
                cleanValidation();
                customAlert("Invalid Date");
                plainValidation('#assessment2Todate');
                return false;

            }

            else if ($('#assessment3Fromdate').val().trim() == "") {
                customAlert("Please select From Date for Assessment 3");
                plainValidation('#assessment3Fromdate');
                return false;
            }
            else if (!isDate($('#assessment3Fromdate').val())) {
                cleanValidation();
                customAlert("Invalid Date");
                plainValidation('#assessment3Fromdate');
                return false;

            }

            else if ($('#assessment3Todate').val().trim() == "") {
                customAlert("Please select To Date for Assessment 3");
                plainValidation('#assessment3Todate');
                return false;
            }
            else if (!isDate($('#assessment3Todate').val())) {
                cleanValidation();
                customAlert("Invalid Date");
                plainValidation('#assessment3Todate');
                return false;

            }

        }
        var checkvalueTodate1 = new Date($('#assessment1Todate').val());
        var checkvalueTodate2 = new Date($('#assessment2Todate').val());
        var checkvalueTodate3 = new Date($('#assessment3Todate').val());
        var checkvalueFromdate1 = new Date($('#assessment1Fromdate').val());
        var checkvalueFromdate2 = new Date($('#assessment2Fromdate').val());
        var checkvalueFromdate3 = new Date($('#assessment3Fromdate').val());

        var GetDate = new Date();
        var month = GetDate.getMonth() + 1;
        var day = GetDate.getDate();
        var CurrentDate = (month < 10 ? '0' : '') + month + '/' + (day < 10 ? '0' : '') + day + '/' + GetDate.getFullYear();

        if (!isUpdate) {
        if ($("input:radio[name='assesmentradio']:checked").val() == 1) {
            if (checkvalueFromdate1 < CurrentDate) {
                customAlert("Assessment From Date Must Be is Greater Than Or Equal Current Date");
                plainValidation('#assessment1Fromdate');
                return false;
            }
            else if (checkvalueTodate1 < checkvalueFromdate1) {
                cleanValidation();
                customAlert("Assessment To Date is Greater Than  From Date");
                plainValidation('#assessment1Todate');
                return false;
            }
        }
        if ($("input:radio[name='assesmentradio']:checked").val() == 2) {

            if (checkvalueFromdate1 < CurrentDate) {
                cleanValidation();
                customAlert("Assessment From Date Must Be is Greater Than Or Equal Current Date");
                plainValidation('#assessment1Fromdate');
                return false;
            }
            else if (checkvalueTodate1 > checkvalueFromdate1) {
                cleanValidation();
                customAlert("Assessment To Date is Greater Than From Date");
                plainValidation('#assessment1Todate');
                return false;
            }

            else if (checkvalueTodate1 > checkvalueFromdate2) {
                cleanValidation();
                customAlert("Assessment 2 From Date is Greater Than From Assessment 1 To Date");
                plainValidation('#assessment2Fromdate');
                return false;
            }

            else if (checkvalueFromdate2 < checkvalueTodate2) {
                cleanValidation();
                customAlert("Assessment 2 To Date is Less Than Assessment 2 From Date");
                plainValidation('#assessment2Todate');
                return false;
            }
        }

        if ($("input:radio[name='assesmentradio']:checked").val() == 3) {
           
            var date1 = new Date(checkvalueTodate1);
           
            var date2 = new Date(checkvalueFromdate2);
            if (checkvalueFromdate1 < CurrentDate) {
                cleanValidation();
                customAlert("Assessment From Date Must Be is Greater Than Or Equal Current Date");
                plainValidation('#assessment1Fromdate');
                return false;
            }
            else if (checkvalueTodate1 < checkvalueFromdate1) {
                cleanValidation();
                customAlert("Assessment To Date is Less Than From Date");
                plainValidation('#assessment1Todate');
                return false;
            }

            else if (checkvalueTodate1 > checkvalueFromdate2) {
                
               
                cleanValidation();
                customAlert("Assessment 2 FromDate5555 is Less Than From Assessment 1 ToDate");
                plainValidation('#assessment2Fromdate');
                return false;
            }

            else if (checkvalueFromdate2 > checkvalueTodate2) {
                cleanValidation();
                customAlert("Assessment 2 To Date is Less Than Assessment 2 From Date");
                plainValidation('#assessment2Todate');
                return false;
            }
            else if (checkvalueTodate2 > checkvalueFromdate3) {
                cleanValidation();
                customAlert("Assessment 3 From Date is Less Than Assessment 2 To Date");
                plainValidation('#assessment3Fromdate');
                return false;
            }
            else if (checkvalueFromdate3 > checkvalueTodate3) {
                cleanValidation();
                customAlert("Assessment 3 From Date is Greater Than To Date");
                plainValidation('#assessment3Todate');
                return false;
            }
        }
        }
        if (assessmentValue == 1) {
            $('#assessment2Fromdate').val('');
            $('#assessment2Todate').val('');
            $('#assessment3Fromdate').val('');
            $('#assessment3Todate').val('');

        }
        if (assessmentValue == 2) {
            $('#assessment3Fromdate').val('');
            $('#assessment3Todate').val('');
        }

        var assessment = {};

        assessment.AnnualAssessmentType = parseInt(assessmentValue);

        assessment.Assessment1From = $('#assessment1Fromdate').val().trim();
        assessment.Assessment1To = $('#assessment1Todate').val().trim();
        assessment.Assessment2From = ($('#assessment2Fromdate').val().trim() == "") ? null : $('#assessment2Fromdate').val().trim();
        assessment.Assessment2To = ($('#assessment2Todate').val().trim() == "") ? null : $('#assessment2Todate').val().trim();
        assessment.Assessment3From = ($('#assessment3Fromdate').val().trim() == "") ? null : $('#assessment3Fromdate').val().trim();
        assessment.Assessment3To = ($('#assessment3Todate').val().trim() == "") ? null : $('#assessment3Todate').val().trim();

        $.ajax({
            url: "/Matrix/AddAnnualAssessment",
            dataType: 'json',
            type: "POST",
            async: false,
            data: assessment,
            success: function (data) {
                if (data) {
                    cleanValidation();
                    customAlert("Record saved successfully.");
                    $('#assessmentName').html('');
                    $('#assessmentName').html($('input[name="assesmentradio"]:checked').attr("data-text"));

                }

                else
                    customAlert(data);

            },
            error: function (data) { alert(data); }
        });

    });


    function isDate(txtDate) {
        var currVal = txtDate;
        if (currVal == '')
            return false;

        var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/; //Declare Regex
        var dtArray = currVal.match(rxDatePattern); // is format OK?

        if (dtArray == null)
            return false;

        //Checks for mm/dd/yyyy format.
        dtMonth = dtArray[1];
        dtDay = dtArray[3];
        dtYear = dtArray[5];

        if (dtMonth < 1 || dtMonth > 12)
            return false;
        else if (dtDay < 1 || dtDay > 31)
            return false;
        else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
            return false;
        else if (dtMonth == 2) {
            var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
            if (dtDay > 29 || (dtDay == 29 && !isleap))
                return false;
        }
        return true;
    }




    $("div").on('keyup', '.datepicker', function (e) {
        flags = 0;
        if (e.keyCode != 193 && e.keyCode != 111) {
            if (e.keyCode != 8) {
                if ($(this).val().length == 2) {
                    $(this).val($(this).val() + "/");
                } else if ($(this).val().length == 5) {
                    $(this).val($(this).val() + "/");
                }
            } else {
                var temp = $(this).val();
                if ($(this).val().length == 5) {
                    $(this).val(temp.substring(0, 4));
                } else if ($(this).val().length == 2) {
                    $(this).val(temp.substring(0, 1));
                }
            }
        } else {
            var temp = $(this).val();
            var tam = $(this).val().length;
            $(this).val(temp.substring(0, tam - 1));
        }
    });

    var flags = 0;
    $('body').on("keydown", ".datepicker", function (e) {
        flags++;
        if (flags > 1) {
            e.preventDefault();
        }

        var key = e.charCode || e.keyCode || 0;

        // allow backspace, tab, delete, enter, arrows, numbers and keypad numbers ONLY
        // home, end, period, and numpad decimal
        return (key == 8 || key == 9 || key == 13 || key == 46 || key == 37 || key == 39 ||
            key == 35 || key == 36 || key == 110 || key == 190 || key == 191 ||
           (key >= 96 && key <= 111 || key >= 48 && key <= 57 && !e.shiftKey));
        //(e.which == keyCode.ENTER)


        var owner = this,
      pps = owner.properties,
      charCode = e.which || e.keyCode || e.charCode || 0;

        // hit backspace when last character is delimiter

        if (Util.isAndroid() &&
            e.target.value.length &&
            e.target.value.length === this._lastEventValue.length - 1 &&
            Util.isDelimiter(pps.result.slice(-1), pps.delimiter, pps.delimiters)
        ) {
            e.charCode = 8;
        }
        if (e.charCode === 8 &&
            Util.isDelimiter(pps.result.slice(-1), pps.delimiter, pps.delimiters)) {
            pps.backspace = true;
        }
        else {
            pps.backspace = false;
        }
        this._lastEventValue = e.target.value;

    });


    $('input:radio').change(function () {

        var assessmentvalue = parseInt($(this).val());

        switch (assessmentvalue) {
            case 1:
                $('.assessment1-date').removeClass('hidden');
                $('.assessment2-date, .assessment3-date').addClass('hidden');
                break;
            case 2:
                $('.assessment1-date, .assessment2-date').removeClass('hidden');
                $('.assessment3-date').addClass('hidden');
                break;
            case 3:
                $('.assessment1-date, .assessment2-date,.assessment3-date').removeClass('hidden');
                break;
        }
    });
});
