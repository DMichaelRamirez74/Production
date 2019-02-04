
var RouteCode = "";
var closedToday = 0;
$(function () {

    //PageLoad
    BindParentAndChildDetailsOnLoad();
    function testTime(time) {
        var time = $(time).val();
        var re = /[0-2]?\d[.:][0-5]\d/;
        var matches = re.test(time);
        if (matches == true) {

            return true;

        } else {
            customAlert("Invalid Time Format");
            return false;
        }

    }
    $('#LateHours').timepicker({ 'timeFormat': 'h:i A' });
    $('#LateHours').val("");


    //BindChildDetails($('.ChidName').eq(0).attr('clientid'));

    if ($('.current-client-id').val() == "" || $('.current-client-id').val() == undefined)
        $('.current-client-id').val($('.ChidName').eq(0).attr('clientid'));

    BindChildDetails($('.current-client-id').val());
    // $('.head-child-name').text($('.li-child').eq(0).find('.ChidName').text());

    $('.ShowAbsent').click(function () {

        if (parseInt(closedToday) == 0) {
            $('#myModalAbsent').modal('show');
            $('.popup-child').text($('.head-child-name').text());
        }
        else {
            return false;
        }
    });

    $('.ShowAbsent').click(function () {

        if (parseInt(closedToday) == 0) {
            $('#myModalAbsent').modal('show');
            $('.popup-child').text($('.head-child-name').text());
        }
        else {
            return false;
        }
    });
    $('.ShowLate').click(function () {

        if (parseInt(closedToday) == 0) {
            $('#LateArrivalModal').modal('show');
            $('.popup-child').text($('.head-child-name').text());
        }
        else {
            return false;
        }
    }); 

    $('.ShowEngagement').click(function () {
        $('#myModalParentEngagement').modal('show');
    });

    $('.ShowEmail').on('click', function () {
        $('.ShowEmail').removeClass('current-message');
        if ($(this).find('.child-userid').text().trim() != "") {
            $('#myModalMessage').modal('show');
            $(this).addClass('current-message');
            $('.txt-mail-to').val($('.current-message').find('.child-name').text().trim());
        }
    });

    $("#myModalMessage").on("hidden.bs.modal", function () {
        $('.ShowEmail').removeClass('current-message');
        $('#myModalMessage .error-message').hide();
        $('.txt-mail-message').val("");
    });

    $("#LateArrivalModal").on("hidden.bs.modal", function () {

        $('#RNotes').val("");
        $('#LateHours').val("");
       
         $('.ShowEmail').removeClass('current-message');
        $('#LateArrivalModal .error-message').hide();
        $('.txt-mail-message').val("");
    });
    $("#myModalAddress").on("hidden.bs.modal", function () {
        $('#myModalAddress .txt-mandatory').val("");
        $('#myModalAddress .ddl-mandatory').val("0");
        $('#myModalAddress .error-message').hide();
    });
    $("#myModalEmployment").on("hidden.bs.modal", function () {
        $('#myModalEmployment .txt-mandatory').val("");
        $('#myModalEmployment .ddl-mandatory').val("0");
        $('#myModalEmployment .error-message').hide();
        $('.txt-employee-notes,.txt-employee-notes-2').val("");
    });
    $("#myModalMilitary").on("hidden.bs.modal", function () {
        $('#myModalMilitary .txt-mandatory').val("");
        $('#myModalMilitary .ddl-mandatory').val("0");
        $('#myModalMilitary .error-message').hide();
        $('.txt-military-notes,.txt-military-notes-2').val("");
    });
    $("#myModalHomeless").on("hidden.bs.modal", function () {
        $('#myModalHomeless .txt-mandatory').val("");
        $('#myModalHomeless .ddl-mandatory').val("0");
        $('#myModalHomeless .error-message').hide();
        $('.txt-homeless-notes').val("");
    });
    $("#myModalEducation").on("hidden.bs.modal", function () {
        $('#myModalEducation .txt-mandatory').val("");
        $('#myModalEducation .ddl-mandatory').val("0");
        $('#myModalEducation .error-message').hide();
        $('.txt-education-notes,.txt-education-notes-2').val("");
    });
    $("#myModalAbsent").on("hidden.bs.modal", function () {
        $('#myModalAbsent .txt-mandatory').val("");
        $('#divNewReason').hide();
        $('#myModalAbsent .ddl-mandatory').val("");
        $('#myModalAbsent .error-message').hide();
    });
    $("#myModalParentEngagement").on("hidden.bs.modal", function () {
        $('#myModalParentEngagement .txt-mandatory').val("");
        $('#myModalParentEngagement .ddl-mandatory').val("0");
        $('#myModalParentEngagement .error-message').hide();
    });
    $("#myModalVolunteer").on("hidden.bs.modal", function () {
        $('#myModalVolunteer .txt-mandatory').val("");
        $('#myModalVolunteer .ddl-mandatory').val("0");
        $('#myModalVolunteer .error-message').hide();
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

    $('body').on('click', '.li-child', function () {
        var clientId = $(this).find('.ChidName').attr('clientid');
        // BindChildDetails(clientId);
    });

    //AddressChange
    $('body').on('click touchstart', '.btn-change-address', function () {
        if (ValidateAddressChange()) {
            var AddressDetails = {};
            AddressDetails.DateOfChange = $('.txtAddressChangeDate').val();
            AddressDetails.Address = $('.txtAddress').val();
            AddressDetails.City = $('.txtAddressCity').val();
            AddressDetails.State = $('.ddlState').val();
            AddressDetails.ZipCode = $('.txtZipCode').val();
            AddressDetails.HouseHoldId = $('.sp-householdid').text();
            AddressDetails = JSON.stringify({ 'AddressDetails': AddressDetails });
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "POST",
                async: false,
                url: "/Parent/ChangeParentAddressRequest",
                data: AddressDetails,
                success: function (data) {
                    $('#myModalAddress').modal('hide');
                    $('#myModalAddress .txt-mandatory').val("");
                    $('#myModalAddress .ddl-mandatory').val("0");
                },
                error: function (data) {
                    console.log('Error');
                }
            });
        }
    });
  
    $('body').on('change','#ReasonList', function () {       
        if ($(this).val() == "-1") {
            $('#divNewReason').show();
        }
        else
        {
            $('#divNewReason').hide();
        }
    });
    //Clear Error Message
    $('body').on('keydown', '.txt-mandatory', function () {
        $(this).parent('.input-container').find('.error-message').hide();
    });
    $('body').on('change', '.ddl-mandatory', function () {
        $(this).parent('.input-container').find('.error-message').hide();
    });

    $('body').on("change", ".ActivityCode", function () {
        $('.error-message-type').hide()
    });

    $('body').on("change", ".dropdown-menu input[type='checkbox']", function () {
        $('.err-message-dayofvolunteer').hide();
    });
    //Employment Status Change
    $('body').on('click touchstart', '.btn-employement-status', function () {
        if (ValidateEmploymentStatusChange()) {
            var StatusChange = [];
            var Status = {};
            Status.Status = $('.ddl-employee-status').val();
            Status.Date = $('.txt-employee-date').val();
            Status.Notes = $('.txt-employee-notes').val();
            Status.ClientId = $('.sp-parent1').val();
            Status.HouseHoldId = $('.sp-householdid').text();
            Status.RouteCode = "46";
            StatusChange.push(Status);
            if ($('.sp-parent2').val() != undefined && $('.sp-parent2').val() != "") {
                Status = {};
                Status.Status = $('.ddl-employee-status-2').val();
                Status.Date = $('.txt-employee-date-2').val();
                Status.ClientId = $('.sp-parent2').val();
                Status.Notes = $('.txt-employee-notes-2').val();
                Status.HouseHoldId = $('.sp-householdid').text();
                Status.RouteCode = "46";
                StatusChange.push(Status);
            }
            StatusChange = JSON.stringify({ 'statuschange': StatusChange });
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "POST",
                async: false,
                url: "/Parent/EmployemntStatusChange",
                data: StatusChange,
                success: function (data) {
                    $('#myModalEmployment').modal('hide');
                    $('#myModalEmployment .txt-mandatory').val("");
                    $('#myModalEmployment .ddl-mandatory').val("0");
                    $('#myModalEmployment .error-message').hide();
                },
                error: function (data) {
                    console.log('Error');
                }
            });
        }
    });

    //Education Status Change
    $('body').on('click touchstart', '.btn-education-status', function () {
        if (ValidateEducationStatusChange()) {
            var StatusChange = [];
            var Status = {};
            Status.Status = $('.ddl-education-status').val();
            Status.Date = $('.txt-education-date').val();
            Status.Notes = $('.txt-education-notes').val();
            Status.ClientId = $('.sp-parent1').val();
            Status.HouseHoldId = $('.sp-householdid').text();
            Status.RouteCode = "49";
            StatusChange.push(Status);
            if ($('.sp-parent2').val() != undefined && $('.sp-parent2').val() != "") {
                Status = {};
                Status.Status = $('.ddl-education-status-2').val();
                Status.Date = $('.txt-education-date-2').val();
                Status.ClientId = $('.sp-parent2').val();
                Status.Notes = $('.txt-education-notes-2').val();
                Status.HouseHoldId = $('.sp-householdid').text();
                Status.RouteCode = "49";
                StatusChange.push(Status);
            }
            StatusChange = JSON.stringify({ 'statuschange': StatusChange });
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "POST",
                async: false,
                url: "/Parent/EducationStatusChange",
                data: StatusChange,
                success: function (data) {
                    $('#myModalEducation').modal('hide');
                    $('#myModalEducation .txt-mandatory').val("");
                    $('#myModalEducation .ddl-mandatory').val("0");
                    $('#myModalEducation .error-message').hide();
                },
                error: function (data) {
                    console.log('Error');
                }
            });
        }
    });

    //Military Status Change
    $('body').on('click touchstart', '.btn-military-status', function () {
        if (ValidateMilitaryStatusChange()) {
            var StatusChange = [];
            var Status = {};
            Status.Status = $('.ddl-military-status').val();
            Status.Date = $('.txt-military-date').val();
            Status.Notes = $('.txt-military-notes').val();
            Status.ClientId = $('.sp-parent1').val();
            Status.HouseHoldId = $('.sp-householdid').text();
            Status.RouteCode = "47";
            StatusChange.push(Status);

            if ($('.sp-parent2').val() != undefined && $('.sp-parent2').val() != "") {
                Status = {};
                Status.Status = $('.ddl-military-status-2').val();
                Status.Date = $('.txt-military-date-2').val();
                Status.ClientId = $('.sp-parent2').val();
                Status.Notes = $('.txt-military-notes-2').val();
                Status.HouseHoldId = $('.sp-householdid').text();
                Status.RouteCode = "47";
                StatusChange.push(Status);
            }
            StatusChange = JSON.stringify({ 'statuschange': StatusChange });
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "POST",
                async: false,
                url: "/Parent/MilitaryStatusChange",
                data: StatusChange,
                success: function (data) {
                    $('#myModalMilitary').modal('hide');
                    $('#myModalMilitary .txt-mandatory').val("");
                    $('#myModalMilitary .ddl-mandatory').val("0");
                    $('#myModalMilitary .error-message').hide();
                },
                error: function (data) {
                    console.log('Error');
                }
            });
        }
    });


    //Homeless Status Change
    $('body').on('click touchstart', '.btn-homeless-status', function () {
        if (ValidateHomelessStatusChange()) {
            var StatusChange = [];
            var status = {};
            status.Status = $('.ddl-homeless-status').val();
            status.Date = $('.txt-homeless-date').val();
            status.Notes = $('.txt-homeless-notes').val();
            status.ClientId = $('.sp-parent1').val();
            status.HouseHoldId = $('.sp-householdid').text();
            status.RouteCode = "48";
            StatusChange.push(status);
            StatusChange = JSON.stringify({ 'statuschange': StatusChange });
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "POST",
                async: false,
                url: "/Parent/HomelessStatusChange",
                data: StatusChange,
                success: function (data) {
                    $('#myModalHomeless').modal('hide');
                    $('#myModalHomeless .txt-mandatory').val("");
                    $('#myModalHomeless .ddl-mandatory').val("0");
                    $('#myModalHomeless .error-message').hide();
                },
                error: function (data) {
                    console.log('Error');
                }
            });
        }
    });
    //Volunteer Status Change
    $('body').on('click touchstart', '.btn-volunteer-status', function () {
        if (ValidateVolunteerChange()) {
            var Volunteer = [];
            var Status = {};
            var days = [];
            var Day = "";
            $('.dropdown-menu li input[type=checkbox]').each(function () {
                if ($(this).is(':checked') && $(this).val() != "multiselect-all") {
                    Day = Day.trim() + "," + $(this).val();
                    //console.log($(this).val());
                    //Day = $(this).val();
                    //days.push(Day);

                }
            });
        
            Status.Status = $('.ddl-volunteer-status').val() == "1" ? true : false;
            Status.Hours = $('.ddl-volunteer-hours').val();
            Status.days = Day;
            Status.ClientId = $('.sp-parent1').val();
            Status.HouseHoldId = $('.sp-householdid').text();
            Status.RouteCode = "44";
            Volunteer.push(Status);

            //if ($('.sp-parent2').val() != undefined && $('.sp-parent2').val() != "") {
            //    Status = {};
            //    Status.Status = $('.ddl-military-status-2').val();
            //    Status.Date = $('.txt-military-date-2').val();
            //    Status.ClientId = $('.sp-parent2').val();
            //    Status.Notes = $('.txt-military-notes-2').val();
            //    Status.RouteCode = "47";
            //    StatusChange.push(Status);
            //}
            Volunteer = JSON.stringify({ 'volunteer': Volunteer });
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "POST",
                async: false,
                url: "/Parent/VolunteerRequest",
                data: Volunteer,
                success: function (data) {
                    $('#myModalVolunteer').modal('hide');
                },
                error: function (data) {
                    console.log('Error');
                }
            });
        }
    });
    $('.btn-lateArrival-status').click(function () {

        if (ValidateLateArrival()) {
            var StatusChange = [];
            var status = {};
            status.Status = $('.ddl-absent-status').val();
            status.Notes = $('.txt-absent-notes').val();
            status.ClientId = $('.current-client-id').val();
            status.HouseHoldId = $('.sp-householdid').text();
            status.IsLateArrival = true;
            status.Time = $('#LateHours').val();
            StatusChange.push(status);
            StatusChange = JSON.stringify({ 'statuschange': StatusChange });
          
           
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "POST",
                async: false,
                url: "/Parent/MarkAbsentStatus",
                data: StatusChange,
                success: function (data) {
                    $('#LateArrivalModal').modal('hide');
                    $('#LateArrivalModal .txt-mandatory').val("");
                    $('#LateArrivalModal .ddl-mandatory').val("0");
                    $('#LateHours').val("");
                    $('#LateArrivalModal .error-message').hide();
                    $('.absentDiv').html("<p>Marked as Late Arrival</p>");
                 
                },
                error: function (data) {
                    console.log('Error');
                }
            });
        }
    });


    $('.btn-absent-status').click(function () {
      
        if (ValidateAbsentChange()) {
            var StatusChange = [];
            var status = {};
            status.Status = $('.ddl-absent-status').val();
            status.Notes = "";
            status.ClientId = $('.current-client-id').val();
            status.HouseHoldId = $('.sp-householdid').text();
            status.RouteCode = "70";
            status.IsLateArrival = false;
            status.Time = "";
            status.NewReason = $('#txtNewReason').val();
            StatusChange.push(status);
            StatusChange = JSON.stringify({ 'statuschange': StatusChange });
           
           
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "POST",
                async: false,
                url: "/Parent/MarkAbsentStatus",
                data: StatusChange,
                success: function (data) {
                    $('#myModalAbsent').modal('hide');
                    $('#myModalAbsent .txt-mandatory').val("");
                    $('#myModalAbsent .ddl-mandatory').val("0");
                    $('#myModalAbsent .error-message').hide();
                    $('.absentDiv').html("<p>Marked as Absent</p>");
                },
                error: function (data) {
                    console.log('Error');
                }
            });
        }
    });

    $('.btn-engagement-status').click(function () {
        if (ValidateParentEngagementChange()) {
            var ParentStatus = [];
            var Status = {};
            var days = [];
            var ActivityId = "";
            $('.ActivityCode').each(function () {
                if ($(this).is(':checked')) {
                    ActivityId = ActivityId.trim() + "," + $(this).val();
                }
            });

            Status.Date = $('.txt-engagement-date').val();
            Status.Hours = $('.ddl-engagement-hours').val();
            Status.Minutes = $('.ddl-engagement-minutes').val();
            Status.Notes = $('.txt-engagement-notes').val();
            Status.ActivityId = ActivityId;
            Status.HouseHoldId = $('.sp-householdid').text();
            Status.ClientId = $('.SelectedClient').find('.ChidName').attr('clientid');
            Status.RouteCode = "71";
            ParentStatus.push(Status);

            //if ($('.sp-parent2').val() != undefined && $('.sp-parent2').val() != "") {
            //    Status = {};
            //    Status.Status = $('.ddl-military-status-2').val();
            //    Status.Date = $('.txt-military-date-2').val();
            //    Status.ClientId = $('.sp-parent2').val();
            //    Status.Notes = $('.txt-military-notes-2').val();
            //    Status.RouteCode = "47";
            //    StatusChange.push(Status);
            //}
            ParentStatus = JSON.stringify({ 'statuschange': ParentStatus });
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "POST",
                async: false,
                url: "/Parent/ParentEngagement",
                data: ParentStatus,
                success: function (data) {
                    $('#myModalParentEngagement').modal('hide');
                },
                error: function (data) {
                    console.log('Error');
                }
            });
        }
    });

    $('body').on('click', '.BillingAttchment', function () {
        window.location.href = "/Parent/BillingAttachments";
    });
    $('body').on('click', '.Library', function () {
        window.location.href = "/Parent/Library";
    });

    $('.ddl-volunteer-status').change(function () {
        if ($(this).val() == "2") {
            $('.volunteer-hour,.volunteer-day').hide();
        }

        else {
            $('.volunteer-hour,.volunteer-day').show();
        }
    });

    $('.txt-mail-message').keyup(function () {
        $('.error-message-msg').hide();
    });
    //Send Message
    $('.btn-send-message').click(function () {
        $('.error-message-msg').hide();
        if ($('.txt-mail-message').val().trim() != "") {
            var ParentStatus = {};
            var ToStaffId = $('.current-message').find('.child-userid').text();
            var Status = {};
            ParentStatus.Status = $('.txt-mail-message').val();
            ParentStatus.ClientId = $('.SelectedClient').find('.ChidName').attr('clientid');
            ParentStatus.HouseHoldId = $('.sp-householdid').text();
            ParentStatus.RouteCode = $('.current-message').attr('route-code');
            ParentStatus = JSON.stringify({ 'statuschange': ParentStatus, 'ToStaffId': ToStaffId });
            $.ajax(
                        {
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            type: "POST",
                            async: false,
                            url: "/Parent/AddParentMessage",
                            data: ParentStatus,
                            success: function (data) {
                                $('#myModalMessage').modal('hide');
                            },
                            error: function (data) {
                                console.log('Error');
                            }
                        });
            $('.txt-mail-message').val("");
        }
        else {
            $('.error-message-msg').show();
        }
    });

    $('body').on("change", ".txt-date", function () {
        if ($(this).val() != "") {
            if (!$(this).hasClass('txt-engagement-date')) {
                var isValid = CheckFutureDate($(this).val());
                if (!isValid)
                    $(this).parent('.input-container').find('.error-future-date').show();
            }
            else {
                var isValid = CheckNotFutureDate($(this).val());
                if (!isValid)
                    $(this).parent('.input-container').find('.error-future-date').show();
                else {
                    isValid = CheckThirtyDaysAfter($(this).val().trim());
                    if (!isValid)
                        $(this).parent('.input-container').find('.error-older-date').show();
                }
            }
        }
    });

    $('.show-screen-details').click(function () {
        $.ajax({
            type: "POST",
            url: "/Parent/GetScreenResults",
            data: { 'ClientId': $('.SelectedClient').find('.ChidName').attr('clientid') },
            success: function (data) {

                $.each(JSON.parse(data), function (i, val) {
                    $('#DivclientmissingList tbody').empty();
                    var data = "<tr><td>" + val["ClientName"] + "</td><td><span>" + val["30 Day Physical"] + "</span></td><td><span>" + val["45 Day Hearing"] + " </span></td><td><span>" + val["45 Day Vision"] + " </span></td><td><span>" + val["90 Day Dental"] + " </span></td></tr>;"
                    $('#DivclientmissingList tbody').append(data);
                });
                $('#myModalScreeningResults').modal('show');
            },
            error: function (data) {
                console.log('Error');
            }
        });
    });

});

