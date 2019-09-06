//!(function ($) {

//    function getRandomNumber() {
//        return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
//            (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
//        )

//    }


//    $.fn.ApplinkCameraCapture = function (option, parameter, extraOptions) {
//        var data = $(this).data('applink-camera');
//        var options = typeof option === 'object' && option;

//        // Initialize the multiselect.
//        if (!data || option.initialLoad) {
//            data = new ApplinkCameraCapture(this, options);
//            $(this).data('applink-camera', data);
//            if (option.initialLoad) {
//                // data.getRecord(data);
//                //navigator.mediaDevices.enumerateDevices().then(function (returndata) { data.gotDevices(returndata, data) });



//                //data.canvas = document.createElement('canvas');
//            }

//        }

//        // Call multiselect method.
//        if (typeof option === 'string') {
//            data[option](parameter, extraOptions);

//            if (option === 'destroy') {
//                $(this).data('applink-camera', false);
//            }
//        }

//    }

//    $.fn.ApplinkCameraCapture.Constructor = ApplinkCameraCapture;

//    ApplinkCameraCapture.prototype = {

//        //camera:{
//        //    multiple:1,
//        //    single:2,
//        //    both:3
//        //},
//       self:this,
//        defaults: {
//            parent:'modal',
//            cameraType: 1,
//            inititalLoad: false,
//            addImagefn: function (events) {

//            },
//            cancelImagefn: function (events) {

//            },
//            modalHidefn:function(events)
//            {

//            },
//            modalShowfn: function (events) {

//            }


//        },
//        documentModalElement:null,
//        addButtonElement: null,
//        cancelButtonElement: null,
//        divaddImageGalleryElement:null,
//        videoSelect: null,
//        screenshotButton:null,
//        img:null,
//        canvas:null,
//        currentStream:null,
//        video:null,
//        userMediaInterval: null,
//        cameraIndex: null,
//        multipleDocumentHtml: null,
//        singleDocumentHtml: null,
//        constructor: ApplinkCameraCapture,
//        handleSuccess:function(stream) {
//        this.screenshotButton.disabled = false;
//        this.video.srcObject = stream;
//    },

     
//        mergeOptions: function (options) {
//            return $.extend(true, {}, this.defaults, this.options, options);
//        },
//        video_Screenshot_Click:function(events){
//            var $index = events.divaddImageGalleryElement.find('.setup_viewscreen').length;

//            var $imageQuery = '<div class="setup_viewscreen col-xs-12 col-sm-12 col-lg-12" style="margin-bottom:10px;" id="image_gallery_' + $index + '">\
//                                        <img id="capt-img" class="setup_viewscreen-camera" src="">\
//                                        <div class="attach-icon-div-gallery">\
//                                            <i class="fa fa-eye view-file-upload" data-toggle="tooltip" tile="view file" aria-hidden="true" data-original-title="" title=""></i>\
//                                            <i class="fa fa-trash delete-file-upload"  data-placement="top" aria-hidden="true" title="" data-original-title="Delete Attachment"></i>\
//                                        </div>\
//                                    </div>';

//            events.canvas.width = events.video.videoWidth;
//            events.canvas.height = events.video.videoHeight;
//            events.canvas.getContext('2d').drawImage(events.video, 0, 0);
//            // Other browsers will fall back to image/png
//         //   console.log(multipleDoc_canvas);
//              events.divaddImageGalleryElement.append($imageQuery);

//              events.divaddImageGalleryElement.find('img').attr('src', events.canvas.toDataURL('image/png'));

//            //$('#modal-uploaddocument').find('.div-image-snap-gallery').find('#image_gallery_' + $index + '').find('img').attr('src', multipleDoc_canvas.toDataURL('image/png'));
//        },
//        videoSelectChange:function(events)
//        {
//            events.getStream(events);
//        },

//    stopMediaTracks:function(stream) {
//        stream.getTracks().forEach(function (track) {
//            track.stop();
//        });
//    },

//    gotDevices:function(mediaDevices,events) {


//       events.videoSelect.html('');

//        var count = 1;
       
//        mediaDevices.forEach(function (mediaDevice) {
//            if (mediaDevice.kind === 'videoinput') {
//                const option = document.createElement('option');
//                option.value = mediaDevice.deviceId;
//                const label = mediaDevice.label || 'Camera ' + (count++) + '';
//                const textNode = document.createTextNode(label);
//                option.appendChild(textNode);
//                events.videoSelect.append(option);
//            }
//        });
//    },

//    getStream:function(events) {


