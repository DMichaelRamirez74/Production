var prgYearMinDate = null;
var anchorCurrentEnrollment = null;
var anchorEnrolledByProgram = null;
var anchorScreeningMatrix = null;
var anchorScreeningReview = null;
var anchorClassroomType = null;
var anchorCaseNote = null;
var anchorInkind = null;
var anchorDisabilities = null;
var anchorOverIncome = null;
var anchorWaitingList = null;



function makeCaseNoteChart(chartData) {
    var chart = AmCharts.makeChart("chartdiv", {
        "hideCredits": true,
        "type": "serial",
        "theme": "light",
        //"dataProvider": [{
        //    "country": "USA",
        //    "visits": 2025
        //}],
        "dataProvider": chartData,
        "graphs": [{
            "fillAlphas": 1,
            // "fillColor":"#296EED",
            "lineAlpha": 0,
            "lineColor": "#fff",
            "type": "column",
            "valueField": "Percentage",
            "fillColors": "#296EED",
            "columnWidth": 0.6,
            "balloonFunction": function (item, content) {
                var html = "<p>" + item.dataContext.Month + "</p></p>Percentage : " + item.dataContext.Percentage + "</p>";
                // console.log(item, content);
                return html;
            }

        }],
        "categoryField": "Month",
        "categoryAxis": {
            "color": "#ffffff",
            "axisColor": "#ffffff",
            "axisAlpha": 1,
            "gridColor": "#ffffff",
            "gridAlpha": 0,
        },
        "valueAxes": [{
            "position": "bottom",
            // "offset": -200,
            "minimum": 0,
            "color": "#ffffff",
            "axisColor": "#ffffff",
            "axisAlpha": 1,
            "gridColor": "#ffffff",
            "gridAlpha": 1,
            "maximum": 100,
            "labelFunction": function (valueText, date, valueAxis) {
                //console.log(valueText, date, valueAxis);
                return valueText + "%";
            }
        },

        /*{
            "position": "left",
            "offset": -200,
            "minimum": 0,
            "maximum": 100
        }
        */
        ],

        "startDuration": 1,
        "chartCursor": {
            "fullWidth": true,
            "cursorAlpha": 0.1,
            "enabled": false,
            "cursorColor": "#000000",
            "valueBalloonsEnabled": false,
            "zoomable": false,
            "listeners": [{
                "event": "changed",
                "method": function (ev) {
                    // Log last cursor position
                    ev.chart.lastCursorPosition = ev.index;
                }
            }]
        },
        "listeners": [{
            "event": "init",
            "method": function (ev) {
                // Set a jQuery click event on chart area
                jQuery(ev.chart.chartDiv).on("click", function (e) {
                    // Check if last cursor position is present
                    if (!isNaN(ev.chart.lastCursorPosition)) {
                        console.log("clicked on " + ev.chart.dataProvider[ev.chart.lastCursorPosition].country);
                    }
                })
            }
        }]
    });
}



function jilioThermExecutiveDashboard()
{

    var hours = parseFloat($('#txtThermHours').val()).toFixed(0);
    var _txtDollars = $('#txtThermDollars').val();
    var _flotDollars = _txtDollars;
    // _txtDollars = 

    for (var i = 0; i < hours.length; i++) {
        hours = hours.replace(hours.charAt(i), '0');
    }
    var totHours = 0;
    if ($('#txtTotalHours').val() != "")
        totHours = parseInt($('#txtTotalHours').val());
    else
        totHours = parseFloat("1" + hours);

    var oTherm1 = new jlionThermometer(0, totHours, false, false);
    oTherm1.RefreshByID('txtThermHours');

    var Dollars = parseFloat(_flotDollars).toFixed(0);
    for (var i = 0; i < Dollars.length; i++) {
        Dollars = Dollars.replace(Dollars.charAt(i), '0');
    }
    var totalDOllers = 0;
    if ($('#txtTotalDollars').val() != "")
        totalDOllers = parseInt($('#txtTotalDollars').val());
    else
        totalDOllers = parseFloat("1" + Dollars);
    var oTherm = new jlionThermometerDollars(0, totalDOllers, true, false);



    $('#txtThermDollars').val(parseFloat(_flotDollars));


    oTherm.RefreshByID('txtThermDollars');

    $('#bar').removeClass('barDollarThermo').addClass('barHoursThermo');
    var height = $('.barHoursThermo').height() + 9;
    $('.barHoursThermo').css('height', height + 'px');
    $('#thermometer .Title').text('');


    $('#thermometer .CurrentValue').text(formatCurrency($('#txtThermHours').val()) + ' Hours');

    $('#thermometer-hours .Title').text('');

    $('#thermometer-hours .CurrentValue').text("$ " + formatCurrency($('#txtThermDollars').val()));
}

