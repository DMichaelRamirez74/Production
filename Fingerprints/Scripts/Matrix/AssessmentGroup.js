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
    drawgrid();
    $('#assessmentGroupTable > thead > tr > th').children('i').addClass('sort-color');

    $('#assessmentGroupTable > thead > tr > th').click(function () {
        sortOrder = this.id;
        if ($(this).children('i').hasClass('fa-sort')) {
            $('#assessmentGroupTable > thead > tr > th').children('i').removeClass('fa-sort fa-sort-up fa-sort-down');
            $('#assessmentGroupTable > thead > tr > th').children('i').addClass('fa-sort sort-color');
            $(this).children('i').removeClass('fa-sort sort-color');
            $(this).children('i').addClass('fa-sort-up');
        }
        else if ($(this).children('i').hasClass('fa-sort-up')) {
            $('#assessmentGroupTable > thead > tr > th').children('i').removeClass('fa-sort fa-sort-up fa-sort-down');
            $('#assessmentGroupTable > thead > tr > th').children('i').addClass('fa-sort sort-color');
            $(this).children('i').removeClass('fa-sort sort-color');
            $(this).children('i').addClass('fa-sort-down');
        }
        else if ($(this).children('i').hasClass('fa-sort-down')) {
            $('#assessmentGroupTable > thead > tr > th').children('i').removeClass('fa-sort fa-sort-up fa-sort-down');
            $('#assessmentGroupTable > thead > tr > th').children('i').addClass('fa-sort sort-color');
            $(this).children('i').removeClass('fa-sort sort-color');
            $(this).children('i').addClass('fa-sort-up');
        }
        if (sortOrder == "thSN")
            return false;
        if (sortOrder == "thRN")
            return false;
        imgID = $("#" + this.id).find('img').attr('id');
        $('#assessmentGroupTable > thead > tr > th > img').css("visibility", "hidden");
        direction = $("#" + imgID).siblings('input').val();
        if (direction == "Asc") {
            $("#" + imgID).siblings('input').val("Desc");
            sortDirection = $("#" + imgID).siblings('input').val();
        } else {
            $("#" + imgID).siblings('input').val("Asc");
            sortDirection = $("#" + imgID).siblings('input').val();
        }
        getList();
        bindGridUser(listAgency, $("#assessmentGroupTable").find('tr')[0].cells.length);
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

    $.ajax({
        url: "/Matrix/GetAssessmentCategoryList",
        type: "POST",
        dataType: "json",
        secureuri: false,
        async: false,
        success: function (response1) {
            if (response1.length > 0)
            {
                $('.select-group').html('');
                $('.select-group').append('<option value=0>Select the category</option>')
                for (var i = 0; i < response1.length; i++)
                {
                    $('.select-group').append('<option value=' + response1[i].AssessmentCategoryId + '>' + response1[i].Category + '</option>')
                }

            }
        }
        , error: function (response1) {
              customAlert("Session Ended Log Onto The System Again.");setTimeout(function () {window.location.href= HostedDir + '/login/Loginagency';   }, 2000);
        }
    });

    $('#btnaddGroup').click(function () {

        if ($('.select-group').val() == 0)
        {
            customAlert("Please select Assessment Category");
            plainValidation('.select-group');
            return false;
        }
        if ($('#assessmentGroupText').val().trim() == "")
        {
            customAlert("Please select Assessment Group");
            plainValidation('#assessmentGroupText');
            return false;
        }
        if (!($("input:radio[name='statusradio']").is(":checked"))) {
            customAlert("Please select status");
            plainValidation('statusradio');
            return false;
        }
        else {
            var status = parseInt($("input[name='statusradio']:checked").val());
            var assessmentCatID = parseInt($('.select-group').val());
            var assessmentGroupType = $('#assessmentGroupText').val();

            $.ajax({
                url: "/Matrix/InsertAssessmentGroup",
                dataType: 'json',
                type: "POST",
                async: false,
                data: {
                    groupType: assessmentGroupType, categoryId: assessmentCatID, status:status
                },
                success: function (data) {
                    if (data == 1) {
                        customAlert("Record saved successfully.");
                        sortOrder = "CreatedDate";
                        sortDirection = "DESC";
                  
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

    $('body').on('click', '.updateGroup', function () {
        var groupId = this.getAttribute('accesskey');
        var categoryId = this.getAttribute('categoryId');
        var categoryName = $(this).closest('tr').find('td').eq(0).html().trim();
        var groupName = $(this).closest('tr').find('td').eq(1).html().trim();
        var GroupStatus = $(this).closest('tr').find('td').eq(2).html().trim();
        $('.select-group').val(categoryId);
        $('#assessmentGroupText').val(groupName);
        $('#btnUpdateGroup').attr('groupId', groupId);
        $('#btnaddGroup').addClass('hidden');
        $('#btnUpdateGroup').removeClass('hidden');
        if (GroupStatus == "Active")
        {
            $('input[name="statusradio"][value=1]').prop('checked', true)
        }
        else
        {
            $('input[name="statusradio"][value=0]').prop('checked', true)
        }
    });

    $('#btnUpdateGroup').click(function () {
        if ($('.select-group').val() == 0) {
            customAlert("Please select Assessment Category");
            plainValidation('.select-group');
            return false;
        }
        if ($('#assessmentGroupText').val().trim() == "") {
            customAlert("Please select Assessment Group");
            plainValidation('#assessmentGroupText');
            return false;
        }
        if (!($("input:radio[name='statusradio']").is(":checked"))) {
            customAlert("Please select status");
            plainValidation('statusradio');
            return false;
        }
        else {
            var GroupStatus = parseInt($("input[name='statusradio']:checked").val());
            var assessmentCatID = parseInt($('.select-group').val());
            var assessmentGroupType = $('#assessmentGroupText').val();
            var GroupId = parseInt($(this).attr('groupId'));

            $.ajax({
                url:"/Matrix/UpdateAssessmentGroup",
                dataType: 'json',
                type: "POST",
                async: false,
                data: {
                    groupType: assessmentGroupType, categoryId: assessmentCatID, status: GroupStatus, groupId: GroupId
                },
                success: function (data) {
                    if (data==1) {
                        customAlert("Record saved successfully.");
                        sortOrder = 'ModifiedDate';
                        sortDirection = "DESC";
                        
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
          

    $('#btnGroupCancel').click(function () {

        $('.select-group').val(0);
        $('#assessmentGroupText').val('');
        $('input[name="statusradio"][value=1]').prop('checked', true);
        $('#btnUpdateGroup').addClass('hidden');
        $('#btnaddGroup').removeClass('hidden');
        cleanValidation();

    });
});


function deleteGroup(value)
{
    var groupID = parseInt($(value).attr("accesskey"));
    BootstrapDialog.confirm('Do you want to delete this Assessment group type ?', function (result) {
        if (result) {
            $.ajax({
                url: "/Matrix/DeleteAssessmentGroup",
                type: "POST",
                data: {
                    groupId: groupID
                },
                dataType: "json",
                secureuri: false,
                async: false,
                success: function (response) {

                    if (response==1) {
                        customAlert("Assessment Group Type deleted successfully.");
                        $(value).closest('tr').remove();

                    }
                    else if (response == 2) {
                        customAlert("Select Assessment Group is having the refernce in another table");
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

    $('.select-group').val(0);
    $('#assessmentGroupText').val('');
    $('input[name="statusradio"][value=1]').prop('checked', true);
    $('#btnUpdateGroup').addClass('hidden');
    $('#btnaddGroup').removeClass('hidden');
}


function getListafterupdation() {

    pageSize = $('#ddlpagetodisplay').val();
    requestedPage = $('#ddlpaging').val();
    StartIndex = (pageSize * (requestedPage - 1)) + 1;
    LastIndex = parseInt(pageSize * requestedPage) - parseInt(pageSize);
    $('#assessmentGroupTable > thead > tr > th > img').css("visibility", "hidden");
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
    bindGridUser(listAgency, $("#assessmentGroupTable").find('tr')[0].cells.length);
    $("#ddlpaging").val(requestedPage);
}
function getList() {
    $.ajax({
        url: "/Matrix/GetAssessmentGroup",
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
              
            getData(response.assessmentGroup);
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
    $('#assessmentGroupTable > thead > tr > th > img').css("visibility", "hidden");
    if (imgID != '' && imgID != 'undefined' && imgID != null) {
        direction = $("#" + imgID).siblings('input').val();
    }
    if (direction == "Asc") {
        sortDirection = $("#" + imgID).siblings('input').val();

    } else if (direction == "Desc") {
        sortDirection = $("#" + imgID).siblings('input').val();
    }
    getList();
    bindGridUser(listAgency, $("#assessmentGroupTable").find('tr')[0].cells.length);
}
function getlistafterstatuschanged() {
    getList();
    bindGridUser(listAgency, $("#assessmentGroupTable").find('tr')[0].cells.length);

}
function drawgrid() {
    requestedPage = 1;
    getList();
    bindGridUser(listAgency, $("#assessmentGroupTable").find('tr')[0].cells.length);
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
    $('#assessmentGroupTable > tbody > tr').remove();
    if (data.length == 0) {
        $('#divPaging').hide();
        $('#div1').show();
    }
    else {
        $('#div1').hide();
        $('#divPaging').show();
        var tbody = $('#assessmentGroupTable > tbody');
        for (var i = 0; i < data.length; i++) {

            var tr = "";
            tr += "<tr>";
            tr += "<td>" + ((typeof (data[i].Category) != 'undefined') && (data[i].Category != null) ? data[i].Category : '') + "</td>";
            tr += "<td>" + ((typeof (data[i].AssessmentGroupType) != 'undefined') && (data[i].AssessmentGroupType != null) ? data[i].AssessmentGroupType : '') + " </span></td>";
            tr += "<td>" + ((typeof (data[i].IsActive) != 'undefined') && (data[i].IsActive != null) && (data[i].IsActive==true) ? 'Active' : 'InActive') + " </span></td>";
            tr += "<td style='text-align:center;'><span class='glyphicon glyphicon-pencil updateGroup' style='padding:0 20px;color:#295b8f;cursor:pointer;'  accesskey=" + data[i].AssessmentGroupId + " categoryId=" + data[i].AssessmentCategoryId + " title='Update'></span> <span class='glyphicon glyphicon-trash deleteGroup'  style='color:#295b8f;cursor:pointer;' onclick='deleteGroup(this);' accesskey=" + data[i].AssessmentGroupId + " title='Delete'></span></td>";
            tr += "</tr>";
            $('#assessmentGroupTable > tbody').append(tr);

        }
    }
}

