


var sortOrder = '';
var sortDirection = '';
var imgID = '';
var direction = '';
var pageSize = 50;
var requestedPage = 1;
var pageLoadedFirst = true;
var totalRecords = 0;
var numOfPages = 0;
var StartIndex = 0;
var LastIndex = 0;
var search = '';

var transitionClients = {

   
    getClassrooms: function (ele) {

        $.ajax({

            url: HostedDir + '/AgencyUser/GetClassroomsWithFSWHVByCenter',
            beforeSend: function () { $('#spinner').show() },
            data: { centerId: $(ele).val() },
            datatype: 'json',
            type: 'post',
            success: function (data) {
                var appendClas = '';
                var appendstaff = '';
                if (data != null && data.classroomList != null && data.classroomList.length > 0) {

                    appendClas += '<option value="0">--Select Classroom--</option>';
                    $.each(data.classroomList, function (i, clsroom) {
                        appendClas += '<option value=' + clsroom.Enc_ClassRoomId + '>' + clsroom.ClassName + '</option>';
                    });
                }
                else {
                    customAlert('Some Error Occured.Please, try again later');
                }
                if (data != null && data.stafflist != null && data.stafflist.length > 0) {

                    appendstaff += '<option value="0">--Select--</option>';
                    $.each(data.stafflist, function (j, staff) {

                        appendstaff += '<option value=' + staff.Value + '>' + staff.Text + '</option>';
                    });
                }
                $('#widClassroomSelect').html(appendClas);
                $('#fswHvselect').html(appendstaff);


            },
            error: function (data) {
                $('#spinner').hide();
                customAlert("Session Ended Log Onto The System Again."); setTimeout(function () { window.location.href = HostedDir + '/login/Loginagency'; }, 2000);

            },

            complete: function (data) {
                $('#spinner').hide();
            }
        });
    },
    uncheckdata: function () {
        if ($('#NONE').prop("checked")) {
            $('#TANF').prop("checked", false);
            $('#SSI').prop("checked", false);
            $('#WIC').prop("checked", false);
            $('#SNAP').prop("checked", false);
        }
    },
    uncheckdataNone: function () {
        if (($('#TANF').prop("checked")) || ($('#SSI').prop("checked")) || ($('#WIC').prop("checked")) || ($('#SNAP').prop("checked"))) {
            $('#NONE').prop("checked", false);
        }
    },
    ClosePopUp: function () {
        debugger;
        $(".question-edu").html("");
        $(".newlist").hide();
    },
    getTotalRecord: function (data) {

        var pageSize = 0;
        var reqPage = 0;
        $('#First').attr('disabled', false);
        $('#Back').attr('disabled', false);
        $('#Next').attr('disabled', false);
        $('#Last').attr('disabled', false);
        pageSize = parseInt($('#ddlpagetodisplay').val());

        if (data > 0) {
            totalRecords = parseInt(data);
            if (totalRecords <= pageSize) {
                $('#First').attr('disabled', true);
                $('#Back').attr('disabled', true);
                $('#Next').attr('disabled', true);
                $('#Last').attr('disabled', true);
            }
            numOfPages = parseInt(totalRecords / pageSize) + ((totalRecords % pageSize == 0) ? 0 : 1);
            $("#ddlpaging").empty()
            for (i = 1; i <= numOfPages; i++) {
                var newOption = "<option value='" + i + "'>" + i + "</option>";
                $("#ddlpaging").append(newOption);
            }



            if (requestedPage == 1 && (totalRecords>pageSize)) {
                $('#First').attr('disabled', true);
                $('#Back').attr('disabled', true);
                $('#Next').attr('disabled', false);
                $('#Last').attr('disabled', false);
            }
            else if (requestedPage == numOfPages) {
                $('#First').attr('disabled', false);
                $('#Back').attr('disabled', false);
                $('#Next').attr('disabled', true);
                $('#Last').attr('disabled', true);
            }
            else {
                $('#First').attr('disabled', false);
                $('#Back').attr('disabled', false);
                $('#Next').attr('disabled', false);
                $('#Last').attr('disabled', false);
            }


            $("#ddlpaging").val(requestedPage);
        }

        else {
            $('#First').attr('disabled', true);
            $('#Back').attr('disabled', true);
            $('#Next').attr('disabled', true);
            $('#Last').attr('disabled', true);
        }

        $("#ddlpaging").val(requestedPage);
    },
    SearchByCenter: function (type, mode) {

        var centerid = "";
        var classroomid = "";
        var fswid = "";
        var searchtext = "";
        var reqPage = 1;
        pageSize = $('#ddlpagetodisplay').val() == null || $('#ddlpagetodisplay').val() == '' ? 10 : parseInt($('#ddlpagetodisplay').val());
       
        if (type == 1) {
            
            centerid = $('#widCenter').val();
            classroomid = $('#widClassroomSelect').val();
            fswid = $('#fswHvselect').val();
            $('#SearchName').val("");
        }
        if (type = 2) {
            searchtext = $('#SearchName').val();
         
        }
        $.ajax({
            url: (isTransition == 0) ? HostedDir + '/AgencyUser/GetWithdrawnListByCenter' : HostedDir + '/AgencyUser/GetTransitionListByCenter',
            type: "POST",
            datatype: 'json',
            data: {
                "CenterId": centerid,
                "ClassRoomID": classroomid,
                "FSWId": fswid,
                "SearchText": searchtext,
                "reqPage": requestedPage,
                "pgSize": pageSize
            },
            beforeSend: function () { $('#spinner').show() },
            success: function (response) {

                $("#partialDiv").html("");
                $("#partialDiv").html(response);
                transitionClients.getTotalRecord($("#clientCount").text());
                if (type == 1) {
                    $('#widCenter').val(centerid);
                    $('#widClassroomSelect').val(classroomid);
                    $('#fswHvselect').val(fswid);
                }
            },
            error: function () {

            },
            complete: function (response) {

                console.log(response);
                $('#spinner').hide();

                if (response.statusText == "OK") {

                    var clas = '#paginationDiv';

                    if (mode == 'First') {

                        $(clas).find('#First').attr('disabled', true);
                        $(clas).find('#Back').attr('disabled', true);
                        $(clas).find('#Next').attr('disabled', false);
                        $(clas).find('#Last').attr('disabled', false);
                        LastIndex = 0;
                    }
                    else if (mode == 'Last') {

                        $(clas).find('#First').attr('disabled', false);
                        $(clas).find('#Back').attr('disabled', false);
                        $(clas).find('#Next').attr('disabled', true);
                        $(clas).find('#Last').attr('disabled', true);
                    }
                    else if (mode == 'Next') {

                        $(clas).find('#First').attr('disabled', false);
                        $(clas).find('#Back').attr('disabled', false);
                        if (parseInt(LastIndex) + parseInt(pageSize) >= totalRecords) {
                            $(clas).find('#Next').attr('disabled', true);
                            $(clas).find('#Last').attr('disabled', true);
                        }
                        else if (parseInt(LastIndex) - parseInt(pageSize) < totalRecords) {
                            $(clas).find('#Next').attr('disabled', false);
                            $(clas).find('#Last').attr('disabled', false);
                        }
                    }
                    else if (mode == 'Back') {

                        if (parseInt(LastIndex) + parseInt(pageSize) > totalRecords) {
                            $(clas).find('#Next').attr('disabled', true);
                            $(clas).find('#Last').attr('disabled', true);
                        }
                        else if (parseInt(LastIndex) - parseInt(pageSize) < totalRecords) {
                            $(clas).find('#Next').attr('disabled', false);
                            $(clas).find('#Last').attr('disabled', false);
                        }
                        if (requestedPage == 1) {
                            $(clas).find('#First').attr('disabled', true);
                            $(clas).find('#Back').attr('disabled', true);
                        }
                    }
                    else if (mode == 'records') {
                        LastIndex = 0;
                        $(clas).find('#First').attr('disabled', true);
                        $(clas).find('#Back').attr('disabled', true);
                    }

                }

            }
        });

    },
    SaveQuestions: function (questionNumber, arg) {
        debugger;
        cleanValidation();
        clientid = $(arg).attr('ecid');

        isValid = true;
        let answerRadio = $(arg).parent('.part_edubtn').siblings('.sec-edu-radio').find('input:radio[name=passed-code]');

        switch (questionNumber) {

            case 1:
                if (!answerRadio.is(':checked')) {
                    customAlert('Please select insurance type');
                    isValid = false;
                }
                else if (answerRadio.filter(':checked').val() == '3' && answerRadio.parent('.edu-radio-rai').next('#otherInsuranceDesc').val() == '') {
                    plainValidation('#otherInsuranceDesc')
                    customAlert('Please enter other insurance description');
                    isValid = false;
                }
                break;

            case 2:

                if (!answerRadio.is(':checked')) {

                    if (ispreg == "True") {
                        customAlert('Please select Yes or No for dental exam');

                    }
                    else {
                        customAlert('Please select Yes or No');
                    }
                    isValid = false;
                }
                break;
            case 3:
                if (!answerRadio.is(':checked')) {
                    customAlert('Please select Yes or No');
                    isValid = false;
                }

            case 8:
                var error = 0;
                $(arg).parent('.part_edubtn').siblings('.sec-edu-radio').each(function (i, edu) {
                    if ($(edu).is(':visible') && i == 0 && $(edu).find('input:radio[name=passed-code]:checked').length == 0) {
                        error++;
                    }

                    if ($(edu).is(':visible') && i == 1 && $(edu).find('input:radio[name=passed-code1]:checked').length == 0) {
                        error++;
                    }

                });

                if (error > 0) {
                    isValid = false;
                    customAlert('Please select education type');
                }
                break;
            case 9:

                var error = 0;

                $(arg).parent('.part_edubtn').siblings('.sec-edu-radio').each(function (i, edu) {
                    if ($(edu).is(':visible') && i == 0 && $(edu).find('input:radio[name=passed-code]:checked').length == 0) {
                        error++;
                    }

                    if ($(edu).is(':visible') && i == 1 && $(edu).find('input:radio[name=passed-code1]:checked').length == 0) {
                        error++;
                    }

                });

                if (error > 0) {
                    isValid = false;
                    customAlert('Please select Yes or No for Parent job training completed');
                }



        }

        if (isValid) {


            var Transition = {};
            Transition.EClientID = clientid;
            Transition.QuestionNumber = questionNumber;
            Transition.InsuranceType = $('.Question1:checked').val();
            Transition.MedicalHome = ($('.Question2:checked').val() == '1') ? true : false;
            Transition.MedicalServices = ($('.Question3:checked').val() == '1') ? true : false;
            Transition.DentalHome = $('.Question4:checked').val() == '1' ? true : false;
            Transition.DentalServices = $('.Question5:checked').val() == '1' ? true : false;
            Transition.ImmunizationService = parseInt($('.Question6:checked').val());
            Transition.TANF = $('#TANF').is(':checked');
            Transition.WIC = $('#WIC').is(':checked');
            Transition.SSI = $('#SSI').is(':checked');
            Transition.NONE = $('#NONE').is(':checked');
            Transition.Enc_ProgID = $(arg).attr('pid');
            Transition.PMDental = $('.Question11:checked').val() == 'true' ? true : false;
            Transition.IsPreg = ispreg;
            Transition.OtherInsuranceTypeDesc = $('.Question1:checked').val() == '3' ? $('#otherInsuranceDesc').val() : "";

            Transition.ParentID = '0';
            Transition.ParentID2 = '0';
                    
            if (questionNumber == 8) {
                Transition.ParentID = ($('#parentEduDiv').is(':visible')) ? $('#parentEduDiv').find('#parentName').attr('pid') : '0';
                Transition.ParentID2 = ($('#parent1EduDiv').is(':visible')) ? $('#parent1EduDiv').find('#parentName1').attr('pid1') : '0';
            }
            if (questionNumber == 9) {
                Transition.ParentID = ($('#parentJobDiv').is(':visible')) ? $('#parentJobDiv').find('#parentName').attr('pid') : '0';
                Transition.ParentID2 = ($('#parent1JobDiv').is(':visible')) ? $('#parent1JobDiv').find('#parentName1').attr('pid1') : '0';
            }

            //father//
            Transition.ShoolAchievement = ($('#parentEduDiv').is(':visible')) ? $('#parentEduDiv').find('input:radio[name=passed-code]:checked').val() : '0';
            Transition.JobTrainingFinished = ($('#parentJobDiv').is(':visible')) ? $('#parentJobDiv').find('imput:radio[name=passed-code]:checked').val() == '1' ? true : false : false;


            //mother//
            Transition.ShoolAchievement2 = ($('#parent1EduDiv').is(':visible')) ? $('#parent1EduDiv').find('input:radio[name=passed-code1]:checked').val() : '0';
            Transition.JobTrainingFinished2 = ($('#parent1JobDiv').is(':visible')) ? $('#parent1JobDiv').find('imput:radio[name=passed-code1]:checked').val() == '1' ? true : false : false;


            console.log(JSON.stringify(Transition));
            $.ajax({
                url: HostedDir + '/AgencyUser/SaveWithdrawnQuestion',
                type: "POST",
                datatype: 'json',
                beforSend: function () { $('#spinner').show() },
                data: { transition: JSON.stringify(Transition) },

                success: function (response) {

                    if (response) {
                        customAlert('Record updated successfully');
                        $(arg).closest('td').addClass("passed");
                        $(".question-edu").html("");
                        $(".newlist").hide();
                    }
                    else {
                        customAlert('Some error occured.Please,try again later');
                    }



                },
                error: function () {

                },
                complete: function () {

                    $('#spinner').hide();
                }
            });
        }

    },

    fnChangePage: function (val) {
        pageLoadedFirst = false;
        pageSize = $('#paginationDiv').find('#ddlpagetodisplay').val();

        if (val == 'First') {
            StartIndex = 0;
            LastIndex = parseInt(pageSize) + parseInt(LastIndex * requestedPage);
            requestedPage = ((StartIndex / 10) + 1);
            this.GoToNextPage(requestedPage, pageSize, val);
           
        }
        else if (val == 'Last') {
            StartIndex = parseInt((totalRecords - 1) / pageSize) * pageSize;
            LastIndex = totalRecords;
            requestedPage = numOfPages;
            this.GoToNextPage(requestedPage, pageSize, val);
            
        }
        else if (val == 'Next') {
            LastIndex = parseInt(pageSize) + parseInt(LastIndex);
            requestedPage = (parseInt(LastIndex / pageSize) + 1);
            this.GoToNextPage(requestedPage, pageSize, val);
           
        }
        else if (val == 'Back') {
            requestedPage = requestedPage - 1;
            LastIndex = parseInt(LastIndex) - parseInt(pageSize);
            this.GoToNextPage(requestedPage, pageSize, val);
           
        }
        else {
        }
    },

    getListafterupdation: function () {

        pageSize = $('#ddlpagetodisplay').val();
        requestedPage = $('#ddlpaging').val();
        StartIndex = (pageSize * (requestedPage - 1)) + 1;
        LastIndex = parseInt(pageSize * requestedPage) - parseInt(pageSize);
        $('#GridRoster > thead > tr > th > img').css("visibility", "hidden");
        if (imgID != '' && imgID != 'undefined' && imgID != null) {
            direction = $("#" + imgID).siblings('input').val();
        }
        if (direction == "Asc") {
            sortDirection = $("#" + imgID).siblings('input').val();
        } else if (direction == "Desc") {
            sortDirection = $("#" + imgID).siblings('input').val();
        }
        this.SearchByCenter(1);

    },

    GoToNextPage: function (requestedPage, pageSize, mode) {

        this.SearchByCenter(1, mode);


    },
    drawgrid: function () {

        requestedPage = 1;


        this.SearchByCenter(1, 'records')
      
    },
    AutoComplete: function (arg) {
        try {
            $(arg).autocomplete({
                minLength: 1,
                source: function (request, response) {
                    $.ajax({
                        url: HostedDir + "/AgencyUser/AutoCompleteWithdrawnList",
                        type: "POST",
                        dataType: "json",
                        data: { term: request.term, trans: (isTransition==1)?1:0 },
                        success: function (data) {
                            response($.map(data, function (item) {
                                return { label: item, id: item };
                            }))
                        }
                    })
                },
                select: function (event, ui) {
                    $(arg).val(ui.item.id);
                    transitionClients.SearchByCenter(2);
                },
                messages: {
                    noResults: "No Result Found", results: ""
                }
            })

        }
        catch (err) {
        }

    },
    ViewClientDetails: function (arg) {
        $('.newlist').hide();
        $(arg).parent('tr').toggleClass('open').next('tr').toggleClass("open");

    },
    ViewQuestions: function (arg) {
        debugger;
        if (!($(arg).parent('td').find(".newlist").is(":visible"))) {
            $(".newlist").hide();
            $(".question-edu").html("");

            var qnvalue = $(arg).parent('td').children('span:nth-child(1)').text();
            clientid = $(arg).parent('td').closest('tr').find('.clientid').val();
            ispreg = $(arg).parent('td').closest('tr').find('.isPreg').val();
            enc_prgID = $(arg).parent('td').closest('tr').find('.enc-prg-id').val();
            var questionNumber = qnvalue.slice(1);

            $.ajax({
                url: HostedDir + '/AgencyUser/GetPIRQuestionAnswer',
                type: "POST",
                datatype: 'json',
                data: {
                    "ClientId": clientid,
                    "QuestionNumber": questionNumber,
                    "IsPregMom": ispreg,
                    "programTypeID": enc_prgID
                },
                success: function (response) {
                    transitionClients.AssignValue(arg, response, qnvalue, ispreg);
                },
                error: function () {
                }
            });

        }

    },
    AssignValue: function (arg, res, qnvalue, ispreg) {

        var newList = $(arg).parent('td').find(".newlist");

        var questions = transitionClients.WithdrawalQuestions;

        switch (qnvalue) {
            case "Q1":
                newList.children('.question-edu').html(questions.WQ1);
                if (res.InsuranceTypeQ1End != '0') {
                    newList.children('.question-edu').find('input:radio[value=' + res.InsuranceTypeQ1End + ']').prop('checked', true);

                    if (res.InsuranceTypeQ1End == '3') {
                        newList.children('.question-edu').find('#otherInsuranceDesc').val(res.DescInsurancTypeQ1End);
                        newList.children('.question-edu').find('#otherInsuranceDesc').show();
                    }
                }
                else if (res.InsuranceTypeQ1Start != '0') {
                    newList.children('.question-edu').find('input:radio[value=' + res.InsuranceTypeQ1Start + ']').prop('checked', true);
                    if (res.InsuranceTypeQ1Start == '3') {
                        newList.children('.question-edu').find('#otherInsuranceDesc').val(res.DescInsurancTypeQ1Start);
                        newList.children('.question-edu').find('#otherInsuranceDesc').show();
                    }
                }
                else {
                    newList.children('.question-edu').find('input:radio[value=2]').prop('checked', true);
                }

                newList.children('.question-edu').find('input:radio').unbind('click');

                newList.children('.question-edu').find('input:radio').on('click', function () {

                    if ($(this).val() == '3') {
                        newList.children('.question-edu').find('#otherInsuranceDesc').show();
                    }
                    else {
                        newList.children('.question-edu').find('#otherInsuranceDesc').hide();
                    }

                });
                break;

            case "Q2":


                if (ispreg == 'True') {
                    newList.children('.question-edu').html(questions.WQ10);

                    newList.children('.question-edu').find('input:radio[name=passed-code][value=' + res.DentalServiceQ5Start + ']').prop('checked', true);
                }

                else {
                    newList.children('.question-edu').html(questions.WQ2);

                    newList.children('.question-edu').find('input:radio[name=passed-code][value=' + res.MedicalHomeQ2Start + ']').prop('checked', true);
                }

                break;

            case "Q3":

                newList.children('.question-edu').html(questions.WQ3);

                newList.children('.question-edu').find('input:radio[name=passed-code][value=' + res.MedicalServiceQ3Start + ']').prop('checked', true);

                break;

            case "Q4":
                newList.children('.question-edu').html(questions.WQ4);

                newList.children('.question-edu').find('input:radio[name=passed-code][value=' + res.DentalHomeQ4Start + ']').prop('checked', true);

                break;

            case "Q5":

                newList.children('.question-edu').html(questions.WQ5);

                newList.children('.question-edu').find('input:radio[name=passed-code][value=' + res.DentalServiceQ5Start + ']').prop('checked', true);


                break;

            case "Q6":
                newList.children('.question-edu').html(questions.WQ6);

                newList.children('.question-edu').find('input:radio[name=passed-code][value=' + res.ImmunizationQ6Start + ']').prop('checked', true);
                break;

            case "Q7":
                newList.children('.question-edu').html(questions.WQ7);


                if (res.FamilyServiceTANFQ7Start == "1") {
                    newList.children('.question-edu').find('input:checkbox[name=TANF]').prop('checked', true);
                }


                if (res.FamilyServiceSSIQ7Start == "1") {
                    newList.children('.question-edu').find('input:checkbox[name=SSI]').prop('checked', true);
                }

                if (res.FamilyServiceWICQ7Start == "1") {
                    newList.children('.question-edu').find('input:checkbox[name=WIC]').prop('checked', true);
                }

                if (res.FamilyServiceSNAPQ7Start == "1") {
                    newList.children('.question-edu').find('input:checkbox[name=SNAP]').prop('checked', true);
                }


                if (res.FamilyServiceNoneQ7Start == "1") {
                    newList.children('.question-edu').find('input:checkbox[name=NONE]').prop('checked', true);
                }

                break;

            case "Q8":

                newList.children('.question-edu').html(questions.WQ8);
                newList.children('.question-edu').find('#parent1EduDiv').find('input:radio').prop('checked', false);
                newList.children('.question-edu').find('#parentEduDiv').find('input:radio').prop('checked', false);
                newList.children('.question-edu').find('#parent1EduDiv').hide();
                newList.children('.question-edu').find('#parentEduDiv').hide();

                if (res.ParentName != '' && res.ParentID != '') {
                    newList.children('.question-edu').find('#parentEduDiv').find('#parentName').html(res.ParentName);
                    newList.children('.question-edu').find('#parentEduDiv').find('#parentName').attr('pid', res.ParentID);
                    newList.children('.question-edu').find('#parentEduDiv').find('input:radio[name=passed-code][value=' + res.EducationQ8Start + ']').prop('checked', true);
                    newList.children('.question-edu').find('#parentEduDiv').show();
                }

                if (res.ParentName1 != '' && res.ParentID1 != '') {

                    newList.children('.question-edu').find('#parent1EduDiv').show();
                    newList.children('.question-edu').find('#parent1EduDiv').find('#parentName1').html(res.ParentName1);
                    newList.children('.question-edu').find('#parent1EduDiv').find('#parentName1').attr('pid1', res.ParentID1);
                    newList.children('.question-edu').find('#parent1EduDiv').find('input:radio[name=passed-code1][value=' + res.EducationQ8Start1 + ']').prop('checked', true);
                }

                if (res.ParentName != '' && res.ParentID != '' && res.ParentName1 != '' && res.ParentID1 != '') {
                    newList.children('.question-edu').css({ 'width': '630px', 'left': '-495px' });
                    newList.children('.question-edu').find('.sec-edu-radio').css({ 'width': '48%', 'margin': '1px' });
                }

                break;

            case "Q9":
                newList.children('.question-edu').html(questions.WQ9);

                newList.children('.question-edu').find('#parent1JobDiv').find('input:radio').prop('checked', false);
                newList.children('.question-edu').find('#parent1JobDiv').hide();

                newList.children('.question-edu').find('#parentJobDiv').find('input:radio').prop('checked', false);
                newList.children('.question-edu').find('#parentJobDiv').hide();

                if (res.ParentName != '' && res.ParentID != '') {
                    newList.children('.question-edu').find('#parentJobDiv').find('#parentName').html(res.ParentName);
                    newList.children('.question-edu').find('#parentJobDiv').find('#parentName').attr('pid', res.ParentID);
                    newList.children('.question-edu').find('#parentJobDiv').find('input:radio[name=passed-code][value=' + res.JobTrainingCompletedQ9Start + ']').prop('checked', true);
                    newList.children('.question-edu').find('#parentJobDiv').show();
                }

                if (res.ParentName1 != '' && res.ParentID1 != '') {


                    newList.children('.question-edu').find('#parent1JobDiv').show();
                    newList.children('.question-edu').find('#parent1JobDiv').find('#parentName1').html(res.ParentName1);
                    newList.children('.question-edu').find('#parent1JobDiv').find('#parentName1').attr('pid1', res.ParentID1);
                    newList.children('.question-edu').find('#parent1JobDiv').find('input:radio[name=passed-code1][value=' + res.JobTrainingCompletedQ9Start1 + ']').prop('checked', true);
                }

                if (res.ParentName != '' && res.ParentID != '' && res.ParentName1 != '' && res.ParentID1 != '') {
                    newList.children('.question-edu').css({ 'width': '630px', 'left': '-495px' });
                    newList.children('.question-edu').find('.sec-edu-radio').css({ 'width': '48%', 'margin': '1px' });
                }
                break;
            default:
                newList.children('.question-edu').html('');
                break;

        }


        $(arg).parent('td').find('.newlist').find('.part_edubtn').children('.btn-edu-sucess').attr({ 'ecid': res.Enc_ClientID, 'pid': res.ProgramTypeID });

        $(arg).parent('td').find(".newlist").show();
    },

    WithdrawalQuestions: {

        WQ1: '<div class="question-edu-inner"><p class="qn-p">\
                Q1. Insurance Type</p><div class="sec-edu-radio">\
                                                    <div class="form-group edu-radio-rai ful-width-edu">\
                                                        <input  type="radio" id="Medicare" class="Question1" value="1"  name="passed-code">\
                                                        <label for="Medicare">Medicare / Chip</label>\
                                                    </div>\
                                                    <div class="form-group edu-radio-rai ful-width-edu">\
                                                        <input type="radio" id="NoIns" checked="" class="Question1" value="2" name="passed-code">\
                                                        <label for="NoIns">No Insurance</label>\
                                                    </div>\
                                                    <div class="form-group edu-radio-rai ful-width-edu">\
                                                        <input type="radio" id="OtherIns" class="Question1" value="3" name="passed-code">\
                                                        <label for="OtherIns">Other Insurance</label>\
                                                    </div>\
                                                      <input type="text" id="otherInsuranceDesc" class="form-control" placeholder="Other Insurance" style="width:91%;margin-left:10px;display:none;">\
                                                    <div class="form-group edu-radio-rai ful-width-edu">\
                                                        <input type="radio" id="HealthIns" class="Question1"  value="4" name="passed-code">\
                                                        <label for="HealthIns">Private Health Insurance</label>\
                                                    </div>\
                                                         <div class="form-group edu-radio-rai ful-width-edu">\
                                                        <input type="radio" id="StateIns" class="Question1" value="5" name="passed-code">\
                                                        <label for="StateIns">State Insurance</label>\
                                                    </div>\
                                                </div>\
                                                <div class="part_edubtn">\
                                                    <button  onclick="transitionClients.SaveQuestions(1,this)" class="btn btn-edu-sucess">Accept</button>\
                                                    <button onclick="transitionClients.ClosePopUp()" class="btn btn-edu-exit">Cancel</button>\
                                                </div>\
                                            </div>',

        WQ2: '<div class="question-edu-inner"><p class="qn-p">Q2. Ongoing source continuous, accessible health care?</p>\
                                        <div class="sec-edu-radio">\
                                            <div class="form-group edu-radio-rai">\
                                                <input  type="radio" class="Question2" id="answer1" value="1" name="passed-code">\
                                                <label for="answer1">Yes</label>\
                                            </div>\
                                            <div class="form-group edu-radio-rai">\
                                                <input type="radio" id="answer0" class="Question2"  value="0"  name="passed-code">\
                                                <label for="answer0">No</label>\
                                            </div>\
                                        </div>\
                                        <div class="part_edubtn">\
                                                    <button  onclick="transitionClients.SaveQuestions(2,this)" class="btn btn-edu-sucess">Accept</button>\
                                                    <button onclick="transitionClients.ClosePopUp()" class="btn btn-edu-exit">Cancel</button>\
                                                </div>\
                                    </div>',

        WQ3: '<div class="question-edu-inner"><p style="margin-top:  20px;">\
    <p class="qn-p">\
         Q3. Is the child up-to-date on a schedule of age-appropriate preventive and primary health care, according to the relevant state’s EPSDT schedule for well child care?</p>\
</p><div class="sec-edu-radio">\
                               <div class="form-group edu-radio-rai">\
                                   <input  type="radio"  id="answer1" class="Question3" value="1" name="passed-code">\
                                   <label for="answer1">Yes</label>\
                               </div>\
                               <div class="form-group edu-radio-rai">\
                                   <input type="radio"  id="answer0" class="Question3" value="0" name="passed-code">\
                                   <label for="answer0">No</label>\
                               </div>\
                           </div>\
                            <div class="part_edubtn">\
                                                    <button  onclick="transitionClients.SaveQuestions(3,this)" class="btn btn-edu-sucess">Accept</button>\
                                                    <button onclick="transitionClients.ClosePopUp()" class="btn btn-edu-exit">Cancel</button>\
                                                </div>\
                       </div>',

        WQ4: '<div class="question-edu-inner"><p style="margin-top:20px;">\
    <p class="qn-p">\
         Q4. Did child have continual accessible dental care provided by a dentist?</p>\
</p><div class="sec-edu-radio">\
                               <div class="form-group edu-radio-rai">\
                                   <input  type="radio"  id="answer1" class="Question4" value="1" name="passed-code">\
                                   <label for="answer1">Yes</label>\
                               </div>\
                               <div class="form-group edu-radio-rai">\
                                   <input type="radio" id="answer0" class="Question4" value="0" name="passed-code">\
                                   <label for="answer0">No</label>\
                               </div>\
                           </div>\
                          <div class="part_edubtn">\
                                                    <button  onclick="transitionClients.SaveQuestions(4,this)" class="btn btn-edu-sucess">Accept</button>\
                                                    <button onclick="transitionClients.ClosePopUp()" class="btn btn-edu-exit">Cancel</button>\
                                                </div>\
                       </div>',

        WQ5: '<div class="question-edu-inner">\
                     <p class="qn-p">Q5. Up-to-date on a schedule of age-appropriate preventive and primary oral health care according to the relevant state’s EPSDT schedule?\
                                 </p><div class="sec-edu-radio">\
                                                 <div class="form-group edu-radio-rai">\
                                                     <input type="radio" id="answer1" class="Question5" value="1" name="passed-code">\
                                                     <label for="answer1">Yes</label>\
                                                 </div>\
                                                 <div class="form-group edu-radio-rai">\
                                                     <input type="radio"id="answer0" class="Question5" value="0" name="passed-code">\
                                                     <label for=answer0">No</label>\
                                                 </div>\
                                             </div>\
                                             <div class="part_edubtn">\
                                                    <button  onclick="transitionClients.SaveQuestions(5,this)" class="btn btn-edu-sucess">Accept</button>\
                                                    <button onclick="transitionClients.ClosePopUp()" class="btn btn-edu-exit">Cancel</button>\
                                                </div>\
                                         </div>',
        WQ6: '<div class="question-edu-inner"><p class="qn-p">\
         Q6.  Is Child up-to-date on all immunizations appropriate for their age?\
         </p><div class="sec-edu-radio">\
                                                 <div class="form-group edu-radio-rai col-xs-12">\
                                                     <input type="radio" class="Question6" id="answer1" value="1" name="passed-code">\
                                                     <label for="answer1">Yes</label>\
                                                 </div>\
         <div class="form-group edu-radio-rai col-xs-12" style="width:100%;">\
                                                     <input type="radio"  class="Question6" id="answer2"  value="2" name="passed-code">\
                                                     <label for="answer2">Exempt</label>\
                                                 </div><div class="form-group form-group-change edu-radio-rai" style="width: 100%;">\
                                                     <input type="radio"  class="Question6" id="answer3"  value="3" name="passed-code">\
                                                     <label for="answer3">Child has not received all immunizations appropriate for their age</label>\
                                                 </div>\
                                             </div>\
                                          <div class="part_edubtn">\
                                                    <button  onclick="transitionClients.SaveQuestions(6,this)" class="btn btn-edu-sucess">Accept</button>\
                                                    <button onclick="transitionClients.ClosePopUp()" class="btn btn-edu-exit">Cancel</button>\
                                                </div>\
                                         </div>',

        WQ7: '<div class="question-edu-inner"><p class="qn-p">\
        Q7.  Is the family receiving any of the following services?\
        </p><div class="sec-edu-radio family-sr-rad"> <div class="col-xs-6 col-sm-2">\
                                                                <label class="checkbox-inline">\
                                                                    <input type="checkbox" id="TANF" name="TANF" class="services" onchange="transitionClients.uncheckdataNone();" value="true">\
                                                                    <span style="color:  #000;">TANF</span>\
                                                                </label>\
                                                            </div>\
                                                            <div class="col-xs-6 col-sm-2">\
                                                                <label class="checkbox-inline">\
                                                                    <input type="checkbox" id="SSI" name="SSI" class="services" onchange="transitionClients.uncheckdataNone();" value="true">\
                                                                    <span style="color: #000;">SSI</span>\
                                                                </label>\
                                                            </div>\
                                                            <div class="col-xs-6 col-sm-2">\
                                                                <label class="checkbox-inline">\
                                                                    <input type="checkbox" id="SNAP" name="SNAP" class="services" onchange="transitionClients.uncheckdataNone();" value="true">\
                                                                    <span style="color: #000;">SNAP</span>\
                                                                </label>\
                                                            </div>\
                                                            <div class="col-xs-6 col-sm-2">\
                                                                <label class="checkbox-inline">\
                                                                    <input type="checkbox" id="WIC" name="WIC" class="services" onchange="transitionClients.uncheckdataNone();" value="true">\
                                                                    <span style="color: #000;">WIC</span>\
                                                                </label>\
                                                            </div>\
                                                            <div class="col-xs-6 col-sm-2">\
                                                                <label class="checkbox-inline">\
                                                                    <input type="checkbox" id="NONE" onchange="transitionClients.uncheckdata();" name="NONE" class="services" value="true">\
                                                                    <span style="color: #000;">NONE</span>\
                                                                </label>\
                                                            </div>\
                                                                </div>\
                                          <div class="part_edubtn">\
                                                    <button  onclick="transitionClients.SaveQuestions(7,this)" class="btn btn-edu-sucess">Accept</button>\
                                                    <button onclick="transitionClients.ClosePopUp()" class="btn btn-edu-exit">Cancel</button>\
                                                </div>\
                                        </div>',
        WQ8: '<div class="question-edu-inner"><p class="qn-p">\
                Q8. Has any parent/guardian completed the following (education)?</p>\
                                    <div class="sec-edu-radio" id="parentEduDiv">\
    <div class="col-xs-12" style="padding-top:  10px;background: #8a6d3b;color:  #fff;"><p id="parentName"></p></div>\
                                        <div class="form-group edu-radio-rai" style="width: 100%;">\
                                                    <input class="Question8 allQues" type="radio" id="answer01" value="1" name="passed-code">\
                                                    <label for="answer01">Baccalaureate or Advanced Degree</label>\
                                                </div>\
        <div class="form-group edu-radio-rai" style="width:  100%;">\
                                                    <input type="radio" id="answer02" class="Question8 allQues"  value="2" name="passed-code">\
                                                    <label for="answer02">Associate Degree</label>\
                                                </div>\
                                                <div class="form-group edu-radio-rai" style="width:100%;">\
                                                    <input type="radio" id="answer03" class="Question8 allQues"  value="3" name="passed-code">\
                                                    <label for="answer03">GED or High School Graduate</label>\
                                                </div>\
        <div class="form-group edu-radio-rai" style="width:100%;">\
                                                    <input type="radio" id="answer04" class="Question8 allQues"   value="4" name="passed-code">\
                                                    <label for="answer04">Grade Level prior to high school</label>\
                                                </div>\
                                            </div>\
              <div class="sec-edu-radio" id="parent1EduDiv" style="display:none;">\
    <div class="col-xs-12" style="padding-top:  10px;background: #337ab7;color:  #fff;"><p id="parentName1">David John(father)</p></div>\
                                        <div class="form-group edu-radio-rai" style="width: 100%;">\
                                                    <input class="Question8 allQues" type="radio" id="answer11" value="1" name="passed-code1">\
                                                    <label for="answer11">Baccalaureate or Advanced Degree</label>\
                                                </div>\
        <div class="form-group edu-radio-rai" style="width:  100%;">\
                                                    <input type="radio" id="answer12" class="Question8 allQues"  value="2" name="passed-code1">\
                                                    <label for="answer12">Associate Degree</label>\
                                                </div>\
                                                <div class="form-group edu-radio-rai" style="width:100%;">\
                                                    <input type="radio" id="answer13" class="Question8 allQues"  value="3" name="passed-code1">\
                                                    <label for="answer13">GED or High School Graduate</label>\
                                                </div>\
        <div class="form-group edu-radio-rai" style="width:100%;">\
                                                    <input type="radio" id="answer14" class="Question8 allQues"   value="4" name="passed-code1">\
                                                    <label for="answer14">Grade Level prior to high school</label>\
                                                </div>\
                                            </div>\
                                            <div class="part_edubtn">\
                                                    <button  onclick="transitionClients.SaveQuestions(8,this)" class="btn btn-edu-sucess">Accept</button>\
                                                    <button onclick="transitionClients.ClosePopUp()" class="btn btn-edu-exit">Cancel</button>\
                                                </div>\
                                        </div>',
        WQ9: '<div class="question-edu-inner"><p class="qn-p">\
                Q9. Has parent/guardian completed Job Training?</p>\
                                    <div class="sec-edu-radio" id="parentJobDiv">\
        <div class="col-xs-12" style="padding-top:  10px;background: #8a6d3b;color:  #fff;"><p id="parentName"></p></div>\
                                        <div class="form-group edu-radio-rai">\
                                                    <input  type="radio"class="Question9 allQues" id="answer01" name="passed-code" value="1">\
                                                    <label for="answer01">Yes</label>\
                                                </div>\
                                          <div class="form-group edu-radio-rai">\
                                                    <input type="radio" id="answer00" class="Question9 allQues"  name="passed-code" value="0">\
                                                    <label for="answer00">No</label>\
                                                </div>\
                                            </div>\
             <div class="sec-edu-radio" id="parent1JobDiv" style="display:none;">\
          <div class="col-xs-12" style="padding-top:  10px;background: #337ab7;color:  #fff;"><p id="parentName1"></p></div>\
                                        <div class="form-group edu-radio-rai">\
                                                    <input  type="radio"class="Question9 allQues" id="answer11" name="passed-code1" value="1">\
                                                    <label for="answer11">Yes</label>\
                                                </div>\
                                          <div class="form-group edu-radio-rai">\
                                                    <input type="radio" id="answer10" class="Question9 allQues"  name="passed-code1" value="0">\
                                                    <label for="answer10">No</label>\
                                                </div>\
                                            </div>\
                                           <div class="part_edubtn">\
                                                    <button  onclick="transitionClients.SaveQuestions(9,this)" class="btn btn-edu-sucess">Accept</button>\
                                                    <button onclick="transitionClients.ClosePopUp()" class="btn btn-edu-exit">Cancel</button>\
                                                </div>\
                                        </div>',

        WQ10: '<div class="question-edu-inner"><p class="qn-p">Q2. Has pregnant mother had a professional dental exam since last year\'s PIR?</p>\
                                        <div class="sec-edu-radio">\
                                                 <div class="form-group edu-radio-rai" style="width: 100%;">\
                                                    <input  type="radio"class="Question11 allQues" id="answer1" value="1" name="passed-code">\
                                                    <label for="answer1">Yes</label>\
                                                </div>\
                                          <div class="form-group edu-radio-rai" style="width:  100%;">\
                                                    <input type="radio" id="answer0" class="Question11 allQues"  value="0" name="passed-code">\
                                                    <label for="answer0">No</label>\
                                                </div>\
                                        </div>\
                                        <div class="part_edubtn">\
                                                    <button  onclick="transitionClients.SaveQuestions(2,this)" class="btn btn-edu-sucess">Accept</button>\
                                                    <button onclick="transitionClients.ClosePopUp()" class="btn btn-edu-exit">Cancel</button>\
                                                </div>\
                                    </div>'
    },


    getFSWHV: function (ele) {

    },

}



