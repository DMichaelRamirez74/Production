var lengthTd = 0;

var requestedPage = 0;
var pageSize = 10;
var StartIndex = 0;
var LastIndex = 0;
var numOfPages = 0;

var totalRecords = 0;
$(document).ready(function () {

    $('.modal').find('.childName').hover(function () {
        $(this).children('span').css('visibility', 'visible');
    });


    $('.modal').on('show.bs.modal', function (e) {

        if (lengthTd < 5) {
            $(this).find(".scroll-thead").css({ "width": "100%" });
        }
        else {
            $(this).find(".scroll-thead").css({ "width": "calc( 100% - 17px )" });
        }
        changeModalSize(this);
    });

    $(window).bind("load resize", function () {

        topOffset = 50;
        width = (this.window.innerWidth > 0) ? this.window.innerWidth : this.screen.width;
        if (width < 768) {
            topOffset = 100; // 2-row-menu
        }

        height = ((this.window.innerHeight > 0) ? this.window.innerHeight : this.screen.height) - 1;

        height = height - topOffset;
        if (height < 1) height = 1;
        if (height > topOffset) {
            var fileHeight = height - 25;

            $('.right-side-container-ch').css('min-height', (fileHeight - 5) + 'px');
        }

    });


    getList(0);
});
$('#searchByprogramId').click(function () {
    var Prog_Id = $('.select_programType option:selected').val();;
    getList(Prog_Id);
});

function changeModalSize(mod) {
    $(mod).find('tbody').height($(window).height() - 412);

}
function getList(programID) {

    $('#spinner').show();
    $.ajax({
        url: '/ERSEA/GetCenterAnalysisList',
        datatype: 'json',
        type: 'post',
        data: { programID: programID },
        success: function (data) {

            $('#spinner').hide();
            var bindDiv = $('.bindTable');
            var appendDiv = '';
            if (data.centerAnalysisList.length > 0) {
                $.each(data.centerAnalysisList, function (i, centerElement) {

                    bindDiv.find('.center-name').html(centerElement.CenterName).attr('center-id', centerElement.Enc_CenterId);
                    bindDiv.find('.seats-count').html(centerElement.Seats);

                    if (centerElement.Enrolled > 0) {
                        bindDiv.find('.enroll-count').html(centerElement.Enrolled).css({ 'text-decoration': 'underline', 'cursor': 'pointer' });
                    }
                    else {
                        bindDiv.find('.enroll-count').html(centerElement.Enrolled).css({ 'text-decoration': 'none' });
                    }

                    if (centerElement.WithDrawn > 0) {
                        bindDiv.find('.withdrawn-count').html(centerElement.WithDrawn).css({ 'text-decoration': 'underline', 'cursor': 'pointer' });
                    }
                    else {
                        bindDiv.find('.withdrawn-count').html(centerElement.WithDrawn).css({ 'text-decoration': 'none' });
                    }

                    if (centerElement.Dropped > 0) {
                        bindDiv.find('.dropped-count').html(centerElement.Dropped).css({ 'text-decoration': 'underline', 'cursor': 'pointer' });
                    }
                    else {
                        bindDiv.find('.dropped-count').html(centerElement.Dropped).css({ 'text-decoration': 'none' });
                    }


                    if (centerElement.Waiting > 0) {
                        bindDiv.find('.waiting-count').html('<span style="text-decoration:underline;cursor:pointer;float:left;" class="waiting-count-span">' + centerElement.Waiting + '</span>  <span style="color: #0fbc0f;font-weight: bold;text-align:right;float:right;">' + centerElement.PercentageFilled.toFixed(1) + '%') + '</span>';

                    }
                    else {
                        bindDiv.find('.waiting-count').html('<span style="text-decoration:none;float:left;">' + centerElement.Waiting + '</span> <span style="color: #0fbc0f;font-weight: bold;text-align:right;float:right;">' + centerElement.PercentageFilled.toFixed(1) + '%') + '</span>';

                    }

                    if (centerElement.Returning > 0) {
                        bindDiv.find('.return-count').html(centerElement.Returning).css({ 'text-decoration': 'underline', 'cursor': 'pointer' }).attr('center', 'count_' + centerElement.Enc_CenterId);
                    }
                    else {
                        bindDiv.find('.return-count').html(centerElement.Returning).css({ 'text-decoration': 'none' }).attr('center', 'count_' + centerElement.Enc_CenterId);

                    }
                    if (centerElement.Graduating > 0) {
                        bindDiv.find('.grad-count').html(centerElement.Graduating).css({ 'text-decoration': 'underline', 'cursor': 'pointer' }).attr({ 'center-id': centerElement.Enc_CenterId });

                    }
                    else {
                        bindDiv.find('.grad-count').html(centerElement.Graduating).css({ 'text-decoration': 'none' }).attr({ 'center-id': centerElement.Enc_CenterId });

                    }

                    if (centerElement.OverIncome > 0) {
                        bindDiv.find('.oi-count').html(centerElement.OverIncome).css({ 'text-decoration': 'underline', 'cursor': 'pointer' }).attr({ 'center-id': centerElement.Enc_CenterId });

                    }
                    else {
                        bindDiv.find('.oi-count').html(centerElement.OverIncome).css({ 'text-decoration': 'none' }).attr({ 'center-id': centerElement.Enc_CenterId });

                    }

                    bindDiv.find('.foster-count').html(centerElement.Foster).attr('en-Cid', centerElement.Enc_CenterId);
                    if (centerElement.Foster > 0) {
                        bindDiv.find('.foster-count').css({ 'cursor': 'pointer', 'text-decoration': 'underline', 'cursor': 'pointer' }).html(centerElement.Foster).attr({ 'center-id': centerElement.CenterId, 'title': 'Click to View Foster Childs' });
                    }
                    else {
                        bindDiv.find('.foster-count').css({ 'cursor': 'auto', 'text-decoration': 'none' }).html(centerElement.Foster).attr({ 'center-id': centerElement.CenterId, 'onclick': '' });
                    }
                    if (centerElement.HomeLess > 0) {
                        bindDiv.find('.homeless-count').html(centerElement.HomeLess).css({ 'text-decoration': 'underline', 'cursor': 'pointer' }).attr({ 'center-id': centerElement.Enc_CenterId });

                    }
                    else {
                        bindDiv.find('.homeless-count').html(centerElement.HomeLess).css({ 'text-decoration': 'none' }).attr({ 'center-id': centerElement.Enc_CenterId });

                    }
                    if (centerElement.Leads > 0) {
                        bindDiv.find('.leads-count').html(centerElement.Leads).css({ 'text-decoration': 'underline', 'cursor': 'pointer' }).attr({ 'center-id': centerElement.Enc_CenterId });
                    }
                    else {
                        bindDiv.find('.leads-count').html(centerElement.Leads).css({ 'text-decoration': 'none' }).attr({ 'center-id': centerElement.Enc_CenterId });
                    }

                    appendDiv += bindDiv.find('tbody').html();
                });

            }
            else {
                appendDiv = '<tr><td colspan=10 style="font-size=20px;padding-top:20px;text-align:center;line-height:30px;padding-bottom:20px;">No details found</td></tr>';
            }
            var totalPerDiv = '';
            totalPerDiv += '<tr style="height: 10px;"></tr> <tr> <td data-title="Center Name">' +
               '<p><strong>Total</strong></p></td>';

            totalPerDiv += '<td data-title="Seats">' +
                           '<p><strong>' + data.calcAnalysis.TotalSeats + '</strong></p></td>';

            if (data.calcAnalysis.TotalEnrolled > 0) {
                // totalPerDiv += '<td data-title="Enrolled">' +
                //'<p><strong style="text-decoration:underline;cursor:pointer;" class="total_enroll" onclick="getEnrolledChildrens(0);">' + data.calcAnalysis.TotalEnrolled + '</strong></p></td>';

                totalPerDiv += '<td data-title="Enrolled">' +
                              '<p><strong style="text-decoration:none;" class="total_enroll">' + data.calcAnalysis.TotalEnrolled + '</strong></p></td>';
            }
            else {
                totalPerDiv += '<td data-title="Enrolled">' +
                              '<p><strong style="text-decoration:none;" class="total_enroll">' + data.calcAnalysis.TotalEnrolled + '</strong></p></td>';
            }
            if (data.calcAnalysis.TotalWithdrawn > 0) {
                // totalPerDiv += '<td data-title="WithDrawn"><p><strong style="text-decoration:underline;cursor:pointer;" class="total_withdrawn" onclick="getWithdrawnChildren(0);">' + data.calcAnalysis.TotalWithdrawn + '</strong></p></td>';
                totalPerDiv += '<td data-title="WithDrawn"><p><strong style="text-decoration:none;" class="total_withdrawn">' + data.calcAnalysis.TotalWithdrawn + '</strong></p></td>';

            }
            else {
                totalPerDiv += '<td data-title="WithDrawn"><p><strong style="text-decoration:none;" class="total_withdrawn">' + data.calcAnalysis.TotalWithdrawn + '</strong></p></td>';
            }
            if (data.calcAnalysis.TotalDropped > 0) {
                //totalPerDiv += '<td data-title="Dropped"><p><strong style="text-decoration:underline;cursor:pointer;" class="total_dropped" onclick="getDroppedChildren(0);"  >' + data.calcAnalysis.TotalDropped + '</strong></p></td>';

                totalPerDiv += '<td data-title="Dropped"><p><strong style="text-decoration:none;" class="total_dropped"  >' + data.calcAnalysis.TotalDropped + '</strong></p></td>';

            }
            else {
                totalPerDiv += '<td data-title="Dropped"><p><strong style="text-decoration:none;" class="total_dropped"  >' + data.calcAnalysis.TotalDropped + '</strong></p></td>';
            }
            if (data.calcAnalysis.TotalWaiting > 0) {
                // totalPerDiv += '<td data-title="Waiting"> <p style="text-align:right;padding-bottom:20px;"><strong ><span style="text-decoration:underline;float:left;cursor:pointer;" class="total_waiting" onclick="getWaitingChildren(0);">' + data.calcAnalysis.TotalWaiting + '</span>   <span  style="color: #0fbc0f;font-weight: bold;float:right;"> ' + parseFloat(data.calcAnalysis.TotalPercentageFilled).toFixed(1) + '% </span></strong></p></td>';

                totalPerDiv += '<td data-title="Waiting"> <p style="text-align:right;padding-bottom:20px;"><strong ><span style="text-decoration:none;float:left;" class="total_waiting">' + data.calcAnalysis.TotalWaiting + '</span>   <span  style="color: #0fbc0f;font-weight: bold;float:right;"> ' + parseFloat(data.calcAnalysis.TotalPercentageFilled).toFixed(1) + '% </span></strong></p></td>';

            }
            else {
                totalPerDiv += '<td data-title="Waiting"> <p style="text-align:right;padding-bottom:20px;"><strong ><span style="text-decoration:none;float:left;" class="total_waiting">' + data.calcAnalysis.TotalWaiting + '</span>   <span  style="color: #0fbc0f;font-weight: bold;float:right;"> ' + parseFloat(data.calcAnalysis.TotalPercentageFilled).toFixed(1) + '% </span></strong></p></td>';

            }
            if (data.calcAnalysis.TotalReturned > 0) {
                // totalPerDiv += '<td data-title="Return"> <p><strong style="text-decoration:underline;cursor:pointer;" class="total_return" onclick="getReturningChildrens(0);" >' + data.calcAnalysis.TotalReturned + '</strong></p></td>';

                totalPerDiv += '<td data-title="Return"> <p><strong style="text-decoration:none;" class="total_return"  >' + data.calcAnalysis.TotalReturned + '</strong></p></td>';

            }
            else {
                totalPerDiv += '<td data-title="Return"> <p><strong style="text-decoration:none;" class="total_return"  >' + data.calcAnalysis.TotalReturned + '</strong></p></td>';

            }

            if (data.calcAnalysis.TotalGraduating > 0) {
                //  totalPerDiv += '<td data-title="Grad"><p><strong class="total_Grad" style="text-decoration:underline;cursor:pointer;" onclick="getGraduatingChildrens(0);">' + data.calcAnalysis.TotalGraduating + '</strong></p></td>';

                totalPerDiv += '<td data-title="Grad"><p><strong class="total_Grad" style="text-decoration:none;">' + data.calcAnalysis.TotalGraduating + '</strong></p></td>';

            }
            else {
                totalPerDiv += '<td data-title="Grad"><p><strong class="total_Grad" style="text-decoration:none;">' + data.calcAnalysis.TotalGraduating + '</strong></p></td>';
            }
            if (data.calcAnalysis.TotalOverIncome > 0) {

                totalPerDiv += '<td data-title="O/I"><p><strong style="text-decoration:underline;cursor:pointer;" onclick="getOverIncomChildren(0,0,1,10);" class="total_overincome" >' + data.calcAnalysis.TotalOverIncome + '</strong></p></td>';


            }
            else {
                totalPerDiv += '<td data-title="O/I"><p><strong style="text-decoration:none;" class="total_overincome" >' + data.calcAnalysis.TotalOverIncome + '</strong></p></td>';

            }
            if (data.calcAnalysis.TotalFoster > 0) {
                totalPerDiv += '<td data-title="Foster"> <p><strong class="total_Foster" style="text-decoration:underline;cursor:pointer;" onclick="getFosterChild(0,0,1,10);" >' + data.calcAnalysis.TotalFoster + '</strong></p></td>';
            }
            else {
                totalPerDiv += '<td data-title="Foster"> <p><strong class="total_Foster" style="text-decoration:none;">' + data.calcAnalysis.TotalFoster + '</strong></p></td>';

            }
            if (data.calcAnalysis.TotalHomeLess > 0) {
                totalPerDiv += '<td data-title="Homeless"> <p><strong style="text-decoration:underline;cursor:pointer;" class="total_homeless" onclick="getHomelessChildren(0, 0, 1, 10);"  >' + data.calcAnalysis.TotalHomeLess + '</strong></p></td>';

            }

            else {
                totalPerDiv += '<td data-title="Homeless"> <p><strong style="text-decoration:none;" class="total_homeless"  >' + data.calcAnalysis.TotalHomeLess + '</strong></p></td>';

            }
            if (data.calcAnalysis.TotalLeads > 0) {
                totalPerDiv += '<td data-title="Leads"><p><strong style="text-decoration:underline;cursor:pointer;" class="total_leads" onclick="getLeadsChildren(0,0,1,10);">' + data.calcAnalysis.TotalLeads + '</strong></p></td>';

            }
            else {
                totalPerDiv += '<td data-title="Leads"><p><strong style="text-decoration:none;" class="total_leads">' + data.calcAnalysis.TotalLeads + '</strong></p></td>';

            }
            totalPerDiv += '</tr>';








            //appendDiv += totalPerDiv;
            $('.tbody_total').html(totalPerDiv);
            var selectOption = '<option value="0">ALL</option>';
            if (data.programList.length > 0) {
                if (data.programList.length === 1) {
                    selectOption = '<option value=' + data.programList[0].Value + '>' + data.programList[0].Text + '</option>';
                }
                else {


                    $.each(data.programList, function (i, programs) {
                        selectOption += '<option value=' + programs.Value + '>' + programs.Text + '</option>';
                    });
                }
            }
            $('.select_programType').html(selectOption);
            $('.select_programType').val(programID);
            $('.bind_body').html(appendDiv);

            //dynamic change of table body height//
            var bodyHeight = 0;
            $('.bind_body ').find('tr').each(function () {
                bodyHeight += $(this).height();
            });
            var bindBodyHeight = $('.bind_body').height();
            if (bodyHeight <= bindBodyHeight) {
                $('.bind_body').css({
                    'height': bodyHeight

                });

                $('#centerTable').find(".scroll-thead").css({ "width": "100%" });
            }
            else {
                $('#centerTable').find(".scroll-thead").css({ "width": "calc( 100% - 17px )" });
            }


        },
        error: function (data) {
            $('#spinner').hide();

        }

    });
}