//$(window).ready(function () {
//    // alert($('.current-client-id').val());
//    //BindChildDetails($('.current-client-id').val());
//});
//Functions
function BindParentAndChildDetailsOnLoad() {
    $.ajax(
        {
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: "POST",
            async: false,
            url: "/Parent/GetParentDetails",
            success: function (data) {

                $(JSON.parse(data.JSONString)).each(function (i, val) {
                    if (val.Isfamily) {
                        $('.sp-householdid').text(val.HouseHoldID);
                        var Employed = val.Employed.toString() == "1" ? "Working" : val.Employed.toString() == "2" ? "Not Working" : "";
                        var CurrentMilitary = val.CurrentMilitary.toString() == "1" ? "None" : val.CurrentMilitary.toString() == "2" ? "Active" : val.CurrentMilitary.toString() == "3" ? "Veteran" : "";
                        var EducationLevel = val.EducationLevel.toString() == "1" ? "Advanced Degree or Baccalaureate" : val.EducationLevel.toString() == "2" ? "Associate, Vocational, or some College" : val.EducationLevel.toString() == "3" ? "High School Graduate or GED" : val.EducationLevel.toString() == "4" ? "Less than High school" : "";
                        if (val.ParentId == false) {
                            var FamilyHomeless = val.FamilyHomeless.toString() == "1" ? "Found Housing" : val.FamilyHomeless.toString() == "2" ? "Homeless" : "";
                            //  $('.sp-parentname').text(val.Name);
                            $('.parent-address').text(val.Address);
                            $('.homeless-status').text(FamilyHomeless);

                            $('.ddl-homeless-status option').each(function () {
                                if ($(this).text().trim() == FamilyHomeless) {
                                    $(this).remove();
                                }
                            });
                            $('.employment-status').text(Employed);
                            $('.ddl-employee-status').empty();
                            $('.ddl-employee-status').append('<option value="0">Choose...</option>');
                            if (Employed == "Working")
                                $('.ddl-employee-status').append('<option value="2">Not Working</option>');
                            if (Employed == "Not Working")
                                $('.ddl-employee-status').append('<option value="1">Working</option>');
                            $('.military-status').text(CurrentMilitary);
                            $('.ddl-military-status').empty();
                            $('.ddl-military-status').append('<option value="0">Choose...</option>');
                            if (CurrentMilitary == "None") {
                                $('.ddl-military-status').append('<option value="2">Active</option>');
                                $('.ddl-military-status').append('<option value="3">Veteran</option>');
                            }
                            else if (CurrentMilitary == "Active") {
                                $('.ddl-military-status').append('<option value="1">None</option>');
                                $('.ddl-military-status').append('<option value="3">Veteran</option>');
                            }
                            else if (CurrentMilitary == "Veteran") {
                                $('.ddl-military-status').append('<option value="1">None</option>');
                                $('.ddl-military-status').append('<option value="3">Active</option>');
                            }
                            $('.education-status').text(EducationLevel);
                            $('.ddl-education-status option').each(function () {
                                if ($(this).text().trim() == EducationLevel)
                                    $(this).remove();
                            });
                            $('.sp-parent1').val(val.ClientID);
                            $('.parent1-name').text(val.Name);
                        }
                        else {
                            $('.modal-parent-2').show();
                            $('.employment-status-2').text(Employed);
                            $('.ddl-employee-status-2').empty();
                            $('.ddl-employee-status-2').append('<option value="0">Choose...</option>');
                            if (Employed == "Working")
                                $('.ddl-employee-status-2').append('<option value="2">Not Working</option>');
                            if (Employed == "Not Working")
                                $('.ddl-employee-status-2').append('<option value="1">Working</option>');
                            $('.military-status-2').text(CurrentMilitary);
                            $('.ddl-military-status-2').empty();
                            $('.ddl-military-status-2').append('<option value="0">Choose...</option>');
                            if (CurrentMilitary == "None") {
                                $('.ddl-military-status-2').append('<option value="2">Active</option>');
                                $('.ddl-military-status-2').append('<option value="3">Veteran</option>');
                            }
                            else if (CurrentMilitary == "Active") {
                                $('.ddl-military-status-2').append('<option value="1">None</option>');
                                $('.ddl-military-status-2').append('<option value="3">Veteran</option>');
                            }
                            else if (CurrentMilitary == "Veteran") {
                                $('.ddl-military-status-2').append('<option value="1">None</option>');
                                $('.ddl-military-status-2').append('<option value="3">Active</option>');
                            }
                            $('.education-status-2').text(EducationLevel);
                            $('.ddl-education-status-2 option').each(function () {
                                if ($(this).text().trim() == EducationLevel)
                                    $(this).remove();
                            });
                            $('.sp-parent2').val(val.ClientID);
                            $('.parent2-name').text(val.Name);
                        }
                    }
                    else {
                        //   var list = "<li class='li-child'><a href='/Parent/ParentInfo?ClientId=" + val.ClientID + "'><span class='dash-icon'><img src='/Images/1.1.png'></span><span class='dash-icon1'><img src='/Images/1.2.png'></span><span clientid='" + val.ClientID + "' class='ChidName'>" + val.Name + "</span></a></li>";
                        //   $('.added-menu').append(list);
                    }
                });

                //console.log(data.ProfilePic);
                // var imagesrc = data.ProfilePic === "" ? ("/Images/prof-image.png") : ("data:image/jpg;base64," + data.ProfilePic);
                // $('#ProfileImage').attr('src', imagesrc);
                //$('.education-profile1 img').attr('src', imagesrc);
            },
            error: function (data) {
                console.log('Error');

            }
        });
}
function BindChildDetails(ClientId) {

    $.ajax({
        type: "POST",
        url: "/Parent/GetChildDetails",
        data: { 'ClientId': ClientId },
        success: function (data) {
        
          
            var Tables = JSON.parse(data.JSONString);
         
            $.each(Tables.Table, function (i, val) {
                var Insurance = val.Insurance.toString() == "1" ? " Medicaid/Chip" : val.Insurance.toString() == "2" ? "No Insurance" : val.Insurance.toString() == "3" ? "Other Insurance" : val.Insurance.toString() == "4" ? "Private Health Insurance" : val.Insurance.toString() == "5" ? "State Funded" : "";
                if (val.RoleId.toString() == "82b862e6-1a0f-46d2-aad4-34f89f72369a") {
                    $('.child-teacher-name').text(val.Name);
                    $('.child-teacher-phone').text(" " + val.Number);
                    $('.child-teacher-userid').text(val.UserId);
                }
                else if (val.RoleId.toString() == "94cdf8a2-8d81-4b80-a2c6-cdbdc5894b6d") {
                    $('.child-fsw-name').text(val.Name);
                    $('.child-fsw-phone').text(" " + val.Number);
                    $('.child-fsw-userid').text(val.UserId);
                }
                else if (val.RoleId.toString() == "A31B1716-B042-46B7-ACC0-95794E378B26") {
                    $('.child-nurse-name').text(val.Name);
                    $('.child-nurse-phone').text(" " + val.Number);
                    $('.child-nurse-userid').text(val.UserId);
                }
                // $('.TeacherEmail').val(data.Email);
                var percentage = parseFloat(val.PresentPercentage).toFixed(2) > 90 ? "Child has very great attendance" : parseFloat(val.PresentPercentage).toFixed(2) > 80 ? "Child has very good attendance" : parseFloat(val.PresentPercentage).toFixed(2) > 70 ? "Child has  good attendance" : parseFloat(val.PresentPercentage).toFixed(2) < 70 ? "Child has poor attendance" : "";
                if (percentage == "Child has poor attendance") {
                    $('.tick-symbol').hide(); $('.mark-symbol').show();
                }
                else {
                    $('.tick-symbol').show(); $('.mark-symbol').hide();
                }
                var percentage = parseFloat(val.PresentPercentage).toFixed(2) > 90 ? _langList.Child_has_very_great_attendance : parseFloat(val.PresentPercentage).toFixed(2) > 80 ? "Child has very good attendance" : parseFloat(val.PresentPercentage).toFixed(2) > 70 ? "Child has  good attendance" : parseFloat(val.PresentPercentage).toFixed(2) < 70 ? _langList.Child_has_poor_attendance : "";

                $('.child-attendance').text(percentage);
                $('.insurance-status').text(Insurance);
                $('.head-child-name').text(val.ClientName);

            });

            if (Tables != null) {
                if (Tables.Table.length > 0) {
                    var closedContent = '';
                    if (Tables.Table[0].TodayClosed > 0) {
                        closedToday = Tables.Table[0].TodayClosed;
                        if (Tables.Table[0].TodayClosed == 1) {
                            //closedContent = 'Agency - <span>' + Tables.Table[0].ClosedAgencyName + '</span> has been closed today';
                            closedContent = 'Classes has been closed today';
                        }
                        else if (Tables.Table[0].TodayClosed == 2) {
                            // closedContent = 'Center - <span>' + Tables.Table[0].ClosedCenterName + '</span> has been closed today';
                            closedContent = 'Classes has been closed today';
                        }
                        else {
                            //closedContent = 'Classroom - <span>' + Tables.Table[0].ClosedClassRoomName + '</span> has been closed today';

                            closedContent = 'Classes has been closed today';
                        }
                        $('#agency-closed-info').html(closedContent);
                        $('#agencyClosedDiv').show();
                    }
                    else {
                        $('#agency-closed-info').html('');
                        $('#agencyClosedDiv').hide();

                    }
                }
                else {
                    $('#agency-closed-info').html('');
                    $('#agencyClosedDiv').hide();

                }
            }
            else {
                $('#agency-closed-info').html('');
                $('#agencyClosedDiv').hide();

            }
            if (data.absenceReasonList != null && data.absenceReasonList.length > 0) {
                var option = "";
                if ($('#ReasonList').find('option').length < 1) {
                    option = option + '<option value="">Choose Reason</option>';
                    for (var i = 0; i < data.absenceReasonList.length; i++) {
                        option = option + '<option value="' + data.absenceReasonList[i].Value + '">' + data.absenceReasonList[i].Text + '</option>';
                    }
                    option = option + '<option value="-1">Others</option>';
                    $('#ReasonList').append(option);

                }
            }
           
            var imagesrc = data.ProfilePic === "" ? ("/Images/prof-image.png") : ("data:image/jpg;base64," + data.ProfilePic);
            // $('#ProfileImage').attr('src', imagesrc);
            $('.education-profile img').attr('src', imagesrc);
          
            if (data.IsLateArrival == true && data.IsMarkAbsent == 0)
            {
                $('.absentDiv').html("<p>Marked as Late Arrival</p>");
            }
            else if(data.IsLateArrival==false && data.IsMarkAbsent!=0)
            {
                $('.absentDiv').html("<p>Marked as Absent</p>");
            }
           

            //$.each(Tables.Table1, function (i, val) {
            //    var path = val.AttachmentPath.split('/');
            //    var Template = ' <a target="_blank" title="' + path[path.length - 1] + '" href="/Teacher/DownloadDocuments/?FilePath=' + val.AttachmentPath + '"><i class="fa fa-download download-ic"></i></a>';
            //    $('.billing-attachment').append(Template);
            //});

        },
        error: function (data) {
            console.log('Error');
        }
    });
}
function CheckFutureDate(date) {
    var isAllow = false;
    var now = new Date();
    var selectedDate = new Date(date);
    if (selectedDate > now) {
        isAllow = true;
    } else {
        isAllow = false;
    }
    return isAllow;
}
function CheckNotFutureDate(date) {
    var isAllow = false;
    var now = new Date();
    var selectedDate = new Date(date);
    if (selectedDate < now) {
        isAllow = true;
    } else {
        isAllow = false;
    }
    return isAllow;
}
function CheckThirtyDaysAfter(date) {
    var isAllow = false;
    var selectedDate = new Date(date);
    var today = new Date();
    var targetDate = new Date();
    targetDate.setDate(today.getDate() - 30);
    if (Date.parse(selectedDate) >= Date.parse(targetDate)) {
        isAllow = true;
    } else {
        isAllow = false;
    }
    return isAllow;
}
function ValidateAddressChange() {
    var isValid = true;
    $('#myModalAddress .txt-mandatory').each(function () {
        if ($(this).val().trim() == "") {
            if ($(this).hasClass('txt-date'))
                $(this).parent('.input-container').find('.error-message-empty').show();
            else
                $(this).parent('.input-container').find('.error-message').show();
            isValid = false;
        }
        else {
            if ($(this).hasClass('txt-date')) {
                isValid = CheckFutureDate($(this).val().trim());

                if (!isValid)
                    $(this).parent('.input-container').find('.error-future-date').show();

            }
            else
                $(this).parent('.input-container').find('.error-message').hide();
        }
    });
    $('#myModalAddress .ddl-mandatory').each(function () {
        if ($(this).val().trim() == 0) {
            $(this).parent('.input-container').find('.error-message').show();
            isValid = false;
        }
        else {
            $(this).parent('.input-container').find('.error-message').hide();
        }
    });
    return isValid;
}


