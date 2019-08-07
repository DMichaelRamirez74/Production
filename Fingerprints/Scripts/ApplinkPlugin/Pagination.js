
!(function ($) {

    function getRandomNumber () {
        return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
            (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
        )

    }

    $.fn.ApplinkPG = function (option, parameter, extraOptions) {  //[int],{PageSize:[int],RequestedPage:[int] },[function]

        var data = $(this).data('applink-pagination');
        var options = typeof option === 'object' && option;

        // Initialize the multiselect.
        if (!data || option.bindInitialLoad) {
            data = new ApplinkPagination(this, options);
            $(this).data('applink-pagination', data);
            if(option.bindInitialLoad)
            {
                data.getRecord(data);
            }

        }

        // Call multiselect method.
        if (typeof option === 'string') {
            data[option](parameter, extraOptions);

            if (option === 'destroy') {
                $(this).data('applink-pagination', false);
            }
        }

        //var _target = $(this);
        //var _pageSizeDrop = _target.find(".pagesize-dropdown-ApplinkPG");
        //var _pagenoDrop = _target.find(".pageno-dropdown-ApplinkPG");
        //var _first = _target.find('a#First');
        //var _back = _target.find('a#Back');
        //var _next = _target.find('a#Next');
        //var _last = _target.find('a#Last');

        ////_target.append('<span>tttt</span>');
        //// _target.html('');
        //if (!_target.html().trim()) {
        //    //    _target.append($("#pagination-temp").html());
        //    _target.append(_pgTemplate);

        //    //  _target.css("color", "green");


        //    _first = _target.find('a#First');
        //    _back = _target.find('a#Back');
        //    _next = _target.find('a#Next');
        //    _last = _target.find('a#Last');

        //    _pageSizeDrop = _target.find(".pagesize-dropdown-ApplinkPG");
        //    _pagenoDrop = _target.find(".pageno-dropdown-ApplinkPG");

        //    $(document).on("change", ".pagesize-dropdown-ApplinkPG", function (e) {
        //        //e.preventDefa
        //        // console.log(this, callback,TotalRecord);

        //        var _pageSize = $(this).val();
        //        callback(_pageSize, 1);
        //    });

        //    $(document).on("change", ".pageno-dropdown-ApplinkPG", function (e) {

        //        var _pageno = $(this).val();
        //        callback(params.PageSize, _pageno);

        //        var noOfPages = $(this).find('option').length;

        //        if (_pageno == 1) {
        //            _first.attr('disabled', true);
        //            _back.attr('disabled', true);
        //            _next.attr('disabled', false);
        //            _last.attr('disabled', false);
        //        }
        //        else if (_pageno == noOfPages) {

        //            _first.attr('disabled', false);
        //            _back.attr('disabled', false);
        //            _next.attr('disabled', true);
        //            _last.attr('disabled', true);
        //        }
        //        else {

        //            _first.attr('disabled', false);
        //            _back.attr('disabled', false);
        //            _next.attr('disabled', false);
        //            _last.attr('disabled', false);
        //        }


        //    });

        //    $(document).on("click", "#First,#Back,#Next,#Last", function (e) {
        //        e.preventDefault();

        //        var _type = $(this).prop('id');
        //        // var _curentPg = parseInt($("#ddlpaging").val());
        //        //var _lastpg = parseInt($("#ddlpaging option:last").val());
        //        var _curentPg = parseInt(_pagenoDrop.val());
        //        var _lastpg = parseInt(_pagenoDrop.find("option:last").val());



        //        switch (_type) {
        //            case "First":
        //                _curentPg = 1;
        //                break;
        //            case "Back":
        //                _curentPg--;
        //                break;
        //            case "Next":
        //                _curentPg++;
        //                break;
        //            case "Last":
        //                _curentPg = _lastpg;
        //                break;

        //        };

        //        // $("#ddlpaging").val(_curentPg);

        //        // self.refreshTagReport(_curentPg, $("#page-length-drop").val());
        //        _pagenoDrop.val(_curentPg);
        //        callback(_pageSizeDrop.val(), _curentPg);



        //        var noOfPages = _pagenoDrop.find('option').length;

        //        if (_curentPg == 1) {
        //            _first.attr('disabled', true);
        //            _back.attr('disabled', true);
        //            _next.attr('disabled', false);
        //            _last.attr('disabled', false);
        //        }
        //        else if (_curentPg == noOfPages) {

        //            _first.attr('disabled', false);
        //            _back.attr('disabled', false);
        //            _next.attr('disabled', true);
        //            _last.attr('disabled', true);
        //        }
        //        else {

        //            _first.attr('disabled', false);
        //            _back.attr('disabled', false);
        //            _next.attr('disabled', false);
        //            _last.attr('disabled', false);
        //        }


        //        //if (_curentPg == 1) {
        //        //    $("#Back").attr("disabled", true);
        //        //} else {
        //        //    $("#Back").attr("disabled", false);
        //        //}

        //        //if (_lastpg == _curentPg) {
        //        //    $("#Next").attr("disabled", true);
        //        //} else {
        //        //    $("#Next").attr("disabled", false);
        //        //}


        //    });

        //    _pageSizeDrop.val(params.PageSize);

        //} else {
        //    //  _target.css("color", "red");
        //}


        //var _psize = parseInt(_pageSizeDrop.val());



        //_first.attr('disabled', false);
        //_back.attr('disabled', false);
        //_next.attr('disabled', false);
        //_last.attr('disabled', false);

        //var _totalRecord = parseInt(TotalRecord);
        //if (_totalRecord > 0) {



        //    if (_totalRecord <= _psize) {

        //        _first.attr('disabled', true);
        //        _back.attr('disabled', true);
        //        _next.attr('disabled', true);
        //        _last.attr('disabled', true);

        //    }


        //    var _pcount = Math.ceil(TotalRecord / _psize); //ceil- Round a number upward to its nearest integer:
        //    _pagenoDrop.html('');
        //    for (var i = 1; i <= _pcount; i++) {

        //        _pagenoDrop.append('<option value="' + i + '">' + i + '</option>')
        //    }
        //    _pagenoDrop.val(params.RequestedPage);
        //}
        //else {
        //    _first.attr('disabled', true);
        //    _back.attr('disabled', true);
        //    _next.attr('disabled', true);
        //    _last.attr('disabled', true);
        //}


        return this;
    };
    $.fn.ApplinkPG.Constructor = ApplinkPagination;


    ApplinkPagination.prototype = {
        
        defaults:{
       
   
        sortOrder: null,
        sortColumn: null,
        isEnableSorting: false,
        totalRecords: 0,
        data: null,
        dataType: null,
        getRecordUrl: null,
        bindInitialLoad:false,
        bindData:function(data)
        {

        },
       

        },
        getRecord: function (events) {

            var jsonData = events.options.data;
            jsonData = Object.assign({ 'SortOrder': events.options.sortOrder, 'SortColumn': events.options.sortColumn, 'RequestedPage': events.requestedPage, 'pageSize': events.pageSize }, jsonData);

            $.ajax({

                url: events.options.getRecordUrl,
                dataType: events.options.dataType,
                contentType: 'application/json',
                // data: JSON.stringify(jsonData),
                data: jsonData,
                async: false,
                beforeSend: function () {
                    $('#spinner').show();
                },
                success: function (data) {
                    events.options.bindData(events, data);

                    events.getTotalRecord(events);

                    $('[data-toggle="tooltip"]').tooltip();

                },
                error: function (data) {
                    console.log(data);

                },
                complete: function (data) {
                    $('#spinner').hide();
                }
            });

        },
        requestedPage: 1,
        pageSize: 10,

        getTotalRecord:function(events){

         
            //var paginationDiv = $(ele).find('#div-pagination-' + index + ''); //records per page div//
            //var dropdownRecordsPage = paginationDiv.find('#ddlpagetodisplay_' + index + '');
            //var pagingDiv = paginationDiv.find('#divPaging_' + index + ''); //pagination div//
            //var firstButton = pagingDiv.find('#First_' + index + '');
            //var backButton = pagingDiv.find('#Back_' + index + '');
            //var dropdownPageNumber = pagingDiv.find('#ddlpaging_' + index + '');
            //var nextButton = pagingDiv.find('#Next_' + index + '');
            //var lastButton = pagingDiv.find('#Last_' + index + '');
            //var _requestedPage = $(ele).find('#requestedPage_' + index + '');
            //var _sortOrder = $(ele).find('#sortOrder_' + index + '');
            //var _sortColumn = $(ele).find('#sortColumn_' + index + '');

            events.firstElement.attr('disabled', false);
            events.backElement.attr('disabled', false);
            events.nextElement.attr('disabled', false);
            events.lastElement.attr('disabled', false);

            var _pageSize = parseInt(events.recordsPerPageElement.val());

            if (events.options.totalRecords > 0) {



                if (events.options.totalRecords <= _pageSize) {


                    events.firstElement.attr('disabled', true);
                    events.backElement.attr('disabled', true);
                    events.nextElement.attr('disabled', true);
                    events.lastElement.attr('disabled', true);
                }

                var _numOfPages = parseInt(events.options.totalRecords / _pageSize) + ((events.options.totalRecords % _pageSize == 0) ? 0 : 1);

                // dropdownPageNumber.empty();

                events.pageElement.empty();

                for (i = 1; i <= _numOfPages; i++) {

                    var newOption = "<option value='" + i + "'>" + i + "</option>";
                    events.pageElement.append(newOption);
                }

                events.pageElement.val(events.requestedPage);
            }
            else {

                events.firstElement.attr('disabled', true);
                events.backElement.attr('disabled', true);
                events.nextElement.attr('disabled', true);
                events.lastElement.attr('disabled', true);
            }

        },
        changePage: function (event) {

            event.preventDefault();

            var ele = event.target;

            var $this = this;

            $('#spinner').show();

            window.setTimeout(function () {

                
                $this.pageLoadedFirst = 0;
                $this.pageSize =parseInt($this.recordsPerPageElement.val());

                //  tabEle.find('#pageLoadedFirst_' + index + '').val(0);



                // tabEle.find('#pageSize_' + index + '').val(tabEle.find('#ddlpagetodisplay_' + index + '').val());


                if ($(ele).closest('li').index() == 0) {   //First

                    $this.startIndex = 0;
                    $this.lastIndex = ($this.pageSize + ($this.lastIndex * $this.requestedPage));
                    $this.requestedPage = (($this.startIndex / 10)+1);
                    $this.lastIndex = 0;

                   // this.options.getRecord();
                        
                    //  $.proxy(this.getRecord, this);

                    $this.getRecord($this);

                    $this.firstElement.attr('disabled', true);
                    $this.backElement.attr('disabled', true);
                    $this.nextElement.attr('disabled', false);
                    $this.lastElement.attr('disabled', false);


                    //tabEle.find('#startIndex_' + index + '').val(0);

                    //                    var $lastindex = parseInt(parseInt(tabEle.find('#pageSize_' + index).val()) + (parseInt(tabEle.find('#lastIndex_' + index).val()) * parseInt(tabEle.find('#requestedPage_' + index + '').val())));

                    //               tabEle.find('#lastIndex_' + index + '').val($lastindex);

                    //    tabEle.find('#requestedPage_' + index + '').val(((parseInt(tabEle.find('#startIndex_' + index + '').val()) / 10) + 1))


                    //   self.getReport(tabEle, index, self.getlistMode.center);

                    //  tabEle.find('#First_' + index + '').attr('disabled', true);
                    //  tabEle.find('#Back_' + index + '').attr('disabled', true);
                    //  tabEle.find('#Next_' + index + '').attr('disabled', false);
                    //  tabEle.find('#Last_' + index + '').attr('disabled', false);

                    //   tabEle.find('#lastIndex_' + index + '').val(0);

                }
                else if ($(ele).closest('li').index() == 4) {  //Last

                    $this.startIndex = ((($this.options.totalRecords - 1) / $this.pageSize) * $this.pageSize);
                    $this.lastIndex = $this.options.totalRecords;
                    $this.requestedPage = parseInt($this.pageElement.children('option:last-child').val());
                    
                    $this.getRecord($this);

                  //  $.proxy(this.getRecord, this);
                  //  this.options.getRecord();

                    //     var $startIndex = parseInt(parseInt((parseInt(tabEle.find('#totalCountSpan_' + index).html()) - 1) / parseInt(tabEle.find('#pageSize_' + index).val())) * parseInt(tabEle.find('#pageSize_' + index).val()));
                    //     tabEle.find('#startIndex_' + index + '').val($startIndex);


                    //  tabEle.find('#lastIndex_' + index + '').val(parseInt(tabEle.find('#totalCountSpan_' + index).html()))

                    //  tabEle.find('#requestedPage_' + index + '').val(tabEle.find('#ddlpaging_' + index + '').children('option:last-child').val());


                    //  self.gotoNextPage(self.requestedPage, self.pageSize);

                    //    self.getReport(tabEle, index, self.getlistMode.center);


                    $this.firstElement.attr('disabled', false);
                    $this.backElement.attr('disabled', false);
                    $this.nextElement.attr('disabled', true);
                    $this.lastElement.attr('disabled', true);



                    //tabEle.find('#First_' + index + '').attr('disabled', false);
                    //   tabEle.find('#Back_' + index + '').attr('disabled', false);
                    //   tabEle.find('#Next_' + index + '').attr('disabled', true);
                    //   tabEle.find('#Last_' + index + '').attr('disabled', true);

                }
                else if ($(ele).closest('li').index() == 3) {  //Next

                    $this.lastIndex = ($this.pageSize + $this.lastIndex);
                    $this.requestedPage = (($this.lastIndex / $this.pageSize) + 1);
                    //$.proxy(this.getRecord, this);
                    $this.getRecord($this);
                  //  this.options.getRecord();

                    $this.firstElement.attr('disabled', false);
                    $this.backElement.attr('disabled', false);

                    if (($this.lastIndex + $this.pageSize) >= $this.options.totalRecords) {
                        $this.nextElement.attr('disabled', true);
                        $this.lastElement.attr('disabled', true);

                    }

                    else if (($this.lastIndex - $this.pageSize) < $this.options.totalRecords) {
                        $this.nextElement.attr('disabled', false);
                        $this.lastElement.attr('disabled', false);
                    }

                    //  var $lastIndex = parseInt(parseInt(tabEle.find('#pageSize_' + index + '').val()) + parseInt(tabEle.find('#lastIndex_' + index + '').val()));

                    //  tabEle.find('#lastIndex_' + index + '').val($lastIndex)


                    // var $reqpage = parseInt(((parseInt(tabEle.find('#lastIndex_' + index + '').val()) / parseInt(tabEle.find('#pageSize_' + index + '').val())) + 1));


                    // self.requestedPage = $reqpage;
                    //  tabEle.find('#requestedPage_' + index + '').val($reqpage);
                    //  self.gotoNextPage(self.requestedPage, self.pageSize);

                    //  self.getReport(tabEle, index, self.getlistMode.center);

                    //  tabEle.find('#First_' + index + '').attr('disabled', false);
                    //  tabEle.find('#Back_' + index + '').attr('disabled', false);

                    //if (parseInt(tabEle.find('#lastIndex_' + index + '').val()) + parseInt(tabEle.find('#pageSize_' + index + '').val()) >= parseInt(tabEle.find('#totalCountSpan_' + index).html())) {

                    //    tabEle.find('#Next_' + index + '').attr('disabled', true);
                    //    tabEle.find('#Last_' + index + '').attr('disabled', true);
                    //}
                    //else if (parseInt(tabEle.find('#lastIndex_' + index + '').val()) - parseInt(tabEle.find('#pageSize_' + index + '').val()) < parseInt(tabEle.find('#totalCountSpan_' + index).html())) {

                    //    tabEle.find('#Next_' + index + '').attr('disabled', false);
                    //    tabEle.find('#Last_' + index + '').attr('disabled', false);
                    //}


                }
                else if ($(ele).closest('li').index() == 1) { //Back

                    $this.requestedPage = ($this.requestedPage - 1);
                    $this.lastIndex = ($this.lastIndex - $this.pageSize);
                   // $.proxy(this.getRecord, this);
                    //  this.options.getRecord();
                    $this.getRecord($this);

                    if (($this.lastIndex + $this.pageSize) > $this.options.totalRecords) {
                        $this.nextElement.attr('disabled', true);
                        $this.lastElement.attr('disabled', true);
                    }

                    else if (($this.lastIndex - $this.pageSize) < $this.options.totalRecords) {
                        $this.nextElement.attr('disabled', false);
                        $this.lastElement.attr('disabled', false);
                    }

                    if ($this.requestedPage == 1) {
                        $this.firstElement.attr('disabled', true);
                        $this.backElement.attr('disabled', true);
                    }

                    // tabEle.find('#requestedPage_' + index + '').val((parseInt(tabEle.find('#requestedPage_' + index + '').val()) - 1));

                    //   tabEle.find('#lastIndex_' + index + '').val((parseInt(tabEle.find('#lastIndex_' + index + '').val()) - parseInt(tabEle.find('#pageSize_' + index + '').val())));


                    //   self.gotoNextPage(self.requestedPage, self.pageSize);

                    //   self.getReport(tabEle, index, self.getlistMode.center);

                    //if (parseInt(tabEle.find('#lastIndex_' + index + '').val()) + parseInt(tabEle.find('#pageSize_' + index + '').val()) > parseInt(tabEle.find('#totalCountSpan_' + index).html())) {

                    //    tabEle.find('#Next_' + index + '').attr('disabled', true);
                    //    tabEle.find('#Last_' + index + '').attr('disabled', true);

                    //}
                    //else if (parseInt(tabEle.find('#lastIndex_' + index + '').val()) - parseInt(tabEle.find('#pageSize_' + index + '').val()) < parseInt(tabEle.find('#totalCountSpan_' + index).html())) {
                    //    tabEle.find('#Next_' + index + '').attr('disabled', false);
                    //    tabEle.find('#Last_' + index + '').attr('disabled', false);

                    //}
                    //if (parseInt(tabEle.find('#requestedPage_' + index + '').val()) == 1) {
                    //    tabEle.find('#First_' + index + '').attr('disabled', true);
                    //    tabEle.find('#Back_' + index + '').attr('disabled', true);
                    //}
                }
                else {
                }

            }, 10)


        },
        pageNumbersChange: function (event) {
            event.preventDefault();
            var ele = event.target;

            this.requestedPage =parseInt($(ele).val());
            var $this = this;

            this.getListafterupdation(event);


        },
        firButtonClick: function (event) {
            event.preventDefault();

            var firstButton = $(event.target);

            if (firstButton.attr('disabled') == "disabled")
                return false;

            //var index = $(this).attr('id').split('_')[1];
            //var tabEle = $('#ddlpagetodisplay_' + index + '').closest('#div-pagination-' + index + '').closest('.tab-pane');
            //self.fnChangePage(self.pageChangeType.first, tabEle, index);

            this.changePage(event);
        },
        backButtonClick:function(event)
        {
            event.preventDefault();

            var backButton = $(event.target);

            if (backButton.attr('disabled') == "disabled")
                return false;

            //var index = $(this).attr('id').split('_')[1];
            //var tabEle = $('#ddlpagetodisplay_' + index + '').closest('#div-pagination-' + index + '').closest('.tab-pane');
            //self.fnChangePage(self.pageChangeType.back, tabEle, index);

            this.changePage(event);

        },

        nextButtonClick: function (event) {
            event.preventDefault();

            var nextButton = $(event.target);

            if (nextButton.attr('disabled') == "disabled")
                return false;

            //     var index = $(this).attr('id').split('_')[1];
            //    var tabEle = $('#ddlpagetodisplay_' + index + '').closest('#div-pagination-' + index + '').closest('.tab-pane');
            // self.fnChangePage(self.pageChangeType.next, tabEle, index);

            this.changePage(event);

        },
        lastButtonClick: function (event) {
            event.preventDefault();

            var lastButton = $(event.target);

            if (lastButton.attr('disabled') == "disabled")
                return false;

            //var index = $(this).attr('id').split('_')[1];
            //var tabEle = $('#ddlpagetodisplay_' + index + '').closest('#div-pagination-' + index + '').closest('.tab-pane');

            //self.fnChangePage(self.pageChangeType.last, tabEle, index);

            this.changePage(event);


        },
        recordsPerPageChange: function (event) {
            event.preventDefault();

            var recordPerPage = $(event.target);

            //  var index = $(this).attr('id').split('_')[1];

            //   var tabEle = $('#ddlpagetodisplay_' + index + '').closest('#div-pagination-' + index + '').closest('.tab-pane');

            this.lastIndex = 0;
            this.pageSize = parseInt(recordPerPage.val());

            //  $('#lastIndex_' + index + '').val(0);

            //   tabEle.find('#pageSize_' + index + '').val($(this).val());

            //   self.showBusy(true);
            var $this = this;
            $('#spinner').show();
            window.setTimeout(function () {



                $this.getRecord($this);

                //$.proxy($this.getRecord, this);
                // self.getReport(tabEle, index, self.getlistMode.center);
               // this.options.getRecord();
                $this.firstElement.attr('disabled', true);
                $this.backElement.attr('disabled', true);
            }, 10);

        },

        getListafterupdation: function (event) {
            event.preventDefault();

            var ele = event.target;

            this.pageSize = parseInt(this.recordsPerPageElement.val());
            this.requestedPage =parseInt( $(ele).val());
            this.startIndex = (((this.pageSize * this.requestedPage) - 1) + 1);
            this.lastIndex = ((this.pageSize * this.requestedPage) - this.pageSize);

            // ele.find('#pageSize_' + index + '').val(ele.find('#ddlpagetodisplay_' + index + '').val())

            //  ele.find('#requestedPage_' + index + '').val(ele.find('#ddlpaging_' + index + '').val());

            //  var $startIndex = parseInt((parseInt(ele.find('#pageSize_' + index + '').val()) * (parseInt(ele.find('#requestedPage_' + index + '').val()) - 1)) + 1);

            //ele.find('#startIndex_' + index + '').val($startIndex);

            //  var $lastIndex = parseInt((parseInt(ele.find('#pageSize_' + index + '').val()) * parseInt(ele.find('#requestedPage_' + index + '').val())) - parseInt(ele.find('#pageSize_' + index + '').val()));


            // ele.find('#lastIndex_' + index + '').val($lastIndex);

             //  $.proxy(this.getRecord, this);

            
            this.getRecord(this);

          //  this.options.getRecord();

            //  self.getReport(ele, index, self.getlistMode.center);

            //self.getReport();

            if (this.requestedPage == 1) {
                this.firstElement.attr('disabled', true);
                this.backElement.attr('disabled', true);
                this.nextElement.attr('disabled', false);
                this.lastElement.attr('disabled', false);
            }
            else if (this.requestedPage == parseInt(this.pageElement.children('option:last-child').val())) {

                this.firstElement.attr('disabled', false);
                this.backElement.attr('disabled', false);
                this.nextElement.attr('disabled', true);
                this.lastElement.attr('disabled', true);
            }
            else {

                this.firstElement.attr('disabled', false);
                this.backElement.attr('disabled', false);
                this.nextElement.attr('disabled', false);
                this.lastElement.attr('disabled', false);
            }



        },
        mergeOptions: function (options) {
            return $.extend(true, {}, this.defaults, this.options, options);
        },
        bindPagenumbers: function () {


            if (this.totalRecords > 0) {



                if (this.totalRecords <= this.pageSize) {

                    this.firstElement.attr('disabled', true);
                    this.backElement.attr('disabled', true);
                    this.nextElement.attr('disabled', true);
                    this.lastElement.attr('disabled', true);
                }


                var _pcount = Math.ceil(this.totalRecords / this.pageSize); //ceil- Round a number upward to its nearest integer:
                this.pageElement.html('');
                for (var i = 1; i <= _pcount; i++) {

                    this.pageElement.append('<option value="' + i + '">' + i + '</option>')
                }
                this.pageElement.val(this.requestedPage);
            }

        },
        searchData: function (dataParameter)
        {
          
            
            this.requestedPage = 1;
            this.recordsPerPageElement.val('10');
            this.pageSize = 10;
            this.options.data = $.extend({}, this.options.data, dataParameter)
            this.getRecord(this);
        },
        paginationIndex: null,
        constructor: ApplinkPagination,


        recordsPerPageElement: null,
        firstElement: null,
        backElement: null,
        nextElement: null,
        pageElement: null,
        lastElement: null,

      
        requestedPage: 1,
        pageSize: 10,
        startIndex: 0,
        lastIndex: 0,
        pageLoadedFirst: 0,
        numOfPages: 0,
      




    }

    function ApplinkPagination(pager, options) {
        this.$select = $(pager);
        this.options = this.mergeOptions($.extend({}, options, this.$select.data()));
        this.paginationIndex = getRandomNumber();

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

        this.recordsPerPageElement.change($.proxy(this.recordsPerPageChange,this));
        this.firstElement.click($.proxy(this.firButtonClick, this));
        this.backElement.click($.proxy(this.backButtonClick, this));
        this.nextElement.click($.proxy(this.nextButtonClick,this));
        this.lastElement.click($.proxy(this.lastButtonClick, this));

        var $this = this;

        if (this.options.isEnableSorting) {
            $('#table_'+this.paginationIndex+' thead>tr>th').each(function () {

                $(this).css('cursor', 'pointer');
            });
            $('#table_' + this.paginationIndex + ' thead>tr>th').on('click', function () {

               
                var th = this;

                var $thisIndex = $(th).index();
                            if ($(th).find('i').length > 0) {
                               

                                $(th).closest('tr').find("th:not(:eq("+$thisIndex+"))").find('i').css('display', 'none');

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

        //// Placeholder via data attributes
        //if (this.$select.attr("data-placeholder")) {
        //    this.options.nonSelectedText = this.$select.data("placeholder");
        //}

        // Initialization.
        // We have to clone to create a new reference.
        //this.originalOptions = this.$select.clone()[0].options;
        //this.query = '';
        //this.searchTimeout = null;
        //this.lastToggledInput = null;

        //this.options.multiple = this.$select.attr('multiple') === "multiple";
        //this.options.onChange = $.proxy(this.options.onChange, this);
        //this.options.onSelectAll = $.proxy(this.options.onSelectAll, this);
        //this.options.onDeselectAll = $.proxy(this.options.onDeselectAll, this);
        //this.options.onDropdownShow = $.proxy(this.options.onDropdownShow, this);
        //this.options.onDropdownHide = $.proxy(this.options.onDropdownHide, this);
        //this.options.onDropdownShown = $.proxy(this.options.onDropdownShown, this);
        //this.options.onDropdownHidden = $.proxy(this.options.onDropdownHidden, this);
        //this.options.onInitialized = $.proxy(this.options.onInitialized, this);
        //this.options.onFiltering = $.proxy(this.options.onFiltering, this);

        // Build select all if enabled.
        //this.buildContainer();
        //this.buildButton();
        //this.buildDropdown();
        //this.buildSelectAll();
        //this.buildDropdownOptions();
        //this.buildFilter();

        //this.updateButtonText();
        //this.updateSelectAll(true);

        //if (this.options.enableClickableOptGroups && this.options.multiple) {
        //    this.updateOptGroups();
        //}

        //this.options.wasDisabled = this.$select.prop('disabled');
        //if (this.options.disableIfEmpty && $('option', this.$select).length <= 0) {
        //    this.disable();
        //}

        //this.$select.wrap('<span class="multiselect-native-select" />').after(this.$container);
        //this.options.onInitialized(this.$select, this.$container);

       

    }
}(jQuery));






