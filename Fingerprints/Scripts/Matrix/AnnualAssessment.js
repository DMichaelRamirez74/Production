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


var assessment_1_days_to = null;
var assessment_2_days_to = null;
var assessment_3_days_to = null;

var assessment_1_days_from = null;
var assessment_2_days_from = null;
var assessment_3_days_from = null;

var annualAssessment = {

    checkNumeric: function (inputTextObject) {

        var ret = true;
        var ex = /^[0-9]*$/;
        if ($(inputTextObject).val().trim() != "") {
            if (!ex.test($(inputTextObject).val())) {
                $(inputTextObject).val('');
                var ret = false;
            }
        }
        return ret;
    },

    checkNumericOn: function (ele) {
        if (!this.checkNumeric(ele)) {
            return false;
        }
        else if (!this.checkValidDays(ele)) {
            customAlert('Entered days should not greater than ' + this.daysOfYear(new Date().getFullYear) + '');
            return false;
        }
        else {
            return true;

        }

    },
    checkNumericOnBlur: function (ele) {

        if (!this.checkNumeric(ele)) {
            plainValidation(ele);
            customAlert("Only numeric value allowed. ");
            return false;
        }
        else if (!this.checkValidDays(ele)) {
            plainValidation(ele);
            customAlert('Entered days should not greater than ' + this.daysOfYear(new Date().getFullYear) + '');
            return false;
        }
        else {
            return true;

        }
    },
    checkValidDays: function (ele) {
        return (parseInt($(ele).val()) <= this.daysOfYear(new Date().getFullYear))

    },
    daysOfYear: function (year) {

        return this.isLeapYear(year) ? 366 : 365;
    },

    isLeapYear: function (year) {
        return year % 400 === 0 || (year % 100 !== 0 && year % 4 === 0);
    }

}