var Accordion = function (el, multiple) {
    this.el = el || {};
    this.multiple = multiple || false;

    // Variables privadas
    var links = this.el.find('.tr.view>td:first-child');
    // Evento
    links.on('click', { el: this.el, multiple: this.multiple }, this.dropdown)
}

Accordion.prototype.dropdown = function (e) {
    var $el = e.data.el;
    $this = $(this),
    $next = $this.next();

    $next.slideToggle();
    $this.parent().toggleClass('open');

    if (!e.data.multiple) {
        $el.find('.submenu').not($next).slideUp().parent().removeClass('open');
    };
}


var clientid;
var ispreg;
var enc_prgID;
$(document).ready(function () {


    //$(".fold-table tr.view>td:first-child").on("click", function () {
    //    if ($('.newlist').is(':visible')) {
    //        $('.newlist').hide();
    //    }
    //    $(this).parent('tr').toggleClass("open").next(".fold").toggleClass("open");
    //});

    $('#widCenter').on('change', function () {
        cleanValidation();
        if ($(this).val() == '0') {
            plainValidation($(this));
            customAlert('Please select center');
            return false;
        }
        transitionClients.getClassrooms(this);
    });

});
function getQuestionsByClient(clientId, programID) {
    $.ajax({

        url: '/AgencyUser/GetWithdrawnTranstionQuestionsByClient',
        datatype: 'json',
        type: 'post',
        data: { clientId: clientId },
        success: function (data) {

        },
        error: function (data) {

        },
        complete: function (data) {

        }

    });
}

