var mapData = JSON.parse($('#inputHidden').val());
var pickupdetails = JSON.parse($('#pickupdetails').val());
var dropdetails = JSON.parse($('#dropdetails').val());
var routenames = $('#routenames').val();
var arr = new Array();
var mapMarkers = [];
var polygons = [];
var map, bounds, infowindow;
var i;
var marker;
var lat = 0;
var long = 0;
function LoadMap() {


    map = new google.maps.Map(document.getElementById('googleMap'), {
        zoom: 10,
        center: new google.maps.LatLng(mapData[0].Latitude, mapData[0].Longitude),
        mapTypeId: google.maps.MapTypeId.ROADMAP
    });
    bounds = new google.maps.LatLngBounds();
    infowindow = new google.maps.InfoWindow();

    PinMapWithRoute();
    map.setCenter(new google.maps.LatLng(lat, long));
}
window.onload = function () {
    LoadMap();
};

var k = 0;
function PinMapWithRoute() {
    k++;
    bounds = new google.maps.LatLngBounds();
    infowindow = new google.maps.InfoWindow();
    polygons = [];
            $('.route-name option').each(function () {
                arr = [];
                var valuetext = $(this).val();

                $(pickupdetails).each(function (i, val) {
                    if (valuetext == val.PickUpRouteName) {

                        arr.push(new google.maps.LatLng(
                                                 parseFloat(val.Latitude),
                                                 parseFloat(val.Longitude)
                                               ));
                        bounds.extend(arr[arr.length - 1]);
                    }
                });
                $(dropdetails).each(function (i, value) {
                    if (valuetext == value.DropRouteName) {

                        arr.push(new google.maps.LatLng(
                                                 parseFloat(value.Latitude),
                                                 parseFloat(value.Longitude)
                                               ));
                        bounds.extend(arr[arr.length - 1]);
                    }
                });

                if (arr.length > 0) {
                    polygons.push(new google.maps.Polygon({
                        paths: arr,
                        strokeColor: '#FF0000',
                        strokeOpacity: 0.8,
                        strokeWeight: 2,
                        fillColor: '#FF0000',
                        fillOpacity: 0.35
                    }));

                    polygons[polygons.length - 1].setMap(map);
                }

            });
            $.each(mapData, function (k, data2) {
               
                if (data2.Latitude != 0) {

                    lat = (lat == 0) ? data2.Latitude : lat;
                    long = (long == 0) ? data2.Longitude : long;

                    marker = new google.maps.Marker({
                        position: new google.maps.LatLng(data2.Latitude, data2.Longitude),
                        title: data2.ChildrenName,
                        map: map,
                        icon: data2.PinSmall,
                        lat: data2.Latitude,
                        long: data2.Longitude
                    });
                    mapMarkers.push(marker);
                   
                }
               
            });

}