$('.table-hd').on('change', '.prog_select_head', function () {
    var ProgramID = $(this).val();
    getReturningByProgram(ProgramID);
});

function getReturningByProgram(progID) {
    $.ajax({

        url: "/ERSEA/GetGraduatingChildByProgram",
        dataType: "json",
        type: "POST",
        data: { programTypeID: progID },
        //contentType: "application/json; charset=utf-8",
        success: function (data) {

            if (data.gradChildList.length > 0) {
                $.each(data.gradChildList, function (k, gradChild) {
                    var attrId = gradChild.Value;
                    $(".bind_body").find("[center-id='" + attrId + "']").html(gradChild.Text);
                });
            }
            $('.total_Grad').html(data.totalGradcount);
        },
        error: function (data) {

        }
    });

}

$('.modal-body').on('click', '.attachment', function (e) {

    if ($(this).attr('data-type') == 'img') {
        var dlnk1 = document.getElementById('dwnldLnkimage_Center');
        dlnk1.href = $(this).find('img').attr('src');
        dlnk1.click();

        var image = new Image();
        image.src = $(this).find('img').attr('src');
        var w = window.open("");
        w.document.write(image.outerHTML);
    }
    else {
        var dlnk = document.getElementById('dwnldLnk_Center');
        dlnk.href = $(this).find('img').attr('src');
        dlnk.click();
    }



});

$('#centerTable').on('click', '.center-name', function () {
    var centerName = $(this).html();
    var enrCount = $(this).closest('tr').find('td').eq(2).children('p').html();
    if (enrCount == '0') {

        customAlert('Childrens are not available');
        return false;
    }
    var centerId = $(this).attr('center-id');
    var center_Name = $(this).html();
    var ProgramId = $('.select_programType').val();
    $('#FosterDisplaymodal').find('#centerName').html(center_Name);

    requestedPage = 1;
    pageSize = 10;
    Displayrosters(centerId, ProgramId, requestedPage, pageSize);
    LastIndex = 0;
    $('#FosterDisplaymodal').find('#First').attr('disabled', true);
    $('#FosterDisplaymodal').find('#Back').attr('disabled', true);
    $('#FosterDisplaymodal').find('#ddlpagetodisplay').val(10);
});

