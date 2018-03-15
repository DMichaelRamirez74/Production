$(document).ready(function () {
    var clientId = $('#clientId_').val();
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
            console.log(arry);
            if (arry.length > 0) {
                $('#spnOrganizationName').html(arry[0].Services);
                $('#SpnCommunityAddress').html(arry[0].Address);
                $('#SpnCommunityPhone').html(arry[0].Phone);
                $('#SpnCommunityEmail').html(arry[0].Email);
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
                            console.log(data);
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

    $('#referralServiceSaveMethod').click(function () {

        var ServiceId = "";
        $(".chk_all").map(function () {
            if ($(this).is(':checked'))
                ServiceId += this.value + ",";
        });
        ServiceId = ServiceId.slice(0, -1);

        var ClientId = "";
        $(".CheckClient").map(function () {
            if ($(this).is(':checked'))
                ClientId += this.value + ",";
        });

        ClientId = ClientId.slice(0, -1);
        var HouseHoldId = $('#HouseHoldId').val();


        if ($('#ClientID:checked').val() == undefined && ($('#txtSearch').val() == "") && ($('#FFReferralSelect').val() == 0)) {
            
            $('#errshow').css("display", "inline-block");
            $('#errshow').text("Select Family Members");
            $('#errspan').css("display", "inline-block");
            $('#errspan').text("Enter Organization Name");
            $('#errnewspan').css("display", "inline-block");
            $('#errnewspan').text("Select Referral Type");
            return false;
        }

        else if ($('#ClientID:checked').val() == undefined) {
            $('#errshow').css("display", "inline-block");
            $('#errshow').text("Select Family Members");
            return false;
        }
        else if ($('#txtSearch').val() == "") {
            $('#errshow').hide();
            $('#errspan').css("display", "inline-block");
            $('#errspan').text("Enter Organization Name");
            //$('#referralError').html('Please select organization name');
            //$('#referralCompanyModal').modal('show');
            return false;
        }
            //else if ($('#FFReferral').hasClass('hidden')) {
        else if ($('#FFReferralSelect').val() == 0) {
            $('#errspan').hide();
            $('#errnewspan').css("display", "inline-block");
            $('#errnewspan').text("Select Referral Type");
            //$('#referralCompanyModal').modal('show');
            return false;
        }
            //    }
        else if ($('#datepicker').val() == "") {
            $('#errshow').hide();
            $('#errspan').hide();
            $('#errnewspan').hide();
            $('#errdate').css("display", "inline-block");
            $('#errdate').text("Please Enter referral date");
            // $('#referralCompanyModal').modal('show');
            return false;
        }
        else if (!isDate($('#datepicker').val().trim())) {
            $('#errshow').hide();
            $('#errspan').hide();
            $('#errnewspan').hide();
            $('#errdate').css("display", "inline-block");
            $('#errdate').text("Please Enter Valid date");
            // $('#referralCompanyModal').modal('show');
            return false;

        }

        var ReferralDate = $('#datepicker').val().trim();
        var Description = $('#Description').val().trim();
        var ref = null;
        var commId = $('#communityId').val();
        var AgencyId = $('#AgencyId_').val();

        if ($('#FFReferralSelect').hasClass('hidden')) {

            ServiceResourceId = $('#FFReferral').attr('referralId');
        }
        else {
            ServiceResourceId = $('#FFReferralSelect').val();
        }

        var ReferralClientServiceId = 0;

        var path = $('#MyURL').val();

        var AddReferral = {};
        AddReferral.ReferralDate = ReferralDate;
        AddReferral.Description = Description.trim();
        AddReferral.ServiceResourceId = parseInt(ServiceResourceId);
        AddReferral.AgencyId = AgencyId;
        AddReferral.CommunityId = parseInt(commId);
        AddReferral.ReferralClientServiceId = parseInt(ReferralClientServiceId);
        AddReferral.ClientId = parseInt(ClientId);
        AddReferral.HouseHoldId = parseInt(HouseHoldId);

        $.ajax({
            url: "/Roster/SaveReferral",
            type: "POST",
            data: AddReferral,
            success: function (data) {
                if (data = true) {
                    //  window.location.href = "@Url.Action("ReferralService", "Roster", new { id = ViewBag.Id, clientName = ViewBag.ClientName })";
                    window.location.href = path;
                }
            }
        });

    })


    $('#btnpdf').on('click', function () {

        if (($('#txtSearch').val() == "") && ($('#FFReferralSelect').val() == 0)) {
            $('#errshow').hide();
            $('#errshow').text("");
            $('#errspan').css("display", "inline");
            $('#errspan').text("Enter Organization Name");
            $('#errnewspan').css("display", "inline");
            $('#errnewspan').text("Select Referral Type");
            return false;
        }
        else if ($('#txtSearch').val() == "") {
            $('#errshow').hide();
            $('#errshow').text("");
            $('#errspan').css("display", "inline");
            $('#errspan').text("Enter Organization Name");
            //$('#referralError').html('Please select organization name');
            //$('#referralCompanyModal').modal('show');
            return false;
        }
            //else if ($('#FFReferral').hasClass('hidden')) {
        else if ($('#FFReferralSelect').val() == 0) {
            $('#errshow').hide();
            $('#errshow').text("");
            $('#errspan').hide();
            $('#errnewspan').css("display", "inline");
            $('#errnewspan').text("Select Referral Type");
            //$('#referralCompanyModal').modal('show');
            return false;
        }
            //    }
        else if ($('#datepicker').val() == "") {
            $('#errshow').hide();
            $('#errshow').text("");
            $('#errspan').hide();
            $('#errnewspan').hide();
            $('#errdate').css("display", "inline");
            $('#errdate').text("Please Enter referral date");
            // $('#referralCompanyModal').modal('show');
            return false;
        }

        else if (!isDate($('#datepicker').val())) {
            debugger;
            $('#errshow').hide();
            $('#errshow').text("");
            $('#errspan').hide();
            $('#errnewspan').hide();
            $('#errdate').css("display", "inline");
            $('#errdate').text("Please Enter Valid date");
            return false;
        }

        $('#errshow').hide();
        $('#errshow').text("");
        $('#errnewspan').hide();
        $('#errnewspan').text("");
        $('#errdate').hide();
        $('#errdate').text("");
        $('#errspan').hide();
        $('#errspan').text("");
        var CommunityId = $('#communityId').val();
        var referrlId = null;
        if ($('#FFReferralSelect').hasClass('hidden')) {
            referrlId = $('#FFReferral').attr('referralId');
        }
        else {
            referrlId = $('#FFReferralSelect').val();
        }

        var ServiceIdPDF = $('#ReferralId').val();
        var AgencyId = $('#AgencyId_').val();
        var clientId = $('#encryptId').val();
        var notes = $('#Description').val().trim();
        var referraldate = $('#datepicker').val();
        window.location.href = "/Roster/CompleteServicePdf?ServiceId=" + referrlId + "&AgencyID=" + AgencyId + "&ClientID=" + clientId + "&CommunityID=" + CommunityId + "&Notes=" + notes + "&referralDate=" + referraldate;



    });




    $('#btnpdf').on('click', function () {

        //if ($('#ClientID:checked').val() == undefined) {
        //    $('#referralError').html('Please select family memeber');
        //    $('#referralCompanyModal').modal('show');
        //    return false;
        //}

        if ($('#txtSearch').val() == "") {
            $('#referralError').html('Please select organization name');
            // $('#referralCompanyModal').modal('show');
            return false;
        }

        if ($('#FFReferral').hasClass('hidden')) {
            if ($('#FFReferralSelect').val() == 0) {
                $('#referralError').html('Please select referral type');
                //  $('#referralCompanyModal').modal('show');
                return false;
            }
        }
        if ($('#datepicker').val() == "") {
            $('#referralError').html('Please select referral date');
            // $('#referralCompanyModal').modal('show');
            return false;
        }
        else if (!isDate($('#datepicker').val())) {
            $('#errdate').css("display", "inline");
            $('#errdate').text("Please Enter Valid date");
            return false;
        }

        var CommunityId = $('#communityId').val();
        var referrlId = null;
        if ($('#FFReferralSelect').hasClass('hidden')) {
            referrlId = $('#FFReferral').attr('referralId');
        }
        else {
            referrlId = $('#FFReferralSelect').val();
        }

        var ServiceIdPDF = $('#ReferralId').val();
        var AgencyId = $('#AgencyId_').val();
        var clientId = $('#encryptId').val();
        var notes = $('#Description').val().trim();
        var referraldate = $('#datepicker').val();
        window.location.href = "/Roster/CompleteServicePdf?ServiceId=" + referrlId + "&AgencyID=" + AgencyId + "&ClientID=" + clientId + "&CommunityID=" + CommunityId + "&Notes=" + notes + "&referralDate=" + referraldate;
    });

    var Convdate1 = new Date();
    var convmonth1 = Convdate1.getMonth() + 1;
    var convdate1 = Convdate1.getDate();
    var month1 = (convmonth1 < 10) ? "0" + convmonth1 : convmonth1;
    var date1 = (convdate1 < 10) ? "0" + convdate1 : convdate1;
    var convertedDate1 = (month1 + '/' + date1 + '/' + Convdate1.getFullYear());
    $('#datepicker').val(convertedDate1);
    $('#datepicker').html(convertedDate1);



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


    $('body').on("keydown", "#datepicker", function (e) {
        flags++;
        if (flags > 1) {
            e.preventDefault();
        }

        var key = e.charCode || e.keyCode || 0;

        // allow backspace, tab, delete, enter, arrows, numbers and keypad numbers ONLY
        // home, end, period, and numpad decimal
        return (key == 8 || key == 9 || key == 13 || key == 46 || key == 32 || key == 37 || key == 39 ||
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


})