function setMapOnAll(map) {
    for (var i = 0; i < mapMarkers.length; i++) {
        mapMarkers[i].setMap(map);
    }

    for (var i = 0; i < polygons.length; i++) {
        polygons[i].setMap(map);
    }

}
$('#addRouteBtn').on('click', function () {

    cleanValidation();
    if ($('#routeNameTxt').val() == '') {

        customAlert('Please Enter Route Name');
        plainValidation('#routeNameTxt');
        return false;
    }
    if ($('#busNameTxt').val() == '') {

        customAlert('Please Enter Bus Name');
        plainValidation('#busNameTxt');
        return false;
    }
    if ($('#busDriverSelect').val() == '0') {
        customAlert('Please Select Bus Driver');
        plainValidation('#busDriverSelect');
        return false;
    }
    if ($('#busMonitorSelect').val() == '0') {
        customAlert('Please Select Bus Monitor');
        plainValidation('#busMonitorSelect');
        return false;
    }
    if (!$('input[name=routeradio]').is(':checked')) {
        customAlert('Please select Route');
        plainValidation('routeradio');
    }
    
    var enc_CenterId = $('#center-select').val();
    var routeModel = {};
    if ($(this).attr('route-id') != undefined)
        routeModel.Enc_RouteId = $(this).attr('route-id');
    routeModel.BusDriverId = $('#busDriverSelect').val();
    routeModel.BusMonitorId = $('#busMonitorSelect').val();
    routeModel.BusName = $('#busNameTxt').val().trim();
    routeModel.RouteName = $('#routeNameTxt').val().trim();
    routeModel.RouteType = parseInt($('input[name=routeradio]:checked').val());
    routeModel.RouteAddress = $('#centerAddressTxt').val().trim();
    routeModel.Enc_CenterId = enc_CenterId;
    
    $.ajax({
        url: '/Transportation/AddRoute',
        datatype: 'json',
        type: 'post',
        data: { routeDetailsString: JSON.stringify(routeModel) },
        success: function (data) {
            if (data) {
                customAlert('Route saved successfully');
                cleanValidation();
            }
            $('#routeNameTxt,#busNameTxt').val('');
            $('#busDriverSelect,#busMonitorSelect').val(0);
            $('#pickupRadio').prop('checked', true);
            $('#addRouteBtn').val('Add Route').attr({ 'route-id': 0, 'mode': 0 });
           
            getAssignedRouteDetailsByCenter(enc_CenterId);
        },
        error: function (data) {

        }
    })
});


$('body').on('click', '.show-child-details', function () {
    $.ajax({
        dataType: 'json',
        type: "POST",
        async: false,
        url: "/Transportation/GetChildDetails",
        data: { 'ClientId': $(this).attr("childid") },
        success: function (data) {

            var result = JSON.parse(data);
            if (result.Table.length > 0) {
                $('.parent-name').text(result.Table[0].ParentName);
                $('.child-first').text(result.Table[0].FirstName);
                $('.child-last').text(result.Table[0].LastName);
                $('.gender').text(result.Table[0].Gender);
                $('.dob').text(result.Table[0].DOB);
                $('.phone').text(result.Table[0].Phone);
                $('.address-child').text(result.Table[0].Address);

                var disbility = "";
                if (result.Table[0].IEP) {
                    disbility = "IEP";
                }
                else if (result.Table[0].IFSP) {
                    disbility = "IFSP";
                }
                else {
                    disbility = "None";
                }
                $('.disability').text(disbility);
            }

            $('#transportation').modal('show');
        },
        error: function (data) {
        }
    });
    $('#child_popup').modal('show');
});

$('body').on('click', '.btn-remove-assigned-route', function () {



    //return false;
    //return false;
    $.ajax({
        dataType: 'json',
        type: "POST",
        async: false,
        url: "/Transportation/DeleteAssignedRoute",
        data: { 'ClientId': $(this).attr('clientid'), 'RouteId': $(this).attr('routeid') },
        success: function (data) {

            if (data) {




                // getAssignedRouteDetails();
                customAlertSuccess("Deletion Successfully Completed");
            }
        },
        error: function (data) {
        }
    });
    var arr = [];
    var routeid = "";
    var index = $(this).closest('tr').index();
    var k = 0;
    $(this).closest('table').find('tr').each(function (i, val) {
        if (index != i) {
            $(this).find('#stopPos').text("Stop " + (k + 1) + "");
            arr.push($(this).attr('childid'));
            routeid = $(this).attr('routeid');
            k++;
        }

    });
    parameters = JSON.stringify({ 'listChild': arr, 'RouteId': routeid });
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: "POST",
        async: false,
        url: "/Transportation/UpdatePosition",
        data: parameters,
        success: function (data) {


        },
        error: function (data) {
        }
    });
    $(this).closest('tr').remove();
});

$('#pickupRadio').on('change', function () {
    var check = $(this).is(":checked");
    if (check) {
        $('#dropUpDiv').hide();
        $('#pickUpDiv').show();

    }
});

