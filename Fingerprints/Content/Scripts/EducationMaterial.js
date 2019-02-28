
$(function () {

    $('.AddAttachments').click(function () {

        var input = '<div class="form-group">\
                                            <input type="file" name="img[]" class="file fileAttachments">\
                                            <div class="input-group col-xs-12">\
                                                <input type="text" class="form-control input-lg" disabled placeholder="Upload File">\
                                                <span class="input-group-btn">\
                                                    <button class="browse btn btn-primary input-lg" type="button"> Browse</button>\
                       <img src="/Content/imgs/close_1.png" class="remove-attachment" style="padding-left:5px;">\
                                                </span>\
                                            </div>\
                                        </div>';
        // var input = "<input type='file' name='fileupload' class='fileAttachments' />";
        $('.attachments-conatiner').append(input);
    });

    $('body').on('click', '.remove-attachment', function () {

        $(this).parent().parent().parent().remove();
    });
    $('.btn-clear').click(function () {
        Clear();
    });
    function GetKey(evt) {
        evt = (evt) ? evt : (window.event) ? event : null;
        if (evt) {
            var cCode = (evt.charCode) ? evt.charCode :
                    ((evt.keyCode) ? evt.keyCode :
                    ((evt.which) ? evt.which : 0));


            return cCode;
        }
    }
    //$('#txtURL').keypress(function (e) {
    //    var regex = new RegExp("^[a-zA-Z0-9-.]+$");
    //    var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);        
    //    if (e.keyCode != 0 && navigator.userAgent.indexOf("Firefox")!=-1)
    //        str = e.keyCode;
    //    //$('#txtURLNote').val(str);
    //    if (regex.test(str)) {
    //        return true;
    //    }        
    //    e.preventDefault();
    //    return false;
    //});

    $('#txtURL').bind('keypress', function (e) {
        var regex = new RegExp("^[a-zA-Z0-9-.:/]+$");
        var str = String.fromCharCode(!e.charCode ? e.which : e.charCode);
        if (e.keyCode != 0 && navigator.userAgent.indexOf("Firefox") != -1)
            str = e.keyCode;

        if (regex.test(str)) {
            return true;
        }
        e.preventDefault();
        return false;
    });

    $('#txtURL').bind('keyup', function () {
        $(this).val($(this).val().replace(/[^a-zA-Z0-9@.:/]+/, ''));
    });

    BindMaterials();
    var EductionMaterial = {};
    EductionMaterial.AttachmentPath = [];
    $('.btn-submit').click(function () {
      
        var txt = $('#txtURL').val().trim();
        console.log('Length:' + txt.split('.').length);
        
        var re = /(http(s)?:\\)?([\w-]+\.)+[\w-]+[.com|.in|.org]+(\[\?%&=]*)?/
        var patt = /^[a-zA-Z0-9\.\_]*$/;
        var isAllow = true;
        if ($('#ddlGroup').val().trim() == "0") {
            customAlert("Group is mandatory");
            isAllow = false;
        }
        else if ($('#txtTitle').val().trim() == "") {
            customAlert("Title is mandatory");
            isAllow = false;
        }
        else if ((!re.test(txt) || txt.split('.').length < 3) && txt != "") {
            customAlert("Enter valid url");
            isAllow = false;
        }
        else if (isAllow && $('.material-id').val().trim() == "") {
            $('.fileAttachments').each(function (i, val) {
                if ($(this).val().trim() == "" && isAllow) {
                    isAllow = false;
                    customAlert("Please upload file");
                }
            });
        }
        if (isAllow) {
            if ($('.material-id').val().trim() != "")
                EductionMaterial.Id = $('.material-id').val();
            EductionMaterial.Group = $('#ddlGroup').val();
            EductionMaterial.Title = $('#txtTitle').val();
            EductionMaterial.Description = $('#txtDescription').val();
            EductionMaterial.URL = $('#txtURL').val();
            EductionMaterial.URLNote = $('#txtURLNote').val();
            if ($('.fileAttachments').eq(0).val() != "") {
                SaveFile();
            }
            else {
                SaveMaterial();
            }
            BindMaterials();
        }
    });
    $('.btn-search').click(function () {
        var searchtext = $('#ddlGroupSearch').val().trim().toLocaleLowerCase();
        if (searchtext !== "0") {
            $('.library-table-head tr').each(function () {
                var text = $(this).attr('group').toLocaleLowerCase();
                if (searchtext == text) {
                    $(this).show();
                }
                else {
                    $(this).hide();
                }
                //if (text.indexOf(searchtext) != -1) {
                //    $(this).show();
                //}
                //else {
                //    $(this).hide();
                //}
            });
        }
        else {
            $('.library-table-head tr').show();
        }

    });
    $('body').on('click', '.dropdown', function () {
        setTimeout(function ()
        { $('.dropdown').addClass('open'); }, 100);

    });
    $('body').on('click touchstart', '.Delete', function () {
        var id = $(this).closest('tr').attr('Id');
        BootstrapDialog.confirm('Do you want to delete this record?', function (result) {
            if (result) {

                Delete(id);
            }
        });

    });
    $('body').on('click touchstart', '.edit', function () {
        var currentrow = $(this).closest('tr');
        $('.material-id').val(currentrow.attr('id'));
        $('#ddlGroup').val(currentrow.attr('group'));
        $('#txtTitle').val(currentrow.find('td').eq(0).text().trim());
        $('#txtDescription').val(currentrow.find('td').eq(1).text().trim());
        $('#txtURL').val(currentrow.find('td').eq(2).text().trim());
        $('#txtURLNote').val(currentrow.find('td').eq(3).text().trim());
        BindAttachmentById(currentrow.attr('id'));
    });

    function Delete(Id) {
        $.ajax({
            type: "POST",
            url: "/EducationMaterial/DeleteMaterial",
            data: { 'Id': Id },
            success: function (data) {
                if (data) {
                    customAlertSuccess("Record Deleted Successfully");
                    Clear();
                    BindMaterials();
                }
            },
            error: function (data) {
                console.log('Error');
            }
        });
    }

    function SaveFile() {
        var data = new FormData();
        $('.fileAttachments').each(function (i, vak) {
            var files = $(this).get(0).files;
          
            if (files.length > 0) {
                data.append("MyImages" + i + "", files[0]);
            }
           
        });

        $.ajax({
            url: "/EducationMaterial/UploadFile",
            type: "POST",
            processData: false,
            contentType: false,
            asyn: false,
            data: data,
            success: function (imagepath) {
                EductionMaterial.AttachmentPath = imagepath;
                SaveMaterial();
            },
            error: function (er) {
                //alert(er);
            }
        });

    }

    function SaveMaterial() {
        EductionMaterial = JSON.stringify({ 'objEductaion': EductionMaterial });
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: "POST",
            async: false,
            url: "/EducationMaterial/SaveEducationMaterial",
            data: EductionMaterial,
            success: function (data) {
                // window.location.href = "/Home/TeacherDashBoard";
                EductionMaterial = {};
                EductionMaterial.AttachmentPath = [];
                Clear();
                BindMaterials();
                customAlertSuccess("Record Saved Successfully");
            },
            error: function (data) {
            }
        });
    }

    function Clear() {
        $('.li-download').hide();
        $('.txt-empty').val("");
        $('#ddlGroup').val("0");
        $('.material-id').val('');
        $('.download-attachment').empty();
        $('.attachments-conatiner').empty();
        var input = '<div class="form-group">\
                                            <input type="file" name="img[]" class="file fileAttachments">\
                                            <div class="input-group col-xs-12">\
                                                <input type="text" class="form-control input-lg" disabled placeholder="Upload File">\
                                                <span class="input-group-btn">\
                                                    <button class="browse btn btn-primary input-lg" type="button"> Browse</button>\
                                                </span>\
                                            </div>\
                                        </div>';
        // var input = "<input type='file' name='fileupload' class='fileAttachments' />";
        $('.attachments-conatiner').append(input);
    }
    function BindAttachmentById(MaterialId) {
        $.ajax({
            type: "POST",
            url: "/EducationMaterial/GetAttachmentByMaterialId",
            data: { 'MaterialId': MaterialId },
            success: function (data) {
                console.log(JSON.parse(data));
                $('.download-attachment').empty();
                $('.li-download').hide();
                //var input = "<option value='0'>Choose</option>";
                $.each(JSON.parse(data), function (i, val) {
                    console.log(val.AttachmentPath);
                    var template = '<div style="display:inline-block;">\
                                                    <a target="_blank" title="Download" href="/Teacher/DownloadDocuments/?FilePath=' + val.AttachmentPath + '"><i class="fa fa-download download-ic"></i></a>\
                                        </div>';
                    $('.download-attachment').append(template);
                    $('.li-download').show();
                });
                //$('#ddlElement').empty();
                //$('#ddlElement').append(input);
            },
            error: function (data) {
                console.log('Error');
            }
        });
    }
    function BindMaterials() {
        $.ajax({
            type: "POST",
            url: "/EducationMaterial/GetMaterialDetails",
            // data: { 'DomainId': DomainId },
            success: function (data) {
             
                $('.library-table-head').empty();

                var input = "<option value='0'>Choose</option>";
                $.each(JSON.parse(data.JSONString), function (i, val) {
                    var template = '<tr group="{Group}" Id="{Id}">\
                                                <td data-title="Title">\
                                                    <div class="library-content-desc url-des" style="position: relative;">\
                                                        <p>{Title}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="Description">\
                                                    <div class="library-content-desc url-des1" style="position: relative;">\
                                                        <p>{Description}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="URL">\
                                                    <div class="library-content-desc url-des">\
                                                        <p>{URL}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="URL Note">\
                                                    <div class="library-content-desc url-des">\
                                                        <p>{URLNote}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="Image">\
                                                    <div class="library-content-desc url-des" style="position: relative;">\
                                                        <p style="padding: 2px;text-align:center;">{Attachment}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="Posted By">\
                                                    <div class="library-content-desc url-des">\
                                                        <p>{Posted}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="Action">\
                                                    <div style="width: auto;display: inline-block;">\
                                                        <div class="Status-title-library">\
                                                            <p style="margin-top: 0;cursor: pointer;" class="edit">\
                                                                <i class="fa fa-pencil" aria-hidden="true"></i><span>Edit</span>\
                                                            </p>\
                                                        </div>\
                                                        <div class="Status-title-library">\
                                                            <p style="margin-top: 5px;cursor: pointer;" class="delete Delete">\
                                                                <i class="fa fa-trash-o" aria-hidden="true"></i><span>Delete</span>\
                                                            </p>\
                                                        </div>\
                                                    </div>\
                                                </td>\
                                            </tr>';
                   
                    if (val["UserId"] != data.UserId) {
                        template = '<tr group="{Group}" Id="{Id}">\
                                                <td data-title="Title">\
                                                    <div class="library-content-desc" style="position: relative;">\
                                                        <p>{Title}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="Description">\
                                                    <div class="library-content-desc" style="position: relative;">\
                                                        <p>{Description}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="URL">\
                                                    <div class="library-content-desc">\
                                                        <p>{URL}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="URL Note">\
                                                    <div class="library-content-desc">\
                                                        <p>{URLNote}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="Image">\
                                                    <div class="library-content-desc" style="position: relative;">\
                                                        <p style="padding: 2px;text-align:center;">{Attachment}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="Posted By">\
                                                    <div class="library-content-desc">\
                                                        <p>{Posted}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="Action">\
                                                    <div style="width: auto;display: inline-block;">\
                                                    </div>\
                                                </td>\
                                            </tr>';
                    }
                    console.log(val["Description"]);
                    template = template.replace("{Id}", val["Id"]);
                    template = template.replace("{Group}", val["Group"]);
                    template = template.replace("{Title}", val["Title"]);
                    template = template.replace("{Description}", val["Description"] == "null" || val["Description"] == null ? "" : val["Description"]);
                    template = template.replace("{URL}", val["URL"]);
                    template = template.replace("{URLNote}", val["URLNote"]);
                    template = template.replace("{Attachment}", val["Attachment"]);
                    template = template.replace("{Posted}", val["Posted"]);
                    $('.library-table-head').append(template);
                });


            },
            error: function (data) {
                console.log('Error');
            }
        });
    }
    function BindMaterials() {
        $.ajax({
            type: "POST",
            url: "/EducationMaterial/GetMaterialDetails",
            // data: { 'DomainId': DomainId },
            success: function (data) {
               
                $('.library-table-head').empty();

                var input = "<option value='0'>Choose</option>";
                $.each(JSON.parse(data.JSONString), function (i, val) {
                    var template = '<tr group="{Group}" Id="{Id}">\
                                                <td data-title="Title">\
                                                    <div class="library-content-desc" style="position: relative;">\
                                                        <p>{Title}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="Description">\
                                                    <div class="library-content-desc" style="position: relative;">\
                                                        <p>{Description}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="URL">\
                                                    <div class="library-content-desc">\
                                                        <p>{URL}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="URL Note">\
                                                    <div class="library-content-desc">\
                                                        <p>{URLNote}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="Image">\
                                                    <div class="library-content-desc" style="position: relative;">\
                                                        <p style="padding: 2px;text-align:center;">{Attachment}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="Posted By">\
                                                    <div class="library-content-desc">\
                                                        <p>{Posted}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="Action">\
                                                    <div style="width: auto;display: inline-block;">\
                                                        <div class="Status-title-library">\
                                                            <p style="margin-top: 0;cursor: pointer;" class="edit">\
                                                                <i class="fa fa-pencil" aria-hidden="true"></i><span>Edit</span>\
                                                            </p>\
                                                        </div>\
                                                        <div class="Status-title-library">\
                                                            <p style="margin-top: 5px;cursor: pointer;" class="delete Delete">\
                                                                <i class="fa fa-trash-o" aria-hidden="true"></i><span>Delete</span>\
                                                            </p>\
                                                        </div>\
                                                    </div>\
                                                </td>\
                                            </tr>';
                   
                    if (val["UserId"] != data.UserId) {
                        template = '<tr group="{Group}" Id="{Id}">\
                                                <td data-title="Title">\
                                                    <div class="library-content-desc" style="position: relative;">\
                                                        <p>{Title}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="Description">\
                                                    <div class="library-content-desc" style="position: relative;">\
                                                        <p>{Description}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="URL">\
                                                    <div class="library-content-desc">\
                                                        <p>{URL}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="URL Note">\
                                                    <div class="library-content-desc">\
                                                        <p>{URLNote}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="Image">\
                                                    <div class="library-content-desc" style="position: relative;">\
                                                        <p style="padding: 2px;text-align:center;">{Attachment}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="Posted By">\
                                                    <div class="library-content-desc">\
                                                        <p>{Posted}</p>\
                                                    </div>\
                                                </td>\
                                                <td data-title="Action">\
                                                    <div style="width: auto;display: inline-block;">\
                                                    </div>\
                                                </td>\
                                            </tr>';
                    }

                    template = template.replace("{Id}", val["Id"]);
                    template = template.replace("{Group}", val["Group"]);
                    template = template.replace("{Title}", val["Title"]);
                    template = template.replace("{Description}", val["Description"] == "null" || val["Description"] == null ? "" : val["Description"]);
                    template = template.replace("{URL}", val["URL"]);
                    template = template.replace("{URLNote}", val["URLNote"]);
                    template = template.replace("{Attachment}", val["Attachment"]);
                    template = template.replace("{Posted}", val["Posted"]);
                    $('.library-table-head').append(template);
                });


            },
            error: function (data) {
                console.log('Error');
            }
        });
    }
});