function getADASeatsDaily() {

    $.ajax({
        url: HostedDir + '/Home/GetADASeatsDaily',
        type: 'post',
        datatype: 'json',
        success: function (result) {


            if (result != null) {
                if ($('.ada-p').length > 0 && result.adaPercentage != undefined && result.adaPercentage != null && result.adaPercentage != '') {
                    $('.ada-p').html(parseFloat(result.adaPercentage) + ' %');
                }

                if ($('.seats-p').length > 0 && result.todaySeats != undefined && result.todaySeats != null && result.todaySeats != '') {
                    $('.seats-p').html(result.todaySeats);
                }
            }

        },
        error: function (data) {
            console.log(data);
        }
    });
}


function formatCurrency(val) {
   
    //var val = ctrl.value;

    val = val.replace(/,/g, "")
    //ctrl.value = "";
    val += '';
    x = val.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';

    var rgx = /(\d+)(\d{3})/;

    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
        console.log(x1);
    }

    return x1 + x2;
}



function refreshDashboardBySection(ele, sectionType, callback) {


    $(ele).children('.fa-refresh').addClass('fa-spin');

    $.ajax({


        url: HostedDir + '/Home/RefreshExecutiveDashboardBySection',
        type: 'GET',
        datatype: 'JSON',
        data: { sectionType: sectionType },
        async: true,
        beforeSend: function () { $('#spinner').hide() },
        success: function (data) {
            callback(data);

        },
        error: function (data) {

        },
        complete: function (data) {
            $('#spinner').hide();

            $(ele).children('.fa-refresh').removeClass('fa-spin');
        }


    });
}

function bindCurrentEnrollment(data) {

    if (data != null && data.EnrollmentTypeList != null && data.EnrollmentTypeList.length > 0) {
        var eduSection = $('#table-current-enroll');

        var appendData = '<tr>\
                        <th>Enrollment Type</th>\
                            <th>Total</th>\
                        </tr>';

        $.each(data.EnrollmentTypeList, function (i, enrollment) {

            appendData += '<tr>\
                            <td>'+ enrollment.CenterType + '</td>\
                                    <td class="number-center">'+ enrollment.Total + '</td>\
                         </tr>';

        });

        eduSection.html(appendData);

    }

}

function bindEnrolledByProgram(data) {

    
    if (data != null && data.EnrolledProgramList != null && data.EnrolledProgramList.length > 0) {

        var eduProgram = $('#table-enroll-program');

        var appendProgram = '<tr>\
                                                        <th>Program</th>\
                                                        <th>Funded</th>\
                                                        <th>Actual</th>\
                                                    </tr>';

        $.each(data.EnrolledProgramList, function (i, program) {
            appendProgram += '<tr>\
                            <td>' + program.ProgramType + '</td>\
                            <td class="number-center">' + program.Total + '</td>\
                            <td class="number-center">' + program.Available + '</td>\
                            </tr>';
        });

        eduProgram.html(appendProgram);
    }
}

function bindScreeningMatrix(data) {

   

    if (data != null && data.ScreeningMatrixList.length > 0)
    {
        var tableBind = $('#table-missing-scr');

      

        var appendData = '';

        $.each(data.ScreeningMatrixList, function (i, screening) {

            appendData+='<tr>\
                            <td data-title="Screening Name">' + screening.ScreeningName + '</td>\
                            <td data-title="Up-to-Date" class="number-center">' + screening.UptoDate + '</td>\
                            <td data-title="Missing" class="number-center">' + screening.Missing + '</td>\
                            <td data-title="Expired" class="number-center">' + screening.Expired + '</td>\
                            <td data-title="Expiring" class="number-center">' + screening.Expiring + '</td>\
                        </tr>'
        });


        tableBind.find('tbody').html(appendData);
    }

}


