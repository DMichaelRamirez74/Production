!(function ($) {

    function getRandomNumber() {
        return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
            (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
        )

    }

    $.fn.ApplinkCaseNote = function (option, parameter, extraOptions) {

    }

    $.fn.ApplinkCaseNote.constructor = ApplinkCaseNote;





    ApplinkCaseNote.prototype = {
        defaults: {
            isPopup: true,
            caseNoteType: 1,   //1- New/Edit 2- Append //
            getCaseNoteUrl: null,
            caseNoteId: null,
            householdId: null,
            clientId: null,

            caseNoteValidation: function (events) {

            },
            caseNoteSubmit:function(events)
            {

            },
            caseNoteClear:function(events)
            {

            },
            caseNoteCancel:function(events)
            {

            },
            addMoreAttachment: function (events) {

            },
            removeAttachment:function(events)
            {

            },
            getCaseNoteHtml:function(events)
            {

            }


        },
        caseNoteDateElement: null,
        caseNoteTitleElement: null,
        caseNoteNotesElement: null,
        caseNoteTagsElement: null,
        caseNoteDivClientsElement: null,
        caseNoteDivStaffsElement: null,
        caseNoteSecurityCheckBox: null,
        caseNoteAttachmentDiv: null,
        caseNoteAdditonAttachmentDiv: null,
        caseNoteImageGalleryDiv: null,
        
    }



    function ApplinkPagination(div, options) {
        
        this.$div = $(div);
        this.options = this.mergeOptions($.extend({}, options, this.$div.data()));
        this.paginationIndex = getRandomNumber();

        var appendDiv = '';

        if (this.options.isPopup)
        {
            if (this.options.caseNoteType==1)
            {

//                appendDiv+='<div class="modal fade scroll-modal custom-modal" id="ModalAddCasenote" role="dialog">\
//     <div class="modal-dialog modal-dialog-ch">\
//         <div class="modal-content">\
//             <div class="modal-body">\
//                 <button type="button" class="close" data-dismiss="modal"><img src="~/Content/CaseNote/images/close.png" /></button>\
//                 <h2 id="CaseNoteHeading" class="extra-title muted">Case Note</h2>\
//                 <div class="col-sm-12 col-xs-12 pull-right" style="padding-right:0;display:none;">\
//                     <div class="btnwrp_subcal" style="padding:0; margin:0;">\
//                         <h3 id="CaseNoteTitle" class="text-center pull-left"><span class="extra-title muted"></span></h3>\
//                         <input id="ClientId" type="hidden" name="ClientId" value="@Model.Enc_ClientID" />\
//                         <input id="CenterId" type="hidden" name="CenterId" value="@Model.CenterID" />\
//                         <input id="ProgramId" type="hidden" name="ProgramId" value="@Model.ProgramId" />\
//                         <input id="HouseHoldId" type="hidden" name="HouseHoldId" value="@Model.Enc_HouseholdID" />\
//                         <input id="Classroomid" type="hidden" name="Classroomid" value="@Model.ClassroomID" />\
//                         <input id="CaseNoteid" type="hidden" name="CaseNoteid" value="0" />\
//                         <div class="clear"></div>\
//                     </div>\
//                 </div>\
               
                

//         </div>



//     </div>
// </div>
//</div>'
            }

        }

        this.$select.prev('div').addClass('div_table_responsive_' + this.paginationIndex + '');
        $('.div_table_responsive_' + this.paginationIndex + '').find('table.glossy-table.table-striped').attr('id', 'table_' + this.paginationIndex + '');

        $('.div_table_responsive_' + this.paginationIndex + '').find('#table_' + this.paginationIndex + '').find('.glossy-table-body').attr('id', 'tbody_' + this.paginationIndex + '');
        var template = '<div class="col-xs-12">\
        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12 pagination-file">\
            <div class="pages_display">\
                <ul>\
                    <li style="">Display</li>\
                    <li>\
                        <select class="pagesize-dropdown-ApplinkPG_'+ this.paginationIndex + '">\
                            <option value="10" selected="selected">10</option>\
                            <option value="25">25</option>\
                            <option value="50">50</option>\
                            <option value="75">75</option>\
                            <option value="100">100</option>\
                        </select>\
                    </li>\
                    <li style="">Records Per Page</li>\
                </ul>\
                <div class="clear"></div>\
            </div>\
        </div>\
        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">\
            <div id="divPaging" class="pagination_wrp">\
                <ul id="ulPaging" class="pagination">\
                    <li for="First_' + this.paginationIndex + '">\
                        <a href="javascript:void(0);" id="First_'+ this.paginationIndex + '" disabled>\
                            <i class="fa fa-angle-double-left" aria-hidden="true" style="margin:auto;padding:-7px;"></i>\
                        </a>\
                    </li>\
                    <li title="Back" for="Back_' + this.paginationIndex + '">\
                        <a href="javascript:void(0);" id="Back_'+ this.paginationIndex + '" disabled>\
                            <i class="fa fa-angle-left" aria-hidden="true" style="margin:auto;padding:-7px;"></i>\
                        </a>\
                    </li>\
                    <li title="Select">\
                        <select class="select_cl pageno-dropdown-ApplinkPG_'+ this.paginationIndex + '" ></select>\
                    </li>\
                    <li title="Next" for="Next_' + this.paginationIndex + '">\
                        <a href="javascript:void(0);" id="Next_'+ this.paginationIndex + '" disabled>\
                            <i class="fa fa-angle-right" aria-hidden="true" style="margin:auto;padding:-7px;"></i>\
                        </a>\
                    </li>\
                    <li title="Last" for="Last_' + this.paginationIndex + '" >\
                        <a href="javascript:void(0);" id="Last_'+ this.paginationIndex + '" disabled>\
                            <i class="fa fa-angle-double-right" aria-hidden="true" style="margin:auto;padding:-7px;"></i>\
                        </a>\
                    </li>\
                </ul>\
            </div>\
        </div>\
    </div>';



        this.$select.html(template);

        this.recordsPerPageElement = this.$select.find('.pagesize-dropdown-ApplinkPG_' + this.paginationIndex);
        this.firstElement = this.$select.find('#First_' + this.paginationIndex);
        this.backElement = this.$select.find('#Back_' + this.paginationIndex);

        this.nextElement = this.$select.find('#Next_' + this.paginationIndex);
        this.lastElement = this.$select.find('#Last_' + this.paginationIndex);
        this.pageElement = this.$select.find('.pageno-dropdown-ApplinkPG_' + this.paginationIndex);
        this.totalRecords = this.options.totalRecords;

        this.bindPagenumbers();


        this.pageElement.change($.proxy(this.pageNumbersChange, this));

        this.recordsPerPageElement.change($.proxy(this.recordsPerPageChange, this));
        this.firstElement.click($.proxy(this.firButtonClick, this));
        this.backElement.click($.proxy(this.backButtonClick, this));
        this.nextElement.click($.proxy(this.nextButtonClick, this));
        this.lastElement.click($.proxy(this.lastButtonClick, this));

        var $this = this;

        if (this.options.isEnableSorting) {
            $('#table_' + this.paginationIndex + ' thead>tr>th').each(function () {

                $(this).css('cursor', 'pointer');
            });
            $('#table_' + this.paginationIndex + ' thead>tr>th').on('click', function () {


                var th = this;

                var $thisIndex = $(th).index();
                if ($(th).find('i').length > 0) {


                    $(th).closest('tr').find("th:not(:eq(" + $thisIndex + "))").find('i').css('display', 'none');

                    //$.each($('#table_' + this.paginationIndex + ' thead>tr>th'), function () {

                    //    if ($(this).index() != $thisIndex)
                    //    {
                    //        $(this).find('i').hide();

                    //    }

                    //});


                    $this.options.sortColumn = $(th).children('span').attr('col-name');

                    if ($(th).find('i').is(':visible')) {
                        if ($(th).find('.i-asc').is(':visible')) {

                            //   $(tabContent).find('#sortOrder_' + index + '').val("DESC");
                            $this.options.sortOrder = 'DESC';


                            //$(th).find('.i-asc').hide();
                            //$(th).find('.i-desc').show();


                            $(th).find('.i-asc,.i-desc').toggle();
                        }
                        else if ($(th).find('.i-desc').is(':visible')) {

                            //  $(tabContent).find('#sortOrder_' + index + '').val("ASC");
                            $this.options.sortOrder = 'ASC';


                            $(th).find('.i-asc,.i-desc').toggle();

                            //$(th).find('.i-asc').show();
                            //$(th).find('.i-desc').hide();
                        }
                    }
                    else {
                        // $(tabContent).find('#sortOrder_' + index + '').val("ASC");
                        $this.options.sortOrder = "ASC";
                        $(th).find('.i-asc').show();
                    }

                    $this.requestedPage = $this.pageElement.val();
                    $this.pageSize = $this.recordsPerPageElement.val();

                    //    var $reqPage = $(tabContent).find('#div-pagination-' + index + '').find('#ddlpaging_' + index + '').val();
                    //   var $pageSize = $(tabContent).find('#div-pagination-' + index + '').find('#ddlpagetodisplay_' + index + '').val();
                    //   $(tabContent).find('#requestedPage_' + index + '').val($reqPage);
                    //  $(tabContent).find('#pageSize_' + index + '').val($pageSize);

                    //  self.requestedPage = self.elements.dropdownPageNumber.val() == null ? self.requestedPage : self.elements.dropdownPageNumber.val();

                    // self.pageSize = self.elements.dropdownRecordsPerpage.val();

                    // var $tabContent = $('#myTabContent').find('.tab-pane.active');

                    //  var $tabindex = $($tabContent).attr('id').replace('tab', "").trim();




                    //self.showBusy(true);
                    $('#spinner').show();
                    window.setTimeout(function () {

                        // self.getReport($tabContent, $tabindex, self.getlistMode.center);

                        $this.getRecord($this);

                    }, 10);




                    // self.getUnscheduledClassDaysList();
                }
                else {
                    return false;
                }



            });
        }

       
    }
    

    


}(JQuery));