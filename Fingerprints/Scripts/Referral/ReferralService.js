function NewReferral()
{  
  
    var ID = $('#id').val();
    var ClientName = $('#clientName').val();
    var arr=[];
    var result={id:ID,clientName:ClientName}
    arr.push(result);
    var jsonData = JSON.stringify(arr);
    window.location.href = "/Roster/ReferralCategory?ListReferralCategory=" + jsonData + "";         
           
}

$(document).ready(function ()
{
    if (parseInt($('#refernceCount').val()) > 8) {
        $('.div-scroll').addClass('ht-scroll');
    }
    else {
        $('.div-scroll').removeClass('ht-scroll');
    }

});

$('.ReferralServiceName').click(function () {
    var ReferralClientId = $(this).attr('ReferralClientId');
    var AgencyValue = "";
    var result = "0";
    var ParentName = $(this).attr('parentName');
    var ClientName=$('#clientName').val();
    var ID = $('#id').val();
    var step = 2;

    var Step = $(this).attr('data-stepId');
    

    var ClientId = $('#ClientId').val();

    var screeningRefYakkr = $(this).parent('span').siblings('#screeningReferralYakkr').val().trim();

    if (parseInt(Step) === 2) {      

        window.location.href = "/Roster/ReferralCategory?id=" + $('#id').val() + "&ReferralClientId=" + ReferralClientId + "&Step=" + 2 + "&clientName=" + $('#clientName').val() + "&parentName=" + ParentName + "&ScreeningReferralYakkr=" + screeningRefYakkr;
    }
    else if (parseInt(Step) === 3) {
        //window.location.href = "/Roster/ReferralCategory?id=" + $('#id').val() + "&ReferralClientId=" + ReferralClientId + "&Step=" + 2 + "&clientName=" + $('#clientName').val() + "&parentName=" + ParentName;
        window.location.href = "/Roster/MatchProviders?AgencyId=" + AgencyValue + "&CommunityId=" + result + "&parentName=" + ParentName + "&referralClientId=" + ReferralClientId + "&clientName=" + $('#clientName').val() + "&id=" + $('#id').val() + "&stepId=" + Step + "&ScreeningReferralYakkr=" + screeningRefYakkr;
          
    }
});


$(".DeleteService").click(function () {
    var ReferralClientServiceId = $('#DeleteClientService').val();
    $.ajax({
        url: "/Roster/DeleteReferralService",
        type: "POST",
        data: {
            ReferralClientServiceId: ReferralClientServiceId
        },
        success: function (data) {
            if (data = true) {
                location.reload();
            }
        }
    });

})