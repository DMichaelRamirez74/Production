var self = null;
var educationComponent = {


    reportFormatType: null,
    init: function () {

        self = this;

        self.initializeElements();
        self.resetElements();
        self.initializeEvents();
    },

    addEducationComponentUrl: HostedDir + '/AgencyUser/AddEducationComponent',
    getEducationCompPartialUrl: HostedDir + '/AgencyUser/GetEducationComponentPartial',
    removeEducationComponentUrl: HostedDir + '/AgencyUser/RemoveEducationComponent',
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
        'formAddDescription': null,
        'lblTotalCount': null,
        'divtableResponsive': null,
        'buttonLoadActivities': null,
        'anchorAddMoreComponent': null,
        'btnAddDesc': null,
        'btnResetActivity': null,
        'divtextareagroup': null,
        'divActivityOption': null,
        'btnAddTransActivity': null,
        'btnResetTransActivity': null,
        'divBindOptionsResponsive': null,
        'divPaginationSection': null,
        'dropdownRecordsPerpage': null,
        'buttonPagingFirst': null,
        'buttonPagingBack': null,
        'buttonPagingNext': null,
        'buttonPagingLast': null,
        'dropdownPageNumber': null,
        'modalEducationComponent': null,
        'textareamodalDescription': null,
        'btnModalSubmitActivity': null,
        'btnModalExitActivity': null,
        'hiddenModaldescID': null


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
   
    dataParameters: educationComponentJson,

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
        $selfElements.dropdownMonths = $('#selectMonths');
        $selfElements.divMonthsDropdown = self.elements.dropdownMonths.closest('.form-group');
        $selfElements.anchorPdf = $('#pdfanchor');
        $selfElements.anchorExcel = $('#excelanchor');
        $selfElements.divBindOptionsResponsive = $('#div-bind-partial');
        $selfElements.buttonLoadActivities = $('#btnLoadMonthlyRecruitment');
        $selfElements.anchorAddMoreComponent = $('#anchorAddMoreComponent');
        $selfElements.btnAddDesc = $('#btn-add-desc');
        $selfElements.btnResetActivity = $('#btn-reset-activity');
        $selfElements.divtextareagroup = $('#div-textarea-edu-group');
        $selfElements.divtableResponsive = $('#div-table-responsive');
        $selfElements.lblTotalCount = $selfElements.divtableResponsive.find('#totalCountSpan');
        $selfElements.formAddDescription = $('#formAddDescription');
        $selfElements.divPaginationSection = $('#div-pagination-section');
        $selfElements.dropdownRecordsPerpage = $('#ddlpagetodisplay');
        $selfElements.buttonPagingFirst = $selfElements.divPaginationSection.find('#ulPaging').find('#First');
        $selfElements.buttonPagingBack = $selfElements.divPaginationSection.find('#ulPaging').find('#Back');
        $selfElements.buttonPagingNext = $selfElements.divPaginationSection.find('#ulPaging').find('#Next');
        $selfElements.buttonPagingLast = $selfElements.divPaginationSection.find('#ulPaging').find('#Last');
        $selfElements.dropdownPageNumber = $selfElements.divPaginationSection.find('#ddlpaging');
        $selfElements.modalEducationComponent = $('#modal-education-component');
        $selfElements.textareamodalDescription = $selfElements.modalEducationComponent.find('#textarea-modal-desc');
        $selfElements.btnModalSubmitActivity = $selfElements.modalEducationComponent.find('#btn-modal-submit');
        $selfElements.btnModalExitActivity = $selfElements.modalEducationComponent.find('#btn-modal-cancel');
        $selfElements.hiddenModaldescID = $selfElements.modalEducationComponent.find('#hide-modal-desc-id');

    },
    resetElements: function () {









    },
    initializeEvents: function () {


        var $selfElements = self.elements;






        $selfElements.anchorAddMoreComponent.on('click', function () {



            self.addMoreActivity(this);

        });

        $selfElements.btnAddDesc.on('click', function () {

            cleanValidation();
            if (self.elements.divtextareagroup.find('textarea').val() == '') {
                customAlertforlongtime(_langList.Atleast_one_edcuation_comp_required);
                plainValidation(self.elements.divtextareagroup.find('textarea:eq(0)'));
                return false;
            }


            self.addEducationComponent();



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

            if (self.elements.textareamodalDescription.val() == '') {
                customAlertforlongtime(_langList.Education_component_required);
                plainValidation(self.elements.textareamodalDescription);
                return false;
            }
            else {


                self.ajaxOptions.url = self.addEducationComponentUrl;
                self.ajaxOptions.datatype = 'JSON';
                self.ajaxOptions.type = "POST";
                self.ajaxOptions.contentType = "application/json; charset=utf-8";
                self.ajaxOptions.async = true;
                self.dataParameters.EducationComponentList = [{ "Description": self.elements.textareamodalDescription.val(), "EducationComponentID": self.elements.hiddenModaldescID.val(), "Status": true }];
                self.dataParameters.CenterID = "0";
                self.dataParameters.Month = 0;

                self.ajaxOptions.data = JSON.stringify({ 'educationComponent': self.dataParameters });


                self.ajaxCall(self.callbackAddEducationComponent);

            }


        });


       



        if (self.elements.formAddDescription.length > 0 && self.elements.formAddDescription.is(':visible')) {
            self.getReport();
        }


    },

    addEducationComponent: function () {
        var dataArray = [];


        $.each(self.elements.divtextareagroup.find('textarea'), function () {

            if ($(this).val().trim() != null && $(this).val().trim() != '') {

                dataArray.push({ 'Description': $(this).val().trim(), 'EducationComponentID': 0, 'Status': true });
            }
        });



        self.ajaxOptions.url = self.addEducationComponentUrl;
        self.ajaxOptions.datatype = 'JSON';
        self.ajaxOptions.type = "POST";
        self.ajaxOptions.contentType = "application/json; charset=utf-8";
        self.ajaxOptions.async = true;

        self.dataParameters.EducationComponentList = dataArray;
        self.dataParameters.CenterID = "0";
        self.dataParameters.Month = 0;

        self.ajaxOptions.data = JSON.stringify({ 'educationComponent': self.dataParameters });


        self.showBusy(true);

        self.ajaxCall(self.callbackAddEducationComponent);



    },

    resetActivityEntry: function () {
        cleanValidation();
        self.elements.divtextareagroup.find('textarea').val('');
        self.elements.divtextareagroup.find('.adddivspace').remove();
    },

    callbackAddEducationComponent: function (data) {
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

   
    generatereport(data) {

    },

  

    addMoreActivity: function (ele) {

        var appendText = '<div class="col-xs-12 col-sm-12 adddivspace">\
                                                    <textarea style="min-height:50px;line-height:normal;" class="col-xs-9"></textarea>\
                                                    <div class="col-xs-3 div-a-add">\
                                                        <a href="javascript:void(0)" id="Attachmectstag" class="a-add-comp" title=' + _langList.Add_another_education_comp + '><i class="fa fa-minus-circle"></i>&nbsp;' + _langList.Remove + '</a>\
                                                    </div>\
                                                </div>';

        var anchorTargetParent = $(ele).parent('div').parent('div').parent('div');

        anchorTargetParent.append(appendText);

        anchorTargetParent.children('div.adddivspace:last').find('a').on('click', function () {

            $(this).parent('div').parent('div').remove();

        });


    },

   

    getReport: function () {


        self.bindAjaxParameters(self.ajaxCallMode.EducationComponentLookup);
        self.ajaxOptions.url = self.getEducationCompPartialUrl;
        self.ajaxOptions.datatype = 'html',
        self.ajaxOptions.type = 'POST',
        self.ajaxOptions.data = JSON.stringify({ educationComponent: self.dataParameters });
        self.ajaxOptions.async = false;
        self.ajaxOptions.contentType = 'application/json; charset=utf-8';
        self.ajaxCall(self.bindReport);




    },
   
    ajaxCallMode: {
        'EducationComponentLookup': 1,
    },

    bindAjaxParameters: function (mode) {

        switch (mode) {
            case self.ajaxCallMode.EducationComponentLookup:

                self.dataParameters.RequestedPage = self.requestedPage == null || self.requestedPage == '' ? 1 : self.requestedPage;
                self.dataParameters.PageSize = self.pageSize == null || self.pageSize == '' ? 10 : self.pageSize;
                self.dataParameters.SortOrder = self.sortOrder;
                self.dataParameters.SortColumn = self.sortColumn

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

    bindReport: function (data) {



        if (data != null && data.Data != null && data.Data == "Login") {
            customAlert("Session Ended Log Onto The System Again."); setTimeout(function () { window.location.href = HostedDir + '/login/Loginagency'; }, 2000);
            return;
        }
        else {
            self.elements.divtableResponsive.html(data);

            var tBody = self.elements.divtableResponsive.find('#education-comp-table').find('#education-comp-tbody');

            $.each(tBody.find('tr'), function () {

                $(this).find('td:eq(2)').find('.btn-edit-trans').on('click', function () {

                    self.editEducationComponent(this);

                });

                $(this).find('td:eq(2)').find('.btn-delete-trans').on('click', function () {
                    self.deleteEducationComponent(this);
                });
            });

        }





        self.initializeElements();



        self.getTotalRecord(parseInt(self.elements.lblTotalCount.html()));

       



    },

    editEducationComponent: function (ele) {


        var $desc = $(ele).closest('td').closest('tr').find('td:eq(0)').find('p').html().trim();
        var $id = $(ele).siblings('#input-hide-edu-id').val();

        self.elements.textareamodalDescription.html($desc);
        self.elements.hiddenModaldescID.val($id);

        self.elements.modalEducationComponent.modal('show');

    },
    deleteEducationComponent: function (ele) {

        var $id = $(ele).siblings('#input-hide-edu-id').val();
        var $desc = $(ele).closest('td').closest('tr').find('td:eq(0)').find('p').html().trim();

        BootstrapDialog.show({
            title: 'Confirmation',
            message: '<p style="font-weight:bold;">' + _langList.You_about_to_delete_education_comp + '</p>\
                        <p><i>'+ $desc + '</i></p>\
                        <p>'+ _langList.Click_ok_to_proceed + '</p>',
            closable: true,
            closeByBackdrop: false,
            closeByKeyboard: false,
            buttons: [{
                label: '' + _langList.Cancel + ' <span class="glossy-button-before"></span><span class="glossy-button-after"></span>',
                cssClass: 'glossy-button-button button-red',
                action: function (dialogRef) {
                    dialogRef.close(true);
                }
            }, {
                label: '' + _langList.Ok + ' <span class="glossy-button-before"></span><span class="glossy-button-after"></span>',
                cssClass: 'glossy-button-button button-green',
                autospin: true,
                action: function (dialogRef) {

                    self.dataParameters.EducationComponentList = [{ "EducationComponentID": $id, "Status": false }];
                    self.dataParameters.CenterID = 0;
                    self.dataParameters.Month = 0;
                    self.ajaxOptions.url = self.removeEducationComponentUrl;
                    self.ajaxOptions.type = 'POST';
                    self.ajaxOptions.datatype = "JSON";
                    self.ajaxOptions.async = true;
                    self.ajaxOptions.contentType = "application/json; charset=utf-8";
                    self.ajaxOptions.data = JSON.stringify({ 'educationComponent': self.dataParameters });

                    self.ajaxCall(self.callbackRemoveEducationComponent);

                }
            }]
        });


    },

    callbackRemoveEducationComponent: function (data) {
        if (data) {
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
            customAlertforlongtime(_langList.ErrrorOccurred);
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

    educationComponent.init();

});