$('#dropoffRadio').on('change', function () {
    var check = $(this).is(":checked");
    if (check) {

        $('#pickUpDiv').hide();
        $('#dropUpDiv').show();

    }
});
var markers = [
{
    "title": 'Aksa Beach',
    "lat": '19.1759668',
    "lng": '72.79504659999998',
    "description": 'Aksa Beach is a popular beach and a vacation spot in Aksa village at Malad, Mumbai.'
},
{
    "title": 'Juhu Beach',
    "lat": '19.0883595',
    "lng": '72.82652380000002',
    "description": 'Juhu Beach is one of favourite tourist attractions situated in Mumbai.'
},
{
    "title": 'Girgaum Beach',
    "lat": '18.9542149',
    "lng": '72.81203529999993',
    "description": 'Girgaum Beach commonly known as just Chaupati is one of the most famous public beaches in Mumbai.'
},
{
    "title": 'Jijamata Udyan',
    "lat": '18.979006',
    "lng": '72.83388300000001',
    "description": 'Jijamata Udyan is situated near Byculla station is famous as Mumbai (Bombay) Zoo.'
},
{
    "title": 'Sanjay Gandhi National Park',
    "lat": '19.2147067',
    "lng": '72.91062020000004',
    "description": 'Sanjay Gandhi National Park is a large protected area in the northern part of Mumbai city.'
}
];
function SetMarker(position) {

    //Remove previous Marker.
    if (marker != null) {
        marker.setMap(null);
    }

    //Set Marker on Map.
    //  var data = markers[1];
    var myLatlng = new google.maps.LatLng(0, 0);
    marker = new google.maps.Marker({
        position: myLatlng,
        map: map
        //title: data.title
    });

    //Create and open InfoWindow.
    infoWindow = new google.maps.InfoWindow();
    //infoWindow.setContent("<div style = 'width:200px;min-height:40px'>" +data.description + "</div>");
    infoWindow.open(map, marker);
};
$('#searchCenter').on('click', function () {
    //SetMarker(0);
    var EnC_id = $('#center-select').val();
    if (EnC_id == '0') {
        $('#addRouteSection').hide();
        bindAddRouteSection(EnC_id);
        $('#routesection').html('');
        return false;
    }
    else {
        bindAddRouteSection(EnC_id);
        getAssignedRouteDetailsByCenter(EnC_id);
    }
});

$(document).ready(function () {
    // $('#pickLocationDiv,#droplocationDiv').hide();
    $('#pickUpDiv').show();
    // LoadMap();
    // getAssignedRouteDetails();
});