$('#centerTable').on('click', '.enroll-count', function () {
    if ($(this).html() == '0') {
        return false;
    }
    lengthTd = $(this).html();
    var center_ID = $(this).closest('tr').children('td').eq(0).children('p').attr('center-id');
    var ProgramId = $('.select_programType').val();
    requestedPage = 1;
    pageSize = 10;
    getEnrolledChildrens(center_ID, ProgramId, requestedPage, pageSize);
    LastIndex = 0;
    $('#EnrolledModal').find('#First').attr('disabled', true);
    $('#EnrolledModal').find('#Back').attr('disabled', true);
    $('#EnrolledModal').find('#ddlpagetodisplay').val(10);

});

$('#centerTable').on('click', '.withdrawn-count', function () {
    if ($(this).html() == '0') {
        return false;
    }
    lengthTd = $(this).html();
    var Wait_center_ID = $(this).closest('tr').children('td').eq(0).children('p').attr('center-id');
    var ProgramId = $('.select_programType').val();
    requestedPage = 1;
    pageSize = 10;
    getWithdrawnChildren(Wait_center_ID, ProgramId, requestedPage, pageSize);
    LastIndex = 0;
    $('#WithdrawnModal').find('#First').attr('disabled', true);
    $('#WithdrawnModal').find('#Back').attr('disabled', true);
    $('#WithdrawnModal').find('#ddlpagetodisplay').val(10);

});

$('#centerTable').on('click', '.dropped-count', function () {
    if ($(this).html() == '0') {
        return false;
    }
    lengthTd = $(this).html();

    var Wait_center_ID = $(this).closest('tr').children('td').eq(0).children('p').attr('center-id');
    var ProgramId = $('.select_programType').val();
    requestedPage = 1;
    pageSize = 10;
    getDroppedChildren(Wait_center_ID, ProgramId, requestedPage, pageSize);
    LastIndex = 0;
    $('#DroppedModal').find('#First').attr('disabled', true);
    $('#DroppedModal').find('#Back').attr('disabled', true);
    $('#DroppedModal').find('#ddlpagetodisplay').val(10);

});

$('#centerTable').on('click', '.waiting-count-span', function () {
    if ($(this).html() == '0') {
        return false;
    }
    lengthTd = $(this).html();
    var Wait_center_ID = $(this).closest('tr').children('td').eq(0).children('p').attr('center-id');
    var ProgramId = $('.select_programType').val();
    requestedPage = 1;
    pageSize = 10;
    getWaitingChildren(Wait_center_ID, ProgramId, requestedPage, pageSize);
    LastIndex = 0;
    $('#WaitingModal').find('#First').attr('disabled', true);
    $('#WaitingModal').find('#Back').attr('disabled', true);
    $('#WaitingModal').find('#ddlpagetodisplay').val(10);

});


$('#centerTable').on('click', '.return-count', function () {
    if ($(this).html() == '0') {
        return false;
    }
    lengthTd = $(this).html();
    var center_ID = $(this).closest('tr').children('td').eq(0).children('p').attr('center-id');
    var ProgramId = $('.select_programType').val();
    requestedPage = 1;
    pageSize = 10;

    getReturningChildrens(center_ID, ProgramId, requestedPage, pageSize);
    LastIndex = 0;
    $('#ReturningModal').find('#First').attr('disabled', true);
    $('#ReturningModal').find('#Back').attr('disabled', true);
    $('#ReturningModal').find('#ddlpagetodisplay').val(10);

});

$('#centerTable').on('click', '.grad-count', function () {
    if ($(this).html() == '0') {
        return false;
    }
    lengthTd = $(this).html();
    var center_ID = $(this).closest('tr').children('td').eq(0).children('p').attr('center-id');
    var ProgramId = $('.select_programType').val();
    requestedPage = 1;
    pageSize = 10;
    getGraduatingChildrens(center_ID, ProgramId, requestedPage, pageSize);
    LastIndex = 0;
    $('#GraduatingModal').find('#First').attr('disabled', true);
    $('#GraduatingModal').find('#Back').attr('disabled', true);
    $('#GraduatingModal').find('#ddlpagetodisplay').val(10);

});

$('#centerTable').on('click', '.oi-count', function () {
    if ($(this).html() == '0') {
        return false;
    }
    lengthTd = $(this).html();
    var center_ID = $(this).closest('tr').children('td').eq(0).children('p').attr('center-id');
    var ProgramId = $('.select_programType').val();
    requestedPage = 1;
    pageSize = 10;
    getOverIncomChildren(center_ID, ProgramId, requestedPage, pageSize);
    LastIndex = 0;
    $('#OverIncomeModal').find('#First').attr('disabled', true);
    $('#OverIncomeModal').find('#Back').attr('disabled', true);
    $('#OverIncomeModal').find('#ddlpagetodisplay').val(10);

});


$('#centerTable').on('click', '.foster-count', function () {
    if ($(this).html() == '0') {
        return false;
    }
    lengthTd = $(this).html();
    var center_ID = $(this).closest('tr').children('td').eq(0).children('p').attr('center-id');
    var ProgramId = $('.select_programType').val();
    requestedPage = 1;
    pageSize = 10;
    getFosterChild(center_ID, ProgramId, requestedPage, pageSize);
    LastIndex = 0;
    $('#FosterModal').find('#First').attr('disabled', true);
    $('#FosterModal').find('#Back').attr('disabled', true);
    $('#FosterModal').find('#ddlpagetodisplay').val(10);

});

$('#centerTable').on('click', '.homeless-count', function () {
    if ($(this).html() == '0') {
        return false;
    }
    lengthTd = $(this).html();
    var center_ID = $(this).closest('tr').children('td').eq(0).children('p').attr('center-id');
    var ProgramId = $('.select_programType').val();
    requestedPage = 1;
    pageSize = 10;
    getHomelessChildren(center_ID, ProgramId, requestedPage, pageSize);
    LastIndex = 0;
    $('#HomeLessModal').find('#First').attr('disabled', true);
    $('#HomeLessModal').find('#Back').attr('disabled', true);
    $('#HomeLessModal').find('#ddlpagetodisplay').val(10);

});

$('#centerTable').on('click', '.leads-count', function () {
    if ($(this).html() == '0') {
        return false;
    }
    lengthTd = $(this).html();
    var center_ID = $(this).closest('tr').children('td').eq(0).children('p').attr('center-id');
    var ProgramId = $('.select_programType').val();
    requestedPage = 1;
    pageSize = 10;
    getLeadsChildren(center_ID, ProgramId, requestedPage, pageSize);
    LastIndex = 0;
    $('#ExternalLeadsModal').find('#First').attr('disabled', true);
    $('#ExternalLeadsModal').find('#Back').attr('disabled', true);
    $('#ExternalLeadsModal').find('#ddlpagetodisplay').val(10);

});

function getTotalRecord(data, modal) {
    var pageSize = 0;
    var reqPage = 0;
    $(modal).find('#First').attr('disabled', false);
    $(modal).find('#Back').attr('disabled', false);
    $(modal).find('#Next').attr('disabled', false);
    $(modal).find('#Last').attr('disabled', false);
    pageSize = parseInt($(modal).find('#ddlpagetodisplay').val());

    if (data > 0) {
        totalRecords = parseInt(data);
        if (totalRecords <= pageSize) {
            $(modal).find('#First').attr('disabled', true);
            $(modal).find('#Back').attr('disabled', true);
            $(modal).find('#Next').attr('disabled', true);
            $(modal).find('#Last').attr('disabled', true);
        }
        numOfPages = parseInt(totalRecords / pageSize) + ((totalRecords % pageSize == 0) ? 0 : 1);
        $(modal).find("#ddlpaging").empty()
        for (i = 1; i <= numOfPages; i++) {
            var newOption = "<option value='" + i + "'>" + i + "</option>";
            $(modal).find("#ddlpaging").append(newOption);
        }
        $(modal).find("#ddlpaging").val(requestedPage);
    }
    else {
        $(modal).find('#First').attr('disabled', true);
        $(modal).find('#Back').attr('disabled', true);
        $(modal).find('#Next').attr('disabled', true);
        $(modal).find('#Last').attr('disabled', true);
    }
}