//        if (typeof events.currentStream !== 'undefined' &&  events.currentStream!=null && events.currentStream!='') {
//            events.stopMediaTracks(events.currentStream);
//        }
//        const videoConstraints = {};
//        if (events.videoSelect.val() === '') {
//            videoConstraints.facingMode = 'environment';
//        } else {
//            videoConstraints.deviceId = { exact: events.videoSelect.val() };
//        }
//        const constraints = {
//            video: videoConstraints,
//            audio: false
//        };

//       // var $this=this;

//        navigator.mediaDevices
//          .getUserMedia(constraints)
//          .then(function (stream) {
//              events.currentStream = stream;
//              events.video.srcObject = stream;
//              return navigator.mediaDevices.enumerateDevices();
//          })
//          .then(function (data) { events.gotDevices(data, events) })
//          .catch(function (error) {
//              console.error(error);
//              //alert(error);
//          });
//    },

//    showCameraOption:function(ele) {

//        var videoInputAvailable = 0;
//        $(ele).find('#uploadImageCamera').tooltip('hide');
//        $(ele).find('#uploadImageCamera').hide();
//        $(ele).find('#uploadImageCamera').parent('div').css('top', '4px');

//        navigator.mediaDevices.enumerateDevices().then(function (devices) {

//            console.log(devices);
//            devices.forEach(function (device) {


//                if (device.kind == 'videoinput') {
//                    videoInputAvailable++;
//                }

//            });


//            if (videoInputAvailable == 0) {
//                $(ele).find('#uploadImageCamera').hide();
//                $(ele).find('#uploadImageCamera').tooltip('hide');

//            }
//            else {
//                $(ele).find('#uploadImageCamera').show();
//                $(ele).find('#uploadImageCamera').tooltip('show');
//                $(ele).find('#uploadImageCamera').parent('div').css('top', '0px');

//            }

//            console.log(videoInputAvailable);

//        }).catch(function (err) {
//            console.log(err.name + ": " + err.message);
//        });


//        //if (videoSelect.options.length > 0) {
//        //    getStream();
//        //}




//    },

//    showVideoStream: function (events) {

//        if (events.videoSelect.find('option').length > 0) {
//            events.getStream(events);
//        }

//    },

//    checkUserMedia:function(ele) {




//        navigator.getMedia = (navigator.getUserMedia || // use the proper vendor prefix
//              navigator.webkitGetUserMedia ||
//              navigator.mozGetUserMedia ||
//              navigator.msGetUserMedia);

//        navigator.getMedia({ video: true }, function () {

//            if (!$(ele).find('#uploadImageCamera').is(':visible')) {

//                $(ele).find('#uploadImageCamera').show();
//                $(ele).find('#uploadImageCamera').tooltip('show');
//            }



//        }, function () {

//            $(ele).find('#uploadImageCamera').hide();
//            $(ele).find('#uploadImageCamera').tooltip('hide');


//        });


//    },
//    setIntervalUserMedia:function(ele) {
//        var $this=this;

//        this.userMediaInterval = setInterval(function () {

//            $this.checkUserMedia(ele);

//        }, 1000);



//    },


//    stopIntervalUserMedia:function(events) {
//       clearInterval(events.userMediaInterval);
//    },

//     getBase64Image:function(img) {

//        //var canvas = document.createElement("canvas");
//        //canvas.width = img.width;
//        //canvas.height = img.height;
//        //var ctx = canvas.getContext("2d");
//        //ctx.drawImage(img, 0, 0);
//        //var dataURL = ctx.toDataURL("image/png");
//        //return dataURL.replace(/^data:image\/(png|jpg);base64,/, "");

//        return $(img)[0].src.replace(/^data:image\/(png|jpg);base64,/, "");
//     },

//        getMultipleCameraUploadDocument:function(events){

//            $.ajax({

//                url: "/AgencyUser/GetMultipleFileUploadHtml",
//                async: false,
//                success:function(data)
//                {
//                    events.multipleDocumentHtml = data;

//                    $('body').append(events.multipleDocumentHtml);

//                    $('#modal-uploaddocument').addClass('multipleDocument_' + events.cameraIndex + '');


//                    events.documentModalElement = $('.multipleDocument_' + events.cameraIndex + '');
//                    events.documentModalElement.find('#btn-modal-doc-add').addClass('multipleAdd_' + events.cameraIndex + '');
//                    events.documentModalElement.find('#btn-modal-doc-cancel').addClass('multipleCancel_' + events.cameraIndex + '');