function getAssignedRouteDetailsByCenter(centerId) {
    $.ajax({

        url: '/Transportation/GetCreatedRouteByCenter',
        type: 'post',
        datatype: 'json',
        async: false,
        data: { enc_CenterId: centerId },
        success: function (data) {
            var sectionDiv = '';
            if (data.length > 0) {

                $.each(data, function (i, route) {
                    var routeType = (route.RouteType == 1) ? 'Pickup Route' : 'Drop-off Route';
                    var appendDiv = '';
                    appendDiv += '<div class="col-xs-12 ecu_topsapc">' +
                                '<div class="icr_accr">' +
                                    '<div class="accordionButton" onclick="showHideAccordian(this);" onmouseover="mouseHoverAccordian(this);" onmouseout="mouseOutAccordian(this);" indexpos=' + i + '>' +
                                        '<span id="routeName">' + route.RouteName + '</span><span class="plusMinus">+</span>' +
                                    '</div>' +
                                    '<div class="accordionContent" style="display: none; overflow: hidden;">' +
                                        '<div class="btn_btn">' +
                                            '<button class="btn btn-success" pos=' + i + ' routeId=' + route.Enc_RouteId + ' routetype=' + route.RouteType + ' onclick="editCreatedRoute(this);"><i class="fa fa-pencil" aria-hidden="true"></i>&nbsp;Edit</button>' +
                                        '</div>' +
                                        '<div class="icr_table table-responsive">' +
                                            '<table class="table table-bordered">' +
                                                '<tbody>' +
                                                    '<tr>' +
                                                        '<td style="width:160px;">Route Name</td>' +
                                                        '<td id="routName_' + i + '">' + route.RouteName + '</td>' +
                                                    '</tr>' +
                                                    '<tr>' +
                                                        '<td style="width:;">Bus Name</td>' +
                                                        '<td id="BusName_' + i + '">' + route.BusName + '</td>' +
                                                    '</tr>' +
                                                    '<tr>' +
                                                        '<td style="width:px;">Bus Driver</td>' +
                                                        '<td id="busDriverName_' + i + '" driverId=' + route.Enc_BusDriverId + '>' + route.BusDriverName + '</td>' +
                                                    '</tr>' +
                                                    '<tr>' +
                                                       ' <td style="width:200px;">Bus Monitor</td>' +
                                                        '<td id="busMonitorName_' + i + '" monitorId=' + route.Enc_BusMonitorId + '>' + route.BusMonitorName + '</td>' +
                                                    '</tr>' +
                                                    '<tr>' +
                                                        '<td style="width:200px;">' + routeType + '</td>' +
                                                        '<td id="RouteAddress_' + i + '">' + route.RouteAddress + '</td>' +
                                                    '</tr>' +
                                                '</tbody>' +
                                            '</table>' +
                                        '</div>' +
                                    '</div>' +
                                    '<div class="header_icc">' +
                                        '<div class="col-lg-3 col-xs-3">Child Name</div>' +
                                        '<div class="col-lg-2 col-xs-3">Intervals</div>' +
                                        '<div class="col-lg-7 col-xs-6" id="routeType">' + routeType + '</div>' +
                                    '</div>' +
                                    '<div class="lower-icr childDiv"><table class="tbl-assigned-route"><tbody>';
                    if (route.ChildrenList.length > 0) {
                        var childDiv = '';
                        $.each(route.ChildrenList, function (j, child) {
                            childDiv += '<tr childid="' + child.Enc_ClientId + '" routeid=' + route.Enc_RouteId + ' style="margin-bottom: 10px;">' +
                                          '<td  style="padding-left: 0;width:221px;">' +
                                             '<div class="route-content-desc show-child-details" style="position: relative;cursor:pointer;" childid="' + child.Enc_ClientId + '" >' +
                                                 '<p id="ChildrenName">' + child.ChildrenName + '</p>' +
                                              '</div>' +
                                           '</td>' +
                                                    '<td  style="padding-left: 0;width:175px;">' +
                                                    '<div class="route-content-desc" style="position: relative;">' +
                                                    '<p id="stopPos">Stop ' + child.StopPosition + '</p>' +
                                                    '</div>' +
                                                    '</td>' +
                                                    '<td  style="padding-left: 0;width:500px;">' +
                                                    '<div class="route-content-desc" style="position: relative;">' +
                                                    '<p id="ChildAddress">' + child.RouteAddress + '</p>' +
                                                    '</div>' +
                                                    '</td>' +
                                                    '<td  style="padding-left: 0;">' +
                                                    '<div class="route-content-desc" style="position: relative;">' +
                                                    '<button style="float:right;" class="btn btn-danger btn-remove-assigned-route" routeid=' + route.Enc_RouteId + ' clientId=' + child.Enc_ClientId + '><i class="fa fa-minus-circle" aria-hidden="true"></i>&nbsp;Remove</button>' +
                                                    '</div>' +
                                                    '</td>' +
                                                    '</tr>';


                        });


                    }
                    else {
                        childDiv = '<div style="text-align:center;font-size:16px;padding-bottom:10px;">No children assigned to this route</div>';
                    }
                    childDiv += "</tbody></table>";
                    appendDiv += childDiv;
                    appendDiv += '</div>';
                    appendDiv += '</div></div>';
                    sectionDiv += appendDiv;
                });
            }
            $('.routesection').html(sectionDiv);

        },
        error: function (data) {

        }

    })
    setTimeout(function () {
        $('.tbl-assigned-route').each(function () {
            $(this).find('tr').each(function (i, val) {
                $(this).find('#stopPos').text("Stop " + (i + 1) + "");
            });
        });

        $('.tbl-assigned-route tbody').sortable();
        $('.tbl-assigned-route tbody').on('sortupdate', function () {
            var arr = [];
            var routeid = "";
            $(this).closest('table').find('tr').each(function (i, val) {
                $(this).find('#stopPos').text("Stop " + (i + 1) + "");
                arr.push($(this).attr('childid'));
                routeid = $(this).attr('routeid');
            });
            parameters = JSON.stringify({ 'listChild': arr, 'RouteId': routeid });
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "POST",
                async: false,
                url: "/Transportation/UpdatePosition",
                data: parameters,
                success: function (data) {


                },
                error: function (data) {
                }
            });
        });
        // $('.tbl-assigned-route tbody').trigger('sortupdate');
    }, 200);

}