function bindScreeningReview(data)
{
    if (data != null && data.NDayScreeningReviewList.length > 0) {
        var tableBind = $('#table-scr-review');



        var appendData = '';

        $.each(data.NDayScreeningReviewList, function (i, screening) {

            appendData += '<tr>\
                            <td data-title="Screening Name">' + screening.ScreeningName + '</td>\
                            <td data-title="Completed" class="number-center">' + screening.Completed + '</td>\
                            <td data-title="Completed but Late" class="number-center">' + screening.CompletedButLate + '</td>\
                            <td data-title="Not Expired" class="number-center">' + screening.NotExpired + '</td>\
                            <td data-title="Not Completed and Late" class="number-center">' + screening.NotCompletedandLate + '</td>\
                        </tr>'
        });


        tableBind.find('tbody').html(appendData);
    }

}


function bindClasroomType(data) {


    if (data != null && data.ClassRoomTypeList != null && data.ClassRoomTypeList.length > 0) {
        var classroomTypetbl = $('#table-classroomtype');

        var appendData = '<tr>\
                        <th>Room Type</th>\
                        <th>Total</th>\
                        <th>Actual</th>\
                        </tr>';

        $.each(data.ClassRoomTypeList, function (i, classroomType) {

            appendData += '<tr>\
                        <td data-title="Room Type">' + classroomType.ClassSession + '</td>\
                        <td data-titlte="Total" class="number-center">' + classroomType.Total + '</td>\
                        <td data-title="Actual" class="number-center">' + classroomType.Available + '</td>\
                        <tr>';
        });


        classroomTypetbl.html(appendData);
    }
}

function bindCaseNoteExecutive(data) {


    if (data != null && data.listCaseNote != null && data.listCaseNote.length > 0) {
        //var caseNoteArray = [];


        //$.each(data.listCaseNote, function (i, caseNote) {

        //    caseNoteArray.push({
        //        "Name": caseNote.Month, "Value": parseInt(caseNote.Percentage) == caseNote.Percentage ? parseInt(caseNote.Percentage) : caseNote.Percentage
        //    });

        //});

        makeCaseNoteChart(data.listCaseNote);

    }

  
}

function bindInkindExecutive(data) {
    if (data != null) {
        $('#txtThermHours').val(data.ThermHours);
        $('#txtThermDollars').val(data.ThermDollars);
        $('#txtTotalHours').val(data.TotalHours);
        $('#txtTotalDollars').val(data.TotalDollars);
        jilioThermExecutiveDashboard();
    }
}

function bindDisabilitiesExecutive(data) {
   

    if(data!=null)
    {
        $('#dis-p').html(data.DisabilityPercentage + ' %');
    }
}


function bindOverIncomeExecutive(data) {

   
    if (data != null) {
        $('#ov-p').html(data.FamilyOverIncome + ' %');
    }
}


function bindWaitingListExecutive(data) {
  

    if(data!=null)
    {
        $('#wait-p').html(data.WaitingList + ' %');
        $('#wait-count').html(data.WaitingListCount);
    }
}