function Displayrosters(centerID, prog_Id, reqPage, pgSize) {
    var skip = 0;
    reqPage = (reqPage == '' || reqPage == null) ? 1 : reqPage;
    pgSize = (pgSize == '' || pgSize == null) ? 10 : pgSize;
    skip = (pgSize * (reqPage - 1));

    $.ajax({

        url: '/ERSEA/GETChildrenByCenter',
        datatype: 'json',
        type: 'post',
        async: false,
        data: { CenterID: centerID, programId: prog_Id, reqPage: reqPage, pgSize: pgSize, skipRow: skip },
        //  data: { CenterID: centerID, programId: prog_Id },
        success: function (data) {

            getTotalRecord(data.TotalRecord, '#FosterDisplaymodal');


            bindGridByCenter(data.ChildrenList);

            $('#FosterDisplaymodal').find('#ddlpagetodisplay').attr('center', centerID);
            $('#FosterDisplaymodal').find('#ddlpaging').attr('center', centerID);
            $('#FosterDisplaymodal').find('#Back').attr('center', centerID);
            $('#FosterDisplaymodal').find('#First').attr('center', centerID);
            $('#FosterDisplaymodal').find('#Next').attr('center', centerID);
            $('#FosterDisplaymodal').find('#Last').attr('center', centerID);

            $('#FosterDisplaymodal').find('.totalCountSpan').text(data.TotalRecord);

            var ClassData = '';
            if (data.ClassRoomInfoList.length > 1) {
                ClassData += '<option value="0" center-id=' + centerID + ' style="color:#fff;">-- Classroom --</option>';
            }
            $.each(data.ClassRoomInfoList, function (c, value1) {

                ClassData += '<option value=' + value1.EnC_ClassRoomId + ' center-id=' + centerID + ' style="color:#fff;">' + value1.ClassRoomName + '</option>';

            });
            $('#Centers1').html(ClassData);

            $('#FosterDisplaymodal').modal('show');

        }
    });
}

function getEnrolledChildrens(center, prog_id, reqPage, pgSize) {
    // var prog_id = $('.select_programType').val();


    var skip = 0;
    reqPage = (reqPage == '' || reqPage == null) ? 1 : reqPage;
    pgSize = (pgSize == '' || pgSize == null) ? 10 : pgSize;
    skip = (pgSize * (reqPage - 1));

    $.ajax({
        url: '/ERSEA/GetEnrolledChildren',
        datatype: 'json',
        type: 'post',
        async: false,
        // data: { centerId: center, programID: prog_id },
        data: { centerId: center, programID: prog_id, reqPage: reqPage, pgSize: pgSize, skipRow: skip },
        success: function (data) {

            var bindEle = '';
            var image = '';
            var foster = '';

            getTotalRecord(data.TotalRecord, '#EnrolledModal');
            $('#EnrolledModal').find('.totalCountSpan').text(data.TotalRecord);
            $('#EnrolledModal').find('#ddlpagetodisplay').attr('center', center);
            $('#EnrolledModal').find('#ddlpaging').attr('center', center);
            $('#EnrolledModal').find('#Back').attr('center', center);
            $('#EnrolledModal').find('#First').attr('center', center);
            $('#EnrolledModal').find('#Next').attr('center', center);
            $('#EnrolledModal').find('#Last').attr('center', center);

            if (data.ChildrenList.length > 0) {
                if (center === 0) {
                    $('#EnrolledModal').find('#centerName').html('Client List');
                    $('#EnrolledModal').find('.center_name_head').show();
                }
                else {
                    $('#EnrolledModal').find('#centerName').html(data.ChildrenList[0].CenterName);
                    $('#EnrolledModal').find('.center_name_head').hide();

                }

                $.each(data.ChildrenList, function (k, enrChild) {
                    if (enrChild.Foster == "1") {
                        foster = "Y"
                    }
                    else if (enrChild.Foster == "2") {
                        foster = "N"
                    }
                    bindEle += '<tr><td data-title="Name" style="cursor:pointer;text-decoration:underline;text-decoration-color:#337ab7;" class="childName" onmouseenter="getChildrenImage(this); " clientId=' + enrChild.Enc_ClientId + '><div>' + enrChild.ClientName + '<span class="tooltiptext"></span></div></td>';
                    bindEle += (enrChild.Gender == "1") ? '<td data-title="Gender">Male</td>' : (enrChild.Gender == "2") ? '<td data-title="Gender">Female</td>' : '<td data-title="Gender">Others</td>';
                    bindEle += '<td data-title="Date Of Birth">' + enrChild.Dob + '</td>';

                    if (center === 0) {
                        bindEle += '<td data-title="Center Name">' + enrChild.CenterName + '</td>';
                    }

                    bindEle += '<td data-title="Class Start Date">' + enrChild.ClassStartDate + '</td>\
                        <td data-title="Attendance">' + enrChild.AttendancePercentage + '% </td><td data-title="Over Income">' + enrChild.OverIncome + '</td><td data-title="Foster">' + foster + '</td>';
                    bindEle += (enrChild.ChildAttendance == '1') ?
                '<td data-title="Present/Absent"><i style="color:#3c763d;" title="Present" class="fa fa-check"></i></td>' :
                (enrChild.ChildAttendance == '2') ? '<td data-title="Present/Absent"><i style="color:#ff2222;" title="Absent" class="fa fa-times"></i></td>' :
                (enrChild.ChildAttendance == '3') ? '<td data-title="Present/Absent"><i style="color:#ff2222;" title="Left Earlier" class="fa fa-times"></i></td>'
                : '<td data-title="Present/Absent"><i style="color:#ff2222;" title="Absent" class="fa fa-times"></i></td>';
                    bindEle += '</tr>';
                });
            }
            else {
                bindEle = 'No details found';
            }
            $('#EnrolledModal').find('.enrolledModalBody').html(bindEle);
            $('#EnrolledModal').modal('show');
        },
        error: function (data) {
            alert('error');
        }

    });

}



function getWithdrawnChildren(centerId, prog_id, reqPage, pgSize) {


    var skip = 0;
    reqPage = (reqPage == '' || reqPage == null) ? 1 : reqPage;
    pgSize = (pgSize == '' || pgSize == null) ? 10 : pgSize;
    skip = (pgSize * (reqPage - 1));

    // var prog_id = $('.select_programType').val();
    $.ajax({
        url: '/ERSEA/GetWithdrawnChildren',
        datatype: 'json',
        type: 'post',
        async: false,
        data: { centerId: centerId, programId: prog_id, reqPage: reqPage, pgSize: pgSize, skipRow: skip },
        success: function (data) {
            var bindEle = '';
            var image = '';


            getTotalRecord(data.TotalRecord, '#WithdrawnModal');




            $('#WithdrawnModal').find('#ddlpagetodisplay').attr('center', centerId);
            $('#WithdrawnModal').find('#ddlpaging').attr('center', centerId);
            $('#WithdrawnModal').find('#Back').attr('center', centerId);
            $('#WithdrawnModal').find('#First').attr('center', centerId);
            $('#WithdrawnModal').find('#Next').attr('center', centerId);
            $('#WithdrawnModal').find('#Last').attr('center', centerId);

            $('#WithdrawnModal').find('.totalCountSpan').text(data.TotalRecord);

            if (data != null && data.WithdrawnChildrenList != null && data.WithdrawnChildrenList.length > 0) {
                lengthTd = data.WithdrawnChildrenList.length;
                if (centerId === 0) {
                    $('#WithdrawnModal').find('#centerName').html('Client List');
                    $('#WithdrawnModal').find('.center_name_head').show();
                }
                else {
                    $('#WithdrawnModal').find('#centerName').html(data.WithdrawnChildrenList[0].CenterName);
                    $('#WithdrawnModal').find('.center_name_head').hide();
                }

                $.each(data.WithdrawnChildrenList, function (k, enrChild) {
                    //bindEle += '<tr><td data-title="Name">' + enrChild.ChildrenName + '</td>';
                    bindEle += '<tr><td data-title="Name" style="cursor:pointer;text-decoration:underline;text-decoration-color:#337ab7;" class="childName" onmouseenter="getChildrenImage(this); " clientId=' + enrChild.Enc_ClientId + '><div>' + enrChild.ChildrenName + '<span class="tooltiptext"></span></div></td>';

                    bindEle += (enrChild.Gender == "1") ? '<td data-title="Gender">Male</td>' : (enrChild.Gender == "2") ? '<td data-title="Gender">Female</td>' : '<td data-title="Gender">Others</td>';
                    bindEle += '<td data-title="Date Of Birth">' + enrChild.Dob + '</td>';
                    if (centerId === 0) {
                        bindEle += '<td data-title="Center Name">' + enrChild.CenterName + '</td>';
                    }

                    bindEle += '<td data-title="Date on List">' + enrChild.DateOnList + '</td><td data-title="Program Type">' + enrChild.ProgramType + '</td>' +
                                '</td><td data-title="Selection Points">' + enrChild.SelectionPoints + '</td></tr>'
                });
            }
            else {
                bindEle = 'No details found';
            }
            $('#WithdrawnModal').find('.withdrwnChildren').html(bindEle);
            $('#WithdrawnModal').modal('show');

        },
        error: function (data) {
            alert('error');
        }

    });
}


