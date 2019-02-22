$(document).ready(function () {

    var activeyear = '';
    GetMatrixSummary();
    changeHeight();
   
    function GetMatrixSummary()
    {
        $.ajax({
            url: "/AgencyUser/GetMatrixReport",
            datatype: "json",
            async: false,
            data:{Year:activeyear},
            success: function (data) {
                var charSectiondiv = $('.chart-block-div');
                var chartAppend_div = '';
                var groupSectiondiv = $('.group-block');
                var matrixValueSectiondiv = $('.matrix-value-section');
                var change_val_div = $('.change-value');
                var downarrow = '/images/dw-arw.png';
                var uparrow = '/images/up-arw.png';

                var group_sec_append = '';
                var mat_sec_append_1 = '';
                var mat_sec_append_2= '';
                var mat_sec_append_3 = '';
                var change_sec_append = '';
                var FullchartDiv = '';
                var categoryCount = 0;
                var assment1ratio = 0;
                var assment2ratio = 0;
                var assment3ratio = 0;
                var count1 = 0;
                var count2 = 0;
                var count3 = 0;
                var groupCount = 0;
                var as1Value = 0;
                var as2Value = 0;
                var as3Value = 0;
                var cateId = 0;
                var groupPosCount = 1;
                var maxMatrixValue = 0;

                var groupId = 0;
                var imageCount = 1;
                charSectiondiv.find('.cat-block').addClass('category-div');
                if (data.obj !== null)
                {
                    if (data.obj.length > 0)
                    {
                        categoryCount = data.obj.length;
                       
                        $.each(data.obj, function (i, element1) {
                            group_sec_append = '';
                            mat_sec_append_1 = '';
                            mat_sec_append_2 = '';
                            mat_sec_append_3 = '';
                            change_sec_append = '';

                             assment1ratio = 0;
                             assment2ratio = 0;
                             assment3ratio = 0;
                             count1 = 0;
                             count2 = 0;
                             count3 = 0;
                              as1Value = 0;
                              as2Value = 0;
                              as3Value = 0;
                              groupPosCount = 1;
                            if (element1 != null)
                            {
                                if(element1.length>0)
                                {

                                    $.each(element1, function (j, element2) {
                                        maxMatrixValue = element2.MaxMatrixValue;
                                        cateId = element2.AssessmentCategoryId;
                                        charSectiondiv.find('.cat-name').html(element2.Category).attr('cat-id', element2.AssessmentCategoryId);
                                        if (groupId != element2.AssessmentGroupId)
                                        {
                                            var isFirst = (i==0)?1:0;
                                            
                                            groupCount++;
                                            groupSectiondiv.find('.group-name').html(element2.AssessmentGroupType).parent('.education-content-desc').attr({ 'group-id': element2.AssessmentGroupId,'isfirst':isFirst});
                                            groupSectiondiv.find('.tooltiptext').html(element2.AssessmentGroupType);
                                            if (imageCount > 5)
                                            {
                                                imageCount = 1;
                                            }
                                            var imgsrc = '/images/3d-color' + imageCount + '.png';
                                            imageCount++;
                                            groupSectiondiv.find('.group-image').attr('src', imgsrc);
                                            //for popup//
                                            groupSectiondiv.find('.education-content-desc').attr({ 'group-pos': groupPosCount, 'cat-no': i, 'onclick': 'getDescriptionSummary(this);' });
                                           // $('.education-content-desc').attr('cat-no', i);

                                            $('.survey-3d').attr({ 'group-id': element2.AssessmentGroupId, 'cat-no': i, 'group-pos': groupPosCount, 'isfirst': isFirst, 'onclick': 'getQuestionSummary(this);' });

                                            // $('.survey-3d').attr('group-id', element2.AssessmentGroupId)
                                           // $('.survey-3d').attr('group-pos', groupPosCount);
                                            //for popup//

                                            groupPosCount++;
                                            group_sec_append += groupSectiondiv.html();
                                            groupId = element2.AssessmentGroupId;
                                           // change_sec_append += change_val_div.html();
                                            count1++;
                                        }
                                        
                                        if (element2.TotalRatio == 0)
                                        {
                                            matrixValueSectiondiv.find('.mat-value').html('-');

                                        }
                                        else
                                        {
                                            matrixValueSectiondiv.find('.mat-value').html(element2.TotalRatio);

                                        }

                                        if (element2.AssessmentNumber > 0) {
                                            if (element2.AssessmentNumber == 1) {
                                                
                                                assment1ratio +=  element2.TotalRatio;
                                                mat_sec_append_1 += matrixValueSectiondiv.html();
                                            }
                                            if (element2.AssessmentNumber == 2) {
                                               // count2 ++;
                                                assment2ratio +=  element2.TotalRatio;
                                                    mat_sec_append_2 += matrixValueSectiondiv.html();
                                                }
                                            if (element2.AssessmentNumber == 3) {
                                              //  count3 ++;
                                                assment3ratio +=  element2.TotalRatio;
                                                    mat_sec_append_3 += matrixValueSectiondiv.html();
                                                }
                                        }
                                        else
                                        {
                                            mat_sec_append_1 += matrixValueSectiondiv.html();
                                            mat_sec_append_2 += matrixValueSectiondiv.html();
                                            mat_sec_append_3 += matrixValueSectiondiv.html();

                                        }
                                       
                                    });

                                    $.each(data.percentageList, function (per, percentelement) {

                                        if (percentelement.AssessmentCategoryId == cateId)
                                        {
                                            var appendText = '';
                                            if (percentelement.ArrowType == null || percentelement.ArrowType == '')
                                            {
                                                appendText ='-';
                                            }
                                            else
                                            {
                                                appendText = percentelement.ChangePercent.toFixed(1) + '% ' + '<span><img src=' + percentelement.ArrowType + '></span>';
                                            }
                                        
                                            change_val_div.find('p').html(appendText).css('color', percentelement.FontColor);
                                            change_sec_append += change_val_div.html();
                                        }
                                    })

                                    var chartbinddiv = charSectiondiv;

                                    //Popup//
                                    var minus = i - 1;
                                    chartbinddiv.find('.div-group-summary').removeClass('div-group-' + minus);
                                    chartbinddiv.find('.div-question-summary').removeClass('div-question-' + minus);
                                    chartbinddiv.find('.div-group-summary').addClass('div-group-' + i);
                                    chartbinddiv.find('.div-question-summary').addClass('div-question-' + i);
                                    //popup//

                                    chartbinddiv.find('.group-section').html(group_sec_append);
                                    chartbinddiv.find('.as1-matrix-value-block').html(mat_sec_append_1);
                                    chartbinddiv.find('.as2-matrix-value-block').html(mat_sec_append_2);
                                    chartbinddiv.find('.as3-matrix-value-block').html(mat_sec_append_3);
                                    chartbinddiv.find('.chg-per-block').html(change_sec_append);
                                    if (i > 0)
                                    {
                                        chartbinddiv.find('.matrix-header-title,.matrix-header-title2').addClass('hidden').addClass('assessments');
                                    }
                                    else
                                    {
                                        chartbinddiv.find('.matrix-header-title, .matrix-header-title2').removeClass('hidden').addClass('assessments1');
                                    }
                                    var ratio1 = parseFloat(assment1ratio / count1).toFixed(1);
                                    var ratio2 = parseFloat(assment2ratio / count1).toFixed(1);
                                    var ratio3 = parseFloat(assment3ratio / count1).toFixed(1);
                                  
                                    var bar1percentage = parseFloat((ratio1 / maxMatrixValue) * 100).toFixed(1);
                                    var bar2percentage = parseFloat((ratio2 / maxMatrixValue) * 100).toFixed(1);
                                    var bar3percentage = parseFloat((ratio3 / maxMatrixValue) * 100).toFixed(1);
                                  
                                    
                                    

                                    chartbinddiv.find('.bar-label_1').children('p').html( parseInt(ratio1)==ratio1?parseInt(ratio1):ratio1);
                                    chartbinddiv.find('.bar-label_2').children('p').html(parseInt(ratio2)==ratio2?parseInt(ratio2):ratio2);
                                    chartbinddiv.find('.bar-label_3').children('p').html(parseInt(ratio3)==ratio3?parseInt(ratio3):ratio3);
                                  
                                 
                                   
                                    
                                    if (parseInt(bar1percentage) > 0)
                                    {
                                        var label1ht = getLabelHeight2(bar1percentage);

                                       // var label1Font = (parseInt(bar1percentage) < 6) ? '10px' : '14px';
                                        chartbinddiv.find('.bar_green1').css('height', bar1percentage + '%');
                                        chartbinddiv.find('.bar_green1').removeClass('hidden');
                                        chartbinddiv.find('.bar-label_1').css({ 'height': label1ht + '%'});

                                    }
                                    else
                                    {
                                        chartbinddiv.find('.bar-label_1').children('p').html(0);
                                        chartbinddiv.find('.bar_green1').addClass('hidden');
                                        chartbinddiv.find('.bar_green1').css('height','0%');
                                        chartbinddiv.find('.bar-label_1').css('height', '18%');
                                    }

                                    if (parseInt(bar2percentage) > 0) {
                                        
                                        chartbinddiv.find('.bar_green2').css('height', bar2percentage + '%');
                                        
                                        chartbinddiv.find('.bar_green2').removeClass('hidden');
                                        var label2ht = getLabelHeight2(bar2percentage);
                                      //  var label2Font = (parseInt(bar2percentage) < 6) ? '10px' : '14px';
                                        chartbinddiv.find('.bar-label_2').css({ 'height': label2ht + '%'});

                                    }
                                    else {
                                        chartbinddiv.find('.bar-label_2').children('p').html(0);
                                        chartbinddiv.find('.bar_green2').addClass('hidden');
                                        chartbinddiv.find('.bar_green2').css('height', '0%');
                                        chartbinddiv.find('.bar-label_2').css('height', '18%');
                                    }
                                    if (parseInt(bar3percentage) > 0) {
                                        
                                        var label3ht = getLabelHeight2(bar3percentage);
                                       // var label3Font = (parseInt(bar3percentage) < 6) ? '10px' : '14px';
                                        chartbinddiv.find('.bar_green3').css({ 'height': bar3percentage + '%'});
                                        chartbinddiv.find('.bar_green3').removeClass('hidden');
                                        chartbinddiv.find('.bar-label_3').css('height', label3ht + '%');

                                    }
                                    else {
                                        chartbinddiv.find('.bar-label_3').children('p').html(0);
                                        chartbinddiv.find('.bar_green3').addClass('hidden');
                                        chartbinddiv.find('.bar_green3').css('height', '0%');
                                        chartbinddiv.find('.bar-label_3').css('height', '18%');
                                    }
                                    var bar_minus = i - 1;
                                   // chartbinddiv.find('.survey-3d-green').removeClass('change-bar-div_' + bar_minus);
                                    chartbinddiv.find('.survey-3d-green').addClass('change-bar-div_' + i);
                                    chartbinddiv.find('.category-div').attr('cat-id' , i);

                                    FullchartDiv += chartbinddiv.html();
                                    chartbinddiv.find('.survey-3d-green').removeClass('change-bar-div_' + i);
                                }
                            }
                           
                        });
                        

                        $('.chart-report').html(FullchartDiv);
                        // $('.block-div').css('margin-top', '30px');
                        $('.chart-report').find('.bar-label_1').addClass('barlable_as1');
                        $('.chart-report').find('.bar-label_2').addClass('barlable_as2');
                        $('.chart-report').find('.bar-label_3').addClass('barlable_as3');

                        charSectiondiv.find('.cat-block').removeClass('category-div');
                    }
                }

                if(data.summarylist!=null)
                {
                    if(data.summarylist.length>0)
                    {
                       
                        $.each(data.summarylist, function (k, element4) {
                            var assmentNo = element4.AssessmentNumberMaster;
                            var percentageFam = element4.PercentFamilyEntered;
                            var masterRatio = 0;
                           
                            $('.as' + assmentNo + '-per').html('(' + parseInt(percentageFam)==percentageFam?parseInt(percentageFam):percentageFam + '%)');
                            $('.barlable_as' + assmentNo).each(function () {
                                masterRatio += parseFloat($(this).children('p').html());
                            });
                        
                            var as1Percentage = parseFloat(masterRatio / categoryCount).toFixed(1);
                       
                            //var totalGroupCount = groupCount;
                            //var converteddenom = (totalGroupCount / categoryCount);
                            //var convertedratio = (100 / converteddenom);

                            //if (as1Percentage > 0)
                            if (percentageFam > 0 && as1Percentage>0)
                            {
                                // var height1 = (as1Percentage * convertedratio);
                                var height1 = percentageFam;
                                if (height1 === 100) {
                                    $('.mastbar-green' + assmentNo).css('bottom', '-1px');
                                }
                                else if (height1 === 0) {
                                    $('.mastbar-green' + assmentNo).addClass('hidden');
                                }
                                else {
                                    $('.mastbar-green' + assmentNo).removeClass('hidden');

                                }
                                $('.mastbar-green' + assmentNo).height(height1 + '%');
                                $('.as' + assmentNo + '-avg').html(parseInt(as1Percentage)==as1Percentage?parseInt(as1Percentage):as1Percentage + '<sub>Avg</sub>')
                               
                            }   
                            else
                            {
                                $('.as' + assmentNo + '-avg').html('0<sub>Avg</sub>');
                                $('.mastbar-green' + assmentNo).height('0%');
                                $('.mastbar-green' + assmentNo).addClass('hidden');
                            }
                         
                        });
                    }
                }

                activeyear = data.programYear;
                $('#yearSelect').val(activeyear);
            }
        });
    }



    //$.ajax({
    //    url: "/AgencyUser/GetActiveYear",
    //    datatype: "json",
    //    async: false,
    //    success: function (data) {
    //        var selectedAppend = '';
    //        if (data.length > 0) {
    //            $.each(data, function (i, element) {

    //                selectedAppend += '<option value=' + element.Text + '>20' + element.Text + '</option>'
    //            });
    //            $('#yearSelect').append(selectedAppend);
    //            $('#yearSelect').val(activeyear);
    //        }
    //    }
         
    //});


    //function to get the Label Height based on the Chart height//
    function getLabelHeight(lblHeight) {

        var heightLbl = 0;

        if (lblHeight == 100) {

            heightLbl = 96;
        }
        else if (lblHeight >= 90 && lblHeight < 100) {
            heightLbl = 87;

        }
        else if (lblHeight >= 80 && lblHeight < 90) {
            heightLbl = 78;
        }
        else if (lblHeight >= 70 && lblHeight < 80) {
            heightLbl = 69;

        }
        else if (lblHeight >= 60 && lblHeight < 70) {
            heightLbl = 60;

        }
        else if (lblHeight >= 50 && lblHeight < 60) {
            heightLbl = 51;

        }
        else if (lblHeight >= 40 && lblHeight < 50) {
            heightLbl = 42;

        }
        else if (lblHeight >= 30 && lblHeight < 40) {
            heightLbl = 34;

        }
        else if (lblHeight >= 20 && lblHeight < 30) {
            heightLbl = 25;

        }
        else if (lblHeight >= 10 && lblHeight < 20) {
            heightLbl = 18;

        }
        else if (lblHeight >= 5 && lblHeight < 10) {
            heightLbl = 18;

        }
        else {
            heightLbl = 18;

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
            86: 82, 87: 84, 88: 84, 89: 86, 90: 86, 91: 87, 92: 88, 93: 89, 94: 89, 95: 90, 96: 90, 97: 92, 98: 92, 99: 94, 100: 94

        };
        var roundValue = Math.round(Height2);
        var heightarr = heightArray[roundValue];
        return heightarr;
    }

    function changeHeight() {
        $('.category-div').each(function () {
            var selfheight = parseInt($(this).find('.survey-block').css('height'));
            var textheight = parseInt($(this).find('.survey-text').css('height'));
            var green_bar = 0;
            var value = 0;
            if (selfheight > textheight) {
                $(this).find('.change-div').css('height', selfheight + 'px');
                value = $(this).attr('cat-id');

                green_bar = parseInt(selfheight - 40);
                if (selfheight > 260)
                {

              
                $('.change-bar-div_' + value).css('height', green_bar + 'px');
                }
            }
            else {
                $(this).find('.change-div').css('height', textheight + 'px');
                value = $(this).attr('cat-id');
                green_bar = parseInt(textheight - 40);
                if (textheight > 260)
                {

              
                $('.change-bar-div_' + value).css('height', green_bar + 'px');
                }
            }
        });
    }



    //On click over the Close icon of the Pop-up//
    $('body').on('click', '.close-div', function () {
        $('.popup-display-overlay').css('display', 'none');

    });


    $(document).on('change', '#yearSelect', function () {
        var prevYear = activeyear;
         activeyear = $(this).val();
         if (activeyear == 0)
         {
             $('#yearSelect').val(prevYear);
             return false;
         }
         GetMatrixSummary();
         changeHeight();
    });
});