function ValidateEmploymentStatusChange() {
    var isValid = true;
    $('.error-message').hide();
    if ($('.sp-parent2').val() != undefined && $('.sp-parent2').val() != "") {
        if (($('.ddl-employee-status').val() == "0" && $('.ddl-employee-status-2').val() == "0") || ($('.ddl-employee-status').val() != "0" && $('.ddl-employee-status-2').val() != "0")) {
            $('#myModalEmployment .modal-parent-1 .txt-mandatory').each(function () {
                if ($(this).val().trim() == "") {
                    if ($(this).hasClass('txt-date'))
                        $(this).parent('.input-container').find('.error-message-empty').show();
                    else
                        $(this).parent('.input-container').find('.error-message').show();
                    isValid = false;
                }
                else {
                    if ($(this).hasClass('txt-date')) {
                        isValid = CheckFutureDate($(this).val().trim());
                        if (!isValid)
                            $(this).parent('.input-container').find('.error-future-date').show();
                    }
                    else
                        $(this).parent('.input-container').find('.error-message').hide();
                }
            });
            $('#myModalEmployment .modal-parent-1 .ddl-mandatory').each(function () {
                if ($(this).val().trim() == 0) {
                    $(this).parent('.input-container').find('.error-message').show();
                    isValid = false;
                }
                else {
                    $(this).parent('.input-container').find('.error-message').hide();
                }
            });
            if ($('.sp-parent2').val() != undefined && $('.sp-parent2').val() != "") {
                $('#myModalEmployment .modal-parent-2 .txt-mandatory').each(function () {
                    if ($(this).val().trim() == "") {
                        if ($(this).hasClass('txt-date'))
                            $(this).parent('.input-container').find('.error-message-empty').show();
                        else
                            $(this).parent('.input-container').find('.error-message').show();
                        isValid = false;
                    }
                    else {
                        if ($(this).hasClass('txt-date')) {
                            isValid = CheckFutureDate($(this).val().trim());
                            if (!isValid)
                                $(this).parent('.input-container').find('.error-future-date').show();
                        }
                        else
                            $(this).parent('.input-container').find('.error-message').hide();
                    }
                });
                $('#myModalEmployment .modal-parent-2 .ddl-mandatory').each(function () {
                    if ($(this).val().trim() == 0) {
                        $(this).parent('.input-container').find('.error-message').show();
                        isValid = false;
                    }
                    else {
                        $(this).parent('.input-container').find('.error-message').hide();
                    }
                });
            }
        }
        else {
            if ($('.ddl-employee-status').val() != "0" && $('.ddl-employee-status-2').val() == "0") {
                $('#myModalEmployment .modal-parent-1 .txt-mandatory').each(function () {
                    if ($(this).val().trim() == "") {
                        if ($(this).hasClass('txt-date'))
                            $(this).parent('.input-container').find('.error-message-empty').show();
                        else
                            $(this).parent('.input-container').find('.error-message').show();
                        isValid = false;
                    }
                    else {
                        if ($(this).hasClass('txt-date')) {
                            isValid = CheckFutureDate($(this).val().trim());
                            if (!isValid)
                                $(this).parent('.input-container').find('.error-future-date').show();
                        }
                        else
                            $(this).parent('.input-container').find('.error-message').hide();
                    }
                });
            }
            else if ($('.ddl-employee-status').val() == "0" && $('.ddl-employee-status-2').val() != "0") {
                $('#myModalEmployment .modal-parent-2 .txt-mandatory').each(function () {
                    if ($(this).val().trim() == "") {
                        if ($(this).hasClass('txt-date'))
                            $(this).parent('.input-container').find('.error-message-empty').show();
                        else
                            $(this).parent('.input-container').find('.error-message').show();
                        isValid = false;
                    }
                    else {
                        if ($(this).hasClass('txt-date')) {
                            isValid = CheckFutureDate($(this).val().trim());
                            if (!isValid)
                                $(this).parent('.input-container').find('.error-future-date').show();
                        }
                        else
                            $(this).parent('.input-container').find('.error-message').hide();
                    }
                });
            }
        }
    }
    else {
        $('#myModalEmployment .modal-parent-1 .txt-mandatory').each(function () {
            if ($(this).val().trim() == "") {
                if ($(this).hasClass('txt-date'))
                    $(this).parent('.input-container').find('.error-message-empty').show();
                else
                    $(this).parent('.input-container').find('.error-message').show();
                isValid = false;
            }
            else {
                if ($(this).hasClass('txt-date')) {
                    isValid = CheckFutureDate($(this).val().trim());
                    if (!isValid)
                        $(this).parent('.input-container').find('.error-future-date').show();
                }
                else
                    $(this).parent('.input-container').find('.error-message').hide();
            }
        });
        $('#myModalEmployment .modal-parent-1 .ddl-mandatory').each(function () {
            if ($(this).val().trim() == 0) {
                $(this).parent('.input-container').find('.error-message').show();
                isValid = false;
            }
            else {
                $(this).parent('.input-container').find('.error-message').hide();
            }
        });
    }

    return isValid;
}