function getDroppedChildren(centerId, prog_id, reqPage, pgSize) {

    var skip = 0;
    reqPage = (reqPage == '' || reqPage == null) ? 1 : reqPage;
    pgSize = (pgSize == '' || pgSize == null) ? 10 : pgSize;
    skip = (pgSize * (reqPage - 1));


    $.ajax({
        url: '/ERSEA/GetDroppedChildren',
        datatype: 'json',
        type: 'post',
        async: false,
        data: { centerId: centerId, programId: prog_id, reqPage: reqPage, pgSize: pgSize, skipRow: skip },
        success: function (data) {
            var bindEle = '';
            var image = '';


            getTotalRecord(data.TotalRecord, '#DroppedModal');




            $('#DroppedModal').find('#ddlpagetodisplay').attr('center', centerId);
            $('#DroppedModal').find('#ddlpaging').attr('center', centerId);
            $('#DroppedModal').find('#Back').attr('center', centerId);
            $('#DroppedModal').find('#First').attr('center', centerId);
            $('#DroppedModal').find('#Next').attr('center', centerId);
            $('#DroppedModal').find('#Last').attr('center', centerId);

            $('#DroppedModal').find('.totalCountSpan').text(data.TotalRecord);

            if (data.DroppedChildrenList.length > 0) {
                lengthTd = data.DroppedChildrenList.length;
                if (centerId === 0) {
                    $('#DroppedModal').find('#centerName').html('Client List');
                    $('#DroppedModal').find('.center_name_head').show();

                }
                else {
                    $('#DroppedModal').find('#centerName').html(data.DroppedChildrenList[0].CenterName);
                    $('#DroppedModal').find('.center_name_head').hide();

                }

                $.each(data.DroppedChildrenList, function (k, enrChild) {
                    //bindEle += '<tr><td data-title="Name">' + enrChild.ChildrenName + '</td>';
                    bindEle += '<tr><td data-title="Name" style="cursor:pointer;text-decoration:underline;text-decoration-color:#337ab7;" class="childName" onmouseenter="getChildrenImage(this); " clientId=' + enrChild.Enc_ClientId + '><div>' + enrChild.ChildrenName + '<span class="tooltiptext"></span></div></td>';

                    bindEle += (enrChild.Gender == "1") ? '<td data-title="Gender">Male</td>' : (enrChild.Gender == "2") ? '<td data-title="Gender">Female</td>' : '<td data-title="Gender">Others</td>';
                    bindEle += '<td data-title="Date Of Birth">' + enrChild.Dob + '</td>';
                    if (centerId === 0) {
                        bindEle += '<td data-title="Center Name">' + enrChild.CenterName + '</td>';
                    }

                    bindEle += '<td data-title="Date on List">' + enrChild.DateOnList + '</td><td data-title="Program Type">' + enrChild.ProgramType + '</td>' +
                                '</td><td data-title="Selection Points">' + enrChild.SelectionPoints + '</td></tr>'
                });
            }
            else {
                bindEle = 'No details found';
            }
            $('#DroppedModal').find('.droppedChildren').html(bindEle);
            $('#DroppedModal').modal('show');

        },
        error: function (data) {
            alert('error');
        }

    });
}


function getWaitingChildren(centerId, prog_id, reqPage, pgSize) {
    // var prog_id = $('.select_programType').val();


    var skip = 0;
    reqPage = (reqPage == '' || reqPage == null) ? 1 : reqPage;
    pgSize = (pgSize == '' || pgSize == null) ? 10 : pgSize;
    skip = (pgSize * (reqPage - 1));

    $.ajax({
        url: '/ERSEA/GetWaitingChildren',
        datatype: 'json',
        type: 'post',
        async: false,
        //  data: { centerId: centerId, programID: prog_id },
        data: { centerId: centerId, programID: prog_id, reqPage: reqPage, pgSize: pgSize, skipRow: skip },

        success: function (data) {
            var bindEle = '';
            var image = '';
            if (data.WaitingChildrenList.length > 0) {

                lengthTd = data.WaitingChildrenList.length;

                getTotalRecord(data.TotalRecord, '#WaitingModal');

                $('#WaitingModal').find('#ddlpagetodisplay').attr('center', centerId);
                $('#WaitingModal').find('#ddlpaging').attr('center', centerId);
                $('#WaitingModal').find('#Back').attr('center', centerId);
                $('#WaitingModal').find('#First').attr('center', centerId);
                $('#WaitingModal').find('#Next').attr('center', centerId);
                $('#WaitingModal').find('#Last').attr('center', centerId);

                $('#WaitingModal').find('.totalCountSpan').text(data.TotalRecord);

                if (centerId === 0) {
                    $('#WaitingModal').find('#centerName').html('Client List');

                }
                else {
                    $('#WaitingModal').find('#centerName').html(data.WaitingChildrenList[0].CenterName);
                }

                $.each(data.WaitingChildrenList, function (k, enrChild) {
                    //bindEle += '<tr><td data-title="Name">' + enrChild.ChildrenName + '</td>';
                    bindEle += '<tr><td data-title="Name" style="cursor:pointer;text-decoration:underline;text-decoration-color:#337ab7;" class="childName" onmouseenter="getChildrenImage(this); " clientId=' + enrChild.Enc_ClientId + '><div>' + enrChild.ChildrenName + '<span class="tooltiptext"></span></div></td>';

                    bindEle += (enrChild.Gender == "1") ? '<td data-title="Gender">Male</td>' : (enrChild.Gender == "2") ? '<td data-title="Gender">Female</td>' : '<td data-title="Gender">Others</td>';
                    bindEle += '<td data-title="Date Of Birth">' + enrChild.Dob + '</td>';
                    bindEle += '<td data-title="Date on List">' + enrChild.DateOnList + '</td><td data-title="Center Choice">' + enrChild.CenterChoice + '<td data-title="Program Type">' + enrChild.ProgramType + '</td>' +
                                '</td><td data-title="Selection Points">' + enrChild.SelectionPoints + '</td></tr>'
                });
            }
            else {
                bindEle = 'No details found';
            }
            $('#WaitingModal').find('.waitingChildren').html(bindEle);
            $('#WaitingModal').modal('show');

        },
        error: function (data) {
            alert('error');
        }

    });
}


function getReturningChildrens(centerId, prog_id, reqPage, pgSize) {
    // var prog_id = $('.select_programType').val();
    var skip = 0;
    reqPage = (reqPage == '' || reqPage == null) ? 1 : reqPage;
    pgSize = (pgSize == '' || pgSize == null) ? 10 : pgSize;
    skip = (pgSize * (reqPage - 1));

    $.ajax({
        url: '/ERSEA/GetReturningChildren',
        datatype: 'json',
        type: 'post',
        async: false,
        data: { centerId: centerId, programID: prog_id, reqPage: reqPage, pgSize: pgSize, skipRow: skip },
        success: function (data) {
            var bindEle = '';
            var image = '';
            var foster = '';







            $('#ReturningModal').find('#ddlpagetodisplay').attr('center', centerId);
            $('#ReturningModal').find('#ddlpaging').attr('center', centerId);
            $('#ReturningModal').find('#Back').attr('center', centerId);
            $('#ReturningModal').find('#First').attr('center', centerId);
            $('#ReturningModal').find('#Next').attr('center', centerId);
            $('#ReturningModal').find('#Last').attr('center', centerId);



            if (data != null && data.ReturningList != null && data.ReturningList.length > 0) {
                getTotalRecord(data.TotalRecord, '#ReturningModal');
                $('#ReturningModal').find('.totalCountSpan').text(data.TotalRecord);
                lengthTd = data.ReturningList.length;

                if (centerId === 0) {
                    $('#ReturningModal').find('#centerName').html('Client List');
                    $('#ReturningModal').find('.center_name_head').show();
                }
                else {
                    $('#ReturningModal').find('#centerName').html(data.ReturningList[0].CenterName);
                    $('#ReturningModal').find('.center_name_head').hide();
                }

                $.each(data.ReturningList, function (k, enrChild) {
                    if (enrChild.Foster == "1") {
                        foster = "Y"
                    }
                    else if (enrChild.Foster == "2") {
                        foster = "N"
                    }
                    //bindEle += '<tr><td data-title="Name">' + enrChild.ClientName + '</td>';
                    bindEle += '<tr><td data-title="Name" style="cursor:pointer;text-decoration:underline;text-decoration-color:#337ab7;" class="childName" onmouseenter="getChildrenImage(this); " clientId=' + enrChild.Enc_ClientId + '><div>' + enrChild.ClientName + '<span class="tooltiptext"></span></div></td>';

                    bindEle += (enrChild.Gender == "1") ? '<td data-title="Gender">Male</td>' : (enrChild.Gender == "2") ? '<td data-title="Gender">Female</td>' : '<td data-title="Gender">Others</td>';
                    bindEle += '<td data-title="Date Of Birth">' + enrChild.Dob + '</td>';
                    if (centerId === 0) {
                        bindEle += '<td data-title="Center Name">' + enrChild.CenterName + '</td>';
                    }


                    bindEle += '<td data-title="Class Start Date">' + enrChild.ClassStartDate + '</td>';

                    bindEle += '<td data-title="Program Type">' + enrChild.ProgramType + '</td>';
                    bindEle += '</tr>';
                });
            }
            else {
                bindEle = 'No details found';
            }
            $('#ReturningModal').find('.returningChildren').html(bindEle);
            $('#ReturningModal').modal('show');
        }

    });
}

