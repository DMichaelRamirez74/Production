var savetype = 0;
var assessmentType = parseInt($('#assessmentType').val().trim());
var houseHoldId = $('#houseHoldId').val().trim();
var activeYear = $('#activeYear').val().trim();
var maxMatrixValue = 0;

$(document).ready(function () {

    $.ajax({
        url: "/Roster/GetClientStatus",
        datatype: "json",
        async: false,
        data: { HouseHoldID: houseHoldId },
        success: function (data) {

            var imagesrc = data.ProfilePic === "" ? ("/Images/prof-image.png") : ("data:image/jpg;base64," + data.ProfilePic);
            $('#profileImage').attr('src', imagesrc);
            $("#profileImage").css("display", "block");

            $('#parentName').html(data.ParentName);
            var selectedAppend = '';
            if (data.ActiveYearList.length > 0) {
                $.each(data.ActiveYearList, function (i, element) {

                    selectedAppend += '<option value=' + element.Text + '>20' + element.Text + '</option>'
                });
                $('#yearSelect').append(selectedAppend);
            }
        }
    });

    $('#yearSelect').val(activeYear);
    var ye = '';
    GetStaffName(ye);
    SetChartDetails(ye);



    if (assessmentType === 1) {
        $('.assment-block-2').addClass('hidden');
        $('.assment-block-3').addClass('hidden');
    }

    if (assessmentType === 2) {
        $('.assment-block-3').addClass('hidden');
    }

    $('.popup-div').css('display', 'none');
    $('.popup-div1').css('display', 'none');



    $('.category-div').each(function () {

        var selfheight = parseInt($(this).find('.survey-block').css('height'));
        var textheight = parseInt($(this).find('.survey-text').css('height'));
        var green_bar = 0;
        var value = 0;
        if (selfheight > textheight) {
            $(this).find('.change-div').css('height', selfheight + 'px');
            value = $(this).attr('cat-id');

            green_bar = parseInt(selfheight - 40);
            $('.change-bar-div_' + value).css('height', green_bar + 'px');

        }
        else {
            $(this).find('.change-div').css('height', textheight + 'px');
            value = $(this).attr('cat-id');
            green_bar = parseInt(textheight - 40);
            $('.change-bar-div_' + value).css('height', green_bar + 'px');

        }
    });
});

//Gets the Staff Details//
function GetStaffName(prog_year) {
    $.ajax({
        url: "/Roster/GetName",
        datatype: "json",
        async: false,
        data: { HouseHoldId: houseHoldId, ActiveYear: prog_year },
        success: function (staffNameList) {

            $('.staff-name').html('');
            $('.completed-date').html('');
            $('.date-para').addClass('hidden');
            if (staffNameList.length > 0) {
                for (var i = 0; i < staffNameList.length; i++) {

                    $('#staff' + staffNameList[i].AssessmentNumber).html(staffNameList[i].StaffName);
                    $('#date' + staffNameList[i].AssessmentNumber).html(staffNameList[i].Date);
                    $('.para-' + staffNameList[i].AssessmentNumber).removeClass('hidden');
                }
            }

            else {
                $('.staff-name').html('');
                $('.date-para').addClass('hidden');
            }
        }
    });
}

