/* ================================================================ 
This copyright notice must be kept untouched in this javascript file
 at all times.

The original version of this javascript and the associated stylesheet
is available at http://www.jlion.com/docs/jsThermometer.aspx
Copyright (c) 2005-2011 Joe Lynds. All rights reserved.
This javascript and the associated stylesheet may be modified in any 
way to fit your requirements.
=================================================================== */


    function jlionThermometer(MIN, MAX, IS_CURRENCY, INCLUDE_DECIMAL) {
    var mMIN = MIN;
    var mMAX = MAX;
    var mIS_CURRENCY = IS_CURRENCY;
    var mINCLUDE_DECIMAL = INCLUDE_DECIMAL;
    var BARTOP = 16
    var BARHEIGHT = 288;

    Number.prototype.formatNumber = function (c, d, t) { var n = this, c = isNaN(c = Math.abs(c)) ? 2 : c, d = d == undefined ? "," : d, t = t == undefined ? "." : t, s = n < 0 ? "-" : "", i = parseInt(n = Math.abs(+n || 0).toFixed(c)) + "", j = (j = i.length) > 3 ? j % 3 : 0; return s + (j ? i.substr(0, j) + t : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + t) + (c ? d + Math.abs(n - i).toFixed(c).slice(2) : ""); };

    this.Init = function () {

        if ($("#thermometer #bar").length == 0) {
            $("#thermometer").height(370);
            $("#thermometer").width(210);

            var oThermPosition = $("#thermometer").position();

            jQuery('<img/>', {
                id: "background",
                src: "../images/therm.gif"
            }).appendTo('#thermometer');

            jQuery('<div/>', {
                id: "label"
            }).appendTo('#thermometer');

            jQuery('<div/>', {
                id: "bar"
            }).appendTo('#thermometer');

            var oBarPosition = $("#thermometer #bar").position();
            var iBarLeft = oThermPosition.left + 25;
            var iBarTop = oThermPosition.top + 100;

            iBarLeft = 152;
            iBarTop = 100;

            $('#thermometer #bar').css('position', "absolute");
            $('#thermometer #bar').css('left', iBarLeft + 'px');
            $('#thermometer #bar').css('top', iBarTop + 'px');

            _DrawMajorTicks(BARHEIGHT);
            _ShowTitle('Thermometer Chart');

            _Refresh(0);
        }
    }

    this.RefreshByID = function (varID) {
        var dValue = $("#" + varID).attr('value');
        _Refresh(dValue);
    }

    this.RefreshByValue = function (dValue) {
        _Refresh(dValue);
    }

    function _Refresh(dValue) {
        var dBarValue;

        if (dValue < mMIN) dBarValue = mMIN
        else if (dValue > mMAX) dBarValue = mMAX
        else dBarValue = dValue;

        var dRange = mMAX - mMIN;

        var dPercent = dBarValue / dRange;
        
        var dNewHeight = (BARHEIGHT * dPercent);
        
        var iNewTop = BARTOP + (BARHEIGHT - dNewHeight);
        
        $('#thermometer #bar').css('top', iNewTop + 'px');
        $('#thermometer #bar').css('height', dNewHeight + 'px');
        //document.getElementById('bar').style.top = (iNewTop + 'px');
        //document.getElementById('bar').style.height = (dNewHeight + 'px');
            
        _ShowCurrentValue(dValue);
    }

    function _FormattedNumber(dNumber) {
        var Symbol = '';
        var Decimal = 2;

        if (mIS_CURRENCY == true) Symbol = '$ ';
        if (mINCLUDE_DECIMAL == false) Decimal = 0;

        return Symbol + (parseFloat(dNumber)).formatNumber(Decimal, '.', ',');
    }

    function _DrawMajorTicks(BARHEIGHT) {
        var oPosition = $("#thermometer #label").position();
        var iLabelLeft = oPosition.left;
        var iLabelWidth = $("#thermometer #label").width();

        var iMajorTickCount = 10;
        var iMajorTickHeight = BARHEIGHT / iMajorTickCount;

        var dValueInc = (MAX - MIN) / iMajorTickCount;

        for (i = 0; i < iMajorTickCount; i++) {
            var vID1 = 'majortick' + i;

            jQuery('<div/>', {
                id: vID1
            }).appendTo('#thermometer #label');

            $('#thermometer #' + vID1).css('border-top', 'solid 1px black');

            $('#thermometer #' + vID1).addClass('MajorTick');
            $('#thermometer #' + vID1).css('height', iMajorTickHeight);

            $('#thermometer #' + vID1).css('position', "absolute");
            $('#thermometer #' + vID1).css('left', (iLabelWidth - 40) + "px");
            $('#thermometer #' + vID1).css('top', (i * iMajorTickHeight) + 'px');

            //***************************************//

            var vIDLabel = 'ticklabel' + i;

            jQuery('<div/>', {
                id: vIDLabel
            }).appendTo('#thermometer #label');

            $('#thermometer #' + vIDLabel).addClass('TickLabel');

            var sNumber = _FormattedNumber((iMajorTickCount - i) * dValueInc + MIN);
            $('#thermometer #' + vIDLabel).text(sNumber);

            $('#thermometer #' + vIDLabel).css('height', iMajorTickHeight + 'px');
            $('#thermometer #' + vIDLabel).css('width', '95');

            $('#thermometer #' + vIDLabel).css('position', "absolute");
            $('#thermometer #' + vIDLabel).css('left', (iLabelWidth - 120) + "px");
            $('#thermometer #' + vIDLabel).css('top', (i * iMajorTickHeight) + 'px');
        }
    }

    function _ShowTitle(sTitle) {
        var iThermWidth = $("#thermometer").width();
        var vIDTitle = 'Title';

        jQuery('<div/>', {
            id: vIDTitle
        }).appendTo('#thermometer');

        $('#thermometer #' + vIDTitle).addClass('Title');
        $('#thermometer #' + vIDTitle).css('width', iThermWidth);
        $('#thermometer #' + vIDTitle).text(sTitle);

        $('#thermometer #' + vIDTitle).css('position', "absolute");
        $('#thermometer #' + vIDTitle).css('left', '0px');
        $('#thermometer #' + vIDTitle).css('top', '0px');
    }

    function _ShowCurrentValue(sValue) {
        var iTherheight = $("#thermometer").height();
        var vIDValue = 'CValue';

        jQuery('<div/>', {
            id: vIDValue
        }).appendTo('#thermometer');

        $('#thermometer #' + vIDValue).addClass('CurrentValue');
        $('#thermometer #' + vIDValue).css('height', iTherheight);

        $('#thermometer #' + vIDValue).text(_FormattedNumber(sValue));

        $('#thermometer #' + vIDValue).css('position', "absolute");
        $('#thermometer #' + vIDValue).css('left', '20px');
        $('#thermometer #' + vIDValue).css('top', (iTherheight - 60) + 'px');
    }

    this.Init();
}