function getDescriptionSummary(val) {

    //  var activeYear = "16-17";
    var dropdownYear = $('#yearSelect').val();
    var expireYear = false;
    var isFirst = parseInt($(val).attr('isfirst'));

    var groupId = parseInt($(val).attr('group-id'));

    var count = $(val).attr('group-pos');
    var catno = $(val).attr('cat-no');
    var pos = parseInt($(val).attr('position'));
    $('.popup-display-overlay').html('');
    $('.popup-display-overlay').css('display', 'none');
    $('.div-group-' + catno).html('');
    $('.popup-div').css('display', 'none');
    $('.popup-div1').css('display', 'none');
    $('.div-question-' + catno).html('');
    $('.div-question-' + catno).css('display', 'none');
    $.ajax({
        url: "/AgencyUser/GetSummaryDescription",
        datatype: 'json',
        type: 'post',
        data: { groupId: groupId },
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
                    getdiv.find('.tooltipdesctext').html(data[i].Description);
                    getdiv.removeClass('matrix-value');
                    getdiv.removeClass('description');
                    bindDiv += getdiv.html();
                }
                $('#popupDiv').html(bindDiv);
                $('.div-group-' + catno).html('');
                $('.div-group-' + catno).html($('.desc-view-popup').html());
                $('.div-group-' + catno).css('display', 'block');
                $('.popup-div').css('display', 'block');



                var heightarray = '';
                var divheight2 = '';
                if (isFirst === 1) {

                    var divHeight = -16;
                    heightarray = ['-16px', '50px', '116px', '182px', '248px', '311px', '377px', '443px'];
                    divheight2 = heightarray[count - 1];
                    $('.popup-div').css('top', divheight2);
                }
                else {
                    heightarray = ['-135px', '-70px', '-5px', '61px', '126px', '192px', '255px', '321px'];
                    divheight2 = heightarray[count - 1];
                    $('.popup-div').css('top', divheight2);
                }


            }
        }
    });

}

