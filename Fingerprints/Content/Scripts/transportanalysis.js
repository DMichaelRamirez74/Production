var centerID = '';
var centerName = '';
var isZero = '0';

$(function () {
    changeSizeOfWindow();
})
function getCenter_Name_ID(ele)
{
    centerID = $(ele).closest('tr').find('td').eq(0).attr('enc_id');
    centerName = $(ele).closest('tr').find('td').eq(0).children('p').html();
}
//$('#no-more-tables-transporation').find('.transport_table_body').on('click', '.riders_count', function () {
//    getCenter_Name_ID(this);
   
//    isZero = $(this).attr('rid_count');
//    //if (isZero == '0')
//    //{
        
//    //    return false;
//    //}
//    getRidersChildren();
   
//});

function ridersCountClick(rider)
{
    getCenter_Name_ID(rider);

    isZero = $(rider).attr('rid_count');
    //if (isZero == '0')
    //{

    //    return false;
    //}
    getRidersChildren();
}

//$('#no-more-tables-transporation').find('.transport_table_body').on('click', '.pickup_count', function () {

//    var centerID = $(this).closest('tr').find('td').eq(0).attr('enc_id')

//    getCenter_Name_ID(this);
//    getAssignedChildrenForPickup(centerID,1);

//});

function pickupCountClick(pickup)
{
    var centerID = $(pickup).closest('tr').find('td').eq(0).attr('enc_id')

    getCenter_Name_ID(pickup);
    getAssignedChildrenForPickup(centerID, 1);
}

//$('#no-more-tables-transporation').find('.transport_table_body').on('click', '.dropoff_count', function () {

//    var centerID = $(this).closest('tr').find('td').eq(0).attr('enc_id')
   
//    getCenter_Name_ID(this);
//    getAssignedChildrenForPickup(centerID, 2);





//});

function dropOffClick(dropOff)
{
    var centerID = $(dropOff).closest('tr').find('td').eq(0).attr('enc_id')

    getCenter_Name_ID(dropOff);
    getAssignedChildrenForPickup(centerID, 2);

}

//$('#no-more-tables-transporation').find('.transport_table_body').on('click', '.dropoff_count', function () {
//    var centerID = $(this).closest('tr').find('td').eq(0).attr('enc_id')
//    getCenter_Name_ID(this);
//});

$('.back_button').on('click', function () {
 
    $('#no-more-tables-transporation').show();
    $('#pickUp_transportationDiv,#riders_transportationDiv,#dropoff_transportationDiv').hide();

});
function getRidersChildren()
{
  
    $.ajax({
        url: '/Transportation/GetRiderChildren',
        datatype: 'json',
        type: 'post',
        data: { centerId: centerID },
        success:function(data)
        {
            var arr=["0","Home","Mobile","Work"];
            var bindTemp = '';
           
            if(data.length>0)
            {
                $.each(data, function (i, riderChild) {
                    bindTemp += '<tr>' +
                                '<td style="line-height: 25px;" data-title="Children Name"><p>' + riderChild.ClientName + '</p></td>';
                    bindTemp += '<td data-title="Address" style="text-align:left;"><p style="font-size:14px;">' + riderChild.Address + '</p></td>';
                    var phone = '<p>';
                 
                    $.each(riderChild.PhoneList,function(j,phoneNo){
                        phone += '<dl><dt>' + arr[phoneNo.Text] + '</dt><dd>' + phoneNo.Value + '</dd></dl>';
                    });
                    phone += '</p>';
                    bindTemp += '<td data-title="Phone No" >' + phone + '</td>' +
                                '<td data-title="Pick Up"><p  style="font-size:14px;">' + riderChild.PickUpAddress + '</p></td>' +
                                '<td data-title="Drop Off"><p style="font-size:14px;"> ' + riderChild.DropAddress + '</p></td>' +
                                '<td data-title="Disability"><p>' + riderChild.Disability + '</p></td>' +
                                '<td data-title="Allergy"><p>' + riderChild.Allergy + '</p></td>' +
                                //'<td data-title="Comments"><p>' + riderChild.Allergy + '</p></td>' +
                            '</tr>';
                });
               
            }
            else {
                bindTemp = '<tr style="line-height:51px;font-size:15px;"><td colspan="7" style="margin-top:20px;text-align:center;margin-top:20px;">No Results Found</td></tr>';
            }
            $('#riders_transportationDiv').find('.riders_table_body').html(bindTemp);
            $('#riders_transportationDiv').find('#rider_centerName').html(centerName+' Rider');
       
            $('#riders_transportationDiv').show();
            $('#pickUp_transportationDiv,#no-more-tables-transporation,#dropoff_transportationDiv').hide();

        },
        error:function(data)
        {

        }
    })
}

