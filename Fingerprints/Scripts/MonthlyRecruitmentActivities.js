var self = null;
var monthlyRecruitment = {


    reportFormatType: null,
    init: function () {

        self = this;

        self.initializeElements();
        self.resetElements();
        self.initializeEvents();
    },

    activityOptionsPartialUrl: HostedDir + '/AgencyUser/GetMonthlyRecruitmentActivities',
    addActivityLookupUrl: HostedDir + '/AgencyUser/AddRecruitmentActivityLookup',
    addTransactionUrl: HostedDir + '/AgencyUser/AddRecruitmentActivityTransaction',
    getActivityLookupPartialUrl: HostedDir + '/AgencyUser/GetRecruitmentActivityLookup',
    removeActivityLookupUrl:HostedDir+'/AgencyUser/RemoveRecruitmentActivityLookup',
    requestedPage: 1,
    pageSize: 10,
    firstIndex: 0,
    startIndex: 0,
    lastIndex: 0,
    pageLoadedFirst: false,
    totalRecords: 0,
    numOfPages: 0,
    sortOrder: null,
    sortColumn: null,
    documentExtensionArray: [".pdf", ".jpg", ".jpeg", ".bmp", ".gif", ".png"],


    elements: {

        'dropdownCenter': null,
        'divCenterDropdown': null,
        'dropdownMonths': null,
        'divMonthsDropdown': null,
        'formAddActivity': null,
        //   'inputsearchReport': null,
        //  'buttonSearch': null,
        'lblTotalCount': null,
        //'divtableResponsive': null,
        //   'tableReport': null,
        //   'tabletheadReport': null,
        //   'tbodyReport': null,
        'buttonLoadActivities': null,
        'anchorAddMoreActivity': null,
        'btnAddActivity': null,
        'btnResetActivity': null,
        'divtextareagroup': null,
        'divActivityOption': null,
        'btnAddTransActivity': null,
        'btnResetTransActivity': null,
        'divBindOptionsResponsive': null,
        'divActivitiesGridResponsive': null,
        'divPaginationSection': null,
        'dropdownRecordsPerpage': null,
        'buttonPagingFirst': null,
        'buttonPagingBack': null,
        'buttonPagingNext': null,
        'buttonPagingLast': null,
        'dropdownPageNumber': null,
        'modalRecruitmentActivity': null,
        'textareamodalActivity': null,
        'btnModalSubmitActivity': null,
        'btnModalExitActivity': null,
        'hiddenModalactivityID':null
        //     'anchorPdf': null,
        //    'anchorExcel': null


    },
    ajaxOptions: {
        url: null,
        type: null,
        datatype: null,
        data: null,
        async: true,
        //  contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        contentType: "application/json; charset=utf-8",
        processData: true
    },
    //dataParameters: {
    //    CenterID: null,
    //   // ClassroomIDs: [],
    //   // ScreeningIDs: [],
    //    Month: null
    //  //  SortOrder: null,
    //  //  SortColumn: null,
    //  //  PageSize: null,
    //  //  RequestedPage: null,
    // //   SearchTerm: null,
    // //   ScreeningReportPeriodID: null,
    // //   EnrollmentStatus: null

    //},
    dataParameters: recruitmentActivityJson,

    showBusy: function (ele) {
        if (ele)
            $('#spinner').show();
        else
            $('#spinner').hide();
    },

    initializeElements: function () {

        var $selfElements = self.elements;

        $selfElements.divCenterDropdown = $('#div-center-dropdown');

        $selfElements.dropdownCenter = $selfElements.divCenterDropdown.find('#selectCenter');

        //$selfElements.dropdownClassroom = $('#selectClassroom');
        //$selfElements.divClassroomDropdown = self.elements.dropdownClassroom.closest('.form-group');
        $selfElements.dropdownMonths = $('#selectMonths');
        $selfElements.divMonthsDropdown = self.elements.dropdownMonths.closest('.form-group');


        $selfElements.anchorPdf = $('#pdfanchor');
        $selfElements.anchorExcel = $('#excelanchor');

        //  $selfElements.divtableResponsive = $('#activities-grid-responsive')

        $selfElements.divBindOptionsResponsive = $('#div-bind-partial');


        //$selfElements.inputsearchReport = $selfElements.divtableResponsive.find('#searchReportText');
        //$selfElements.buttonSearch = $selfElements.divtableResponsive.find('#btnSearchauto');

        // $selfElements.tableReport = $selfElements.divtableResponsive.find('#family-activity-table');
        //   $selfElements.tbodyReport = $selfElements.divtableResponsive.find('#family-activity-tbody');


        $selfElements.buttonLoadActivities = $('#btnLoadMonthlyRecruitment');
        //  $selfElements.tabletheadReport = $selfElements.tableReport.find('thead');

        $selfElements.anchorAddMoreActivity = $('#anchorAddMoreActivity');
        $selfElements.btnAddActivity = $('#btn-add-activity');
        $selfElements.btnResetActivity = $('#btn-reset-activity');
        $selfElements.divtextareagroup = $('#div-textarea-activity-group');
        $selfElements.divActivitiesGridResponsive = $('#activities-grid-responsive');
        $selfElements.lblTotalCount = $selfElements.divActivitiesGridResponsive.find('#totalCountSpan');
        $selfElements.formAddActivity = $('#formAddActivity');

        $selfElements.divPaginationSection = $('#div-pagination-recruitment-activities');
        $selfElements.dropdownRecordsPerpage = $('#ddlpagetodisplay');
        $selfElements.buttonPagingFirst = $selfElements.divPaginationSection.find('#ulPaging').find('#First');
        $selfElements.buttonPagingBack = $selfElements.divPaginationSection.find('#ulPaging').find('#Back');
        $selfElements.buttonPagingNext = $selfElements.divPaginationSection.find('#ulPaging').find('#Next');
        $selfElements.buttonPagingLast = $selfElements.divPaginationSection.find('#ulPaging').find('#Last');
        $selfElements.dropdownPageNumber = $selfElements.divPaginationSection.find('#ddlpaging');
        $selfElements.modalRecruitmentActivity = $('#modal-recruitment-activity');
        $selfElements.textareamodalActivity = $selfElements.modalRecruitmentActivity.find('#textarea-modal-activity');
        $selfElements.btnModalSubmitActivity = $selfElements.modalRecruitmentActivity.find('#btn-modal-submit');
        $selfElements.btnModalExitActivity = $selfElements.modalRecruitmentActivity.find('#btn-modal-cancel');
        $selfElements.hiddenModalactivityID=$selfElements.modalRecruitmentActivity.find('#hide-modal-act-id');

    },
    resetElements: function () {


        //self.elements.dropdownCenter.multiselect({

        //    maxHeight: 200,
        //    includeSelectAllOption: true,
        //    enableFiltering: true,
        //    filterPlaceholder: _langList.SearchCenter,
        //    enableCaseInsensitiveFiltering: true,
        //    numberDisplayed: 1,


        //});
        //self.elements.divCenterDropdown.find('.multiselect').addClass('glossy-select').removeClass('btn btn-default');
        //self.elements.divCenterDropdown.find('.btn-group').css({ 'width': '100%' });



        //self.elements.dropdownClassroom.multiselect({

        //    maxHeight: 200,
        //    includeSelectAllOption: true,
        //    enableFiltering: true,
        //    filterPlaceholder:_langList.SearchClassroom,
        //    enableCaseInsensitiveFiltering: true,
        //    numberDisplayed: 1,


        //});
        //self.elements.divClassroomDropdown.find('.multiselect').addClass('glossy-select').removeClass('btn btn-default');
        //self.elements.divClassroomDropdown.find('.btn-group').css({ 'width': '100%' });


        //self.elements.dropdownMonths.multiselect({

        //    maxHeight: 200,
        //    includeSelectAllOption: true,
        //    enableFiltering: true,
        //    filterPlaceholder: _langList.SearchScreeningType,
        //    enableCaseInsensitiveFiltering: true,
        //    numberDisplayed: 1,


        //});
        //self.elements.divMonthsDropdown.find('.multiselect').addClass('glossy-select').removeClass('btn btn-default');
        //self.elements.divMonthsDropdown.find('.btn-group').css({ 'width': '100%' });










    },
    initializeEvents: function () {


        var $selfElements = self.elements;




        //$selfElements.dropdownCenter.on('change', function (event) {

        //    var $centerId = $selfElements.dropdownCenter.val().join();

        //    if ($centerId != '0' && $selfElements.dropdownCenter.val().length == 1) {
        //        self.getClassrooms($centerId);

        //    }
        //    else {
        //        self.elements.dropdownClassroom.closest('.form-group').hide('slow');
        //    }
        //});


        $selfElements.buttonLoadActivities.on('click', function () {

            // self.dataParameters.SearchTerm = '';

            self.showBusy(true);

            window.setTimeout(function () {

                self.getMonthlyRecruitments();

                // self.getReport();

            }, 10);

        });

        $selfElements.anchorAddMoreActivity.on('click', function () {



            self.addMoreActivity(this);

        });

        $selfElements.btnAddActivity.on('click', function () {

            cleanValidation();
            if (self.elements.divtextareagroup.find('textarea').val() == '') {
                customAlertforlongtime(_langList.Atleast_one_activity_required);
                plainValidation(self.elements.divtextareagroup.find('textarea:eq(0)'));
                return false;
            }


            self.addActivity();



        });

        $selfElements.btnResetActivity.on('click', function () {

            self.resetActivityEntry();

        });


        self.elements.dropdownRecordsPerpage.on('change', function () {

            self.lastIndex = 0;


            self.requestedPage = self.elements.dropdownPageNumber.val() == null ? self.requestedPage : self.elements.dropdownPageNumber.val();

            self.pageSize = self.elements.dropdownRecordsPerpage.val();

            self.showBusy(true);

            window.setTimeout(function () {


                self.getReport();
                self.elements.buttonPagingFirst.attr('disabled', true);
                self.elements.buttonPagingBack.attr('disabled', true);
            }, 10);




        });

        self.elements.buttonPagingFirst.on('click', function () {

            var $thisValue = 'First';

            self.requestedPage = self.elements.dropdownPageNumber.val() == null ? self.requestedPage : self.elements.dropdownPageNumber.val();

            self.fnChangePage($thisValue);

        });

        self.elements.buttonPagingBack.on('click', function () {

            var $thisValue = 'Back';

            self.requestedPage = self.elements.dropdownPageNumber.val() == null ? self.requestedPage : self.elements.dropdownPageNumber.val();

            self.fnChangePage($thisValue);

        });

        self.elements.dropdownPageNumber.on('change', function () {

            self.showBusy(true);

            window.setTimeout(function () {

                self.getListafterupdation();

            }, 1)
        });

        self.elements.buttonPagingNext.on('click', function () {

            var $thisValue = 'Next';

            self.requestedPage = self.elements.dropdownPageNumber.val() == null ? self.requestedPage : self.elements.dropdownPageNumber.val();

            self.fnChangePage($thisValue);

        });

        self.elements.buttonPagingLast.on('click', function () {

            var $thisValue = 'Last';

            self.requestedPage = self.elements.dropdownPageNumber.val() == null ? self.requestedPage : self.elements.dropdownPageNumber.val();

            self.fnChangePage($thisValue);

        });

        self.elements.btnModalSubmitActivity.on('click', function () {

            cleanValidation();

            if(self.elements.textareamodalActivity.val()=='')
            {
                customAlertforlongtime(_langList.Atleast_one_edcuation_comp_required);
                plainValidation(self.elements.textareamodalActivity);
                return false;
            }
            else {


                self.ajaxOptions.url = self.addActivityLookupUrl;
                self.ajaxOptions.datatype = 'JSON';
                self.ajaxOptions.type = "POST";
                self.ajaxOptions.contentType = "application/json; charset=utf-8";
                self.ajaxOptions.async = true;
                self.dataParameters.RecruitmentActivityList = [{ "Description": self.elements.textareamodalActivity.val(), "RecruitmentActivityID": self.elements.hiddenModalactivityID.val(), "Status": true }];
                self.dataParameters.CenterID = "0";
                self.dataParameters.Month = 0;

                self.ajaxOptions.data = JSON.stringify({ 'activities': self.dataParameters });


                self.ajaxCall(self.callbackAddActivity);

            }


        });


        //self.elements.anchorPdf.on('click', function (e) {

        //    self.SearchTerm = '';




        //    e.preventDefault();



        //    if (self.validateReportFilter()) {
        //        self.bindAjaxParameters();

        //        $('input[name="reportFormatType"]').val(self.reportFormatType.Pdf);
        //        $('#formReport').submit();

        //    }
        //    else {
        //        return false;
        //    }




        //});


        //self.elements.anchorExcel.on('click', function (e) {

        //    self.SearchTerm = '';


        //    e.preventDefault();



        //    if (self.validateReportFilter()) {
        //        self.bindAjaxParameters();

        //        $('input[name="reportFormatType"]').val(self.reportFormatType.Xls);
        //        $('#formReport').submit();

        //    }
        //    else {
        //        return false;
        //    }

        //});




        if (self.elements.formAddActivity.length > 0 && self.elements.formAddActivity.is(':visible')) {
            self.getReport();
        }


    },

    addActivity: function () {
        var dataArray = [];

       
        $.each(self.elements.divtextareagroup.find('textarea'), function () {

            if ($(this).val().trim() != null && $(this).val().trim() != '') {

                dataArray.push({'Description':$(this).val().trim(),'RecruitmentActivityID':0,'Status':true});
            }
        });



        self.ajaxOptions.url = self.addActivityLookupUrl;
        self.ajaxOptions.datatype = 'JSON';
        self.ajaxOptions.type = "POST";
        self.ajaxOptions.contentType = "application/json; charset=utf-8";
        self.ajaxOptions.async = true;

        self.dataParameters.RecruitmentActivityList = dataArray;
        self.dataParameters.CenterID = "0";
        self.dataParameters.Month = 0;

        self.ajaxOptions.data = JSON.stringify({ 'activities': self.dataParameters });


        self.showBusy(true);

        self.ajaxCall(self.callbackAddActivity);



    },

    resetActivityEntry: function () {
        cleanValidation();
        self.elements.divtextareagroup.find('textarea').val('');
        self.elements.divtextareagroup.find('.adddivspace').remove();
    },

    callbackAddActivity: function (data) {
        if (data != null) {
            if (data) {
                self.resetActivityEntry();

                customAlertSuccess(_langList.RecordSavedSuccessfully);

                window.setTimeout(function () {

                    window.location.reload(true);

                }, 500);
            }
            else {
                customAlert(_langList.ErrrorOccurred);

            }
        }
        else {
            customAlert(_langList.ErrrorOccurred);

        }

    },

    callbackAddActivityTransaction: function (data) {
        if (data != null) {
            if (data) {

                self.elements.dropdownCenter.val(0);
                self.elements.dropdownMonths.val(0);
                self.elements.divBindOptionsResponsive.html('');
                customAlertSuccess(_langList.RecordSavedSuccessfully);

                if(self.elements.formAddActivity.length>0 && self.elements.formAddActivity.is(':visible'))
                {
                    self.showBusy(true);
                    window.setTimeout(function () {

                        window.location.reload(true);
                    },10)
                }

            }
            else {
                customAlert(_langList.ErrrorOccurred);

            }
        }
        else {
            customAlert(_langList.ErrrorOccurred);

        }
    },

    generatereport(data) {

    },

    //getClassrooms: function (cId) {

    //    self.ajaxOptions.url = self.getClassroomsUrl;
    //    self.ajaxOptions.datatype = 'JSON';
    //    self.ajaxOptions.type = 'POST',
    //    self.ajaxOptions.data = $.parseJSON(JSON.stringify({ 'centerId': cId }));
    //    self.ajaxOptions.async = true;

    //    $.ajax({
    //        url: self.getClassroomsUrl,
    //        type: 'POST',
    //        dataType: 'JSON',
    //        beforeSend: function () {
    //            self.showBusy(true);
    //        },
    //        async: true,
    //        data: { 'centerId': cId },
    //        success: function (data) {

    //            self.callbackGetClassrooms(data);

    //        },
    //        error: function (data) {

    //            console.log(data);
    //        },
    //        complete: function (data) {
    //            console.log(data);

    //            self.showBusy(false);
    //        }

    //    });

    //},
    //callbackGetClassrooms: function (data) {


    //    var bindData = '';
    //    if (data != null && data.CenterList != null && data.CenterList.length > 0 && data.CenterList[0].Classroom != null && data.CenterList[0].Classroom.length > 0) {

    //        self.classroomsJson = data.CenterList[0].Classroom;
    //        $.each(data.CenterList[0].Classroom, function (i, classroom) {

    //            bindData += '<option value=' + classroom.Enc_ClassRoomId + '>' + classroom.ClassName + '</option>';
    //        });
    //    }




    //    self.elements.dropdownClassroom.multiselect('destroy');

    //    self.elements.dropdownClassroom.html(bindData);

    //    self.elements.dropdownClassroom.multiselect({

    //        maxHeight: 200,
    //        includeSelectAllOption: true,
    //        enableFiltering: true,
    //        filterPlaceholder: 'Search Classroom',
    //        enableCaseInsensitiveFiltering: true,
    //        numberDisplayed: 1

    //    });
    //    self.elements.divClassroomDropdown.find('.multiselect').addClass('glossy-select').removeClass('btn btn-default');
    //    self.elements.divClassroomDropdown.find('.btn-group').css({ 'width': '100%' });

    //    self.elements.dropdownClassroom.closest('.form-group').show('slow');




    //},

    addMoreActivity: function (ele) {

        var appendText = '<div class="col-xs-12 col-sm-12 adddivspace">\
                                                    <textarea style="min-height:50px;line-height:normal;" class="col-xs-9"></textarea>\
                                                    <div class="col-xs-3 div-a-add">\
                                                        <a href="javascript:void(0)" id="Attachmectstag" class="a-add-activity" title=' + _langList.Add_another_recruitment_activity + '><i class="fa fa-minus-circle"></i>&nbsp;' + _langList.Remove + '</a>\
                                                    </div>\
                                                </div>';

        var anchorTargetParent = $(ele).parent('div').parent('div').parent('div');

        anchorTargetParent.append(appendText);

        anchorTargetParent.children('div.adddivspace:last').find('a').on('click', function () {

            $(this).parent('div').parent('div').remove();

        });


    },

    getMonthlyRecruitments: function () {
        if (self.validateReportFilter()) {
        
            self.bindAjaxParameters(self.ajaxCallMode.activityTransactions);
            self.ajaxOptions.url = self.activityOptionsPartialUrl;
            self.ajaxOptions.datatype = 'html',
            self.ajaxOptions.type = 'POST',
            self.ajaxOptions.data = JSON.stringify({ activities: self.dataParameters });
            self.ajaxOptions.async = false;
            self.ajaxOptions.contentType = 'application/json; charset=utf-8';






            self.ajaxCall(self.bindMonthlyRecruitments);

        }
        else {

            self.showBusy(false);
        }


    },

    getReport: function () {


        self.bindAjaxParameters(self.ajaxCallMode.activityLookup);
        self.ajaxOptions.url = self.getActivityLookupPartialUrl;
        self.ajaxOptions.datatype = 'html',
        self.ajaxOptions.type = 'POST',
        self.ajaxOptions.data = JSON.stringify({ activities: self.dataParameters });
        self.ajaxOptions.async = false;
        self.ajaxOptions.contentType = 'application/json; charset=utf-8';
        self.ajaxCall(self.bindReport);




    },
    validateReportFilter: function () {
        var result = true;

    


        if (self.elements.dropdownCenter.val() == null || self.elements.dropdownCenter.val() == '' || self.elements.dropdownCenter.val() == '0') {
            customAlertforlongtime(_langList.Centerisrequired);
            plainValidation(self.elements.dropdownCenter);
            result = false;
        }

            //else if (self.elements.dropdownCenter.val().length==1 &&( self.elements.dropdownClassroom.val() == null || self.elements.dropdownClassroom.val()=='' ||self.elements.dropdownClassroom.val()== '0')) {
            //    customAlertforlongtime(_langList.Classroomisrequired);
            //    plainValidation(self.elements.dropdownClassroom);
            //    result = false;
            //}

        else if (self.elements.dropdownMonths.val() == null || self.elements.dropdownMonths.val() == '' || self.elements.dropdownMonths.val() == '0') {
            customAlertforlongtime(_langList.MonthisRequired);
            plainValidation(self.elements.dropdownMonths);
            result = false;
        }

        return result;


    },

    ajaxCallMode: {
        'activityLookup': 1,
        'activityTransactions': 2
    },

    bindAjaxParameters: function (mode) {

        switch (mode) {
            case self.ajaxCallMode.activityLookup:

                self.dataParameters.RequestedPage = self.requestedPage == null || self.requestedPage == '' ? 1 : self.requestedPage;
                self.dataParameters.PageSize = self.pageSize == null || self.pageSize == '' ? 10 : self.pageSize;
                self.dataParameters.SortOrder = self.sortOrder;
                self.dataParameters.SortColumn = self.sortColumn

                break;

            case self.ajaxCallMode.activityTransactions:
                self.dataParameters.CenterID = self.elements.dropdownCenter.val();
                self.dataParameters.Month = self.elements.dropdownMonths.val();

                break;
        }

    },

    ajaxCall: function (callback) {

        console.log(self.ajaxOptions);
        $.ajax({
            url: self.ajaxOptions.url,
            type: self.ajaxOptions.type,
            dataType: self.ajaxOptions.datatype,
            beforeSend: function () {
                self.showBusy(true);
            },
            async: self.ajaxOptions.async,
            data: self.ajaxOptions.data,
            traditional: true,
            processData: self.ajaxOptions.processData,
            // contentType: "application/json; charset=utf-8",
            contentType: self.ajaxOptions.contentType,
            success: function (data) {
              
                callback(data);

            },
            error: function (data) {
              
                console.log(data);
                //   customAlert("Session Ended Log Onto The System Again."); setTimeout(function () { window.location.href = HostedDir + '/login/Loginagency'; }, 2000);

            },
            complete: function (data) {
                console.log(data);
              
                self.showBusy(false);
            }

        })
    },

    addActivityTransaction: function (ele) {
      
        if (self.elements.divActivityOption.find('input:checkbox:checked').length == 0) {
            customAlertforlongtime(_langList.Atleast_one_activity_required);
            self.elements.divActivityOption.find('input:checkbox:eq(0)').focus();
            return false;
        }

        var arrayActivity = [];

        $.each(self.elements.divActivityOption.find('input:checkbox:checked'), function (i, checkbox) {

            arrayActivity.push({ 'RecruitmentActivityID': $(checkbox).val(), 'Status': true });
        });



        self.bindAjaxParameters(self.ajaxCallMode.activityTransactions);

        self.dataParameters.RecruitmentActivityList = arrayActivity;

        self.dataParameters.ActivityTransactionID = $(ele).siblings('#hide-trans-id').val();

        console.log(self.dataParameters);

        self.ajaxOptions.url = self.addTransactionUrl;
        self.ajaxOptions.type = 'POST';
        self.ajaxOptions.datatype = "JSON";
        self.ajaxOptions.data = JSON.stringify({ 'activities': self.dataParameters });
        self.ajaxOptions.async = true;
        self.showBusy(true);
        self.ajaxCall(self.callbackAddActivityTransaction)


    },

    bindMonthlyRecruitments: function (data) {

        if (data != null && data.Data != null && data.Data == "Login") {
            customAlert("Session Ended Log Onto The System Again."); setTimeout(function () { window.location.href = HostedDir + '/login/Loginagency'; }, 2000);
            return;
        }

        self.elements.divBindOptionsResponsive.html(data);

        self.elements.divActivityOption = $('#activity-option-div');
        self.elements.btnAddTransActivity = $('#btn-add-trans-activity');
        self.elements.btnResetTransActivity = $('#btn-reset-trans-activity');

        self.elements.btnAddTransActivity.on('click', function () {
            self.addActivityTransaction(this);
        });


        self.elements.btnResetTransActivity.on('click', function () {
            cleanValidation();
            self.elements.divBindOptionsResponsive.html('');
            self.elements.dropdownCenter.val(0);
            self.elements.dropdownMonths.val(0);
        });
    },

    bindReport: function (data) {



        if (data != null && data.Data != null && data.Data == "Login") {
            customAlert("Session Ended Log Onto The System Again."); setTimeout(function () { window.location.href = HostedDir + '/login/Loginagency'; }, 2000);
            return;
        }
        else {
            self.elements.divActivitiesGridResponsive.html(data);

            var tBody = self.elements.divActivitiesGridResponsive.find('#monthly-recruitment-table').find('#recruitmentActivity-tbody');

            $.each(tBody.find('tr'), function () {

                $(this).find('td:eq(2)').find('.btn-edit-trans').on('click', function () {

                    self.editRecruitmentActivity(this);

                });

                $(this).find('td:eq(2)').find('.btn-delete-trans').on('click', function () {
                    self.deleteRecruitmentActivity(this);
                });
            });

        }





        self.initializeElements();



        self.getTotalRecord(parseInt(self.elements.lblTotalCount.html()));

        //  self.elements.tbodyReport.find('[data-toggle="tooltip"]').tooltip();




        //self.elements.buttonSearch.on('click', function () {


        //    if (self.elements.inputsearchReport.val() == null || self.elements.inputsearchReport.val() == '') {
        //        plainValidation(self.elements.inputsearchReport);
        //        customAlert(_langList.EnterTermToSearch);
        //        return false;
        //    }

        //    else {

        //        self.elements.dropdownPageNumber.val('1');
        //        self.requestedPage = self.elements.dropdownPageNumber.val();
        //        self.dataParameters.SearchTerm = self.elements.inputsearchReport.val();

        //        self.showBusy(true);
        //        window.setTimeout(function(){
        //            self.getReport();

        //        },10);
        //    }

        //});

        //self.elements.tableReport.find('thead th').on('click', function () {

      

        //    if ($(this).find('i').length > 0) {

        //        self.sortColumn = $(this).children('span').attr('data-column').trim();

        //        if ($(this).find('i').is(':visible')) {
        //            if ($(this).find('.i-asc').is(':visible')) {
        //                self.sortOrder = "DESC";
        //                $(this).find('.i-asc,.i-desc').toggle();
        //            }
        //            else if ($(this).find('.i-desc').is(':visible')) {
        //                self.sortOrder = "ASC";
        //                $(this).find('.i-asc,.i-desc').toggle();
        //            }
        //        }
        //        else {
        //            self.sortOrder = 'ASC';
        //            $(this).find('.i-asc').show();
        //        }


        //        self.requestedPage = self.elements.dropdownPageNumber.val() == null ? self.requestedPage : self.elements.dropdownPageNumber.val();

        //        self.pageSize = self.elements.dropdownRecordsPerpage.val();

        //        self.showBusy(true);

        //        window.setTimeout(function(){
        //            self.getReport();

        //        },10);


        //    }
        //    else {
        //        return false;
        //    }



        //});




    },

    editRecruitmentActivity: function (ele) {


        var $desc = $(ele).closest('td').closest('tr').find('td:eq(0)').find('p').html().trim();
        var $id = $(ele).siblings('#input-hide-act-id').val();

        self.elements.textareamodalActivity.html($desc);
        self.elements.hiddenModalactivityID.val($id);

        self.elements.modalRecruitmentActivity.modal('show');

    },
    deleteRecruitmentActivity: function (ele) {

        var $id = $(ele).siblings('#input-hide-act-id').val();
        var $desc = $(ele).closest('td').closest('tr').find('td:eq(0)').find('p').html().trim();

        BootstrapDialog.show({
            title:'Confirmation',
            message: '<p style="font-weight:bold;">'+_langList.You_about_to_delete_activity+'</p>\
                        <p><i>'+$desc+'</i></p>\
                        <p>'+_langList.Click_ok_to_proceed+'</p>',
            closable: true,
            closeByBackdrop: false,
            closeByKeyboard: false,
            buttons: [{
                label: ''+_langList.Cancel+' <span class="glossy-button-before"></span><span class="glossy-button-after"></span>',
                cssClass: 'glossy-button-button button-red',
                action: function (dialogRef) {
                    dialogRef.close(true);
                }
            }, {
                label: '' + _langList.Ok + ' <span class="glossy-button-before"></span><span class="glossy-button-after"></span>',
                cssClass: 'glossy-button-button button-green',
                autospin: true,
                action: function (dialogRef) {

                    self.dataParameters.RecruitmentActivityList = [{ "RecruitmentActivityID": $id, "Status": false }];
                    self.dataParameters.CenterID = 0;
                    self.dataParameters.Month = 0;
                    self.ajaxOptions.url = self.removeActivityLookupUrl;
                    self.ajaxOptions.type = 'POST';
                    self.ajaxOptions.datatype = "JSON";
                    self.ajaxOptions.async = true;
                    self.ajaxOptions.contentType = "application/json; charset=utf-8";
                    self.ajaxOptions.data = JSON.stringify({ 'activities': self.dataParameters });
                    //dialogRef.setClosable(false);

                    self.ajaxCall(self.callbackDeleteActivityLookup);

                }
            }]
        });


    },

    callbackDeleteActivityLookup:function(data)
    {
        if(data)
        {
            customAlertforlongtime(_langList.RecordSavedSuccessfully);
            $('.modal').modal('hide');
            
            self.elements.dropdownPageNumber.val(1);
            self.requestedPage = 1;
            self.pageSize = 10;
          
            self.showBusy(true);

            window.setTimeout(function () {

                self.getReport();

            }, 10);
            
        }
        else {
            customAlertForlongtime(_langList.ErrrorOccurred);
        }
    },

    fnChangePage: function (val) {


        self.showBusy(true);

        window.setTimeout(function () {

            var $selfElements = self.elements;

            self.pageLoadedFirst = false;

            self.pageSize = $selfElements.dropdownRecordsPerpage.val();

            if (val == 'First') {
                self.startIndex = 0;

                self.lastIndex = parseInt(self.pageSize) + parseInt(self.lastIndex * self.requestedPage);

                self.requestedPage = ((self.startIndex / 10) + 1);

                self.gotoNextPage(self.requestedPage, self.pageSize);

                $selfElements.buttonPagingFirst.attr('disabled', true);
                $selfElements.buttonPagingBack.attr('disabled', true);
                $selfElements.buttonPagingNext.attr('disabled', false);
                $selfElements.buttonPagingLast.attr('disabled', false);

                self.lastIndex = 0;
            }
            else if (val == 'Last') {

                self.startIndex = parseInt((self.totalRecords - 1) / self.pageSize) * self.pageSize;

                self.lastIndex = self.totalRecords;

                self.requestedPage = self.numOfPages;

                self.gotoNextPage(self.requestedPage, self.pageSize);


                $selfElements.buttonPagingFirst.attr('disabled', false);
                $selfElements.buttonPagingBack.attr('disabled', false);
                $selfElements.buttonPagingNext.attr('disabled', true);
                $selfElements.buttonPagingLast.attr('disabled', true);

            }
            else if (val == 'Next') {

              
                self.lastIndex = parseInt(self.pageSize) + parseInt(self.lastIndex);

                self.requestedPage = (parseInt(self.lastIndex / self.pageSize) + 1);

                self.gotoNextPage(self.requestedPage, self.pageSize);

                $selfElements.buttonPagingFirst.attr('disabled', false);
                $selfElements.buttonPagingBack.attr('disabled', false);

                if (parseInt(self.lastIndex) + parseInt(self.pageSize) >= self.totalRecords) {

                    $selfElements.buttonPagingNext.attr('disabled', true);
                    $selfElements.buttonPagingLast.attr('disabled', true);
                }
                else if (parseInt(self.lastIndex) - parseInt(self.pageSize) < self.totalRecords) {

                    $selfElements.buttonPagingNext.attr('disabled', false);
                    $selfElements.buttonPagingLast.attr('disabled', false);
                }
            }
            else if (val == 'Back') {


                self.requestedPage = self.requestedPage - 1;

                self.lastIndex = parseInt(self.lastIndex) - parseInt(self.pageSize);

                self.gotoNextPage(self.requestedPage, self.pageSize);

                if (parseInt(self.lastIndex) + parseInt(self.pageSize) > self.totalRecords) {

                    $selfElements.buttonPagingNext.attr('disabled', true);
                    $selfElements.buttonPagingLast.attr('disabled', true);

                }
                else if (parseInt(self.lastIndex) - parseInt(self.pageSize) < self.totalRecords) {
                    $selfElements.buttonPagingNext.attr('disabled', false);
                    $selfElements.buttonPagingLast.attr('disabled', false);

                }
                if (self.requestedPage == 1) {
                    $selfElements.buttonPagingFirst.attr('disabled', true);
                    $selfElements.buttonPagingBack.attr('disabled', true);
                }
            }
            else {
            }

        }, 10)
    },
    gotoNextPage: function (reqPage, pageSize) {


        self.getReport();

    },

    getTotalRecord: function (data) {
       
        self = this;

        var $selfElements = self.elements;

        $selfElements.buttonPagingFirst.attr('disabled', false);
        $selfElements.buttonPagingBack.attr('disabled', false);
        $selfElements.buttonPagingNext.attr('disabled', false);
        $selfElements.buttonPagingLast.attr('disabled', false);

        self.pageSize = $selfElements.dropdownRecordsPerpage.val();

        if (data > 0) {

            self.totalRecords = parseInt(data);

            if (self.totalRecords <= parseInt(self.pageSize)) {


                $selfElements.buttonPagingFirst.attr('disabled', true);
                $selfElements.buttonPagingBack.attr('disabled', true);
                $selfElements.buttonPagingNext.attr('disabled', true);
                $selfElements.buttonPagingLast.attr('disabled', true);

            }

            self.numOfPages = parseInt(self.totalRecords / self.pageSize) + ((self.totalRecords % self.pageSize == 0) ? 0 : 1);

            $selfElements.dropdownPageNumber.empty();

            for (i = 1; i <= self.numOfPages; i++) {

                var newOption = "<option value='" + i + "'>" + i + "</option>";
                $selfElements.dropdownPageNumber.append(newOption);
            }

            $selfElements.dropdownPageNumber.val(self.requestedPage);
        }
        else {
            $selfElements.buttonPagingFirst.attr('disabled', true);
            $selfElements.buttonPagingBack.attr('disabled', true);
            $selfElements.buttonPagingNext.attr('disabled', true);
            $selfElements.buttonPagingLast.attr('disabled', true);
        }
    },

    getListafterupdation: function () {
        self = this;

        self.pageSize = self.elements.dropdownRecordsPerpage.val();
        self.requestedPage = self.elements.dropdownPageNumber.val();

        self.startIndex = (self.pageSize * (self.requestedPage - 1)) + 1;
        self.lastIndex = parseInt(self.pageSize * self.requestedPage) - parseInt(self.pageSize);


        var $selfElements = self.elements;



        self.getReport();

        if (self.requestedPage == 1) {
            $selfElements.buttonPagingFirst.attr('disabled', true);
            $selfElements.buttonPagingBack.attr('disabled', true);
            $selfElements.buttonPagingNext.attr('disabled', false);
            $selfElements.buttonPagingLast.attr('disabled', false);
        }
        else if (self.requestedPage == self.numOfPages) {

            $selfElements.buttonPagingFirst.attr('disabled', false);
            $selfElements.buttonPagingBack.attr('disabled', false);
            $selfElements.buttonPagingNext.attr('disabled', true);
            $selfElements.buttonPagingLast.attr('disabled', true);
        }
        else {



            $selfElements.buttonPagingFirst.attr('disabled', false);
            $selfElements.buttonPagingBack.attr('disabled', false);
            $selfElements.buttonPagingNext.attr('disabled', false);
            $selfElements.buttonPagingLast.attr('disabled', false);

        }




    },
}


$(function () {


    monthlyRecruitment.init();






});