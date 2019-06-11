


$.fn.isBound = function (type, fn) {
    var data = this.data('events')[type];

    if (data === undefined || data.length === 0) {
        return false;
    }

    return (-1 !== $.inArray(fn, data));
};



function ufcReport()
{


    var self = null;
    var _ufcReport = {

        classroomsJson: null,
        // addSubstituteRoleUrl: HostedDir + '/AgencyUser/AddSubstituteRole',
        getUFCReportUrl: HostedDir + '/Reporting/GetUFCReport',
        getUFCReportByCenterUrl: HostedDir + '/Reporting/GetUFCReportByCenter',
        removeSubstituteRoleUrl: HostedDir + '/AgencyUser/RemoveSubstituteRole',
        getClassroomsUrl: HostedDir + '/Teacher/GetClassRoomsByCenterHistorical',
        exportReportUrl: HostedDir + '/Reporting/ExportSubstituteRoleReport',
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
            //  'fromDateInput': null,
            // 'toDateInput': null,
            'buttonLoadReport': null,
            'buttonClearTeacher': null,
            'dropdownPageNumber': null,
            'dropdownRecordsPerpage': null,
            'divtableResponsive': null,
            'dropdownMonths': null,
            'divMonthsDropdown': null,
            'anchorPdf': null,
            'anchorExcel': null


        },
        parametersMode: {
            'filter': 1,
            'get': 2
        },
        getlistMode: {
            'all': 0,
            'center': 1
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

        reportFormatType: reportType,
      //  substituteRolePageType: substituteRoleModeType,
        dataParameters: dataModel,
        initializeElements: function () {
            self.elements.divFilterSection = $('#div-filter-section');
            self.elements.dropdownCenter = self.elements.divFilterSection.find('#selectCenter');
            self.elements.divCenterDropdown = self.elements.dropdownCenter.closest('.form-group');
           // self.elements.dropdownClassroom = self.elements.divFilterSection.find('#selectClassroom');
          //  self.elements.divdropdownClassroom = self.elements.dropdownClassroom.closest('.form-group');
            self.elements.buttonLoadReport = self.elements.divFilterSection.find('#buttonLoadReport');
           // self.elements.buttonClearTeacher = self.elements.divFilterSection.find('#btnClearTeacher');
            self.elements.divtableResponsive = $('#report-table-responsive');
            self.elements.dropdownMonths = $('#selectMonths');
            self.elements.divMonthsDropdown = self.elements.dropdownMonths.closest('.form-group');
            self.elements.anchorPdf = self.elements.divFilterSection.find('#pdfanchor');
            self.elements.anchorExcel = self.elements.divFilterSection.find('#excelanchor');

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

            //self.elements.buttonClearTeacher.on('click', function () {

            //    self.resetElements();
            //});


            self.elements.anchorPdf.on('click', function (e) {

                // self.SearchTerm = '';




                e.preventDefault();



                if (self.validateFilterInputs()) {

                    $('#formReport').find('input[name="CenterID"]').val(self.elements.dropdownCenter.val().join(','));
                    $('#formReport').find('input[name="MonthType"]').val(self.elements.dropdownMonths.val().join(','));
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


              
                if (self.validateFilterInputs()) {
                    //  self.bindAjaxParameters();
                    $('#formReport').find('input[name="CenterID"]').val(self.elements.dropdownCenter.val().join(','));
                    $('#formReport').find('input[name="MonthType"]').val(self.elements.dropdownMonths.val().join(','));
                    $('input[name="reportFormatType"]').val(self.reportFormatType.Xls);
                    
                    $('#formReport').submit();

                }
                else {
                    return false;
                }

            });



            // self.getSubsituteRole(null, null, self.getlistMode.all);
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






            self.elements.dropdownCenter.val('0');



            self.dataParameters.SearchTerm = '';
        },

        bindAjaxParameters: function (mode, tabEle, index) {
            debugger
            switch (mode) {
                case self.parametersMode.filter:

                    self.dataParameters.CenterID = self.elements.dropdownCenter.val().join(",");
                    //self.dataParameters.SubstituteID = 0;
                    self.dataParameters.SearchTerm = '';
                    self.dataParameters.MonthType = self.elements.dropdownMonths.val().join(",");

                    break;
                case self.parametersMode.get:
                    self.dataParameters.CenterID = tabEle != null ? $('#myTab').find('li.active').children('a').attr('accesskey') : "0";
                    self.dataParameters.ClassroomID = "";
                    //self.dataParameters.RequestedPage = tabEle != null ? tabEle.find('#requestedPage_' + index + '').val() : 1;
                    //self.dataParameters.PageSize = tabEle != null ? tabEle.find('#pageSize_' + index + '').val() : 10;
                    //self.dataParameters.SortOrder = tabEle != null ? tabEle.find('#sortOrder_' + index + '').val() : "ASC";
                    //self.dataParameters.SortColumn = tabEle != null ? tabEle.find('#sortColumn_' + index + '').val() : "Classroom";
                    //self.dataParameters.SearchTerm = tabEle != null ? tabEle.find('#searchReportText').val() : "";

                    break;
            }

            self.dataParameters.RequestedPage = tabEle != null ? tabEle.find('#requestedPage_' + index + '').val() : 1;
            self.dataParameters.PageSize = tabEle != null ? tabEle.find('#pageSize_' + index + '').val() : 10;
            self.dataParameters.SortOrder = tabEle != null ? tabEle.find('#sortOrder_' + index + '').val() : "ASC";
            self.dataParameters.SortColumn = tabEle != null ? tabEle.find('#sortColumn_' + index + '').val() : "Month";
            self.dataParameters.SearchTerm = tabEle != null ? tabEle.find('#searchReportText').val() : "";
         //   self.dataParameters.SubstituteRoleMode = self.substituteRolePageType.Report;




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

        //callbackGetClassrooms: function (data) {




        //    var bindData = '';
        //    if (data != null && data.CenterList != null && data.CenterList.length > 0 && data.CenterList[0].Classroom != null && data.CenterList[0].Classroom.length > 0) {

        //        bindData += '<option value="0">--Select--</option>';

        //        self.classroomsJson = data.CenterList[0].Classroom;
        //        $.each(data.CenterList[0].Classroom, function (i, classroom) {

        //            bindData += '<option value=' + classroom.Enc_ClassRoomId + '>' + classroom.ClassName + '</option>';
        //        });
        //    }





        //    self.elements.dropdownClassroom.html(bindData);

        //    self.elements.divdropdownClassroom.show('slow');






        //},
        showBusy: function (status) {
            if (status) {
                $('#spinner').show();
            }
            else {
                $('#spinner').hide();
            }
        },
        validateFilterInputs: function () {

            //if (self.elements.searchStaffText.val() == '') {
            //    customAlert('Staff name is required');
            //    plainValidation(self.elements.searchStaffText);
            //    return false;
            //}

            //if (self.elements.dropdownCenter.val() == '0') {
            //    customAlertforlongtime('Center is required');
            //    plainValidation(self.elements.dropdownCenter);
            //    return false;
            //}

            if (self.elements.dropdownCenter.val() == null || self.elements.dropdownCenter.val() == '' || self.elements.dropdownCenter.val() == '0') {
                customAlertforlongtime(_langList.Centerisrequired);
                plainValidation(self.elements.dropdownCenter);
                return false;
            }

                //if (self.elements.dropdownClassroom.val() == '0') {
                //    customAlert('Classroom is required');
                //    plainValidation(self.elements.dropdownClassroom);
                //    return false;
                //}

                //if (self.elements.fromDateInput.val() == '' || self.elements.fromDateInput.val() == '__/__/____') {
                //    customAlert('From Date is required');
                //    plainValidation(self.elements.fromDateInput);
                //    return false;
                //}

                //if (new Date(self.elements.fromDateInput.val()).setHours(0, 0, 0, 0) < new Date().setHours(0, 0, 0, 0)) {
                //    customAlert('From Date should be greater than or equal to today\'s date');
                //    plainValidation(self.elements.fromDateInput);
                //    return false;
                //}

                //if (self.elements.toDateInput.val() == '' || self.elements.toDateInput.val() == '__/__/____') {
                //    customAlert('To Date is required');
                //    plainValidation(self.elements.toDateInput);
                //    return false;
                //}

                //if (new Date(self.elements.toDateInput.val()).setHours(0, 0, 0, 0) < new Date().setHours(0, 0, 0, 0)) {
                //    customAlert('To Date should be greater than or equal to today\'s date');
                //    plainValidation(self.elements.toDateInput);
                //    return false;
                //}


                //if (new Date(self.elements.fromDateInput.val()).setHours(0, 0, 0, 0) > new Date(self.elements.toDateInput.val()).setHours(0, 0, 0, 0)) {
                //    customAlert('From Date should be less than or equal to To Date');
                //    plainValidation(self.elements.fromDateInput);
                //    return false;
                //}


            else if (self.elements.dropdownMonths.val() == null || self.elements.dropdownMonths.val() == '' || self.elements.dropdownMonths.val() == '0') {
                customAlertforlongtime("Month is required");
                plainValidation(self.elements.dropdownMonths);
                return false;
            }


            return true;


        },
        //addSubstituteRole: function () {

        //    self.bindAjaxParameters(self.parametersMode.save, null, null);


        //    self.ajaxOptions.datatype = 'JSON';
        //    self.ajaxOptions.type = 'POST';
        //    self.ajaxOptions.url = self.addSubstituteRoleUrl;

        //    self.ajaxOptions.data = JSON.stringify(self.dataParameters);
        //    self.ajaxOptions.async = true;
        //    self.ajaxCall(self.callbackAddSubstituteRole);


        //},
        //removeSubstituteRole: function (ele) {


        //    var accesskey = $(ele).siblings('#subsituteId').val();
        //    var $centerId = $('#myTab li.active a').attr('accesskey');
        //    var $classroomId = $(ele).parent('td').parent('tr').children('td:eq(0)').children('p').attr('data-accesskey');
        //    var $className = $(ele).parent('td').parent('tr').children('td:eq(0)').children('p').html();
        //    var $fromDate = $(ele).parent('td').parent('tr').children('td:eq(2)').children('p').html();
        //    var $toDate = $(ele).parent('td').parent('tr').children('td:eq(3)').children('p').html();
        //    BootstrapDialog.show({
        //        title: 'Confirmation',
        //        message: '<p>You are about to delete the substitute teacher role assigned to Classroom <strong>' + $className + '</strong>  from the date <strong>' + $fromDate + '</strong> to <strong>' + $toDate + '</strong>.</p>\
        //                    <p>Click <strong>OK</strong> to Proceed.</p>',
        //        closable: true,
        //        closeByBackdrop: false,
        //        closeByKeyboard: false,
        //        buttons: [{
        //            label: '' + _langList.Cancel + ' <span class="glossy-button-before"></span><span class="glossy-button-after"></span>',
        //            cssClass: 'glossy-button-button button-red',
        //            action: function (dialogRef) {
        //                dialogRef.close(true);
        //            }
        //        }, {
        //            label: '' + _langList.Ok + ' <span class="glossy-button-before"></span><span class="glossy-button-after"></span>',
        //            cssClass: 'glossy-button-button button-green',
        //            autospin: true,
        //            action: function (dialogRef) {

        //                self.dataParameters.SubstituteID = accesskey;
        //                self.dataParameters.CenterID = $centerId;
        //                self.dataParameters.ClassroomID = $classroomId;
        //                self.ajaxOptions.url = self.removeSubstituteRoleUrl;
        //                self.ajaxOptions.type = 'POST';
        //                self.ajaxOptions.datatype = "JSON";
        //                self.ajaxOptions.async = true;
        //                //self.ajaxOptions.contentType = "application/json; charset=utf-8";
        //                self.ajaxOptions.data = JSON.stringify({ 'substituteRole': self.dataParameters });
        //                //dialogRef.setClosable(false);

        //                self.ajaxCall(self.callbackRemoveSubstituteRole);

        //            }
        //        }]
        //    });
        //},
        //callbackAddSubstituteRole: function (data) {

        //    switch (data) {
        //        case 1:
        //            customAlert("Record saved successfully");

        //            $('#myTab').find('li a[accesskey="' + self.elements.dropdownCenter.val() + '"]').trigger('click');

        //            window.setTimeout(function () {
        //                var $tabEle = $('#myTabContent .tab-pane.active');
        //                var $index = $tabEle.attr('id').replace('tab', '').trim();
        //                self.resetElements();
        //                self.getSubsituteRole($tabEle, $index, self.getlistMode.center);

        //            }, 10);


        //            break;
        //        case 0:
        //            customAlert('Error occurred. Please, try again later.');
        //            break;
        //        case 2:
        //            customAlert('Entered dates falls under the existing assigned dates.');
        //            break;
        //        case 3:
        //            customAlert('Selected staff was assigned to another classroom for entered dates.');
        //            break;
        //        case data.Data == "Login":
        //            customAlert("Session Ended Log Onto The System Again."); setTimeout(function () { window.location.href = HostedDir + '/login/Loginagency'; }, 2000);
        //            break;
        //    }


        //},
        //callbackRemoveSubstituteRole: function (data) {
        //    switch (data) {
        //        case 1:
        //            customAlert("Record deleted successfully");
        //            $('.modal').modal('hide');



        //            self.showBusy(true);

        //            window.setTimeout(function () {

        //                var $activeEle = $('#myTabContent').find('.tab-pane.active');
        //                var $index = $activeEle.attr('id').replace('tab', '').trim();

        //                self.getSubsituteRole($activeEle, $index, 1);

        //            }, 10);

        //            break;
        //        case 0:
        //            customAlert('Error occurred. Please, try again later.');
        //            break;

        //        case data.Data == "Login":
        //            customAlert("Session Ended Log Onto The System Again."); setTimeout(function () { window.location.href = HostedDir + '/login/Loginagency'; }, 2000);
        //            break;
        //    }
        //},

        getReport: function (ele, index, isCenter) {


            self.ajaxOptions.datatype = 'html';
            self.ajaxOptions.type = 'POST';

            switch (isCenter) {
                case self.getlistMode.center:
                    self.bindAjaxParameters(self.parametersMode.get, ele, index);
                    self.ajaxOptions.url = self.getUFCReportByCenterUrl;
                    break;
                case self.getlistMode.all:
                    self.bindAjaxParameters(self.parametersMode.filter, ele, index);
                    self.ajaxOptions.url = self.getUFCReportUrl;
                    $('#myTab').html('');
                    break;
            }

            self.ajaxOptions.data = JSON.stringify(self.dataParameters);

            self.ajaxOptions.async = false;
            self.ajaxCall(self.callbackGetReport);
        },
        callbackGetReport: function (data) {
            debugger;
            if (data.Data == "Login") {
                customAlert("Session Ended Log Onto The System Again."); setTimeout(function () { window.location.href = HostedDir + '/login/Loginagency'; }, 2000);
            }
            else {

                var $activeTab = $('#myTab').find('li.active');
                var activeTabcontent = '';

                if ($activeTab.length > 0) {

                    var index = $activeTab.index();

                    activeTabcontent = $('#myTabContent').find('#' + index + 'tab');

                    //activeTabcontent = $('#myTabContent').find('.tab-pane.active');

                    //var index = activeTabcontent.attr('id').replace('tab', '').trim();

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


                        $(tr).find('td:last').find('i.btn-delete-trans').off('click').on('click', function () {

                            self.removeSubstituteRole(this);

                        });

                    });






                    self.getTotalRecord($totalRecord, this, index);

                });





            }


            $('[data-toggle="tooltip"]').tooltip();
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


                debugger;

                var index = $(this).attr('id').split('_')[1];

                var tabEle = $('#ddlpagetodisplay_' + index + '').closest('#div-pagination-' + index + '').closest('.tab-pane');



                $('#lastIndex_' + index + '').val(0);

                tabEle.find('#pageSize_' + index + '').val($(this).val());

                self.showBusy(true);

                window.setTimeout(function () {





                    self.getReport(tabEle, index, self.getlistMode.center);

                    firstButton.attr('disabled', true);
                    backButton.attr('disabled', true);
                }, 10);




            });

            firstButton.off('click').on('click', function () {

                if ($(this).attr('disabled') == "disabled")
                    return false;

                var index = $(this).attr('id').split('_')[1];
                var tabEle = $('#ddlpagetodisplay_' + index + '').closest('#div-pagination-' + index + '').closest('.tab-pane');


                self.fnChangePage(self.pageChangeType.first, tabEle, index);

            });

            backButton.off('click').on('click', function () {

                if ($(this).attr('disabled') == "disabled")
                    return false;

                var index = $(this).attr('id').split('_')[1];
                var tabEle = $('#ddlpagetodisplay_' + index + '').closest('#div-pagination-' + index + '').closest('.tab-pane');



                self.fnChangePage(self.pageChangeType.back, tabEle, index);

            });

            dropdownPageNumber.off('change').on('change', function () {

                self.showBusy(true);

                var index = $(this).attr('id').split('_')[1];
                var tabEle = $('#ddlpagetodisplay_' + index + '').closest('#div-pagination-' + index + '').closest('.tab-pane');

                tabEle.find('#requestedPage_' + index + '').val($(this).val());

                window.setTimeout(function () {

                    self.getListafterupdation(tabEle, index);

                }, 1)
            });

            nextButton.off('click').on('click', function () {

                if ($(this).attr('disabled') == "disabled")
                    return false;

                var index = $(this).attr('id').split('_')[1];
                var tabEle = $('#ddlpagetodisplay_' + index + '').closest('#div-pagination-' + index + '').closest('.tab-pane');

                self.fnChangePage(self.pageChangeType.next, tabEle, index);

            });

            lastButton.off('click').on('click', function () {
                if ($(this).attr('disabled') == "disabled")
                    return false;

                var index = $(this).attr('id').split('_')[1];
                var tabEle = $('#ddlpagetodisplay_' + index + '').closest('#div-pagination-' + index + '').closest('.tab-pane');

                self.fnChangePage(self.pageChangeType.last, tabEle, index);

            });

        },

        fnChangePage: function (val, tabEle, index) {

            debugger;
            self.showBusy(true);

            window.setTimeout(function () {



                tabEle.find('#pageLoadedFirst_' + index + '').val(0);
                tabEle.find('#pageSize_' + index + '').val(tabEle.find('#ddlpagetodisplay_' + index + '').val());


                if (val == self.pageChangeType.first) {

                    tabEle.find('#startIndex_' + index + '').val(0);

                    var $lastindex = parseInt(parseInt(tabEle.find('#pageSize_' + index).val()) + (parseInt(tabEle.find('#lastIndex_' + index).val()) * parseInt(tabEle.find('#requestedPage_' + index + '').val())));

                    tabEle.find('#lastIndex_' + index + '').val($lastindex);

                    tabEle.find('#requestedPage_' + index + '').val(((parseInt(tabEle.find('#startIndex_' + index + '').val()) / 10) + 1))


                    self.getReport(tabEle, index, self.getlistMode.center);

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

                    self.getReport(tabEle, index, self.getlistMode.center);


                    tabEle.find('#First_' + index + '').attr('disabled', false);
                    tabEle.find('#Back_' + index + '').attr('disabled', false);
                    tabEle.find('#Next_' + index + '').attr('disabled', true);
                    tabEle.find('#Last_' + index + '').attr('disabled', true);

                }
                else if (val == self.pageChangeType.next) {

                    var $lastIndex = parseInt(parseInt(tabEle.find('#pageSize_' + index + '').val()) + parseInt(tabEle.find('#lastIndex_' + index + '').val()));

                    tabEle.find('#lastIndex_' + index + '').val($lastIndex)


                    var $reqpage = parseInt(((parseInt(tabEle.find('#lastIndex_' + index + '').val()) / parseInt(tabEle.find('#pageSize_' + index + '').val())) + 1));


                    self.requestedPage = $reqpage;
                    tabEle.find('#requestedPage_' + index + '').val($reqpage);
                    //  self.gotoNextPage(self.requestedPage, self.pageSize);

                    self.getReport(tabEle, index, self.getlistMode.center);

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

                    self.getReport(tabEle, index, self.getlistMode.center);

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

            debugger;
            ele.find('#pageSize_' + index + '').val(ele.find('#ddlpagetodisplay_' + index + '').val())

            ele.find('#requestedPage_' + index + '').val(ele.find('#ddlpaging_' + index + '').val());

            var $startIndex =  parseInt((parseInt(ele.find('#pageSize_' + index + '').val()) * (parseInt(ele.find('#requestedPage_' + index + '').val()) - 1)) + 1);

            ele.find('#startIndex_' + index + '').val($startIndex);

            var $lastIndex =parseInt((parseInt(ele.find('#pageSize_' + index + '').val()) * parseInt(ele.find('#requestedPage_' + index + '').val())) - parseInt(ele.find('#pageSize_' + index + '').val()));


            ele.find('#lastIndex_' + index + '').val($lastIndex);




            self.getReport(ele, index, self.getlistMode.center);

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

    }
    return _ufcReport;

}



$(function () {

    ufcReport().init();

});

