var sortOrder = '';
var sortDirection = '';
var imgID = '';
var direction = '';
var pageSize = 10;
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
    sortOrder = 'CategoryPosition';
    sortDirection = 'ASC';
    drawgrid();

    $('#CategoryTable > thead > tr > th').children('i').addClass('sort-color');

    $('#CategoryTable > thead > tr > th').click(function () {
        sortOrder = this.id;
        if (sortOrder == "thSN")
            return false;
        if (sortOrder == "thRN")
            return false;
        if (sortOrder == "thPos")
            return false;

        if ($(this).children('i').hasClass('fa-sort')) {
            $('#CategoryTable > thead > tr > th').children('i').removeClass('fa-sort fa-sort-up fa-sort-down');
            $('#CategoryTable > thead > tr > th').children('i').addClass('fa-sort sort-color');
            $(this).children('i').removeClass('fa-sort sort-color');
            $(this).children('i').addClass('fa-sort-up');
        }
        else if ($(this).children('i').hasClass('fa-sort-up')) {
            $('#CategoryTable > thead > tr > th').children('i').removeClass('fa-sort fa-sort-up fa-sort-down');
            $('#CategoryTable > thead > tr > th').children('i').addClass('fa-sort sort-color');
            $(this).children('i').removeClass('fa-sort sort-color');
            $(this).children('i').addClass('fa-sort-down');
        }
        else if ($(this).children('i').hasClass('fa-sort-down')) {
            $('#CategoryTable > thead > tr > th').children('i').removeClass('fa-sort fa-sort-up fa-sort-down');
            $('#CategoryTable > thead > tr > th').children('i').addClass('fa-sort sort-color');
            $(this).children('i').removeClass('fa-sort sort-color');
            $(this).children('i').addClass('fa-sort-up');
        }
        imgID = $("#" + this.id).find('img').attr('id');
        $('#CategoryTable > thead > tr > th > img').css("visibility", "hidden");
        direction = $("#" + imgID).siblings('input').val();
        if (direction == "Asc") {
            $("#" + imgID).siblings('input').val("Desc");
            sortDirection = $("#" + imgID).siblings('input').val();
        } else {
            $("#" + imgID).siblings('input').val("Asc");
            sortDirection = $("#" + imgID).siblings('input').val();
        }
        getList();
        bindGridUser(listAgency, $("#CategoryTable").find('tr')[0].cells.length);
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


    $('#btnaddCategory').click(function () {
        cleanValidation();
        if ($('#categoryType').val().trim() == "") {
            customAlert("Please enter category type.");
            plainValidation('#categoryType');
            return false;
        }
        if ($('#txtCategoryPosition').val().trim() == "") {
            customAlert("Please enter category position.");
            plainValidation('#txtCategoryPosition');
            return false;
        }

       else if (!$.isNumeric($('#txtCategoryPosition').val().trim()))
        {
            customAlert("Please enter valid number.");
            plainValidation('#txtCategoryPosition');
            return false;
       }
       else if(parseInt($('#txtCategoryPosition').val().trim())==0)
       {
           customAlert("Please enter valid Position.");
           plainValidation('#txtCategoryPosition');
           return false;

          
       }

        //var count = 0;

       
        //$('.cat-position').each(function () {
        //    var availpos = parseInt($(this).text());
        //    if (availpos == pos) {
        //        count++;
        //    }
        //});
        //if (count > 0)
        //{
        //    customAlert("Already available.");
        //    plainValidation('#txtCategoryPosition');
        //    return false;
        //}

       else {

           var category = $('#categoryType').val().trim();
           var pos = parseInt($('#txtCategoryPosition').val().trim());
            $.ajax({
                url: "/Matrix/InsertAssessmentCategory",
                dataType: 'json',
                type: "POST",
                async: false,
                data: {
                    categoryName: category, categoryPosition:pos
                },
                success: function (data) {
                    if (data==1) {
                        drawgrid();
                       
                        clearcontrols();
                        customAlert("Record added successfully.");
                    }
                    if (data == 2) {
                        customAlert("Record already Exists");
                        plainValidation('#categoryType');
                        plainValidation('#txtCategoryPosition');
                    }

                    else
                        customAlert(data);

                },
                error: function (data) { alert(data); }
            });

        }
    });

    $('body').on('click', '.updateCategory' ,function(){
        var categoryId = this.getAttribute('accesskey');
        var categoryName = $(this).closest('tr').find('td').eq(0).html().trim();
        var catPosition = $(this).closest('tr').find('td').eq(1).html().trim();
        $('#categoryType').val(categoryName);
        $('#txtCategoryPosition').val(catPosition);
        $('#btnUpdateCategory').attr('category-id', categoryId);
        $('#btnaddCategory').addClass('hidden');
        $('#btnUpdateCategory').removeClass('hidden');
    })

    $('#btnUpdateCategory').click(function () {

        if ($('#categoryType').val().trim() == "") 
        {
            customAlert("Please enter category type.");
            plainValidation('#categoryType');
            return false;
        }
        if ($('#txtCategoryPosition').val().trim() == "") {
            customAlert("Please enter category position.");
            plainValidation('#txtCategoryPosition');
            return false;
        }
       else if (!$.isNumeric($('#txtCategoryPosition').val().trim())) {
            customAlert("Please enter valid number.");
            plainValidation('#txtCategoryPosition');
            return false;
        }
       else if (parseInt($('#txtCategoryPosition').val().trim()) == 0)
        {
            customAlert("Please enter valid Position.");
            plainValidation('#txtCategoryPosition');
            return false;
        }
        var catId = $(this).attr('category-id');
        var categoryname = $('#categoryType').val();
        var categoryPos =parseInt( $('#txtCategoryPosition').val().trim());
        $.ajax({
            url: "/Matrix/UpdateAssessmentCategory",
            dataType: 'json',
            type: "POST",
            async: false,
            data: {
                categoryName: categoryname, categoryId: parseInt(catId), position: categoryPos
            },
            success: function (data) {
                if (data==1) {
                    sortOrder = 'CategoryPosition';
                    sortDirection = 'ASC';
                    drawgrid();
                    clearcontrols();
                    customAlert("Record updated successfully.");
                    $('#btnUpdateCategory').addClass('hidden');
                    $('#btnaddCategory').removeClass('hidden');
                }
                if (data == 2) {
                    customAlert("Record already exists");
                    plainValidation('#categoryType');
                    plainValidation('#txtCategoryPosition');
                       
                }

                else
                    customAlert(data);

            },
            error: function (data) { alert(data); }
        });
          

    })

    $('#txtCategoryPosition').keypress(function (evt) {
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
    $('#CategoryTable > thead > tr > th > img').css("visibility", "hidden");
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
    bindGridUser(listAgency, $("#CategoryTable").find('tr')[0].cells.length);
    $("#ddlpaging").val(requestedPage);
}
function getList() {
    $.ajax({
        url:"/Matrix/GetAssessmentCategory",
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
            getData(response.category);
            getTotalRecord(response.totalCount)
        }
        , error: function (response) {
             customAlert("Session Ended Log Onto The System Again.");setTimeout(function () {window.location.href= HostedDir + '/login/Loginagency';   }, 2000);
        }
    });
}
function getData(dataAgency) {
    listAgency = dataAgency;
}
function GoToNextPage(requestedPage, pageSize) {
    $('#CategoryTable > thead > tr > th > img').css("visibility", "hidden");
    if (imgID != '' && imgID != 'undefined' && imgID != null) {
        direction = $("#" + imgID).siblings('input').val();
    }
    if (direction == "Asc") {
        sortDirection = $("#" + imgID).siblings('input').val();

    } else if (direction == "Desc") {
        sortDirection = $("#" + imgID).siblings('input').val();
    }
    getList();
    bindGridUser(listAgency, $("#CategoryTable").find('tr')[0].cells.length);
}
function getlistafterstatuschanged() {
    getList();
    bindGridUser(listAgency, $("#CategoryTable").find('tr')[0].cells.length);

}
function drawgrid() {
    requestedPage = 1;
    getList();
    bindGridUser(listAgency, $("#CategoryTable").find('tr')[0].cells.length);
    LastIndex = 0;
    $('#First').attr('disabled', true);
    $('#Back').attr('disabled', true);
}
function cleargrid() {

    sortOrder = 'CategoryPosition';
    sortDirection = 'ASC';
    $('#categoryType').val('');
    $('#txtCategoryPosition').val('');
    $('#btnaddCategory').removeClass('hidden');
    $('#btnUpdateCategory').addClass('hidden');
    $('button').attr('category-id', '');
    cleanValidation();

    drawgrid();

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
    $('#CategoryTable > tbody > tr').remove();
    if (data.length == 0) {
        $('#divPaging').hide();
        $('#div1').show();
    }
    else {
        $('#div1').hide();
        $('#divPaging').show();
        var tbody = $('#CategoryTable > tbody');
        for (var i = 0; i < data.length; i++) {
            //var inc = i+1;
            var tr = "";
            tr += "<tr>";
            //tr += "<td>" + inc + "</td>";
            tr += "<td>" + ((typeof (data[i].Category) != 'undefined') && (data[i].Category != null) ? data[i].Category : '') + " </span></td>";
            tr += "<td class='cat-position'>" + ((typeof (data[i].CategoryPosition) != 'undefined') && (data[i].CategoryPosition != null) ? data[i].CategoryPosition : '') + " </span></td>";
            tr += "<td style='text-align:center;'><span class='glyphicon glyphicon-pencil updateCategory' accesskey=" + data[i].AssessmentCategoryId + " style='padding:0 20px;color:#295b8f;cursor:pointer;' title='Update'></span> <span  class='glyphicon glyphicon-trash' style='color:#295b8f;cursor:pointer;' id='deleteCategory' onclick='deleteCategory(this);' accesskey=" + data[i].AssessmentCategoryId + " title='Delete'></a></td>";
            tr += "</tr>";
            $('#CategoryTable > tbody').append(tr);

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
    $('#categoryType').val('');
    $('#txtCategoryPosition').val('');
    cleanValidation();
    $('button').attr('category-id', "");
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
                if (data)
                {

                    drawgrid();
                    clearcontrols();
                    customAlert("Record added successfully.");
                }

                else
                    customAlert(data);

            },
            error: function (data) { alert(data); }
        });
    }
}

function deleteCategory(value) {
    BootstrapDialog.confirm('Do you want to delete this Category?', function (result) {
        if (result) {
            $.ajax({
                url: "/Matrix/DeleteAssessmentCategory",
                type: "POST",
                data: {
                    categoryId: $(value).attr("accesskey"),
                },
                dataType: "json",
                secureuri: false,
                async: false,
                success: function (response) {
                    if (response==1) {
                        customAlert("Category deleted successfully");
                        $(value).closest('tr').remove();

                    }
                    else if (response == 2) {
                        customAlert("Selected Category is having the reference in Assessment Group");

                    }
                }
            , error: function (response) {
                customAlert("Session Ended Log Onto The System Again."); setTimeout(function () { window.location.href = HostedDir + '/login/Loginagency'; }, 2000);
            }
            });
        }
    });

}