function getGraduatingChildrens(centerId, prog_id, reqPage, pgSize) {
    // var prog_id = $('.select_programType').val();
    var skip = 0;
    reqPage = (reqPage == '' || reqPage == null) ? 1 : reqPage;
    pgSize = (pgSize == '' || pgSize == null) ? 10 : pgSize;
    skip = (pgSize * (reqPage - 1));

    $.ajax({
        url: '/ERSEA/GetGraduatingChildren',
        datatype: 'json',
        type: 'post',
        async: false,
        data: { centerId: centerId, programID: prog_id, reqPage: reqPage, pgSize: pgSize, skipRow: skip },

        success: function (data) {
            var bindEle = '';
            var image = '';
            var foster = '';

            $('#GraduatingModal').find('#ddlpagetodisplay').attr('center', centerId);
            $('#GraduatingModal').find('#ddlpaging').attr('center', centerId);
            $('#GraduatingModal').find('#Back').attr('center', centerId);
            $('#GraduatingModal').find('#First').attr('center', centerId);
            $('#GraduatingModal').find('#Next').attr('center', centerId);
            $('#GraduatingModal').find('#Last').attr('center', centerId);

            if (data != null && data.GraduatingList != null && data.GraduatingList.length > 0) {
                getTotalRecord(data.TotalRecord, '#GraduatingModal');
                $('#GraduatingModal').find('.totalCountSpan').text(data.TotalRecord);
                lengthTd = data.GraduatingList.length;

                if (centerId === 0) {
                    $('#GraduatingModal').find('#centerName').html('Client List');
                    $('#GraduatingModal').find('.center_name_head').show();

                }
                else {
                    $('#GraduatingModal').find('#centerName').html(data.GraduatingList[0].CenterName);
                    $('#GraduatingModal').find('.center_name_head').show();

                }


                $.each(data.GraduatingList, function (k, enrChild) {
                    if (enrChild.Foster == "1") {
                        foster = "Y"
                    }
                    else if (enrChild.Foster == "2") {
                        foster = "N"
                    }
                    bindEle += '<tr><td data-title="Name" style="cursor:pointer;text-decoration:underline;text-decoration-color:#337ab7;" class="childName" onmouseenter="getChildrenImage(this); " clientId=' + enrChild.Enc_ClientId + '><div>' + enrChild.ClientName + '<span class="tooltiptext"></span></div></td>';

                    // bindEle += '<tr><td data-title="Name">' + enrChild.ClientName + '</td>';


                    bindEle += (enrChild.Gender == "1") ? '<td data-title="Gender">Male</td>' : (enrChild.Gender == "2") ? '<td data-title="Gender">Female</td>' : '<td data-title="Gender">Others</td>';
                    bindEle += '<td data-title="Date Of Birth">' + enrChild.Dob + '</td>';
                    if (centerId === 0) {
                        bindEle += '<td data-title="Center Name">' + enrChild.CenterName + '</td>';
                    }


                    bindEle += '<td data-title="Class Start Date">' + enrChild.ClassStartDate + '</td>';

                    bindEle += '<td data-title="Program Type">' + enrChild.ProgramType + '</td>';
                    bindEle += '</tr>';
                });
            }
            else {
                bindEle = 'No details found';
            }
            $('#GraduatingModal').find('.gradChildren').html(bindEle);
            $('#GraduatingModal').modal('show');
        }

    });
}

function getOverIncomChildren(centerId, prog_id, reqPage, pgSize) {
    
    requestedPage = reqPage;
    pageSize = pgSize;
    // var prog_id = $('.select_programType').val();
    prog_id = (prog_id == '0') ? $('.select_programType').val() : prog_id;
    var skip = 0;
    reqPage = (reqPage == '' || reqPage == null) ? 1 : reqPage;
    pgSize = (pgSize == '' || pgSize == null) ? 10 : pgSize;
    skip = (pgSize * (reqPage - 1));
    $.ajax({
        url: '/ERSEA/GetOverIncomeChildren',
        datatype: 'json',
        type: 'post',
        async: false,
        data: { centerId: centerId, programID: prog_id, reqPage: reqPage, pgSize: pgSize, skipRow: skip },
        success: function (data) {
            var bindEle = '';
            var image = '';
            var foster = '';

            $('#OverIncomeModal').find('#ddlpagetodisplay').attr('center', centerId);
            $('#OverIncomeModal').find('#ddlpaging').attr('center', centerId);
            $('#OverIncomeModal').find('#Back').attr('center', centerId);
            $('#OverIncomeModal').find('#First').attr('center', centerId);
            $('#OverIncomeModal').find('#Next').attr('center', centerId);
            $('#OverIncomeModal').find('#Last').attr('center', centerId);


            if (data != null && data.childInfo.OverIncomeChildrenList != null && data.childInfo.OverIncomeChildrenList.length > 0) {

                getTotalRecord(data.TotalRecord, '#OverIncomeModal');
                $('#OverIncomeModal').find('.totalCountSpan').text(data.TotalRecord);
                lengthTd = data.childInfo.OverIncomeChildrenList.length;
                if (centerId === 0) {
                    $('#OverIncomeModal').find('#centerName').html("Client List");
                    $('#OverIncomeModal').find('.center_name_head').show();

                }
                else {
                    $('#OverIncomeModal').find('#centerName').html(data.childInfo.OverIncomeChildrenList[0].CenterName);
                    $('#OverIncomeModal').find('.center_name_head').hide();

                }


                $.each(data.childInfo.OverIncomeChildrenList, function (k, enrChild) {
                    if (enrChild.Foster == "1") {
                        foster = "Y"
                    }
                    else if (enrChild.Foster == "2") {
                        foster = "N"
                    }
                    // bindEle += '<tr><td data-title="Name">' + enrChild.ClientName + '</td>';

                    bindEle += '<tr><td data-title="Name" style="cursor:pointer;text-decoration:underline;text-decoration-color:#337ab7;" class="childName" onmouseenter="getChildrenImage(this); " clientId=' + enrChild.Enc_ClientId + '><div>' + enrChild.ClientName + '<span class="tooltiptext"></span></div></td>';

                    bindEle += (enrChild.Gender == "1") ? '<td data-title="Gender">Male</td>' : (enrChild.Gender == "2") ? '<td data-title="Gender">Female</td>' : '<td data-title="Gender">Others</td>';
                    bindEle += '<td data-title="Date Of Birth">' + enrChild.Dob + '</td>';

                    if (centerId === 0) {
                        bindEle += '<td data-title="Center Name">' + enrChild.CenterName + '</td>';

                    }
                    bindEle += '<td data-title="Class Start Date">' + enrChild.ClassStartDate + '</td>';
                    if (data.ParentList.length > 0) {
                        var parentarray = [];
                        $.each(data.ParentList, function (k, parent) {
                            if (enrChild.ClientId == parent.Value)
                                parentarray.push(parent.Text);
                        });
                        bindEle += '<td data-title="Parent Name">' + parentarray.join() + '</td>';
                    }

                    bindEle += '<td data-title="Income Percentage">' + enrChild.ChildIncome + '</td>';

                    bindEle += '</tr>';
                });
            }
            else {
                bindEle = 'No details found';
            }
            $('#OverIncomeModal').find('.overIncomeChildren').html(bindEle);
            $('#OverIncomeModal').modal('show');
        }

    });
}

