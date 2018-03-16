
$(function () {
    $('.slots-board').click(function () {
        $('.error-message').hide();
        $('.txtslotdate').val(getYesterdaysDate());
        GetSlotsDetails(getYesterdaysDate());
        $('#myModalSlots').modal('show');
    });
    $('.seats-board').click(function () {
        $('.error-message').hide();
        $('.txtseatdate').val(getYesterdaysDate());
        GetSeatsDetail(getYesterdaysDate());
        $('#myModalSeats').modal('show');
    });


    $('#slotsDatetimePicker').datetimepicker({
        timepicker: false,
        format: 'm/d/Y',
        validateOnBlur: false
    });

    $('#seatsDatetimePicker').datetimepicker({
        timepicker: false,
        format: 'm/d/Y',
        validateOnBlur: false
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
        return (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
    }

    function GetSlotsDetails(date) {
        $.ajax({
            type: "POST",
            url: "/Home/GetSlotsDetailByDate",
            data: { 'Date': date },
            success: function (data) {
             
                $.each(JSON.parse(data), function (i, val) {
                    $('.sp-slots-count').text(val["SlotCount"]);
                    $('.sp-expiringslots-count').text(val["Expiring"]);
                    $('.sp-emptyslots-count').text(val["EmptySlots"]);
                });
            },
            error: function (data) {
                console.log('Error');
            }
        });
    }

    function GetSeatsDetail(date) {
        $.ajax({
            type: "POST",
            url: "/Home/GetSeatsDetailByDate",
            data: { 'Date': date },
            success: function (data) {
                $.each(JSON.parse(data), function (i, val) {
                    $('.sp-seats-count').text(val["Count"]);
                    $('.sp-seatpercentage-count').text(val["Percentage"]);
                    $('.sp-seats-home-count').text(val["HomePresent"]);
                    $('.sp-seatpercentage-home-count').text(val["HomePercentage"]);

                });
            },
            error: function (data) {
                console.log('Error');
            }
        });
    }


    $('.btn-slot-submit').click(function () {
        $('.error-message').hide();
        if ($('.txtslotdate').val().trim() != "") {
            if (validDate($('.txtslotdate').val().trim())) {
                $('.slots-invalid-date').hide();
                if (Checkdate($('.txtslotdate').val().trim()))
                    GetSlotsDetails($('.txtslotdate').val().trim());
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
                if (Checkdate($('.txtseatdate').val().trim()))
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