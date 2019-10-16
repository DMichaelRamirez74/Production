var savetype = 0;
var assessmentType = parseInt($('#assessmentType').val().trim());
var houseHoldId = $('#houseHoldId').val().trim();
var activeYear = $('#activeYear').val().trim();
var clientId = $('#clientID').val().trim();
var assessmentNo_rec = 0;
var clientName = $('#clientName').val().trim();
var agencyId = '';
var referralClientServiceId = 0;
var result = "0";
var ParentName = '';
var step_id = 0;
var serviceCount = 0;
var communityId = 0;
var serviceId = 0;
var isUpdate = false;
var singleParent = false;
var goalFor = 0;
var round = 1;
var maxMatrixValue = 0;


//$(window).on('load',function () {


//  //  $('.bar-green').css({ 'transition': 'height 5s ease-in 2s' });
//});

//check-parent
$(document).ready(function () {
    $('#matrix_recommendations,#matrix_fbaPopup,#matrix_referalpopup').on('shown.bs.modal', function (e) {
        $(".nano").nanoScroller();
    });

    $("#matrix_recommendations").on("hide.bs.modal", function () {
        // put your default event here

    });

    $('#recommClosBtn').on('click', function () {
        var browsernames = getBrowserName();
        if (browsernames.IsSafari) {
            location.reload();
        }

    });

    $.ajax({
        url: "/Roster/GetClientStatus",
        datatype: "json",
        async: false,
        data: { HouseHoldID: houseHoldId },
        success: function (data) {

            var imagesrc = data.matrixscore.ProfilePic === "" ? ("/Images/prof-image.png") : ("data:image/jpg;base64," + data.matrixscore.ProfilePic);
            $('#profileImage').attr('src', imagesrc);
            $("#profileImage").css("display", "block");

            $('#parentName').html(data.matrixscore.ParentName);
            ParentName = data.matrixscore.ParentName;
            var selectedAppend = '';
            if (data.matrixscore.ActiveYearList.length > 0) {
                $.each(data.matrixscore.ActiveYearList, function (i, element) {

                    selectedAppend += '<option value=' + element.Text + '>20' + element.Text + '</option>'
                });
                $('#yearSelect').append(selectedAppend);
            }
            if (data.recommList.length > 0) {
                $.each(data.recommList, function (j, element2) {

                    var as_No = element2.AssessmentNumber;
                    var isShow = element2.ShowPopup;
                    if (isShow) {
                        $('.rec_as_' + as_No).removeClass('hidden');
                    }
                    else {
                        $('.rec_as_' + as_No).addClass('hidden');
                    }

                })
            }
        }
    });

    initializeFPAPopUp();

    $('#testdate').datepicker();
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
        var percentChange = 10;
        var changePx = 0;
        var diffPx = 0;
        if (selfheight > textheight) {
            $(this).find('.change-div').css('height', selfheight + 'px');
            value = $(this).attr('cat-id');

            green_bar = parseInt(selfheight - 40);
            if (selfheight > 260) {
                $('.change-bar-div_' + value).css('height', green_bar + 'px');

            }

            //changePx = parseFloat((percentChange * selfheight) / 100).toFixed(2);
            //diffPx = selfheight - changePx;
            //$('.change-bar-div_' + value).css('height', diffPx + 'px');

        }
        else {
            $(this).find('.change-div').css('height', textheight + 'px');
            value = $(this).attr('cat-id');
            green_bar = parseInt(textheight - 40);
            if (textheight > 260) {
                $('.change-bar-div_' + value).css('height', green_bar + 'px');

            }

            //changePx = parseFloat((percentChange * textheight) / 100).toFixed(2);
            //diffPx = textheight - changePx;
            //$('.change-bar-div_' + value).css('height', diffPx + 'px');

        }
    });


    $("body").on('keyup', '.txt-date', function (e) {

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

    $('body').on("keydown", "#datecompleted,.txt-date", function (e) {
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
    var Enc_clientId = $('#clientID').val().trim();
    $.ajax({
        url: "/Roster/GetChartDetails",
        datatype: "json",
        type: 'post',
        async: false,
        data: { houseHoldId: houseHoldId, date: year, clientId: Enc_clientId },
        success: function (data) {

            $('.bar-green').css('height', '0%');
            $('.bar-green').addClass('hidden');

            $('.bar-label-new').css('height', '18%');
            $('.bar-label-new').children('p').html('');
            $('.char-percentage').html('-').css('color', 'black');
            $('.percentage-image').addClass('hidden');
            $('.avg-p').html('0<sub>Avg</sub>')
            $('.bar-label1').children('p').html('0');
            $('.elipse-grade').children('p').html('0.00');
            $('.mat-score').html('-');
            $('.mat-score').attr('data-score', 0);
            $('.mat-score').attr('testvalue', 0);
            // alert(data.groupType);
            savetype = data.groupType;

            switch (savetype) {
                case 1:
                   
                    $('#Ass1').removeAttr('disabled');
                    $('#Ass1').attr('checked', true);
                    break;
                case 2:                   
                    $('#Ass2').removeAttr('disabled');
                    $('#Ass2').attr('checked', true);
                    break;
                case 3:                   
                    $('#Ass3').removeAttr('disabled');
                    $('#Ass3').attr('checked', true);
                    break;
            }

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
                    chardetails = true;

                    $.each(data.chardetailsList, function (k, chart) {

                        if (chart.MaximumMatrixValue > 0 && maxMatrixValue == 0) {
                            maxMatrixValue = chart.MaximumMatrixValue;
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
                           
                            var label_Height = getLabelHeight2(height);
                         
                            $('.labelAs' + assessmentNumber + '_' + catId).css('height', label_Height + '%');
                            $('.labelAs' + assessmentNumber + '_' + catId).children('p').html(parseInt(percentage)==percentage?parseInt(percentage):percentage);
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
                $('.bar-label-new').css('height', '18%');
                $('.bar-label-new').children('p').html('');
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
                                        finalper =(test1=0)?0: Math.abs((differnceval / test1) * 100);
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
                                            finalper =(test1==0)?0: Math.abs((differnceval / test1) * 100);
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
                                        test1 = parseInt($('#fill_As1_' + Group_Id).attr('TestValue'));
                                        test2 = parseInt($('#fill_As2_' + Group_Id).attr('TestValue'));
                                        // var cat_Id = data.arraylist[i][c].AssessmentCategoryId;
                                        differnceval = test1 - test2;
                                        finalper =(test1==0)?0: Math.abs((differnceval / test1) * 100);
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

            var height1 = (maxMatrixValue==0)?0: ((as1Percentage / maxMatrixValue) * 100);

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
            $('.master-bar1').children('.bar-label2').children('.avg-p').html( parseInt(as1Percentage)==as1Percentage?parseInt(as1Percentage):as1Percentage + '<sub>Avg</sub>');
            $('.master-bar1').children('.bar-label1').children('p').html(assessment1score);
        }
        var height2 = 0;

        if (assessmentType === 2) {

            if (as2Percentage > 0) {


                //height2 = (as2Percentage * convertedratio);
                height2 = (maxMatrixValue == 0) ? 0 : ((as2Percentage / maxMatrixValue) * 100);

                if (height2 === 0) {
                    $('.bar-green2').addClass('hidden');
                }
                else {
                    $('.bar-green2').removeClass('hidden');

                }
                $('.bar-green2').height(height2 + '%');

                $('.master-bar2').children('.bar-label2').children('.avg-p').html(parseInt(as2Percentage)==as2Percentage?parseInt(as2Percentage):as2Percentage + '<sub>Avg</sub>');
                $('.master-bar2').children('.bar-label1').children('p').html(assessment2score);

            }
            $('.barl-2diff').children('p').html(as1_as2diff);
        }
        if (assessmentType === 3) {

            if (as2Percentage > 0) {


                //  height2 = (as2Percentage * convertedratio);

                height2 = (maxMatrixValue == 0) ? 0 : ((as2Percentage / maxMatrixValue) * 100);

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

                $('.master-bar2').children('.bar-label2').children('.avg-p').html(parseInt(as2Percentage) == as2Percentage ? parseInt(as2Percentage) : as2Percentage + '<sub>Avg</sub>');
                $('.master-bar2').children('.bar-label1').children('p').html(assessment2score);


            }
            if (as3Percentage > 0) {

                // var height3 = (as3Percentage * convertedratio);

                height3 = (maxMatrixValue == 0) ? 0 : ((as3Percentage / maxMatrixValue) * 100);

                if (height3 === 100) {
                    $('.bar-green3').css('bottom', '-1px');
                }
                else if (height3 === 0) {
                    $('.bar-green3').addClass('hidden');
                }
                else {
                    $('.bar-green3').removeClass('hidden');

                }

                //   $('.bar-green3').height(height3 + '%');

                $('.bar-green3').css({ 'height': height3 + '%' });

                //$('.bar-green3').css({ 'transition': 'height 1s ease-in 0s' });

                $('.master-bar3').children('.bar-label2').children('.avg-p').html(parseInt(as3Percentage)==as3Percentage?parseInt(as3Percentage):as3Percentage + '<sub>Avg</sub>');
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

//function to get the Label Height based on the Chart height//
function getLabelHeight(lblHeight) {
     
    var heightLbl = 0;

    if (lblHeight == 100) {

        heightLbl = 97;
    }
    else if (lblHeight >= 90 && lblHeight < 100) {
        heightLbl = 88;

    }
    else if (lblHeight >= 80 && lblHeight < 90) {
        heightLbl = 79;
    }
    else if (lblHeight >= 70 && lblHeight < 80) {
        heightLbl = 70;

    }
    else if (lblHeight >= 60 && lblHeight < 70) {
        heightLbl = 61;

    }
    else if (lblHeight >= 50 && lblHeight < 60) {
        heightLbl = 52;

    }
    else if (lblHeight >= 40 && lblHeight < 50) {
        heightLbl = 48;

    }
    else if (lblHeight >= 30 && lblHeight < 40) {
        heightLbl = 35;

    }
    else if (lblHeight >= 20 && lblHeight < 30) {
        heightLbl = 26;

    }
    else if (lblHeight >= 10 && lblHeight < 20) {
        heightLbl = 19;

    }
    else if (lblHeight >= 5 && lblHeight < 10) {
        heightLbl = 19;

    }
    else {
        heightLbl = 19;

    }


    return heightLbl;
}

function getLabelHeight2(Height2) {
    var heightArray = {
        1: 18,
        2: 18,
        3: 18,
        4: 18,
        5: 18,
        6: 18,
        7: 18,
        8: 18,
        9: 20,
        10: 20,
        11: 21,
        12: 22,
        13: 22,
        14: 24,
        15: 25,
        16: 26,
        17: 26,
        18: 27,
        19: 28,
        20: 30,
        21: 29,
        22: 30,
        23: 31,
        24: 32,
        25: 35,
        26: 34, 27: 35, 28: 35, 29: 36, 30: 37, 31: 37,
        32: 39,
        33: 39, 34: 41, 35: 41, 36: 42, 37: 42, 38: 43, 39: 43, 40: 45, 41: 45, 42: 47, 43: 47, 44: 49, 45: 49, 46: 50,
        47: 50, 48: 52, 49: 52, 50: 53, 51: 53, 52: 55, 53: 55, 54: 57, 55: 57, 56: 58, 57: 58, 58: 60, 59: 60, 60: 61, 61: 61, 62: 63, 63: 63, 64: 65, 65: 65, 66: 66, 67: 66,
        68: 68, 69: 68, 70: 70, 71: 70, 72: 72, 73: 72, 74: 73, 75: 73, 76: 75, 77: 75, 78: 77, 79: 77, 80: 79, 81: 79, 82: 79, 83: 80, 84: 80, 85: 82,
        86:82,87:84,88:84,89:86,90:86,91:87,92:88,93:89,94:89,95:90,96:90,97:92,98:92,99:94,100:94

    };
    var roundValue = Math.round(Height2);
  
    var heightAr = heightArray[roundValue];
   
    return heightAr;
}

//On Click over the assessment group to show the description popup//
$('.assessment-group').click(function () {

    if ($('.CheckClient').is(':checked') == false) {

        customAlert("Please select any one parents.");
        return false;
    }
    if ($('#yearSelect').val() == null) {
        customAlert("Please select any one parents.");
        return false;
    }


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
                    //  $('.desc-text').attr('title',data[i].Description);
                    $('#toolTipDesc').html(data[i].Description);
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


            if (data.isShow) {
                if (data.recommendationList != null) {
                    assessmentNo_rec = savetype;
                    bindRecommendtions(data.recommendationList);

                }
            
            }

            var act_year = $('#yearSelect').val().trim();
            GetStaffName(act_year);
            SetChartDetails(act_year);
        }

    });


});


function bindRecommendtions(data2) {
    var appendDiv = $('.rec-pop-div');
    var bindGroup = '';
    var appendGroup = '';
    var bindGroupSection = '';
    // var lastEntered = data.lastId;
    var id = $('#clientID').val();
    var householdid = $('#houseHoldId').val();
    var centerId = $('#centerId').val();
    var programId = $('#programId').val();
    var clientName = $('#parentName').html();

    if (data2.length > 0) {
        $.each(data2, function (i, recommendElement) {
            bindGroup = '';
            $.each(recommendElement, function (j, element2) {
                var catName = element2.Category;
                var catId = element2.AssessmentCategoryId;
                var description = element2.Description;
                var IsReff = element2.ReferralSuggested;
                var IsFPA = element2.FPASuggested;
                appendDiv.find('.category-Name').html(catName);
                var linkFPA = '';
                var linkRef = '';
                if (IsFPA) {
                    //linkFPA = '<a href="/Roster/FPAList?id=' + id + '&Householdid=' + householdid + '&centerid=' + centerId + '&Programid=' + programId + '&ClientName=' + clientName + '">Yes</a>';

                    linkFPA = '<a href="#" id="fpaYes" data-toggle="modal" data-target="#matrix_fbaPopup" style="text-decoration:underline;">Yes</a>';

                }
                else {
                    linkFPA = '<a>No</a>';
                }
                linkRef = (IsFPA) ? '<span class="rgt"><a href="javascript:void(0);" id="referralYes" style="text-decoration:underline;" >Yes</a></span>' : '<span class="rgt"><a>No</a></span>';
                bindGroup += '<li><div><span class="lft ">' + description + '</span><span class="mid">' + linkFPA + '</span>' + linkRef + '</div></li>';
            });
            appendGroup = bindGroup;
            appendDiv.find('.max_items').addClass('pop-up-cat');
            appendDiv.find('.bind-group').html(appendGroup).addClass('bind-group-' + i);
            bindGroupSection += appendDiv.html();
            appendDiv.find('.bind-group').removeClass('bind-group-' + i);
            appendDiv.find('.max_items').removeClass('pop-up-cat');
        });
        $('.bind-div-recommendations').html(bindGroupSection);

        $('#matrix_recommendations').modal('show');
    }
}
$('.view_recom').click(function () {

    assessmentNo_rec = parseInt($(this).attr('as-no'));

    $.ajax({
        url: '/Roster/GetRecommendations',
        datatype: 'json',
        type: 'post',
        data: { HouseholdId: houseHoldId, assessmentNo: assessmentNo_rec, activeProgramYear: activeYear },
        success: function (data) {
            bindRecommendtions(data);

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
                    // $('.desc-qn').attr('title', data[i].AssessmentQuestion);
                    $('#toolTipQn').html(data[i].AssessmentQuestion);
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
    $('.view_recom').addClass('hidden');
    SetChartDetails(year);
    GetStaffName(year);
});


//On Click of Referral Yes on the popup//


$('#matrix_recommendations').on('click', '#referralYes', function () {
    $.ajax({
        url: '/Roster/ReferralServicePopUp',
        datatype: 'json',
        type: 'post',
        data: { ClientId: clientId, ClientName: clientName },
        success: function (data) {
            if (data != null) {
                if (data.length > 0) {

                    var referralbindDiv = $('.referralservice-div');
                    var bindDiv = '';
                    var refDate = '';
                    var count = 0;
                    $.each(data, function (i, element) {

                        count = i + 1;
                        referralbindDiv.find('.referral-date-span').html('Referral #' + count + ' ' + element.ConvCreatedDate);
                        referralbindDiv.find('.ReferralServiceName').html(element.ServiceName).attr({ 'data-stepId': element.Step, 'parentname': element.ParentName, 'referralclientid': element.ReferralClientServiceId });
                        referralbindDiv.find('#client_id').val(element.ClientId);
                        referralbindDiv.find('#service_id').val(element.ClientId);
                        referralbindDiv.find('#client_id').val(element.ServiceId);
                        referralbindDiv.find('.delete_span').children('#DeleteClientService').val(element.ReferralClientServiceId);
                        referralbindDiv.find('.service-No').addClass('remove_' + element.ReferralClientServiceId);

                        if (element.Step == 2) {
                            referralbindDiv.find('.delete_span').removeClass('hidden');
                            referralbindDiv.find('.delete_span').attr('refID', element.ReferralClientServiceId);
                        }
                        else {
                            referralbindDiv.find('.delete_span').addClass('hidden');
                        }
                        bindDiv += referralbindDiv.html();
                        referralbindDiv.find('.service-No').removeClass('remove_' + element.ReferralClientServiceId);

                    });
                    $('.referrservice_div').html(bindDiv);

                }
                //$('.modal').modal('hide');
                $('#matrix_referalpopup').find('.new-referral-step2').hide();
                $('#matrix_referalpopup').find('.new-referral-step1').hide();
                $('#matrix_referalpopup').find('.new-referral-agency-step1').hide();
                $('.referalservice').show();
                $('#matrix_referalpopup').modal('show');
            }
        },
        error: function (data) {

        }
    })
});


$('.referrservice_div').on('click', '.ReferralServiceName', function () {
    referralClientServiceId = $(this).attr('referralclientid');
    var AgencyValue = "";

    ParentName = $(this).attr('parentname');

    var ClientName = $('#clientName').val();
    var ID = $('#id').val();
    step_id = parseInt($(this).attr('data-stepId'));
    referralClientServiceId = parseInt($(this).attr('referralclientid'));
    if (step_id == 2) {
        bindReferralCategoryAjax();
    }
    else {
        bindMatchProviders();
    }
});


//$('.ReferralServiceName').click(function () {
//    var ReferralClientId = $(this).attr('ReferralClientId');
//    var AgencyValue = "";
//    var result = "0";
//    var ParentName = $(this).attr('parentName');
//    var ClientName=$('#clientName').val();
//    var ID = $('#id').val();

//    var Step = $(this).attr('data-stepId');

//    var ClientId = $('#ClientId').val();
//    if (parseInt(Step) === 2) {      

//        window.location.href = "/Roster/ReferralCategory?id=" + $('#id').val() + "&ReferralClientId=" + ReferralClientId + "&Step=" + 2 + "&clientName=" + $('#clientName').val() + "&parentName=" + ParentName;
//    }
//    else if (parseInt(Step) === 3) {
//        //window.location.href = "/Roster/ReferralCategory?id=" + $('#id').val() + "&ReferralClientId=" + ReferralClientId + "&Step=" + 2 + "&clientName=" + $('#clientName').val() + "&parentName=" + ParentName;
//        window.location.href = "/Roster/MatchProviders?AgencyId=" + AgencyValue + "&CommunityId=" + result + "&parentName=" + ParentName + "&referralClientId=" + ReferralClientId + "&clientName=" + $('#clientName').val() + "&id=" + $('#id').val() + "&stepId=" + Step;

//    }
//});


//$(".DeleteService").click(function () {
//    var ReferralClientServiceId = $('#DeleteClientService').val();
//    $.ajax({
//        url: "/Roster/DeleteReferralService",
//        type: "POST",
//        data: {
//            ReferralClientServiceId: ReferralClientServiceId
//        },
//        success: function (data) {
//            if (data = true) {
//                location.reload();
//            }
//        }
//    });

//})

$('.new-request-family').click(function () {
    referralClientServiceId = 0;
    bindReferralCategoryAjax();

});

//Referral Catgory Page Method//
function bindReferralCategoryAjax() {
    $.ajax({

        url: '/Roster/ReferralCategoryPopup',
        datatype: 'json',
        type: 'post',
        data: { id: clientId, clientName: clientName, ReferralClientId: referralClientServiceId },
        success: function (data) {

            var parent_childDiv = $('.parent-span');
            referralClientServiceId = data.refferralClientId;
            var parentDiv = '';
            var ChildDiv = '';
            var parentSpan = '';
            var childSpan = '';
            var CategorySpan = '';
            if (data.refList != null && data.refList.length > 0) {

                $.each(data.refList, function (i, refElement) {


                    if (refElement.ClientID != null) {

                        if (!refElement.IsChild) {


                            agencyId = refElement.AgencyID;
                            // parentName = refElement.ParentName;
                            parentSpan += '<span><input type="checkbox" parentName=' + refElement.ParentName + ' " class="CheckClient" value=' + refElement.ClientID + ' householdid=' + refElement.HouseHoldId + ' id="ClientID" style="margin-left:0;">' + refElement.ParentName + '</span>';

                        }
                        else {
                            childSpan += '<span><input type="checkbox" parentName=' + refElement.ParentName + ' " class="CheckClient"  value=' + refElement.ClientID + ' householdid=' + refElement.HouseHoldId + ' id="ClientID" style="margin-left:0;">' + refElement.ParentName + '</span>';
                        }


                    }
                    else {

                        $('.category_heading').html(refElement.Description).attr({ 'category-id': refElement.CategoryID });

                        CategorySpan += (refElement.Status) ? '<li><input type="checkbox" class="chk_all"  id="chkBox_' + refElement.ServiceID + '" value=' + refElement.ServiceID + ' checked >' + refElement.Services + '</li>' : '<li><input type="checkbox" class="chk_all"  id="chkBox_' + refElement.ServiceID + '" value=' + refElement.ServiceID + '>' + refElement.Services + '</li>';;

                    }

                });

                $('.ul_cateList').html('<ul>' + CategorySpan + '</ul>');

                $('.chil_append').html(childSpan);
                $('.parent_append').html(parentSpan);
                showCategoryPage();
                bindCheckClientCategory();
            }
        },
        error: function (data) {

        }
    });
}

function bindCheckClientCategory() {
    $.ajax({

        url: "/Roster/HouseHoldReferrals",
        datatype: "json/application",
        data: { referralClientId: referralClientServiceId },
        success: function (data) {
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    var parentId = data[i].Value;
                    $('input[type=checkbox][value=' + parentId + ']').prop('checked', true)

                }
            }
            else {

            }
        }
    });
}
//click of all check all button//
$('.check').on('click', function (e) {

    $('.chk_all').prop('checked', true)
});
$('.uncheck').on('click', function (e) {

    $('.chk_all').prop('checked', false)
});

//on click of Save on Referral Category Pop up//

$('#Save').click(function (e) {
    var AgencyValue = agencyId;
    var ServiceId = "";
    $(".chk_all").map(function () {
        if ($(this).is(':checked'))
            ServiceId += this.value + ",";
    });
    ServiceId = ServiceId.slice(0, -1);

    var client_Id = "";

    $(".CheckClient").map(function () {
        if ($(this).is(':checked'))
            client_Id += this.value + ",";
    });

    client_Id = client_Id.slice(0, -1);
    var householdId = parseInt($('.CheckClient').attr('householdid'));

    if ($('#ClientID:checked').val() == undefined && ($('.chk_all:checked').val() == undefined)) {
        $('.validation1').css("display", "inline-block");
        $('.validation1').text("Select Family Members");
        $('.validation2').css("display", "inline-block");
        $('.validation2').text(" Select Referral Type");
        $('.setfamily').addClass('setcolor');
        $('.setreferral').addClass('setcolor');


    }
    else if ($('#ClientID:checked').val() == undefined) {
        $('.validation1').css("display", "inline-block");
        $('.validation1').text("Select Family Members");
        $('.validation2').hide();
        $('.setfamily').addClass('setcolor');
        $('.setreferral').removeClass('setcolor');

    }
    else if ($('.chk_all:checked').val() == undefined) {
        $('.validation2').css("display", "inline-block");
        $('.validation2').text(" Select Referral Type");
        $('.validation1').hide();
        $('.setfamily').removeClass('setcolor');
        $('.setreferral').addClass('setcolor');

    }

    else {

        $('.validation1').hide();
        $('.validation2').hide();
        $('.setfamily').removeClass('setcolor');
        $('.setreferral').removeClass('setcolor');
        var SaveReferral = {};
        SaveReferral.ServiceId = ServiceId;
        SaveReferral.AgencyId = AgencyValue;
        SaveReferral.CommonClientId = clientId;
        SaveReferral.HouseHoldId = householdId;
        SaveReferral.ClientId = client_Id;
        SaveReferral.referralClientId = referralClientServiceId;

        $.ajax({
            url: "/Roster/SaveReferralClient",
            type: "POST",
            data: SaveReferral,
            success: function (data) {
                if (data) {
                    customAlert('Record saved successfully');

                    $('#matrix_referalpopup').modal('hide');
                    $('#matrix_recommendations').modal('show');
                }

            }
        });


    }
});
//on click of match providers on Referral Category Pop up//

$("#Matchprovider").click(function (e) {
    result = '';
    $(".chk_all").map(function () {
        if ($(this).is(':checked'))
            result += this.value + ",";
    });
    result = result.slice(0, -1);


    var CommunityId = $('.chk_all:checked').val();
    $('.parentName').val();
    var ParentName = $('.CheckClient').attr('parentName');
    var householdId = parseInt($('.CheckClient').attr('householdid'));

    if ($('#ClientID:checked').val() == undefined && ($('.chk_all:checked').val() == undefined)) {
        $('.validation1').css("display", "inline-block");
        $('.validation1').text("Select Family Members");
        $('.validation2').css("display", "inline-block");
        $('.validation2').text(" Select Referral Type");
        $('.setfamily').addClass('setcolor');
        $('.setreferral').addClass('setcolor');
    }
    else if ($('#ClientID:checked').val() == undefined) {
        $('.validation1').css("display", "inline-block");
        $('.validation1').text("Select Family Members");
        $('.validation2').hide();
        $('.setfamily').addClass('setcolor');
        $('.setreferral').removeClass('setcolor');

    }
    else if ($('.chk_all:checked').val() == undefined) {
        $('.validation2').css("display", "inline-block");
        $('.validation2').text("Select Referral Type");
        $('.validation1').hide();
        $('.setfamily').removeClass('setcolor');
        $('.setreferral').addClass('setcolor');

    }
    else {
        $('.validation1').hide();
        $('.validation2').hide();
        $('.setfamily').removeClass('setcolor');
        $('.setreferral').removeClass('setcolor');

        var matchProviderList = {
            'AgencyId': agencyId,
            'id': clientId,
            'parentName': ParentName,
            'referralClientId': 0,
            'clientName': clientName
        }
        step_id = 0;
        bindMatchProviders();


    }

});
//function to Bind Match Providers//
function bindMatchProviders() {
    $.ajax({
        url: '/Roster/MatchProvidersPopUp',
        datatype: 'json',
        type: 'post',
        data: { id: clientId, parentName: ParentName, referralClientId: referralClientServiceId, clientName: clientName, CommunityIds: result },
        success: function (data) {


            serviceCount = (data.MPMList != null) ? data.MPMList.length : 0;
            if (data.MPMList != null && data.MPMList.length > 1) {
                var serviceOption = ' <option value="0">Select Resources</option>';
                $.each(data.MPMList, function (i, serviceElement) {
                    serviceOption += '<option value=' + serviceElement.ServiceId + '>' + serviceElement.Services + '</option>';
                    referralClientServiceId = serviceElement.ReferralClientServiceId;
                });
                $('.new-referral-step2').find('#FSResourcesSelect').html(serviceOption).removeClass('hidden');
                $('.new-referral-step2').find('#servicesName').addClass('hidden');
                var organisationSelection = ' <option value="0">Select Organization</option>';
                $('.new-referral-step2').find('#ddFsOrganization').html(organisationSelection).removeClass('hidden');
                $('.new-referral-step2').find('#ddFsOrganizationLbl').html('').addClass('hidden');
                $('.new-referral-step2').find('#spnOrganizationName').html('');
                $('.new-referral-step2').find('#SpnCommunityAddress').html('');
                $('.new-referral-step2').find('#SpnCommunityPhone').html('');
                $('.new-referral-step2').find('#SpnCommunityEmail').html('');
                $('.new-referral-step2').find('#SpnCommunityCity').html('');
                $('.new-referral-step2').find('#SpnCommunityState').html('');
                $('.new-referral-step2').find('#SpnCommunityZipCode').html('');
            }
            else {
                $('.new-referral-step2').find('#FSResourcesSelect').html('').addClass('hidden');
                $('.new-referral-step2').find('#servicesName').html(data.MPMList[0].Services).removeClass('hidden');
                $('.new-referral-step2').find('#FSResources').val(data.MPMList[0].ServiceId);
                $('.new-referral-step2').find('#ddFsOrganization').html('').addClass('hidden');
                $('.new-referral-step2').find('#ddFsOrganizationLbl').html(data.OrganizationList[0].Text).removeClass('hidden');
                $('.new-referral-step2').find('#OrganizationId').val(data.OrganizationList[0].Value);
                referralClientServiceId = data.MPMList[0].ReferralClientServiceId;
            }
            $('.new-referral-step2').find('#Description').html('');
            $('.new-referral-step2').find('#Description').val('');
            $('.new-referral-step2').find('#Description').val(data.MPMList[0].Notes)
            var jqueryDate = (data.MPMList[0].ReferralDate == null) ? new Date() : new Date(data.MPMList[0].ReferralDate);
            var formattedDate = getFormattedDate(jqueryDate);
            $('.new-referral-step2').find('#datepicker').val(formattedDate);
            if (data.MPMList.length == 1) {
                $.ajax({
                    url: "/Roster/FamilyResourcesList",
                    type: "POST",
                    async: false,
                    data: { serviceId: data.MPMList[0].ServiceId },
                    success: function (data) {
                        $("#ddFsOrganization").html('');
                        $('#OrganizationId').val('');
                        $("#ddFsOrganization").html(data.va);
                        for (var i = 0; i < data.listOrganization.length; i++) {
                            $('#OrganizationId').val(data.listOrganization[i].Value);
                            $("#ddFsOrganization").html(data.listOrganization[i].Text);
                        }
                    }
                });

                communityId = $('.new-referral-step2').find('#OrganizationId').val();

                $.ajax({
                    url: "/Roster/GetOrganization",
                    type: "POST",
                    data: { CommunityId: communityId },
                    success: function (data) {
                        $('.new-referral-step2').find('#CommunityId').val(data.CommunityId);
                        if (data.Address == null) {
                            $('.new-referral-step2').find('#SpnCommunityAddress').text("");
                        } else {
                            $('.new-referral-step2').find('#SpnCommunityAddress').text(data.Address);
                        }
                        if (data.City == null) {
                            $('.new-referral-step2').find('#SpnCommunityCity').text("");
                        }
                        else {
                            $('.new-referral-step2').find('#SpnCommunityCity').text(data.City);
                        }

                        if (data.State == null) {
                            $('.new-referral-step2').find('#SpnCommunityState').text("");
                        }
                        else {
                            $('.new-referral-step2').find('#SpnCommunityState').text(data.State + ",");
                        }

                        if (data.ZipCode == null) {
                            $('.new-referral-step2').find('#SpnCommunityZipCode').text("");
                        }
                        else {
                            $('.new-referral-step2').find('#SpnCommunityZipCode').text(data.ZipCode);
                        }

                        if (data.OrganizationName == null) {
                            $('.new-referral-step2').find('#spnOrganizationName').text("");
                        }
                        else {
                            $('.new-referral-step2').find('#spnOrganizationName').text(data.OrganizationName);
                        }

                        if (data.Phone == null) {
                            $('.new-referral-step2').find('#SpnCommunityPhone').text("");
                        }
                        else {
                            $('.new-referral-step2').find('#SpnCommunityPhone').text(data.Phone);
                        }

                        if (data.Email == null) {
                            $('.new-referral-step2').find('#SpnCommunityEmail').text("");
                        }
                        else {
                            $('.new-referral-step2').find('#SpnCommunityEmail').text(data.Email);
                        }

                    }
                });
            }
            $('.referalservice').hide();
            $('.new-referral-agency-step1').hide();
            $('.new-referral-step1').hide();
            $('.new-referral-step2').show().find('.error').addClass('hidden');
            if (step_id === 3) {

                $('.new-referral-step2').find('#datepicker').prop('disabled', true);
                $('.new-referral-step2').find('#referralServiceSaveMethod').addClass('hidden');
                $('.new-referral-step2').find('#Description').prop('disabled', true);

            }
            else {
                $('.new-referral-step2').find('#datepicker').prop('disabled', false);
                $('.new-referral-step2').find('#referralServiceSaveMethod').removeClass('hidden');
                $('.new-referral-step2').find('#Description').prop('disabled', false);
            }
            if (new Date(new Date().toDateString()) > new Date(new Date($('#datepicker').val()).toDateString())) {
                $('.new-referral-step2').find('#surveryRef').removeClass('hidden');
            }
            else {
                $('.new-referral-step2').find('#surveryRef').addClass('hidden');
            }


        },
        error: function (data) {

        }
    });
}
//On Change of Services on MatchProviders popUp
$('#FSResourcesSelect').on('change', function () {

    var serviceId = (this.value);
    $.ajax({
        url: "/Roster/FamilyResourcesList",
        type: "POST",
        data: { serviceId: serviceId },
        success: function (data) {
            $("#ddFsOrganization").html('');
            $("#ddFsOrganization").append('<option value=' + 0 + '>' + "Select Organization" + '</option>');
            for (var i = 0; i < data.listOrganization.length; i++) {
                $("#ddFsOrganization").append('<option value=' + data.listOrganization[i].Value + '>' + data.listOrganization[i].Text + '</option>');
            }
        }
    });
})

//On Change of Organization Name//
$('#ddFsOrganization').on('change', function () {
    var communityId = (this.value);
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
                $('#SpnCommunityState').text(data.State + ', ');
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

$('#referralServiceSaveMethod').click(function () {
    if (serviceCount > 1) {
        if (parseInt($('#FSResourcesSelect').val()) == 0) {
            $('#answererror').html('Please select service resources');
            $('#FSResources').focus();
            $('#err_resource').css("display", "inline-block");
            $('#err_resource').text("Please Select Resource");
            $('#err_resource').removeClass('hidden');
            //   $('#surveyAnswerError').modal('show');

            return false;
        }

        if (parseInt($('#ddFsOrganization').val()) == 0) {
            $('#err_resource').hide();
            $('#err_resource').text("");
            $('#answererror').html('Please select service organization');
            $('#ddFsOrganization').focus();
            $('#err_organization').css('display', 'inline-block');
            $('#err_organization').removeClass('hidden');
            $('#err_organization').text('Please Select Organization Name');
            //$('#surveyAnswerError').modal('show');
            return false;
        }
    }

    if ($('#datepicker').val() == "") {
        $('#err_resource').hide();
        $('#err_resource').text("");
        $('#err_organization').hide();
        $('#err_organization').text("");
        $('#answererror').html('Please enter referral date.');
        $('#datepicker').focus();
        $('#err_date').css('display', 'inline-block');
        $('#err_date').removeClass('hidden');
        $('#err_date').text('Please Enter Referral Date');

        return false;
    }
    else if (!isDate($('#datepicker').val().trim())) {
        $('#err_resource').hide();
        $('#err_resource').text("");
        $('#err_organization').hide();
        $('#err_organization').text("");
        $('#err_date').css('display', 'inline-block');
        //$('#err_date').removeClass('hidden');
        $('#err_date').removeClass('hidden');
        $('#err_date').text('Please Enter Valid Date');
        return false;
    }

    $('#err_date').hide();
    $('#err_date').text('');

    var ReferralDate = $('.new-referral-step2').find('#datepicker').val().trim();
    var Description = $('.new-referral-step2').find('#Description').val().trim();
    // var ServiceResourceId = $('#FSResources').val();
    if ($('.new-referral-step2').find('#FSResourcesSelect').hasClass('hidden')) {
        serviceId = $('.new-referral-step2').find('#FSResources').val();
    }
    else {
        serviceId = $('.new-referral-step2').find('#FSResourcesSelect').val();
    }



    if ($('.new-referral-step2').find('#ddFsOrganization').hasClass('hidden')) {
        communityId = $('.new-referral-step2').find('#OrganizationId').val();
    }
    else {
        communityId = $('.new-referral-step2').find('#ddFsOrganization').val();
    }

    var SaveMatchProviders = {};
    SaveMatchProviders.ReferralDate = ReferralDate;
    SaveMatchProviders.Description = Description;
    SaveMatchProviders.ServiceResourceId = parseInt(serviceId);
    SaveMatchProviders.AgencyId = agencyId;
    SaveMatchProviders.CommunityId = communityId;
    SaveMatchProviders.ReferralClientServiceId = parseInt(referralClientServiceId);
    SaveMatchProviders.ClientId = clientId;



    $.ajax({
        url: "/Roster/SaveMatchProviders",
        type: "POST",
        data: SaveMatchProviders,
        success: function (data) {
            if (data = true) {
                customAlert('Record saved successfully');
                $('#matrix_referalpopup').modal('hide');
                // $('#matrix_recommendations').modal('show');
                window.setTimeout(function () {
                    $('#matrix_recommendations').modal('show');

                }, 1000);
            }
        }

    });


});

function getFormattedDate(date) {
    var year = date.getFullYear();

    var month = (1 + date.getMonth()).toString();
    month = month.length > 1 ? month : '0' + month;

    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;

    return month + '/' + day + '/' + year;
}
//On Click of Save Recommendation//

$('#saveRecommendation').on('click', function () {
    saveRecommendation(true);

});

//For Recommendation
$('#clearRecommendation').on('click', function () {

    saveRecommendation(false);
});

function saveRecommendation(status) {
    var recommendations = {
        'CientId': clientId,
        'HouseHoldId': houseHoldId,
        'AssessmentNumber': parseInt(assessmentNo_rec),
        'ActiveProgramYear': activeYear,
        'Status': status
    };
    $.ajax({

        url: '/Roster/InsertMatrixRecommendation',
        datatype: 'json',
        type: 'post',
        data: { matrixRecString: JSON.stringify(recommendations) },
        success: function (data) {
            if (data) {
                if (!status) {
                    $('.rec_as_' + assessmentNo_rec).addClass('hidden');
                }
                customAlert('Record Saved successfully');
            }
            $('#matrix_recommendations').modal('hide');
        }
    });
}
function getDate() {

    var todaydate = new Date();
    var day = todaydate.getDate();
    var month = todaydate.getMonth() + 1;
    var year = todaydate.getFullYear();
    var datestring = month + "/" + day + "/" + year;
}

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
var count = 0;
$('#DescriptionAddBtn').click(function () {
    count++;
    if (count == 1) {
        $('#DynamicDesc1').show();
    }
    if (count == 2) {
        $('#DynamicDesc2').show();
    }
    if (count == 3) {
        $('#DynamicDesc3').show();
    }
})


function showCategoryPage() {
    $('.new-referral-step1').hide();
    $('.referalservice').hide();
    $('.new-referral-agency-step1').hide();
    $('.new-referral-step1').show();
    $('.new-referral-step1').find('.error').css('display', 'none');
    $('.new-referral-step1').children('div').removeClass('setcolor');
}
//$('#Matchprovider').click(function () {
//    $('.referalservice').hide();
//    $('.new-referral-agency-step1').hide();
//    $('.new-referral-step1').hide();
//    $('.new-referral-step2').show();
//})

$('.new-referral-agency').click(function () {

    bindReferralCategoryCompany();


});

function bindReferralCategoryCompany() {
    $.ajax({
        url: '/Roster/ReferralCategorycompanyPopUp',
        datatype: 'json',
        type: 'post',
        data: { id: clientId, clientName: clientName },
        success: function (data) {


            if (data != null && data.length > 0) {
                var bind_Company_ParentDiv = '';
                var bind_Company_ChildDiv = '';
                $.each(data, function (l, companydata) {
                    if (companydata.ClientID != null) {
                        agencyId = companydata.AgencyID;

                        if (companydata.IsChild) {
                            bind_Company_ChildDiv += '<span><input type="checkbox" parentname="' + companydata.ParentName + '" housholdid=' + companydata.HouseHoldId + '  class="CheckClient1" value=' + companydata.ClientID + '  id="ClientID1" style="margin-left:0;">' + companydata.ParentName + '</span>';
                        }
                        else {
                            bind_Company_ParentDiv += '<span><input type="checkbox" parentname="' + companydata.ParentName + '" housholdid=' + companydata.HouseHoldId + ' class="CheckClient1" value=' + companydata.ClientID + '  id="ClientID1" style="margin-left:0;">' + companydata.ParentName + '</span>';
                        }
                    }
                });
                $('.new-referral-agency-step1').find('.referral_company_Parent').html(bind_Company_ParentDiv);
                $('.new-referral-agency-step1').find('.referral_company_Child').html(bind_Company_ChildDiv);
                var todaydate = new Date();
                var ConvertedDate = getFormattedDate(todaydate);
                $('.new-referral-agency-step1').find('#datepicker1').val(ConvertedDate);
                $('.new-referral-agency-step1').find('.error').css('display', 'none');
                $('.new-referral-agency-step1').find('.addresssection').html('');
                $('.new-referral-agency-step1').find('#FFReferralSelect').html('<option value="0">Select Referral Type</option>');
                $('.new-referral-agency-step1').find('#txtSearch,#Description').val('');
                $('.referalservice').hide();
                $('.new-referral-step1').hide();
                $('.new-referral-step2').hide();
                //$('#max_items').css('display','none');
                $('.new-referral-agency-step1').show();
            }

        },
        error: function (data) {

        }

    });
}
var arry = null;
$("#txtSearch").autocomplete({

    source: function (request, response) {
        $('.addresssection').html('');
        $('#FFCommunitySelect').removeClass('hidden');
        $('#FFcommunity').addClass('hidden');
        var sertext = $('#txtSearch').val();

        $.ajax({
            url: "/Roster/AutoCompleteSerType",
            type: "POST",
            dataType: "json",
            data: { Services: sertext },
            success: function (data) {
                arry = data;
                response($.map(data, function (item) {
                    //$('#ServiceID').val('');
                    return { label: item.Services, value: item.Services, id: item.ServiceID };
                }))
            }
        })
    },
    messages: {
        noResults: "",
        results: function (count) {
            return count + (count > 1 ? ' results' : ' result ') + ' found';
        }
    },



    select: function (e, ui) {
        var label = ui.item.label;
        var value = ui.item.value;

        if (arry.length > 0) {
            $('#spnOrganizationName1').html(arry[0].Services);
            $('#SpnCommunityAddress1').html(arry[0].Address);
            $('#SpnCommunityPhone1').html(arry[0].Phone);
            $('#SpnCommunityEmail1').html(arry[0].Email);
        }
        var sid = ui.item.id;
        $('#communityId').val(sid);
        var sidi = $('#communityId').val(sid);
        //store in session
        document.valueSelectedForAutocomplete = value

        var AgencyVal = $('#AgencyId_').val();
        var AgencyId = AgencyVal;

        $.ajax({
            url: "/Roster/GetReferralType",
            type: "POST",
            data: { communityId: sid },
            success: function (data) {

                if (data.length > 0) {
                    if (data.length > 1) {

                        $('#FFReferral').addClass('hidden');
                        $("#FFReferralSelect").html('');
                        $("#FFReferralSelect").append('<option value=' + 0 + '>' + "Select Referral Type" + '</option>');
                        for (var i = 0; i < data.length; i++) {
                            $("#FFReferralSelect").append('<option value=' + data[i].Value + '>' + data[i].Text + '</option>');
                        }
                        $("#FFReferralSelect").removeClass('hidden');
                    }
                    else {
                        $("#FFReferralSelect").addClass('hidden');
                        $('#FFReferral').html('');
                        $('#FFReferral').html(data[0].Text);
                        $('#FFReferral').attr('referralId', data[0].value);
                        $('#FFReferral').removeClass('hidden');
                    }
                }


            }

        });
    }
});

$('#fbaCancelBtn').click(function () {
    $('#matrix_fbaPopup').modal('hide');
});

$('#matrix_referalpopup').on('click', '#Cancel', function () {

    $('.referalservice').hide();
    $('.new-referral-step1').hide();
    $('.new-referral-step2').hide();
    $('.new-referral-agency-step1').hide();
    $('#matrix_referalpopup').modal('hide');
    $('#matrix_recommendations').modal('show');
});




///Referral category company///
$('#referralServiceMethodSave').click(function () {

    var ServiceId = "";
    $('.new-referral-agency-step1').find(".chk_all").map(function () {
        if ($(this).is(':checked'))
            ServiceId += this.value + ",";
    });
    ServiceId = ServiceId.slice(0, -1);

    var ClientId = "";
    $('.new-referral-agency-step1').find(".CheckClient1").map(function () {
        if ($(this).is(':checked'))
            ClientId += this.value + ",";
    });
    var HouseHoldId = parseInt($('.new-referral-agency-step1').find('.CheckClient1').attr('housholdid'));
    ClientId = ClientId.slice(0, -1);
    //var HouseHoldId = $('#HouseHoldId').val();


    if ($('.new-referral-agency-step1').find('#ClientID1:checked').val() == undefined && ($('.new-referral-agency-step1').find('#txtSearch').val() == "") && ($('.new-referral-agency-step1').find('#FFReferralSelect').val() == 0)) {

        $('.new-referral-agency-step1').find('#errshow').css("display", "inline-block");
        $('.new-referral-agency-step1').find('#errshow').text("Select Family Members");
        $('.new-referral-agency-step1').find('#errspan').css("display", "inline-block");
        $('.new-referral-agency-step1').find('#errspan').text("Enter Organization Name");
        $('.new-referral-agency-step1').find('#errnewspan').css("display", "inline-block");
        $('.new-referral-agency-step1').find('#errnewspan').text("Select Referral Type");
        return false;
    }

    else if ($('#ClientID1:checked').val() == undefined) {
        $('.new-referral-agency-step1').find('#errshow').css("display", "inline-block");
        $('.new-referral-agency-step1').find('#errshow').text("Select Family Members");
        return false;
    }
    else if ($('.new-referral-agency-step1').find('#txtSearch').val() == "") {
        $('.new-referral-agency-step1').find('#errshow').hide();
        $('.new-referral-agency-step1').find('#errspan').css("display", "inline-block");
        $('.new-referral-agency-step1').find('#errspan').text("Enter Organization Name");
        //$('#referralError').html('Please select organization name');
        //$('#referralCompanyModal').modal('show');
        return false;
    }
        //else if ($('#FFReferral').hasClass('hidden')) {
    else if ($('.new-referral-agency-step1').find('#FFReferralSelect').val() == 0) {
        $('.new-referral-agency-step1').find('#errspan').hide();
        $('.new-referral-agency-step1').find('#errnewspan').css("display", "inline-block");
        $('.new-referral-agency-step1').find('#errnewspan').text("Select Referral Type");
        //$('#referralCompanyModal').modal('show');
        return false;
    }
        //    }

    else if ($('.new-referral-agency-step1').find('#datepicker1').val() == "") {
        $('.new-referral-agency-step1').find('#errshow').hide();
        $('.new-referral-agency-step1').find('#errspan').hide();
        $('.new-referral-agency-step1').find('#errnewspan').hide();
        $('.new-referral-agency-step1').find('#errdate').css("display", "inline-block");
        $('.new-referral-agency-step1').find('#errdate').text("Please Enter referral date");
        // $('#referralCompanyModal').modal('show');
        return false;
    }
    else if (!isDate($('.new-referral-agency-step1').find('#datepicker1').val().trim())) {
        $('.new-referral-agency-step1').find('#errshow').hide();
        $('.new-referral-agency-step1').find('#errspan').hide();
        $('.new-referral-agency-step1').find('#errnewspan').hide();
        $('.new-referral-agency-step1').find('#errdate').css("display", "inline-block");
        $('.new-referral-agency-step1').find('#errdate').text("Please Enter Valid date");
        // $('#referralCompanyModal').modal('show');
        return false;

    }

    var ReferralDate = $('.new-referral-agency-step1').find('#datepicker1').val().trim();
    var Description = $('.new-referral-agency-step1').find('#Description').val().trim();
    var ref = null;
    var commId = $('#communityId').val();
    var AgencyId = $('#AgencyId_').val();

    if ($('.new-referral-agency-step1').find('#FFReferralSelect').hasClass('hidden')) {

        serviceId = $('#FFReferral').attr('referralId');
    }
    else {
        serviceId = $('#FFReferralSelect').val();
    }

    var ReferralClientServiceId = 0;

    //var path = $('#MyURL').val();

    var AddReferral = {};
    AddReferral.ReferralDate = ReferralDate;
    AddReferral.Description = Description;
    AddReferral.ServiceResourceId = parseInt(serviceId);
    AddReferral.AgencyId = agencyId;
    AddReferral.CommunityId = parseInt(commId);
    AddReferral.ReferralClientServiceId = parseInt(ReferralClientServiceId);
    AddReferral.ClientId = ClientId;
    AddReferral.HouseHoldId = HouseHoldId;
    AddReferral.CommonClientId = clientId;

    $.ajax({
        url: "/Roster/SaveReferral",
        type: "POST",
        data: AddReferral,
        success: function (data) {
            if (data = true) {
                customAlert('Record saved successfully');
                window.setTimeout(function () {
                    $('#matrix_referalpopup').modal('hide');
                    $('#matrix_recommendations').modal('show');

                }, 1000);


                //  window.location.href = "@Url.Action("ReferralService", "Roster", new { id = ViewBag.Id, clientName = ViewBag.ClientName })";
                //window.location.href = path;
            }
        }
    });

});

$('#referralLetterPdf').on('click', function () {
    if (serviceCount > 1) {
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
    else if (!isDate($('#datepicker').val().trim())) {
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
    //if ($('#_AgencyId').val() == "") {
    //    $('#_AgencyId').val($('#AgencyId').val());
    //}

    //if ($('#_CommunityId').val() == "0" || $('#_CommunityId').val() == "") {
    //    var communityId = $('#CommunityId').val();
    //}
    //else {
    //    var communityId = $('#_CommunityId').val();
    //}

    if ($('.new-referral-step2').find('#ddFsOrganization').hasClass('hidden')) {
        communityId = $('#OrganizationId').val();
    }
    else {
        communityId = $('.new-referral-step2').find('#ddFsOrganization').val();
    }
    if ($('.new-referral-step2').find('#FSResourcesSelect').hasClass('hidden')) {
        serviceId = $('.new-referral-step2').find('#FSResources').val();
    }
    else {
        serviceId = $('.new-referral-step2').find('#FSResourcesSelect').val();
    }

    var Refnotes = $('.new-referral-step2').find('#Description').val();
    window.location.href = "/Roster/CompleteServicePdf?ServiceId=" + parseInt($('#FSResources').val().trim()) + "&AgencyID=" + agencyId + "&ClientID=" + clientId + "&CommunityID=" + communityId + "&Notes=" + Refnotes + "&referralDate=" + $('#datepicker').val();
});


$('#btnpdf').on('click', function () {

    if (($('.new-referral-agency-step1').find('#txtSearch').val() == "") && ($('.new-referral-agency-step1').find('#FFReferralSelect').val() == 0)) {
        $('.new-referral-agency-step1').find('#errshow').hide();
        $('.new-referral-agency-step1').find('#errshow').text("");
        $('.new-referral-agency-step1').find('#errspan').css("display", "inline");
        $('.new-referral-agency-step1').find('#errspan').text("Enter Organization Name");
        $('.new-referral-agency-step1').find('#errnewspan').css("display", "inline");
        $('.new-referral-agency-step1').find('#errnewspan').text("Select Referral Type");
        return false;
    }
    else if ($('.new-referral-agency-step1').find('#txtSearch').val() == "") {
        $('.new-referral-agency-step1').find('#errshow').hide();
        $('.new-referral-agency-step1').find('#errshow').text("");
        $('.new-referral-agency-step1').find('#errspan').css("display", "inline");
        $('.new-referral-agency-step1').find('#errspan').text("Enter Organization Name");
        //$('#referralError').html('Please select organization name');
        //$('#referralCompanyModal').modal('show');
        return false;
    }
        //else if ($('#FFReferral').hasClass('hidden')) {
    else if ($('.new-referral-agency-step1').find('#FFReferralSelect').val() == 0) {
        $('.new-referral-agency-step1').find('#errshow').hide();
        $('.new-referral-agency-step1').find('#errshow').text("");
        $('.new-referral-agency-step1').find('#errspan').hide();
        $('.new-referral-agency-step1').find('#errnewspan').css("display", "inline");
        $('.new-referral-agency-step1').find('#errnewspan').text("Select Referral Type");
        //$('#referralCompanyModal').modal('show');
        return false;
    }
        //    }
    else if ($('.new-referral-agency-step1').find('#datepicker1').val() == "") {
        $('.new-referral-agency-step1').find('#errshow').hide();
        $('.new-referral-agency-step1').find('#errshow').text("");
        $('.new-referral-agency-step1').find('#errspan').hide();
        $('.new-referral-agency-step1').find('#errnewspan').hide();
        $('.new-referral-agency-step1').find('#errdate').css("display", "inline");
        $('.new-referral-agency-step1').find('#errdate').text("Please Enter referral date");
        // $('#referralCompanyModal').modal('show');
        return false;
    }

    else if (!isDate($('.new-referral-agency-step1').find('#datepicker1').val())) {
        $('.new-referral-agency-step1').find('#errshow').hide();
        $('.new-referral-agency-step1').find('#errshow').text("");
        $('.new-referral-agency-step1').find('#errspan').hide();
        $('.new-referral-agency-step1').find('#errnewspan').hide();
        $('.new-referral-agency-step1').find('#errdate').css("display", "inline");
        $('.new-referral-agency-step1').find('#errdate').text("Please Enter Valid date");
        return false;
    }

    $('.new-referral-agency-step1').find('#errshow').hide();
    $('.new-referral-agency-step1').find('#errshow').text("");
    $('.new-referral-agency-step1').find('#errnewspan').hide();
    $('.new-referral-agency-step1').find('#errnewspan').text("");
    $('.new-referral-agency-step1').find('#errdate').hide();
    $('.new-referral-agency-step1').find('#errdate').text("");
    $('.new-referral-agency-step1').find('#errspan').hide();
    $('.new-referral-agency-step1').find('#errspan').text("");
    communityId = $('#communityId').val();

    if ($('#FFReferralSelect').hasClass('hidden')) {
        serviceId = $('#FFReferral').attr('referralId');
    }
    else {
        serviceId = $('#FFReferralSelect').val();
    }

    //var ServiceIdPDF = $('#ReferralId').val();
    //var AgencyId = $('#AgencyId_').val();
    //var clientId = $('#encryptId').val();
    var agennotes = $('.new-referral-agency-step1').find('#Description').val().trim();
    var referraldate = $('.new-referral-agency-step1').find('#datepicker1').val();
    window.location.href = "/Roster/CompleteServicePdf?ServiceId=" + serviceId + "&AgencyID=" + agencyId + "&ClientID=" + clientId + "&CommunityID=" + communityId + "&Notes=" + agennotes + "&referralDate=" + referraldate;
});



$('.new-referral-step2').on('click', '#spnOrganizationName', function () {

    if ($('.new-referral-step2').find('#ddFsOrganization').hasClass('hidden')) {
        communityId = $('.new-referral-step2').find('#OrganizationId').val();
    }
    else {
        communityId = $('.new-referral-step2').find('#ddFsOrganization').val();
    }
    if ($('.new-referral-step2').find('#FSResourcesSelect').hasClass('hidden')) {
        serviceId = $('.new-referral-step2').find('#FSResources').val();
    }
    else {
        serviceId = $('.new-referral-step2').find('#FSResourcesSelect').val();
    }
    $.ajax({
        url: "/Roster/GetBusinessHours",
        datatype: "application/json",
        async: false,
        data: {
            ServiceId: serviceId, AgencyID: agencyId, CommunityID: communityId
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
            $('#refferedtomodal').html($('.new-referral-step2').find('#spnOrganizationName').text());
            $('#streetModal').html($('.new-referral-step2').find('#SpnCommunityAddress').text().split(',')[0] + ',');
            $('#cityModal').html($('.new-referral-step2').find('#SpnCommunityCity').text().split(',')[0]);
            $('#stateModal').html($('.new-referral-step2').find('#SpnCommunityState').text().split(',')[0] + ", " + $('.new-referral-step2').find('#SpnCommunityZipCode').text().split(',')[0]);
            $('#emailModal').html($('.new-referral-step2').find('#SpnCommunityEmail').text().split(',')[0]);
            $('#phonenoModal').html($('.new-referral-step2').find('#SpnCommunityPhone').text().split(',')[0]);
            $('#resourceNamemodal').html(clientName);
            if (serviceCount === 1) {
                $('#serviceModal').html($('.new-referral-step2').find('#servicesName').html());

            }
            else {
                $('#serviceModal').html($('.new-referral-step2').find('#FSResourcesSelect option:selected').text());

            }
            $('#monFrmHours').html(monFrom);
            $('#monToHours').html(monTo);
            $('#tueFrmHours').html(tueFrom);
            $('#tueToHours').html(tueTo);
            $('#wedFrmHours').html(wedFrom);

            $('#wedToHours').html(wedTo);
            $('#thuFrmHours').html(thuFrom);
            $('#thuToHours').html(thuTo);
            $('#friFrmHours').html(friFrom);
            $('#friToHours').html(friTo);
            $('#satFrmHours').html(satFrom);
            $('#satToHours').html(satTo);
            $('#sunFrmHours').html(sunFrom);
            $('#sunToHours').html(sunTo);

            $('#OrganizationInfoModal').modal('show');
        }

    });




});


$('.new-referral-step2').on('click', '#surveryRef', function () {

    //var referralclientId = $('#ReferralClientServiceId').val();

    $.ajax({
        url: "/Roster/LoadSurveyOptions",
        datatype: "application/json",
        async: false,
        data: { ReferralClientId: referralClientServiceId },
        success: function (data) {

            if (data.length > 0) {
                if (data[0].Answer == "") {
                    isUpdate = false;
                }
                if (data[0].Answer != "") {

                    isUpdate = true;
                }
                $('#surveyModalPopUp').find('textarea').val('');
                $('#surveyModalPopUp').find('textarea').html('');
                $('#surveyModalPopUp').find('input[type=radio]').attr('checked', '');
                $('#surveyModalPopUp').find('input[type=radio]').prop('checked', false);
                $('#surveyModalPopUp').find('.error').css('display', 'none');
                if (!isUpdate) {
                    var Convdate1 = new Date();
                    var convmonth1 = Convdate1.getMonth() + 1;
                    var convdate1 = Convdate1.getDate();
                    var month1 = (convmonth1 < 10) ? "0" + convmonth1 : convmonth1;
                    var date1 = (convdate1 < 10) ? "0" + convdate1 : convdate1;
                    var convertedDate1 = (month1 + '/' + date1 + '/' + Convdate1.getFullYear());
                    $('#datecompleted').val(convertedDate1);
                    $('#datecompleted').html(convertedDate1);
                }
                else {
                    if (data[0].CreatedDate != "") {
                        var Convdate = new Date(data[0].CreatedDate);
                        var convmonth = Convdate.getMonth() + 1;
                        var convdate = Convdate.getDate();
                        var month = (convmonth < 10) ? "0" + convmonth : convmonth;
                        var date = (convdate < 10) ? "0" + convdate : convdate;
                        var convertedDate = (month + '/' + date + '/' + Convdate.getFullYear());
                        $('#datecompleted').val(convertedDate);
                        $('#datecompleted').html(convertedDate);
                    }
                }


                for (var i = 0; i < data.length; i++) {
                    var num = i + 1;
                    $('#question' + num).html(data[i].QuestionsId + ". " + data[i].Questions);
                    $('#question' + num).attr('data-questionid', data[i].QuestionsId);
                    $('#question' + num).attr('data-answerid', data[i].AnswerId);


                    if (data[i].Answer != "") {

                        var value = data[i].Answer;
                        if (num == 6) {
                            if (isUpdate) {
                                $('#answerexp6').html(data[i].Answer);
                                $('#answerexp6').val(data[i].Answer);
                                $('#answerexp6').attr('disabled', false);
                            }
                            else {
                                $('#answerexp6').html(data[i].Answer);
                                $('#answerexp6').val(data[i].Answer);
                            }

                        }
                        else {

                            $("input[name=answerradio" + num + "][value='" + value + "']").attr('checked', 'checked');
                            $("input[name=answerradio" + num + "][value='" + value + "']").prop('checked', true);

                        }
                    }
                    if (num != 6) {
                        var explanation = (data[i].Explanation == "NULL") ? "" : data[i].Explanation;
                        if (isUpdate) {

                            $('#answerexp' + num).html(explanation);
                            $('#answerexp' + num).val(explanation);
                            if (explanation == "") {
                                $('#answerexp' + num).prop('disabled', true);
                            }
                            else {
                                $('#answerexp' + num).prop('disabled', false);
                            }

                        }
                        else {
                            $('#answerexp' + num).html(explanation);
                            $('#answerexp' + num).val(explanation);
                        }

                    }

                }
            }

        },
        error: function (data) {

        }
    });


    if (!isUpdate) {

        $('input[type=radio]').prop('checked', false)
        $('#answerexp1').val("");
        $('#answerexp2').val("");
        $('#answerexp3').val("");
        $('#answerexp4').val("");
        $('#answerexp5').val("");
        $('#answerexp6').val("");

    }


    $('#Name').text(clientName);
    if (serviceCount == 1) {
        $('#OrganName').text($('.new-referral-step2').find('#ddFsOrganization').html());
        $('#ServicesName').text($('.new-referral-step2').find('#servicesName').html());

    }
    else {
        $('#OrganName').text($('.new-referral-step2').find('#ddFsOrganization option:selected').text());
        $('#ServicesName').text($('.new-referral-step2').find('#FSResourcesSelect option:selected').text());
    }
    $('#ReferralDate').text($('.new-referral-step2').find('#datepicker').val());
    $('#surveyModalPopUp').modal('show');
});

$('#SaveSurvey').click(function () {

    $('#surveyModalPopUp').find('.error').removeClass('hidden');

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
        if (isUpdate) {
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

        $.ajax({

            url: "/Roster/InsertSurveyOptions",
            datatype: "application/json",
            type: "POST",
            data: { surveyoptions: jsonData, ReferralClientId: referralClientServiceId, isUpdate: update },
            success: function (data) {
                if (data) {

                    customAlert('Record saved successfully');
                    $('#surveyModalPopUp').modal('hide');
                }
            },
            error: function (data) {

            }

        });
    }
});

$('#surveyModalPopUp').on('hidden.bs.modal', function () {
    //location.reload();

})

var count = 0;
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

function PrintModal() {
    $('#surveyModalPopUp').children('div').find('.modal-dialog').removeClass('modal-width-change');
    $('#surveyModalPopUp').children('div').find('.modal-body').removeClass('modal-height-change');

    var contents = $('#surveyModalPopUp').children('div').find(".modal-body").html();
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
    $('#surveyModalPopUp').find('.modal-dialog').addClass('modal-width-change');
    $('#surveyModalPopUp').find('.modal-body').addClass('modal-height-change');
    $('#surveyModalPopUp').find('.check_btn').removeClass('hidden');
    $('#surveyModalPopUp').find('#datecompleted').removeClass('hidden');
    $('#surveyModalPopUp').find('#datecompletespan').addClass('hidden');
    $('#surveyModalPopUp').find('.radio-inline,.no-print').removeClass('hidden');
    $('.radio-inline,.no-print').removeClass('hidden');
    $('#surveyModalPopUp').find('.print-answer').addClass('hidden');

};


$('#surveyModalPopUp').find('#Matchprovider').click(function () {
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
    $('#surveyModalPopUp').find('.radio-inline,.no-print').addClass('hidden');
    $('#surveyModalPopUp').find('.check_btn').addClass('hidden');
    $('#surveyModalPopUp').find('.print-answer').removeClass('hidden');
    $('#surveyModalPopUp').find('.error').addClass('hidden');
    PrintModal();
});

$('input[type=radio]').change(function () {


    if ((!isUpdate) && count == 0) {


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


    if ((isUpdate) && count != 0) {


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


    if ((isUpdate) && count == 0) {

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

    if ((isUpdate) && count == 0) {
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

$('#matrix_referalpopup').on('click', '.close', function () {

    $('#matrix_recommendations').modal('show');
});


$('.referalservice').on('click', '#deleteservice', function () {
    var refId = $(this).attr('refid');


    $('.DeleteService').attr('refID', refId);
});
$(".DeleteService").click(function () {
    var ReferralClientServiceId = parseInt($(this).attr('refID'));
    $.ajax({
        url: "/Roster/DeleteReferralService",
        type: "POST",
        data: {
            ReferralClientServiceId: ReferralClientServiceId
        },
        success: function (data) {
            if (data) {
                $('#deleteServicePopUp').modal('hide');
                $('.remove_' + ReferralClientServiceId).remove();
                customAlert('Record deleted successfully');
            }
        }
    });

});


///FPA/////

$('#fbsSaveBtn').click(function () {
    if (!validateFPAPopup()) {
        return false;
    }

    var array = [];
    if (round > 1) {
        $('.Steps').each(function () {

            if ($(this).find('#Description').val().trim() != '') {
                var module = {
                    'Description': $(this).find('#Description').val().trim(),
                    'Status': $(this).find('#Status').val(),
                    'StepsCompletionDate': $(this).find('#StepsCompletionDate').val().trim(),
                    'Reminderdays': ($(this).find('#dfReminder').val() == '' || $(this).find('#dfReminder').val() == null) ? 0 : parseInt($(this).find('#dfReminder').val().trim()),
                };
            }
            array.push(module);
        });
    }
    else {
        if ($('.Steps').find('#Description').val().trim() != '') {
            var module = {
                'Description': $('.Steps').find('#Description').val().trim(),
                'Status': $('.Steps').find('#Status').val(),
                'StepsCompletionDate': $('.Steps').find('#StepsCompletionDate').val().trim(),
                'Reminderdays': ($('.Steps').find('#dfReminder').val() == null || $('.Steps').find('#dfReminder').val() == '') ? 0 : parseInt($('.Steps').find('#dfReminder').val().trim()),
            };
            array.push(module);
        }
    }
    var fPaModel = {
        'FPAID': 0,
        'Goal': $('#Goal').val().trim(),
        'GoalDate': $('#GoalDate').val().trim(),
        'CompletionDate': $('#CompletionDate').val().trim(),
        'Element': null,
        'Domain': null,
        'Category': $('#DdlCateList').val(),
        'GoalStatus': $('#GoalStatus').val().trim(),
        'GoalFor': (parseInt(goalFor) == 0) ? 1 : parseInt(goalFor),
        'GoalSteps': array,
        'SignatureData': $('#imgCapture').val()

    };
    var stringarray = JSON.stringify(fPaModel);
    $.ajax({

        url: '/Roster/SaveFbaForParentMatrix',
        datatype: 'json',
        type: 'post',
        data: { ClientId: clientId, infoString: stringarray },
        success: function (data) {
            customAlert(data);
            $('#matrix_fbaPopup').modal('hide');
        }
    })


});
$('#matrix_fbaPopup').find('#monthText').on('blur', function () {
    getmonthnew();
});

$('#matrix_fbaPopup').find('#weekText').on('blur', function () {
    getweeknew();
});

function validate(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /[0-9]|\./;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}

function onchangeevent() {

    var maxndate = new Date($('#CompletionDate').val());
    $('#StepsCompletionDate').datetimepicker({
        timepicker: false,
        maxDate: maxndate,
        format: 'm/d/Y',
        validateOnBlur: false
    });

}

function getweeknew() {
    var x = document.getElementById("weekText").value;
    if (x != "") {
        document.getElementById("monthText").value = "";
        var t = new Date();
        t.setDate(t.getDate() + 7 * (x));
      
        var cmpdate = t.getMonth() + 1 + "/" + t.getDate() + "/" + t.getFullYear();
        document.getElementById("CompletionDate").value = cmpdate;
        var maxDate = new Date(cmpdate);
        $('.Steps').each(function (i) {
            $('input[name="GoalSteps[' + i + '].StepsCompletionDate"]').mask("99/99/9999", { placeholder: 'MM/DD/YYYY' });
            $('input[name="GoalSteps[' + i + '].StepsCompletionDate"]').datetimepicker({
                timepicker: false,
                maxDate: maxDate,
                format: 'm/d/Y',
                scrollMonth: false,
                scrollInput: false,
                validateOnBlur: false
            });
        });

    }
    else {
        // document.getElementById("CompletionDate").value = "";
    }
}

function getmonthnew() {
    var x = document.getElementById("monthText").value;
    if (x != "") {
        document.getElementById("weekText").value = "";
        var t = new Date();
        t.setMonth(t.getMonth() + parseInt(x));
       
        var cmpdate = t.getMonth() + 1 + "/" + t.getDate() + "/" + t.getFullYear();
        document.getElementById("CompletionDate").value = cmpdate;
        var maxDate = new Date(cmpdate);
        $('.Steps').each(function (i) {
            $('input[name="GoalSteps[' + i + '].StepsCompletionDate"]').mask("99/99/9999", { placeholder: 'MM/DD/YYYY' });
            $('input[name="GoalSteps[' + i + '].StepsCompletionDate"]').datetimepicker({
                timepicker: false,
                maxDate: maxDate,
                format: 'm/d/Y',
                scrollMonth: false,
                scrollInput: false,
                validateOnBlur: false
            });
        });


    }
    else {
        //  document.getElementById("CompletionDate").value = "";
    }
}

function isabletosign() {

    var needtoshow = 0;
    $('.Steps').each(function (i) {
        if ($('input[name="GoalSteps[' + i + '].StepsCompletionDate"]').val().length > 0 && $('input[name="GoalSteps[' + i + '].Description"]').val().length > 0) {
            needtoshow = 1;
        }

    });

    if (needtoshow == 1) {
        $('.signatureDiv').show();
    }
    else {
        $('.signatureDiv').hide();
    }

}
function validateFPAPopup() {
    isValid = true;
    cleanValidation();
    $("#message").text('');
    if ($('#Goal')[0].value.trim() == "") {
        isValid = false;
        customAlert("Goal is required.");
        plainValidation('#Goal');
        return isValid;
    }
    else if ($('#DdlCateList')[0].value.trim() == 0) {
        isValid = false;
        customAlert("Category is required.");
        plainValidation('#DdlCateList');
        return isValid;
    }
    else if ($('#GoalDate')[0].value.trim() == "") {
        isValid = false;
        customAlert("Goal Date is required.");
        plainValidation('#GoalDate');
        return isValid;
    }

    else if ($('#CompletionDate')[0].value.trim() == "") {
        isValid = false;
        customAlert("Completion Date is required.");
        plainValidation('#CompletionDate');
        return isValid;
    }
    if (!$('.goal-for-parent').hasClass('hidden')) {
        if ($("#parent1")[0].checked == true || $("#parent2")[0].checked) {
        }
        else {
            isValid = false;
            customAlert("Atleast one parent is required.");
            plainValidation('#parent1');
            return isValid;
        }
    }
    $('.Steps').each(function (i) {
        if ($('input[name="GoalSteps[' + i + '].Description"]').val().trim() == "") {
            customAlert("Step description is required.");
            plainValidation($('input[name="GoalSteps[' + i + '].Description"]'));
            isValid = false;
            return isValid;
        }
        if ($('input[name="GoalSteps[' + i + '].StepsCompletionDate"]').val().trim() == "") {
            customAlert("Step completion date is required.");
            plainValidation($('input[name="GoalSteps[' + i + '].StepsCompletionDate"]'));
            isValid = false;
            return isValid;
        }
        else if ($('input[name="GoalSteps[' + i + '].StepsCompletionDate"]').val().trim().length > 0) {
            var maxdate = new Date($('#CompletionDate').val());
            var stepdate = new Date($('input[name="GoalSteps[' + i + '].StepsCompletionDate"]').val().trim());
            if (stepdate != "InvalidDate") {
                var diff = 0;
                if (maxdate - stepdate > 0) {

                }
                else {
                    customAlert("Step completion date should be less than goal completion Date.");
                    plainValidation($('input[name="GoalSteps[' + i + '].StepsCompletionDate"]'));
                    isValid = false;
                    return isValid;
                }
            }
        }


    });

    //$('.Steps').each(function (i) {
    //    if ($('input[name="GoalSteps[' + i + '].StepsCompletionDate"]').val().trim() == "") {
    //        customAlert("Step completion date is required.");
    //        plainValidation($('input[name="GoalSteps[' + i + '].StepsCompletionDate"]'));
    //        isValid = false;
    //        return isValid;
    //    }
    //});


    return isValid;
}
function AddSteps() {
    var numItems = $('.Steps').length;

    for (var i = round; i < numItems; i++) {

        $('.Steps #Description')[i].name = 'GoalSteps[' + i + '].Description';
        $('.Steps #dfReminder')[i].name = 'GoalSteps[' + i + '].Reminderdays';
        $('.Steps #Status')[i].name = 'GoalSteps[' + i + '].Status';
        $('.Steps #StepsCompletionDate')[i].name = 'GoalSteps[' + i + '].StepsCompletionDate';
        if ($('.Steps #StepsID')[i]) {
            $('.Steps #StepsID')[i].name = 'GoalSteps[' + i + '].StepsID';
        }
    }

    round = numItems;
    var stepsdiv = '<div id="AddSteps' + round + '" class="row Steps"><div class="max_items_main2"><div class="col-xs-12"><div class="close-inside">' +
                                    '<a onclick="DeleteStepSecond(this,' + round + ');" style="cursor: pointer;"> <img src="/images/popup-div-close.png" /></a></div></div><div class="col-xs-12" style="padding: 0;"><div class="col-sm-6"><div class="family-poup-div"><label>Description<span>*</span></label>' +
                                        '<input class="form-control ui-autocomplete-input Description" name="GoalSteps[' + round + '].Description" id="Description" onblur="isabletosign()" type="text" placeholder="Description"></div></div><div class="col-sm-6"><div class="family-poup-div"><label>Status<span>*</span></label><div class="col-xs-12" style="padding: 0;position: relative;">' +
                                            '<select id="Status" name="GoalSteps[' + round + '].Status" class="form-control status"><option value="0">Open</option><option value="1">Complete</option><option value="2">Abandoned</option></select><div class="dwn"><img src="/images/dwn-arrow-popup.png"></div></div></div></div></div><div class="col-xs-12" style="padding: 0;"><div class="col-sm-6"><div class="family-poup-div"><label>Completion Date<span>*</span></label>' +
                                        '<input class="form-control ui-autocomplete-input StepsCompletionDate" name="GoalSteps[' + round + '].StepsCompletionDate" type="text" id="StepsCompletionDate" placeholder="MM/DD/YYYY" readonly style="background:#fff;">' +
                                        '<input id="StepsID" type="hidden" name="GoalSteps[' + round + '].StepsID"></div></div><div class="col-sm-6"><div class="family-poup-div"><label>Days Prior To Remind Parent</label>' +
                                        '<input name="GoalSteps[' + round + '].Reminderdays" type="number" onkeydown="setmxlength(this)" onkeypress="validate(event)" min="0" class="form-control Description" id="dfReminder" placeholder="Days for reminder" maxlength="2"></div></div></div></div></div>'


    $(stepsdiv).insertAfter('#addStepsDiv');

    var maxndate = new Date($('#CompletionDate').val());
    var mindate = new Date();
    $('.StepsCompletionDate').datetimepicker({
        timepicker: false,
        maxDate: maxndate,
        minDate: mindate,
        format: 'm/d/Y',
        scrollMonth: false,
        scrollInput: false,
        validateOnBlur: false
    });

    round = round + 1;
}

function DeleteStep(value) {
    if ($('.Steps').length > 1) {
        DeleteStepSecond(value, 0);
    }
    else {
        $('input[name="GoalSteps[0].Description"]').val("");
        $('input[name="GoalSteps[0].Status"]').val("0");
        var currentdate = new Date();
        $('input[name="GoalSteps[0].Reminderdays"]').val("");
        var maxndate = new Date($('#CompletionDate').val());
        $('input[name="GoalSteps[0].StepsCompletionDate"]').val("");
        $('input[name="GoalSteps[0].StepsCompletionDate"]').datetimepicker({
            timepicker: false,
            maxDate: maxndate,
            format: 'm/d/Y',
            validateOnBlur: false
        });   // depends on only goal-- CompletionDate

    }
}

function DeleteStepSecond(value, round) {
    if ($('.Steps').length == 1) {
        $('input[name="GoalSteps[' + round + '].Description"]').val("");
        $('input[name="GoalSteps[' + round + '].Status"]').val("0");
        var currentdate = new Date();
        $('input[name="GoalSteps[' + round + '].Reminderdays"]').val("");
        var maxndate = new Date($('#CompletionDate').val());
        $('input[name="GoalSteps[' + round + '].StepsCompletionDate"]').val("");
        $('input[name="GoalSteps[' + round + '].StepsCompletionDate"]').datetimepicker({
            timepicker: false,
            maxDate: maxndate,
            format: 'm/d/Y',
            validateOnBlur: false
        });
    } else {
        var divclass = 'AddSteps' + round;
        $("#" + divclass).remove();
        var s = $('.Steps').length;
        for (var i = 0; i < s; i++) {
            $('.Steps a')[i].attributes.onclick.nodeValue = "DeleteStepSecond(this, " + i + ")";
            $('.Steps #Description')[i].name = 'GoalSteps[' + i + '].Description';
            $('.Steps #dfReminder')[i].name = 'GoalSteps[' + i + '].Reminderdays';
            $('.Steps #Status')[i].name = 'GoalSteps[' + i + '].Status';
            $('.Steps #StepsCompletionDate')[i].name = 'GoalSteps[' + i + '].StepsCompletionDate';
            if ($('.Steps #StepsID')[i]) {
                $('.Steps #StepsID')[i].name = 'GoalSteps[' + i + '].StepsID';
            }
            //        $(this).find('[name^="StepsID"]').attr("name", "GoalSteps[" + i + "].StepsID");
            //    }
        }
    }
}

function validate(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /[0-9]|\./;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}

function setmxlength(e) {

    $(".Steps").each(function (i) {

        if ($('input[name="GoalSteps[' + i + '].Reminderdays"]').val().length > 0) {
            var val = $('input[name="GoalSteps[' + i + '].Reminderdays"]').val().substring(0, 2);
            val = val.replace('e', '');
            val = val.replace('/\+/g', '');
            $('input[name="GoalSteps[' + i + '].Reminderdays"]').val(val);
        }

    });
}

$('#matrix_recommendations').on('click', '#fpaYes', function () {
    getFPAForMatrix();
});
function getFPAForMatrix() {



    $.ajax({
        url: '/Roster/GetFPAForMatrix',
        datatype: 'json',
        type: 'post',
        data: { clientId: clientId },
        success: function (data) {


            if (data != null) {

                if (data.IsSingleParent) {
                    $('#matrix_fbaPopup').find('.goal-for-parent').addClass('hidden');
                    singleParent = true;
                }
                else {
                    $('#matrix_fbaPopup').find('.goal-for-parent').removeClass('hidden');
                    $('#matrix_fbaPopup').find('.parentName1').text(data.ParentName1);
                    $('#matrix_fbaPopup').find('.parentName2').text(data.ParentName2);
                    singleParent = false;
                }
                var catOption = '<option value=0>Select Category</option>';
                if (data.cateList != null) {
                    if (data.cateList.length > 0) {

                        $.each(data.cateList, function (k, cateElement) {
                            catOption += '<option value=' + cateElement.Id + '>' + cateElement.Name + '</option>';
                        });
                        $('#matrix_fbaPopup').find('#DdlCateList').html(catOption);
                    }
                }
            }
            initializeFPAPopUp();


        }

    });
}


function initializeFPAPopUp() {

    $('#matrix_fbaPopup').find('.Steps').remove();
    var appendStepsDiv = $('.fpaSteps');
    appendStepsDiv.find('.row').addClass('Steps');
    var appendDiv = appendStepsDiv.html();
    $(appendDiv).insertAfter('#addStepsDiv');
    appendStepsDiv.find('.row').removeClass('Steps');
    $('#matrix_fbaPopup').find('.signatureDiv').hide();
    $('#matrix_fbaPopup').find('#imagedata').attr('src', '');
    $('#drawSignatureModal').find('#clearDrawing').trigger('click');
    //$('#matrix_fbaPopup').find('#colors_sketch').sketch();
    round = 1;

    var date = new Date();
    var Mindate = new Date().setDate(date.getDate() - 30)

    $("#GoalDate").datetimepicker({
        timepicker: false,
        minDate: Mindate,
        maxDate: new Date(),
        format: 'm/d/Y',
        scrollMonth: false,
        scrollInput: false,
        value: date,
        validateOnBlur: false,

    });


    $("#DynamicCompletionDate").datetimepicker({
        timepicker: false,
        minDate: Mindate,
        maxDate: new Date(),
        format: 'm/d/Y',
        scrollMonth: false,
        scrollInput: false,
        validateOnBlur: false
    });

    $('#StepsCompletionDate').datetimepicker(
        {
            timepicker: false,
            format: 'm/d/Y',
            scrollMonth: false,
            scrollInput: false,
            validateOnBlur: false,
            minDate: new Date(),
            maxDate: new Date()

        });


    var d = new Date(),
     date = (d.getUTCFullYear()) + '/' + (d.getUTCMonth() + 1) + '/' + (d.getUTCDate());
    var currentdate = new Date();
    var mindate = new Date(currentdate.setDate(currentdate.getDate() - 30));

    if ($("#CompletionDate").val()) {
        $("#CompletionDate").datetimepicker({
            timepicker: false,
            format: 'm/d/Y',
            minDate: mindate,
            scrollMonth: false,
            scrollInput: false,
            value: date,
            validateOnBlur: false

        });
    }
    else {
        $("#CompletionDate").datetimepicker({
            timepicker: false,
            format: 'm/d/Y',
            minDate: mindate,
            value: date,
            scrollMonth: false,
            scrollInput: false,
            validateOnBlur: false
        });
    }
    var maxndate = new Date($('#CompletionDate').val());
    var min = new Date();
    $('#Description').val('');
    $('#Status').val(0);
    $('#StepsCompletionDate').val('');
    $('#dfReminder').val('');
    $('#monthText').val('');
    $('#weekText').val('');
    $('#GoalStatus').val(0);
    $('#Goal').val('');


}

$('#matrix_fbaPopup').on('change', '#parent1,#parent2', function () {
    setGoalfor(this);
});

function setGoalfor(val) {

    var chk1;
    var chk2;
    if (singleParent) {
        goalFor = 1;
        return;
    }
    else {
        chk1 = $("#parent1")[0].checked;
        chk2 = $("#parent2")[0].checked;
    }
    if (chk1 && chk2) {

        goalFor = 3;
    }
    else if (chk1) {

        goalFor = 1;
    }
    else if (chk2) {

        goalFor = 2;

    }
    else {
        goalFor = 0;
    }

}

function getBrowserName() {
    // Opera 8.0+
    var isOpera = (!!window.opr && !!opr.addons) || !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0;

    // Firefox 1.0+
    var isFirefox = typeof InstallTrigger !== 'undefined';

    // Safari 3.0+ "[object HTMLElementConstructor]" 
    var isSafari = /constructor/i.test(window.HTMLElement) || (function (p) { return p.toString() === "[object SafariRemoteNotification]"; })(!window['safari'] || safari.pushNotification);

    // Internet Explorer 6-11
    var isIE = /*@cc_on!@*/false || !!document.documentMode;

    // Edge 20+
    var isEdge = !isIE && !!window.StyleMedia;

    // Chrome 1+
    var isChrome = !!window.chrome && !!window.chrome.webstore;

    // Blink engine detection
    var isBlink = (isChrome || isOpera) && !!window.CSS;

    var browserArray = {
        IsOpera: isOpera,
        IsFirefox: isFirefox,
        IsSafari: isSafari,
        IsIE: isIE,
        IsChrome: isChrome,
        IsBlink: isBlink
    };
    return browserArray;
}