function getFosterChild(centerId, prog_id, reqPage, pgSize) {
    requestedPage = reqPage;
    pageSize = pgSize;
    prog_id = (prog_id == '0') ? $('.select_programType').val() : prog_id;
    var skip = 0;
    reqPage = (reqPage == '' || reqPage == null) ? 1 : reqPage;
    pgSize = (pgSize == '' || pgSize == null) ? 10 : pgSize;
    skip = (pgSize * (reqPage - 1));

    $.ajax({

        url: '/ERSEA/GetFosterChild',
        datatype: 'json',
        type: 'post',
        async: false,
        data: { centerId: centerId, programID: prog_id, reqPage: reqPage, pgSize: pgSize, skipRow: skip },
        success: function (data) {
            var bindDiv = '';
            var image = '';

            $('#FosterModal').find('#ddlpagetodisplay').attr('center', centerId);
            $('#FosterModal').find('#ddlpaging').attr('center', centerId);
            $('#FosterModal').find('#Back').attr('center', centerId);
            $('#FosterModal').find('#First').attr('center', centerId);
            $('#FosterModal').find('#Next').attr('center', centerId);
            $('#FosterModal').find('#Last').attr('center', centerId);


            if (data != null && data.FosterChildrenList != null && data.FosterChildrenList.length > 0) {

                getTotalRecord(data.TotalRecord, '#FosterModal');
                $('#FosterModal').find('.totalCountSpan').text(data.TotalRecord);
                lengthTd = data.childInfo.FosterChildrenList.length;

                if (centerId === 0) {
                    $('#FosterModal').find('#centerName').html('Client List');
                    $('#FosterModal').find('.center_name_head').show();

                }
                else {
                    $('#FosterModal').find('#centerName').html(data[0].CenterName);
                    $('#FosterModal').find('.center_name_head').hide();

                }

                $.each(data.FosterChildrenList, function (k, fosterchild) {

                    bindDiv += '<tr>';
                    // bindDiv += '<td style="line-height: 25px;" data-title="Attachment">' + fosterchild.ClientName + ' </td>';
                    bindDiv += '<tr><td data-title="Name" style="cursor:pointer;text-decoration:underline;text-decoration-color:#337ab7;" class="childName" onmouseenter="getChildrenImage(this); " clientId=' + fosterchild.Enc_ClientId + '><div>' + fosterchild.ClientName + '<span class="tooltiptext"></span></div></td>';

                    bindDiv += (fosterchild.Gender == "1") ? '<td data-title="Gender">Male</td>' : (fosterchild.Gender == "2") ? '<td data-title="Gender">Female</td>' : '<td data-title="Gender">Others</td>';
                    bindDiv += '<td data-title="Date Of Birth">' + fosterchild.Dob + '</td>';

                    if (centerId === 0) {
                        bindDiv += '<td data-title="Center Name">' + fosterchild.CenterName + '</td>';
                    }


                    if (fosterchild.FileExtension != '.pdf') {
                        bindDiv += '<td data-title="Attachment" style="text-align:center;"><a style="cursor:pointer;" class="attachment"  title="Download"  data-type="img"><img  style="display:none;" src="data:image/jpg;base64,' + fosterchild.FileAttached + '"><i class="fa fa-download download-ic"></i></a></td>';
                    }
                    else {
                        bindDiv += '<td data-title="Attachment" style="text-align:center;"><a style="cursor:pointer;" class="attachment"  title="Download"  data-type="pdf"><img  style="display:none;" src="data:application/octet-stream;base64,' + fosterchild.FileAttached + '"><i class="fa fa-download download-ic"></i></a></td>';
                    }
                });
                $('#FosterModal').find('.fosterChildren').html(bindDiv);
                $('#FosterModal').modal('show');
            }

        }

    });
}

function getHomelessChildren(centerId, prog_id, reqPage, pgSize) {
    requestedPage = reqPage;
    pageSize = pgSize;
    prog_id = (prog_id == '0') ? $('.select_programType').val() : prog_id;
    var skip = 0;
    reqPage = (reqPage == '' || reqPage == null) ? 1 : reqPage;
    pgSize = (pgSize == '' || pgSize == null) ? 10 : pgSize;
    skip = (pgSize * (reqPage - 1));
   
    $.ajax({
        url: '/ERSEA/GetHomelessChildren',
        datatype: 'json',
        type: 'post',
        async: false,
        data: { centerId: centerId, programID: prog_id, reqPage: reqPage, pgSize: pgSize, skipRow: skip },
        success: function (data) {
            var bindEle = '';
            var image = '';

            $('#HomeLessModal').find('#ddlpagetodisplay').attr('center', centerId);
            $('#HomeLessModal').find('#ddlpaging').attr('center', centerId);
            $('#HomeLessModal').find('#Back').attr('center', centerId);
            $('#HomeLessModal').find('#First').attr('center', centerId);
            $('#HomeLessModal').find('#Next').attr('center', centerId);
            $('#HomeLessModal').find('#Last').attr('center', centerId);

            if (data != null && data.HomeLessChildrenList != null && data.HomeLessChildrenList.length > 0) {

                getTotalRecord(data.TotalRecord, '#HomeLessModal');
                $('#HomeLessModal').find('.totalCountSpan').text(data.TotalRecord);
                lengthTd = data.HomeLessChildrenList.length;

                if (centerId === 0) {
                    $('#HomeLessModal').find('#centerName').html('Client List');
                    $('#HomeLessModal').find('.center_name_head').show();

                }
                else {
                    $('#HomeLessModal').find('#centerName').html(data.HomeLessChildrenList[0].CenterName);
                    $('#HomeLessModal').find('.center_name_head').hide();

                }

                $.each(data.HomeLessChildrenList, function (k, hlChild) {

                    //  bindEle += '<tr><td data-title="Name">' + hlChild.ChildrenName + '</td>';

                    bindEle += '<tr><td data-title="Name" style="cursor:pointer;text-decoration:underline;text-decoration-color:#337ab7;" class="childName" onmouseenter="getChildrenImage(this); " clientId=' + hlChild.Enc_ClientId + '><div>' + hlChild.ChildrenName + '<span class="tooltiptext"></span></div></td>';

                    bindEle += (hlChild.Gender == "1") ? '<td data-title="Gender">Male</td>' : (hlChild.Gender == "2") ? '<td data-title="Gender">Female</td>' : '<td data-title="Gender">Others</td>';
                    bindEle += '<td data-title="Date Of Birth">' + hlChild.Dob + '</td>';
                    if (centerId === 0) {
                        bindEle += '<td data-title="Center Name">' + hlChild.CenterName + '</td>';
                    }

                    bindEle += '<td data-title="Class Start Date">' + hlChild.ClassStartDate + '</td></tr>';
                });
            }
            else {
                bindEle = 'No details found';
            }
            $('#HomeLessModal').find('.homeLessChildren').html(bindEle);
            $('#HomeLessModal').modal('show');
        }

    });

}

function getLeadsChildren(centerId, prog_id, reqPage, pgSize) {
    requestedPage = reqPage;
    pageSize = pgSize;
    prog_id = (prog_id == '0') ? $('.select_programType').val() : prog_id;
    var skip = 0;
    reqPage = (reqPage == '' || reqPage == null) ? 1 : reqPage;
    pgSize = (pgSize == '' || pgSize == null) ? 10 : pgSize;
    skip = (pgSize * (reqPage - 1));

    $.ajax({
        url: '/ERSEA/GetLeadschildren',
        datatype: 'json',
        type: 'post',
        async: false,
        data: { centerId: centerId, programID: prog_id, reqPage: reqPage, pgSize: pgSize, skipRow: skip },
        success: function (data) {

            $('#ExternalLeadsModal').find('#ddlpagetodisplay').attr('center', centerId);
            $('#ExternalLeadsModal').find('#ddlpaging').attr('center', centerId);
            $('#ExternalLeadsModal').find('#Back').attr('center', centerId);
            $('#ExternalLeadsModal').find('#First').attr('center', centerId);
            $('#ExternalLeadsModal').find('#Next').attr('center', centerId);
            $('#ExternalLeadsModal').find('#Last').attr('center', centerId);

            if (data != null && data.LeadsChildrenList.length > 0) {

                getTotalRecord(data.TotalRecord, '#ExternalLeadsModal');
                $('#ExternalLeadsModal').find('.totalCountSpan').text(data.TotalRecord);
                lengthTd = data.LeadsChildrenList.length;

                if (centerId === 0) {
                    $('#ExternalLeadsModal').find('#centerName').html('Client List');
                    $('#ExternalLeadsModal').find('.center_name_head').show();


                }
                else {
                    $('#ExternalLeadsModal').find('#centerName').html(data.LeadsChildrenList[0].CenterName);
                    $('#ExternalLeadsModal').find('.center_name_head').hide();

                }
                var bindEle = '';
                $.each(data.LeadsChildrenList, function (k, leadChild) {
                    bindEle += '<tr><td data-title="Name">' + leadChild.ChildrenName + '</td>';
                    bindEle += '<td data-title="Gender">' + leadChild.Gender + '</td>';
                    bindEle += '<td data-title="Date of Birth">' + leadChild.Dob + '</td>';
                    bindEle += '<td data-title="Parent/Guardian">' + leadChild.ParentName + '</td>';
                    if (centerId === 0) {
                        bindEle += '<td data-title="Center Name">' + leadChild.CenterName + '</td>';

                    }

                    bindEle += '<td data-title="Contacted by FSW" style="text-align:center;">' + leadChild.ContactStatus + '</td>';
                    bindEle += '<td data-title="Disability">' + leadChild.Disability + '</td>';
                    bindEle += '</tr>';
                });
            }
            else {
                bindEle = 'No details found';
            }
            $('#ExternalLeadsModal').find('.LeadsModalBody').html(bindEle);
            $('#ExternalLeadsModal').modal('show');
        }

    });

}