$(document).ready(function () {





    // GoogleChart();
    //  CaseNoteAmChart();




    anchorCurrentEnrollment = $('#anchorCurrentEnrollment');
    anchorEnrolledByProgram = $('#anchorEnrolledByProgram');
    anchorScreeningMatrix = $('#anchorScreeningMatrix');
    anchorScreeningReview = $('#anchorScreeningReview');
    anchorClassroomType = $('#anchorClassroomType');
    anchorCaseNote = $('#anchorCaseNote');
    anchorInkind = $('#anchorInkind');
    anchorDisabilities = $('#anchorDisabilities');
    anchorOverIncome = $('#anchorOverIncome');
    anchorWaitingList = $('#anchorWaitingList');


    $('[data-toggle="tooltip"]').tooltip();


    $('[data-toggle="tooltip"]').on('click', function () {


        $(this).tooltip('hide');
    });


    prgYearMinDate = new Date($('#ProgramYearStartDate').val());

    $('.slots-board').click(function () {
        $('.error-message').hide();
        $('.txtslotdate').val(getYesterdaysDate());
        GetSlotsDetails(getYesterdaysDate());

    });
    $('.seats-board').click(function () {
        $('.error-message').hide();
        $('.txtseatdate').val(getYesterdaysDate());
        GetSeatsDetail(getYesterdaysDate());

    });


    $('#slotsDatetimePicker').datetimepicker({
        timepicker: false,
        format: 'm/d/Y',
        validateOnBlur: false,
        maxDate: new Date(),
        minDate: new Date(prgYearMinDate)

    });

  


    $(document).on('click', '.slots-icon', function () {

        $('#slotsDatetimePicker').datetimepicker('show');
    });

    $(document).on('click', '.seats-icon', function () {

        $('#seatsDatetimePicker').datetimepicker('show');
    });




    anchorCurrentEnrollment.on('click', function () {

        
        refreshDashboardBySection(this, dashboardSectionType.CurrentEnrollment, bindCurrentEnrollment);
    });


    anchorEnrolledByProgram.on('click', function () {

        refreshDashboardBySection(this, dashboardSectionType.EnrolledByProgram, bindEnrolledByProgram);


    });


    anchorScreeningMatrix.on('click', function () {

        refreshDashboardBySection(this, dashboardSectionType.MissingScreening, bindScreeningMatrix);

    });


    anchorScreeningReview.on('click', function () {
   
        refreshDashboardBySection(this, dashboardSectionType.ScreeningReview, bindScreeningReview);
    });


    anchorClassroomType.on('click', function () {

        refreshDashboardBySection(this, dashboardSectionType.ClassroomType, bindClasroomType);


    });

    anchorCaseNote.on('click', function () {



        refreshDashboardBySection(this, dashboardSectionType.CaseNoteAnalysis, bindCaseNoteExecutive);

    });

    anchorInkind.on('click', function () {
        refreshDashboardBySection(this, dashboardSectionType.InKindHoursDollars, bindInkindExecutive);


    });

    anchorDisabilities.on('click', function () {

        refreshDashboardBySection(this, dashboardSectionType.Disabilities, bindDisabilitiesExecutive);
    });

    anchorOverIncome.on('click', function () {

        refreshDashboardBySection(this, dashboardSectionType.OverIncome, bindOverIncomeExecutive);

    });

    anchorWaitingList.on('click', function () {


        refreshDashboardBySection(this, dashboardSectionType.WaitingList, bindWaitingListExecutive);
    });



    getADASeatsDaily();



    function getYesterdaysDate() {
        var date = new Date();
        date.setDate(date.getDate() - 1);

        var monthString = (date.getMonth() + 1) < 10 ? '0' + (date.getMonth() + 1).toString() : (date.getMonth() + 1);
        var dateString = date.getDate() < 10 ? '0' + date.getDate().toString() : date.getDate();


        return monthString + '/' + dateString + '/' + date.getFullYear();
    }

    function GetSlotsDetails(date) {
        $.ajax({
            type: "POST",
            url: "/Home/GetSlotsDetailByDate",
            data: { 'Date': date },
            beforeSend: function () { $('#spinner').show() },
            success: function (data) {
                try {

                   
                 
                    $.each(JSON.parse(data), function (i, val) {
                        $('.sp-slots-count').text(val["SlotCount"]);
                        $('.sp-expiringslots-count').text(val["Expiring"]);
                        $('.sp-emptyslots-count').text(val["EmptySlots"]);
                        $('.sp-slots-per').text(parseFloat(val["SlotsDailyPercentage"]) + ' %');
                        $('.sp-clientserved-count').text(val["ClientsServed"]);
                    });

                    $('#spinner').hide();
                    $('#myModalSlots').modal('show');
                }
                catch (error) {
                    $('#spinner').hide();
                }
            },
            error: function (data) {
                console.log('Error');
                $('#spinner').hide();
            }

        });
    }

    function GetSeatsDetail(date) {
        $.ajax({
            type: "POST",
            url: HostedDir+ "/Home/GetSeatsDetailByDate",
            data: { 'Date': date },
            beforeSend: function () { $('#spinner').show() },
            success: function (data) {

                try {

                    $.each(JSON.parse(data), function (i, val) {
                        $('.sp-seats-count').text(val["Count"]);
                        $('.sp-seatpercentage-count').text(val["Percentage"]);
                        $('.sp-seats-home-count').text(val["HomePresent"]);
                        $('.sp-seatpercentage-home-count').text(parseFloat(val["HomePercentage"]));
                        $('.span-week-start-date').text('(' + val["WeekStartDate"] + ' to '+ val["WeekEndDate"]+')');

                        $('#seatsDatetimePicker').datetimepicker('destroy');

                        $('#seatsDatetimePicker').datetimepicker({
                            timepicker: false,
                            format: 'm/d/Y',
                            validateOnBlur: false,
                            minDate: new Date(val["ProgramYearStartDate"]),
                            maxDate: new Date()
                        });

                        prgYearMinDate = val["ProgramYearStartDate"];

                    });

                    $('#spinner').hide();
                    $('#myModalSeats').modal('show');
                }
                catch (error) {
                    $('#spinner').hide();
                }
            },
            error: function (data) {
                $('#spinner').hide();
                console.log('Error');
            }
        });
    }


    $('.btn-slot-submit').click(function () {
        $('.error-message').hide();
        if ($('.txtslotdate').val().trim() != "") {
            if (validDate($('.txtslotdate').val().trim())) {
                $('.slots-invalid-date').hide();
                if (Checkdate($('.txtslotdate').val().trim())) {
                    if (new Date($('.txtslotdate').val()).setHours(0, 0, 0, 0) <= new Date(prgYearMinDate).setHours(0, 0, 0, 0)) {
                        customAlert('Entered date should be greater than or equal to program year start date (' + $('#ProgramYearStartDate').val() + ')');
                        $('.slots-invalid-date').show();
                    }
                    else {
                        GetSlotsDetails($('.txtslotdate').val().trim());
                    }
                }
                else
                    $('.slots-future-date').show();
            }
            else {
                $('.slots-invalid-date').show();
            }
        }
        else {
            $('.slots-message-empty').show();
        }


    });
    $('.btn-seat-submit').click(function () {
        $('.error-message').hide();
        if ($('.txtseatdate').val().trim() != "") {
            if (validDate($('.txtseatdate').val().trim())) {
                $('.seats-invalid-date').hide();


                if (new Date($('.txtseatdate').val()).setHours(0, 0, 0, 0) < new Date(prgYearMinDate).setHours(0, 0, 0, 0)) {
                    customAlert('Entered date should be greater than or equal to program year start date (' + $('#ProgramYearStartDate').val() + ')');
                    $('.seats-invalid-date').show();
                }
                else if (Checkdate($('.txtseatdate').val().trim()))
                    GetSeatsDetail($('.txtseatdate').val().trim());
                else
                    $('.seats-future-date').show();
            }


            else {
                $('.seats-invalid-date').show();
            }
        }
        else {
            $('.seats-message-empty').show();
        }
    });


    $('body').on("focusout", ".txt-date", function () {
        if ($(this).val() != "") {
            var isValid = Checkdate($(this).val());
            if (!validDate($(this).val())) {
                $(this).parent('.input-container').find('.error-invalid-date').show();
            }
            else if (!isValid) {
                $(this).parent('.input-container').find('.error-future-date').show();
            }
        }
        else {
            $('.error-message').hide();

        }


    });
    $('body').on("keyup", ".txt-date", function () {

        $(this).parent('.input-container').find('span').hide();
    });


    function validDate(text) {
        var isValid = true;
        var comp = text.split('/');
        if (comp.length !== 3)
            return false;
        if (comp[2].length != 4)
            return false;
        if (comp[2] <= 1901)
            return false;
        if (new Date(text).toString() == "Invalid Date")
            return false;
        if (!isvalid_mdy(text))
            return false;
        var TodayDate = new Date();
        var endDate = new Date(text);

        return isValid;
    }
    function isvalid_mdy(s) {

        var day, A = s.match(/[1-9][\d]*/g);
        try {
            A[0] -= 1;
            day = new Date(+A[2], A[0], +A[1]);

            if (day.getMonth() == A[0] && day.getDate() == A[1]) return day;
        }
        catch (er) {
            return er.message;
        }

    }



    function Checkdate(date) {
        var isAllow = false;
        var now = new Date();
        var selectedDate = new Date(date);
        if (selectedDate < now && selectedDate != now) {
            isAllow = true;
        } else {
            isAllow = false;
        }
        return isAllow;
    }
    var flag = 0;
    $(".txt-date").on('keydown', function (e) {
        flag++;
        if (flag > 1) {
            e.preventDefault();
        }
    });
    $(".txt-date").on('keyup', function (e) {
        flag = 0;
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



    //GoogleChart();
    $('body').on('click', '.dropdown', function () {
        setTimeout(function ()
        { $('.dropdown').addClass('open'); }, 100);

    });


    //updates the ADA count for every 5 minutes
    setInterval(function () {
        getADASeatsDaily()
    }, 120000); // this will run after every 2 minutes
    ///3000
    ///300000
    //120000


    if (caseNoteJson != null && caseNoteJson.length > 0) {
        makeCaseNoteChart(caseNoteJson);
    }


});


$(window).load(function () {

    jilioThermExecutiveDashboard();

   

});