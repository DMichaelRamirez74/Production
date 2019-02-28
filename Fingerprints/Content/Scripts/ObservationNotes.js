$(function () {

    $('.ddl-domain').change(function () {
        if ($('.ddl-domain').val() != "0")
            BindElementById($('.ddl-domain').val());
        else
            $('#ddlElement').empty();
    });
    if ($('.hdn-domain').val() != "" && $('.hdn-domain').val() != "0") {
        $('.ddl-domain').val($('.hdn-domain').val());
        if ($('.ddl-domain').val() != "0") {
            BindElementById($('.ddl-domain').val());
            setTimeout(function () {
                $('#ddlElement').val($('.hdn-element').val());
            }, 100);
        }
        else
            $('#ddlElement').empty();
    }
    $('.AddAttachments').click(function () {
        var input = '<div style="position:relative" class="input_file_container">\
                                                <span class="remove_icon"><img src="/Content/imagechild/rem_icon.png"></span>\
                                                <input type="file" name="fileupload" class="fileAttachments" />\
                                            </div>';
        // var input = "<input type='file' name='fileupload' class='fileAttachments' />";
        $('.attachments-conatiner').append(input);
    });
    $('body').on('click', '.remove_icon', function () {
        $(this).parent('.input_file_container').remove();
    });
    var ObservationNotes = {};
    ObservationNotes.AttachmentPath = [];
    $('.btn-submit').click(function () {

        if ($('#txtDate').val().trim() == "") {
            customAlert("Date should not be empty");
        }
        else if ($('#txtTitle').val().trim() == "") {
            customAlert("Title should not be empty");
        }
        else if ($('#txtNote').val().trim() == "") {
            customAlert("Note should not be empty");
        }
        else if ($('.ddl-domain').val().trim() == "0") {
            customAlert("Please select domain");
        }
        else if ($('#ddlElement').val().trim() == "0" || $('#ddlElement').val() == null) {
            customAlert("Please select element");
        }
        else if ($('.client-id:checked').length == 0) {
            customAlert("Please select child");
        }
        else if (!validDate($('#txtDate').val().trim())) {
            customAlert("Enter valid date");
        }
        else if (!CheckFutureDate($('#txtDate').val())) {
            customAlert("Future data is not allowed");
        }
        else {
            ObservationNotes.lstClientid = [];
            $('.client-id').each(function () {
                if ($(this).is(':checked'))
                    ObservationNotes.lstClientid.push($(this).val());
            });
            if ($('.hdn-noteid').val().trim() != "")
                ObservationNotes.NoteId = $('.hdn-noteid').val();
            ObservationNotes.Date = $('#txtDate').val();
            ObservationNotes.Title = $('#txtTitle').val();
            ObservationNotes.Note = $('#txtNote').val();
            ObservationNotes.DomainId = $('.ddl-domain').val();
            ObservationNotes.ElementId = $('#ddlElement').val();
            ObservationNotes.ElementId = $('#ddlElement').val();



            if ($('.fileAttachments').eq(0).val() != "") {
                SaveFile();
            }
            else {
                SaveNotes();
            }
        }

    });

    $('.btn-Clear').click(function () {
        Clear();
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

    $('.client-details').click(function () {
        if ($('.lbl-child-details').text() == "Single Child")
            return false;
    });

    $(".txt-date").on('focusout', function (e) {
        if ($(this).val().trim() != "") {
            if (!validDate($(this).val().trim())) {
                customAlert("Enter valid date");
            }
            else if (!CheckFutureDate($(this).val())) {
                customAlert("Future data is not allowed");
            }
        }
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
    function SaveFile() {
        var data = new FormData();
        $('.fileAttachments').each(function (i, vak) {
            var files = $(this).get(0).files;
            if (files.length > 0) {
                data.append("MyImages" + i + "", files[0]);
            }
        });
        $.ajax({
            url: "/Roster/UploadFile",
            type: "POST",
            processData: false,
            contentType: false,
            asyn: false,
            data: data,
            success: function (imagepath) {
                ObservationNotes.AttachmentPath = imagepath;
                SaveNotes();
            },
            error: function (er) {
                //alert(er);
            }
        });
    }

    function SaveNotes() {
        ObservationNotes = JSON.stringify({ 'objNotes': ObservationNotes });
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: "POST",
            async: false,
            url: "/Roster/SaveObservationNotes",
            data: ObservationNotes,
            success: function (data) {
                // window.location.href = "/Home/TeacherDashBoard";
                ObservationNotes = {};
                ObservationNotes.AttachmentPath = [];
                Clear();
                customAlertSuccess("Record Saved Successfully");
            },
            error: function (data) {
            }
        });
    }

    function Clear() {
        $('.txt-empty').val("");
        $('.ddl-domain').val("0");
        $('#ddlElement').empty();
        $('.attachments-conatiner').empty();
        var input = '<div style="position:relative" class="input_file_container">\
                                                <span class="remove_icon"><img src="/Content/imagechild/rem_icon.png"></span>\
                                                <input type="file" name="fileupload" class="fileAttachments" />\
                                            </div>'
        $('.attachments-conatiner').append(input);
        $('.div-attachment').remove();
        if ($('.lbl-child-details').text() != "Single Child")
            $('.client-id').prop('checked', false);
    }
    function CheckFutureDate(date) {
        var isAllow = false;
        var now = new Date();
        var selectedDate = new Date(date);
        if (selectedDate < now) {
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
});



function BindElementById(DomainId) {
    $.ajax({
        type: "POST",
        url: "/Roster/GetElementDetailsByDomainId",
        data: { 'DomainId': DomainId },
        success: function (data) {
            console.log(JSON.parse(data));
            var template = "<option value='##ID##'>##NAME##</option>";
            var input = "<option value='0'>Choose</option>";
            $.each(JSON.parse(data), function (i, val) {
                input += template.replace("##ID##", val["DomainId"]).replace("##NAME##", val["Name"]);
            });
            $('#ddlElement').empty();
            $('#ddlElement').append(input);
        },
        error: function (data) {
            console.log('Error');
        }
    });
}