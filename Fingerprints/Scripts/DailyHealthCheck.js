var self = null;
var dailyHealthCheck = {


    reportFormatType: null,
    init: function () {

        self = this;

        self.initializeElements();
        self.resetElements();
        self.initializeEvents();
    },

    addDailyObservationLookupUrl: HostedDir + '/Agency/UpsertDailyObservationLookup',
    getDailyObservationLookupUrl: HostedDir + '/Agency/GetDailyObservationLookup',
    removeDailyObservationLookupUrl: HostedDir + '/Agency/RemoveRecruitmentActivityLookup',
    chekDailyObservaionLookupUrl: HostedDir + '/Agency/CheckDailyObservationLookupExists',
    availDailyObservationLookupAllAgencyUrl:HostedDir+'/Agency/AvailDailyObservationLookupAllAgencies',
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
        'anchorAddMoreDesc': null,
        'btnAddDesc': null,
        'btnResetDesc': null,
        'divtextareagroup': null,
        'divActivityOption': null,
        'btnAddTransActivity': null,
        'btnResetTransActivity': null,
        'divBindOptionsResponsive': null,
        'divObservationgridResponsive': null,
        'divPaginationSection': null,
        'dropdownRecordsPerpage': null,
        'buttonPagingFirst': null,
        'buttonPagingBack': null,
        'buttonPagingNext': null,
        'buttonPagingLast': null,
        'dropdownPageNumber': null,
        'modalDailyObservation': null,
        'textareamodalDesc': null,
        'btnModalSubmitObservation': null,
        'btnModalExitActivity': null,
        'hiddenModalObsID': null,
        'tbodyDailyObservation': null,
        'tableDailyObservation': null,
        'buttonSearch': null,
        'inputSearchTxt': null


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
    dataParameters: dailyObservationJson,
    healthCheckMode: dailyObservationMode,
    checkMode: {
        'insert': 1,
        'popup': 2

    },
    showBusy: function (ele) {
        if (ele)
            $('#spinner').show();
        else
            $('#spinner').hide();
    },

    initializeElements: function () {

        var $selfElements = self.elements;

        $selfElements.anchorAddMoreDesc = $('#anchorAddMoreDesc');
        $selfElements.btnAddDesc = $('#btn-add-desc');
        $selfElements.btnResetDesc = $('#btn-reset-desc');
        $selfElements.divtextareagroup = $('#div-textarea-desc-group');

        $selfElements.divObservationgridResponsive = $('#observation-grid-responsive');
        $selfElements.lblTotalCount = $selfElements.divObservationgridResponsive.find('#totalCountSpan');
        $selfElements.formAddDescription = $('#formAddDescription');

        $selfElements.divPaginationSection = $('#div-pagination-recruitment-activities');
        $selfElements.dropdownRecordsPerpage = $('#ddlpagetodisplay');
        $selfElements.buttonPagingFirst = $selfElements.divPaginationSection.find('#ulPaging').find('#First');
        $selfElements.buttonPagingBack = $selfElements.divPaginationSection.find('#ulPaging').find('#Back');
        $selfElements.buttonPagingNext = $selfElements.divPaginationSection.find('#ulPaging').find('#Next');
        $selfElements.buttonPagingLast = $selfElements.divPaginationSection.find('#ulPaging').find('#Last');
        $selfElements.dropdownPageNumber = $selfElements.divPaginationSection.find('#ddlpaging');
        $selfElements.modalDailyObservation = $('#modal-daily-observation');
        $selfElements.textareamodalDesc = $selfElements.modalDailyObservation.find('#textarea-modal-desc');
        $selfElements.btnModalSubmitObservation = $selfElements.modalDailyObservation.find('#btn-modal-submit');
        $selfElements.btnModalExitActivity = $selfElements.modalDailyObservation.find('#btn-modal-cancel');
        $selfElements.hiddenModalObsID = $selfElements.modalDailyObservation.find('#hide-modal-obs-id');
        $selfElements.tbodyDailyObservation = $('#daily-health-check-tbody');
        $selfElements.tableDailyObservation = $('#daily-health-check-table');
        $selfElements.buttonSearch = $selfElements.divObservationgridResponsive.find('#btnSearchauto');
        $selfElements.inputSearchTxt = $selfElements.divObservationgridResponsive.find('#searchText');
    },
    resetElements: function () {

    },
    initializeEvents: function () {


        var $selfElements = self.elements;



        $selfElements.anchorAddMoreDesc.on('click', function () {



            self.addMoreActivity(this);

        });

        $selfElements.btnAddDesc.on('click', function () {

            cleanValidation();

            if (self.validateReportFilter()) {




                self.checkDailyObservationLookup(self.checkMode.insert);
            }



        });

        $selfElements.btnResetDesc.on('click', function () {

            self.resetActivityEntry();

        });

        $selfElements.buttonSearch.on('click', function () {

            self.showBusy(true);
            window.setTimeout(function () {

                self.getReport();
            }, 10);
        });





        document.getElementById('formAddDescription').addEventListener('submit', function (e) {
            debugger;
            e.preventDefault();
        }, false);



        self.elements.tableDailyObservation.find('tr th').on('click', function (e) {
            e.preventDefault();
            debugger;


            if ($(this).children('i.fa').length > 0) {
                if ($(this).children('i.fa:visible').length > 0) {
                    if ($(this).children('i.fa:visible').hasClass('i-asc')) {
                        $(this).children('i.fa.i-asc').hide();
                        $(this).children('i.fa.i-desc').css('display', 'inline-block');
                    }
                    else {
                        $(this).children('i.fa.i-desc').hide();
                        $(this).children('i.fa.i-asc').css('display', 'inline-block');
                    }
                }
                else {
                    self.elements.tableDailyObservation.find('tr th').find('i.fa').hide();
                    $(this).children('i.fa.i-asc').css('display', 'inline-block');
                }

                self.showBusy(true);
                window.setTimeout(function () {
                    self.getReport();

                }, 10);
            }




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

        self.elements.btnModalSubmitObservation.on('click', function () {

            cleanValidation();

            if (self.elements.textareamodalDesc.val() == '') {
                customAlertforlongtime("Description is required");
                plainValidation(self.elements.textareamodalDesc);
                return false;
            }
            else {




                self.checkDailyObservationLookup(self.checkMode.popup);

            }
        });



        self.showBusy(true);
        window.setTimeout(function () {
            self.getReport();

        }, 10);

    },

    checkDailyObservationLookup: function (mode) {
        debugger;
        var dataArraycheck = [];


        self.ajaxOptions.url = self.chekDailyObservaionLookupUrl;
        self.ajaxOptions.datatype = 'JSON';
        self.ajaxOptions.type = "POST";
        self.ajaxOptions.contentType = "application/json; charset=utf-8";
        self.ajaxOptions.async = true;
        switch (mode) {
            case self.checkMode.insert:


                $.each(self.elements.divtextareagroup.find('textarea'), function () {

                    if ($(this).val().trim() != null && $(this).val().trim() != '') {

                        dataArraycheck.push({ 'Description': $(this).val().trim(), 'ObservationID': 0, 'Status': true });
                    }
                });

                self.dataParameters.DailyObservationList = dataArraycheck;
                self.ajaxOptions.data = JSON.stringify({ 'dailyObservation': self.dataParameters });
                self.showBusy(true);
                self.ajaxCall(self.callbackCheckDailyObservation);

                break;


            case self.checkMode.popup:

                dataArraycheck.push({ 'Description': self.elements.textareamodalDesc.val(), 'ObservationID': self.elements.hiddenModalObsID.val(), 'Status': true });

                self.dataParameters.DailyObservationList = dataArraycheck;
                self.ajaxOptions.data = JSON.stringify({ 'dailyObservation': self.dataParameters });
                self.showBusy(true);
                self.ajaxCall(self.callbackModalCheckDailyObservation);

                break;
        }





    },

    addDescription: function (mode) {
        var dataArray = [];


        switch (mode) {
            case self.checkMode.insert:
                $.each(self.elements.divtextareagroup.find('textarea'), function () {

                    if ($(this).val().trim() != null && $(this).val().trim() != '') {

                        dataArray.push({ 'Description': $(this).val().trim(), 'ObservationID': 0, 'Status': true });
                    }
                });


                break;
            case self.checkMode.popup:


                dataArray.push({ 'Description': self.elements.textareamodalDesc.val().trim(), 'ObservationID': self.elements.hiddenModalObsID.val().trim(), 'Status': true });

                break;


        }







        self.ajaxOptions.url = self.addDailyObservationLookupUrl;
        self.ajaxOptions.datatype = 'JSON';
        self.ajaxOptions.type = "POST";
        self.ajaxOptions.contentType = "application/json; charset=utf-8";
        self.ajaxOptions.async = true;
        self.dataParameters.DailyObservationList = dataArray;
        self.ajaxOptions.data = JSON.stringify({ 'dailyObservation': self.dataParameters, 'mode': self.healthCheckMode.Report });


        self.showBusy(true);

        self.ajaxCall(self.callbackaddDescription);



    },

    resetActivityEntry: function () {
        cleanValidation();
        self.elements.divtextareagroup.find('textarea').val('');
        self.elements.divtextareagroup.find('.adddivspace').remove();

        if (self.elements.modalDailyObservation.is(':visible')) {
            self.elements.modalDailyObservation.modal('hide');
            self.elements.textareamodalDesc.val('');
            self.elements.hiddenModalObsID.val('');
        }

    },

    callbackCheckDailyObservation: function (data) {
        debugger;
        if (data != null && data.Data != null && data.Data == "Login") {
            customAlert("Session Ended Log Onto The System Again."); setTimeout(function () { window.location.href = HostedDir + '/login/Loginagency'; }, 2000);
            return;
        }
        else if (data != null) {
            if (data.Result == 1) {

                if (data.Model != null && data.Model.DailyObservationList.length > 0) {
                    var descArr = [];

                    $.each(data.Model.DailyObservationList, function (i, desc) {

                        descArr.push(desc.Description.trim().toLowerCase());
                    });

                    if (descArr.length > 0) {
                        $.each(self.elements.divtextareagroup.find('textarea'), function () {

                            if ($.inArray($(this).val().trim().toLowerCase(), descArr) > -1) {
                                plainValidation($(this));
                            }
                        });

                        customAlertforlongtime('Entered description already exists in the system');
                    }
                }
            }
            else if (data.Result == 2) {
                customAlert(_langList.ErrrorOccurred);
            }
            else {
                self.addDescription(self.checkMode.insert);
            }
        }
        else {
            customAlert(_langList.ErrrorOccurred);
        }



    },

    callbackModalCheckDailyObservation: function (data) {
        debugger;
        if (data != null && data.Data != null && data.Data == "Login") {
            customAlert("Session Ended Log Onto The System Again."); setTimeout(function () { window.location.href = HostedDir + '/login/Loginagency'; }, 2000);
            return;
        }
        else if (data != null) {
            if (data.Result == 1) {

                if (data.Model != null && data.Model.DailyObservationList.length > 0) {
                    var descArr = [];

                    $.each(data.Model.DailyObservationList, function (i, desc) {

                        descArr.push(desc.Description.trim().toLowerCase());
                    });

                    if (descArr.length > 0) {
                        plainValidation(self.elements.textareamodalDesc);
                        customAlertforlongtime('Entered description already exists in the system');
                    }
                }
            }
            else if (data.Result == 2) {
                customAlert(_langList.ErrrorOccurred);
            }
            else {
                self.addDescription(self.checkMode.popup);
            }
        }
        else {
            customAlert(_langList.ErrrorOccurred);
        }



    },
    callbackaddDescription: function (data) {
        debugger;
        if (data != null && data.Data != null && data.Data == "Login") {
            customAlert("Session Ended Log Onto The System Again."); setTimeout(function () { window.location.href = HostedDir + '/login/Loginagency'; }, 2000);
            return;
        }
        else if (data != null) {
            if (data.Result) {

                if (data.StatusResult == 1) {
                    self.resetActivityEntry();
                    customAlertSuccess(_langList.RecordSavedSuccessfully);
                    self.bindReport(data.Model);
                }
                else if (data.StatusResult == 2) {
                    BootstrapDialog.show({
                        title: 'Information',
                        message: '<p style="font-weight:normal;">Health check description is already reported.</p>',
                        closable: false,
                        closeByBackdrop: false,
                        closeByKeyboard: false,
                        buttons: [{
                            label: 'Exit <span class="glossy-button-before"></span><span class="glossy-button-after"></span>',
                            cssClass: 'glossy-button-button button-red',
                            action: function (dialogRef) {
                                dialogRef.close(true);
                                self.resetActivityEntry();
                                self.bindReport(data.Model);
                            }
                        }]
                    });
                }
                else {
                    customAlert(_langList.ErrrorOccurred);
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


    callbackavailDailyObservationAllAgency:function(data)
    {
        if (data != null && data.Data != null && data.Data == "Login") {
            customAlert("Session Ended Log Onto The System Again."); setTimeout(function () { window.location.href = HostedDir + '/login/Loginagency'; }, 2000);
            return;
        }
        else if(data!=null)
        {
            if(data.Result)
            {
                self.resetActivityEntry();
                customAlertSuccess(_langList.RecordSavedSuccessfully);
                self.bindReport(data.Model);
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
                                                    <textarea style="min-height:50px;max-width:734px;line-height:normal;" maxlength="50" class="col-xs-9"></textarea>\
                                                    <div class="col-xs-3 div-a-add">\
                                                        <a href="javascript:void(0)" id="Attachmectstag" class="a-add-desc" title="Add another description"><i class="fa fa-minus-circle"></i>&nbsp;' + _langList.Remove + '</a>\
                                                    </div>\
                                                </div>';

        var anchorTargetParent = $(ele).parent('div').parent('div').parent('div');

        anchorTargetParent.append(appendText);

        anchorTargetParent.children('div.adddivspace:last').find('a').on('click', function () {

            $(this).parent('div').parent('div').remove();

        });


    },
    getReport: function () {


        self.bindAjaxParameters();
        self.ajaxOptions.url = self.getDailyObservationLookupUrl;
        self.ajaxOptions.datatype = 'JSON',
        self.ajaxOptions.type = 'POST',
        self.ajaxOptions.data = JSON.stringify({ dailyObservation: self.dataParameters, mode: self.healthCheckMode.Report });
        self.ajaxOptions.async = false;
        self.ajaxOptions.contentType = 'application/json; charset=utf-8';
        self.ajaxCall(self.bindReport);




    },
    validateReportFilter: function () {
        debugger;
        cleanValidation();
        if (self.elements.divtextareagroup.find('textarea').val() == '') {
            customAlertforlongtime("At least one description is required");
            plainValidation(self.elements.divtextareagroup.find('textarea:eq(0)'));
            return false;
        }
        else {

            var arr = [];

            var dupList = [];
            var uniqueList = [];

            $.each(self.elements.divtextareagroup.find('textarea'), function () {

                arr.push({ 'index': $(this).index(), 'value': $(this).val().trim().toLowerCase() });
            });

            if (arr.length > 1) {


                $.each(arr, function () {
                    if ($.inArray(this.value, uniqueList) == -1) {
                        uniqueList.push(this.value);
                    }
                    else {
                        if ($.inArray(this.value, dupList) == -1) {
                            dupList.push(this.value);
                        }
                    }
                });

                if (dupList.length > 0) {
                    $.each(self.elements.divtextareagroup.find('textarea'), function (i, textarea) {

                        if ($.inArray($(textarea).val().trim().toLowerCase(), dupList) > -1) {
                            // $(textarea).css('background-color','pink');
                            plainValidation($(textarea));
                        }

                    });

                    customAlertforlongtime('Descriptions should be unique');

                    return false;
                }
                else {
                    return true;
                }

            }
            else {
                return true;
            }


        }




    },



    bindAjaxParameters: function () {

        debugger;

        if (self.elements.tableDailyObservation.is(':visible')) {
            self.sortOrder = self.elements.tableDailyObservation.find('thead tr th .fa:visible').hasClass('i-asc') ? 'ASC' : 'DESC';
            self.sortColumn = self.elements.tableDailyObservation.find('thead tr th .fa:visible').siblings('span').attr('col-name');
        }


        self.dataParameters.RequestedPage = self.requestedPage == null || self.requestedPage == '' ? 1 : self.requestedPage;
        self.dataParameters.PageSize = self.pageSize == null || self.pageSize == '' ? 10 : self.pageSize;
        self.dataParameters.SortOrder = self.sortOrder;
        self.dataParameters.SortColumn = self.sortColumn;
        self.dataParameters.SearchTerm = self.elements.inputSearchTxt.val().trim();





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





    bindReport: function (data) {

        console.log(data);

        if (data != null && data.Data != null && data.Data == "Login") {
            customAlert("Session Ended Log Onto The System Again."); setTimeout(function () { window.location.href = HostedDir + '/login/Loginagency'; }, 2000);
            return;
        }
        else {
            var thLength = self.elements.tableDailyObservation.find('thead').find('tr>th').length;

            var pClass = (thLength == 4) ? 'with-agency' : 'no-agency';

            if (thLength == 4)
                self.elements.inputSearchTxt.attr('placeholder', 'Search Agency, description...');

            var appendData = '';
            $.each(data.DailyObservationList, function (i, obj) {

                var selectOption = '';


                if (obj.IsReported || (!obj.Editable)) {
                    selectOption = '<label class="lbl-text">';

                    if (obj.Status)
                        selectOption += 'Active</label>';
                    else
                        selectOption += 'Inactive</label>';
                }
                else {
                    selectOption = '<select id=selectStatus_' + i + ' class="glossy-select selectStatus" >';
                    if (obj.Status) {
                        selectOption += '<option value="1"  selected>Active</option>';
                        selectOption += '<option value="0">Inactive</option>';
                    }
                    else {
                        selectOption += '<option value="0" selected>Inactive</option>';
                        selectOption += '<option value="1" >Active</option>';
                    }
                    selectOption += '</select>';
                }


                appendData += '<tr>\
                             <td data-title="Description" role="gridcell"><p class="'+ pClass + '" data-toggle="tooltip" title="'+obj.Description+'" >' + obj.Description + '</p></td>';

                if (thLength == 4) {

                    appendData += '<td data-title="Agency Name" role="gridcell"><p class="' + pClass + '" data-toggle="tooltip" title="'+obj.AgencyName+'" >' + obj.AgencyName + '</p></td>';
                }

                appendData += '<td data-title="Status" role="gridcell"><p class="' + pClass + '">' + selectOption + '</p></td>\
                             <td data-title="Action" role="gridcell">\
                            <input type="hidden" id="input-accesskey_'+ i + '" value=' + obj.ObservationID + '>';
                if (obj.Editable && obj.Status) {
                    appendData += '<i class="fa fa-edit  btn-edit-trans" aria-hidden="true" data-toggle="tooltip" title="Edit"></i>';
                }
                else {
                    appendData += '<i class="" aria-hidden="true" style="visibility:hidden;">N/A</i>';

                }
                if (thLength == 4 && obj.AgencyName.trim() != '') {

                    appendData += '<i class="fa fa-users  btn-to-all" aria-hidden="true" data-toggle="tooltip" title="Make this health check description available to all active agencies"></i>';

                }
                else {
                    appendData += '<i class="" aria-hidden="true"  style="visibility:hidden;">N/A</i>';

                }
                

                

                appendData += ' </td>\
                                </tr>';



            });

            self.elements.lblTotalCount.html(data.TotalRecord);

            self.elements.tbodyDailyObservation.html(appendData);

            self.elements.tbodyDailyObservation.find('[data-toggle="tooltip"]').tooltip();


            $.each(self.elements.tbodyDailyObservation.find('tr'), function () {

                var $tdindex = $(this).find('td').length - 1;

                $(this).find('td:eq(' + ($tdindex - 1) + ')').find('.selectStatus').off('change').on('change', function () {

                    self.changeDailyObservationStatus(this);
                });

                $(this).find('td:eq(' + $tdindex + ')').find('.btn-edit-trans').off('click').on('click', function () {

                    self.editDailyObservation(this);

                });

                $(this).find('td:eq(' + $tdindex + ')').find('.btn-to-all').off('click').on('click', function () {
                    self.availDailyObservationAllAgency(this);
                });


            });

        }





        self.initializeElements();



        self.getTotalRecord(parseInt(self.elements.lblTotalCount.html()));


    },


    editDailyObservation: function (ele) {


        var $desc = $(ele).closest('td').closest('tr').find('td:eq(0)').find('p').html().trim();
        var $id = $(ele).siblings('#input-accesskey_' + $(ele).closest('td').closest('tr').index() + '').val();

        self.elements.textareamodalDesc.val($desc);
        self.elements.hiddenModalObsID.val($id);

        self.elements.modalDailyObservation.modal('show');

    },
    changeDailyObservationStatus: function (ele) {

        var $tr = $(ele).closest('tr');
        var rowIndex = $tr.index();
        var tdIndex = $tr.find('td').length - 1;
        var observationId = $tr.find('td:eq(' + tdIndex + ')').find('#input-accesskey_' + rowIndex + '').val();
        var $description = $tr.find('td:eq(0)').find('p').html();
        var $status = parseInt($(ele).val()) == 1;

        var dataArray = [];

        dataArray.push({ 'ObservationID': observationId, 'Description': $description, 'Status': $status })

        self.ajaxOptions.url = self.addDailyObservationLookupUrl;
        self.ajaxOptions.datatype = 'JSON';
        self.ajaxOptions.type = "POST";
        self.ajaxOptions.contentType = "application/json; charset=utf-8";
        self.ajaxOptions.async = true;
        self.dataParameters.DailyObservationList = dataArray;
        self.ajaxOptions.data = JSON.stringify({ 'dailyObservation': self.dataParameters, 'mode': self.healthCheckMode.Report });


        if ($(ele).val() == '0') {
            BootstrapDialog.show({
                title: 'Confirmation',
                message: '<p style="font-weight:normal;">You are about to change the status of description (<i>' + $description + '</i>) to Inactive.</p>\
                          <p>Click OK to Proceed.</p>',
                closable: false,
                closeByBackdrop: false,
                closeByKeyboard: false,
                buttons: [{
                    label: 'Cancel <span class="glossy-button-before"></span><span class="glossy-button-after"></span>',
                    cssClass: 'glossy-button-button button-red',
                    action: function (dialogRef) {
                        dialogRef.close(true);
                        $(ele).val(1);
                    }
                }, {
                    label: 'OK <span class="glossy-button-before"></span><span class="glossy-button-after"></span>',
                    cssClass: 'glossy-button-button button-green',
                    autospin: true,
                    action: function (dialogRef) {

                        self.showBusy(true);

                        self.ajaxCall(self.callbackaddDescription);

                        window.setTimeout(function () {
                            dialogRef.close(true);

                        }, 10);

                    }
                }]
            });

        }
        else {

            self.showBusy(true);

            self.ajaxCall(self.callbackaddDescription);
        }






    },
    availDailyObservationAllAgency:function(ele)
    {
        var $tr = $(ele).closest('tr');
        var rowIndex = $tr.index();
        var tdIndex = $tr.find('td').length - 1;
        var observationId = $tr.find('td:eq(' + tdIndex + ')').find('#input-accesskey_' + rowIndex + '').val();
        var $description = $tr.find('td:eq(0)').find('p').html();

        var dataArray = [];

        dataArray.push({ 'ObservationID': observationId, 'Description': $description, 'Status': true })

        self.ajaxOptions.url = self.availDailyObservationLookupAllAgencyUrl;
        self.ajaxOptions.datatype = 'JSON';
        self.ajaxOptions.type = "POST";
        self.ajaxOptions.contentType = "application/json; charset=utf-8";
        self.ajaxOptions.async = true;
        self.dataParameters.DailyObservationList = dataArray;
        self.ajaxOptions.data = JSON.stringify({ 'dailyObservation': self.dataParameters, 'mode': self.healthCheckMode.Report });

   


        BootstrapDialog.show({
            title: 'Confirmation',
            message: '<p style="font-weight:normal;">You are about to make the description (<i style="font-weight:normal;">'+$description+'</i>) available to all active agencies.</p>\
                        <p>Click OK to Proceed.</p>',
            closable: false,
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

                    self.showBusy(true);

                    self.ajaxCall(self.callbackavailDailyObservationAllAgency);

                    window.setTimeout(function () {
                        dialogRef.close(true);

                    }, 10);

                }
            }]
        });
    },
    callbackDeleteActivityLookup: function (data) {
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


    dailyHealthCheck.init();






});