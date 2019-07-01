






function centerAuditReport() {


    var self = null;
    var _centerAuditReport = {

        classroomsJson: null,
        getReportUrl: HostedDir + '/Reporting/GetCenterAuditReport',
        getReportByCenterUrl: HostedDir + '/Reporting/GetCenterAuditReportByCenter',
        exportReportUrl: HostedDir + '/Reporting/ExportSubstituteRoleReport',
        getAttendanceDetailUrl: HostedDir + '/Roster/GetAttendenceByDate',
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

        bindfiltertext: '<div class="col-xs-12 glossy-panel-div table-back-shadow text-center" style="padding:25px;">\
                                                            <span class="col-xs-12">Search by Filter <sup style="color:red;">*</sup></span>\
                                                        </div>',
        init: function () {
            self = this;

            self.initializeElements();
            self.initializeEvents();
            self.resetElements();



            if (self.elements.dropdownCenter.find('option').length == 1) {

                self.elements.dropdownCenter.find('option:first-child').prop('selected', true)

                //   self.getClassrooms(self.elements.dropdownCenter.val());
            }
        },
        elements: {

            'divFilterSection': null,
            //  'searchStaffText': null,
            'dropdownCenter': null,
            'divCenterDropdown': null,
            'dropdownClassroom': null,
            'divdropdownClassroom': null,
            'buttonLoadReport': null,
            'buttonClearTeacher': null,
            'dropdownPageNumber': null,
            'dropdownRecordsPerpage': null,
            'divtableResponsive': null,
            'anchorPdf': null,
            'anchorExcel': null,
            'modalAttendanceDetails': null,
            'fromDateInput': null,
            'toDateInput': null,
            'buttonLoadAttendance': null,
            'modalbindReportDiv': null,
            'modaldivAttendanceSection': null

        },
        parametersMode: {
            'filter': 1,
            'get': 2,
            'modalGet': 3
        },
        getlistMode: {
            'all': 0,
            'center': 1,
            'modalGet': 2
        },
        pageChangeType: {
            'first': 1,
            'back': 2,
            'pageNo': 3,
            'next': 4,
            'last': 5,
            'displayPage': 6
        },
        ajaxOptions: {
            url: null,
            type: null,
            datatype: null,
            data: null,
            async: true
        },
        modalIndex: 'modal',

        reportFormatType: reportType,
        //  substituteRolePageType: substituteRoleModeType,
        dataParameters: dataModel,
        initializeElements: function () {

            self.elements.divFilterSection = $('#div-filter-section');
            self.elements.dropdownCenter = self.elements.divFilterSection.find('#selectCenter');
            self.elements.divCenterDropdown = self.elements.dropdownCenter.closest('.form-group');
            self.elements.buttonLoadReport = self.elements.divFilterSection.find('#buttonLoadReport');
            self.elements.divtableResponsive = $('#report-table-responsive');
            self.elements.modalAttendanceDetails = $('#modal-child-attendance');
            self.elements.anchorPdf = self.elements.modalAttendanceDetails.find('#pdfanchor');
            self.elements.buttonLoadAttendance = self.elements.modalAttendanceDetails.find('#btnLoadAttendance');
            self.elements.fromDateInput = self.elements.modalAttendanceDetails.find('#fromAttendanceDate');
            self.elements.toDateInput = self.elements.modalAttendanceDetails.find('#toAttendanceDate');
            self.elements.modalbindReportDiv = self.elements.modalAttendanceDetails.find('#attendance-details-section');
            self.elements.modaldivAttendanceSection = self.elements.modalAttendanceDetails.find('#div-attendance-section');


        },
        initializeEvents: function () {



            self.elements.buttonLoadReport.on('click', function () {

                if (self.validateFilterInputs()) {

                    self.showBusy(true);
                    window.setTimeout(function () {
                        self.getReport(null, null, self.getlistMode.all);
                    }, 10);

                }

            });



            self.elements.anchorPdf.on('click', function (e) {






                e.preventDefault();



                if (self.validateModalFilterInputs()) {
                    $('#formModalAttendance').find('input[name="Enc_ClientID"]').val(self.elements.modalAttendanceDetails.find('#lbl-popup-client').attr('accesskey'));
                    $('#formModalAttendance').find('input[name="reportFormatType"]').val(self.reportFormatType.Pdf);
                    $('#formModalAttendance').find('input[name="FromDate"]').val(new Date(self.elements.fromDateInput.val()).toJSON());

                    $('#formModalAttendance').find('input[name="ToDate"]').val(new Date(self.elements.toDateInput.val()).toJSON());

                    $('#formModalAttendance').submit();

                    //  location.href = HostedDir + '/Reporting/ExportCenterAuditReport/?fromDate="' + new Date(self.elements.fromDateInput.val()).toJSON() + '"&toDate="' + new Date(self.elements.toDateInput.val()).toJSON() + '"&reportFormatType="' + self.elements.modalAttendanceDetails.find('#lbl-popup-client').attr('accesskey') + '"&reportFormatType=' + self.reportFormatType.Pdf + '';

                }
                else {
                    return false;
                }




            });




            self.elements.buttonLoadAttendance.on('click', function () {



                if (self.validateModalFilterInputs()) {
                    self.showBusy(true);
                    window.setTimeout(function () {

                        self.getReport(self.elements.modaldivAttendanceSection, self.modalIndex, self.getlistMode.modalGet);

                    }, 10);

                }











            });

            self.elements.modalAttendanceDetails.on('hidden.bs.modal', function () {

                $(this).find('#lbl-popup-client,#lbl-popup-dob,#lbl-popup-center,#lbl-popup-classroom,#lbl-popup-dfs,#lbl-popup-enrollment-status,#lbl-popup-enroll-days,#lbl-popup-enroll-ada').html('');
                self.elements.fromDateInput.val('');
                self.elements.toDateInput.val('');
                self.elements.modalbindReportDiv.html(self.bindfiltertext);

                self.elements.modaldivAttendanceSection.find('#sortOrder_' + self.modalIndex).val('ASC');
                self.elements.modaldivAttendanceSection.find('#sortColumn_' + self.modalIndex).val('th1');
                self.elements.modaldivAttendanceSection.find('#requestedPage_' + self.modalIndex).val('1');
                self.elements.modaldivAttendanceSection.find('#startIndex_' + self.modalIndex).val('0');
                self.elements.modaldivAttendanceSection.find('#lastIndex_' + self.modalIndex).val('1');
                self.elements.modaldivAttendanceSection.find('#pageSize_' + self.modalIndex).val('10');
                self.elements.modaldivAttendanceSection.find('#pageLoadedFirst_' + self.modalIndex).val('0');
                self.elements.modaldivAttendanceSection.find('#numOfPages_' + self.modalIndex).val('0');


                self.elements.modaldivAttendanceSection.find('#ddlpaging_' + self.modalIndex).find('option').remove();
                self.elements.modaldivAttendanceSection.find('#ddlpagetodisplay_' + self.modalIndex).val(10);
                self.elements.modaldivAttendanceSection.find('#First_' + self.modalIndex).attr('disabled', true);
                self.elements.modaldivAttendanceSection.find('#Back_' + self.modalIndex).prop('disabled', true);
                self.elements.modaldivAttendanceSection.find('#Next_' + self.modalIndex).prop('disabled', true);
                self.elements.modaldivAttendanceSection.find('#Last_' + self.modalIndex).prop('disabled', true);








            });


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






            self.elements.dropdownCenter.val('0');



            self.dataParameters.SearchTerm = '';
        },

        bindAjaxParameters: function (mode, tabEle, index) {

            switch (mode) {
                case self.parametersMode.filter:

                    self.dataParameters.CenterID = self.elements.dropdownCenter.val().join(",");
                    self.dataParameters.SearchTerm = '';

                    break;
                case self.parametersMode.get:
                    self.dataParameters.CenterID = tabEle != null ? $('#myTab').find('li.active').children('a').attr('accesskey') : "0";
                    self.dataParameters.ClassroomID = "";

                    break;
                case self.parametersMode.modalGet:
                    self.dataParameters.Enc_ClientID = self.elements.modalAttendanceDetails.find('#lbl-popup-client').attr('accesskey');
                    self.dataParameters.FromDate = new Date(self.elements.fromDateInput.val()).toJSON();
                    self.dataParameters.ToDate = new Date(self.elements.toDateInput.val()).toJSON();
                    break;
            }

            self.dataParameters.RequestedPage = tabEle != null ? tabEle.find('#requestedPage_' + index + '').val() : 1;
            self.dataParameters.PageSize = tabEle != null ? tabEle.find('#pageSize_' + index + '').val() : 10;
            self.dataParameters.SortOrder = tabEle != null ? tabEle.find('#sortOrder_' + index + '').val() : "ASC";
            self.dataParameters.SortColumn = tabEle != null ? tabEle.find('#sortColumn_' + index + '').val() : "th1";
            self.dataParameters.SearchTerm = tabEle != null ? tabEle.find('#searchReportText').val() : "";


        },

        ajaxCall: function (callback) {


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
                contentType: "application/json; charset=utf-8",
                success: function (data) {

                    callback(data);

                },
                error: function (data) {

                    customAlert('Error occurred. Please, try again later.');
                },
                complete: function (data) {

                    self.showBusy(false);
                }

            })
        },


        showBusy: function (status) {
            if (status) {
                $('#spinner').show();
            }
            else {
                $('#spinner').hide();
            }
        },
        validateFilterInputs: function () {



            if (self.elements.dropdownCenter.val() == null || self.elements.dropdownCenter.val() == '' || self.elements.dropdownCenter.val() == '0') {
                customAlertforlongtime(_langList.Centerisrequired);
                plainValidation(self.elements.dropdownCenter);
                return false;
            }

            return true;


        },
        validateModalFilterInputs: function () {

            var $modal = $('#modal-child-attendance');


            if (self.elements.fromDateInput.val() == "" || self.elements.fromDateInput.val() == "__/__/____") {
                plainValidation(self.elements.fromDateInput);
                customAlert('From date is required');
                return false;
            }

            if (new Date(self.elements.fromDateInput.val()).setHours(0, 0, 0, 0) > new Date().setHours(0, 0, 0, 0)) {
                plainValidation(self.elements.fromDateInput);
                customAlert('From date should not exceed today\'s date');
                return false;
            }

            if (new Date(self.elements.fromDateInput.val()).setHours(0, 0, 0, 0) < new Date(new Date().setFullYear(new Date().getFullYear() - 1)).setHours(0, 0, 0, 0)) {
                plainValidation(self.elements.fromDateInput);
                customAlert('From date should not less than one year from today\'s date');
                return false;
            }




            if (self.elements.toDateInput.val() == "" || self.elements.toDateInput.val() == "__/__/____") {
                plainValidation(self.elements.toDateInput);
                customAlert('To date is required');
                return false;
            }


            if (new Date(self.elements.toDateInput.val()).setHours(0, 0, 0, 0) > new Date().setHours(0, 0, 0, 0)) {
                plainValidation(self.elements.toDateInput);
                customAlert('To date should not exceed today\'s date');
                return false;
            }

            if (new Date(self.elements.toDateInput.val()).setHours(0, 0, 0, 0) < new Date(new Date().setFullYear(new Date().getFullYear() - 1)).setHours(0, 0, 0, 0)) {
                plainValidation(self.elements.toDateInput);
                customAlert('To date should not less than one year from today\'s date');
                return false;
            }


            if (self.elements.fromDateInput.val() != "" && self.elements.toDateInput.val() != "") {

                if (new Date(self.elements.toDateInput.val()).setHours(0, 0, 0, 0) < new Date(self.elements.fromDateInput.val()).setHours(0, 0, 0, 0)) {
                    plainValidation(self.elements.toDateInput);
                    customAlert('To date should be greater or equal to from date');
                    return false;
                }
            }

            return true;
        },
        getReport: function (ele, index, isCenter) {


            self.ajaxOptions.datatype = 'html';
            self.ajaxOptions.type = 'POST';


            switch (isCenter) {
                case self.getlistMode.center:
                    self.bindAjaxParameters(self.parametersMode.get, ele, index);
                    self.ajaxOptions.url = self.getReportByCenterUrl;

                    break;
                case self.getlistMode.all:
                    self.bindAjaxParameters(self.parametersMode.filter, ele, index);
                    self.ajaxOptions.url = self.getReportUrl;

                    $('#myTab').html('');
                    break;
                case self.getlistMode.modalGet:
                    self.bindAjaxParameters(self.parametersMode.modalGet, ele, index);
                    self.ajaxOptions.url = self.getAttendanceDetailUrl;

                    break;
            }

            var $callback = self.getlistMode.modalGet == isCenter ? self.callbackGetAttendanceReport : self.callbackGetReport;
            self.ajaxOptions.data = JSON.stringify(self.dataParameters);
            self.ajaxOptions.async = false;
            self.ajaxCall($callback);


        },
        callbackGetReport: function (data) {
            if (data.Data == "Login") {
                customAlert("Session Ended Log Onto The System Again."); setTimeout(function () { window.location.href = HostedDir + '/login/Loginagency'; }, 2000);
            }
            else {

                var $activeTab = $('#myTab').find('li.active');
                var activeTabcontent = '';

                if ($activeTab.length > 0) {

                    var index = $activeTab.index();

                    activeTabcontent = $('#myTabContent').find('#' + index + 'tab');

                    $('#myTabContent').find('#div-table_' + index + '').html(data);

                    var $totalRecord = parseInt($('#myTabContent').find('#div-table_' + index + '').find('#totalRecordTable').val());
                    $('#myTabContent').find('#totalCountSpan_' + index + '').html($totalRecord);

                }
                else {



                    self.elements.divtableResponsive.html(data);

                    activeTabcontent = self.elements.divtableResponsive.find('#myTabContent').find('.tab-pane');

                    $('#myTab li').off('click').on('click', function () {


                        if (!$(this).hasClass('active')) {
                            self.dataParameters.SearchTerm = '';
                            var index = $(this).index();
                            var tabContent = $('#' + index + 'tab');

                            self.showBusy(true);


                            window.setTimeout(function () {
                                self.getReport(tabContent, index, self.getlistMode.center);
                            }, 10);
                        }






                    });

                }


                $.each(activeTabcontent, function (k, tabContent) {


                    var index = $(tabContent).attr('id').replace('tab', "").trim();
                    var $totalRecord = parseInt($(tabContent).find('#totalCountSpan_' + index + '').html());




                    $(tabContent).find('#btnSearchauto').off('click').on('click', function () {

                        cleanValidation();
                        //if ($(this).parent('div').siblings('#searchReportText').val() != null && $(this).parent('div').siblings('#searchReportText').val() != '')
                        //{
                        self.dataParameters.SearchTerm = $(this).parent('div').siblings('#searchReportText').val();

                        self.showBusy(true);

                        window.setTimeout(function () {

                            self.getReport($(tabContent), index, self.getlistMode.center);

                        }, 10);



                        //}
                        //else {
                        //    customAlert('Search term is required');
                        //    plainValidation($(this).parent('div').siblings('#searchReportText'));
                        //}

                    });

                    $(tabContent).find('#btnRemoveauto').off('click').on('click', function () {
                        cleanValidation();



                        var searchTextInput = $(this).parent('div').siblings('#searchReportText');
                        searchTextInput.val('');

                        self.dataParameters.SearchTerm = searchTextInput.val();

                        self.showBusy(true);

                        window.setTimeout(function () {

                            self.getReport($(tabContent), index, self.getlistMode.center);

                        }, 10);

                    });

                    $(tabContent).find('#btnRemoveauto').tooltip();


                    $.each($(tabContent).find('#table-report tbody tr'), function (i, tr) {



                        var gridTable = $(tr).parent('tbody').parent('#table-report');



                        gridTable.find('thead th').off('click').on('click', function () {


                            var th = this;
                            if ($(th).find('i').length > 0) {
                                $(tabContent).find('#sortColumn_' + index + '').val($(th).children('span').attr('col-name'));

                                if ($(th).find('i').is(':visible')) {
                                    if ($(th).find('.i-asc').is(':visible')) {

                                        $(tabContent).find('#sortOrder_' + index + '').val("DESC");
                                        $(th).find('.i-asc,.i-desc').toggle();
                                    }
                                    else if ($(th).find('.i-desc').is(':visible')) {

                                        $(tabContent).find('#sortOrder_' + index + '').val("ASC");


                                        $(th).find('.i-asc,.i-desc').toggle();
                                    }
                                }
                                else {
                                    $(tabContent).find('#sortOrder_' + index + '').val("ASC");
                                    $(th).find('.i-asc').show();
                                }

                                var $reqPage = $(tabContent).find('#div-pagination-' + index + '').find('#ddlpaging_' + index + '').val();
                                var $pageSize = $(tabContent).find('#div-pagination-' + index + '').find('#ddlpagetodisplay_' + index + '').val();
                                $(tabContent).find('#requestedPage_' + index + '').val($reqPage);
                                $(tabContent).find('#pageSize_' + index + '').val($pageSize);

                                //  self.requestedPage = self.elements.dropdownPageNumber.val() == null ? self.requestedPage : self.elements.dropdownPageNumber.val();

                                // self.pageSize = self.elements.dropdownRecordsPerpage.val();

                                var $tabContent = $('#myTabContent').find('.tab-pane.active');

                                var $tabindex = $($tabContent).attr('id').replace('tab', "").trim();



                                self.showBusy(true);

                                window.setTimeout(function () {

                                    self.getReport($tabContent, $tabindex, self.getlistMode.center);

                                }, 10);




                                // self.getUnscheduledClassDaysList();
                            }
                            else {
                                return false;
                            }



                        });


                        //$(tr).find('td:last').find('i.btn-delete-trans').off('click').on('click', function () {

                        //    self.removeSubstituteRole(this);

                        //});

                        $(tr).find('td.child-td').find('.anchor-child').off('click').on('click', function () {




                            self.getAttendanceDetailsPopup(this);

                        });
                    });






                    self.getTotalRecord($totalRecord, this, index);

                });





            }


            $('[data-toggle="tooltip"]').tooltip();
        },
        callbackGetAttendanceReport: function (data) {
          
            if (data.Data == "Login") {
                customAlert("Session Ended Log Onto The System Again."); setTimeout(function () { window.location.href = HostedDir + '/login/Loginagency'; }, 2000);
            }
            else {

                self.elements.modalbindReportDiv.html(data);

                $.each(self.elements.modalbindReportDiv.find('.sig-div'), function () {

                    var sigString = $(this).children('input:hidden').val();
                    $(this).signature();
                    $(this).signature('draw', sigString);
                    $(this).signature('disable');


                });

                $.each(self.elements.modalbindReportDiv.find('.kbw-signature'), function () {

                    $(this).css({ 'height': '94px', 'width': '260px', 'border': '1px solid #74777b' });
                    $(this).children('canvas').css({ 'height': '90px', 'width': '250px' });

                });

                var $totalRecord = self.elements.modaldivAttendanceSection.find('#totalCountSpan_' + self.modalIndex + '').html();

                self.getTotalRecord($totalRecord, self.elements.modaldivAttendanceSection, self.modalIndex);

            }
        },
        getTotalRecord: function (totalRecords, ele, index) {


            var paginationDiv = $(ele).find('#div-pagination-' + index + ''); //records per page div//
            var dropdownRecordsPage = paginationDiv.find('#ddlpagetodisplay_' + index + '');

            var pagingDiv = paginationDiv.find('#divPaging_' + index + ''); //pagination div//
            var firstButton = pagingDiv.find('#First_' + index + '');
            var backButton = pagingDiv.find('#Back_' + index + '');
            var dropdownPageNumber = pagingDiv.find('#ddlpaging_' + index + '');
            var nextButton = pagingDiv.find('#Next_' + index + '');
            var lastButton = pagingDiv.find('#Last_' + index + '');

            var _requestedPage = $(ele).find('#requestedPage_' + index + '');
            var _sortOrder = $(ele).find('#sortOrder_' + index + '');
            var _sortColumn = $(ele).find('#sortColumn_' + index + '');

            var indexClass = index == self.modalIndex ? '#' + self.elements.modaldivAttendanceSection.attr('id') : '.tab-pane';
            var _callback = index == self.modalIndex ? self.getlistMode.modalGet : self.getlistMode.center;

            firstButton.attr('disabled', false);
            backButton.attr('disabled', false);
            nextButton.attr('disabled', false);
            lastButton.attr('disabled', false);

            var _pageSize = parseInt(dropdownRecordsPage.val());

            if (totalRecords > 0) {



                if (totalRecords <= _pageSize) {


                    firstButton.attr('disabled', true);
                    backButton.attr('disabled', true);
                    nextButton.attr('disabled', true);
                    lastButton.attr('disabled', true);
                }

                var _numOfPages = parseInt(totalRecords / _pageSize) + ((totalRecords % _pageSize == 0) ? 0 : 1);

                dropdownPageNumber.empty();

                for (i = 1; i <= _numOfPages; i++) {

                    var newOption = "<option value='" + i + "'>" + i + "</option>";
                    dropdownPageNumber.append(newOption);
                }

                dropdownPageNumber.val(_requestedPage.val());
            }
            else {

                firstButton.attr('disabled', true);
                backButton.attr('disabled', true);
                nextButton.attr('disabled', true);
                lastButton.attr('disabled', true);
            }





            dropdownRecordsPage.off('change').on('change', function () {


           

                var index = $(this).attr('id').split('_')[1];

                var tabEle = $('#ddlpagetodisplay_' + index + '').closest('#div-pagination-' + index + '').closest(indexClass);



                $('#lastIndex_' + index + '').val(0);

                tabEle.find('#pageSize_' + index + '').val($(this).val());

                self.showBusy(true);

                window.setTimeout(function () {





                    self.getReport(tabEle, index, _callback);

                    firstButton.attr('disabled', true);
                    backButton.attr('disabled', true);
                }, 10);




            });

            firstButton.off('click').on('click', function () {

                if ($(this).attr('disabled') == "disabled")
                    return false;

                var index = $(this).attr('id').split('_')[1];
                var tabEle = $('#ddlpagetodisplay_' + index + '').closest('#div-pagination-' + index + '').closest(indexClass);


                self.fnChangePage(self.pageChangeType.first, tabEle, index);

            });

            backButton.off('click').on('click', function () {

                if ($(this).attr('disabled') == "disabled")
                    return false;

                var index = $(this).attr('id').split('_')[1];
                var tabEle = $('#ddlpagetodisplay_' + index + '').closest('#div-pagination-' + index + '').closest(indexClass);



                self.fnChangePage(self.pageChangeType.back, tabEle, index);

            });

            dropdownPageNumber.off('change').on('change', function () {

                self.showBusy(true);

                var index = $(this).attr('id').split('_')[1];
                var tabEle = $('#ddlpagetodisplay_' + index + '').closest('#div-pagination-' + index + '').closest(indexClass);

                tabEle.find('#requestedPage_' + index + '').val($(this).val());

                window.setTimeout(function () {

                    self.getListafterupdation(tabEle, index);

                }, 1)
            });

            nextButton.off('click').on('click', function () {

                if ($(this).attr('disabled') == "disabled")
                    return false;

                var index = $(this).attr('id').split('_')[1];
                var tabEle = $('#ddlpagetodisplay_' + index + '').closest('#div-pagination-' + index + '').closest(indexClass);

                self.fnChangePage(self.pageChangeType.next, tabEle, index);

            });

            lastButton.off('click').on('click', function () {
                if ($(this).attr('disabled') == "disabled")
                    return false;

                var index = $(this).attr('id').split('_')[1];
                var tabEle = $('#ddlpagetodisplay_' + index + '').closest('#div-pagination-' + index + '').closest(indexClass);

                self.fnChangePage(self.pageChangeType.last, tabEle, index);

            });

        },

        fnChangePage: function (val, tabEle, index) {

           

            self.showBusy(true);
            var _callback = index == self.modalIndex ? self.getlistMode.modalGet : self.getlistMode.center;

            window.setTimeout(function () {



                tabEle.find('#pageLoadedFirst_' + index + '').val(0);
                tabEle.find('#pageSize_' + index + '').val(tabEle.find('#ddlpagetodisplay_' + index + '').val());


                if (val == self.pageChangeType.first) {

                    tabEle.find('#startIndex_' + index + '').val(0);

                    var $lastindex = parseInt(parseInt(tabEle.find('#pageSize_' + index).val()) + (parseInt(tabEle.find('#lastIndex_' + index).val()) * parseInt(tabEle.find('#requestedPage_' + index + '').val())));

                    tabEle.find('#lastIndex_' + index + '').val($lastindex);

                    tabEle.find('#requestedPage_' + index + '').val(((parseInt(tabEle.find('#startIndex_' + index + '').val()) / 10) + 1))


                    self.getReport(tabEle, index, _callback);

                    tabEle.find('#First_' + index + '').attr('disabled', true);
                    tabEle.find('#Back_' + index + '').attr('disabled', true);
                    tabEle.find('#Next_' + index + '').attr('disabled', false);
                    tabEle.find('#Last_' + index + '').attr('disabled', false);

                    tabEle.find('#lastIndex_' + index + '').val(0);

                }
                else if (val == self.pageChangeType.last) {


                    var $startIndex = parseInt(parseInt((parseInt(tabEle.find('#totalCountSpan_' + index).html()) - 1) / parseInt(tabEle.find('#pageSize_' + index).val())) * parseInt(tabEle.find('#pageSize_' + index).val()));
                    tabEle.find('#startIndex_' + index + '').val($startIndex);


                    tabEle.find('#lastIndex_' + index + '').val(parseInt(tabEle.find('#totalCountSpan_' + index).html()))

                    tabEle.find('#requestedPage_' + index + '').val(tabEle.find('#ddlpaging_' + index + '').children('option:last-child').val());


                    //  self.gotoNextPage(self.requestedPage, self.pageSize);

                    self.getReport(tabEle, index, _callback);


                    tabEle.find('#First_' + index + '').attr('disabled', false);
                    tabEle.find('#Back_' + index + '').attr('disabled', false);
                    tabEle.find('#Next_' + index + '').attr('disabled', true);
                    tabEle.find('#Last_' + index + '').attr('disabled', true);

                }
                else if (val == self.pageChangeType.next) {

                    var $lastIndex = parseInt(parseInt(tabEle.find('#pageSize_' + index + '').val()) + parseInt(tabEle.find('#lastIndex_' + index + '').val()));

                    tabEle.find('#lastIndex_' + index + '').val($lastIndex)


                    var $reqpage = parseInt(((parseInt(tabEle.find('#lastIndex_' + index + '').val()) / parseInt(tabEle.find('#pageSize_' + index + '').val())) + 1));


                    //self.requestedPage = $reqpage;
                    tabEle.find('#requestedPage_' + index + '').val($reqpage);
                    //  self.gotoNextPage(self.requestedPage, self.pageSize);

                    self.getReport(tabEle, index, _callback);

                    tabEle.find('#First_' + index + '').attr('disabled', false);
                    tabEle.find('#Back_' + index + '').attr('disabled', false);

                    if (parseInt(tabEle.find('#lastIndex_' + index + '').val()) + parseInt(tabEle.find('#pageSize_' + index + '').val()) >= parseInt(tabEle.find('#totalCountSpan_' + index).html())) {

                        tabEle.find('#Next_' + index + '').attr('disabled', true);
                        tabEle.find('#Last_' + index + '').attr('disabled', true);
                    }
                    else if (parseInt(tabEle.find('#lastIndex_' + index + '').val()) - parseInt(tabEle.find('#pageSize_' + index + '').val()) < parseInt(tabEle.find('#totalCountSpan_' + index).html())) {

                        tabEle.find('#Next_' + index + '').attr('disabled', false);
                        tabEle.find('#Last_' + index + '').attr('disabled', false);
                    }
                }
                else if (val == self.pageChangeType.back) {


                    tabEle.find('#requestedPage_' + index + '').val((parseInt(tabEle.find('#requestedPage_' + index + '').val()) - 1));

                    tabEle.find('#lastIndex_' + index + '').val((parseInt(tabEle.find('#lastIndex_' + index + '').val()) - parseInt(tabEle.find('#pageSize_' + index + '').val())));


                    //   self.gotoNextPage(self.requestedPage, self.pageSize);

                    self.getReport(tabEle, index, _callback);

                    if (parseInt(tabEle.find('#lastIndex_' + index + '').val()) + parseInt(tabEle.find('#pageSize_' + index + '').val()) > parseInt(tabEle.find('#totalCountSpan_' + index).html())) {

                        tabEle.find('#Next_' + index + '').attr('disabled', true);
                        tabEle.find('#Last_' + index + '').attr('disabled', true);

                    }
                    else if (parseInt(tabEle.find('#lastIndex_' + index + '').val()) - parseInt(tabEle.find('#pageSize_' + index + '').val()) < parseInt(tabEle.find('#totalCountSpan_' + index).html())) {
                        tabEle.find('#Next_' + index + '').attr('disabled', false);
                        tabEle.find('#Last_' + index + '').attr('disabled', false);

                    }
                    if (parseInt(tabEle.find('#requestedPage_' + index + '').val()) == 1) {
                        tabEle.find('#First_' + index + '').attr('disabled', true);
                        tabEle.find('#Back_' + index + '').attr('disabled', true);
                    }
                }
                else {
                }

            }, 10)
        },
        gotoNextPage: function (ele, pageSize) {


            self.getReport();

        },



        getListafterupdation: function (ele, index) {

           

            var _callback = index == self.modalIndex ? self.getlistMode.modalGet : self.getlistMode.center;
            ele.find('#pageSize_' + index + '').val(ele.find('#ddlpagetodisplay_' + index + '').val())

            ele.find('#requestedPage_' + index + '').val(ele.find('#ddlpaging_' + index + '').val());

            var $startIndex = parseInt((parseInt(ele.find('#pageSize_' + index + '').val()) * (parseInt(ele.find('#requestedPage_' + index + '').val()) - 1)) + 1);

            ele.find('#startIndex_' + index + '').val($startIndex);

            var $lastIndex = parseInt((parseInt(ele.find('#pageSize_' + index + '').val()) * parseInt(ele.find('#requestedPage_' + index + '').val())) - parseInt(ele.find('#pageSize_' + index + '').val()));


            ele.find('#lastIndex_' + index + '').val($lastIndex);




            self.getReport(ele, index, _callback);

            //self.getReport();

            if (parseInt(ele.find('#requestedPage_' + index + '').val()) == 1) {
                ele.find('#First_' + index + '').attr('disabled', true);
                ele.find('#Back_' + index + '').attr('disabled', true);
                ele.find('#Next_' + index + '').attr('disabled', false);
                ele.find('#Last_' + index + '').attr('disabled', false);
            }
            else if (parseInt(ele.find('#requestedPage_' + index + '').val()) == parseInt(ele.find('#ddlpaging_' + index + '').children('option:last-child').val())) {

                ele.find('#First_' + index + '').attr('disabled', false);
                ele.find('#Back_' + index + '').attr('disabled', false);
                ele.find('#Next_' + index + '').attr('disabled', true);
                ele.find('#Last_' + index + '').attr('disabled', true);
            }
            else {
                ele.find('#First_' + index + '').attr('disabled', false);
                ele.find('#Back_' + index + '').attr('disabled', false);
                ele.find('#Next_' + index + '').attr('disabled', false);
                ele.find('#Last_' + index + '').attr('disabled', false);
            }




        },

        getAttendanceDetailsPopup: function (ele) {
         

            var $rowIndex = $(ele).closest('td').parent('tr').index();
            var $childId = $(ele).closest('.child-td').find('#data-child-id_' + $rowIndex + '').val();
            var $childName = $(ele).html();
            var $childDob = $(ele).closest('.child-td').find('#data-child-dob_' + $rowIndex + '').val();
            var $dateofService = $(ele).closest('.child-td').find('#data-child-dfs_' + $rowIndex + '').val();
            var $enrollstatus = $(ele).closest('.child-td').find('#data-child-enroll-status_' + $rowIndex + '').val();
            var $enrolldays = $(ele).closest('.child-td').find('#data-child-enroll-days_' + $rowIndex + '').val();
            var $adaPercentage = $(ele).closest('.child-td').find('#data-child-ada_' + $rowIndex + '').val();


            var $clsName = $(ele).closest('.child-td').find('#data-child-clsname_' + $rowIndex + '').val();
            var $centerName = self.elements.divtableResponsive.find('#myTab').find('li.active a').html();


            self.elements.modalAttendanceDetails.find('#lbl-popup-client').html($childName).attr('accesskey', $childId);
            self.elements.modalAttendanceDetails.find('#lbl-popup-dob').html($childDob);
            self.elements.modalAttendanceDetails.find('#lbl-popup-center').html($centerName);
            self.elements.modalAttendanceDetails.find('#lbl-popup-classroom').html($clsName);
            self.elements.modalAttendanceDetails.find('#lbl-popup-dfs').html($dateofService);


            var enrollstatus = '';
            switch ($enrollstatus) {
                case '1':
                    enrollstatus = '<span style="color:red;">Enrolled</span>';
                    break;
                case '3':
                    enrollstatus = '<span style="color:red;">Withdrawn</span>';
                    break;
                case '4':
                    enrollstatus = '<span style="color:red;">Re-enrolled</span>';
                    break;
                case '5':
                    enrollstatus = '<span style="color:red;">Transitioned</span>';
                    break;
                default:
                    enrollstatus = '<span style="color:green">Accepted</span>';
                    break;
            }



            self.elements.modalAttendanceDetails.find('#lbl-popup-enrollment-status').html(enrollstatus);
            self.elements.modalAttendanceDetails.find('#lbl-popup-enroll-days').html($enrolldays);
            self.elements.modalAttendanceDetails.find('#lbl-popup-enroll-ada').html($adaPercentage);





            self.elements.fromDateInput.datetimepicker('destroy');



            self.elements.fromDateInput.datetimepicker({
                timepicker: false,
                format: 'm/d/Y',
                validateOnBlur: false,
                mask: true,
                minDate: new Date(new Date().setFullYear(new Date().getFullYear() - 1)),
                maxDate: new Date(),
                onSelectDate: function (e, ele) {
                    //if (!self.isValidAllowedDates(e)) {

                    //    customAlert('Entered date is not allowed');
                    //    plainValidation(self.elements.modalinkindActivityDate);
                    //    return false;
                    //}

                },
                onChangeDateTime: function (e, ele) {

                    //if (!self.isValidAllowedDates(e)) {

                    //    customAlert('Entered date is not allowed');
                    //    plainValidation(self.elements.modalinkindActivityDate);
                    //    return false;
                    //}
                },

                validateOnBlur: true
            });


            self.elements.fromDateInput.siblings('.datepicker-icon').off('click').on('click', function () {

                self.elements.fromDateInput.datetimepicker('show');
            });

            self.elements.toDateInput.datetimepicker({
                timepicker: false,
                format: 'm/d/Y',
                validateOnBlur: false,
                mask: true,
                minDate: new Date(new Date().setFullYear(new Date().getFullYear() - 1)),
                maxDate: new Date(),
                onSelectDate: function (e, ele) {
                    //if (!self.isValidAllowedDates(e)) {

                    //    customAlert('Entered date is not allowed');
                    //    plainValidation(self.elements.modalinkindActivityDate);
                    //    return false;
                    //}

                },
                onChangeDateTime: function (e, ele) {

                    //if (!self.isValidAllowedDates(e)) {

                    //    customAlert('Entered date is not allowed');
                    //    plainValidation(self.elements.modalinkindActivityDate);
                    //    return false;
                    //}
                },

                validateOnBlur: true
            });
            self.elements.toDateInput.siblings('.datepicker-icon').off('click').on('click', function () {

                self.elements.toDateInput.datetimepicker('show');
            });

            self.elements.modalAttendanceDetails.modal('show');
        }

    }
    return _centerAuditReport;

}



$(function () {

    centerAuditReport().init();

});

