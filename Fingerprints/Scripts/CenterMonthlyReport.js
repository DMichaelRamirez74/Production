

var self = null;
var centerMonthly = {


    reportFormatType: reportType,
    init: function () {

        self = this;

        self.initializeElements();
        self.resetElements();
        self.initializeEvents();
    },

    reportPartialUrl: HostedDir + '/Reporting/GetFamilyActivityReport',
    //  getClassroomsUrl: HostedDir + '/Teacher/GetClassRoomsByCenterHistorical',
    // exportReportUrl: HostedDir + '/Screening/ExportScreeningReviewReport',
    // getscreeningUrl:HostedDir+'/Screening/GetScreeningByReportPeriods',
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
        'divClassroomDropdown': null,
        'dropdownClassroom': null,
        'dropdownMonths': null,
        'divMonthsDropdown': null,
        'inputsearchReport': null,
        'buttonSearch': null,
        'lblTotalCount': null,
        'divtableResponsive': null,
        'tableReport': null,
        'tabletheadReport': null,
        'tbodyReport': null,
        'buttonLoadReport': null,
        'divPaginationSection': null,
        'dropdownRecordsPerpage': null,
        'buttonPagingFirst': null,
        'buttonPagingBack': null,
        'buttonPagingNext': null,
        'buttonPagingLast': null,
        'dropdownPageNumber': null,
        'anchorPdf': null,
        'anchorExcel': null


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
    dataParameters: {
        CenterIDs: [],
        ClassroomIDs: [],
        ScreeningIDs: [],
        Months: [],
        SortOrder: null,
        SortColumn: null,
        PageSize: null,
        RequestedPage: null,
        SearchTerm: null,
        ScreeningReportPeriodID: null,
        EnrollmentStatus: null

    },

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


      //  $selfElements.divtableResponsive = $('#report-table-responsive');


        //$selfElements.inputsearchReport = $selfElements.divtableResponsive.find('#searchReportText');
        //$selfElements.lblTotalCount = $selfElements.divtableResponsive.find('#totalCountSpan');
        //$selfElements.buttonSearch = $selfElements.divtableResponsive.find('#btnSearchauto');

       // $selfElements.tableReport = $selfElements.divtableResponsive.find('#family-activity-table');
      //  $selfElements.tbodyReport = $selfElements.divtableResponsive.find('#family-activity-tbody');


     //   $selfElements.buttonLoadReport = $('#btnLoadFamilyActivity');
     //   $selfElements.tabletheadReport = $selfElements.tableReport.find('thead');

        //$selfElements.divPaginationSection = $('#divPaginationFamilyActivity');

        //$selfElements.dropdownRecordsPerpage = $('#ddlpagetodisplay');
        //$selfElements.buttonPagingFirst = $selfElements.divPaginationSection.find('#ulPaging').find('#First');
        //$selfElements.buttonPagingBack = $selfElements.divPaginationSection.find('#ulPaging').find('#Back');
        //$selfElements.buttonPagingNext = $selfElements.divPaginationSection.find('#ulPaging').find('#Next');
        //$selfElements.buttonPagingLast = $selfElements.divPaginationSection.find('#ulPaging').find('#Last');
        //$selfElements.dropdownPageNumber = $selfElements.divPaginationSection.find('#ddlpaging');
    },
    resetElements: function () {


        self.elements.dropdownCenter.multiselect({

            maxHeight: 200,
            includeSelectAllOption: true,
            enableFiltering: true,
            filterPlaceholder: _langList.SearchCenter,
            enableCaseInsensitiveFiltering: true,
            numberDisplayed: 1,


        });
        self.elements.divCenterDropdown.find('.multiselect').addClass('glossy-select').removeClass('btn btn-default');
        self.elements.divCenterDropdown.find('.btn-group').css({ 'width': '100%' });



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


        self.elements.dropdownMonths.multiselect({

            maxHeight: 200,
            includeSelectAllOption: true,
            enableFiltering: true,
            filterPlaceholder: _langList.SearchScreeningType,
            enableCaseInsensitiveFiltering: true,
            numberDisplayed: 1,


        });
        self.elements.divMonthsDropdown.find('.multiselect').addClass('glossy-select').removeClass('btn btn-default');
        self.elements.divMonthsDropdown.find('.btn-group').css({ 'width': '100%' });










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


        //$selfElements.buttonLoadReport.on('click', function () {

        //    self.dataParameters.SearchTerm = '';

        //    self.showBusy(true);

        //    window.setTimeout(function () {


        //        self.getReport();

        //    }, 10);

        //});


        //self.elements.dropdownRecordsPerpage.on('change', function () {

        //    self.lastIndex = 0;


        //    self.requestedPage = self.elements.dropdownPageNumber.val() == null ? self.requestedPage : self.elements.dropdownPageNumber.val();

        //    self.pageSize = self.elements.dropdownRecordsPerpage.val();

        //    self.showBusy(true);

        //    window.setTimeout(function(){


        //        self.getReport();
        //        self.elements.buttonPagingFirst.attr('disabled', true);
        //        self.elements.buttonPagingBack.attr('disabled', true);
        //    },10);




        //});

        //self.elements.buttonPagingFirst.on('click', function () {

        //    var $thisValue = 'First';

        //    self.requestedPage = self.elements.dropdownPageNumber.val() == null ? self.requestedPage : self.elements.dropdownPageNumber.val();

        //    self.fnChangePage($thisValue);

        //});

        //self.elements.buttonPagingBack.on('click', function () {

        //    var $thisValue = 'Back';

        //    self.requestedPage = self.elements.dropdownPageNumber.val() == null ? self.requestedPage : self.elements.dropdownPageNumber.val();

        //    self.fnChangePage($thisValue);

        //});

        //self.elements.dropdownPageNumber.on('change', function () {

        //    self.showBusy(true);

        //    window.setTimeout(function(){

        //        self.getListafterupdation();

        //    },1)
        //});

        //self.elements.buttonPagingNext.on('click', function () {

        //    var $thisValue = 'Next';

        //    self.requestedPage = self.elements.dropdownPageNumber.val() == null ? self.requestedPage : self.elements.dropdownPageNumber.val();

        //    self.fnChangePage($thisValue);

        //});

        //self.elements.buttonPagingLast.on('click', function () {

        //    var $thisValue = 'Last';

        //    self.requestedPage = self.elements.dropdownPageNumber.val() == null ? self.requestedPage : self.elements.dropdownPageNumber.val();

        //    self.fnChangePage($thisValue);

        //});


        self.elements.anchorPdf.on('click', function (e) {

            self.SearchTerm = '';




            e.preventDefault();



            if (self.validateReportFilter()) {
               // self.bindAjaxParameters();

                $('input[name="reportFormatType"]').val(self.reportFormatType.Pdf);
                $('#formReport').submit();

            }
            else {
                return false;
            }




        });


        self.elements.anchorExcel.on('click', function (e) {

            self.SearchTerm = '';


            e.preventDefault();



            if (self.validateReportFilter()) {
              //  self.bindAjaxParameters();

                $('input[name="reportFormatType"]').val(self.reportFormatType.Xls);
                $('#formReport').submit();

            }
            else {
                return false;
            }

        });




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



    //getReport: function () {

    //    if (self.validateReportFilter()) {

    //        self.bindAjaxParameters();
    //        self.ajaxOptions.url = self.reportPartialUrl;
    //        self.ajaxOptions.datatype = 'html',
    //        self.ajaxOptions.type = 'POST',
    //        self.ajaxOptions.data = JSON.stringify({ familyActivityReport: self.dataParameters });
    //        self.ajaxOptions.async = false;
    //        self.ajaxOptions.contentType = 'application/json; charset=utf-8';






    //        self.ajaxCall(self.bindReport);

    //    }
    //    else {

    //        self.showBusy(false);
    //    }


    //},
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
            customAlertforlongtime("Month is required");
            plainValidation(self.elements.dropdownMonths);
            result = false;
        }

        return result;


    },

    //bindAjaxParameters: function () {
    //    self.dataParameters.CenterIDs = self.elements.dropdownCenter.val();
    //    //   self.dataParameters.ClassroomIDs = self.elements.dropdownClassroom.closest('.form-group').is(':visible') ? self.elements.dropdownClassroom.val().join() : '0';

    //    //  self.dataParameters.RequestedPage = self.requestedPage;
    //    //self.dataParameters.PageSize = self.pageSize;
    //    self.dataParameters.Months = self.elements.dropdownMonths.val();
    //    self.dataParameters.SortOrder = self.sortOrder == null || self.sortOrder == '' ? 'ASC' : self.sortOrder;
    //    // self.dataParameters.SortColumn = self.sortColumn == null || self.sortColumn == '' ? self.elements.tableReport.find('thead').find('tr:eq(0)').find('th').children('span').html().trim() : self.sortColumn;
    //    self.dataParameters.SortColumn = "Center";
    //},

    //ajaxCall: function (callback) {

    //    console.log(self.ajaxOptions);
    //    $.ajax({
    //        url: self.ajaxOptions.url,
    //        type: self.ajaxOptions.type,
    //        dataType: self.ajaxOptions.datatype,
    //        beforeSend: function () {
    //            self.showBusy(true);
    //        },
    //        async: self.ajaxOptions.async,
    //        data: self.ajaxOptions.data,
    //        traditional: true,
    //        processData: self.ajaxOptions.processData,
    //        // contentType: "application/json; charset=utf-8",
    //        contentType: self.ajaxOptions.contentType,
    //        success: function (data) {
    //            callback(data);

    //        },
    //        error: function (data) {
    //            console.log(data);
    //            //   customAlert("Session Ended Log Onto The System Again."); setTimeout(function () { window.location.href = HostedDir + '/login/Loginagency'; }, 2000);

    //        },
    //        complete: function (data) {
    //            console.log(data);
    //            self.showBusy(false);
    //        }

    //    })
    //},

    //bindReport: function (data) {


    //    if (data != null && data.Data != null && data.Data == "Login") {
    //        // customAlert("Session Ended Log Onto The System Again."); setTimeout(function () { window.location.href = HostedDir + '/login/Loginagency'; }, 2000);
    //        return;
    //    }

    //    console.log(data);
    //    self.elements.divtableResponsive.html(data);



    //    self.initializeElements();

    //    //       self.getTotalRecord(parseInt(self.elements.lblTotalCount.html()));

    //    self.elements.tbodyReport.find('[data-toggle="tooltip"]').tooltip();




    //    //self.elements.buttonSearch.on('click', function () {


    //    //    if (self.elements.inputsearchReport.val() == null || self.elements.inputsearchReport.val() == '') {
    //    //        plainValidation(self.elements.inputsearchReport);
    //    //        customAlert(_langList.EnterTermToSearch);
    //    //        return false;
    //    //    }

    //    //    else {

    //    //        self.elements.dropdownPageNumber.val('1');
    //    //        self.requestedPage = self.elements.dropdownPageNumber.val();
    //    //        self.dataParameters.SearchTerm = self.elements.inputsearchReport.val();

    //    //        self.showBusy(true);
    //    //        window.setTimeout(function(){
    //    //            self.getReport();

    //    //        },10);
    //    //    }

    //    //});

    //    //self.elements.tableReport.find('thead th').on('click', function () {


    //    //    if ($(this).find('i').length > 0) {

    //    //        self.sortColumn = $(this).children('span').attr('data-column').trim();

    //    //        if ($(this).find('i').is(':visible')) {
    //    //            if ($(this).find('.i-asc').is(':visible')) {
    //    //                self.sortOrder = "DESC";
    //    //                $(this).find('.i-asc,.i-desc').toggle();
    //    //            }
    //    //            else if ($(this).find('.i-desc').is(':visible')) {
    //    //                self.sortOrder = "ASC";
    //    //                $(this).find('.i-asc,.i-desc').toggle();
    //    //            }
    //    //        }
    //    //        else {
    //    //            self.sortOrder = 'ASC';
    //    //            $(this).find('.i-asc').show();
    //    //        }


    //    //        self.requestedPage = self.elements.dropdownPageNumber.val() == null ? self.requestedPage : self.elements.dropdownPageNumber.val();

    //    //        self.pageSize = self.elements.dropdownRecordsPerpage.val();

    //    //        self.showBusy(true);

    //    //        window.setTimeout(function(){
    //    //            self.getReport();

    //    //        },10);


    //    //    }
    //    //    else {
    //    //        return false;
    //    //    }



    //    //});




    //},
    //fnChangePage: function (val) {

    //    if (self.elements.tbodyReport.find('tr').find('td[data-title="Center"]').length === 0) {

    //        return false;
    //    }




    //    self.showBusy(true);

    //    window.setTimeout(function () {

    //        var $selfElements = self.elements;

    //        self.pageLoadedFirst = false;

    //        self.pageSize = $selfElements.dropdownRecordsPerpage.val();

    //        if (val == 'First') {
    //            self.startIndex = 0;

    //            self.lastIndex = parseInt(self.pageSize) + parseInt(self.lastIndex * self.requestedPage);

    //            self.requestedPage = ((self.startIndex / 10) + 1);

    //            self.gotoNextPage(self.requestedPage, self.pageSize);

    //            $selfElements.buttonPagingFirst.attr('disabled', true);
    //            $selfElements.buttonPagingBack.attr('disabled', true);
    //            $selfElements.buttonPagingNext.attr('disabled', false);
    //            $selfElements.buttonPagingLast.attr('disabled', false);

    //            self.lastIndex = 0;
    //        }
    //        else if (val == 'Last') {

    //            self.startIndex = parseInt((self.totalRecords - 1) / self.pageSize) * self.pageSize;

    //            self.lastIndex = self.totalRecords;

    //            self.requestedPage = self.numOfPages;

    //            self.gotoNextPage(self.requestedPage, self.pageSize);


    //            $selfElements.buttonPagingFirst.attr('disabled', false);
    //            $selfElements.buttonPagingBack.attr('disabled', false);
    //            $selfElements.buttonPagingNext.attr('disabled', true);
    //            $selfElements.buttonPagingLast.attr('disabled', true);

    //        }
    //        else if (val == 'Next') {

    //            self.lastIndex = parseInt(self.pageSize) + parseInt(self.lastIndex);

    //            self.requestedPage = (parseInt(self.lastIndex / self.pageSize) + 1);

    //            self.gotoNextPage(self.requestedPage, self.pageSize);

    //            $selfElements.buttonPagingFirst.attr('disabled', false);
    //            $selfElements.buttonPagingBack.attr('disabled', false);

    //            if (parseInt(self.lastIndex) + parseInt(self.pageSize) >= self.totalRecords) {

    //                $selfElements.buttonPagingNext.attr('disabled', true);
    //                $selfElements.buttonPagingLast.attr('disabled', true);
    //            }
    //            else if (parseInt(self.lastIndex) - parseInt(self.pageSize) < self.totalRecords) {

    //                $selfElements.buttonPagingNext.attr('disabled', false);
    //                $selfElements.buttonPagingLast.attr('disabled', false);
    //            }
    //        }
    //        else if (val == 'Back') {


    //            self.requestedPage = self.requestedPage - 1;

    //            self.lastIndex = parseInt(self.lastIndex) - parseInt(self.pageSize);

    //            self.gotoNextPage(self.requestedPage, self.pageSize);

    //            if (parseInt(self.lastIndex) + parseInt(self.pageSize) > self.totalRecords) {

    //                $selfElements.buttonPagingNext.attr('disabled', true);
    //                $selfElements.buttonPagingLast.attr('disabled', true);

    //            }
    //            else if (parseInt(self.lastIndex) - parseInt(self.pageSize) < self.totalRecords) {
    //                $selfElements.buttonPagingNext.attr('disabled', false);
    //                $selfElements.buttonPagingLast.attr('disabled', false);

    //            }
    //            if (self.requestedPage == 1) {
    //                $selfElements.buttonPagingFirst.attr('disabled', true);
    //                $selfElements.buttonPagingBack.attr('disabled', true);
    //            }
    //        }
    //        else {
    //        }

    //    }, 10)
    //},
    //gotoNextPage: function (reqPage, pageSize) {


    //    self.getReport();

    //},

    //getTotalRecord: function (data) {
    //    self = this;

    //    var $selfElements = self.elements;

    //    $selfElements.buttonPagingFirst.attr('disabled', false);
    //    $selfElements.buttonPagingBack.attr('disabled', false);
    //    $selfElements.buttonPagingNext.attr('disabled', false);
    //    $selfElements.buttonPagingLast.attr('disabled', false);

    //    self.pageSize = $selfElements.dropdownRecordsPerpage.val();

    //    if (data > 0) {

    //        self.totalRecords = parseInt(data);

    //        if (self.totalRecords <= parseInt(self.pageSize)) {


    //            $selfElements.buttonPagingFirst.attr('disabled', true);
    //            $selfElements.buttonPagingBack.attr('disabled', true);
    //            $selfElements.buttonPagingNext.attr('disabled', true);
    //            $selfElements.buttonPagingLast.attr('disabled', true);

    //        }

    //        self.numOfPages = parseInt(self.totalRecords / self.pageSize) + ((self.totalRecords % self.pageSize == 0) ? 0 : 1);

    //        $selfElements.dropdownPageNumber.empty();

    //        for (i = 1; i <= self.numOfPages; i++) {

    //            var newOption = "<option value='" + i + "'>" + i + "</option>";
    //            $selfElements.dropdownPageNumber.append(newOption);
    //        }

    //        $selfElements.dropdownPageNumber.val(self.requestedPage);
    //    }
    //    else {
    //        $selfElements.buttonPagingFirst.attr('disabled', true);
    //        $selfElements.buttonPagingBack.attr('disabled', true);
    //        $selfElements.buttonPagingNext.attr('disabled', true);
    //        $selfElements.buttonPagingLast.attr('disabled', true);
    //    }
    //},

    //getListafterupdation: function () {
    //    self = this;

    //    self.pageSize = self.elements.dropdownRecordsPerpage.val();
    //    self.requestedPage = self.elements.dropdownPageNumber.val();

    //    self.startIndex = (self.pageSize * (self.requestedPage - 1)) + 1;
    //    self.lastIndex = parseInt(self.pageSize * self.requestedPage) - parseInt(self.pageSize);


    //    var $selfElements = self.elements;



    //    self.getReport();

    //    if (self.requestedPage == 1) {
    //        $selfElements.buttonPagingFirst.attr('disabled', true);
    //        $selfElements.buttonPagingBack.attr('disabled', true);
    //        $selfElements.buttonPagingNext.attr('disabled', false);
    //        $selfElements.buttonPagingLast.attr('disabled', false);
    //    }
    //    else if (self.requestedPage == self.numOfPages) {

    //        $selfElements.buttonPagingFirst.attr('disabled', false);
    //        $selfElements.buttonPagingBack.attr('disabled', false);
    //        $selfElements.buttonPagingNext.attr('disabled', true);
    //        $selfElements.buttonPagingLast.attr('disabled', true);
    //    }
    //    else {



    //        $selfElements.buttonPagingFirst.attr('disabled', false);
    //        $selfElements.buttonPagingBack.attr('disabled', false);
    //        $selfElements.buttonPagingNext.attr('disabled', false);
    //        $selfElements.buttonPagingLast.attr('disabled', false);

    //    }




    //},
}


$(function () {


    centerMonthly.init();






});