//Gets and Sets the Chart Details in the Chart//
function SetChartDetails(e) {
    var catcount = 0;
    var assessment_1_total = 0;
    var assessment_2_total = 0;
    var assessment_3_total = 0;
    var assessment1score = 0;
    var assessment2score = 0;
    var assessment3score = 0;
    var year = e;
    var chardetails = false;

    $.ajax({
        url: "/Roster/GetChartDetails",
        datatype: "json",
        type: 'post',
        async: false,
        data: { houseHoldId: houseHoldId, date: year },
        success: function (data) {

            $('.bar-green').css('height', '0%');
            $('.bar-green').addClass('hidden');

            $('.bar-label').css('height', '14%');
            $('.bar-label').children('p').html('');
            $('.char-percentage').html('-').css('color', 'black');
            $('.percentage-image').addClass('hidden');
            $('.avg-p').html('0<sub>Avg</sub>')
            $('.bar-label1').children('p').html('0');
            $('.elipse-grade').children('p').html('0.00');
            $('.mat-score').html('-');
            $('.mat-score').attr('data-score', 0);
            $('.mat-score').attr('testvalue', 0);

            savetype = data.groupType;

            if (data.scoreList.length > 0) {
                for (var z = 0; z < data.scoreList.length; z++) {
                    var assesmentId = data.scoreList[z].AnnualAssessmentType;
                    var AsGroupId = data.scoreList[z].AssessmentGroupId;
                    $('#fill_As' + assesmentId + '_' + AsGroupId).html(data.scoreList[z].Testvalue);
                    $('#fill_As' + assesmentId + '_' + AsGroupId).attr('data-score', data.scoreList[z].MatrixScoreId);
                    assessment1score += (assesmentId === 1) ? data.scoreList[z].Testvalue : 0;
                    assessment2score += (assesmentId === 2) ? data.scoreList[z].Testvalue : 0;
                    assessment3score += (assesmentId === 3) ? data.scoreList[z].Testvalue : 0;
                }
            }
            if (data.chardetailsList != null) {
                if (data.chardetailsList.length > 0) {
                    //  debugger;
                    chardetails = true;

                    $.each(data.chardetailsList, function (k, chart) {

                        if (chart.MaximumMatrixValue > 0) {
                            maxMatrixValue = maxMatrixValue;
                        }

                    });

                    for (var s = 0; s < data.chardetailsList.length; s++) {

                        var assessmentNumber = data.chardetailsList[s].AssessmentNumber;
                        var catId = data.chardetailsList[s].AssessementCategoryId;
                        var height = (data.chardetailsList[s].ChartHeight);
                        var percentage = data.chardetailsList[s].ResultPercentage.toFixed(1);
                        if (percentage > 0) {
                            if (height === 100) {
                                $('.greenAs' + assessmentNumber + '_' + catId).css('bottom', '-1px');
                            }
                            $('.greenAs' + assessmentNumber + '_' + catId).removeClass('hidden');
                            $('.greenAs' + assessmentNumber + '_' + catId).css('height', height + '%');

                            $('.labelAs' + assessmentNumber + '_' + catId).css('height', height + 4 + '%');
                            $('.labelAs' + assessmentNumber + '_' + catId).children('p').html(percentage);
                        }
                        if (assessmentNumber === 1) {
                            assessment_1_total = parseFloat(assessment_1_total) + parseFloat(percentage);
                        }

                        if (assessmentNumber === 2) {
                            assessment_2_total = parseFloat(assessment_2_total) + parseFloat(percentage);
                        }
                        if (assessmentNumber === 3) {
                            assessment_3_total = parseFloat(assessment_3_total) + parseFloat(percentage);
                        }


                    }
                }
            }
            if (data.chardetailsList == null) {
                $('.bar-green').css('height', '0%');
                $('.bar-green').addClass('hidden');
                $('.bar-label').css('height', '14%');
                $('.bar-label').html('');
                $('.char-percentage').html('-').css('color', 'black');
                $('.percentage-image').addClass('hidden');
                $('.avg-p').html('0<sub>Avg</sub>')
                $('.bar-label1').children('p').html('0');
                $('.elipse-grade').children('p').html('0.00');
                $('.mat-score').html('-');
                $('.mat-score').attr('data-score', 0);
                $('.mat-score').attr('testvalue', 0);
            }

            if (data.chardetailsList != null) {
                if (data.arraylist.length > 0) {
                    for (var i = 0; i < data.arraylist.length; i++) {
                        if (data.arraylist[i].length > 0) {
                            var as2Count = 0;
                            var as3Count = 0;
                            for (var j = 0; j < data.arraylist[i].length; j++) {
                                var assessment_Num = data.arraylist[i][j].AnnualAssessmentType;
                                var TestValue = data.arraylist[i][j].Testvalue;
                                var Group_Id_array = data.arraylist[i][j].AssessmentGroupId;
                                $('#fill_As' + assessment_Num + '_' + Group_Id_array).attr('TestValue', TestValue);
                                if (assessment_Num === 2) {
                                    as2Count++;
                                }
                                if (assessment_Num === 3) {
                                    as3Count++;
                                }

                            }
                            var assessment_Number = 0;
                            var Group_Id = 0;
                            var cat_Id = 0;
                            var differnceval = 0;
                            var finalper = 0;
                            var img_src_up = '/images/dw-arw.png';
                            var img_src_down = '/images/tp-arw.png';
                            var test1 = 0;
                            var test2 = 0;

                            if (assessmentType === 2) {
                                for (var l = 0; l < data.arraylist[i].length; l++) {
                                    assessment_Number = data.arraylist[i][l].AnnualAssessmentType;
                                    Group_Id = data.arraylist[i][l].AssessmentGroupId;
                                    cat_Id = data.arraylist[i][l].AssessementCategoryId;

                                    if (as2Count > 0) {

                                        test1 = parseInt($('#fill_As1_' + Group_Id).attr('TestValue'));
                                        test2 = parseInt($('#fill_As2_' + Group_Id).attr('TestValue'));
                                        differnceval = test1 - test2;
                                        finalper = Math.abs((differnceval / test1) * 100);
                                        if (finalper % 1 !== 0) {

                                            finalper = finalper.toFixed(1);
                                        }

                                        if (differnceval > 0 && test2 !== 0) {
                                            $('.chg_per_img_' + cat_Id + '_' + Group_Id).removeClass('hidden');
                                            $('.chg_per_' + cat_Id + '_' + Group_Id).html(finalper + '%').css('color', '#e74c3c');
                                            $('.chg_per_img' + cat_Id + '_' + Group_Id).attr('src', img_src_down);
                                            $('.chg_per_img' + cat_Id + '_' + Group_Id).css('display', 'block');
                                        }

                                        else if (differnceval <= 0 && test2 !== 0) {
                                            $('.chg_per_img_' + cat_Id + '_' + Group_Id).removeClass('hidden');
                                            $('.chg_per_' + cat_Id + '_' + Group_Id).html(finalper + '%').css('color', '#2ecc71');
                                            $('.chg_per_img' + cat_Id + '_' + Group_Id).attr('src', img_src_up);
                                            $('.chg_per_img' + cat_Id + '_' + Group_Id).css('display', 'block');
                                        }
                                        else {
                                            $('.chg_per_' + cat_Id + '_' + Group_Id).html('-').css('color', 'black');
                                        }

                                    }

                                    else {
                                        $('.chg_per_' + cat_Id + '_' + Group_Id).html('-').css('color', 'black');
                                    }
                                }
                            }


                            if (assessmentType === 3) {

                                for (var c = 0; c < data.arraylist[i].length; c++) {
                                    assessment_Number = data.arraylist[i][c].AnnualAssessmentType;
                                    Group_Id = data.arraylist[i][c].AssessmentGroupId;
                                    cat_Id = data.arraylist[i][c].AssessmentCategoryId;

                                    if (as3Count > 0) {

                                        test1 = parseInt($('#fill_As1_' + Group_Id).attr('TestValue'));
                                        var test3 = parseInt($('#fill_As3_' + Group_Id).attr('TestValue'));
                                        if (test1 != 0) {


                                            differnceval = test1 - test3;
                                            finalper = Math.abs((differnceval / test1) * 100);
                                            if (finalper % 1 !== 0) {
                                                finalper = finalper.toFixed(1);
                                            }
                                        }

                                        if (differnceval > 0 && test3 !== 0) {
                                            $('.chg_per_img_' + cat_Id + '_' + Group_Id).removeClass('hidden');
                                            $('.chg_per_' + cat_Id + '_' + Group_Id).html(finalper + '%').css('color', '#e74c3c');
                                            $('.chg_per_img_' + cat_Id + '_' + Group_Id).attr('src', img_src_down);
                                            $('.chg_per_img_' + cat_Id + '_' + Group_Id).css('display', 'block');
                                        }

                                        else if (differnceval <= 0 && test3 !== 0 && test1 != 0) {
                                            $('.chg_per_img_' + cat_Id + '_' + Group_Id).removeClass('hidden');
                                            $('.chg_per_' + cat_Id + '_' + Group_Id).html(finalper + '%').css('color', '#2ecc71');
                                            $('.chg_per_img_' + cat_Id + '_' + Group_Id).attr('src', img_src_up);
                                            $('.chg_per_img_' + cat_Id + '_' + Group_Id).css('display', 'block');
                                        }
                                        else {
                                            $('.chg_per_' + cat_Id + '_' + Group_Id).html('-').css('color', 'black');
                                        }
                                    }

                                    else if (as2Count > 0) {
                                        test1 = $('#fill_As1_' + Group_Id).attr('TestValue');
                                        test2 = $('#fill_As2_' + Group_Id).attr('TestValue');
                                        // var cat_Id = data.arraylist[i][c].AssessmentCategoryId;
                                        differnceval = test1 - test2;
                                        finalper = Math.abs((differnceval / test1) * 100);
                                        if (finalper % 1 !== 0) {
                                            finalper = finalper.toFixed(1);
                                        }
                                        if (differnceval > 0 && test2 !== 0) {
                                            $('.chg_per_img_' + cat_Id + '_' + Group_Id).removeClass('hidden');
                                            $('.chg_per_' + cat_Id + '_' + Group_Id).html(finalper + '%').css('color', '#e74c3c');
                                            $('.chg_per_img_' + cat_Id + '_' + Group_Id).attr('src', img_src_down);
                                            $('.chg_per_img_' + cat_Id + '_' + Group_Id).css('display', 'block');
                                        }

                                        else if (differnceval <= 0 && test2 !== 0) {
                                            $('.chg_per_img_' + cat_Id + '_' + Group_Id).removeClass('hidden');
                                            $('.chg_per_' + cat_Id + '_' + Group_Id).html(finalper + '%').css('color', '#2ecc71');
                                            $('.chg_per_img_' + cat_Id + '_' + Group_Id).attr('src', img_src_up);
                                            $('.chg_per_img_' + cat_Id + '_' + Group_Id).css('display', 'block');
                                        }
                                        else {
                                            $('.chg_per_' + cat_Id + '_' + Group_Id).html('-').css('color', 'black');
                                        }
                                    }
                                    else
                                        $('.chg_per_' + cat_Id + '_' + Group_Id).html('-').css('color', 'black');
                                }
                            }
                        }
                    }
                }
            }
        }
    });

    if (chardetails) {
        //   debugger;
        catcount = $('#categoryIdCount').val().trim();
        var as1Percentage = (assessment_1_total / catcount).toFixed(2);
        var as2Percentage = (assessment_2_total / catcount).toFixed(2);
        var as3Percentage = (assessment_3_total / catcount).toFixed(2);

        var as1_as2diff = Math.abs(as1Percentage - as2Percentage).toFixed(2);
        var as1_as3diff = Math.abs(as1Percentage - as3Percentage).toFixed(2);
        var totalGroupCount = $('#TotalgroupCount').val().trim();
        //var converteddenom = (totalGroupCount / catcount);
        //var convertedratio = (100 / converteddenom);

        

        if (as1Percentage > 0) {

            //var height1 = (as1Percentage * convertedratio);

            var height1=((as1Percentage/maxMatrixValue)*100);

            if (height1 === 100) {
                $('.bar-green1').css('bottom', '-1px');
            }
            else if (height1 === 0) {
                $('.bar-green1').addClass('hidden');
            }
            else {
                $('.bar-green1').removeClass('hidden');

            }
            $('.bar-green1').height(height1 + '%');
            $('.master-bar1').children('.bar-label2').children('.avg-p').html(as1Percentage + '<sub>Avg</sub>');
            $('.master-bar1').children('.bar-label1').children('p').html(assessment1score);
        }
        var height2 = 0;

        if (assessmentType === 2) {

            if (as2Percentage > 0) {


                //height2 = (as2Percentage * convertedratio);
                height2=((as2Percentage/maxMatrixValue)*100);

                if (height2 === 0) {
                    $('.bar-green2').addClass('hidden');
                }
                else {
                    $('.bar-green2').removeClass('hidden');

                }
                $('.bar-green2').height(height2 + '%');

                $('.master-bar2').children('.bar-label2').children('.avg-p').html(as2Percentage + '<sub>Avg</sub>');
                $('.master-bar2').children('.bar-label1').children('p').html(assessment2score);

            }
            $('.barl-2diff').children('p').html(as1_as2diff);
        }
        if (assessmentType === 3) {

            if (as2Percentage > 0) {


              //  height2 = (as2Percentage * convertedratio);

                height2 = ((as2Percentage / maxMatrixValue) * 100);

                if (height2 === 100) {
                    $('.bar-green2').css('bottom', '-1px');

                }
                else if (height2 === 0) {
                    $('.bar-green2').addClass('hidden');

                }
                else {
                    $('.bar-green2').removeClass('hidden');
                }
                $('.bar-green2').height(height2 + '%');

                $('.master-bar2').children('.bar-label2').children('.avg-p').html(as2Percentage + '<sub>Avg</sub>');
                $('.master-bar2').children('.bar-label1').children('p').html(assessment2score);


            }
            if (as3Percentage > 0) {

               // var height3 = (as3Percentage * convertedratio);

                height3 = ((as3Percentage / maxMatrixValue) * 100);

                if (height3 === 100) {
                    $('.bar-green3').css('bottom', '-1px');
                }
                else if (height3 === 0) {
                    $('.bar-green3').addClass('hidden');
                }
                else {
                    $('.bar-green3').removeClass('hidden');

                }

                $('.bar-green3').height(height3 + '%');

                $('.master-bar3').children('.bar-label2').children('.avg-p').html(as3Percentage + '<sub>Avg</sub>');
                $('.master-bar3').children('.bar-label1').children('p').html(assessment3score);

            }

            $('.barl-2diff').children('p').html(as1_as2diff);
            if (assessment_3_total !== 0) {
                $('.bar1-3diff').children('p').html(as1_as3diff);

            }
            else {
                $('.bar1-3diff').children('p').html('0.00');
            }
        }
    }

}

