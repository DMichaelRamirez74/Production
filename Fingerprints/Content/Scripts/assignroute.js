var pickupData = '';
var dropUpData = '';
var allData = '';
var conVlat = 0;
var convLong = 0;
function initMap()
{
    var map = new google.maps.Map(document.getElementById('googleMap'), {
        zoom: 10,
        center: new google.maps.LatLng(-33.92, 151.25),
        mapTypeId: google.maps.MapTypeId.ROADMAP
    });

}
$(document).ready(function () {


    $.ajax({
        url: '/Transportation/GetChildrenToAssignRoute',
        datatype: 'json',
        type: 'post',
        success: function (data) {
            if (data.length > 0) {
                LoadMap(data);
            }
        },
        error: function (data) {

        }
    });
});

function getAllData()
{
    $.ajax({
        url: '/Transportation/GetChildrenToAssignRoute',
        datatype: 'json',
        type: 'post',
        success: function (data) {
            if (data.length > 0) {
                LoadMap(data);
            }
        },
        error: function (data) {

        }
    });
}


function getPickUpChildren()
{
    $.ajax({
        url: '/Transportation/GetPickUpChildren',
        datatype: 'json',
        type: 'post',
        success: function (data) {
            if (data.length > 0) {
                LoadMap(data);
            }
        },
        error: function (data) {

        }
    });
}

function getDropUpChildren() {
    $.ajax({
        url: '/Transportation/GetDropUpChildren',
        datatype: 'json',
        type: 'post',
        success: function (data) {
            if (data.length > 0) {
                LoadMap(data);
            }
        },
        error: function (data) {

        }
    });
}
function getLatLongFromAddress(address1, city, state, zipcode) {

    var geocoder = new google.maps.Geocoder();

      var con = address1 + ',' + city + ',' + state + ',' + zipcode;
    var con = parseInt(zipcode);
    var latitude = 0;
    var longitude = 0;
    var mapAddress = "https://maps.googleapis.com/maps/api/geocode/json?address="+con
    $.getJSON(mapAddress, function (data) {
   
        if (data.status == "OK")
        {
           
            conVlat = data.results[0].geometry.location.lat;
            convLong = data.results[0].geometry.location.lng;
          
        }
      
    });
    //var address2 = "new york"
    geocoder.geocode({ 'address': con}, function (results, status) {

        if (status == google.maps.GeocoderStatus.OK) {
            conVlat = results[0].geometry.location.lat();
            convLong = results[0].geometry.location.lng();

        }
        else {
          //  alert('No data');
        }
    });
   // return [latitude, longitude];
}

function LoadMap(mapData) {
    var lat = 0;
    var long = 0;
    var map = new google.maps.Map(document.getElementById('googleMap'), {
        zoom: 15,
        center: new google.maps.LatLng(mapData[0].Latitude, mapData[0].Longitude),
        mapTypeId: google.maps.MapTypeId.ROADMAP
    });


        var infowindow = new google.maps.InfoWindow();

    var marker, i;

    $.each(mapData, function (k, data2) {
        //var address = data2.Address + ' ' + data2.City + ',' + data2.State + ' ' + data2.ZipCode;
        //getBounds(address);
        
                if (data2.Latitude != 0) {

                    lat = (lat == 0) ? data2.Latitude : lat;
                    long = (long == 0) ? data2.Longitude : long;

                    marker = new google.maps.Marker({
                        position: new google.maps.LatLng(data2.Latitude, data2.Longitude),
                        title: data2.ChildrenName,
                        map: map,
                        icon: data2.PinSmall,
                        lat: data2.Latitude,
                        long:data2.Longitude
                    });

                    google.maps.event.addListener(marker, 'click', (function (marker, i) {
                        return function () {
                            var mapClient = $('.map-notify');
                            mapClient.find('#clientName').html(data2.ChildrenName);
                            mapClient.find('#clientAddress').html(data2.Address);
                            mapClient.find('#clientCity').html(data2.City);
                            mapClient.find('#clientState').html(data2.State);
                            mapClient.find('#clientZipCode').html(data2.ZipCode);
                            infowindow.setContent(data2.ChildrenName + ' <br>' + data2.Address + ' <br>' + data2.City + ' <br>' + data2.State + ' <br>' + data2.ZipCode);
                            infowindow.setContent(mapClient.html());
                            infowindow.open(map, marker);
                             map.setCenter(new google.maps.LatLng($(this).attr('lat'), $(this).attr('long')));
                            map.panTo(this.getPosition());
                           // map.setZoom(15);
                        }
                    })(marker, i));

                    google.maps.event.addListener(infowindow, 'closeclick', function () {
                        map.panTo(this.getPosition());
                           // map.setZoom(10);
                    });
                }

                //if (conVlat!= 0) {

                //   // lat = (lat == 0) ? data2.Latitude : lat;
                //   // long = (long == 0) ? data2.Longitude : long;

                //    marker = new google.maps.Marker({
                //        position: new google.maps.LatLng(conVlat, convLong),
                //        title: data2.ChildrenName,
                //        map: map,
                //        icon: data2.PinSmall,
                //        lat: data2.Latitude,
                //        long: data2.Longitude
                //    });



                //    google.maps.event.addListener(marker, 'click', (function (marker, i) {
                //        return function () {
                //            var mapClient = $('.map-notify');
                //            mapClient.find('#clientName').html(data2.ChildrenName);
                //            mapClient.find('#clientAddress').html(data2.Address);
                //            mapClient.find('#clientCity').html(data2.City);
                //            mapClient.find('#clientState').html(data2.State);
                //            mapClient.find('#clientZipCode').html(data2.ZipCode);
                //            // infowindow.setContent(data2.ChildrenName + ' <br>' + data2.Address + ' <br>' + data2.City + ' <br>' + data2.State + ' <br>' + data2.ZipCode);
                //            infowindow.setContent(mapClient.html());
                //            infowindow.open(map, marker);
                //            // marker.setIcon(locations[i][9]);
                //            // map.setCenter(new google.maps.LatLng($(this).attr('lat'), $(this).attr('long')));
                //            map.panTo(this.getPosition());
                //            map.setZoom(20);
                //        }
                //    })(marker, i));

                //    google.maps.event.addListener(infowindow, 'closeclick', function () {
                //        map.panTo(this.getPosition());
                //        map.setZoom(8);
                //    });
                //}
    });

    map.setCenter(new google.maps.LatLng(lat, long));
  //  map.setCenter(new google.maps.LatLng(conVlat, convLong));

   
   
    
   
}
//function getBounds(address)
//{
//    debugger;
//    var geocoder = new google.maps.Geocoder();
//    geocoder.geocode({ 'address': address }, function (results, status) {

//        if (status == google.maps.GeocoderStatus.OK) {
//            var x = results[0].geometry.location.lat();
//            var y = results[0].geometry.location.lng();
//            conVlat = x;
//            convLong = y;
//        }
//    });
//}

$('#searchRoute').on('click', function () {
    var dropDownValue = $('#route-select').val();
    if(dropDownValue=='0')
    {
        getAllData();
   
    }
    else if(dropDownValue=='1')
    {
        getPickUpChildren();
    }
    else {
        getDropUpChildren();
    }

});