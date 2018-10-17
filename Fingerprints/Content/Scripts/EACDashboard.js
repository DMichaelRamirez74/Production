var prgYearMinDate = null;

function getADASeatsDaily() {

    $.ajax({
        url: HostedDir + '/Home/GetADASeatsDaily',
        type: 'post',
        datatype: 'json',
        success: function (result) {


            if (result != null) {
                if ($('.ada-p').length > 0 && result.adaPercentage != undefined && result.adaPercentage != null && result.adaPercentage != '') {
                    $('.ada-p').html(result.adaPercentage + ' %');
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





$(function () {

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
            url: "/Home/GetSeatsDetailByDate",
            data: { 'Date': date },
            beforeSend:function(){$('#spinner').show()},
            success: function (data) {

                try{

                $.each(JSON.parse(data), function (i, val) {
                    $('.sp-seats-count').text(val["Count"]);
                    $('.sp-seatpercentage-count').text(val["Percentage"]);
                    $('.sp-seats-home-count').text(val["HomePresent"]);
                    $('.sp-seatpercentage-home-count').text(val["HomePercentage"]);

                    $('#seatsDatetimePicker').datetimepicker('destroy');

                    $('#seatsDatetimePicker').datetimepicker({
                        timepicker: false,
                        format: 'm/d/Y',
                        validateOnBlur: false,
                        minDate: new Date(val["ProgramYearStartDate"]),
                        maxDate:new Date()
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
});


$(window).load(function () {

    GoogleChart();
    var hours = parseFloat($('#txtThermHours').val()).toFixed(0);

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

    var Dollars = parseFloat($('#txtThermDollars').val()).toFixed(0);
    for (var i = 0; i < Dollars.length; i++) {
        Dollars = Dollars.replace(Dollars.charAt(i), '0');
    }
    var totalDOllers=0;
    if($('#txtTotalDollars').val()!="")
        totalDOllers= parseInt($('#txtTotalDollars').val());
    else
        totalDOllers=parseFloat("1" + Dollars);
    var oTherm = new jlionThermometerDollars(0, totalDOllers, false, false);
    oTherm.RefreshByID('txtThermDollars');

    $('#bar').removeClass('barDollarThermo').addClass('barHoursThermo');
    var height = $('.barHoursThermo').height() + 9;
    $('.barHoursThermo').css('height', height + 'px');
    $('#thermometer .Title').text('');
    $('#thermometer .CurrentValue').text($('#txtThermHours').val() + " Hours");


    $('#thermometer-hours .Title').text('');
    $('#thermometer-hours .CurrentValue').text("$ " + parseFloat($('#txtThermDollars').val()).toFixed(2));
});