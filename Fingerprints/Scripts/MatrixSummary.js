@{
    ViewBag.Title = "MatrixSummary";
    Layout = "~/Views/Shared/AgencyStaffLayout.cshtml";
}

@section MainContentHolder{
    
    <link href="~/Content/css/MatrixSummary.css" rel="stylesheet" />

        <style>
            .bar12
    {
        float: left;
        position: relative;
        bottom: 0;
        z-index: -90;
        display: block;
        height: 100%;
        margin: 0 40px;
        bottom: 0;
        height: 100%;
        width: 37px;
        background-color: #8297AA;
        transform: rotateX(17.7deg) rotateY(-19deg);
        -webkit-transform: rotateX(16.7deg) rotateY(-19deg);
        -o-transform: rotateX(17.7deg) rotateY(-19deg);
        -moz-transform: rotateX(17.7deg) rotateY(-19deg);
        -ms-transform: rotateX(17.7deg) rotateY(-19deg);
    }

    /*on-hover Assessment Group Text*/

    .group-tooltip .tooltiptext {
        visibility: hidden;
        width: 120px;
        background-color: black;
        color: #fff;
        text-align: center;
        border-radius: 6px;
        padding: 5px 0;
        position: absolute;
        z-index: 1;
        top: 120%;
        left: 50%;
        margin-left: -60px;
        line-height: 25px;
    }

    .group-tooltip .tooltiptext::after {
        content: "";
        position: absolute;
        bottom: 100%;
        left: 50%;
        margin-left: -5px;
        border-width: 5px;
        border-style: solid;
        border-color: transparent transparent black transparent;
    }

    .group-tooltip:hover .tooltiptext {
        visibility: visible;
    }

    /*on-hover Description Text*/
    .desc-tooltip .tooltipdesctext {
        visibility: hidden;
        width: 250px;
        background-color: black;
        color: #fff;
        text-align: center;
        border-radius: 6px;
        padding: 5px 0;
        position: absolute;
        z-index: 1;
        top: 90%;
        left: 33%;
        margin-left: -60px;
        line-height: 25px;
    }

    .desc-tooltip .tooltipdesctext::after {
        content: "";
        position: absolute;
        bottom: 100%;
        left: 50%;
        margin-left: -5px;
        border-width: 5px;
        border-style: solid;
        border-color: transparent transparent black transparent;
    }

    .desc-tooltip:hover .tooltipdesctext {
        visibility: visible;
    }

    /*on-hover Question Text*/
    .qn-tooltip .tooltipqntext {
        visibility: hidden;
        width: 250px;
        background-color: black;
        color: #fff;
        text-align: center;
        border-radius: 6px;
        padding: 5px 0;
        position: absolute;
        z-index: 1;
        top: 90%;
        left: 33%;
        margin-left: -60px;
        line-height: 25px;
    }

    .qn-tooltip .tooltipqntext::after {
        content: "";
        position: absolute;
        bottom: 100%;
        left: 50%;
        margin-left: -5px;
        border-width: 5px;
        border-style: solid;
        border-color: transparent transparent black transparent;
    }

    .qn-tooltip:hover .tooltipqntext {
        visibility: visible;
    }

    </style>
<div class="container-fluid no-padding">
    <div class="row">
        <div class="col-xs-12 no-padding">
            <div class="col-xs-12 no-padding">
                <div class="top-header-title" style="margin-top: 30px;">
                    <h1>MATRIX SUMMARY</h1>
                </div>
            </div>
        </div>
        <div class="col-xs-12 no-padding">
            <div class="col-xs-12 select-block no-padding">
                <div class="col-lg-6 col-md-8 col-sm-8 col-xs-12" style="position:relative;">
                    <select name="" id="yearSelect" class="matrix-select">
                        <option value="0">--Select the program year--</option>
                    </select>
                   
                </div>
                <div class="col-lg-2 col-md-3 col-sm-4 col-xs-12">
                    <select name="" class="matrix-select hidden">
                        <option value="0">Year 2015</option>
                        <option value="1">Year 2016</option>
                        <option value="2" selected>Year 2017</option>
                    </select>
                </div>
            </div>

            <div class="col-xs-12 no-padding">
                <div class="col-lg-4 col-md-4 col-sm-6 col-xs-12 block assment-block-1">
                    <div class="assment-block ">
                        <h3>ASSESSMENT 1<span style="color:#f3f6f8;display:inline-block;width:auto;padding-left:8px;" class="as1-per">(93%)</span></h3>
                        <div class="survey-3d-green survey-3d-green-ht master-bar1">
                            <div class="bar-margin"></div>
                            <div class="bar-label2"><p class="avg-p as1-avg">0<sub>Avg</sub></p></div>
                            <!--new-->
                            <div class="bar12">
                                <div class="bar1a"></div>
                                <div class="bar1b"></div>
                            </div>
                            <div class="bar-green hidden mastbar-green1">
                                <div class="bar-in1">
                                    <div class="bar-in1a"></div>
                                    <div class="bar-in1b"></div>
                                </div>
                            </div>
                            <!---new -->
                        </div>

                       

                    </div>
                </div>
                <div class="col-lg-4 col-md-4 col-sm-6 col-xs-12 block assment-block-2">
                    <div class="assment-block">
                        <h3>ASSESSMENT 2<span style="color:#f3f6f8;display:inline-block;width:auto;padding-left:8px;" class="as2-per">(93%)</span></h3>
                        <div class="survey-3d-green survey-3d-green-ht master-bar2">
                            <div class="bar-margin"></div>
                            <div class="bar-label2"><p class="avg-p as2-avg">0<sub>Avg</sub></p></div>
                            <!---new -->
                            <div class="bar1">
                                <div class="bar1a"></div>
                                <div class="bar1b"></div>
                            </div>
                            <div class="bar-green hidden mastbar-green2">
                                <div class="bar-in1">
                                    <div class="bar-in1a"></div>
                                    <div class="bar-in1b"></div>
                                </div>
                            </div>
                            <!--new-->
                        </div>

                      

                    </div>
                </div>
                <div class="col-lg-4 col-md-4 col-sm-6 col-xs-12 block assment-block-3">
                    <div class="assment-block">
                        <h3>ASSESSMENT 3<span style="color:#f3f6f8;display:inline-block;width:auto;padding-left:8px;"  class="as3-per">(93%)</span></h3>
                        <div class="survey-3d-green survey-3d-green-ht master-bar3">
                            <div class="bar-margin"></div>
                            <div class="bar-label2"><p class="avg-p as3-avg">0<sub>Avg</sub></p></div>
                            <!--new-->
                            <div class="bar1">
                                <div class="bar1a"></div>
                                <div class="bar1b"></div>
                            </div>
                            <div class="bar-green hidden mastbar-green3">
                                <div class="bar-in1">
                                    <div class="bar-in1a"></div>
                                    <div class="bar-in1b"></div>
                                </div>
                            </div>
                            <!--new-->
                        </div>
                       

                    </div>
                </div>
                </div>

            <div class="chart-report block col-xs-12 no-padding">


           </div>

            </div>

        </div>
    </div>

   

    <!----------------Chart block-div--------->
<div class="col-xs-12  chart-block-div hidden">

    <div class="col-xs-12 no-padding cat-block category-div" cat-id="0">

        <div class="div-group-summary popup-display-overlay"></div>
        <div class="div-question-summary popup-display-overlay"></div>

        <div class="col-lg-4 col-xs-12 z-index-change">
            <div class="col-xs-12 no-padding">
                <div class="matrix-header-title">
                    <h1>MATRIX SURVEY</h1>
                </div>
            </div>

            <div class="col-sm-12 no-padding block-div float-div height-auto" style="background: #f1f5f8;border-radius: 5px;float: left;width: 100%;float: left;width: 100%;">
                <div class="col-lg-2 no-padding survey-text change-div height-auto">
                        <h2 class="cat-name" cat-id="0" style="text-transform:uppercase;"></h2>
                    </div>
                    <div class="col-lg-10 survey-block change-div group-section">

                    </div>
                </div>
            </div>
        <div class="col-lg-6 col-xs-12 no-padding">
            <div class="col-xs-12 no-padding matrix-header-title2">
                <div class="col-sm-4 no-padding">
                    <div class="matrix-header-title1" style="background: none;">
                        <h1>ASSESSMENT 1</h1>
                    </div>
                </div>
                <div class="col-sm-4 no-padding">
                    <div class="matrix-header-title1" style="background: none;">
                        <h1>ASSESSMENT 2</h1>
                    </div>
                </div>
                <div class="col-sm-4 no-padding">
                    <div class="matrix-header-title1" style="background: none;">
                        <h1>ASSESSMENT 3</h1>
                    </div>
                </div>
            </div>
            <div class="col-sm-12 no-padding block-div height-auto change-div" style="background: #f1f5f8;border-radius: 5px;margin-bottom:20px;float: left; width: 100%;overflow: hidden;">
                <div class="col-sm-4">
                    <div class="col-xs-12 no-padding">
                        <div class="col-sm-4 col-xs-3 no-padding margin-change as1-matrix-value-block">

                        </div>
                        <div class="col-sm-8 col-xs-4 pad-change">
                            <div class="bar-label bar-label_1"><p></p></div>
                            <div class="survey-3d-green">
                                <div class="bar-margin"></div>
                                <!--new -->
                                <div class="bar">
                                    <div class="bara"></div>
                                    <div class="barb"></div>
                                </div>
                                <div class="bar-green hidden  bar_green1">
                                    <div class="bar-in1">
                                        <div class="bar-in1a"></div>
                                        <div class="bar-in1b"></div>
                                    </div>
                                </div>
                                <!--new-->

                            </div>

                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="col-xs-12 no-padding">
                        <div class="col-sm-4 col-xs-3 no-padding margin-change as2-matrix-value-block">

                        </div>
                        <div class="col-sm-8 col-xs-4 pad-change">
                            <div class="bar-label bar-label_2"><p></p></div>
                            <div class="survey-3d-green">
                                <div class="bar-margin"></div>
                                <!--new -->
                                <div class="bar">
                                    <div class="bara"></div>
                                    <div class="barb"></div>
                                </div>
                                <div class="bar-green hidden bar_green2">
                                    <div class="bar-in1">
                                        <div class="bar-in1a"></div>
                                        <div class="bar-in1b"></div>
                                    </div>
                                </div>
                                <!--new-->
                            </div>

                        </div>
                    </div>
                </div>
                <div class="col-sm-4">
                    <div class="col-xs-12 no-padding">
                        <div class="col-sm-4 col-xs-3 no-padding margin-change as3-matrix-value-block">

                        </div>
                        <div class="col-sm-8 col-xs-4 pad-change">
                            <div class="bar-label bar-label_3"><p>0</p></div>
                            <div class="survey-3d-green">
                                <div class="bar-margin"></div>
                             
                                <!---new-->
                                <div class="bar">
                                    <div class="bara"></div>
                                    <div class="barb"></div>
                                </div>
                                <div class="bar-green hidden  bar_green3">
                                    <div class="bar-in1">
                                        <div class="bar-in1a"></div>
                                        <div class="bar-in1b"></div>
                                    </div>
                                </div>
                                <!--new-->
                            </div>

                        </div>
                    </div>
                </div>
            </div>

        </div>
        <div class="col-lg-2 col-xs-12">
            <div class="col-xs-12 no-padding">
                <div class="matrix-header-title">
                    <h1>CHG<span style="padding-left: 10px;"><img src="/images/dw-arw.png"></span><span><img src="/images/tp-arw.png"></span></h1>
                </div>
            </div>
            <div class="col-sm-12 block-div height-auto change-div" style="background: #f1f5f8;border-radius: 5px;padding: 15px;margin-bottom:20px;float:left;width:100%;">
                <div class="chg-text chg-per-block">

                </div>
            </div>
        </div>
    </div>
</div>
<!----------------Chart block-div--------->


    <!---------Group Block-------->
<div class="group-block hidden">
    <div class="col-xs-12 no-padding">
        <div class="col-sm-2 col-xs-2 pad-change">
            <div class="survey-3d">
                <img class="group-image" src="/images/3d-color1.png">
            </div>
        </div>
        <div class="col-sm-10 col-xs-10 pad-change">
            <div class="education-content-desc group-tooltip" style="position: relative;">
                <p class="group-name" group-id="0"></p>
                <span class="rt-arw"><img src="/images/rt-aw.png"></span>
                <span class="tooltiptext"></span>
            </div>
        </div>
    </div>
</div>

<!---------Group Block-------->
<!--------------Matrix value Section----------->
<div class="matrix-value-section hidden">
    <div class="education-content-3d-desc">
        <p style="margin-top: 15px;" class="mat-value"></p>
    </div>
</div>
<!--------------Matrix value Section----------->
<!------Change---Value--Section----------->
<div class="change-value hidden">
    <p style="color:#2ecc71;">10% <span><img src="/images/dw-arw.png"></span></p>
</div>
<!------Change---Value--Section----------->


     <!--div for Description Popup- Start-->
<div class="hidden" id="descText">
    <div class="col-xs-12 no-padding">
        <div class="col-lg-1 col-md-2 col-sm-3 col-xs-2 pad-change">
            <div class="education-content-desc2">
                <p class="matrix-value" id="matrixValue" score-id="0"></p>
            </div>
        </div>
        <div class="col-lg-11 col-md-10 col-sm-9 col-xs-10 pad-change">
            <div class="education-content-desc desc-tooltip">
                <p class="description" id="descriptionText"></p>
                <span class="tooltipdesctext"></span>
            </div>
        </div>
    </div>
</div>
<div class="desc-view-popup" style="position: relative;display:none">
    <div class="col-lg-8 col-xs-12 popup-div">
        <div class="left-arw">
            <img src="/images/left-aw.png">
        </div>
        <div class="col-xs-12 survey-margin" style="background: #f1f5f8;border-radius: 5px;padding: 12px;border: 1px solid #333;position:relative;" id="popupDiv">

        </div>
    </div>
</div>

<!--div for Description Popup- ENd-->
<!--div for Question Popup- Start-->
<div class="hidden" id="questionText">
    <div class="col-xs-12 no-padding">
        <div class="col-lg-1 col-md-2 col-sm-3 col-xs-2 pad-change">
            <div class="education-content-desc2">
                <p class="sl-no" id="serialNo">5</p>
            </div>
        </div>
        <div class="col-lg-11 col-md-10 col-sm-9 col-xs-10 pad-change">
            <div class="education-content-desc qn-tooltip">
                <p class="question" id="questionText"></p>
                <span class="tooltipqntext"></span>
            </div>
        </div>
    </div>
</div>
<div class="question-view-popup" style="display:none;">
    <div class="col-lg-8 col-xs-12 question-popup-div popup-div1">
        <div class="left-arw"><img src="/images/left-aw.png"></div>
        <div class="col-xs-12 survey-margin" style="background: #f1f5f8;border-radius: 5px;padding: 12px;border: 1px solid #333;position:relative;" id="questionpopupDiv">
        </div>
    </div>
</div>

<!--div for Question Popup- ENd-->
<script src="~/Scripts/MatrixSummary.js"></script>
     }