function getAssignedRouteDetails() {
    $.ajax({

        url: '/Transportation/GetCreatedRoute',
        type: 'post',
        datatype: 'json',
        success: function (data) {
            if (data.length > 0) {
                var sectionDiv = '';
                $.each(data, function (i, route) {
                    var routeType = (route.RouteType == 1) ? 'Pickup Route' : 'Drop-off Route';
                    var appendDiv = '';
                    appendDiv += '<div class="col-xs-12 ecu_topsapc">' +
                                '<div class="icr_accr">' +
                                    '<div class="accordionButton" onclick="showHideAccordian(this);" onmouseover="mouseHoverAccordian(this);" onmouseout="mouseOutAccordian(this);" indexpos=' + i + '>' +
                                        '<span id="routeName">' + route.RouteName + '</span><span class="plusMinus">+</span>' +
                                    '</div>' +
                                    '<div class="accordionContent" style="display: none; overflow: hidden;">' +
                                        '<div class="btn_btn">' +
                                            '<button class="btn btn-success" pos=' + i + ' routeId=' + route.Enc_RouteId + ' routetype=' + route.RouteType + ' onclick="editCreatedRoute(this);"><i class="fa fa-pencil" aria-hidden="true"></i>&nbsp;Edit</button>' +
                                        '</div>' +
                                        '<div class="icr_table table-responsive">' +
                                            '<table class="table table-bordered">' +
                                                '<tbody>' +
                                                    '<tr>' +
                                                        '<td style="width:160px;">Route Name</td>' +
                                                        '<td id="routName_' + i + '">' + route.RouteName + '</td>' +
                                                    '</tr>' +
                                                    '<tr>' +
                                                        '<td style="width:;">Bus Name</td>' +
                                                        '<td id="BusName_' + i + '">' + route.BusName + '</td>' +
                                                    '</tr>' +
                                                    '<tr>' +
                                                        '<td style="width:px;">Bus Driver</td>' +
                                                        '<td id="busDriverName_' + i + '" driverId=' + route.Enc_BusDriverId + '>' + route.BusDriverName + '</td>' +
                                                    '</tr>' +
                                                    '<tr>' +
                                                       ' <td style="width:200px;">Bus Monitor</td>' +
                                                        '<td id="busMonitorName_' + i + '" monitorId=' + route.Enc_BusMonitorId + '>' + route.BusMonitorName + '</td>' +
                                                    '</tr>' +
                                                    '<tr>' +
                                                        '<td style="width:200px;">' + routeType + '</td>' +
                                                        '<td id="RouteAddress_' + i + '">' + route.RouteAddress + '</td>' +
                                                    '</tr>' +
                                                '</tbody>' +
                                            '</table>' +
                                        '</div>' +
                                    '</div>' +
                                    '<div class="header_icc">' +
                                        '<div class="col-lg-3 col-xs-3">Child Name</div>' +
                                        '<div class="col-lg-2 col-xs-3">Intervals</div>' +
                                        '<div class="col-lg-7 col-xs-6" id="routeType">' + routeType + '</div>' +
                                    '</div>' +
                                    '<div class="lower-icr childDiv"><table class="tbl-assigned-route"><tbody>';
                    if (route.ChildrenList.length > 0) {
                        var childDiv = '';
                        $.each(route.ChildrenList, function (j, child) {
                            childDiv += '<tr childid="' + child.Enc_ClientId + '" routeid=' + route.Enc_RouteId + ' style="margin-bottom: 10px;">' +
                                          '<td  style="padding-left: 0;width:221px;">' +
                                             '<div class="route-content-desc show-child-details" style="position: relative;cursor:pointer;" childid="' + child.Enc_ClientId + '" >' +
                                                 '<p id="ChildrenName">' + child.ChildrenName + '</p>' +
                                              '</div>' +
                                           '</td>' +
                                                    '<td  style="padding-left: 0;width:175px;">' +
                                                    '<div class="route-content-desc" style="position: relative;">' +
                                                    '<p id="stopPos">Stop ' + child.StopPosition + '</p>' +
                                                    '</div>' +
                                                    '</td>' +
                                                    '<td  style="padding-left: 0;width:500px;">' +
                                                    '<div class="route-content-desc" style="position: relative;">' +
                                                    '<p id="ChildAddress">' + child.RouteAddress + '</p>' +
                                                    '</div>' +
                                                    '</td>' +
                                                    '<td  style="padding-left: 0;">' +
                                                    '<div class="route-content-desc" style="position: relative;">' +
                                                    '<button style="float:right;" class="btn btn-danger btn-remove-assigned-route" routeid=' + route.Enc_RouteId + ' clientId=' + child.Enc_ClientId + '><i class="fa fa-minus-circle" aria-hidden="true"></i>&nbsp;Remove</button>' +
                                                    '</div>' +
                                                    '</td>' +
                                                    '</tr>';


                        });


                    }
                    else {
                        childDiv = '<div style="text-align:center;font-size:16px;padding-bottom:10px;">No children assigned to this route</div>';
                    }
                    childDiv += "</tbody></table>";
                    appendDiv += childDiv;
                    appendDiv += '</div>';
                    appendDiv += '</div></div>';
                    sectionDiv += appendDiv;
                });

                $('.routesection').html(sectionDiv);
            }
        },
        error: function (data) {

        }
    })
    setTimeout(function () {
        $('.tbl-assigned-route').each(function () {
            $(this).find('tr').each(function (i, val) {
                $(this).find('#stopPos').text("Stop " + (i + 1) + "");
            });
        });

        $('.tbl-assigned-route tbody').sortable();
        $('.tbl-assigned-route tbody').on('sortupdate', function () {
            var arr = [];
            var routeid = "";
            $(this).closest('table').find('tr').each(function (i, val) {
                $(this).find('#stopPos').text("Stop " + (i + 1) + "");
                arr.push($(this).attr('childid'));
                routeid = $(this).attr('routeid');
            });
            parameters = JSON.stringify({ 'listChild': arr, 'RouteId': routeid });
            $.ajax({
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: "POST",
                async: false,
                url: "/Transportation/UpdatePosition",
                data: parameters,
                success: function (data) {


                },
                error: function (data) {
                }
            });
        });
        // $('.tbl-assigned-route tbody').trigger('sortupdate');
    }, 200);


}