function getQuestionSummary(qnVal) {
    var groupId = parseInt($(qnVal).attr('group-id'));
    //   var clientId = $('#clientID').val().trim();
    var count = $(qnVal).attr('group-pos');
    var catno = $(qnVal).attr('cat-no');
    var pos = parseInt($(qnVal).attr('position'));
    var isFirstQn = parseInt($(qnVal).attr('isfirst'));
    $('.div-question-' + catno).html('');
    $('.popup-div1').css('display', 'none');
    $('.popup-div').css('display', 'none');
    $('.div-group-' + catno).css('display', 'none');
    $('.div-group-' + catno).html('');
    $('.popup-display-overlay').html('');
    $('.popup-display-overlay').css('display', 'none');
    $.ajax({
        url: "/AgencyUser/GetSummaryQuestions",
        datatype: 'json',
        type: 'post',
        data: { groupId: groupId },
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
                    $('#questionText').find('.tooltipqntext').html(data[i].AssessmentQuestion);
                    getdiv.removeClass('sl-no question');
                    bindDiv += getdiv.html();
                }
                $('#questionpopupDiv').html(bindDiv);
                $('.div-question-' + catno).html('');
                $('.div-question-' + catno).html($('.question-view-popup').html());
                $('.div-question-' + catno).css('display', 'block');
                $('.popup-div1').css('display', 'block');


                var heightarrayQn = '';
                var divheightQn = '';
                if (isFirstQn === 1) {

                    var divHeight = -16;
                    heightarrayQn = ['-16px', '50px', '116px', '182px', '248px', '311px', '377px', '443px'];
                    divheightQn = heightarrayQn[count - 1];
                    $('.popup-div1').css('top', divheightQn);
                }
                else {
                    heightarrayQn = ['-135px', '-70px', '-5px', '61px', '126px', '192px', '255px', '321px'];
                    divheightQn = heightarrayQn[count - 1];
                    $('.popup-div1  ').css('top', divheightQn);
                }

            }
        }
    });
}