$(document).ready(function () {

    var value = 0;


    assessment_1_days_from = $('#assessment1Fromdate');
    assessment_2_days_from = $('#assessment2Fromdate');
    assessment_3_days_from = $('#assessment3Fromdate');

    assessment_1_days_to = $('#assessment1Todate');
    assessment_2_days_to = $('#assessment2Todate');
    assessment_3_days_to = $('#assessment3Todate');


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
            /// $('#assessmentName').text(assessmentname);

            assessment_1_days_from.val(data.Assessment1From);
            assessment_1_days_to.val(data.Assessment1To);
            assessment_2_days_from.val(data.Assessment2From);
            assessment_2_days_to.val(data.Assessment2To);
            assessment_3_days_from.val(data.Assessment3From);
            assessment_3_days_to.val(data.Assessment3To);


            if (!data.EditAssessment1) {
                assessment_1_days_from.prop('readonly', true);
                assessment_1_days_to.prop('readonly', true);
            }
            else {
                assessment_1_days_from.prop('readonly', false);
                assessment_1_days_to.prop('readonly', false);
            }

            if (!data.EditAssessment2) {
                assessment_2_days_from.prop('readonly', true);
                assessment_2_days_to.prop('readonly', true);
            }
            else {
                assessment_2_days_from.prop('readonly', false);
                assessment_2_days_to.prop('readonly', false);
            }

            if (!data.EditAssessment3) {
                assessment_3_days_from.prop('readonly', true);
                assessment_3_days_to.prop('readonly', true);
            }
            else {
                assessment_3_days_from.prop('readonly', false);
                assessment_3_days_to.prop('readonly', false);
            }


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
            customAlert("Session Ended Log Onto The System Again."); setTimeout(function () { window.location.href = HostedDir + '/login/Loginagency'; }, 2000);
        }
    });


    //$('#btnadd').click(function () {
    //    cleanValidation();
    //    var GetDate = new Date();
    //    var month = GetDate.getMonth() + 1;
    //    var day = GetDate.getDate();
    //    var CurrentDate = new Date((month < 10 ? '0' : '') + month + '/' + (day < 10 ? '0' : '') + day + '/' + GetDate.getFullYear());
    //    if (!($("input:radio[name='assesmentradio']").is(":checked"))) {
    //        customAlert("Please select assessment type");
    //        plainValidation('assesmentradio');
    //        return false;
    //    }
    //    var assessmentValue = $("input[name='assesmentradio']:checked").val();
    //    if (assessmentValue == 1) {
    //        if ($('#assessment1Fromdate').val().trim() == "") {
    //            customAlert("Please select From Date");
    //            plainValidation('#assessment1Fromdate');
    //            return false;
    //        }
    //        else if (!isDate($('#assessment1Fromdate').val())) {
    //            cleanValidation();
    //            customAlert("Invalid Date");
    //            plainValidation('#assessment1Fromdate');
    //            return false;
    //        }
    //        else if ($('#assessment1Todate').val().trim() == "") {
    //            customAlert("Please select To Date");
    //            plainValidation('#assessment1Todate');
    //            return false;
    //        }
    //        else if (!isDate($('#assessment1Todate').val())) {
    //            cleanValidation();
    //            customAlert("Invalid Date");
    //            plainValidation('#assessment1Todate');
    //            return false;
    //        }
    //    }
    //    if (assessmentValue == 2) {
    //        if ($('#assessment1Fromdate').val().trim() == "") {
    //            customAlert("Please select From Date");
    //            plainValidation('#assessment1Fromdate');
    //            return false;
    //        }
    //        else if (!isDate($('#assessment1Fromdate').val())) {
    //            cleanValidation();
    //            customAlert("Invalid Date");
    //            plainValidation('#assessment1Fromdate');
    //            return false;
    //        }
    //        else if ($('#assessment1Todate').val().trim() == "") {
    //            customAlert("Please select To Date");
    //            plainValidation('#assessment1Todate');
    //            return false;
    //        }
    //        else if (!isDate($('#assessment1Todate').val())) {
    //            cleanValidation();
    //            customAlert("Invalid Date");
    //            plainValidation('#assessment1Todate');
    //            return false;
    //        }
    //        else if ($('#assessment2Fromdate').val().trim() == "") {
    //            customAlert("Please select From Date for Assessment 2");
    //            plainValidation('#assessment2Fromdate');
    //            return false;
    //        }
    //        else if (!isDate($('#assessment2Fromdate').val())) {
    //            cleanValidation();
    //            customAlert("Invalid Date");
    //            plainValidation('#assessment2Fromdate');
    //            return false;
    //        }
    //        else if ($('#assessment2Todate').val().trim() == "") {
    //            customAlert("Please select To Date");
    //            plainValidation('#assessment2Todate');
    //            return false;
    //        }
    //        else if (!isDate($('#assessment2Todate').val())) {
    //            cleanValidation();
    //            customAlert("Invalid Date");
    //            plainValidation('#assessment2Todate');
    //            return false;
    //        }
    //    }
    //    if (assessmentValue == 3) {
    //        if ($('#assessment1Fromdate').val().trim() == "") {
    //            customAlert("Please select From Date");
    //            plainValidation('#assessment1Fromdate');
    //            return false;
    //        }
    //        else if (!isDate($('#assessment1Fromdate').val())) {
    //            cleanValidation();
    //            customAlert("Invalid Date");
    //            plainValidation('#assessment1Fromdate');
    //            return false;
    //        }
    //        else if ($('#assessment1Todate').val().trim() == "") {
    //            customAlert("Please select To Date for Assessment 1");
    //            plainValidation('#assessment1Todate');
    //            return false;
    //        }
    //        else if (!isDate($('#assessment1Todate').val())) {
    //            cleanValidation();
    //            customAlert("Invalid Date");
    //            plainValidation('#assessment1Todate');
    //            return false;
    //        }
    //        else if ($('#assessment2Fromdate').val().trim() == "") {
    //            customAlert("Please select From Date for Assessment 2");
    //            plainValidation('#assessment2Fromdate');
    //            return false;
    //        }
    //        else if (!isDate($('#assessment2Fromdate').val())) {
    //            cleanValidation();
    //            customAlert("Invalid Date");
    //            plainValidation('#assessment2Fromdate');
    //            return false;
    //        }
    //        else if ($('#assessment2Todate').val().trim() == "") {
    //            customAlert("Please select To Date for Assessment 2");
    //            plainValidation('#assessment2Todate');
    //            return false;
    //        }
    //        else if (!isDate($('#assessment2Todate').val())) {
    //            cleanValidation();
    //            customAlert("Invalid Date");
    //            plainValidation('#assessment2Todate');
    //            return false;
    //        }
    //        else if ($('#assessment3Fromdate').val().trim() == "") {
    //            customAlert("Please select From Date for Assessment 3");
    //            plainValidation('#assessment3Fromdate');
    //            return false;
    //        }
    //        else if (!isDate($('#assessment3Fromdate').val())) {
    //            cleanValidation();
    //            customAlert("Invalid Date");
    //            plainValidation('#assessment3Fromdate');
    //            return false;
    //        }
    //        else if ($('#assessment3Todate').val().trim() == "") {
    //            customAlert("Please select To Date for Assessment 3");
    //            plainValidation('#assessment3Todate');
    //            return false;
    //        }
    //        else if (!isDate($('#assessment3Todate').val())) {
    //            cleanValidation();
    //            customAlert("Invalid Date");
    //            plainValidation('#assessment3Todate');
    //            return false;
    //        }
    //    }
    //    var checkvalueTodate1 = new Date($('#assessment1Todate').val());
    //    var checkvalueTodate2 = new Date($('#assessment2Todate').val());
    //    var checkvalueTodate3 = new Date($('#assessment3Todate').val());
    //    var checkvalueFromdate1 = new Date($('#assessment1Fromdate').val());
    //    var checkvalueFromdate2 = new Date($('#assessment2Fromdate').val());
    //    var checkvalueFromdate3 = new Date($('#assessment3Fromdate').val());
    //    var GetDate = new Date();
    //    var month = GetDate.getMonth() + 1;
    //    var day = GetDate.getDate();
    //    var CurrentDate = (month < 10 ? '0' : '') + month + '/' + (day < 10 ? '0' : '') + day + '/' + GetDate.getFullYear();
    //    if (!isUpdate) {
    //        if ($("input:radio[name='assesmentradio']:checked").val() == 1) {
    //            if (checkvalueFromdate1 < CurrentDate) {
    //                customAlert("Assessment From Date Must Be is Greater Than Or Equal Current Date");
    //                plainValidation('#assessment1Fromdate');
    //                return false;
    //            }
    //            else if (checkvalueTodate1 < checkvalueFromdate1) {
    //                cleanValidation();
    //                customAlert("Assessment To Date is Greater Than  From Date");
    //                plainValidation('#assessment1Todate');
    //                return false;
    //            }
    //        }
    //        if ($("input:radio[name='assesmentradio']:checked").val() == 2) {
    //            if (checkvalueFromdate1 < CurrentDate) {
    //                cleanValidation();
    //                customAlert("Assessment From Date Must Be is Greater Than Or Equal Current Date");
    //                plainValidation('#assessment1Fromdate');
    //                return false;
    //            }
    //            else if (checkvalueTodate1 > checkvalueFromdate1) {
    //                cleanValidation();
    //                customAlert("Assessment To Date is Greater Than From Date");
    //                plainValidation('#assessment1Todate');
    //                return false;
    //            }
    //            else if (checkvalueTodate1 > checkvalueFromdate2) {
    //                cleanValidation();
    //                customAlert("Assessment 2 From Date is Greater Than From Assessment 1 To Date");
    //                plainValidation('#assessment2Fromdate');
    //                return false;
    //            }
    //            else if (checkvalueFromdate2 < checkvalueTodate2) {
    //                cleanValidation();
    //                customAlert("Assessment 2 To Date is Less Than Assessment 2 From Date");
    //                plainValidation('#assessment2Todate');
    //                return false;
    //            }
    //        }
    //        if ($("input:radio[name='assesmentradio']:checked").val() == 3) {
    //            var date1 = new Date(checkvalueTodate1);
    //            var date2 = new Date(checkvalueFromdate2);
    //            if (checkvalueFromdate1 < CurrentDate) {
    //                cleanValidation();
    //                customAlert("Assessment From Date Must Be is Greater Than Or Equal Current Date");
    //                plainValidation('#assessment1Fromdate');
    //                return false;
    //            }
    //            else if (checkvalueTodate1 < checkvalueFromdate1) {
    //                cleanValidation();
    //                customAlert("Assessment To Date is Less Than From Date");
    //                plainValidation('#assessment1Todate');
    //                return false;
    //            }
    //            else if (checkvalueTodate1 > checkvalueFromdate2) {
    //                cleanValidation();
    //                customAlert("Assessment 2 FromDate is Less Than From Assessment 1 ToDate");
    //                plainValidation('#assessment2Fromdate');
    //                return false;
    //            }
    //            else if (checkvalueFromdate2 > checkvalueTodate2) {
    //                cleanValidation();
    //                customAlert("Assessment 2 To Date is Less Than Assessment 2 From Date");
    //                plainValidation('#assessment2Todate');
    //                return false;
    //            }
    //            else if (checkvalueTodate2 > checkvalueFromdate3) {
    //                cleanValidation();
    //                customAlert("Assessment 3 From Date is Less Than Assessment 2 To Date");
    //                plainValidation('#assessment3Fromdate');
    //                return false;
    //            }
    //            else if (checkvalueFromdate3 > checkvalueTodate3) {
    //                cleanValidation();
    //                customAlert("Assessment 3 From Date is Greater Than To Date");
    //                plainValidation('#assessment3Todate');
    //                return false;
    //            }
    //        }
    //    }
    //    if (assessmentValue == 1) {
    //        $('#assessment2Fromdate').val('');
    //        $('#assessment2Todate').val('');
    //        $('#assessment3Fromdate').val('');
    //        $('#assessment3Todate').val('');
    //    }
    //    if (assessmentValue == 2) {
    //        $('#assessment3Fromdate').val('');
    //        $('#assessment3Todate').val('');
    //    }
    //    var assessment = {};
    //    assessment.AnnualAssessmentType = parseInt(assessmentValue);
    //    assessment.Assessment1From = $('#assessment1Fromdate').val().trim();
    //    assessment.Assessment1To = $('#assessment1Todate').val().trim();
    //    assessment.Assessment2From = ($('#assessment2Fromdate').val().trim() == "") ? null : $('#assessment2Fromdate').val().trim();
    //    assessment.Assessment2To = ($('#assessment2Todate').val().trim() == "") ? null : $('#assessment2Todate').val().trim();
    //    assessment.Assessment3From = ($('#assessment3Fromdate').val().trim() == "") ? null : $('#assessment3Fromdate').val().trim();
    //    assessment.Assessment3To = ($('#assessment3Todate').val().trim() == "") ? null : $('#assessment3Todate').val().trim();
    //    $.ajax({
    //        url: "/Matrix/AddAnnualAssessment",
    //        dataType: 'json',
    //        type: "POST",
    //        async: false,
    //        data: assessment,
    //        success: function (data) {
    //            if (data) {
    //                cleanValidation();
    //                customAlert("Record saved successfully.");
    //                //$('#assessmentName').html('');
    //                //$('#assessmentName').html($('input[name="assesmentradio"]:checked').attr("data-text"));
    //            }
    //            else
    //                customAlert(data);
    //        },
    //        error: function (data) { alert(data); }
    //    });
    //});



    $('#btnadd').click(function () {


        cleanValidation();


        debugger;

        if (!($("input:radio[name='assesmentradio']").is(":checked"))) {
            customAlert("Please select assessment type");
            plainValidation('assesmentradio');
            return false;
        }


        var assessmentValue = $("input[name='assesmentradio']:checked").val();



        if (assessment_1_days_from.val().trim() == "") {

            customAlert("Please enter number of days");
            plainValidation(assessment_1_days_from);
            return false;
        }
        if (!annualAssessment.checkNumericOnBlur(assessment_1_days_from)) {
            return false;
        }

        if (assessment_1_days_to.val().trim() == "") {
            customAlert("Please enter number of days");
            plainValidation('#assessment1Todate');
            return false;
        }
        if (!annualAssessment.checkNumericOnBlur(assessment_1_days_to)) {
            return false;
        }
        if (parseInt(assessment_1_days_to.val()) < parseInt(assessment_1_days_from.val())) {
            customAlert(' "Days Enrolled To" for Assessment 1, should be greater than or equal to " Days Enrolled From"');
            plainValidation(assessment_1_days_to);
            return false;
        }




        if (assessmentValue == 2 || assessmentValue == 3) {



            if (assessment_2_days_from.val().trim() == "") {
                customAlert("Please enter number of days");
                plainValidation(assessment_2_days_from);
                return false;
            }
            if (!annualAssessment.checkNumericOnBlur(assessment_2_days_from)) {
                return false;
            }




            if (parseInt(assessment_2_days_from.val()) <= parseInt(assessment_1_days_to.val())) {
                customAlert(' "Days Enrolled From " for Assessment 2, should be greater than Assessment 1 "Days Enrolled To" Days');
                plainValidation(assessment_2_days_from);
                return false;
            }


            if (assessment_2_days_to.val().trim() == "") {
                customAlert("Please enter number of days");
                plainValidation(assessment_2_days_to);
                return false;
            }
            if (!annualAssessment.checkNumericOnBlur(assessment_2_days_to)) {
                return false;
            }

            if (parseInt(assessment_2_days_to.val()) < parseInt(assessment_2_days_from.val())) {
                customAlert('"Days Enrolled To" for Assessment 2, should be greater than Assessment 2 "Days Enrolled From"');
                plainValidation(assessment_2_days_to);
                return false;
            }

            if (assessmentValue == 3) {

                if (assessment_3_days_from.val().trim() == "") {
                    customAlert("Please enter number of days");
                    plainValidation(assessment_3_days_from);
                    return false;
                }

                if (!annualAssessment.checkNumericOnBlur(assessment_3_days_from)) {
                    return false;
                }


                if (parseInt(assessment_3_days_from.val()) <= parseInt(assessment_2_days_to.val())) {
                    customAlert('"Days Enrolled From" for Assessment 3, should greater than Assessment 2 "Days Enrolled To"');
                    plainValidation(assessment_3_days_from);
                    return false;
                }


                if (assessment_3_days_to.val().trim() == "") {
                    customAlert("Please enter number of days");
                    plainValidation(assessment_3_days_to);
                    return false;
                }

                if (!annualAssessment.checkNumericOnBlur(assessment_3_days_to)) {
                    return false;

                }

                if (parseInt(assessment_3_days_to.val()) < parseInt(assessment_3_days_from.val())) {

                    customAlert('"Days Enrolled To" for Assessment 3, should be greater than Assessment 3 "Days Enrolled From"');
                    plainValidation(assessment_3_days_to);
                    return false;
                }



            }

        }



        //if (!isUpdate) {
        //    if ($("input:radio[name='assesmentradio']:checked").val() == 1) {
        //        if (checkvalueFromdate1 < CurrentDate) {
        //            customAlert("Assessment From Date Must Be is Greater Than Or Equal Current Date");
        //            plainValidation('#assessment1Fromdate');
        //            return false;
        //        }
        //        else if (checkvalueTodate1 < checkvalueFromdate1) {
        //            cleanValidation();
        //            customAlert("Assessment To Date is Greater Than  From Date");
        //            plainValidation('#assessment1Todate');
        //            return false;
        //        }
        //    }
        //    if ($("input:radio[name='assesmentradio']:checked").val() == 2) {

        //        if (checkvalueFromdate1 < CurrentDate) {
        //            cleanValidation();
        //            customAlert("Assessment From Date Must Be is Greater Than Or Equal Current Date");
        //            plainValidation('#assessment1Fromdate');
        //            return false;
        //        }
        //        else if (checkvalueTodate1 > checkvalueFromdate1) {
        //            cleanValidation();
        //            customAlert("Assessment To Date is Greater Than From Date");
        //            plainValidation('#assessment1Todate');
        //            return false;
        //        }

        //        else if (checkvalueTodate1 > checkvalueFromdate2) {
        //            cleanValidation();
        //            customAlert("Assessment 2 From Date is Greater Than From Assessment 1 To Date");
        //            plainValidation('#assessment2Fromdate');
        //            return false;
        //        }

        //        else if (checkvalueFromdate2 < checkvalueTodate2) {
        //            cleanValidation();
        //            customAlert("Assessment 2 To Date is Less Than Assessment 2 From Date");
        //            plainValidation('#assessment2Todate');
        //            return false;
        //        }
        //    }

        //    if ($("input:radio[name='assesmentradio']:checked").val() == 3) {

        //        var date1 = new Date(checkvalueTodate1);

        //        var date2 = new Date(checkvalueFromdate2);
        //        if (checkvalueFromdate1 < CurrentDate) {
        //            cleanValidation();
        //            customAlert("Assessment From Date Must Be is Greater Than Or Equal Current Date");
        //            plainValidation('#assessment1Fromdate');
        //            return false;
        //        }
        //        else if (checkvalueTodate1 < checkvalueFromdate1) {
        //            cleanValidation();
        //            customAlert("Assessment To Date is Less Than From Date");
        //            plainValidation('#assessment1Todate');
        //            return false;
        //        }

        //        else if (checkvalueTodate1 > checkvalueFromdate2) {
        //            cleanValidation();
        //            customAlert("Assessment 2 FromDate is Less Than From Assessment 1 ToDate");
        //            plainValidation('#assessment2Fromdate');
        //            return false;
        //        }

        //        else if (checkvalueFromdate2 > checkvalueTodate2) {
        //            cleanValidation();
        //            customAlert("Assessment 2 To Date is Less Than Assessment 2 From Date");
        //            plainValidation('#assessment2Todate');
        //            return false;
        //        }
        //        else if (checkvalueTodate2 > checkvalueFromdate3) {
        //            cleanValidation();
        //            customAlert("Assessment 3 From Date is Less Than Assessment 2 To Date");
        //            plainValidation('#assessment3Fromdate');
        //            return false;
        //        }
        //        else if (checkvalueFromdate3 > checkvalueTodate3) {
        //            cleanValidation();
        //            customAlert("Assessment 3 From Date is Greater Than To Date");
        //            plainValidation('#assessment3Todate');
        //            return false;
        //        }
        //    }
        //}


        if (assessmentValue == 1) {
            assessment_2_days_from.val('');
            assessment_2_days_to.val('');
            assessment_3_days_from.val('');
            assessment_3_days_to.val('');

        }


        if (assessmentValue == 2) {
            assessment_3_days_from.val('');
            assessment_3_days_to.val('');
        }

        var assessment = {};

        assessment.AnnualAssessmentType = parseInt(assessmentValue);

        assessment.Assessment1From = assessment_1_days_from.val().trim();
        assessment.Assessment1To = assessment_1_days_to.val().trim();

        assessment.Assessment2From = (assessment_2_days_from.val().trim() == "") ? null : assessment_2_days_from.val().trim();
        assessment.Assessment2To = (assessment_2_days_to.val().trim() == "") ? null : assessment_2_days_to.val().trim();

        assessment.Assessment3From = (assessment_3_days_from.val().trim() == "") ? null : assessment_3_days_from.val().trim();
        assessment.Assessment3To = (assessment_3_days_to.val().trim() == "") ? null : assessment_3_days_to.val().trim();





        $.ajax({

            url: HostedDir + "/Matrix/AddAnnualAssessment",
            dataType: 'json',
            type: "POST",
            async: false,
            data: assessment,
            success: function (data) {
                if (data != null) {

                    cleanValidation();
                    if (data.isResult == true) {

                        customAlert("Record saved successfully.");
                    }

                    else if (data.isResult == false && data.errorType > 0) {
                        if (data.errorType == 1 || data.errorType == 2) {
                            customAlert(' "Days Enrolled To" for Assessment 1, should be greater than or equal to " Days Enrolled From"');
                            plainValidation(assessment_1_days_to);
                        }

                        else if (data.errorType == 3) {
                            customAlert(' "Days Enrolled From " for Assessment 2, should be greater than Assessment 1 "Days Enrolled To" Days');
                            plainValidation(assessment_2_days_from);
                        }

                        else if (data.errorType == 4) {
                            customAlert('"Days Enrolled To" for Assessment 2, should be greater than Assessment 2 "Days Enrolled From"');
                            plainValidation(assessment_2_days_to);
                        }

                        else if (data.errorType == 5) {
                            customAlert('"Days Enrolled From" for Assessment 3, should greater than Assessment 2 "Days Enrolled To"');
                            plainValidation(assessment_3_days_from);
                        }

                        else if (data.errorType == 6) {
                            customAlert('"Days Enrolled To" for Assessment 3, should be greater than Assessment 3 "Days Enrolled From"');
                            plainValidation(assessment_3_days_to);
                        }
                    }
                }

                else {
                    customAlert("Error occurred.Please, try again later");
                }


            },
            error: function (data) {

                customAlert("Session Ended Log Onto The System Again."); setTimeout(function () { window.location.href = HostedDir + '/login/Loginagency'; }, 2000);

            }
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


    $('.days-num').on('input', function () {

        return annualAssessment.checkNumericOn(this);

    });
});