function bindAddRouteSection(centerId) {
    $.ajax({
        url: '/Transportation/GetBusStaffByCenter',
        type: 'post',
        datatype: 'json',
        data: { enc_CenterId: centerId },
        success: function (data) {
            var busDriver = '<option value="0">--Select Bus Driver--</option>';
            var busMonitor = '<option value="0">--Select Bus Monitor--</option>';
            var centerAddress = '';
            if (centerId != "0") {

                if (data.staff.BusDriverList.length > 0) {
                    $.each(data.staff.BusDriverList, function (i, driver) {
                        busDriver += '<option value=' + driver.Enc_BusDriverId + '>' + driver.BusDriverName + '</option>';
                    });
                }
                if (data.staff.BusMonitorList.length > 0) {
                    $.each(data.staff.BusMonitorList, function (j, monitor) {
                        busMonitor += '<option value=' + monitor.Enc_BusMonitorId + '>' + monitor.BusMonitorName + '</option>';
                    });
                }
                centerAddress = data.staff.Center.CenterAddress;
                $('#busDriverSelect').html(busDriver);
                $('#busMonitorSelect').html(busMonitor);
                $('.center-addr-route').html(centerAddress);
                $('#addRouteSection').show();
            }
            mapData1 = JSON.parse(data.TestString);
            pickupdetails1 = JSON.parse(data.pickUpRouteNames);
            dropdetails1 = JSON.parse(data.dropRouteName);
            routenames1 = data.pickUpList;
         
           
           loadMapByCenter(mapData1, pickupdetails1, dropdetails1, routenames1);
           // PinMapWithRoute();

        },
        error: function (data) {

        }
    })
}