//                    events.addButtonElement = $('.multipleAdd_' + events.cameraIndex + '');
//                    events.cancelButtonElement = $('.multipleCancel_' + events.cameraIndex + '');
//                    events.divaddImageGalleryElement = events.documentModalElement.find('.div-image-snap-gallery');
//                    events.screenshotButton = events.documentModalElement.find('#anchor-capture');
//                    events.videoSelect = events.documentModalElement.find('#videoSource');
//                    events.video = events.documentModalElement.find('#videoElement');

//                }

//            })

//            //$.get("/AgencyUser/GetMultipleFileUploadHtml",function (data) {

//            //        events.multipleDocumentHtml = data;

//            //        $('body').append(events.multipleDocumentHtml);

//            //        $('#modal-uploaddocument').addClass('multipleDocument_' + events.cameraIndex + '');
                    

//            //        events.documentModalElement = $('.multipleDocument_' + events.cameraIndex + '');
//            //        events.documentModalElement.find('#btn-modal-doc-add').addClass('multipleAdd_' + events.cameraIndex + '');
//            //        events.documentModalElement.find('#btn-modal-doc-cancel').addClass('multipleCancel_' + events.cameraIndex + '');

//            //        events.addButtonElement = $('.multipleAdd_' + events.cameraIndex + '');
//            //        events.cancelButtonElement = $('.multipleCancel_' + events.cameraIndex + '');
//            //        events.divaddImageGalleryElement = events.documentModalElement.find('.div-image-snap-gallery');
//            //        events.screenshotButton = events.documentModalElement.find('#anchor-capture');
//            //        events.videoSelect = events.documentModalElement.find('#videoSource');
//            //        events.video = events.documentModalElement.find('#videoElement');
                   

//            //    });

//        },

//        getSingleCameraUploadDocument: function (events) {

//                $.get("/AgencyUser/GetSingleFileUploadHtml", function (data) {

//                    events.singleDocumentHtml = data;
//                    $('body').append(events.singleDocumentHtml);


//                });
//        }

//    }


//    function ApplinkCameraCapture(camera, options) {


        
//        this.$camera = $(camera);
//        this.options = this.mergeOptions($.extend({}, options, this.$camera.data()));
//        this.cameraIndex = getRandomNumber();
//        var $this=this;
//        switch(this.options.cameraType)
//        {
//               case 1:
//                   this.getMultipleCameraUploadDocument(this);
//                   break;

//            case 2:
//                this.getSingleCameraUploadDocument(this);
//                break;
//             default:
//                this.getMultipleCameraUploadDocument(this);
//                this.getSingleCameraUploadDocument(this);
//                break;


//        }

//        if (this.video) {


//            this.video.attr('autoplay', '');
//            this.video.attr('muted', '');
//            this.video.attr('playsinline', '')

//          //  multipleDoc_screenshotButton = document.querySelector('#modal-uploaddocument #anchor-capture');


//            //multipleDoc_screenshotButton.onclick = multipleDoc_video.onclick = function () {

              
//            //};

//            this.screenshotButton.click($.proxy(this.video_Screenshot_Click, this));
//            this.video.click($.proxy(this.video_Screenshot_Click, this));
//            this.videoSelect.change($.proxy(this.videoSelectChange, $this));
//            this.documentModalElement.on('hidden.bs.modal',($.proxy(this.modalHidefn, this)));


           

//            //multipleDoc_videoSelect.addEventListener('change', function (event) {

               
//            //});





//            navigator.mediaDevices.enumerateDevices().then(function (data) { $this.gotDevices(data,$this) });



//            this.canvas = document.createElement('canvas');



//            //$(document).on('click', '.view-file', function () {

//            //    var $attchmentId = $(this).closest('.attachment-icons-block').data('attach-id');
//            //    var $inkindtransactionId = self.elements.modalhiddenTransID.val();
//            //    self.getInkindAttachment($attchmentId, $inkindtransactionId);
//            //});


//            //$(document).on('click', '.delete-file', function () {


//            //    $(this).attr({ 'data-toggle': 'popover', 'title': 'Confirmation', 'data-placement': 'top' });

//            //    $(this).popover('show');

//            //});



//            $(document).on('click', '.delete-file-upload', function () {

//                $(this).closest('.setup_viewscreen').remove();

//            });



//            $(document).on('click', '.view-file-upload', function () {

//                //  window.open($(this)., 'newwindow','width=500,height=500');

//                var imageAttr = $(this).closest('.setup_viewscreen').find('.setup_viewscreen-camera').attr('src');
//                //   imageAttr=   imageAttr.replace(/^data:image\/(png|jpg);base64,/, "");
//                console.log(imageAttr);

