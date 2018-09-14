
$(document).ready(function () {

    var allUsers = '';
    var allMeals = '';
    var days = [];
    var childInfoJson = '';
    var childAttendanceJson = '';
    var centerInfoJson = '';
    var childString = $('#childInfoString');
    var attendanceString = $('#WeeklyAttendancestring');
    var centerInfoString = $('#centerInfoString');
    var UserId = $('#userId').val();
    var agencyId = $('#agencyId').val();
    var center_para = $('#center-para');
    var class_para = $('#class-para');
    var off_hist_div = $('#offline-hist-div');
    var working_days_div = $('#working-days-div');
    var working_time_div = $('#working-time-div');
    var working_days_check = $('#working-days-check');
    var attendance_div = $('#attendance-div');
    var no_data_div = $('#no-data-div');
    var weekly_child_info_body = $('#weekly-child-info-body');
    var offline_child_info_body = $('#offline-child-info-tbody');

    var weekAttendance_div = $('#weekDaysAttendance');
    var offlineAttendance_div = $('#singleDayAttance');
    var teacherTimeDiff = $('#teacherTimeDiff');
    var fswTimeDiff = $('#fswTimeDiff');
    var roleId = $('#roleId');
    var enc_CenterId = '';
    var enc_ClassRoomId = '';
    var historicalRoleArray = [];
    historicalRoleArray = ['b4d86d72-0b86-41b2-adc4-5ccce7e9775b', 'a65bb7c2-e320-42a2-aed4-409a321c08a5', '3b49b025-68eb-4059-8931-68a0577e5fa2', 'b65759ba-4813-4906-9a69-e180156e42fc', 'c352f959-cfd5-4902-a529-71de1f4824cc'];

    if (class_para.children('select').length === 0) {
        working_days_div.show();
        off_hist_div.show();
        working_time_div.show();
    }
    else {
        working_days_div.hide();
        off_hist_div.hide();
        working_time_div.hide();
    }

    var weeklyAttendance = {

        initializeElement: function () {
            try {

                $('.kbw-signature').signature();

                $('#datetimepicker1').datetimepicker({
                    timepicker: false,
                    format: 'm/d/Y',
                    validateOnBlur: false,
                    value: this.getFormattedDate(new Date()),
                    autoClose: true,
                    maxDate: new Date(),
                    minDate: new Date($('#accessStartDate').val()),
                    closeOnDateSelect:true
                });

            }
            catch (e) {
                console.log(e);
            }
        },

        bindAllHiddenData: function () {
            try {

                //childInfoJson = (childString.val() != "") ? JSON.parse(childString.val()) : "";
                //childAttendanceJson = (attendanceString.val() != "") ? JSON.parse(attendanceString.val()) : "";
                //centerInfoJson = (centerInfoString.val() != "") ? JSON.parse(centerInfoString.val()) : "";

                childInfoJson = JSON.parse(childString.val());
                childAttendanceJson = JSON.parse(attendanceString.val());
                centerInfoJson = JSON.parse(centerInfoString.val());

            }
            catch (e) {
                console.log(e)
            }

        },
        getChildAttendanceData: function () {
            childAttendanceJson = (attendanceString.val() != "") ? JSON.parse(attendanceString.val()) : "";
            return childAttendanceJson;
        },
        getAllUser: function (data) {
            try {
                DataBaseManager.GetAllClient(this.listAllUsers);
            }
            catch (e) {
                console.log(e)
            }
        },
        listAllUsers: function (data) {
            try {
                this.clearAllInputs();

                if (data.length > 0) {

                    $.each(data, function (j, table) {
                        var tableIndex = table.UserID.charAt(0);
                        var rowIndex = table.UserID.charAt(1);
                        var row = $('table[day="' + tableIndex + '"]').find('tr[clientid="' + table.ClientID + '"]')
                        if ($(row).attr('clientid') == table.ClientID) {

                            $(row).find('.in-time ').val(table.TimeIn);
                            $(row).find('.out-time').val(table.TimeOut);

                            if (table.BreakFast == 1) {
                                $(row).find('.break-check').prop('checked', true);
                            }

                            if (table.Lunch == 1) {
                                $(row).find('.lunch-check').prop('checked', true);
                            }
                            if (table.Snacks == 1) {
                                $(row).find('.snack-check').prop('checked', true);
                            }
                        }
                        else {

                        }

                    });
                }
            }
            catch (e) {
                console.log(e);
            }
        },
        clearAllInputs: function () {
            $('.time-text').val('');
            $('.meals-check').each(function () {
                $(this).prop('checked', false);
            });
            $('.child-breakfastcout,.child-lunchcount,.child-snackscount').html(0);
            $('.adult-breakfastcout,.adult-lunchcount,.adult-snackscount').val(0);
        },

        getFormattedDate: function (date) {
            date = new Date(date);
            var year = date.getFullYear();

            var month = (1 + date.getMonth()).toString();
            month = month.length > 1 ? month : '0' + month;

            var day = date.getDate().toString();
            day = day.length > 1 ? day : '0' + day;

            return month + '/' + day + '/' + year;
        },
        getCenterClassId: function () {
            //Get's Center and Class Room Id//
            enc_CenterId = (center_para.children('select').length === 0) ? center_para.children('#centerSpan').attr('enc-cen-id') : center_para.children('select').val();
            enc_ClassRoomId = (class_para.children('select').length === 0) ? class_para.children('#classRoomSpan').attr('enc-cls-id') : class_para.children('select').val();

            return center = {
                'enc_CenterId': enc_CenterId,
                'enc_ClassRoomId': enc_ClassRoomId
            };
        },
        getTimeZoneMinutes: function () {
            return (center_para.children('select').length === 0) ? center_para.children('#centerSpan').attr('minudiff') : center_para.find('option:selected', 'select').attr('minudiff');
        },

        convertToUserTimeZone: function () {
            var minDiff = this.getTimeZoneMinutes();
            milliseconds = (parseInt(minDiff) * 60000);
            clientDate = new Date();
            utc = clientDate.getTime() + (clientDate.getTimezoneOffset() * 60000);
            serverDate = new Date(utc + (milliseconds));
            return serverDate;
        },
        formatOfflineAMPM: function () {
            var date = this.convertToUserTimeZone();
            var hours = date.getHours();
            var minutes = date.getMinutes();
            var ampm = hours >= 12 ? 'PM' : 'AM';
            hours = hours % 12;
            hours = hours ? hours : 12; // the hour '0' should be '12'
            minutes = minutes < 10 ? '0' + minutes : minutes;
            hours = hours < 10 ? '0' + hours : hours;
            var strTime = hours + ':' + minutes + ' ' + ampm;
            return strTime;
        },
        initializeDatetimePicker: function (id) {

            var self = this;

            $('#' + id + '').blur(function () {
                var validTime = $(this).val().match(/^(0?[1-9]|1[012])(:[0-5]\d) [APap][mM]$/);
                if (!validTime) {
                    $(this).val('').focus().css('background', '#fdd');
                } else {
                    $(this).css('background', 'transparent');
                }
            });

            $('#' + id + '').datetimepicker('destroy');

            //Initializing the time picker for intime and out time input//
            $('#' + id + '').datetimepicker({
                datepicker: false,
                format: "h:i A",
                formatTime: "h:i A",
                validateOnBlur: false,
                step: 15,
                allowTimes: self.allowTime(),
                autoClose: true
               


            });
        },
        getClassStartEndTime24hr: function () {
         
            var timeJson = {
                'StartTime': '',
                'EndTime': ''
            };
            var workingTime = class_para.find('option:selected', 'select').attr('working-time');
            var fromTime = workingTime.split('-')[0].trim();
            var toTime = workingTime.split('-')[1].trim();

            timeJson.StartTime = this.get24hrTimeFormat(fromTime);
            timeJson.EndTime = this.get24hrTimeFormat(toTime);
            return timeJson;
        },
        get24hrTimeFormat: function (timeFormat) {
            var time = '';
            // var date = new Date();
            var hours = timeFormat.split(' ')[1] == "PM" ? parseInt(timeFormat.split(' ')[0].split(':')[0]) + 12 : parseInt(timeFormat.split(' ')[0].split(':')[0]) % 12;
            var minutes = parseInt(timeFormat.split(' ')[0].split(':')[1]);
            hours = hours < 10 ? "0" + hours : hours;
            minutes = minutes < 10 ? "0" + minutes : minutes;
            // var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
            //time = hours + ":" + minutes + ":" + seconds;
            time = hours + ":" + minutes;
            return time;
        },

        allowTime: function () {

            var timeJsonArray = this.getClassStartEndTime24hr();
        
            //var start = 6;
            //var end = 14;
            var start = parseInt(timeJsonArray.StartTime.split(':')[0]) + parseInt(timeJsonArray.StartTime.split(':')[1]) / 60;
            var end = parseInt(timeJsonArray.EndTime.split(':')[0]) + parseInt(timeJsonArray.EndTime.split(':')[1]) / 60;
            var arr = [];
          
           
            while (end > start) {
                var time = 0;
                for (var t = ((start % parseInt(start)) * 60) ; t < 60;) {
                    var convTimes=t;
                    if (convTimes == 0) {
                        time = (parseInt(start) < 10) ? '0' + parseInt(start) + ':' + '0' + convTimes : parseInt(start) + ':' + '0' + convTimes;
                        arr.push(time);
                    }
                    else {
                        time = (parseInt(start) < 10) ? '0' + parseInt(start) + ':' + convTimes : parseInt(start) + ':' + convTimes;
                        arr.push(time);
                    }
                    t = t + 15 + ((start % parseInt(start)) * 60);
                    if ((parseInt(start) + (convTimes / 12)) > end) {
                        break;
                    }
                }
                start += 1;
            }
            arr.push(timeJsonArray.EndTime);

           
            return arr;
        },
        tConvert: function (time) {
            // Check correct time format and split into components
            time = time.toString().match(/^([01]\d|2[0-3])(:)([0-5]\d)(:[0-5]\d)?$/) || [time];

            if (time.length > 1) { // If time format correct
                time = time.slice(1);  // Remove full string match value
                time[5] = +time[0] < 12 ? 'AM' : 'PM'; // Set AM/PM
                time[0] = +time[0] % 12 || 12; // Adjust hours
            }
            return time.join(''); // return adjusted time or original string
        },
        getMonday: function (d) {
            d = new Date(d);
            var day = d.getDay(),
                diff = d.getDate() - day + (day == 0 ? -6 : 1); // adjust when day is sunday
            return new Date(d.setDate(diff));
        },
        getFriday: function (d) {
            var curr;
            curr = new Date(d);
            var fridayDate;
            fridayDate = new Date(d);
            var friday;
            friday = 5 - curr.getDay();
            fridayDate.setDate(fridayDate.getDate() + friday);
            return fridayDate;
        },

        getEmptyClientJson: function () {
            var clientJson = {};
            clientJson = {
                'ClientID': '',
                'AttendanceDate': '',
                'TimeIn': '',
                'TimeOut': '',
                'BreakFast': '',
                'Lunch': '',
                'Snacks': '',
                'AttendanceType': '',
                'SignedInBy': '',
                'PSignatureIn': '',
                'TSignatureIn': '',
                'SignedOutBy': '',
                'PSignatureOut': '',
                'TSignatureOut': '',
                'AbsenceReasonId': '',
                'UserID': '',
                'CenterID': '',
                'ClassroomID': ''
            };

            return clientJson;
        },
        getEmptyMealsJson: function () {
            var mealsJson = {};
            mealsJson = {
                'AdultBreakFast': '',
                'AdultLunch': '',
                'AdultSnacks': '',
                'AttendanceDate': '',
                'ChildBreakFast': '',
                'ChildExcused': '',
                'ChildLunch': '',
                'ChildPresent': '',
                'ChildSnacks': '',
                'ChildUnExcused': '',
                'DailyID': '',
                'CenterID': '',
                'ClassroomID': ''
            };

            return mealsJson;
        },

        displayTwelveFormatTime: function (date) {
            //var date = new Date();
            var hours = date.getHours() > 12 ? date.getHours() - 12 : date.getHours();
            var am_pm = date.getHours() >= 12 ? "PM" : "AM";
            hours = hours < 10 ? "0" + hours : hours;
            var minutes = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
            var seconds = date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds();
            time = hours + ":" + minutes + ":" + seconds + " " + am_pm;
            return time;
        },
        timeChangeUpdate: function (val) {

            var hisIndex = $(val).closest('.att-tr').attr('row-index');
            var time = $(val).closest('.time-td').find('.time-text').val();
            if (hisIndex != undefined && time != '') {
                var attendanceDate = this.getAttendanceDate($(val).closest('.att-tr').attr('day'));
                hisIndex = getDatabaseIndex(hisIndex, attendanceDate);
                var clientId = $(val).closest('.att-tr').attr('clientid');
                DataBaseManager.GetSingleClient(hisIndex, clientId, attendanceDate, updateRecordOnTimechange, val);

            }

        },
        onCheckMeals: function (val) {

            var hisIndex = $(val).closest('.att-tr').attr('row-index');
            var dayIndex = $(val).closest('.att-tr').attr('day');
            if (hisIndex != undefined) {
                var attendanceDate = (this.isHistorical()) ? this.getAttendanceDate($(val).closest('.att-tr').attr('day')) : this.getAttendanceDate(new Date());
                hisIndex = getDatabaseIndex(hisIndex, attendanceDate);
                var clientId = $(val).closest('.att-tr').attr('clientid');
                DataBaseManager.GetSingleClient(hisIndex, clientId, attendanceDate, updateOnCheckMeals, val);


            }
        },
        onCheckOfflineMeals: function (val) {
            var hisIndex = $(val).closest('.att-tr').attr('row-index');
            var dayIndex = $(val).closest('.att-tr').attr('day');
            if (hisIndex != undefined) {
                var attendanceDate = getOfflineAttendanceDate();
                hisIndex = getDatabaseIndex(hisIndex, attendanceDate);
                var clientId = $(val).closest('.att-tr').attr('clientid');
                DataBaseManager.GetSingleClient(hisIndex, clientId, attendanceDate, updateOfflineOnCheckMeals, val);

            }
        },
        isHistorical: function () {
            return ($('#historicalatt').is(':checked'))
        },
        getAttendanceDate: function (date) {
            var returnDate = '';
            if (this.isHistorical()) {
                switch (date) {
                    case '0':
                        returnDate = weekAttendance_div.find('#week-monday').attr('date');
                        break;
                    case '1':
                        returnDate = weekAttendance_div.find('#week-tuesday').attr('date');
                        break;
                    case '2':
                        returnDate = weekAttendance_div.find('#week-wednesday').attr('date');
                        break;
                    case '3':
                        returnDate = weekAttendance_div.find('#week-thursday').attr('date');
                        break;
                    case '4':
                        returnDate = weekAttendance_div.find('#week-friday').attr('date');
                        break;
                }
            }
            else {
                returnDate = this.getFormattedDate(date);
            }

            return returnDate;
        },

        getChildInformation: function () {
            var childInfoAppend = '';
            var childList = this.getChildInfoList();
            if (childList.length > 0) {
                $.each(childList, function (i, child) {

                    childInfoAppend += '<tr id="childinfo_' + i + '">\
                                          <td class="weekly-table-text-al1"><p style="text-align:left;" >'+ child.CName + '</p></td>\
                                        <td class="weekly-table-text-al1"><p style="text-align:left;">'+ child.CDOB + '</p></td>\
                                         <input type="hidden" id="father_'+ i + '" father_name="' + child.Parent1Name + '" value=' + child.Parent1ID + ' />\
                                         <input type="hidden" id="mother_' + i + '" mother_name="' + child.Parent2Name + '" value=' + child.Parent2ID + ' />\
                                         </tr>';
                });
            }


            return childInfoAppend;
        },
        getChildInfoList: function () {

            var childInfoList = '';
            var center = this.getCenterClassId();
            if (childInfoJson != '') {
                if (childInfoJson.length > 0) {
                    childInfoList = $.grep(childInfoJson, function (element, index) {
                        return element.Enc_CenterId === center.enc_CenterId && element.Enc_ClassRoomId === center.enc_ClassRoomId
                    });
                }
            }
            return childInfoList;
        },
        ShowBusy: function (stat) {

            if (stat) {
                $('#spinner').show();
            }
            else {
                $('#spinner').hide();
            }
        },

        GetClassrooms: function (select) {
            $.ajax({
                url: "/Teacher/GetClassRoomsByCenterHistorical",
                type: "POST",
                data: {
                    Centerid: $(select, 'option:selected').val()
                },
                dataType: "json",
                secureuri: false,
                async: true,
                beforeSend:function(){weeklyAttendance.ShowBusy(true);},
                success: function (response) {

                    weeklyAttendance.ShowBusy(false);

                        var appendDiv = '<option value="0"></option>';
                        if (response != null) {
                            if (response.CenterList.length > 0) {
                                appendDiv = '<option value="0">--Select Classroom--</option>';
                                if (response.CenterList[0].Classroom.length > 0) {
                                    $.each(response.CenterList[0].Classroom, function (i, classroom) {

                                        appendDiv += '<option closed=' + classroom.ClosedToday + ' value=' + classroom.Enc_ClassRoomId + ' mon=' + classroom.Monday.toString() + ' tue=' + classroom.Tuesday.toString() + ' wed=' + classroom.Wednesday.toString() + ' thu=' + classroom.Thursday.toString() + ' fri=' + classroom.Friday.toString() + ' sat=' + classroom.Saturday.toString() + ' sun=' + classroom.Sunday.toString() + ' working-time="' + classroom.StartTime + ' - ' + classroom.StopTime + '" >' + classroom.ClassName + '</option>';
                                    });
                                }
                            }
                        }
                        class_para.find('select').html(appendDiv);
                    }
            , error: function (response) {
                weeklyAttendance.ShowBusy(false);
                //customAlert("Session Ended Log Onto The System Again."); setTimeout(function () { window.location.href = HostedDir + '/login/Loginagency'; }, 2000);
            }
            });
        },
        GetFSWandTeacher: function () {
            var centerClass = this.getCenterClassId();
            $.ajax({
                url: '/Teacher/GetFSWNameandTeacherName',
                type: 'post',
                datatype: 'json',
                async: true,
                beforeSend: function () { weeklyAttendance.ShowBusy(true); },
                data: { centerId: centerClass.enc_CenterId, classroomId: centerClass.enc_ClassRoomId },
                success: function (data) {
                    if (off_hist_div.is(':visible')) {
                        weeklyAttendance.ShowBusy(false);
                    }

                    $('#teacherName_div').show();
                    var teacherName = (data.TeacherName == null || data.TeacherName == '') ? '' : data.TeacherName;
                    var fswName = (data.FSWName == null || data.FSWName == '') ? '' : data.FSWName;
                    $('#teacherNameSpan').text(teacherName);
                    $('#teacherTimeDiff').val(data.TeacherTimeZoneDiff);
                    $('#fswTimeDiff').val(data.FSWTimeZoneDiff);
                    $('#fswName_div').show();
                    $('#fswNameSpan').text(fswName);
                    $('#userId').val(data.UserId);
                },
                error: function (data) {
                    weeklyAttendance.ShowBusy(false);
                }

        });
    },

getWeekDatesFormatted: function (dateEntered) {


    var startDate = new Date(new Date(dateEntered).setDate(new Date(dateEntered).getDate() - new Date(dateEntered).getDay() + 1));
    var endDate = new Date(new Date(dateEntered).setDate(new Date(dateEntered).getDate() - new Date(dateEntered).getDay() + 5));


    var datesforamt = [],
        currentDate = startDate,
        addDays = function (days) {
            var date = new Date(this.valueOf());
            date.setDate(date.getDate() + days);
            return date;
        };
    while (currentDate <= endDate) {
        datesforamt.push(this.getFormattedDate(currentDate));
        currentDate = addDays.call(currentDate, 1);
    }
    return datesforamt;
}
};

weeklyAttendance.initializeElement();
weeklyAttendance.clearAllInputs();


//error function  Start//
function customAlert(MessageText) {

    if (MessageText.indexOf("added") != -1 || MessageText.indexOf("updated") != -1 || MessageText.indexOf("successfully") != -1 || MessageText.indexOf("Invatation") != -1) {
        $("#noty_topRight_layout_container").css("background-color", "green");
    }
    else {
        $("#noty_topRight_layout_container").css("background-color", "#a94442");
    }
    $("#noty_topRight_layout_container")[0].children[0].children[0].children[0].innerHTML = MessageText;
    $("#noty_topRight_layout_container").slideToggle();
    setTimeout(function () { $("#noty_topRight_layout_container").slideToggle(); }, 4000);
}
function customAlertSuccess(MessageText) {
    $("#noty_topRight_layout_container").css("background-color", "green");
    $("#noty_topRight_layout_container")[0].children[0].children[0].children[0].innerHTML = MessageText;
    $("#noty_topRight_layout_container").slideToggle();
    setTimeout(function () { $("#noty_topRight_layout_container").slideToggle(); }, 4000);
}
function customAlertforlongtime(MessageText) {

    if (MessageText.indexOf("added") != -1 || MessageText.indexOf("updated") != -1 || MessageText.indexOf("successfully") != -1) {
        $("#noty_topRight_layout_container").css("background-color", "green");
    }
    else {
        $("#noty_topRight_layout_container").css("background-color", "#a94442");
    }
    $("#noty_topRight_layout_container")[0].children[0].children[0].children[0].innerHTML = MessageText;
    $("#noty_topRight_layout_container").slideToggle();
    setTimeout(function () { $("#noty_topRight_layout_container").slideToggle(); }, 6000);
}
function plainValidation(id) {

    $(id).focus();
    $(id).css("background-color", "pink");
}
function cleanValidation() {

    $('input,textarea,select').each(function () {

        $(this).css("background-color", "");
    });
}
//Error Function End//



if (historicalRoleArray.indexOf(roleId.val()) > -1) {
    $('#attendanceHeading').html('Center Based Historical');
}
else {
    $('#attendanceHeading').html('Daily Attendance');
}
//on change of Center select option//

center_para.children('select').on('change', function () {

    if ($(this).val() === '0') {

        class_para.find('select').find('option').remove();

        //$('#teacherName_div').hide();
        //$('#fswName_div').hide();
        //$('#working-days-div').hide();
        //$('#working-time-div').hide();
        //$('#offline-hist-div').hide();
        //$('#attendance-div').hide();
        //  return false;

        $('#datetimepicker1').val(weeklyAttendance.getFormattedDate(new Date()));
         
    }
    else {
        weeklyAttendance.GetClassrooms(this);
    }

    $('#teacherName_div,#fswName_div,#working-days-div,#working-time-div,#offline-hist-div,#attendance-div,#no-data-div').hide();



});



//on change of class select option//
class_para.children('select').on('change', function () {


    var self = $(this);
    if (self.val() === "0" || self.val() === "") {
        working_time_div.hide();
        working_days_div.hide();
        off_hist_div.hide();
        attendance_div.hide();
        no_data_div.hide();
        $('#datetimepicker1').val(weeklyAttendance.getFormattedDate(new Date()));
    }

    else {

        weeklyAttendance.ShowBusy(true);

        weeklyAttendance.GetFSWandTeacher();

        bindWorkingDaysOnClassChange(this);

        if (historicalRoleArray.indexOf(roleId.val()) > -1) {
            // $('#attendanceHeading').html('Center Based Historical');
            off_hist_div.find('input[name=attentyperadio][value=1]').trigger('click');
        }
        else {
            // $('#attendanceHeading').html('Daily Attendance');
            off_hist_div.show();

        }
    }
    showHideClassClosedToday(this);
 //   weeklyAttendance.ShowBusy(false);

});


$('#back-to-roster').on('click', function () {
    if (isOnLine()) {
        if (historicalRoleArray.indexOf(roleId.val()) > -1) {
            if (roleId.val() == 'b4d86d72-0b86-41b2-adc4-5ccce7e9775b') {
                window.location.href = '/Home/Dashboard';
            }
            else if (roleId.val() == 'b65759ba-4813-4906-9a69-e180156e42fc') {
                window.location.href = '/ERSEA/ERSEADashboard';
            }
            else if (roleId.val() == 'c352f959-cfd5-4902-a529-71de1f4824cc') {
                window.location.href = '/Home/Agencyuserdashboard';
            }
            else {
                window.location.href = '/Home/AgencyAdminDashboard';
            }
        }
        else {
            window.location.href = '/Teacher/Roster';

        }
    }
    else {
        customAlert('Your device is in offline');
    }
});
//On click of attendance type radio//
$('#offline-hist-div').find('input[name=attentyperadio]').on('click', function () {


      
    var center_class = weeklyAttendance.getCenterClassId();
    var attendanceDates = '';
    //if (weeklyAttendance.isHistorical()) {
    if ($(this).val() === '1') {
        //  var weekDates = weeklyAttendance.getWeekDatesFormatted(new Date());
        var weekDates=weeklyAttendance.getWeekDatesFormatted(new Date($('#datetimepicker1').val()))
        $.each(weekDates, function (l, dates) {

            if ((weekDates.length - 1) == l) {
                attendanceDates += dates;
            }
            else {
                attendanceDates += dates + ',';
            }
        });
    }
    else {
        attendanceDates = weeklyAttendance.getFormattedDate(new Date());
    }

    $.ajax({

        url: '/Teacher/GetChildListForCenterBased',
        type: 'post',
        datatype: 'json',
        beforeSend:function(){ weeklyAttendance.ShowBusy(true);},
        data: { centerId: center_class.enc_CenterId.toString(), classroomId: center_class.enc_ClassRoomId.toString(), isHistorical: weeklyAttendance.isHistorical(), attendanceDate: attendanceDates },
        success: function (data) {

          //  weeklyAttendance.ShowBusy(false);
            attendanceString.val(data.WeeklyAttendancestring);
            childAttendanceJson = JSON.parse(data.WeeklyAttendancestring);
            childString.val(data.ChildInfoString);
            centerInfoString.val(data.CenterString);
            $('#absenceListString').val(data.AbsenceReasonString);
             

            weeklyAttendance.bindAllHiddenData();

            loadWeeklyDailyAttendance();


        },
        error: function (data) {
            weeklyAttendance.ShowBusy(false);
        }

    });




    return true;

});

//On check of checkbox in working days div//
$('#working-days-check').find('input[type=checkbox]').on('change', function () {
    hideShowHistoricalDiv(this);
});

$('#datetimepicker12').on('click', function () {

    $('#datetimepicker1').datetimepicker('show');
});

//function to show or hide respective attendance section on check of checkbox
function hideShowHistoricalDiv(val) {
    //var weekly_attendance = $('#weekDaysAttendance');
    var self = $(val);
    var dayIndex = self.attr('day');
    if (self.is(':checked')) {
        $('#workingDaysheader').find('p[day=' + dayIndex + ']').addClass('selectd-day');
        var date = weekAttendance_div.find('#day-heading-row').children('td[day=' + dayIndex + ']').attr('date');

        if (new Date(date) < new Date(new Date().setHours(0, 0, 0, 0))) {

            weekAttendance_div.find('#day-heading-row').children('td[day=' + dayIndex + ']').show();
            weekAttendance_div.find('#day-in-out-row').children('td[day=' + dayIndex + ']').show();
            weekAttendance_div.find('.day-table[day=' + dayIndex + ']').parent('td').show();

            weekAttendance_div.find('.day-table[day=' + dayIndex + ']').find('.att-tr').find('.in-time-div').show();

            weekAttendance_div.find('.day-table[day=' + dayIndex + ']').find('.att-tr').find('.out-time-div').show();
            weekAttendance_div.find('.day-table[day=' + dayIndex + ']').find('.att-tr').find('.meals-row').show();

            weekAttendance_div.find('#child-meals-row').find('table[day=' + dayIndex + ']').parent('td').show();
            weekAttendance_div.find('#adult-meals-row').find('table[day=' + dayIndex + ']').parent('td').show();
            weekAttendance_div.find('#daily-attendance-count').find('table[day=' + dayIndex + ']').parent('td').show();
        }
        else {
            weekAttendance_div.find('#day-heading-row').children('td[day=' + dayIndex + ']').hide();
            weekAttendance_div.find('#day-in-out-row').children('td[day=' + dayIndex + ']').hide();
            weekAttendance_div.find('.day-table[day=' + dayIndex + ']').parent('td').hide();
            weekAttendance_div.find('#child-meals-row').find('table[day=' + dayIndex + ']').parent('td').hide();
            weekAttendance_div.find('#adult-meals-row').find('table[day=' + dayIndex + ']').parent('td').hide();
            weekAttendance_div.find('#daily-attendance-count').find('table[day=' + dayIndex + ']').parent('td').hide();
        }
    }
    else {
        $('#workingDaysheader').find('p[day=' + dayIndex + ']').removeClass('selectd-day');
        weekAttendance_div.find('#day-heading-row').children('td[day=' + dayIndex + ']').hide();
        weekAttendance_div.find('#day-in-out-row').children('td[day=' + dayIndex + ']').hide();
        weekAttendance_div.find('.day-table[day=' + dayIndex + ']').parent('td').hide();
        weekAttendance_div.find('#child-meals-row').find('table[day=' + dayIndex + ']').parent('td').hide();
        weekAttendance_div.find('#adult-meals-row').find('table[day=' + dayIndex + ']').parent('td').hide();
        weekAttendance_div.find('#daily-attendance-count').find('table[day=' + dayIndex + ']').parent('td').hide();

        weekAttendance_div.find('.day-table[day=' + dayIndex + ']').find('.att-tr').each(function () {

            var rowIndex = $(this).attr('row-index');
            var day = $(this).attr('day');
            var attendanceDate = weeklyAttendance.getAttendanceDate(day);
            // var dailyRow=weekAttendance_div.find('#daily-attendance-count').find('table[day=' + day + ']').find('tr[day=' + day + ']').find('.present-count')
            $(this).find('.time-text').val('').css({ 'display': 'none' });

            $(this).find('.time-td').each(function (k, timeradiotd) {

                $(timeradiotd).find('input[type=radio]:checked').each(function (r, radio) {

                    //if ($(radio).is(':checked')) {
                    $(radio).prop('checked', false);
                    var hisIndex = getDatabaseIndex(rowIndex, attendanceDate);
                    DataBaseManager.DeleteUser(hisIndex, false);


                    //}
                });

            });

            $(this).find('.meals-td').find('.meals-check:checked').each(function (l, meals) {
                $(meals).prop('checked', false);
            });


        });

        var mealsIndex = getDailyMealsIndex(dayIndex, weeklyAttendance.getAttendanceDate(dayIndex));
        DataBaseManager.DeleteMeals(mealsIndex, false);

        var adultMealsRow = weekAttendance_div.find('#adult-meals-row').find('table[day=' + dayIndex + ']').find('tr[day=' + dayIndex + ']');
        adultMealsRow.find('.adult-breakfastcout').val(0);
        adultMealsRow.find('.adult-lunchcount').val(0);
        adultMealsRow.find('.adult-snackscount').val(0);

        updateDailyMealsAndAttendance(dayIndex, false);
    }

    setWidthofAttendanceTable();
}



function setWidthofAttendanceTable() {

    if (weeklyAttendance.isHistorical()) {
        var attendanceDaysLength = weekAttendance_div.find('#day-heading-row').children(':visible').length;
        if (attendanceDaysLength > 0 && attendanceDaysLength == 1) {
            weekAttendance_div.find('#table-width-change').css({ 'width': '100%' });
        }
        else {
            weekAttendance_div.find('#table-width-change').css({ 'width': '2100px' });

        }
    }
}


jQuery.fn.extend({
    bindAbsenceReason: function () {
        var absenceList = $('#absenceListString').val();
        var reasonJson = {};
        var selectString = '<option value="0">--Select Reason--</option>';
        if (absenceList != '') {
            reasonJson = JSON.parse(absenceList);
        }
        if (reasonJson.length > 0) {
            $.map(reasonJson, function (reason, i) {

                selectString += '<option value=' + reason.Value + '>' + reason.Text + '</option>';
            });
        }

        return $(this).html(selectString);
    }
    //uncheck: function () {
    //    return this.each(function () {
    //        this.checked = false;
    //    });
    //}
});

function getAbsenceReason() {
    var absenceList = $('#absenceListString').val();
    var reasonJson = {};
    if (absenceList != '') {
        reasonJson = JSON.parse(absenceList);
    }
    return reasonJson;
}

function hideFutureDaysAttendance() {

    var timepickerValue = $('#datetimepicker1').val();

    var weekDates = [];
    var futureDates = [];
    weekDates = getDates(timepickerValue);
    if (weekDates.length > 0) {
        futureDates = $.grep(weekDates, function (n, i) {
            return n >= new Date();
        });
        if (futureDates.length > 0) {

            $.each(futureDates, function (k, date) {

                var futureDate = weeklyAttendance.getFormattedDate(date);
                var future_day_index = weekAttendance_div.find('#day-heading-row').children('td[date="' + futureDate + '"]').attr('day');

                weekAttendance_div.find('#day-heading-row').children('td[day=' + future_day_index + ']').hide();
                weekAttendance_div.find('#day-in-out-row').children('td[day=' + future_day_index + ']').hide();
                weekAttendance_div.find('.day-table[day=' + future_day_index + ']').parent('td').hide();

                weekAttendance_div.find('#child-meals-row').find('table[day=' + future_day_index + ']').parent('td').hide();
                weekAttendance_div.find('#adult-meals-row').find('table[day=' + future_day_index + ']').parent('td').hide();
                weekAttendance_div.find('#daily-attendance-count').find('table[day=' + future_day_index + ']').parent('td').hide();

            });
        }
        else {

            $.each(weekDates, function (l, week) {
                var weekDate = weeklyAttendance.getFormattedDate(week);
                var weekDateIndex = weekAttendance_div.find('#day-heading-row').children('td[date="' + weekDate + '"]').attr('day');
                weekAttendance_div.find('#day-heading-row').children('td[day=' + weekDateIndex + ']').show();
                weekAttendance_div.find('#day-in-out-row').children('td[day=' + weekDateIndex + ']').show();
                weekAttendance_div.find('.day-table[day=' + weekDateIndex + ']').parent('td').show();

                weekAttendance_div.find('#child-meals-row').find('table[day=' + weekDateIndex + ']').parent('td').show();
                weekAttendance_div.find('#adult-meals-row').find('table[day=' + weekDateIndex + ']').parent('td').show();
                weekAttendance_div.find('#daily-attendance-count').find('table[day=' + weekDateIndex + ']').parent('td').show();
            });

        }
    }

    showHideDaysBasedOncheck();
}

//function to load the weekly attendance//
function loadWeeklyDailyAttendance() {

   // weeklyAttendance.ShowBusy(true);

    var childString = weeklyAttendance.getChildInformation();
    var childList = weeklyAttendance.getChildInfoList();

    if (childString != '') {
        var childAttendAppend = '';

        if (weeklyAttendance.isHistorical()) {
            $('#push-to-server').show();
            working_days_div.show();
            weekly_child_info_body.html(childString);

            var timePara = working_time_div.find('#working-time-para').html();
            var inTimeDefault = timePara.split('-')[0].trim();
            var outTimeDefault = timePara.split('-')[1].trim();

            //Bind Child Attendance Info//
            weekAttendance_div.find('.day-table').each(function (j, daytab) {
                childAttendAppend = '';


                    if (childList.length > 0) {
                        $.each(childList, function (k, dayChild) {

                            var rowindex = j + '' + k;
                            var accessDaysArray = [];
                            var isShowDetails = false;
                            var iterativeDate = '';
                            var display = '';


                            accessDaysArray = dayChild.AccessDateString.split(',');
                            if (weeklyAttendance.isHistorical()) {

                                iterativeDate = weeklyAttendance.getFormattedDate(getDates($('#datetimepicker1').val())[parseInt($(daytab).attr('day'))]);
                            }
                            else {

                            }

                            isShowDetails = (accessDaysArray.indexOf(iterativeDate) > -1) ? true : false;
                            display = (isShowDetails) ? 'block' : 'none';

                            childAttendAppend += '<tr class="att-tr" clientid=' + dayChild.Enc_ClientId + ' day=' + j + ' row-index=' + rowindex + ' >' +
                                                                    '<td class="weekly-table-text-al1 time-td in-time-td" client-id=' + dayChild.Enc_ClientId + '>';
                            if (isShowDetails) {
                                childAttendAppend += '<div class="weekly-div-radio-btn3  in-time-div col-xs-12 no-padding">' +
                         '<div class="radio radio-info">' +
                             '<input type="radio" name="intimeradio_' + rowindex + '" id="presentin_' + dayChild.ClientID + '" value="1">' +
                            '<label>P</label></div>';


                                if (dayChild.Dateofclassstartdate.split(',').indexOf(iterativeDate) > -1) {

                                    childAttendAppend += '<div class="radio radio-info" style="display:none;">' +
                             '<input type="radio" name="intimeradio_' + rowindex + '" id="presentin_' + dayChild.ClientID + '" value="2">' +
                             '<label>A</label>' +
                         '</div>' +
                         '<div class="radio radio-info"  style="display:none;">' +
                             '<input type="radio" name="intimeradio_' + rowindex + '"  id="presentin_' + dayChild.ClientID + '" value="3">' +
                             '<label>N</label>' +
                         '</div>';



                                }
                                else {


                                    childAttendAppend += '<div class="radio radio-info">' +
                 '<input type="radio" name="intimeradio_' + rowindex + '" id="presentin_' + dayChild.ClientID + '" value="2">' +
                 '<label>A</label>' +
             '</div>' +
             '<div class="radio radio-info">' +
                 '<input type="radio" name="intimeradio_' + rowindex + '"  id="presentin_' + dayChild.ClientID + '" value="3">' +
                 '<label>N</label>' +
             '</div>';
                                }



                                childAttendAppend+=  '</div>';
                            }
                            else {
                                childAttendAppend += '<div class="weekly-div-radio-btn3  in-time-div col-xs-12 no-padding">' +
                      '<div class="radio radio-info" style="display:none;">' +
                          '<input type="radio" name="intimeradio_' + rowindex + '" id="presentin_' + dayChild.ClientID + '" value="1">' +
                         '<label>P</label>' +
                      '</div>' +
                      '<div class="radio radio-info" style="display:none;">' +
                          '<input type="radio" name="intimeradio_' + rowindex + '" id="presentin_' + dayChild.ClientID + '" value="2">' +
                          '<label>A</label>' +
                      '</div>' +
                      '<div class="radio radio-info" style="display:none;">' +
                          '<input type="radio" name="intimeradio_' + rowindex + '"  id="presentin_' + dayChild.ClientID + '" value="3">' +
                          '<label>N</label>' +
                      '</div>' +
                  '</div>';
                            }


                            childAttendAppend += '<input in-attr=' + dayChild.Enc_ClientId + ' id="' + rowindex + 'DayIn_' + dayChild.ClientID + '" type="text" class="in-time time-text"  value="' + inTimeDefault + '"  placeholder="HH:mm" style="">' +
                      '<button style="display:none;" id="firstDayReasonIn_' + rowindex + dayChild.ClientID + '" onclick="bindReasonSourceBtn(this);" reason-id="0"  class="btn-reason btn-reason-normal" data-toggle="modal" data-target="#addReasonModal">Add Reason</button>' +
                    '</td>' +
                    '<td class="weekly-table-text-al1 time-td out-time-td" client-id=' + dayChild.Enc_ClientId + '>' +
                        '<input type="text" id="' + rowindex + 'DayOut_' + dayChild.ClientID + '" out-attr=' + dayChild.Enc_ClientId + ' class="out-time time-text"  value="' + outTimeDefault + '"   placeholder="HH:mm" style="">' +
                    '</td>' +
                    '<td style="width:87px;" class="meals-td weekly-table-text-al1">';
                            if (isShowDetails) {
                                childAttendAppend += '<table width="100%" border="0" cellspacing="0" cellpadding="0"><tbody>';
                            }
                            else {
                                childAttendAppend += '<table width="100%" border="0" cellspacing="0" cellpadding="0" style="display:none;"><tbody>';
                            }


                            childAttendAppend += '<tr class="meals-row"><td><p><input type="checkbox" name="" value="1" class="meals-check break-check"   style="position: relative;top: 2px;margin-right:2px;" id="fveDayB_' + dayChild.ClientID + '">B</p></td>' +
                                '<td><p><input type="checkbox" name="" value="2" class="meals-check lunch-check"    style="position: relative;top: 2px;margin-right:2px;" id="fveDayL_' + dayChild.ClientID + '">L</p></td>' +
                                '<td><p><input type="checkbox" name="" value="3" class="meals-check snack-check"  style="position: relative;top: 2px;margin-right:2px;" id="fveDayS_' + dayChild.ClientID + '">S</p></td>' +
                            '</tr>' +
                       '</tbody>' +
                    '</table>' +
                '</td>' +
            '</tr>';

                        });

                        $(daytab).children('#day-body').html(childAttendAppend);
                        $(daytab).children('#day-body').find('.meals-check').on('change', function () { weeklyAttendance.onCheckMeals(this); })
                    }

                });

                //method to set the date for week days//
                setWeekDays($('#datetimepicker1').val());

            //method to bind the attendance data for week clients//
            DataBaseManager.GetAllClient(setTableOnDateChange);

            weekAttendance_div.find('.in-time-td').find('input[type=radio]').on('click', function () {

                 
                  
                var self = $(this);
                var selfTimeText = self.parent().parent().siblings('.in-time');
                //var selfReasonSelect = self.parent().parent().siblings('#absenceReasonSelect');
                if (self.val() === "1") {
                    weeklyAttendance.initializeDatetimePicker(selfTimeText.attr('id'));

                    selfTimeText.val(selfTimeText.attr('value')).show();
                    selfTimeText.change(function () {
                        weeklyAttendance.timeChangeUpdate(this);
                    });
                    self.parent().parent().parent('.in-time-td').siblings('.meals-td').find('.meals-row').find('.meals-check').prop('checked', false);
                    self.parent().parent().parent('.in-time-td').siblings('.meals-td').find('.meals-row').show(); +
                    self.parent().parent().parent().parent('.att-tr').find('.in-time-td').find('.btn-reason').attr('reason-id', '0').removeClass('btn-reason-success').addClass('btn-reason-normal').hide().html('Add Reason');
                    var otherTimeText = self.parent().parent().parent('.in-time-td').siblings('.out-time-td').find('.out-time');
                    weeklyAttendance.initializeDatetimePicker(otherTimeText.attr('id'));
                    otherTimeText.val(otherTimeText.attr('value')).show();
                    otherTimeText.change(function () {
                        weeklyAttendance.timeChangeUpdate(this);
                    });


                    // bindAbsenceReason(selfReasonSelect);
                }
                else if (self.val() === '2') {
                    selfTimeText.val('').hide();
                    self.parent().parent().parent('.in-time-td').siblings('.out-time-td').find('.out-time').val('').hide();
                    self.parent().parent().parent('.in-time-td').siblings('.meals-td').find('.meals-row').find('.meals-check').prop('checked', false);
                    self.parent().parent().parent('.in-time-td').siblings('.meals-td').find('.meals-row').hide();
                    self.parent().parent().parent().parent('.att-tr').find('.in-time-td').find('.btn-reason').attr('reason-id', '0').removeClass('btn-reason-success').addClass('btn-reason-normal').show().html('Add Reason');
                    // self.parent().parent().siblings('#absenceReasonSelect').bindAbsenceReason().show();
                    self.parent().parent().siblings('.btn-reason').show();


                }
                else {

                    selfTimeText.val('').hide();
                    self.parent().parent().parent('.in-time-td').siblings('.out-time-td').find('.out-time').val('').hide();
                    self.parent().parent().parent('.in-time-td').siblings('.meals-td').find('.meals-row').find('.meals-check').prop('checked', false);
                    self.parent().parent().parent().parent('.att-tr').find('.in-time-td').find('.btn-reason').attr('reason-id', '0').removeClass('btn-reason-success').addClass('btn-reason-normal').hide().html('Add Reason');
                    self.parent().parent().parent('.in-time-td').siblings('.meals-td').find('.meals-row').hide();
                }

                //  checkAnotherRadio(self);

                insertOnTimeRadio(self);

            });

            //weekAttendance_div.find('.out-time-td').find('input[type=radio]').on('click', function () {

            //    var self2 = $(this);
            //    if (self2.val() === "1") {
            //        self2.parent().parent().siblings('.time-text').val('').show();
            //        self2.parent().parent().parent('.out-time-td').siblings('.meals-td ').find('.meals-row').show();
            //    }
            //    else {

            //        self2.parent().parent().siblings('.time-text').val('').hide();
            //        self2.parent().parent().parent('.out-time-td').siblings('.meals-td ').find('.meals-row').find('.meals-check').prop('checked', false);
            //        self2.parent().parent().parent('.out-time-td').siblings('.meals-td ').find('.meals-row').hide();
            //    }

            //    checkAnotherRadio(self2);

            //    insertOnTimeRadio(self2);
            //});


            working_days_check.find('input[type=checkbox]').each(function () {
                hideShowHistoricalDiv(this);

            });

            //method to hide the future dates in historical data entry//
            // hideFutureDaysAttendance();

            showAttendanceDiv(1);
        }
        else {

            if ($('#agency-closed-info').length > 0 && $('#agency-closed-info').is(':visible')) {

                attendance_div.hide();
                no_data_div.html($('#agency-closed-info').html()).show();
                return false;
            }

            else {

                $('#push-to-server').hide();
                working_days_div.hide();
                offline_child_info_body.html(childString);

                //Bind Child Attendance Info//
                offlineAttendance_div.find('.day-table').each(function (j, daytab) {
                    childAttendAppend = '';

                    if (childList.length > 0) {
                        $.each(childList, function (k, dayChild) {
                            var rowindex = j + '' + k;
                            childAttendAppend += '<tr class="att-tr" clientid=' + dayChild.Enc_ClientId + ' day=' + j + ' row-index=' + rowindex + ' >\
                                                                    <td class="weekly-table-text-al1 offline-in-time-td time-td in-time-td" style="text-align:center;" client-id=' + dayChild.Enc_ClientId + '>\
                                                                        <div class="weekly-div-radio-btn3 col-xs-12 no-padding" style="text-align:center;">\
                                                                            <div class="radio radio-info" style="float:none;">\
                                                                                <input type="radio" name="intimeradio_' + rowindex + '" id="presentin_' + dayChild.ClientID + '" value="1">\
                                                                                <label>P</label>\
                                                                            </div>\
                                                                            <div class="radio radio-info" style="float:none;">\
                                                                                <input type="radio" name="intimeradio_' + rowindex + '" id="presentin_' + dayChild.ClientID + '" value="2">\
                                                                                <label>A</label>\
                                                                            </div>\
                                                                            <div class="radio radio-info" style="float:none;">\
                                                                                <input type="radio" name="intimeradio_' + rowindex + '"  id="presentin_' + dayChild.ClientID + '" value="3">\
                                                                                <label>N</label>\
                                                                            </div>\
                                                                        </div>\
                                                                        <button style="cursor: pointer;width: auto;height: 20px;text-align: center;margin-bottom: 5px;padding: 0px;font-size: 12px;line-height: 9px;display: inline-block;padding: 5px;margin-right: 0px;" id="firstDayPSIn_' + dayChild.ClientID + '" onclick="bindSourceBtnId(this);" isparent="1" class="addSignature normal-sig-btn tools parent-sig-btn" data-toggle="modal" data-target="#drawSignatureModal">Parent Sign</button>\
                                                                        <button style="cursor: pointer;width: auto;height: auto;text-align: center;margin-bottom: 5px;padding: 0px;font-size: 12px;line-height: 9px;display: inline-block;padding: 5px;" id="firstDayTSIn_' + dayChild.ClientID + '" onclick="bindSourceBtnId(this);" isparent="0" class="addSignature normal-sig-btn tools teacher-sig-btn" data-toggle="modal" data-target="#drawSignatureModal">Teacher Sign</button>\
                                                                  <button  style="display:none;" id="offlineReasonIn_' + dayChild.ClientID + '" onclick="bindReasonSourceBtn(this);"  reason-id="0"  class="btn-reason btn-reason-normal" data-toggle="modal" data-target="#addReasonModal">Add Reason</button>\
                                                                  <input type="text" id="sig_firstDayPSIn__' + dayChild.ClientID + '" class="sig-parent-hidden" style="display:none;">\
                                                                   <input type="text" id="sig_firstDayTSIn__' + dayChild.ClientID + '" class="sig-teacher-hidden" style="display:none;">\
                                                                  </td>\
                                                                    <td style="width:87px;" class="weekly-table-text-al1 offline-intime-input">\
                                                                        <input in-attr=' + dayChild.Enc_ClientId + ' id="firstDayIn_' + dayChild.ClientID + '" type="text" class="in-time time-text" disabled placeholder="HH:mm" style="">\
                                                                        </td>\
                                                                    <td class="weekly-table-text-al1 time-td out-time-td offline-out-time-td" style="text-align:center;" client-id=' + dayChild.Enc_ClientId + '>\
                                                                     <button style="cursor: pointer;width: auto;height: 20px;text-align: center;margin-bottom: 5px;padding: 0px;font-size: 12px;line-height: 9px;display: inline-block;padding: 5px;margin-right: 0px;" id="firstDayPSOut_' + dayChild.ClientID + '" onclick="bindSourceBtnId(this);" isparent="1" class="addSignature normal-sig-btn tools parent-sig-btn" data-toggle="modal" data-target="#drawSignatureModal">Parent Sign</button>\
                                                                   <input type="text" id="sig_firstDayPSOut__' + dayChild.ClientID + '" class="sig-parent-hidden" style="display:none;">\
                                                                  </td>\
                                                                    <td style="width:87px;" class="weekly-table-text-al1 offline-outtime-input">\
                                                                        <input type="text" id="firstDayOut_' + dayChild.ClientID + '" out-attr=' + dayChild.Enc_ClientId + ' class="out-time time-text" disabled placeholder="HH:mm" style="">\
                                                                     </td>\
                                                                    <td style="width:87px;" class="weekly-table-text-al1 meals-td" >\
                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">\
                                                                            <tbody>\
                                                                                <tr class="meals-row">\
                                                                                    <td><p><input type="checkbox" name="" value="1" class="meals-check break-check"  style="position: relative;top: 2px;margin-right:2px;" id="fveDayB_' + dayChild.ClientID + '">B</p></td>\
                                                                                    <td><p><input type="checkbox" name="" value="2" class="meals-check lunch-check"   style="position: relative;top: 2px;margin-right:2px;" id="fveDayL_' + dayChild.ClientID + '">L</p></td>\
                                                                                    <td><p><input type="checkbox" name="" value="3" class="meals-check snack-check"  style="position: relative;top: 2px;margin-right:2px;" id="fveDayS_' + dayChild.ClientID + '">S</p></td>\
                                                                                </tr>\
                                                                            </tbody>\
                                                                        </table>\
                                                                    </td>\
                                                                </tr>';

                        });

                        $(daytab).children('#day-body').html(childAttendAppend);

                        $(daytab).children('#day-body').find('.meals-check').on('change', function () { weeklyAttendance.onCheckMeals(this); });


                    }

                });

                offlineAttendance_div.find('.offline-in-time-td').find('input[type=radio]').on('change', function () {
                    var self = $(this);
                    var selfTimeSig = self.parent().parent().siblings('.addSignature');

                    if (self.val() === "1") {
                        $('#drawSignatureModal').find('#sigParent').signature();
                        selfTimeSig.show();
                        //initializeDatetimePicker(selfTimeText)
                        //selfTimeText.val('').show();
                        self.parent().parent().parent().parent('.att-tr').find('.out-time-td').find('.addSignature').show();
                        self.parent().parent().parent().parent('.att-tr').find('.meals-td').find('.meals-row').show();
                        self.parent().parent().parent().parent('.att-tr').find('.meals-td').find('.meals-row').find('.meals-check').prop('checked', false);
                        self.parent().parent().parent().parent('.att-tr').find('.in-time-td').find('.btn-reason').attr('reason-id', '0').removeClass('btn-reason-success').addClass('btn-reason-normal').hide().html('Add Reason');

                    }
                    else if (self.val() === '2') {
                        selfTimeSig.removeClass('added-sig-btn').addClass('normal-sig-btn').hide();
                        selfTimeSig.siblings('.sig-parent-hidden,.sig-teacher-hidden').val('');

                        self.parent().parent().parent().parent('.att-tr').find('.out-time-td').find('.addSignature').removeClass('added-sig-btn').addClass('normal-sig-btn').hide();
                        self.parent().parent().parent().parent('.att-tr').find('.out-time-td').find('.addSignature').siblings('.sig-parent-hidden,.sig-teacher-hidden').val('');
                        self.parent().parent().parent().parent('.att-tr').find('.in-time-td').siblings('td').find('.in-time').val('').hide();
                        self.parent().parent().parent().parent('.att-tr').find('.in-time-td').find('.btn-reason').attr('reason-id', '0').removeClass('btn-reason-success').addClass('btn-reason-normal').show().html('Add Reason');
                        self.parent().parent().parent().parent('.att-tr').find('.out-time-td').siblings('td').find('.out-time').val('').hide();
                        self.parent().parent().parent().parent('.att-tr').find('.meals-td').find('.meals-row').find('.meals-check').prop('checked', false);
                        self.parent().parent().parent().parent('.att-tr').find('.meals-td').find('.meals-row').hide();

                    }
                    else {

                        selfTimeSig.removeClass('added-sig-btn').addClass('normal-sig-btn').hide();
                        selfTimeSig.siblings('.sig-parent-hidden,.sig-teacher-hidden').val('');
                        self.parent().parent().parent().parent('.att-tr').find('.out-time-td').find('.addSignature').removeClass('added-sig-btn').addClass('normal-sig-btn').hide();
                        self.parent().parent().parent().parent('.att-tr').find('.out-time-td').find('.addSignature').siblings('.sig-parent-hidden,.sig-teacher-hidden').val('');
                        self.parent().parent().parent().parent('.att-tr').find('.in-time-td').siblings('td').find('.in-time').val('').hide();
                        self.parent().parent().parent().parent('.att-tr').find('.out-time-td').siblings('td').find('.out-time').val('').hide();
                        self.parent().parent().parent().parent('.att-tr').find('.meals-td').find('.meals-row').find('.meals-check').prop('checked', false);
                        self.parent().parent().parent().parent('.att-tr').find('.meals-td').find('.meals-row').hide();
                        self.parent().parent().parent().parent('.att-tr').find('.in-time-td').find('.btn-reason').attr('reason-id', '0').removeClass('btn-reason-success').addClass('btn-reason-normal').hide().html('Add Reason');

                    }

                    var rowheight = self.parent().parent().parent().parent('.att-tr').height();
                    var rowIndex = self.parent().parent().parent().parent('.att-tr').attr('row-index').substring(1);

                    offlineAttendance_div.find('#offline-child-info-tbody').children('#childinfo_' + rowIndex + '').height(rowheight);

                    //checkAnotherRadio(self);
                    insertOnTimeRadio(self);
                });




                //offlineAttendance_div.find('.offline-out-time-td').find('input[type=radio]').on('click', function () {

                //    var self2 = $(this);
                //    var parentRow = self2.parent().parent().parent().parent('.att-tr');
                //    if (self2.val() === "1") {
                //        $('#drawSignatureModal').find('#sigParent').signature();
                //        self2.parent().parent().siblings('.addSignature').show();
                //        parentRow.find('.offline-out-time-td').find('.addSignature ').show();
                //        parentRow.find('.meals-td').find('.meals-row').show();
                //    }
                //    else {

                //        self2.parent().parent().siblings('.addSignature').hide();
                //        parentRow.find('.offline-out-time-td').find('.addSignature ').hide();
                //        self2.parent().parent().parent().parent('.att-tr').find('.meals-td').find('.meals-row').hide();
                //    }

                //    // checkAnotherRadio(self2);
                //    insertOnTimeRadio(self2);
                //});

                var today_day = (new Date($('#datetimepicker1').val()).getDay()) - 1;



                //if ($('#working-days-check').find('input[day=' + today_day + ']').is(':checked')) {
                //    offlineAttendance_div.find('#week-monday').html(setTodayDate(new Date())).attr('date', weeklyAttendance.getFormattedDate(new Date()));
                //    DataBaseManager.GetAllClient(setTableOnDateChange);
                //    showAttendanceDiv(2);
                //}
                //else {
                //    $('#attendance-div').hide();
                //    $('#no-data-div').html('Classroom is closed for today').show();
                //}

                offlineAttendance_div.find('#week-monday').html(setTodayDate(new Date())).attr('date', weeklyAttendance.getFormattedDate(new Date()));
                DataBaseManager.GetAllClient(setTableOnDateChange);
                showAttendanceDiv(2);

            }
        }

    }
    else {
        //  attendance_div.hide();
        if (weeklyAttendance.isHistorical()) {
            if (weekAttendance_div.is(':visible')) {
                weekAttendance_div.hide();
            }
        }
        no_data_div.html('No records found').show();
    }

   weeklyAttendance.ShowBusy(false);
}


function showAttendanceDiv(type) {
    if (type === 1) {
        $('#singleDayAttance').hide();
        attendance_div.show();
        //$('#submitForm').hide();
        $('#weekDaysAttendance').show();
        //  $('.weekly-date').show();
        $('#datetimepicker2').hide();
        $('#offlineSpan').show();
        $('#weeklyDateDiv').show();
        $('#offlineDateDiv').hide();

    }
    else {
        $('#offlineSpan').show();
        $('#singleDayAttance').find('.addSignature').hide();
        $('#weekDaysAttendance').hide();
        //$('#submitForm').hide();
        //  $('.weekly-date').show();

        $('#weeklyDateDiv').hide();
        $('#singleDayAttance').show();
        attendance_div.show();
        $('#offlineDateDiv').show();
        $('#datetimepicker2').val(weeklyAttendance.getFormattedDate(new Date())).show();
        $('#datetimepicker2').attr({ 'disabled': 'disabled' });


    }
}


function showHideDaysBasedOncheck() {

    $('#working-days-check').find('input[type=checkbox]').each(function () {
        hideShowHistoricalDiv(this);
    });
}


function checkValidation() {
        
    var error = false;
    if (weeklyAttendance.isHistorical()) {
        weekAttendance_div.find('.day-table').each(function (i, table) {

            $(table).find('.att-tr').each(function (j, row) {



                $(row).find('input[type=radio]:checked').each(function (k, radio) {

                    if ($(radio).attr('value') == '1') {
                        var time_text = $(radio).parent().parent().parent('.time-td').children('.time-text');
                        var time_text_out = $(radio).parent().parent().parent('.time-td').siblings('.out-time-td').children('.time-text');

                        if (time_text.val() == '') {
                            var textId = time_text.attr('id');
                            plainValidation('#' + textId + '');
                            error = true;
                        }
                        else if (time_text_out.val() == '') {
                            var textId = time_text_out.attr('id');
                            plainValidation('#' + textId + '');
                            error = true;
                        }
                    }

                    else if ($(radio).attr('value') == '2') {

                        var buttonReason = $(radio).parent().parent().parent('.time-td').children('.btn-reason');
                        var hasSuccess = buttonReason.hasClass('btn-reason-success');
                        if (!hasSuccess) {
                            var reasonAttrId = buttonReason.attr('id');

                            plainValidation('#' + reasonAttrId + '');
                            error = true;
                        }
                    }
                });
            });
        });
    }

    else {
        offlineAttendance_div.find('.day-table').each(function (i, table) {

            $(table).find('.att-tr').each(function (j, row) {

                $(row).find('input[type=radio][value=1]:checked').each(function (k, radio) {

                    if ($(radio).attr('value') == '1') {
                        var timeTd = $(radio).parent().parent().parent('.time-td');
                        var parentSign = timeTd.children('.sig-parent-hidden');
                        var teacherSign = timeTd.children('.sig-teacher-hidden');
                        if (parentSign.val() == '') {
                            //var textId = time_text.attr('id');
                            //plainValidation('#' + textId + '');
                            timeTd.find('.parent-sig-btn').removeClass('normal-sig-btn,added-sig-btn').addClass('err-sig-btn');
                            error = true;
                        }
                        if (teacherSign.val() == '') {
                            timeTd.find('.teacher-sig-btn').removeClass('normal-sig-btn,added-sig-btn').addClass('err-sig-btn');
                            error = true;
                        }
                    }
                });
            });
        });
    }
    return error;
}


function checkAnotherRadio(selfRadio) {
    var outradio = selfRadio.parent().parent().parent('td').siblings('td').find('input[type=radio]');

    selfRadio.parent().parent().parent('td').siblings('td').find('input[type=radio][value=' + selfRadio.val() + ']').prop('checked', true);
    if (weeklyAttendance.isHistorical()) {

        if (selfRadio.val() === '1') {
            selfRadio.parent().parent().parent('td').siblings('td').find('.weekly-div-radio-btn3').css('display', 'block');

            weeklyAttendance.initializeDatetimePicker(outradio.parent().parent().siblings('.time-text').attr('id'));
            outradio.parent().parent().siblings('.time-text').show();
            outradio.parent().parent().siblings('.time-text').change(function () {
                cleanValidation('#' + $(this).attr('id') + '');
                weeklyAttendance.timeChangeUpdate(this);
            });

        }

        else {
            outradio.parent().parent().siblings('.time-text').hide();
            selfRadio.parent().parent().parent('td').siblings('td').find('.weekly-div-radio-btn3').css('display', 'none');

        }
    }
    else {

        if (selfRadio.val() === '1') {
            $('#sigParent').signature();
            selfRadio.parent().parent().parent('td').siblings('td').find('.weekly-div-radio-btn3').css('display', 'block');
            outradio.parent().parent().siblings('.addSignature').show();
        }

        else {
            outradio.parent().parent().siblings('.addSignature').hide();
            selfRadio.parent().parent().parent('td').siblings('td').find('.weekly-div-radio-btn3').css('display', 'none');

        }
    }

}

//$('#pustToserver').on('click', function () {


//});

if (navigator.onLine) {

    $('#network-status').css('background', '#4CAF50');
    $('#network-span').html('Online');
    $('#push-to-server').show();
}
else {
    $('#network-status').css('background', 'red');
    $('#network-span').html('Offline');
    $('#push-to-server').hide();
}



function checkSingleClassRoom() {
    if (centerInfoJson.length === 1) {
        if (centerInfoJson[0].Classroom.length === 1) {

            attendance_div.show();
        }
        else {
            attendance_div.hide();
        }

    }

}




function bindWorkingDaysOnClassChange(selectEle) {
    var workingDaysRow = $('#workingDaysheader');
    var working_time = $('option:selected', selectEle).attr('working-time');
    working_time_div.find('#working-time-para').html(working_time);
    working_time_div.show();
    workingDaysRow.find('p').removeClass('selectd-day');
    working_days_check.find('input[type=checkbox]').prop('checked', false);

    if ($('option:selected', selectEle).attr('mon').toLowerCase() === 'true') {
        workingDaysRow.find('p[day=0]').addClass('selectd-day');
        working_days_check.find('input[day=0]').prop('checked', true);
    }

    if ($('option:selected', selectEle).attr('tue').toLowerCase() === 'true') {
        workingDaysRow.find('p[day=1]').addClass('selectd-day');
        working_days_check.find('input[day=1]').prop('checked', true);
    }

    if ($('option:selected', selectEle).attr('wed').toLowerCase() === 'true') {
        workingDaysRow.find('p[day=2]').addClass('selectd-day');
        working_days_check.find('input[day=2]').prop('checked', true);
    }

    if ($('option:selected', selectEle).attr('thu').toLowerCase() === 'true') {
        workingDaysRow.find('p[day=3]').addClass('selectd-day');
        working_days_check.find('input[day=3]').prop('checked', true);
    }

    if ($('option:selected', selectEle).attr('fri').toLowerCase() === 'true') {
        workingDaysRow.find('p[day=4]').addClass('selectd-day');
        working_days_check.find('input[day=4]').prop('checked', true);
    }

    $('option:selected', selectEle).attr('working-time');

    working_days_div.show();

    if ($('#historicalatt').is(':checked') || $('#offlineatt').is(':checked')) {
        loadWeeklyDailyAttendance();
        //DataBaseManager.GetAllClient(setTableOnDateChange);
    }

}

function showHideClassClosedToday(selectEle) {

    if ($('option:selected', selectEle).val() == '0') {
        $('#closed-agency-section').html('').hide();
    }
    else {
        var closedType = parseInt($('option:selected', selectEle).attr('closed'));
        if (closedType > 0) {
            var closedHtml = '<div class="col-xs-12 no-padding" id="agencyClosedDiv" style="margin-top:25px;margin-bottom:-6px;">\
                                        <div class="col-xs-12">\
                                            <div class="education-content-desc1">\
                                            <p id="agency-closed-info">##AgencyContent##</p>\
                                            </div>\
                                        </div>\
                                    </div>';
            if (closedType == 1) {
                closedHtml = closedHtml.replace("##AgencyContent##", 'Agency has been closed today');
            }
            else if (closedType == 2) {
                var center = '';
                if ($('#centerSpan').length > 0) {
                    center = 'Center - <span>' + $('#centerSpan').html() + '</span> has been closed today';
                }
                else {
                    center = 'Center - <span>' + $('#center-para').find('select option:selected').text() + '</span> has been closed today';

                }
                closedHtml = closedHtml.replace("##AgencyContent##", center);

            }
            else {

                var classroom = '';
                classroom = 'Classroom - <span>' + $('option:selected', selectEle).text() + '</span> has been closed today';
                closedHtml = closedHtml.replace("##AgencyContent##", classroom);
            }
            $('#closed-agency-section').html(closedHtml).show();
        }
        else {
            $('#closed-agency-section').html('').hide();

        }
    }


   // weeklyAttendance.ShowBusy(false);
}

weekAttendance_div.find('.adult-breakfastcout,.adult-lunchcount,.adult-snackscount').on('keypress', function (event) {

    isNumber(event);
});

weekAttendance_div.find('.adult-breakfastcout').on('input', function () {

    var adultBreakfastCount = $('#adult-week-breakfast-count');
    var dayIndex = $(this).attr('day');
    if ($(this).val() != '') {
        var breakFast = 0;
        weekAttendance_div.find('.adult-breakfastcout').each(function (i, breakfast) {
            breakFast += ($(breakfast).val() != '') ? parseInt($(breakfast).val()) : 0;
        });

        //   var totalCount = breakFast + parseInt(adultBreakfastCount.html());
        adultBreakfastCount.html(breakFast);
        updateDailyMealsAndAttendance(dayIndex, true);
    }
});

weekAttendance_div.find('.adult-lunchcount').on('input', function () {

    var adultLunchCount = $('#adult-week-lunch-count');
    var dayIndex = $(this).attr('day');
    if ($(this).val() != '') {
        var lunchCount = 0;
        weekAttendance_div.find('.adult-lunchcount').each(function (i, lunch) {
            lunchCount += ($(lunch).val() != '') ? parseInt($(lunch).val()) : 0;
        });

        //   var totalCount = breakFast + parseInt(adultBreakfastCount.html());
        adultLunchCount.html(lunchCount);
        updateDailyMealsAndAttendance(dayIndex, true);
    }
});

weekAttendance_div.find('.adult-snackscount').on('input', function () {
    var adultSnacksCount = $('#adult-week-snacks-count');
    var dayIndex = $(this).attr('day');
    if ($(this).val() != '') {
        var snackCount = 0;
        weekAttendance_div.find('.adult-snackscount').each(function (i, snack) {
            snackCount += ($(snack).val() != '') ? parseInt($(snack).val()) : 0;
        });

        //   var totalCount = breakFast + parseInt(adultBreakfastCount.html());
        adultSnacksCount.html(snackCount);
        updateDailyMealsAndAttendance(dayIndex, true);
    }
});


//Single Day Insert
offlineAttendance_div.find('.adult-breakfastcout').on('input', function () {

    var dayIndex = $(this).attr('day');
    if ($(this).val() != '') {

        updateDailyMealsAndAttendance(dayIndex, true);
    }
});

offlineAttendance_div.find('.adult-lunchcount').on('input', function () {

    var dayIndex = $(this).attr('day');
    if ($(this).val() != '') {

        // updateOfflineDailyMealsAndAttendance(dayIndex, true);

        updateDailyMealsAndAttendance(dayIndex, true);
    }
});


offlineAttendance_div.find('.adult-snackscount').on('input', function () {

    var dayIndex = $(this).attr('day');
    if ($(this).val() != '') {

        updateDailyMealsAndAttendance(dayIndex, true);
    }
});



function bindUser(data) {
    allUsers = data;
}

function bindMeals(data) {
    allMeals = data;
}


function attendanceRadioCallback(data, element) {

    var index = $(element).closest('.att-tr').attr('row-index');
    var dayIndex = $(element).closest('.att-tr').attr('day');
    var attendanceDate = (weeklyAttendance.isHistorical()) ? weeklyAttendance.getAttendanceDate($(element).closest('.att-tr').attr('day')) : weeklyAttendance.getAttendanceDate(new Date());
    var hisIndex = getDatabaseIndex(index, attendanceDate);
    var clientId = $(element).closest('.att-tr').attr('clientid');
    var attendance_type = $(element).attr('value');
    var time = $(element).closest('.time-td').find('.time-text').val();
    var breakFast = '';
    var lunch = '';
    var snack = '';
    var inTime = (weeklyAttendance.isHistorical()) ? $(element).closest('.att-tr').find('.in-time-td').children('.in-time-div').siblings('.in-time').val() : '';
    var outTime = (weeklyAttendance.isHistorical()) ? $(element).closest('.att-tr').find('.out-time-td').children('.out-time').val() : '';

    var client = {};
    var centerArr = weeklyAttendance.getCenterClassId();
    if (data.length > 0) {

        client = weeklyAttendance.getEmptyClientJson();

        client.AttendanceDate = data[0].AttendanceDate;
        client.BreakFast = data[0].BreakFast;
        client.Lunch = data[0].Lunch;
        client.Snacks = data[0].Snacks;
        client.TimeIn = data[0].TimeIn;
        client.TimeOut = data[0].TimeOut;
        client.UserID = data[0].UserID;
        client.ClientID = data[0].ClientID;
        client.AttendanceType = data[0].AttendanceType;
        client.AbsenceReasonId = data[0].AbsenceReasonId;
        client.SignedInBy = data[0].SignedInBy;
        client.SignedOutBy = data[0].SignedOutBy;
        client.PSignatureIn = data[0].PSignatureIn;
        client.PSignatureOut = data[0].PSignatureOut;
        client.TSignatureIn = data[0].TSignatureIn;
        client.TSignatureOut = data[0].TSignatureOut;
        client.CenterID = data[0].CenterID;
        client.ClassroomID = data[0].ClassroomID;

        if (attendance_type == '1') {
            //$.each(data, function (i, attend) {
            //    attend.AttendanceType = attendance_type;
            //    attend.UserID = hisIndex;
            //    attend.AbsenceReasonId = '0';
            //});

            client.AttendanceType = attendance_type;
            client.UserID = hisIndex;
            client.AbsenceReasonId = '0';

        }
        else {

            //$.each(data, function (i, attend) {
            //    attend.ClientID = clientId;
            //    attend.AttendanceDate = attendanceDate;
            //    attend.TimeIn = '';
            //    attend.TimeOut = '';
            //    attend.BreakFast = '';
            //    attend.Lunch = '';
            //    attend.Snacks = '';
            //    attend.AttendanceType = attendance_type;
            //    attend.SignedInBy = '';
            //    attend.PSignatureIn = '';
            //    attend.TSignatureIn = '';
            //    attend.SignedOutBy = '';
            //    attend.PSignatureOut = '';
            //    attend.TSignatureOut = '';
            //    attend.TimeOut = '';
            //    attend.UserID = hisIndex;
            //    attend.AbsenceReasonId = '0';
            //});


            client.AttendanceDate = attendanceDate;
            client.BreakFast = '';
            client.Lunch = '';
            client.Snacks = '';
            client.TimeIn = inTime
            client.TimeOut = outTime;
            client.UserID = hisIndex;
            client.ClientID = clientId;
            client.AttendanceType = attendance_type;
            client.AbsenceReasonId = '0';
            client.SignedInBy = '';
            client.SignedOutBy = '';
            client.PSignatureIn = '';
            client.PSignatureOut = '';
            client.TSignatureIn = '';
            client.TSignatureOut = '';
            client.CenterID = centerArr.enc_CenterId;
            client.ClassroomID = centerArr.enc_ClassRoomId;
        }
    }
    else {

        client = {
            'ClientID': clientId,
            'AttendanceDate': attendanceDate,
            'TimeIn': inTime,
            'TimeOut':outTime,
            'BreakFast': '',
            'Lunch': '',
            'Snacks': '',
            'AttendanceType': attendance_type,
            'SignedInBy': '',
            'PSignatureIn': '',
            'TSignatureIn': '',
            'SignedOutBy': '',
            'PSignatureOut': '',
            'TSignatureOut': '',
            'UserID': hisIndex,
            'AbsenceReasonId': '0',
            'CenterID': centerArr.enc_CenterId,
            'ClassroomID': centerArr.enc_ClassRoomId
        };

    }

    if (data.length > 0) {
        DataBaseManager.UpdateUser(client, false);
    }


    else {
        DataBaseManager.InsertIntoTable(client, false);
    }

    updateDailyMealsAndAttendance(dayIndex, true);

    if (!weeklyAttendance.isHistorical()) {
        if (attendance_type == '2' || attendance_type == '3' || attendance_type == '1') {
            showPushToServerBtnByData();
        }
    }

}


function updateDailyMealsAndAttendance(_dayIndex, isInsert) {

    var presentCount = 0;
    var execus = 0;
    var unexec = 0;
    var breakfastCount = 0;
    var lunchCount = 0;
    var snacksCount = 0;
    var dailyTable = '';
    var attendanceDate = '';
    var dailyAttendTable = '';
    var attendanceDiv = '';
    var childMeals_row = '';

    if (weeklyAttendance.isHistorical()) {
        attendanceDiv = weekAttendance_div;
        attendanceDate = weeklyAttendance.getAttendanceDate(_dayIndex);
    }
    else {
        attendanceDiv = offlineAttendance_div;
        attendanceDate = weeklyAttendance.getAttendanceDate(new Date());
    }

    dailyTable = attendanceDiv.find('.day-table[day="' + _dayIndex + '"]');
    dailyAttendTable = attendanceDiv.find('#daily-attendance-count').find('table[day="' + _dayIndex + '"]');
    childMeals_row = attendanceDiv.find('#child-meals-row');

    dailyTable.find('.in-time-td').each(function (k, element) {

        presentCount += $(element).find('input:radio[value=1]:checked').length;
        execus += $(element).find('input:radio[value=2]:checked').length;
        unexec += $(element).find('input:radio[value=3]:checked').length;
    });

    breakfastCount = attendanceDiv.find('.day-table[day="' + _dayIndex + '"]').find('.break-check:checked').length;
    lunchCount = attendanceDiv.find('.day-table[day="' + _dayIndex + '"]').find('.lunch-check:checked').length;
    snacksCount = attendanceDiv.find('.day-table[day="' + _dayIndex + '"]').find('.snack-check:checked').length;

    dailyAttendTable.find('.present-count').html(presentCount);
    dailyAttendTable.find('.execuse-count').html(execus);
    dailyAttendTable.find('.unexecuse-count').html(unexec);

    childMeals_row.find('tr[day="' + _dayIndex + '"]').find('.child-breakfastcout').html(breakfastCount);
    childMeals_row.find('tr[day="' + _dayIndex + '"]').find('.child-lunchcount').html(lunchCount);
    childMeals_row.find('tr[day="' + _dayIndex + '"]').find('.child-snackscount').html(snacksCount);

    var adult_meals_row = attendanceDiv.find('#adult-meals-row');
    var adult_meals_table = adult_meals_row.find('table[day="' + _dayIndex + '"]');

    var adult_breakfast_count = (adult_meals_table.find('.adult-breakfastcout').val() == '') ? 0 : adult_meals_table.find('.adult-breakfastcout').val();
    var adult_lunch_count = (adult_meals_table.find('.adult-lunchcount').val() == '') ? 0 : adult_meals_table.find('.adult-lunchcount').val();
    var adult_snack_count = (adult_meals_table.find('.adult-snackscount').val() == '') ? 0 : adult_meals_table.find('.adult-snackscount').val();

    var centerArr = weeklyAttendance.getCenterClassId();

    var dailyMeals = {};
    dailyMeals = {
        'DailyID': getDailyMealsIndex(_dayIndex, attendanceDate),
        'AttendanceDate': attendanceDate,
        'ChildBreakFast': breakfastCount,
        'ChildLunch': lunchCount,
        'ChildSnacks': snacksCount,
        'AdultBreakFast': adult_breakfast_count,
        'AdultLunch': adult_lunch_count,
        'AdultSnacks': adult_snack_count,
        'ChildPresent': presentCount,
        'ChildExcused': execus,
        'ChildUnExcused': unexec,
        'CenterID': centerArr.enc_CenterId,
        'ClassroomID': centerArr.enc_ClassRoomId
    };

    if (isInsert) {
        insertOnMealsAttendance(_dayIndex, attendanceDate, dailyMeals)

    }
    else {

        DataBaseManager.GetAllMeals(updateDailyandWeeklyMealsOnLoad, _dayIndex);
    }


}


function updateDailyandWeeklyMealsOnLoad(dayindex, data) {


    var attendanceDiv = (weeklyAttendance.isHistorical()) ? weekAttendance_div : offlineAttendance_div;
    var adult_meals_row = attendanceDiv.find('#adult-meals-row');
    var adult_meals_table = adult_meals_row.find('table[day="' + dayindex + '"]');
    var arr = [];
    var attendanceDate = (weeklyAttendance.isHistorical()) ? weeklyAttendance.getAttendanceDate(dayindex) : weeklyAttendance.getAttendanceDate(new Date());

    if (data.length > 0) {

        arr = $.grep(data, function (element, index) {
            return element.AttendanceDate === attendanceDate;
        });

        if (arr.length > 0) {

            $.each(arr, function (i, ele) {

                adult_meals_table.find('.adult-breakfastcout').val(ele.AdultBreakFast);
                adult_meals_table.find('.adult-lunchcount').val(ele.AdultLunch);
                adult_meals_table.find('.adult-snackscount').val(ele.AdultSnacks);
            });

        }
        else {
            var attJson = weeklyAttendance.getChildAttendanceData();

            if (attJson.length > 0) {
                arr = $.grep(attJson, function (element, index) {
                    return element.AttendanceDate.toString() === attendanceDate.toString();
                });
            }
            if (arr.length > 0) {
                $.each(arr, function (i, ele) {
                    var aBreakFast = (ele.AdultBreakFast == null || ele.AdultBreakFast == '') ? 0 : ele.AdultBreakFast;
                    var aLuch = (ele.AdultLunch == null || ele.AdultLunch == '') ? 0 : ele.AdultLunch;
                    var aSnack = (ele.AdultSnacks == null || ele.AdultSnacks == '') ? 0 : ele.AdultSnacks;
                    adult_meals_table.find('.adult-breakfastcout').val(aBreakFast);
                    adult_meals_table.find('.adult-lunchcount').val(aLuch);
                    adult_meals_table.find('.adult-snackscount').val(aSnack);
                })
            }
        }
    }
    else {
        if (childAttendanceJson.length == 0) {
            childAttendanceJson = weeklyAttendance.getChildAttendanceData();

        }

        if (childAttendanceJson.length > 0) {
            arr = $.grep(childAttendanceJson, function (element, index1) {
                return element.AttendanceDate == attendanceDate;
            });
        }

        if (arr.length > 0) {
            $.each(arr, function (i, ele) {
                var aBreakFast = (ele.AdultBreakFast == null || ele.AdultBreakFast == '') ? 0 : ele.AdultBreakFast;
                var aLuch = (ele.AdultLunch == null || ele.AdultLunch == '') ? 0 : ele.AdultLunch;
                var aSnack = (ele.AdultSnacks == null || ele.AdultSnacks == '') ? 0 : ele.AdultSnacks;

                adult_meals_table.find('.adult-breakfastcout').val(aBreakFast);
                adult_meals_table.find('.adult-lunchcount').val(aLuch);
                adult_meals_table.find('.adult-snackscount').val(aSnack);
            })
        }
    }

    if (weeklyAttendance.isHistorical()) {
        updateWeeklyMealsAndAttendance();
    }

}


function updateWeeklyMealsAndAttendance() {

    var _weeklyattendance = $('#weekDaysAttendance');
    var child_meals_row = _weeklyattendance.find('#child-meals-row');
    var adult_meals_row = _weeklyattendance.find('#adult-meals-row');
    var daily_attendance_table = _weeklyattendance.find('#daily-attendance-count');
    var _weeklyChildBreakfast = 0;
    var _weeklyChildLunch = 0;
    var _weeklyChildSnacks = 0;
    var _weeklyAdultBreakfast = 0;
    var _weeklyAdultLunch = 0;
    var _weeklyAdultSnacks = 0;

    var _weeklyPresentCount = 0;
    var _weeklyExecuseCount = 0;
    var _weeklyUnexcuseCount = 0;

    child_meals_row.find('.child-breakfastcout').each(function () {
        _weeklyChildBreakfast += ($(this).html() == "") ? 0 : parseInt($(this).html());
    });

    child_meals_row.find('.child-lunchcount').each(function () {
        _weeklyChildLunch += ($(this).html() == "") ? 0 : parseInt($(this).html());
    });

    child_meals_row.find('.child-snackscount').each(function () {
        _weeklyChildSnacks += ($(this).html() == "") ? 0 : parseInt($(this).html());
    });

    adult_meals_row.find('.adult-breakfastcout').each(function () {
        _weeklyAdultBreakfast += ($(this).val() == "") ? 0 : parseInt($(this).val());
    });

    adult_meals_row.find('.adult-lunchcount').each(function () {
        _weeklyAdultLunch += ($(this).val() == "") ? 0 : parseInt($(this).val());
    });
    adult_meals_row.find('.adult-snackscount').each(function () {
        _weeklyAdultSnacks += ($(this).val() == "") ? 0 : parseInt($(this).val());
    });

    daily_attendance_table.find('.present-count').each(function () {
        _weeklyPresentCount += ($(this).html() == "") ? 0 : parseInt($(this).html());
    });

    daily_attendance_table.find('.execuse-count').each(function () {
        _weeklyExecuseCount += ($(this).html() == "") ? 0 : parseInt($(this).html());
    });

    daily_attendance_table.find('.unexecuse-count').each(function () {
        _weeklyUnexcuseCount += ($(this).html() == "") ? 0 : parseInt($(this).html());
    });

    _weeklyattendance.find('#child-week-breakfast-count').html(_weeklyChildBreakfast);
    _weeklyattendance.find('#child-week-lunch-count').html(_weeklyChildLunch);
    _weeklyattendance.find('#child-week-snacks-count').html(_weeklyChildSnacks);

    _weeklyattendance.find('#adult-week-breakfast-count').html(_weeklyAdultBreakfast);
    _weeklyattendance.find('#adult-week-lunch-count').html(_weeklyAdultLunch);
    _weeklyattendance.find('#adult-week-snacks-count').html(_weeklyAdultSnacks);

    _weeklyattendance.find('#weekly-present-count').html(_weeklyPresentCount);
    _weeklyattendance.find('#weekly-excuse-count').html(_weeklyExecuseCount);
    _weeklyattendance.find('#weekly-unexcuse-count').html(_weeklyUnexcuseCount);


}


function insertOnTimeRadio(val) {
    var attendanceDate = (weeklyAttendance.isHistorical()) ? weeklyAttendance.getAttendanceDate($(val).closest('.att-tr').attr('day')) : weeklyAttendance.getAttendanceDate(new Date());
    var index = $(val).closest('.att-tr').attr('row-index');
    index = getDatabaseIndex(index, attendanceDate);
    var clientId = $(val).closest('.att-tr').attr('clientid');

    DataBaseManager.GetSingleClient(index, clientId, attendanceDate, attendanceRadioCallback, val);
}

function insertOnMealsAttendance(dayIndex, attend_date, mealsArray) {

    DataBaseManager.GetDailyMeals(mealsArray.DailyID, attend_date, insertOnMealsCallback, mealsArray);
}

function insertOnMealsCallback(data, _mealsArray) {
    mealsData = weeklyAttendance.getEmptyMealsJson();
    var mealsData = {};
    if (data.length > 0) {


        mealsData.AdultBreakFast = data[0].AdultBreakFast;
        mealsData.AdultLunch = data[0].AdultLunch;
        mealsData.AdultSnacks = data[0].AdultSnacks;
        mealsData.AttendanceDate = data[0].AttendanceDate;
        mealsData.ChildBreakFast = data[0].ChildBreakFast;
        mealsData.ChildLunch = data[0].ChildLunch;
        mealsData.ChildSnacks = data[0].ChildSnacks;
        mealsData.ChildPresent = data[0].ChildPresent;
        mealsData.ChildExcused = data[0].ChildExcused;
        mealsData.ChildUnExcused = data[0].ChildUnExcused;
        mealsData.DailyID = data[0].DailyID;
        mealsData.CenterID = data[0].CenterID;
        mealsData.ClassroomID = data[0].ClassroomID;

        if (_mealsArray != null || _mealsArray != undefined || _mealsArray != '') {

            //$.each(data, function (i, meals) {
            //    meals.AdultBreakFast = _mealsArray.AdultBreakFast;
            //    meals.AdultLunch = _mealsArray.AdultLunch;
            //    meals.AdultSnacks = _mealsArray.AdultSnacks;
            //    meals.ChildBreakFast = _mealsArray.ChildBreakFast;
            //    meals.ChildLunch = _mealsArray.ChildLunch;
            //    meals.ChildSnacks = _mealsArray.ChildSnacks;
            //    meals.ChildPresent = _mealsArray.ChildPresent;
            //    meals.ChildExcused = _mealsArray.ChildExcused;
            //    meals.ChildUnExcused = _mealsArray.ChildUnExcused;
            //});

            mealsData.AdultBreakFast = _mealsArray.AdultBreakFast;
            mealsData.AdultLunch = _mealsArray.AdultLunch;
            mealsData.AdultSnacks = _mealsArray.AdultSnacks;
            mealsData.ChildBreakFast = _mealsArray.ChildBreakFast;
            mealsData.ChildLunch = _mealsArray.ChildLunch;
            mealsData.ChildSnacks = _mealsArray.ChildSnacks;
            mealsData.ChildPresent = _mealsArray.ChildPresent;
            mealsData.ChildExcused = _mealsArray.ChildExcused;
            mealsData.ChildUnExcused = _mealsArray.ChildUnExcused;


        }
        else {
            //data[0] = _mealsArray;

            mealsData.AdultBreakFast = _mealsArray.AdultBreakFast;
            mealsData.AdultLunch = _mealsArray.AdultLunch;
            mealsData.AdultSnacks = _mealsArray.AdultSnacks;
            mealsData.AttendanceDate = _mealsArray.AttendanceDate;
            mealsData.ChildBreakFast = _mealsArray.ChildBreakFast;
            mealsData.ChildLunch = _mealsArray.ChildLunch;
            mealsData.ChildSnacks = _mealsArray.ChildSnacks;
            mealsData.ChildPresent = _mealsArray.ChildPresent;
            mealsData.ChildExcused = _mealsArray.ChildExcused;
            mealsData.ChildUnExcused = _mealsArray.ChildUnExcused;
            mealsData.DailyID = _mealsArray.DailyID;
            mealsData.CenterID = _mealsArray.CenterID;
            mealsData.ClassroomID = _mealsArray.ClassroomID;
        }
        DataBaseManager.UpdateDailyMealsAttendance(mealsData, updateResultStatus);
    }
    else {
        if (_mealsArray != null || _mealsArray != undefined || _mealsArray != '') {

            DataBaseManager.InsertDailyMealsAttendance(_mealsArray, updateResultStatus);
        }
    }

    if (weeklyAttendance.isHistorical()) {
        updateWeeklyMealsAndAttendance();

    }
}

function getDatabaseIndex(index, attend_date) {
        
    if (weeklyAttendance.isHistorical()) {

        var classId = class_para.find('option:selected', 'select').attr('value');
            
        var clientId = weekAttendance_div.find('.att-tr[row-index=' + index + ']').attr('clientid');
         
        index = 'his' + index + "_" + attend_date.split('/')[0] + attend_date.split('/')[1] + attend_date.split('/')[2] + classId + clientId;
    }
    else {
        index = 'off' + index + attend_date.split('/')[0] + attend_date.split('/')[1] + attend_date.split('/')[2];
    }
    return index;
}


function getDailyMealsIndex(index, _attendDate) {
    if (weeklyAttendance.isHistorical()) {
        index = 'hismeal' + index + _attendDate.split('/')[0] + _attendDate.split('/')[1] + _attendDate.split('/')[2];
    }
    else {
        index = 'daimeal' + index + _attendDate.split('/')[0] + _attendDate.split('/')[1] + _attendDate.split('/')[2];
    }
    return index;
}

function updateOnCheckMeals(data, element) {

    var ischeck = ($(element).is(':checked')) ? '1' : '0';

    var checkvalue = $(element).val();

    var attendanceDate = (weeklyAttendance.isHistorical()) ? weeklyAttendance.getAttendanceDate($(element).closest('.att-tr').attr('day')) : weeklyAttendance.getAttendanceDate(new Date());
    var hisIndex = $(element).closest('.att-tr').attr('row-index');
    var dayIndex = $(element).closest('.att-tr').attr('day');
    hisIndex = getDatabaseIndex(hisIndex, attendanceDate);
    var clientId = $(element).closest('.att-tr').attr('clientid');
    var inTime = (weeklyAttendance.isHistorical()) ? $(element).parent().parent().parent('.meals-row').parent().parent().parent('.meals-td').siblings('.in-time-td').find('.in-time').val() : $(element).parent().parent().parent('.meals-row').parent().parent().parent('.meals-td').siblings('.offline-intime-input').find('.in-time').val();
    var outTime = (weeklyAttendance.isHistorical()) ? $(element).parent().parent().parent('.meals-row').parent().parent().parent('.meals-td').siblings('.out-time-td').find('.out-time').val() : $(element).parent().parent().parent('.meals-row').parent().parent().parent('.meals-td').siblings('.offline-outtime-input').find('.out-time').val();
    var PSignatureIn = (!weeklyAttendance.isHistorical()) ? $(element).parent().parent().parent('.meals-row').parent().parent().parent('.meals-td').siblings('.offline-in-time-td').find('.sig-parent-hidden').val() : '';
    var PSignatureOut = (!weeklyAttendance.isHistorical()) ? $(element).parent().parent().parent('.meals-row').parent().parent().parent('.meals-td').siblings('.offline-out-time-td').find('.sig-parent-hidden').val() : '';
    var SignedInBy = (!weeklyAttendance.isHistorical()) ? $(element).parent().parent().parent('.meals-row').parent().parent().parent('.meals-td').siblings('.offline-in-time-td').find('.sig-parent-hidden').attr('parentId') : '';
    var TSignatureIn = (!weeklyAttendance.isHistorical()) ? $(element).parent().parent().parent('.meals-row').parent().parent().parent('.meals-td').siblings('.offline-in-time-td').find('.sig-teacher-hidden').val() : '';
    var SignedOutBy = (!weeklyAttendance.isHistorical()) ? $(element).parent().parent().parent('.meals-row').parent().parent().parent('.meals-td').siblings('.offline-out-time-td').find('.sig-parent-hidden').attr('parentId') : '';
    var breakFast = '';
    var lunch = '';
    var snacks = '';

    var client = {};

    var centerArr = weeklyAttendance.getCenterClassId();


    if (data.length > 0) {

        client = weeklyAttendance.getEmptyClientJson();

        client.AttendanceDate = data[0].AttendanceDate;
        client.BreakFast = data[0].BreakFast;
        client.Lunch = data[0].Lunch;
        client.Snacks = data[0].Snacks;
        client.TimeIn = data[0].TimeIn;
        client.TimeOut = data[0].TimeOut;
        client.UserID = data[0].UserID;
        client.ClientID = data[0].ClientID;
        client.AttendanceType = data[0].AttendanceType;
        client.AbsenceReasonId = data[0].AbsenceReasonId;
        client.SignedInBy = data[0].SignedInBy;
        client.SignedOutBy = data[0].SignedOutBy;
        client.PSignatureIn = data[0].PSignatureIn;
        client.PSignatureOut = data[0].PSignatureOut;
        client.TSignatureIn = data[0].TSignatureIn;
        client.TSignatureOut = data[0].TSignatureOut;
        client.UserID = data[0].UserID;
        client.CenterID = data[0].CenterID;
        client.ClassroomID = data[0].ClassroomID;
        switch (checkvalue) {
            case '1':
                //$.each(data2, function (i, attend) {
                //    attend.AttendanceDate = attendanceDate;
                //    attend.BreakFast = ischeck;
                //    attend.UserID = hisIndex;
                //});

                client.AttendanceDate = attendanceDate;
                client.BreakFast = ischeck;
                client.UserID = hisIndex;

                break;

            case '2':
                //$.each(data2, function (i, attend) {
                //    attend.AttendanceDate = attendanceDate;
                //    attend.Lunch = ischeck;
                //    attend.UserID = hisIndex;
                //});
                client.AttendanceDate = attendanceDate;
                client.Lunch = ischeck;
                client.UserID = hisIndex;
                break;

            case '3':
                //$.each(data2, function (i, attend) {
                //    attend.AttendanceDate = attendanceDate;
                //    attend.Snacks = ischeck;
                //    attend.UserID = hisIndex;
                //});
                client.AttendanceDate = attendanceDate;
                client.Snacks = ischeck;
                client.UserID = hisIndex;
                break;
        }

    }

    else {
        //if ($(element).closest('.time-td').hasClass('in-time-td')) {
        switch (checkvalue) {
            case '1':

                breakFast = ischeck;
                lunch = ($(element).closest('.meals-row').find('.lunch-check').is(':checked')) ? '1' : '0';
                snacks = ($(element).closest('.meals-row').find('.snack-check').is(':checked')) ? '1' : '0';
                //client = {
                //    'ClientID': clientId,
                //    'AttendanceDate': attendanceDate,
                //    'TimeIn': time,
                //    'TimeOut': '',
                //    'BreakFast': ischeck,
                //    'Lunch': '',
                //    'Snacks': '',
                //    'AttendanceType': '1',
                //    'SignedInBy': '',
                //    'PSignatureIn': '',
                //    'TSignatureIn': '',
                //    'SignedOutBy': '',
                //    'PSignatureOut': '',
                //    'TSignatureOut': '',
                //    'UserID': hisIndex
                //};
                break;

            case '2':
                breakFast = ($(element).closest('.meals-row').find('.break-check').is(':checked')) ? '1' : '0';
                lunch = ischeck;
                snacks = ($(element).closest('.meals-row').find('.snack-check').is(':checked')) ? '1' : '0';
                //client = {
                //    'ClientID': clientId,
                //    'AttendanceDate': attendanceDate,
                //    'TimeIn': time,
                //    'TimeOut': '',
                //    'BreakFast': '',
                //    'Lunch': ischeck,
                //    'Snacks': '',
                //    'AttendanceType': '1',
                //    'SignedInBy': '',
                //    'PSignatureIn': '',
                //    'TSignatureIn': '',
                //    'SignedOutBy': '',
                //    'PSignatureOut': '',
                //    'TSignatureOut': '',
                //    'UserID': hisIndex
                //};
                break;

            case '3':

                breakFast = ($(element).closest('.meals-row').find('.break-check').is(':checked')) ? '1' : '0';
                lunch = ($(element).closest('.meals-row').find('.lunch-check').is(':checked')) ? '1' : '0';
                snacks = ischeck;

                //client = {
                //    'ClientID': clientId,
                //    'AttendanceDate': attendanceDate,
                //    'TimeIn': time,
                //    'TimeOut': '',
                //    'BreakFast': '',
                //    'Lunch': '',
                //    'Snacks': ischeck,
                //    'AttendanceType': '1',
                //    'SignedInBy': '',
                //    'PSignatureIn': '',
                //    'TSignatureIn': '',
                //    'SignedOutBy': '',
                //    'PSignatureOut': '',
                //    'TSignatureOut': '',
                //    'UserID': hisIndex
                //};

                break;
        }

        client = weeklyAttendance.getEmptyClientJson();

        client.AttendanceDate = attendanceDate;
        client.BreakFast = breakFast;
        client.Lunch = lunch;
        client.Snacks = snacks;
        client.TimeIn = inTime;
        client.TimeOut = outTime;
        client.UserID = hisIndex;
        client.ClientID = clientId;
        client.AttendanceType = '1';
        client.PSignatureIn = PSignatureIn;
        client.PSignatureOut = PSignatureOut;
        client.SignedInBy = SignedInBy;
        client.SignedOutBy = SignedOutBy;
        client.AbsenceReasonId = '0';
        client.TSignatureIn = TSignatureIn;
        client.TSignatureOut = '';
        client.CenterID = centerArr.enc_CenterId;
        client.ClassroomID = centerArr.enc_ClassRoomId;
    }


    if (data.length > 0) {

        DataBaseManager.UpdateUser(client, false);
    }
    else {
        DataBaseManager.InsertIntoTable(client, false);
    }

    updateDailyMealsAndAttendance(dayIndex, true);
}

function getOfflineAttendanceDate() {


    var date = '';
    date = $('#singleDayAttance').find('#week-monday').attr('date');
    return date;
}

function updateResultStatus(data) {
    console.log(data);
}

function updateRecordOnTimechange(data, element) {

    var recordIndex_time = '';
    var timeValue_time = '';
    var attendanceDate_time = '';
    var clientId_time = '';
    var day_index = '';
    var isInTime = false;
    var breakFast = '';
    var lunch = '';
    var snacks = '';
    var inTime = '';
    var outTime = '';


    var client = {};
    var centerArr = weeklyAttendance.getCenterClassId();

    isInTime = $(element).parent('.time-td').hasClass('in-time-td');
    breakFast = ($(element).closest('.att-tr').children('.meals-td').find('.meals-row').find('.break-check').is(':checked')) ? '1' : '0';
    lunch = ($(element).closest('.att-tr').children('.meals-td').find('.meals-row').find('.lunch-check').is(':checked')) ? '1' : '0';
    snacks = ($(element).closest('.att-tr').children('.meals-td').find('.meals-row').find('.snack-check').is(':checked')) ? '1' : '0';
    inTime = $(element).closest('.att-tr').children('.in-time-td').find('.in-time').val();
    outTime = $(element).closest('.att-tr').children('.out-time-td').find('.out-time').val();

    if (data.length > 0) {
        client = weeklyAttendance.getEmptyClientJson();

        client.AttendanceDate = data[0].AttendanceDate;
        client.BreakFast = data[0].BreakFast;
        client.Lunch = data[0].Lunch;
        client.Snacks = data[0].Snacks;
        client.TimeIn = data[0].TimeIn;
        client.TimeOut = data[0].TimeOut;
        client.UserID = data[0].UserID;
        client.ClientID = data[0].ClientID;
        client.AttendanceType = data[0].AttendanceType;
        client.AbsenceReasonId = data[0].AbsenceReasonId;
        client.SignedInBy = data[0].SignedInBy;
        client.SignedOutBy = data[0].SignedOutBy;
        client.PSignatureIn = data[0].PSignatureIn;
        client.PSignatureOut = data[0].PSignatureOut;
        client.TSignatureIn = data[0].TSignatureIn;
        client.TSignatureOut = data[0].TSignatureOut;
        client.UserID = data[0].UserID;
        client.CenterID = data[0].CenterID;
        client.ClassroomID = data[0].ClassroomID;
    }

    recordIndex_time = $(element).closest('.att-tr').attr('row-index');
    timeValue_time = $(element).closest('.time-td').find('.time-text').val();

    day_index = $(element).closest('.att-tr').attr('day');

    if (recordIndex_time != undefined && timeValue_time != '') {
        attendanceDate_time = weeklyAttendance.getAttendanceDate(day_index);
        recordIndex_time = getDatabaseIndex(recordIndex_time, attendanceDate_time);
        clientId_time = $(element).closest('.att-tr').attr('clientid');

        if (isInTime) {
            if (data.length > 0) {


                //$.map(data, function (attend, i) {
                //    attend.TimeIn = timeValue_time;
                //    attend.UserID = recordIndex_time;
                //});
                client.TimeIn = timeValue_time;
                client.UsetID = recordIndex_time;

            }
            else {



                client = weeklyAttendance.getEmptyClientJson();

                client.AttendanceDate = attendanceDate_time;
                client.BreakFast = breakFast;
                client.Lunch = lunch;
                client.Snacks = snacks;
                client.TimeIn = timeValue_time;
                client.TimeOut = outTime;
                client.ClientID = clientId_time;
                client.AttendanceType = '1';
                client.AbsenceReasonId = '0';
                client.SignedInBy = '';
                client.SignedOutBy = '';
                client.PSignatureIn = '';
                client.PSignatureOut = '';
                client.TSignatureIn = '';
                client.TSignatureOut = '';
                client.UserID = recordIndex_time;
                client.CenterID = centerArr.enc_CenterId;
                client.ClassroomID = centerArr.enc_ClassRoomId;

            }

        }
        else {
            if (data.length > 0) {
                //$.map(data, function (attend, i) {
                //    attend.TimeOut = timeValue_time;
                //    attend.UserID = recordIndex_time;
                //});

                //  DataBaseManager.UpdateUser(data[0], updateResultStatus);
                client.TimeOut = timeValue_time;
                client.UserID = recordIndex_time
            }
            else {

                //client = {
                //    'ClientID': clientId_time,
                //    'AttendanceDate': attendanceDate_time,
                //    'TimeIn': '',
                //    'TimeOut': timeValue_time,
                //    'BreakFast': '',
                //    'Lunch': '',
                //    'Snacks': '',
                //    'AttendanceType': '1',
                //    'SignedInBy': '',
                //    'PSignatureIn': '',
                //    'TSignatureIn': '',
                //    'SignedOutBy': '',
                //    'PSignatureOut': '',
                //    'TSignatureOut': '',
                //    'UserID': recordIndex_time,
                //    'AbsenceReasonId': ''
                //};

                client = weeklyAttendance.getEmptyClientJson();

                client.AttendanceDate = attendanceDate_time
                client.BreakFast = breakFast;
                client.Lunch = lunch;
                client.Snacks = snacks;
                client.TimeIn = inTime;
                client.TimeOut = timeValue_time;
                client.ClientID = clientId_time;
                client.AttendanceType = '1';
                client.AbsenceReasonId = '0';
                client.SignedInBy = '';
                client.SignedOutBy = '';
                client.PSignatureIn = '';
                client.PSignatureOut = '';
                client.TSignatureIn = '';
                client.TSignatureOut = '';
                client.UserID = recordIndex_time;
                client.CenterID = centerArr.enc_CenterId;
                client.ClassroomID = centerArr.enc_ClassRoomId;

            }
        }


        if (data.length > 0) {

            DataBaseManager.UpdateUser(client, updateResultStatus);
        }
        else {

            DataBaseManager.InsertIntoTable(client, updateResultStatus);
        }
    }



}

function insertDailyDataToLocal() {

    DataBaseManager.GetAllClient(bindUser);
    var centerArr = weeklyAttendance.getCenterClassId();
    $('#singleDayAttance').find('.day-table').each(function (i, table) {

        $.each($(table).find('.att-tr'), function (j, row) {


            var intime = $(row).children('td').find('.in-time').val();
            var userId = "off" + $(row).attr('row-index');
            var inradiocheck = $(row).children('td').find('input[type=radio]').is(':checked');

            if (inradiocheck) {
                var clientid = $(row).attr('clientid');
                // var attendanceDate = getAttendanceDate($(row).attr('day'));
                var attendanceDate = getFormattedDate(new Date());

                var isUpdate = false;
                if (allUsers.length > 0) {

                    for (var l = 0; l < allUsers.length; l++) {

                        if ((allUsers[l].ClientID == clientid) && (allUsers[l].AttendanceDate == attendanceDate)) {

                            isUpdate = true;
                            break;
                        }
                    }
                }
                var AttendanceType = $(row).children('td').find('input[type=radio]:checked').val();
                var intime = (AttendanceType === '1') ? $(row).children('td').find('.in-time').val() : '';
                var outtime = (AttendanceType === '1') ? $(row).children('td').find('.out-time').val() : '';
                var BreakFast = ($(row).find('.break-check').is(':checked')) ? '1' : ' 0';
                var lunch = ($(row).find('.lunch-check').is(':checked')) ? '1' : '0';
                var snacks = ($(row).find('.snack-check').is(':checked')) ? '1' : '0';

                var client = {
                    'ClientID': clientid,
                    'AttendanceDate': attendanceDate,
                    'TimeIn': intime,
                    'TimeOut': outtime,
                    'BreakFast': BreakFast,
                    'Lunch': lunch,
                    'Snacks': snacks,
                    'AttendanceType': AttendanceType,
                    'SignedInBy': '',
                    'PSignatureIn': '',
                    'TSignatureIn': '',
                    'SignedOutBy': '',
                    'PSignatureOut': '',
                    'TSignatureOut': '',
                    'UserID': userId,
                    'CenterID': centerArr.enc_CenterId,
                    'ClassroomID': centerArr.enc_ClassRoomId
                };


                if (isUpdate) {
                    DataBaseManager.UpdateUser(client, updateResultStatus);

                }
                else {
                    DataBaseManager.InsertIntoTable(client, updateResultStatus);
                }



            }

        });
    });
}

$('#datetimepicker1').on('change', function () {
    //  clearAllInputs();

    var date = $(this).val();
    setWeekDays($('#datetimepicker1').val());
    var formatDate = (new Date(date).getMonth() + 1) + '/' + new Date(date).getDate() + '/' + new Date(date).getFullYear();

    if (weeklyAttendance.isHistorical()) {
        var centerArray = weeklyAttendance.getCenterClassId();
        var attendanceDateArray = weeklyAttendance.getWeekDatesFormatted(date);
        var stringDate = '';
        $.each(attendanceDateArray, function (k, attendanceDate) {
            if ((attendanceDateArray.length - 1) == k) {
                stringDate += attendanceDate;
            }
            else {
                stringDate += attendanceDate + ',';
            }
        });
        $.ajax({
            type: 'post',
            datatype: 'json',
            url: '/Teacher/GetChildAttendanceDetailsByDate',
            beforeSend:function(){weeklyAttendance.ShowBusy(true)},
            data: { centerId: centerArray.enc_CenterId, classroomId: centerArray.enc_ClassRoomId, attendaneDate: stringDate, isHistorical: true },
            success: function (data) {
                attendanceString.val('');
                attendanceString.val(data.WeeklyAttendancestring);
                childAttendanceJson = JSON.parse(data.WeeklyAttendancestring);
                childString.val(data.ChildInfoString);
                childInfoJson = JSON.parse(data.ChildInfoString);
                //DataBaseManager.GetAllClient(setTableOnDateChange);
                loadWeeklyDailyAttendance();
                weeklyAttendance.ShowBusy(false);
            },
            error: function (data) {
                DataBaseManager.GetAllClient(setTableOnDateChange);
                weeklyAttendance.ShowBusy(false);
            }
        });
    }


});


$('#push-to-server').on('click', function () {
    //if ($('#weekDaysAttendance').is(':visible')) {

    //}

    cleanValidation();
    if (checkValidation()) {
        if (weeklyAttendance.isHistorical()) {
            customAlert('Some fields are required');
        }
        else {
            customAlert('Please add parent or teacher signature');
        }

        weeklyAttendance.ShowBusy(false);

        return false;

    }
    else {
        weeklyAttendance.ShowBusy(true);
        insertToServer();
    }

});


function setTableOnDateChange(data) {

    weeklyAttendance.ShowBusy(true);

    var weekDays = [];
    var serverData = [];
    clearAllInputs();
    if (!weeklyAttendance.isHistorical()) {
        //days = [];
        weekDays.push(new Date());
    }
    else {
        weekDays = getDates($('#datetimepicker1').val());
        // hideFutureDaysAttendance();
        working_days_check.find('input[type=checkbox]').each(function () {
            hideShowHistoricalDiv(this);
        });
    }

    if (weekDays.length > 0) {
        //gets the Center/Class Id//
        var classJson = weeklyAttendance.getCenterClassId();

        if (data.length > 0) {
            serverData = $.grep(data, function (element, index) {
                return element.CenterID == classJson.enc_CenterId && element.ClassroomID == classJson.enc_ClassRoomId &&
               (element.AttendanceDate == weeklyAttendance.getFormattedDate(weekDays[0]) || element.AttendanceDate == weeklyAttendance.getFormattedDate(weekDays[1]) ||
               element.AttendanceDate == weeklyAttendance.getFormattedDate(weekDays[2]) || element.AttendanceDate == weeklyAttendance.getFormattedDate(weekDays[3]) || element.AttendanceDate == weeklyAttendance.getFormattedDate(weekDays[4]))
            });
        }



        //if (serverData.length === 0) {


        if (childAttendanceJson.length > 0) {

            if (weeklyAttendance.isHistorical()) {

                data = $.grep(childAttendanceJson, function (element, index) {
                    return element.CenterID == classJson.enc_CenterId && element.ClassroomID == classJson.enc_ClassRoomId &&
                   (element.AttendanceDate == weeklyAttendance.getFormattedDate(weekDays[0]) || element.AttendanceDate == weeklyAttendance.getFormattedDate(weekDays[1]) ||
                   element.AttendanceDate == weeklyAttendance.getFormattedDate(weekDays[2]) || element.AttendanceDate == weeklyAttendance.getFormattedDate(weekDays[3]) || element.AttendanceDate == weeklyAttendance.getFormattedDate(weekDays[4]))
                });

            }
            else {
                data = $.grep(childAttendanceJson, function (element, index) {
                    return element.CenterID == classJson.enc_CenterId && element.ClassroomID == classJson.enc_ClassRoomId &&
                   (element.AttendanceDate == weeklyAttendance.getFormattedDate(weekDays[0]))
                });

            }
        }
        //}

        if (serverData.length > 0) {
            if (data.length > 0) {
                $.each(serverData, function (m, data2) {
                    data.push(data2);
                });
            }
            else {
                data = serverData;
            }

        }

        if (data != '' && data.length > 0) {


            $.each(data, function (j, table) {

                $.each(weekDays, function (k, day) {

                    if (weeklyAttendance.getFormattedDate(day) === table.AttendanceDate) {
                      
                        var tableIndex = '';
                        var rowIndex = '';
                        if (table.UserID == undefined || table.UserID == null) {
                            var row_index = getDatabaseIndexByDateString(table.AttendanceDate, table.ClientID);

                            if (row_index.length > 0) {
                                //var indexsplit = row_index.substring(0, row_index.length - 8);
                                //  indexsplit = indexsplit.substring(3, indexsplit.length);
                                var indexsplit = '';
                                indexsplit = row_index;
                                tableIndex = row_index.charAt(0);
                                indexsplit = indexsplit.substring(1, indexsplit.length);
                                rowIndex = indexsplit;
                                table.UserID = getDatabaseIndex(row_index, table.AttendanceDate);
                            }
                        }
                        else {
                            var indexsplit = table.UserID;
                            //indexsplit = table.UserID.substring(0, table.UserID.length - 8);
                            //indexsplit = indexsplit.substring(3, indexsplit.length);
                            //tableIndex = indexsplit.charAt(0);
                            //indexsplit = indexsplit.substring(1, indexsplit.length);
                            //rowIndex = indexsplit;

                            //new code//
                            indexsplit = indexsplit.split('_')[0];
                            indexsplit = indexsplit.substring(3, indexsplit.length);
                            tableIndex = indexsplit.charAt(0).toString();
                            rowIndex = indexsplit.substring(1, indexsplit.length).toString();

                        }

                        var row = '';
                        if (weeklyAttendance.isHistorical()) {
                            row = $('#weekDaysAttendance').find('table[day="' + tableIndex + '"]').find('tr[clientid="' + table.ClientID + '"]')
                            var refUserId = getDatabaseIndex((tableIndex + rowIndex), table.AttendanceDate);
                         
                            

                            if ($(row).attr('clientid') == table.ClientID && table.UserID == refUserId) {

                                //$(row).children('td').find('.btn-reason').attr('reason-id', '0').removeClass('btn-reason-success').addClass('btn-reason-normal').hide();

                                switch (table.AttendanceType) {
                                    case '1':

                                        $(row).children('td').find('input[type=radio][value=' + table.AttendanceType + ']').prop('checked', true);
                                        $(row).find('.in-time-div,.out-time-div').show();
                                        var inTimePicker = $(row).children('td').find('.in-time ');
                                        var outTimePicker = $(row).children('td').find('.out-time');
                                        inTimePicker.val(table.TimeIn).show();
                                        outTimePicker.val(table.TimeOut).show();
                                        $(row).children('td').find('.btn-reason').attr('reason-id', '0').removeClass('btn-reason-success').addClass('btn-reason-normal').hide().html('Add Reason');
                                        weeklyAttendance.initializeDatetimePicker(inTimePicker.attr('id'));
                                        inTimePicker.on('change', function () {
                                            cleanValidation();
                                            weeklyAttendance.timeChangeUpdate(this);
                                        });


                                        weeklyAttendance.initializeDatetimePicker(outTimePicker.attr('id'));
                                        outTimePicker.on('change', function () {
                                            cleanValidation();
                                            weeklyAttendance.timeChangeUpdate(this);
                                        });

                                        $(row).children('.meals-td').find('.meals-row').show();
                                        break;
                                    case '2':
                                        $(row).children('td').find('input[type=radio][value=' + table.AttendanceType + ']').prop('checked', true);
                                        $(row).children('td').find('.in-time ').val(table.TimeIn).hide();
                                        $(row).children('td').find('.out-time').val(table.TimeOut).hide();
                                        if (table.AbsenceReasonId == '0' || table.AbsenceReasonId == '') {
                                            $(row).children('td').find('.btn-reason').attr('reason-id', table.AbsenceReasonId).removeClass('btn-reason-success').addClass('btn-reason-normal').show().html('Add Reason');

                                        }
                                        else {
                                            $(row).children('td').find('.btn-reason').attr('reason-id', table.AbsenceReasonId).removeClass('btn-reason-normal').addClass('btn-reason-success').show().html('Show Reason');

                                        }

                                        $(row).children('.out-time-td').find('.out-time-div').hide();
                                        $(row).children('.meals-td').find('.meals-row').hide();
                                        break;
                                    case '3':
                                        $(row).children('td').find('input[type=radio][value=' + table.AttendanceType + ']').prop('checked', true);
                                        $(row).children('td').find('.in-time ').val(table.TimeIn).hide();
                                        $(row).children('td').find('.out-time').val(table.TimeOut).hide();
                                        $(row).children('td').find('.btn-reason').attr('reason-id', '0').removeClass('btn-reason-success').addClass('btn-reason-normal').hide().html('Add Reason');
                                        $(row).children('.meals-td').find('.meals-row').hide();
                                        break;
                                    default:
                                        $(row).children('td').find('input[type=radio]').prop('checked', false);
                                        $(row).children('td').find('.in-time').val('').hide();
                                        $(row).children('td').find('.out-time').val('').hide();
                                        $(row).find('.in-time-div,.out-time-div').show();
                                        $(row).find('.out-time-div').show();
                                        $(row).find('.meals-check').prop('checked', false);
                                        $(row).children('.meals-td').find('.meals-row').show();
                                        $(row).children('td').find('.btn-reason').attr('reason-id', '0').removeClass('btn-reason-success').addClass('btn-reason-normal').hide().html('Add Reason');
                                        break;

                                }

                                $(row).find('.meals-check').each(function () { $(this).prop('checked', false); });

                            
                                if (table.BreakFast == '1' || (table.BreakFast==true)) {
                                    $(row).find('.break-check').prop('checked', true);
                                }
                                //else {
                                //    $(row).find('.break-check').prop('checked', false);
                                //} 

                                if (table.Lunch == '1' || (table.Lunch==true)) {
                                    $(row).find('.lunch-check').prop('checked', true);
                                }
                                //else {
                                //    $(row).find('.lunch-check').prop('checked', false);
                                //}
                                if (table.Snacks == '1' || (table.Snack==true)) {
                                    $(row).find('.snack-check').prop('checked', true);
                                }
                                //else {
                                //    $(row).find('.snack-check').prop('checked', false);
                                //}
                            }
                        }
                        else {

                            row = $('#singleDayAttance').find('table[day="' + tableIndex + '"]').find('tr[clientid="' + table.ClientID + '"]')
                            var refUserId = getDatabaseIndex((tableIndex + rowIndex), table.AttendanceDate);

                            if ($(row).attr('clientid') == table.ClientID && table.UserID == refUserId) {

                                switch (table.AttendanceType) {
                                    case '1':

                                        $(row).children('td').find('input[type=radio][value=' + table.AttendanceType + ']').prop('checked', true);
                                        $(row).find('.in-time-div,.out-time-div').show();
                                        var inTimePicker = $(row).children('td').find('.in-time ');
                                        var outTimePicker = $(row).children('td').find('.out-time');

                                        $(row).children('.time-td').children('.parent-sig-btn').removeClass('added-sig-btn');
                                        $(row).children('.in-time-td').children('.parent-sig-btn').removeClass('err-sig-btn');
                                        $(row).children('.in-time-td').children('.parent-sig-btn').addClass('normal-sig-btn');

                                        if (table.PSignatureIn != '') {
                                            $(row).children('.in-time-td').children('.sig-parent-hidden').val(table.PSignatureIn)
                                            $(row).children('.in-time-td').children('.sig-parent-hidden').val(table.PSignatureIn).attr('parentid', table.SignedInBy);
                                            $(row).children('.in-time-td').children('.parent-sig-btn').removeClass('normal-sig-btn');
                                            $(row).children('.in-time-td').children('.parent-sig-btn').removeClass('err-sig-btn');
                                            $(row).children('.in-time-td').children('.parent-sig-btn').addClass('added-sig-btn');
                                        }

                                        if (table.TSignatureIn != '') {
                                            $(row).children('.in-time-td').children('.sig-teacher-hidden').val(table.TSignatureIn);
                                            $(row).children('.in-time-td').children('.teacher-sig-btn').removeClass('normal-sig-btn');
                                            $(row).children('.in-time-td').children('.teacher-sig-btn').removeClass('err-sig-btn');
                                            $(row).children('.in-time-td').children('.teacher-sig-btn').addClass('added-sig-btn');
                                        }

                                        if (table.PSignatureOut != '') {
                                            $(row).children('.out-time-td').children('.sig-parent-hidden').val(table.PSignatureOut)
                                            $(row).children('.out-time-td').children('.sig-parent-hidden').val(table.PSignatureOut).attr('parentid', table.SignedOutBy);
                                            $(row).children('.out-time-td').children('.parent-sig-btn').removeClass('normal-sig-btn');
                                            $(row).children('.out-time-td').children('.parent-sig-btn').removeClass('err-sig-btn');
                                            $(row).children('.out-time-td').children('.parent-sig-btn').addClass('added-sig-btn');
                                        }

                                        //if (table.TSignatureOut != '') {
                                        //    $(row).children('.out-time-td').children('.sig-teacher-hidden').val(table.TSignatureIn);
                                        //    $(row).children('.out-time-td').children('.teacher-sig-btn').removeClass('normal-sig-btn');
                                        //    $(row).children('.out-time-td').children('.teacher-sig-btn').removeClass('err-sig-btn');
                                        //    $(row).children('.out-time-td').children('.teacher-sig-btn').addClass('added-sig-btn');
                                        //}

                                        inTimePicker.val(table.TimeIn).show();
                                        outTimePicker.val(table.TimeOut).show();


                                        $(row).find('.addSignature').show();
                                        $('#drawSignatureModal').find('#sigParent').signature();
                                        $(row).children('.meals-td').find('.meals-row').show();
                                        break;
                                    case '2':
                                        $(row).children('td').find('input[type=radio][value=' + table.AttendanceType + ']').prop('checked', true);
                                        $(row).children('td').find('.in-time ').val(table.TimeIn).hide();
                                        $(row).children('td').find('.out-time').val(table.TimeOut).hide();

                                        if (table.AbsenceReasonId == '' || table.AbsenceReasonId == '0') {
                                            $(row).children('td').find('.btn-reason').attr('reason-id', '0').addClass('.btn-reason-normal').removeClass('btn-reason-success').html('Add Reason').show();
                                        }
                                        else {
                                            $(row).children('td').find('.btn-reason').attr('reason-id', (table.AbsenceReasonId == '') ? '0' : table.AbsenceReasonId).addClass('btn-reason-success').removeClass('btn-reason-normal').html('Show Reason').show();

                                        }
                                        $(row).children('.out-time-td').find('.out-time-div').hide();
                                        $(row).children('.meals-td').find('.meals-row').hide();
                                        $(row).find('.addSignature').hide();
                                        break;
                                    case '3':
                                        $(row).children('td').find('input[type=radio][value=' + table.AttendanceType + ']').prop('checked', true);
                                        $(row).children('td').find('.in-time ').val(table.TimeIn).hide();
                                        $(row).children('td').find('.out-time').val(table.TimeOut).hide();
                                        $(row).children('.meals-td').find('.meals-row').hide();
                                        $(row).find('.addSignature').hide();
                                        break;
                                    default:
                                        $(row).children('td').find('input[type=radio]').prop('checked', false);
                                        $(row).children('td').find('.in-time').val('').hide();
                                        $(row).children('td').find('.out-time').val('').hide();
                                        $(row).find('.in-time-div,.out-time-div').show();
                                        $(row).find('.out-time-div').show();
                                        $(row).find('.meals-check').prop('checked', false);
                                        $(row).children('.meals-td').find('.meals-row').show();
                                        $(row).find('.addSignature').hide();
                                        break;

                                }

                                $(row).find('.meals-check').prop('checked', false);
                                if (table.BreakFast == '1') {
                                    $(row).find('.break-check').prop('checked', true);
                                }

                                if (table.Lunch == '1') {
                                    $(row).find('.lunch-check').prop('checked', true);
                                }
                                if (table.Snacks == '1') {
                                    $(row).find('.snack-check').prop('checked', true);
                                }
                            }


                            //functionality to change the height of the child info table w.r.to the attendance section-- for offline attendance only//

                            var rowheight = $(row).height();
                            var rowIndex = $(row).attr('row-index').substring(1);
                            offlineAttendance_div.find('#offline-child-info-tbody').find('#childinfo_' + rowIndex + '').height(rowheight);
                            //end of change row height functionality//  

                            showPushToServerBtnByData();
                        }


                        updateDailyMealsAndAttendance(tableIndex, false);


                    }

                });


            });
        }


    }


    setWidthofAttendanceTable();

    weeklyAttendance.ShowBusy(false);
}



function getDatabaseIndexByDateString(date, clientId) {

    if (weeklyAttendance.isHistorical()) {


        var dayIndex = weekAttendance_div.find('#day-heading-row').find('td[date="' + date + '"]').attr('day');
        var rowIndex = weekAttendance_div.find('.day-table[day=' + dayIndex + ']').find('.att-tr[day=' + dayIndex + ']').attr('row-index');
        return rowIndex
    }
    else {
        var dayIndex = offlineAttendance_div.find('#day-heading-row').children('td').attr('day');
        var rowIndex = offlineAttendance_div.find('.day-table[day=' + dayIndex + ']').find('.att-tr[day=' + dayIndex + ']').attr('row-index');
        return rowIndex
    }
}

function clearAllInputs() {
    cleanValidation();
    $('.in-time-div').show();
    $('.out-time-div').show();
    $('.meals-row').show();
    $('.time-text').val('').hide();
    $('.meals-check').each(function () {
        $(this).prop('checked', false);
    });
    $('#attendance-div').find('input[type=radio]').prop('checked', false);
    $('.child-breakfastcout').html(0);
    $('.child-lunchcount').html(0);
    $('.child-snackscount').html(0);
    $('.adult-breakfastcout').val(0);
    $('.adult-lunchcount').val(0);
    $('.adult-snackscount').val(0);

    $('.present-count').html(0);
    $('.execuse-count').html(0);
    $('.unexecuse-count').html(0);
    $('#child-week-breakfast-count').html(0);
    $('#child-week-lunch-count').html(0);
    $('#child-week-snacks-count').html(0);
    $('#adult-week-breakfast-count').html(0);
    $('#adult-week-lunch-count').html(0);
    $('#adult-week-snacks-count').html(0);
    $('#weekly-present-count').html(0);
    $('#weekly-excuse-count').html(0);
    $('#weekly-unexcuse-count').html(0);
}

function setWeekDays(dateEntered) {

    var MonthArr = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'July', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var arr = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday'];
    //var days = [];


    days = getDates(dateEntered);
    if (days.length > 0) {
        $.each(days, function (i, day) {
            var formattedtext = arr[day.getDay()] + '-' + day.getDate() + ' ' + MonthArr[day.getMonth()] + ' ' + day.getFullYear();
            var formattedDate = weeklyAttendance.getFormattedDate(day);
            switch (i) {
                case 0:
                    weekAttendance_div.find('#week-monday').html(formattedtext).attr({ 'date': formattedDate });
                    break;
                case 1:
                    weekAttendance_div.find('#week-tuesday').html(formattedtext).attr({ 'date': formattedDate });
                    break;
                case 2:
                    weekAttendance_div.find('#week-wednesday').html(formattedtext).attr({ 'date': formattedDate });
                    break;
                case 3:
                    weekAttendance_div.find('#week-thursday').html(formattedtext).attr({ 'date': formattedDate });
                    break;
                case 4:
                    weekAttendance_div.find('#week-friday').html(formattedtext).attr({ 'date': formattedDate });
                    break;

            }

        });
    }
}

function getDates(dateEntered) {


    var startDate = new Date(new Date(dateEntered).setDate(new Date(dateEntered).getDate() - new Date(dateEntered).getDay() + 1));
    var endDate = new Date(new Date(dateEntered).setDate(new Date(dateEntered).getDate() - new Date(dateEntered).getDay() + 5));


    var dates = [],
        currentDate = startDate,
        addDays = function (days) {
            var date = new Date(this.valueOf());
            date.setDate(date.getDate() + days);
            return date;
        };
    while (currentDate <= endDate) {
        dates.push(currentDate);
        currentDate = addDays.call(currentDate, 1);
    }
    return dates;
}

function setTodayDate(date) {
    var formattedDate = '';
    var arr1 = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday'];
    var MonthArr1 = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'July', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    formattedDate = arr1[date.getDay()] + ' - ' + date.getDate() + ' ' + MonthArr1[date.getMonth()] + ' ' + date.getFullYear();
    return formattedDate;
}

//callback functions
function alertCall(data) {
    bootbox.alert(data);
}

function InsertWeeklyAttendanceSubmit(data) {
    if (data.length > 0) {
        DataBaseManager.GetAllMeals(callBackInsertGetAllMeals, data);
    }
    customAlert('No Changes to Save.');
    weeklyAttendance.ShowBusy(false);
}


function deleteAllUsersFromDb(attendanceData, mealsData) {

    if (attendanceData.length > 0) {

        $.each(attendanceData, function (i, serverData) {

            DataBaseManager.DeleteUser(serverData.UserID, false);
        });

    }

    if (mealsData.length > 0) {
        $.each(mealsData, function (j, meals) {

            DataBaseManager.DeleteMeals(meals.DailyID, false);
        });
    }
}

function callBackInsertGetAllMeals(weeklyData, MealsData) {


    var mealsData = '';
    var attendanceData = '';
    var centerArr = weeklyAttendance.getCenterClassId();

    if (weeklyData.length > 0) {
        if (weeklyAttendance.isHistorical()) {
            attendanceData = $.grep(weeklyData, function (a) {
                return a.UserID.indexOf('his') > -1 && weeklyAttendance.getWeekDatesFormatted($('#datetimepicker1').val()).indexOf(a.AttendanceDate) > -1
                    && a.CenterID == centerArr.enc_CenterId && a.ClassroomID == centerArr.enc_ClassRoomId;
            });
        }
        else {
            attendanceData = $.grep(weeklyData, function (a) {
                return a.UserID.indexOf('off') > -1 && a.AttendanceDate == weeklyAttendance.getFormattedDate(new Date())
            });

        }
        attendanceData = (attendanceData.length > 0) ? JSON.stringify(attendanceData) : '';
    }
    if (MealsData.length > 0) {

        if (weeklyAttendance.isHistorical()) {

            mealsData = $.grep(MealsData, function (a) {
                return a.DailyID.indexOf('his') > -1 &&
                    weeklyAttendance.getWeekDatesFormatted($('#datetimepicker1').val()).indexOf(a.AttendanceDate) > -1 && a.CenterID == centerArr.enc_CenterId && a.ClassroomID == centerArr.enc_ClassRoomId
            });
        }
        else {

            mealsData = $.grep(MealsData, function (a) {
                return a.DailyID.indexOf('dai') > -1 && a.AttendanceDate == weeklyAttendance.getFormattedDate(new Date())
                    && a.CenterID == centerArr.enc_CenterId && a.ClassroomID == centerArr.enc_ClassRoomId
            });
        }
        mealsData = (mealsData.length > 0) ? JSON.stringify(mealsData) : '';
    }

    if (attendanceData.length > 0) {
        var classJson = weeklyAttendance.getCenterClassId();

        var attendanceDateString = '';
        if (weeklyAttendance.isHistorical()) {
            var attendDates = weeklyAttendance.getWeekDatesFormatted($('#datetimepicker1').val());
            $.each(attendDates, function (m, dates) {

                if ((attendDates.length - 1) == m) {
                    attendanceDateString += dates;
                }
                else {
                    attendanceDateString += dates + ',';
                }
            });
        }
        else {
            attendanceDateString = weeklyAttendance.getFormattedDate(new Date());
        }
        $.ajax({
            url: '/Teacher/InsertAttendanceData',
            type: 'post',
            //async:false,
            datatype: 'json',
            data: { userId: $('#userId').val(), agencyId: $('#agencyId').val(), centerId: classJson.enc_CenterId, classRoomId: classJson.enc_ClassRoomId, WeeklyAttendString: attendanceData, dailyMealsString: mealsData, dateString: attendanceDateString },
            success: function (data) {
                if (data.length > 0) {
                    weeklyAttendance.ShowBusy(false);
                    customAlert('Data saved successfully');
                    childAttendanceJson = data;

                    deleteAllUsersFromDb(JSON.parse(attendanceData), JSON.parse(mealsData));

                }
            },
            error: function (data) {
                weeklyAttendance.ShowBusy(false);
                customAlert('Error Occurred.Please,try again later')
            }

        });
    }
    else {
    }
}

function insertToServer() {
    
    DataBaseManager.GetAllClient(InsertWeeklyAttendanceSubmit);
}




//Shows the Status Online or Offline//

window.addEventListener("online", function (e) {
    reportOnlineStatus();

}, true);

window.addEventListener("offline", function (e) {
    reportOnlineStatus();
}, true);

$(document).on("keydown", disableF5);

$(window).bind("load resize", function () {

    topOffset = 0;
    width = (this.window.innerWidth > 0) ? this.window.innerWidth : this.screen.width;
    if (width < 768) {
        topOffset = 100; // 2-row-menu
    }

    height = ((this.window.innerHeight > 0) ? this.window.innerHeight : this.screen.height) - 1;

    height = height - topOffset;
    if (height < 1) height = 1;
    if (height > topOffset) {
        var fileHeight = height - 25;

        $('.right-side-container-ch').css('min-height', (fileHeight - 0) + 'px');
    }

    //Functionality to add the height to the child div// based on the screen size//
    var checkedDiv = $('#offline-hist-div').find('input[name=attentyperadio]:checked');

    if (checkedDiv.length > 0) {

        var _attendanceDiv = (checkedDiv.val() === '1') ? weekAttendance_div : offlineAttendance_div;
        var childDiv = (checkedDiv.val() === '1') ? _attendanceDiv.find('#weekly-child-info-body') : _attendanceDiv.find('#offline-child-info-tbody');
        //currently height has been adjusted for offline attendance only//
        if (checkedDiv.val() === '2') {
            _attendanceDiv.find('.day-table').each(function (n, daytable) {

                $(daytable).find('.att-tr').each(function (m, dayrow) {

                    var rowheight = $(dayrow).height();
                    var rowIndex = $(dayrow).attr('row-index').substring(1);
                    childDiv.find('#childinfo_' + rowIndex + '').height(rowheight);
                });

            });
        }


    }



});

function reportOnlineStatus() {
    var networkStatus = $('#network-status');
    var networkSpan = $('#network-span');
    if (isOnLine()) {
        networkStatus.css({ 'background': '#4CAF50' });
        networkSpan.text('Online');
        $('#push-to-server').show();
        center_para.find('select').prop('disabled', false);
        class_para.find('select').prop('disabled', false);
        off_hist_div.find('input[name=attentyperadio]').prop('disabled', false);
        $('#datetimepicker1').prop('disabled', false);
        $('#push-to-server').prop('disabled', false);
    }
    else {
        networkStatus.css({ 'background': '#e84c3d' });
        networkSpan.text('Offline');
        $('#push-to-server').hide();
        center_para.find('select').prop('disabled', true);
        class_para.find('select').prop('disabled', true);
        off_hist_div.find('input[name=attentyperadio]').prop('disabled', true);
        $('#datetimepicker1').prop('disabled', true);
        $('#push-to-server').prop('disabled', true);



    }


}

function isOnLine() {
    return navigator.onLine;
}

function isNumber(evt) {

    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        //return false;
        evt.preventDefault();
    }

}

$('#drawSignatureModal').find('#accept').on('click', function () {
    cleanValidation();
    isValid = false;
    var sigToImage = $('#drawSignatureModal').find('#sigParent').signature('toJSON');
    var source_btn = $(this).attr('source-btn');
    var isparent = $(this).attr('isparent');
    var client = {};
    client = weeklyAttendance.getEmptyClientJson();
    var centerArr = weeklyAttendance.getCenterClassId();

    var radioCheckLength = $('#drawSignatureModal').find('#parentNameDiv').find('input[type=radio]:checked').length;
    if (radioCheckLength === 0 && isparent === '1') {
        customAlert('Please select parent');
        return false;
    }

    var parentId = $('#drawSignatureModal').find('#parentNameDiv').find('input[type=radio]:checked').val();
    if ((sigToImage != '{"lines":[]}') && sigToImage != '') {

        $('#' + source_btn + '').removeClass('normal-sig-btn');
        $('#' + source_btn + '').removeClass('err-sig-btn');
        $('#' + source_btn + '').addClass('added-sig-btn');

        var parent_sign = $('#' + source_btn + '').siblings('.sig-parent-hidden');
        var teacher_sign = $('#' + source_btn + '').siblings('.sig-teacher-hidden');
        var row = $('#' + source_btn + '').parent('.time-td').parent('.att-tr');
        var row_index = $(row).attr('row-index');
        var timeIn = $('#' + source_btn + '').parent('.time-td').siblings('td').find('.in-time');
        var timeOut = $('#' + source_btn + '').parent('.time-td').siblings('td').find('.out-time');
        var isIntime = $('#' + source_btn + '').parent('.time-td').hasClass('in-time-td');
        var parentIn_sign = $('#' + source_btn + '').parent('.time-td').parent('.att-tr').children('.in-time-td').children('.sig-parent-hidden').val();
        var parentIn_Id = $('#' + source_btn + '').parent('.time-td').parent('.att-tr').children('.in-time-td').children('.sig-parent-hidden').attr('parentId');
        var teacherIn_sign = $('#' + source_btn + '').parent('.time-td').parent('.att-tr').children('.in-time-td').children('.sig-teacher-hidden').val();
        var parentOut_sign = $('#' + source_btn + '').parent('.time-td').parent('.att-tr').children('.out-time-td').children('.sig-parent-hidden').val();
        var parentOut_Id = $('#' + source_btn + '').parent('.time-td').parent('.att-tr').children('.out-time-td').children('.sig-parent-hidden').attr('parentId');

        // var teacherOut_sign = $('#' + source_btn + '').parent('.time-td').parent('.att-tr').children('.out-time-td').children('.sig-teacher-hidden').val();
        teacherOut_sign = '';
        var mealsRow = $('#' + source_btn + '').parent('.time-td').parent('.att-tr').children('.meals-td').find('.meals-row');


        client.ClientID = $(row).attr('clientid');
        client.AttendanceDate = weeklyAttendance.getAttendanceDate(new Date());
        client.UserID = getDatabaseIndex(row_index, weeklyAttendance.getFormattedDate(new Date()));
        client.AttendanceType = '1';

        client.BreakFast = (mealsRow.find('.meals-check').is(':checked')) ? '1' : '';
        client.Lunch = (mealsRow.find('.lunch-check').is(':checked')) ? '1' : '';
        client.Snacks = (mealsRow.find('.snack-check').is(':checked')) ? '1' : '';
        client.SignedInBy = parentIn_Id;
        client.SignedOutBy = parentOut_Id;
        if (isIntime) {
            if (isparent === '1') {
                timeIn.val(weeklyAttendance.formatOfflineAMPM()).show();
                parent_sign.val(sigToImage);
                parent_sign.attr('parentId', parentId);
                client.SignedInBy = parentId;
            }
            else {
                teacher_sign.val(sigToImage);
            }
            client.PSignatureIn = parent_sign.val();
            client.PSignatureOut = parentOut_sign;
            client.TSignatureIn = teacher_sign.val();
            client.TSignatureOut = teacherOut_sign;

            //InsertSignatureOnChange(client);
        }
        else {

            if (isparent === '1') {
                timeOut.val(weeklyAttendance.formatOfflineAMPM()).show();
                parent_sign.val(sigToImage);
                parent_sign.attr('parentId', parentId);
                client.SignedOutBy = parentId;

            }
            else {
                teacher_sign.val(sigToImage);
            }
            client.PSignatureIn = parentIn_sign;
            client.TSignatureIn = teacherIn_sign;
            client.PSignatureOut = parent_sign.val();
            client.TSignatureOut = '';

        }
        client.TimeIn = timeIn.val();
        client.TimeOut = timeOut.val();
        $('#drawSignatureModal').modal('hide');
        client.CenterID = centerArr.enc_CenterId;
        client.ClassroomID = centerArr.enc_ClassRoomId;
        InsertSignatureOnChange(client);


    }
    else {
        customAlert('Please enter signature');
        plainValidation('#sigParent');
    }
    return isValid;
});


$('#addReasonModal').find('#acceptReason').on('click', function () {

    cleanValidation();
    var absenceReasonId = $('#absenceReasonSelect').val();
    var centerArr = weeklyAttendance.getCenterClassId();

    var reasonId = '';
    var reasonBtn = '';
    var row = '';
    var row_index = '';
    var day = '';
    var date = '';
    var attendanceDate = '';

    if (absenceReasonId === '0') {
        customAlert('Please select reason');
        plainValidation('#absenceReasonSelect');
        return false;
    }
    else {
        reasonId = $(this).attr('sourcebtn');
        reasonBtn = $('#' + reasonId + '');
        row = $(reasonBtn).parent('.time-td').parent('.att-tr');
        row_index = $(row).attr('row-index');
        day = $(row).attr('day');

        if (weeklyAttendance.isHistorical()) {
            //$('#' + reasonId + '').attr('reason-id', absenceReasonId);
            $('#' + reasonId + '').css('background-color', '');
            //  $('#' + reasonId + '').addClass('btn-reason-success');
            weekAttendance_div.find('#' + reasonId + '').attr('reason-id', absenceReasonId).removeClass('btn-reason-normal').addClass('btn-reason-success').html('Show Reason');
            date = weekAttendance_div.find('#day-heading-row').find('td[day=' + day + ']').attr('date');
            attendanceDate = weeklyAttendance.getAttendanceDate(day);
        }
        else {
            offlineAttendance_div.find('#' + reasonId + '').attr('reason-id', absenceReasonId).removeClass('btn-reason-normal').addClass('btn-reason-success').html('Show Reason');
            date = offlineAttendance_div.find('#day-heading-row').find('td[day=' + day + ']').attr('date');
            attendanceDate = weeklyAttendance.getAttendanceDate(date);
        }

        var userId = getDatabaseIndex(row_index, attendanceDate);
        var clientId = $(row).attr('clientid');
        var client = {};
        client = weeklyAttendance.getEmptyClientJson();

        client.AbsenceReasonId = absenceReasonId;
        client.AttendanceDate = attendanceDate;
        client.AttendanceType = '2';
        client.BreakFast = '';
        client.ClientID = clientId;
        client.PSignatureIn = '';
        client.PSignatureOut = '';
        client.SignedInBy = '';
        client.SignedOutBy = '';
        client.Snacks = '';
        client.Lunch = '';
        client.TSignatureIn = '';
        client.TSignatureOut = '';
        client.TimeIn = '';
        client.TimeOut = '';
        client.UserID = userId;
        client.CenterID = centerArr.enc_CenterId;
        client.ClassroomID = centerArr.enc_ClassRoomId;


        DataBaseManager.GetSingleClient(client.UserID, client.ClientID, client.AttendanceDate, UpdateReasonCallback, client);
        $('#addReasonModal').modal('hide');
    }

});

$('#drawSignatureModal').find('#clear').on('click', function () {
    isValid = false;
    $('#drawSignatureModal').find('#sigParent').signature('clear');
    return isValid;
});


function UpdateReasonCallback(serverData, ClientData) {

    var client = {};
    client = weeklyAttendance.getEmptyClientJson();


    if (serverData.length > 0) {

        client.TimeIn = serverData[0].TimeIn;
        client.TimeOut = serverData[0].TimeOut;
        client.AbsenceReasonId = serverData[0].AbsenceReasonId;
        client.AttendanceDate = serverData[0].AttendanceDate;
        client.BreakFast = serverData[0].BreakFast;
        client.Lunch = serverData[0].Lunch;
        client.Snacks = serverData[0].Snacks;
        client.PSignatureIn = serverData[0].PSignatureIn;
        client.PSignatureOut = serverData[0].PSignatureOut;
        client.TSignatureIn = serverData[0].TSignatureIn;
        client.TSignatureOut = serverData[0].TSignatureOut;
        client.UserID = serverData[0].UserID;
        client.ClientID = serverData[0].ClientID;
        client.AttendanceType = serverData[0].AttendanceType;
        client.SignedInBy = serverData[0].SignedInBy;
        client.SignedOutBy = serverData[0].SignedOutBy;
        client.CenterID = serverData[0].CenterID;
        client.ClassroomID = serverData[0].ClassroomID;
        if (ClientData != null || ClientData != '') {


            //$.each(serverData, function (i, ele) {
            //    ele.TimeIn = ClientData.TimeIn;
            //    ele.TimeOut = ClientData.TimeOut;
            //    ele.PSignatureIn = ClientData.PSignatureIn;
            //    ele.TSignatureIn = ClientData.TSignatureIn;
            //    ele.PSignatureOut = ClientData.PSignatureOut;
            //    ele.TSignatureOut = ClientData.TSignatureOut;
            //    ele.AttendanceType = ClientData.AttendanceType;
            //    ele.AbsenceReasonId = ClientData.AbsenceReasonId;
            //    ele.BreakFast = ClientData.BreakFast;
            //    ele.Lunch = ClientData.Lunch;
            //    ele.Snacks = ClientData.Snacks;
            //});

            client.TimeIn = ClientData.TimeIn;
            client.TimeOut = ClientData.TimeOut;
            client.AbsenceReasonId = ClientData.AbsenceReasonId;
            client.BreakFast = serverData[0].BreakFast;
            client.Lunch = ClientData.Lunch;
            client.Snacks = ClientData.Snacks;
            client.PSignatureIn = ClientData.PSignatureIn;
            client.PSignatureOut = ClientData.PSignatureOut;
            client.TSignatureIn = ClientData.TSignatureIn;
            client.TSignatureOut = ClientData.TSignatureOut;
            client.AttendanceType = ClientData.AttendanceType;

            DataBaseManager.UpdateUser(client, false);
        }
    }
    else {
        DataBaseManager.InsertIntoTable(ClientData, false);
    }
}


function InsertSignatureOnChange(data) {

    if (data != null || data != "") {
        DataBaseManager.GetSingleClient(data.UserID, data.ClientID, data.AttendanceDate, updateOfflineSignatureCallback, data);
    }
}

function updateOfflineSignatureCallback(data, ClientData) {

    var client = {};
    if (data.length > 0) {

        client = weeklyAttendance.getEmptyClientJson();

        client.AttendanceDate = data[0].AttendanceDate;
        client.BreakFast = data[0].BreakFast;
        client.Lunch = data[0].Lunch;
        client.Snacks = data[0].Snacks;
        client.TimeIn = data[0].TimeIn;
        client.TimeOut = data[0].TimeOut;
        client.UserID = data[0].UserID;
        client.ClientID = data[0].ClientID;
        client.AttendanceType = data[0].AttendanceType;
        client.AbsenceReasonId = data[0].AbsenceReasonId;
        client.SignedInBy = data[0].SignedInBy;
        client.SignedOutBy = data[0].SignedOutBy;
        client.PSignatureIn = data[0].PSignatureIn;
        client.PSignatureOut = data[0].PSignatureOut;
        client.TSignatureIn = data[0].TSignatureIn;
        client.TSignatureOut = data[0].TSignatureOut;
        client.UserID = data[0].UserID;
        client.CenterID = data[0].CenterID;
        client.ClassroomID = data[0].ClassroomID;


        if (ClientData != null || ClientData != "") {
            //$.each(data, function (i, ele) {
            //    ele.TimeIn = ClientData.TimeIn;
            //    ele.TimeOut = ClientData.TimeOut;
            //    ele.PSignatureIn = ClientData.PSignatureIn;
            //    ele.TSignatureIn = ClientData.TSignatureIn;
            //    ele.PSignatureOut = ClientData.PSignatureOut;
            //    ele.TSignatureOut = ClientData.TSignatureOut;
            //    ele.SignedInBy = ClientData.SignedInBy;
            //    ele.SignedOutBy = ClientData.SignedOutBy;
            //});

            client.TimeIn = ClientData.TimeIn;
            client.TimeOut = ClientData.TimeOut;
            client.PSignatureIn = ClientData.PSignatureIn;
            client.TSignatureIn = ClientData.TSignatureIn;
            client.PSignatureOut = ClientData.PSignatureOut;
            client.TSignatureOut = ClientData.TSignatureOut;
            client.SignedInBy = ClientData.SignedInBy;
            client.SignedOutBy = ClientData.SignedOutBy;
        }
        DataBaseManager.UpdateUser(client, false);
    }
    else {
        DataBaseManager.InsertIntoTable(ClientData, false);
    }

    showPushToServerBtnByData();

}

function checkForToshowPushBtn() {

    var validcount = 0
    var isValid = false;
    var dayTable = offlineAttendance_div.find('table[day=0]');
    dayTable.find('.att-tr').each(function (i, dayrow) {
        // var radio = $(dayrow).children('.in-time-td').find('input[type=radio]');

        //  if ($(radio ).is(':checked')) {
        if ($(dayrow).children('.in-time-td').find('input[type=radio]:checked').length > 0) {
            var radio = $(dayrow).children('.in-time-td').find('input[type=radio]:checked');
            if ($(radio).attr('value') == '1') {
                var in_time_td = radio.parent().parent().parent('.in-time-td');
                var out_time_td = radio.parent().parent().parent('.in-time-td').siblings('.out-time-td');
                var in_time_td = radio.parent().parent().parent('.in-time-td');
                var out_time_td = radio.parent().parent().parent('.in-time-td').siblings('.out-time-td');

                if ((in_time_td.children('.sig-parent-hidden').val() == "") || (in_time_td.children('.sig-teacher-hidden').val() == "")) {
                    //isValid = false;
                    validcount++;
                }
                else {
                    isValid = true;
                }
                //if ((out_time_td.children('.sig-parent-hidden').val() == "") || (out_time_td.children('.sig-teacher-hidden').val() == "")) {
                //    //isValid = false;
                //    validcount++;
                //}

                if ((out_time_td.children('.sig-parent-hidden').val() == "")) {
                    //isValid = false;
                    validcount++;
                }
                else {
                    isValid = true;
                }
            }
            else if ($(radio).attr('value') == '2') {

                var reason_id = $(radio).parent('div').siblings('.btn-reason').attr('reason-id');
                if (reason_id == '0' || reason_id == '') {
                    validcount++;
                }
                else {
                    isValid = true;
                }
            }
            else {
                isValid = true;
            }
        }
        else {
            //isValid = false;
            validcount++;
        }
    });

    if (validcount > 0) {
        isValid = false;
    }
    else {
        isValid = true;
    }

    return isValid;
}

function showPushToServerBtnByData() {
    if (checkForToshowPushBtn()) {
        $('#push-to-server').show();
    }
    else {
        $('#push-to-server').hide();
    }
}

function disableF5(e) {

    if (!weeklyAttendance.isHistorical()) {
        if ((e.which || e.keyCode) == 116 || (e.which || e.keyCode) == 82) e.preventDefault();
    }

};
});