function loadMapByCenter(mapData2, pickupdetails2, dropdetails2, routenames2) {

    setMapOnAll(null);
    var lat = 0;
    var long = 0;
   
    bounds = new google.maps.LatLngBounds();


    infowindow = new google.maps.InfoWindow();



   


    $('.route-name option').each(function () {
        arr = [];
        var valuetext = $(this).val();

        $(pickupdetails2).each(function (i, val) {
            if (valuetext == val.PickUpRouteName) {
                arr.push(new google.maps.LatLng(
                                         parseFloat(val.Latitude),
                                         parseFloat(val.Longitude)
                                       ));
                bounds.extend(arr[arr.length - 1]);
            }
        });
        $(dropdetails2).each(function (i, value) {
            if (valuetext == value.DropRouteName) {
                arr.push(new google.maps.LatLng(
                                         parseFloat(value.Latitude),
                                         parseFloat(value.Longitude)
                                       ));
                bounds.extend(arr[arr.length - 1]);
            }
        });

        if (arr.length > 0) {

            polygons.push(new google.maps.Polygon({
                paths: arr,
                strokeColor: '#FF0000',
                strokeOpacity: 0.8,
                strokeWeight: 2,
                fillColor: '#FF0000',
                fillOpacity: 0.35
            }));

            polygons[polygons.length - 1].setMap(map);

        }

    });
    //map.fitBounds(bounds);
    $.each(mapData2, function (k, data2) {
        if (data2.Latitude != 0) {

            lat = (lat == 0) ? data2.Latitude : lat;
            long = (long == 0) ? data2.Longitude : long;

            marker = new google.maps.Marker({
                position: new google.maps.LatLng(data2.Latitude, data2.Longitude),
                title: data2.ChildrenName,
                map: map,
                icon: data2.PinSmall,
                lat: data2.Latitude,
                long: data2.Longitude
            });
            mapMarkers.push(marker);

            google.maps.event.addListener(marker, 'click', (function (marker, i) {
                return function () {

                    var Template = '<div class="map-notify1">\
                                    <div class="col-xs-12 no-padding" style="position: relative;">\
                                        <div class="map-arrow-left"></div>\
                                        <div class="col-xs-12 no-padding">\
                                            <div class="map-notify-title assign_route">\
                                                <h2>##clientName##</h2>\
                                               <input type="hidden" class="client-id" value="##CLIENTID##"/>\
                                              <input type="hidden" class="route-type" value="##ROUTETYPE##"/>\
                                            </div>\
                                        </div>\
                                        <div class="col-xs-12 no-padding">\
                                            <div class="map-notify-cont">\
                                                <p>Address</p>\
                                            </div>\
                                            <div class="map-notify-cont1">\
                                                <p>##clientAddress##,##clientCity##,##clientState## ##clientZipCode##</p>\
                                            </div>\
                                        </div>\
                                        <div class="col-xs-12 no-padding">\
                                            <div class="map-notify-cont">\
                                                <p>##RouteHeader##</p>\
                                            </div>\
                                            <div class="map-notify-cont1">\
                                                <p>##AssignedRoute##</p>\
                                            </div>\
                                        </div>\
                                    </div>\
                                </div>';
                    if (data2.PickUpStatus == 1 || data2.PickUpStatus == 0) {
                        Template = Template.replace("##RouteHeader##", "Pickup Route");
                        Template = Template.replace("##AssignedRoute##", data2.PickUpRouteName);
                        Template = Template.replace("##ROUTETYPE##", 1);
                    }
                    else if (data2.DropStatus == 1 || data2.DropStatus == 0) {
                        Template = Template.replace("##RouteHeader##", "Drop Route");
                        Template = Template.replace("##AssignedRoute##", data2.DropRouteName);
                        Template = Template.replace("##ROUTETYPE##", 2);
                    }
                    Template = Template.replace("##CLIENTID##", data2.ClientId);
                    Template = Template.replace("##clientName##", data2.ChildrenName);
                    Template = Template.replace("##clientAddress##", data2.Address);
                    Template = Template.replace("##clientCity##", data2.City);
                    Template = Template.replace("##clientState##", data2.State);
                    Template = Template.replace("##clientZipCode##", data2.ZipCode);
                    infowindow.setContent(Template);
                    infowindow.open(map, marker);
                    map.setCenter(new google.maps.LatLng($(this).attr('lat'), $(this).attr('long')));
                    map.panTo(this.getPosition());
                }
            })(marker, i));

            google.maps.event.addListener(infowindow, 'closeclick', function () {
                map.panTo(this.getPosition());
            });
        }


    });

    map.setCenter(new google.maps.LatLng(lat, long));

}

