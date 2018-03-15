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

$(document).ready(function () {
    $('#DashboardHV').removeClass("active");
    drawgrid();
    $('#matrixTypeTable > thead > tr > th').children('i').addClass('sort-color');

    $('#matrixTypeTable > thead > tr > th').click(function () {
        sortOrder = this.id;

        if ($(this).children('i').hasClass('fa-sort'))
        {
            $('#matrixTypeTable > thead > tr > th').children('i').removeClass('fa-sort fa-sort-up fa-sort-down');
            $('#matrixTypeTable > thead > tr > th').children('i').addClass('fa-sort sort-color');
            $(this).children('i').removeClass('fa-sort sort-color');
            $(this).children('i').addClass('fa-sort-up');
        }
        else if ($(this).children('i').hasClass('fa-sort-up')) {
            $('#matrixTypeTable > thead > tr > th').children('i').removeClass('fa-sort fa-sort-up fa-sort-down');
            $('#matrixTypeTable > thead > tr > th').children('i').addClass('fa-sort sort-color');
            $(this).children('i').removeClass('fa-sort sort-color');
            $(this).children('i').addClass('fa-sort-down');
        }
        else if ($(this).children('i').hasClass('fa-sort-down')) {
            $('#matrixTypeTable > thead > tr > th').children('i').removeClass('fa-sort fa-sort-up fa-sort-down');
            $('#matrixTypeTable > thead > tr > th').children('i').addClass('fa-sort sort-color');
            $(this).children('i').removeClass('fa-sort sort-color');
            $(this).children('i').addClass('fa-sort-up');
        }
        if (sortOrder == "thSN")
            return false;
        if (sortOrder == "thRN")
            return false;
        imgID = $("#" + this.id).find('img').attr('id');
        $('#matrixTypeTable > thead > tr > th > img').css("visibility", "hidden");
        direction = $("#" + imgID).siblings('input').val();
        if (direction == "Asc") {
            $("#" + imgID).siblings('input').val("Desc");
            sortDirection = $("#" + imgID).siblings('input').val();
        } else {
            $("#" + imgID).siblings('input').val("Asc");
            sortDirection = $("#" + imgID).siblings('input').val();
        }
          
        getList();
        bindGridUser(listAgency, $("#matrixTypeTable").find('tr')[0].cells.length);
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

    $('body').on('click','.updateMatrix',function(){
        var matrixID = this.getAttribute('accesskey').trim();
        var matrixvalue = $(this).closest('tr').find('td').eq(0).html().trim();
        var matrixType = $(this).closest('tr').find('td').eq(1).html().trim();
        $('#matrixValue').val(matrixvalue);
        $('#matrixType').val(matrixType);
        $('#btnUpdate').attr('matrixId', matrixID);
        $('#btnUpdate').removeClass('hidden');
        $('#btnadd').addClass('hidden');
    });

    $('#btnUpdate').click(function () {
         
        if (checkInfo()) {

            //var matrixValue = parseInt($('#matrixValue').val().trim());
            var matrixValue = $('#matrixValue').val().trim();
            var matrixType = $('#matrixType').val().trim();
            var matrixId = $('#btnUpdate').attr('matrixId');
            $.ajax({
                url: "/Matrix/UpdateMatrixType",
                dataType: 'json',
                type: "POST",
                async: false,
                data: {
                    matrixId: matrixId, matrixType: matrixType, matrixValue: matrixValue
                },
                success: function (data) {
                    if (data) {
                        drawgrid();
                        clearcontrols();
                        customAlert("Record updated successfully.");
                        $('#btnUpdate').addClass('hidden');
                        $('#btnadd').removeClass('hidden');
                          
                    }

                    else
                        customAlert(data);

                },
                error: function (data) { alert(data); }
            });

        }
    });

    $('#matrixValue').keypress(function (evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        return true;
    });


});
function getListafterupdation() {

    pageSize = $('#ddlpagetodisplay').val();
    requestedPage = $('#ddlpaging').val();
    StartIndex = (pageSize * (requestedPage - 1)) + 1;
    LastIndex = parseInt(pageSize * requestedPage) - parseInt(pageSize);
    $('#matrixTypeTable > thead > tr > th > img').css("visibility", "hidden");
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
    bindGridUser(listAgency, $("#matrixTypeTable").find('tr')[0].cells.length);
    $("#ddlpaging").val(requestedPage);
}
function getList() {
    $.ajax({
        url:"/Matrix/GetMatrixType",
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
        success: function (data) {
            getData(data.matrixList);
            getTotalRecord(data.totalCount)
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
    $('#matrixTypeTable > thead > tr > th > img').css("visibility", "hidden");
    if (imgID != '' && imgID != 'undefined' && imgID != null) {
        direction = $("#" + imgID).siblings('input').val();
    }
    if (direction == "Asc") {
        sortDirection = $("#" + imgID).siblings('input').val();

    } else if (direction == "Desc") {
        sortDirection = $("#" + imgID).siblings('input').val();
    }
    getList();
    bindGridUser(listAgency, $("#matrixTypeTable").find('tr')[0].cells.length);
}
function getlistafterstatuschanged() {
    getList();
    bindGridUser(listAgency, $("#matrixTypeTable").find('tr')[0].cells.length);

}
function drawgrid() {
    requestedPage = 1;
    getList();
    bindGridUser(listAgency, $("#matrixTypeTable").find('tr')[0].cells.length);
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
    $('#matrixTypeTable > tbody > tr').remove();
    if (data.length == 0) {
        $('#divPaging').hide();
        $('#div1').show();
    }
    else {
        $('#div1').hide();
        $('#divPaging').show();
        var tbody = $('#matrixTypeTable > tbody');
        for (var i = 0; i < data.length; i++) {

            var tr = "";
            tr += "<tr>";
            tr += "<td>" + ((typeof (data[i].MatrixValue) != 'undefined') && (data[i].MatrixValue != null) ? data[i].MatrixValue:'')+"</td>";
            tr += "<td>" + ((typeof (data[i].MatrixType) != 'undefined') && (data[i].MatrixType != null) ? data[i].MatrixType : '') + " </span></td>";
            tr += "<td style='text-align:center;'><span  class='glyphicon glyphicon-pencil updateMatrix' accesskey=" + data[i].MatrixId + " style='padding:0 20px;color:#295b8f;cursor:pointer;' title='Update'></span> <span class='glyphicon glyphicon-trash'  style='color:#295b8f;cursor:pointer;' id='deleteMatrix' onclick='DeleteMatrixType(this);' accesskey=" + data[i].MatrixId + " title='Delete'></span></td>";
            tr += "</tr>";
            $('#matrixTypeTable > tbody').append(tr);

        }
    }
}
   

function checkInfo() {
    var isValid = true;
    cleanValidation();
    if ($('#matrixValue').val().trim() == "") {
        isValid = false;
        customAlert("Please enter Matrix Value.");
        plainValidation('#matrixValue');
        return isValid;
    }
    else if ($('#matrixType').val().trim()== "") {
        isValid = false;
        customAlert("Please enter Matrix Type.");
        plainValidation('#matrixType');
        return isValid;
    }
    return true;
}


function clearcontrols()
{
    $('#matrixValue').val('');
    $('#matrixType').val('');
}
function AddMatrixTypeDetails() {
      
    if (checkInfo()) {
          
        var matrixValue = parseInt($('#matrixValue').val().trim());
        var matrixType = $('#matrixType').val().trim();
           
        $.ajax({
            url: "/Matrix/AddMatrxiType",
            dataType: 'json',
            type: "POST",
            async: false,
            data: {
                matrixvalue: matrixValue, matrixtype: matrixType
            },
            success: function (data) {
                if (data==1) 
                {

                    drawgrid();
                    clearcontrols();
                    customAlert("Record added successfully.");
                }
                if (data ==2) {

                    customAlert("Record already exists.");
                    plainValidation('#matrixType');
                    plainValidation('#matrixValue');
                }

                else
                    customAlert(data);
                    
            },
            error: function (data) { alert(data); }
        });
        return false;
    }
}

function DeleteMatrixType(value) {
    BootstrapDialog.confirm('Do you want to delete this Matrix Type?', function (result) {
        if (result) {
            $.ajax({
                url: "/Matrix/DeleteMatrixType",
                type: "POST",
                data: {
                    ID: $(value).attr("accesskey"),
                },
                dataType: "json",
                secureuri: false,
                async: false,
                success: function (response) {

                    if (response == 1) {
                        customAlert("Matrix Type deleted successfully");
                        $(value).closest('tr').remove();

                    }
                    else if (response == 2) {
                        customAlert("Selected Matrix type is having the reference in Assessment Results");

                    }
                }
            , error: function (response) {
                customAlert("Session Ended Log Onto The System Again."); setTimeout(function () { window.location.href = HostedDir + '/login/Loginagency'; }, 2000);
            }
            });
        }
    });

}

function SetMatrix(value)
{
    var matrixID = this.getAttribute('accesskey');
    var matrixvalue = this.closest('tr').find('td').eq(0).html();
    var matrixType = this.closest('tr').find('td').eq(1).html();
    $('#matrixValue').val(matrixvalue);
    $('#matrixType').val(matrixType);
    $('#btnUpdate').attr('matrixId', matrixID);
    $('#btnUpdate').removeClass('hidden');
    $('#btnadd').addClass('hidden');
}
  
