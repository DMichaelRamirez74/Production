var $divmatchProviderPartial = null;
var count = 0;

var serviceId = null;
var AgencyId = null;
var mpmlistcount = null;
var orgListCoun = null;
$(document).ready(function () {

    $divmatchProviderPartial = $('#div_matchproviders_partial');

    //Agency Review PopUp

    $divmatchProviderPartial.find("#Org-row").slideUp();

    $(document).on("click", ".agency-review", function (e) {
        var _id = $(this).data("index");

        $("spinner").show();

        $.ajax({
            url: "/Roster/CommunityResourceReviewList?id="+_id,
            type: "GET",
            success: function (data) {

            },
            fail: function () { },
            complete:  function () { 
                $("spinner").show();
            }
        })

    });







     serviceId = $divmatchProviderPartial.find('#FSResources').val();
     AgencyId = $divmatchProviderPartial.find('#AgencyId').val();
     mpmlistcount = $divmatchProviderPartial.find('#mpmlistCount').val();
     orgListCount = $divmatchProviderPartial.find('#organizationListCount').val();

    if (parseInt(mpmlistcount) > 1) {
        $divmatchProviderPartial.find('#ddFsOrganization').prop('disabled', true);
    }

    if (parseInt(mpmlistcount) === 1 && parseInt(orgListCount) === 1) {

        $.ajax({
            url: "/Roster/FamilyResourcesList",
            type: "POST",
            async: false,
            data: { ServiceId: serviceId, AgencyId: AgencyId },
            success: function (data) {
                $divmatchProviderPartial.find("#ddFsOrganization").html('');
                $divmatchProviderPartial.find('#OrganizationId').val('');
                for (var i = 0; i < data.listOrganization.length; i++) {
                    $divmatchProviderPartial.find('#OrganizationId').val(data.listOrganization[i].Value);
                    $divmatchProviderPartial.find("#ddFsOrganization").html(data.listOrganization[i].Text);
                }
            }
        });

        var communId =$divmatchProviderPartial.find('#OrganizationId').val();

        if (!$divmatchProviderPartial.find('#OrganizationId').val()) return false;

        $.ajax({
            url: "/Roster/GetOrganization",
            type: "POST",
            data: { CommunityId: communId },
            success: function (data) {



                $divmatchProviderPartial.find('#CommunityId').val(data.CommunityId);
                if (data.Address == null) {
                    $divmatchProviderPartial.find('#SpnCommunityAddress').text("");
                } else {
                    $divmatchProviderPartial.find('#SpnCommunityAddress').text(data.Address);
                }
                if (data.City == null) {
                    $divmatchProviderPartial.find('#SpnCommunityCity').text("");
                }
                else {
                    $divmatchProviderPartial.find('#SpnCommunityCity').text(data.City);
                }

                if (data.State == null) {
                    $divmatchProviderPartial.find('#SpnCommunityState').text("");
                }
                else {
                    $divmatchProviderPartial.find('#SpnCommunityState').text(data.State + ",");
                }

                if (data.ZipCode == null) {
                    $divmatchProviderPartial.find('#SpnCommunityZipCode').text("");
                }
                else {
                    $divmatchProviderPartial.find('#SpnCommunityZipCode').text(data.ZipCode);
                }

                if (data.OrganizationName == null) {
                    $divmatchProviderPartial.find('#spnOrganizationName').text("");
                }
                else {
                    $divmatchProviderPartial.find('#spnOrganizationName').text(data.OrganizationName);
                }

                if (data.Phone == null) {
                    $divmatchProviderPartial.find('#SpnCommunityPhone').text("");
                }
                else {
                    $divmatchProviderPartial.find('#SpnCommunityPhone').text(data.Phone);
                }

                if (data.Email == null) {
                    $divmatchProviderPartial.find('#SpnCommunityEmail').text("");
                }
                else {
                    $divmatchProviderPartial.find('#SpnCommunityEmail').text(data.Email);
                }

            }
        });

    }


    if (parseInt($divmatchProviderPartial.find('#stepId').val()) == 3) {
        $divmatchProviderPartial.find('#datepicker').prop('disabled', true)
        $divmatchProviderPartial.find('#referralServiceSaveMethod').addClass('hidden');
        $divmatchProviderPartial.find('#Description').prop('disabled', true)
    }

    //if (new Date(new Date().toDateString()) > new Date(new Date($('#datepicker').val()).toDateString())) {
    //    $('#surveryRef').removeClass('hidden');
    //}





$('#myModal5').on('hidden.bs.modal', function () {
    location.reload();

})


$('#updatealertCancel').click(function () {

    if ($("input[name=answerradio1][value='No']").prop('checked')) {
        $("input[name=answerradio1][value='Yes']").prop('checked', true)
    }
    else {
        $("input[name=answerradio1][value='No']").prop('checked', true)
    }

    count = 0;

});

$('#updatealertConfirm').click(function () {

    if ($("input[name=answerradio1][value='No']").prop('checked')) {
        $('input[name=answerradio1 ]').closest('li').next().find('textarea').val('');
        $('input[name=answerradio1]').closest('li').next().find('textarea').html('');
        for (var i = 2; i < 6; i++) {

            $("input[name=answerradio" + i + "]:last").prop('checked', true)

            $('input[name=answerradio' + i + ']:radio:checked').attr('checked', 'checked');
            $('input[name=answerradio' + i + ']').closest('li').next().find('textarea').val('');
            $('input[name=answerradio' + i + ']').closest('li').next().find('textarea').html('');
            $('input[name=answerradio' + i + ']').closest('li').next().find('textarea').attr('disabled', 'disabled')
        }
        $('#answerexp6').html('');
        $('#answerexp6').val('');
        $('#answerexp1').prop('disabled', false);
    }

    if ($("input[name=answerradio1][value='Yes']").prop('checked')) {
        $('input[name=answerradio1 ]').closest('li').next().find('textarea').val('');
        $('input[name=answerradio1]').closest('li').next().find('textarea').html('');
        $('input[name=answerradio1]').closest('li').next().find('textarea').attr('disabled', 'disabled')
        for (var i = 2; i < 6; i++) {

            $("input[name=answerradio" + i + "]").prop('checked', false);
            $('input[name=answerradio' + i + ']:radio').attr('checked', false);
            $('input[name=answerradio' + i + ']').closest('li').next().find('textarea').val('');
            $('input[name=answerradio' + i + ']').closest('li').next().find('textarea').html('');
            $('input[name=answerradio' + i + ']').closest('li').next().find('textarea').attr('disabled', 'disabled')
        }
        $('#answerexp6').html('');
        $('#answerexp6').val('');
        $('#answerexp1').prop('disabled', true);
    }


});




$('#Matchprovider').click(function () {

    $('#answer1').html($("input[name=answerradio1]:checked").val());
    $('#answer2').html($("input[name=answerradio2]:checked").val());
    $('#answer3').html($("input[name=answerradio3]:checked").val());
    $('#answer4').html($("input[name=answerradio4]:checked").val());
    $('#answer5').html($("input[name=answerradio5]:checked").val());
    $('#answerlabel1').html($('#answerexp1').val());
    $('#answerlabel2').html($('#answerexp2').val());
    $('#answerlabel3').html($('#answerexp3').val());
    $('#answerlabel4').html($('#answerexp4').val());
    $('#answerlabel5').html($('#answerexp5').val());
    $('#answerlabel6').html($('#answerexp6').val());
    $('#datecompletespan').html($('#datecompleted').val());
    $('.radio-inline,.no-print').addClass('hidden');
    $('.check_btn').addClass('hidden');
    $('.print-answer').removeClass('hidden');
    PrintModal();
});

$('input[type=radio]').change(function () {


    if ($('#isupdate').val() == 'false' && count == 0) {


        if (this.value == 'No' || this.value == 'Fair' || this.value == 'Poor') {
            var radioName = this.name;

            $('input[name=' + radioName + ']').closest('li').next().find('textarea').attr('disabled', false)
        }
        else {
            var radioName = this.name;
            $('input[name=' + radioName + ']').closest('li').next().find('textarea').val('');
            $('input[name=' + radioName + ']').closest('li').next().find('textarea').html('');
            $('input[name=' + radioName + ']').closest('li').next().find('textarea').prop('disabled', true)
        }
    }


    if ($('#isupdate').val() == 'true' && count != 0) {


        if (this.value == 'No' || this.value == 'Fair' || this.value == 'Poor') {
            var radioName = this.name;

            $('input[name=' + radioName + ']').closest('li').next().find('textarea').attr('disabled', false)
        }
        else {
            var radioName = this.name;
            $('input[name=' + radioName + ']').closest('li').next().find('textarea').val('');
            $('input[name=' + radioName + ']').closest('li').next().find('textarea').html('');
            $('input[name=' + radioName + ']').closest('li').next().find('textarea').prop('disabled', true)
        }
    }


    if ($('#isupdate').val() == 'true' && count == 0) {

        if (this.name != 'answerradio1') {


            if (this.value == 'No' || this.value == 'Fair' || this.value == 'Poor') {
                var radioName = this.name;

                $('input[name=' + radioName + ']').closest('li').next().find('textarea').attr('disabled', false)
            }
            else {
                var radioName = this.name;
                $('input[name=' + radioName + ']').closest('li').next().find('textarea').val('');
                $('input[name=' + radioName + ']').closest('li').next().find('textarea').html('');
                $('input[name=' + radioName + ']').closest('li').next().find('textarea').prop('disabled', true)
            }
        }
    }

});


$("input[name=answerradio1]:radio").change(function () {

    if ($('#isupdate').val() == 'true' && count == 0) {
        $('#updatealert').html("You already completed the survey?.Do you want to Re-Enter the Survey Form.");
        $('#updateChangeModalAlert').modal('show');
        count++;
        return false;
    }

    if ($("input[name=answerradio1]:checked").val() == "No") {
        $('input[name=answerradio1]').closest('li').next().find('textarea').attr('disabled', false)
        for (var i = 1; i < 6; i++) {

            $("input[name=answerradio" + i + "]:last").prop('checked', true);

            $('input[name=answerradio' + i + ']:radio:checked').attr('checked', 'checked');
        }

    }

    if ($("input[name=answerradio1]:checked").val() == "Yes") {
        for (var i = 2; i < 6; i++) {
            $("input[name=answerradio" + i + "]:last").prop('checked', false)
        }

    }
});


$('#SaveSurvey').click(function () {


    if ($("input:radio[name='answerradio1']").is(":checked")) {
        if ($("input[name=answerradio1]:checked").val() == "No") {
            if ($('#answerexp1').val() == "") {
                $('#errquestion1').hide();
                $('#errquestion1txt').css("display", "inline-block");
                $('#errquestion1txt').text("Please enter the explanation for question 1");
                $('#answerexp1').focus();
                // $('#answerexp1').addClass('setborder');
                return false;
            }
        }
    }
    else {
        $('#errquestion1').text("");
        $('#errquestion1').css("display", "inline");
        $('#errquestion1').text("Please select option for question 1");
        $('input[name=answerradio1]').focus();
        return false;
    }

    if ($("input:radio[name='answerradio2']").is(":checked")) {
        if ($("input[name=answerradio2]:checked").val() == "No") {
            if ($('#answerexp2').val() == "") {
                $('#errquestion1txt').hide();
                $('#errquestion1txt').text("");
                $('#errquestion2').hide();
                $('#errquestion2txt').css("display", "inline-block");
                $('#errquestion2txt').text("Please enter the explanation for question 2");
                $('#answerexp2').focus();
                return false;
            }
        }
    }
    else {
        $('#errquestion1').hide();
        $('#errquestion1txt').text("");
        $('#errquestion2').css("display", "inline");
        $('#errquestion2').text("Please select option for question 2");
        $('input[name=answerradio2]').focus();
        return false;
    }

    if ($("input:radio[name='answerradio3']").is(":checked")) {
        if ($("input[name=answerradio3]:checked").val() == "Fair" || $("input[name=answerradio3]:checked").val() == "Poor") {
            if ($('#answerexp3').val() == "") {
                $('#errquestion3').hide();
                $('#errquestion2txt').hide();
                $('#errquestion2txt').text("");
                $('#errquestion3txt').css("display", "inline-block");
                $('#errquestion3txt').text("Please enter the explanation for question 3");
                $('#answerexp3').focus();
                return false;
            }
        }
    }
    else {
        $('#errquestion2').hide();
        $('#errquestion2txt').hide();
        $('#errquestion2txt').text("");
        $('#errquestion3').css("display", "inline");
        $('#errquestion3').text("Please select option for question 3");
        $('input[name=answerradio3]').focus();
        return false;
    }

    if ($("input:radio[name='answerradio4']").is(":checked")) {
        if ($("input[name=answerradio4]:checked").val() == "Fair" || $("input[name=answerradio4]:checked").val() == "Poor") {
            if ($('#answerexp4').val() == "") {
                $('#errquestion4').hide();
                $('#errquestion3txt').hide();
                $('#errquestion3txt').text("");
                $('#errquestion4txt').css("display", "inline");
                $('#errquestion4txt').text("Please enter the explanation for question 4");
                $('#answerexp4').focus();
                return false;
            }
        }
    }
    else {
        $('#errquestion3').hide();
        $('#errquestion3txt').hide();
        $('#errquestion3txt').text("");
        $('#errquestion4').css("display", "inline");
        $('#errquestion4').text("Please select option for question 4");
        $('input[name=answerradio4]').focus();
        return false;
    }

    if ($("input:radio[name='answerradio5']").is(":checked")) {
        if ($("input[name=answerradio5]:checked").val() == "Fair" || $("input[name=answerradio5]:checked").val() == "Poor") {
            if ($('#answerexp5').val() == "") {
                $('#errquestion5').hide();
                $('#errquestion4txt').hide();
                $('#errquestion4txt').text("");
                $('#errquestion5txt').css("display", "inline");
                $('#errquestion5txt').text("Please enter the explanation for question 5");
                $('#answerexp5').focus();
                return false;
            }
        }
    }
    else {
        $('#errquestion4').hide();
        $('#errquestion4txt').hide();
        $('#errquestion4txt').text("");
        $('#errquestion5').css("display", "inline");
        $('#errquestion5').text("Please select option for question 5");
        $('input[name=answerradio5]').focus()
        return false;
    }


    if ($('#datecompleted').val() == "") {
        $('#errquestion5txt').hide();
        $('#errquestion5txt').text("");
        $('#errquestion6').css("display", "inline");
        $('#errquestion6').text("Please enter the date completed field");
        $('#datecompleted').focus();
        return false;
    }
    else if (!isDate($('#datecompleted').val().trim())) {
        $('#errquestion6').css("display", "inline");
        $('#errquestion6').text("Please Enter Valid Date");
        $('#datecompleted').focus();
        return false;

    }


    else {
        $('#errquestion6').hide();
        $('#errquestion6').text('');

        var answer1option = $("input[name=answerradio1]:checked").val();
        var answer1Exp = $('#answerexp1').val();

        var answer2option = $("input[name=answerradio2]:checked").val();
        var answer2Exp = $('#answerexp2').val();

        var answer3option = $("input[name=answerradio3]:checked").val();
        var answer3Exp = $('#answerexp3').val();

        var answer4option = $("input[name=answerradio4]:checked").val();
        var answer4Exp = $('#answerexp4').val();

        var answer5option = $("input[name=answerradio5]:checked").val();
        var answer5Exp = $('#answerexp5').val();
        var answer6 = $('#answerexp6').val();
        var answer6Exp = null;
        var arr = [];
        var update = false;
        if ($('#isupdate').val() == 'true') {
            update = true;
        }
        for (var i = 1; i < 7; i++) {

            var questionid = $('#question' + i).attr('data-questionid');
            var answerid = $('#question' + i).attr('data-answerid');
            var answer = null;
            var explanation = null;
            if (i == 6) {
                answer = $('#answerexp6').val();
                explanation = null;

            }
            else {
                answer = $("input[name=answerradio" + i + "]:checked").val();
                explanation = $('#answerexp' + i).val();
            }
            var questions = null;
            var obj = { QuestionsId: questionid, Questions: questions, Answer: answer, Explanation: explanation, AnswerId: answerid };
            arr.push(obj);
        }

        var jsonData = JSON.stringify(arr);
        var referralclientId = $('#ReferralClientServiceId').val();
        $.ajax({

            url: "/Roster/InsertSurveyOptions",
            datatype: "application/json",
            type: "POST",
            data: { surveyoptions: jsonData, ReferralClientId: referralclientId, isUpdate: update },
            success: function (data) {
                if (data) {
                    $('#myModal5').modal('hide');
                }
            },
            error: function (data) {

            }

        });
    }
});

//$('#surveryRef').click(function () {

//    var referralclientId = $('#ReferralClientServiceId').val();

//    $.ajax({
//        url: "/Roster/LoadSurveyOptions",
//        datatype: "application/json",
//        async: false,
//        data: { ReferralClientId: referralclientId },
//        success: function (data) {

//            if (data.length > 0) {
//                if (data[0].Answer == "") {
//                    $('#isupdate').val(false);
//                }
//                if (data[0].Answer != "") {

//                    $('#isupdate').val(true);
//                }
//                var updatevalue = $.parseJSON($('#isupdate').val());
//                if (!updatevalue) {
//                    var Convdate1 = new Date();
//                    var convmonth1 = Convdate1.getMonth() + 1;
//                    var convdate1 = Convdate1.getDate();
//                    var month1 = (convmonth1 < 10) ? "0" + convmonth1 : convmonth1;
//                    var date1 = (convdate1 < 10) ? "0" + convdate1 : convdate1;
//                    var convertedDate1 = (month1 + '/' + date1 + '/' + Convdate1.getFullYear());
//                    $('#datecompleted').val(convertedDate1);
//                    $('#datecompleted').html(convertedDate1);
//                }
//                else {
//                    if (data[0].CreatedDate != "") {
//                        var Convdate = new Date(data[0].CreatedDate);
//                        var convmonth = Convdate.getMonth() + 1;
//                        var convdate = Convdate.getDate();
//                        var month = (convmonth < 10) ? "0" + convmonth : convmonth;
//                        var date = (convdate < 10) ? "0" + convdate : convdate;
//                        var convertedDate = (month + '/' + date + '/' + Convdate.getFullYear());
//                        $('#datecompleted').val(convertedDate);
//                        $('#datecompleted').html(convertedDate);
//                    }
//                }


//                for (var i = 0; i < data.length; i++) {
//                    var num = i + 1;
//                    $('#question' + num).html(data[i].QuestionsId + ". " + data[i].Questions);
//                    $('#question' + num).attr('data-questionid', data[i].QuestionsId);
//                    $('#question' + num).attr('data-answerid', data[i].AnswerId);


//                    if (data[i].Answer != "") {

//                        var value = data[i].Answer;
//                        if (num == 6) {
//                            if (updatevalue) {
//                                $('#answerexp6').html(data[i].Answer);
//                                $('#answerexp6').val(data[i].Answer);
//                                $('#answerexp6').attr('disabled', false);
//                            }
//                            else {
//                                $('#answerexp6').html(data[i].Answer);
//                                $('#answerexp6').val(data[i].Answer);
//                            }

//                        }
//                        else {

//                            $("input[name=answerradio" + num + "][value='" + value + "']").attr('checked', 'checked');

//                        }
//                    }
//                    if (num != 6) {
//                        var explanation = (data[i].Explanation == "NULL") ? "" : data[i].Explanation;
//                        if (updatevalue) {

//                            $('#answerexp' + num).html(explanation);
//                            $('#answerexp' + num).val(explanation);
//                            if (explanation == "") {
//                                $('#answerexp' + num).prop('disabled', true);
//                            }
//                            else {
//                                $('#answerexp' + num).prop('disabled', false);
//                            }

//                        }
//                        else {
//                            $('#answerexp' + num).html(explanation);
//                            $('#answerexp' + num).val(explanation);
//                        }

//                    }

//                }
//            }

//        },
//        error: function (data) {

//        }
//    });


//    if ($('#isupdate').val() == 'false') {

//        $('input[type=radio]').prop('checked', false)
//        $('#answerexp1').val("");
//        $('#answerexp2').val("");
//        $('#answerexp3').val("");
//        $('#answerexp4').val("");
//        $('#answerexp5').val("");
//        $('#answerexp6').val("");

//    }


//    $('#Name').text($('#clientName').val());
//    if ($('#mpmlistCount').val() == 1) {
//        $('#OrganName').text($('#ddFsOrganization').html());
//        $('#ServicesName').text($('#servicesName').html());

//    }
//    else {
//        $('#OrganName').text($('#ddFsOrganization option:selected').text());
//        $('#ServicesName').text($('#FSResources option:selected').text());
//    }
//    $('#ReferralDate').text($('#datepicker').val());
//});


//$('.check').on('click', function (e) {
//    $('.chk_all').prop('checked', true)
//});
//$('.uncheck').on('click', function (e) {
//    $('.chk_all').prop('checked', false)
//});

$('#datepicker').on('click', function (e) {

});
//----------------------------------------------------------------------------------//
$divmatchProviderPartial.find('#spnOrganizationName').click(function () {
    var commId = null;
    var $organizationModal = $($(this).data('target'));

    if ($divmatchProviderPartial.find('#_CommunityId').val() == "0" || $divmatchProviderPartial.find('#_CommunityId').val() == "") {
        commId = $divmatchProviderPartial.find('#CommunityId').val();
    }
    else {
        commId = $divmatchProviderPartial.find('#_CommunityId').val();
    }
    $.ajax({
        url: "/Roster/GetBusinessHours",
        datatype: "application/json",
        async: false,
        data: {
            ServiceId: $divmatchProviderPartial.find('#_ServiceId').val(), AgencyID: $divmatchProviderPartial.find('#AgencyId').val(), CommunityID: commId
        },
        success: function (data) {
            if (data.length == 1) {
                var monFrom = (data[0].MonFrom != "") ? data[0].MonFrom : "N/A";
                var monTo = (data[0].MonTo != "") ? data[0].MonTo : "N/A";
                var tueTo = (data[0].TueTo != "") ? data[0].TueTo : "N/A";
                var tueFrom = (data[0].TueFrom != "") ? data[0].TueFrom : "N/A";
                var wedTo = (data[0].WedTo != "") ? data[0].WedTo : "N/A";
                var wedFrom = (data[0].WedFrom != "") ? data[0].WedFrom : "N/A";
                var thuTo = (data[0].ThursTo != "") ? data[0].ThursTo : "N/A";
                var thuFrom = (data[0].ThursFrom != "") ? data[0].ThursFrom : "N/A";
                var friTo = (data[0].FriTo != "") ? data[0].FriTo : "N/A";
                var friFrom = (data[0].FriFrom != "") ? data[0].FriFrom : "N/A";
                var satTo = (data[0].SatTo != "") ? data[0].SatTo : "N/A";
                var satFrom = (data[0].SatFrom != "") ? data[0].SatFrom : "N/A";
                var sunFrom = (data[0].SunFrom != "") ? data[0].SunFrom : "N/A";
                var sunTo = (data[0].SunTo != "") ? data[0].SunTo : "N/A";
            }
            else {
                var monFrom = "N/A";
                var monTo = "N/A";
                var tueTo = "N/A";
                var tueFrom = "N/A";
                var wedTo = "N/A";
                var wedFrom = "N/A";
                var thuTo = "N/A";
                var thuFrom = "N/A";
                var friTo = "N/A";
                var friFrom = "N/A";
                var satTo = "N/A";
                var satFrom = "N/A";
                var sunFrom = "N/A";
                var sunTo = "N/A";
            }
            $organizationModal.find('#refferedtomodal').html($divmatchProviderPartial.find('#spnOrganizationName').text());
            $organizationModal.find('#streetModal').html($divmatchProviderPartial.find('#SpnCommunityAddress').text().split(',')[0] + ',');
            $organizationModal.find('#cityModal').html($divmatchProviderPartial.find('#SpnCommunityCity').text().split(',')[0]);
            $organizationModal.find('#stateModal').html($divmatchProviderPartial.find('#SpnCommunityState').text().split(',')[0] + ", " + $('#SpnCommunityZipCode').text().split(',')[0]);
            $organizationModal.find('#emailModal').html($divmatchProviderPartial.find('#SpnCommunityEmail').text().split(',')[0]);
            $organizationModal.find('#phonenoModal').html($divmatchProviderPartial.find('#SpnCommunityPhone').text().split(',')[0]);
            $organizationModal.find('#resourceNamemodal').html($divmatchProviderPartial.find('#clientName').val());
            if (parseInt($divmatchProviderPartial.find('#mpmlistCount').val()) === 1) {
                $organizationModal.find('#serviceModal').html($divmatchProviderPartial.find('#servicesName').html());

            }
            else {
                $organizationModal.find('#serviceModal').html($divmatchProviderPartial.find('#FSResources option:selected').text());

            }
            $organizationModal.find('#monFrmHours').html(monFrom);
            $organizationModal.find('#monToHours').html(monTo);
            $organizationModal.find('#tueFrmHours').html(tueFrom);
            $organizationModal.find('#tueToHours').html(tueTo);
            $organizationModal.find('#wedFrmHours').html(wedFrom);

            $organizationModal.find('#wedToHours').html(wedTo);
            $organizationModal.find('#thuFrmHours').html(thuFrom);
            $organizationModal.find('#thuToHours').html(thuTo);
            $organizationModal.find('#friFrmHours').html(friFrom);
            $organizationModal.find('#friToHours').html(friTo);
            $organizationModal.find('#satFrmHours').html(satFrom);
            $organizationModal.find('#satToHours').html(satTo);
            $organizationModal.find('#sunFrmHours').html(sunFrom);
            $organizationModal.find('#sunToHours').html(sunTo);

            //$divmatchProviderPartial.find('#myModalOrganization').show();
            //  $organizationModal.modal('show');
            //$organizationModal.show();
        }

    });




});

$divmatchProviderPartial.find('#referralServiceSaveMethod').click(function () {

    if (parseInt($divmatchProviderPartial.find('#servicescount').val()) > 1) {
        if (parseInt($divmatchProviderPartial.find('#FSResources').val()) == 0) {
            $divmatchProviderPartial.find('#answererror').html('Please select service resources');
            $divmatchProviderPartial.find('#FSResources').focus();
            $divmatchProviderPartial.find('#err_resource').css("display", "inline-block");
            $divmatchProviderPartial.find('#err_resource').text("Please Select Resource");

            //   $('#surveyAnswerError').modal('show');

            return false;
        }

     /*   if (parseInt($('#ddFsOrganization').val()) == 0) {
            $('#err_resource').hide();
            $('#err_resource').text("");
            $('#answererror').html('Please select service organization');
            $('#ddFsOrganization').focus();
            $('#err_organization').css('display', 'inline-block');
            $('#err_organization').text('Please Select Organization Name');
            //$('#surveyAnswerError').modal('show');
            return false;
        }
        */
    }

    if (parseInt($divmatchProviderPartial.find('#organizationListCount').val()) > 1) {

     /*   if (parseInt($('#ddFsOrganization').val()) == 0) {
            $('#err_resource').hide();
            $('#err_resource').text("");
            $('#answererror').html('Please select service organization');
            $('#ddFsOrganization').focus();
            $('#err_organization').css('display', 'inline-block');
            $('#err_organization').text('Please Select Organization Name');
            //$('#surveyAnswerError').modal('show');
            return false;
        }
        */
    }

    if (!$divmatchProviderPartial.find('[name="company"]:checked').val()) {
        customAlert("Please Select Organization Name");
        return false;
    }

    if ($divmatchProviderPartial.find('#datepicker').val() == "") {
        $divmatchProviderPartial.find('#err_resource').hide();
        $divmatchProviderPartial.find('#err_resource').text("");
        $divmatchProviderPartial.find('#err_organization').hide();
        $divmatchProviderPartial.find('#err_organization').text("");
        $divmatchProviderPartial.find('#answererror').html('Please enter referral date.');
        $divmatchProviderPartial.find('#datepicker').focus();
        $divmatchProviderPartial.find('#err_date').css('display', 'inline-block');
        $divmatchProviderPartial.find('#err_date').text('Please Enter Referral Date');
        //  $('#surveyAnswerError').modal('show');
        return false;
    }
    else if (!isDate($divmatchProviderPartial.find('#datepicker').val().trim())) {
        $divmatchProviderPartial.find('#err_resource').hide();
        $divmatchProviderPartial.find('#err_resource').text("");
        $divmatchProviderPartial.find('#err_organization').hide();
        $divmatchProviderPartial.find('#err_organization').text("");
        $divmatchProviderPartial.find('#err_date').css('display', 'inline-block');
        $divmatchProviderPartial.find('#err_date').text('Please Enter Valid Date');
        return false;
    }

    $divmatchProviderPartial.find('#err_date').hide();
    $divmatchProviderPartial.find('#err_date').text('');

    var ReferralDate = $divmatchProviderPartial.find('#datepicker').val().trim();
    var Description = $divmatchProviderPartial.find('#Description').val().trim();
    var ServiceResourceId = $divmatchProviderPartial.find('#FSResources').val();


    if ($divmatchProviderPartial.find('#CommunityId').val() == undefined) {
        var CommunityId = 0;
        if ($divmatchProviderPartial.find('#_CommunityId').val() != "" && $divmatchProviderPartial.find('#_CommunityId').val() != null) {
            CommunityId = $divmatchProviderPartial.find('#_CommunityId').val();
        } else {
            CommunityId = 0;
        }
    } else {
        var CommunityId = 0;
        if ($divmatchProviderPartial.find('#CommunityId').val() != "" && $divmatchProviderPartial.find('#CommunityId').val() != null) {
            CommunityId = $divmatchProviderPartial.find('#CommunityId').val();
        } else {
            CommunityId = 0;
        }
    }




    if ($divmatchProviderPartial.find('#ReferralClientServiceId').val() == "") {
        $divmatchProviderPartial.find('#ReferralClientServiceId').val(0);

    } else {

        AgencyId = 0;
        CommunityId = $divmatchProviderPartial.find('#_CommunityId').val();

    }

    var AgencyId = $divmatchProviderPartial.find('#AgencyId').val();
    var referralclientid = 0;
    if ($divmatchProviderPartial.find('#referralClientId').val() != "" || $divmatchProviderPartial.find('#referralClientId').val() != 0) {
        referralclientid = $divmatchProviderPartial.find('#referralClientId').val().trim();
    }
    var SaveMatchProviders = {};
    SaveMatchProviders.ReferralDate = ReferralDate;
    SaveMatchProviders.Description = Description;
    SaveMatchProviders.ServiceResourceId = parseInt(ServiceResourceId);
    SaveMatchProviders.AgencyId = AgencyId;
    SaveMatchProviders.CommunityId = CommunityId;
    SaveMatchProviders.ReferralClientServiceId = parseInt(referralclientid);
    SaveMatchProviders.ScreeningReferralYakkr = $divmatchProviderPartial.find('#screeningReferralYakkr').val();
    SaveMatchProviders.ClientId = isMatchProvidersReferralPopup() ? $('#att-issue-modal').find('#hiddenatt-issue-clientid').val() : $divmatchProviderPartial.find('#_ClientId').val();

    $.ajax({
        url: "/Roster/SaveMatchProviders",
        type: "POST",
        data: SaveMatchProviders,
        success: function (data) {
            if (data = true) {

                var $isPopup = isMatchProvidersReferralPopup();

                if ($isPopup)
                {
                    customAlert('Record saved successfully');
                    window.setTimeout(function () {

                        $divmatchProviderPartial.find('#Cancel').trigger('click');

                    }, 1000);
                }
                else {

                    $('#SuccessMatchProviders').modal('show');
                }
                
            }
        }

    });


});


$('#succesMatchClose').click(function () {
    location.href = "/Roster/ReferralService?id=" + $divmatchProviderPartial.find('#encryptId').val() + "&clientName=" + $divmatchProviderPartial.find('#clientName').val() + "&scrYakkr=" + $divmatchProviderPartial.find('#screeningReferralYakkr').val();
});


    //intial load when single service selected
    //_CommunityId
    //if (parseInt($divmatchProviderPartial.find("#FSResources").val()) > 0 && $divmatchProviderPartial.find('#AgencyId').val()) {

if (parseInt($divmatchProviderPartial.find("#FSResources").val()) > 0 && ($divmatchProviderPartial.find('#_CommunityId').val()==null ||$divmatchProviderPartial.find('#_CommunityId').val()=='0') ) {
    var serviceId = $divmatchProviderPartial.find('#FSResources').val();
    var AgencyId = $divmatchProviderPartial.find('#AgencyId').val();
    getOrganization(serviceId, AgencyId);
}


$divmatchProviderPartial.find('#FSResources').on('change', function () {

    var serviceId = (this.value);
    var AgencyId = $divmatchProviderPartial.find('#AgencyId').val();
    getOrganization(serviceId, AgencyId);

});




$(document).on("click", ".show-reviewmodal", function (e) {
    var _id = $(this).data('index');
    $('#spinner').show();
    $.ajax({
        url: "/Roster/GetReviewList?id="+_id,
        type:"GET",
        //type: "POST",
        //data: { id: _id},
        success: function (data) {
            // drawOrganization(data);
            $('#spinner').hide();
            renderReviewList(data);
           
        }, fail: function (res) {

        },
        complete: function (res) {
            $('#spinner').hide();
        }
    });


    

});




//Old Method
/*
$('#FSResources').on('change', function () {

    var serviceId = (this.value);
    var AgencyId = $('#AgencyId').val();
    $.ajax({
        url: "/Roster/FamilyResourcesList",
        type: "POST",
        data: { ServiceId: serviceId, AgencyId: AgencyId },
        success: function (data) {

            $("#ddFsOrganization").prop('disabled', false);
            $("#ddFsOrganization").html('');
            $("#ddFsOrganization").append('<option value=' + 0 + '>' + "--Select Organization--" + '</option>');
            for (var i = 0; i < data.listOrganization.length; i++) {
                $("#ddFsOrganization").append('<option value=' + data.listOrganization[i].Value + '>' + data.listOrganization[i].Text + '</option>');
            }
            $("#ddFsOrganization").prop('disabled', false);
        }
    });
})

*/


//$('#ddFsOrganization').on('change', function () {

$(document).on('change', '[name="company"]', function (e) {


    var communityId = (this.value);

   // if(communityId == )
    $('#CommunityId').val(communityId);
    $('#_CommunityId').val(communityId);

    $.ajax({
        url: "/Roster/GetOrganization",
        type: "POST",
        data: { CommunityId: communityId },
        success: function (data) {

            $('#CommunityId').val(data.CommunityId);
            if (data.Address == null) {
                $('#SpnCommunityAddress').text("");
            } else {
                $('#SpnCommunityAddress').text(data.Address);
            }
            if (data.City == null) {
                $('#SpnCommunityCity').text("");
            }
            else {
                $('#SpnCommunityCity').text(data.City);
            }

            if (data.State == null) {
                $('#SpnCommunityState').text("");
            }
            else {
                $('#SpnCommunityState').text(data.State);
            }

            if (data.ZipCode == null) {
                $('#SpnCommunityZipCode').text("");
            }
            else {
                $('#SpnCommunityZipCode').text(data.ZipCode);
            }

            if (data.OrganizationName == null) {
                $('#spnOrganizationName').text("");
            }
            else {
                $('#spnOrganizationName').text(data.OrganizationName);
            }

            if (data.Phone == null) {
                $('#SpnCommunityPhone').text("");
            }
            else {
                $('#SpnCommunityPhone').text(data.Phone);
            }

            if (data.Email == null) {
                $('#SpnCommunityEmail').text("");
            }
            else {
                $('#SpnCommunityEmail').text(data.Email);
            }

        }
    });
})

//Old Method
/*

$('#ddFsOrganization').on('change', function () {
    var communityId = (this.value);
    $('#CommunityId').val(communityId);
    $('#_CommunityId').val(communityId);

    $.ajax({
        url: "/Roster/GetOrganization",
        type: "POST",
        data: { CommunityId: communityId },
        success: function (data) {

            $('#CommunityId').val(data.CommunityId);
            if (data.Address == null) {
                $('#SpnCommunityAddress').text("");
            } else {
                $('#SpnCommunityAddress').text(data.Address);
            }
            if (data.City == null) {
                $('#SpnCommunityCity').text("");
            }
            else {
                $('#SpnCommunityCity').text(data.City);
            }

            if (data.State == null) {
                $('#SpnCommunityState').text("");
            }
            else {
                $('#SpnCommunityState').text(data.State);
            }

            if (data.ZipCode == null) {
                $('#SpnCommunityZipCode').text("");
            }
            else {
                $('#SpnCommunityZipCode').text(data.ZipCode);
            }

            if (data.OrganizationName == null) {
                $('#spnOrganizationName').text("");
            }
            else {
                $('#spnOrganizationName').text(data.OrganizationName);
            }

            if (data.Phone == null) {
                $('#SpnCommunityPhone').text("");
            }
            else {
                $('#SpnCommunityPhone').text(data.Phone);
            }

            if (data.Email == null) {
                $('#SpnCommunityEmail').text("");
            }
            else {
                $('#SpnCommunityEmail').text(data.Email);
            }

        }
    });
})

*/

$('#referralLetterPdf').on('click', function () {
    if (parseInt($('#servicescount').val()) > 1) {
        if (parseInt($('#FSResources').val()) == 0) {
            $('#answererror').html('Please select service resources');
            $('#FSResources').focus();
            $('#err_resource').css("display", "inline-block");
            $('#err_resource').text("Please Select Resource");
            //$('#surveyAnswerError').modal('show');
            return false;
        }

        if (parseInt($('#ddFsOrganization').val()) == 0) {
            $('#err_resource').hide();
            $('#err_resource').text("");
            $('#answererror').html('Please select service organization');
            $('#ddFsOrganization').focus();
            $('#err_organization').css('display', 'inline-block');
            $('#err_organization').text('Please Select Organization Name');
            // $('#surveyAnswerError').modal('show');
            return false;
        }
    }

     if (parseInt($('#organizationListCount').val()) > 1) {
        if (parseInt($('#ddFsOrganization').val()) == 0) {
            $('#err_resource').hide();
            $('#err_resource').text("");
            $('#answererror').html('Please select service organization');
            $('#ddFsOrganization').focus();
            $('#err_organization').css('display', 'inline-block');
            $('#err_organization').text('Please Select Organization Name');
            // $('#surveyAnswerError').modal('show');
            return false;
        }
        else {
            $('#err_organization').css('display', 'none');
        }

     }

     if ( $('[name="company"]').length>0  && !$('[name="company"]:checked').val() ) {

         customAlert("Please Choose Service Organization");

         $('#err_resource').hide();
         $('#err_resource').text("");
         $('#err_organization').hide();
         $('#err_organization').text("");
         //$('#answererror').html('Please Choose Service Organization');


         return false;
     }


     if ($('#datepicker').val() == "") {
        $('#err_resource').hide();
        $('#err_resource').text("");
        $('#err_organization').hide();
        $('#err_organization').text("");
        $('#answererror').html('Please enter referral date.');
        $('#datepicker').focus();
        $('#err_date').css('display', 'inline-block');
        $('#err_date').text('Please Enter Referral Date');
        // $('#surveyAnswerError').modal('show');
        return false;
    }
     if (!isDate($('#datepicker').val().trim())) {
        $('#err_resource').hide();
        $('#err_resource').text("");
        $('#err_organization').hide();
        $('#err_organization').text("");
        $('#err_date').css('display', 'inline-block');
        $('#err_date').text('Please Enter Valid Date');
        return false;
    }

    $('#err_date').hide();
    $('#err_date').text("");
    if ($('#_AgencyId').val() == "") {
        $('#_AgencyId').val($('#AgencyId').val());
    }

    if ($('#_CommunityId').val() == "0" || $('#_CommunityId').val() == "") {
        var communityId = $('#CommunityId').val();
    }
    else {
        var communityId = $('#_CommunityId').val();
    }

    

    var notes = $('#Description').val();
    window.location.href = "/Roster/CompleteServicePdf?ServiceId=" + $('#_ServiceId').val() + "&AgencyID=" + $('#_AgencyId').val() + "&ClientID=" + $('#encryptId').val() + "&CommunityID=" + communityId + "&Notes=" + notes + "&referralDate=" + $('#datepicker').val();
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


function PrintModal() {
    $('.modal-dialog').removeClass('modal-width-change');
    $('.modal-body').removeClass('modal-height-change');

    var contents = $(".modal-body").html();
    var frame1 = $('<iframe />');
    frame1[0].name = "frame1";
    frame1.css({ "position": "absolute", "top": "-1000000px", "height": "100%" });
    $("body").append(frame1);
    var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
    frameDoc.document.open();
    //Create a new HTML document.
    frameDoc.document.write('<html><head><title>Referral Survey</title>');
    frameDoc.document.write('</head><body>');
    //Append the external CSS file.
    frameDoc.document.write('<link href="/Content/print.css" rel="stylesheet" type="text/css" />');
    //Append the DIV contents.
    frameDoc.document.write(contents);
    frameDoc.document.write('</body></html>');
    frameDoc.document.close();
    setTimeout(function () {
        window.frames["frame1"].focus();
        window.frames["frame1"].print();
        frame1.remove();
    }, 500);
    $('.modal-dialog').addClass('modal-width-change');
    $('.modal-body').addClass('modal-height-change');
    $('.check_btn').removeClass('hidden');
    $('#datecompleted').removeClass('hidden');
    $('#datecompletespan').addClass('hidden');
    $('.radio-inline,.no-print').removeClass('hidden');
    $('.print-answer').addClass('hidden');

};






function renderReviewList(data) {

    $("#reviewlist-modal .modal-body").html('');

    if (data.length == 0) return false;

    var _tblStr = '<table class="table table-bordered" id="review-modal-table"><thead><tr><th>Review Color</th><th>Notes</th><th>Entered By</th><th>Entered Date</th><tr></thead><tbody>';

    data.forEach(function (item) {
        var _clr = item.CRColorCode == 1 ? 'red' : item.CRColorCode == 2 ? '#ffff00' : 'green';

        _tblStr += '<tr><td><div class="rlclrdiv" style="background:' + _clr + ';"></div></td><td>' + item.MgNotes + '</td><td>' + item.ModifiedBy + '</td><td>' + item.ModifiedDate + '</td> </tr>'
    });

    _tblStr += '<tbody></table>';

    $("#reviewlist-modal .modal-body").append(_tblStr);
  //  $('#att-issue-modal').modal('hide');
    $("#reviewlist-modal").modal("show");

};

function getOrganization(serviceId, AgencyId) {

    $divmatchProviderPartial.find("#Org-row").slideDown();

    $.ajax({
        url: "/Roster/GetOrganizationList",
        type: "POST",
        data: { ServiceId: serviceId, AgencyId: AgencyId },
        success: function (data) {
            drawOrganization(data);
        }, fail: function (res) {

        },
        complete: function (res) {

        }
    });

}



function drawOrganization(data) {

    $divmatchProviderPartial.find("#organization-list").html('');

    if (data.length == 0) return false;


    data.forEach(function (item) {

        var _badgeDis = item.ReviewCount ? 'inline' : 'none';

        var _clrCode = item.CRColorCode == 1 ? 'red' : item.CRColorCode == 2 ? '#ffff00' : 'green';
        var _coutClr = item.CRColorCode == 1 ? '#ffffff' : item.CRColorCode == 2 ? '#333333' : '#ffffff';
        var _radioStr = '<div class="col-lg-12 col-sm-12 col-xs-12" style="padding-left:0px;">'
      // var _radioStr = '<div class="" style="padding-bottom:10px;">'
       + '<label title="' + item.CompanyName + '" class="container-radio col-sm-11" style="display:inline;white-space:nowrap;overflow:hidden;text-overflow:ellipsis;width:auto;max-width:91.6%;">'
       + '' + item.CompanyName + ''
       + '<input type="radio" name="company" value="' + item.CommunityResourceID + '">'
       + '<span class="checkmark"></span>'
       + '</label>'
      // + '<i class="fa fa-external-link agency-review" data-index="' + item.CommunityResourceID + '"></i>'
      + '<span data-index="' + item.CommunityResourceID + '" class="badge show-reviewmodal col-sm-1" style="width:auto;background:' + _clrCode + ';color:' + _coutClr + ';display:' + _badgeDis + ';cursor:pointer;" data-toggle="tooltip" data-placement="right" title="' + item.ReviewCount + ' reviews">' + item.ReviewCount + '</span>'
       + '</div>';

        $divmatchProviderPartial.find("#organization-list").append(_radioStr);

    });


    $divmatchProviderPartial.find('[data-toggle="tooltip"]').tooltip();


};


function isMatchProvidersReferralPopup() {

    if ($('#att-issue-modal').length > 0 && $('#att-issue-modal').is(':visible') && $('#refmathcprovidersattenissuediv').length > 0 && $('#refmathcprovidersattenissuediv').is(':visible')) {

        return true;
    }
    else {
        return false;
    }

}