//On Click over the assessment group to show the description popup//
$('.assessment-group').click(function () {
    // debugger;
    var dropdownYear = $('#yearSelect').val();
    var parsedYear = parseInt(dropdownYear.substr(dropdownYear.length - 2));
    var currentYear = parseInt(activeYear.substr(activeYear.length - 2));
    var expireYear = false;
    var isFirst = parseInt($(this).attr('isfirst'));
    if (parsedYear > currentYear || parsedYear < currentYear) {
        expireYear = true;
    }
    if ((savetype == 0) || (expireYear == true)) {
        $('#assessmenterror').html('Your assessment Date Range is expired');
        $('#AssessmentexpirePopup').modal('show');
        return false;
    }
    var groupId = parseInt($(this).attr('group-id'));
    var clientId = $('#clientID').val().trim();
    var count = $('#count_' + groupId).val().trim();
    var pos = parseInt($(this).attr('position'));
    $('.popup-display-overlay').html('');
    $('.popup-display-overlay').css('display', 'none');
    $('.div-group-' + count).html('');
    $('.popup-div').css('display', 'none');
    $('.popup-div1').css('display', 'none');
    $('.div-question-' + count).html('');
    $('.div-question-' + count).css('display', 'none');
    $.ajax({
        url: "/Roster/GetDescripton",
        datatype: 'json',
        type: 'post',
        data: { groupId: groupId, clientId: clientId },
        success: function (data) {
            var bindDiv = "<div class='col-xs-12 text-center' style='padding:10px;font-size:24px; margin:auto;color:#163b69; margin-bottom:10px;'>Assessment Description</div><div class='close-div'><i class='fa fa-times' aria-hidden='true'></i></div>";
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    var getdiv = "";
                    getdiv = $('#descText');
                    $('.matrix-value').html(data[i].MatrixValue);
                    $('.matrix-value').attr('group-id', groupId);
                    $('.matrix-value').attr('assessmnet-id', data[i].AssessmentGroupId);
                    $('.description').html(data[i].Description);
                    getdiv.removeClass('matrix-value');
                    getdiv.removeClass('description');
                    bindDiv += getdiv.html();
                }
                $('#popupDiv').html(bindDiv);
                $('.div-group-' + count).html('');
                $('.div-group-' + count).html($('.desc-view-popup').html());
                $('.div-group-' + count).css('display', 'block');
                $('.popup-div').css('display', 'block');


                var heightarray = '';
                var divheight2 = '';
                if (isFirst === 1) {

                    var divHeight = -16;
                    heightarray = ['-16px', '50px', '116px', '182px', '248px', '311px', '377px', '443px'];
                    divheight2 = heightarray[pos - 1];
                    $('.popup-div').css('top', divheight2);
                }
                else {
                    heightarray = ['-135px', '-70px', '-5px', '61px', '126px', '192px', '255px', '321px'];
                    divheight2 = heightarray[pos - 1];
                    $('.popup-div').css('top', divheight2);
                }


            }
        }
    });


});

