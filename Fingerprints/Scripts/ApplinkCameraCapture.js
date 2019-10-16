



var multipleDoc_videoSelect = null;
var multipleDoc_screenshotButton = null;
var mulitpleDoc_img = null;
var multipleDoc_canvas = null;
var multipleDoc_currentStream;
var multipleDoc_video;




function handleSuccess(stream) {
    multipleDoc_screenshotButton.disabled = false;
    multipleDoc_video.srcObject = stream;
}

function stopMediaTracks(stream) {
    stream.getTracks().forEach(function (track) {
        track.stop();
    });
}

function gotDevices(mediaDevices) {

    if (multipleDoc_videoSelect.options.length == 0)
    {
        multipleDoc_videoSelect.innerHTML = "";

        var count = 1;
        mediaDevices.forEach(function (mediaDevice) {
            if (mediaDevice.kind === 'videoinput') {
                const option = document.createElement('option');
                option.value = mediaDevice.deviceId;
                const label = mediaDevice.label || 'Camera ' + (count++) + '';
                const textNode = document.createTextNode(label);
                option.appendChild(textNode);
                multipleDoc_videoSelect.appendChild(option);
            }
        });
    }

   
   
}

function getStream() {

    if (typeof multipleDoc_currentStream !== 'undefined') {
        stopMediaTracks(multipleDoc_currentStream);
    }
    const videoConstraints = {};
    if (multipleDoc_videoSelect.value === '') {
        videoConstraints.facingMode = 'environment';
    } else {
        videoConstraints.deviceId = { exact: multipleDoc_videoSelect.value };
    }
    const constraints = {
        video: videoConstraints,
        audio: false
    };
    navigator.mediaDevices
      .getUserMedia(constraints)
      .then(function (stream) {
          multipleDoc_currentStream = stream;
          multipleDoc_video.srcObject = stream;
          return navigator.mediaDevices.enumerateDevices();
      })
      .then(gotDevices)
      .catch(function (error) {
       
      });
}

function showCameraOption(ele) {

    var videoInputAvailable = 0;
    $(ele).find('#uploadImageCamera').tooltip('hide');
    $(ele).find('#uploadImageCamera').hide();
    $(ele).find('#uploadImageCamera').parent('div').css('top', '4px');

    navigator.mediaDevices.enumerateDevices().then(function (devices) {

       
        devices.forEach(function (device) {


            if (device.kind == 'videoinput') {
                videoInputAvailable++;
            }

        });


        if (videoInputAvailable == 0) {
            $(ele).find('#uploadImageCamera').hide();
            $(ele).find('#uploadImageCamera').tooltip('hide');

        }
        else {
            $(ele).find('#uploadImageCamera').show();
            $(ele).find('#uploadImageCamera').tooltip('show');
            $(ele).find('#uploadImageCamera').parent('div').css('top', '0px');

        }

   

    }).catch(function (err) {
        console.log(err.name + ": " + err.message);
    });


  


}

function showVideoStream() {
    if (multipleDoc_videoSelect.options.length > 0) {
        getStream();
    }

}

function checkUserMedia(ele) {




    navigator.getMedia = (navigator.getUserMedia || // use the proper vendor prefix
          navigator.webkitGetUserMedia ||
          navigator.mozGetUserMedia ||
          navigator.msGetUserMedia);

    navigator.getMedia({ video: true }, function () {

        if (!$(ele).find('#uploadImageCamera').is(':visible')) {

            $(ele).find('#uploadImageCamera').show();
            $(ele).find('#uploadImageCamera').tooltip('show');
        }



    }, function () {

        $(ele).find('#uploadImageCamera').hide();
        $(ele).find('#uploadImageCamera').tooltip('hide');


    });


}

var userMediaInterval = null;

function setIntervalUserMedia(ele) {

    userMediaInterval = setInterval(function () {

        checkUserMedia(ele);

    }, 1000);



}

function stopIntervalUserMedia() {
    clearInterval(userMediaInterval);
}

function getBase64Image(img) {

    return $(img)[0].src.replace(/^data:image\/(png|jpg);base64,/, "");
}


