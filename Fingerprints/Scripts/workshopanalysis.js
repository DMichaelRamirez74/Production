$(document).ready(function () {
    function createEvent(val) {

        location.href = "/Events/CreateEvent";
    }
    GetWorkShopAnalysis(0);

    $(window).bind("load resize", function () {

        topOffset = 50;
        width = (this.window.innerWidth > 0) ? this.window.innerWidth : this.screen.width;
        if (width < 768) {
            topOffset = 100; // 2-row-menu
        }

        height = ((this.window.innerHeight > 0) ? this.window.innerHeight : this.screen.height) - 1;

        height = height - topOffset;
        if (height < 1) height = 1;
        if (height > topOffset) {
            var fileHeight = height - 30;

            $('.right-side-container-ch').css('min-height', (fileHeight) + 'px');
            $('.append_WorkshopDiv').height(fileHeight - 317);
        }

    });

});

function reSizeContent() {

    $('.append_WorkshopDiv').height($('.right-side-container-ch').height() - 317);
}


$('#searchByCenter').click(function () {
    var center_Id = parseInt($('.select_center').val());
    GetWorkShopAnalysis(center_Id);
});
function GetWorkShopAnalysis(centerId) {
    $.ajax({
        url: '/ERSEA/GetWorkShopAnalysis',
        datatype: 'json',
        type: 'post',
        data: { centerId: centerId },
        success: function (data) {

            var selectOption = ' <option value="0">All</option>';
            var appendDiv = '';
            var bindDiv = $('.append_Workop');
            if (centerId == 0) {
                if (data.centerList.length > 0) {
                    if (data.centerList.length == 1) {
                        $.each(data.centerList, function (i, centerDropDown) {
                            $('#singleCenterBlock').find('#centernameSpan').html(centerDropDown.Text).attr('enc-id', centerDropDown.Value);
                        });

                        $('#singleCenterBlock').show();
                        $('#multipleCenter').hide();
                    }
                    else {

                        $.each(data.centerList, function (i, centerDropDown) {
                            selectOption += '<option value=' + centerDropDown.Value + '>' + centerDropDown.Text + '</option>';
                        });
                        $('#singleCenterBlock').hide();
                        $('#multipleCenter').show();
                    }
                }
                $('.select_center').html(selectOption);
            }

            if (data.workshopList.length > 0) {
                if (data.isLink) {
                    $.each(data.workshopList, function (j, worshopElement) {
                      
                        bindDiv.find('.workshop-name').html(worshopElement.WorkShopName).attr({ 'workshop-id': worshopElement.WorkShopId, 'onclick': 'location.href="/Events/CreateEvent?Id=' + worshopElement.WorkShopId + '&Name=' + worshopElement.WorkShopName + '&cId='+centerId+'"' }).css({ 'text-decoration': 'underline', 'cursor': 'pointer' });
                        bindDiv.find('.house-enrolled').html(worshopElement.HouseholdEnrolled);
                        appendDiv += bindDiv.html();
                    });
                }
                else {
                    $.each(data.workshopList, function (j, worshopElement) {

                        bindDiv.find('.workshop-name').html(worshopElement.WorkShopName);
                        bindDiv.find('.house-enrolled').html(worshopElement.HouseholdEnrolled);
                        appendDiv += bindDiv.html();
                    });
                }
            }
            else {
                appendDiv = '<div class="center" style="text-align: center;font-size: 20px;margin: auto;padding: 10px;">No data found</div>';
            }

            $('.append_WorkshopDiv').html(appendDiv);
        }
    });
}