function UpdatePosition() {

}
function showHideAccordian(val) {

    $('.accordionButton').removeClass('on');
    $('.accordionContent').slideUp('normal');
    $('.plusMinus').text('+');
    if ($(val).next().is(':hidden') == true) {
        $(val).addClass('on');
        $(val).next().slideDown('normal');
        $(val).children('.plusMinus').text('-');
    }

}
function mouseHoverAccordian(acr) {

    $(acr).addClass('over');


}

function mouseOutAccordian(mouseout) {

    $(mouseout).removeClass('over');
}

function showChildInformation(clientId) {
    

}

function editCreatedRoute(editroute) {
    var routeId = $(editroute).attr('routeId');
    var pos = $(editroute).attr('pos');
    var route_type = $(editroute).attr('routetype');
    $('#routeNameTxt').val($('#routName_' + pos).html());
    $('#busNameTxt').val($('#BusName_' + pos).html());
    $('#busDriverSelect').val($('#busDriverName_' + pos).attr('driverid'));
    $('#busMonitorSelect').val($('#busMonitorName_' + pos).attr('monitorid'));
    //$('#routeNameTxt').val($('#RouteAddress_' + pos).html());
    $('#addRouteBtn').html('Update Route');
    $('#addRouteBtn').attr({ 'route-id': routeId, 'mode': 1 });
    
    $("input[name=routeradio][value=" + route_type + "]").prop('checked', true);
    $('html, body').animate({ scrollTop: $('#addRouteDiv').position().top }, 'slow');

}