function ValidateMilitaryStatusChange() {
    var isValid = true;
    $('.error-message').hide();
    if ($('.sp-parent2').val() != undefined && $('.sp-parent2').val() != "") {
        if (($('.ddl-military-status').val() == "0" && $('.ddl-military-status-2').val() == "0") || ($('.ddl-military-status').val() != "0" && $('.ddl-military-status-2').val() != "0")) {
            $('#myModalMilitary .modal-parent-1 .txt-mandatory').each(function () {
                if ($(this).val().trim() == "") {
                    if ($(this).hasClass('txt-date'))
                        $(this).parent('.input-container').find('.error-message-empty').show();
                    else
                        $(this).parent('.input-container').find('.error-message').show();
                    isValid = false;
                }
                else {
                    if ($(this).hasClass('txt-date')) {
                        isValid = CheckFutureDate($(this).val().trim());
                        if (!isValid)
                            $(this).parent('.input-container').find('.error-future-date').show();
                    }
                    else
                        $(this).parent('.input-container').find('.error-message').hide();
                }
            });
            $('#myModalMilitary .modal-parent-1 .ddl-mandatory').each(function () {
                if ($(this).val().trim() == 0) {
                    $(this).parent('.input-container').find('.error-message').show();
                    isValid = false;
                }
                else {
                    $(this).parent('.input-container').find('.error-message').hide();
                }
            });
            if ($('.sp-parent2').val() != undefined && $('.sp-parent2').val() != "") {
                $('#myModalMilitary .modal-parent-2 .txt-mandatory').each(function () {
                    if ($(this).val().trim() == "") {
                        if ($(this).hasClass('txt-date'))
                            $(this).parent('.input-container').find('.error-message-empty').show();
                        else
                            $(this).parent('.input-container').find('.error-message').show();
                        isValid = false;
                    }
                    else {
                        if ($(this).hasClass('txt-date')) {
                            isValid = CheckFutureDate($(this).val().trim());
                            if (!isValid)
                                $(this).parent('.input-container').find('.error-future-date').show();
                        }
                        else
                            $(this).parent('.input-container').find('.error-message').hide();
                    }
                });
                $('#myModalMilitary .modal-parent-2 .ddl-mandatory').each(function () {
                    if ($(this).val().trim() == 0) {
                        $(this).parent('.input-container').find('.error-message').show();
                        isValid = false;
                    }
                    else {
                        $(this).parent('.input-container').find('.error-message').hide();
                    }
                });
            }
        }
        else {
            if ($('.ddl-military-status').val() != "0" && $('.ddl-military-status-2').val() == "0") {
                $('#myModalMilitary .modal-parent-1 .txt-mandatory').each(function () {
                    if ($(this).val().trim() == "") {
                        if ($(this).hasClass('txt-date'))
                            $(this).parent('.input-container').find('.error-message-empty').show();
                        else
                            $(this).parent('.input-container').find('.error-message').show();
                        isValid = false;
                    }
                    else {
                        if ($(this).hasClass('txt-date')) {
                            isValid = CheckFutureDate($(this).val().trim());
                            if (!isValid)
                                $(this).parent('.input-container').find('.error-future-date').show();
                        }
                        else
                            $(this).parent('.input-container').find('.error-message').hide();
                    }
                });
            }
            else if ($('.ddl-military-status').val() == "0" && $('.ddl-military-status-2').val() != "0") {
                $('#myModalMilitary .modal-parent-2 .txt-mandatory').each(function () {
                    if ($(this).val().trim() == "") {
                        if ($(this).hasClass('txt-date'))
                            $(this).parent('.input-container').find('.error-message-empty').show();
                        else
                            $(this).parent('.input-container').find('.error-message').show();
                        isValid = false;
                    }
                    else {
                        if ($(this).hasClass('txt-date')) {
                            isValid = CheckFutureDate($(this).val().trim());
                            if (!isValid)
                                $(this).parent('.input-container').find('.error-future-date').show();
                        }
                        else
                            $(this).parent('.input-container').find('.error-message').hide();
                    }
                });
            }
        }
    }
    else {
        $('#myModalMilitary .modal-parent-1 .txt-mandatory').each(function () {
            if ($(this).val().trim() == "") {
                if ($(this).hasClass('txt-date'))
                    $(this).parent('.input-container').find('.error-message-empty').show();
                else
                    $(this).parent('.input-container').find('.error-message').show();
                isValid = false;
            }
            else {
                if ($(this).hasClass('txt-date')) {
                    isValid = CheckFutureDate($(this).val().trim());
                    if (!isValid)
                        $(this).parent('.input-container').find('.error-future-date').show();
                }
                else
                    $(this).parent('.input-container').find('.error-message').hide();
            }
        });
        $('#myModalMilitary .modal-parent-1 .ddl-mandatory').each(function () {
            if ($(this).val().trim() == 0) {
                $(this).parent('.input-container').find('.error-message').show();
                isValid = false;
            }
            else {
                $(this).parent('.input-container').find('.error-message').hide();
            }
        });
    }

    return isValid;
}