//On click over the Close icon of the Pop-up//
$('body').on('click', '.close-div', function () {
    $('.popup-display-overlay').css('display', 'none');

});

//On Click over the Matrix Value or Score in the Description Popup/Insert matrix value//
$('body').on('click', '#matrixValue', function () {
    var groupID = $(this).attr('group-id');
    var value = $(this).html().trim();
    $('#fill_As' + savetype + '_' + groupID).html(value);
    $('.popup-display-overlay').css('display', 'none');
    var scoreId = parseInt($('#fill_As' + savetype + '_' + groupID).attr('data-score'));
    var matrixScore = {};
    matrixScore.ClientId = $('#clientID').val().trim();
    matrixScore.HouseHoldId = $('#houseHoldId').val().trim();
    matrixScore.TestValue = parseInt(value);
    matrixScore.ActiveYear = $('#yearSelect').val().trim();
    matrixScore.CenterId = $('#centerId').val().trim();
    matrixScore.ProgramId = $('#programId').val().trim();
    matrixScore.AnnualAssessmentType = parseInt(savetype);
    matrixScore.AssessmentGroupId = parseInt(groupID);
    matrixScore.ProgramType = $('#programType').val().trim();
    matrixScore.classRoomId = parseInt($('#classRoomId').val().trim());
    matrixScore.MatrixScoreId = scoreId;
    $.ajax({
        url: "/Roster/InsertMatrixScore",
        datatype: 'json',
        type: 'post',
        data: matrixScore,
        success: function (data) {

            var act_year = $('#yearSelect').val().trim();
            GetStaffName(act_year);
            SetChartDetails(act_year);
        }
    });
});

