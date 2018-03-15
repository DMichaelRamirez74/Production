$(document).ready(function () {
    if ($('#referralClientId').val().trim() != "") {

        var referralclientId = $('#referralClientId').val();
        $.ajax({

            url: "/Roster/HouseHoldReferrals",
            datatype: "json/application",
            data: { referralClientId: referralclientId },
            success: function (data) {
                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        var parentId = data[i].Value;
                        $('input[type=checkbox][value=' + parentId + ']').prop('checked', true)

                    }
                }
            }
        });
    }
});



$('.check').on('click', function (e) {

    $('.chk_all').prop('checked', true)
});
$('.uncheck').on('click', function (e) {

    $('.chk_all').prop('checked', false)
});



$('#Save').click(function (e) {

    var AgencyValue = $('#AgencyId_').val();
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
    HouseHoldId = $('#HouseHoldId').val();

    var UniqueClientId = $('#UniqueClientId').val();

    if ($('#ClientID:checked').val() == undefined && ($('.chk_all:checked').val() == undefined)) {
        $('.validation1').css("display", "inline-block");
        $('.validation1').text("Select Family Members");
        $('.validation2').css("display", "inline-block");
        $('.validation2').text(" Select Referral Members");
        $('.setfamily').addClass('setcolor');
        $('.setreferral').addClass('setcolor');


    }
    else if ($('#ClientID:checked').val() == undefined) {
        $('.validation1').css("display", "inline-block");
        $('.validation1').text("Select Family Members");
        $('.validation2').hide();
        $('.setfamily').addClass('setcolor');
        $('.setreferral').removeClass('setcolor');

    }
    else if ($('.chk_all:checked').val() == undefined) {
        $('.validation2').css("display", "inline-block");
        $('.validation2').text(" Select Referral Members");
        $('.validation1').hide();
        $('.setfamily').removeClass('setcolor');
        $('.setreferral').addClass('setcolor');

    }

    else {
        $('.validation1').hide();
        $('.validation2').hide();
        $('.setfamily').removeClass('setcolor');
        $('.setreferral').removeClass('setcolor');
        var SaveReferral = {};
        SaveReferral.ServiceId = ServiceId;
        SaveReferral.AgencyId = AgencyValue;
        SaveReferral.CommonClientId = UniqueClientId;
        SaveReferral.HouseHoldId = parseInt(HouseHoldId);
        SaveReferral.ClientId = ClientId;
        SaveReferral.referralClientId = parseInt($('#referralClientId').val());

        $.ajax({
            url: "/Roster/SaveReferralClient",
            type: "POST",
            data: SaveReferral,
            success: function (data) {
                $('#myModal').modal('show');

            }
        });


    }
});


$("#Matchprovider").click(function (e) {

    var AgencyValue = $('#AgencyId_').val();

    var result = "";
    $(".chk_all").map(function () {
        if ($(this).is(':checked'))
            result += this.value + ",";
    });
    result = result.slice(0, -1);


    var CommunityId = $('.chk_all:checked').val();
    $('.parentName').val();
    var ParentName = $('.CheckClient').attr('parentname');
    var householdId = $('#HouseHoldId').val();

    if ($('#ClientID:checked').val() == undefined && ($('.chk_all:checked').val() == undefined)) {
        $('.validation1').css("display", "inline-block");
        $('.validation1').text("Select Family Members");
        $('.validation2').css("display", "inline-block");
        $('.validation2').text(" Select Referral Members");
        $('.setfamily').addClass('setcolor');
        $('.setreferral').addClass('setcolor');
    }
    else if ($('#ClientID:checked').val() == undefined) {
        $('.validation1').css("display", "inline-block");
        $('.validation1').text("Select Family Members");
        $('.validation2').hide();
        $('.setfamily').addClass('setcolor');
        $('.setreferral').removeClass('setcolor');

    }
    else if ($('.chk_all:checked').val() == undefined) {
        $('.validation2').css("display", "inline-block");
        $('.validation2').text(" Select Referral Members");
        $('.validation1').hide();
        $('.setfamily').removeClass('setcolor');
        $('.setreferral').addClass('setcolor');

    }
    else {

        window.location.href = "/Roster/MatchProviders?AgencyId=" + AgencyValue +  "&CommunityIds=" + result + "&parentName=" + ParentName + "&referralClientId=" + $('#referralClientId').val() + "&clientName=" + $('#clientName').val() + "&id=" + $('#id').val();


    }

});
