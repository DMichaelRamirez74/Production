//!(function ($) {

//    function getRandomNumber() {
//        return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
//            (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
//        )

//    }

//    $.fn.ApplinkCaseNote = function (option, parameter, extraOptions) {
//        //debugger;
//        //return this.each(function () {
//        //    var data = $(this).data('casenote');
//        //    var options = typeof option === 'object' && option;

//        //    // Initialize the multiselect.
//        //    if (!data) {
//        //        data = new ApplinkCaseNote(this, options);
//        //        $(this).data('casenote', data);
//        //    }

//        //    // Call multiselect method.
//        //    if (typeof option === 'string') {
//        //        data[option](parameter, extraOptions);

//        //        if (option === 'destroy') {
//        //            $(this).data('casenote', false);
//        //        }
//        //    }
//        //});

//        return this;
//    }

//    $.fn.ApplinkCaseNote.Constructor = ApplinkCaseNote;





//    ApplinkCaseNote.prototype = {
//        defaults: {
//            isPopup: true,
//            isSection:false,
//            caseNoteType: 1,   //1- New/Edit 2- Append //
//            getCaseNoteUrl: null,
//            caseNoteId: null,
//            householdId: null,
//            clientId: null,

//            caseNoteValidation: function (events) {

//            },
//            caseNoteSubmit:function(events)
//            {

//            },
//            caseNoteClear:function(events)
//            {

//            },
//            caseNoteCancel:function(events)
//            {

//            },
//            addMoreAttachment: function (events) {

//            },
//            removeAttachment:function(events)
//            {

//            },



//        },
//        mergeOptions: function (options) {
//            return $.extend(true, {}, this.defaults, this.options, options);
//        },
//        getCaseNoteHtml:function(events,element)
//        {
//            var caseNoteUrl=''
//            if (events.isSection)
//                caseNoteUrl = '/Roster/GetCaseNoteSectionPartial';
//            else
//                caseNoteUrl = '/Roster/Getcasenotedetails';

//            $.ajax({
//                url: caseNoteUrl,
//                type: 'post',
//                dataType: 'html',
//                data: { casenoteId: events.defaults.casenoteId, householdId: events.defaults.householdId, clientId: events.defaults.clientId },
//                success: function (data) {
//                    element.html(data);
//                },
//                error: function (data) {

//                }
//            });
//        },
//        caseNoteDateElement: null,
//        caseNoteTitleElement: null,
//        caseNoteNotesElement: null,
//        caseNoteTagsElement: null,
//        caseNoteDivClientsElement: null,
//        caseNoteDivStaffsElement: null,
//        caseNoteSecurityCheckBox: null,
//        caseNoteAttachmentDiv: null,
//        caseNoteAdditonAttachmentDiv: null,
//        caseNoteImageGalleryDiv: null,
//        constructor:ApplinkCaseNote
//    }



//    function ApplinkCaseNote(element, options)
//    {
//        var $this = this;
//        this.element = $(element);
//        this.options = this.mergeOptions($.extend({}, options, this.element.data()));
//        //this.getCaseNoteHtml(this, this.element).done(function () {
//        //    var guid = '';

//        //    guid= $his.element.find('#divCaseNoteSection').attr('data-section-id');
//        //    $this.caseNoteDateElement = $this.element.find('#txtCaseNoteDate_' + guid + '');



//        //});

//    }








//}(JQuery));