function bindSourceBtnId(val) {
    var rowIndex = $(val).parent('td').parent('.att-tr').attr('row-index');
    var row = rowIndex.substring(1, rowIndex.length);
    var childRow = $('#offline-child-info-tbody').children('#childinfo_' + row + '');
    var fatherId = $(childRow).children('#father_' + row + '').val();
    var fatherName = $(childRow).children('#father_' + row + '').attr('father_name');
    var motherId = $(childRow).children('#mother_' + row + '').val();
    var motherName = $(childRow).children('#mother_' + row + '').attr('mother_name');
    var parentString = '';

    parentString += '<label style="display: block;width:100%;margin-right:10px;" class="lbl-required">\
    Select Parent\
</label>';
    if (fatherId !== '0') {
        parentString += '<div class="" style="float:left;">\
                                   <input type="radio" name="parentRadio id="parent_radio" value=' + fatherId + '>&nbsp;<label style="font-weight:normal;">' + fatherName + ' (Father)</label>\
                               </div>';
    }

    if (motherId !== '0') {
        parentString += '<div class="" style="float:left;">\
                                   <input type="radio" name="parentRadio id="parent_radio" value='+ motherId + '>&nbsp;<label style="font-weight:normal;">' + motherName + ' (Mother)</label>\
                               </div>';
    }



    $('#drawSignatureModal').find('#sigParent').css({ 'background-color': '' });

    $('#drawSignatureModal').find('#accept').attr({ 'source-btn': $(val).attr('id'), 'isparent': $(val).attr('isparent') });
    if ($(val).attr('isparent') == '1') {
        $('#drawSignatureModal').find('#parentNameDiv').html(parentString).show();
        var sigValue = $(val).siblings('.sig-parent-hidden').val();
        var sigparentId = $(val).siblings('.sig-parent-hidden').attr('parentid');
        if (sigValue != '') {
            $('#drawSignatureModal').find('#parentNameDiv').find('input:radio[value="' + sigparentId + '"]').prop('checked', true);
            $('#drawSignatureModal').find('#sigParent').signature('draw', sigValue);
        }
        else {
            $('#drawSignatureModal').find('#sigParent').signature('clear');
        }
    }
    else {
        $('#drawSignatureModal').find('#parentNameDiv').html('').hide();
        var sigValue = $(val).siblings('.sig-teacher-hidden').val();
        if (sigValue != '') {
            $('#drawSignatureModal').find('#sigParent').signature('draw', sigValue);
        }
        else {
            $('#drawSignatureModal').find('#sigParent').signature('clear');
        }
    }

}

function bindReasonSourceBtn(val) {

    var reasonId = ($(val).attr('reason-id') == '' || $(val).attr('reason-id') == '0') ? '0' : $(val).attr('reason-id');
    $('#addReasonModal').find('#absenceReasonSelect').bindAbsenceReason().val(reasonId);
    $('#addReasonModal').find('#acceptReason').attr('sourceBtn', $(val).attr('id'));
}








