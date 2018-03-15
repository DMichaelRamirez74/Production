var sortOrder = 'CategoryPosition,AssessmentGroupType';
var sortDirection = 'ASC';
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
$(document).ready(function () {
    $('#DashboardHV').removeClass("active");

    $.ajax({
        url: "/Matrix/GetMatrixTypeList",
        type: "POST",
        dataType: "json",
        secureuri: false,
        async: false,
        success: function (response1) {
            if (response1.length > 0) {
                $('.select-matrix').html('');
                $('.select-matrix').append('<option matrixvalue="" value=0>Select Matrix Type</option>')
                for (var i = 0; i < response1.length; i++) {
                    $('.select-matrix').append('<option matrixvalue='+response1[i].MatrixValue+' value=' + response1[i].MatrixId + '>' + response1[i].MatrixType + '</option>')
                }

            }
        }
     , error: function (response1) {
         //  customAlert("Session Ended Log Onto The System Again.");setTimeout(function () {window.location.href= HostedDir + '/login/Loginagency';   }, 2000);
     }
    });

    $.ajax({
        url: "/Matrix/GetGroupDetails",
        type: "POST",
        dataType: "json",
        secureuri: false,
        async: false,
        success: function (response3) {
            if (response3.length > 0) {
                $('.select-group').html('');
                $('.select-group').append('<option value=0>Select Assessment Group Type</option>')
                for (var i = 0; i < response3.length; i++) {
                    $('.select-group').append('<option value=' + response3[i].AssessmentGroupId + '>' + response3[i].AssessmentGroupType + '</option>')
                }

            }
        }
     , error: function (response3) {
         //  customAlert("Session Ended Log Onto The System Again.");setTimeout(function () {window.location.href= HostedDir + '/login/Loginagency';   }, 2000);
     }
    });

    drawgrid();
    $('#assessmetResultsTable > thead > tr > th').children('i').addClass('sort-color');

    $('#assessmetResultsTable > thead > tr > th').click(function () {
        sortOrder = this.id;
        if ($(this).children('i').hasClass('fa-sort')) {
            $('#assessmetResultsTable > thead > tr > th').children('i').removeClass('fa-sort fa-sort-up fa-sort-down');
            $('#assessmetResultsTable > thead > tr > th').children('i').addClass('fa-sort sort-color');
            $(this).children('i').removeClass('fa-sort sort-color');
            $(this).children('i').addClass('fa-sort-up');
                
        }
        else if ($(this).children('i').hasClass('fa-sort-up')) {
            $('#assessmetResultsTable > thead > tr > th').children('i').removeClass('fa-sort fa-sort-up fa-sort-down');
            $('#assessmetResultsTable > thead > tr > th').children('i').addClass('fa-sort sort-color');
            $(this).children('i').removeClass('fa-sort sort-color');
            $(this).children('i').addClass('fa-sort-down');
        }
        else if ($(this).children('i').hasClass('fa-sort-down')) {
            $('#assessmetResultsTable > thead > tr > th').children('i').removeClass('fa-sort fa-sort-up fa-sort-down');
            $('#assessmetResultsTable > thead > tr > th').children('i').addClass('fa-sort sort-color');
            $(this).children('i').removeClass('fa-sort sort-color');
            $(this).children('i').addClass('fa-sort-up');
        }
        if (sortOrder == "thSN")
            return false;
        if (sortOrder == "thRN")
            return false;
        imgID = $("#" + this.id).find('img').attr('id');
        $('#assessmetResultsTable > thead > tr > th > img').css("visibility", "hidden");
        direction = $("#" + imgID).siblings('input').val();
        if (direction == "Asc") {
            $("#" + imgID).siblings('input').val("Desc");
            sortDirection = $("#" + imgID).siblings('input').val();
        } else {
            $("#" + imgID).siblings('input').val("Asc");
            sortDirection = $("#" + imgID).siblings('input').val();
        }
        getList();
        bindGridUser(listAgency, $("#assessmetResultsTable").find('tr')[0].cells.length);
    });
    $("#txtsearch").on('change keypress paste', function (e) {
        var key = e.which;
        if (key == 13) {
            drawgrid();
        }
    });
    $('#ddlpaging').change(function () {
        getListafterupdation();
    });

    $('.select-matrix').change(function () {
        if($(this).val()!=0)
        {
            $('#matrixValue').html('');
            $('#matrixValue').html($('option:selected', this).attr('matrixvalue'));
        }
        else
        {
            $('#matrixValue').html('');

        }

    })
         

    $('#btnaddResult').click(function () {
       
                
        if ($('.select-group').val() == 0) {
            customAlert("Please select Assessment Group Type");
            plainValidation('.select-group');
            return false;
        }
        if ($('.select-matrix').val() == 0) {
            customAlert("Please select Matrix Type");
            plainValidation('.select-matrix');
            return false;
        }
        if ($('#descriptionText').val().trim() == "")
        {
            customAlert("Please enter description");
            plainValidation('#descriptionText');
            return false;
        }
        if (!($("input:radio[name='referralradio']").is(":checked"))) {
            customAlert("Please select referral status");
            plainValidation('referralradio');
            return false;
        }
        if (!($("input:radio[name='fparadio']").is(":checked"))) {
            customAlert("Please select fpa status");
            plainValidation('fparadio');
            return false;
        }
               
        else {
            var groupId = parseInt($('.select-group').val());
            var matrixid = parseInt($('.select-matrix').val());
            var description = $('#descriptionText').val().trim();
            var isRefferral = (parseInt($("input[name='referralradio']:checked").val())==1)?true:false;
            var isFpa = (parseInt($("input[name='fparadio']:checked").val())==1)?true:false;
            
            $.ajax({
                url: "/Matrix/InsertAssessmentResults",
                dataType: 'json',
                type: "POST",
                async: false,
                data: {
                    groupID: groupId, matrixID: matrixid, Description:description,referralSuggested: isRefferral, FPASuggessted: isFpa
                },
                success: function (data) {
                    if (data == 1) {
                        customAlert("Record saved successfully.");
                        sortOrder = 'CreatedDate';
                        sortDirection = 'Desc';
                        drawgrid();
                        clearcontrols();
                    }
                    if (data == 2) {
                        customAlert("Record already exists");
                    }


                    else
                        customAlert(data);

                },
                error: function (data) { alert(data); }
            });
        }
    });

    $('body').on('click', '.updateResult', function () {
        var resultId = this.getAttribute('accesskey');
        var matrxiId = this.getAttribute('matrixid');
        var groupId = this.getAttribute('groupid');
        var matrixValue = $(this).closest('tr').find('td').eq(2).html().trim();
        var description = $(this).closest('tr').find('td').eq(3).html().trim();
        var referralstatus = ($(this).closest('tr').find('td').eq(4).html().trim()=="Yes")?1:0;
        var fpastatus = ($(this).closest('tr').find('td').eq(5).html().trim()=="Yes")?1:0;
        $('#matrixValue').html(matrixValue);
        $('.select-matrix').val(matrxiId);
        $('.select-group').val(groupId);
        $('#descriptionText').val(description);
        $('#btnUpdateResult').attr('resultId', resultId);
        $('#btnaddResult').addClass('hidden');
        $('#btnUpdateResult').removeClass('hidden');
        $('input[name="referralradio"][value=' + referralstatus + ']').prop('checked', true)
        $('input[name="fparadio"][value=' + fpastatus + ']').prop('checked', true)
               
    });

    $('#btnUpdateResult').click(function () {
        if ($('.select-group').val() == 0) {
            customAlert("Please select Assessment Group Type");
            plainValidation('.select-group');
            return false;
        }
        if ($('.select-matrix').val() == 0) {
            customAlert("Please select Matrix Type");
            plainValidation('.select-matrix');
            return false;
        }
        if ($('#descriptionText').val().trim() == "") {
            customAlert("Please enter description");
            plainValidation('#descriptionText');
            return false;
        }
        if (!($("input:radio[name='referralradio']").is(":checked"))) {
            customAlert("Please select referral status");
            plainValidation('referralradio');
            return false;
        }
        if (!($("input:radio[name='fparadio']").is(":checked"))) {
            customAlert("Please select fpa status");
            plainValidation('fparadio');
            return false;
        }
        else {
            var referralstatus = (parseInt($("input[name='referralradio']:checked").val())==1)?true:false;
            var fpastatus = (parseInt($("input[name='fparadio']:checked").val())==1)?true:false;
            var groupid = parseInt($('.select-group').val());
            var matrixId = parseInt($('.select-matrix').val());
            var description = $('#descriptionText').val();
            var resultid = parseInt($(this).attr('resultId'));
            $.ajax({
                url:"/Matrix/UpdateAssessmentResults",
                dataType: 'json',
                type: "POST",
                async: false,
                data: {
                    groupId: groupid, matrixId: matrixId, referralStatus: referralstatus, fpaStatus: fpastatus, Description: description, resultID: resultid
                },
                success: function (data) {
                  
                    if (data==1) {
                        customAlert("Record updated successfully.");
                        sortOrder = 'ModifiedDate';
                        sortDirection = 'Desc';
                        drawgrid();
                        clearcontrols();
                    }
                    if(data==2)
                    {
                        customAlert("Record already exists");
                    }

                    else
                        customAlert(data);

                },
                error: function (data) { alert(data); }
            });
        }


    });

    $('#btnResultCancel').click(function () {

        $('.select-group').val(0);
        $('.select-matrix').val(0);
        $('#descriptionText').val('');
        $('input[name="referralradio"][value=1]').prop('checked', true)
        $('input[name="fparadio"][value=1]').prop('checked', true)
        $('#btnUpdateResult').addClass('hidden');
        $('#btnaddResult').removeClass('hidden');
        $('#matrixValue').html('');
        cleanValidation();

    });
});