//On click over the Question  Color box in the Assessment Group//
$('.question-image').click(function () {
    var groupId = parseInt($(this).attr('group-id'));
    var clientId = $('#clientID').val().trim();
    var count = $('#count_' + groupId).val().trim();
    var pos = parseInt($(this).attr('position'));
    var isFirstQn = parseInt($(this).attr('isfirst'));
    $('.div-question-' + count).html('');
    $('.popup-div1').css('display', 'none');
    $('.popup-div').css('display', 'none');
    $('.div-group-' + count).css('display', 'none');
    $('.div-group-' + count).html('');
    $('.popup-display-overlay').html('');
    $('.popup-display-overlay').css('display', 'none');
    $.ajax({
        url: "/Roster/GetQuestions",
        datatype: 'json',
        type: 'post',
        data: { groupId: groupId, clientId: clientId },
        success: function (data) {
            var bindDiv = "<div class='col-xs-12 text-center' style='padding:10px;font-size:24px; margin:auto;color:#163b69; margin-bottom:10px;'>Questions</div><div class='close-div'><i class='fa fa-times' aria-hidden='true'></i></div>";
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    var getdiv = "";
                    getdiv = $('#questionText');
                    var clno = i + 1;
                    $('.sl-no').html(clno);
                    $('.question').attr('question-id', data[i].AssessmentQuestionId);
                    $('.question').html(data[i].AssessmentQuestion);
                    getdiv.removeClass('sl-no question');
                    bindDiv += getdiv.html();
                }
                $('#questionpopupDiv').html(bindDiv);
                $('.div-question-' + count).html('');
                $('.div-question-' + count).html($('.question-view-popup').html());
                $('.div-question-' + count).css('display', 'block');
                $('.popup-div1').css('display', 'block');

                //var arrwheight = $('.left-arw').css('top');
                //var height = parseInt(arrwheight.split('px')[0]);
                //if (pos !== 1) {
                //    var arwConvHeitht = (pos * 45) + "px";
                //    $('.left-arw').css('top', arwConvHeitht);
                //}
                //else {
                //    $('.left-arw').css('top', '45px');
                //}



                //var divHeight = -16;
                //var heightarray = ['-16px', '36px', '90px', '148px', '201px']

                //var divheight2 = heightarray[pos - 1];
                //$('.popup-div1').css('top', divheight2);


                var heightarrayQn = '';
                var divheightQn = '';
                if (isFirstQn === 1) {

                    var divHeight = -16;
                    heightarrayQn = ['-16px', '50px', '116px', '182px', '248px', '311px', '377px', '443px'];
                    divheightQn = heightarrayQn[pos - 1];
                    $('.popup-div1').css('top', divheightQn);
                }
                else {
                    heightarrayQn = ['-135px', '-70px', '-5px', '61px', '126px', '192px', '255px', '321px'];
                    divheightQn = heightarrayQn[pos - 1];
                    $('.popup-div1  ').css('top', divheightQn);
                }

            }
        }
    });
});

//on Change of Year Select Option//
$(document).on('change', '#yearSelect', function () {

    var year = $(this).val();

    SetChartDetails(year);
    GetStaffName(year);
});