function ValidateEducationStatusChange() {
    var isValid = true;
    $('.error-message').hide();
    if ($('.sp-parent2').val() != undefined && $('.sp-parent2').val() != "") {
        if (($('.ddl-education-status').val() == "0" && $('.ddl-education-status-2').val() == "0") || ($('.ddl-education-status').val() != "0" && $('.ddl-education-status-2').val() != "0")) {
            $('#myModalEducation .modal-parent-1 .txt-mandatory').each(function () {
                if ($(this).val().trim() == "") {
                    if ($(this).hasClass('txt-date'))
                        $(this).parent('.input-container').find('.error-message-empty').show();
                    else
                        $(this).parent('.input-container').find('.error-message').show();
                    isValid = false;
                }
                else {
                    if ($(this).hasClass('txt-date')) {
                        isValid = CheckFutureDate($(this).val().trim());
                        if (!isValid)
                            $(this).parent('.input-container').find('.error-future-date').show();
                    }
                    else
                        $(this).parent('.input-container').find('.error-message').hide();
                }
            });
            $('#myModalEducation .modal-parent-1 .ddl-mandatory').each(function () {
                if ($(this).val().trim() == 0) {
                    $(this).parent('.input-container').find('.error-message').show();
                    isValid = false;
                }
                else {
                    $(this).parent('.input-container').find('.error-message').hide();
                }
            });
            if ($('.sp-parent2').val() != undefined && $('.sp-parent2').val() != "") {
                $('#myModalEducation .modal-parent-2 .txt-mandatory').each(function () {
                    if ($(this).val().trim() == "") {
                        if ($(this).hasClass('txt-date'))
                            $(this).parent('.input-container').find('.error-message-empty').show();
                        else
                            $(this).parent('.input-container').find('.error-message').show();
                        isValid = false;
                    }
                    else {
                        if ($(this).hasClass('txt-date')) {
                            isValid = CheckFutureDate($(this).val().trim());
                            if (!isValid)
                                $(this).parent('.input-container').find('.error-future-date').show();
                        }
                        else
                            $(this).parent('.input-container').find('.error-message').hide();
                    }
                });
                $('#myModalEducation .modal-parent-2 .ddl-mandatory').each(function () {
                    if ($(this).val().trim() == 0) {
                        $(this).parent('.input-container').find('.error-message').show();
                        isValid = false;
                    }
                    else {
                        $(this).parent('.input-container').find('.error-message').hide();
                    }
                });
            }
        }
        else {
            if ($('.ddl-education-status').val() != "0" && $('.ddl-education-status-2').val() == "0") {
                $('#myModalEducation .modal-parent-1 .txt-mandatory').each(function () {
                    if ($(this).val().trim() == "") {
                        if ($(this).hasClass('txt-date'))
                            $(this).parent('.input-container').find('.error-message-empty').show();
                        else
                            $(this).parent('.input-container').find('.error-message').show();
                        isValid = false;
                    }
                    else {
                        if ($(this).hasClass('txt-date')) {
                            isValid = CheckFutureDate($(this).val().trim());
                            if (!isValid)
                                $(this).parent('.input-container').find('.error-future-date').show();
                        }
                        else
                            $(this).parent('.input-container').find('.error-message').hide();
                    }
                });
            }
            else if ($('.ddl-education-status').val() == "0" && $('.ddl-education-status-2').val() != "0") {
                $('#myModalEducation .modal-parent-2 .txt-mandatory').each(function () {
                    if ($(this).val().trim() == "") {
                        if ($(this).hasClass('txt-date'))
                            $(this).parent('.input-container').find('.error-message-empty').show();
                        else
                            $(this).parent('.input-container').find('.error-message').show();
                        isValid = false;
                    }
                    else {
                        if ($(this).hasClass('txt-date')) {
                            isValid = CheckFutureDate($(this).val().trim());
                            if (!isValid)
                                $(this).parent('.input-container').find('.error-future-date').show();
                        }
                        else
                            $(this).parent('.input-container').find('.error-message').hide();
                    }
                });
            }

        }
    }
    else {
        $('#myModalEducation .modal-parent-1 .txt-mandatory').each(function () {
            if ($(this).val().trim() == "") {
                if ($(this).hasClass('txt-date'))
                    $(this).parent('.input-container').find('.error-message-empty').show();
                else
                    $(this).parent('.input-container').find('.error-message').show();
                isValid = false;
            }
            else {
                if ($(this).hasClass('txt-date')) {
                    isValid = CheckFutureDate($(this).val().trim());
                    if (!isValid)
                        $(this).parent('.input-container').find('.error-future-date').show();
                }
                else
                    $(this).parent('.input-container').find('.error-message').hide();
            }
        });
        $('#myModalEducation .modal-parent-1 .ddl-mandatory').each(function () {
            if ($(this).val().trim() == 0) {
                $(this).parent('.input-container').find('.error-message').show();
                isValid = false;
            }
            else {
                $(this).parent('.input-container').find('.error-message').hide();
            }
        });
    }

    return isValid;
}

