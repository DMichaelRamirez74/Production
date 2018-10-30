
$(function () {
    var MonitorList = [];
    var data = new FormData();

    $('body').on('click', '.imgIssues,.pDesc', function () {
     
        // if (!$(this).parent().parent().find('.img-tick').is(':visible') && !$(this).parent().parent().find('.img-cross').is(':visible') && $(this).parent().parent().find('.hdn-passfailcode').val() == "") {
       // $('#myModal .modal-header').css('border-bottom', '0px').css('height', '25px');
      //  $('#myModal').modal('show');
        $('.error-center-message').hide();
        $('.ImgContainer').removeClass('Selected');
        $(this).parent().parent().addClass('Selected');
        $('.tblFailDetails,.btn-submit,.btn-back').hide();
        $('.btn-pass,.btn-fail').show();

        //  }
        //if (!$(this).parent().parent().find('.img-tick').is(':visible') && !$(this).parent().parent().find('.img-cross').is(':visible') && $(this).parent().parent().find('.hdn-passfailcode').val() != "") {
        //    $('.fb-msg').text('You have already given feedback for this item.');
        //    $('#myModalAlert').modal('show');
        //}
    });

    $('body').on('click', '.btn-pass1', function () {
        $('.ImgContainer').removeClass('Selected');
        $(this).parent().parent().parent().addClass('Selected');
        $('.error-center-message').hide();
        $('.Selected').find('.img-tick').show();
        $('.Selected').find('.img-cross').show();
        $('.Selected').find('.trans-tick').hide();
        $('.Selected').find('.trans-cross').hide();
        $('.Selected').find('.trans-tick').show();
        var monitor = {};
        monitor.ImageId = $('.Selected .hdn-imageid').val();
        monitor.PassFailCode = true;
        $('.Selected .hdn-passfailcode').val('True');
        monitor.ToSataffId = $('.Selected .hdn-tostaffid').val();
        monitor.RouteCode = $('.Selected .hdn-routecode').val();
        if ($('.center-id').val().trim() != "")
            monitor.CenterId = $('.center-id').val().trim();
        DeleteExistingMonitor(monitor.RouteCode);
        MonitorList.push(monitor);
        $('.ImgContainer').removeClass('Selected');
        $('#myModal').modal('hide');

    });

    function DeleteExistingMonitor(RouteCode) {
        console.log(MonitorList);
        $(MonitorList).each(function (i, val) {
            if (val.RouteCode == RouteCode) {
                MonitorList.splice(i, 1);
            }

        });
        console.log(MonitorList);
    }

    $('body').on('click', '.btn-fail1', function () {

        $('.ImgContainer').removeClass('Selected');
        $(this).parent().parent().parent().addClass('Selected');
        var desc = $('.Selected').find('.pDesc').text().trim();
        var rolename = $('#myModal').find('.roleinfo').text();
        $('#myModal').find('.modal-title').text("");
        $('#myModal').find('.modal-title').text(rolename +" for "+desc);
        $('#myModal').modal('show');
        $('.error-center-message').hide();
        $('.err-message').hide();
        $('.tblFailDetails,.btn-submit,.btn-back').show();
        $('.btn-pass,.btn-fail').hide();
        $('.txtDescription').val("");
        $('#imgDamage').val("");
        $('#myModal .modal-header').css('border-bottom', '1px solid #e5e5e5').css('height', '45px');

    });

    $('body').on('click', '.btn-back', function () {
        $('.tblFailDetails,.btn-submit,.btn-back').hide();
        $('.btn-pass,.btn-fail').hide();
        $('#myModal').modal('hide');
       // $('#myModal .modal-header').css('border-bottom', '0px').css('height', '25px');
    });

    var ImagePath = "";

    $('.btn-submit').click(function () {
        if (ValidateInput()) {

            
            $('.error-center-message').hide();
            // $('.btn-pass1').find('.ImgContainer').find('.trans-tick').show();
            $('.Selected').find('.img-tick').show();
            $('.Selected').find('.img-cross').show();
            $('.Selected').find('.trans-tick').hide();
            $('.Selected').find('.trans-cross').hide();
            $('.Selected').find('.trans-cross').show();
            $('#myModal').modal('hide');
            SaveFile();
            $('#myModal .modal-header').css('border-bottom', '0px').css('height', '25px');
        }
    });

    $('.btn-save').click(function () {
        $('.ImgContainer').removeClass('Selected');
        var imageLength = $('.imgIssues').length;
        var imageCrossLength = $('.trans-cross:visible').length;
        var imageCheckLength = $('.trans-tick:visible').length;
        var totalcheck = (imageCrossLength + imageCheckLength);
        if (totalcheck != imageLength) {
            customAlert('Daily safety check is required for all blocks');
            return false;
        }

        SaveDetails();
    });
    //$('.btn-save-details').click(function () {
    //    $('.ImgContainer').removeClass('Selected');
    //    SaveDetail();
    //});
    $('#imgDamage').change(function () {
        var ext = $('#imgDamage').val().split('.').pop().toLowerCase();
        if ($.inArray(ext, ['gif', 'png', 'jpg', 'jpeg']) == -1) {
            $(this).val("");
            $('.err-message-dimagefile').show();
        }
        else {
            $('.err-message-dimagefile').hide();
        }
        $('.err-message-dimage ').hide();
    });

    $('.txtDescription').keyup(function () {
        $('.err-message-desc').hide();
    });

    $('body').on('click', '.btn-expiration', function () {
        $('#myModalExpirationDate').modal('show');
    });

    var flag = 0;
    $(".txt-date").on('keydown', function (e) {
        flag++;
        if (flag > 1) {
            e.preventDefault();
        }
    });
    $(".txt-date").on('keyup', function (e) {
        flag = 0;
    });

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
    $('.btnUpdateDate').click(function () {

        if ($('#txtExpirationDate').val() != "" && validDate($('#txtExpirationDate').val())) {
            var Role = $(this).attr('data-role');
            if (Checkdate($('#txtExpirationDate').val())) {
                $.ajax(
           {
               type: "POST",
               url: "/Teacher/UpdateFireExpiration",
               data: { 'Expire': $('#txtExpirationDate').val(), 'Role': Role },
               //data:{TextExpire: JSON.stringify('test')},
               success: function (data) {

                   if (data != "")
                       $('.spn-expiration-date').text($('#txtExpirationDate').val()).show();
                   $('#txtExpirationDate').val("");
               },
               error: function (data) {


               }
           });
                $('#myModalExpirationDate').modal('hide');
            }
            else {
                $('.date-future-validation').show();
            }

        }
        else {

            $('.date-valid-validation').show();
        }

    });

    function Checkdate(date) {
        var isAllow = false;
        var now = new Date();
        var selectedDate = new Date(date);
        if (selectedDate > now) {
            isAllow = true;
        } else {
            isAllow = false;
        }
        return isAllow;
    }
    function validDate(text) {
        var isValid = true;
        var comp = text.split('/');
        if (comp.length !== 3)
            return false;
        if (comp[2].length != 4)
            return false;
        if (comp[2] <= 1901)
            return false;
        if (new Date(text).toString() == "Invalid Date")
            return false;
        if (!isvalid_mdy(text))
            return false;
        var TodayDate = new Date();
        var endDate = new Date(text);

        return isValid;
    }
    function isvalid_mdy(s) {

        var day, A = s.match(/[1-9][\d]*/g);
        try {
            A[0] -= 1;
            day = new Date(+A[2], A[0], +A[1]);

            if (day.getMonth() == A[0] && day.getDate() == A[1]) return day;
        }
        catch (er) {
            return er.message;
        }

    }

    function ValidateInput() {
        $('.err-message').hide();
        var isAllow = true;
        if ($('.txtDescription').val() == "") {
            $('.err-message-desc').show();
            isAllow = false;
        }
        //if ($('#imgDamage').val() == "") {
        //    $('.err-message-dimage').show();
        //    isAllow = false;
        //}
        return isAllow;
    }

    function SaveDetails() {
        $('.ImgContainer').removeClass('Selected');


        var monitorList = {};
        monitorList= getMonitorList();
       
        if (monitorList.length > 0) {

            //var isAllowed = true;
            //$(MonitorList).each(function (i, val) {
            //    if (isAllowed)
            //        isAllowed = val.PassFailCode

            //});
            // if ($('.img-cross:visible').length > 0) {
            if ($('.trans-cross:visible').length > 0) {

                $('.btn-room').show();
                $('#myModalClassRoom').modal('show');
                return false;
            }
            else {
                //  return false;
                monitorList = JSON.stringify({ 'monitor': monitorList, 'Message': '' });
                $.ajax(
                            {
                                contentType: 'application/json; charset=utf-8',
                                dataType: 'json',
                                type: "POST",
                                async: false,
                                url: "/Teacher/InsertMonitoring",
                                data: monitorList,
                                success: function (data) {
                                    window.location.href = "/Home/TeacherDashBoard";
                                },
                                error: function (data) {

                                }
                            });
            }


        }
        else {
            $('.fb-msg').text('Feedback is not given for any item.');
            $('#myModalAlert').modal('show');

        }
    }
    $('#ddlCenters').change(function () {
        $('.error-center-message').hide();
    });

    $('.btn-center-submit').click(function () {
        if ($('.ddl-Centers').val() == "0") {
            $('.error-center-message').show();
        }
        else {
            $('.error-center-message').hide();
            $('#myModalCenterList').modal('hide');
        }
    });

    $('body').on('click', '.btn-room', function () {
        var isClosed = $(this).attr("isclosed") == "true" ? true : $(this).attr("isclosed") == "false" ? false : null;
        if (isClosed) {
            Save(isClosed, $('#ddlCenters').val());
        }
        else
            Save(isClosed, null);
    });

    function Save(isClosed, CenterId) {
        var MonitorList = [];
        MonitorList = getMonitorList();

        MonitorList = JSON.stringify({ 'monitor': MonitorList, 'Message': $(this).attr('message'), 'isClosed': isClosed, 'CenterId': CenterId });


        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: "POST",
            async: false,
            url: "/Teacher/InsertMonitoring",
            data: MonitorList,
            success: function (data) {
                window.location.href = "/Home/TeacherDashBoard";
            },
            error: function (data) {
            }
        });
    }

 

    function SaveFile() {
        var data = new FormData();
        var files = $("#imgDamage").get(0).files;
        if (files.length > 0) {
            data.append("MyImages", files[0]);
        }

        $.ajax({
            url: "/Teacher/UploadFile",
            type: "POST",
            processData: false,
            contentType: false,
            asyn: false,
            data: data,
            success: function (imagepath) {
                var monitor = {};
                monitor.ImageId = $('.Selected .hdn-imageid').val();
                monitor.PassFailCode = false;
                monitor.ImageOfDamage = imagepath;
                monitor.Description = $('.txtDescription').val();
                monitor.ToSataffId = $('.Selected .hdn-tostaffid').val();
                monitor.RouteCode = $('.Selected .hdn-routecode').val();
                $('.Selected .hdn-damage-image').val(imagepath);
                $('.Selected .hdn-damage-desc').val($('.txtDescription').val());
                $('.Selected .hdn-passfailcode').val('False');
                if ($('.center-id').val().trim() != "")
                    monitor.CenterId = $('.center-id').val().trim();
                DeleteExistingMonitor(monitor.RouteCode);
                MonitorList.push(monitor);

            },
            error: function (er) {
                alert(er);
            }
        });

    }
});

function getMonitorList() {
    var monitorListArray = [];
    $('.ImgContainer').each(function (k, container) {
        var monitor = {};
        monitor.ImageId = $(container).find('.hdn-imageid').val();
        monitor.PassFailCode = ($(container).find('.hdn-passfailcode').val() == 'True') ? true : false;
        monitor.ImageOfDamage = $(container).find('.hdn-damage-image').val();
        monitor.Description = $(container).find('.hdn-damage-desc').val();
        monitor.ToSataffId = $(container).find('.hdn-tostaffid').val();
        monitor.RouteCode = $(container).find('.hdn-routecode').val();
        if ($('.center-id').val().trim() != "")
        monitor.CenterId = $('.center-id').val().trim();
        monitorListArray.push(monitor);
   
    });

    console.log(monitorListArray);
    return monitorListArray;

}