function deleteResult(value)
{
    var ResultID = parseInt($(value).attr('accesskey'));
    BootstrapDialog.confirm('Do you want to delete this Assessment group type ?', function (result) {
        if (result) {
            $.ajax({
                url: "/Matrix/DeleteAssessmentResults",
                type: "POST",
                data: {
                    resultId: ResultID
                },
                dataType: "json",
                secureuri: false,
                async: false,
                success: function (response) {
                    if (response) {
                        customAlert("Assessment group type deleted successfully");
                        $(value).closest('tr').remove();
                    }
                }
            , error: function (response) {
                customAlert("Session Ended Log Onto The System Again."); setTimeout(function () { window.location.href = HostedDir + '/login/Loginagency'; }, 2000);
            }
            });
        }
    });
}

function clearcontrols() {
    cleanValidation();
    $('.select-group').val(0);
    $('.select-matrix').val(0);
    $('#descriptionText').val('');
    $('input[name="referralradio"][value=1]').prop('checked', true)
    $('input[name="fparadio"][value=1]').prop('checked', true)
    $('#btnUpdateResult').addClass('hidden');
    $('#btnaddResult').removeClass('hidden');
    $('#matrixValue').html('');
}


function getListafterupdation() {

    pageSize = $('#ddlpagetodisplay').val();
    requestedPage = $('#ddlpaging').val();
    StartIndex = (pageSize * (requestedPage - 1)) + 1;
    LastIndex = parseInt(pageSize * requestedPage) - parseInt(pageSize);
    $('#assessmetResultsTable > thead > tr > th > img').css("visibility", "hidden");
    if (imgID != '' && imgID != 'undefined' && imgID != null) {
        direction = $("#" + imgID).siblings('input').val();
    }
    if (direction == "Asc") {
        sortDirection = $("#" + imgID).siblings('input').val();
    } else if (direction == "Desc") {
        sortDirection = $("#" + imgID).siblings('input').val();
    }
    getList();
    //var totalRecord = getTotalRecord();
    if (requestedPage == 1) {
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
    bindGridUser(listAgency, $("#assessmetResultsTable").find('tr')[0].cells.length);
    $("#ddlpaging").val(requestedPage);
}
function getList() {
    $.ajax({
        url: "/Matrix/GetAssessmentResults",
        type: "POST",
        data: {
            sortOrder: sortOrder,
            sortDirection: sortDirection,
            pageSize: $('#ddlpagetodisplay').val(),
            requestedPage: requestedPage
        },
        dataType: "json",
        secureuri: false,
        async: false,
        success: function (response) {
            getData(response.assessmentResults);
            getTotalRecord(response.totalCount)

        }
        , error: function (response) {
            //  customAlert("Session Ended Log Onto The System Again.");setTimeout(function () {window.location.href= HostedDir + '/login/Loginagency';   }, 2000);
        }
    });
    return false;
}
function getData(dataAgency) {
    listAgency = dataAgency;
}
function GoToNextPage(requestedPage, pageSize) {
    $('#assessmetResultsTable > thead > tr > th > img').css("visibility", "hidden");
    if (imgID != '' && imgID != 'undefined' && imgID != null) {
        direction = $("#" + imgID).siblings('input').val();
    }
    if (direction == "Asc") {
        sortDirection = $("#" + imgID).siblings('input').val();

    } else if (direction == "Desc") {
        sortDirection = $("#" + imgID).siblings('input').val();
    }
    getList();
    bindGridUser(listAgency, $("#assessmetResultsTable").find('tr')[0].cells.length);
}
function getlistafterstatuschanged() {
    getList();
    bindGridUser(listAgency, $("#assessmetResultsTable").find('tr')[0].cells.length);

}
function drawgrid() {
    requestedPage = 1;
    getList();
    bindGridUser(listAgency, $("#assessmetResultsTable").find('tr')[0].cells.length);
    LastIndex = 0;
    $('#First').attr('disabled', true);
    $('#Back').attr('disabled', true);
    return false;
}
function cleargrid() {

    sortOrder = '';
    sortDirection = '';
    $('#matrixValue').val('');
    $('#matrixType').val('');
    $('#btnUpdate').addClass('hidden');
    $('#btnadd').removeClass('hidden');
    cleanValidation();
    drawgrid();
    return false;

}
function fnChangePage(val) {
    pageLoadedFirst = false;
    pageSize = $('#ddlpagetodisplay').val();
    if (val == 'First') {
        StartIndex = 0;
        LastIndex = parseInt(pageSize) + parseInt(LastIndex * requestedPage);
        requestedPage = ((StartIndex / 10) + 1);
        GoToNextPage(requestedPage, pageSize);
        $('#First').attr('disabled', true);
        $('#Back').attr('disabled', true);
        $('#Next').attr('disabled', false);
        $('#Last').attr('disabled', false);
        LastIndex = 0;
    }
    else if (val == 'Last') {
        StartIndex = parseInt((totalRecords - 1) / pageSize) * pageSize;
        LastIndex = totalRecords;
        requestedPage = numOfPages;
        GoToNextPage(requestedPage, pageSize)
        $('#First').attr('disabled', false);
        $('#Back').attr('disabled', false);
        $('#Next').attr('disabled', true);
        $('#Last').attr('disabled', true);
    }
    else if (val == 'Next') {
        LastIndex = parseInt(pageSize) + parseInt(LastIndex);
        requestedPage = (parseInt(LastIndex / pageSize) + 1);
        GoToNextPage(requestedPage, pageSize);
        $('#First').attr('disabled', false);
        $('#Back').attr('disabled', false);
        if (parseInt(LastIndex) + parseInt(pageSize) >= totalRecords) {
            $('#Next').attr('disabled', true);
            $('#Last').attr('disabled', true);
        }
        else if (parseInt(LastIndex) - parseInt(pageSize) < totalRecords) {
            $('#Next').attr('disabled', false);
            $('#Last').attr('disabled', false);
        }
    }
    else if (val == 'Back') {
        requestedPage = requestedPage - 1;
        LastIndex = parseInt(LastIndex) - parseInt(pageSize);
        GoToNextPage(requestedPage, pageSize)
        if (parseInt(LastIndex) + parseInt(pageSize) > totalRecords) {
            $('#Next').attr('disabled', true);
            $('#Last').attr('disabled', true);
        }
        else if (parseInt(LastIndex) - parseInt(pageSize) < totalRecords) {
            $('#Next').attr('disabled', false);
            $('#Last').attr('disabled', false);
        }
        if (requestedPage == 1) {
            $('#First').attr('disabled', true);
            $('#Back').attr('disabled', true);
        }
    }
    else {
    }
}
function getTotalRecord(data) {
    $('#First').attr('disabled', false);
    $('#Back').attr('disabled', false);
    $('#Next').attr('disabled', false);
    $('#Last').attr('disabled', false);
    pageSize = $('#ddlpagetodisplay').val();
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
        $("#ddlpaging").val(requestedPage);
    }
    else {
        $('#First').attr('disabled', true);
        $('#Back').attr('disabled', true);
        $('#Next').attr('disabled', true);
        $('#Last').attr('disabled', true);
    }
}
function bindGridUser(data, num_cols) {
    $('#assessmetResultsTable > tbody > tr').remove();
    if (data.length == 0) {
        $('#divPaging').hide();
        $('#div1').show();
    }
    else {
        $('#div1').hide();
        $('#divPaging').show();
        var tbody = $('#assessmetResultsTable > tbody');
        for (var i = 0; i < data.length; i++) {
            console.log(data);
            var tr = "";
            tr += "<tr>";
            tr += "<td>" + ((typeof (data[i].AssessmentGroupType) != 'undefined') && (data[i].AssessmentGroupType != null) ? data[i].AssessmentGroupType : '') + "</td>";
            tr += "<td>" + ((typeof (data[i].MatrixType) != 'undefined') && (data[i].MatrixType != null) ? data[i].MatrixType : '') + " </span></td>";
            tr += "<td>" + ((typeof (data[i].MatrixValue) != 'undefined') && (data[i].MatrixValue != null) ? data[i].MatrixValue : '') + " </span></td>";
            tr += "<td>" + ((typeof (data[i].Description) != 'undefined') && (data[i].Description != null) ? data[i].Description : '') + " </span></td>";
            tr += "<td>" + ((typeof (data[i].ReferralSuggested) != 'undefined') && (data[i].ReferralSuggested != null) && (data[i].ReferralSuggested == true) ? 'Yes' : "No") + " </span></td>";
            tr += "<td>" + ((typeof (data[i].FPASuggested) != 'undefined') && (data[i].FPASuggested != null) && (data[i].FPASuggested == true) ? 'Yes' : "No") + " </span></td>";
            tr += "<td style='text-align:center;'><span class='glyphicon glyphicon-pencil updateResult' style='padding:0 20px;color:#295b8f;cursor:pointer;' accesskey=" + data[i].AssessmentResultId + " matrixId=" + data[i].MatrixId + " groupId=" + data[i].AssessmentGroupId + " title='Update'></span> <span class='glyphicon glyphicon-trash deleteGroup'  style='color:#295b8f;cursor:pointer;' onclick='deleteResult(this);' accesskey=" + data[i].AssessmentResultId + " title='Delete'></span></td>";
            tr += "</tr>";
            $('#assessmetResultsTable > tbody').append(tr);

        }
    }
}