function ValidateHomelessStatusChange() {
    var isValid = true;
    $('#myModalHomeless  .txt-mandatory').each(function () {
        if ($(this).val().trim() == "") {
            if ($(this).hasClass('txt-date'))
                $(this).parent('.input-container').find('.error-message-empty').show();
            else
                $(this).parent('.input-container').find('.error-message').show();
            isValid = false;
        }
        else {
            if ($(this).hasClass('txt-date')) {
                isValid = CheckFutureDate($(this).val().trim());
                if (!isValid)
                    $(this).parent('.input-container').find('.error-future-date').show();
            }
            else
                $(this).parent('.input-container').find('.error-message').hide();
        }
    });
    $('#myModalHomeless .ddl-mandatory').each(function () {
        if ($(this).val().trim() == 0) {
            $(this).parent('.input-container').find('.error-message').show();
            isValid = false;
        }
        else {
            $(this).parent('.input-container').find('.error-message').hide();
        }
    });

    return isValid;
}



function ValidateVolunteerChange() {
    var isValid = true;

    if ($('.ddl-volunteer-status').val() != "2") {
        $('#myModalVolunteer .ddl-mandatory').each(function () {
            if ($(this).val().trim() == 0) {
                $(this).parent('.input-container').find('.error-message').show();
                isValid = false;
            }
            else {
                $(this).parent('.input-container').find('.error-message').hide();
            }
        });
        if ($('#myModalVolunteer .dropdown-toggle').attr('title') == "None selected") {
            isValid = false;
            $('.err-message-dayofvolunteer').show();
        }
        else {
            $('.err-message-dayofvolunteer').hide();
        }
        $('#myModalVolunteer .txt-mandatory').each(function () {
            if ($(this).val().trim() == "") {
                $(this).parent('.input-container').find('.error-message').show();
                isValid = false;
            }
            else {
                $(this).parent('.input-container').find('.error-message').hide();
            }
        });
    }

    return isValid;
}