//                //imageAttr='<embed src='+imageAttr+'></embed>';
//                //window.open(imageAttr, 'newwindow','width=500,height=500');



//                var w = window.open('about:blank', 'newwindow', 'width=500,height=500');

//                setTimeout(function () { //FireFox seems to require a setTimeout for this to work.
//                    w.document.body.appendChild(w.document.createElement('img'))
//                        .src = imageAttr;
//                }, 0);

//            });


//            var $this = this;

//            $(document).on('click', '.img-camera', function () {

//                $('.modal:visible').modal('hide');

//               // $('#modal-uploaddocument').find('.div-image-snap-gallery').html('');

//                $this.divaddImageGalleryElement.html('');
//                $this.documentModalElement.modal('show');

//               // $('#modal-uploaddocument').modal('show');
//            });

//          //  '#modal-uploaddocument'

//            $(document).on('shown.bs.modal', $this.documentModalElement, function () {
               

//                $('body').addClass('modal-open');




//                $this.showVideoStream($this);
//            }).on('hidden.bs.modal', function (event) {

//                $this.stopIntervalUserMedia($this);
//                //   $selfElements.modalEditInkind.modal('show');
//            });

//        }


//        if(this.options.parent=='modal')
//        {
//            var $this = this;

//           this.$camera.closest('.modal').on('shown.bs.modal', function () {





//                $this.showCameraOption(this);

//                //$selfElements.modalImageUploadCamera.hide();

//                $this.$camera.hide();
//                //$(this).find('#uploadImageCamera').hide();

//                $this.setIntervalUserMedia(this);

//            });

            

//        }
       

       

//        //if(this.options.multiple)
//        //{
//        //    if($('.multipleDocument_'+))
//        //}



//    }

//}(jQuery));





var multipleDoc_videoSelect = null;
var multipleDoc_screenshotButton = null;
var mulitpleDoc_img = null;
var multipleDoc_canvas = null;
//var button = null;
var multipleDoc_currentStream;
var multipleDoc_video;
// var constraints = {};
// var front = true;





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


    //if (videoSelect.options.length > 0) {
    //    getStream();
    //}




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


    //$(document).on('click', '.view-file', function () {

    //    var $attchmentId = $(this).closest('.attachment-icons-block').data('attach-id');
    //    var $inkindtransactionId = self.elements.modalhiddenTransID.val();
    //    //  self.getInkindAttachment($attchmentId, $inkindtransactionId);
    //});


    //$(document).on('click', '.delete-file', function () {


    //    $(this).attr({ 'data-toggle': 'popover', 'title': 'Confirmation', 'data-placement': 'top' });

    //    $(this).popover('show');

    //});



    $(document).on('click', '.delete-file-upload', function () {

        $(this).closest('.setup_viewscreen').remove();

    });



    $(document).on('click', '.view-file-upload', function () {

        //  window.open($(this)., 'newwindow','width=500,height=500');

        var imageAttr = $(this).closest('.setup_viewscreen').find('.setup_viewscreen-camera').attr('src');
        //   imageAttr=   imageAttr.replace(/^data:image\/(png|jpg);base64,/, "");
        console.log(imageAttr);

        //imageAttr='<embed src='+imageAttr+'></embed>';
        //window.open(imageAttr, 'newwindow','width=500,height=500');



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
        //   $selfElements.modalEditInkind.modal('show');
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
       
        //$('#ModalAddCasenote').modal('show');
    });



    $('#modal-uploaddocument #btn-modal-doc-add').on('click', function () {

      
        var $imageGallerylength = $('#modal-uploaddocument .div-image-snap-gallery').find('.setup_viewscreen').length;

        if ($imageGallerylength == 0) {
            customAlert('Please capture image using camera');
            return false;
        }



        var targetMode = $('#modal-uploaddocument').attr('target-modal');
        var $guid = $('#modal-uploaddocument').attr('target-index');


        if ($(targetMode + ' .div-edit-gallery_' + $guid + '').length > 0)
        {
            $(targetMode + ' .div-edit-gallery_' + $guid + '').append($('#modal-uploaddocument .div-image-snap-gallery').html());

        }
        else {
            $(targetMode + ' .div_append_image_gallery_' + $guid + '').append($('#modal-uploaddocument .div-image-snap-gallery').html());

        }

        $('#modal-uploaddocument .div-image-snap-gallery').html('');

        $('#modal-uploaddocument').modal('hide');

        if ($(targetMode).hasClass('modal'))
        {
            $('body').addClass('modal-open');

        }


    });

});