!(function ($) {

    function getRandomNumber() {
        return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
            (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
        )

    }

    $.fn.ApplinkCaseNote = function (option, parameter, extraOptions) {  //[int],{PageSize:[int],RequestedPage:[int] },[function]

        var data = null;
        var options = typeof option === 'object' && option;


        data = $(this).data('applink-casenote');


        debugger;
        //  Initialize the multiselect.
        if (!data || option.forceLoad) {
            data = new ApplinkCaseNote(this, options);
            $(this).data('applink-casenote', data);


        }



        // Call multiselect method.
        if (typeof option === 'string') {
            var param = parameter != undefined && parameter != null && parameter != "" ? parameter : data;
            var result = null;

            try {

                result = data[option](param, extraOptions);

            }
            catch (error) {
                console.log(error);

                result = data.options[option](param, extraOptions);
            }

            if (option === 'destroy') {
                $(this).data('applink-pagination', false);
            }

        }


        if (option == "caseNoteValidation")
            return result;
        else
            return this;
    };
    $.fn.ApplinkCaseNote.Constructor = ApplinkCaseNote;


    ApplinkCaseNote.prototype = {

        defaults: {


            isPopup: true,
            isSection: false,
            caseNoteType: 1,   //1- New/2- Edit /3- Append //
            getCaseNoteUrl: null,
            caseNoteId: null,
            householdId: null,
            clientId: null,
            isEditable: true,
            isShowExportIcon: true,
            isShowSubmitButton: true,
            forceLoad: false,

            caseNoteValidation: function (events) {
             
                isValid = true;
                cleanValidation();
                var Clientcount = 0;
                var Tags = '';

                var $modal = null;

                if (events.options.isPopup) {
                    $modal = events.element.find('.modal');
                }
                else {
                    $modal = events.element;
                }



                $modal.find('#txtCaseNoteTags_' + events.guid + '_tagsinput .tag span').each(function () {
                    Tags = Tags + $(this).text().trim() + ',';
                });

                events.caseNoteTagsElement.val(Tags);

                events.caseNoteNotesElement.val(CKEDITOR.instances['txtareaCaseNote_' + events.guid + ''].getData());


                events.caseNoteDivClientsElement.find('input:checkbox:checked').each(function () {
                    if ($(this).prop("checked")) {
                        Clientcount = 1;
                    }
                });

                Tags = '';
                if (events.caseNoteDateElement.val().trim() == "") {
                    isValid = false;
                    customAlert("Case note date is required. ");
                    plainValidation(events.caseNoteDateElement);
                    return isValid;
                }
                else if (events.caseNoteTitleElement.val().trim() == "") {
                    isValid = false;
                    customAlert("Title is required.");
                    plainValidation(events.caseNoteTitleElement);
                    return isValid;
                }
                else if (CKEDITOR.instances['txtareaCaseNote_' + events.guid + ''].getData() == "") {
                    isValid = false;
                    customAlert("Note is required.");
                    return isValid;
                }
                else if (events.caseNoteTagsElement.val().trim() == "") {
                    isValid = false;
                    customAlert("Tags are required.");
                    plainValidation(events.caseNoteTagsElement);
                    return isValid;
                }

                else if (Clientcount == 0) {
                    isValid = false;
                    customAlert("Clients name is required.");
                    events.caseNoteDivClientsElement.focus();
                    return isValid;
                }










                return isValid;
            },
            saveCaseNote: function (events) {

                var caseNote = events.bindAjaxParameters(events);

                $.ajax({
                    type: "POST",
                    url: "/Roster/SaveCaseNote",
                    dataType: "json",
                    contentType: 'application/json; charset=utf-8',
                    beforeSend: function () {
                        $('#spinner').show();
                    },
                    data: JSON.stringify(caseNote),
                    success: function (data) {


                        events.options.successCaseNoteSave(data);

                    },
                    error: function (data) {
                        console.log(data);
                        customAlert('Error occurred. Please, try again later.')
                    },
                    complete: function (data) {
                        $('#spinner').hide();
                    }
                });
            },
            caseNoteClear: function (events) {

            },
            caseNoteCancel: function (events) {

            },
            addMoreAttachment: function (events) {

            },
            removeAttachment: function (events) {

            },

            successCaseNoteSave: function (data) {

            },
            modalOnShownEvent: function (element) {

            },
            
            ajaxComplete:function(element)
            {

            },
            buttonSaveOnClick: function (events) {
              
                if (this.options.caseNoteValidation(this)) {
                    this.options.saveCaseNote(this);

                }
            },
        },

        bindAjaxParameters: function (events) {
            var caseNote = {};
            var clientIDs = null;

            var clientArray = [];
            $.each(events.caseNoteDivClientsElement.find('input:checkbox:checked'), function () {

                clientArray.push($(this).val());
            });

            var staffArray = [];
            $.each(events.caseNoteDivStaffsElement.find('input:checkbox:checked'), function () {

                staffArray.push($(this).val());
            });


            var cameraDocumentsArray = [];


            this.caseNoteAttachmentDiv.find('input:file').each(function (a, b) {

                var fileInput = $(this);

                if (fileInput.val() != undefined && fileInput.val() != null && fileInput.val() != '') {
                    var fileUpload = fileInput.get(0);
                    var files = fileUpload.files;

                    for (var i = 0; i < files.length; i++) {


                        var convImage = fileInput.attr('conv-img');

                        if (convImage != null && convImage != "")
                            cameraDocumentsArray.push({ AttachmentFileName: files[i].name, AttachmentFileExtension: '.' + files[i].name.split('.')[files[i].name.split('.').length - 1], AttachmentJson: convImage });


                    }
                }
            });



            var $cameraDocuments = events.caseNoteImageGalleryDiv.find('.setup_viewscreen');



            if ($cameraDocuments.length > 0) {

                $.each($cameraDocuments, function (j, doc) {

                    var $doc = $(doc).find('img');
                    cameraDocumentsArray.push({ AttachmentFileName: 'CaseNoteDocument', AttachmentFileExtension: '.png', AttachmentJson: getBase64Image($doc) });

                });
            }



            caseNote.ClientId = events.hiddenClientId.val();
            caseNote.CenterId = events.hiddenCenterId.val();
            caseNote.HouseHoldId = events.hiddenHouseholdId.val();
            caseNote.CaseNoteid = events.hiddenCaseNoteId.val();
            caseNote.ProgramId = events.hiddenProgramId.val();
            caseNote.ClientIds = clientArray.join(',');
            caseNote.StaffIds = staffArray.join(',');
            caseNote.CaseNoteDate = events.caseNoteDateElement.val();
            caseNote.CaseNoteTitle = events.caseNoteTitleElement.val();
            caseNote.CaseNotetags = events.caseNoteTagsElement.val();
            caseNote.Note = events.caseNoteNotesElement.val();
            caseNote.CaseNoteSecurity = events.caseNoteSecurityCheckBox.is(':checked');
            caseNote.CaseNoteAttachmentList = cameraDocumentsArray;

            return caseNote;
        },
        constructor: ApplinkCaseNote,

        guid: null,

        mergeOptions: function (options) {
            return $.extend(true, {}, this.defaults, this.options, options);
        },
        getCaseNoteHtml: function (element) {
            var $this = this;

        
            //  var caseNoteUrl = ''
            //if (events.options.isSection)
            //    caseNoteUrl = '/Roster/GetCaseNoteSectionPartial';
            //else
            //    caseNoteUrl = '/Roster/Getcasenotedetails';

            if (this.options.caseNoteType == 1 || this.options.caseNoteType == 2)

                this.getAddOrEditCaseNoteHtml(element);
            else
                this.getAppendCaseNoteHtml(element);
        },
        getAddOrEditCaseNoteHtml: function (element) {
            var $this = this;
            $.ajax({
                url: $this.options.getCaseNoteUrl,
                type: 'post',
                dataType: 'html',
                async: true,
                beforeSend: function () {
                    $('#spinner').show();
                },
                data: { casenoteId: $this.options.caseNoteId, householdId: $this.options.householdId, clientId: $this.options.clientId },
                success: function (data) {


                    element.html('');

                    if (data != null) {
                        element.html(data);


                        element.find('#CaseNoteHeading').html("Add Case Note");

                        if ($this.options.caseNoteType == 2) {
                            element.find('#CaseNoteHeading').html("Edit Case Note");

                        }
                        else {
                            element.find('#heading_case_note').siblings('a').hide();
                        }


                        $this.guid = element.find('#divCaseNoteSection').attr('data-current-id');
                        var caseNoteSectionDiv = element.find('#divCaseNoteSection');
                        $this.caseNoteDateElement = caseNoteSectionDiv.find('#txtCaseNoteDate_' + $this.guid + '');
                        $this.caseNoteTitleElement = caseNoteSectionDiv.find('#txtCaseNoteTitle_' + $this.guid + '');
                        $this.caseNoteNotesElement = caseNoteSectionDiv.find('#txtareaCaseNote_' + $this.guid + '');
                        $this.caseNoteTagsElement = caseNoteSectionDiv.find('#txtCaseNoteTags_' + $this.guid + '');
                        $this.caseNoteDivClientsElement = caseNoteSectionDiv.find('#divCaseNoteClients_' + $this.guid + '');
                        $this.caseNoteDivStaffsElement = caseNoteSectionDiv.find('#divCaseNoteStaffs_' + $this.guid + '');
                        $this.caseNoteAttachmentDiv = caseNoteSectionDiv.find('#caseNoteAttachmentsDiv_' + $this.guid + '');
                        $this.caseNoteAdditonAttachmentDiv = caseNoteSectionDiv.find('#addAttachmentDiv_' + $this.guid + '');
                        $this.caseNoteImageGalleryDiv = caseNoteSectionDiv.find('.div-edit-gallery_' + $this.guid + '');
                        $this.caseNoteSecurityCheckBox = caseNoteSectionDiv.find('#checkboxSecureNote_' + $this.guid + '');
                        $this.hiddenCaseNoteId = caseNoteSectionDiv.find('#hdn-casenoteid_' + $this.guid + '');
                        $this.hiddenClientId = caseNoteSectionDiv.find('#hidden_clientId_' + $this.guid + '');
                        $this.hiddenCenterId = caseNoteSectionDiv.find('#hidden_centerId_' + $this.guid + '');
                        $this.hiddenClassroomId = caseNoteSectionDiv.find('#hidden_classroomId_' + $this.guid + '');
                        $this.hiddenHouseholdId = caseNoteSectionDiv.find('#hidden_householdId_' + $this.guid + '');
                        $this.hiddenProgramId = caseNoteSectionDiv.find('#hidden_programId_' + $this.guid + '');

                        $this.caseNoteDateElement.prop('disabled', false);

                        caseNoteSectionDiv.find('.sub-notes-details').find(".accordion-desc").fadeOut(0);

                        caseNoteSectionDiv.find('.sub-notes-details').find(".accordion").click(function () {



                            caseNoteSectionDiv.find('.sub-notes-details').find(".accordion-desc").not($(this).next()).slideUp('slow');
                            $(this).next().slideToggle(400);
                            $(this).toggleClass('active');

                            caseNoteSectionDiv.find('.sub-notes-details').find('.accordion').not($(this)).removeClass('active');

                        });



                        if (!$this.options.isEditable) {

                            caseNoteSectionDiv.find('[name="ClientIds.IDS"][type="checkbox"]').prop('disabled', true);
                            caseNoteSectionDiv.find('[name="TeamIds.IDS"][type="checkbox"]').prop('disabled', true);
                            caseNoteSectionDiv.find(".addn-poup-div1").find('[type="checkbox"]').prop('disabled', true);
                            caseNoteSectionDiv.find('#FirstFile[type="file"]').prop("disabled", true);
                            caseNoteSectionDiv.find(".add-attach-btn, #SaveNoteSubmit").css("display", "none");
                            caseNoteSectionDiv.find("#txtCaseNoteTags_tag").prop("disabled", true);

                        } else {
                            caseNoteSectionDiv.find('#FirstFile[type="file"]').prop("disabled", false);
                            caseNoteSectionDiv.find(".add-attach-btn, #SaveNoteSubmit").css("display", "inline-block");

                        }

                        if (caseNoteSectionDiv.find('#hdn-casenoteid_' + $this.guid + '').val() == "" || element.find('#hdn-casenoteid_' + $this.guid + '').val() == "0") {
                            element.find('#txtCaseNoteDate_' + $this.guid + '').val(getFormattedDateNumber(new Date()));
                        }

                        if (!$this.options.isShowExportIcon)
                            caseNoteSectionDiv.find('a.export-new').hide();

                        if (!$this.options.isShowSubmitButton)
                            caseNoteSectionDiv.find('.save-casenote').parent('div').hide();

                        $this.caseNoteSaveElement = caseNoteSectionDiv.find('.save_casenote_' + $this.guid + '');
                        $this.caseNoteSaveElement.removeAttr('onclick');

                        $this.caseNoteSaveElement.click($.proxy($this.options.buttonSaveOnClick, $this));

                        $('#spinner').hide();

                        if ($this.options.isPopup) {
                            caseNoteSectionDiv.closest('.modal').modal('show');
                        }

                       


                    }
                    else {
                        customAlert('Error occurred. Please, try again later');
                        $('#spinner').hide();
                    }
                },
                error: function (data) {
                    customAlert('Error occurred. Please, try again later');
                    $('#spinner').hide();
                },
                complete:function(data)
                {
                    $this.options.ajaxComplete($this.element);
                }
            });
        },

        getAppendCaseNoteHtml: function (element) {

            var $this = this;

            $.ajax({
                url: $this.options.isPopup? '/Roster/GetAppendCaseNotesPopup': '/Roster/GetAppendCaseNoteSection',
                dataType: 'html',
                type: 'post',
                success: function (data) {
                    if (data != null) {
                        
                        element.html(data);

                        $this.getCaseNotesByNote($this);
                    }
                }
            });

        },
        //saveCaseNoteOnClick: function (events) {

        //    if (this.options.caseNoteValidation(this)) {
        //        this.options.saveCaseNote(this);

        //    }
        //},

        getCaseNotesByNote: function (events) {
          
            var $this = this;
            var $modal = null;
            var $modalAppendcn=null;


            if( this.options.isPopup)
            {
                $modal = this.element.find('.modal');
                $modalAppendcn = $modal.find('#divAppendCaseNoteSection');

            }
               
            else
            {
                $modalAppendcn = this.element.find('#divAppendCaseNoteSection');
                $modal = $modalAppendcn;
            }
               

           
           

            $modalAppendcn.find('#hdn-casenoteid_' + $this.guid + '').val($this.options.caseNoteId);


            $.ajax({
                type: "POST",
                url: HostedDir + "/Roster/GetCaseNoteByNoteId",
                data: { 'NoteId': $this.options.caseNoteId },
                beforeSend: function () {
                    $('#spinner').show();
                },
                success: function (data) {
                    $modalAppendcn.find('.txt-main-note').empty();
                    var table = JSON.parse(data);

                    if (table != null && table.length > 0) {

                     
                        $this.guid = $modalAppendcn.attr('data-section-id');


                        $modalAppendcn.find('#hdn-casenoteid_' + $this.guid + '').val($this.options.caseNoteId);
                        $modalAppendcn.find('#txtAppendCaseNoteDate_' + $this.guid + '').val(getFormattedDateNumber(new Date(table[0].CurrentDate)));
                        $modalAppendcn.find('#lblCaseNoteTitle_' + $this.guid + '').html(table[0].Title);
                        $modalAppendcn.find('.txt-main-note').html(table[0].NoteField);
                        $modalAppendcn.find('#lblCaseNotetDate_' + $this.guid + '').html(getFormattedDate_Words(new Date(table[0].CaseNoteDate)) + '&nbsp;&nbsp;<i style="color:red;"> (Written on ' + table[0].ActualWrittenDateTime + ')</i>');
                        $modalAppendcn.find('#hidden_casenote_date_' + $this.guid + '').val(table[0].CaseNoteDate);
                        $modalAppendcn.find('#lblWrittenBy_' + $this.guid + '').html(table[0].StaffName);
                        $modalAppendcn.find('#lblWrittenByRole_' + $this.guid + '').html(table[0].RoleName);


                        $this.resetAppendCaseNoteSection($this, events.element);



                        $modalAppendcn.find('#hdn-householdid_' + $this.guid + '').val($this.options.householdId);
                        $modalAppendcn.find('#hdn-centerid_' + $this.guid + '').val($this.options.centerId);
                        $modalAppendcn.find('#hdn-classroomid_' + $this.guid + '').val(0);

                        $modalAppendcn.find('.img-camera').attr('data-guid', $this.guid);

                        $modalAppendcn.show();

                        $this.GetSubNotesByCaseNote($modal, $modalAppendcn.find('#hdn-casenoteid_' + $this.guid + '').val());

                     

                        $modalAppendcn.find('a.export-new').attr('href', '/Reporting/ExportCaseNote?caseNoteId=' + $this.options.caseNoteId + '&clientId=0&householdId=' + $('#HouseHoldId_casenote').val() + '');

                        $('#spinner').hide();

                      

                        if ($this.options.isPopup)
                        {
                            $modal.on('shown.bs.modal', function () {

                                $this.options.modalOnShownEvent(this);

                            });

                            $modal.modal('show');

                        }
                        else if ($this.options.isSection) {

                            $modalAppendcn.css('border', 'unset');

                            if (!$this.options.isShowExportIcon)
                            {
                                $this.element.find('.export-new').remove();


                            }
                            if (!$this.options.isShowSubmitButton)
                            {
                                $this.element.find('#btn_append_casenote_save').parent('div').remove();

                            }
                           

                          
                            $this.element.show();
                            $this.options.modalOnShownEvent($this.element);
                        

                           

                           

                        }



                    }




                },
                error: function (data) {
                    $('#spinner').hide();
                }

            });



        },
        GetSubNotesByCaseNote: function ($modal, id) {

            $.ajax({
                type: "POST",
                url: HostedDir + "/Roster/GetSubNotes",
                async: false,
                data: { 'CaseNoteId': id },

                success: function (data) {

                    $modal.find('.divAppendedNotes').show();
                    $modal.find('.sub-notes-details').empty();
                    console.clear();

                    if (data != null && data.length > 0) {
                        $(data).each(function (i, val) {
                           
                            var attachments = '';
                            var tagsName = '';
                            for (var i = 0; i < val.Attachment.length; i++) {
                                if (val.Attachment[i] != "0" && val.Attachment[i] != "")
                                    attachments += ("<a  title='Click here to download' href='" + HostedDir + "/Agencyuser/getpdfimage1/" + val.Attachment[i] + ",CaseNote,0'><i class='fa fa-download'></i></a>&nbsp;&nbsp;&nbsp;&nbsp;");

                            }
                            for (var i = 0; i < val.Tags.length; i++) {
                                if (val.Tags[i] != "0" && val.Tags[i] != "")
                                    tagsName += ('<span id="subTags" class="tag-text">' + val.Tags[i] + '</span>');

                            }
                            if (tagsName == "") {
                                tagsName = "<span>No tags available</span>";
                            }


                            var template = '<div class="accordion">\
                    <a href="javascript:void(0);">\
                        <h4>Posted on <span >{date}</span></h4>\
                    </a>\
                </div>';


                            template += '<div class="addn-poup-div accordion-desc sub-text-block" style="display:none;"> \
                          <table class="table" style="outline:1px solid #ddd;">\
                                <tbody>\
                                 <tr> \
                                <td><strong>Name of the Staff, Role</strong></td>\
                                <td> {name}, {RoleOfOwner}</td>\
                                </tr>\
                                 <tr>\
                                <tr> \
                                <td><strong>Note</strong></td>\
                                <td style="background-color:yellow;"> {notes}</td>\
                                </tr>\
                                 <tr>\
                                <td>\
                                <strong>Tags</strong></td>\
                                <td>\{SubTags} </td>\
                                </tr>\
                                <tr>\
                                <td><strong>Attachments</strong></td>\
                                <td>{SubAttach}</td>\
                                </tr>\
                            </tbody>\
                            </table>\
                             </div>';




                            template = template.replace('{date}', val.WrittenDate);
                            template = template.replace('{name}', val.Name);
                            template = template.replace('{RoleOfOwner}', val.RoleOfOwner);
                            template = template.replace('{notes}', val.Notes);
                            template = template.replace('{SubAttach}', attachments);
                            template = template.replace('{SubTags}', tagsName);
                            $modal.find('.sub-notes-details').append(template);
                        });
                    }
                    else {
                        var template = '<div class="addn-poup-div col-xs-12 no-padding">\
                                             <h4 style="margin-top:0;">\
                                                 N/A\
                                             </h4>\
                                             \
                                         </div>';
                        $modal.find('.sub-notes-details').append(template);
                    }



                    $modal.find('.sub-notes-details').find(".accordion-desc").fadeOut(0);

                    $modal.find('.sub-notes-details').find(".accordion").click(function () {

                        $modal.find('.sub-notes-details').find(".accordion-desc").not($(this).next()).slideUp('slow');
                        $(this).next().slideToggle(400);
                        $(this).toggleClass('active');
                        $modal.find('.sub-notes-details').find('.accordion').not($(this)).removeClass('active');

                    });

                },
                error: function (data) {

                }
            });
        },

        resetAppendCaseNoteSection: function (events, element) {

            element.find('#txtAppendCaseNoteDate_' + events.guid + '').val(getFormattedDateNumber(new Date()));
            CKEDITOR.instances['subnotes_caseNote_' + events.guid + ''].setData("");
            $('#AppendCaseNotetags_' + events.guid + '_tagsinput .tag ').each(function () {
                $(this).remove();
            });

            element.find('#addAttachmentDiv_AppendNote_' + events.guid + '').html('');
            element.find('#appendCaseNote_attachmentsDiv_' + events.guid + '').find('input:file').val('').removeAttr('conv-img');
            element.find('.div_append_image_gallery_' + events.guid + '').html('');

            element.find('#appendCaseNotetags_' + events.guid + '_tagsinput').find('.tag').remove();

        },
        caseNoteDateElement: null,
        caseNoteTitleElement: null,
        caseNoteNotesElement: null,
        caseNoteTagsElement: null,
        caseNoteDivClientsElement: null,
        caseNoteDivStaffsElement: null,
        caseNoteSaveElement: null,
        caseNoteSecurityCheckBox: null,
        caseNoteAttachmentDiv: null,
        caseNoteAdditonAttachmentDiv: null,
        caseNoteImageGalleryDiv: null,
        hiddenCaseNoteId: null,
        hiddenCenterId: null,
        hiddenClassroomId: null,
        hiddenProgramId: null,
        hiddenClientId: null,
        hiddenHouseholdId: null




    }

    function ApplinkCaseNote(element, options) {
        var $this = this;
        this.element = $(element);
        this.options = this.mergeOptions($.extend({}, options, this.element.data()));
        //this.getCaseNoteHtml(this, this.element);


        $.proxy(this.getCaseNoteHtml(this.element), this)

    }
}(jQuery));


