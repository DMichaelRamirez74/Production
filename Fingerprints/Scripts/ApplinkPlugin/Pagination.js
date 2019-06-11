
(function ($) {

    $.fn.ApplinkPG = function (TotalRecord, params, callback) {  //[int],{PageSize:[int],RequestedPage:[int] },[function]
        console.log(TotalRecord, params, callback);
        
        var _target = $(this);
        var _pageSizeDrop = _target.find(".pagesize-dropdown-ApplinkPG");
        var _pagenoDrop = _target.find(".pageno-dropdown-ApplinkPG");
        var _first = _target.find('a#First');
        var _back = _target.find('a#Back');
        var _next = _target.find('a#Next');
        var _last = _target.find('a#Last');

        //_target.append('<span>tttt</span>');
        // _target.html('');
        if (!_target.html().trim()) {
            //    _target.append($("#pagination-temp").html());
            _target.append(_pgTemplate);

            //  _target.css("color", "green");


            _first = _target.find('a#First');
            _back = _target.find('a#Back');
            _next = _target.find('a#Next');
            _last = _target.find('a#Last');

            _pageSizeDrop = _target.find(".pagesize-dropdown-ApplinkPG");
            _pagenoDrop = _target.find(".pageno-dropdown-ApplinkPG");

            $(document).on("change", ".pagesize-dropdown-ApplinkPG", function (e) {
                //e.preventDefa
                // console.log(this, callback,TotalRecord);

                var _pageSize = $(this).val();
                callback(_pageSize, 1);
            });

            $(document).on("change", ".pageno-dropdown-ApplinkPG", function (e) {
              
                var _pageno = $(this).val();
                callback(params.PageSize, _pageno);

                var noOfPages = $(this).find('option').length;

                if (_pageno == 1) {
                    _first.attr('disabled', true);
                    _back.attr('disabled', true);
                    _next.attr('disabled', false);
                    _last.attr('disabled', false);
                }
                else if (_pageno == noOfPages) {

                    _first.attr('disabled', false);
                    _back.attr('disabled', false);
                    _next.attr('disabled', true);
                    _last.attr('disabled', true);
                }
                else {

                    _first.attr('disabled', false);
                    _back.attr('disabled', false);
                    _next.attr('disabled', false);
                    _last.attr('disabled', false);
                }


            });

            $(document).on("click", "#First,#Back,#Next,#Last", function (e) {
                e.preventDefault();
             
                var _type = $(this).prop('id');
                // var _curentPg = parseInt($("#ddlpaging").val());
                //var _lastpg = parseInt($("#ddlpaging option:last").val());
                var _curentPg = parseInt(_pagenoDrop.val());
                var _lastpg = parseInt(_pagenoDrop.find("option:last").val());



                switch (_type) {
                    case "First":
                        _curentPg = 1;
                        break;
                    case "Back":
                        _curentPg--;
                        break;
                    case "Next":
                        _curentPg++;
                        break;
                    case "Last":
                        _curentPg = _lastpg;
                        break;

                };

                // $("#ddlpaging").val(_curentPg);

                // self.refreshTagReport(_curentPg, $("#page-length-drop").val());
                _pagenoDrop.val(_curentPg);
                callback(_pageSizeDrop.val(), _curentPg);



                var noOfPages = _pagenoDrop.find('option').length;

                if (_curentPg == 1) {
                    _first.attr('disabled', true);
                    _back.attr('disabled', true);
                    _next.attr('disabled', false);
                    _last.attr('disabled', false);
                }
                else if (_curentPg == noOfPages) {

                    _first.attr('disabled', false);
                    _back.attr('disabled', false);
                    _next.attr('disabled', true);
                    _last.attr('disabled', true);
                }
                else {

                    _first.attr('disabled', false);
                    _back.attr('disabled', false);
                    _next.attr('disabled', false);
                    _last.attr('disabled', false);
                }


                //if (_curentPg == 1) {
                //    $("#Back").attr("disabled", true);
                //} else {
                //    $("#Back").attr("disabled", false);
                //}

                //if (_lastpg == _curentPg) {
                //    $("#Next").attr("disabled", true);
                //} else {
                //    $("#Next").attr("disabled", false);
                //}


            });

            _pageSizeDrop.val(params.PageSize);

        } else {
            //  _target.css("color", "red");
        }


        var _psize = parseInt(_pageSizeDrop.val());



        _first.attr('disabled', false);
        _back.attr('disabled', false);
        _next.attr('disabled', false);
        _last.attr('disabled', false);

        var _totalRecord = parseInt(TotalRecord);
        if (_totalRecord > 0) {



            if (_totalRecord <= _psize) {

                _first.attr('disabled', true);
                _back.attr('disabled', true);
                _next.attr('disabled', true);
                _last.attr('disabled', true);

            }


            var _pcount = Math.ceil(TotalRecord / _psize); //ceil- Round a number upward to its nearest integer:
            _pagenoDrop.html('');
            for (var i = 1; i <= _pcount; i++) {

                _pagenoDrop.append('<option value="' + i + '">' + i + '</option>')
            }
            _pagenoDrop.val(params.RequestedPage);
        }
        else {
            _first.attr('disabled', true);
            _back.attr('disabled', true);
            _next.attr('disabled', true);
            _last.attr('disabled', true);
        }
        return this;
    };

}(jQuery));







/******Template Section******/

var _pgTemplate = `<div class="col-xs-12">
        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12 pagination-file">
            <div class="pages_display">
                <ul>
                    <li style="">Display</li>
                    <li>
                        <select class="pagesize-dropdown-ApplinkPG">
                            <option value="10" selected="selected">10</option>
                            <option value="25">25</option>
                            <option value="50">50</option>
                            <option value="75">75</option>
                            <option value="100">100</option>
                        </select>
                    </li>
                    <li style="">Records Per Page</li>
                </ul>
                <div class="clear"></div>
            </div>
        </div>
        <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
            <div id="divPaging" class="pagination_wrp">
                <ul id="ulPaging" class="pagination">
                    <li>
                        <a href="javascript:void(0);" id="First" disabled>
                            <i class="fa fa-angle-double-left" aria-hidden="true" style="margin:auto;padding:-7px;"></i>
                        </a>

                    </li>
                    <li title="Back">
                        <a href="javascript:void(0);" id="Back" disabled>
                            <i class="fa fa-angle-left" aria-hidden="true" style="margin:auto;padding:-7px;"></i>
                        </a>

                    </li>
                    <li title="Select">
                        <select class="select_cl pageno-dropdown-ApplinkPG" ></select>
                    </li>
                    <li title="Next">
                        <a href="javascript:void(0);" id="Next" disabled>
                            <i class="fa fa-angle-right" aria-hidden="true" style="margin:auto;padding:-7px;"></i>
                        </a>
                    </li>
                    <li title="Last">
                        <a href="javascript:void(0);" id="Last" disabled>
                            <i class="fa fa-angle-double-right" aria-hidden="true" style="margin:auto;padding:-7px;"></i>
                        </a>
                    </li>
                </ul>
            </div>
        </div>
    </div>`