$(function () {

    //Image Camera Option Start //

    


    multipleDoc_videoSelect = document.querySelector('#modal-uploaddocument select#videoSource');
    multipleDoc_video = document.querySelector('#modal-uploaddocument #setup-camera-div video');

   
    if (multipleDoc_video) {



        multipleDoc_video.setAttribute('autoplay', '');
        multipleDoc_video.setAttribute('muted', '');
        multipleDoc_video.setAttribute('playsinline', '')

        multipleDoc_screenshotButton = document.querySelector('#modal-uploaddocument #anchor-capture');


        multipleDoc_screenshotButton.onclick = multipleDoc_video.onclick = function () {

            var $index = $('#modal-uploaddocument').find('.div-image-snap-gallery').find('.setup_viewscreen').length;

            var $imageQuery = '<div class="setup_viewscreen col-xs-12 col-sm-12 col-lg-12" style="margin-bottom:10px;" id="image_gallery_' + $index + '">\
                                <img id="capt-img" class="setup_viewscreen-camera" src="">\
                                <div class="attach-icon-div-gallery">\
                                    <i class="fa fa-eye view-file-upload" data-toggle="tooltip" tile="view file" aria-hidden="true" data-original-title="" title=""></i>\
                                    <i class="fa fa-trash delete-file-upload"  data-placement="top" aria-hidden="true" title="" data-original-title="Delete Attachment"></i>\
                                </div>\
                            </div>';

            multipleDoc_canvas.width = multipleDoc_video.videoWidth;
            multipleDoc_canvas.height = multipleDoc_video.videoHeight;
            multipleDoc_canvas.getContext('2d').drawImage(multipleDoc_video, 0, 0);
            // Other browsers will fall back to image/png
    

            $('#modal-uploaddocument').find('.div-image-snap-gallery').append($imageQuery);




            $('#modal-uploaddocument').find('.div-image-snap-gallery').find('#image_gallery_' + $index + '').find('img').attr('src', multipleDoc_canvas.toDataURL('image/png'));
       
        };


        multipleDoc_videoSelect.addEventListener('change', function (event) {

            getStream();
        });





        navigator.mediaDevices.enumerateDevices().then(gotDevices);



        multipleDoc_canvas = document.createElement('canvas');



      

    }


    


    $(document).on('click', '.delete-file-upload', function () {

        $(this).closest('.setup_viewscreen').remove();

    });



    $(document).on('click', '.view-file-upload', function () {

        

        var imageAttr = $(this).closest('.setup_viewscreen').find('.setup_viewscreen-camera').attr('src');
      


        var w = window.open('about:blank', 'newwindow', 'width=500,height=500');

        setTimeout(function () { //FireFox seems to require a setTimeout for this to work.
            w.document.body.appendChild(w.document.createElement('img'))
                .src = imageAttr;
        }, 0);

    });


    $(document).on('show.bs.modal', '#modal-uploaddocument', function () {
        $('.modal:visible').modal('hide');


    });

    $(document).on('shown.bs.modal', '#modal-uploaddocument', function () {

      $('body').addClass('modal-open');




        showVideoStream();
    }).on('hidden.bs.modal', function (event) {

        stopIntervalUserMedia();
        
    });


    $(document).on('click', '.img-camera', function () {

     

        var $dateIndex=$(this).attr('data-guid')



       



        if ($('.modal:visible').length > 0) {
            $('#modal-uploaddocument').attr({ 'target-modal': '#' + $('.modal:visible').attr('id') + '', 'target-index': $dateIndex });
            $('.modal:visible').modal('hide');

        }
        else {
            $('#modal-uploaddocument').attr({ 'target-modal': '#' + $(this).attr('target-id') + '', 'target-index': $dateIndex });

        }



        $('#modal-uploaddocument').find('.div-image-snap-gallery').html('');

        $('#modal-uploaddocument').modal('show');
    });


    $('#modal-uploaddocument').on('hidden.bs.modal', function () {

        if ($('' + $(this).attr('target-modal') + '').hasClass('modal'))
        {
            $('' + $(this).attr('target-modal') + '').modal('show');
            $('body').addClass('modal-open');
        }

       
     

        $('body').css('padding-right', '0px');
       
        
    });



    $('#modal-uploaddocument #btn-modal-doc-add').on('click', function () {

      
        var $imageGallerylength = $('#modal-uploaddocument .div-image-snap-gallery').find('.setup_viewscreen').length;

        if ($imageGallerylength == 0) {
            customAlert('Please capture image using camera');
            return false;
        }



        var targetMode = $('#modal-uploaddocument').attr('target-modal');
        var $guid = $('#modal-uploaddocument').attr('target-index');


        if (targetMode!=null && targetMode!="" && $(targetMode + ' .div-edit-gallery_' + $guid + '').length > 0)
        {
            $(targetMode + ' .div-edit-gallery_' + $guid + '').append($('#modal-uploaddocument .div-image-snap-gallery').html());

        }
        else if (targetMode != null && targetMode != "" &&  $(targetMode + ' .div_append_image_gallery_' + $guid + '').length > 0) {
            $(targetMode + ' .div_append_image_gallery_' + $guid + '').append($('#modal-uploaddocument .div-image-snap-gallery').html());

        }
        else {

            $('#div-edit-modal-img-gallery').append($('#modal-uploaddocument .div-image-snap-gallery').html());
        }

        $('#modal-uploaddocument .div-image-snap-gallery').html('');

        $('#modal-uploaddocument').modal('hide');

        if (targetMode!=null && targetMode!='' && $(targetMode).hasClass('modal'))
        {
            $('body').addClass('modal-open');

        }


    });

});