function getAssignedChildrenForPickup(centerID,route)
{
    $.ajax({
        url: '/Transportation/GetRouteAssignedByCenter',
        type: 'post',
        datatype: 'post',
        data: { centerId: centerID, routeType: route },
        success:function(data)
        {
            var bindDiv='';
            if (data.length > 0)
            {
                $.each(data, function (i, route) {

                    bindDiv += '<tr>' +
                                                 '<td data-title="Route Name">' +
                                                     '<p>'+route.RouteName+'</p>' +
                                                 '</td>' +
                                                ' <td data-title="Bus Driver">' +
                                                     '<p>'+route.BusDriverName+'</p>' +
                                                ' </td>' +
                                                 '<td data-title="Bus Monitor">' +
                                                    ' <p>'+route.BusMonitorName+'</p>' +
                                                 '</td>' +
                                                 '<td data-title="No Of Stops">' +
                                                     '<p>'+route.NoOfStops+'</p>' +
                                                 '</td>' +
                                            ' </tr>';
                });
            }
            if (route == 1)
            {
                $('.pickupTableBody').html(bindDiv);
                
                if (data.length == '0') {
                    customAlert('Children are not assigned to the route for ' + centerName);
                    return false;
                }
                $('#pickUp_transportationDiv').find('#pickup_centerName').html(centerName + ' Pickup');
                $('#pickUp_transportationDiv').show();
                $('#riders_transportationDiv,#no-more-tables-transporation,#dropoff_transportationDiv').hide();

                var bodyHeight = 0;
                $('.pickupTableBody ').find('tr').each(function () {
                    bodyHeight += $(this).height();
                });
                var bindBodyHeight = $('.pickupTableBody').height();
                if (bodyHeight <= bindBodyHeight) {
                    $('.pickupTableBody').css({
                        'height': bodyHeight

                    });

                    $('#pickUp_transportationDiv').find(".scroll-thead").css({ "width": "100%" });
                }
                else {
                    $('#pickUp_transportationDiv').find(".scroll-thead").css({ "width": "calc( 100% - 17px )" });
                }

            }
            else
            {
                if (data.length == '0') {
                    customAlert('Children are not assigned to the route for '+centerName);
                    return false;
                }
                $('#dropoff_transportationDiv').find('#dropoff_centerName').html(centerName + ' Drop-off');
                $('#dropoff_transportationDiv').show();
                $('#riders_transportationDiv,#no-more-tables-transporation,#pickUp_transportationDiv').hide();
                $('.dropoffTableBody').html(bindDiv);
                var bodyHeight1 = 0;
                $('.dropoffTableBody ').find('tr').each(function () {
                    bodyHeight1 += $(this).height();
                });
                var bindBodyHeight1 = $('.dropoffTableBody').height();
              
                if (bodyHeight1 <= bindBodyHeight1) {
                    $('.dropoffTableBody').css({
                        'height': bodyHeight1

                    });

                    $('#dropoff_transportationDiv').find(".scroll-thead").css({ "width": "100%" });
                }
                else {
                    $('#dropoff_transportationDiv').find(".scroll-thead").css({ "width": "calc( 100% - 17px )" });
                }
            }
   
          
        }

    })
}

function changeSizeOfWindow()
{
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
            var fileHeight = height - 25;
        
            $('.right-side-container-ch').css('min-height', (fileHeight - 5) + 'px');
        }

    });
}



