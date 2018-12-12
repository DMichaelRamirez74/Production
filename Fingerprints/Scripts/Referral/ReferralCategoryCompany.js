
var $referralCategoryCompanyPartial = null;
var _UserAgencyId = null;

$(document).ready(function () {

    $referralCategoryCompanyPartial = $('#div_referral_categorycompany_partial');
    _UserAgencyId = $referralCategoryCompanyPartial.find('#_userAgencyId').val();

    getOrganization();

    function getOrganization() {

        var serviceId =

        $("#Org-row").slideDown();

        $.ajax({
            url: "/Roster/GetOrganizationListByAgency",
            type: "POST",
            data: { AgencyId: _UserAgencyId },
            success: function (data) {
                drawOrganization(data);
            }, fail: function (res) {

            },
            complete: function (res) {

            }
        });

    }



    function drawOrganization(data) {

        $referralCategoryCompanyPartial.find("#organization-list").html('');

        if (data.length == 0) return false;


        data.forEach(function (item) {

            var _badgeDis = item.ReviewCount ? 'inline' : 'none';

            var _clrCode = item.CRColorCode == 1 ? 'red' : item.CRColorCode == 2 ? '#ffff00' : 'green';
            var _coutClr = item.CRColorCode == 1 ? '#ffffff' : item.CRColorCode == 2 ? '#333333' : '#ffffff';
            var _radioStr = '<div class="col-md-12 col-sm-12 col-xs-12" style="padding-left:0px;">'
          // var _radioStr = '<div class="" style="padding-bottom:10px;">'
           + '<label title="' + item.CompanyName + '" class="container-radio col-sm-11" style="display:inline;white-space:nowrap;overflow:hidden;text-overflow:ellipsis;width:auto;max-width:91.6%;">'
           + '' + item.CompanyName + ''
         //  + '<input type="radio" name="Organization" id="txtSearch" value="' + item.CommunityResourceID + '">'

  + '<input type="radio" name="Organization"  value="' + item.CommunityResourceID + '">' + '<span class="checkmark"></span>'
           + '</label>'
          // + '<i class="fa fa-external-link agency-review" data-index="' + item.CommunityResourceID + '"></i>'
          + '<span data-index="' + item.CommunityResourceID + '" class="badge show-reviewmodal col-sm-1" style="width:auto;background:' + _clrCode + ';color:' + _coutClr + ';display:' + _badgeDis + ';cursor:pointer;" data-toggle="tooltip" data-placement="right" title="' + item.ReviewCount + ' reviews">' + item.ReviewCount + '</span>'
           + '</div>';

            $referralCategoryCompanyPartial.find("#organization-list").append(_radioStr);

        });


        $referralCategoryCompanyPartial.find('[data-toggle="tooltip"]').tooltip();


    };



    //$(document).on("click", ".show-reviewmodal", function (e) {

    $referralCategoryCompanyPartial.find('.show-reviewmodal').on('click', function (e) {

        var _id = $(this).data('index');
        $('#spinner').show();
        $.ajax({
            url: "/Roster/GetReviewList?id=" + _id,
            type: "GET",
            //type: "POST",
            //data: { id: _id},
            success: function (data) {
                // drawOrganization(data);
                $('#spinner').hide();
                renderReviewList(data);

            }, fail: function (res) {

            },
            complete: function (res) {
                $('#spinner').hide();
            }
        });




    });

    function renderReviewList(data) {

        $referralCategoryCompanyPartial.find("#reviewlist-modal .modal-body").html('');

        if (data.length == 0) return false;

        var _tblStr = '<table class="table table-bordered" id="review-modal-table"><thead><tr><th>Review Color</th><th>Notes</th><th>Entered By</th><th>Entered Date</th><tr></thead><tbody>';

        data.forEach(function (item) {
            var _clr = item.CRColorCode == 1 ? 'red' : item.CRColorCode == 2 ? '#ffff00' : 'green';

            _tblStr += '<tr><td><div class="rlclrdiv" style="background:' + _clr + ';"></div></td><td>' + item.MgNotes + '</td><td>' + item.ModifiedBy + '</td><td>' + item.ModifiedDate + '</td> </tr>'
        });

        _tblStr += '<tbody></table>';

        $referralCategoryCompanyPartial.find("#reviewlist-modal .modal-body").append(_tblStr);
        $referralCategoryCompanyPartial.find("#reviewlist-modal").modal("show");

    };


    $(document).on("change", '[name="Organization"]', function (e) {
        var _cID = $(this).val();

        $.ajax({
            url: "/Roster/GetOrganization",
            type: "POST",
            dataType: "json",
            data: { CommunityId: _cID },
            success: function (data) {
                console.log(data);

                if (data) {

                    $referralCategoryCompanyPartial.find('#spnOrganizationName').html(data.OrganizationName);
                    $referralCategoryCompanyPartial.find('#SpnCommunityAddress').html(data.Address + ',' + data.City + ', ' + data.State + ',' + data.ZipCode);
                    $referralCategoryCompanyPartial.find('#SpnCommunityPhone').html(data.Phone);
                    $referralCategoryCompanyPartial.find('#SpnCommunityEmail').html(data.Email);
                }

                // var sid = ui.item.id;
                var sid = _cID;
                $referralCategoryCompanyPartial.find('#communityId').val(sid);
                var sidi = $referralCategoryCompanyPartial.find('#communityId').val(sid);
                //store in session
                //document.valueSelectedForAutocomplete = value

                var AgencyVal = $referralCategoryCompanyPartial.find('#AgencyId_').val();
                var AgencyId = AgencyVal;

                $.ajax({
                    url: "/Roster/GetReferralType",
                    type: "POST",
                    data: { communityId: sid },
                    success: function (data) {

                        if (data.length > 0) {
                            if (data.length > 1) {
                                console.log(data);
                                $referralCategoryCompanyPartial.find('#FFReferral').addClass('hidden');
                                $referralCategoryCompanyPartial.find("#FFReferralSelect").html('');
                                $referralCategoryCompanyPartial.find("#FFReferralSelect").append('<option value=' + 0 + '>' + "Select Referral Type" + '</option>');
                                for (var i = 0; i < data.length; i++) {
                                    $referralCategoryCompanyPartial.find("#FFReferralSelect").append('<option value=' + data[i].Value + '>' + data[i].Text + '</option>');
                                }
                                $referralCategoryCompanyPartial.find("#FFReferralSelect").removeClass('hidden');
                            }
                            else {
                                $referralCategoryCompanyPartial.find("#FFReferralSelect").addClass('hidden');
                                $referralCategoryCompanyPartial.find('#FFReferral').html('');
                                $referralCategoryCompanyPartial.find('#FFReferral').html(data[0].Text);
                                $referralCategoryCompanyPartial.find('#FFReferral').attr('referralId', data[0].Value);
                                $referralCategoryCompanyPartial.find('#FFReferral').removeClass('hidden');
                                $referralCategoryCompanyPartial.find('#errnewspan').css('display', 'none');
                            }
                        }


                    }

                });

            }
        })

    });


    var clientId = $referralCategoryCompanyPartial.find('#clientId_').val();
    var arry = null;
    var flags = 0;

    /*deprecated code=> org list changed to redio btns 12.10.18 - code0643*/
    /*
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
                            $('#FFReferral').attr('referralId', data[0].Value);
                            $('#FFReferral').removeClass('hidden');
                            $('#errnewspan').css('display', 'none');
                        }
                    }


                }

            });
        }
    });

    */

    $referralCategoryCompanyPartial.find('#referralServiceSaveMethod').click(function () {

        var ServiceId = "";
        $referralCategoryCompanyPartial.find(".chk_all").map(function () {
            if ($(this).is(':checked'))
                ServiceId += this.value + ",";
        });
        ServiceId = ServiceId.slice(0, -1);

        var ClientId = "";
        $referralCategoryCompanyPartial.find(".CheckClient").map(function () {
            if ($(this).is(':checked'))
                ClientId += this.value + ",";
        });

        ClientId = ClientId.slice(0, -1);
        var HouseHoldId = $referralCategoryCompanyPartial.find('#HouseHoldId').val();


        //  if ($('#ClientID:checked').val() == undefined && ($('#txtSearch').val() == "") && ($('#FFReferralSelect').val() == 0)) {
        if ($referralCategoryCompanyPartial.find('.CheckClient:checked').val() == undefined && (!$referralCategoryCompanyPartial.find('[name="Organization"]:checked').val()) && ($referralCategoryCompanyPartial.find('#FFReferralSelect').val() == 0)) {


            $referralCategoryCompanyPartial.find('#errshow').css("display", "inline-block");
            $referralCategoryCompanyPartial.find('#errshow').text("Select Family Members");
            $referralCategoryCompanyPartial.find('#errspan').css("display", "inline-block");
            $referralCategoryCompanyPartial.find('#errspan').text("Enter Organization Name");
            $referralCategoryCompanyPartial.find('#errnewspan').css("display", "inline-block");
            $referralCategoryCompanyPartial.find('#errnewspan').text("Select Referral Type");
            customAlert('Mandatory fields are required');

            $('html,body').animate({
                scrollTop: $referralCategoryCompanyPartial.find('#familymembersSpan').offset().top
            },
    'slow');

            return false;
        }

        else if ($referralCategoryCompanyPartial.find('.CheckClient:checked').val() == undefined) {
            $referralCategoryCompanyPartial.find('#errshow').css("display", "inline-block");
            $referralCategoryCompanyPartial.find('#errshow').text("Select Family Members");
            customAlert('Select family members');

            $('html,body').animate({
                scrollTop: $referralCategoryCompanyPartial.find('#familymembersSpan').offset().top
            },
  'slow');

            return false;
        }
            // else if ($('#txtSearch').val() == "") {
        else if (!$referralCategoryCompanyPartial.find('[name="Organization"]:checked').val()) {
            //           $('#errshow').hide();
            //           $('#errspan').css("display", "inline-block");
            //           $('#errspan').text("Enter Organization Name");

            //           $('html,body').animate({
            //               scrollTop: $('#txtSearch').offset().top
            //           },
            //'slow');

            customAlert("Please choose organization");

            return false;
        }
        else if ($referralCategoryCompanyPartial.find('#FFReferralSelect').is(':visible') && $referralCategoryCompanyPartial.find('#FFReferralSelect').val() == 0) {
            $referralCategoryCompanyPartial.find('#errspan').hide();
            $referralCategoryCompanyPartial.find('#errnewspan').css("display", "inline-block");
            $referralCategoryCompanyPartial.find('#errnewspan').text("Select Referral Type");
            customAlert('Please select referral type');
            $('html,body').animate({
                scrollTop: $referralCategoryCompanyPartial.find('#FFReferralSelect').offset().top
            },
 'slow');
            return false;
        }
        else if ($referralCategoryCompanyPartial.find('#datepicker').val() == "") {
            $referralCategoryCompanyPartial.find('#errshow').hide();
            $referralCategoryCompanyPartial.find('#errspan').hide();
            $referralCategoryCompanyPartial.find('#errnewspan').hide();
            $referralCategoryCompanyPartial.find('#errdate').css("display", "inline-block");
            $referralCategoryCompanyPartial.find('#errdate').text("Please Enter referral date");
            customAlert('Please enter referral date');

            $('html,body').animate({
                scrollTop: $referralCategoryCompanyPartial.find('#datepicker').offset().top
            },
 'slow');
            return false;
        }
        else if (!isDate($referralCategoryCompanyPartial.find('#datepicker').val().trim())) {
            $referralCategoryCompanyPartial.find('#errshow').hide();
            $referralCategoryCompanyPartial.find('#errspan').hide();
            $referralCategoryCompanyPartial.find('#errnewspan').hide();
            $referralCategoryCompanyPartial.find('#errdate').css("display", "inline-block");
            $referralCategoryCompanyPartial.find('#errdate').text("Please Enter Valid date");
            customAlert('Please enter valid date');

            $('html,body').animate({
                scrollTop: $referralCategoryCompanyPartial.find('#datepicker').offset().top
            },
'slow');
            return false;

        }

        var ReferralDate = $referralCategoryCompanyPartial.find('#datepicker').val().trim();
        var Description = $referralCategoryCompanyPartial.find('#Description').val().trim();
        var ref = null;
        var commId = $referralCategoryCompanyPartial.find('#communityId').val();
        var AgencyId = $referralCategoryCompanyPartial.find('#AgencyId_').val();

        if ($referralCategoryCompanyPartial.find('#FFReferralSelect').hasClass('hidden') || !$referralCategoryCompanyPartial.find('#FFReferralSelect').is(':visible')) {

            ServiceResourceId = $referralCategoryCompanyPartial.find('#FFReferral').attr('referralId');
        }
        else {
            ServiceResourceId = $referralCategoryCompanyPartial.find('#FFReferralSelect').val();
        }

        var ReferralClientServiceId = 0;

        var path = $referralCategoryCompanyPartial.find('#MyURL').val();

        var AddReferral = {};
        AddReferral.ReferralDate = ReferralDate;
        AddReferral.Description = Description.trim();
        AddReferral.ServiceResourceId = parseInt(ServiceResourceId);
        AddReferral.AgencyId = AgencyId;
        AddReferral.CommunityId = parseInt(commId);
        AddReferral.ReferralClientServiceId = parseInt(ReferralClientServiceId);
        AddReferral.ClientId = ClientId;
        AddReferral.HouseHoldId = parseInt(HouseHoldId);
        AddReferral.ScreeningReferralYakkr = $referralCategoryCompanyPartial.find('#referralYakkrId').val();
        AddReferral.CommonClientId = $referralCategoryCompanyPartial.find('#_encClientId').val();
        $.ajax({
            url: HostedDir + "/Roster/SaveReferral",
            type: "POST",
            data: AddReferral,
            success: function (data) {
                if (data = true) {

                    customAlert('Record saved successfully');

                    if (isReferralCateogryCompanyPopup())
                    {
                        window.setTimeout(function () {
                            $referralCategoryCompanyPartial.find('#Cancel').trigger('click');

                        }, 1000);
                    }
                    else
                    {

                        //  window.location.href = "@Url.Action("ReferralService", "Roster", new { id = ViewBag.Id, clientName = ViewBag.ClientName })";
                        window.location.href = path;

                    }

                }

                else {

                    customAlert('Error occurred. Please, try again later.');
                }

            },
            error:function(data)
            {
                
            }




            
        });

    })


    $referralCategoryCompanyPartial.find('#btnpdf').on('click', function () {

        debugger;
        if ($referralCategoryCompanyPartial.find('.CheckClient:checked').length === 0) {
            $referralCategoryCompanyPartial.find('#errshow').css("display", "inline-block");
            $referralCategoryCompanyPartial.find('#errshow').text("Select Family Members");
            return false;
        }


            // else if (($('#txtSearch').val() == "") && ($('#FFReferralSelect').val() == 0)) {
        else if (!$referralCategoryCompanyPartial.find('[name="Organization"]:checked').val() && ($referralCategoryCompanyPartial.find('#FFReferralSelect').val() == 0)) {
            $referralCategoryCompanyPartial.find('#errshow').hide();
            $referralCategoryCompanyPartial.find('#errshow').text("");
            $referralCategoryCompanyPartial.find('#errspan').css("display", "inline");
            $referralCategoryCompanyPartial.find('#errspan').text("Enter Organization Name");
            $referralCategoryCompanyPartial.find('#errnewspan').css("display", "inline");
            $referralCategoryCompanyPartial.find('#errnewspan').text("Select Referral Type");
            return false;
        }
            //else if ($('#txtSearch').val() == "") {
        else if (!$referralCategoryCompanyPartial.find('[name="Organization"]:checked').val()) {
            $referralCategoryCompanyPartial.find('#errshow').hide();
            $referralCategoryCompanyPartial.find('#errshow').text("");
            $referralCategoryCompanyPartial.find('#errspan').css("display", "inline");
            $referralCategoryCompanyPartial.find('#errspan').text("Enter Organization Name");
            //$('#referralError').html('Please select organization name');
            //$('#referralCompanyModal').modal('show');
            return false;
        }
            //else if ($('#FFReferral').hasClass('hidden')) {
        else if ($referralCategoryCompanyPartial.find('#FFReferralSelect').is(':visible') && $referralCategoryCompanyPartial.find('#FFReferralSelect').val() == 0) {
            $referralCategoryCompanyPartial.find('#errshow').hide();
            $referralCategoryCompanyPartial.find('#errshow').text("");
            $referralCategoryCompanyPartial.find('#errspan').hide();
            $referralCategoryCompanyPartial.find('#errnewspan').css("display", "inline");
            $referralCategoryCompanyPartial.find('#errnewspan').text("Select Referral Type");
            //$('#referralCompanyModal').modal('show');
            return false;
        }
            //    }
        else if ($referralCategoryCompanyPartial.find('#datepicker').val() == "") {
            $referralCategoryCompanyPartial.find('#errshow').hide();
            $referralCategoryCompanyPartial.find('#errshow').text("");
            $referralCategoryCompanyPartial.find('#errspan').hide();
            $referralCategoryCompanyPartial.find('#errnewspan').hide();
            $referralCategoryCompanyPartial.find('#errdate').css("display", "inline");
            $referralCategoryCompanyPartial.find('#errdate').text("Please Enter referral date");
            // $('#referralCompanyModal').modal('show');
            return false;
        }

        else if (!isDate($referralCategoryCompanyPartial.find('#datepicker').val())) {
            $referralCategoryCompanyPartial.find('#errshow').hide();
            $referralCategoryCompanyPartial.find('#errshow').text("");
            $referralCategoryCompanyPartial.find('#errspan').hide();
            $referralCategoryCompanyPartial.find('#errnewspan').hide();
            $referralCategoryCompanyPartial.find('#errdate').css("display", "inline");
            $referralCategoryCompanyPartial.find('#errdate').text("Please Enter Valid date");
            return false;
        }


        else {



            $referralCategoryCompanyPartial.find('#errshow').hide();
            $referralCategoryCompanyPartial.find('#errshow').text("");
            $referralCategoryCompanyPartial.find('#errnewspan').hide();
            $referralCategoryCompanyPartial.find('#errnewspan').text("");
            $referralCategoryCompanyPartial.find('#errdate').hide();
            $referralCategoryCompanyPartial.find('#errdate').text("");
            $referralCategoryCompanyPartial.find('#errspan').hide();
            $referralCategoryCompanyPartial.find('#errspan').text("");
            var CommunityId = $referralCategoryCompanyPartial.find('#communityId').val();
            var referrlId = null;
            if ($referralCategoryCompanyPartial.find('#FFReferralSelect').hasClass('hidden') || !$referralCategoryCompanyPartial.find('#FFReferralSelect').is(':visible')) {
                referrlId = $referralCategoryCompanyPartial.find('#FFReferral').attr('referralId');
            }
            else {
                referrlId = $referralCategoryCompanyPartial.find('#FFReferralSelect').val();
            }

            var ServiceIdPDF = $referralCategoryCompanyPartial.find('#ReferralId').val();
            var AgencyId = $referralCategoryCompanyPartial.find('#AgencyId_').val();
            var clientId = $referralCategoryCompanyPartial.find('#encryptId').val();
            var notes = $referralCategoryCompanyPartial.find('#Description').val().trim();
            var referraldate = $referralCategoryCompanyPartial.find('#datepicker').val();
            window.location.href = "/Roster/CompleteServicePdf?ServiceId=" + referrlId + "&AgencyID=" + AgencyId + "&ClientID=" + clientId + "&CommunityID=" + CommunityId + "&Notes=" + notes + "&referralDate=" + referraldate;
        }


    });




    //$referralCategoryCompanyPartial.find('#btnpdf').on('click', function () {

    //    //if ($('#ClientID:checked').val() == undefined) {
    //    //    $('#referralError').html('Please select family memeber');
    //    //    $('#referralCompanyModal').modal('show');
    //    //    return false;
    //    //}

    //    // if ($('#txtSearch').val() == "") {
    //    if (!$referralCategoryCompanyPartial.find('[name="Organization"]:checked').val()) {
    //        $referralCategoryCompanyPartial.find('#referralError').html('Please select organization name');
    //        // $('#referralCompanyModal').modal('show');
    //        return false;
    //    }

    //    if ($referralCategoryCompanyPartial.find('#FFReferral').hasClass('hidden')) {
    //        if ($referralCategoryCompanyPartial.find('#FFReferralSelect').val() == 0) {
    //            $referralCategoryCompanyPartial.find('#referralError').html('Please select referral type');
    //            //  $('#referralCompanyModal').modal('show');
    //            return false;
    //        }
    //    }
    //    if ($referralCategoryCompanyPartial.find('#datepicker').val() == "") {
    //        $referralCategoryCompanyPartial.find('#referralError').html('Please select referral date');
    //        // $('#referralCompanyModal').modal('show');
    //        return false;
    //    }
    //    else if (!isDate($referralCategoryCompanyPartial.find('#datepicker').val())) {
    //        $referralCategoryCompanyPartial.find('#errdate').css("display", "inline");
    //        $referralCategoryCompanyPartial.find('#errdate').text("Please Enter Valid date");
    //        return false;
    //    }

    //    var CommunityId = $referralCategoryCompanyPartial.find('#communityId').val();
    //    var referrlId = null;
    //    if ($referralCategoryCompanyPartial.find('#FFReferralSelect').hasClass('hidden')) {
    //        referrlId = $referralCategoryCompanyPartial.find('#FFReferral').attr('referralId');
    //    }
    //    else {
    //        referrlId = $referralCategoryCompanyPartial.find('#FFReferralSelect').val();
    //    }

    //    var ServiceIdPDF = $referralCategoryCompanyPartial.find('#ReferralId').val();
    //    var AgencyId = $referralCategoryCompanyPartial.find('#AgencyId_').val();
    //    var clientId = $referralCategoryCompanyPartial.find('#encryptId').val();
    //    var notes = $referralCategoryCompanyPartial.find('#Description').val().trim();
    //    var referraldate = $referralCategoryCompanyPartial.find('#datepicker').val();
    //    window.location.href = "/Roster/CompleteServicePdf?ServiceId=" + referrlId + "&AgencyID=" + AgencyId + "&ClientID=" + clientId + "&CommunityID=" + CommunityId + "&Notes=" + notes + "&referralDate=" + referraldate;
    //});



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


    function isReferralCateogryCompanyPopup() {

        if ($('#att-issue-modal').length > 0 && $('#att-issue-modal').is(':visible') && $('#refcatagorycompanyattenissuediv').length > 0 && $('#refcatagorycompanyattenissuediv').is(':visible')) {

            return true;
        }
        else {
            return false;
        }

    }


    //review list modal
    $(document).on("click", ".show-reviewmodal", function (e) {
        var _id = $(this).data('index');
        $('#spinner').show();
        $.ajax({
            url: "/Roster/GetReviewList?id=" + _id,
            type: "GET",
            //type: "POST",
            //data: { id: _id},
            success: function (data) {
                // drawOrganization(data);
                $('#spinner').hide();
                renderReviewList(data);

            }, fail: function (res) {

            },
            complete: function (res) {
                $('#spinner').hide();
            }
        });




    });





});