function bindGridByCenter(binData) {
    lengthTd = binData.length;
    var value = '';
    if (binData.length > 0) {

        $.each(binData, function (c, value1) {
            var foster = '';
            var image = '';
            var attendance = '';
            if (value1.Foster == "1") {
                foster = "Y"
            }
            else if (value1.Foster == "2") {
                foster = "N"
            }
            if (value1.Image == "" && value1.Gender == "2") {
                image = '<img class="roundimage"  width="50" height="50"  src="/Content/img/ic_female.png" />';
            }
            else if (value1.Image == "" && value1.Gender == "1") {
                image = '<img class="roundimage" width="50" height="50"   src="/Content/img/ic_male.png" />';
            }
            else if (value1.Image == "" && value1.Gender == "3") {
                image = '<img class="roundimage"  width="50" height="50"  src="/Content/img/ic_male_default.png" />';
            }
            else if (value1.Image != "") {
                image = '<img class="roundimage" width="50" height="50"   src="data:image/jpg;base64,' + value1.Image + '"/></td>'
            }
            else {
                image = '<img class="roundimage"  width="50" height="50"  src="/Content/img/download.jpg" />';
            }

            value += '<tr><td data-title="Name">' + value1.ClientName + '</td><td data-title="Image">' + image + '</td>';
            value += (value1.Gender == "1") ? '<td data-title="Gender">Male</td>' : (value1.Gender == "2") ? '<td data-title="Gender">Female</td>' : '<td data-title="Gender">Others</td>';
            value += '<td data-title="DOB">' + value1.Dob + '</td>';
            value += '<td data-title="Start Date">' + value1.ClassStartDate + '</td>\
                    <td data-title="Attendance">' + value1.AttendancePercentage + '%' + '</td>\
                    <td data-title="Over Income">' + value1.OverIncome + '</td>\
                   <td data-title="Foster">' + foster + '</td>';

            value += (value1.ChildAttendance == '1') ?
                '<td data-title="Present/Absent"><i style="color:#3c763d;" title="Present" class="fa fa-check"></i></td>' :
                (value1.ChildAttendance == '2') ? '<td data-title="Present/Absent"><i style="color:#ff2222;" title="Absent" class="fa fa-times"></i></td>' :
                (value1.ChildAttendance == '3') ? '<td data-title="Present/Absent"><i style="color:#ff2222;" title="Left Earlier" class="fa fa-times"></i></td>'
                : '<td data-title="Present/Absent"><i style="color:#ff2222;" title="Absent" class="fa fa-times"></i></td>';
            value += '</tr>';
        });
    }

    else {
        value = 'No detail Found';
    }


    $('#FosterDisplaymodal').find('.totalChildBody').html(value);


    if (lengthTd < 5) {
        $('#FosterDisplaymodal').find(".scroll-thead").css({ "width": "100%" });
    }
    else {
        $('#FosterDisplaymodal').find(".scroll-thead").css({ "width": "calc( 100% - 17px )" });
    }
}


function btnSearch() {

    var ClassroomId = $('#Centers1 option:selected').val();
    var centerId = $('#Centers1 option:selected').attr('center-id');
    var programId = $('.select_programType').val();
    $.ajax({
        url: '/ERSEA/GetChildrenByClassRoom',
        datatype: 'json',
        type: 'post',
        data: { CenterId: centerId, classroomId: ClassroomId, programId: programId },
        success: function (data) {
            bindGridByCenter(data);
            $('#FosterDisplaymodal').find('#ddlpagetodisplay').attr('center', centerId);
        }

    });

}

function getChildrenImage(val) {
    var encClientId = $(val).attr('clientId');
    var position = $(val).position();
    var topalignment = position.top;
    $.ajax({
        url: '/ERSEA/GetChildrenImage',
        type: 'post',
        datatype: 'json',
        data: { enc_clientId: encClientId },
        success: function (data) {
            var image = '';
            if (data.Text == "" && data.Value == "2") {
                image = '<img class="roundimage"  width="100" height="100"  src="/Content/img/ic_female.png" />';
            }
            else if (data.Text == "" && data.Value == "1") {
                image = '<img class="roundimage" width="100" height="100"   src="/Content/img/ic_male.png" />';
            }
            else if (data.Text == "" && ata.Text == "3") {
                image = '<img class="roundimage"  width="100" height="100"  src="/Content/img/ic_male_default.png" />';
            }
            else if (data.Text != "") {
                image = '<img class="roundimage" width="100" height="100"   src="data:image/jpg;base64,' + data.Text + '"/></td>'
            }
            else {
                image = '<img class="roundimage"  width="100" height="100"  src="/Content/img/download.jpg" />';
            }

            $(val).find('.tooltiptext').html(image).css('top', (topalignment - 30));

        }
    });
}


function drawgrid(cls, ele) {
    requestedPage = 1;
    pageSize = $(ele).val();
    var centerid = $(ele).attr('center');

    callDisplayAjaxMethods(requestedPage, pageSize, cls, centerid);

    LastIndex = 0;
    $(cls).find('#First').attr('disabled', true);
    $(cls).find('#Back').attr('disabled', true);
}

function fnChangePage(clas, val) {
    pageLoadedFirst = false;
    pageSize = $(clas).find('#ddlpagetodisplay').val();
    var centrId = 0;
    centrId = $(clas).find('#' + val + '').attr('center');
    if (val == 'First') {
        StartIndex = 0;
        LastIndex = parseInt(pageSize) + parseInt(LastIndex * requestedPage);
        requestedPage = ((StartIndex / 10) + 1);
        GoToNextPage(requestedPage, pageSize, clas, centrId);
        $(clas).find('#First').attr('disabled', true);
        $(clas).find('#Back').attr('disabled', true);
        $(clas).find('#Next').attr('disabled', false);
        $(clas).find('#Last').attr('disabled', false);
        LastIndex = 0;
    }
    else if (val == 'Last') {
        StartIndex = parseInt((totalRecords - 1) / pageSize) * pageSize;
        LastIndex = totalRecords;
        requestedPage = numOfPages;
        GoToNextPage(requestedPage, pageSize, clas, centrId)
        $(clas).find('#First').attr('disabled', false);
        $(clas).find('#Back').attr('disabled', false);
        $(clas).find('#Next').attr('disabled', true);
        $(clas).find('#Last').attr('disabled', true);
    }
    else if (val == 'Next') {
        LastIndex = parseInt(pageSize) + parseInt(LastIndex);
        requestedPage = (parseInt(LastIndex / pageSize) + 1);
        GoToNextPage(requestedPage, pageSize, clas, centrId);
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
    else if (val == 'Back') {
        requestedPage = requestedPage - 1;
        LastIndex = parseInt(LastIndex) - parseInt(pageSize);
        GoToNextPage(requestedPage, pageSize, clas, centrId)
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
    else {
    }
}


function getListafterupdation(clsName, ele) {

    pageSize = $(clsName).find('#ddlpagetodisplay').val();
    requestedPage = $(ele).val();
    StartIndex = (pageSize * (requestedPage - 1)) + 1;
    LastIndex = parseInt(pageSize * requestedPage) - parseInt(pageSize);
    var centerid = $(ele).attr('center');
    var prgId = $('.select_programType').val();


    callDisplayAjaxMethods(requestedPage, pageSize, clsName, centerid);

    //switch (clsName) {

    //    case '.totalChildBody':

    //        Displayrosters(centerid, prgId, requestedPage, pageSize);
    //        break;
    //}

    if (requestedPage == 1) {
        $(clsName).find('#First').attr('disabled', true);
        $(clsName).find('#Back').attr('disabled', true);
        $(clsName).find('#Next').attr('disabled', false);
        $(clsName).find('#Last').attr('disabled', false);
    }
    else if (requestedPage == numOfPages) {
        $(clsName).find('#First').attr('disabled', false);
        $(clsName).find('#Back').attr('disabled', false);
        $(clsName).find('#Next').attr('disabled', true);
        $(clsName).find('#Last').attr('disabled', true);
    }
    else {
        $(clsName).find('#First').attr('disabled', false);
        $(clsName).find('#Back').attr('disabled', false);
        $(clsName).find('#Next').attr('disabled', false);
        $(clsName).find('#Last').attr('disabled', false);
    }
    $(clsName).find("#ddlpaging").val(requestedPage);
}


function GoToNextPage(requestedPage, pageSize, clsName, centId) {

    callDisplayAjaxMethods(requestedPage, pageSize, clsName, centId);

}

function callDisplayAjaxMethods(_reqPage, _pgSize, _clsName, _centerId) {
    var prgId = $('.select_programType').val();
    switch (_clsName) {
        case '#FosterDisplaymodal':
            Displayrosters(_centerId, prgId, _reqPage, _pgSize);
            break;
        case '#EnrolledModal':
            getEnrolledChildrens(_centerId, prgId, _reqPage, _pgSize);
            break;
        case '#WithdrawnModal':
            getWithdrawnChildren(_centerId, prgId, _reqPage, _pgSize);
            break;
        case '#DroppedModal':
            getDroppedChildren(_centerId, prgId, _reqPage, _pgSize);
            break;

        case '#WaitingModal':
            getWaitingChildren(_centerId, prgId, _reqPage, _pgSize);
            break;
        case '#ReturningModal':
            getReturningChildrens(_centerId, prgId, _reqPage, _pgSize);
            break;
        case '#GraduatingModal':
            getGraduatingChildrens(_centerId, prgId, _reqPage, _pgSize);
            break;

        case '#OverIncomeModal':
            getOverIncomChildren(_centerId, prgId, _reqPage, _pgSize);
            break;

        case '#FosterModal':
            getFosterChild(_centerId, prgId, _reqPage, _pgSize);
            break;
        case '#HomeLessModal':
            getHomelessChildren(_centerId, prgId, _reqPage, _pgSize);
            break;
        case '#ExternalLeadsModal':
            getLeadsChildren(_centerId, prgId, _reqPage, _pgSize);
            break;


    }
}
