var $referralCategoryPartial=null;
var AgencyValue = null;

var result = "";
var ParentName = "";

$(document).ready(function () {


    $referralCategoryPartial = $('#div_referral_category_partial');

    if ($('#referralClientId').val().trim() != "" && $('#referralClientId').val() != '0') {

        var referralclientId =  $referralCategoryPartial.find('#referralClientId').val();
        $.ajax({

            url: HostedDir+"/Roster/HouseHoldReferrals",
            datatype: "json/application",
            data: { referralClientId: referralclientId },
            success: function (data) {
                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        var parentId = data[i].Value;

                            $referralCategoryPartial.find('input[type=checkbox][value=' + parentId + ']').prop('checked', true)
                    }
                }
            }
        });
    }




$referralCategoryPartial.find('.check').on('click', function (e) {

    $referralCategoryPartial.find('.chk_all').prop('checked', true)
});


$referralCategoryPartial.find('.uncheck').on('click', function (e) {

    $referralCategoryPartial.find('.chk_all').prop('checked', false)
});



$referralCategoryPartial.find('#Save').click(function (e) {

     AgencyValue = $('#AgencyId_').val();
    var ServiceId = "";
    $referralCategoryPartial.find(".chk_all").map(function () {
        if ($(this).is(':checked'))
            ServiceId += this.value + ",";
    });
    ServiceId = ServiceId.slice(0, -1);

    var ClientId = "";
    $referralCategoryPartial.find(".CheckClient").map(function () {
        if ($(this).is(':checked'))
            ClientId += this.value + ",";
    });

    ClientId = ClientId.slice(0, -1);
    HouseHoldId = $('#HouseHoldId').val();

    var UniqueClientId = $('#UniqueClientId').val();

    if ($referralCategoryPartial.find('.CheckClient:checked').val() == undefined && ($('.chk_all:checked').val() == undefined)) {
        $referralCategoryPartial.find('.validation1').css("display", "inline-block");
        $referralCategoryPartial.find('.validation1').text("Select Family Members");
        $referralCategoryPartial.find('.validation2').css("display", "inline-block");
        $referralCategoryPartial.find('.validation2').text(" Select Referral Members");
        $referralCategoryPartial.find('.setfamily').addClass('setcolor');
        $referralCategoryPartial.find('.setreferral').addClass('setcolor');

        $('html,body').animate({
            scrollTop: $referralCategoryPartial.find('.setfamily').offset().top
        },
      'slow');

        customAlert('Select family member(s)');

    }
    else if ($referralCategoryPartial.find('.CheckClient:checked').val() == undefined) {
        $referralCategoryPartial.find('.validation1').css("display", "inline-block");
        $referralCategoryPartial.find('.validation1').text("Select Family Members");
        $referralCategoryPartial.find('.validation2').hide();
        $referralCategoryPartial.find('.setfamily').addClass('setcolor');
        $referralCategoryPartial.find('.setreferral').removeClass('setcolor');
        $('html,body').animate({
            scrollTop: $referralCategoryPartial.find('.setfamily').offset().top
        },
  'slow');

        customAlert('Select family member(s)');
    }
    else if ($referralCategoryPartial.find('.chk_all:checked').val() == undefined) {
        $referralCategoryPartial.find('.validation2').css("display", "inline-block");
        $referralCategoryPartial.find('.validation2').text(" Select Referral Members");
        $referralCategoryPartial.find('.validation1').hide();
        $referralCategoryPartial.find('.setfamily').removeClass('setcolor');
        $referralCategoryPartial.find('.setreferral').addClass('setcolor');

        $('html,body').animate({
            scrollTop: $referralCategoryPartial.find('.setreferral').offset().top
        },
  'slow');
        customAlert('Select referral member(s)');

    }

    else {
        $referralCategoryPartial.find('.validation1').hide();
        $referralCategoryPartial.find('.validation2').hide();
        $referralCategoryPartial.find('.setfamily').removeClass('setcolor');
        $referralCategoryPartial.find('.setreferral').removeClass('setcolor');
        var SaveReferral = {};
        SaveReferral.ServiceId = ServiceId;
        SaveReferral.AgencyId = AgencyValue;
        SaveReferral.CommonClientId = UniqueClientId;
        SaveReferral.HouseHoldId = parseInt(HouseHoldId);
        SaveReferral.ClientId = ClientId;
        SaveReferral.referralClientId = parseInt($referralCategoryPartial.find('#referralClientId').val());
        SaveReferral.ScreeningReferralYakkr = $referralCategoryPartial.find('#referenceYakkrId').val();
  
        $.ajax({
            url: HostedDir+"/Roster/SaveReferralClient",
            type: "POST",
            data: SaveReferral,
            beforeSend: function () { $('#spinner').show() },
            success: function (data) {

               

                if (data)
                {
                    customAlert('Record saved successfully');

                    window.setTimeout(function () {
                        $referralCategoryPartial.find('#Cancel').trigger('click');
                    }, 1000);



                }

                else {

                    $('#spinner').hide();
                    customAlert('Error occurred. Please, try again later');
                }
                //$('#myModal').modal('show');
              

                //window.setTimeout(function () {


                //});

               

            },
            error: function (data) {
                $('#spinner').hide();
            },
            complete: function (data) {
                $('#spinner').hide();
            }

        });


    }
});