function ValidateAbsentChange() {
    var isValid = true;
    $('.error-message').hide();
    if ($('#ReasonList').val().trim() == 0)
    {
        $('#ReasonList').parent('.input-container').find('.error-message').show();
        isValid = false;
    }

    if ($('#ReasonList').val().trim() == "-1") {
        if ($('#txtNewReason').val().trim() == "")
        {
            $('#txtNewReason').parent('.input-container').find('.error-message').show();
           isValid = false;
        }       
    }
   
    return isValid;
}

function ValidateLateArrival() {
    var isValid = true;
    $('.error-message').hide();
    if ($('#RNotes').val().trim() == "") {
        $('#RNotes').parent('.input-container').find('.error-message').show();
        isValid = false;
    }

    if ($('#LateHours').val() == "") {
        $('#LateHours').parent('.input-container').find('.error-message').show();
        isValid = false;
    }
    

    return isValid;
}
function getTime() {

    var dateselect = $('#StartDate').val();
    var time1 = $('#StartTime').val();
    var time2 = $('#TestEndTime').val();
    var am_pm = "AM";
    var Starttimehours = time1.substring(0, 1);

    //  var totalstarttime = starttime.substring(0, 5);
    var min1 = GetMinutes(time1.toString());
    var min2 = GetMinutes(time2.toString());
    var totalMins = parseInt(min1 + min2);
    var hours = parseInt((totalMins % (60 * 24)) / 60);
    var mins = parseInt((totalMins % (60 * 24)) % 60);
    if (mins == 0) {
        mins = "00";
    }
    if (hours > 12) {
        am_pm = "PM";
        hours = "0" + (hours - 12);
    }
    // alert(hours + ":" + mins + " " + am_pm);
    $('#EndTimeDuration').val(hours + ":" + mins + " " + am_pm);



    //var d = new Date(dateselect + starttime);
    //d.setHours(d.getHours() + 5);
    //d.setMinutes(d.getMinutes() + 30);
}
function ValidateParentEngagementChange() {
    var isValid = true;
    $('#myModalParentEngagement .txt-mandatory').each(function () {
        if ($(this).val().trim() == "") {
            if ($(this).hasClass('txt-date'))
                $(this).parent('.input-container').find('.error-message-empty').show();
            else
                $(this).parent('.input-container').find('.error-message').show();
            isValid = false;
        }
        else {
            if ($(this).hasClass('txt-date')) {
                isValid = CheckNotFutureDate($(this).val().trim());
                if (!isValid)
                    $(this).parent('.input-container').find('.error-future-date').show();
                else {
                    isValid = CheckThirtyDaysAfter($(this).val().trim());
                    if (!isValid)
                        $(this).parent('.input-container').find('.error-older-date').show();
                }
            }
            else
                $(this).parent('.input-container').find('.error-message').hide();
        }
    });
    $('#myModalParentEngagement .ddl-mandatory').each(function () {
        if ($('#ddlHours').val().trim() == 0 && $('#ddlMinutes').val().trim() == 0) {
            //$(this).parent('.input-container').find('.error-message').show();
            $('.error-hours,.error-minutes').show();
            isValid = false;
        }
        else {
            $('.error-hours,.error-minutes').hide();
        }
    });
    if ($('.ActivityCode:checked').val() == undefined) {
        isValid = false;
        $('.error-message-type').show();
    }
    else {
        $('.error-message-type').hide();
    }
    return isValid;
}

