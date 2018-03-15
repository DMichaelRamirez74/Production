var sortOrder = '';
var sortDirection = '';
var imgID = '';
var direction = '';
var pageSize = 50;
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
    $.ajax({
        url: "/Matrix/GetAcronym",
        type: "POST",
        dataType: "json",
        secureuri: false,
        async: false,
        success: function (data) {
            $('#spanAcronym').text(data.AcronymName);
            $('#btnSave').attr('acr-id', data.AcronymId);
        }
        , error: function (response) {
             customAlert("Session Ended Log Onto The System Again.");setTimeout(function () {window.location.href= HostedDir + '/login/Loginagency';   }, 2000);
        }
    });


    $('#btnSave').click(function () {

        if ($('#acronymValue').val().trim() == "")
        {
            customAlert("Please enter acronym");
            plainValidation('#acronymValue');
            return false;
        }

        $.ajax({
            url: "/Matrix/AddAcronym",
            dataType: 'json',
            type: "POST",
            async: false,
            data: {
                AcronymName: $('#acronymValue').val().trim(), acronymId: parseInt($(this).attr('acr-id'))
            },
            success: function (data) {
                if (data) {
                    customAlert("Record saved successfully.");
                    $('#spanAcronym').text($('#acronymValue').val().trim().toUpperCase());
                    $('#acronymValue').val('');

                }
                       
                else
                    customAlert(data);

            },
            error: function (data) { alert(data); }
        });


    });

    $("#acronymValue").keypress(function (event) {
        var inputValue = event.which;
        if (!(inputValue >= 65 && inputValue <= 120) && (inputValue != 0)) {
            event.preventDefault();
        }
        $(this).val($(this).val().toUpperCase());
    });

});
   