$referralCategoryPartial.find("#Matchprovider").click(function (e) {

     AgencyValue = $referralCategoryPartial.find('#AgencyId_').val();
     result = "";
    $referralCategoryPartial.find(".chk_all").map(function () {
        if ($(this).is(':checked'))
            result += this.value + ",";
    });
    result = result.slice(0, -1);


    var CommunityId = $referralCategoryPartial.find('.chk_all:checked').val();
    $referralCategoryPartial.find('.parentName').val();
     ParentName = $referralCategoryPartial.find('.CheckClient').attr('parentname');
    var householdId = $referralCategoryPartial.find('#HouseHoldId').val();

    if ($referralCategoryPartial.find('.CheckClient:checked').val() == undefined && ($referralCategoryPartial.find('.chk_all:checked').val() == undefined)) {
        $referralCategoryPartial.find('.validation1').css("display", "inline-block");
        $referralCategoryPartial.find('.validation1').text("Select Family Members");
        $referralCategoryPartial.find('.validation2').css("display", "inline-block");
        $referralCategoryPartial.find('.validation2').text(" Select Referral Members");
        $referralCategoryPartial.find('.setfamily').addClass('setcolor');
        $referralCategoryPartial.find('.setreferral').addClass('setcolor');
    }
    else if ($referralCategoryPartial.find('.CheckClient:checked').val() == undefined) {
        $referralCategoryPartial.find('.validation1').css("display", "inline-block");
        $referralCategoryPartial.find('.validation1').text("Select Family Members");
        $referralCategoryPartial.find('.validation2').hide();
        $referralCategoryPartial.find('.setfamily').addClass('setcolor');
        $referralCategoryPartial.find('.setreferral').removeClass('setcolor');

    }
    else if ($referralCategoryPartial.find('.chk_all:checked').val() == undefined) {
        $referralCategoryPartial.find('.validation2').css("display", "inline-block");
        $referralCategoryPartial.find('.validation2').text(" Select Referral Members");
        $referralCategoryPartial.find('.validation1').hide();
        $referralCategoryPartial.find('.setfamily').removeClass('setcolor');
        $referralCategoryPartial.find('.setreferral').addClass('setcolor');

    }
    else {

        if (!isReferralPopup())
        {
            window.location.href = HostedDir + "/Roster/MatchProviders?AgencyId=" + AgencyValue + "&CommunityIds=" + result + "&parentName=" + ParentName + "&referralClientId=" + $referralCategoryPartial.find('#referralClientId').val() + "&clientName=" + $referralCategoryPartial.find('#clientName').val() + "&id=" + $referralCategoryPartial.find('#id').val() + "&ScreeningReferralYakkr=" + $referralCategoryPartial.find('#referenceYakkrId').val();

        }
        else {
            getMatchProviders(this);
        }



    }

});

});


function isReferralPopup()
{

    if($('#att-issue-modal').length>0 && $('#att-issue-modal').is(':visible') && $('#refcatagoryattenissuediv').length>0 && $('#refcatagoryattenissuediv').is(':visible'))
    {
        
        return true;
    }
    else 
    {
        return false;
    }

}


function getMatchProviders(ele) {

    var AgencyValue = $('#AgencyId_').val();
    $('#refAttendanceIssueDiv').find('#refmathcprovidersattenissuediv').load(HostedDir+"/Roster/GetMatchProvidersPartial", {
        AgencyId: AgencyValue
      , CommunityIds: result
      , parentName: ParentName
      , referralClientId: $referralCategoryPartial.find('#referralClientId').val()
      , clientName: $referralCategoryPartial.find('#clientName').val()
      , id: $referralCategoryPartial.find('#id').val()
      , ScreeningReferralYakkr: $referralCategoryPartial.find('#referenceYakkrId').val()
    }, function (responseTxt, statusTxt, xhr) {
        console.log(responseTxt);

        if (statusTxt == 'success') {
            $('#refmathcprovidersattenissuediv').html(responseTxt);

            $('#referralserviceSectionDiv').hide();
            $('#refcatagoryattenissuediv').hide();
            $('#refmathcprovidersattenissuediv').show();
            $('#refmathcprovidersattenissuediv').find('#Cancel').attr({ 'onclick': 'getReferralService(this)' });

        }
        else {
            customAlert('Error occurred. Please try again later.');
        }


        $('#spinner').hide();


    });
}