$(window).ready(function () {
    GetDocumentsDetails($('.ChidName').eq(0).attr('clientid'));
    function GetDocumentsDetails(ClientId) {
        $.ajax({
            type: "POST",
            url: "/EducationMaterial/GetPostedDocumentsDetailsForParent",
            data: { 'ClientId': ClientId },
            success: function (data) {
               
                // $('.library-table-head-history').empty();


                $('.library-table-head-General').empty();

                $('.library-table-head-education').empty();

                $('.library-table-head-Disabilites').empty();

                $('.library-table-head-Mental').empty();

                $('.library-table-head-Nutrition').empty();

                $('.library-table-head-Social').empty();

                $('.library-table-head-Transportation').empty();

                $('.library-table-head-Nursing').empty();

                // var input = "<option value='0'>Choose</option>";
                $.each(JSON.parse(data).Table, function (i, val) {
                   
                    var attchmenttemplate = "";
                    $.each(JSON.parse(data).Table1, function (i, value) {
                        if (val["Id"] == value["MaterialId"]) {
                            attchmenttemplate += '<a target="_blank" title="Download" href="/Teacher/DownloadDocuments/?FilePath=' + value["Attachmentpath"] + '"><i class="fa fa-download download-ic dwnload-ic" style="  font-size: 20px;\
                                padding-right: 10px;padding-top:10px;color:#f9c751;"></i></a>';

                        }
                    });


                    var template = '<tr>\
                                         <td data-title="Title">\
                                                                        <div class="library-content-desc" style="position: relative;">\
                                                                            <p>{Title}</p>\
                                                                        </div>\
                                                                    </td>\
                                                                    <td data-title="Description">\
                                                                        <div class="library-content-desc" style="position: relative;">\
                                                                            <p>{Description}</p>\
                                                                        </div>\
                                                                    </td>\
                                                                    <td data-title="Image">\
                                                                        <div class="library-content-desc" style="position: relative;">\
                                                                            <p>{Attachment}</p>\
                                                                        </div>\
                                                                    </td>\
                                                                    <td data-title="URL">\
                                                                        <div class="library-content-desc">\
                                                                            <p>{URL}</p>\
                                                                        </div>\
                                                                    </td>\
                                                                    <td data-title="Assigned By">\
                                                                        <div class="library-content-desc">\
                                                                            <p>{Assigned}</p>\
                                                                        </div>\
                                                                    </td>\
                                                                </tr>';
                    template = template.replace("{Title}", val["Title"]);
                    template = template.replace("{Description}", val["Description"]);
                    template = template.replace("{URL}", val["URL"]);
                    template = template.replace("{Attachment}", attchmenttemplate);
                    template = template.replace("{Assigned}", val["AssignedBy"]);




                    if (val["Group"] == "General")
                        $('.library-table-head-General').append(template);
                    else if (val["Group"] == "Education")
                        $('.library-table-head-education').append(template);
                    else if (val["Group"] == "Disabilites")
                        $('.library-table-head-Disabilites').append(template);
                    else if (val["Group"] == "Mental Health")
                        $('.library-table-head-Mental').append(template);
                    else if (val["Group"] == "Nutrition")
                        $('.library-table-head-Nutrition').append(template);
                    else if (val["Group"] == "Social Services")
                        $('.library-table-head-Social').append(template);
                    else if (val["Group"] == "Transportation")
                        $('.library-table-head-Transportation').append(template);
                    else if (val["Group"] == "Nursing")
                        $('.library-table-head-Nursing').append(template);

                });


            },
            error: function (data) {
                console.log('Error');
            }
        });
    }
});