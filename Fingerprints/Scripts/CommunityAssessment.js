
var zipCode_bind = '';

var states = {
    'alabama': 'AL',
    'alaska': 'AK',
    'arizona': 'AZ',
    'arkansas': 'AR',
    'california': 'CA',
    'colorado': 'CO',
    'connecticut': 'CT',
    'delaware': 'DE',
    'florida': 'FL',
    'georgia': 'GA',
    'hawaii': 'HI',
    'idaho': 'ID',
    'illinois': 'IL',
    'indiana': 'IN',
    'iowa': 'IA',
    'kansas': 'KS',
    'kentucky': 'KY',
    'louisiana': 'LA',
    'maine': 'ME',
    'maryland': 'MD',
    'massachusetts': 'MA',
    'michigan': 'MI',
    'minnesota': 'MN',
    'mississippi': 'MS',
    'missouri': 'MO',
    'montana': 'MT',
    'nebraska': 'NE',
    'nevada': 'NV',
    'new hampshire': 'NH',
    'new jersey': 'NJ',
    'new mexico': 'NM',
    'new york': 'NY',
    'north carolina': 'NC',
    'north dakota': 'ND',
    'ohio': 'OH',
    'oklahoma': 'OK',
    'oregon': 'OR',
    'oennsylvania': 'PA',
    'rhode island': 'RI',
    'south varolina': 'SC',
    'south dakota': 'SD',
    'tennessee': 'TN',
    'texas': 'TX',
    'utah': 'UT',
    'vermont': 'VT',
    'virginia': 'VA',
    'washington': 'WA',
    'west virginia': 'WV',
    'wisconsin': 'WI',
    'wyoming': 'WY'
};

$.fn.digits = function () {
    return this.each(function () {
        $(this).text($(this).text().replace(/(\d)(?=(\d\d\d)+(?!\d))/g, "$1,"));
    })
}
$.fn.removeQ = function () {
    return this.replace(/\"/g, "");
};

$.fn.sqMiles = function () {
    return $(this).html((parseInt($(this).html()) / 2589988.11).toFixed(1));
};
$.fn.removeQuotes = function () {
    return $(this).html($(this).html().replace(/\"/g, ""));
}

$.fn.addLeadingZeros = function () {
    return ($(this).html(("00000" + $(this).html()).slice(-5)));
}

function formatCurrency(total) {
    var neg = false;
    if (total < 0) {
        neg = true;
        total = Math.abs(total);
    }
    return (neg ? "-$" : '$') + parseFloat(total, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString();
}

//function zipcodelength() {
//    var i = 1234;
//    var inputlength = i.toString().length;
//    if (inputlength < 5) {
//        customAlert("zipcode must have 5 digits");
//        return false;
//    }
//}
//function addzeros() {
//    var n = 1234;
//    var m = 23;
//    var k = ("00000" + n).slice(-5);
//    var p = ("00000" + m).slice(-5);
//}


$('#population_tile').click(function () {
    $('html, body').animate({ scrollTop: $("#stats").offset().top }, 1000);
});

$('#real_estate_tile').click(function () {
    $('html, body').animate({ scrollTop: $("#real_estate").offset().top }, 1000);
});

$('.zip_code_appendDiv,.zip_state_city_section').on('click', '.zip_colorcode', function () {
    var zip_By_state = $(this).html();

    $('#q').val(zip_By_state);
    $('.btn-search-location').trigger('click');
});


$('#zip_code_tile').click(function () {
    $('html, body').animate({ scrollTop: $("#zips-list").offset().top }, 1000);
});


$('#most_pop_zip').click(function () {

    var zipCode = $(this).html();
    $('#q').val(zipCode);
    $('.btn-search-location').trigger('click');

});

function bindChartData(zip) {
    var text = zip;
    if (text == '') {
        return false;
    }
    else {
        getData(text.trim());

    }
}

function bindChartDataZip(zip) {
    var text = zip;
    if (text == '') {
        return false;
    }
    else {

        getData(text.trim());
    }
}

function getData(search) {

    var searchData = '';
    if (search.indexOf(',') === -1) {
        if ($.isNumeric(search)) {
            getChartDateByZip(search.replace(/^0+/, ''));
        }
        else {

            searchData = states[search.toLowerCase()];
            allow = false;
            getZipCodesByState(searchData);
        }
    }
    else {

        if (search.split(',').length == 2) {

            getZipCodesByStateAndCity(search);
        }

    }


}


function getChartDateByZip(zip) {
    zipCode_bind = zip;
    $.ajax({
        url: '/ERSEA/GetChartDataByZipCode',
        datatype: 'json',
        type: 'post',
        data: { zipcode: zip },
        success: function (data) {
            if (data.ZipCodeInfo != null) {
                $('.no_stats_found,.no_zip_code').addClass('hidden');
                bindZipCodeInfo(data.ZipCodeInfo);
                bindCitiesInZipcode(data.CitiesInZipCode);
                bindStatDemoGraphicsinZipCode(data.StatDemoGraphicsZip);
                bindPopulationChart(data.PopulationOverTime);
                bindGenderPieChart(data.PopulationByGender);
                bindRacePieChart(data.PopulationByRace);
                bindPopulationByAgeChart(data.TotalPopulationBasedOnAge);
                bindHeadOfHouseholdAgeChart(data.HouseholdByAge);
                bindFamiliesVsSinglesPieChart(data.FamilyAndSingles);
                bindHouseholdWithKidsChart(data.HouseholdsKids);
                bindChildrensByAgeChart(data.ChildrenBasedOnAge);
                bindHousing(data.HousingTypeFacilities);
                bindHousingOccupancy(data.OccupancyByHousing);
                bindVacanyReasons(data.Vacancy_Reasons);
            }
            else {
                bindZipCodeInfo(data.ZipCodeInfo);
                $('.State_title,.City_State_title,#full_chart_section,.zipcodeSection').addClass('hidden');
                $('.zip_title').removeClass('hidden');
                if ($.isNumeric($('#q').val())) {
                    $('.no_zip_code').html('No details found for the ZIP Code ' + $('#q').val()).removeClass('hidden');
                }
                else {
                    $('.no_zip_code').html('No results found for that location').removeClass('hidden');
                }
            }
            $('html, body').animate({ scrollTop: $(".ersea-detl-title").offset().top }, 1000);

            $('.City_State_title,.State_title,.zip_state_city_section,.zipcodeSection').addClass('hidden');

        },
        error: function (data) {

        }
    });
}

function getZipCodesByState(state) {
    $.ajax({
        url: '/ERSEA/GetZipcodesbystate',
        datatype: 'json',
        type: 'post',
        data: { searchText: state },
        success: function (data) {
            $('#full_chart_section,.zip_state_city_section').addClass('hidden');
            var bindDiv = '';
            $.each(data, function (k, list) {

                $.each(list, function (l, list2) {

                    var areacode = '';

                    if (list2.area_codes.indexOf(',') != -1) {
                        var split = list2.area_codes.split(',');

                        $.each(split, function (m, code) {

                            areacode += '<span>Area Code ' + code + '</span><br>';
                        });
                    }
                    else {
                        areacode = '<span href="javascript:void(0);">Area Code ' + list2.area_codes + '</span><br>';
                    }
                    bindDiv += '<div class="list-group-item">\
                   <div class="row"><div class="col-xs-12 prefix-col1 "><a href="javascript:void(0);" class="zip_colorcode">' + list2.zip + '</a>\
                  </div><div class="col-xs-12 prefix-col2" style="text-transform:capitalize"> ' + list2.type + ' </div><div class="col-xs-12 prefix-col3" style="text-transform:capitalize">' + list2.City + '</div>\
                  <div class="col-xs-12 prefix-col4">' + list2.county + '</div><div class="col-xs-12 prefix-col5">' + areacode +
            '</div></div></div>';
                });
            });

            if (bindDiv != '') {

                $('.zip_code_appendDiv').html(bindDiv);
                $('.map_image').css({ 'width': '58.33333333%', 'height': '100%' });
                $('.zipcodeSection').removeClass('hidden');
                $('.no_zip_code').addClass('hidden');
                $('html, body').animate({ scrollTop: $(".ersea-detl-title").offset().top }, 2000);
                $('.City_State_title,.zip_title').addClass('hidden');
                $('.State_title').removeClass('hidden');

                $('.state_heading').html($('#q').val().trim());
                $('#coordinates_state').html(latitutde.toFixed(1) + ', ' + longitude.toFixed(1));
            }
            else {
                $('.zipcodeSection,.zip_state_city_section,.zip_state_city_section').addClass('hidden');
                $('.no_zip_code').html('No results found for the entered area').removeClass('hidden');
                $('html, body').animate({ scrollTop: $(".no_zip_code").offset().top }, 2000);
            }

        },
        error: function (data) {

        }
    });
}

function getZipCodesByStateAndCity(state_City) {
    searchDataCity = state_City.split(',')[0].trim();
    searchDataState = state_City.split(',')[1].trim();
    searchDataState = (searchDataState.length > 3) ? states[searchDataState.toLowerCase()] : searchDataState;
    $('#full_chart_section,.zipcodeSection,.zip_title,.State_title').addClass('hidden');
    $('.no_zip_code').addClass('hidden');
    $.ajax({
        url: '/ERSEA/GetZipcodesbystateandCity',
        datatype: 'json',
        type: 'post',
        data: { searchcity: searchDataCity, searchstate: searchDataState },
        success: function (data) {

            var bindDivZip = '';
            $.each(data.ZipcodestateCitylist, function (k, list) {

                bindDivZip += '<tr>\
							<td style="text-align: center"><a href="javascript:void(0);" class="zip_colorcode">' + list.Zipcode + '</a></td>\
							<td style="text-align: center">' + parseFloat(list.population_count).toFixed(1) + '%' + '</td>\
							<td nowrap="" style="text-align: center">' + list.type + '</td>\
							<td nowrap="" style="text-align: center;text-transform: capitalize;">' + list.City + '</td>\
							<td style="text-align:center;text-transform: capitalize;">' + list.acceptable_cities + '</td>\
							<td style="text-align:center;text-transform: capitalize;">' + list.unacceptable_cities + '</td>\
							</tr>';
            });



            if (bindDivZip != '') {
                $('.state_city_span').html(state_City);
                $('.City_State_title').find('#zip_count').html(data.ZipcodestateCitylist.length + ' ZIP Codes');
                $('.City_State_title').find('#area_code').html((data.cityState.AreaCode == '') ? 'N/A' : data.cityState.AreaCode);
                $('.City_State_title').find('#county').html((data.cityState.County == '') ? 'N/A' : data.cityState.County);
                $('.City_State_title').find('#coordinates').html((data.cityState.CoOrdinates == ',') ? 'N/A' : data.cityState.CoOrdinates);
                $('.City_State_title').find('#most_pop_zip').html(data.cityState.MostPopulatedZip).addLeadingZeros();
                $('.City_State_title').removeClass('hidden');
                $('.zip_state_city_div').html(bindDivZip);
            }
            else {
                $('.state_city_span').html(state_City);
                $('.City_State_title').find('#zip_count').html(0 + ' ZIP Codes');
                $('.City_State_title').find('#area_code').html('N/A');
                $('.City_State_title').find('#county').html('N/a');
                $('.City_State_title').find('#coordinates').html('N/A');
                $('.City_State_title').find('#most_pop_zip').html('');
                $('.City_State_title').html('No Result Found');
                $('.City_State_title').removeClass('hidden');
                $('.zip_state_city_div').html(bindDivZip);
                $('.zip_state_city_div').html('No results found for the entered area');
            }
            $('.zip_state_city_section').removeClass('hidden');
        },
        error: function (data) {

        }
    })
}

function bindZipCodeInfo(zipInfo) {
    $('.zip_heading').html(zipCode_bind).addLeadingZeros();
    if (zipInfo != null) {
        $('#primary_city').html(zipInfo.Primarycity + ', ' + zipInfo.State);
        $('#county').html(zipInfo.County);
        $('.zip_title').find('.icon_sec').removeClass('hidden');
        $('#area_code').html((zipInfo.AreaCodes == '') ? 'N/A' : zipInfo.AreaCodes);

        $('#coordinates').html(zipInfo.Latitude + ', ' + zipInfo.Longitude);
    }
    else {
        $('.zip_title').find('.icon_sec').addClass('hidden');
        $('#primary_city').html('N/A');
        $('#county').html('N/A');
        $('#area_code').html('N/A');

        $('#coordinates').html('N/A');
    }

    $('.zip_title').removeClass('hidden');
    $('.map_image').css({ 'width': '58.33333333%', 'height': '100%' });
    $('#full_chart_section').removeClass('hidden');
    $('.zipcodeSection').addClass('hidden');
}

function bindCitiesInZipcode(cities) {

    $('.detail_abt_zip').find('.zip_code_detail').html(cities.ZipCode);
    $('.detail_abt_zip').find('#primary_city_acc').html(cities.Primarycity + ', ' + cities.State);
    var newVal = "";
    if (cities.Acceptablecities != null && cities.Acceptablecities != undefined)
        newVal = cities.Acceptablecities.substring(1, cities.Acceptablecities.length);
    $('.detail_abt_zip').find('#acceptable').html(newVal);
    if (cities.ZipCode != '') {

        $('.detail_abt_zip').removeClass('hidden');
    }
    else {
        $('.detail_abt_zip').addClass('hidden');
    }
}

function bindStatDemoGraphicsinZipCode(statics) {

    if (statics.State != '' || statics.State != null) {
        var keys = Object.keys(states);
        $.each(keys, function (k, obj) {
            if (states[obj] == statics.State) {
                $('#state_demographics').html(obj).css({ 'text-transform': 'capitalize' });

            }
        })

    }
    else {
        $('#state_demographics').html('');
    }

    $('#land_area').html(statics.AreaLand).sqMiles();
    $('#population').html(statics.PopulationCount).digits();
    var curr = (statics.MedianHomeValue == "") ? 0 : statics.MedianHomeValue
    $('#median_home_value').html(formatCurrency(curr));
    var income = (statics.MedianHouseholdIncome == "") ? 0 : statics.MedianHouseholdIncome
    $('#median_household_income').html(formatCurrency(income));
    $('#housing_units').html(statics.HousingUnitCount).digits();
    $('#water_area').html(statics.AreaWater).sqMiles();
    $('#occ_housing_units').html(statics.total_households_occupied).digits();
    $('#population_density').html(PopulationDensity(parseInt(statics.PopulationCount), parseInt(statics.AreaLand))).digits();
    $('.zip_heading_stats').html(statics.ZipCode);
    if (statics.PopulationCount == 0) {
        $('.div_statistics,#real_estate,#all_chart_section').addClass('hidden');
        $('.no_stats_found').removeClass('hidden');
    }
    else {
        $('.div_statistics,#real_estate,#all_chart_section').removeClass('hidden');
        $('.no_stats_found').addClass('hidden');
    }
}

function bindPopulationChart(popData) {
    var LineChart0_data;
    var LineChart0_chart;
    var LineChart0_svg;
    var LineChart0_x_annotations;
    var LineChart0_y_annotations;

    nv.addGraph(function () {
        var data = [{ "key": "Data", "values": [{ "x": 2005, "y": popData.EstimatedPopulation2005 }, { "x": 2006, "y": popData.EstimatedPopulation2006 }, { "x": 2007, "y": popData.EstimatedPopulation2007 }, { "x": 2008, "y": popData.EstimatedPopulation2008 }, { "x": 2009, "y": popData.EstimatedPopulation2009 }, { "x": 2010, "y": popData.EstimatedPopulation2010 }, { "x": 2011, "y": popData.EstimatedPopulation2011 }, { "x": 2012, "y": popData.EstimatedPopulation2012 }, { "x": 2013, "y": popData.EstimatedPopulation2013 }] }];
        var chart = nv.models.lineChart()
            .useInteractiveGuideline(true);

        var svg = d3.select('#LineChart0');

        var total = 0;
        data[0].values.forEach(function (d) {
            total = total + d.y;
        });
        if (total == 0) {
            $('.population').addClass('hidden');
        }
        else {
            $('.population').removeClass('hidden');
        }
        var x_annotations = [];
        var y_annotations = [];


        chart.margin({ top: 30, right: 20, bottom: 20, left: 50 })
        if (typeof chart.legend != 'undefined') chart.legend.margin().right -= 30;
        chart.showLegend(false);

        var y_window_formats = 0;
        nv.utils.windowResize(function () {
            y_window_formats = 0;
        });
        if (typeof chart.dispatch.stateChange != 'undefined') {
            // some charts (including bar charts) don't have state changes
            chart.dispatch.on('stateChange.y_window_formats', function () {
                y_window_formats = 0;
            });
        }
        if (typeof chart.yAxis != 'undefined') {
            // some charts (including pie charts) don't have a y axis
            chart.yAxis.tickFormat(function (d) {
                if (chart.yScale().domain()[1] < 10) {
                    return d3.format(',')(d);
                } else {

                    if (this === window) {
                        if (y_window_formats >= 2) {
                            return d3.format(',')(d);
                        }
                        y_window_formats++;
                    }

                    // axis
                    return d3.format(',.0f')(d);;
                }
            });
        }
        if (typeof chart.valueFormat == 'function') {
            // over bar
            chart.valueFormat(d3.format(','));
        }


        svg.datum(data)
            .call(chart);

        nv.utils.windowResize(function () { chart.update(); });

        chart.dispatch.on('renderEnd.alignAnnotations', function () {


        });

        LineChart0_data = data;
        LineChart0_chart = chart;
        LineChart0_svg = svg;
        LineChart0_x_annotations = x_annotations;
        LineChart0_y_annotations = y_annotations;

        return chart;
    }, function () {

    });

}

function bindGenderPieChart(genData) {
    var PieChart0_data;
    var PieChart0_chart;
    var PieChart0_svg;

    nv.addGraph(function () {
        var data = [{ "key": "Data", "values": [{ "x": "Male", "y": parseInt(genData.TotalMalePopulation) }, { "x": "Female", "y": parseInt(genData.TotalFemalePopulation) }] }];

        var total = 0;
        data[0].values.forEach(function (d) {
            total = total + d.y;
        });
        $('#PieChart0').attr('gender_total', total);
        if (total == 0) {
            $('.population_by_gender').addClass('hidden');
            return false;
        }
        else {
            $('.population_by_gender').removeClass('hidden');
        }
        var chart = nv.models.pieChart();
        var svg = d3.select('#PieChart0');
        var maleCount = $('#maleCount').html(genData.TotalMalePopulation).digits();
        var femalecount = $('#femaleCount').html(genData.TotalFemalePopulation).digits();
        var malePercent = $('#malePercent').html((parseInt(genData.TotalMalePopulation) / total * 100).toFixed() + '%');
        var femalePercent = $('#femalePercent').html((parseInt(genData.TotalFemalePopulation) / total * 100).toFixed() + '%');


        chart.margin({ top: 30, right: 20, bottom: 20, left: 50 })
        if (typeof chart.legend != 'undefined') chart.legend.margin().right -= 30;
        chart.showLegend(false);

        var y_window_formats = 0;
        nv.utils.windowResize(function () {
            y_window_formats = 0;
        });
        if (typeof chart.dispatch.stateChange != 'undefined') {
            // some charts (including bar charts) don't have state changes
            chart.dispatch.on('stateChange.y_window_formats', function () {
                y_window_formats = 0;
            });
        }
        if (typeof chart.yAxis != 'undefined') {
            // some charts (including pie charts) don't have a y axis
            chart.yAxis.tickFormat(function (d) {
                if (chart.yScale().domain()[1] < 10) {
                    return d3.format(',')(d);
                } else {
                    // tooltip via https://github.com/novus/nvd3/issues/428
                    if (this === window) {
                        if (y_window_formats >= 2) {
                            return d3.format(',')(d);
                        }
                        y_window_formats++;
                    }

                    // axis
                    return d3.format(',.0f')(d);;
                }
            });
        }
        if (typeof chart.valueFormat == 'function') {
            // over bar
            chart.valueFormat(d3.format(','));
        }

        chart.color(["#1f77b4", "#aec7e8"]);
        chart.margin({ top: 5, right: 5, bottom: 5, left: 5 });

        chart.tooltip.valueFormatter(function (d) {
            return (d / parseInt($('#PieChart0').attr('gender_total')) * 100).toFixed() + ' %<br><span style="font-weight: normal; font-size: 75%">' + d3.format(',')(d) + ' of ' + d3.format(',')(parseInt($('#PieChart0').attr('gender_total'))) + '</span>';
        });
        chart.showLabels(false);



        chart.growOnHover(false);

        if (!chart.labelsOutside()) {

            // the current nvd3 version makes the following adjustment even if growOnHover is turned off
            // need to undo it
            // the result is a pie chart that doesn't fill the entire container
            //d.outer = (d.outer - d.outer / 5)

            var arcs = chart.arcsRadius();
            var new_arcs = [];
            if (arcs.length === 0) {
                data[0].values.forEach(function (d) {
                    new_arcs.push({ outer: 1.25, inner: 0 });
                });
            } else {
                arcs.forEach(function (d) {
                    new_arcs.push({ outer: d.outer * 1.25, inner: d.inner * 1.25 });
                });
            }
            chart.arcsRadius(new_arcs);
        }


        // note that a pie chart uses a different data format
        svg.datum(data[0].values)
            .call(chart);

        nv.utils.windowResize(function () { chart.update(); });

        PieChart0_data = data;
        PieChart0_chart = chart;
        PieChart0_svg = svg;


        return chart;
    }, function () {

    });
}

function bindRacePieChart(raceData) {
    var PieChart1_data;
    var PieChart1_chart;
    var PieChart1_svg;
    nv.addGraph(function () {
        var data = [{
            "key": "Data", "values": [{ "x": "White", "y": raceData.White },
                { "x": "Black Or African American", "y": raceData.BlackOrAfricanAmerican },
                { "x": "American Indian Or Alaskan Native", "y": raceData.AmericanIndian },
                { "x": "Asian", "y": raceData.Asian },
                { "x": "Native Hawaiian & Other Pacific Islander", "y": raceData.NativeHawaiian },
                { "x": "Other Race", "y": raceData.OtherRace },
                { "x": "Two Or More Races", "y": raceData.TwoOrMoreRace }]
        }];

        var total = 0;

        data[0].values.forEach(function (d) {
            total = total + parseInt(d.y);

        });
        $('#PieChart1').attr('race_total', total);
        if (total == 0) {
            $('.population_by_race').addClass('hidden');
        }
        else {
            $('.population_by_race').removeClass('hidden');
        }
        var chart = nv.models.pieChart();

        var svg = d3.select('#PieChart1');
        $('#pie_white_count').html(raceData.White).digits();
        $('#bar_black_count').html(raceData.BlackOrAfricanAmerican).digits();
        $('#pie_Native').html(raceData.AmericanIndian).digits();
        $('#pie_asian_count').html(raceData.Asian).digits();
        $('#pie_hawaai_count').html(raceData.NativeHawaiian).digits();
        $('#pie_other_count').html(raceData.OtherRace).digits();
        $('#pie_two_More_count').html(raceData.TwoOrMoreRace).digits();

        $('#pie_white_percent').html((parseFloat(raceData.White) / total * 100).toFixed(1) + '%');
        $('#pie_black_percent').html((parseFloat(raceData.BlackOrAfricanAmerican) / total * 100).toFixed(1) + '%');
        $('#pie_native_percent').html((parseFloat(raceData.AmericanIndian) / total * 100).toFixed(1) + '%');
        $('#pie_asian_percent').html((parseFloat(raceData.Asian) / total * 100).toFixed(1) + '%');
        $('#pie_hawaai_percent').html((parseFloat(raceData.NativeHawaiian) / total * 100).toFixed(1) + '%');
        $('#pie_other_percent').html((parseFloat(raceData.OtherRace) / total * 100).toFixed(1) + '%');
        $('#pie_two_more_percent').html((parseFloat(raceData.TwoOrMoreRace) / total * 100).toFixed(1) + '%');


        chart.margin({ top: 30, right: 20, bottom: 20, left: 50 })
        if (typeof chart.legend != 'undefined') chart.legend.margin().right -= 30;
        chart.showLegend(false);

        var y_window_formats = 0;
        nv.utils.windowResize(function () {
            y_window_formats = 0;
        });
        if (typeof chart.dispatch.stateChange != 'undefined') {
            // some charts (including bar charts) don't have state changes
            chart.dispatch.on('stateChange.y_window_formats', function () {
                y_window_formats = 0;
            });
        }
        if (typeof chart.yAxis != 'undefined') {
            // some charts (including pie charts) don't have a y axis
            chart.yAxis.tickFormat(function (d) {
                if (chart.yScale().domain()[1] < 10) {
                    return d3.format(',')(d);
                } else {
                    if (this === window) {
                        if (y_window_formats >= 2) {
                            return d3.format(',')(d);
                        }
                        y_window_formats++;
                    }

                    // axis
                    return d3.format(',.0f')(d);;
                }
            });
        }
        if (typeof chart.valueFormat == 'function') {
            // over bar
            chart.valueFormat(d3.format(','));
        }

        chart.color(["#1f77b4", "#aec7e8", "#ff7f0e", "#ffbb78", "#2ca02c", "#98df8a", "#d62728"]);
        chart.margin({ top: 5, right: 5, bottom: 5, left: 5 });

        chart.tooltip.valueFormatter(function (d) {
            return (d / parseInt($('#PieChart1').attr('race_total')) * 100).toFixed() + ' %<br><span style="font-weight: normal; font-size: 75%">' + d3.format(',')(d) + ' of ' + d3.format(',')(parseInt($('#PieChart1').attr('race_total'))) + '</span>';
        });
        chart.showLabels(false);



        chart.growOnHover(false);

        if (!chart.labelsOutside()) {

            // the current nvd3 version makes the following adjustment even if growOnHover is turned off
            // need to undo it
            // the result is a pie chart that doesn't fill the entire container
            //d.outer = (d.outer - d.outer / 5)

            var arcs = chart.arcsRadius();
            var new_arcs = [];
            if (arcs.length === 0) {
                data[0].values.forEach(function (d) {
                    new_arcs.push({ outer: 1.25, inner: 0 });
                });
            } else {
                arcs.forEach(function (d) {
                    new_arcs.push({ outer: d.outer * 1.25, inner: d.inner * 1.25 });
                });
            }
            chart.arcsRadius(new_arcs);
        }


        // note that a pie chart uses a different data format
        svg.datum(data[0].values)
            .call(chart);

        nv.utils.windowResize(function () { chart.update(); });

        PieChart1_data = data;
        PieChart1_chart = chart;
        PieChart1_svg = svg;

        return chart;
    }, function () {

    });
}

function PopulationDensity(totalpopulation, area_land) {
    var landArea = area_land / 2589988.11;
    var total_pop = totalpopulation,
    area = landArea,
    result = total_pop / area;
    return result.toFixed();
}


function bindPopulationByAgeChart(popAge) {

    var LineChart1_data;
    var LineChart1_chart;
    var LineChart1_svg;
    var LineChart1_x_annotations;
    var LineChart1_y_annotations;

    var male_under5 = parseInt(popAge.MaleUnder5);
    var male_5to9 = parseInt(popAge.Male5To9);
    var male_10to14 = parseInt(popAge.Male10To14);
    var male_15to19 = parseInt(popAge.Male15To17) + parseInt(popAge.Male18To19);
    var male_20to24 = parseInt(popAge.Male20) + parseInt(popAge.Male21) + parseInt(popAge.Male22To24);
    var male_25to29 = parseInt(popAge.Male25To29);
    var male_30to34 = parseInt(popAge.Male30To34);
    var male_35to39 = parseInt(popAge.Male35To39);
    var male_40to44 = parseInt(popAge.Male40To44);
    var male_45to49 = parseInt(popAge.Male45to49);
    var male_50to54 = parseInt(popAge.Male50To54);
    var male_55to59 = parseInt(popAge.Male55To59);
    var male_60to64 = parseInt(popAge.Male60To61) + parseInt(popAge.Male62To64);
    var male_65to69 = parseInt(popAge.Male65To66) + parseInt(popAge.Male67To69);
    var male_70to74 = parseInt(popAge.MAle70To74);
    var male_75to79 = parseInt(popAge.Male75To79);
    var male_80to84 = parseInt(popAge.Male80To84);
    var male_85plus = parseInt(popAge.Male85Plus);

    var female_under5 = parseInt(popAge.FemaleUnder5);
    var female_5to9 = parseInt(popAge.Female5to9);
    var female_10to14 = parseInt(popAge.Female10To14);
    var female_15to19 = parseInt(popAge.Female15To17) + parseInt(popAge.Female18to19);
    var female_20to24 = parseInt(popAge.Female20) + parseInt(popAge.Female21) + parseInt(popAge.Female22To24);
    var female_25to29 = parseInt(popAge.Female25To29);
    var female_30to34 = parseInt(popAge.Female30To34);
    var female_35to39 = parseInt(popAge.Female35To39);
    var female_40to44 = parseInt(popAge.Female40To44);
    var female_45to49 = parseInt(popAge.Female45To49);
    var female_50to54 = parseInt(popAge.Female50To54);
    var female_55to59 = parseInt(popAge.Female55To59);
    var female_60to64 = parseInt(popAge.Female60To61) + parseInt(popAge.Female62to64);
    var female_65to69 = parseInt(popAge.Female65To66) + parseInt(popAge.Female67To69);
    var female_70to74 = parseInt(popAge.Female70To74);
    var female_75to79 = parseInt(popAge.Female75To79);
    var female_80to84 = parseInt(popAge.Female80To84);
    var female_85plus = parseInt(popAge.Female85Plus);


    var total_under5 = male_under5 + female_under5;
    var total_5to9 = male_5to9 + female_5to9;
    var total_10to14 = male_10to14 + female_10to14;
    var total_15to19 = male_15to19 + female_15to19;
    var total_20to24 = male_20to24 + female_20to24;
    var total_25to29 = male_25to29 + female_25to29;
    var total_30to34 = male_30to34 + female_30to34;
    var total_35to39 = male_35to39 + female_35to39;
    var total_40to44 = male_40to44 + female_40to44;
    var total_45to49 = male_45to49 + female_45to49;
    var total_50to54 = male_50to54 + female_50to54;
    var total_55to59 = male_55to59 + female_55to59;
    var total_60to64 = male_60to64 + female_60to64;
    var total_65to69 = male_65to69 + female_65to69;
    var total_70to74 = male_70to74 + female_70to74;
    var total_75to79 = male_75to79 + female_75to79;
    var total_80to84 = male_80to84 + female_80to84;
    var total_85plus_age = male_85plus + female_85plus;


    //male label//

    $('#male_under5').html(male_under5).digits();
    $('#male_5to9').html(male_5to9).digits();
    $('#male_10to14').html(male_10to14).digits();
    $('#male_15to19').html(male_15to19).digits();
    $('#male_20to24').html(male_20to24).digits();
    $('#male_25to29').html(male_25to29).digits();
    $('#male_30to34').html(male_30to34).digits();
    $('#male_35to39').html(male_35to39).digits();
    $('#male_40to44').html(male_40to44).digits();
    $('#male_45to49').html(male_45to49).digits();
    $('#male_50to54').html(male_50to54).digits();
    $('#male_55to59').html(male_55to59).digits();
    $('#male_60to64').html(male_60to64).digits();
    $('#male_65to69').html(male_65to69).digits();
    $('#male_70to74').html(male_70to74).digits();
    $('#male_75to79').html(male_75to79).digits();
    $('#male_80to84').html(male_80to84).digits();
    $('#male_85plus').html(male_85plus).digits();

    //female label//
    $('#female_under5').html(female_under5).digits();
    $('#female_5to9').html(female_5to9).digits();
    $('#female_10to14').html(female_10to14).digits();
    $('#female_15to19').html(female_15to19).digits();
    $('#female_20to24').html(female_20to24).digits();
    $('#female_25to29').html(female_25to29).digits();
    $('#female_30to34').html(female_30to34).digits();
    $('#female_35to39').html(female_35to39).digits();
    $('#female_40to44').html(female_40to44).digits();
    $('#female_45to49').html(female_45to49).digits();
    $('#female_50to54').html(female_50to54).digits();
    $('#female_55to59').html(female_55to59).digits();
    $('#female_60to64').html(female_60to64).digits();
    $('#female_65to69').html(female_65to69).digits();
    $('#female_70to74').html(female_70to74).digits();
    $('#female_75to79').html(female_75to79).digits();
    $('#female_80to84').html(female_80to84).digits();
    $('#female_85plus').html(female_85plus).digits();

    //total label//
    $('#total_under5').html(total_under5).digits();
    $('#total_5to9').html(total_5to9).digits();
    $('#total_10to14').html(total_10to14).digits();
    $('#total_15to19').html(total_15to19).digits();
    $('#total_20to24').html(total_20to24).digits();

    $('#total_25to29').html(total_25to29).digits();
    $('#total_30to34').html(total_30to34).digits();
    $('#total_35to39').html(total_35to39).digits();
    $('#total_40to44').html(total_40to44).digits();
    $('#total_45to49').html(total_45to49).digits();
    $('#total_50to54').html(total_50to54).digits();
    $('#total_55to59').html(total_55to59).digits();
    $('#total_60to64').html(total_60to64).digits();
    $('#total_65to69').html(total_65to69).digits();
    $('#total_70to74').html(total_70to74).digits();
    $('#total_75to79').html(total_75to79).digits();
    $('#total_80to84').html(total_80to84).digits();
    $('#total_85plus_age').html(total_85plus_age).digits();

    //Median ages//
    $('#median_age').html(popAge.MedianAge);
    $('#male_median_age').html(popAge.MAleMedianAge);
    $('#female_median_age').html(popAge.FemaleMedainAge);
    $('.population_by_age').removeClass('hidden');

    nv.addGraph(function () {
        var data = [{
            "key": "Male", "values": [{ "x": 0, "y": male_under5 },
                { "x": 1, "y": male_5to9 }, { "x": 2, "y": male_10to14 },
                { "x": 3, "y": male_15to19 }, { "x": 4, "y": male_20to24 },
                { "x": 5, "y": male_25to29 }, { "x": 6, "y": male_30to34 },
                { "x": 7, "y": male_35to39 }, { "x": 8, "y": male_40to44 },
                { "x": 9, "y": male_45to49 }, { "x": 10, "y": male_50to54 },
                { "x": 11, "y": male_55to59 }, { "x": 12, "y": male_60to64 },
                { "x": 13, "y": male_65to69 }, { "x": 14, "y": male_70to74 },
                { "x": 15, "y": male_75to79 }, { "x": 16, "y": male_80to84 },
                { "x": 17, "y": male_85plus }
            ]
        }, {
            "key": "Female", "values": [{ "x": 0, "y": female_under5 },
                { "x": 1, "y": female_5to9 },
                { "x": 2, "y": female_10to14 },
                { "x": 3, "y": female_15to19 },
                { "x": 4, "y": female_20to24 },
                { "x": 5, "y": female_25to29 },
                { "x": 6, "y": female_30to34 },
                { "x": 7, "y": female_35to39 },
                { "x": 8, "y": female_40to44 },
                { "x": 9, "y": female_45to49 },
                { "x": 10, "y": female_50to54 },
                { "x": 11, "y": female_55to59 },
                { "x": 12, "y": female_60to64 },
                { "x": 13, "y": female_65to69 },
                { "x": 14, "y": female_70to74 },
                { "x": 15, "y": female_75to79 },
                { "x": 16, "y": female_80to84 },
                { "x": 17, "y": female_85plus }]
        }, {
            "key": "Total", "color": "#000000", "values": [{ "x": 0, "y": total_under5 },
                { "x": 1, "y": total_5to9 },
                { "x": 2, "y": total_10to14 },
                { "x": 3, "y": total_15to19 },
                { "x": 4, "y": total_20to24 },
                { "x": 5, "y": total_25to29 },
                { "x": 6, "y": total_30to34 },
                { "x": 7, "y": total_35to39 },
                { "x": 8, "y": total_40to44 },
                { "x": 9, "y": total_45to49 },
                { "x": 10, "y": total_50to54 },
                { "x": 11, "y": total_55to59 },
                { "x": 12, "y": total_60to64 },
                { "x": 13, "y": total_65to69 },
                { "x": 14, "y": total_70to74 },
                { "x": 15, "y": total_75to79 },
                { "x": 16, "y": total_80to84 },
                { "x": 17, "y": total_85plus_age }]
        }];
        var total = 0;
        data[0].values.forEach(function (d) {
            total = total + d.y;
        });

        if (total == 0) {

            $('.population_by_age').addClass('hidden');
            return false;

        }
        else {
            $('.population_by_age').removeClass('hidden');
        }
        var chart = nv.models.lineChart()
            .useInteractiveGuideline(true);

        var svg = d3.select('#LineChart1');

        var x_annotations = [];
        var y_annotations = [];


        chart.margin({ top: 30, right: 20, bottom: 20, left: 50 })
        if (typeof chart.legend != 'undefined') chart.legend.margin().right -= 30;

        if (typeof chart.legend != 'undefined') {
            chart.legend.vers('furious');
        }

        var y_window_formats = 0;
        nv.utils.windowResize(function () {
            y_window_formats = 0;
        });
        if (typeof chart.dispatch.stateChange != 'undefined') {
            // some charts (including bar charts) don't have state changes
            chart.dispatch.on('stateChange.y_window_formats', function () {
                y_window_formats = 0;
            });
        }
        if (typeof chart.yAxis != 'undefined') {
            // some charts (including pie charts) don't have a y axis
            chart.yAxis.tickFormat(function (d) {
                if (chart.yScale().domain()[1] < 10) {
                    return d3.format(',')(d);
                } else {
                    if (this === window) {
                        if (y_window_formats >= 2) {
                            return d3.format(',')(d);
                        }
                        y_window_formats++;
                    }

                    // axis
                    return d3.format(',.0f')(d);;
                }
            });
        }
        if (typeof chart.valueFormat == 'function') {
            // over bar
            chart.valueFormat(d3.format(','));
        }
        chart.forceY(0);

        if (data.length > 2 && parseInt(svg.style('width')) < 768 / 2) {
            chart.showLegend(false);
        }

        var x_discreet_labels = ["Under 5", "5-9", "10-14", "15-19", "20-24", "25-29", "30-34", "35-39", "40-44", "45-49", "50-54", "55-59", "60-64", "65-69", "70-74", "75-79", "80-84", "85 Plus"];

        chart.xAxis.tickFormat(function (d) {
            return x_discreet_labels[d];
        });


        svg.datum(data)
            .call(chart);

        nv.utils.windowResize(function () { chart.update(); });

        chart.dispatch.on('renderEnd.alignAnnotations', function () {


        });

        LineChart1_data = data;
        LineChart1_chart = chart;
        LineChart1_svg = svg;
        LineChart1_x_annotations = x_annotations;
        LineChart1_y_annotations = y_annotations;

        return chart;
    }, function () {

    });

}

function bindHeadOfHouseholdAgeChart(headAge) {
    var LineChart2_data;
    var LineChart2_chart;
    var LineChart2_svg;
    var LineChart2_x_annotations;
    var LineChart2_y_annotations;

    var owner15to24 = parseInt(headAge.owner_15_to_24);
    var owner25to34 = parseInt(headAge.owner_25_to_34);
    var owner35to44 = parseInt(headAge.owner_35_to_44);
    var owner45to54 = parseInt(headAge.owner_45_to_54);
    var owner55to64 = parseInt(headAge.owner_55_to_59) + parseInt(headAge.owner_60_to_64);
    var owner65to74 = parseInt(headAge.owner_65_to_74);
    var owner75to84 = parseInt(headAge.owner_75_to_84);
    var owner85plus = parseInt(headAge.owner_85_plus);

    var renter15to24 = parseInt(headAge.renter_15_to_24);
    var renter25to34 = parseInt(headAge.renter_25_to_34);
    var renter35to44 = parseInt(headAge.renter_35_to_44);
    var renter45to54 = parseInt(headAge.renter_45_to_54);
    var renter55to64 = parseInt(headAge.renter_55_to_59) + parseInt(headAge.renter_60_to_64);
    var renter65to74 = parseInt(headAge.renter_65_to_74);
    var renter75to84 = parseInt(headAge.renter_75_to_84);
    var renter85plus = parseInt(headAge.renter_85_plus);

    $('#owner_15to24').html(owner15to24).digits();
    $('#owner_25to34').html(owner25to34).digits();
    $('#owner_35to44').html(owner35to44).digits();
    $('#owner_45to54').html(owner45to54).digits();
    $('#owner_55to64').html(owner55to64).digits();
    $('#owner_65to74').html(owner65to74).digits();
    $('#owner_75to84').html(owner75to84).digits();
    $('#owner_85plus').html(owner85plus).digits();

    $('#renter_15to24').html(renter15to24).digits();
    $('#renter_25to34').html(renter25to34).digits();
    $('#renter_35to44').html(renter35to44).digits();
    $('#renter_45to54').html(renter45to54).digits();
    $('#renter_55to64').html(renter55to64).digits();
    $('#renter_65to74').html(renter65to74).digits();
    $('#renter_75to84').html(renter75to84).digits();
    $('#renter_85plus').html(renter85plus).digits();

    $('#total_15to24').html(owner15to24 + renter15to24).digits();
    $('#total_25to34').html(owner25to34 + renter25to34).digits();
    $('#total_35to44').html(owner35to44 + renter35to44).digits();
    $('#total_45to54').html(owner45to54 + renter45to54).digits();
    $('#total_55to64').html(owner55to64 + renter55to64).digits();
    $('#total_65to74').html(owner65to74 + renter65to74).digits();
    $('#total_75to84').html(owner75to84 + renter75to84).digits();
    $('#total_85plus_head').html(owner85plus + renter85plus).digits();

    nv.addGraph(function () {
        var data = [{
            "key": "Owner", "values": [{ "x": 0, "y": owner15to24 },
                { "x": 1, "y": owner25to34 },
                { "x": 2, "y": owner35to44 },
                { "x": 3, "y": owner45to54 },
                { "x": 4, "y": owner55to64 },
                { "x": 5, "y": owner65to74 },
                { "x": 6, "y": owner75to84 },
                { "x": 7, "y": owner85plus }

            ]
        }, {
            "key": "Renter", "values": [
                { "x": 0, "y": renter15to24 },
                { "x": 1, "y": renter25to34 },
                { "x": 2, "y": renter35to44 },
                { "x": 3, "y": renter45to54 },
                { "x": 4, "y": renter55to64 },
                { "x": 5, "y": renter65to74 },
                { "x": 6, "y": renter75to84 },
                { "x": 7, "y": renter85plus }
            ]
        }, {
            "key": "Total", "color": "#000000", "values": [
                { "x": 0, "y": owner15to24 + renter15to24 },
                { "x": 1, "y": owner25to34 + renter25to34 },
                { "x": 2, "y": owner35to44 + renter35to44 },
                { "x": 3, "y": owner45to54 + renter45to54 },
                { "x": 4, "y": owner55to64 + renter55to64 },
                { "x": 5, "y": owner65to74 + renter65to74 },
                { "x": 6, "y": owner75to84 + renter75to84 },
                { "x": 7, "y": owner85plus + renter85plus }
            ]
        }];

        var total = 0;
        data[0].values.forEach(function (d) {
            total = total + d.y;
        });

        if (total == 0) {

            $('.head_of_household').addClass('hidden');
            return false;

        }
        else {
            $('.head_of_household').removeClass('hidden');
        }

        var chart = nv.models.lineChart()
            .useInteractiveGuideline(true);

        var svg = d3.select('#LineChart2');

        var x_annotations = [];
        var y_annotations = [];


        chart.margin({ top: 30, right: 20, bottom: 20, left: 50 })
        if (typeof chart.legend != 'undefined') chart.legend.margin().right -= 30;

        if (typeof chart.legend != 'undefined') {
            chart.legend.vers('furious');
        }

        var y_window_formats = 0;
        nv.utils.windowResize(function () {
            y_window_formats = 0;
        });
        if (typeof chart.dispatch.stateChange != 'undefined') {
            // some charts (including bar charts) don't have state changes
            chart.dispatch.on('stateChange.y_window_formats', function () {
                y_window_formats = 0;
            });
        }
        if (typeof chart.yAxis != 'undefined') {
            // some charts (including pie charts) don't have a y axis
            chart.yAxis.tickFormat(function (d) {
                if (chart.yScale().domain()[1] < 10) {
                    return d3.format(',')(d);
                } else {
                    if (this === window) {
                        if (y_window_formats >= 2) {
                            return d3.format(',')(d);
                        }
                        y_window_formats++;
                    }

                    // axis
                    return d3.format(',.0f')(d);;
                }
            });
        }
        if (typeof chart.valueFormat == 'function') {
            // over bar
            chart.valueFormat(d3.format(','));
        }
        chart.forceY(0);

        if (data.length > 2 && parseInt(svg.style('width')) < 768 / 2) {
            chart.showLegend(false);
        }

        var x_discreet_labels = ["15-24", "25-34", "35-44", "45-54", "55-64", "65-74", "75-84", "85 Plus"];

        chart.xAxis.tickFormat(function (d) {
            return x_discreet_labels[d];
        });


        svg.datum(data)
            .call(chart);

        nv.utils.windowResize(function () { chart.update(); });

        chart.dispatch.on('renderEnd.alignAnnotations', function () {


        });

        LineChart2_data = data;
        LineChart2_chart = chart;
        LineChart2_svg = svg;
        LineChart2_x_annotations = x_annotations;
        LineChart2_y_annotations = y_annotations;

        return chart;
    }, function () {

    });
}

function bindFamiliesVsSinglesPieChart(familyKids) {
    var PieChart2_data;
    var PieChart2_chart;
    var PieChart2_svg;

    var husbandwifefamilyhouseholds = parseInt(familyKids.HousbandWifeFamilyHouseholds);
    var singleguardian = parseInt(familyKids.OtherFamilyHouseholds);
    var singles = parseInt(familyKids.LivingAlone);
    var singlewithroommate = parseInt(familyKids.NotLivingAlone);

    nv.addGraph(function () {
        var data = [{
            "key": "Data", "values": [
                { "x": "Husband Wife Family Households", "y": husbandwifefamilyhouseholds },
                { "x": "Single Guardian", "y": singleguardian },
                { "x": "Singles", "y": singles },
                { "x": "Singles With Roommate", "y": singlewithroommate }]
        }];

        var total = 0;

        data[0].values.forEach(function (d) {
            total = total + d.y;
        });

        $('#PieChart2').attr('fam_sing_total', total);
        if (total == 0) {
            $('.families_singles').addClass('hidden');
            return false;
        }
        else {
            $('.families_singles').removeClass('hidden');
        }
        var chart = nv.models.pieChart();

        var svg = d3.select('#PieChart2');

        $('#housband_wife_family').html(husbandwifefamilyhouseholds).digits();
        $('#singleguardian').html(singleguardian).digits();
        $('#singles').html(singles).digits();
        $('#single_withRoomate').html(singlewithroommate).digits();

        $('#housband_wife_family_percent').html((husbandwifefamilyhouseholds / total * 100).toFixed() + '%');
        $('#singleGuardian_percentage').html((singleguardian / total * 100).toFixed() + '%');
        $('#singles_percentage').html((singles / total * 100).toFixed() + '%');
        $('#singleswithroomatepercentage').html((singlewithroommate / total * 100).toFixed() + '%');

        chart.margin({ top: 30, right: 20, bottom: 20, left: 50 })
        if (typeof chart.legend != 'undefined') chart.legend.margin().right -= 30;
        chart.showLegend(false);

        var y_window_formats = 0;
        nv.utils.windowResize(function () {
            y_window_formats = 0;
        });
        if (typeof chart.dispatch.stateChange != 'undefined') {
            // some charts (including bar charts) don't have state changes
            chart.dispatch.on('stateChange.y_window_formats', function () {
                y_window_formats = 0;
            });
        }
        if (typeof chart.yAxis != 'undefined') {
            // some charts (including pie charts) don't have a y axis
            chart.yAxis.tickFormat(function (d) {
                if (chart.yScale().domain()[1] < 10) {
                    return d3.format(',')(d);
                } else {
                    // tooltip via https://github.com/novus/nvd3/issues/428
                    if (this === window) {
                        if (y_window_formats >= 2) {
                            return d3.format(',')(d);
                        }
                        y_window_formats++;
                    }

                    // axis
                    return d3.format(',.0f')(d);;
                }
            });
        }
        if (typeof chart.valueFormat == 'function') {
            // over bar
            chart.valueFormat(d3.format(','));
        }

        chart.color(["#1f77b4", "#aec7e8", "#ff7f0e", "#ffbb78"]);
        chart.margin({ top: 5, right: 5, bottom: 5, left: 5 });

        chart.tooltip.valueFormatter(function (d) {
            return (d / parseInt($('#PieChart2').attr('fam_sing_total')) * 100).toFixed() + ' %<br><span style="font-weight: normal; font-size: 75%">' + d3.format(',')(d) + ' of ' + d3.format(',')(parseInt($('#PieChart2').attr('fam_sing_total'))) + '</span>';
        });
        chart.showLabels(false);



        chart.growOnHover(false);

        if (!chart.labelsOutside()) {

            // the current nvd3 version makes the following adjustment even if growOnHover is turned off
            // need to undo it
            // the result is a pie chart that doesn't fill the entire container
            //d.outer = (d.outer - d.outer / 5)

            var arcs = chart.arcsRadius();
            var new_arcs = [];
            if (arcs.length === 0) {
                data[0].values.forEach(function (d) {
                    new_arcs.push({ outer: 1.25, inner: 0 });
                });
            } else {
                arcs.forEach(function (d) {
                    new_arcs.push({ outer: d.outer * 1.25, inner: d.inner * 1.25 });
                });
            }
            chart.arcsRadius(new_arcs);
        }


        // note that a pie chart uses a different data format
        svg.datum(data[0].values)
            .call(chart);

        nv.utils.windowResize(function () { chart.update(); });

        PieChart2_data = data;
        PieChart2_chart = chart;
        PieChart2_svg = svg;

        return chart;
    }, function () {

    });
}

function bindHouseholdWithKidsChart(housholdKids) {

    var PieChart3_data;
    var PieChart3_chart;
    var PieChart3_svg;
    var housewithoutKids = parseInt(housholdKids.households_wo_kids);
    var householdswithKids = parseInt(housholdKids.TotalHouseholdsWKids);
    var avgHouseholdsize = parseInt(housholdKids.AverageHouseHoldSize);

    $('#householdwithoutkids').html(housewithoutKids).digits();
    $('#householdwithkids').html(householdswithKids).digits();
    $('#avghouseholdsize').html(avgHouseholdsize);


    nv.addGraph(function () {
        var data = [{
            "key": "Data", "values": [
                { "x": "Households Without Kids", "y": housewithoutKids },
                { "x": "Households With Kids", "y": householdswithKids }]
        }];

        var total = 0;
        data[0].values.forEach(function (d) {
            total = total + d.y;
        });
        $('#PieChart3').attr('house_kids_total', total);
        if (total == 0) {
            $('.households_kids').addClass('hidden');
            return false;
        }
        else {
            $('.households_kids').removeClass('hidden');
        }
        var chart = nv.models.pieChart();

        var svg = d3.select('#PieChart3');

        $('#householdwithoutkidsPercent').html((housewithoutKids / total * 100).toFixed() + '%');
        $('#householdwithkidspercent').html((householdswithKids / total * 100).toFixed() + '%');

        chart.margin({ top: 30, right: 20, bottom: 20, left: 50 })
        if (typeof chart.legend != 'undefined') chart.legend.margin().right -= 30;
        chart.showLegend(false);

        var y_window_formats = 0;
        nv.utils.windowResize(function () {
            y_window_formats = 0;
        });
        if (typeof chart.dispatch.stateChange != 'undefined') {
            // some charts (including bar charts) don't have state changes
            chart.dispatch.on('stateChange.y_window_formats', function () {
                y_window_formats = 0;
            });
        }
        if (typeof chart.yAxis != 'undefined') {
            // some charts (including pie charts) don't have a y axis
            chart.yAxis.tickFormat(function (d) {
                if (chart.yScale().domain()[1] < 10) {
                    return d3.format(',')(d);
                } else {
                    if (this === window) {
                        if (y_window_formats >= 2) {
                            return d3.format(',')(d);
                        }
                        y_window_formats++;
                    }

                    // axis
                    return d3.format(',.0f')(d);;
                }
            });
        }
        if (typeof chart.valueFormat == 'function') {
            // over bar
            chart.valueFormat(d3.format(','));
        }

        chart.color(["#1f77b4", "#aec7e8"]);
        chart.margin({ top: 5, right: 5, bottom: 5, left: 5 });

        chart.tooltip.valueFormatter(function (d) {
            return (d / parseInt($('#PieChart3').attr('house_kids_total')) * 100).toFixed() + ' %<br><span style="font-weight: normal; font-size: 75%">' + d3.format(',')(d) + ' of ' + d3.format(',')(parseInt($('#PieChart3').attr('house_kids_total'))) + '</span>';
        });
        chart.showLabels(false);



        chart.growOnHover(false);

        if (!chart.labelsOutside()) {

            // the current nvd3 version makes the following adjustment even if growOnHover is turned off
            // need to undo it
            // the result is a pie chart that doesn't fill the entire container
            //d.outer = (d.outer - d.outer / 5)

            var arcs = chart.arcsRadius();
            var new_arcs = [];
            if (arcs.length === 0) {
                data[0].values.forEach(function (d) {
                    new_arcs.push({ outer: 1.25, inner: 0 });
                });
            } else {
                arcs.forEach(function (d) {
                    new_arcs.push({ outer: d.outer * 1.25, inner: d.inner * 1.25 });
                });
            }
            chart.arcsRadius(new_arcs);
        }


        // note that a pie chart uses a different data format
        svg.datum(data[0].values)
            .call(chart);

        nv.utils.windowResize(function () { chart.update(); });

        PieChart3_data = data;
        PieChart3_chart = chart;
        PieChart3_svg = svg;

        return chart;
    }, function () {

    });

}


function bindChildrensByAgeChart(childByAge) {
    var LineChart3_data;
    var LineChart3_chart;
    var LineChart3_svg;
    var LineChart3_x_annotations;
    var LineChart3_y_annotations;

    var male1 = parseInt(childByAge.Male1);
    var male2 = parseInt(childByAge.Male2);
    var male3 = parseInt(childByAge.Male3);
    var male4 = parseInt(childByAge.Male4);
    var male5 = parseInt(childByAge.Male5);
    var male6 = parseInt(childByAge.Male6);
    var male7 = parseInt(childByAge.Male7);
    var male8 = parseInt(childByAge.Male8);
    var male9 = parseInt(childByAge.Male9);
    var male10 = parseInt(childByAge.Male10);
    var male11 = parseInt(childByAge.Male11);
    var male12 = parseInt(childByAge.Male12);
    var male13 = parseInt(childByAge.Male13);
    var male14 = parseInt(childByAge.Male14);
    var male15 = parseInt(childByAge.Male15);
    var male16 = parseInt(childByAge.Male16);
    var male17 = parseInt(childByAge.Male17);
    var male18 = parseInt(childByAge.Male18);
    var male19 = parseInt(childByAge.Male19);
    var male20 = parseInt(childByAge.Male20);

    var female1 = parseInt(childByAge.Female1);
    var female2 = parseInt(childByAge.Female2);
    var female3 = parseInt(childByAge.Female3);
    var female4 = parseInt(childByAge.Female4);
    var female5 = parseInt(childByAge.Female5);
    var female6 = parseInt(childByAge.Female6);
    var female7 = parseInt(childByAge.Female7);
    var female8 = parseInt(childByAge.Female8);
    var female9 = parseInt(childByAge.Female9);
    var female10 = parseInt(childByAge.Female10);
    var female11 = parseInt(childByAge.Female11);
    var female12 = parseInt(childByAge.Female12);
    var female13 = parseInt(childByAge.Female13);
    var female14 = parseInt(childByAge.Female14);
    var female15 = parseInt(childByAge.Female15);
    var female16 = parseInt(childByAge.Female16);
    var female17 = parseInt(childByAge.Female17);
    var female18 = parseInt(childByAge.Female18);
    var female19 = parseInt(childByAge.Female19);
    var female20_child = parseInt(childByAge.Female20);

    var totalage1 = male1 + female1;
    var totalage2 = male2 + female2;
    var totalage3 = male3 + female4;
    var totalage4 = male4 + female4;
    var totalage5 = male5 + female5;
    var totalage6 = male6 + female6;
    var totalage7 = male7 + female7;
    var totalage8 = male8 + female8;
    var totalage9 = male9 + female9;
    var totalage10 = male10 + female10;
    var totalage11 = male11 + female11;
    var totalage12 = male12 + female12;
    var totalage13 = male13 + female13;
    var totalage14 = male14 + female14;
    var totalage15 = male15 + female15;
    var totalage16 = male16 + female16;
    var totalage17 = male17 + female17;
    var totalage18 = male18 + female18;
    var totalage19 = male19 + female19;
    var totalage20 = male20 + female20_child;

    $('#male1').html(male1).digits();
    $('#male2').html(male2).digits();
    $('#male3').html(male3).digits();
    $('#male4').html(male4).digits();
    $('#male5').html(male5).digits();
    $('#male6').html(male6).digits();
    $('#male7').html(male7).digits();
    $('#male8').html(male8).digits();
    $('#male9').html(male9).digits();
    $('#male10').html(male10).digits();
    $('#male11').html(male11).digits();
    $('#male12').html(male12).digits();
    $('#male13').html(male13).digits();
    $('#male14').html(male14).digits();
    $('#male15').html(male15).digits();
    $('#male16').html(male16).digits();
    $('#male17').html(male17).digits();
    $('#male18').html(male18).digits();
    $('#male19').html(male19).digits();
    $('#male20').html(male20).digits();

    $('#female1').html(female1).digits();
    $('#female2').html(female2).digits();
    $('#female3').html(female3).digits();
    $('#female4').html(female4).digits();
    $('#female5').html(female5).digits();
    $('#female6').html(female6).digits();
    $('#female7').html(female7).digits();
    $('#female8').html(female8).digits();
    $('#female9').html(female9).digits();
    $('#female10').html(female10).digits();
    $('#female11').html(female11).digits();
    $('#female12').html(female12).digits();
    $('#female13').html(female13).digits();
    $('#female14').html(female14).digits();
    $('#female15').html(female15).digits();
    $('#female16').html(female16).digits();
    $('#female17').html(female17).digits();
    $('#female18').html(female18).digits();
    $('#female19').html(female19).digits();
    $('#female20').html(female20_child).digits();

    $('#totalage1').html(totalage1).digits();
    $('#totalage2').html(totalage2).digits();
    $('#totalage3').html(totalage3).digits();
    $('#totalage4').html(totalage4).digits();
    $('#totalage5').html(totalage5).digits();
    $('#totalage6').html(totalage6).digits();
    $('#totalage7').html(totalage7).digits();
    $('#totalage8').html(totalage8).digits();
    $('#totalage9').html(totalage9).digits();
    $('#totalage10').html(totalage10).digits();
    $('#totalage11').html(totalage11).digits();
    $('#totalage12').html(totalage12).digits();
    $('#totalage13').html(totalage13).digits();
    $('#totalage14').html(totalage14).digits();
    $('#totalage15').html(totalage15).digits();
    $('#totalage16').html(totalage16).digits();
    $('#totalage17').html(totalage17).digits();
    $('#totalage18').html(totalage18).digits();
    $('#totalage19').html(totalage19).digits();
    $('#totalage20').html(totalage20).digits();



    nv.addGraph(function () {
        var data = [{
            "key": "Male", "values": [
                { "x": 0, "y": male1 },
                { "x": 1, "y": male2 },
                { "x": 2, "y": male3 },
                { "x": 3, "y": male4 },
                { "x": 4, "y": male5 },
                { "x": 5, "y": male6 },
                { "x": 6, "y": male7 },
                { "x": 7, "y": male8 },
                { "x": 8, "y": male9 },
                { "x": 9, "y": male10 },
                { "x": 10, "y": male11 },
                { "x": 11, "y": male12 },
                { "x": 12, "y": male13 },
                { "x": 13, "y": male14 },
                { "x": 14, "y": male15 },
                { "x": 15, "y": male16 },
                { "x": 16, "y": male17 },
                { "x": 17, "y": male18 },
                { "x": 18, "y": male19 },
                { "x": 19, "y": male20 }
            ]
        }, {
            "key": "Female", "values": [
                { "x": 0, "y": female1 },
                { "x": 1, "y": female2 },
                { "x": 2, "y": female3 },
                { "x": 3, "y": female4 },
                { "x": 4, "y": female5 },
                { "x": 5, "y": female6 },
                { "x": 6, "y": female7 },
                { "x": 7, "y": female8 },
                { "x": 8, "y": female9 },
                { "x": 9, "y": female10 },
                { "x": 10, "y": female11 },
                { "x": 11, "y": female12 },
                { "x": 12, "y": female13 },
                { "x": 13, "y": female14 },
                { "x": 14, "y": female15 },
                { "x": 15, "y": female16 },
                { "x": 16, "y": female17 },
                { "x": 17, "y": female18 },
                { "x": 18, "y": female19 },
                { "x": 19, "y": female20_child }
            ]
        }, {
            "key": "Total", "color": "#000000", "values": [
                { "x": 0, "y": totalage1 },
                { "x": 1, "y": totalage2 },
                { "x": 2, "y": totalage3 },
                { "x": 3, "y": totalage4 },
                { "x": 4, "y": totalage5 },
                { "x": 5, "y": totalage6 },
                { "x": 6, "y": totalage7 },
                { "x": 7, "y": totalage8 },
                { "x": 8, "y": totalage9 },
                { "x": 9, "y": totalage10 },
                { "x": 10, "y": totalage11 },
                { "x": 11, "y": totalage12 },
                { "x": 12, "y": totalage13 },
                { "x": 13, "y": totalage14 },
                { "x": 14, "y": totalage15 },
                { "x": 15, "y": totalage16 },
                { "x": 16, "y": totalage17 },
                { "x": 17, "y": totalage18 },
                { "x": 18, "y": totalage19 },
                { "x": 19, "y": totalage20 }
            ]
        }];

        var total = 0;
        data[0].values.forEach(function (d) {
            total = total + d.y;
        });
        if (total == 0) {
            $('.children_by_age').addClass('hidden');
            return false;
        }
        else {
            $('.children_by_age').removeClass('hidden');
        }
        var chart = nv.models.lineChart()
            .useInteractiveGuideline(true);

        var svg = d3.select('#LineChart3');

        var x_annotations = [];
        var y_annotations = [];


        chart.margin({ top: 30, right: 20, bottom: 20, left: 50 })
        if (typeof chart.legend != 'undefined') chart.legend.margin().right -= 30;

        if (typeof chart.legend != 'undefined') {
            chart.legend.vers('furious');
        }

        var y_window_formats = 0;
        nv.utils.windowResize(function () {
            y_window_formats = 0;
        });
        if (typeof chart.dispatch.stateChange != 'undefined') {
            // some charts (including bar charts) don't have state changes
            chart.dispatch.on('stateChange.y_window_formats', function () {
                y_window_formats = 0;
            });
        }
        if (typeof chart.yAxis != 'undefined') {
            // some charts (including pie charts) don't have a y axis
            chart.yAxis.tickFormat(function (d) {
                if (chart.yScale().domain()[1] < 10) {
                    return d3.format(',')(d);
                } else {
                    if (this === window) {
                        if (y_window_formats >= 2) {
                            return d3.format(',')(d);
                        }
                        y_window_formats++;
                    }

                    // axis
                    return d3.format(',.0f')(d);;
                }
            });
        }
        if (typeof chart.valueFormat == 'function') {
            // over bar
            chart.valueFormat(d3.format(','));
        }
        chart.forceY(0);

        if (data.length > 2 && parseInt(svg.style('width')) < 768 / 2) {
            chart.showLegend(false);
        }

        var x_discreet_labels = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20"];

        chart.xAxis.tickFormat(function (d) {
            return x_discreet_labels[d];
        });


        svg.datum(data)
            .call(chart);

        nv.utils.windowResize(function () { chart.update(); });

        chart.dispatch.on('renderEnd.alignAnnotations', function () {


        });

        LineChart3_data = data;
        LineChart3_chart = chart;
        LineChart3_svg = svg;
        LineChart3_x_annotations = x_annotations;
        LineChart3_y_annotations = y_annotations;

        return chart;
    }, function () {

    });
}


function bindVacanyReasons(dataVacancy) {
    var PieChart6_data;
    var PieChart6_chart;
    var PieChart6_svg;
    nv.addGraph(function () {
        var data = [{
            "key": "Data", "values": [
                { "x": "For Rent", "y": parseFloat(dataVacancy.vacant_housing_units_for_rent) },
                { "x": "Rented & Unoccupied", "y": parseFloat(dataVacancy.vacant_housing_units_rented_and_unoccupied) },
                { "x": "For Sale Only", "y": parseFloat(dataVacancy.vacant_housing_units_for_sale_only) },
                { "x": "Sold & Unoccupied", "y": parseFloat(dataVacancy.vacant_housing_units_sold_and_unoccupied) },
                { "x": "For Season Recreational Or Occasional Use", "y": parseFloat(dataVacancy.vacant_housing_units_for_season_recreational_or_occasional_use) },
                { "x": "For Migrant Workers", "y": parseFloat(dataVacancy.vacant_housing_units_for_migrant_workers) },
                { "x": "Vacant For Other Reasons", "y": parseFloat(dataVacancy.vacant_housing_units_vacant_for_other_reasons) }]
        }];

        var total = 0;
        data[0].values.forEach(function (d) {
            total = total + d.y;
        });
        $('#PieChart6').attr('vac_rea_total', total);
        if (total == 0) {
            $('.vacancy_reasons').addClass('hidden');
            return false;
        }
        else {
            $('.vacancy_reasons').removeClass('hidden');
        }
        var chart = nv.models.pieChart();

        var svg = d3.select('#PieChart6');
        $('#pie-vacancy-rent').html(dataVacancy.vacant_housing_units_for_rent).digits();
        $('#pie-vacancy-unoccupied').html(dataVacancy.vacant_housing_units_rented_and_unoccupied).digits();
        $('#pie-vacancy-sale').html(dataVacancy.vacant_housing_units_for_sale_only).digits();
        $('#pie-vacancy-sold').html(dataVacancy.vacant_housing_units_sold_and_unoccupied).digits();
        $('#pie-vacancy-season').html(dataVacancy.vacant_housing_units_for_season_recreational_or_occasional_use).digits();
        $('#pie-vacancy-workers').html(dataVacancy.vacant_housing_units_for_migrant_workers).digits();
        $('#pie-vacancy-otherreasons').html(dataVacancy.vacant_housing_units_vacant_for_other_reasons).digits();

        $('#pie-vacancy-rent-perent').html((parseInt(dataVacancy.vacant_housing_units_for_rent) / total * 100).toFixed() + '%');
        $('#pie-vacancy-unoccupied-percent').html((parseInt(dataVacancy.vacant_housing_units_rented_and_unoccupied) / total * 100).toFixed() + '%');
        $('#pie-vacancy-sale-percent').html((parseInt(dataVacancy.vacant_housing_units_for_sale_only) / total * 100).toFixed() + '%');
        $('#pie-vacancy-sold-percent').html((parseInt(dataVacancy.vacant_housing_units_sold_and_unoccupied) / total * 100).toFixed() + '%');
        $('#pie-vacancy-season-percent').html((parseInt(dataVacancy.vacant_housing_units_for_season_recreational_or_occasional_use) / total * 100).toFixed() + '%');
        $('#pie-vacancy-workers-percent').html((parseInt(dataVacancy.vacant_housing_units_for_migrant_workers) / total * 100).toFixed() + '%');
        $('#pie-vacancy-otherreasons-percent').html((parseInt(dataVacancy.vacant_housing_units_vacant_for_other_reasons) / total * 100).toFixed() + '%');



        chart.margin({ top: 30, right: 20, bottom: 20, left: 50 })
        if (typeof chart.legend != 'undefined') chart.legend.margin().right -= 30;

        if (typeof chart.yScale != 'undefined') {
            chart.margin().left -= 3 * 8;
        }
        chart.showLegend(false);

        var y_window_formats = 0;
        nv.utils.windowResize(function () {
            y_window_formats = 0;
        });
        if (typeof chart.dispatch.stateChange != 'undefined') {
            // some charts (including bar charts) don't have state changes
            chart.dispatch.on('stateChange.y_window_formats', function () {
                y_window_formats = 0;
            });
        }
        if (typeof chart.yAxis != 'undefined') {
            // some charts (including pie charts) don't have a y axis
            chart.yAxis.tickFormat(function (d) {
                if (chart.yScale().domain()[1] < 10) {
                    return d3.format(',')(d);
                } else {
                    if (this === window) {
                        if (y_window_formats >= 2) {
                            return d3.format(',')(d);
                        }
                        y_window_formats++;
                    }

                    // axis
                    return d3.format(',.0f')(d);;
                }
            });
        }
        if (typeof chart.valueFormat == 'function') {
            // over bar
            chart.valueFormat(d3.format(','));
        }

        chart.color(["#1f77b4", "#aec7e8", "#ff7f0e", "#ffbb78", "#2ca02c", "#98df8a", "#d62728"]);
        chart.margin({ top: 5, right: 5, bottom: 5, left: 5 });

        chart.tooltip.valueFormatter(function (d) {
            return (d / parseInt($('#PieChart6').attr('vac_rea_total')) * 100).toFixed() + ' %<br><span style="font-weight: normal; font-size: 75%">' + d3.format(',')(d) + ' of ' + d3.format(',')(parseInt($('#PieChart6').attr('vac_rea_total'))) + '</span>';
        });
        chart.showLabels(false);



        chart.growOnHover(false);

        if (!chart.labelsOutside()) {

            // the current nvd3 version makes the following adjustment even if growOnHover is turned off
            // need to undo it
            // the result is a pie chart that doesn't fill the entire container
            //d.outer = (d.outer - d.outer / 5)

            var arcs = chart.arcsRadius();
            var new_arcs = [];
            if (arcs.length === 0) {
                data[0].values.forEach(function (d) {
                    new_arcs.push({ outer: 1.25, inner: 0 });
                });
            } else {
                arcs.forEach(function (d) {
                    new_arcs.push({ outer: d.outer * 1.25, inner: d.inner * 1.25 });
                });
            }
            chart.arcsRadius(new_arcs);
        }


        // note that a pie chart uses a different data format
        svg.datum(data[0].values)
            .call(chart);

        nv.utils.windowResize(function () { chart.update(); });

        PieChart6_data = data;
        PieChart6_chart = chart;
        PieChart6_svg = svg;

        return chart;
    }, function () {

    });
}

function bindHousingOccupancy(dataHouse) {
    var PieChart5_data;
    var PieChart5_chart;
    var PieChart5_svg;
    nv.addGraph(function () {
        var data = [{ "key": "Data", "values": [{ "x": "Owned Households With A Mortgage", "y": parseFloat(dataHouse.owned_households_with_a_mortgage_or_loan) }, { "x": "Owned Households Free & Clear", "y": parseFloat(dataHouse.owned_households_free_and_clear) }, { "x": "Renter Occupied Households", "y": parseFloat(dataHouse.renter_occupied_households) }, { "x": "Households Vacant", "y": parseFloat(dataHouse.total_households_by_vacancy_status) }] }];

        var total = 0;
        data[0].values.forEach(function (d) {
            total = total + d.y;
        });
        $('#PieChart5').attr('hous_occ_total', total);
        if (total == 0) {
            $('.housing_occupancy').addClass('hidden');
            return false;
        }
        else {
            $('.housing_occupancy').removeClass('hidden');
        }
        var chart = nv.models.pieChart();

        var svg = d3.select('#PieChart5');
        $('#pie-occupancy-mortgage').html(dataHouse.owned_households_with_a_mortgage_or_loan).digits();
        $('#pie-occupancy-free').html(dataHouse.owned_households_free_and_clear).digits();
        $('#pie-occupancy-renter').html(dataHouse.total_rented_households_by_age).digits();
        $('#pie-occupancy-vacant').html(dataHouse.total_households_by_vacancy_status).digits();

        $('#pie-occupancy-mortgage-percent').html((parseInt(dataHouse.owned_households_with_a_mortgage_or_loan) / total * 100).toFixed(1) + '%');
        $('#pie-occupancy-free-percent').html((parseInt(dataHouse.owned_households_free_and_clear) / total * 100).toFixed(1) + '%');
        $('#pie-occupancy-renter-percent').html((parseInt(dataHouse.total_rented_households_by_age) / total * 100).toFixed(1) + '%');
        $('#pie-occupancy-vacant-percent').html((parseInt(dataHouse.total_households_by_vacancy_status) / total * 100).toFixed(1) + '%');


        chart.margin({ top: 30, right: 20, bottom: 20, left: 50 })
        if (typeof chart.legend != 'undefined') chart.legend.margin().right -= 30;
        chart.showLegend(false);

        var y_window_formats = 0;
        nv.utils.windowResize(function () {
            y_window_formats = 0;
        });
        if (typeof chart.dispatch.stateChange != 'undefined') {
            // some charts (including bar charts) don't have state changes
            chart.dispatch.on('stateChange.y_window_formats', function () {
                y_window_formats = 0;
            });
        }
        if (typeof chart.yAxis != 'undefined') {
            // some charts (including pie charts) don't have a y axis
            chart.yAxis.tickFormat(function (d) {
                if (chart.yScale().domain()[1] < 10) {
                    return d3.format(',')(d);
                } else {
                    if (this === window) {
                        if (y_window_formats >= 2) {
                            return d3.format(',')(d);
                        }
                        y_window_formats++;
                    }

                    // axis
                    return d3.format(',.0f')(d);;
                }
            });
        }
        if (typeof chart.valueFormat == 'function') {
            // over bar
            chart.valueFormat(d3.format(','));
        }

        chart.color(["#1f77b4", "#aec7e8", "#ff7f0e", "#ffbb78"]);
        chart.margin({ top: 5, right: 5, bottom: 5, left: 5 });

        chart.tooltip.valueFormatter(function (d) {
            return (d / parseInt($('#PieChart5').attr('hous_occ_total')) * 100).toFixed() + ' %<br><span style="font-weight: normal; font-size: 75%">' + d3.format(',')(d) + ' of ' + d3.format(',')(parseInt($('#PieChart5').attr('hous_occ_total'))) + '</span>';
        });
        chart.showLabels(false);



        chart.growOnHover(false);

        if (!chart.labelsOutside()) {

            // the current nvd3 version makes the following adjustment even if growOnHover is turned off
            // need to undo it
            // the result is a pie chart that doesn't fill the entire container
            //d.outer = (d.outer - d.outer / 5)

            var arcs = chart.arcsRadius();
            var new_arcs = [];
            if (arcs.length === 0) {
                data[0].values.forEach(function (d) {
                    new_arcs.push({ outer: 1.25, inner: 0 });
                });
            } else {
                arcs.forEach(function (d) {
                    new_arcs.push({ outer: d.outer * 1.25, inner: d.inner * 1.25 });
                });
            }
            chart.arcsRadius(new_arcs);
        }


        // note that a pie chart uses a different data format
        svg.datum(data[0].values)
            .call(chart);

        nv.utils.windowResize(function () { chart.update(); });

        PieChart5_data = data;
        PieChart5_chart = chart;
        PieChart5_svg = svg;

        return chart;
    }, function () {

    });
}

function bindHousing(housingData) {
    //Housing Type
    var PieChart4_data;
    var PieChart4_chart;
    var PieChart4_svg;
    nv.addGraph(function () {
        var data = [{ "key": "Data", "values": [{ "x": "In Occupied Housing Units", "y": parseFloat(housingData.population_in_occupied_housing_units) }, { "x": "Correctional Facility For Adults", "y": parseFloat(housingData.correctional_facility_for_adults_population) }, { "x": "Juvenile Facilities", "y": parseFloat(housingData.juvenile_facilities_population) }, { "x": "Nursing Facilities", "y": parseFloat(housingData.nursing_facilities_population) }, { "x": "Other Institutional", "y": parseFloat(housingData.other_institutional_population) }, { "x": "College Student Housing", "y": parseFloat(housingData.college_student_housing_population) }, { "x": "Military Quarters", "y": parseFloat(housingData.military_quarters_population) }, { "x": "Other Noninstitutional", "y": parseFloat(housingData.other_noninstitutional_population) }] }];

        var total = 0;
        data[0].values.forEach(function (d) {
            total = total + d.y;
        });
        $('#PieChart4').attr('hous_type_total', total);
        if (total == 0) {
            $('.housing_type').addClass('hidden');
            return false;
        }
        else {
            $('.housing_type').removeClass('hidden');
        }
        var chart = nv.models.pieChart();

        var svg = d3.select('#PieChart4');
        var pie_occupied_housing = $('#pie_occupied_housing').html(housingData.population_in_occupied_housing_units).digits();
        var pie_correctional = $('#pie_correctional').html(housingData.correctional_facility_for_adults_population).digits();
        var pie_Juvenile = $('#pie_Juvenile').html(housingData.juvenile_facilities_population).digits();
        var pie_Nursing = $('#pie_Nursing').html(housingData.nursing_facilities_population).digits();
        var pie_Institutional_count = $('#pie_Institutional_count').html(housingData.other_institutional_population).digits();
        var pie_College_count = $('#pie_College_count').html(housingData.college_student_housing_population).digits();
        var pie_Military_count = $('#pie_Military_count').html(housingData.military_quarters_population).digits();
        var pie_Noninstitutional_count = $('#pie_Noninstitutional_count').html(housingData.other_noninstitutional_population).digits();


        var pie_occupied_housing_percent = $('#pie_occupied_housing_percent').html((parseInt(housingData.population_in_occupied_housing_units) / total * 100).toFixed(1) + '%');
        var pie_correctional = $('#pie_correctional_percent').html((parseInt(housingData.correctional_facility_for_adults_population) / total * 100).toFixed(1) + '%');
        var pie_Juvenile = $('#pie_Juvenile_percent').html((parseInt(housingData.juvenile_facilities_population) / total * 100).toFixed(1) + '%');
        var pie_Nursing = $('#pie_Nursing_percent').html((parseInt(housingData.nursing_facilities_population) / total * 100).toFixed(1) + '%');
        var pie_Institutional_count = $('#pie_Institutional_percent').html((parseInt(housingData.other_institutional_population) / total * 100).toFixed(1) + '%');
        var pie_College_count = $('#pie_College_percent').html((parseInt(housingData.college_student_housing_population) / total * 100).toFixed(1) + '%');
        var pie_Military_count = $('#pie_Military_percent').html((parseInt(housingData.military_quarters_population) / total * 100).toFixed(1) + '%');
        var pie_Noninstitutional_count = $('#pie_Noninstitutional_percent').html((parseInt(housingData.other_noninstitutional_population) / total * 100).toFixed(1) + '%');

        //var pie_Institutional_count = $('#pie_Institutional_count').html((parseInt(genData.TotalMalePopulation) / total * 100).toFixed() + '%');
        // var femalePercent = $('#femalePercent').html((parseInt(genData.TotalFemalePopulation) / total * 100).toFixed() + '%');

        chart.margin({ top: 30, right: 20, bottom: 20, left: 50 })
        if (typeof chart.legend != 'undefined') chart.legend.margin().right -= 30;
        chart.showLegend(false);

        var y_window_formats = 0;
        nv.utils.windowResize(function () {
            y_window_formats = 0;
        });
        if (typeof chart.dispatch.stateChange != 'undefined') {
            // some charts (including bar charts) don't have state changes
            chart.dispatch.on('stateChange.y_window_formats', function () {
                y_window_formats = 0;
            });
        }
        if (typeof chart.yAxis != 'undefined') {
            // some charts (including pie charts) don't have a y axis
            chart.yAxis.tickFormat(function (d) {
                if (chart.yScale().domain()[1] < 10) {
                    return d3.format(',')(d);
                } else {
                    // tooltip via https://github.com/novus/nvd3/issues/428
                    if (this === window) {
                        if (y_window_formats >= 2) {
                            return d3.format(',')(d);
                        }
                        y_window_formats++;
                    }

                    // axis
                    return d3.format(',.0f')(d);;
                }
            });
        }
        if (typeof chart.valueFormat == 'function') {
            // over bar
            chart.valueFormat(d3.format(','));
        }

        chart.color(["#1f77b4", "#aec7e8", "#ff7f0e", "#ffbb78", "#2ca02c", "#98df8a", "#d62728", "#ff9896"]);
        chart.margin({ top: 5, right: 5, bottom: 5, left: 5 });

        chart.tooltip.valueFormatter(function (d) {
            return (d / parseInt($('#PieChart4').attr('hous_type_total')) * 100).toFixed() + ' %<br><span style="font-weight: normal; font-size: 75%">' + d3.format(',')(d) + ' of ' + d3.format(',')(parseInt($('#PieChart4').attr('hous_type_total'))) + '</span>';
        });
        chart.showLabels(false);



        chart.growOnHover(false);

        if (!chart.labelsOutside()) {

            // the current nvd3 version makes the following adjustment even if growOnHover is turned off
            // need to undo it
            // the result is a pie chart that doesn't fill the entire container
            //d.outer = (d.outer - d.outer / 5)

            var arcs = chart.arcsRadius();
            var new_arcs = [];
            if (arcs.length === 0) {
                data[0].values.forEach(function (d) {
                    new_arcs.push({ outer: 1.25, inner: 0 });
                });
            } else {
                arcs.forEach(function (d) {
                    new_arcs.push({ outer: d.outer * 1.25, inner: d.inner * 1.25 });
                });
            }
            chart.arcsRadius(new_arcs);
        }


        // note that a pie chart uses a different data format
        svg.datum(data[0].values)
            .call(chart);

        nv.utils.windowResize(function () { chart.update(); });

        PieChart4_data = data;
        PieChart4_chart = chart;
        PieChart4_svg = svg;

        return chart;
    }, function () {

    });
    //Housing Type
}


//Chart Map Functionality//
var holdvalue1 = "", state = "", county = "", zipcode = 0;
var allow = false;
var pageload = 0;

$(document).ready(function () {
   

    $('.btn-search-location').on('click touchstart', function () {
        allow = false;
        var textValue = $('#q').val();
        cleanValidation();
        if (textValue != '') {
            if ($.isNumeric(textValue.trim()) && textValue.trim().length != 5) {
                customAlert('ZIP Code should be 5 digits');
                plainValidation($('#q'));
                return false;
            }
            GetBounds();
            $('.map_image').css('width', '58.3333%');
        }
        else {
            customAlert('Please enter ZIP Code or address');
            plainValidation($('#q'));
            return false;
        }
    });

    $('body').on('click', '.state-links a,#state_demographics', function () {
        $('#q').val($(this).text());
        $('.map_image').css('width', '58.3333%');
        allow = false;
        GetBounds();
        return false;
    });



    //Bind polygon end//
    var linkys = document.getElementsByClassName('dropdown-toggle');
    for (var i = 0; i < linkys.length; ++i) {
        linkys[i].onclick = function (e) {
            this.focus();
            e.preventDefault();
        };
    }
    $('.zip_title').addClass('hidden');
    $('.map_image').css({ 'width': '100%', 'height': '100%' });
    $('#full_chart_section').addClass('hidden');



});
function GetBounds() {

    var con = $('#q').val();

    if (con != "") {

        var geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'address': con }, function (results, status) {

            if (status == google.maps.GeocoderStatus.OK) {

                var x = results[0].geometry.location.lat();
                var y = results[0].geometry.location.lng();
                latitutde = x;
                longitude = y;

                if ($('#q').val().split(',').length > 2) {
                    var latlng = new google.maps.LatLng(x, y);
                    var geocoder1 = new google.maps.Geocoder();
                    geocoder1.geocode({ 'latLng': latlng }, function (results1, status1) {
                        if (results1.length > 0) {

                            for (var k = 0; k < results1[0].address_components.length; k++) {
                                if (results1[0].address_components[k].types[0] == "postal_code") {
                                    zipcode = results1[0].address_components[k].short_name;
                                    GetBoundZip(zipcode);
                                }
                            }

                        }
                    });
                }
                else {
                    var first = results[0].geometry.viewport.b;
                    var second = results[0].geometry.viewport.f;
                    bounds = { "south": 33.935, "west": -81.171, "north": 34.133, "east": -80.704 };
                    bounds = { "south": second.b, "west": first.b, "north": second.f, "east": first.f };
                    if (!allow)
                        BindDataByZip();
                    if (!allow)
                        BindDataByState();
                    if (!allow)
                        BindDataByCounty();

                    bindChartData(con);
                }



            } else {
            }
        });
    }
}

function GetBoundZip(zip) {
    if (zip != "") {
        var geocoder = new google.maps.Geocoder();
        geocoder.geocode({ 'address': zip }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                var x = results[0].geometry.location.lat();
                var y = results[0].geometry.location.lng();
                latitutde = x;
                longitude = y;
                var first = results[0].geometry.viewport.b;
                var second = results[0].geometry.viewport.f;
                bounds = { "south": 33.935, "west": -81.171, "north": 34.133, "east": -80.704 };
                bounds = { "south": second.b, "west": first.b, "north": second.f, "east": first.f };
                if (!allow)
                    BindDataZip(zip);                 
            } else {
            }
        });
    }
}



function BindDataZip(zip) {
   
    var value = zip;
    BindDataByZip(value);    
    bindChartData(zip);
}
function BindDataByZip(zip) {
    var value = $('#q').val().trim();
    if (zip != undefined)
        value = zip;
     
    if (value != "")
    {
        $.ajax({
            url: '/ERSEA/GetgeometryByZip',
            type: 'post',
            async :false,
            data: { 'Zipcode': value },
            success: function (data) {
                console.log(JSON.parse(data));
                geojson = { "type": "FeatureCollection", "features": [{ "type": "Feature", "properties": { "fillOpacity": 0, "strokeColor": "#FF0000", "strokeOpacity": 0.5 }, "geometry": { "type": "MultiPolygon", "coordinates": [[[[-80.997985, 34.046995], [-80.997463, 34.047383], [-80.997399, 34.047439], [-80.997334, 34.047363], [-80.9973, 34.047323], [-80.997261, 34.047214], [-80.997242, 34.047162], [-80.997216, 34.046987], [-80.997216, 34.046978], [-80.9973, 34.046978], [-80.997985, 34.046995]]], [[[-81.01, 34.066], [-81.009, 34.066], [-81.009, 34.067], [-81.008, 34.067], [-81.007, 34.065], [-81.008, 34.064], [-81.009, 34.064], [-81.01, 34.066]]], [[[-81.031, 34.061], [-81.03, 34.062], [-81.03, 34.061], [-81.031, 34.061]]], [[[-81.097, 34.034], [-81.097, 34.033], [-81.098, 34.033], [-81.098, 34.034], [-81.097, 34.034]]], [[[-81.142, 34.082], [-81.141, 34.083], [-81.141, 34.084], [-81.141, 34.085], [-81.14, 34.085], [-81.14, 34.084], [-81.139, 34.084], [-81.138, 34.084], [-81.138, 34.083], [-81.139, 34.083], [-81.139, 34.082], [-81.14, 34.082], [-81.142, 34.082]]], [[[-81.171, 34.091], [-81.17, 34.091], [-81.169, 34.091], [-81.168, 34.091], [-81.168, 34.092], [-81.168, 34.093], [-81.167, 34.093], [-81.166, 34.093], [-81.165, 34.093], [-81.166, 34.093], [-81.165, 34.094], [-81.165, 34.093], [-81.164, 34.093], [-81.164, 34.094], [-81.163, 34.094], [-81.163, 34.093], [-81.163, 34.092], [-81.162, 34.092], [-81.161, 34.092], [-81.16, 34.092], [-81.16, 34.093], [-81.161, 34.093], [-81.162, 34.094], [-81.163, 34.094], [-81.162, 34.094], [-81.162, 34.095], [-81.161, 34.094], [-81.16, 34.094], [-81.16, 34.095], [-81.16, 34.096], [-81.161, 34.095], [-81.161, 34.096], [-81.161, 34.097], [-81.162, 34.097], [-81.161, 34.098], [-81.162, 34.098], [-81.162, 34.097], [-81.163, 34.097], [-81.163, 34.099], [-81.163, 34.1], [-81.162, 34.102], [-81.162, 34.101], [-81.161, 34.101], [-81.16, 34.101], [-81.16, 34.1], [-81.16, 34.099], [-81.161, 34.099], [-81.162, 34.099], [-81.16, 34.099], [-81.16, 34.097], [-81.159, 34.097], [-81.159, 34.098], [-81.159, 34.097], [-81.158, 34.098], [-81.157, 34.098], [-81.156, 34.097], [-81.155, 34.097], [-81.155, 34.096], [-81.156, 34.097], [-81.157, 34.097], [-81.156, 34.096], [-81.156, 34.095], [-81.157, 34.096], [-81.157, 34.095], [-81.157, 34.094], [-81.156, 34.094], [-81.156, 34.093], [-81.156, 34.092], [-81.157, 34.092], [-81.157, 34.091], [-81.157, 34.09], [-81.158, 34.09], [-81.158, 34.089], [-81.157, 34.089], [-81.156, 34.089], [-81.155, 34.089], [-81.154, 34.089], [-81.153, 34.092], [-81.151, 34.093], [-81.15, 34.094], [-81.149, 34.095], [-81.148, 34.096], [-81.147, 34.096], [-81.147, 34.097], [-81.146, 34.098], [-81.145, 34.099], [-81.144, 34.1], [-81.142, 34.102], [-81.14, 34.103], [-81.14, 34.104], [-81.14, 34.105], [-81.14, 34.104], [-81.139, 34.104], [-81.139, 34.105], [-81.139, 34.106], [-81.138, 34.106], [-81.137, 34.106], [-81.138, 34.106], [-81.138, 34.107], [-81.138, 34.106], [-81.137, 34.106], [-81.137, 34.107], [-81.137, 34.108], [-81.136, 34.108], [-81.135, 34.108], [-81.135, 34.107], [-81.135, 34.108], [-81.136, 34.108], [-81.136, 34.109], [-81.135, 34.109], [-81.135, 34.11], [-81.134, 34.109], [-81.133, 34.109], [-81.131, 34.112], [-81.13, 34.112], [-81.13, 34.113], [-81.131, 34.113], [-81.131, 34.114], [-81.132, 34.114], [-81.132, 34.115], [-81.133, 34.115], [-81.133, 34.114], [-81.133, 34.113], [-81.134, 34.113], [-81.135, 34.113], [-81.135, 34.114], [-81.136, 34.114], [-81.138, 34.115], [-81.138, 34.116], [-81.139, 34.116], [-81.139, 34.117], [-81.139, 34.118], [-81.138, 34.118], [-81.137, 34.119], [-81.137, 34.12], [-81.137, 34.121], [-81.138, 34.121], [-81.138, 34.12], [-81.139, 34.12], [-81.14, 34.12], [-81.141, 34.12], [-81.142, 34.12], [-81.143, 34.12], [-81.144, 34.119], [-81.143, 34.119], [-81.142, 34.119], [-81.142, 34.118], [-81.143, 34.117], [-81.143, 34.116], [-81.143, 34.115], [-81.144, 34.115], [-81.144, 34.114], [-81.145, 34.114], [-81.146, 34.114], [-81.146, 34.115], [-81.146, 34.116], [-81.146, 34.117], [-81.146, 34.118], [-81.146, 34.119], [-81.145, 34.119], [-81.146, 34.119], [-81.146, 34.12], [-81.147, 34.121], [-81.147, 34.12], [-81.148, 34.12], [-81.149, 34.12], [-81.15, 34.12], [-81.15, 34.121], [-81.15, 34.122], [-81.15, 34.123], [-81.15, 34.122], [-81.151, 34.123], [-81.151, 34.124], [-81.152, 34.125], [-81.153, 34.124], [-81.154, 34.123], [-81.154, 34.124], [-81.155, 34.125], [-81.155, 34.126], [-81.154, 34.126], [-81.153, 34.126], [-81.153, 34.127], [-81.152, 34.127], [-81.151, 34.127], [-81.152, 34.128], [-81.152, 34.129], [-81.153, 34.13], [-81.151, 34.131], [-81.149, 34.131], [-81.149, 34.132], [-81.147, 34.132], [-81.146, 34.132], [-81.145, 34.132], [-81.144, 34.132], [-81.142, 34.132], [-81.139, 34.133], [-81.138, 34.133], [-81.137, 34.133], [-81.137, 34.132], [-81.136, 34.132], [-81.136, 34.131], [-81.136, 34.13], [-81.135, 34.13], [-81.135, 34.129], [-81.135, 34.128], [-81.134, 34.128], [-81.134, 34.127], [-81.133, 34.127], [-81.133, 34.126], [-81.132, 34.126], [-81.132, 34.125], [-81.131, 34.125], [-81.132, 34.125], [-81.133, 34.124], [-81.135, 34.123], [-81.137, 34.121], [-81.137, 34.12], [-81.137, 34.119], [-81.137, 34.118], [-81.138, 34.118], [-81.138, 34.117], [-81.139, 34.117], [-81.139, 34.116], [-81.138, 34.116], [-81.138, 34.115], [-81.137, 34.115], [-81.136, 34.114], [-81.135, 34.113], [-81.134, 34.113], [-81.134, 34.114], [-81.133, 34.115], [-81.132, 34.115], [-81.132, 34.114], [-81.131, 34.114], [-81.131, 34.113], [-81.13, 34.113], [-81.129, 34.113], [-81.129, 34.112], [-81.128, 34.112], [-81.128, 34.111], [-81.127, 34.111], [-81.126, 34.112], [-81.125, 34.112], [-81.124, 34.112], [-81.123, 34.112], [-81.123, 34.111], [-81.122, 34.111], [-81.122, 34.112], [-81.121, 34.112], [-81.12, 34.112], [-81.12, 34.113], [-81.12, 34.112], [-81.119, 34.112], [-81.119, 34.111], [-81.119, 34.11], [-81.119, 34.109], [-81.119, 34.108], [-81.118, 34.108], [-81.118, 34.107], [-81.118, 34.106], [-81.117, 34.106], [-81.117, 34.105], [-81.116, 34.105], [-81.115, 34.105], [-81.114, 34.104], [-81.113, 34.104], [-81.112, 34.104], [-81.112, 34.103], [-81.111, 34.103], [-81.11, 34.102], [-81.11, 34.101], [-81.109, 34.101], [-81.109, 34.1], [-81.108, 34.099], [-81.108, 34.098], [-81.108, 34.097], [-81.107, 34.097], [-81.107, 34.096], [-81.107, 34.095], [-81.106, 34.095], [-81.106, 34.094], [-81.106, 34.093], [-81.105, 34.092], [-81.105, 34.091], [-81.104, 34.091], [-81.104, 34.09], [-81.103, 34.089], [-81.102, 34.089], [-81.102, 34.088], [-81.101, 34.088], [-81.1, 34.088], [-81.1, 34.087], [-81.099, 34.087], [-81.098, 34.087], [-81.098, 34.086], [-81.097, 34.086], [-81.097, 34.085], [-81.096, 34.085], [-81.095, 34.085], [-81.095, 34.084], [-81.094, 34.084], [-81.094, 34.083], [-81.093, 34.083], [-81.093, 34.082], [-81.092, 34.082], [-81.091, 34.081], [-81.09, 34.081], [-81.09, 34.08], [-81.089, 34.08], [-81.088, 34.08], [-81.088, 34.079], [-81.088, 34.078], [-81.087, 34.078], [-81.087, 34.077], [-81.087, 34.076], [-81.086, 34.076], [-81.086, 34.075], [-81.086, 34.074], [-81.085, 34.074], [-81.085, 34.073], [-81.084, 34.073], [-81.083, 34.073], [-81.082, 34.073], [-81.076, 34.078], [-81.075, 34.079], [-81.074, 34.079], [-81.072, 34.08], [-81.071, 34.081], [-81.071, 34.082], [-81.07, 34.081], [-81.069, 34.081], [-81.068, 34.081], [-81.069, 34.081], [-81.069, 34.08], [-81.068, 34.08], [-81.067, 34.079], [-81.067, 34.078], [-81.066, 34.078], [-81.068, 34.077], [-81.067, 34.075], [-81.066, 34.074], [-81.068, 34.072], [-81.067, 34.072], [-81.067, 34.071], [-81.068, 34.072], [-81.069, 34.071], [-81.071, 34.069], [-81.073, 34.068], [-81.072, 34.068], [-81.073, 34.068], [-81.073, 34.067], [-81.073, 34.068], [-81.076, 34.065], [-81.078, 34.064], [-81.078, 34.063], [-81.077, 34.063], [-81.077, 34.062], [-81.077, 34.061], [-81.076, 34.061], [-81.076, 34.06], [-81.076, 34.059], [-81.075, 34.059], [-81.075, 34.058], [-81.075, 34.057], [-81.075, 34.056], [-81.074, 34.056], [-81.074, 34.055], [-81.074, 34.054], [-81.074, 34.053], [-81.074, 34.052], [-81.073, 34.052], [-81.073, 34.051], [-81.073, 34.05], [-81.073, 34.049], [-81.072, 34.048], [-81.072, 34.049], [-81.071, 34.049], [-81.069, 34.05], [-81.069, 34.049], [-81.068, 34.049], [-81.068, 34.05], [-81.068, 34.048], [-81.068, 34.047], [-81.069, 34.046], [-81.069, 34.045], [-81.069, 34.044], [-81.068, 34.042], [-81.068, 34.041], [-81.068, 34.039], [-81.067, 34.039], [-81.066, 34.039], [-81.067, 34.039], [-81.067, 34.038], [-81.068, 34.038], [-81.068, 34.037], [-81.068, 34.035], [-81.067, 34.034], [-81.067, 34.033], [-81.067, 34.032], [-81.068, 34.032], [-81.067, 34.032], [-81.066, 34.033], [-81.066, 34.034], [-81.065, 34.034], [-81.064, 34.035], [-81.064, 34.034], [-81.062, 34.033], [-81.062, 34.032], [-81.063, 34.032], [-81.064, 34.032], [-81.065, 34.031], [-81.064, 34.03], [-81.065, 34.03], [-81.064, 34.029], [-81.063, 34.029], [-81.062, 34.029], [-81.062, 34.03], [-81.062, 34.029], [-81.062, 34.028], [-81.061, 34.028], [-81.061, 34.027], [-81.06, 34.028], [-81.059, 34.028], [-81.058, 34.029], [-81.057, 34.029], [-81.057, 34.03], [-81.058, 34.03], [-81.059, 34.03], [-81.06, 34.029], [-81.059, 34.03], [-81.06, 34.03], [-81.06, 34.031], [-81.059, 34.032], [-81.059, 34.033], [-81.059, 34.034], [-81.06, 34.034], [-81.06, 34.035], [-81.059, 34.035], [-81.059, 34.036], [-81.059, 34.037], [-81.06, 34.036], [-81.06, 34.037], [-81.06, 34.038], [-81.059, 34.038], [-81.059, 34.039], [-81.059, 34.04], [-81.058, 34.041], [-81.058, 34.042], [-81.06, 34.042], [-81.061, 34.042], [-81.061, 34.041], [-81.062, 34.041], [-81.062, 34.042], [-81.062, 34.043], [-81.062, 34.044], [-81.061, 34.045], [-81.062, 34.045], [-81.062, 34.046], [-81.061, 34.046], [-81.058, 34.045], [-81.057, 34.045], [-81.057, 34.044], [-81.058, 34.044], [-81.057, 34.044], [-81.057, 34.045], [-81.056, 34.045], [-81.056, 34.044], [-81.056, 34.045], [-81.055, 34.045], [-81.055, 34.046], [-81.054, 34.046], [-81.054, 34.047], [-81.053, 34.047], [-81.052, 34.047], [-81.052, 34.048], [-81.053, 34.048], [-81.052, 34.048], [-81.051, 34.048], [-81.051, 34.049], [-81.05, 34.049], [-81.049, 34.049], [-81.048, 34.05], [-81.048, 34.051], [-81.047, 34.051], [-81.047, 34.05], [-81.046, 34.05], [-81.045, 34.05], [-81.045, 34.051], [-81.045, 34.05], [-81.045, 34.051], [-81.044, 34.051], [-81.043, 34.051], [-81.044, 34.051], [-81.043, 34.051], [-81.042, 34.051], [-81.041, 34.051], [-81.04, 34.051], [-81.039, 34.051], [-81.039, 34.05], [-81.038, 34.05], [-81.037, 34.05], [-81.036, 34.05], [-81.037, 34.05], [-81.037, 34.051], [-81.036, 34.051], [-81.037, 34.052], [-81.036, 34.052], [-81.036, 34.053], [-81.036, 34.054], [-81.036, 34.053], [-81.037, 34.054], [-81.036, 34.054], [-81.037, 34.055], [-81.037, 34.056], [-81.038, 34.056], [-81.039, 34.056], [-81.039, 34.057], [-81.038, 34.057], [-81.039, 34.057], [-81.039, 34.058], [-81.04, 34.058], [-81.04, 34.057], [-81.04, 34.058], [-81.041, 34.058], [-81.042, 34.058], [-81.042, 34.057], [-81.042, 34.058], [-81.042, 34.059], [-81.043, 34.059], [-81.043, 34.06], [-81.044, 34.059], [-81.044, 34.06], [-81.044, 34.061], [-81.045, 34.061], [-81.045, 34.062], [-81.046, 34.062], [-81.046, 34.063], [-81.047, 34.063], [-81.047, 34.064], [-81.048, 34.064], [-81.049, 34.064], [-81.049, 34.065], [-81.048, 34.065], [-81.048, 34.066], [-81.047, 34.066], [-81.046, 34.066], [-81.045, 34.065], [-81.044, 34.065], [-81.044, 34.064], [-81.044, 34.063], [-81.043, 34.063], [-81.042, 34.064], [-81.042, 34.063], [-81.041, 34.063], [-81.04, 34.064], [-81.039, 34.062], [-81.036, 34.062], [-81.035, 34.062], [-81.034, 34.063], [-81.033, 34.063], [-81.032, 34.063], [-81.031, 34.063], [-81.032, 34.061], [-81.033, 34.062], [-81.034, 34.062], [-81.036, 34.062], [-81.036, 34.061], [-81.036, 34.06], [-81.036, 34.059], [-81.035, 34.059], [-81.034, 34.059], [-81.033, 34.059], [-81.033, 34.058], [-81.032, 34.059], [-81.031, 34.059], [-81.03, 34.059], [-81.03, 34.06], [-81.028, 34.06], [-81.028, 34.061], [-81.027, 34.061], [-81.029, 34.061], [-81.03, 34.061], [-81.03, 34.062], [-81.028, 34.064], [-81.029, 34.064], [-81.03, 34.064], [-81.03, 34.063], [-81.03, 34.064], [-81.031, 34.064], [-81.033, 34.064], [-81.034, 34.064], [-81.033, 34.064], [-81.034, 34.065], [-81.034, 34.064], [-81.035, 34.064], [-81.036, 34.064], [-81.037, 34.064], [-81.037, 34.065], [-81.037, 34.066], [-81.037, 34.067], [-81.038, 34.067], [-81.038, 34.068], [-81.038, 34.069], [-81.039, 34.072], [-81.042, 34.072], [-81.043, 34.072], [-81.044, 34.072], [-81.043, 34.072], [-81.043, 34.073], [-81.042, 34.073], [-81.041, 34.073], [-81.041, 34.074], [-81.04, 34.074], [-81.041, 34.074], [-81.041, 34.075], [-81.04, 34.075], [-81.04, 34.076], [-81.039, 34.076], [-81.039, 34.077], [-81.038, 34.077], [-81.037, 34.077], [-81.037, 34.078], [-81.036, 34.077], [-81.036, 34.07], [-81.036, 34.068], [-81.035, 34.067], [-81.035, 34.066], [-81.034, 34.066], [-81.033, 34.066], [-81.032, 34.066], [-81.031, 34.066], [-81.029, 34.066], [-81.028, 34.066], [-81.027, 34.066], [-81.026, 34.066], [-81.025, 34.066], [-81.024, 34.066], [-81.023, 34.067], [-81.022, 34.067], [-81.022, 34.066], [-81.023, 34.065], [-81.023, 34.064], [-81.023, 34.063], [-81.024, 34.063], [-81.025, 34.063], [-81.025, 34.062], [-81.025, 34.061], [-81.026, 34.061], [-81.025, 34.06], [-81.024, 34.061], [-81.024, 34.062], [-81.023, 34.062], [-81.022, 34.061], [-81.022, 34.06], [-81.021, 34.06], [-81.021, 34.059], [-81.02, 34.059], [-81.019, 34.059], [-81.02, 34.058], [-81.019, 34.058], [-81.018, 34.058], [-81.018, 34.059], [-81.017, 34.058], [-81.017, 34.059], [-81.017, 34.058], [-81.016, 34.059], [-81.016, 34.06], [-81.015, 34.06], [-81.016, 34.059], [-81.016, 34.058], [-81.015, 34.058], [-81.015, 34.059], [-81.015, 34.058], [-81.016, 34.058], [-81.015, 34.058], [-81.015, 34.057], [-81.014, 34.057], [-81.014, 34.058], [-81.013, 34.058], [-81.012, 34.058], [-81.011, 34.059], [-81.01, 34.059], [-81.01, 34.06], [-81.009, 34.061], [-81.01, 34.062], [-81.01, 34.063], [-81.009, 34.062], [-81.009, 34.063], [-81.008, 34.064], [-81.007, 34.065], [-81.007, 34.066], [-81.006, 34.067], [-81.006, 34.068], [-81.005, 34.068], [-81.005, 34.069], [-81.004, 34.069], [-81.003, 34.07], [-81.002, 34.07], [-81.003, 34.071], [-81.004, 34.071], [-81.005, 34.071], [-81.004, 34.072], [-81.005, 34.073], [-81.006, 34.074], [-81.005, 34.074], [-81.004, 34.074], [-81.003, 34.074], [-81.002, 34.074], [-81.001, 34.074], [-81, 34.074], [-81, 34.075], [-81, 34.076], [-80.999, 34.075], [-80.999, 34.076], [-80.998, 34.076], [-80.998, 34.077], [-80.996, 34.077], [-80.995, 34.077], [-80.996, 34.077], [-80.996, 34.078], [-80.993, 34.078], [-80.992, 34.078], [-80.992, 34.082], [-80.993, 34.082], [-80.993, 34.081], [-80.994, 34.081], [-80.995, 34.081], [-80.995, 34.08], [-80.996, 34.08], [-80.997, 34.08], [-80.996, 34.082], [-80.996, 34.083], [-80.996, 34.084], [-80.996, 34.085], [-80.996, 34.086], [-80.992, 34.085], [-80.99, 34.086], [-80.989, 34.086], [-80.988, 34.087], [-80.987, 34.088], [-80.99, 34.089], [-80.991, 34.09], [-80.99, 34.093], [-80.991, 34.094], [-80.993, 34.091], [-80.994, 34.092], [-80.994, 34.093], [-80.993, 34.094], [-80.993, 34.095], [-80.992, 34.097], [-80.992, 34.098], [-80.991, 34.099], [-80.987, 34.096], [-80.984, 34.099], [-80.982, 34.101], [-80.983, 34.101], [-80.981, 34.106], [-80.979, 34.107], [-80.98, 34.105], [-80.979, 34.104], [-80.977, 34.105], [-80.976, 34.106], [-80.974, 34.104], [-80.975, 34.1], [-80.973, 34.099], [-80.971, 34.099], [-80.971, 34.098], [-80.972, 34.097], [-80.972, 34.096], [-80.973, 34.097], [-80.973, 34.096], [-80.972, 34.095], [-80.97, 34.094], [-80.97, 34.093], [-80.969, 34.093], [-80.968, 34.094], [-80.967, 34.094], [-80.966, 34.094], [-80.965, 34.094], [-80.964, 34.095], [-80.963, 34.095], [-80.963, 34.094], [-80.963, 34.095], [-80.962, 34.095], [-80.961, 34.095], [-80.961, 34.094], [-80.961, 34.092], [-80.96, 34.091], [-80.96, 34.09], [-80.96, 34.089], [-80.961, 34.088], [-80.961, 34.087], [-80.962, 34.085], [-80.963, 34.084], [-80.964, 34.084], [-80.965, 34.083], [-80.966, 34.084], [-80.966, 34.085], [-80.967, 34.085], [-80.968, 34.085], [-80.97, 34.085], [-80.969, 34.085], [-80.969, 34.084], [-80.97, 34.084], [-80.971, 34.087], [-80.971, 34.088], [-80.972, 34.089], [-80.973, 34.088], [-80.974, 34.088], [-80.975, 34.087], [-80.975, 34.086], [-80.976, 34.084], [-80.976, 34.083], [-80.976, 34.082], [-80.977, 34.081], [-80.977, 34.08], [-80.978, 34.079], [-80.978, 34.078], [-80.978, 34.077], [-80.978, 34.076], [-80.979, 34.076], [-80.979, 34.075], [-80.98, 34.074], [-80.98, 34.073], [-80.984, 34.073], [-80.986, 34.073], [-80.985, 34.071], [-80.983, 34.072], [-80.984, 34.071], [-80.984, 34.07], [-80.983, 34.071], [-80.981, 34.069], [-80.981, 34.068], [-80.982, 34.068], [-80.982, 34.067], [-80.983, 34.067], [-80.984, 34.066], [-80.985, 34.065], [-80.987, 34.064], [-80.991, 34.061], [-80.992, 34.061], [-80.993, 34.06], [-80.995, 34.06], [-80.995, 34.059], [-80.996, 34.058], [-80.997, 34.059], [-80.999, 34.058], [-81, 34.057], [-81.001, 34.057], [-81.002, 34.056], [-81.002, 34.055], [-81.002, 34.056], [-81.001, 34.056], [-81, 34.056], [-80.999, 34.056], [-80.998, 34.056], [-80.997, 34.056], [-80.996, 34.056], [-80.995, 34.056], [-80.994, 34.056], [-80.993, 34.057], [-80.992, 34.057], [-80.992, 34.056], [-80.993, 34.056], [-80.992, 34.056], [-80.992, 34.055], [-80.992, 34.054], [-80.993, 34.054], [-80.992, 34.054], [-80.991, 34.053], [-80.992, 34.053], [-80.993, 34.053], [-80.994, 34.052], [-80.995, 34.052], [-80.995, 34.051], [-80.996, 34.051], [-80.997, 34.051], [-80.998, 34.05], [-81, 34.05], [-81.001, 34.05], [-80.999, 34.048], [-80.998, 34.047], [-80.997, 34.046], [-80.997, 34.047], [-80.997, 34.046], [-80.998, 34.046], [-80.996, 34.044], [-80.995, 34.044], [-80.995, 34.043], [-80.994, 34.043], [-80.992, 34.043], [-80.993, 34.043], [-80.994, 34.042], [-80.995, 34.042], [-80.996, 34.042], [-80.996, 34.041], [-80.997, 34.041], [-80.998, 34.041], [-80.998, 34.04], [-80.999, 34.04], [-81, 34.04], [-80.999, 34.039], [-80.999, 34.04], [-80.998, 34.04], [-80.998, 34.039], [-80.997, 34.039], [-80.996, 34.039], [-80.995, 34.039], [-80.995, 34.038], [-80.994, 34.038], [-80.993, 34.038], [-80.994, 34.039], [-80.993, 34.039], [-80.994, 34.039], [-80.993, 34.039], [-80.993, 34.04], [-80.993, 34.041], [-80.992, 34.041], [-80.991, 34.04], [-80.99, 34.04], [-80.99, 34.039], [-80.989, 34.039], [-80.988, 34.039], [-80.989, 34.039], [-80.989, 34.038], [-80.99, 34.038], [-80.991, 34.039], [-80.992, 34.037], [-80.991, 34.037], [-80.99, 34.037], [-80.989, 34.037], [-80.989, 34.036], [-80.988, 34.036], [-80.988, 34.037], [-80.987, 34.037], [-80.986, 34.037], [-80.986, 34.036], [-80.985, 34.036], [-80.984, 34.037], [-80.983, 34.037], [-80.983, 34.038], [-80.983, 34.037], [-80.982, 34.038], [-80.981, 34.038], [-80.981, 34.037], [-80.98, 34.037], [-80.98, 34.036], [-80.981, 34.036], [-80.982, 34.036], [-80.983, 34.036], [-80.982, 34.035], [-80.983, 34.033], [-80.982, 34.033], [-80.983, 34.032], [-80.982, 34.032], [-80.982, 34.031], [-80.982, 34.032], [-80.983, 34.032], [-80.984, 34.032], [-80.984, 34.033], [-80.985, 34.033], [-80.986, 34.033], [-80.986, 34.03], [-80.985, 34.03], [-80.985, 34.029], [-80.985, 34.028], [-80.986, 34.028], [-80.987, 34.028], [-80.987, 34.027], [-80.988, 34.027], [-80.989, 34.026], [-80.99, 34.026], [-80.991, 34.026], [-80.992, 34.026], [-80.993, 34.025], [-80.994, 34.025], [-80.995, 34.025], [-80.995, 34.024], [-80.995, 34.023], [-80.995, 34.021], [-80.995, 34.02], [-80.995, 34.019], [-80.995, 34.018], [-80.995, 34.017], [-80.995, 34.016], [-80.995, 34.015], [-80.995, 34.014], [-80.995, 34.013], [-80.995, 34.012], [-80.995, 34.01], [-80.994, 34.01], [-80.993, 34.011], [-80.992, 34.011], [-80.991, 34.012], [-80.99, 34.012], [-80.989, 34.012], [-80.988, 34.012], [-80.988, 34.013], [-80.987, 34.013], [-80.986, 34.013], [-80.985, 34.013], [-80.984, 34.013], [-80.983, 34.013], [-80.982, 34.013], [-80.981, 34.014], [-80.98, 34.014], [-80.979, 34.014], [-80.978, 34.014], [-80.977, 34.014], [-80.976, 34.014], [-80.975, 34.014], [-80.975, 34.013], [-80.974, 34.013], [-80.973, 34.013], [-80.972, 34.012], [-80.971, 34.012], [-80.97, 34.012], [-80.969, 34.011], [-80.968, 34.011], [-80.967, 34.011], [-80.966, 34.011], [-80.965, 34.011], [-80.964, 34.011], [-80.963, 34.011], [-80.963, 34.012], [-80.962, 34.012], [-80.961, 34.012], [-80.961, 34.013], [-80.96, 34.013], [-80.96, 34.015], [-80.96, 34.016], [-80.96, 34.018], [-80.96, 34.019], [-80.96, 34.02], [-80.96, 34.021], [-80.961, 34.021], [-80.96, 34.021], [-80.959, 34.022], [-80.958, 34.022], [-80.958, 34.023], [-80.957, 34.023], [-80.956, 34.023], [-80.955, 34.023], [-80.954, 34.023], [-80.953, 34.024], [-80.952, 34.024], [-80.952, 34.025], [-80.951, 34.025], [-80.951, 34.024], [-80.95, 34.024], [-80.949, 34.024], [-80.948, 34.024], [-80.946, 34.024], [-80.945, 34.025], [-80.944, 34.026], [-80.945, 34.026], [-80.946, 34.027], [-80.947, 34.027], [-80.947, 34.028], [-80.946, 34.029], [-80.945, 34.029], [-80.944, 34.03], [-80.943, 34.03], [-80.944, 34.03], [-80.944, 34.031], [-80.944, 34.03], [-80.943, 34.03], [-80.943, 34.031], [-80.943, 34.032], [-80.942, 34.032], [-80.941, 34.031], [-80.941, 34.032], [-80.94, 34.032], [-80.94, 34.033], [-80.939, 34.034], [-80.939, 34.035], [-80.939, 34.036], [-80.939, 34.037], [-80.94, 34.037], [-80.94, 34.036], [-80.94, 34.037], [-80.941, 34.037], [-80.941, 34.038], [-80.94, 34.038], [-80.939, 34.038], [-80.939, 34.039], [-80.939, 34.04], [-80.938, 34.041], [-80.938, 34.042], [-80.937, 34.042], [-80.937, 34.043], [-80.936, 34.043], [-80.936, 34.044], [-80.935, 34.044], [-80.934, 34.044], [-80.934, 34.045], [-80.933, 34.045], [-80.931, 34.047], [-80.93, 34.047], [-80.93, 34.048], [-80.929, 34.048], [-80.928, 34.049], [-80.926, 34.05], [-80.925, 34.051], [-80.924, 34.051], [-80.924, 34.052], [-80.923, 34.052], [-80.922, 34.053], [-80.921, 34.053], [-80.921, 34.054], [-80.92, 34.054], [-80.919, 34.054], [-80.919, 34.055], [-80.92, 34.055], [-80.92, 34.056], [-80.919, 34.057], [-80.919, 34.056], [-80.918, 34.057], [-80.918, 34.056], [-80.917, 34.056], [-80.915, 34.058], [-80.914, 34.058], [-80.913, 34.059], [-80.912, 34.059], [-80.911, 34.06], [-80.91, 34.061], [-80.909, 34.062], [-80.908, 34.062], [-80.909, 34.062], [-80.908, 34.062], [-80.907, 34.063], [-80.906, 34.063], [-80.906, 34.064], [-80.907, 34.064], [-80.906, 34.064], [-80.905, 34.064], [-80.904, 34.065], [-80.903, 34.065], [-80.903, 34.066], [-80.904, 34.066], [-80.904, 34.067], [-80.905, 34.067], [-80.907, 34.067], [-80.906, 34.068], [-80.903, 34.07], [-80.902, 34.07], [-80.901, 34.07], [-80.902, 34.07], [-80.902, 34.069], [-80.903, 34.068], [-80.902, 34.068], [-80.901, 34.067], [-80.9, 34.067], [-80.9, 34.068], [-80.899, 34.068], [-80.898, 34.069], [-80.897, 34.07], [-80.896, 34.07], [-80.895, 34.071], [-80.894, 34.072], [-80.893, 34.072], [-80.892, 34.073], [-80.891, 34.074], [-80.89, 34.074], [-80.889, 34.075], [-80.888, 34.076], [-80.887, 34.076], [-80.886, 34.077], [-80.885, 34.077], [-80.885, 34.078], [-80.884, 34.078], [-80.883, 34.079], [-80.882, 34.079], [-80.881, 34.08], [-80.882, 34.08], [-80.882, 34.081], [-80.883, 34.081], [-80.883, 34.08], [-80.882, 34.08], [-80.883, 34.08], [-80.884, 34.079], [-80.885, 34.079], [-80.886, 34.079], [-80.886, 34.078], [-80.887, 34.078], [-80.887, 34.077], [-80.888, 34.077], [-80.888, 34.076], [-80.889, 34.076], [-80.89, 34.076], [-80.891, 34.075], [-80.891, 34.076], [-80.891, 34.077], [-80.891, 34.078], [-80.892, 34.078], [-80.892, 34.079], [-80.892, 34.08], [-80.885, 34.083], [-80.882, 34.084], [-80.879, 34.085], [-80.876, 34.082], [-80.875, 34.082], [-80.874, 34.082], [-80.874, 34.083], [-80.875, 34.083], [-80.876, 34.084], [-80.877, 34.086], [-80.876, 34.087], [-80.876, 34.088], [-80.877, 34.088], [-80.876, 34.088], [-80.876, 34.089], [-80.879, 34.089], [-80.88, 34.091], [-80.881, 34.092], [-80.881, 34.093], [-80.88, 34.093], [-80.879, 34.094], [-80.878, 34.094], [-80.878, 34.095], [-80.877, 34.095], [-80.876, 34.096], [-80.875, 34.096], [-80.875, 34.097], [-80.874, 34.097], [-80.873, 34.098], [-80.872, 34.098], [-80.872, 34.099], [-80.871, 34.099], [-80.87, 34.099], [-80.869, 34.1], [-80.868, 34.099], [-80.868, 34.098], [-80.868, 34.097], [-80.869, 34.097], [-80.871, 34.096], [-80.872, 34.096], [-80.869, 34.094], [-80.868, 34.094], [-80.868, 34.093], [-80.867, 34.093], [-80.867, 34.092], [-80.866, 34.092], [-80.864, 34.094], [-80.863, 34.094], [-80.861, 34.094], [-80.86, 34.095], [-80.859, 34.094], [-80.859, 34.095], [-80.858, 34.094], [-80.858, 34.093], [-80.857, 34.092], [-80.856, 34.092], [-80.855, 34.092], [-80.856, 34.092], [-80.856, 34.093], [-80.856, 34.094], [-80.857, 34.095], [-80.857, 34.096], [-80.857, 34.097], [-80.856, 34.097], [-80.856, 34.096], [-80.856, 34.095], [-80.854, 34.096], [-80.855, 34.096], [-80.855, 34.097], [-80.854, 34.097], [-80.853, 34.097], [-80.852, 34.097], [-80.852, 34.098], [-80.851, 34.098], [-80.85, 34.099], [-80.85, 34.1], [-80.849, 34.1], [-80.848, 34.1], [-80.847, 34.1], [-80.847, 34.101], [-80.847, 34.102], [-80.844, 34.101], [-80.841, 34.101], [-80.846, 34.099], [-80.849, 34.098], [-80.85, 34.097], [-80.851, 34.097], [-80.851, 34.096], [-80.85, 34.094], [-80.851, 34.095], [-80.852, 34.094], [-80.851, 34.094], [-80.85, 34.094], [-80.849, 34.093], [-80.849, 34.092], [-80.846, 34.094], [-80.844, 34.094], [-80.833, 34.099], [-80.826, 34.102], [-80.823, 34.103], [-80.822, 34.103], [-80.821, 34.104], [-80.821, 34.105], [-80.82, 34.106], [-80.82, 34.107], [-80.821, 34.107], [-80.824, 34.106], [-80.824, 34.107], [-80.826, 34.106], [-80.827, 34.106], [-80.828, 34.106], [-80.829, 34.106], [-80.83, 34.106], [-80.831, 34.106], [-80.831, 34.107], [-80.832, 34.107], [-80.832, 34.106], [-80.833, 34.107], [-80.834, 34.107], [-80.835, 34.107], [-80.835, 34.106], [-80.836, 34.106], [-80.837, 34.105], [-80.837, 34.106], [-80.837, 34.107], [-80.837, 34.108], [-80.836, 34.108], [-80.835, 34.108], [-80.836, 34.109], [-80.837, 34.109], [-80.836, 34.11], [-80.837, 34.11], [-80.837, 34.111], [-80.838, 34.111], [-80.839, 34.112], [-80.841, 34.11], [-80.843, 34.109], [-80.844, 34.108], [-80.845, 34.108], [-80.846, 34.108], [-80.847, 34.108], [-80.847, 34.109], [-80.847, 34.11], [-80.846, 34.111], [-80.847, 34.112], [-80.847, 34.113], [-80.847, 34.114], [-80.847, 34.115], [-80.846, 34.115], [-80.845, 34.115], [-80.845, 34.116], [-80.846, 34.116], [-80.846, 34.117], [-80.846, 34.118], [-80.847, 34.118], [-80.847, 34.119], [-80.847, 34.12], [-80.848, 34.12], [-80.847, 34.12], [-80.846, 34.12], [-80.844, 34.12], [-80.844, 34.119], [-80.843, 34.119], [-80.842, 34.119], [-80.841, 34.119], [-80.84, 34.119], [-80.839, 34.119], [-80.838, 34.119], [-80.838, 34.12], [-80.837, 34.12], [-80.837, 34.121], [-80.836, 34.121], [-80.837, 34.121], [-80.836, 34.121], [-80.835, 34.121], [-80.835, 34.122], [-80.834, 34.122], [-80.835, 34.122], [-80.835, 34.123], [-80.836, 34.123], [-80.835, 34.123], [-80.836, 34.123], [-80.835, 34.123], [-80.834, 34.123], [-80.833, 34.123], [-80.833, 34.122], [-80.834, 34.122], [-80.834, 34.121], [-80.833, 34.121], [-80.832, 34.122], [-80.832, 34.123], [-80.832, 34.124], [-80.833, 34.124], [-80.833, 34.125], [-80.834, 34.125], [-80.834, 34.124], [-80.835, 34.124], [-80.836, 34.124], [-80.836, 34.125], [-80.835, 34.125], [-80.835, 34.126], [-80.834, 34.127], [-80.833, 34.128], [-80.832, 34.129], [-80.831, 34.129], [-80.83, 34.129], [-80.829, 34.13], [-80.829, 34.129], [-80.828, 34.129], [-80.829, 34.128], [-80.828, 34.128], [-80.828, 34.127], [-80.827, 34.126], [-80.827, 34.125], [-80.826, 34.125], [-80.825, 34.125], [-80.826, 34.125], [-80.826, 34.124], [-80.827, 34.124], [-80.828, 34.125], [-80.829, 34.125], [-80.829, 34.126], [-80.83, 34.126], [-80.831, 34.125], [-80.832, 34.124], [-80.832, 34.123], [-80.832, 34.122], [-80.831, 34.122], [-80.831, 34.121], [-80.83, 34.121], [-80.831, 34.121], [-80.831, 34.12], [-80.83, 34.121], [-80.829, 34.12], [-80.829, 34.119], [-80.828, 34.119], [-80.828, 34.118], [-80.828, 34.117], [-80.829, 34.117], [-80.828, 34.117], [-80.828, 34.116], [-80.827, 34.116], [-80.827, 34.115], [-80.826, 34.115], [-80.825, 34.115], [-80.824, 34.115], [-80.823, 34.114], [-80.822, 34.114], [-80.822, 34.113], [-80.821, 34.113], [-80.82, 34.113], [-80.819, 34.112], [-80.818, 34.112], [-80.817, 34.112], [-80.818, 34.112], [-80.818, 34.113], [-80.819, 34.113], [-80.819, 34.112], [-80.819, 34.113], [-80.82, 34.113], [-80.821, 34.114], [-80.82, 34.114], [-80.82, 34.115], [-80.821, 34.116], [-80.822, 34.116], [-80.823, 34.117], [-80.824, 34.117], [-80.825, 34.117], [-80.825, 34.118], [-80.826, 34.118], [-80.827, 34.118], [-80.827, 34.119], [-80.828, 34.119], [-80.828, 34.12], [-80.83, 34.122], [-80.829, 34.122], [-80.828, 34.122], [-80.827, 34.122], [-80.826, 34.122], [-80.825, 34.121], [-80.824, 34.121], [-80.823, 34.121], [-80.821, 34.121], [-80.819, 34.121], [-80.818, 34.121], [-80.817, 34.121], [-80.817, 34.122], [-80.817, 34.123], [-80.818, 34.123], [-80.819, 34.123], [-80.82, 34.123], [-80.821, 34.123], [-80.821, 34.124], [-80.821, 34.125], [-80.822, 34.125], [-80.822, 34.124], [-80.822, 34.125], [-80.823, 34.125], [-80.823, 34.126], [-80.822, 34.127], [-80.822, 34.128], [-80.822, 34.129], [-80.821, 34.129], [-80.82, 34.13], [-80.819, 34.131], [-80.82, 34.131], [-80.819, 34.131], [-80.818, 34.131], [-80.818, 34.13], [-80.817, 34.13], [-80.817, 34.129], [-80.816, 34.129], [-80.816, 34.128], [-80.817, 34.128], [-80.817, 34.129], [-80.817, 34.128], [-80.818, 34.129], [-80.818, 34.128], [-80.819, 34.128], [-80.82, 34.128], [-80.819, 34.128], [-80.819, 34.127], [-80.82, 34.127], [-80.82, 34.128], [-80.82, 34.127], [-80.821, 34.127], [-80.82, 34.127], [-80.821, 34.127], [-80.821, 34.126], [-80.82, 34.126], [-80.82, 34.125], [-80.819, 34.125], [-80.819, 34.124], [-80.818, 34.124], [-80.818, 34.123], [-80.817, 34.123], [-80.817, 34.122], [-80.817, 34.121], [-80.816, 34.121], [-80.816, 34.122], [-80.815, 34.122], [-80.814, 34.122], [-80.814, 34.121], [-80.815, 34.121], [-80.815, 34.12], [-80.816, 34.12], [-80.816, 34.119], [-80.815, 34.119], [-80.816, 34.119], [-80.815, 34.118], [-80.815, 34.117], [-80.815, 34.116], [-80.815, 34.115], [-80.815, 34.114], [-80.816, 34.114], [-80.817, 34.114], [-80.817, 34.113], [-80.817, 34.112], [-80.817, 34.111], [-80.816, 34.11], [-80.816, 34.109], [-80.815, 34.106], [-80.82, 34.104], [-80.819, 34.104], [-80.818, 34.104], [-80.818, 34.105], [-80.817, 34.105], [-80.816, 34.105], [-80.81, 34.104], [-80.809, 34.104], [-80.808, 34.104], [-80.807, 34.104], [-80.806, 34.104], [-80.805, 34.104], [-80.804, 34.103], [-80.803, 34.103], [-80.802, 34.103], [-80.801, 34.103], [-80.797, 34.103], [-80.795, 34.102], [-80.787, 34.102], [-80.785, 34.102], [-80.784, 34.102], [-80.783, 34.102], [-80.78, 34.102], [-80.778, 34.102], [-80.777, 34.102], [-80.776, 34.102], [-80.775, 34.102], [-80.774, 34.102], [-80.772, 34.101], [-80.771, 34.101], [-80.771, 34.1], [-80.77, 34.1], [-80.769, 34.1], [-80.768, 34.099], [-80.767, 34.098], [-80.767, 34.097], [-80.766, 34.097], [-80.766, 34.096], [-80.766, 34.095], [-80.765, 34.095], [-80.765, 34.094], [-80.763, 34.092], [-80.763, 34.091], [-80.762, 34.09], [-80.762, 34.089], [-80.761, 34.089], [-80.759, 34.087], [-80.758, 34.086], [-80.757, 34.085], [-80.756, 34.085], [-80.756, 34.084], [-80.755, 34.084], [-80.754, 34.084], [-80.753, 34.083], [-80.752, 34.083], [-80.752, 34.084], [-80.751, 34.084], [-80.75, 34.084], [-80.749, 34.084], [-80.748, 34.084], [-80.748, 34.083], [-80.747, 34.083], [-80.746, 34.083], [-80.744, 34.082], [-80.743, 34.082], [-80.742, 34.082], [-80.742, 34.081], [-80.741, 34.081], [-80.74, 34.081], [-80.74, 34.08], [-80.737, 34.079], [-80.736, 34.078], [-80.735, 34.077], [-80.734, 34.076], [-80.733, 34.075], [-80.732, 34.074], [-80.732, 34.073], [-80.731, 34.073], [-80.73, 34.072], [-80.728, 34.07], [-80.727, 34.07], [-80.727, 34.069], [-80.726, 34.069], [-80.725, 34.068], [-80.724, 34.068], [-80.724, 34.067], [-80.723, 34.067], [-80.722, 34.067], [-80.721, 34.067], [-80.721, 34.066], [-80.72, 34.066], [-80.719, 34.066], [-80.719, 34.065], [-80.718, 34.065], [-80.717, 34.065], [-80.714, 34.064], [-80.713, 34.064], [-80.713, 34.063], [-80.712, 34.063], [-80.711, 34.063], [-80.71, 34.063], [-80.709, 34.063], [-80.708, 34.063], [-80.707, 34.063], [-80.707, 34.062], [-80.706, 34.062], [-80.705, 34.062], [-80.704, 34.061], [-80.704, 34.06], [-80.704, 34.057], [-80.704, 34.05], [-80.704, 34.049], [-80.704, 34.047], [-80.704, 34.045], [-80.704, 34.043], [-80.704, 34.042], [-80.704, 34.04], [-80.704, 34.039], [-80.704, 34.038], [-80.704, 34.037], [-80.704, 34.036], [-80.704, 34.034], [-80.704, 34.033], [-80.704, 34.032], [-80.704, 34.031], [-80.704, 34.03], [-80.704, 34.029], [-80.704, 34.027], [-80.704, 34.026], [-80.704, 34.025], [-80.704, 34.024], [-80.704, 34.023], [-80.704, 34.022], [-80.704, 34.021], [-80.704, 34.02], [-80.704, 34.019], [-80.704, 34.016], [-80.704, 34.014], [-80.704, 34.01], [-80.705, 34.01], [-80.705, 34.009], [-80.706, 34.009], [-80.706, 34.008], [-80.707, 34.008], [-80.709, 34.008], [-80.714, 34.008], [-80.715, 34.008], [-80.717, 34.008], [-80.718, 34.008], [-80.719, 34.008], [-80.72, 34.008], [-80.724, 34.007], [-80.726, 34.007], [-80.727, 34.007], [-80.731, 34.007], [-80.733, 34.007], [-80.734, 34.007], [-80.736, 34.007], [-80.742, 34.007], [-80.744, 34.007], [-80.748, 34.006], [-80.751, 34.006], [-80.759, 34.006], [-80.764, 34.006], [-80.766, 34.006], [-80.767, 34.005], [-80.768, 34.005], [-80.769, 34.005], [-80.771, 34.005], [-80.773, 34.005], [-80.776, 34.004], [-80.78, 34.004], [-80.782, 34.004], [-80.783, 34.003], [-80.789, 34.003], [-80.791, 34.002], [-80.792, 34.002], [-80.795, 34.002], [-80.803, 34.001], [-80.805, 34], [-80.806, 34], [-80.807, 34], [-80.811, 34], [-80.813, 33.999], [-80.814, 33.999], [-80.815, 33.999], [-80.816, 33.999], [-80.817, 33.999], [-80.818, 33.999], [-80.819, 33.999], [-80.819, 33.998], [-80.822, 33.998], [-80.823, 33.998], [-80.825, 33.998], [-80.826, 33.998], [-80.826, 33.997], [-80.828, 33.997], [-80.831, 33.997], [-80.833, 33.997], [-80.833, 33.996], [-80.834, 33.996], [-80.835, 33.996], [-80.836, 33.996], [-80.837, 33.996], [-80.84, 33.996], [-80.841, 33.995], [-80.842, 33.995], [-80.843, 33.995], [-80.847, 33.995], [-80.848, 33.994], [-80.85, 33.994], [-80.852, 33.994], [-80.853, 33.994], [-80.855, 33.994], [-80.855, 33.993], [-80.856, 33.993], [-80.857, 33.993], [-80.861, 33.993], [-80.863, 33.992], [-80.864, 33.992], [-80.866, 33.992], [-80.867, 33.992], [-80.868, 33.992], [-80.869, 33.991], [-80.87, 33.991], [-80.871, 33.991], [-80.872, 33.991], [-80.873, 33.99], [-80.874, 33.99], [-80.878, 33.989], [-80.88, 33.989], [-80.88, 33.988], [-80.881, 33.988], [-80.881, 33.987], [-80.882, 33.986], [-80.881, 33.985], [-80.88, 33.985], [-80.88, 33.984], [-80.88, 33.983], [-80.877, 33.983], [-80.877, 33.982], [-80.876, 33.982], [-80.875, 33.982], [-80.873, 33.98], [-80.872, 33.982], [-80.871, 33.982], [-80.871, 33.979], [-80.872, 33.979], [-80.873, 33.979], [-80.874, 33.979], [-80.874, 33.98], [-80.875, 33.98], [-80.876, 33.98], [-80.876, 33.979], [-80.877, 33.979], [-80.877, 33.98], [-80.877, 33.981], [-80.878, 33.981], [-80.879, 33.981], [-80.879, 33.98], [-80.879, 33.979], [-80.879, 33.978], [-80.88, 33.978], [-80.881, 33.978], [-80.882, 33.979], [-80.883, 33.979], [-80.884, 33.979], [-80.885, 33.98], [-80.884, 33.982], [-80.884, 33.983], [-80.884, 33.985], [-80.885, 33.985], [-80.886, 33.985], [-80.886, 33.986], [-80.884, 33.986], [-80.883, 33.987], [-80.882, 33.987], [-80.883, 33.988], [-80.884, 33.987], [-80.888, 33.986], [-80.889, 33.986], [-80.89, 33.986], [-80.89, 33.985], [-80.89, 33.984], [-80.891, 33.984], [-80.892, 33.984], [-80.893, 33.984], [-80.893, 33.985], [-80.894, 33.985], [-80.895, 33.985], [-80.895, 33.984], [-80.894, 33.984], [-80.895, 33.983], [-80.896, 33.983], [-80.897, 33.983], [-80.896, 33.983], [-80.896, 33.984], [-80.897, 33.984], [-80.898, 33.984], [-80.899, 33.984], [-80.899, 33.983], [-80.899, 33.982], [-80.897, 33.982], [-80.898, 33.981], [-80.898, 33.979], [-80.896, 33.979], [-80.896, 33.978], [-80.897, 33.979], [-80.9, 33.979], [-80.901, 33.978], [-80.902, 33.977], [-80.9, 33.975], [-80.9, 33.974], [-80.9, 33.973], [-80.9, 33.972], [-80.9, 33.973], [-80.901, 33.973], [-80.902, 33.973], [-80.903, 33.973], [-80.904, 33.973], [-80.904, 33.974], [-80.904, 33.975], [-80.904, 33.976], [-80.904, 33.977], [-80.904, 33.978], [-80.904, 33.979], [-80.905, 33.979], [-80.904, 33.979], [-80.904, 33.98], [-80.905, 33.98], [-80.904, 33.981], [-80.903, 33.981], [-80.903, 33.982], [-80.904, 33.982], [-80.905, 33.982], [-80.906, 33.982], [-80.905, 33.982], [-80.906, 33.982], [-80.907, 33.982], [-80.908, 33.982], [-80.909, 33.982], [-80.91, 33.981], [-80.911, 33.981], [-80.912, 33.981], [-80.913, 33.981], [-80.913, 33.98], [-80.914, 33.98], [-80.915, 33.98], [-80.914, 33.98], [-80.915, 33.98], [-80.914, 33.98], [-80.915, 33.98], [-80.915, 33.979], [-80.915, 33.978], [-80.915, 33.977], [-80.916, 33.976], [-80.915, 33.977], [-80.915, 33.976], [-80.916, 33.976], [-80.916, 33.975], [-80.917, 33.975], [-80.917, 33.974], [-80.917, 33.973], [-80.918, 33.974], [-80.92, 33.974], [-80.92, 33.973], [-80.92, 33.972], [-80.921, 33.972], [-80.92, 33.972], [-80.919, 33.972], [-80.919, 33.971], [-80.918, 33.971], [-80.919, 33.97], [-80.919, 33.969], [-80.919, 33.968], [-80.918, 33.968], [-80.918, 33.967], [-80.917, 33.967], [-80.918, 33.965], [-80.92, 33.965], [-80.922, 33.966], [-80.922, 33.965], [-80.922, 33.964], [-80.923, 33.964], [-80.923, 33.965], [-80.923, 33.964], [-80.924, 33.964], [-80.924, 33.965], [-80.924, 33.966], [-80.923, 33.966], [-80.922, 33.968], [-80.922, 33.969], [-80.922, 33.968], [-80.921, 33.969], [-80.921, 33.968], [-80.922, 33.972], [-80.923, 33.971], [-80.923, 33.97], [-80.923, 33.971], [-80.924, 33.97], [-80.924, 33.971], [-80.925, 33.971], [-80.925, 33.972], [-80.926, 33.973], [-80.926, 33.972], [-80.926, 33.973], [-80.926, 33.974], [-80.926, 33.973], [-80.927, 33.973], [-80.926, 33.974], [-80.927, 33.974], [-80.927, 33.975], [-80.928, 33.975], [-80.927, 33.975], [-80.928, 33.976], [-80.927, 33.976], [-80.925, 33.977], [-80.925, 33.978], [-80.925, 33.979], [-80.926, 33.979], [-80.927, 33.979], [-80.928, 33.978], [-80.929, 33.979], [-80.929, 33.978], [-80.93, 33.978], [-80.93, 33.979], [-80.931, 33.979], [-80.93, 33.979], [-80.93, 33.98], [-80.931, 33.98], [-80.931, 33.981], [-80.934, 33.98], [-80.935, 33.979], [-80.938, 33.978], [-80.937, 33.977], [-80.936, 33.977], [-80.937, 33.977], [-80.936, 33.978], [-80.936, 33.977], [-80.937, 33.976], [-80.936, 33.976], [-80.937, 33.975], [-80.938, 33.975], [-80.939, 33.975], [-80.939, 33.974], [-80.939, 33.973], [-80.938, 33.973], [-80.94, 33.971], [-80.941, 33.971], [-80.942, 33.97], [-80.942, 33.969], [-80.941, 33.969], [-80.94, 33.968], [-80.939, 33.97], [-80.938, 33.971], [-80.937, 33.97], [-80.937, 33.971], [-80.937, 33.972], [-80.936, 33.972], [-80.936, 33.973], [-80.935, 33.973], [-80.935, 33.972], [-80.934, 33.971], [-80.934, 33.97], [-80.933, 33.97], [-80.933, 33.969], [-80.932, 33.97], [-80.932, 33.969], [-80.931, 33.969], [-80.931, 33.968], [-80.93, 33.968], [-80.929, 33.968], [-80.928, 33.967], [-80.926, 33.967], [-80.926, 33.966], [-80.927, 33.965], [-80.927, 33.964], [-80.927, 33.963], [-80.928, 33.963], [-80.928, 33.964], [-80.929, 33.964], [-80.929, 33.963], [-80.93, 33.962], [-80.928, 33.962], [-80.93, 33.962], [-80.93, 33.961], [-80.929, 33.96], [-80.929, 33.959], [-80.93, 33.958], [-80.931, 33.958], [-80.932, 33.958], [-80.932, 33.959], [-80.933, 33.959], [-80.934, 33.959], [-80.934, 33.96], [-80.934, 33.961], [-80.935, 33.962], [-80.937, 33.961], [-80.938, 33.959], [-80.939, 33.959], [-80.94, 33.96], [-80.941, 33.959], [-80.941, 33.96], [-80.942, 33.959], [-80.943, 33.959], [-80.943, 33.958], [-80.944, 33.958], [-80.945, 33.958], [-80.945, 33.957], [-80.944, 33.957], [-80.944, 33.956], [-80.943, 33.956], [-80.943, 33.955], [-80.943, 33.954], [-80.942, 33.954], [-80.941, 33.954], [-80.94, 33.954], [-80.939, 33.954], [-80.939, 33.955], [-80.939, 33.956], [-80.938, 33.956], [-80.937, 33.958], [-80.936, 33.958], [-80.937, 33.957], [-80.936, 33.958], [-80.935, 33.958], [-80.935, 33.957], [-80.936, 33.957], [-80.936, 33.956], [-80.937, 33.957], [-80.938, 33.956], [-80.936, 33.956], [-80.936, 33.955], [-80.935, 33.956], [-80.934, 33.957], [-80.933, 33.957], [-80.933, 33.958], [-80.932, 33.958], [-80.932, 33.957], [-80.931, 33.957], [-80.931, 33.958], [-80.93, 33.957], [-80.93, 33.956], [-80.929, 33.956], [-80.928, 33.956], [-80.927, 33.956], [-80.927, 33.955], [-80.928, 33.954], [-80.928, 33.953], [-80.928, 33.952], [-80.931, 33.95], [-80.93, 33.949], [-80.931, 33.95], [-80.932, 33.949], [-80.933, 33.948], [-80.934, 33.948], [-80.935, 33.948], [-80.938, 33.949], [-80.939, 33.949], [-80.941, 33.95], [-80.944, 33.951], [-80.945, 33.951], [-80.947, 33.952], [-80.948, 33.952], [-80.948, 33.951], [-80.948, 33.95], [-80.949, 33.95], [-80.95, 33.95], [-80.95, 33.951], [-80.951, 33.951], [-80.951, 33.95], [-80.951, 33.951], [-80.951, 33.95], [-80.952, 33.95], [-80.952, 33.949], [-80.953, 33.948], [-80.953, 33.947], [-80.954, 33.948], [-80.957, 33.949], [-80.958, 33.949], [-80.958, 33.95], [-80.959, 33.95], [-80.96, 33.951], [-80.961, 33.951], [-80.962, 33.952], [-80.963, 33.952], [-80.964, 33.952], [-80.966, 33.951], [-80.967, 33.95], [-80.967, 33.951], [-80.967, 33.95], [-80.968, 33.95], [-80.968, 33.949], [-80.969, 33.949], [-80.97, 33.948], [-80.971, 33.949], [-80.972, 33.948], [-80.973, 33.948], [-80.975, 33.947], [-80.974, 33.946], [-80.975, 33.946], [-80.974, 33.945], [-80.974, 33.944], [-80.973, 33.944], [-80.972, 33.943], [-80.973, 33.943], [-80.972, 33.943], [-80.972, 33.942], [-80.97, 33.943], [-80.969, 33.944], [-80.968, 33.944], [-80.966, 33.942], [-80.965, 33.941], [-80.964, 33.942], [-80.963, 33.942], [-80.963, 33.943], [-80.962, 33.943], [-80.962, 33.942], [-80.96, 33.943], [-80.959, 33.942], [-80.96, 33.939], [-80.96, 33.938], [-80.96, 33.937], [-80.961, 33.936], [-80.961, 33.935], [-80.961, 33.936], [-80.962, 33.936], [-80.963, 33.937], [-80.963, 33.938], [-80.964, 33.938], [-80.964, 33.939], [-80.965, 33.939], [-80.966, 33.939], [-80.966, 33.94], [-80.966, 33.939], [-80.967, 33.939], [-80.968, 33.939], [-80.969, 33.939], [-80.97, 33.939], [-80.97, 33.94], [-80.971, 33.94], [-80.972, 33.94], [-80.974, 33.941], [-80.974, 33.94], [-80.975, 33.94], [-80.976, 33.939], [-80.977, 33.939], [-80.978, 33.939], [-80.978, 33.938], [-80.979, 33.938], [-80.979, 33.939], [-80.981, 33.94], [-80.981, 33.939], [-80.982, 33.939], [-80.983, 33.94], [-80.984, 33.941], [-80.985, 33.942], [-80.983, 33.943], [-80.983, 33.944], [-80.981, 33.945], [-80.982, 33.946], [-80.983, 33.948], [-80.983, 33.947], [-80.985, 33.947], [-80.985, 33.946], [-80.986, 33.946], [-80.987, 33.947], [-80.988, 33.947], [-80.989, 33.947], [-80.989, 33.948], [-80.989, 33.949], [-80.99, 33.949], [-80.99, 33.95], [-80.991, 33.951], [-80.991, 33.952], [-80.992, 33.953], [-80.991, 33.953], [-80.991, 33.954], [-80.99, 33.954], [-80.989, 33.955], [-80.988, 33.955], [-80.988, 33.956], [-80.986, 33.955], [-80.986, 33.956], [-80.985, 33.956], [-80.984, 33.957], [-80.983, 33.957], [-80.982, 33.957], [-80.981, 33.956], [-80.98, 33.956], [-80.978, 33.956], [-80.978, 33.957], [-80.979, 33.957], [-80.978, 33.957], [-80.978, 33.958], [-80.98, 33.959], [-80.979, 33.96], [-80.98, 33.961], [-80.979, 33.961], [-80.98, 33.961], [-80.98, 33.962], [-80.98, 33.963], [-80.981, 33.963], [-80.982, 33.963], [-80.981, 33.964], [-80.981, 33.965], [-80.982, 33.965], [-80.982, 33.966], [-80.983, 33.966], [-80.983, 33.967], [-80.984, 33.968], [-80.983, 33.969], [-80.983, 33.97], [-80.982, 33.97], [-80.981, 33.97], [-80.981, 33.969], [-80.981, 33.97], [-80.981, 33.971], [-80.981, 33.972], [-80.98, 33.973], [-80.98, 33.974], [-80.981, 33.974], [-80.981, 33.973], [-80.982, 33.973], [-80.982, 33.972], [-80.982, 33.971], [-80.983, 33.971], [-80.984, 33.971], [-80.985, 33.971], [-80.986, 33.971], [-80.986, 33.972], [-80.987, 33.972], [-80.988, 33.97], [-80.987, 33.97], [-80.986, 33.97], [-80.986, 33.969], [-80.987, 33.97], [-80.988, 33.97], [-80.988, 33.969], [-80.988, 33.968], [-80.987, 33.968], [-80.986, 33.968], [-80.986, 33.967], [-80.985, 33.967], [-80.986, 33.966], [-80.988, 33.967], [-80.989, 33.968], [-80.99, 33.968], [-80.991, 33.968], [-80.992, 33.968], [-80.993, 33.968], [-80.994, 33.969], [-80.997, 33.97], [-81, 33.972], [-81.003, 33.973], [-81.008, 33.976], [-81.009, 33.976], [-81.012, 33.978], [-81.013, 33.978], [-81.013, 33.979], [-81.014, 33.979], [-81.015, 33.979], [-81.015, 33.98], [-81.016, 33.98], [-81.017, 33.98], [-81.02, 33.982], [-81.021, 33.982], [-81.022, 33.983], [-81.023, 33.983], [-81.023, 33.982], [-81.022, 33.982], [-81.022, 33.981], [-81.021, 33.98], [-81.02, 33.98], [-81.019, 33.979], [-81.017, 33.979], [-81.017, 33.978], [-81.017, 33.979], [-81.016, 33.979], [-81.015, 33.979], [-81.016, 33.979], [-81.016, 33.978], [-81.017, 33.979], [-81.017, 33.978], [-81.016, 33.978], [-81.017, 33.977], [-81.016, 33.976], [-81.015, 33.976], [-81.014, 33.975], [-81.013, 33.975], [-81.014, 33.975], [-81.014, 33.974], [-81.015, 33.975], [-81.016, 33.975], [-81.016, 33.974], [-81.016, 33.973], [-81.015, 33.973], [-81.015, 33.972], [-81.016, 33.972], [-81.016, 33.971], [-81.016, 33.972], [-81.017, 33.972], [-81.016, 33.972], [-81.017, 33.972], [-81.017, 33.971], [-81.018, 33.971], [-81.018, 33.972], [-81.019, 33.971], [-81.02, 33.972], [-81.021, 33.973], [-81.022, 33.971], [-81.023, 33.972], [-81.023, 33.971], [-81.024, 33.971], [-81.025, 33.972], [-81.026, 33.972], [-81.025, 33.973], [-81.023, 33.975], [-81.024, 33.975], [-81.024, 33.976], [-81.025, 33.976], [-81.026, 33.976], [-81.026, 33.975], [-81.027, 33.976], [-81.026, 33.977], [-81.025, 33.977], [-81.024, 33.977], [-81.024, 33.978], [-81.025, 33.978], [-81.025, 33.979], [-81.024, 33.979], [-81.023, 33.979], [-81.023, 33.98], [-81.023, 33.981], [-81.024, 33.981], [-81.023, 33.981], [-81.023, 33.982], [-81.024, 33.982], [-81.025, 33.982], [-81.025, 33.983], [-81.024, 33.983], [-81.023, 33.984], [-81.024, 33.985], [-81.025, 33.986], [-81.025, 33.987], [-81.026, 33.986], [-81.026, 33.987], [-81.026, 33.986], [-81.027, 33.986], [-81.026, 33.986], [-81.025, 33.985], [-81.026, 33.985], [-81.026, 33.984], [-81.026, 33.983], [-81.027, 33.982], [-81.028, 33.982], [-81.027, 33.981], [-81.028, 33.981], [-81.029, 33.981], [-81.03, 33.981], [-81.029, 33.982], [-81.029, 33.983], [-81.03, 33.983], [-81.029, 33.984], [-81.028, 33.984], [-81.028, 33.985], [-81.027, 33.985], [-81.027, 33.986], [-81.028, 33.986], [-81.029, 33.986], [-81.028, 33.985], [-81.029, 33.985], [-81.03, 33.985], [-81.031, 33.985], [-81.032, 33.985], [-81.033, 33.985], [-81.033, 33.984], [-81.033, 33.983], [-81.033, 33.984], [-81.034, 33.984], [-81.035, 33.984], [-81.035, 33.983], [-81.035, 33.982], [-81.036, 33.981], [-81.035, 33.981], [-81.037, 33.98], [-81.038, 33.98], [-81.039, 33.98], [-81.039, 33.981], [-81.04, 33.982], [-81.041, 33.982], [-81.041, 33.981], [-81.042, 33.981], [-81.042, 33.98], [-81.043, 33.979], [-81.044, 33.978], [-81.045, 33.978], [-81.046, 33.978], [-81.046, 33.98], [-81.046, 33.983], [-81.046, 33.986], [-81.046, 33.987], [-81.046, 33.988], [-81.047, 33.988], [-81.048, 33.991], [-81.051, 33.996], [-81.053, 33.998], [-81.054, 33.999], [-81.054, 34], [-81.055, 34], [-81.055, 34.001], [-81.055, 34.002], [-81.056, 34.002], [-81.059, 34.004], [-81.059, 34.005], [-81.06, 34.005], [-81.061, 34.005], [-81.063, 34.006], [-81.064, 34.007], [-81.064, 34.008], [-81.065, 34.009], [-81.066, 34.009], [-81.067, 34.009], [-81.068, 34.009], [-81.07, 34.008], [-81.071, 34.007], [-81.072, 34.007], [-81.073, 34.007], [-81.074, 34.007], [-81.076, 34.008], [-81.077, 34.009], [-81.079, 34.011], [-81.08, 34.012], [-81.082, 34.012], [-81.084, 34.012], [-81.086, 34.014], [-81.088, 34.014], [-81.089, 34.014], [-81.09, 34.014], [-81.091, 34.014], [-81.092, 34.014], [-81.095, 34.016], [-81.097, 34.018], [-81.098, 34.019], [-81.099, 34.022], [-81.1, 34.024], [-81.101, 34.024], [-81.102, 34.024], [-81.104, 34.023], [-81.104, 34.024], [-81.103, 34.024], [-81.104, 34.025], [-81.104, 34.026], [-81.105, 34.026], [-81.108, 34.026], [-81.109, 34.026], [-81.11, 34.026], [-81.112, 34.026], [-81.112, 34.027], [-81.113, 34.027], [-81.113, 34.028], [-81.112, 34.028], [-81.111, 34.028], [-81.11, 34.027], [-81.11, 34.028], [-81.11, 34.029], [-81.11, 34.031], [-81.11, 34.03], [-81.11, 34.031], [-81.109, 34.031], [-81.108, 34.031], [-81.107, 34.031], [-81.106, 34.031], [-81.105, 34.032], [-81.105, 34.033], [-81.104, 34.032], [-81.104, 34.031], [-81.103, 34.031], [-81.102, 34.031], [-81.103, 34.031], [-81.103, 34.03], [-81.102, 34.03], [-81.101, 34.03], [-81.101, 34.029], [-81.101, 34.028], [-81.1, 34.028], [-81.1, 34.027], [-81.1, 34.026], [-81.099, 34.026], [-81.098, 34.025], [-81.098, 34.026], [-81.097, 34.026], [-81.097, 34.025], [-81.098, 34.025], [-81.097, 34.025], [-81.097, 34.024], [-81.096, 34.025], [-81.095, 34.025], [-81.096, 34.024], [-81.096, 34.023], [-81.095, 34.023], [-81.095, 34.024], [-81.095, 34.025], [-81.094, 34.025], [-81.093, 34.025], [-81.093, 34.026], [-81.093, 34.027], [-81.094, 34.027], [-81.094, 34.028], [-81.094, 34.029], [-81.093, 34.029], [-81.092, 34.029], [-81.092, 34.03], [-81.093, 34.03], [-81.093, 34.031], [-81.094, 34.031], [-81.095, 34.031], [-81.096, 34.031], [-81.096, 34.03], [-81.097, 34.03], [-81.097, 34.031], [-81.097, 34.03], [-81.098, 34.03], [-81.099, 34.03], [-81.1, 34.03], [-81.099, 34.03], [-81.1, 34.032], [-81.099, 34.032], [-81.098, 34.032], [-81.098, 34.033], [-81.097, 34.033], [-81.097, 34.032], [-81.096, 34.032], [-81.095, 34.033], [-81.093, 34.034], [-81.093, 34.035], [-81.092, 34.035], [-81.091, 34.035], [-81.09, 34.035], [-81.089, 34.035], [-81.089, 34.034], [-81.088, 34.034], [-81.087, 34.034], [-81.087, 34.033], [-81.088, 34.033], [-81.089, 34.033], [-81.091, 34.032], [-81.091, 34.031], [-81.09, 34.031], [-81.089, 34.031], [-81.088, 34.031], [-81.087, 34.031], [-81.086, 34.031], [-81.087, 34.031], [-81.087, 34.03], [-81.087, 34.029], [-81.087, 34.027], [-81.086, 34.026], [-81.087, 34.026], [-81.086, 34.026], [-81.087, 34.026], [-81.088, 34.025], [-81.088, 34.024], [-81.087, 34.024], [-81.087, 34.023], [-81.086, 34.023], [-81.087, 34.023], [-81.087, 34.022], [-81.087, 34.023], [-81.088, 34.022], [-81.087, 34.022], [-81.088, 34.022], [-81.088, 34.021], [-81.088, 34.02], [-81.087, 34.02], [-81.086, 34.02], [-81.086, 34.018], [-81.085, 34.018], [-81.084, 34.018], [-81.083, 34.018], [-81.085, 34.017], [-81.084, 34.017], [-81.085, 34.016], [-81.085, 34.015], [-81.086, 34.015], [-81.086, 34.014], [-81.085, 34.015], [-81.085, 34.016], [-81.084, 34.016], [-81.083, 34.016], [-81.082, 34.016], [-81.082, 34.017], [-81.082, 34.016], [-81.081, 34.016], [-81.08, 34.016], [-81.081, 34.017], [-81.081, 34.019], [-81.082, 34.02], [-81.081, 34.02], [-81.078, 34.02], [-81.078, 34.021], [-81.077, 34.021], [-81.078, 34.022], [-81.078, 34.023], [-81.079, 34.022], [-81.08, 34.022], [-81.081, 34.023], [-81.081, 34.024], [-81.083, 34.025], [-81.082, 34.026], [-81.081, 34.027], [-81.08, 34.027], [-81.08, 34.026], [-81.079, 34.026], [-81.079, 34.025], [-81.079, 34.024], [-81.078, 34.024], [-81.078, 34.023], [-81.077, 34.023], [-81.078, 34.024], [-81.077, 34.024], [-81.078, 34.025], [-81.077, 34.025], [-81.077, 34.026], [-81.078, 34.027], [-81.079, 34.027], [-81.079, 34.028], [-81.079, 34.027], [-81.08, 34.027], [-81.081, 34.028], [-81.081, 34.027], [-81.081, 34.028], [-81.08, 34.028], [-81.08, 34.029], [-81.08, 34.03], [-81.081, 34.03], [-81.08, 34.03], [-81.08, 34.031], [-81.079, 34.031], [-81.079, 34.032], [-81.08, 34.032], [-81.08, 34.033], [-81.08, 34.034], [-81.079, 34.035], [-81.079, 34.034], [-81.078, 34.034], [-81.078, 34.035], [-81.077, 34.035], [-81.077, 34.036], [-81.076, 34.036], [-81.076, 34.035], [-81.075, 34.035], [-81.076, 34.036], [-81.076, 34.037], [-81.076, 34.038], [-81.072, 34.038], [-81.072, 34.039], [-81.072, 34.04], [-81.072, 34.041], [-81.073, 34.042], [-81.073, 34.043], [-81.073, 34.044], [-81.074, 34.044], [-81.074, 34.045], [-81.075, 34.044], [-81.076, 34.044], [-81.078, 34.044], [-81.078, 34.045], [-81.079, 34.045], [-81.08, 34.045], [-81.08, 34.046], [-81.079, 34.046], [-81.08, 34.047], [-81.079, 34.047], [-81.078, 34.048], [-81.078, 34.049], [-81.078, 34.048], [-81.077, 34.048], [-81.076, 34.048], [-81.075, 34.049], [-81.075, 34.05], [-81.075, 34.051], [-81.076, 34.051], [-81.076, 34.052], [-81.076, 34.053], [-81.076, 34.054], [-81.076, 34.055], [-81.076, 34.056], [-81.077, 34.056], [-81.077, 34.057], [-81.078, 34.056], [-81.079, 34.056], [-81.08, 34.056], [-81.081, 34.055], [-81.081, 34.056], [-81.082, 34.056], [-81.083, 34.058], [-81.084, 34.058], [-81.085, 34.058], [-81.089, 34.058], [-81.092, 34.057], [-81.093, 34.057], [-81.093, 34.056], [-81.094, 34.056], [-81.095, 34.056], [-81.095, 34.055], [-81.096, 34.055], [-81.096, 34.054], [-81.097, 34.055], [-81.096, 34.055], [-81.097, 34.057], [-81.098, 34.056], [-81.099, 34.056], [-81.099, 34.055], [-81.099, 34.056], [-81.1, 34.056], [-81.101, 34.055], [-81.102, 34.054], [-81.102, 34.053], [-81.102, 34.052], [-81.101, 34.052], [-81.101, 34.051], [-81.101, 34.052], [-81.102, 34.052], [-81.102, 34.051], [-81.102, 34.05], [-81.103, 34.05], [-81.103, 34.051], [-81.104, 34.051], [-81.104, 34.05], [-81.104, 34.051], [-81.104, 34.052], [-81.105, 34.051], [-81.106, 34.052], [-81.107, 34.052], [-81.108, 34.052], [-81.108, 34.053], [-81.109, 34.053], [-81.109, 34.054], [-81.11, 34.054], [-81.11, 34.055], [-81.111, 34.055], [-81.111, 34.056], [-81.11, 34.056], [-81.11, 34.055], [-81.109, 34.055], [-81.108, 34.056], [-81.107, 34.056], [-81.108, 34.057], [-81.109, 34.057], [-81.109, 34.058], [-81.108, 34.059], [-81.107, 34.059], [-81.106, 34.059], [-81.105, 34.06], [-81.105, 34.061], [-81.107, 34.06], [-81.108, 34.06], [-81.109, 34.06], [-81.109, 34.061], [-81.11, 34.061], [-81.106, 34.063], [-81.107, 34.063], [-81.107, 34.064], [-81.108, 34.064], [-81.109, 34.064], [-81.112, 34.063], [-81.113, 34.063], [-81.114, 34.062], [-81.115, 34.063], [-81.115, 34.064], [-81.114, 34.064], [-81.113, 34.064], [-81.11, 34.064], [-81.109, 34.065], [-81.108, 34.065], [-81.109, 34.065], [-81.108, 34.065], [-81.108, 34.066], [-81.109, 34.066], [-81.109, 34.067], [-81.11, 34.068], [-81.113, 34.067], [-81.114, 34.066], [-81.116, 34.065], [-81.117, 34.066], [-81.117, 34.067], [-81.118, 34.069], [-81.119, 34.069], [-81.119, 34.07], [-81.12, 34.07], [-81.121, 34.07], [-81.12, 34.07], [-81.12, 34.071], [-81.121, 34.071], [-81.122, 34.072], [-81.122, 34.073], [-81.123, 34.073], [-81.124, 34.072], [-81.124, 34.073], [-81.125, 34.072], [-81.126, 34.072], [-81.126, 34.071], [-81.127, 34.071], [-81.126, 34.071], [-81.125, 34.071], [-81.126, 34.07], [-81.127, 34.07], [-81.127, 34.069], [-81.126, 34.068], [-81.125, 34.068], [-81.125, 34.067], [-81.126, 34.067], [-81.127, 34.068], [-81.129, 34.068], [-81.13, 34.068], [-81.131, 34.07], [-81.129, 34.072], [-81.128, 34.072], [-81.129, 34.073], [-81.13, 34.074], [-81.131, 34.074], [-81.131, 34.073], [-81.133, 34.073], [-81.134, 34.072], [-81.135, 34.072], [-81.135, 34.073], [-81.136, 34.072], [-81.136, 34.074], [-81.137, 34.075], [-81.138, 34.075], [-81.139, 34.075], [-81.14, 34.074], [-81.14, 34.075], [-81.139, 34.075], [-81.139, 34.076], [-81.138, 34.076], [-81.138, 34.075], [-81.137, 34.075], [-81.136, 34.075], [-81.135, 34.075], [-81.134, 34.075], [-81.132, 34.075], [-81.132, 34.076], [-81.132, 34.077], [-81.133, 34.077], [-81.133, 34.078], [-81.133, 34.079], [-81.132, 34.079], [-81.132, 34.08], [-81.131, 34.08], [-81.132, 34.08], [-81.132, 34.081], [-81.133, 34.081], [-81.134, 34.081], [-81.134, 34.082], [-81.135, 34.082], [-81.136, 34.083], [-81.137, 34.083], [-81.138, 34.083], [-81.138, 34.084], [-81.138, 34.085], [-81.138, 34.086], [-81.139, 34.086], [-81.138, 34.086], [-81.139, 34.086], [-81.139, 34.085], [-81.139, 34.086], [-81.14, 34.086], [-81.141, 34.086], [-81.142, 34.086], [-81.142, 34.087], [-81.143, 34.087], [-81.144, 34.087], [-81.144, 34.088], [-81.145, 34.088], [-81.146, 34.089], [-81.147, 34.089], [-81.148, 34.089], [-81.148, 34.088], [-81.147, 34.088], [-81.147, 34.087], [-81.146, 34.087], [-81.147, 34.087], [-81.148, 34.087], [-81.148, 34.086], [-81.149, 34.085], [-81.149, 34.083], [-81.151, 34.084], [-81.151, 34.083], [-81.152, 34.083], [-81.151, 34.083], [-81.15, 34.082], [-81.149, 34.082], [-81.148, 34.082], [-81.147, 34.082], [-81.148, 34.08], [-81.149, 34.081], [-81.15, 34.08], [-81.151, 34.08], [-81.15, 34.08], [-81.15, 34.079], [-81.15, 34.078], [-81.149, 34.078], [-81.149, 34.077], [-81.148, 34.077], [-81.147, 34.077], [-81.147, 34.076], [-81.146, 34.076], [-81.146, 34.075], [-81.145, 34.075], [-81.145, 34.074], [-81.146, 34.074], [-81.146, 34.075], [-81.147, 34.074], [-81.148, 34.074], [-81.148, 34.073], [-81.147, 34.073], [-81.146, 34.073], [-81.145, 34.072], [-81.145, 34.073], [-81.144, 34.073], [-81.143, 34.073], [-81.145, 34.072], [-81.146, 34.072], [-81.147, 34.072], [-81.147, 34.071], [-81.147, 34.072], [-81.148, 34.071], [-81.149, 34.071], [-81.15, 34.071], [-81.153, 34.069], [-81.153, 34.068], [-81.159, 34.066], [-81.16, 34.066], [-81.161, 34.067], [-81.162, 34.067], [-81.163, 34.066], [-81.164, 34.065], [-81.165, 34.065], [-81.166, 34.065], [-81.167, 34.065], [-81.167, 34.064], [-81.167, 34.065], [-81.168, 34.066], [-81.167, 34.067], [-81.168, 34.067], [-81.168, 34.068], [-81.165, 34.07], [-81.163, 34.071], [-81.163, 34.072], [-81.162, 34.072], [-81.162, 34.073], [-81.162, 34.074], [-81.162, 34.075], [-81.162, 34.076], [-81.163, 34.076], [-81.164, 34.076], [-81.164, 34.077], [-81.163, 34.076], [-81.162, 34.077], [-81.161, 34.077], [-81.161, 34.078], [-81.161, 34.079], [-81.16, 34.079], [-81.16, 34.08], [-81.161, 34.08], [-81.161, 34.081], [-81.161, 34.082], [-81.162, 34.082], [-81.162, 34.083], [-81.162, 34.082], [-81.162, 34.081], [-81.163, 34.081], [-81.163, 34.08], [-81.163, 34.079], [-81.163, 34.078], [-81.164, 34.078], [-81.164, 34.079], [-81.164, 34.08], [-81.165, 34.08], [-81.165, 34.081], [-81.166, 34.082], [-81.166, 34.083], [-81.165, 34.083], [-81.164, 34.083], [-81.164, 34.084], [-81.166, 34.085], [-81.165, 34.085], [-81.165, 34.086], [-81.164, 34.086], [-81.164, 34.087], [-81.165, 34.088], [-81.166, 34.088], [-81.166, 34.089], [-81.167, 34.089], [-81.168, 34.089], [-81.168, 34.09], [-81.169, 34.09], [-81.17, 34.09], [-81.171, 34.09], [-81.171, 34.091]], [[-81.074, 34.023], [-81.074, 34.024], [-81.075, 34.024], [-81.074, 34.023]], [[-81.074, 34.023], [-81.075, 34.023], [-81.075, 34.024], [-81.076, 34.024], [-81.076, 34.023], [-81.075, 34.023], [-81.075, 34.024], [-81.075, 34.023], [-81.075, 34.022], [-81.074, 34.021], [-81.074, 34.02], [-81.073, 34.018], [-81.072, 34.018], [-81.072, 34.017], [-81.071, 34.017], [-81.071, 34.016], [-81.07, 34.016], [-81.07, 34.015], [-81.069, 34.015], [-81.068, 34.014], [-81.068, 34.013], [-81.067, 34.013], [-81.066, 34.013], [-81.066, 34.014], [-81.067, 34.014], [-81.067, 34.015], [-81.068, 34.015], [-81.068, 34.016], [-81.069, 34.016], [-81.068, 34.016], [-81.068, 34.017], [-81.067, 34.017], [-81.068, 34.017], [-81.067, 34.017], [-81.068, 34.016], [-81.067, 34.016], [-81.066, 34.016], [-81.067, 34.016], [-81.066, 34.016], [-81.066, 34.015], [-81.065, 34.015], [-81.065, 34.016], [-81.066, 34.016], [-81.065, 34.016], [-81.066, 34.016], [-81.066, 34.017], [-81.065, 34.017], [-81.065, 34.018], [-81.064, 34.018], [-81.064, 34.019], [-81.065, 34.019], [-81.065, 34.02], [-81.066, 34.019], [-81.066, 34.02], [-81.067, 34.02], [-81.068, 34.02], [-81.069, 34.02], [-81.073, 34.02], [-81.073, 34.021], [-81.074, 34.021], [-81.074, 34.022], [-81.074, 34.023]], [[-81.056, 34.028], [-81.056, 34.027], [-81.055, 34.027], [-81.056, 34.027], [-81.056, 34.028]], [[-81.055, 34.027], [-81.056, 34.027], [-81.056, 34.026], [-81.056, 34.025], [-81.055, 34.025], [-81.056, 34.025], [-81.056, 34.024], [-81.055, 34.024], [-81.054, 34.024], [-81.054, 34.025], [-81.055, 34.025], [-81.055, 34.026], [-81.054, 34.026], [-81.055, 34.026], [-81.055, 34.027]], [[-80.843, 34.119], [-80.843, 34.118], [-80.842, 34.118], [-80.841, 34.117], [-80.843, 34.115], [-80.842, 34.114], [-80.841, 34.114], [-80.839, 34.112], [-80.838, 34.112], [-80.838, 34.111], [-80.832, 34.107], [-80.832, 34.108], [-80.831, 34.107], [-80.83, 34.107], [-80.831, 34.107], [-80.83, 34.107], [-80.83, 34.106], [-80.829, 34.106], [-80.828, 34.106], [-80.827, 34.106], [-80.826, 34.107], [-80.826, 34.108], [-80.825, 34.109], [-80.824, 34.109], [-80.823, 34.11], [-80.822, 34.11], [-80.823, 34.109], [-80.822, 34.109], [-80.822, 34.108], [-80.821, 34.108], [-80.82, 34.111], [-80.82, 34.112], [-80.819, 34.112], [-80.82, 34.113], [-80.821, 34.113], [-80.822, 34.113], [-80.822, 34.114], [-80.823, 34.114], [-80.824, 34.115], [-80.825, 34.114], [-80.825, 34.115], [-80.826, 34.115], [-80.827, 34.115], [-80.828, 34.115], [-80.828, 34.116], [-80.828, 34.117], [-80.829, 34.117], [-80.83, 34.116], [-80.83, 34.115], [-80.831, 34.114], [-80.83, 34.114], [-80.828, 34.114], [-80.829, 34.113], [-80.83, 34.112], [-80.831, 34.112], [-80.832, 34.112], [-80.832, 34.111], [-80.833, 34.111], [-80.834, 34.112], [-80.835, 34.112], [-80.835, 34.113], [-80.836, 34.113], [-80.836, 34.114], [-80.837, 34.115], [-80.838, 34.115], [-80.838, 34.116], [-80.837, 34.116], [-80.836, 34.116], [-80.836, 34.117], [-80.836, 34.118], [-80.837, 34.118], [-80.838, 34.119], [-80.839, 34.119], [-80.84, 34.119], [-80.841, 34.119], [-80.842, 34.119], [-80.843, 34.119]], [[-80.964, 33.958], [-80.963, 33.958], [-80.962, 33.958], [-80.961, 33.958], [-80.961, 33.959], [-80.96, 33.959], [-80.959, 33.959], [-80.959, 33.96], [-80.958, 33.96], [-80.958, 33.959], [-80.957, 33.959], [-80.957, 33.96], [-80.956, 33.96], [-80.955, 33.96], [-80.955, 33.963], [-80.955, 33.964], [-80.955, 33.966], [-80.956, 33.967], [-80.954, 33.967], [-80.954, 33.968], [-80.955, 33.968], [-80.956, 33.968], [-80.956, 33.969], [-80.956, 33.972], [-80.956, 33.973], [-80.956, 33.974], [-80.956, 33.975], [-80.957, 33.975], [-80.957, 33.974], [-80.957, 33.973], [-80.957, 33.972], [-80.957, 33.969], [-80.957, 33.968], [-80.958, 33.968], [-80.959, 33.969], [-80.96, 33.968], [-80.961, 33.967], [-80.961, 33.966], [-80.961, 33.965], [-80.962, 33.965], [-80.962, 33.964], [-80.961, 33.964], [-80.962, 33.964], [-80.962, 33.963], [-80.963, 33.962], [-80.963, 33.961], [-80.963, 33.96], [-80.963, 33.959], [-80.963, 33.958], [-80.964, 33.958]], [[-80.989, 33.988], [-80.989, 33.987], [-80.988, 33.987], [-80.989, 33.987], [-80.989, 33.986], [-80.988, 33.986], [-80.987, 33.986], [-80.988, 33.986], [-80.989, 33.986], [-80.989, 33.985], [-80.988, 33.985], [-80.989, 33.985], [-80.989, 33.984], [-80.989, 33.983], [-80.988, 33.983], [-80.988, 33.982], [-80.987, 33.982], [-80.987, 33.981], [-80.988, 33.981], [-80.987, 33.981], [-80.987, 33.982], [-80.987, 33.981], [-80.986, 33.981], [-80.986, 33.982], [-80.986, 33.981], [-80.985, 33.981], [-80.984, 33.981], [-80.984, 33.98], [-80.984, 33.981], [-80.983, 33.981], [-80.983, 33.98], [-80.982, 33.98], [-80.982, 33.981], [-80.983, 33.981], [-80.982, 33.981], [-80.983, 33.981], [-80.984, 33.981], [-80.983, 33.981], [-80.983, 33.982], [-80.982, 33.982], [-80.982, 33.981], [-80.981, 33.981], [-80.981, 33.982], [-80.98, 33.982], [-80.98, 33.981], [-80.979, 33.981], [-80.979, 33.982], [-80.98, 33.982], [-80.98, 33.983], [-80.981, 33.983], [-80.982, 33.983], [-80.983, 33.983], [-80.983, 33.984], [-80.982, 33.984], [-80.983, 33.984], [-80.983, 33.985], [-80.982, 33.985], [-80.983, 33.985], [-80.983, 33.986], [-80.982, 33.986], [-80.982, 33.987], [-80.982, 33.986], [-80.979, 33.986], [-80.979, 33.987], [-80.98, 33.987], [-80.981, 33.987], [-80.982, 33.987], [-80.983, 33.987], [-80.983, 33.988], [-80.982, 33.988], [-80.983, 33.988], [-80.983, 33.987], [-80.984, 33.987], [-80.984, 33.988], [-80.985, 33.988], [-80.985, 33.987], [-80.984, 33.987], [-80.984, 33.986], [-80.983, 33.986], [-80.984, 33.986], [-80.984, 33.987], [-80.985, 33.987], [-80.986, 33.987], [-80.987, 33.987], [-80.987, 33.988], [-80.988, 33.988], [-80.989, 33.988]], [[-80.955, 33.979], [-80.954, 33.979], [-80.953, 33.979], [-80.952, 33.979], [-80.951, 33.979], [-80.95, 33.979], [-80.949, 33.978], [-80.948, 33.978], [-80.947, 33.978], [-80.947, 33.977], [-80.946, 33.977], [-80.945, 33.977], [-80.944, 33.976], [-80.944, 33.977], [-80.945, 33.979], [-80.946, 33.979], [-80.947, 33.981], [-80.948, 33.981], [-80.949, 33.981], [-80.95, 33.981], [-80.951, 33.981], [-80.95, 33.982], [-80.95, 33.984], [-80.95, 33.985], [-80.952, 33.984], [-80.953, 33.984], [-80.954, 33.984], [-80.954, 33.982], [-80.954, 33.979], [-80.955, 33.979]], [[-81.016, 34.045], [-81.015, 34.044], [-81.014, 34.045], [-81.014, 34.044], [-81.015, 34.043], [-81.014, 34.043], [-81.014, 34.042], [-81.014, 34.041], [-81.013, 34.041], [-81.012, 34.041], [-81.012, 34.042], [-81.011, 34.042], [-81.011, 34.043], [-81.01, 34.044], [-81.009, 34.044], [-81.009, 34.045], [-81.008, 34.046], [-81.008, 34.047], [-81.008, 34.048], [-81.009, 34.047], [-81.01, 34.047], [-81.01, 34.048], [-81.011, 34.048], [-81.011, 34.047], [-81.012, 34.048], [-81.013, 34.047], [-81.013, 34.048], [-81.013, 34.049], [-81.012, 34.049], [-81.013, 34.049], [-81.013, 34.05], [-81.014, 34.05], [-81.015, 34.049], [-81.015, 34.048], [-81.014, 34.047], [-81.014, 34.048], [-81.013, 34.048], [-81.013, 34.047], [-81.014, 34.047], [-81.015, 34.046], [-81.015, 34.045], [-81.015, 34.046], [-81.016, 34.046], [-81.016, 34.045]], [[-80.973, 33.979], [-80.973, 33.978], [-80.972, 33.978], [-80.972, 33.977], [-80.973, 33.977], [-80.974, 33.977], [-80.973, 33.977], [-80.973, 33.976], [-80.973, 33.975], [-80.972, 33.975], [-80.971, 33.975], [-80.969, 33.974], [-80.968, 33.974], [-80.968, 33.975], [-80.968, 33.976], [-80.967, 33.976], [-80.967, 33.977], [-80.967, 33.978], [-80.966, 33.979], [-80.966, 33.98], [-80.966, 33.981], [-80.967, 33.981], [-80.967, 33.98], [-80.967, 33.979], [-80.968, 33.979], [-80.968, 33.98], [-80.969, 33.98], [-80.968, 33.979], [-80.969, 33.979], [-80.97, 33.979], [-80.971, 33.979], [-80.972, 33.98], [-80.972, 33.979], [-80.973, 33.979]], [[-81.015, 34.035], [-81.014, 34.035], [-81.014, 34.034], [-81.013, 34.034], [-81.013, 34.035], [-81.013, 34.034], [-81.012, 34.034], [-81.011, 34.034], [-81.01, 34.034], [-81.009, 34.034], [-81.009, 34.033], [-81.009, 34.032], [-81.008, 34.032], [-81.007, 34.032], [-81.006, 34.032], [-81.006, 34.033], [-81.005, 34.033], [-81.005, 34.034], [-81.006, 34.034], [-81.006, 34.035], [-81.004, 34.035], [-81.005, 34.035], [-81.005, 34.036], [-81.005, 34.037], [-81.004, 34.037], [-81.003, 34.037], [-81.004, 34.038], [-81.005, 34.038], [-81.005, 34.037], [-81.006, 34.037], [-81.007, 34.037], [-81.008, 34.037], [-81.009, 34.037], [-81.01, 34.037], [-81.01, 34.036], [-81.011, 34.036], [-81.012, 34.036], [-81.012, 34.035], [-81.013, 34.035], [-81.013, 34.036], [-81.013, 34.035], [-81.014, 34.035], [-81.015, 34.035]], [[-80.97, 33.952], [-80.969, 33.952], [-80.968, 33.952], [-80.965, 33.953], [-80.962, 33.954], [-80.963, 33.954], [-80.963, 33.955], [-80.964, 33.955], [-80.965, 33.955], [-80.966, 33.954], [-80.967, 33.954], [-80.969, 33.955], [-80.972, 33.957], [-80.973, 33.957], [-80.974, 33.955], [-80.975, 33.955], [-80.97, 33.952]], [[-80.996, 34.077], [-80.995, 34.077], [-80.994, 34.076], [-80.994, 34.075], [-80.995, 34.075], [-80.995, 34.074], [-80.994, 34.074], [-80.994, 34.073], [-80.995, 34.073], [-80.994, 34.071], [-80.992, 34.069], [-80.991, 34.068], [-80.991, 34.067], [-80.99, 34.067], [-80.99, 34.068], [-80.991, 34.068], [-80.99, 34.068], [-80.992, 34.071], [-80.993, 34.072], [-80.993, 34.073], [-80.993, 34.074], [-80.992, 34.074], [-80.988, 34.073], [-80.988, 34.074], [-80.99, 34.075], [-80.991, 34.075], [-80.992, 34.076], [-80.993, 34.076], [-80.995, 34.077], [-80.996, 34.077]], [[-80.978, 33.969], [-80.977, 33.969], [-80.976, 33.968], [-80.977, 33.968], [-80.975, 33.967], [-80.974, 33.966], [-80.974, 33.967], [-80.974, 33.966], [-80.973, 33.966], [-80.972, 33.965], [-80.971, 33.965], [-80.97, 33.964], [-80.97, 33.965], [-80.97, 33.964], [-80.969, 33.964], [-80.968, 33.965], [-80.969, 33.965], [-80.968, 33.965], [-80.968, 33.966], [-80.968, 33.965], [-80.968, 33.966], [-80.969, 33.966], [-80.969, 33.967], [-80.97, 33.967], [-80.971, 33.967], [-80.972, 33.968], [-80.973, 33.967], [-80.973, 33.968], [-80.972, 33.968], [-80.973, 33.968], [-80.975, 33.969], [-80.976, 33.969], [-80.975, 33.969], [-80.975, 33.97], [-80.977, 33.97], [-80.978, 33.969]], [[-81.012, 34.05], [-81.011, 34.05], [-81.01, 34.05], [-81.009, 34.05], [-81.008, 34.051], [-81.007, 34.051], [-81.007, 34.05], [-81.007, 34.049], [-81.008, 34.049], [-81.008, 34.048], [-81.007, 34.048], [-81.006, 34.048], [-81.005, 34.049], [-81.004, 34.049], [-81.003, 34.049], [-81.002, 34.05], [-81.003, 34.05], [-81.004, 34.051], [-81.005, 34.051], [-81.006, 34.051], [-81.006, 34.052], [-81.007, 34.052], [-81.007, 34.053], [-81.006, 34.053], [-81.007, 34.053], [-81.008, 34.052], [-81.01, 34.052], [-81.01, 34.051], [-81.011, 34.051], [-81.012, 34.051], [-81.012, 34.05]], [[-80.926, 33.98], [-80.926, 33.979], [-80.925, 33.979], [-80.924, 33.979], [-80.923, 33.98], [-80.922, 33.98], [-80.921, 33.98], [-80.92, 33.98], [-80.919, 33.98], [-80.918, 33.98], [-80.917, 33.98], [-80.918, 33.981], [-80.917, 33.98], [-80.917, 33.981], [-80.918, 33.981], [-80.919, 33.981], [-80.918, 33.981], [-80.919, 33.981], [-80.919, 33.982], [-80.92, 33.982], [-80.921, 33.982], [-80.922, 33.983], [-80.923, 33.982], [-80.925, 33.981], [-80.926, 33.98]], [[-81.032, 34.057], [-81.031, 34.057], [-81.03, 34.056], [-81.029, 34.055], [-81.029, 34.056], [-81.029, 34.055], [-81.028, 34.055], [-81.027, 34.056], [-81.028, 34.056], [-81.029, 34.057], [-81.029, 34.056], [-81.03, 34.057], [-81.031, 34.057], [-81.03, 34.058], [-81.027, 34.057], [-81.027, 34.058], [-81.026, 34.058], [-81.026, 34.059], [-81.025, 34.059], [-81.025, 34.06], [-81.026, 34.06], [-81.026, 34.061], [-81.028, 34.06], [-81.029, 34.059], [-81.03, 34.058], [-81.031, 34.058], [-81.032, 34.057]], [[-81.063, 34.022], [-81.061, 34.02], [-81.06, 34.02], [-81.059, 34.02], [-81.058, 34.021], [-81.057, 34.022], [-81.058, 34.022], [-81.058, 34.023], [-81.059, 34.023], [-81.06, 34.023], [-81.06, 34.024], [-81.062, 34.023], [-81.062, 34.022], [-81.063, 34.022]], [[-81.079, 34.031], [-81.079, 34.03], [-81.079, 34.031], [-81.079, 34.03], [-81.078, 34.03], [-81.077, 34.03], [-81.077, 34.029], [-81.077, 34.03], [-81.076, 34.03], [-81.075, 34.03], [-81.075, 34.031], [-81.075, 34.032], [-81.075, 34.033], [-81.077, 34.033], [-81.078, 34.033], [-81.078, 34.034], [-81.079, 34.033], [-81.078, 34.033], [-81.078, 34.032], [-81.078, 34.031], [-81.079, 34.031]], [[-81.024, 34.039], [-81.024, 34.038], [-81.024, 34.039], [-81.024, 34.038], [-81.024, 34.037], [-81.024, 34.036], [-81.023, 34.036], [-81.023, 34.037], [-81.022, 34.037], [-81.022, 34.038], [-81.022, 34.037], [-81.022, 34.036], [-81.021, 34.036], [-81.02, 34.036], [-81.02, 34.037], [-81.021, 34.037], [-81.02, 34.037], [-81.021, 34.038], [-81.021, 34.039], [-81.02, 34.039], [-81.021, 34.04], [-81.022, 34.04], [-81.022, 34.039], [-81.023, 34.039], [-81.023, 34.038], [-81.023, 34.039], [-81.024, 34.039], [-81.023, 34.039], [-81.024, 34.04], [-81.024, 34.039]], [[-81.147, 34.09], [-81.146, 34.089], [-81.145, 34.089], [-81.146, 34.089], [-81.145, 34.089], [-81.145, 34.088], [-81.144, 34.088], [-81.143, 34.088], [-81.142, 34.088], [-81.143, 34.088], [-81.142, 34.087], [-81.141, 34.087], [-81.141, 34.088], [-81.143, 34.09], [-81.145, 34.091], [-81.146, 34.091], [-81.146, 34.09], [-81.147, 34.09]], [[-80.95, 33.968], [-80.949, 33.967], [-80.948, 33.967], [-80.949, 33.968], [-80.948, 33.968], [-80.947, 33.968], [-80.948, 33.969], [-80.947, 33.968], [-80.947, 33.969], [-80.946, 33.969], [-80.947, 33.969], [-80.947, 33.97], [-80.946, 33.971], [-80.947, 33.971], [-80.948, 33.97], [-80.947, 33.971], [-80.948, 33.972], [-80.949, 33.972], [-80.948, 33.972], [-80.948, 33.971], [-80.949, 33.97], [-80.949, 33.971], [-80.95, 33.97], [-80.949, 33.97], [-80.949, 33.969], [-80.95, 33.969], [-80.95, 33.968]], [[-80.821, 34.108], [-80.82, 34.108], [-80.818, 34.109], [-80.817, 34.109], [-80.818, 34.111], [-80.818, 34.112], [-80.819, 34.112], [-80.82, 34.111], [-80.821, 34.108]], [[-80.982, 33.984], [-80.982, 33.983], [-80.98, 33.984], [-80.98, 33.983], [-80.977, 33.983], [-80.977, 33.984], [-80.977, 33.985], [-80.976, 33.985], [-80.976, 33.986], [-80.977, 33.986], [-80.977, 33.985], [-80.978, 33.985], [-80.981, 33.985], [-80.982, 33.985], [-80.982, 33.984]], [[-80.992, 33.976], [-80.992, 33.975], [-80.991, 33.975], [-80.991, 33.974], [-80.99, 33.974], [-80.99, 33.975], [-80.99, 33.976], [-80.99, 33.977], [-80.99, 33.978], [-80.991, 33.978], [-80.99, 33.978], [-80.991, 33.978], [-80.99, 33.978], [-80.99, 33.979], [-80.991, 33.979], [-80.992, 33.978], [-80.992, 33.977], [-80.992, 33.976], [-80.993, 33.976], [-80.992, 33.976]], [[-81.106, 34.03], [-81.106, 34.029], [-81.106, 34.027], [-81.105, 34.027], [-81.104, 34.027], [-81.104, 34.03], [-81.105, 34.03], [-81.106, 34.03]], [[-80.821, 34.119], [-80.82, 34.119], [-80.82, 34.118], [-80.82, 34.117], [-80.819, 34.116], [-80.819, 34.115], [-80.818, 34.115], [-80.817, 34.116], [-80.818, 34.116], [-80.818, 34.117], [-80.819, 34.117], [-80.819, 34.118], [-80.818, 34.118], [-80.819, 34.118], [-80.819, 34.119], [-80.82, 34.12], [-80.821, 34.12], [-80.821, 34.119]], [[-81.017, 34.036], [-81.016, 34.036], [-81.015, 34.036], [-81.015, 34.037], [-81.014, 34.037], [-81.014, 34.038], [-81.014, 34.039], [-81.013, 34.039], [-81.014, 34.039], [-81.013, 34.039], [-81.013, 34.04], [-81.014, 34.04], [-81.015, 34.04], [-81.015, 34.039], [-81.015, 34.038], [-81.016, 34.038], [-81.016, 34.037], [-81.017, 34.037], [-81.017, 34.036]], [[-81.092, 34.023], [-81.091, 34.022], [-81.092, 34.022], [-81.091, 34.021], [-81.09, 34.022], [-81.091, 34.023], [-81.09, 34.022], [-81.089, 34.021], [-81.089, 34.022], [-81.088, 34.022], [-81.089, 34.023], [-81.089, 34.022], [-81.089, 34.023], [-81.09, 34.023], [-81.089, 34.023], [-81.09, 34.024], [-81.089, 34.024], [-81.088, 34.024], [-81.089, 34.024], [-81.089, 34.025], [-81.089, 34.024], [-81.091, 34.023], [-81.092, 34.023]], [[-80.975, 33.942], [-80.975, 33.941], [-80.974, 33.942], [-80.976, 33.944], [-80.977, 33.943], [-80.975, 33.942]], [[-81.066, 34.027], [-81.066, 34.026], [-81.066, 34.027], [-81.065, 34.027], [-81.064, 34.027], [-81.063, 34.027], [-81.063, 34.028], [-81.064, 34.029], [-81.065, 34.028], [-81.066, 34.028], [-81.065, 34.027], [-81.066, 34.027]], [[-80.954, 33.977], [-80.953, 33.977], [-80.952, 33.975], [-80.951, 33.976], [-80.951, 33.977], [-80.952, 33.977], [-80.952, 33.978], [-80.953, 33.978], [-80.954, 33.978], [-80.954, 33.977]], [[-80.969, 33.958], [-80.968, 33.958], [-80.967, 33.958], [-80.965, 33.957], [-80.965, 33.958], [-80.967, 33.959], [-80.966, 33.959], [-80.968, 33.959], [-80.969, 33.958]], [[-81.094, 34.021], [-81.093, 34.021], [-81.094, 34.021], [-81.093, 34.021], [-81.093, 34.02], [-81.092, 34.02], [-81.092, 34.021], [-81.092, 34.022], [-81.093, 34.022], [-81.094, 34.022], [-81.094, 34.021]], [[-80.855, 34.091], [-80.855, 34.09], [-80.854, 34.09], [-80.854, 34.091], [-80.853, 34.091], [-80.852, 34.091], [-80.853, 34.092], [-80.854, 34.092], [-80.855, 34.092], [-80.855, 34.091]], [[-81.082, 34.014], [-81.08, 34.014], [-81.079, 34.015], [-81.079, 34.016], [-81.08, 34.016], [-81.08, 34.015], [-81.081, 34.016], [-81.082, 34.015], [-81.082, 34.014]], [[-80.868, 34.086], [-80.867, 34.086], [-80.866, 34.086], [-80.865, 34.086], [-80.865, 34.087], [-80.866, 34.087], [-80.868, 34.087], [-80.868, 34.086]], [[-80.96, 33.955], [-80.959, 33.955], [-80.959, 33.954], [-80.96, 33.954], [-80.958, 33.954], [-80.957, 33.955], [-80.958, 33.955], [-80.959, 33.955], [-80.96, 33.955]], [[-80.956, 33.989], [-80.955, 33.989], [-80.954, 33.99], [-80.954, 33.988], [-80.954, 33.989], [-80.954, 33.99], [-80.955, 33.991], [-80.955, 33.99], [-80.956, 33.989]], [[-81.065, 34.015], [-81.064, 34.015], [-81.063, 34.015], [-81.063, 34.016], [-81.063, 34.017], [-81.064, 34.017], [-81.065, 34.016], [-81.064, 34.016], [-81.065, 34.016], [-81.065, 34.015]], [[-80.864, 34.087], [-80.863, 34.087], [-80.86, 34.088], [-80.861, 34.088], [-80.862, 34.088], [-80.861, 34.088], [-80.862, 34.088], [-80.864, 34.087]], [[-80.98, 33.991], [-80.979, 33.99], [-80.978, 33.989], [-80.978, 33.99], [-80.979, 33.99], [-80.979, 33.991], [-80.98, 33.991], [-80.98, 33.992], [-80.979, 33.992], [-80.979, 33.991], [-80.979, 33.992], [-80.979, 33.993], [-80.98, 33.992], [-80.98, 33.991]], [[-80.954, 33.971], [-80.952, 33.971], [-80.951, 33.972], [-80.953, 33.972], [-80.954, 33.972], [-80.954, 33.971]], [[-81.025, 34.032], [-81.024, 34.032], [-81.025, 34.032], [-81.024, 34.032], [-81.024, 34.031], [-81.024, 34.032], [-81.024, 34.033], [-81.023, 34.033], [-81.023, 34.034], [-81.024, 34.034], [-81.025, 34.033], [-81.025, 34.032]], [[-80.999, 34.072], [-80.999, 34.071], [-80.998, 34.071], [-80.996, 34.072], [-80.997, 34.072], [-80.998, 34.072], [-80.999, 34.072]], [[-81.088, 34.018], [-81.088, 34.017], [-81.087, 34.017], [-81.086, 34.017], [-81.086, 34.018], [-81.087, 34.018], [-81.088, 34.018]], [[-80.95, 33.977], [-80.95, 33.976], [-80.948, 33.976], [-80.948, 33.977], [-80.95, 33.977]], [[-81.15, 34.076], [-81.149, 34.074], [-81.148, 34.075], [-81.148, 34.076], [-81.149, 34.076], [-81.15, 34.076], [-81.151, 34.076], [-81.15, 34.076]], [[-81.074, 34.024], [-81.074, 34.023], [-81.073, 34.023], [-81.073, 34.024], [-81.074, 34.024], [-81.073, 34.024], [-81.072, 34.024], [-81.072, 34.025], [-81.072, 34.024], [-81.072, 34.025], [-81.073, 34.025], [-81.073, 34.024], [-81.074, 34.025], [-81.074, 34.024]], [[-81.142, 34.087], [-81.141, 34.086], [-81.14, 34.086], [-81.14, 34.087], [-81.141, 34.087], [-81.142, 34.087]], [[-80.872, 34.09], [-80.872, 34.089], [-80.87, 34.09], [-80.87, 34.091], [-80.872, 34.09]], [[-81.158, 34.074], [-81.157, 34.073], [-81.157, 34.074], [-81.156, 34.074], [-81.156, 34.075], [-81.157, 34.074], [-81.158, 34.074]], [[-80.904, 33.977], [-80.904, 33.976], [-80.903, 33.976], [-80.903, 33.977], [-80.903, 33.978], [-80.904, 33.978], [-80.904, 33.977]], [[-81.089, 34.019], [-81.088, 34.019], [-81.087, 34.019], [-81.087, 34.02], [-81.088, 34.02], [-81.088, 34.019], [-81.089, 34.019]], [[-80.983, 33.948], [-80.982, 33.948], [-80.982, 33.949], [-80.983, 33.949], [-80.983, 33.948]], [[-81.012, 34.03], [-81.011, 34.03], [-81.01, 34.03], [-81.01, 34.031], [-81.011, 34.031], [-81.012, 34.031], [-81.012, 34.03]], [[-80.964, 33.956], [-80.963, 33.956], [-80.962, 33.956], [-80.963, 33.956], [-80.964, 33.957], [-80.964, 33.956]], [[-81.157, 34.073], [-81.156, 34.073], [-81.156, 34.072], [-81.155, 34.072], [-81.156, 34.073], [-81.157, 34.073]], [[-80.951, 33.965], [-80.952, 33.964], [-80.951, 33.964], [-80.95, 33.964], [-80.95, 33.965], [-80.951, 33.965]], [[-80.981, 33.989], [-80.98, 33.989], [-80.98, 33.99], [-80.981, 33.99], [-80.981, 33.989]], [[-80.96, 34.012], [-80.96, 34.011], [-80.959, 34.011], [-80.959, 34.012], [-80.96, 34.012]], [[-80.816, 34.121], [-80.816, 34.12], [-80.816, 34.121], [-80.815, 34.121], [-80.815, 34.122], [-80.816, 34.121]], [[-81.079, 34.029], [-81.078, 34.029], [-81.078, 34.028], [-81.077, 34.029], [-81.078, 34.029], [-81.079, 34.029]], [[-80.975, 33.986], [-80.974, 33.986], [-80.973, 33.986], [-80.973, 33.987], [-80.974, 33.987], [-80.975, 33.987], [-80.975, 33.986]], [[-80.834, 34.114], [-80.833, 34.114], [-80.832, 34.114], [-80.831, 34.114], [-80.832, 34.114], [-80.833, 34.114], [-80.834, 34.114]], [[-81.076, 34.016], [-81.075, 34.016], [-81.074, 34.016], [-81.075, 34.016], [-81.076, 34.017], [-81.076, 34.016]], [[-81.024, 33.973], [-81.024, 33.972], [-81.023, 33.972], [-81.024, 33.972], [-81.023, 33.972], [-81.023, 33.973], [-81.023, 33.974], [-81.024, 33.973]], [[-81.026, 34.029], [-81.025, 34.03], [-81.025, 34.031], [-81.026, 34.03], [-81.026, 34.029]], [[-80.981, 33.974], [-80.98, 33.974], [-80.98, 33.975], [-80.981, 33.975], [-80.981, 33.974]], [[-81.078, 34.015], [-81.077, 34.015], [-81.077, 34.016], [-81.078, 34.016], [-81.078, 34.015]], [[-80.833, 34.118], [-80.832, 34.118], [-80.832, 34.119], [-80.833, 34.119], [-80.833, 34.118]], [[-80.952, 33.962], [-80.951, 33.962], [-80.951, 33.963], [-80.952, 33.963], [-80.952, 33.962]], [[-80.975, 33.989], [-80.974, 33.989], [-80.974, 33.99], [-80.975, 33.99], [-80.975, 33.989]], [[-80.94, 33.973], [-80.94, 33.974], [-80.941, 33.974], [-80.941, 33.973], [-80.94, 33.973]], [[-81.167, 34.067], [-81.166, 34.067], [-81.166, 34.068], [-81.167, 34.067]], [[-80.819, 34.129], [-80.818, 34.129], [-80.818, 34.13], [-80.819, 34.13], [-80.819, 34.129]], [[-81.16, 34.096], [-81.16, 34.095], [-81.159, 34.096], [-81.16, 34.096]], [[-80.951, 33.977], [-80.95, 33.977], [-80.95, 33.978], [-80.951, 33.977]], [[-80.956, 33.98], [-80.956, 33.979], [-80.955, 33.979], [-80.955, 33.98], [-80.956, 33.98]], [[-81.058, 34.044], [-81.058, 34.043], [-81.057, 34.043], [-81.057, 34.044], [-81.058, 34.044]], [[-81.059, 34.025], [-81.058, 34.025], [-81.058, 34.026], [-81.059, 34.025]], [[-81.159, 34.073], [-81.158, 34.073], [-81.158, 34.074], [-81.159, 34.073]], [[-80.972, 33.987], [-80.972, 33.986], [-80.971, 33.987], [-80.972, 33.987]], [[-81.155, 34.075], [-81.155, 34.074], [-81.154, 34.074], [-81.155, 34.075]], [[-81.08, 34.03], [-81.08, 34.029], [-81.079, 34.029], [-81.08, 34.03]], [[-81.016885, 34.044016], [-81.016868, 34.043995], [-81.016817, 34.043932], [-81.0168, 34.043912], [-81.016772, 34.043877], [-81.016689, 34.043775], [-81.016673, 34.043755], [-81.016662, 34.043741], [-81.016599, 34.043664], [-81.016533, 34.0437], [-81.016211, 34.043865], [-81.015919, 34.04401], [-81.016031, 34.044146], [-81.016134, 34.044272], [-81.016243, 34.044399], [-81.016633, 34.044166], [-81.016885, 34.044016]], [[-80.830989, 34.117888], [-80.830988, 34.117879], [-80.830982, 34.117822], [-80.830943, 34.117772], [-80.83081, 34.117723], [-80.830536, 34.11778], [-80.830497, 34.117789], [-80.830394, 34.117811], [-80.830359, 34.117811], [-80.830353, 34.117882], [-80.830306, 34.1179], [-80.830107, 34.117855], [-80.829973, 34.118228], [-80.829997, 34.118234], [-80.830021, 34.118244], [-80.830049, 34.118262], [-80.830069, 34.11828], [-80.830387, 34.118181], [-80.830427, 34.118184], [-80.830722, 34.118094], [-80.830907, 34.118039], [-80.830916, 34.118031], [-80.830989, 34.117888]], [[-81.009639, 34.029661], [-81.009546, 34.02963], [-81.009439, 34.029594], [-81.009383, 34.029576], [-81.009265, 34.029537], [-81.009075, 34.029903], [-81.009117, 34.029944], [-81.00953, 34.03034], [-81.00974, 34.029875], [-81.009812, 34.029718], [-81.009639, 34.029661]], [[-80.965423, 33.98101], [-80.96504, 33.980807], [-80.96486, 33.980712], [-80.964805, 33.980772], [-80.964641, 33.980955], [-80.964587, 33.981016], [-80.964681, 33.981074], [-80.964963, 33.98125], [-80.965058, 33.981309], [-80.965189, 33.981408], [-80.965223, 33.981368], [-80.96549, 33.981051], [-80.965423, 33.98101]], [[-80.8184, 34.113218], [-80.818389, 34.113206], [-80.81838, 34.113191], [-80.818372, 34.113173], [-80.818368, 34.113154], [-80.818367, 34.113138], [-80.818371, 34.113118], [-80.81777, 34.113052], [-80.817649, 34.113036], [-80.817563, 34.113353], [-80.817541, 34.113432], [-80.817535, 34.113468], [-80.817723, 34.113538], [-80.817856, 34.11359], [-80.818412, 34.113225], [-80.8184, 34.113218]], [[-81.016, 34.034], [-81.015, 34.034], [-81.015, 34.035], [-81.016, 34.035], [-81.016, 34.034]], [[-81.016, 34.042], [-81.015, 34.042], [-81.016, 34.043], [-81.016, 34.042]], [[-81.03, 34.025], [-81.029, 34.024], [-81.029, 34.025], [-81.03, 34.025]], [[-81.028519, 34.064386], [-81.028515, 34.064326], [-81.028524, 34.064048], [-81.028538, 34.063926], [-81.028291, 34.063926], [-81.028046, 34.063918], [-81.027723, 34.064364], [-81.028172, 34.064377], [-81.028316, 34.06438], [-81.028519, 34.064386]], [[-80.892398, 33.984457], [-80.892369, 33.984342], [-80.892315, 33.98415], [-80.891574, 33.984302], [-80.891584, 33.984334], [-80.891612, 33.984433], [-80.891627, 33.984484], [-80.891663, 33.984612], [-80.892398, 33.984457]], [[-81.095073, 34.021445], [-81.094575, 34.021751], [-81.09461, 34.021792], [-81.094638, 34.021832], [-81.094657, 34.021862], [-81.09468, 34.021905], [-81.094695, 34.02194], [-81.094705, 34.021965], [-81.09472, 34.022015], [-81.094731, 34.02207], [-81.094735, 34.022106], [-81.095329, 34.021737], [-81.095073, 34.021445]], [[-80.969464, 33.96971], [-80.9693, 33.969708], [-80.968808, 33.969697], [-80.968693, 33.969694], [-80.968694, 33.969728], [-80.968695, 33.969774], [-80.968696, 33.969816], [-80.968699, 33.970002], [-80.969464, 33.970012], [-80.969464, 33.96971]], [[-81.02, 33.984], [-81.019, 33.984], [-81.019, 33.985], [-81.02, 33.985], [-81.02, 33.984]], [[-81.028, 34.026], [-81.027, 34.026], [-81.027, 34.027], [-81.028, 34.027], [-81.028, 34.026]], [[-80.819865, 34.128243], [-80.819766, 34.128075], [-80.819673, 34.12792], [-80.819266, 34.128082], [-80.819361, 34.128234], [-80.819517, 34.128484], [-80.819594, 34.128452], [-80.819657, 34.128423], [-80.819789, 34.128368], [-80.81991, 34.128317], [-80.819865, 34.128243]], [[-81.05688, 34.044108], [-81.056854, 34.04407], [-81.056824, 34.044025], [-81.056807, 34.044], [-81.056774, 34.043925], [-81.056761, 34.043878], [-81.056748, 34.043827], [-81.056685, 34.043837], [-81.056497, 34.043867], [-81.056435, 34.043878], [-81.056393, 34.043884], [-81.056376, 34.043888], [-81.056339, 34.043897], [-81.056271, 34.043913], [-81.056329, 34.044127], [-81.056393, 34.044338], [-81.056729, 34.04419], [-81.056889, 34.044122], [-81.05688, 34.044108]], [[-80.817, 34.115], [-80.817, 34.114], [-80.816, 34.114], [-80.816, 34.115], [-80.817, 34.115]], [[-80.921706, 33.97204], [-80.921679, 33.971875], [-80.921043, 33.971944], [-80.921074, 33.971978], [-80.921155, 33.972063], [-80.921189, 33.972113], [-80.921223, 33.972154], [-80.921281, 33.972227], [-80.921365, 33.972334], [-80.921636, 33.972206], [-80.921721, 33.972166], [-80.921706, 33.97204]], [[-81.080266, 34.024702], [-81.080026, 34.025155], [-81.080095, 34.025178], [-81.080342, 34.025263], [-81.080486, 34.025002], [-81.080582, 34.024825], [-81.080266, 34.024702]], [[-81.054, 34.025], [-81.054, 34.026], [-81.055, 34.026], [-81.054, 34.025]], [[-81.065384, 34.012832], [-81.065279, 34.012752], [-81.064887, 34.013119], [-81.06486, 34.01315], [-81.064939, 34.013212], [-81.064984, 34.013248], [-81.065061, 34.013309], [-81.065078, 34.013323], [-81.065486, 34.01291], [-81.065384, 34.012832]], [[-80.954052, 33.974779], [-80.95358, 33.974852], [-80.953509, 33.974973], [-80.953358, 33.975228], [-80.953619, 33.975199], [-80.953981, 33.97494], [-80.953996, 33.974927], [-80.954052, 33.974779]], [[-81.043356, 34.050796], [-81.043303, 34.050634], [-81.042711, 34.050649], [-81.042801, 34.05092], [-81.043356, 34.050796]], [[-81.020675, 33.984893], [-81.020547, 33.984792], [-81.020487, 33.984746], [-81.020123, 33.985055], [-81.020185, 33.985103], [-81.020206, 33.98512], [-81.02023, 33.985138], [-81.020302, 33.985194], [-81.020326, 33.985213], [-81.020428, 33.985119], [-81.020675, 33.984893]], [[-80.986, 33.988], [-80.986, 33.987], [-80.985, 33.987], [-80.985, 33.988], [-80.986, 33.988]], [[-81.054069, 34.025187], [-81.053937, 34.025089], [-81.053564, 34.025455], [-81.053513, 34.02551], [-81.053619, 34.025587], [-81.053634, 34.025598], [-81.054069, 34.025187]], [[-81.062638, 34.027846], [-81.062585, 34.027804], [-81.062495, 34.027723], [-81.062436, 34.027661], [-81.06238, 34.027597], [-81.062361, 34.027571], [-81.062317, 34.027513], [-81.062153, 34.027569], [-81.062391, 34.027944], [-81.062514, 34.027898], [-81.062638, 34.027846]], [[-81.023105, 33.973605], [-81.022943, 33.973466], [-81.022618, 33.973714], [-81.022769, 33.973862], [-81.022781, 33.973852], [-81.022883, 33.973773], [-81.023085, 33.97362], [-81.023105, 33.973605]], [[-81.053444, 34.024739], [-81.052983, 34.025128], [-81.053113, 34.025222], [-81.053169, 34.025176], [-81.053545, 34.024812], [-81.053548, 34.024809], [-81.053444, 34.024739]], [[-80.96545, 33.981545], [-80.965165, 33.981731], [-80.964852, 33.981933], [-80.964941, 33.981984], [-80.965006, 33.981943], [-80.965544, 33.981606], [-80.96545, 33.981545]], [[-81.07206, 34.047913], [-81.072041, 34.047864], [-81.071999, 34.047803], [-81.071744, 34.047915], [-81.071859, 34.048077], [-81.07209, 34.047979], [-81.07206, 34.047913]], [[-81.009, 34.029], [-81.008, 34.029], [-81.009, 34.03], [-81.009, 34.029]], [[-81.066151, 34.013285], [-81.065887, 34.013098], [-81.065835, 34.013118], [-81.065822, 34.013161], [-81.066082, 34.013351], [-81.066151, 34.013285]], [[-81.022795, 34.034569], [-81.022755, 34.034569], [-81.022682, 34.034568], [-81.02266, 34.03475], [-81.02276, 34.034619], [-81.022795, 34.034569]]], [[[-80.97, 33.979], [-80.969, 33.979], [-80.969, 33.978], [-80.97, 33.978], [-80.97, 33.979]]]] } }] };
                geojson.features[0].geometry = JSON.parse(data);
                allow = true;
                LoadMapUpdate();
            },
            error: function () {

            }
        });
       
    }      
    
}

function BindDataByCounty() {
    var value = $('#q').val().trim();

    if (value != "") {
        $.ajax({
            url: '/ERSEA/GetgeometryByCounty',
            type: 'post',
            async: false,
            data: { 'County': value },
            success: function (data) {
                console.log(JSON.parse(data));
                geojson = { "type": "FeatureCollection", "features": [{ "type": "Feature", "properties": { "fillOpacity": 0, "strokeColor": "#FF0000", "strokeOpacity": 0.5 }, "geometry": { "type": "MultiPolygon", "coordinates": [[[[-80.997985, 34.046995], [-80.997463, 34.047383], [-80.997399, 34.047439], [-80.997334, 34.047363], [-80.9973, 34.047323], [-80.997261, 34.047214], [-80.997242, 34.047162], [-80.997216, 34.046987], [-80.997216, 34.046978], [-80.9973, 34.046978], [-80.997985, 34.046995]]], [[[-81.01, 34.066], [-81.009, 34.066], [-81.009, 34.067], [-81.008, 34.067], [-81.007, 34.065], [-81.008, 34.064], [-81.009, 34.064], [-81.01, 34.066]]], [[[-81.031, 34.061], [-81.03, 34.062], [-81.03, 34.061], [-81.031, 34.061]]], [[[-81.097, 34.034], [-81.097, 34.033], [-81.098, 34.033], [-81.098, 34.034], [-81.097, 34.034]]], [[[-81.142, 34.082], [-81.141, 34.083], [-81.141, 34.084], [-81.141, 34.085], [-81.14, 34.085], [-81.14, 34.084], [-81.139, 34.084], [-81.138, 34.084], [-81.138, 34.083], [-81.139, 34.083], [-81.139, 34.082], [-81.14, 34.082], [-81.142, 34.082]]], [[[-81.171, 34.091], [-81.17, 34.091], [-81.169, 34.091], [-81.168, 34.091], [-81.168, 34.092], [-81.168, 34.093], [-81.167, 34.093], [-81.166, 34.093], [-81.165, 34.093], [-81.166, 34.093], [-81.165, 34.094], [-81.165, 34.093], [-81.164, 34.093], [-81.164, 34.094], [-81.163, 34.094], [-81.163, 34.093], [-81.163, 34.092], [-81.162, 34.092], [-81.161, 34.092], [-81.16, 34.092], [-81.16, 34.093], [-81.161, 34.093], [-81.162, 34.094], [-81.163, 34.094], [-81.162, 34.094], [-81.162, 34.095], [-81.161, 34.094], [-81.16, 34.094], [-81.16, 34.095], [-81.16, 34.096], [-81.161, 34.095], [-81.161, 34.096], [-81.161, 34.097], [-81.162, 34.097], [-81.161, 34.098], [-81.162, 34.098], [-81.162, 34.097], [-81.163, 34.097], [-81.163, 34.099], [-81.163, 34.1], [-81.162, 34.102], [-81.162, 34.101], [-81.161, 34.101], [-81.16, 34.101], [-81.16, 34.1], [-81.16, 34.099], [-81.161, 34.099], [-81.162, 34.099], [-81.16, 34.099], [-81.16, 34.097], [-81.159, 34.097], [-81.159, 34.098], [-81.159, 34.097], [-81.158, 34.098], [-81.157, 34.098], [-81.156, 34.097], [-81.155, 34.097], [-81.155, 34.096], [-81.156, 34.097], [-81.157, 34.097], [-81.156, 34.096], [-81.156, 34.095], [-81.157, 34.096], [-81.157, 34.095], [-81.157, 34.094], [-81.156, 34.094], [-81.156, 34.093], [-81.156, 34.092], [-81.157, 34.092], [-81.157, 34.091], [-81.157, 34.09], [-81.158, 34.09], [-81.158, 34.089], [-81.157, 34.089], [-81.156, 34.089], [-81.155, 34.089], [-81.154, 34.089], [-81.153, 34.092], [-81.151, 34.093], [-81.15, 34.094], [-81.149, 34.095], [-81.148, 34.096], [-81.147, 34.096], [-81.147, 34.097], [-81.146, 34.098], [-81.145, 34.099], [-81.144, 34.1], [-81.142, 34.102], [-81.14, 34.103], [-81.14, 34.104], [-81.14, 34.105], [-81.14, 34.104], [-81.139, 34.104], [-81.139, 34.105], [-81.139, 34.106], [-81.138, 34.106], [-81.137, 34.106], [-81.138, 34.106], [-81.138, 34.107], [-81.138, 34.106], [-81.137, 34.106], [-81.137, 34.107], [-81.137, 34.108], [-81.136, 34.108], [-81.135, 34.108], [-81.135, 34.107], [-81.135, 34.108], [-81.136, 34.108], [-81.136, 34.109], [-81.135, 34.109], [-81.135, 34.11], [-81.134, 34.109], [-81.133, 34.109], [-81.131, 34.112], [-81.13, 34.112], [-81.13, 34.113], [-81.131, 34.113], [-81.131, 34.114], [-81.132, 34.114], [-81.132, 34.115], [-81.133, 34.115], [-81.133, 34.114], [-81.133, 34.113], [-81.134, 34.113], [-81.135, 34.113], [-81.135, 34.114], [-81.136, 34.114], [-81.138, 34.115], [-81.138, 34.116], [-81.139, 34.116], [-81.139, 34.117], [-81.139, 34.118], [-81.138, 34.118], [-81.137, 34.119], [-81.137, 34.12], [-81.137, 34.121], [-81.138, 34.121], [-81.138, 34.12], [-81.139, 34.12], [-81.14, 34.12], [-81.141, 34.12], [-81.142, 34.12], [-81.143, 34.12], [-81.144, 34.119], [-81.143, 34.119], [-81.142, 34.119], [-81.142, 34.118], [-81.143, 34.117], [-81.143, 34.116], [-81.143, 34.115], [-81.144, 34.115], [-81.144, 34.114], [-81.145, 34.114], [-81.146, 34.114], [-81.146, 34.115], [-81.146, 34.116], [-81.146, 34.117], [-81.146, 34.118], [-81.146, 34.119], [-81.145, 34.119], [-81.146, 34.119], [-81.146, 34.12], [-81.147, 34.121], [-81.147, 34.12], [-81.148, 34.12], [-81.149, 34.12], [-81.15, 34.12], [-81.15, 34.121], [-81.15, 34.122], [-81.15, 34.123], [-81.15, 34.122], [-81.151, 34.123], [-81.151, 34.124], [-81.152, 34.125], [-81.153, 34.124], [-81.154, 34.123], [-81.154, 34.124], [-81.155, 34.125], [-81.155, 34.126], [-81.154, 34.126], [-81.153, 34.126], [-81.153, 34.127], [-81.152, 34.127], [-81.151, 34.127], [-81.152, 34.128], [-81.152, 34.129], [-81.153, 34.13], [-81.151, 34.131], [-81.149, 34.131], [-81.149, 34.132], [-81.147, 34.132], [-81.146, 34.132], [-81.145, 34.132], [-81.144, 34.132], [-81.142, 34.132], [-81.139, 34.133], [-81.138, 34.133], [-81.137, 34.133], [-81.137, 34.132], [-81.136, 34.132], [-81.136, 34.131], [-81.136, 34.13], [-81.135, 34.13], [-81.135, 34.129], [-81.135, 34.128], [-81.134, 34.128], [-81.134, 34.127], [-81.133, 34.127], [-81.133, 34.126], [-81.132, 34.126], [-81.132, 34.125], [-81.131, 34.125], [-81.132, 34.125], [-81.133, 34.124], [-81.135, 34.123], [-81.137, 34.121], [-81.137, 34.12], [-81.137, 34.119], [-81.137, 34.118], [-81.138, 34.118], [-81.138, 34.117], [-81.139, 34.117], [-81.139, 34.116], [-81.138, 34.116], [-81.138, 34.115], [-81.137, 34.115], [-81.136, 34.114], [-81.135, 34.113], [-81.134, 34.113], [-81.134, 34.114], [-81.133, 34.115], [-81.132, 34.115], [-81.132, 34.114], [-81.131, 34.114], [-81.131, 34.113], [-81.13, 34.113], [-81.129, 34.113], [-81.129, 34.112], [-81.128, 34.112], [-81.128, 34.111], [-81.127, 34.111], [-81.126, 34.112], [-81.125, 34.112], [-81.124, 34.112], [-81.123, 34.112], [-81.123, 34.111], [-81.122, 34.111], [-81.122, 34.112], [-81.121, 34.112], [-81.12, 34.112], [-81.12, 34.113], [-81.12, 34.112], [-81.119, 34.112], [-81.119, 34.111], [-81.119, 34.11], [-81.119, 34.109], [-81.119, 34.108], [-81.118, 34.108], [-81.118, 34.107], [-81.118, 34.106], [-81.117, 34.106], [-81.117, 34.105], [-81.116, 34.105], [-81.115, 34.105], [-81.114, 34.104], [-81.113, 34.104], [-81.112, 34.104], [-81.112, 34.103], [-81.111, 34.103], [-81.11, 34.102], [-81.11, 34.101], [-81.109, 34.101], [-81.109, 34.1], [-81.108, 34.099], [-81.108, 34.098], [-81.108, 34.097], [-81.107, 34.097], [-81.107, 34.096], [-81.107, 34.095], [-81.106, 34.095], [-81.106, 34.094], [-81.106, 34.093], [-81.105, 34.092], [-81.105, 34.091], [-81.104, 34.091], [-81.104, 34.09], [-81.103, 34.089], [-81.102, 34.089], [-81.102, 34.088], [-81.101, 34.088], [-81.1, 34.088], [-81.1, 34.087], [-81.099, 34.087], [-81.098, 34.087], [-81.098, 34.086], [-81.097, 34.086], [-81.097, 34.085], [-81.096, 34.085], [-81.095, 34.085], [-81.095, 34.084], [-81.094, 34.084], [-81.094, 34.083], [-81.093, 34.083], [-81.093, 34.082], [-81.092, 34.082], [-81.091, 34.081], [-81.09, 34.081], [-81.09, 34.08], [-81.089, 34.08], [-81.088, 34.08], [-81.088, 34.079], [-81.088, 34.078], [-81.087, 34.078], [-81.087, 34.077], [-81.087, 34.076], [-81.086, 34.076], [-81.086, 34.075], [-81.086, 34.074], [-81.085, 34.074], [-81.085, 34.073], [-81.084, 34.073], [-81.083, 34.073], [-81.082, 34.073], [-81.076, 34.078], [-81.075, 34.079], [-81.074, 34.079], [-81.072, 34.08], [-81.071, 34.081], [-81.071, 34.082], [-81.07, 34.081], [-81.069, 34.081], [-81.068, 34.081], [-81.069, 34.081], [-81.069, 34.08], [-81.068, 34.08], [-81.067, 34.079], [-81.067, 34.078], [-81.066, 34.078], [-81.068, 34.077], [-81.067, 34.075], [-81.066, 34.074], [-81.068, 34.072], [-81.067, 34.072], [-81.067, 34.071], [-81.068, 34.072], [-81.069, 34.071], [-81.071, 34.069], [-81.073, 34.068], [-81.072, 34.068], [-81.073, 34.068], [-81.073, 34.067], [-81.073, 34.068], [-81.076, 34.065], [-81.078, 34.064], [-81.078, 34.063], [-81.077, 34.063], [-81.077, 34.062], [-81.077, 34.061], [-81.076, 34.061], [-81.076, 34.06], [-81.076, 34.059], [-81.075, 34.059], [-81.075, 34.058], [-81.075, 34.057], [-81.075, 34.056], [-81.074, 34.056], [-81.074, 34.055], [-81.074, 34.054], [-81.074, 34.053], [-81.074, 34.052], [-81.073, 34.052], [-81.073, 34.051], [-81.073, 34.05], [-81.073, 34.049], [-81.072, 34.048], [-81.072, 34.049], [-81.071, 34.049], [-81.069, 34.05], [-81.069, 34.049], [-81.068, 34.049], [-81.068, 34.05], [-81.068, 34.048], [-81.068, 34.047], [-81.069, 34.046], [-81.069, 34.045], [-81.069, 34.044], [-81.068, 34.042], [-81.068, 34.041], [-81.068, 34.039], [-81.067, 34.039], [-81.066, 34.039], [-81.067, 34.039], [-81.067, 34.038], [-81.068, 34.038], [-81.068, 34.037], [-81.068, 34.035], [-81.067, 34.034], [-81.067, 34.033], [-81.067, 34.032], [-81.068, 34.032], [-81.067, 34.032], [-81.066, 34.033], [-81.066, 34.034], [-81.065, 34.034], [-81.064, 34.035], [-81.064, 34.034], [-81.062, 34.033], [-81.062, 34.032], [-81.063, 34.032], [-81.064, 34.032], [-81.065, 34.031], [-81.064, 34.03], [-81.065, 34.03], [-81.064, 34.029], [-81.063, 34.029], [-81.062, 34.029], [-81.062, 34.03], [-81.062, 34.029], [-81.062, 34.028], [-81.061, 34.028], [-81.061, 34.027], [-81.06, 34.028], [-81.059, 34.028], [-81.058, 34.029], [-81.057, 34.029], [-81.057, 34.03], [-81.058, 34.03], [-81.059, 34.03], [-81.06, 34.029], [-81.059, 34.03], [-81.06, 34.03], [-81.06, 34.031], [-81.059, 34.032], [-81.059, 34.033], [-81.059, 34.034], [-81.06, 34.034], [-81.06, 34.035], [-81.059, 34.035], [-81.059, 34.036], [-81.059, 34.037], [-81.06, 34.036], [-81.06, 34.037], [-81.06, 34.038], [-81.059, 34.038], [-81.059, 34.039], [-81.059, 34.04], [-81.058, 34.041], [-81.058, 34.042], [-81.06, 34.042], [-81.061, 34.042], [-81.061, 34.041], [-81.062, 34.041], [-81.062, 34.042], [-81.062, 34.043], [-81.062, 34.044], [-81.061, 34.045], [-81.062, 34.045], [-81.062, 34.046], [-81.061, 34.046], [-81.058, 34.045], [-81.057, 34.045], [-81.057, 34.044], [-81.058, 34.044], [-81.057, 34.044], [-81.057, 34.045], [-81.056, 34.045], [-81.056, 34.044], [-81.056, 34.045], [-81.055, 34.045], [-81.055, 34.046], [-81.054, 34.046], [-81.054, 34.047], [-81.053, 34.047], [-81.052, 34.047], [-81.052, 34.048], [-81.053, 34.048], [-81.052, 34.048], [-81.051, 34.048], [-81.051, 34.049], [-81.05, 34.049], [-81.049, 34.049], [-81.048, 34.05], [-81.048, 34.051], [-81.047, 34.051], [-81.047, 34.05], [-81.046, 34.05], [-81.045, 34.05], [-81.045, 34.051], [-81.045, 34.05], [-81.045, 34.051], [-81.044, 34.051], [-81.043, 34.051], [-81.044, 34.051], [-81.043, 34.051], [-81.042, 34.051], [-81.041, 34.051], [-81.04, 34.051], [-81.039, 34.051], [-81.039, 34.05], [-81.038, 34.05], [-81.037, 34.05], [-81.036, 34.05], [-81.037, 34.05], [-81.037, 34.051], [-81.036, 34.051], [-81.037, 34.052], [-81.036, 34.052], [-81.036, 34.053], [-81.036, 34.054], [-81.036, 34.053], [-81.037, 34.054], [-81.036, 34.054], [-81.037, 34.055], [-81.037, 34.056], [-81.038, 34.056], [-81.039, 34.056], [-81.039, 34.057], [-81.038, 34.057], [-81.039, 34.057], [-81.039, 34.058], [-81.04, 34.058], [-81.04, 34.057], [-81.04, 34.058], [-81.041, 34.058], [-81.042, 34.058], [-81.042, 34.057], [-81.042, 34.058], [-81.042, 34.059], [-81.043, 34.059], [-81.043, 34.06], [-81.044, 34.059], [-81.044, 34.06], [-81.044, 34.061], [-81.045, 34.061], [-81.045, 34.062], [-81.046, 34.062], [-81.046, 34.063], [-81.047, 34.063], [-81.047, 34.064], [-81.048, 34.064], [-81.049, 34.064], [-81.049, 34.065], [-81.048, 34.065], [-81.048, 34.066], [-81.047, 34.066], [-81.046, 34.066], [-81.045, 34.065], [-81.044, 34.065], [-81.044, 34.064], [-81.044, 34.063], [-81.043, 34.063], [-81.042, 34.064], [-81.042, 34.063], [-81.041, 34.063], [-81.04, 34.064], [-81.039, 34.062], [-81.036, 34.062], [-81.035, 34.062], [-81.034, 34.063], [-81.033, 34.063], [-81.032, 34.063], [-81.031, 34.063], [-81.032, 34.061], [-81.033, 34.062], [-81.034, 34.062], [-81.036, 34.062], [-81.036, 34.061], [-81.036, 34.06], [-81.036, 34.059], [-81.035, 34.059], [-81.034, 34.059], [-81.033, 34.059], [-81.033, 34.058], [-81.032, 34.059], [-81.031, 34.059], [-81.03, 34.059], [-81.03, 34.06], [-81.028, 34.06], [-81.028, 34.061], [-81.027, 34.061], [-81.029, 34.061], [-81.03, 34.061], [-81.03, 34.062], [-81.028, 34.064], [-81.029, 34.064], [-81.03, 34.064], [-81.03, 34.063], [-81.03, 34.064], [-81.031, 34.064], [-81.033, 34.064], [-81.034, 34.064], [-81.033, 34.064], [-81.034, 34.065], [-81.034, 34.064], [-81.035, 34.064], [-81.036, 34.064], [-81.037, 34.064], [-81.037, 34.065], [-81.037, 34.066], [-81.037, 34.067], [-81.038, 34.067], [-81.038, 34.068], [-81.038, 34.069], [-81.039, 34.072], [-81.042, 34.072], [-81.043, 34.072], [-81.044, 34.072], [-81.043, 34.072], [-81.043, 34.073], [-81.042, 34.073], [-81.041, 34.073], [-81.041, 34.074], [-81.04, 34.074], [-81.041, 34.074], [-81.041, 34.075], [-81.04, 34.075], [-81.04, 34.076], [-81.039, 34.076], [-81.039, 34.077], [-81.038, 34.077], [-81.037, 34.077], [-81.037, 34.078], [-81.036, 34.077], [-81.036, 34.07], [-81.036, 34.068], [-81.035, 34.067], [-81.035, 34.066], [-81.034, 34.066], [-81.033, 34.066], [-81.032, 34.066], [-81.031, 34.066], [-81.029, 34.066], [-81.028, 34.066], [-81.027, 34.066], [-81.026, 34.066], [-81.025, 34.066], [-81.024, 34.066], [-81.023, 34.067], [-81.022, 34.067], [-81.022, 34.066], [-81.023, 34.065], [-81.023, 34.064], [-81.023, 34.063], [-81.024, 34.063], [-81.025, 34.063], [-81.025, 34.062], [-81.025, 34.061], [-81.026, 34.061], [-81.025, 34.06], [-81.024, 34.061], [-81.024, 34.062], [-81.023, 34.062], [-81.022, 34.061], [-81.022, 34.06], [-81.021, 34.06], [-81.021, 34.059], [-81.02, 34.059], [-81.019, 34.059], [-81.02, 34.058], [-81.019, 34.058], [-81.018, 34.058], [-81.018, 34.059], [-81.017, 34.058], [-81.017, 34.059], [-81.017, 34.058], [-81.016, 34.059], [-81.016, 34.06], [-81.015, 34.06], [-81.016, 34.059], [-81.016, 34.058], [-81.015, 34.058], [-81.015, 34.059], [-81.015, 34.058], [-81.016, 34.058], [-81.015, 34.058], [-81.015, 34.057], [-81.014, 34.057], [-81.014, 34.058], [-81.013, 34.058], [-81.012, 34.058], [-81.011, 34.059], [-81.01, 34.059], [-81.01, 34.06], [-81.009, 34.061], [-81.01, 34.062], [-81.01, 34.063], [-81.009, 34.062], [-81.009, 34.063], [-81.008, 34.064], [-81.007, 34.065], [-81.007, 34.066], [-81.006, 34.067], [-81.006, 34.068], [-81.005, 34.068], [-81.005, 34.069], [-81.004, 34.069], [-81.003, 34.07], [-81.002, 34.07], [-81.003, 34.071], [-81.004, 34.071], [-81.005, 34.071], [-81.004, 34.072], [-81.005, 34.073], [-81.006, 34.074], [-81.005, 34.074], [-81.004, 34.074], [-81.003, 34.074], [-81.002, 34.074], [-81.001, 34.074], [-81, 34.074], [-81, 34.075], [-81, 34.076], [-80.999, 34.075], [-80.999, 34.076], [-80.998, 34.076], [-80.998, 34.077], [-80.996, 34.077], [-80.995, 34.077], [-80.996, 34.077], [-80.996, 34.078], [-80.993, 34.078], [-80.992, 34.078], [-80.992, 34.082], [-80.993, 34.082], [-80.993, 34.081], [-80.994, 34.081], [-80.995, 34.081], [-80.995, 34.08], [-80.996, 34.08], [-80.997, 34.08], [-80.996, 34.082], [-80.996, 34.083], [-80.996, 34.084], [-80.996, 34.085], [-80.996, 34.086], [-80.992, 34.085], [-80.99, 34.086], [-80.989, 34.086], [-80.988, 34.087], [-80.987, 34.088], [-80.99, 34.089], [-80.991, 34.09], [-80.99, 34.093], [-80.991, 34.094], [-80.993, 34.091], [-80.994, 34.092], [-80.994, 34.093], [-80.993, 34.094], [-80.993, 34.095], [-80.992, 34.097], [-80.992, 34.098], [-80.991, 34.099], [-80.987, 34.096], [-80.984, 34.099], [-80.982, 34.101], [-80.983, 34.101], [-80.981, 34.106], [-80.979, 34.107], [-80.98, 34.105], [-80.979, 34.104], [-80.977, 34.105], [-80.976, 34.106], [-80.974, 34.104], [-80.975, 34.1], [-80.973, 34.099], [-80.971, 34.099], [-80.971, 34.098], [-80.972, 34.097], [-80.972, 34.096], [-80.973, 34.097], [-80.973, 34.096], [-80.972, 34.095], [-80.97, 34.094], [-80.97, 34.093], [-80.969, 34.093], [-80.968, 34.094], [-80.967, 34.094], [-80.966, 34.094], [-80.965, 34.094], [-80.964, 34.095], [-80.963, 34.095], [-80.963, 34.094], [-80.963, 34.095], [-80.962, 34.095], [-80.961, 34.095], [-80.961, 34.094], [-80.961, 34.092], [-80.96, 34.091], [-80.96, 34.09], [-80.96, 34.089], [-80.961, 34.088], [-80.961, 34.087], [-80.962, 34.085], [-80.963, 34.084], [-80.964, 34.084], [-80.965, 34.083], [-80.966, 34.084], [-80.966, 34.085], [-80.967, 34.085], [-80.968, 34.085], [-80.97, 34.085], [-80.969, 34.085], [-80.969, 34.084], [-80.97, 34.084], [-80.971, 34.087], [-80.971, 34.088], [-80.972, 34.089], [-80.973, 34.088], [-80.974, 34.088], [-80.975, 34.087], [-80.975, 34.086], [-80.976, 34.084], [-80.976, 34.083], [-80.976, 34.082], [-80.977, 34.081], [-80.977, 34.08], [-80.978, 34.079], [-80.978, 34.078], [-80.978, 34.077], [-80.978, 34.076], [-80.979, 34.076], [-80.979, 34.075], [-80.98, 34.074], [-80.98, 34.073], [-80.984, 34.073], [-80.986, 34.073], [-80.985, 34.071], [-80.983, 34.072], [-80.984, 34.071], [-80.984, 34.07], [-80.983, 34.071], [-80.981, 34.069], [-80.981, 34.068], [-80.982, 34.068], [-80.982, 34.067], [-80.983, 34.067], [-80.984, 34.066], [-80.985, 34.065], [-80.987, 34.064], [-80.991, 34.061], [-80.992, 34.061], [-80.993, 34.06], [-80.995, 34.06], [-80.995, 34.059], [-80.996, 34.058], [-80.997, 34.059], [-80.999, 34.058], [-81, 34.057], [-81.001, 34.057], [-81.002, 34.056], [-81.002, 34.055], [-81.002, 34.056], [-81.001, 34.056], [-81, 34.056], [-80.999, 34.056], [-80.998, 34.056], [-80.997, 34.056], [-80.996, 34.056], [-80.995, 34.056], [-80.994, 34.056], [-80.993, 34.057], [-80.992, 34.057], [-80.992, 34.056], [-80.993, 34.056], [-80.992, 34.056], [-80.992, 34.055], [-80.992, 34.054], [-80.993, 34.054], [-80.992, 34.054], [-80.991, 34.053], [-80.992, 34.053], [-80.993, 34.053], [-80.994, 34.052], [-80.995, 34.052], [-80.995, 34.051], [-80.996, 34.051], [-80.997, 34.051], [-80.998, 34.05], [-81, 34.05], [-81.001, 34.05], [-80.999, 34.048], [-80.998, 34.047], [-80.997, 34.046], [-80.997, 34.047], [-80.997, 34.046], [-80.998, 34.046], [-80.996, 34.044], [-80.995, 34.044], [-80.995, 34.043], [-80.994, 34.043], [-80.992, 34.043], [-80.993, 34.043], [-80.994, 34.042], [-80.995, 34.042], [-80.996, 34.042], [-80.996, 34.041], [-80.997, 34.041], [-80.998, 34.041], [-80.998, 34.04], [-80.999, 34.04], [-81, 34.04], [-80.999, 34.039], [-80.999, 34.04], [-80.998, 34.04], [-80.998, 34.039], [-80.997, 34.039], [-80.996, 34.039], [-80.995, 34.039], [-80.995, 34.038], [-80.994, 34.038], [-80.993, 34.038], [-80.994, 34.039], [-80.993, 34.039], [-80.994, 34.039], [-80.993, 34.039], [-80.993, 34.04], [-80.993, 34.041], [-80.992, 34.041], [-80.991, 34.04], [-80.99, 34.04], [-80.99, 34.039], [-80.989, 34.039], [-80.988, 34.039], [-80.989, 34.039], [-80.989, 34.038], [-80.99, 34.038], [-80.991, 34.039], [-80.992, 34.037], [-80.991, 34.037], [-80.99, 34.037], [-80.989, 34.037], [-80.989, 34.036], [-80.988, 34.036], [-80.988, 34.037], [-80.987, 34.037], [-80.986, 34.037], [-80.986, 34.036], [-80.985, 34.036], [-80.984, 34.037], [-80.983, 34.037], [-80.983, 34.038], [-80.983, 34.037], [-80.982, 34.038], [-80.981, 34.038], [-80.981, 34.037], [-80.98, 34.037], [-80.98, 34.036], [-80.981, 34.036], [-80.982, 34.036], [-80.983, 34.036], [-80.982, 34.035], [-80.983, 34.033], [-80.982, 34.033], [-80.983, 34.032], [-80.982, 34.032], [-80.982, 34.031], [-80.982, 34.032], [-80.983, 34.032], [-80.984, 34.032], [-80.984, 34.033], [-80.985, 34.033], [-80.986, 34.033], [-80.986, 34.03], [-80.985, 34.03], [-80.985, 34.029], [-80.985, 34.028], [-80.986, 34.028], [-80.987, 34.028], [-80.987, 34.027], [-80.988, 34.027], [-80.989, 34.026], [-80.99, 34.026], [-80.991, 34.026], [-80.992, 34.026], [-80.993, 34.025], [-80.994, 34.025], [-80.995, 34.025], [-80.995, 34.024], [-80.995, 34.023], [-80.995, 34.021], [-80.995, 34.02], [-80.995, 34.019], [-80.995, 34.018], [-80.995, 34.017], [-80.995, 34.016], [-80.995, 34.015], [-80.995, 34.014], [-80.995, 34.013], [-80.995, 34.012], [-80.995, 34.01], [-80.994, 34.01], [-80.993, 34.011], [-80.992, 34.011], [-80.991, 34.012], [-80.99, 34.012], [-80.989, 34.012], [-80.988, 34.012], [-80.988, 34.013], [-80.987, 34.013], [-80.986, 34.013], [-80.985, 34.013], [-80.984, 34.013], [-80.983, 34.013], [-80.982, 34.013], [-80.981, 34.014], [-80.98, 34.014], [-80.979, 34.014], [-80.978, 34.014], [-80.977, 34.014], [-80.976, 34.014], [-80.975, 34.014], [-80.975, 34.013], [-80.974, 34.013], [-80.973, 34.013], [-80.972, 34.012], [-80.971, 34.012], [-80.97, 34.012], [-80.969, 34.011], [-80.968, 34.011], [-80.967, 34.011], [-80.966, 34.011], [-80.965, 34.011], [-80.964, 34.011], [-80.963, 34.011], [-80.963, 34.012], [-80.962, 34.012], [-80.961, 34.012], [-80.961, 34.013], [-80.96, 34.013], [-80.96, 34.015], [-80.96, 34.016], [-80.96, 34.018], [-80.96, 34.019], [-80.96, 34.02], [-80.96, 34.021], [-80.961, 34.021], [-80.96, 34.021], [-80.959, 34.022], [-80.958, 34.022], [-80.958, 34.023], [-80.957, 34.023], [-80.956, 34.023], [-80.955, 34.023], [-80.954, 34.023], [-80.953, 34.024], [-80.952, 34.024], [-80.952, 34.025], [-80.951, 34.025], [-80.951, 34.024], [-80.95, 34.024], [-80.949, 34.024], [-80.948, 34.024], [-80.946, 34.024], [-80.945, 34.025], [-80.944, 34.026], [-80.945, 34.026], [-80.946, 34.027], [-80.947, 34.027], [-80.947, 34.028], [-80.946, 34.029], [-80.945, 34.029], [-80.944, 34.03], [-80.943, 34.03], [-80.944, 34.03], [-80.944, 34.031], [-80.944, 34.03], [-80.943, 34.03], [-80.943, 34.031], [-80.943, 34.032], [-80.942, 34.032], [-80.941, 34.031], [-80.941, 34.032], [-80.94, 34.032], [-80.94, 34.033], [-80.939, 34.034], [-80.939, 34.035], [-80.939, 34.036], [-80.939, 34.037], [-80.94, 34.037], [-80.94, 34.036], [-80.94, 34.037], [-80.941, 34.037], [-80.941, 34.038], [-80.94, 34.038], [-80.939, 34.038], [-80.939, 34.039], [-80.939, 34.04], [-80.938, 34.041], [-80.938, 34.042], [-80.937, 34.042], [-80.937, 34.043], [-80.936, 34.043], [-80.936, 34.044], [-80.935, 34.044], [-80.934, 34.044], [-80.934, 34.045], [-80.933, 34.045], [-80.931, 34.047], [-80.93, 34.047], [-80.93, 34.048], [-80.929, 34.048], [-80.928, 34.049], [-80.926, 34.05], [-80.925, 34.051], [-80.924, 34.051], [-80.924, 34.052], [-80.923, 34.052], [-80.922, 34.053], [-80.921, 34.053], [-80.921, 34.054], [-80.92, 34.054], [-80.919, 34.054], [-80.919, 34.055], [-80.92, 34.055], [-80.92, 34.056], [-80.919, 34.057], [-80.919, 34.056], [-80.918, 34.057], [-80.918, 34.056], [-80.917, 34.056], [-80.915, 34.058], [-80.914, 34.058], [-80.913, 34.059], [-80.912, 34.059], [-80.911, 34.06], [-80.91, 34.061], [-80.909, 34.062], [-80.908, 34.062], [-80.909, 34.062], [-80.908, 34.062], [-80.907, 34.063], [-80.906, 34.063], [-80.906, 34.064], [-80.907, 34.064], [-80.906, 34.064], [-80.905, 34.064], [-80.904, 34.065], [-80.903, 34.065], [-80.903, 34.066], [-80.904, 34.066], [-80.904, 34.067], [-80.905, 34.067], [-80.907, 34.067], [-80.906, 34.068], [-80.903, 34.07], [-80.902, 34.07], [-80.901, 34.07], [-80.902, 34.07], [-80.902, 34.069], [-80.903, 34.068], [-80.902, 34.068], [-80.901, 34.067], [-80.9, 34.067], [-80.9, 34.068], [-80.899, 34.068], [-80.898, 34.069], [-80.897, 34.07], [-80.896, 34.07], [-80.895, 34.071], [-80.894, 34.072], [-80.893, 34.072], [-80.892, 34.073], [-80.891, 34.074], [-80.89, 34.074], [-80.889, 34.075], [-80.888, 34.076], [-80.887, 34.076], [-80.886, 34.077], [-80.885, 34.077], [-80.885, 34.078], [-80.884, 34.078], [-80.883, 34.079], [-80.882, 34.079], [-80.881, 34.08], [-80.882, 34.08], [-80.882, 34.081], [-80.883, 34.081], [-80.883, 34.08], [-80.882, 34.08], [-80.883, 34.08], [-80.884, 34.079], [-80.885, 34.079], [-80.886, 34.079], [-80.886, 34.078], [-80.887, 34.078], [-80.887, 34.077], [-80.888, 34.077], [-80.888, 34.076], [-80.889, 34.076], [-80.89, 34.076], [-80.891, 34.075], [-80.891, 34.076], [-80.891, 34.077], [-80.891, 34.078], [-80.892, 34.078], [-80.892, 34.079], [-80.892, 34.08], [-80.885, 34.083], [-80.882, 34.084], [-80.879, 34.085], [-80.876, 34.082], [-80.875, 34.082], [-80.874, 34.082], [-80.874, 34.083], [-80.875, 34.083], [-80.876, 34.084], [-80.877, 34.086], [-80.876, 34.087], [-80.876, 34.088], [-80.877, 34.088], [-80.876, 34.088], [-80.876, 34.089], [-80.879, 34.089], [-80.88, 34.091], [-80.881, 34.092], [-80.881, 34.093], [-80.88, 34.093], [-80.879, 34.094], [-80.878, 34.094], [-80.878, 34.095], [-80.877, 34.095], [-80.876, 34.096], [-80.875, 34.096], [-80.875, 34.097], [-80.874, 34.097], [-80.873, 34.098], [-80.872, 34.098], [-80.872, 34.099], [-80.871, 34.099], [-80.87, 34.099], [-80.869, 34.1], [-80.868, 34.099], [-80.868, 34.098], [-80.868, 34.097], [-80.869, 34.097], [-80.871, 34.096], [-80.872, 34.096], [-80.869, 34.094], [-80.868, 34.094], [-80.868, 34.093], [-80.867, 34.093], [-80.867, 34.092], [-80.866, 34.092], [-80.864, 34.094], [-80.863, 34.094], [-80.861, 34.094], [-80.86, 34.095], [-80.859, 34.094], [-80.859, 34.095], [-80.858, 34.094], [-80.858, 34.093], [-80.857, 34.092], [-80.856, 34.092], [-80.855, 34.092], [-80.856, 34.092], [-80.856, 34.093], [-80.856, 34.094], [-80.857, 34.095], [-80.857, 34.096], [-80.857, 34.097], [-80.856, 34.097], [-80.856, 34.096], [-80.856, 34.095], [-80.854, 34.096], [-80.855, 34.096], [-80.855, 34.097], [-80.854, 34.097], [-80.853, 34.097], [-80.852, 34.097], [-80.852, 34.098], [-80.851, 34.098], [-80.85, 34.099], [-80.85, 34.1], [-80.849, 34.1], [-80.848, 34.1], [-80.847, 34.1], [-80.847, 34.101], [-80.847, 34.102], [-80.844, 34.101], [-80.841, 34.101], [-80.846, 34.099], [-80.849, 34.098], [-80.85, 34.097], [-80.851, 34.097], [-80.851, 34.096], [-80.85, 34.094], [-80.851, 34.095], [-80.852, 34.094], [-80.851, 34.094], [-80.85, 34.094], [-80.849, 34.093], [-80.849, 34.092], [-80.846, 34.094], [-80.844, 34.094], [-80.833, 34.099], [-80.826, 34.102], [-80.823, 34.103], [-80.822, 34.103], [-80.821, 34.104], [-80.821, 34.105], [-80.82, 34.106], [-80.82, 34.107], [-80.821, 34.107], [-80.824, 34.106], [-80.824, 34.107], [-80.826, 34.106], [-80.827, 34.106], [-80.828, 34.106], [-80.829, 34.106], [-80.83, 34.106], [-80.831, 34.106], [-80.831, 34.107], [-80.832, 34.107], [-80.832, 34.106], [-80.833, 34.107], [-80.834, 34.107], [-80.835, 34.107], [-80.835, 34.106], [-80.836, 34.106], [-80.837, 34.105], [-80.837, 34.106], [-80.837, 34.107], [-80.837, 34.108], [-80.836, 34.108], [-80.835, 34.108], [-80.836, 34.109], [-80.837, 34.109], [-80.836, 34.11], [-80.837, 34.11], [-80.837, 34.111], [-80.838, 34.111], [-80.839, 34.112], [-80.841, 34.11], [-80.843, 34.109], [-80.844, 34.108], [-80.845, 34.108], [-80.846, 34.108], [-80.847, 34.108], [-80.847, 34.109], [-80.847, 34.11], [-80.846, 34.111], [-80.847, 34.112], [-80.847, 34.113], [-80.847, 34.114], [-80.847, 34.115], [-80.846, 34.115], [-80.845, 34.115], [-80.845, 34.116], [-80.846, 34.116], [-80.846, 34.117], [-80.846, 34.118], [-80.847, 34.118], [-80.847, 34.119], [-80.847, 34.12], [-80.848, 34.12], [-80.847, 34.12], [-80.846, 34.12], [-80.844, 34.12], [-80.844, 34.119], [-80.843, 34.119], [-80.842, 34.119], [-80.841, 34.119], [-80.84, 34.119], [-80.839, 34.119], [-80.838, 34.119], [-80.838, 34.12], [-80.837, 34.12], [-80.837, 34.121], [-80.836, 34.121], [-80.837, 34.121], [-80.836, 34.121], [-80.835, 34.121], [-80.835, 34.122], [-80.834, 34.122], [-80.835, 34.122], [-80.835, 34.123], [-80.836, 34.123], [-80.835, 34.123], [-80.836, 34.123], [-80.835, 34.123], [-80.834, 34.123], [-80.833, 34.123], [-80.833, 34.122], [-80.834, 34.122], [-80.834, 34.121], [-80.833, 34.121], [-80.832, 34.122], [-80.832, 34.123], [-80.832, 34.124], [-80.833, 34.124], [-80.833, 34.125], [-80.834, 34.125], [-80.834, 34.124], [-80.835, 34.124], [-80.836, 34.124], [-80.836, 34.125], [-80.835, 34.125], [-80.835, 34.126], [-80.834, 34.127], [-80.833, 34.128], [-80.832, 34.129], [-80.831, 34.129], [-80.83, 34.129], [-80.829, 34.13], [-80.829, 34.129], [-80.828, 34.129], [-80.829, 34.128], [-80.828, 34.128], [-80.828, 34.127], [-80.827, 34.126], [-80.827, 34.125], [-80.826, 34.125], [-80.825, 34.125], [-80.826, 34.125], [-80.826, 34.124], [-80.827, 34.124], [-80.828, 34.125], [-80.829, 34.125], [-80.829, 34.126], [-80.83, 34.126], [-80.831, 34.125], [-80.832, 34.124], [-80.832, 34.123], [-80.832, 34.122], [-80.831, 34.122], [-80.831, 34.121], [-80.83, 34.121], [-80.831, 34.121], [-80.831, 34.12], [-80.83, 34.121], [-80.829, 34.12], [-80.829, 34.119], [-80.828, 34.119], [-80.828, 34.118], [-80.828, 34.117], [-80.829, 34.117], [-80.828, 34.117], [-80.828, 34.116], [-80.827, 34.116], [-80.827, 34.115], [-80.826, 34.115], [-80.825, 34.115], [-80.824, 34.115], [-80.823, 34.114], [-80.822, 34.114], [-80.822, 34.113], [-80.821, 34.113], [-80.82, 34.113], [-80.819, 34.112], [-80.818, 34.112], [-80.817, 34.112], [-80.818, 34.112], [-80.818, 34.113], [-80.819, 34.113], [-80.819, 34.112], [-80.819, 34.113], [-80.82, 34.113], [-80.821, 34.114], [-80.82, 34.114], [-80.82, 34.115], [-80.821, 34.116], [-80.822, 34.116], [-80.823, 34.117], [-80.824, 34.117], [-80.825, 34.117], [-80.825, 34.118], [-80.826, 34.118], [-80.827, 34.118], [-80.827, 34.119], [-80.828, 34.119], [-80.828, 34.12], [-80.83, 34.122], [-80.829, 34.122], [-80.828, 34.122], [-80.827, 34.122], [-80.826, 34.122], [-80.825, 34.121], [-80.824, 34.121], [-80.823, 34.121], [-80.821, 34.121], [-80.819, 34.121], [-80.818, 34.121], [-80.817, 34.121], [-80.817, 34.122], [-80.817, 34.123], [-80.818, 34.123], [-80.819, 34.123], [-80.82, 34.123], [-80.821, 34.123], [-80.821, 34.124], [-80.821, 34.125], [-80.822, 34.125], [-80.822, 34.124], [-80.822, 34.125], [-80.823, 34.125], [-80.823, 34.126], [-80.822, 34.127], [-80.822, 34.128], [-80.822, 34.129], [-80.821, 34.129], [-80.82, 34.13], [-80.819, 34.131], [-80.82, 34.131], [-80.819, 34.131], [-80.818, 34.131], [-80.818, 34.13], [-80.817, 34.13], [-80.817, 34.129], [-80.816, 34.129], [-80.816, 34.128], [-80.817, 34.128], [-80.817, 34.129], [-80.817, 34.128], [-80.818, 34.129], [-80.818, 34.128], [-80.819, 34.128], [-80.82, 34.128], [-80.819, 34.128], [-80.819, 34.127], [-80.82, 34.127], [-80.82, 34.128], [-80.82, 34.127], [-80.821, 34.127], [-80.82, 34.127], [-80.821, 34.127], [-80.821, 34.126], [-80.82, 34.126], [-80.82, 34.125], [-80.819, 34.125], [-80.819, 34.124], [-80.818, 34.124], [-80.818, 34.123], [-80.817, 34.123], [-80.817, 34.122], [-80.817, 34.121], [-80.816, 34.121], [-80.816, 34.122], [-80.815, 34.122], [-80.814, 34.122], [-80.814, 34.121], [-80.815, 34.121], [-80.815, 34.12], [-80.816, 34.12], [-80.816, 34.119], [-80.815, 34.119], [-80.816, 34.119], [-80.815, 34.118], [-80.815, 34.117], [-80.815, 34.116], [-80.815, 34.115], [-80.815, 34.114], [-80.816, 34.114], [-80.817, 34.114], [-80.817, 34.113], [-80.817, 34.112], [-80.817, 34.111], [-80.816, 34.11], [-80.816, 34.109], [-80.815, 34.106], [-80.82, 34.104], [-80.819, 34.104], [-80.818, 34.104], [-80.818, 34.105], [-80.817, 34.105], [-80.816, 34.105], [-80.81, 34.104], [-80.809, 34.104], [-80.808, 34.104], [-80.807, 34.104], [-80.806, 34.104], [-80.805, 34.104], [-80.804, 34.103], [-80.803, 34.103], [-80.802, 34.103], [-80.801, 34.103], [-80.797, 34.103], [-80.795, 34.102], [-80.787, 34.102], [-80.785, 34.102], [-80.784, 34.102], [-80.783, 34.102], [-80.78, 34.102], [-80.778, 34.102], [-80.777, 34.102], [-80.776, 34.102], [-80.775, 34.102], [-80.774, 34.102], [-80.772, 34.101], [-80.771, 34.101], [-80.771, 34.1], [-80.77, 34.1], [-80.769, 34.1], [-80.768, 34.099], [-80.767, 34.098], [-80.767, 34.097], [-80.766, 34.097], [-80.766, 34.096], [-80.766, 34.095], [-80.765, 34.095], [-80.765, 34.094], [-80.763, 34.092], [-80.763, 34.091], [-80.762, 34.09], [-80.762, 34.089], [-80.761, 34.089], [-80.759, 34.087], [-80.758, 34.086], [-80.757, 34.085], [-80.756, 34.085], [-80.756, 34.084], [-80.755, 34.084], [-80.754, 34.084], [-80.753, 34.083], [-80.752, 34.083], [-80.752, 34.084], [-80.751, 34.084], [-80.75, 34.084], [-80.749, 34.084], [-80.748, 34.084], [-80.748, 34.083], [-80.747, 34.083], [-80.746, 34.083], [-80.744, 34.082], [-80.743, 34.082], [-80.742, 34.082], [-80.742, 34.081], [-80.741, 34.081], [-80.74, 34.081], [-80.74, 34.08], [-80.737, 34.079], [-80.736, 34.078], [-80.735, 34.077], [-80.734, 34.076], [-80.733, 34.075], [-80.732, 34.074], [-80.732, 34.073], [-80.731, 34.073], [-80.73, 34.072], [-80.728, 34.07], [-80.727, 34.07], [-80.727, 34.069], [-80.726, 34.069], [-80.725, 34.068], [-80.724, 34.068], [-80.724, 34.067], [-80.723, 34.067], [-80.722, 34.067], [-80.721, 34.067], [-80.721, 34.066], [-80.72, 34.066], [-80.719, 34.066], [-80.719, 34.065], [-80.718, 34.065], [-80.717, 34.065], [-80.714, 34.064], [-80.713, 34.064], [-80.713, 34.063], [-80.712, 34.063], [-80.711, 34.063], [-80.71, 34.063], [-80.709, 34.063], [-80.708, 34.063], [-80.707, 34.063], [-80.707, 34.062], [-80.706, 34.062], [-80.705, 34.062], [-80.704, 34.061], [-80.704, 34.06], [-80.704, 34.057], [-80.704, 34.05], [-80.704, 34.049], [-80.704, 34.047], [-80.704, 34.045], [-80.704, 34.043], [-80.704, 34.042], [-80.704, 34.04], [-80.704, 34.039], [-80.704, 34.038], [-80.704, 34.037], [-80.704, 34.036], [-80.704, 34.034], [-80.704, 34.033], [-80.704, 34.032], [-80.704, 34.031], [-80.704, 34.03], [-80.704, 34.029], [-80.704, 34.027], [-80.704, 34.026], [-80.704, 34.025], [-80.704, 34.024], [-80.704, 34.023], [-80.704, 34.022], [-80.704, 34.021], [-80.704, 34.02], [-80.704, 34.019], [-80.704, 34.016], [-80.704, 34.014], [-80.704, 34.01], [-80.705, 34.01], [-80.705, 34.009], [-80.706, 34.009], [-80.706, 34.008], [-80.707, 34.008], [-80.709, 34.008], [-80.714, 34.008], [-80.715, 34.008], [-80.717, 34.008], [-80.718, 34.008], [-80.719, 34.008], [-80.72, 34.008], [-80.724, 34.007], [-80.726, 34.007], [-80.727, 34.007], [-80.731, 34.007], [-80.733, 34.007], [-80.734, 34.007], [-80.736, 34.007], [-80.742, 34.007], [-80.744, 34.007], [-80.748, 34.006], [-80.751, 34.006], [-80.759, 34.006], [-80.764, 34.006], [-80.766, 34.006], [-80.767, 34.005], [-80.768, 34.005], [-80.769, 34.005], [-80.771, 34.005], [-80.773, 34.005], [-80.776, 34.004], [-80.78, 34.004], [-80.782, 34.004], [-80.783, 34.003], [-80.789, 34.003], [-80.791, 34.002], [-80.792, 34.002], [-80.795, 34.002], [-80.803, 34.001], [-80.805, 34], [-80.806, 34], [-80.807, 34], [-80.811, 34], [-80.813, 33.999], [-80.814, 33.999], [-80.815, 33.999], [-80.816, 33.999], [-80.817, 33.999], [-80.818, 33.999], [-80.819, 33.999], [-80.819, 33.998], [-80.822, 33.998], [-80.823, 33.998], [-80.825, 33.998], [-80.826, 33.998], [-80.826, 33.997], [-80.828, 33.997], [-80.831, 33.997], [-80.833, 33.997], [-80.833, 33.996], [-80.834, 33.996], [-80.835, 33.996], [-80.836, 33.996], [-80.837, 33.996], [-80.84, 33.996], [-80.841, 33.995], [-80.842, 33.995], [-80.843, 33.995], [-80.847, 33.995], [-80.848, 33.994], [-80.85, 33.994], [-80.852, 33.994], [-80.853, 33.994], [-80.855, 33.994], [-80.855, 33.993], [-80.856, 33.993], [-80.857, 33.993], [-80.861, 33.993], [-80.863, 33.992], [-80.864, 33.992], [-80.866, 33.992], [-80.867, 33.992], [-80.868, 33.992], [-80.869, 33.991], [-80.87, 33.991], [-80.871, 33.991], [-80.872, 33.991], [-80.873, 33.99], [-80.874, 33.99], [-80.878, 33.989], [-80.88, 33.989], [-80.88, 33.988], [-80.881, 33.988], [-80.881, 33.987], [-80.882, 33.986], [-80.881, 33.985], [-80.88, 33.985], [-80.88, 33.984], [-80.88, 33.983], [-80.877, 33.983], [-80.877, 33.982], [-80.876, 33.982], [-80.875, 33.982], [-80.873, 33.98], [-80.872, 33.982], [-80.871, 33.982], [-80.871, 33.979], [-80.872, 33.979], [-80.873, 33.979], [-80.874, 33.979], [-80.874, 33.98], [-80.875, 33.98], [-80.876, 33.98], [-80.876, 33.979], [-80.877, 33.979], [-80.877, 33.98], [-80.877, 33.981], [-80.878, 33.981], [-80.879, 33.981], [-80.879, 33.98], [-80.879, 33.979], [-80.879, 33.978], [-80.88, 33.978], [-80.881, 33.978], [-80.882, 33.979], [-80.883, 33.979], [-80.884, 33.979], [-80.885, 33.98], [-80.884, 33.982], [-80.884, 33.983], [-80.884, 33.985], [-80.885, 33.985], [-80.886, 33.985], [-80.886, 33.986], [-80.884, 33.986], [-80.883, 33.987], [-80.882, 33.987], [-80.883, 33.988], [-80.884, 33.987], [-80.888, 33.986], [-80.889, 33.986], [-80.89, 33.986], [-80.89, 33.985], [-80.89, 33.984], [-80.891, 33.984], [-80.892, 33.984], [-80.893, 33.984], [-80.893, 33.985], [-80.894, 33.985], [-80.895, 33.985], [-80.895, 33.984], [-80.894, 33.984], [-80.895, 33.983], [-80.896, 33.983], [-80.897, 33.983], [-80.896, 33.983], [-80.896, 33.984], [-80.897, 33.984], [-80.898, 33.984], [-80.899, 33.984], [-80.899, 33.983], [-80.899, 33.982], [-80.897, 33.982], [-80.898, 33.981], [-80.898, 33.979], [-80.896, 33.979], [-80.896, 33.978], [-80.897, 33.979], [-80.9, 33.979], [-80.901, 33.978], [-80.902, 33.977], [-80.9, 33.975], [-80.9, 33.974], [-80.9, 33.973], [-80.9, 33.972], [-80.9, 33.973], [-80.901, 33.973], [-80.902, 33.973], [-80.903, 33.973], [-80.904, 33.973], [-80.904, 33.974], [-80.904, 33.975], [-80.904, 33.976], [-80.904, 33.977], [-80.904, 33.978], [-80.904, 33.979], [-80.905, 33.979], [-80.904, 33.979], [-80.904, 33.98], [-80.905, 33.98], [-80.904, 33.981], [-80.903, 33.981], [-80.903, 33.982], [-80.904, 33.982], [-80.905, 33.982], [-80.906, 33.982], [-80.905, 33.982], [-80.906, 33.982], [-80.907, 33.982], [-80.908, 33.982], [-80.909, 33.982], [-80.91, 33.981], [-80.911, 33.981], [-80.912, 33.981], [-80.913, 33.981], [-80.913, 33.98], [-80.914, 33.98], [-80.915, 33.98], [-80.914, 33.98], [-80.915, 33.98], [-80.914, 33.98], [-80.915, 33.98], [-80.915, 33.979], [-80.915, 33.978], [-80.915, 33.977], [-80.916, 33.976], [-80.915, 33.977], [-80.915, 33.976], [-80.916, 33.976], [-80.916, 33.975], [-80.917, 33.975], [-80.917, 33.974], [-80.917, 33.973], [-80.918, 33.974], [-80.92, 33.974], [-80.92, 33.973], [-80.92, 33.972], [-80.921, 33.972], [-80.92, 33.972], [-80.919, 33.972], [-80.919, 33.971], [-80.918, 33.971], [-80.919, 33.97], [-80.919, 33.969], [-80.919, 33.968], [-80.918, 33.968], [-80.918, 33.967], [-80.917, 33.967], [-80.918, 33.965], [-80.92, 33.965], [-80.922, 33.966], [-80.922, 33.965], [-80.922, 33.964], [-80.923, 33.964], [-80.923, 33.965], [-80.923, 33.964], [-80.924, 33.964], [-80.924, 33.965], [-80.924, 33.966], [-80.923, 33.966], [-80.922, 33.968], [-80.922, 33.969], [-80.922, 33.968], [-80.921, 33.969], [-80.921, 33.968], [-80.922, 33.972], [-80.923, 33.971], [-80.923, 33.97], [-80.923, 33.971], [-80.924, 33.97], [-80.924, 33.971], [-80.925, 33.971], [-80.925, 33.972], [-80.926, 33.973], [-80.926, 33.972], [-80.926, 33.973], [-80.926, 33.974], [-80.926, 33.973], [-80.927, 33.973], [-80.926, 33.974], [-80.927, 33.974], [-80.927, 33.975], [-80.928, 33.975], [-80.927, 33.975], [-80.928, 33.976], [-80.927, 33.976], [-80.925, 33.977], [-80.925, 33.978], [-80.925, 33.979], [-80.926, 33.979], [-80.927, 33.979], [-80.928, 33.978], [-80.929, 33.979], [-80.929, 33.978], [-80.93, 33.978], [-80.93, 33.979], [-80.931, 33.979], [-80.93, 33.979], [-80.93, 33.98], [-80.931, 33.98], [-80.931, 33.981], [-80.934, 33.98], [-80.935, 33.979], [-80.938, 33.978], [-80.937, 33.977], [-80.936, 33.977], [-80.937, 33.977], [-80.936, 33.978], [-80.936, 33.977], [-80.937, 33.976], [-80.936, 33.976], [-80.937, 33.975], [-80.938, 33.975], [-80.939, 33.975], [-80.939, 33.974], [-80.939, 33.973], [-80.938, 33.973], [-80.94, 33.971], [-80.941, 33.971], [-80.942, 33.97], [-80.942, 33.969], [-80.941, 33.969], [-80.94, 33.968], [-80.939, 33.97], [-80.938, 33.971], [-80.937, 33.97], [-80.937, 33.971], [-80.937, 33.972], [-80.936, 33.972], [-80.936, 33.973], [-80.935, 33.973], [-80.935, 33.972], [-80.934, 33.971], [-80.934, 33.97], [-80.933, 33.97], [-80.933, 33.969], [-80.932, 33.97], [-80.932, 33.969], [-80.931, 33.969], [-80.931, 33.968], [-80.93, 33.968], [-80.929, 33.968], [-80.928, 33.967], [-80.926, 33.967], [-80.926, 33.966], [-80.927, 33.965], [-80.927, 33.964], [-80.927, 33.963], [-80.928, 33.963], [-80.928, 33.964], [-80.929, 33.964], [-80.929, 33.963], [-80.93, 33.962], [-80.928, 33.962], [-80.93, 33.962], [-80.93, 33.961], [-80.929, 33.96], [-80.929, 33.959], [-80.93, 33.958], [-80.931, 33.958], [-80.932, 33.958], [-80.932, 33.959], [-80.933, 33.959], [-80.934, 33.959], [-80.934, 33.96], [-80.934, 33.961], [-80.935, 33.962], [-80.937, 33.961], [-80.938, 33.959], [-80.939, 33.959], [-80.94, 33.96], [-80.941, 33.959], [-80.941, 33.96], [-80.942, 33.959], [-80.943, 33.959], [-80.943, 33.958], [-80.944, 33.958], [-80.945, 33.958], [-80.945, 33.957], [-80.944, 33.957], [-80.944, 33.956], [-80.943, 33.956], [-80.943, 33.955], [-80.943, 33.954], [-80.942, 33.954], [-80.941, 33.954], [-80.94, 33.954], [-80.939, 33.954], [-80.939, 33.955], [-80.939, 33.956], [-80.938, 33.956], [-80.937, 33.958], [-80.936, 33.958], [-80.937, 33.957], [-80.936, 33.958], [-80.935, 33.958], [-80.935, 33.957], [-80.936, 33.957], [-80.936, 33.956], [-80.937, 33.957], [-80.938, 33.956], [-80.936, 33.956], [-80.936, 33.955], [-80.935, 33.956], [-80.934, 33.957], [-80.933, 33.957], [-80.933, 33.958], [-80.932, 33.958], [-80.932, 33.957], [-80.931, 33.957], [-80.931, 33.958], [-80.93, 33.957], [-80.93, 33.956], [-80.929, 33.956], [-80.928, 33.956], [-80.927, 33.956], [-80.927, 33.955], [-80.928, 33.954], [-80.928, 33.953], [-80.928, 33.952], [-80.931, 33.95], [-80.93, 33.949], [-80.931, 33.95], [-80.932, 33.949], [-80.933, 33.948], [-80.934, 33.948], [-80.935, 33.948], [-80.938, 33.949], [-80.939, 33.949], [-80.941, 33.95], [-80.944, 33.951], [-80.945, 33.951], [-80.947, 33.952], [-80.948, 33.952], [-80.948, 33.951], [-80.948, 33.95], [-80.949, 33.95], [-80.95, 33.95], [-80.95, 33.951], [-80.951, 33.951], [-80.951, 33.95], [-80.951, 33.951], [-80.951, 33.95], [-80.952, 33.95], [-80.952, 33.949], [-80.953, 33.948], [-80.953, 33.947], [-80.954, 33.948], [-80.957, 33.949], [-80.958, 33.949], [-80.958, 33.95], [-80.959, 33.95], [-80.96, 33.951], [-80.961, 33.951], [-80.962, 33.952], [-80.963, 33.952], [-80.964, 33.952], [-80.966, 33.951], [-80.967, 33.95], [-80.967, 33.951], [-80.967, 33.95], [-80.968, 33.95], [-80.968, 33.949], [-80.969, 33.949], [-80.97, 33.948], [-80.971, 33.949], [-80.972, 33.948], [-80.973, 33.948], [-80.975, 33.947], [-80.974, 33.946], [-80.975, 33.946], [-80.974, 33.945], [-80.974, 33.944], [-80.973, 33.944], [-80.972, 33.943], [-80.973, 33.943], [-80.972, 33.943], [-80.972, 33.942], [-80.97, 33.943], [-80.969, 33.944], [-80.968, 33.944], [-80.966, 33.942], [-80.965, 33.941], [-80.964, 33.942], [-80.963, 33.942], [-80.963, 33.943], [-80.962, 33.943], [-80.962, 33.942], [-80.96, 33.943], [-80.959, 33.942], [-80.96, 33.939], [-80.96, 33.938], [-80.96, 33.937], [-80.961, 33.936], [-80.961, 33.935], [-80.961, 33.936], [-80.962, 33.936], [-80.963, 33.937], [-80.963, 33.938], [-80.964, 33.938], [-80.964, 33.939], [-80.965, 33.939], [-80.966, 33.939], [-80.966, 33.94], [-80.966, 33.939], [-80.967, 33.939], [-80.968, 33.939], [-80.969, 33.939], [-80.97, 33.939], [-80.97, 33.94], [-80.971, 33.94], [-80.972, 33.94], [-80.974, 33.941], [-80.974, 33.94], [-80.975, 33.94], [-80.976, 33.939], [-80.977, 33.939], [-80.978, 33.939], [-80.978, 33.938], [-80.979, 33.938], [-80.979, 33.939], [-80.981, 33.94], [-80.981, 33.939], [-80.982, 33.939], [-80.983, 33.94], [-80.984, 33.941], [-80.985, 33.942], [-80.983, 33.943], [-80.983, 33.944], [-80.981, 33.945], [-80.982, 33.946], [-80.983, 33.948], [-80.983, 33.947], [-80.985, 33.947], [-80.985, 33.946], [-80.986, 33.946], [-80.987, 33.947], [-80.988, 33.947], [-80.989, 33.947], [-80.989, 33.948], [-80.989, 33.949], [-80.99, 33.949], [-80.99, 33.95], [-80.991, 33.951], [-80.991, 33.952], [-80.992, 33.953], [-80.991, 33.953], [-80.991, 33.954], [-80.99, 33.954], [-80.989, 33.955], [-80.988, 33.955], [-80.988, 33.956], [-80.986, 33.955], [-80.986, 33.956], [-80.985, 33.956], [-80.984, 33.957], [-80.983, 33.957], [-80.982, 33.957], [-80.981, 33.956], [-80.98, 33.956], [-80.978, 33.956], [-80.978, 33.957], [-80.979, 33.957], [-80.978, 33.957], [-80.978, 33.958], [-80.98, 33.959], [-80.979, 33.96], [-80.98, 33.961], [-80.979, 33.961], [-80.98, 33.961], [-80.98, 33.962], [-80.98, 33.963], [-80.981, 33.963], [-80.982, 33.963], [-80.981, 33.964], [-80.981, 33.965], [-80.982, 33.965], [-80.982, 33.966], [-80.983, 33.966], [-80.983, 33.967], [-80.984, 33.968], [-80.983, 33.969], [-80.983, 33.97], [-80.982, 33.97], [-80.981, 33.97], [-80.981, 33.969], [-80.981, 33.97], [-80.981, 33.971], [-80.981, 33.972], [-80.98, 33.973], [-80.98, 33.974], [-80.981, 33.974], [-80.981, 33.973], [-80.982, 33.973], [-80.982, 33.972], [-80.982, 33.971], [-80.983, 33.971], [-80.984, 33.971], [-80.985, 33.971], [-80.986, 33.971], [-80.986, 33.972], [-80.987, 33.972], [-80.988, 33.97], [-80.987, 33.97], [-80.986, 33.97], [-80.986, 33.969], [-80.987, 33.97], [-80.988, 33.97], [-80.988, 33.969], [-80.988, 33.968], [-80.987, 33.968], [-80.986, 33.968], [-80.986, 33.967], [-80.985, 33.967], [-80.986, 33.966], [-80.988, 33.967], [-80.989, 33.968], [-80.99, 33.968], [-80.991, 33.968], [-80.992, 33.968], [-80.993, 33.968], [-80.994, 33.969], [-80.997, 33.97], [-81, 33.972], [-81.003, 33.973], [-81.008, 33.976], [-81.009, 33.976], [-81.012, 33.978], [-81.013, 33.978], [-81.013, 33.979], [-81.014, 33.979], [-81.015, 33.979], [-81.015, 33.98], [-81.016, 33.98], [-81.017, 33.98], [-81.02, 33.982], [-81.021, 33.982], [-81.022, 33.983], [-81.023, 33.983], [-81.023, 33.982], [-81.022, 33.982], [-81.022, 33.981], [-81.021, 33.98], [-81.02, 33.98], [-81.019, 33.979], [-81.017, 33.979], [-81.017, 33.978], [-81.017, 33.979], [-81.016, 33.979], [-81.015, 33.979], [-81.016, 33.979], [-81.016, 33.978], [-81.017, 33.979], [-81.017, 33.978], [-81.016, 33.978], [-81.017, 33.977], [-81.016, 33.976], [-81.015, 33.976], [-81.014, 33.975], [-81.013, 33.975], [-81.014, 33.975], [-81.014, 33.974], [-81.015, 33.975], [-81.016, 33.975], [-81.016, 33.974], [-81.016, 33.973], [-81.015, 33.973], [-81.015, 33.972], [-81.016, 33.972], [-81.016, 33.971], [-81.016, 33.972], [-81.017, 33.972], [-81.016, 33.972], [-81.017, 33.972], [-81.017, 33.971], [-81.018, 33.971], [-81.018, 33.972], [-81.019, 33.971], [-81.02, 33.972], [-81.021, 33.973], [-81.022, 33.971], [-81.023, 33.972], [-81.023, 33.971], [-81.024, 33.971], [-81.025, 33.972], [-81.026, 33.972], [-81.025, 33.973], [-81.023, 33.975], [-81.024, 33.975], [-81.024, 33.976], [-81.025, 33.976], [-81.026, 33.976], [-81.026, 33.975], [-81.027, 33.976], [-81.026, 33.977], [-81.025, 33.977], [-81.024, 33.977], [-81.024, 33.978], [-81.025, 33.978], [-81.025, 33.979], [-81.024, 33.979], [-81.023, 33.979], [-81.023, 33.98], [-81.023, 33.981], [-81.024, 33.981], [-81.023, 33.981], [-81.023, 33.982], [-81.024, 33.982], [-81.025, 33.982], [-81.025, 33.983], [-81.024, 33.983], [-81.023, 33.984], [-81.024, 33.985], [-81.025, 33.986], [-81.025, 33.987], [-81.026, 33.986], [-81.026, 33.987], [-81.026, 33.986], [-81.027, 33.986], [-81.026, 33.986], [-81.025, 33.985], [-81.026, 33.985], [-81.026, 33.984], [-81.026, 33.983], [-81.027, 33.982], [-81.028, 33.982], [-81.027, 33.981], [-81.028, 33.981], [-81.029, 33.981], [-81.03, 33.981], [-81.029, 33.982], [-81.029, 33.983], [-81.03, 33.983], [-81.029, 33.984], [-81.028, 33.984], [-81.028, 33.985], [-81.027, 33.985], [-81.027, 33.986], [-81.028, 33.986], [-81.029, 33.986], [-81.028, 33.985], [-81.029, 33.985], [-81.03, 33.985], [-81.031, 33.985], [-81.032, 33.985], [-81.033, 33.985], [-81.033, 33.984], [-81.033, 33.983], [-81.033, 33.984], [-81.034, 33.984], [-81.035, 33.984], [-81.035, 33.983], [-81.035, 33.982], [-81.036, 33.981], [-81.035, 33.981], [-81.037, 33.98], [-81.038, 33.98], [-81.039, 33.98], [-81.039, 33.981], [-81.04, 33.982], [-81.041, 33.982], [-81.041, 33.981], [-81.042, 33.981], [-81.042, 33.98], [-81.043, 33.979], [-81.044, 33.978], [-81.045, 33.978], [-81.046, 33.978], [-81.046, 33.98], [-81.046, 33.983], [-81.046, 33.986], [-81.046, 33.987], [-81.046, 33.988], [-81.047, 33.988], [-81.048, 33.991], [-81.051, 33.996], [-81.053, 33.998], [-81.054, 33.999], [-81.054, 34], [-81.055, 34], [-81.055, 34.001], [-81.055, 34.002], [-81.056, 34.002], [-81.059, 34.004], [-81.059, 34.005], [-81.06, 34.005], [-81.061, 34.005], [-81.063, 34.006], [-81.064, 34.007], [-81.064, 34.008], [-81.065, 34.009], [-81.066, 34.009], [-81.067, 34.009], [-81.068, 34.009], [-81.07, 34.008], [-81.071, 34.007], [-81.072, 34.007], [-81.073, 34.007], [-81.074, 34.007], [-81.076, 34.008], [-81.077, 34.009], [-81.079, 34.011], [-81.08, 34.012], [-81.082, 34.012], [-81.084, 34.012], [-81.086, 34.014], [-81.088, 34.014], [-81.089, 34.014], [-81.09, 34.014], [-81.091, 34.014], [-81.092, 34.014], [-81.095, 34.016], [-81.097, 34.018], [-81.098, 34.019], [-81.099, 34.022], [-81.1, 34.024], [-81.101, 34.024], [-81.102, 34.024], [-81.104, 34.023], [-81.104, 34.024], [-81.103, 34.024], [-81.104, 34.025], [-81.104, 34.026], [-81.105, 34.026], [-81.108, 34.026], [-81.109, 34.026], [-81.11, 34.026], [-81.112, 34.026], [-81.112, 34.027], [-81.113, 34.027], [-81.113, 34.028], [-81.112, 34.028], [-81.111, 34.028], [-81.11, 34.027], [-81.11, 34.028], [-81.11, 34.029], [-81.11, 34.031], [-81.11, 34.03], [-81.11, 34.031], [-81.109, 34.031], [-81.108, 34.031], [-81.107, 34.031], [-81.106, 34.031], [-81.105, 34.032], [-81.105, 34.033], [-81.104, 34.032], [-81.104, 34.031], [-81.103, 34.031], [-81.102, 34.031], [-81.103, 34.031], [-81.103, 34.03], [-81.102, 34.03], [-81.101, 34.03], [-81.101, 34.029], [-81.101, 34.028], [-81.1, 34.028], [-81.1, 34.027], [-81.1, 34.026], [-81.099, 34.026], [-81.098, 34.025], [-81.098, 34.026], [-81.097, 34.026], [-81.097, 34.025], [-81.098, 34.025], [-81.097, 34.025], [-81.097, 34.024], [-81.096, 34.025], [-81.095, 34.025], [-81.096, 34.024], [-81.096, 34.023], [-81.095, 34.023], [-81.095, 34.024], [-81.095, 34.025], [-81.094, 34.025], [-81.093, 34.025], [-81.093, 34.026], [-81.093, 34.027], [-81.094, 34.027], [-81.094, 34.028], [-81.094, 34.029], [-81.093, 34.029], [-81.092, 34.029], [-81.092, 34.03], [-81.093, 34.03], [-81.093, 34.031], [-81.094, 34.031], [-81.095, 34.031], [-81.096, 34.031], [-81.096, 34.03], [-81.097, 34.03], [-81.097, 34.031], [-81.097, 34.03], [-81.098, 34.03], [-81.099, 34.03], [-81.1, 34.03], [-81.099, 34.03], [-81.1, 34.032], [-81.099, 34.032], [-81.098, 34.032], [-81.098, 34.033], [-81.097, 34.033], [-81.097, 34.032], [-81.096, 34.032], [-81.095, 34.033], [-81.093, 34.034], [-81.093, 34.035], [-81.092, 34.035], [-81.091, 34.035], [-81.09, 34.035], [-81.089, 34.035], [-81.089, 34.034], [-81.088, 34.034], [-81.087, 34.034], [-81.087, 34.033], [-81.088, 34.033], [-81.089, 34.033], [-81.091, 34.032], [-81.091, 34.031], [-81.09, 34.031], [-81.089, 34.031], [-81.088, 34.031], [-81.087, 34.031], [-81.086, 34.031], [-81.087, 34.031], [-81.087, 34.03], [-81.087, 34.029], [-81.087, 34.027], [-81.086, 34.026], [-81.087, 34.026], [-81.086, 34.026], [-81.087, 34.026], [-81.088, 34.025], [-81.088, 34.024], [-81.087, 34.024], [-81.087, 34.023], [-81.086, 34.023], [-81.087, 34.023], [-81.087, 34.022], [-81.087, 34.023], [-81.088, 34.022], [-81.087, 34.022], [-81.088, 34.022], [-81.088, 34.021], [-81.088, 34.02], [-81.087, 34.02], [-81.086, 34.02], [-81.086, 34.018], [-81.085, 34.018], [-81.084, 34.018], [-81.083, 34.018], [-81.085, 34.017], [-81.084, 34.017], [-81.085, 34.016], [-81.085, 34.015], [-81.086, 34.015], [-81.086, 34.014], [-81.085, 34.015], [-81.085, 34.016], [-81.084, 34.016], [-81.083, 34.016], [-81.082, 34.016], [-81.082, 34.017], [-81.082, 34.016], [-81.081, 34.016], [-81.08, 34.016], [-81.081, 34.017], [-81.081, 34.019], [-81.082, 34.02], [-81.081, 34.02], [-81.078, 34.02], [-81.078, 34.021], [-81.077, 34.021], [-81.078, 34.022], [-81.078, 34.023], [-81.079, 34.022], [-81.08, 34.022], [-81.081, 34.023], [-81.081, 34.024], [-81.083, 34.025], [-81.082, 34.026], [-81.081, 34.027], [-81.08, 34.027], [-81.08, 34.026], [-81.079, 34.026], [-81.079, 34.025], [-81.079, 34.024], [-81.078, 34.024], [-81.078, 34.023], [-81.077, 34.023], [-81.078, 34.024], [-81.077, 34.024], [-81.078, 34.025], [-81.077, 34.025], [-81.077, 34.026], [-81.078, 34.027], [-81.079, 34.027], [-81.079, 34.028], [-81.079, 34.027], [-81.08, 34.027], [-81.081, 34.028], [-81.081, 34.027], [-81.081, 34.028], [-81.08, 34.028], [-81.08, 34.029], [-81.08, 34.03], [-81.081, 34.03], [-81.08, 34.03], [-81.08, 34.031], [-81.079, 34.031], [-81.079, 34.032], [-81.08, 34.032], [-81.08, 34.033], [-81.08, 34.034], [-81.079, 34.035], [-81.079, 34.034], [-81.078, 34.034], [-81.078, 34.035], [-81.077, 34.035], [-81.077, 34.036], [-81.076, 34.036], [-81.076, 34.035], [-81.075, 34.035], [-81.076, 34.036], [-81.076, 34.037], [-81.076, 34.038], [-81.072, 34.038], [-81.072, 34.039], [-81.072, 34.04], [-81.072, 34.041], [-81.073, 34.042], [-81.073, 34.043], [-81.073, 34.044], [-81.074, 34.044], [-81.074, 34.045], [-81.075, 34.044], [-81.076, 34.044], [-81.078, 34.044], [-81.078, 34.045], [-81.079, 34.045], [-81.08, 34.045], [-81.08, 34.046], [-81.079, 34.046], [-81.08, 34.047], [-81.079, 34.047], [-81.078, 34.048], [-81.078, 34.049], [-81.078, 34.048], [-81.077, 34.048], [-81.076, 34.048], [-81.075, 34.049], [-81.075, 34.05], [-81.075, 34.051], [-81.076, 34.051], [-81.076, 34.052], [-81.076, 34.053], [-81.076, 34.054], [-81.076, 34.055], [-81.076, 34.056], [-81.077, 34.056], [-81.077, 34.057], [-81.078, 34.056], [-81.079, 34.056], [-81.08, 34.056], [-81.081, 34.055], [-81.081, 34.056], [-81.082, 34.056], [-81.083, 34.058], [-81.084, 34.058], [-81.085, 34.058], [-81.089, 34.058], [-81.092, 34.057], [-81.093, 34.057], [-81.093, 34.056], [-81.094, 34.056], [-81.095, 34.056], [-81.095, 34.055], [-81.096, 34.055], [-81.096, 34.054], [-81.097, 34.055], [-81.096, 34.055], [-81.097, 34.057], [-81.098, 34.056], [-81.099, 34.056], [-81.099, 34.055], [-81.099, 34.056], [-81.1, 34.056], [-81.101, 34.055], [-81.102, 34.054], [-81.102, 34.053], [-81.102, 34.052], [-81.101, 34.052], [-81.101, 34.051], [-81.101, 34.052], [-81.102, 34.052], [-81.102, 34.051], [-81.102, 34.05], [-81.103, 34.05], [-81.103, 34.051], [-81.104, 34.051], [-81.104, 34.05], [-81.104, 34.051], [-81.104, 34.052], [-81.105, 34.051], [-81.106, 34.052], [-81.107, 34.052], [-81.108, 34.052], [-81.108, 34.053], [-81.109, 34.053], [-81.109, 34.054], [-81.11, 34.054], [-81.11, 34.055], [-81.111, 34.055], [-81.111, 34.056], [-81.11, 34.056], [-81.11, 34.055], [-81.109, 34.055], [-81.108, 34.056], [-81.107, 34.056], [-81.108, 34.057], [-81.109, 34.057], [-81.109, 34.058], [-81.108, 34.059], [-81.107, 34.059], [-81.106, 34.059], [-81.105, 34.06], [-81.105, 34.061], [-81.107, 34.06], [-81.108, 34.06], [-81.109, 34.06], [-81.109, 34.061], [-81.11, 34.061], [-81.106, 34.063], [-81.107, 34.063], [-81.107, 34.064], [-81.108, 34.064], [-81.109, 34.064], [-81.112, 34.063], [-81.113, 34.063], [-81.114, 34.062], [-81.115, 34.063], [-81.115, 34.064], [-81.114, 34.064], [-81.113, 34.064], [-81.11, 34.064], [-81.109, 34.065], [-81.108, 34.065], [-81.109, 34.065], [-81.108, 34.065], [-81.108, 34.066], [-81.109, 34.066], [-81.109, 34.067], [-81.11, 34.068], [-81.113, 34.067], [-81.114, 34.066], [-81.116, 34.065], [-81.117, 34.066], [-81.117, 34.067], [-81.118, 34.069], [-81.119, 34.069], [-81.119, 34.07], [-81.12, 34.07], [-81.121, 34.07], [-81.12, 34.07], [-81.12, 34.071], [-81.121, 34.071], [-81.122, 34.072], [-81.122, 34.073], [-81.123, 34.073], [-81.124, 34.072], [-81.124, 34.073], [-81.125, 34.072], [-81.126, 34.072], [-81.126, 34.071], [-81.127, 34.071], [-81.126, 34.071], [-81.125, 34.071], [-81.126, 34.07], [-81.127, 34.07], [-81.127, 34.069], [-81.126, 34.068], [-81.125, 34.068], [-81.125, 34.067], [-81.126, 34.067], [-81.127, 34.068], [-81.129, 34.068], [-81.13, 34.068], [-81.131, 34.07], [-81.129, 34.072], [-81.128, 34.072], [-81.129, 34.073], [-81.13, 34.074], [-81.131, 34.074], [-81.131, 34.073], [-81.133, 34.073], [-81.134, 34.072], [-81.135, 34.072], [-81.135, 34.073], [-81.136, 34.072], [-81.136, 34.074], [-81.137, 34.075], [-81.138, 34.075], [-81.139, 34.075], [-81.14, 34.074], [-81.14, 34.075], [-81.139, 34.075], [-81.139, 34.076], [-81.138, 34.076], [-81.138, 34.075], [-81.137, 34.075], [-81.136, 34.075], [-81.135, 34.075], [-81.134, 34.075], [-81.132, 34.075], [-81.132, 34.076], [-81.132, 34.077], [-81.133, 34.077], [-81.133, 34.078], [-81.133, 34.079], [-81.132, 34.079], [-81.132, 34.08], [-81.131, 34.08], [-81.132, 34.08], [-81.132, 34.081], [-81.133, 34.081], [-81.134, 34.081], [-81.134, 34.082], [-81.135, 34.082], [-81.136, 34.083], [-81.137, 34.083], [-81.138, 34.083], [-81.138, 34.084], [-81.138, 34.085], [-81.138, 34.086], [-81.139, 34.086], [-81.138, 34.086], [-81.139, 34.086], [-81.139, 34.085], [-81.139, 34.086], [-81.14, 34.086], [-81.141, 34.086], [-81.142, 34.086], [-81.142, 34.087], [-81.143, 34.087], [-81.144, 34.087], [-81.144, 34.088], [-81.145, 34.088], [-81.146, 34.089], [-81.147, 34.089], [-81.148, 34.089], [-81.148, 34.088], [-81.147, 34.088], [-81.147, 34.087], [-81.146, 34.087], [-81.147, 34.087], [-81.148, 34.087], [-81.148, 34.086], [-81.149, 34.085], [-81.149, 34.083], [-81.151, 34.084], [-81.151, 34.083], [-81.152, 34.083], [-81.151, 34.083], [-81.15, 34.082], [-81.149, 34.082], [-81.148, 34.082], [-81.147, 34.082], [-81.148, 34.08], [-81.149, 34.081], [-81.15, 34.08], [-81.151, 34.08], [-81.15, 34.08], [-81.15, 34.079], [-81.15, 34.078], [-81.149, 34.078], [-81.149, 34.077], [-81.148, 34.077], [-81.147, 34.077], [-81.147, 34.076], [-81.146, 34.076], [-81.146, 34.075], [-81.145, 34.075], [-81.145, 34.074], [-81.146, 34.074], [-81.146, 34.075], [-81.147, 34.074], [-81.148, 34.074], [-81.148, 34.073], [-81.147, 34.073], [-81.146, 34.073], [-81.145, 34.072], [-81.145, 34.073], [-81.144, 34.073], [-81.143, 34.073], [-81.145, 34.072], [-81.146, 34.072], [-81.147, 34.072], [-81.147, 34.071], [-81.147, 34.072], [-81.148, 34.071], [-81.149, 34.071], [-81.15, 34.071], [-81.153, 34.069], [-81.153, 34.068], [-81.159, 34.066], [-81.16, 34.066], [-81.161, 34.067], [-81.162, 34.067], [-81.163, 34.066], [-81.164, 34.065], [-81.165, 34.065], [-81.166, 34.065], [-81.167, 34.065], [-81.167, 34.064], [-81.167, 34.065], [-81.168, 34.066], [-81.167, 34.067], [-81.168, 34.067], [-81.168, 34.068], [-81.165, 34.07], [-81.163, 34.071], [-81.163, 34.072], [-81.162, 34.072], [-81.162, 34.073], [-81.162, 34.074], [-81.162, 34.075], [-81.162, 34.076], [-81.163, 34.076], [-81.164, 34.076], [-81.164, 34.077], [-81.163, 34.076], [-81.162, 34.077], [-81.161, 34.077], [-81.161, 34.078], [-81.161, 34.079], [-81.16, 34.079], [-81.16, 34.08], [-81.161, 34.08], [-81.161, 34.081], [-81.161, 34.082], [-81.162, 34.082], [-81.162, 34.083], [-81.162, 34.082], [-81.162, 34.081], [-81.163, 34.081], [-81.163, 34.08], [-81.163, 34.079], [-81.163, 34.078], [-81.164, 34.078], [-81.164, 34.079], [-81.164, 34.08], [-81.165, 34.08], [-81.165, 34.081], [-81.166, 34.082], [-81.166, 34.083], [-81.165, 34.083], [-81.164, 34.083], [-81.164, 34.084], [-81.166, 34.085], [-81.165, 34.085], [-81.165, 34.086], [-81.164, 34.086], [-81.164, 34.087], [-81.165, 34.088], [-81.166, 34.088], [-81.166, 34.089], [-81.167, 34.089], [-81.168, 34.089], [-81.168, 34.09], [-81.169, 34.09], [-81.17, 34.09], [-81.171, 34.09], [-81.171, 34.091]], [[-81.074, 34.023], [-81.074, 34.024], [-81.075, 34.024], [-81.074, 34.023]], [[-81.074, 34.023], [-81.075, 34.023], [-81.075, 34.024], [-81.076, 34.024], [-81.076, 34.023], [-81.075, 34.023], [-81.075, 34.024], [-81.075, 34.023], [-81.075, 34.022], [-81.074, 34.021], [-81.074, 34.02], [-81.073, 34.018], [-81.072, 34.018], [-81.072, 34.017], [-81.071, 34.017], [-81.071, 34.016], [-81.07, 34.016], [-81.07, 34.015], [-81.069, 34.015], [-81.068, 34.014], [-81.068, 34.013], [-81.067, 34.013], [-81.066, 34.013], [-81.066, 34.014], [-81.067, 34.014], [-81.067, 34.015], [-81.068, 34.015], [-81.068, 34.016], [-81.069, 34.016], [-81.068, 34.016], [-81.068, 34.017], [-81.067, 34.017], [-81.068, 34.017], [-81.067, 34.017], [-81.068, 34.016], [-81.067, 34.016], [-81.066, 34.016], [-81.067, 34.016], [-81.066, 34.016], [-81.066, 34.015], [-81.065, 34.015], [-81.065, 34.016], [-81.066, 34.016], [-81.065, 34.016], [-81.066, 34.016], [-81.066, 34.017], [-81.065, 34.017], [-81.065, 34.018], [-81.064, 34.018], [-81.064, 34.019], [-81.065, 34.019], [-81.065, 34.02], [-81.066, 34.019], [-81.066, 34.02], [-81.067, 34.02], [-81.068, 34.02], [-81.069, 34.02], [-81.073, 34.02], [-81.073, 34.021], [-81.074, 34.021], [-81.074, 34.022], [-81.074, 34.023]], [[-81.056, 34.028], [-81.056, 34.027], [-81.055, 34.027], [-81.056, 34.027], [-81.056, 34.028]], [[-81.055, 34.027], [-81.056, 34.027], [-81.056, 34.026], [-81.056, 34.025], [-81.055, 34.025], [-81.056, 34.025], [-81.056, 34.024], [-81.055, 34.024], [-81.054, 34.024], [-81.054, 34.025], [-81.055, 34.025], [-81.055, 34.026], [-81.054, 34.026], [-81.055, 34.026], [-81.055, 34.027]], [[-80.843, 34.119], [-80.843, 34.118], [-80.842, 34.118], [-80.841, 34.117], [-80.843, 34.115], [-80.842, 34.114], [-80.841, 34.114], [-80.839, 34.112], [-80.838, 34.112], [-80.838, 34.111], [-80.832, 34.107], [-80.832, 34.108], [-80.831, 34.107], [-80.83, 34.107], [-80.831, 34.107], [-80.83, 34.107], [-80.83, 34.106], [-80.829, 34.106], [-80.828, 34.106], [-80.827, 34.106], [-80.826, 34.107], [-80.826, 34.108], [-80.825, 34.109], [-80.824, 34.109], [-80.823, 34.11], [-80.822, 34.11], [-80.823, 34.109], [-80.822, 34.109], [-80.822, 34.108], [-80.821, 34.108], [-80.82, 34.111], [-80.82, 34.112], [-80.819, 34.112], [-80.82, 34.113], [-80.821, 34.113], [-80.822, 34.113], [-80.822, 34.114], [-80.823, 34.114], [-80.824, 34.115], [-80.825, 34.114], [-80.825, 34.115], [-80.826, 34.115], [-80.827, 34.115], [-80.828, 34.115], [-80.828, 34.116], [-80.828, 34.117], [-80.829, 34.117], [-80.83, 34.116], [-80.83, 34.115], [-80.831, 34.114], [-80.83, 34.114], [-80.828, 34.114], [-80.829, 34.113], [-80.83, 34.112], [-80.831, 34.112], [-80.832, 34.112], [-80.832, 34.111], [-80.833, 34.111], [-80.834, 34.112], [-80.835, 34.112], [-80.835, 34.113], [-80.836, 34.113], [-80.836, 34.114], [-80.837, 34.115], [-80.838, 34.115], [-80.838, 34.116], [-80.837, 34.116], [-80.836, 34.116], [-80.836, 34.117], [-80.836, 34.118], [-80.837, 34.118], [-80.838, 34.119], [-80.839, 34.119], [-80.84, 34.119], [-80.841, 34.119], [-80.842, 34.119], [-80.843, 34.119]], [[-80.964, 33.958], [-80.963, 33.958], [-80.962, 33.958], [-80.961, 33.958], [-80.961, 33.959], [-80.96, 33.959], [-80.959, 33.959], [-80.959, 33.96], [-80.958, 33.96], [-80.958, 33.959], [-80.957, 33.959], [-80.957, 33.96], [-80.956, 33.96], [-80.955, 33.96], [-80.955, 33.963], [-80.955, 33.964], [-80.955, 33.966], [-80.956, 33.967], [-80.954, 33.967], [-80.954, 33.968], [-80.955, 33.968], [-80.956, 33.968], [-80.956, 33.969], [-80.956, 33.972], [-80.956, 33.973], [-80.956, 33.974], [-80.956, 33.975], [-80.957, 33.975], [-80.957, 33.974], [-80.957, 33.973], [-80.957, 33.972], [-80.957, 33.969], [-80.957, 33.968], [-80.958, 33.968], [-80.959, 33.969], [-80.96, 33.968], [-80.961, 33.967], [-80.961, 33.966], [-80.961, 33.965], [-80.962, 33.965], [-80.962, 33.964], [-80.961, 33.964], [-80.962, 33.964], [-80.962, 33.963], [-80.963, 33.962], [-80.963, 33.961], [-80.963, 33.96], [-80.963, 33.959], [-80.963, 33.958], [-80.964, 33.958]], [[-80.989, 33.988], [-80.989, 33.987], [-80.988, 33.987], [-80.989, 33.987], [-80.989, 33.986], [-80.988, 33.986], [-80.987, 33.986], [-80.988, 33.986], [-80.989, 33.986], [-80.989, 33.985], [-80.988, 33.985], [-80.989, 33.985], [-80.989, 33.984], [-80.989, 33.983], [-80.988, 33.983], [-80.988, 33.982], [-80.987, 33.982], [-80.987, 33.981], [-80.988, 33.981], [-80.987, 33.981], [-80.987, 33.982], [-80.987, 33.981], [-80.986, 33.981], [-80.986, 33.982], [-80.986, 33.981], [-80.985, 33.981], [-80.984, 33.981], [-80.984, 33.98], [-80.984, 33.981], [-80.983, 33.981], [-80.983, 33.98], [-80.982, 33.98], [-80.982, 33.981], [-80.983, 33.981], [-80.982, 33.981], [-80.983, 33.981], [-80.984, 33.981], [-80.983, 33.981], [-80.983, 33.982], [-80.982, 33.982], [-80.982, 33.981], [-80.981, 33.981], [-80.981, 33.982], [-80.98, 33.982], [-80.98, 33.981], [-80.979, 33.981], [-80.979, 33.982], [-80.98, 33.982], [-80.98, 33.983], [-80.981, 33.983], [-80.982, 33.983], [-80.983, 33.983], [-80.983, 33.984], [-80.982, 33.984], [-80.983, 33.984], [-80.983, 33.985], [-80.982, 33.985], [-80.983, 33.985], [-80.983, 33.986], [-80.982, 33.986], [-80.982, 33.987], [-80.982, 33.986], [-80.979, 33.986], [-80.979, 33.987], [-80.98, 33.987], [-80.981, 33.987], [-80.982, 33.987], [-80.983, 33.987], [-80.983, 33.988], [-80.982, 33.988], [-80.983, 33.988], [-80.983, 33.987], [-80.984, 33.987], [-80.984, 33.988], [-80.985, 33.988], [-80.985, 33.987], [-80.984, 33.987], [-80.984, 33.986], [-80.983, 33.986], [-80.984, 33.986], [-80.984, 33.987], [-80.985, 33.987], [-80.986, 33.987], [-80.987, 33.987], [-80.987, 33.988], [-80.988, 33.988], [-80.989, 33.988]], [[-80.955, 33.979], [-80.954, 33.979], [-80.953, 33.979], [-80.952, 33.979], [-80.951, 33.979], [-80.95, 33.979], [-80.949, 33.978], [-80.948, 33.978], [-80.947, 33.978], [-80.947, 33.977], [-80.946, 33.977], [-80.945, 33.977], [-80.944, 33.976], [-80.944, 33.977], [-80.945, 33.979], [-80.946, 33.979], [-80.947, 33.981], [-80.948, 33.981], [-80.949, 33.981], [-80.95, 33.981], [-80.951, 33.981], [-80.95, 33.982], [-80.95, 33.984], [-80.95, 33.985], [-80.952, 33.984], [-80.953, 33.984], [-80.954, 33.984], [-80.954, 33.982], [-80.954, 33.979], [-80.955, 33.979]], [[-81.016, 34.045], [-81.015, 34.044], [-81.014, 34.045], [-81.014, 34.044], [-81.015, 34.043], [-81.014, 34.043], [-81.014, 34.042], [-81.014, 34.041], [-81.013, 34.041], [-81.012, 34.041], [-81.012, 34.042], [-81.011, 34.042], [-81.011, 34.043], [-81.01, 34.044], [-81.009, 34.044], [-81.009, 34.045], [-81.008, 34.046], [-81.008, 34.047], [-81.008, 34.048], [-81.009, 34.047], [-81.01, 34.047], [-81.01, 34.048], [-81.011, 34.048], [-81.011, 34.047], [-81.012, 34.048], [-81.013, 34.047], [-81.013, 34.048], [-81.013, 34.049], [-81.012, 34.049], [-81.013, 34.049], [-81.013, 34.05], [-81.014, 34.05], [-81.015, 34.049], [-81.015, 34.048], [-81.014, 34.047], [-81.014, 34.048], [-81.013, 34.048], [-81.013, 34.047], [-81.014, 34.047], [-81.015, 34.046], [-81.015, 34.045], [-81.015, 34.046], [-81.016, 34.046], [-81.016, 34.045]], [[-80.973, 33.979], [-80.973, 33.978], [-80.972, 33.978], [-80.972, 33.977], [-80.973, 33.977], [-80.974, 33.977], [-80.973, 33.977], [-80.973, 33.976], [-80.973, 33.975], [-80.972, 33.975], [-80.971, 33.975], [-80.969, 33.974], [-80.968, 33.974], [-80.968, 33.975], [-80.968, 33.976], [-80.967, 33.976], [-80.967, 33.977], [-80.967, 33.978], [-80.966, 33.979], [-80.966, 33.98], [-80.966, 33.981], [-80.967, 33.981], [-80.967, 33.98], [-80.967, 33.979], [-80.968, 33.979], [-80.968, 33.98], [-80.969, 33.98], [-80.968, 33.979], [-80.969, 33.979], [-80.97, 33.979], [-80.971, 33.979], [-80.972, 33.98], [-80.972, 33.979], [-80.973, 33.979]], [[-81.015, 34.035], [-81.014, 34.035], [-81.014, 34.034], [-81.013, 34.034], [-81.013, 34.035], [-81.013, 34.034], [-81.012, 34.034], [-81.011, 34.034], [-81.01, 34.034], [-81.009, 34.034], [-81.009, 34.033], [-81.009, 34.032], [-81.008, 34.032], [-81.007, 34.032], [-81.006, 34.032], [-81.006, 34.033], [-81.005, 34.033], [-81.005, 34.034], [-81.006, 34.034], [-81.006, 34.035], [-81.004, 34.035], [-81.005, 34.035], [-81.005, 34.036], [-81.005, 34.037], [-81.004, 34.037], [-81.003, 34.037], [-81.004, 34.038], [-81.005, 34.038], [-81.005, 34.037], [-81.006, 34.037], [-81.007, 34.037], [-81.008, 34.037], [-81.009, 34.037], [-81.01, 34.037], [-81.01, 34.036], [-81.011, 34.036], [-81.012, 34.036], [-81.012, 34.035], [-81.013, 34.035], [-81.013, 34.036], [-81.013, 34.035], [-81.014, 34.035], [-81.015, 34.035]], [[-80.97, 33.952], [-80.969, 33.952], [-80.968, 33.952], [-80.965, 33.953], [-80.962, 33.954], [-80.963, 33.954], [-80.963, 33.955], [-80.964, 33.955], [-80.965, 33.955], [-80.966, 33.954], [-80.967, 33.954], [-80.969, 33.955], [-80.972, 33.957], [-80.973, 33.957], [-80.974, 33.955], [-80.975, 33.955], [-80.97, 33.952]], [[-80.996, 34.077], [-80.995, 34.077], [-80.994, 34.076], [-80.994, 34.075], [-80.995, 34.075], [-80.995, 34.074], [-80.994, 34.074], [-80.994, 34.073], [-80.995, 34.073], [-80.994, 34.071], [-80.992, 34.069], [-80.991, 34.068], [-80.991, 34.067], [-80.99, 34.067], [-80.99, 34.068], [-80.991, 34.068], [-80.99, 34.068], [-80.992, 34.071], [-80.993, 34.072], [-80.993, 34.073], [-80.993, 34.074], [-80.992, 34.074], [-80.988, 34.073], [-80.988, 34.074], [-80.99, 34.075], [-80.991, 34.075], [-80.992, 34.076], [-80.993, 34.076], [-80.995, 34.077], [-80.996, 34.077]], [[-80.978, 33.969], [-80.977, 33.969], [-80.976, 33.968], [-80.977, 33.968], [-80.975, 33.967], [-80.974, 33.966], [-80.974, 33.967], [-80.974, 33.966], [-80.973, 33.966], [-80.972, 33.965], [-80.971, 33.965], [-80.97, 33.964], [-80.97, 33.965], [-80.97, 33.964], [-80.969, 33.964], [-80.968, 33.965], [-80.969, 33.965], [-80.968, 33.965], [-80.968, 33.966], [-80.968, 33.965], [-80.968, 33.966], [-80.969, 33.966], [-80.969, 33.967], [-80.97, 33.967], [-80.971, 33.967], [-80.972, 33.968], [-80.973, 33.967], [-80.973, 33.968], [-80.972, 33.968], [-80.973, 33.968], [-80.975, 33.969], [-80.976, 33.969], [-80.975, 33.969], [-80.975, 33.97], [-80.977, 33.97], [-80.978, 33.969]], [[-81.012, 34.05], [-81.011, 34.05], [-81.01, 34.05], [-81.009, 34.05], [-81.008, 34.051], [-81.007, 34.051], [-81.007, 34.05], [-81.007, 34.049], [-81.008, 34.049], [-81.008, 34.048], [-81.007, 34.048], [-81.006, 34.048], [-81.005, 34.049], [-81.004, 34.049], [-81.003, 34.049], [-81.002, 34.05], [-81.003, 34.05], [-81.004, 34.051], [-81.005, 34.051], [-81.006, 34.051], [-81.006, 34.052], [-81.007, 34.052], [-81.007, 34.053], [-81.006, 34.053], [-81.007, 34.053], [-81.008, 34.052], [-81.01, 34.052], [-81.01, 34.051], [-81.011, 34.051], [-81.012, 34.051], [-81.012, 34.05]], [[-80.926, 33.98], [-80.926, 33.979], [-80.925, 33.979], [-80.924, 33.979], [-80.923, 33.98], [-80.922, 33.98], [-80.921, 33.98], [-80.92, 33.98], [-80.919, 33.98], [-80.918, 33.98], [-80.917, 33.98], [-80.918, 33.981], [-80.917, 33.98], [-80.917, 33.981], [-80.918, 33.981], [-80.919, 33.981], [-80.918, 33.981], [-80.919, 33.981], [-80.919, 33.982], [-80.92, 33.982], [-80.921, 33.982], [-80.922, 33.983], [-80.923, 33.982], [-80.925, 33.981], [-80.926, 33.98]], [[-81.032, 34.057], [-81.031, 34.057], [-81.03, 34.056], [-81.029, 34.055], [-81.029, 34.056], [-81.029, 34.055], [-81.028, 34.055], [-81.027, 34.056], [-81.028, 34.056], [-81.029, 34.057], [-81.029, 34.056], [-81.03, 34.057], [-81.031, 34.057], [-81.03, 34.058], [-81.027, 34.057], [-81.027, 34.058], [-81.026, 34.058], [-81.026, 34.059], [-81.025, 34.059], [-81.025, 34.06], [-81.026, 34.06], [-81.026, 34.061], [-81.028, 34.06], [-81.029, 34.059], [-81.03, 34.058], [-81.031, 34.058], [-81.032, 34.057]], [[-81.063, 34.022], [-81.061, 34.02], [-81.06, 34.02], [-81.059, 34.02], [-81.058, 34.021], [-81.057, 34.022], [-81.058, 34.022], [-81.058, 34.023], [-81.059, 34.023], [-81.06, 34.023], [-81.06, 34.024], [-81.062, 34.023], [-81.062, 34.022], [-81.063, 34.022]], [[-81.079, 34.031], [-81.079, 34.03], [-81.079, 34.031], [-81.079, 34.03], [-81.078, 34.03], [-81.077, 34.03], [-81.077, 34.029], [-81.077, 34.03], [-81.076, 34.03], [-81.075, 34.03], [-81.075, 34.031], [-81.075, 34.032], [-81.075, 34.033], [-81.077, 34.033], [-81.078, 34.033], [-81.078, 34.034], [-81.079, 34.033], [-81.078, 34.033], [-81.078, 34.032], [-81.078, 34.031], [-81.079, 34.031]], [[-81.024, 34.039], [-81.024, 34.038], [-81.024, 34.039], [-81.024, 34.038], [-81.024, 34.037], [-81.024, 34.036], [-81.023, 34.036], [-81.023, 34.037], [-81.022, 34.037], [-81.022, 34.038], [-81.022, 34.037], [-81.022, 34.036], [-81.021, 34.036], [-81.02, 34.036], [-81.02, 34.037], [-81.021, 34.037], [-81.02, 34.037], [-81.021, 34.038], [-81.021, 34.039], [-81.02, 34.039], [-81.021, 34.04], [-81.022, 34.04], [-81.022, 34.039], [-81.023, 34.039], [-81.023, 34.038], [-81.023, 34.039], [-81.024, 34.039], [-81.023, 34.039], [-81.024, 34.04], [-81.024, 34.039]], [[-81.147, 34.09], [-81.146, 34.089], [-81.145, 34.089], [-81.146, 34.089], [-81.145, 34.089], [-81.145, 34.088], [-81.144, 34.088], [-81.143, 34.088], [-81.142, 34.088], [-81.143, 34.088], [-81.142, 34.087], [-81.141, 34.087], [-81.141, 34.088], [-81.143, 34.09], [-81.145, 34.091], [-81.146, 34.091], [-81.146, 34.09], [-81.147, 34.09]], [[-80.95, 33.968], [-80.949, 33.967], [-80.948, 33.967], [-80.949, 33.968], [-80.948, 33.968], [-80.947, 33.968], [-80.948, 33.969], [-80.947, 33.968], [-80.947, 33.969], [-80.946, 33.969], [-80.947, 33.969], [-80.947, 33.97], [-80.946, 33.971], [-80.947, 33.971], [-80.948, 33.97], [-80.947, 33.971], [-80.948, 33.972], [-80.949, 33.972], [-80.948, 33.972], [-80.948, 33.971], [-80.949, 33.97], [-80.949, 33.971], [-80.95, 33.97], [-80.949, 33.97], [-80.949, 33.969], [-80.95, 33.969], [-80.95, 33.968]], [[-80.821, 34.108], [-80.82, 34.108], [-80.818, 34.109], [-80.817, 34.109], [-80.818, 34.111], [-80.818, 34.112], [-80.819, 34.112], [-80.82, 34.111], [-80.821, 34.108]], [[-80.982, 33.984], [-80.982, 33.983], [-80.98, 33.984], [-80.98, 33.983], [-80.977, 33.983], [-80.977, 33.984], [-80.977, 33.985], [-80.976, 33.985], [-80.976, 33.986], [-80.977, 33.986], [-80.977, 33.985], [-80.978, 33.985], [-80.981, 33.985], [-80.982, 33.985], [-80.982, 33.984]], [[-80.992, 33.976], [-80.992, 33.975], [-80.991, 33.975], [-80.991, 33.974], [-80.99, 33.974], [-80.99, 33.975], [-80.99, 33.976], [-80.99, 33.977], [-80.99, 33.978], [-80.991, 33.978], [-80.99, 33.978], [-80.991, 33.978], [-80.99, 33.978], [-80.99, 33.979], [-80.991, 33.979], [-80.992, 33.978], [-80.992, 33.977], [-80.992, 33.976], [-80.993, 33.976], [-80.992, 33.976]], [[-81.106, 34.03], [-81.106, 34.029], [-81.106, 34.027], [-81.105, 34.027], [-81.104, 34.027], [-81.104, 34.03], [-81.105, 34.03], [-81.106, 34.03]], [[-80.821, 34.119], [-80.82, 34.119], [-80.82, 34.118], [-80.82, 34.117], [-80.819, 34.116], [-80.819, 34.115], [-80.818, 34.115], [-80.817, 34.116], [-80.818, 34.116], [-80.818, 34.117], [-80.819, 34.117], [-80.819, 34.118], [-80.818, 34.118], [-80.819, 34.118], [-80.819, 34.119], [-80.82, 34.12], [-80.821, 34.12], [-80.821, 34.119]], [[-81.017, 34.036], [-81.016, 34.036], [-81.015, 34.036], [-81.015, 34.037], [-81.014, 34.037], [-81.014, 34.038], [-81.014, 34.039], [-81.013, 34.039], [-81.014, 34.039], [-81.013, 34.039], [-81.013, 34.04], [-81.014, 34.04], [-81.015, 34.04], [-81.015, 34.039], [-81.015, 34.038], [-81.016, 34.038], [-81.016, 34.037], [-81.017, 34.037], [-81.017, 34.036]], [[-81.092, 34.023], [-81.091, 34.022], [-81.092, 34.022], [-81.091, 34.021], [-81.09, 34.022], [-81.091, 34.023], [-81.09, 34.022], [-81.089, 34.021], [-81.089, 34.022], [-81.088, 34.022], [-81.089, 34.023], [-81.089, 34.022], [-81.089, 34.023], [-81.09, 34.023], [-81.089, 34.023], [-81.09, 34.024], [-81.089, 34.024], [-81.088, 34.024], [-81.089, 34.024], [-81.089, 34.025], [-81.089, 34.024], [-81.091, 34.023], [-81.092, 34.023]], [[-80.975, 33.942], [-80.975, 33.941], [-80.974, 33.942], [-80.976, 33.944], [-80.977, 33.943], [-80.975, 33.942]], [[-81.066, 34.027], [-81.066, 34.026], [-81.066, 34.027], [-81.065, 34.027], [-81.064, 34.027], [-81.063, 34.027], [-81.063, 34.028], [-81.064, 34.029], [-81.065, 34.028], [-81.066, 34.028], [-81.065, 34.027], [-81.066, 34.027]], [[-80.954, 33.977], [-80.953, 33.977], [-80.952, 33.975], [-80.951, 33.976], [-80.951, 33.977], [-80.952, 33.977], [-80.952, 33.978], [-80.953, 33.978], [-80.954, 33.978], [-80.954, 33.977]], [[-80.969, 33.958], [-80.968, 33.958], [-80.967, 33.958], [-80.965, 33.957], [-80.965, 33.958], [-80.967, 33.959], [-80.966, 33.959], [-80.968, 33.959], [-80.969, 33.958]], [[-81.094, 34.021], [-81.093, 34.021], [-81.094, 34.021], [-81.093, 34.021], [-81.093, 34.02], [-81.092, 34.02], [-81.092, 34.021], [-81.092, 34.022], [-81.093, 34.022], [-81.094, 34.022], [-81.094, 34.021]], [[-80.855, 34.091], [-80.855, 34.09], [-80.854, 34.09], [-80.854, 34.091], [-80.853, 34.091], [-80.852, 34.091], [-80.853, 34.092], [-80.854, 34.092], [-80.855, 34.092], [-80.855, 34.091]], [[-81.082, 34.014], [-81.08, 34.014], [-81.079, 34.015], [-81.079, 34.016], [-81.08, 34.016], [-81.08, 34.015], [-81.081, 34.016], [-81.082, 34.015], [-81.082, 34.014]], [[-80.868, 34.086], [-80.867, 34.086], [-80.866, 34.086], [-80.865, 34.086], [-80.865, 34.087], [-80.866, 34.087], [-80.868, 34.087], [-80.868, 34.086]], [[-80.96, 33.955], [-80.959, 33.955], [-80.959, 33.954], [-80.96, 33.954], [-80.958, 33.954], [-80.957, 33.955], [-80.958, 33.955], [-80.959, 33.955], [-80.96, 33.955]], [[-80.956, 33.989], [-80.955, 33.989], [-80.954, 33.99], [-80.954, 33.988], [-80.954, 33.989], [-80.954, 33.99], [-80.955, 33.991], [-80.955, 33.99], [-80.956, 33.989]], [[-81.065, 34.015], [-81.064, 34.015], [-81.063, 34.015], [-81.063, 34.016], [-81.063, 34.017], [-81.064, 34.017], [-81.065, 34.016], [-81.064, 34.016], [-81.065, 34.016], [-81.065, 34.015]], [[-80.864, 34.087], [-80.863, 34.087], [-80.86, 34.088], [-80.861, 34.088], [-80.862, 34.088], [-80.861, 34.088], [-80.862, 34.088], [-80.864, 34.087]], [[-80.98, 33.991], [-80.979, 33.99], [-80.978, 33.989], [-80.978, 33.99], [-80.979, 33.99], [-80.979, 33.991], [-80.98, 33.991], [-80.98, 33.992], [-80.979, 33.992], [-80.979, 33.991], [-80.979, 33.992], [-80.979, 33.993], [-80.98, 33.992], [-80.98, 33.991]], [[-80.954, 33.971], [-80.952, 33.971], [-80.951, 33.972], [-80.953, 33.972], [-80.954, 33.972], [-80.954, 33.971]], [[-81.025, 34.032], [-81.024, 34.032], [-81.025, 34.032], [-81.024, 34.032], [-81.024, 34.031], [-81.024, 34.032], [-81.024, 34.033], [-81.023, 34.033], [-81.023, 34.034], [-81.024, 34.034], [-81.025, 34.033], [-81.025, 34.032]], [[-80.999, 34.072], [-80.999, 34.071], [-80.998, 34.071], [-80.996, 34.072], [-80.997, 34.072], [-80.998, 34.072], [-80.999, 34.072]], [[-81.088, 34.018], [-81.088, 34.017], [-81.087, 34.017], [-81.086, 34.017], [-81.086, 34.018], [-81.087, 34.018], [-81.088, 34.018]], [[-80.95, 33.977], [-80.95, 33.976], [-80.948, 33.976], [-80.948, 33.977], [-80.95, 33.977]], [[-81.15, 34.076], [-81.149, 34.074], [-81.148, 34.075], [-81.148, 34.076], [-81.149, 34.076], [-81.15, 34.076], [-81.151, 34.076], [-81.15, 34.076]], [[-81.074, 34.024], [-81.074, 34.023], [-81.073, 34.023], [-81.073, 34.024], [-81.074, 34.024], [-81.073, 34.024], [-81.072, 34.024], [-81.072, 34.025], [-81.072, 34.024], [-81.072, 34.025], [-81.073, 34.025], [-81.073, 34.024], [-81.074, 34.025], [-81.074, 34.024]], [[-81.142, 34.087], [-81.141, 34.086], [-81.14, 34.086], [-81.14, 34.087], [-81.141, 34.087], [-81.142, 34.087]], [[-80.872, 34.09], [-80.872, 34.089], [-80.87, 34.09], [-80.87, 34.091], [-80.872, 34.09]], [[-81.158, 34.074], [-81.157, 34.073], [-81.157, 34.074], [-81.156, 34.074], [-81.156, 34.075], [-81.157, 34.074], [-81.158, 34.074]], [[-80.904, 33.977], [-80.904, 33.976], [-80.903, 33.976], [-80.903, 33.977], [-80.903, 33.978], [-80.904, 33.978], [-80.904, 33.977]], [[-81.089, 34.019], [-81.088, 34.019], [-81.087, 34.019], [-81.087, 34.02], [-81.088, 34.02], [-81.088, 34.019], [-81.089, 34.019]], [[-80.983, 33.948], [-80.982, 33.948], [-80.982, 33.949], [-80.983, 33.949], [-80.983, 33.948]], [[-81.012, 34.03], [-81.011, 34.03], [-81.01, 34.03], [-81.01, 34.031], [-81.011, 34.031], [-81.012, 34.031], [-81.012, 34.03]], [[-80.964, 33.956], [-80.963, 33.956], [-80.962, 33.956], [-80.963, 33.956], [-80.964, 33.957], [-80.964, 33.956]], [[-81.157, 34.073], [-81.156, 34.073], [-81.156, 34.072], [-81.155, 34.072], [-81.156, 34.073], [-81.157, 34.073]], [[-80.951, 33.965], [-80.952, 33.964], [-80.951, 33.964], [-80.95, 33.964], [-80.95, 33.965], [-80.951, 33.965]], [[-80.981, 33.989], [-80.98, 33.989], [-80.98, 33.99], [-80.981, 33.99], [-80.981, 33.989]], [[-80.96, 34.012], [-80.96, 34.011], [-80.959, 34.011], [-80.959, 34.012], [-80.96, 34.012]], [[-80.816, 34.121], [-80.816, 34.12], [-80.816, 34.121], [-80.815, 34.121], [-80.815, 34.122], [-80.816, 34.121]], [[-81.079, 34.029], [-81.078, 34.029], [-81.078, 34.028], [-81.077, 34.029], [-81.078, 34.029], [-81.079, 34.029]], [[-80.975, 33.986], [-80.974, 33.986], [-80.973, 33.986], [-80.973, 33.987], [-80.974, 33.987], [-80.975, 33.987], [-80.975, 33.986]], [[-80.834, 34.114], [-80.833, 34.114], [-80.832, 34.114], [-80.831, 34.114], [-80.832, 34.114], [-80.833, 34.114], [-80.834, 34.114]], [[-81.076, 34.016], [-81.075, 34.016], [-81.074, 34.016], [-81.075, 34.016], [-81.076, 34.017], [-81.076, 34.016]], [[-81.024, 33.973], [-81.024, 33.972], [-81.023, 33.972], [-81.024, 33.972], [-81.023, 33.972], [-81.023, 33.973], [-81.023, 33.974], [-81.024, 33.973]], [[-81.026, 34.029], [-81.025, 34.03], [-81.025, 34.031], [-81.026, 34.03], [-81.026, 34.029]], [[-80.981, 33.974], [-80.98, 33.974], [-80.98, 33.975], [-80.981, 33.975], [-80.981, 33.974]], [[-81.078, 34.015], [-81.077, 34.015], [-81.077, 34.016], [-81.078, 34.016], [-81.078, 34.015]], [[-80.833, 34.118], [-80.832, 34.118], [-80.832, 34.119], [-80.833, 34.119], [-80.833, 34.118]], [[-80.952, 33.962], [-80.951, 33.962], [-80.951, 33.963], [-80.952, 33.963], [-80.952, 33.962]], [[-80.975, 33.989], [-80.974, 33.989], [-80.974, 33.99], [-80.975, 33.99], [-80.975, 33.989]], [[-80.94, 33.973], [-80.94, 33.974], [-80.941, 33.974], [-80.941, 33.973], [-80.94, 33.973]], [[-81.167, 34.067], [-81.166, 34.067], [-81.166, 34.068], [-81.167, 34.067]], [[-80.819, 34.129], [-80.818, 34.129], [-80.818, 34.13], [-80.819, 34.13], [-80.819, 34.129]], [[-81.16, 34.096], [-81.16, 34.095], [-81.159, 34.096], [-81.16, 34.096]], [[-80.951, 33.977], [-80.95, 33.977], [-80.95, 33.978], [-80.951, 33.977]], [[-80.956, 33.98], [-80.956, 33.979], [-80.955, 33.979], [-80.955, 33.98], [-80.956, 33.98]], [[-81.058, 34.044], [-81.058, 34.043], [-81.057, 34.043], [-81.057, 34.044], [-81.058, 34.044]], [[-81.059, 34.025], [-81.058, 34.025], [-81.058, 34.026], [-81.059, 34.025]], [[-81.159, 34.073], [-81.158, 34.073], [-81.158, 34.074], [-81.159, 34.073]], [[-80.972, 33.987], [-80.972, 33.986], [-80.971, 33.987], [-80.972, 33.987]], [[-81.155, 34.075], [-81.155, 34.074], [-81.154, 34.074], [-81.155, 34.075]], [[-81.08, 34.03], [-81.08, 34.029], [-81.079, 34.029], [-81.08, 34.03]], [[-81.016885, 34.044016], [-81.016868, 34.043995], [-81.016817, 34.043932], [-81.0168, 34.043912], [-81.016772, 34.043877], [-81.016689, 34.043775], [-81.016673, 34.043755], [-81.016662, 34.043741], [-81.016599, 34.043664], [-81.016533, 34.0437], [-81.016211, 34.043865], [-81.015919, 34.04401], [-81.016031, 34.044146], [-81.016134, 34.044272], [-81.016243, 34.044399], [-81.016633, 34.044166], [-81.016885, 34.044016]], [[-80.830989, 34.117888], [-80.830988, 34.117879], [-80.830982, 34.117822], [-80.830943, 34.117772], [-80.83081, 34.117723], [-80.830536, 34.11778], [-80.830497, 34.117789], [-80.830394, 34.117811], [-80.830359, 34.117811], [-80.830353, 34.117882], [-80.830306, 34.1179], [-80.830107, 34.117855], [-80.829973, 34.118228], [-80.829997, 34.118234], [-80.830021, 34.118244], [-80.830049, 34.118262], [-80.830069, 34.11828], [-80.830387, 34.118181], [-80.830427, 34.118184], [-80.830722, 34.118094], [-80.830907, 34.118039], [-80.830916, 34.118031], [-80.830989, 34.117888]], [[-81.009639, 34.029661], [-81.009546, 34.02963], [-81.009439, 34.029594], [-81.009383, 34.029576], [-81.009265, 34.029537], [-81.009075, 34.029903], [-81.009117, 34.029944], [-81.00953, 34.03034], [-81.00974, 34.029875], [-81.009812, 34.029718], [-81.009639, 34.029661]], [[-80.965423, 33.98101], [-80.96504, 33.980807], [-80.96486, 33.980712], [-80.964805, 33.980772], [-80.964641, 33.980955], [-80.964587, 33.981016], [-80.964681, 33.981074], [-80.964963, 33.98125], [-80.965058, 33.981309], [-80.965189, 33.981408], [-80.965223, 33.981368], [-80.96549, 33.981051], [-80.965423, 33.98101]], [[-80.8184, 34.113218], [-80.818389, 34.113206], [-80.81838, 34.113191], [-80.818372, 34.113173], [-80.818368, 34.113154], [-80.818367, 34.113138], [-80.818371, 34.113118], [-80.81777, 34.113052], [-80.817649, 34.113036], [-80.817563, 34.113353], [-80.817541, 34.113432], [-80.817535, 34.113468], [-80.817723, 34.113538], [-80.817856, 34.11359], [-80.818412, 34.113225], [-80.8184, 34.113218]], [[-81.016, 34.034], [-81.015, 34.034], [-81.015, 34.035], [-81.016, 34.035], [-81.016, 34.034]], [[-81.016, 34.042], [-81.015, 34.042], [-81.016, 34.043], [-81.016, 34.042]], [[-81.03, 34.025], [-81.029, 34.024], [-81.029, 34.025], [-81.03, 34.025]], [[-81.028519, 34.064386], [-81.028515, 34.064326], [-81.028524, 34.064048], [-81.028538, 34.063926], [-81.028291, 34.063926], [-81.028046, 34.063918], [-81.027723, 34.064364], [-81.028172, 34.064377], [-81.028316, 34.06438], [-81.028519, 34.064386]], [[-80.892398, 33.984457], [-80.892369, 33.984342], [-80.892315, 33.98415], [-80.891574, 33.984302], [-80.891584, 33.984334], [-80.891612, 33.984433], [-80.891627, 33.984484], [-80.891663, 33.984612], [-80.892398, 33.984457]], [[-81.095073, 34.021445], [-81.094575, 34.021751], [-81.09461, 34.021792], [-81.094638, 34.021832], [-81.094657, 34.021862], [-81.09468, 34.021905], [-81.094695, 34.02194], [-81.094705, 34.021965], [-81.09472, 34.022015], [-81.094731, 34.02207], [-81.094735, 34.022106], [-81.095329, 34.021737], [-81.095073, 34.021445]], [[-80.969464, 33.96971], [-80.9693, 33.969708], [-80.968808, 33.969697], [-80.968693, 33.969694], [-80.968694, 33.969728], [-80.968695, 33.969774], [-80.968696, 33.969816], [-80.968699, 33.970002], [-80.969464, 33.970012], [-80.969464, 33.96971]], [[-81.02, 33.984], [-81.019, 33.984], [-81.019, 33.985], [-81.02, 33.985], [-81.02, 33.984]], [[-81.028, 34.026], [-81.027, 34.026], [-81.027, 34.027], [-81.028, 34.027], [-81.028, 34.026]], [[-80.819865, 34.128243], [-80.819766, 34.128075], [-80.819673, 34.12792], [-80.819266, 34.128082], [-80.819361, 34.128234], [-80.819517, 34.128484], [-80.819594, 34.128452], [-80.819657, 34.128423], [-80.819789, 34.128368], [-80.81991, 34.128317], [-80.819865, 34.128243]], [[-81.05688, 34.044108], [-81.056854, 34.04407], [-81.056824, 34.044025], [-81.056807, 34.044], [-81.056774, 34.043925], [-81.056761, 34.043878], [-81.056748, 34.043827], [-81.056685, 34.043837], [-81.056497, 34.043867], [-81.056435, 34.043878], [-81.056393, 34.043884], [-81.056376, 34.043888], [-81.056339, 34.043897], [-81.056271, 34.043913], [-81.056329, 34.044127], [-81.056393, 34.044338], [-81.056729, 34.04419], [-81.056889, 34.044122], [-81.05688, 34.044108]], [[-80.817, 34.115], [-80.817, 34.114], [-80.816, 34.114], [-80.816, 34.115], [-80.817, 34.115]], [[-80.921706, 33.97204], [-80.921679, 33.971875], [-80.921043, 33.971944], [-80.921074, 33.971978], [-80.921155, 33.972063], [-80.921189, 33.972113], [-80.921223, 33.972154], [-80.921281, 33.972227], [-80.921365, 33.972334], [-80.921636, 33.972206], [-80.921721, 33.972166], [-80.921706, 33.97204]], [[-81.080266, 34.024702], [-81.080026, 34.025155], [-81.080095, 34.025178], [-81.080342, 34.025263], [-81.080486, 34.025002], [-81.080582, 34.024825], [-81.080266, 34.024702]], [[-81.054, 34.025], [-81.054, 34.026], [-81.055, 34.026], [-81.054, 34.025]], [[-81.065384, 34.012832], [-81.065279, 34.012752], [-81.064887, 34.013119], [-81.06486, 34.01315], [-81.064939, 34.013212], [-81.064984, 34.013248], [-81.065061, 34.013309], [-81.065078, 34.013323], [-81.065486, 34.01291], [-81.065384, 34.012832]], [[-80.954052, 33.974779], [-80.95358, 33.974852], [-80.953509, 33.974973], [-80.953358, 33.975228], [-80.953619, 33.975199], [-80.953981, 33.97494], [-80.953996, 33.974927], [-80.954052, 33.974779]], [[-81.043356, 34.050796], [-81.043303, 34.050634], [-81.042711, 34.050649], [-81.042801, 34.05092], [-81.043356, 34.050796]], [[-81.020675, 33.984893], [-81.020547, 33.984792], [-81.020487, 33.984746], [-81.020123, 33.985055], [-81.020185, 33.985103], [-81.020206, 33.98512], [-81.02023, 33.985138], [-81.020302, 33.985194], [-81.020326, 33.985213], [-81.020428, 33.985119], [-81.020675, 33.984893]], [[-80.986, 33.988], [-80.986, 33.987], [-80.985, 33.987], [-80.985, 33.988], [-80.986, 33.988]], [[-81.054069, 34.025187], [-81.053937, 34.025089], [-81.053564, 34.025455], [-81.053513, 34.02551], [-81.053619, 34.025587], [-81.053634, 34.025598], [-81.054069, 34.025187]], [[-81.062638, 34.027846], [-81.062585, 34.027804], [-81.062495, 34.027723], [-81.062436, 34.027661], [-81.06238, 34.027597], [-81.062361, 34.027571], [-81.062317, 34.027513], [-81.062153, 34.027569], [-81.062391, 34.027944], [-81.062514, 34.027898], [-81.062638, 34.027846]], [[-81.023105, 33.973605], [-81.022943, 33.973466], [-81.022618, 33.973714], [-81.022769, 33.973862], [-81.022781, 33.973852], [-81.022883, 33.973773], [-81.023085, 33.97362], [-81.023105, 33.973605]], [[-81.053444, 34.024739], [-81.052983, 34.025128], [-81.053113, 34.025222], [-81.053169, 34.025176], [-81.053545, 34.024812], [-81.053548, 34.024809], [-81.053444, 34.024739]], [[-80.96545, 33.981545], [-80.965165, 33.981731], [-80.964852, 33.981933], [-80.964941, 33.981984], [-80.965006, 33.981943], [-80.965544, 33.981606], [-80.96545, 33.981545]], [[-81.07206, 34.047913], [-81.072041, 34.047864], [-81.071999, 34.047803], [-81.071744, 34.047915], [-81.071859, 34.048077], [-81.07209, 34.047979], [-81.07206, 34.047913]], [[-81.009, 34.029], [-81.008, 34.029], [-81.009, 34.03], [-81.009, 34.029]], [[-81.066151, 34.013285], [-81.065887, 34.013098], [-81.065835, 34.013118], [-81.065822, 34.013161], [-81.066082, 34.013351], [-81.066151, 34.013285]], [[-81.022795, 34.034569], [-81.022755, 34.034569], [-81.022682, 34.034568], [-81.02266, 34.03475], [-81.02276, 34.034619], [-81.022795, 34.034569]]], [[[-80.97, 33.979], [-80.969, 33.979], [-80.969, 33.978], [-80.97, 33.978], [-80.97, 33.979]]]] } }] };
                geojson.features[0].geometry = JSON.parse(data);
                allow = true;
                LoadMapUpdate();
            },
            error: function () {

            }
        });
    }   
}
function BindDataByState() {
    var value = $('#q').val().trim();

    if (value != "") {
        $.ajax({
            url: '/ERSEA/GetgeometryByState',
            type: 'post',
            async: false,
            data: { 'State': value },
            success: function (data) {
                console.log(JSON.parse(data));
                geojson = { "type": "FeatureCollection", "features": [{ "type": "Feature", "properties": { "fillOpacity": 0, "strokeColor": "#FF0000", "strokeOpacity": 0.5 }, "geometry": { "type": "MultiPolygon", "coordinates": [[[[-80.997985, 34.046995], [-80.997463, 34.047383], [-80.997399, 34.047439], [-80.997334, 34.047363], [-80.9973, 34.047323], [-80.997261, 34.047214], [-80.997242, 34.047162], [-80.997216, 34.046987], [-80.997216, 34.046978], [-80.9973, 34.046978], [-80.997985, 34.046995]]], [[[-81.01, 34.066], [-81.009, 34.066], [-81.009, 34.067], [-81.008, 34.067], [-81.007, 34.065], [-81.008, 34.064], [-81.009, 34.064], [-81.01, 34.066]]], [[[-81.031, 34.061], [-81.03, 34.062], [-81.03, 34.061], [-81.031, 34.061]]], [[[-81.097, 34.034], [-81.097, 34.033], [-81.098, 34.033], [-81.098, 34.034], [-81.097, 34.034]]], [[[-81.142, 34.082], [-81.141, 34.083], [-81.141, 34.084], [-81.141, 34.085], [-81.14, 34.085], [-81.14, 34.084], [-81.139, 34.084], [-81.138, 34.084], [-81.138, 34.083], [-81.139, 34.083], [-81.139, 34.082], [-81.14, 34.082], [-81.142, 34.082]]], [[[-81.171, 34.091], [-81.17, 34.091], [-81.169, 34.091], [-81.168, 34.091], [-81.168, 34.092], [-81.168, 34.093], [-81.167, 34.093], [-81.166, 34.093], [-81.165, 34.093], [-81.166, 34.093], [-81.165, 34.094], [-81.165, 34.093], [-81.164, 34.093], [-81.164, 34.094], [-81.163, 34.094], [-81.163, 34.093], [-81.163, 34.092], [-81.162, 34.092], [-81.161, 34.092], [-81.16, 34.092], [-81.16, 34.093], [-81.161, 34.093], [-81.162, 34.094], [-81.163, 34.094], [-81.162, 34.094], [-81.162, 34.095], [-81.161, 34.094], [-81.16, 34.094], [-81.16, 34.095], [-81.16, 34.096], [-81.161, 34.095], [-81.161, 34.096], [-81.161, 34.097], [-81.162, 34.097], [-81.161, 34.098], [-81.162, 34.098], [-81.162, 34.097], [-81.163, 34.097], [-81.163, 34.099], [-81.163, 34.1], [-81.162, 34.102], [-81.162, 34.101], [-81.161, 34.101], [-81.16, 34.101], [-81.16, 34.1], [-81.16, 34.099], [-81.161, 34.099], [-81.162, 34.099], [-81.16, 34.099], [-81.16, 34.097], [-81.159, 34.097], [-81.159, 34.098], [-81.159, 34.097], [-81.158, 34.098], [-81.157, 34.098], [-81.156, 34.097], [-81.155, 34.097], [-81.155, 34.096], [-81.156, 34.097], [-81.157, 34.097], [-81.156, 34.096], [-81.156, 34.095], [-81.157, 34.096], [-81.157, 34.095], [-81.157, 34.094], [-81.156, 34.094], [-81.156, 34.093], [-81.156, 34.092], [-81.157, 34.092], [-81.157, 34.091], [-81.157, 34.09], [-81.158, 34.09], [-81.158, 34.089], [-81.157, 34.089], [-81.156, 34.089], [-81.155, 34.089], [-81.154, 34.089], [-81.153, 34.092], [-81.151, 34.093], [-81.15, 34.094], [-81.149, 34.095], [-81.148, 34.096], [-81.147, 34.096], [-81.147, 34.097], [-81.146, 34.098], [-81.145, 34.099], [-81.144, 34.1], [-81.142, 34.102], [-81.14, 34.103], [-81.14, 34.104], [-81.14, 34.105], [-81.14, 34.104], [-81.139, 34.104], [-81.139, 34.105], [-81.139, 34.106], [-81.138, 34.106], [-81.137, 34.106], [-81.138, 34.106], [-81.138, 34.107], [-81.138, 34.106], [-81.137, 34.106], [-81.137, 34.107], [-81.137, 34.108], [-81.136, 34.108], [-81.135, 34.108], [-81.135, 34.107], [-81.135, 34.108], [-81.136, 34.108], [-81.136, 34.109], [-81.135, 34.109], [-81.135, 34.11], [-81.134, 34.109], [-81.133, 34.109], [-81.131, 34.112], [-81.13, 34.112], [-81.13, 34.113], [-81.131, 34.113], [-81.131, 34.114], [-81.132, 34.114], [-81.132, 34.115], [-81.133, 34.115], [-81.133, 34.114], [-81.133, 34.113], [-81.134, 34.113], [-81.135, 34.113], [-81.135, 34.114], [-81.136, 34.114], [-81.138, 34.115], [-81.138, 34.116], [-81.139, 34.116], [-81.139, 34.117], [-81.139, 34.118], [-81.138, 34.118], [-81.137, 34.119], [-81.137, 34.12], [-81.137, 34.121], [-81.138, 34.121], [-81.138, 34.12], [-81.139, 34.12], [-81.14, 34.12], [-81.141, 34.12], [-81.142, 34.12], [-81.143, 34.12], [-81.144, 34.119], [-81.143, 34.119], [-81.142, 34.119], [-81.142, 34.118], [-81.143, 34.117], [-81.143, 34.116], [-81.143, 34.115], [-81.144, 34.115], [-81.144, 34.114], [-81.145, 34.114], [-81.146, 34.114], [-81.146, 34.115], [-81.146, 34.116], [-81.146, 34.117], [-81.146, 34.118], [-81.146, 34.119], [-81.145, 34.119], [-81.146, 34.119], [-81.146, 34.12], [-81.147, 34.121], [-81.147, 34.12], [-81.148, 34.12], [-81.149, 34.12], [-81.15, 34.12], [-81.15, 34.121], [-81.15, 34.122], [-81.15, 34.123], [-81.15, 34.122], [-81.151, 34.123], [-81.151, 34.124], [-81.152, 34.125], [-81.153, 34.124], [-81.154, 34.123], [-81.154, 34.124], [-81.155, 34.125], [-81.155, 34.126], [-81.154, 34.126], [-81.153, 34.126], [-81.153, 34.127], [-81.152, 34.127], [-81.151, 34.127], [-81.152, 34.128], [-81.152, 34.129], [-81.153, 34.13], [-81.151, 34.131], [-81.149, 34.131], [-81.149, 34.132], [-81.147, 34.132], [-81.146, 34.132], [-81.145, 34.132], [-81.144, 34.132], [-81.142, 34.132], [-81.139, 34.133], [-81.138, 34.133], [-81.137, 34.133], [-81.137, 34.132], [-81.136, 34.132], [-81.136, 34.131], [-81.136, 34.13], [-81.135, 34.13], [-81.135, 34.129], [-81.135, 34.128], [-81.134, 34.128], [-81.134, 34.127], [-81.133, 34.127], [-81.133, 34.126], [-81.132, 34.126], [-81.132, 34.125], [-81.131, 34.125], [-81.132, 34.125], [-81.133, 34.124], [-81.135, 34.123], [-81.137, 34.121], [-81.137, 34.12], [-81.137, 34.119], [-81.137, 34.118], [-81.138, 34.118], [-81.138, 34.117], [-81.139, 34.117], [-81.139, 34.116], [-81.138, 34.116], [-81.138, 34.115], [-81.137, 34.115], [-81.136, 34.114], [-81.135, 34.113], [-81.134, 34.113], [-81.134, 34.114], [-81.133, 34.115], [-81.132, 34.115], [-81.132, 34.114], [-81.131, 34.114], [-81.131, 34.113], [-81.13, 34.113], [-81.129, 34.113], [-81.129, 34.112], [-81.128, 34.112], [-81.128, 34.111], [-81.127, 34.111], [-81.126, 34.112], [-81.125, 34.112], [-81.124, 34.112], [-81.123, 34.112], [-81.123, 34.111], [-81.122, 34.111], [-81.122, 34.112], [-81.121, 34.112], [-81.12, 34.112], [-81.12, 34.113], [-81.12, 34.112], [-81.119, 34.112], [-81.119, 34.111], [-81.119, 34.11], [-81.119, 34.109], [-81.119, 34.108], [-81.118, 34.108], [-81.118, 34.107], [-81.118, 34.106], [-81.117, 34.106], [-81.117, 34.105], [-81.116, 34.105], [-81.115, 34.105], [-81.114, 34.104], [-81.113, 34.104], [-81.112, 34.104], [-81.112, 34.103], [-81.111, 34.103], [-81.11, 34.102], [-81.11, 34.101], [-81.109, 34.101], [-81.109, 34.1], [-81.108, 34.099], [-81.108, 34.098], [-81.108, 34.097], [-81.107, 34.097], [-81.107, 34.096], [-81.107, 34.095], [-81.106, 34.095], [-81.106, 34.094], [-81.106, 34.093], [-81.105, 34.092], [-81.105, 34.091], [-81.104, 34.091], [-81.104, 34.09], [-81.103, 34.089], [-81.102, 34.089], [-81.102, 34.088], [-81.101, 34.088], [-81.1, 34.088], [-81.1, 34.087], [-81.099, 34.087], [-81.098, 34.087], [-81.098, 34.086], [-81.097, 34.086], [-81.097, 34.085], [-81.096, 34.085], [-81.095, 34.085], [-81.095, 34.084], [-81.094, 34.084], [-81.094, 34.083], [-81.093, 34.083], [-81.093, 34.082], [-81.092, 34.082], [-81.091, 34.081], [-81.09, 34.081], [-81.09, 34.08], [-81.089, 34.08], [-81.088, 34.08], [-81.088, 34.079], [-81.088, 34.078], [-81.087, 34.078], [-81.087, 34.077], [-81.087, 34.076], [-81.086, 34.076], [-81.086, 34.075], [-81.086, 34.074], [-81.085, 34.074], [-81.085, 34.073], [-81.084, 34.073], [-81.083, 34.073], [-81.082, 34.073], [-81.076, 34.078], [-81.075, 34.079], [-81.074, 34.079], [-81.072, 34.08], [-81.071, 34.081], [-81.071, 34.082], [-81.07, 34.081], [-81.069, 34.081], [-81.068, 34.081], [-81.069, 34.081], [-81.069, 34.08], [-81.068, 34.08], [-81.067, 34.079], [-81.067, 34.078], [-81.066, 34.078], [-81.068, 34.077], [-81.067, 34.075], [-81.066, 34.074], [-81.068, 34.072], [-81.067, 34.072], [-81.067, 34.071], [-81.068, 34.072], [-81.069, 34.071], [-81.071, 34.069], [-81.073, 34.068], [-81.072, 34.068], [-81.073, 34.068], [-81.073, 34.067], [-81.073, 34.068], [-81.076, 34.065], [-81.078, 34.064], [-81.078, 34.063], [-81.077, 34.063], [-81.077, 34.062], [-81.077, 34.061], [-81.076, 34.061], [-81.076, 34.06], [-81.076, 34.059], [-81.075, 34.059], [-81.075, 34.058], [-81.075, 34.057], [-81.075, 34.056], [-81.074, 34.056], [-81.074, 34.055], [-81.074, 34.054], [-81.074, 34.053], [-81.074, 34.052], [-81.073, 34.052], [-81.073, 34.051], [-81.073, 34.05], [-81.073, 34.049], [-81.072, 34.048], [-81.072, 34.049], [-81.071, 34.049], [-81.069, 34.05], [-81.069, 34.049], [-81.068, 34.049], [-81.068, 34.05], [-81.068, 34.048], [-81.068, 34.047], [-81.069, 34.046], [-81.069, 34.045], [-81.069, 34.044], [-81.068, 34.042], [-81.068, 34.041], [-81.068, 34.039], [-81.067, 34.039], [-81.066, 34.039], [-81.067, 34.039], [-81.067, 34.038], [-81.068, 34.038], [-81.068, 34.037], [-81.068, 34.035], [-81.067, 34.034], [-81.067, 34.033], [-81.067, 34.032], [-81.068, 34.032], [-81.067, 34.032], [-81.066, 34.033], [-81.066, 34.034], [-81.065, 34.034], [-81.064, 34.035], [-81.064, 34.034], [-81.062, 34.033], [-81.062, 34.032], [-81.063, 34.032], [-81.064, 34.032], [-81.065, 34.031], [-81.064, 34.03], [-81.065, 34.03], [-81.064, 34.029], [-81.063, 34.029], [-81.062, 34.029], [-81.062, 34.03], [-81.062, 34.029], [-81.062, 34.028], [-81.061, 34.028], [-81.061, 34.027], [-81.06, 34.028], [-81.059, 34.028], [-81.058, 34.029], [-81.057, 34.029], [-81.057, 34.03], [-81.058, 34.03], [-81.059, 34.03], [-81.06, 34.029], [-81.059, 34.03], [-81.06, 34.03], [-81.06, 34.031], [-81.059, 34.032], [-81.059, 34.033], [-81.059, 34.034], [-81.06, 34.034], [-81.06, 34.035], [-81.059, 34.035], [-81.059, 34.036], [-81.059, 34.037], [-81.06, 34.036], [-81.06, 34.037], [-81.06, 34.038], [-81.059, 34.038], [-81.059, 34.039], [-81.059, 34.04], [-81.058, 34.041], [-81.058, 34.042], [-81.06, 34.042], [-81.061, 34.042], [-81.061, 34.041], [-81.062, 34.041], [-81.062, 34.042], [-81.062, 34.043], [-81.062, 34.044], [-81.061, 34.045], [-81.062, 34.045], [-81.062, 34.046], [-81.061, 34.046], [-81.058, 34.045], [-81.057, 34.045], [-81.057, 34.044], [-81.058, 34.044], [-81.057, 34.044], [-81.057, 34.045], [-81.056, 34.045], [-81.056, 34.044], [-81.056, 34.045], [-81.055, 34.045], [-81.055, 34.046], [-81.054, 34.046], [-81.054, 34.047], [-81.053, 34.047], [-81.052, 34.047], [-81.052, 34.048], [-81.053, 34.048], [-81.052, 34.048], [-81.051, 34.048], [-81.051, 34.049], [-81.05, 34.049], [-81.049, 34.049], [-81.048, 34.05], [-81.048, 34.051], [-81.047, 34.051], [-81.047, 34.05], [-81.046, 34.05], [-81.045, 34.05], [-81.045, 34.051], [-81.045, 34.05], [-81.045, 34.051], [-81.044, 34.051], [-81.043, 34.051], [-81.044, 34.051], [-81.043, 34.051], [-81.042, 34.051], [-81.041, 34.051], [-81.04, 34.051], [-81.039, 34.051], [-81.039, 34.05], [-81.038, 34.05], [-81.037, 34.05], [-81.036, 34.05], [-81.037, 34.05], [-81.037, 34.051], [-81.036, 34.051], [-81.037, 34.052], [-81.036, 34.052], [-81.036, 34.053], [-81.036, 34.054], [-81.036, 34.053], [-81.037, 34.054], [-81.036, 34.054], [-81.037, 34.055], [-81.037, 34.056], [-81.038, 34.056], [-81.039, 34.056], [-81.039, 34.057], [-81.038, 34.057], [-81.039, 34.057], [-81.039, 34.058], [-81.04, 34.058], [-81.04, 34.057], [-81.04, 34.058], [-81.041, 34.058], [-81.042, 34.058], [-81.042, 34.057], [-81.042, 34.058], [-81.042, 34.059], [-81.043, 34.059], [-81.043, 34.06], [-81.044, 34.059], [-81.044, 34.06], [-81.044, 34.061], [-81.045, 34.061], [-81.045, 34.062], [-81.046, 34.062], [-81.046, 34.063], [-81.047, 34.063], [-81.047, 34.064], [-81.048, 34.064], [-81.049, 34.064], [-81.049, 34.065], [-81.048, 34.065], [-81.048, 34.066], [-81.047, 34.066], [-81.046, 34.066], [-81.045, 34.065], [-81.044, 34.065], [-81.044, 34.064], [-81.044, 34.063], [-81.043, 34.063], [-81.042, 34.064], [-81.042, 34.063], [-81.041, 34.063], [-81.04, 34.064], [-81.039, 34.062], [-81.036, 34.062], [-81.035, 34.062], [-81.034, 34.063], [-81.033, 34.063], [-81.032, 34.063], [-81.031, 34.063], [-81.032, 34.061], [-81.033, 34.062], [-81.034, 34.062], [-81.036, 34.062], [-81.036, 34.061], [-81.036, 34.06], [-81.036, 34.059], [-81.035, 34.059], [-81.034, 34.059], [-81.033, 34.059], [-81.033, 34.058], [-81.032, 34.059], [-81.031, 34.059], [-81.03, 34.059], [-81.03, 34.06], [-81.028, 34.06], [-81.028, 34.061], [-81.027, 34.061], [-81.029, 34.061], [-81.03, 34.061], [-81.03, 34.062], [-81.028, 34.064], [-81.029, 34.064], [-81.03, 34.064], [-81.03, 34.063], [-81.03, 34.064], [-81.031, 34.064], [-81.033, 34.064], [-81.034, 34.064], [-81.033, 34.064], [-81.034, 34.065], [-81.034, 34.064], [-81.035, 34.064], [-81.036, 34.064], [-81.037, 34.064], [-81.037, 34.065], [-81.037, 34.066], [-81.037, 34.067], [-81.038, 34.067], [-81.038, 34.068], [-81.038, 34.069], [-81.039, 34.072], [-81.042, 34.072], [-81.043, 34.072], [-81.044, 34.072], [-81.043, 34.072], [-81.043, 34.073], [-81.042, 34.073], [-81.041, 34.073], [-81.041, 34.074], [-81.04, 34.074], [-81.041, 34.074], [-81.041, 34.075], [-81.04, 34.075], [-81.04, 34.076], [-81.039, 34.076], [-81.039, 34.077], [-81.038, 34.077], [-81.037, 34.077], [-81.037, 34.078], [-81.036, 34.077], [-81.036, 34.07], [-81.036, 34.068], [-81.035, 34.067], [-81.035, 34.066], [-81.034, 34.066], [-81.033, 34.066], [-81.032, 34.066], [-81.031, 34.066], [-81.029, 34.066], [-81.028, 34.066], [-81.027, 34.066], [-81.026, 34.066], [-81.025, 34.066], [-81.024, 34.066], [-81.023, 34.067], [-81.022, 34.067], [-81.022, 34.066], [-81.023, 34.065], [-81.023, 34.064], [-81.023, 34.063], [-81.024, 34.063], [-81.025, 34.063], [-81.025, 34.062], [-81.025, 34.061], [-81.026, 34.061], [-81.025, 34.06], [-81.024, 34.061], [-81.024, 34.062], [-81.023, 34.062], [-81.022, 34.061], [-81.022, 34.06], [-81.021, 34.06], [-81.021, 34.059], [-81.02, 34.059], [-81.019, 34.059], [-81.02, 34.058], [-81.019, 34.058], [-81.018, 34.058], [-81.018, 34.059], [-81.017, 34.058], [-81.017, 34.059], [-81.017, 34.058], [-81.016, 34.059], [-81.016, 34.06], [-81.015, 34.06], [-81.016, 34.059], [-81.016, 34.058], [-81.015, 34.058], [-81.015, 34.059], [-81.015, 34.058], [-81.016, 34.058], [-81.015, 34.058], [-81.015, 34.057], [-81.014, 34.057], [-81.014, 34.058], [-81.013, 34.058], [-81.012, 34.058], [-81.011, 34.059], [-81.01, 34.059], [-81.01, 34.06], [-81.009, 34.061], [-81.01, 34.062], [-81.01, 34.063], [-81.009, 34.062], [-81.009, 34.063], [-81.008, 34.064], [-81.007, 34.065], [-81.007, 34.066], [-81.006, 34.067], [-81.006, 34.068], [-81.005, 34.068], [-81.005, 34.069], [-81.004, 34.069], [-81.003, 34.07], [-81.002, 34.07], [-81.003, 34.071], [-81.004, 34.071], [-81.005, 34.071], [-81.004, 34.072], [-81.005, 34.073], [-81.006, 34.074], [-81.005, 34.074], [-81.004, 34.074], [-81.003, 34.074], [-81.002, 34.074], [-81.001, 34.074], [-81, 34.074], [-81, 34.075], [-81, 34.076], [-80.999, 34.075], [-80.999, 34.076], [-80.998, 34.076], [-80.998, 34.077], [-80.996, 34.077], [-80.995, 34.077], [-80.996, 34.077], [-80.996, 34.078], [-80.993, 34.078], [-80.992, 34.078], [-80.992, 34.082], [-80.993, 34.082], [-80.993, 34.081], [-80.994, 34.081], [-80.995, 34.081], [-80.995, 34.08], [-80.996, 34.08], [-80.997, 34.08], [-80.996, 34.082], [-80.996, 34.083], [-80.996, 34.084], [-80.996, 34.085], [-80.996, 34.086], [-80.992, 34.085], [-80.99, 34.086], [-80.989, 34.086], [-80.988, 34.087], [-80.987, 34.088], [-80.99, 34.089], [-80.991, 34.09], [-80.99, 34.093], [-80.991, 34.094], [-80.993, 34.091], [-80.994, 34.092], [-80.994, 34.093], [-80.993, 34.094], [-80.993, 34.095], [-80.992, 34.097], [-80.992, 34.098], [-80.991, 34.099], [-80.987, 34.096], [-80.984, 34.099], [-80.982, 34.101], [-80.983, 34.101], [-80.981, 34.106], [-80.979, 34.107], [-80.98, 34.105], [-80.979, 34.104], [-80.977, 34.105], [-80.976, 34.106], [-80.974, 34.104], [-80.975, 34.1], [-80.973, 34.099], [-80.971, 34.099], [-80.971, 34.098], [-80.972, 34.097], [-80.972, 34.096], [-80.973, 34.097], [-80.973, 34.096], [-80.972, 34.095], [-80.97, 34.094], [-80.97, 34.093], [-80.969, 34.093], [-80.968, 34.094], [-80.967, 34.094], [-80.966, 34.094], [-80.965, 34.094], [-80.964, 34.095], [-80.963, 34.095], [-80.963, 34.094], [-80.963, 34.095], [-80.962, 34.095], [-80.961, 34.095], [-80.961, 34.094], [-80.961, 34.092], [-80.96, 34.091], [-80.96, 34.09], [-80.96, 34.089], [-80.961, 34.088], [-80.961, 34.087], [-80.962, 34.085], [-80.963, 34.084], [-80.964, 34.084], [-80.965, 34.083], [-80.966, 34.084], [-80.966, 34.085], [-80.967, 34.085], [-80.968, 34.085], [-80.97, 34.085], [-80.969, 34.085], [-80.969, 34.084], [-80.97, 34.084], [-80.971, 34.087], [-80.971, 34.088], [-80.972, 34.089], [-80.973, 34.088], [-80.974, 34.088], [-80.975, 34.087], [-80.975, 34.086], [-80.976, 34.084], [-80.976, 34.083], [-80.976, 34.082], [-80.977, 34.081], [-80.977, 34.08], [-80.978, 34.079], [-80.978, 34.078], [-80.978, 34.077], [-80.978, 34.076], [-80.979, 34.076], [-80.979, 34.075], [-80.98, 34.074], [-80.98, 34.073], [-80.984, 34.073], [-80.986, 34.073], [-80.985, 34.071], [-80.983, 34.072], [-80.984, 34.071], [-80.984, 34.07], [-80.983, 34.071], [-80.981, 34.069], [-80.981, 34.068], [-80.982, 34.068], [-80.982, 34.067], [-80.983, 34.067], [-80.984, 34.066], [-80.985, 34.065], [-80.987, 34.064], [-80.991, 34.061], [-80.992, 34.061], [-80.993, 34.06], [-80.995, 34.06], [-80.995, 34.059], [-80.996, 34.058], [-80.997, 34.059], [-80.999, 34.058], [-81, 34.057], [-81.001, 34.057], [-81.002, 34.056], [-81.002, 34.055], [-81.002, 34.056], [-81.001, 34.056], [-81, 34.056], [-80.999, 34.056], [-80.998, 34.056], [-80.997, 34.056], [-80.996, 34.056], [-80.995, 34.056], [-80.994, 34.056], [-80.993, 34.057], [-80.992, 34.057], [-80.992, 34.056], [-80.993, 34.056], [-80.992, 34.056], [-80.992, 34.055], [-80.992, 34.054], [-80.993, 34.054], [-80.992, 34.054], [-80.991, 34.053], [-80.992, 34.053], [-80.993, 34.053], [-80.994, 34.052], [-80.995, 34.052], [-80.995, 34.051], [-80.996, 34.051], [-80.997, 34.051], [-80.998, 34.05], [-81, 34.05], [-81.001, 34.05], [-80.999, 34.048], [-80.998, 34.047], [-80.997, 34.046], [-80.997, 34.047], [-80.997, 34.046], [-80.998, 34.046], [-80.996, 34.044], [-80.995, 34.044], [-80.995, 34.043], [-80.994, 34.043], [-80.992, 34.043], [-80.993, 34.043], [-80.994, 34.042], [-80.995, 34.042], [-80.996, 34.042], [-80.996, 34.041], [-80.997, 34.041], [-80.998, 34.041], [-80.998, 34.04], [-80.999, 34.04], [-81, 34.04], [-80.999, 34.039], [-80.999, 34.04], [-80.998, 34.04], [-80.998, 34.039], [-80.997, 34.039], [-80.996, 34.039], [-80.995, 34.039], [-80.995, 34.038], [-80.994, 34.038], [-80.993, 34.038], [-80.994, 34.039], [-80.993, 34.039], [-80.994, 34.039], [-80.993, 34.039], [-80.993, 34.04], [-80.993, 34.041], [-80.992, 34.041], [-80.991, 34.04], [-80.99, 34.04], [-80.99, 34.039], [-80.989, 34.039], [-80.988, 34.039], [-80.989, 34.039], [-80.989, 34.038], [-80.99, 34.038], [-80.991, 34.039], [-80.992, 34.037], [-80.991, 34.037], [-80.99, 34.037], [-80.989, 34.037], [-80.989, 34.036], [-80.988, 34.036], [-80.988, 34.037], [-80.987, 34.037], [-80.986, 34.037], [-80.986, 34.036], [-80.985, 34.036], [-80.984, 34.037], [-80.983, 34.037], [-80.983, 34.038], [-80.983, 34.037], [-80.982, 34.038], [-80.981, 34.038], [-80.981, 34.037], [-80.98, 34.037], [-80.98, 34.036], [-80.981, 34.036], [-80.982, 34.036], [-80.983, 34.036], [-80.982, 34.035], [-80.983, 34.033], [-80.982, 34.033], [-80.983, 34.032], [-80.982, 34.032], [-80.982, 34.031], [-80.982, 34.032], [-80.983, 34.032], [-80.984, 34.032], [-80.984, 34.033], [-80.985, 34.033], [-80.986, 34.033], [-80.986, 34.03], [-80.985, 34.03], [-80.985, 34.029], [-80.985, 34.028], [-80.986, 34.028], [-80.987, 34.028], [-80.987, 34.027], [-80.988, 34.027], [-80.989, 34.026], [-80.99, 34.026], [-80.991, 34.026], [-80.992, 34.026], [-80.993, 34.025], [-80.994, 34.025], [-80.995, 34.025], [-80.995, 34.024], [-80.995, 34.023], [-80.995, 34.021], [-80.995, 34.02], [-80.995, 34.019], [-80.995, 34.018], [-80.995, 34.017], [-80.995, 34.016], [-80.995, 34.015], [-80.995, 34.014], [-80.995, 34.013], [-80.995, 34.012], [-80.995, 34.01], [-80.994, 34.01], [-80.993, 34.011], [-80.992, 34.011], [-80.991, 34.012], [-80.99, 34.012], [-80.989, 34.012], [-80.988, 34.012], [-80.988, 34.013], [-80.987, 34.013], [-80.986, 34.013], [-80.985, 34.013], [-80.984, 34.013], [-80.983, 34.013], [-80.982, 34.013], [-80.981, 34.014], [-80.98, 34.014], [-80.979, 34.014], [-80.978, 34.014], [-80.977, 34.014], [-80.976, 34.014], [-80.975, 34.014], [-80.975, 34.013], [-80.974, 34.013], [-80.973, 34.013], [-80.972, 34.012], [-80.971, 34.012], [-80.97, 34.012], [-80.969, 34.011], [-80.968, 34.011], [-80.967, 34.011], [-80.966, 34.011], [-80.965, 34.011], [-80.964, 34.011], [-80.963, 34.011], [-80.963, 34.012], [-80.962, 34.012], [-80.961, 34.012], [-80.961, 34.013], [-80.96, 34.013], [-80.96, 34.015], [-80.96, 34.016], [-80.96, 34.018], [-80.96, 34.019], [-80.96, 34.02], [-80.96, 34.021], [-80.961, 34.021], [-80.96, 34.021], [-80.959, 34.022], [-80.958, 34.022], [-80.958, 34.023], [-80.957, 34.023], [-80.956, 34.023], [-80.955, 34.023], [-80.954, 34.023], [-80.953, 34.024], [-80.952, 34.024], [-80.952, 34.025], [-80.951, 34.025], [-80.951, 34.024], [-80.95, 34.024], [-80.949, 34.024], [-80.948, 34.024], [-80.946, 34.024], [-80.945, 34.025], [-80.944, 34.026], [-80.945, 34.026], [-80.946, 34.027], [-80.947, 34.027], [-80.947, 34.028], [-80.946, 34.029], [-80.945, 34.029], [-80.944, 34.03], [-80.943, 34.03], [-80.944, 34.03], [-80.944, 34.031], [-80.944, 34.03], [-80.943, 34.03], [-80.943, 34.031], [-80.943, 34.032], [-80.942, 34.032], [-80.941, 34.031], [-80.941, 34.032], [-80.94, 34.032], [-80.94, 34.033], [-80.939, 34.034], [-80.939, 34.035], [-80.939, 34.036], [-80.939, 34.037], [-80.94, 34.037], [-80.94, 34.036], [-80.94, 34.037], [-80.941, 34.037], [-80.941, 34.038], [-80.94, 34.038], [-80.939, 34.038], [-80.939, 34.039], [-80.939, 34.04], [-80.938, 34.041], [-80.938, 34.042], [-80.937, 34.042], [-80.937, 34.043], [-80.936, 34.043], [-80.936, 34.044], [-80.935, 34.044], [-80.934, 34.044], [-80.934, 34.045], [-80.933, 34.045], [-80.931, 34.047], [-80.93, 34.047], [-80.93, 34.048], [-80.929, 34.048], [-80.928, 34.049], [-80.926, 34.05], [-80.925, 34.051], [-80.924, 34.051], [-80.924, 34.052], [-80.923, 34.052], [-80.922, 34.053], [-80.921, 34.053], [-80.921, 34.054], [-80.92, 34.054], [-80.919, 34.054], [-80.919, 34.055], [-80.92, 34.055], [-80.92, 34.056], [-80.919, 34.057], [-80.919, 34.056], [-80.918, 34.057], [-80.918, 34.056], [-80.917, 34.056], [-80.915, 34.058], [-80.914, 34.058], [-80.913, 34.059], [-80.912, 34.059], [-80.911, 34.06], [-80.91, 34.061], [-80.909, 34.062], [-80.908, 34.062], [-80.909, 34.062], [-80.908, 34.062], [-80.907, 34.063], [-80.906, 34.063], [-80.906, 34.064], [-80.907, 34.064], [-80.906, 34.064], [-80.905, 34.064], [-80.904, 34.065], [-80.903, 34.065], [-80.903, 34.066], [-80.904, 34.066], [-80.904, 34.067], [-80.905, 34.067], [-80.907, 34.067], [-80.906, 34.068], [-80.903, 34.07], [-80.902, 34.07], [-80.901, 34.07], [-80.902, 34.07], [-80.902, 34.069], [-80.903, 34.068], [-80.902, 34.068], [-80.901, 34.067], [-80.9, 34.067], [-80.9, 34.068], [-80.899, 34.068], [-80.898, 34.069], [-80.897, 34.07], [-80.896, 34.07], [-80.895, 34.071], [-80.894, 34.072], [-80.893, 34.072], [-80.892, 34.073], [-80.891, 34.074], [-80.89, 34.074], [-80.889, 34.075], [-80.888, 34.076], [-80.887, 34.076], [-80.886, 34.077], [-80.885, 34.077], [-80.885, 34.078], [-80.884, 34.078], [-80.883, 34.079], [-80.882, 34.079], [-80.881, 34.08], [-80.882, 34.08], [-80.882, 34.081], [-80.883, 34.081], [-80.883, 34.08], [-80.882, 34.08], [-80.883, 34.08], [-80.884, 34.079], [-80.885, 34.079], [-80.886, 34.079], [-80.886, 34.078], [-80.887, 34.078], [-80.887, 34.077], [-80.888, 34.077], [-80.888, 34.076], [-80.889, 34.076], [-80.89, 34.076], [-80.891, 34.075], [-80.891, 34.076], [-80.891, 34.077], [-80.891, 34.078], [-80.892, 34.078], [-80.892, 34.079], [-80.892, 34.08], [-80.885, 34.083], [-80.882, 34.084], [-80.879, 34.085], [-80.876, 34.082], [-80.875, 34.082], [-80.874, 34.082], [-80.874, 34.083], [-80.875, 34.083], [-80.876, 34.084], [-80.877, 34.086], [-80.876, 34.087], [-80.876, 34.088], [-80.877, 34.088], [-80.876, 34.088], [-80.876, 34.089], [-80.879, 34.089], [-80.88, 34.091], [-80.881, 34.092], [-80.881, 34.093], [-80.88, 34.093], [-80.879, 34.094], [-80.878, 34.094], [-80.878, 34.095], [-80.877, 34.095], [-80.876, 34.096], [-80.875, 34.096], [-80.875, 34.097], [-80.874, 34.097], [-80.873, 34.098], [-80.872, 34.098], [-80.872, 34.099], [-80.871, 34.099], [-80.87, 34.099], [-80.869, 34.1], [-80.868, 34.099], [-80.868, 34.098], [-80.868, 34.097], [-80.869, 34.097], [-80.871, 34.096], [-80.872, 34.096], [-80.869, 34.094], [-80.868, 34.094], [-80.868, 34.093], [-80.867, 34.093], [-80.867, 34.092], [-80.866, 34.092], [-80.864, 34.094], [-80.863, 34.094], [-80.861, 34.094], [-80.86, 34.095], [-80.859, 34.094], [-80.859, 34.095], [-80.858, 34.094], [-80.858, 34.093], [-80.857, 34.092], [-80.856, 34.092], [-80.855, 34.092], [-80.856, 34.092], [-80.856, 34.093], [-80.856, 34.094], [-80.857, 34.095], [-80.857, 34.096], [-80.857, 34.097], [-80.856, 34.097], [-80.856, 34.096], [-80.856, 34.095], [-80.854, 34.096], [-80.855, 34.096], [-80.855, 34.097], [-80.854, 34.097], [-80.853, 34.097], [-80.852, 34.097], [-80.852, 34.098], [-80.851, 34.098], [-80.85, 34.099], [-80.85, 34.1], [-80.849, 34.1], [-80.848, 34.1], [-80.847, 34.1], [-80.847, 34.101], [-80.847, 34.102], [-80.844, 34.101], [-80.841, 34.101], [-80.846, 34.099], [-80.849, 34.098], [-80.85, 34.097], [-80.851, 34.097], [-80.851, 34.096], [-80.85, 34.094], [-80.851, 34.095], [-80.852, 34.094], [-80.851, 34.094], [-80.85, 34.094], [-80.849, 34.093], [-80.849, 34.092], [-80.846, 34.094], [-80.844, 34.094], [-80.833, 34.099], [-80.826, 34.102], [-80.823, 34.103], [-80.822, 34.103], [-80.821, 34.104], [-80.821, 34.105], [-80.82, 34.106], [-80.82, 34.107], [-80.821, 34.107], [-80.824, 34.106], [-80.824, 34.107], [-80.826, 34.106], [-80.827, 34.106], [-80.828, 34.106], [-80.829, 34.106], [-80.83, 34.106], [-80.831, 34.106], [-80.831, 34.107], [-80.832, 34.107], [-80.832, 34.106], [-80.833, 34.107], [-80.834, 34.107], [-80.835, 34.107], [-80.835, 34.106], [-80.836, 34.106], [-80.837, 34.105], [-80.837, 34.106], [-80.837, 34.107], [-80.837, 34.108], [-80.836, 34.108], [-80.835, 34.108], [-80.836, 34.109], [-80.837, 34.109], [-80.836, 34.11], [-80.837, 34.11], [-80.837, 34.111], [-80.838, 34.111], [-80.839, 34.112], [-80.841, 34.11], [-80.843, 34.109], [-80.844, 34.108], [-80.845, 34.108], [-80.846, 34.108], [-80.847, 34.108], [-80.847, 34.109], [-80.847, 34.11], [-80.846, 34.111], [-80.847, 34.112], [-80.847, 34.113], [-80.847, 34.114], [-80.847, 34.115], [-80.846, 34.115], [-80.845, 34.115], [-80.845, 34.116], [-80.846, 34.116], [-80.846, 34.117], [-80.846, 34.118], [-80.847, 34.118], [-80.847, 34.119], [-80.847, 34.12], [-80.848, 34.12], [-80.847, 34.12], [-80.846, 34.12], [-80.844, 34.12], [-80.844, 34.119], [-80.843, 34.119], [-80.842, 34.119], [-80.841, 34.119], [-80.84, 34.119], [-80.839, 34.119], [-80.838, 34.119], [-80.838, 34.12], [-80.837, 34.12], [-80.837, 34.121], [-80.836, 34.121], [-80.837, 34.121], [-80.836, 34.121], [-80.835, 34.121], [-80.835, 34.122], [-80.834, 34.122], [-80.835, 34.122], [-80.835, 34.123], [-80.836, 34.123], [-80.835, 34.123], [-80.836, 34.123], [-80.835, 34.123], [-80.834, 34.123], [-80.833, 34.123], [-80.833, 34.122], [-80.834, 34.122], [-80.834, 34.121], [-80.833, 34.121], [-80.832, 34.122], [-80.832, 34.123], [-80.832, 34.124], [-80.833, 34.124], [-80.833, 34.125], [-80.834, 34.125], [-80.834, 34.124], [-80.835, 34.124], [-80.836, 34.124], [-80.836, 34.125], [-80.835, 34.125], [-80.835, 34.126], [-80.834, 34.127], [-80.833, 34.128], [-80.832, 34.129], [-80.831, 34.129], [-80.83, 34.129], [-80.829, 34.13], [-80.829, 34.129], [-80.828, 34.129], [-80.829, 34.128], [-80.828, 34.128], [-80.828, 34.127], [-80.827, 34.126], [-80.827, 34.125], [-80.826, 34.125], [-80.825, 34.125], [-80.826, 34.125], [-80.826, 34.124], [-80.827, 34.124], [-80.828, 34.125], [-80.829, 34.125], [-80.829, 34.126], [-80.83, 34.126], [-80.831, 34.125], [-80.832, 34.124], [-80.832, 34.123], [-80.832, 34.122], [-80.831, 34.122], [-80.831, 34.121], [-80.83, 34.121], [-80.831, 34.121], [-80.831, 34.12], [-80.83, 34.121], [-80.829, 34.12], [-80.829, 34.119], [-80.828, 34.119], [-80.828, 34.118], [-80.828, 34.117], [-80.829, 34.117], [-80.828, 34.117], [-80.828, 34.116], [-80.827, 34.116], [-80.827, 34.115], [-80.826, 34.115], [-80.825, 34.115], [-80.824, 34.115], [-80.823, 34.114], [-80.822, 34.114], [-80.822, 34.113], [-80.821, 34.113], [-80.82, 34.113], [-80.819, 34.112], [-80.818, 34.112], [-80.817, 34.112], [-80.818, 34.112], [-80.818, 34.113], [-80.819, 34.113], [-80.819, 34.112], [-80.819, 34.113], [-80.82, 34.113], [-80.821, 34.114], [-80.82, 34.114], [-80.82, 34.115], [-80.821, 34.116], [-80.822, 34.116], [-80.823, 34.117], [-80.824, 34.117], [-80.825, 34.117], [-80.825, 34.118], [-80.826, 34.118], [-80.827, 34.118], [-80.827, 34.119], [-80.828, 34.119], [-80.828, 34.12], [-80.83, 34.122], [-80.829, 34.122], [-80.828, 34.122], [-80.827, 34.122], [-80.826, 34.122], [-80.825, 34.121], [-80.824, 34.121], [-80.823, 34.121], [-80.821, 34.121], [-80.819, 34.121], [-80.818, 34.121], [-80.817, 34.121], [-80.817, 34.122], [-80.817, 34.123], [-80.818, 34.123], [-80.819, 34.123], [-80.82, 34.123], [-80.821, 34.123], [-80.821, 34.124], [-80.821, 34.125], [-80.822, 34.125], [-80.822, 34.124], [-80.822, 34.125], [-80.823, 34.125], [-80.823, 34.126], [-80.822, 34.127], [-80.822, 34.128], [-80.822, 34.129], [-80.821, 34.129], [-80.82, 34.13], [-80.819, 34.131], [-80.82, 34.131], [-80.819, 34.131], [-80.818, 34.131], [-80.818, 34.13], [-80.817, 34.13], [-80.817, 34.129], [-80.816, 34.129], [-80.816, 34.128], [-80.817, 34.128], [-80.817, 34.129], [-80.817, 34.128], [-80.818, 34.129], [-80.818, 34.128], [-80.819, 34.128], [-80.82, 34.128], [-80.819, 34.128], [-80.819, 34.127], [-80.82, 34.127], [-80.82, 34.128], [-80.82, 34.127], [-80.821, 34.127], [-80.82, 34.127], [-80.821, 34.127], [-80.821, 34.126], [-80.82, 34.126], [-80.82, 34.125], [-80.819, 34.125], [-80.819, 34.124], [-80.818, 34.124], [-80.818, 34.123], [-80.817, 34.123], [-80.817, 34.122], [-80.817, 34.121], [-80.816, 34.121], [-80.816, 34.122], [-80.815, 34.122], [-80.814, 34.122], [-80.814, 34.121], [-80.815, 34.121], [-80.815, 34.12], [-80.816, 34.12], [-80.816, 34.119], [-80.815, 34.119], [-80.816, 34.119], [-80.815, 34.118], [-80.815, 34.117], [-80.815, 34.116], [-80.815, 34.115], [-80.815, 34.114], [-80.816, 34.114], [-80.817, 34.114], [-80.817, 34.113], [-80.817, 34.112], [-80.817, 34.111], [-80.816, 34.11], [-80.816, 34.109], [-80.815, 34.106], [-80.82, 34.104], [-80.819, 34.104], [-80.818, 34.104], [-80.818, 34.105], [-80.817, 34.105], [-80.816, 34.105], [-80.81, 34.104], [-80.809, 34.104], [-80.808, 34.104], [-80.807, 34.104], [-80.806, 34.104], [-80.805, 34.104], [-80.804, 34.103], [-80.803, 34.103], [-80.802, 34.103], [-80.801, 34.103], [-80.797, 34.103], [-80.795, 34.102], [-80.787, 34.102], [-80.785, 34.102], [-80.784, 34.102], [-80.783, 34.102], [-80.78, 34.102], [-80.778, 34.102], [-80.777, 34.102], [-80.776, 34.102], [-80.775, 34.102], [-80.774, 34.102], [-80.772, 34.101], [-80.771, 34.101], [-80.771, 34.1], [-80.77, 34.1], [-80.769, 34.1], [-80.768, 34.099], [-80.767, 34.098], [-80.767, 34.097], [-80.766, 34.097], [-80.766, 34.096], [-80.766, 34.095], [-80.765, 34.095], [-80.765, 34.094], [-80.763, 34.092], [-80.763, 34.091], [-80.762, 34.09], [-80.762, 34.089], [-80.761, 34.089], [-80.759, 34.087], [-80.758, 34.086], [-80.757, 34.085], [-80.756, 34.085], [-80.756, 34.084], [-80.755, 34.084], [-80.754, 34.084], [-80.753, 34.083], [-80.752, 34.083], [-80.752, 34.084], [-80.751, 34.084], [-80.75, 34.084], [-80.749, 34.084], [-80.748, 34.084], [-80.748, 34.083], [-80.747, 34.083], [-80.746, 34.083], [-80.744, 34.082], [-80.743, 34.082], [-80.742, 34.082], [-80.742, 34.081], [-80.741, 34.081], [-80.74, 34.081], [-80.74, 34.08], [-80.737, 34.079], [-80.736, 34.078], [-80.735, 34.077], [-80.734, 34.076], [-80.733, 34.075], [-80.732, 34.074], [-80.732, 34.073], [-80.731, 34.073], [-80.73, 34.072], [-80.728, 34.07], [-80.727, 34.07], [-80.727, 34.069], [-80.726, 34.069], [-80.725, 34.068], [-80.724, 34.068], [-80.724, 34.067], [-80.723, 34.067], [-80.722, 34.067], [-80.721, 34.067], [-80.721, 34.066], [-80.72, 34.066], [-80.719, 34.066], [-80.719, 34.065], [-80.718, 34.065], [-80.717, 34.065], [-80.714, 34.064], [-80.713, 34.064], [-80.713, 34.063], [-80.712, 34.063], [-80.711, 34.063], [-80.71, 34.063], [-80.709, 34.063], [-80.708, 34.063], [-80.707, 34.063], [-80.707, 34.062], [-80.706, 34.062], [-80.705, 34.062], [-80.704, 34.061], [-80.704, 34.06], [-80.704, 34.057], [-80.704, 34.05], [-80.704, 34.049], [-80.704, 34.047], [-80.704, 34.045], [-80.704, 34.043], [-80.704, 34.042], [-80.704, 34.04], [-80.704, 34.039], [-80.704, 34.038], [-80.704, 34.037], [-80.704, 34.036], [-80.704, 34.034], [-80.704, 34.033], [-80.704, 34.032], [-80.704, 34.031], [-80.704, 34.03], [-80.704, 34.029], [-80.704, 34.027], [-80.704, 34.026], [-80.704, 34.025], [-80.704, 34.024], [-80.704, 34.023], [-80.704, 34.022], [-80.704, 34.021], [-80.704, 34.02], [-80.704, 34.019], [-80.704, 34.016], [-80.704, 34.014], [-80.704, 34.01], [-80.705, 34.01], [-80.705, 34.009], [-80.706, 34.009], [-80.706, 34.008], [-80.707, 34.008], [-80.709, 34.008], [-80.714, 34.008], [-80.715, 34.008], [-80.717, 34.008], [-80.718, 34.008], [-80.719, 34.008], [-80.72, 34.008], [-80.724, 34.007], [-80.726, 34.007], [-80.727, 34.007], [-80.731, 34.007], [-80.733, 34.007], [-80.734, 34.007], [-80.736, 34.007], [-80.742, 34.007], [-80.744, 34.007], [-80.748, 34.006], [-80.751, 34.006], [-80.759, 34.006], [-80.764, 34.006], [-80.766, 34.006], [-80.767, 34.005], [-80.768, 34.005], [-80.769, 34.005], [-80.771, 34.005], [-80.773, 34.005], [-80.776, 34.004], [-80.78, 34.004], [-80.782, 34.004], [-80.783, 34.003], [-80.789, 34.003], [-80.791, 34.002], [-80.792, 34.002], [-80.795, 34.002], [-80.803, 34.001], [-80.805, 34], [-80.806, 34], [-80.807, 34], [-80.811, 34], [-80.813, 33.999], [-80.814, 33.999], [-80.815, 33.999], [-80.816, 33.999], [-80.817, 33.999], [-80.818, 33.999], [-80.819, 33.999], [-80.819, 33.998], [-80.822, 33.998], [-80.823, 33.998], [-80.825, 33.998], [-80.826, 33.998], [-80.826, 33.997], [-80.828, 33.997], [-80.831, 33.997], [-80.833, 33.997], [-80.833, 33.996], [-80.834, 33.996], [-80.835, 33.996], [-80.836, 33.996], [-80.837, 33.996], [-80.84, 33.996], [-80.841, 33.995], [-80.842, 33.995], [-80.843, 33.995], [-80.847, 33.995], [-80.848, 33.994], [-80.85, 33.994], [-80.852, 33.994], [-80.853, 33.994], [-80.855, 33.994], [-80.855, 33.993], [-80.856, 33.993], [-80.857, 33.993], [-80.861, 33.993], [-80.863, 33.992], [-80.864, 33.992], [-80.866, 33.992], [-80.867, 33.992], [-80.868, 33.992], [-80.869, 33.991], [-80.87, 33.991], [-80.871, 33.991], [-80.872, 33.991], [-80.873, 33.99], [-80.874, 33.99], [-80.878, 33.989], [-80.88, 33.989], [-80.88, 33.988], [-80.881, 33.988], [-80.881, 33.987], [-80.882, 33.986], [-80.881, 33.985], [-80.88, 33.985], [-80.88, 33.984], [-80.88, 33.983], [-80.877, 33.983], [-80.877, 33.982], [-80.876, 33.982], [-80.875, 33.982], [-80.873, 33.98], [-80.872, 33.982], [-80.871, 33.982], [-80.871, 33.979], [-80.872, 33.979], [-80.873, 33.979], [-80.874, 33.979], [-80.874, 33.98], [-80.875, 33.98], [-80.876, 33.98], [-80.876, 33.979], [-80.877, 33.979], [-80.877, 33.98], [-80.877, 33.981], [-80.878, 33.981], [-80.879, 33.981], [-80.879, 33.98], [-80.879, 33.979], [-80.879, 33.978], [-80.88, 33.978], [-80.881, 33.978], [-80.882, 33.979], [-80.883, 33.979], [-80.884, 33.979], [-80.885, 33.98], [-80.884, 33.982], [-80.884, 33.983], [-80.884, 33.985], [-80.885, 33.985], [-80.886, 33.985], [-80.886, 33.986], [-80.884, 33.986], [-80.883, 33.987], [-80.882, 33.987], [-80.883, 33.988], [-80.884, 33.987], [-80.888, 33.986], [-80.889, 33.986], [-80.89, 33.986], [-80.89, 33.985], [-80.89, 33.984], [-80.891, 33.984], [-80.892, 33.984], [-80.893, 33.984], [-80.893, 33.985], [-80.894, 33.985], [-80.895, 33.985], [-80.895, 33.984], [-80.894, 33.984], [-80.895, 33.983], [-80.896, 33.983], [-80.897, 33.983], [-80.896, 33.983], [-80.896, 33.984], [-80.897, 33.984], [-80.898, 33.984], [-80.899, 33.984], [-80.899, 33.983], [-80.899, 33.982], [-80.897, 33.982], [-80.898, 33.981], [-80.898, 33.979], [-80.896, 33.979], [-80.896, 33.978], [-80.897, 33.979], [-80.9, 33.979], [-80.901, 33.978], [-80.902, 33.977], [-80.9, 33.975], [-80.9, 33.974], [-80.9, 33.973], [-80.9, 33.972], [-80.9, 33.973], [-80.901, 33.973], [-80.902, 33.973], [-80.903, 33.973], [-80.904, 33.973], [-80.904, 33.974], [-80.904, 33.975], [-80.904, 33.976], [-80.904, 33.977], [-80.904, 33.978], [-80.904, 33.979], [-80.905, 33.979], [-80.904, 33.979], [-80.904, 33.98], [-80.905, 33.98], [-80.904, 33.981], [-80.903, 33.981], [-80.903, 33.982], [-80.904, 33.982], [-80.905, 33.982], [-80.906, 33.982], [-80.905, 33.982], [-80.906, 33.982], [-80.907, 33.982], [-80.908, 33.982], [-80.909, 33.982], [-80.91, 33.981], [-80.911, 33.981], [-80.912, 33.981], [-80.913, 33.981], [-80.913, 33.98], [-80.914, 33.98], [-80.915, 33.98], [-80.914, 33.98], [-80.915, 33.98], [-80.914, 33.98], [-80.915, 33.98], [-80.915, 33.979], [-80.915, 33.978], [-80.915, 33.977], [-80.916, 33.976], [-80.915, 33.977], [-80.915, 33.976], [-80.916, 33.976], [-80.916, 33.975], [-80.917, 33.975], [-80.917, 33.974], [-80.917, 33.973], [-80.918, 33.974], [-80.92, 33.974], [-80.92, 33.973], [-80.92, 33.972], [-80.921, 33.972], [-80.92, 33.972], [-80.919, 33.972], [-80.919, 33.971], [-80.918, 33.971], [-80.919, 33.97], [-80.919, 33.969], [-80.919, 33.968], [-80.918, 33.968], [-80.918, 33.967], [-80.917, 33.967], [-80.918, 33.965], [-80.92, 33.965], [-80.922, 33.966], [-80.922, 33.965], [-80.922, 33.964], [-80.923, 33.964], [-80.923, 33.965], [-80.923, 33.964], [-80.924, 33.964], [-80.924, 33.965], [-80.924, 33.966], [-80.923, 33.966], [-80.922, 33.968], [-80.922, 33.969], [-80.922, 33.968], [-80.921, 33.969], [-80.921, 33.968], [-80.922, 33.972], [-80.923, 33.971], [-80.923, 33.97], [-80.923, 33.971], [-80.924, 33.97], [-80.924, 33.971], [-80.925, 33.971], [-80.925, 33.972], [-80.926, 33.973], [-80.926, 33.972], [-80.926, 33.973], [-80.926, 33.974], [-80.926, 33.973], [-80.927, 33.973], [-80.926, 33.974], [-80.927, 33.974], [-80.927, 33.975], [-80.928, 33.975], [-80.927, 33.975], [-80.928, 33.976], [-80.927, 33.976], [-80.925, 33.977], [-80.925, 33.978], [-80.925, 33.979], [-80.926, 33.979], [-80.927, 33.979], [-80.928, 33.978], [-80.929, 33.979], [-80.929, 33.978], [-80.93, 33.978], [-80.93, 33.979], [-80.931, 33.979], [-80.93, 33.979], [-80.93, 33.98], [-80.931, 33.98], [-80.931, 33.981], [-80.934, 33.98], [-80.935, 33.979], [-80.938, 33.978], [-80.937, 33.977], [-80.936, 33.977], [-80.937, 33.977], [-80.936, 33.978], [-80.936, 33.977], [-80.937, 33.976], [-80.936, 33.976], [-80.937, 33.975], [-80.938, 33.975], [-80.939, 33.975], [-80.939, 33.974], [-80.939, 33.973], [-80.938, 33.973], [-80.94, 33.971], [-80.941, 33.971], [-80.942, 33.97], [-80.942, 33.969], [-80.941, 33.969], [-80.94, 33.968], [-80.939, 33.97], [-80.938, 33.971], [-80.937, 33.97], [-80.937, 33.971], [-80.937, 33.972], [-80.936, 33.972], [-80.936, 33.973], [-80.935, 33.973], [-80.935, 33.972], [-80.934, 33.971], [-80.934, 33.97], [-80.933, 33.97], [-80.933, 33.969], [-80.932, 33.97], [-80.932, 33.969], [-80.931, 33.969], [-80.931, 33.968], [-80.93, 33.968], [-80.929, 33.968], [-80.928, 33.967], [-80.926, 33.967], [-80.926, 33.966], [-80.927, 33.965], [-80.927, 33.964], [-80.927, 33.963], [-80.928, 33.963], [-80.928, 33.964], [-80.929, 33.964], [-80.929, 33.963], [-80.93, 33.962], [-80.928, 33.962], [-80.93, 33.962], [-80.93, 33.961], [-80.929, 33.96], [-80.929, 33.959], [-80.93, 33.958], [-80.931, 33.958], [-80.932, 33.958], [-80.932, 33.959], [-80.933, 33.959], [-80.934, 33.959], [-80.934, 33.96], [-80.934, 33.961], [-80.935, 33.962], [-80.937, 33.961], [-80.938, 33.959], [-80.939, 33.959], [-80.94, 33.96], [-80.941, 33.959], [-80.941, 33.96], [-80.942, 33.959], [-80.943, 33.959], [-80.943, 33.958], [-80.944, 33.958], [-80.945, 33.958], [-80.945, 33.957], [-80.944, 33.957], [-80.944, 33.956], [-80.943, 33.956], [-80.943, 33.955], [-80.943, 33.954], [-80.942, 33.954], [-80.941, 33.954], [-80.94, 33.954], [-80.939, 33.954], [-80.939, 33.955], [-80.939, 33.956], [-80.938, 33.956], [-80.937, 33.958], [-80.936, 33.958], [-80.937, 33.957], [-80.936, 33.958], [-80.935, 33.958], [-80.935, 33.957], [-80.936, 33.957], [-80.936, 33.956], [-80.937, 33.957], [-80.938, 33.956], [-80.936, 33.956], [-80.936, 33.955], [-80.935, 33.956], [-80.934, 33.957], [-80.933, 33.957], [-80.933, 33.958], [-80.932, 33.958], [-80.932, 33.957], [-80.931, 33.957], [-80.931, 33.958], [-80.93, 33.957], [-80.93, 33.956], [-80.929, 33.956], [-80.928, 33.956], [-80.927, 33.956], [-80.927, 33.955], [-80.928, 33.954], [-80.928, 33.953], [-80.928, 33.952], [-80.931, 33.95], [-80.93, 33.949], [-80.931, 33.95], [-80.932, 33.949], [-80.933, 33.948], [-80.934, 33.948], [-80.935, 33.948], [-80.938, 33.949], [-80.939, 33.949], [-80.941, 33.95], [-80.944, 33.951], [-80.945, 33.951], [-80.947, 33.952], [-80.948, 33.952], [-80.948, 33.951], [-80.948, 33.95], [-80.949, 33.95], [-80.95, 33.95], [-80.95, 33.951], [-80.951, 33.951], [-80.951, 33.95], [-80.951, 33.951], [-80.951, 33.95], [-80.952, 33.95], [-80.952, 33.949], [-80.953, 33.948], [-80.953, 33.947], [-80.954, 33.948], [-80.957, 33.949], [-80.958, 33.949], [-80.958, 33.95], [-80.959, 33.95], [-80.96, 33.951], [-80.961, 33.951], [-80.962, 33.952], [-80.963, 33.952], [-80.964, 33.952], [-80.966, 33.951], [-80.967, 33.95], [-80.967, 33.951], [-80.967, 33.95], [-80.968, 33.95], [-80.968, 33.949], [-80.969, 33.949], [-80.97, 33.948], [-80.971, 33.949], [-80.972, 33.948], [-80.973, 33.948], [-80.975, 33.947], [-80.974, 33.946], [-80.975, 33.946], [-80.974, 33.945], [-80.974, 33.944], [-80.973, 33.944], [-80.972, 33.943], [-80.973, 33.943], [-80.972, 33.943], [-80.972, 33.942], [-80.97, 33.943], [-80.969, 33.944], [-80.968, 33.944], [-80.966, 33.942], [-80.965, 33.941], [-80.964, 33.942], [-80.963, 33.942], [-80.963, 33.943], [-80.962, 33.943], [-80.962, 33.942], [-80.96, 33.943], [-80.959, 33.942], [-80.96, 33.939], [-80.96, 33.938], [-80.96, 33.937], [-80.961, 33.936], [-80.961, 33.935], [-80.961, 33.936], [-80.962, 33.936], [-80.963, 33.937], [-80.963, 33.938], [-80.964, 33.938], [-80.964, 33.939], [-80.965, 33.939], [-80.966, 33.939], [-80.966, 33.94], [-80.966, 33.939], [-80.967, 33.939], [-80.968, 33.939], [-80.969, 33.939], [-80.97, 33.939], [-80.97, 33.94], [-80.971, 33.94], [-80.972, 33.94], [-80.974, 33.941], [-80.974, 33.94], [-80.975, 33.94], [-80.976, 33.939], [-80.977, 33.939], [-80.978, 33.939], [-80.978, 33.938], [-80.979, 33.938], [-80.979, 33.939], [-80.981, 33.94], [-80.981, 33.939], [-80.982, 33.939], [-80.983, 33.94], [-80.984, 33.941], [-80.985, 33.942], [-80.983, 33.943], [-80.983, 33.944], [-80.981, 33.945], [-80.982, 33.946], [-80.983, 33.948], [-80.983, 33.947], [-80.985, 33.947], [-80.985, 33.946], [-80.986, 33.946], [-80.987, 33.947], [-80.988, 33.947], [-80.989, 33.947], [-80.989, 33.948], [-80.989, 33.949], [-80.99, 33.949], [-80.99, 33.95], [-80.991, 33.951], [-80.991, 33.952], [-80.992, 33.953], [-80.991, 33.953], [-80.991, 33.954], [-80.99, 33.954], [-80.989, 33.955], [-80.988, 33.955], [-80.988, 33.956], [-80.986, 33.955], [-80.986, 33.956], [-80.985, 33.956], [-80.984, 33.957], [-80.983, 33.957], [-80.982, 33.957], [-80.981, 33.956], [-80.98, 33.956], [-80.978, 33.956], [-80.978, 33.957], [-80.979, 33.957], [-80.978, 33.957], [-80.978, 33.958], [-80.98, 33.959], [-80.979, 33.96], [-80.98, 33.961], [-80.979, 33.961], [-80.98, 33.961], [-80.98, 33.962], [-80.98, 33.963], [-80.981, 33.963], [-80.982, 33.963], [-80.981, 33.964], [-80.981, 33.965], [-80.982, 33.965], [-80.982, 33.966], [-80.983, 33.966], [-80.983, 33.967], [-80.984, 33.968], [-80.983, 33.969], [-80.983, 33.97], [-80.982, 33.97], [-80.981, 33.97], [-80.981, 33.969], [-80.981, 33.97], [-80.981, 33.971], [-80.981, 33.972], [-80.98, 33.973], [-80.98, 33.974], [-80.981, 33.974], [-80.981, 33.973], [-80.982, 33.973], [-80.982, 33.972], [-80.982, 33.971], [-80.983, 33.971], [-80.984, 33.971], [-80.985, 33.971], [-80.986, 33.971], [-80.986, 33.972], [-80.987, 33.972], [-80.988, 33.97], [-80.987, 33.97], [-80.986, 33.97], [-80.986, 33.969], [-80.987, 33.97], [-80.988, 33.97], [-80.988, 33.969], [-80.988, 33.968], [-80.987, 33.968], [-80.986, 33.968], [-80.986, 33.967], [-80.985, 33.967], [-80.986, 33.966], [-80.988, 33.967], [-80.989, 33.968], [-80.99, 33.968], [-80.991, 33.968], [-80.992, 33.968], [-80.993, 33.968], [-80.994, 33.969], [-80.997, 33.97], [-81, 33.972], [-81.003, 33.973], [-81.008, 33.976], [-81.009, 33.976], [-81.012, 33.978], [-81.013, 33.978], [-81.013, 33.979], [-81.014, 33.979], [-81.015, 33.979], [-81.015, 33.98], [-81.016, 33.98], [-81.017, 33.98], [-81.02, 33.982], [-81.021, 33.982], [-81.022, 33.983], [-81.023, 33.983], [-81.023, 33.982], [-81.022, 33.982], [-81.022, 33.981], [-81.021, 33.98], [-81.02, 33.98], [-81.019, 33.979], [-81.017, 33.979], [-81.017, 33.978], [-81.017, 33.979], [-81.016, 33.979], [-81.015, 33.979], [-81.016, 33.979], [-81.016, 33.978], [-81.017, 33.979], [-81.017, 33.978], [-81.016, 33.978], [-81.017, 33.977], [-81.016, 33.976], [-81.015, 33.976], [-81.014, 33.975], [-81.013, 33.975], [-81.014, 33.975], [-81.014, 33.974], [-81.015, 33.975], [-81.016, 33.975], [-81.016, 33.974], [-81.016, 33.973], [-81.015, 33.973], [-81.015, 33.972], [-81.016, 33.972], [-81.016, 33.971], [-81.016, 33.972], [-81.017, 33.972], [-81.016, 33.972], [-81.017, 33.972], [-81.017, 33.971], [-81.018, 33.971], [-81.018, 33.972], [-81.019, 33.971], [-81.02, 33.972], [-81.021, 33.973], [-81.022, 33.971], [-81.023, 33.972], [-81.023, 33.971], [-81.024, 33.971], [-81.025, 33.972], [-81.026, 33.972], [-81.025, 33.973], [-81.023, 33.975], [-81.024, 33.975], [-81.024, 33.976], [-81.025, 33.976], [-81.026, 33.976], [-81.026, 33.975], [-81.027, 33.976], [-81.026, 33.977], [-81.025, 33.977], [-81.024, 33.977], [-81.024, 33.978], [-81.025, 33.978], [-81.025, 33.979], [-81.024, 33.979], [-81.023, 33.979], [-81.023, 33.98], [-81.023, 33.981], [-81.024, 33.981], [-81.023, 33.981], [-81.023, 33.982], [-81.024, 33.982], [-81.025, 33.982], [-81.025, 33.983], [-81.024, 33.983], [-81.023, 33.984], [-81.024, 33.985], [-81.025, 33.986], [-81.025, 33.987], [-81.026, 33.986], [-81.026, 33.987], [-81.026, 33.986], [-81.027, 33.986], [-81.026, 33.986], [-81.025, 33.985], [-81.026, 33.985], [-81.026, 33.984], [-81.026, 33.983], [-81.027, 33.982], [-81.028, 33.982], [-81.027, 33.981], [-81.028, 33.981], [-81.029, 33.981], [-81.03, 33.981], [-81.029, 33.982], [-81.029, 33.983], [-81.03, 33.983], [-81.029, 33.984], [-81.028, 33.984], [-81.028, 33.985], [-81.027, 33.985], [-81.027, 33.986], [-81.028, 33.986], [-81.029, 33.986], [-81.028, 33.985], [-81.029, 33.985], [-81.03, 33.985], [-81.031, 33.985], [-81.032, 33.985], [-81.033, 33.985], [-81.033, 33.984], [-81.033, 33.983], [-81.033, 33.984], [-81.034, 33.984], [-81.035, 33.984], [-81.035, 33.983], [-81.035, 33.982], [-81.036, 33.981], [-81.035, 33.981], [-81.037, 33.98], [-81.038, 33.98], [-81.039, 33.98], [-81.039, 33.981], [-81.04, 33.982], [-81.041, 33.982], [-81.041, 33.981], [-81.042, 33.981], [-81.042, 33.98], [-81.043, 33.979], [-81.044, 33.978], [-81.045, 33.978], [-81.046, 33.978], [-81.046, 33.98], [-81.046, 33.983], [-81.046, 33.986], [-81.046, 33.987], [-81.046, 33.988], [-81.047, 33.988], [-81.048, 33.991], [-81.051, 33.996], [-81.053, 33.998], [-81.054, 33.999], [-81.054, 34], [-81.055, 34], [-81.055, 34.001], [-81.055, 34.002], [-81.056, 34.002], [-81.059, 34.004], [-81.059, 34.005], [-81.06, 34.005], [-81.061, 34.005], [-81.063, 34.006], [-81.064, 34.007], [-81.064, 34.008], [-81.065, 34.009], [-81.066, 34.009], [-81.067, 34.009], [-81.068, 34.009], [-81.07, 34.008], [-81.071, 34.007], [-81.072, 34.007], [-81.073, 34.007], [-81.074, 34.007], [-81.076, 34.008], [-81.077, 34.009], [-81.079, 34.011], [-81.08, 34.012], [-81.082, 34.012], [-81.084, 34.012], [-81.086, 34.014], [-81.088, 34.014], [-81.089, 34.014], [-81.09, 34.014], [-81.091, 34.014], [-81.092, 34.014], [-81.095, 34.016], [-81.097, 34.018], [-81.098, 34.019], [-81.099, 34.022], [-81.1, 34.024], [-81.101, 34.024], [-81.102, 34.024], [-81.104, 34.023], [-81.104, 34.024], [-81.103, 34.024], [-81.104, 34.025], [-81.104, 34.026], [-81.105, 34.026], [-81.108, 34.026], [-81.109, 34.026], [-81.11, 34.026], [-81.112, 34.026], [-81.112, 34.027], [-81.113, 34.027], [-81.113, 34.028], [-81.112, 34.028], [-81.111, 34.028], [-81.11, 34.027], [-81.11, 34.028], [-81.11, 34.029], [-81.11, 34.031], [-81.11, 34.03], [-81.11, 34.031], [-81.109, 34.031], [-81.108, 34.031], [-81.107, 34.031], [-81.106, 34.031], [-81.105, 34.032], [-81.105, 34.033], [-81.104, 34.032], [-81.104, 34.031], [-81.103, 34.031], [-81.102, 34.031], [-81.103, 34.031], [-81.103, 34.03], [-81.102, 34.03], [-81.101, 34.03], [-81.101, 34.029], [-81.101, 34.028], [-81.1, 34.028], [-81.1, 34.027], [-81.1, 34.026], [-81.099, 34.026], [-81.098, 34.025], [-81.098, 34.026], [-81.097, 34.026], [-81.097, 34.025], [-81.098, 34.025], [-81.097, 34.025], [-81.097, 34.024], [-81.096, 34.025], [-81.095, 34.025], [-81.096, 34.024], [-81.096, 34.023], [-81.095, 34.023], [-81.095, 34.024], [-81.095, 34.025], [-81.094, 34.025], [-81.093, 34.025], [-81.093, 34.026], [-81.093, 34.027], [-81.094, 34.027], [-81.094, 34.028], [-81.094, 34.029], [-81.093, 34.029], [-81.092, 34.029], [-81.092, 34.03], [-81.093, 34.03], [-81.093, 34.031], [-81.094, 34.031], [-81.095, 34.031], [-81.096, 34.031], [-81.096, 34.03], [-81.097, 34.03], [-81.097, 34.031], [-81.097, 34.03], [-81.098, 34.03], [-81.099, 34.03], [-81.1, 34.03], [-81.099, 34.03], [-81.1, 34.032], [-81.099, 34.032], [-81.098, 34.032], [-81.098, 34.033], [-81.097, 34.033], [-81.097, 34.032], [-81.096, 34.032], [-81.095, 34.033], [-81.093, 34.034], [-81.093, 34.035], [-81.092, 34.035], [-81.091, 34.035], [-81.09, 34.035], [-81.089, 34.035], [-81.089, 34.034], [-81.088, 34.034], [-81.087, 34.034], [-81.087, 34.033], [-81.088, 34.033], [-81.089, 34.033], [-81.091, 34.032], [-81.091, 34.031], [-81.09, 34.031], [-81.089, 34.031], [-81.088, 34.031], [-81.087, 34.031], [-81.086, 34.031], [-81.087, 34.031], [-81.087, 34.03], [-81.087, 34.029], [-81.087, 34.027], [-81.086, 34.026], [-81.087, 34.026], [-81.086, 34.026], [-81.087, 34.026], [-81.088, 34.025], [-81.088, 34.024], [-81.087, 34.024], [-81.087, 34.023], [-81.086, 34.023], [-81.087, 34.023], [-81.087, 34.022], [-81.087, 34.023], [-81.088, 34.022], [-81.087, 34.022], [-81.088, 34.022], [-81.088, 34.021], [-81.088, 34.02], [-81.087, 34.02], [-81.086, 34.02], [-81.086, 34.018], [-81.085, 34.018], [-81.084, 34.018], [-81.083, 34.018], [-81.085, 34.017], [-81.084, 34.017], [-81.085, 34.016], [-81.085, 34.015], [-81.086, 34.015], [-81.086, 34.014], [-81.085, 34.015], [-81.085, 34.016], [-81.084, 34.016], [-81.083, 34.016], [-81.082, 34.016], [-81.082, 34.017], [-81.082, 34.016], [-81.081, 34.016], [-81.08, 34.016], [-81.081, 34.017], [-81.081, 34.019], [-81.082, 34.02], [-81.081, 34.02], [-81.078, 34.02], [-81.078, 34.021], [-81.077, 34.021], [-81.078, 34.022], [-81.078, 34.023], [-81.079, 34.022], [-81.08, 34.022], [-81.081, 34.023], [-81.081, 34.024], [-81.083, 34.025], [-81.082, 34.026], [-81.081, 34.027], [-81.08, 34.027], [-81.08, 34.026], [-81.079, 34.026], [-81.079, 34.025], [-81.079, 34.024], [-81.078, 34.024], [-81.078, 34.023], [-81.077, 34.023], [-81.078, 34.024], [-81.077, 34.024], [-81.078, 34.025], [-81.077, 34.025], [-81.077, 34.026], [-81.078, 34.027], [-81.079, 34.027], [-81.079, 34.028], [-81.079, 34.027], [-81.08, 34.027], [-81.081, 34.028], [-81.081, 34.027], [-81.081, 34.028], [-81.08, 34.028], [-81.08, 34.029], [-81.08, 34.03], [-81.081, 34.03], [-81.08, 34.03], [-81.08, 34.031], [-81.079, 34.031], [-81.079, 34.032], [-81.08, 34.032], [-81.08, 34.033], [-81.08, 34.034], [-81.079, 34.035], [-81.079, 34.034], [-81.078, 34.034], [-81.078, 34.035], [-81.077, 34.035], [-81.077, 34.036], [-81.076, 34.036], [-81.076, 34.035], [-81.075, 34.035], [-81.076, 34.036], [-81.076, 34.037], [-81.076, 34.038], [-81.072, 34.038], [-81.072, 34.039], [-81.072, 34.04], [-81.072, 34.041], [-81.073, 34.042], [-81.073, 34.043], [-81.073, 34.044], [-81.074, 34.044], [-81.074, 34.045], [-81.075, 34.044], [-81.076, 34.044], [-81.078, 34.044], [-81.078, 34.045], [-81.079, 34.045], [-81.08, 34.045], [-81.08, 34.046], [-81.079, 34.046], [-81.08, 34.047], [-81.079, 34.047], [-81.078, 34.048], [-81.078, 34.049], [-81.078, 34.048], [-81.077, 34.048], [-81.076, 34.048], [-81.075, 34.049], [-81.075, 34.05], [-81.075, 34.051], [-81.076, 34.051], [-81.076, 34.052], [-81.076, 34.053], [-81.076, 34.054], [-81.076, 34.055], [-81.076, 34.056], [-81.077, 34.056], [-81.077, 34.057], [-81.078, 34.056], [-81.079, 34.056], [-81.08, 34.056], [-81.081, 34.055], [-81.081, 34.056], [-81.082, 34.056], [-81.083, 34.058], [-81.084, 34.058], [-81.085, 34.058], [-81.089, 34.058], [-81.092, 34.057], [-81.093, 34.057], [-81.093, 34.056], [-81.094, 34.056], [-81.095, 34.056], [-81.095, 34.055], [-81.096, 34.055], [-81.096, 34.054], [-81.097, 34.055], [-81.096, 34.055], [-81.097, 34.057], [-81.098, 34.056], [-81.099, 34.056], [-81.099, 34.055], [-81.099, 34.056], [-81.1, 34.056], [-81.101, 34.055], [-81.102, 34.054], [-81.102, 34.053], [-81.102, 34.052], [-81.101, 34.052], [-81.101, 34.051], [-81.101, 34.052], [-81.102, 34.052], [-81.102, 34.051], [-81.102, 34.05], [-81.103, 34.05], [-81.103, 34.051], [-81.104, 34.051], [-81.104, 34.05], [-81.104, 34.051], [-81.104, 34.052], [-81.105, 34.051], [-81.106, 34.052], [-81.107, 34.052], [-81.108, 34.052], [-81.108, 34.053], [-81.109, 34.053], [-81.109, 34.054], [-81.11, 34.054], [-81.11, 34.055], [-81.111, 34.055], [-81.111, 34.056], [-81.11, 34.056], [-81.11, 34.055], [-81.109, 34.055], [-81.108, 34.056], [-81.107, 34.056], [-81.108, 34.057], [-81.109, 34.057], [-81.109, 34.058], [-81.108, 34.059], [-81.107, 34.059], [-81.106, 34.059], [-81.105, 34.06], [-81.105, 34.061], [-81.107, 34.06], [-81.108, 34.06], [-81.109, 34.06], [-81.109, 34.061], [-81.11, 34.061], [-81.106, 34.063], [-81.107, 34.063], [-81.107, 34.064], [-81.108, 34.064], [-81.109, 34.064], [-81.112, 34.063], [-81.113, 34.063], [-81.114, 34.062], [-81.115, 34.063], [-81.115, 34.064], [-81.114, 34.064], [-81.113, 34.064], [-81.11, 34.064], [-81.109, 34.065], [-81.108, 34.065], [-81.109, 34.065], [-81.108, 34.065], [-81.108, 34.066], [-81.109, 34.066], [-81.109, 34.067], [-81.11, 34.068], [-81.113, 34.067], [-81.114, 34.066], [-81.116, 34.065], [-81.117, 34.066], [-81.117, 34.067], [-81.118, 34.069], [-81.119, 34.069], [-81.119, 34.07], [-81.12, 34.07], [-81.121, 34.07], [-81.12, 34.07], [-81.12, 34.071], [-81.121, 34.071], [-81.122, 34.072], [-81.122, 34.073], [-81.123, 34.073], [-81.124, 34.072], [-81.124, 34.073], [-81.125, 34.072], [-81.126, 34.072], [-81.126, 34.071], [-81.127, 34.071], [-81.126, 34.071], [-81.125, 34.071], [-81.126, 34.07], [-81.127, 34.07], [-81.127, 34.069], [-81.126, 34.068], [-81.125, 34.068], [-81.125, 34.067], [-81.126, 34.067], [-81.127, 34.068], [-81.129, 34.068], [-81.13, 34.068], [-81.131, 34.07], [-81.129, 34.072], [-81.128, 34.072], [-81.129, 34.073], [-81.13, 34.074], [-81.131, 34.074], [-81.131, 34.073], [-81.133, 34.073], [-81.134, 34.072], [-81.135, 34.072], [-81.135, 34.073], [-81.136, 34.072], [-81.136, 34.074], [-81.137, 34.075], [-81.138, 34.075], [-81.139, 34.075], [-81.14, 34.074], [-81.14, 34.075], [-81.139, 34.075], [-81.139, 34.076], [-81.138, 34.076], [-81.138, 34.075], [-81.137, 34.075], [-81.136, 34.075], [-81.135, 34.075], [-81.134, 34.075], [-81.132, 34.075], [-81.132, 34.076], [-81.132, 34.077], [-81.133, 34.077], [-81.133, 34.078], [-81.133, 34.079], [-81.132, 34.079], [-81.132, 34.08], [-81.131, 34.08], [-81.132, 34.08], [-81.132, 34.081], [-81.133, 34.081], [-81.134, 34.081], [-81.134, 34.082], [-81.135, 34.082], [-81.136, 34.083], [-81.137, 34.083], [-81.138, 34.083], [-81.138, 34.084], [-81.138, 34.085], [-81.138, 34.086], [-81.139, 34.086], [-81.138, 34.086], [-81.139, 34.086], [-81.139, 34.085], [-81.139, 34.086], [-81.14, 34.086], [-81.141, 34.086], [-81.142, 34.086], [-81.142, 34.087], [-81.143, 34.087], [-81.144, 34.087], [-81.144, 34.088], [-81.145, 34.088], [-81.146, 34.089], [-81.147, 34.089], [-81.148, 34.089], [-81.148, 34.088], [-81.147, 34.088], [-81.147, 34.087], [-81.146, 34.087], [-81.147, 34.087], [-81.148, 34.087], [-81.148, 34.086], [-81.149, 34.085], [-81.149, 34.083], [-81.151, 34.084], [-81.151, 34.083], [-81.152, 34.083], [-81.151, 34.083], [-81.15, 34.082], [-81.149, 34.082], [-81.148, 34.082], [-81.147, 34.082], [-81.148, 34.08], [-81.149, 34.081], [-81.15, 34.08], [-81.151, 34.08], [-81.15, 34.08], [-81.15, 34.079], [-81.15, 34.078], [-81.149, 34.078], [-81.149, 34.077], [-81.148, 34.077], [-81.147, 34.077], [-81.147, 34.076], [-81.146, 34.076], [-81.146, 34.075], [-81.145, 34.075], [-81.145, 34.074], [-81.146, 34.074], [-81.146, 34.075], [-81.147, 34.074], [-81.148, 34.074], [-81.148, 34.073], [-81.147, 34.073], [-81.146, 34.073], [-81.145, 34.072], [-81.145, 34.073], [-81.144, 34.073], [-81.143, 34.073], [-81.145, 34.072], [-81.146, 34.072], [-81.147, 34.072], [-81.147, 34.071], [-81.147, 34.072], [-81.148, 34.071], [-81.149, 34.071], [-81.15, 34.071], [-81.153, 34.069], [-81.153, 34.068], [-81.159, 34.066], [-81.16, 34.066], [-81.161, 34.067], [-81.162, 34.067], [-81.163, 34.066], [-81.164, 34.065], [-81.165, 34.065], [-81.166, 34.065], [-81.167, 34.065], [-81.167, 34.064], [-81.167, 34.065], [-81.168, 34.066], [-81.167, 34.067], [-81.168, 34.067], [-81.168, 34.068], [-81.165, 34.07], [-81.163, 34.071], [-81.163, 34.072], [-81.162, 34.072], [-81.162, 34.073], [-81.162, 34.074], [-81.162, 34.075], [-81.162, 34.076], [-81.163, 34.076], [-81.164, 34.076], [-81.164, 34.077], [-81.163, 34.076], [-81.162, 34.077], [-81.161, 34.077], [-81.161, 34.078], [-81.161, 34.079], [-81.16, 34.079], [-81.16, 34.08], [-81.161, 34.08], [-81.161, 34.081], [-81.161, 34.082], [-81.162, 34.082], [-81.162, 34.083], [-81.162, 34.082], [-81.162, 34.081], [-81.163, 34.081], [-81.163, 34.08], [-81.163, 34.079], [-81.163, 34.078], [-81.164, 34.078], [-81.164, 34.079], [-81.164, 34.08], [-81.165, 34.08], [-81.165, 34.081], [-81.166, 34.082], [-81.166, 34.083], [-81.165, 34.083], [-81.164, 34.083], [-81.164, 34.084], [-81.166, 34.085], [-81.165, 34.085], [-81.165, 34.086], [-81.164, 34.086], [-81.164, 34.087], [-81.165, 34.088], [-81.166, 34.088], [-81.166, 34.089], [-81.167, 34.089], [-81.168, 34.089], [-81.168, 34.09], [-81.169, 34.09], [-81.17, 34.09], [-81.171, 34.09], [-81.171, 34.091]], [[-81.074, 34.023], [-81.074, 34.024], [-81.075, 34.024], [-81.074, 34.023]], [[-81.074, 34.023], [-81.075, 34.023], [-81.075, 34.024], [-81.076, 34.024], [-81.076, 34.023], [-81.075, 34.023], [-81.075, 34.024], [-81.075, 34.023], [-81.075, 34.022], [-81.074, 34.021], [-81.074, 34.02], [-81.073, 34.018], [-81.072, 34.018], [-81.072, 34.017], [-81.071, 34.017], [-81.071, 34.016], [-81.07, 34.016], [-81.07, 34.015], [-81.069, 34.015], [-81.068, 34.014], [-81.068, 34.013], [-81.067, 34.013], [-81.066, 34.013], [-81.066, 34.014], [-81.067, 34.014], [-81.067, 34.015], [-81.068, 34.015], [-81.068, 34.016], [-81.069, 34.016], [-81.068, 34.016], [-81.068, 34.017], [-81.067, 34.017], [-81.068, 34.017], [-81.067, 34.017], [-81.068, 34.016], [-81.067, 34.016], [-81.066, 34.016], [-81.067, 34.016], [-81.066, 34.016], [-81.066, 34.015], [-81.065, 34.015], [-81.065, 34.016], [-81.066, 34.016], [-81.065, 34.016], [-81.066, 34.016], [-81.066, 34.017], [-81.065, 34.017], [-81.065, 34.018], [-81.064, 34.018], [-81.064, 34.019], [-81.065, 34.019], [-81.065, 34.02], [-81.066, 34.019], [-81.066, 34.02], [-81.067, 34.02], [-81.068, 34.02], [-81.069, 34.02], [-81.073, 34.02], [-81.073, 34.021], [-81.074, 34.021], [-81.074, 34.022], [-81.074, 34.023]], [[-81.056, 34.028], [-81.056, 34.027], [-81.055, 34.027], [-81.056, 34.027], [-81.056, 34.028]], [[-81.055, 34.027], [-81.056, 34.027], [-81.056, 34.026], [-81.056, 34.025], [-81.055, 34.025], [-81.056, 34.025], [-81.056, 34.024], [-81.055, 34.024], [-81.054, 34.024], [-81.054, 34.025], [-81.055, 34.025], [-81.055, 34.026], [-81.054, 34.026], [-81.055, 34.026], [-81.055, 34.027]], [[-80.843, 34.119], [-80.843, 34.118], [-80.842, 34.118], [-80.841, 34.117], [-80.843, 34.115], [-80.842, 34.114], [-80.841, 34.114], [-80.839, 34.112], [-80.838, 34.112], [-80.838, 34.111], [-80.832, 34.107], [-80.832, 34.108], [-80.831, 34.107], [-80.83, 34.107], [-80.831, 34.107], [-80.83, 34.107], [-80.83, 34.106], [-80.829, 34.106], [-80.828, 34.106], [-80.827, 34.106], [-80.826, 34.107], [-80.826, 34.108], [-80.825, 34.109], [-80.824, 34.109], [-80.823, 34.11], [-80.822, 34.11], [-80.823, 34.109], [-80.822, 34.109], [-80.822, 34.108], [-80.821, 34.108], [-80.82, 34.111], [-80.82, 34.112], [-80.819, 34.112], [-80.82, 34.113], [-80.821, 34.113], [-80.822, 34.113], [-80.822, 34.114], [-80.823, 34.114], [-80.824, 34.115], [-80.825, 34.114], [-80.825, 34.115], [-80.826, 34.115], [-80.827, 34.115], [-80.828, 34.115], [-80.828, 34.116], [-80.828, 34.117], [-80.829, 34.117], [-80.83, 34.116], [-80.83, 34.115], [-80.831, 34.114], [-80.83, 34.114], [-80.828, 34.114], [-80.829, 34.113], [-80.83, 34.112], [-80.831, 34.112], [-80.832, 34.112], [-80.832, 34.111], [-80.833, 34.111], [-80.834, 34.112], [-80.835, 34.112], [-80.835, 34.113], [-80.836, 34.113], [-80.836, 34.114], [-80.837, 34.115], [-80.838, 34.115], [-80.838, 34.116], [-80.837, 34.116], [-80.836, 34.116], [-80.836, 34.117], [-80.836, 34.118], [-80.837, 34.118], [-80.838, 34.119], [-80.839, 34.119], [-80.84, 34.119], [-80.841, 34.119], [-80.842, 34.119], [-80.843, 34.119]], [[-80.964, 33.958], [-80.963, 33.958], [-80.962, 33.958], [-80.961, 33.958], [-80.961, 33.959], [-80.96, 33.959], [-80.959, 33.959], [-80.959, 33.96], [-80.958, 33.96], [-80.958, 33.959], [-80.957, 33.959], [-80.957, 33.96], [-80.956, 33.96], [-80.955, 33.96], [-80.955, 33.963], [-80.955, 33.964], [-80.955, 33.966], [-80.956, 33.967], [-80.954, 33.967], [-80.954, 33.968], [-80.955, 33.968], [-80.956, 33.968], [-80.956, 33.969], [-80.956, 33.972], [-80.956, 33.973], [-80.956, 33.974], [-80.956, 33.975], [-80.957, 33.975], [-80.957, 33.974], [-80.957, 33.973], [-80.957, 33.972], [-80.957, 33.969], [-80.957, 33.968], [-80.958, 33.968], [-80.959, 33.969], [-80.96, 33.968], [-80.961, 33.967], [-80.961, 33.966], [-80.961, 33.965], [-80.962, 33.965], [-80.962, 33.964], [-80.961, 33.964], [-80.962, 33.964], [-80.962, 33.963], [-80.963, 33.962], [-80.963, 33.961], [-80.963, 33.96], [-80.963, 33.959], [-80.963, 33.958], [-80.964, 33.958]], [[-80.989, 33.988], [-80.989, 33.987], [-80.988, 33.987], [-80.989, 33.987], [-80.989, 33.986], [-80.988, 33.986], [-80.987, 33.986], [-80.988, 33.986], [-80.989, 33.986], [-80.989, 33.985], [-80.988, 33.985], [-80.989, 33.985], [-80.989, 33.984], [-80.989, 33.983], [-80.988, 33.983], [-80.988, 33.982], [-80.987, 33.982], [-80.987, 33.981], [-80.988, 33.981], [-80.987, 33.981], [-80.987, 33.982], [-80.987, 33.981], [-80.986, 33.981], [-80.986, 33.982], [-80.986, 33.981], [-80.985, 33.981], [-80.984, 33.981], [-80.984, 33.98], [-80.984, 33.981], [-80.983, 33.981], [-80.983, 33.98], [-80.982, 33.98], [-80.982, 33.981], [-80.983, 33.981], [-80.982, 33.981], [-80.983, 33.981], [-80.984, 33.981], [-80.983, 33.981], [-80.983, 33.982], [-80.982, 33.982], [-80.982, 33.981], [-80.981, 33.981], [-80.981, 33.982], [-80.98, 33.982], [-80.98, 33.981], [-80.979, 33.981], [-80.979, 33.982], [-80.98, 33.982], [-80.98, 33.983], [-80.981, 33.983], [-80.982, 33.983], [-80.983, 33.983], [-80.983, 33.984], [-80.982, 33.984], [-80.983, 33.984], [-80.983, 33.985], [-80.982, 33.985], [-80.983, 33.985], [-80.983, 33.986], [-80.982, 33.986], [-80.982, 33.987], [-80.982, 33.986], [-80.979, 33.986], [-80.979, 33.987], [-80.98, 33.987], [-80.981, 33.987], [-80.982, 33.987], [-80.983, 33.987], [-80.983, 33.988], [-80.982, 33.988], [-80.983, 33.988], [-80.983, 33.987], [-80.984, 33.987], [-80.984, 33.988], [-80.985, 33.988], [-80.985, 33.987], [-80.984, 33.987], [-80.984, 33.986], [-80.983, 33.986], [-80.984, 33.986], [-80.984, 33.987], [-80.985, 33.987], [-80.986, 33.987], [-80.987, 33.987], [-80.987, 33.988], [-80.988, 33.988], [-80.989, 33.988]], [[-80.955, 33.979], [-80.954, 33.979], [-80.953, 33.979], [-80.952, 33.979], [-80.951, 33.979], [-80.95, 33.979], [-80.949, 33.978], [-80.948, 33.978], [-80.947, 33.978], [-80.947, 33.977], [-80.946, 33.977], [-80.945, 33.977], [-80.944, 33.976], [-80.944, 33.977], [-80.945, 33.979], [-80.946, 33.979], [-80.947, 33.981], [-80.948, 33.981], [-80.949, 33.981], [-80.95, 33.981], [-80.951, 33.981], [-80.95, 33.982], [-80.95, 33.984], [-80.95, 33.985], [-80.952, 33.984], [-80.953, 33.984], [-80.954, 33.984], [-80.954, 33.982], [-80.954, 33.979], [-80.955, 33.979]], [[-81.016, 34.045], [-81.015, 34.044], [-81.014, 34.045], [-81.014, 34.044], [-81.015, 34.043], [-81.014, 34.043], [-81.014, 34.042], [-81.014, 34.041], [-81.013, 34.041], [-81.012, 34.041], [-81.012, 34.042], [-81.011, 34.042], [-81.011, 34.043], [-81.01, 34.044], [-81.009, 34.044], [-81.009, 34.045], [-81.008, 34.046], [-81.008, 34.047], [-81.008, 34.048], [-81.009, 34.047], [-81.01, 34.047], [-81.01, 34.048], [-81.011, 34.048], [-81.011, 34.047], [-81.012, 34.048], [-81.013, 34.047], [-81.013, 34.048], [-81.013, 34.049], [-81.012, 34.049], [-81.013, 34.049], [-81.013, 34.05], [-81.014, 34.05], [-81.015, 34.049], [-81.015, 34.048], [-81.014, 34.047], [-81.014, 34.048], [-81.013, 34.048], [-81.013, 34.047], [-81.014, 34.047], [-81.015, 34.046], [-81.015, 34.045], [-81.015, 34.046], [-81.016, 34.046], [-81.016, 34.045]], [[-80.973, 33.979], [-80.973, 33.978], [-80.972, 33.978], [-80.972, 33.977], [-80.973, 33.977], [-80.974, 33.977], [-80.973, 33.977], [-80.973, 33.976], [-80.973, 33.975], [-80.972, 33.975], [-80.971, 33.975], [-80.969, 33.974], [-80.968, 33.974], [-80.968, 33.975], [-80.968, 33.976], [-80.967, 33.976], [-80.967, 33.977], [-80.967, 33.978], [-80.966, 33.979], [-80.966, 33.98], [-80.966, 33.981], [-80.967, 33.981], [-80.967, 33.98], [-80.967, 33.979], [-80.968, 33.979], [-80.968, 33.98], [-80.969, 33.98], [-80.968, 33.979], [-80.969, 33.979], [-80.97, 33.979], [-80.971, 33.979], [-80.972, 33.98], [-80.972, 33.979], [-80.973, 33.979]], [[-81.015, 34.035], [-81.014, 34.035], [-81.014, 34.034], [-81.013, 34.034], [-81.013, 34.035], [-81.013, 34.034], [-81.012, 34.034], [-81.011, 34.034], [-81.01, 34.034], [-81.009, 34.034], [-81.009, 34.033], [-81.009, 34.032], [-81.008, 34.032], [-81.007, 34.032], [-81.006, 34.032], [-81.006, 34.033], [-81.005, 34.033], [-81.005, 34.034], [-81.006, 34.034], [-81.006, 34.035], [-81.004, 34.035], [-81.005, 34.035], [-81.005, 34.036], [-81.005, 34.037], [-81.004, 34.037], [-81.003, 34.037], [-81.004, 34.038], [-81.005, 34.038], [-81.005, 34.037], [-81.006, 34.037], [-81.007, 34.037], [-81.008, 34.037], [-81.009, 34.037], [-81.01, 34.037], [-81.01, 34.036], [-81.011, 34.036], [-81.012, 34.036], [-81.012, 34.035], [-81.013, 34.035], [-81.013, 34.036], [-81.013, 34.035], [-81.014, 34.035], [-81.015, 34.035]], [[-80.97, 33.952], [-80.969, 33.952], [-80.968, 33.952], [-80.965, 33.953], [-80.962, 33.954], [-80.963, 33.954], [-80.963, 33.955], [-80.964, 33.955], [-80.965, 33.955], [-80.966, 33.954], [-80.967, 33.954], [-80.969, 33.955], [-80.972, 33.957], [-80.973, 33.957], [-80.974, 33.955], [-80.975, 33.955], [-80.97, 33.952]], [[-80.996, 34.077], [-80.995, 34.077], [-80.994, 34.076], [-80.994, 34.075], [-80.995, 34.075], [-80.995, 34.074], [-80.994, 34.074], [-80.994, 34.073], [-80.995, 34.073], [-80.994, 34.071], [-80.992, 34.069], [-80.991, 34.068], [-80.991, 34.067], [-80.99, 34.067], [-80.99, 34.068], [-80.991, 34.068], [-80.99, 34.068], [-80.992, 34.071], [-80.993, 34.072], [-80.993, 34.073], [-80.993, 34.074], [-80.992, 34.074], [-80.988, 34.073], [-80.988, 34.074], [-80.99, 34.075], [-80.991, 34.075], [-80.992, 34.076], [-80.993, 34.076], [-80.995, 34.077], [-80.996, 34.077]], [[-80.978, 33.969], [-80.977, 33.969], [-80.976, 33.968], [-80.977, 33.968], [-80.975, 33.967], [-80.974, 33.966], [-80.974, 33.967], [-80.974, 33.966], [-80.973, 33.966], [-80.972, 33.965], [-80.971, 33.965], [-80.97, 33.964], [-80.97, 33.965], [-80.97, 33.964], [-80.969, 33.964], [-80.968, 33.965], [-80.969, 33.965], [-80.968, 33.965], [-80.968, 33.966], [-80.968, 33.965], [-80.968, 33.966], [-80.969, 33.966], [-80.969, 33.967], [-80.97, 33.967], [-80.971, 33.967], [-80.972, 33.968], [-80.973, 33.967], [-80.973, 33.968], [-80.972, 33.968], [-80.973, 33.968], [-80.975, 33.969], [-80.976, 33.969], [-80.975, 33.969], [-80.975, 33.97], [-80.977, 33.97], [-80.978, 33.969]], [[-81.012, 34.05], [-81.011, 34.05], [-81.01, 34.05], [-81.009, 34.05], [-81.008, 34.051], [-81.007, 34.051], [-81.007, 34.05], [-81.007, 34.049], [-81.008, 34.049], [-81.008, 34.048], [-81.007, 34.048], [-81.006, 34.048], [-81.005, 34.049], [-81.004, 34.049], [-81.003, 34.049], [-81.002, 34.05], [-81.003, 34.05], [-81.004, 34.051], [-81.005, 34.051], [-81.006, 34.051], [-81.006, 34.052], [-81.007, 34.052], [-81.007, 34.053], [-81.006, 34.053], [-81.007, 34.053], [-81.008, 34.052], [-81.01, 34.052], [-81.01, 34.051], [-81.011, 34.051], [-81.012, 34.051], [-81.012, 34.05]], [[-80.926, 33.98], [-80.926, 33.979], [-80.925, 33.979], [-80.924, 33.979], [-80.923, 33.98], [-80.922, 33.98], [-80.921, 33.98], [-80.92, 33.98], [-80.919, 33.98], [-80.918, 33.98], [-80.917, 33.98], [-80.918, 33.981], [-80.917, 33.98], [-80.917, 33.981], [-80.918, 33.981], [-80.919, 33.981], [-80.918, 33.981], [-80.919, 33.981], [-80.919, 33.982], [-80.92, 33.982], [-80.921, 33.982], [-80.922, 33.983], [-80.923, 33.982], [-80.925, 33.981], [-80.926, 33.98]], [[-81.032, 34.057], [-81.031, 34.057], [-81.03, 34.056], [-81.029, 34.055], [-81.029, 34.056], [-81.029, 34.055], [-81.028, 34.055], [-81.027, 34.056], [-81.028, 34.056], [-81.029, 34.057], [-81.029, 34.056], [-81.03, 34.057], [-81.031, 34.057], [-81.03, 34.058], [-81.027, 34.057], [-81.027, 34.058], [-81.026, 34.058], [-81.026, 34.059], [-81.025, 34.059], [-81.025, 34.06], [-81.026, 34.06], [-81.026, 34.061], [-81.028, 34.06], [-81.029, 34.059], [-81.03, 34.058], [-81.031, 34.058], [-81.032, 34.057]], [[-81.063, 34.022], [-81.061, 34.02], [-81.06, 34.02], [-81.059, 34.02], [-81.058, 34.021], [-81.057, 34.022], [-81.058, 34.022], [-81.058, 34.023], [-81.059, 34.023], [-81.06, 34.023], [-81.06, 34.024], [-81.062, 34.023], [-81.062, 34.022], [-81.063, 34.022]], [[-81.079, 34.031], [-81.079, 34.03], [-81.079, 34.031], [-81.079, 34.03], [-81.078, 34.03], [-81.077, 34.03], [-81.077, 34.029], [-81.077, 34.03], [-81.076, 34.03], [-81.075, 34.03], [-81.075, 34.031], [-81.075, 34.032], [-81.075, 34.033], [-81.077, 34.033], [-81.078, 34.033], [-81.078, 34.034], [-81.079, 34.033], [-81.078, 34.033], [-81.078, 34.032], [-81.078, 34.031], [-81.079, 34.031]], [[-81.024, 34.039], [-81.024, 34.038], [-81.024, 34.039], [-81.024, 34.038], [-81.024, 34.037], [-81.024, 34.036], [-81.023, 34.036], [-81.023, 34.037], [-81.022, 34.037], [-81.022, 34.038], [-81.022, 34.037], [-81.022, 34.036], [-81.021, 34.036], [-81.02, 34.036], [-81.02, 34.037], [-81.021, 34.037], [-81.02, 34.037], [-81.021, 34.038], [-81.021, 34.039], [-81.02, 34.039], [-81.021, 34.04], [-81.022, 34.04], [-81.022, 34.039], [-81.023, 34.039], [-81.023, 34.038], [-81.023, 34.039], [-81.024, 34.039], [-81.023, 34.039], [-81.024, 34.04], [-81.024, 34.039]], [[-81.147, 34.09], [-81.146, 34.089], [-81.145, 34.089], [-81.146, 34.089], [-81.145, 34.089], [-81.145, 34.088], [-81.144, 34.088], [-81.143, 34.088], [-81.142, 34.088], [-81.143, 34.088], [-81.142, 34.087], [-81.141, 34.087], [-81.141, 34.088], [-81.143, 34.09], [-81.145, 34.091], [-81.146, 34.091], [-81.146, 34.09], [-81.147, 34.09]], [[-80.95, 33.968], [-80.949, 33.967], [-80.948, 33.967], [-80.949, 33.968], [-80.948, 33.968], [-80.947, 33.968], [-80.948, 33.969], [-80.947, 33.968], [-80.947, 33.969], [-80.946, 33.969], [-80.947, 33.969], [-80.947, 33.97], [-80.946, 33.971], [-80.947, 33.971], [-80.948, 33.97], [-80.947, 33.971], [-80.948, 33.972], [-80.949, 33.972], [-80.948, 33.972], [-80.948, 33.971], [-80.949, 33.97], [-80.949, 33.971], [-80.95, 33.97], [-80.949, 33.97], [-80.949, 33.969], [-80.95, 33.969], [-80.95, 33.968]], [[-80.821, 34.108], [-80.82, 34.108], [-80.818, 34.109], [-80.817, 34.109], [-80.818, 34.111], [-80.818, 34.112], [-80.819, 34.112], [-80.82, 34.111], [-80.821, 34.108]], [[-80.982, 33.984], [-80.982, 33.983], [-80.98, 33.984], [-80.98, 33.983], [-80.977, 33.983], [-80.977, 33.984], [-80.977, 33.985], [-80.976, 33.985], [-80.976, 33.986], [-80.977, 33.986], [-80.977, 33.985], [-80.978, 33.985], [-80.981, 33.985], [-80.982, 33.985], [-80.982, 33.984]], [[-80.992, 33.976], [-80.992, 33.975], [-80.991, 33.975], [-80.991, 33.974], [-80.99, 33.974], [-80.99, 33.975], [-80.99, 33.976], [-80.99, 33.977], [-80.99, 33.978], [-80.991, 33.978], [-80.99, 33.978], [-80.991, 33.978], [-80.99, 33.978], [-80.99, 33.979], [-80.991, 33.979], [-80.992, 33.978], [-80.992, 33.977], [-80.992, 33.976], [-80.993, 33.976], [-80.992, 33.976]], [[-81.106, 34.03], [-81.106, 34.029], [-81.106, 34.027], [-81.105, 34.027], [-81.104, 34.027], [-81.104, 34.03], [-81.105, 34.03], [-81.106, 34.03]], [[-80.821, 34.119], [-80.82, 34.119], [-80.82, 34.118], [-80.82, 34.117], [-80.819, 34.116], [-80.819, 34.115], [-80.818, 34.115], [-80.817, 34.116], [-80.818, 34.116], [-80.818, 34.117], [-80.819, 34.117], [-80.819, 34.118], [-80.818, 34.118], [-80.819, 34.118], [-80.819, 34.119], [-80.82, 34.12], [-80.821, 34.12], [-80.821, 34.119]], [[-81.017, 34.036], [-81.016, 34.036], [-81.015, 34.036], [-81.015, 34.037], [-81.014, 34.037], [-81.014, 34.038], [-81.014, 34.039], [-81.013, 34.039], [-81.014, 34.039], [-81.013, 34.039], [-81.013, 34.04], [-81.014, 34.04], [-81.015, 34.04], [-81.015, 34.039], [-81.015, 34.038], [-81.016, 34.038], [-81.016, 34.037], [-81.017, 34.037], [-81.017, 34.036]], [[-81.092, 34.023], [-81.091, 34.022], [-81.092, 34.022], [-81.091, 34.021], [-81.09, 34.022], [-81.091, 34.023], [-81.09, 34.022], [-81.089, 34.021], [-81.089, 34.022], [-81.088, 34.022], [-81.089, 34.023], [-81.089, 34.022], [-81.089, 34.023], [-81.09, 34.023], [-81.089, 34.023], [-81.09, 34.024], [-81.089, 34.024], [-81.088, 34.024], [-81.089, 34.024], [-81.089, 34.025], [-81.089, 34.024], [-81.091, 34.023], [-81.092, 34.023]], [[-80.975, 33.942], [-80.975, 33.941], [-80.974, 33.942], [-80.976, 33.944], [-80.977, 33.943], [-80.975, 33.942]], [[-81.066, 34.027], [-81.066, 34.026], [-81.066, 34.027], [-81.065, 34.027], [-81.064, 34.027], [-81.063, 34.027], [-81.063, 34.028], [-81.064, 34.029], [-81.065, 34.028], [-81.066, 34.028], [-81.065, 34.027], [-81.066, 34.027]], [[-80.954, 33.977], [-80.953, 33.977], [-80.952, 33.975], [-80.951, 33.976], [-80.951, 33.977], [-80.952, 33.977], [-80.952, 33.978], [-80.953, 33.978], [-80.954, 33.978], [-80.954, 33.977]], [[-80.969, 33.958], [-80.968, 33.958], [-80.967, 33.958], [-80.965, 33.957], [-80.965, 33.958], [-80.967, 33.959], [-80.966, 33.959], [-80.968, 33.959], [-80.969, 33.958]], [[-81.094, 34.021], [-81.093, 34.021], [-81.094, 34.021], [-81.093, 34.021], [-81.093, 34.02], [-81.092, 34.02], [-81.092, 34.021], [-81.092, 34.022], [-81.093, 34.022], [-81.094, 34.022], [-81.094, 34.021]], [[-80.855, 34.091], [-80.855, 34.09], [-80.854, 34.09], [-80.854, 34.091], [-80.853, 34.091], [-80.852, 34.091], [-80.853, 34.092], [-80.854, 34.092], [-80.855, 34.092], [-80.855, 34.091]], [[-81.082, 34.014], [-81.08, 34.014], [-81.079, 34.015], [-81.079, 34.016], [-81.08, 34.016], [-81.08, 34.015], [-81.081, 34.016], [-81.082, 34.015], [-81.082, 34.014]], [[-80.868, 34.086], [-80.867, 34.086], [-80.866, 34.086], [-80.865, 34.086], [-80.865, 34.087], [-80.866, 34.087], [-80.868, 34.087], [-80.868, 34.086]], [[-80.96, 33.955], [-80.959, 33.955], [-80.959, 33.954], [-80.96, 33.954], [-80.958, 33.954], [-80.957, 33.955], [-80.958, 33.955], [-80.959, 33.955], [-80.96, 33.955]], [[-80.956, 33.989], [-80.955, 33.989], [-80.954, 33.99], [-80.954, 33.988], [-80.954, 33.989], [-80.954, 33.99], [-80.955, 33.991], [-80.955, 33.99], [-80.956, 33.989]], [[-81.065, 34.015], [-81.064, 34.015], [-81.063, 34.015], [-81.063, 34.016], [-81.063, 34.017], [-81.064, 34.017], [-81.065, 34.016], [-81.064, 34.016], [-81.065, 34.016], [-81.065, 34.015]], [[-80.864, 34.087], [-80.863, 34.087], [-80.86, 34.088], [-80.861, 34.088], [-80.862, 34.088], [-80.861, 34.088], [-80.862, 34.088], [-80.864, 34.087]], [[-80.98, 33.991], [-80.979, 33.99], [-80.978, 33.989], [-80.978, 33.99], [-80.979, 33.99], [-80.979, 33.991], [-80.98, 33.991], [-80.98, 33.992], [-80.979, 33.992], [-80.979, 33.991], [-80.979, 33.992], [-80.979, 33.993], [-80.98, 33.992], [-80.98, 33.991]], [[-80.954, 33.971], [-80.952, 33.971], [-80.951, 33.972], [-80.953, 33.972], [-80.954, 33.972], [-80.954, 33.971]], [[-81.025, 34.032], [-81.024, 34.032], [-81.025, 34.032], [-81.024, 34.032], [-81.024, 34.031], [-81.024, 34.032], [-81.024, 34.033], [-81.023, 34.033], [-81.023, 34.034], [-81.024, 34.034], [-81.025, 34.033], [-81.025, 34.032]], [[-80.999, 34.072], [-80.999, 34.071], [-80.998, 34.071], [-80.996, 34.072], [-80.997, 34.072], [-80.998, 34.072], [-80.999, 34.072]], [[-81.088, 34.018], [-81.088, 34.017], [-81.087, 34.017], [-81.086, 34.017], [-81.086, 34.018], [-81.087, 34.018], [-81.088, 34.018]], [[-80.95, 33.977], [-80.95, 33.976], [-80.948, 33.976], [-80.948, 33.977], [-80.95, 33.977]], [[-81.15, 34.076], [-81.149, 34.074], [-81.148, 34.075], [-81.148, 34.076], [-81.149, 34.076], [-81.15, 34.076], [-81.151, 34.076], [-81.15, 34.076]], [[-81.074, 34.024], [-81.074, 34.023], [-81.073, 34.023], [-81.073, 34.024], [-81.074, 34.024], [-81.073, 34.024], [-81.072, 34.024], [-81.072, 34.025], [-81.072, 34.024], [-81.072, 34.025], [-81.073, 34.025], [-81.073, 34.024], [-81.074, 34.025], [-81.074, 34.024]], [[-81.142, 34.087], [-81.141, 34.086], [-81.14, 34.086], [-81.14, 34.087], [-81.141, 34.087], [-81.142, 34.087]], [[-80.872, 34.09], [-80.872, 34.089], [-80.87, 34.09], [-80.87, 34.091], [-80.872, 34.09]], [[-81.158, 34.074], [-81.157, 34.073], [-81.157, 34.074], [-81.156, 34.074], [-81.156, 34.075], [-81.157, 34.074], [-81.158, 34.074]], [[-80.904, 33.977], [-80.904, 33.976], [-80.903, 33.976], [-80.903, 33.977], [-80.903, 33.978], [-80.904, 33.978], [-80.904, 33.977]], [[-81.089, 34.019], [-81.088, 34.019], [-81.087, 34.019], [-81.087, 34.02], [-81.088, 34.02], [-81.088, 34.019], [-81.089, 34.019]], [[-80.983, 33.948], [-80.982, 33.948], [-80.982, 33.949], [-80.983, 33.949], [-80.983, 33.948]], [[-81.012, 34.03], [-81.011, 34.03], [-81.01, 34.03], [-81.01, 34.031], [-81.011, 34.031], [-81.012, 34.031], [-81.012, 34.03]], [[-80.964, 33.956], [-80.963, 33.956], [-80.962, 33.956], [-80.963, 33.956], [-80.964, 33.957], [-80.964, 33.956]], [[-81.157, 34.073], [-81.156, 34.073], [-81.156, 34.072], [-81.155, 34.072], [-81.156, 34.073], [-81.157, 34.073]], [[-80.951, 33.965], [-80.952, 33.964], [-80.951, 33.964], [-80.95, 33.964], [-80.95, 33.965], [-80.951, 33.965]], [[-80.981, 33.989], [-80.98, 33.989], [-80.98, 33.99], [-80.981, 33.99], [-80.981, 33.989]], [[-80.96, 34.012], [-80.96, 34.011], [-80.959, 34.011], [-80.959, 34.012], [-80.96, 34.012]], [[-80.816, 34.121], [-80.816, 34.12], [-80.816, 34.121], [-80.815, 34.121], [-80.815, 34.122], [-80.816, 34.121]], [[-81.079, 34.029], [-81.078, 34.029], [-81.078, 34.028], [-81.077, 34.029], [-81.078, 34.029], [-81.079, 34.029]], [[-80.975, 33.986], [-80.974, 33.986], [-80.973, 33.986], [-80.973, 33.987], [-80.974, 33.987], [-80.975, 33.987], [-80.975, 33.986]], [[-80.834, 34.114], [-80.833, 34.114], [-80.832, 34.114], [-80.831, 34.114], [-80.832, 34.114], [-80.833, 34.114], [-80.834, 34.114]], [[-81.076, 34.016], [-81.075, 34.016], [-81.074, 34.016], [-81.075, 34.016], [-81.076, 34.017], [-81.076, 34.016]], [[-81.024, 33.973], [-81.024, 33.972], [-81.023, 33.972], [-81.024, 33.972], [-81.023, 33.972], [-81.023, 33.973], [-81.023, 33.974], [-81.024, 33.973]], [[-81.026, 34.029], [-81.025, 34.03], [-81.025, 34.031], [-81.026, 34.03], [-81.026, 34.029]], [[-80.981, 33.974], [-80.98, 33.974], [-80.98, 33.975], [-80.981, 33.975], [-80.981, 33.974]], [[-81.078, 34.015], [-81.077, 34.015], [-81.077, 34.016], [-81.078, 34.016], [-81.078, 34.015]], [[-80.833, 34.118], [-80.832, 34.118], [-80.832, 34.119], [-80.833, 34.119], [-80.833, 34.118]], [[-80.952, 33.962], [-80.951, 33.962], [-80.951, 33.963], [-80.952, 33.963], [-80.952, 33.962]], [[-80.975, 33.989], [-80.974, 33.989], [-80.974, 33.99], [-80.975, 33.99], [-80.975, 33.989]], [[-80.94, 33.973], [-80.94, 33.974], [-80.941, 33.974], [-80.941, 33.973], [-80.94, 33.973]], [[-81.167, 34.067], [-81.166, 34.067], [-81.166, 34.068], [-81.167, 34.067]], [[-80.819, 34.129], [-80.818, 34.129], [-80.818, 34.13], [-80.819, 34.13], [-80.819, 34.129]], [[-81.16, 34.096], [-81.16, 34.095], [-81.159, 34.096], [-81.16, 34.096]], [[-80.951, 33.977], [-80.95, 33.977], [-80.95, 33.978], [-80.951, 33.977]], [[-80.956, 33.98], [-80.956, 33.979], [-80.955, 33.979], [-80.955, 33.98], [-80.956, 33.98]], [[-81.058, 34.044], [-81.058, 34.043], [-81.057, 34.043], [-81.057, 34.044], [-81.058, 34.044]], [[-81.059, 34.025], [-81.058, 34.025], [-81.058, 34.026], [-81.059, 34.025]], [[-81.159, 34.073], [-81.158, 34.073], [-81.158, 34.074], [-81.159, 34.073]], [[-80.972, 33.987], [-80.972, 33.986], [-80.971, 33.987], [-80.972, 33.987]], [[-81.155, 34.075], [-81.155, 34.074], [-81.154, 34.074], [-81.155, 34.075]], [[-81.08, 34.03], [-81.08, 34.029], [-81.079, 34.029], [-81.08, 34.03]], [[-81.016885, 34.044016], [-81.016868, 34.043995], [-81.016817, 34.043932], [-81.0168, 34.043912], [-81.016772, 34.043877], [-81.016689, 34.043775], [-81.016673, 34.043755], [-81.016662, 34.043741], [-81.016599, 34.043664], [-81.016533, 34.0437], [-81.016211, 34.043865], [-81.015919, 34.04401], [-81.016031, 34.044146], [-81.016134, 34.044272], [-81.016243, 34.044399], [-81.016633, 34.044166], [-81.016885, 34.044016]], [[-80.830989, 34.117888], [-80.830988, 34.117879], [-80.830982, 34.117822], [-80.830943, 34.117772], [-80.83081, 34.117723], [-80.830536, 34.11778], [-80.830497, 34.117789], [-80.830394, 34.117811], [-80.830359, 34.117811], [-80.830353, 34.117882], [-80.830306, 34.1179], [-80.830107, 34.117855], [-80.829973, 34.118228], [-80.829997, 34.118234], [-80.830021, 34.118244], [-80.830049, 34.118262], [-80.830069, 34.11828], [-80.830387, 34.118181], [-80.830427, 34.118184], [-80.830722, 34.118094], [-80.830907, 34.118039], [-80.830916, 34.118031], [-80.830989, 34.117888]], [[-81.009639, 34.029661], [-81.009546, 34.02963], [-81.009439, 34.029594], [-81.009383, 34.029576], [-81.009265, 34.029537], [-81.009075, 34.029903], [-81.009117, 34.029944], [-81.00953, 34.03034], [-81.00974, 34.029875], [-81.009812, 34.029718], [-81.009639, 34.029661]], [[-80.965423, 33.98101], [-80.96504, 33.980807], [-80.96486, 33.980712], [-80.964805, 33.980772], [-80.964641, 33.980955], [-80.964587, 33.981016], [-80.964681, 33.981074], [-80.964963, 33.98125], [-80.965058, 33.981309], [-80.965189, 33.981408], [-80.965223, 33.981368], [-80.96549, 33.981051], [-80.965423, 33.98101]], [[-80.8184, 34.113218], [-80.818389, 34.113206], [-80.81838, 34.113191], [-80.818372, 34.113173], [-80.818368, 34.113154], [-80.818367, 34.113138], [-80.818371, 34.113118], [-80.81777, 34.113052], [-80.817649, 34.113036], [-80.817563, 34.113353], [-80.817541, 34.113432], [-80.817535, 34.113468], [-80.817723, 34.113538], [-80.817856, 34.11359], [-80.818412, 34.113225], [-80.8184, 34.113218]], [[-81.016, 34.034], [-81.015, 34.034], [-81.015, 34.035], [-81.016, 34.035], [-81.016, 34.034]], [[-81.016, 34.042], [-81.015, 34.042], [-81.016, 34.043], [-81.016, 34.042]], [[-81.03, 34.025], [-81.029, 34.024], [-81.029, 34.025], [-81.03, 34.025]], [[-81.028519, 34.064386], [-81.028515, 34.064326], [-81.028524, 34.064048], [-81.028538, 34.063926], [-81.028291, 34.063926], [-81.028046, 34.063918], [-81.027723, 34.064364], [-81.028172, 34.064377], [-81.028316, 34.06438], [-81.028519, 34.064386]], [[-80.892398, 33.984457], [-80.892369, 33.984342], [-80.892315, 33.98415], [-80.891574, 33.984302], [-80.891584, 33.984334], [-80.891612, 33.984433], [-80.891627, 33.984484], [-80.891663, 33.984612], [-80.892398, 33.984457]], [[-81.095073, 34.021445], [-81.094575, 34.021751], [-81.09461, 34.021792], [-81.094638, 34.021832], [-81.094657, 34.021862], [-81.09468, 34.021905], [-81.094695, 34.02194], [-81.094705, 34.021965], [-81.09472, 34.022015], [-81.094731, 34.02207], [-81.094735, 34.022106], [-81.095329, 34.021737], [-81.095073, 34.021445]], [[-80.969464, 33.96971], [-80.9693, 33.969708], [-80.968808, 33.969697], [-80.968693, 33.969694], [-80.968694, 33.969728], [-80.968695, 33.969774], [-80.968696, 33.969816], [-80.968699, 33.970002], [-80.969464, 33.970012], [-80.969464, 33.96971]], [[-81.02, 33.984], [-81.019, 33.984], [-81.019, 33.985], [-81.02, 33.985], [-81.02, 33.984]], [[-81.028, 34.026], [-81.027, 34.026], [-81.027, 34.027], [-81.028, 34.027], [-81.028, 34.026]], [[-80.819865, 34.128243], [-80.819766, 34.128075], [-80.819673, 34.12792], [-80.819266, 34.128082], [-80.819361, 34.128234], [-80.819517, 34.128484], [-80.819594, 34.128452], [-80.819657, 34.128423], [-80.819789, 34.128368], [-80.81991, 34.128317], [-80.819865, 34.128243]], [[-81.05688, 34.044108], [-81.056854, 34.04407], [-81.056824, 34.044025], [-81.056807, 34.044], [-81.056774, 34.043925], [-81.056761, 34.043878], [-81.056748, 34.043827], [-81.056685, 34.043837], [-81.056497, 34.043867], [-81.056435, 34.043878], [-81.056393, 34.043884], [-81.056376, 34.043888], [-81.056339, 34.043897], [-81.056271, 34.043913], [-81.056329, 34.044127], [-81.056393, 34.044338], [-81.056729, 34.04419], [-81.056889, 34.044122], [-81.05688, 34.044108]], [[-80.817, 34.115], [-80.817, 34.114], [-80.816, 34.114], [-80.816, 34.115], [-80.817, 34.115]], [[-80.921706, 33.97204], [-80.921679, 33.971875], [-80.921043, 33.971944], [-80.921074, 33.971978], [-80.921155, 33.972063], [-80.921189, 33.972113], [-80.921223, 33.972154], [-80.921281, 33.972227], [-80.921365, 33.972334], [-80.921636, 33.972206], [-80.921721, 33.972166], [-80.921706, 33.97204]], [[-81.080266, 34.024702], [-81.080026, 34.025155], [-81.080095, 34.025178], [-81.080342, 34.025263], [-81.080486, 34.025002], [-81.080582, 34.024825], [-81.080266, 34.024702]], [[-81.054, 34.025], [-81.054, 34.026], [-81.055, 34.026], [-81.054, 34.025]], [[-81.065384, 34.012832], [-81.065279, 34.012752], [-81.064887, 34.013119], [-81.06486, 34.01315], [-81.064939, 34.013212], [-81.064984, 34.013248], [-81.065061, 34.013309], [-81.065078, 34.013323], [-81.065486, 34.01291], [-81.065384, 34.012832]], [[-80.954052, 33.974779], [-80.95358, 33.974852], [-80.953509, 33.974973], [-80.953358, 33.975228], [-80.953619, 33.975199], [-80.953981, 33.97494], [-80.953996, 33.974927], [-80.954052, 33.974779]], [[-81.043356, 34.050796], [-81.043303, 34.050634], [-81.042711, 34.050649], [-81.042801, 34.05092], [-81.043356, 34.050796]], [[-81.020675, 33.984893], [-81.020547, 33.984792], [-81.020487, 33.984746], [-81.020123, 33.985055], [-81.020185, 33.985103], [-81.020206, 33.98512], [-81.02023, 33.985138], [-81.020302, 33.985194], [-81.020326, 33.985213], [-81.020428, 33.985119], [-81.020675, 33.984893]], [[-80.986, 33.988], [-80.986, 33.987], [-80.985, 33.987], [-80.985, 33.988], [-80.986, 33.988]], [[-81.054069, 34.025187], [-81.053937, 34.025089], [-81.053564, 34.025455], [-81.053513, 34.02551], [-81.053619, 34.025587], [-81.053634, 34.025598], [-81.054069, 34.025187]], [[-81.062638, 34.027846], [-81.062585, 34.027804], [-81.062495, 34.027723], [-81.062436, 34.027661], [-81.06238, 34.027597], [-81.062361, 34.027571], [-81.062317, 34.027513], [-81.062153, 34.027569], [-81.062391, 34.027944], [-81.062514, 34.027898], [-81.062638, 34.027846]], [[-81.023105, 33.973605], [-81.022943, 33.973466], [-81.022618, 33.973714], [-81.022769, 33.973862], [-81.022781, 33.973852], [-81.022883, 33.973773], [-81.023085, 33.97362], [-81.023105, 33.973605]], [[-81.053444, 34.024739], [-81.052983, 34.025128], [-81.053113, 34.025222], [-81.053169, 34.025176], [-81.053545, 34.024812], [-81.053548, 34.024809], [-81.053444, 34.024739]], [[-80.96545, 33.981545], [-80.965165, 33.981731], [-80.964852, 33.981933], [-80.964941, 33.981984], [-80.965006, 33.981943], [-80.965544, 33.981606], [-80.96545, 33.981545]], [[-81.07206, 34.047913], [-81.072041, 34.047864], [-81.071999, 34.047803], [-81.071744, 34.047915], [-81.071859, 34.048077], [-81.07209, 34.047979], [-81.07206, 34.047913]], [[-81.009, 34.029], [-81.008, 34.029], [-81.009, 34.03], [-81.009, 34.029]], [[-81.066151, 34.013285], [-81.065887, 34.013098], [-81.065835, 34.013118], [-81.065822, 34.013161], [-81.066082, 34.013351], [-81.066151, 34.013285]], [[-81.022795, 34.034569], [-81.022755, 34.034569], [-81.022682, 34.034568], [-81.02266, 34.03475], [-81.02276, 34.034619], [-81.022795, 34.034569]]], [[[-80.97, 33.979], [-80.969, 33.979], [-80.969, 33.978], [-80.97, 33.978], [-80.97, 33.979]]]] } }] };
                geojson.features[0].geometry = JSON.parse(data);
                allow = true;
                LoadMapUpdate();
            },
            error: function () {

            }
        });
    }   
}

var Map;
var bounds;
var markers;
var windowtext;
var geojson;
var map_control_html;
var map_control_onclick;
if (typeof zoom === "undefined") {
    zoom = 7
}
if (typeof center === "undefined") {
    center = {
        lat: 39.5,
        lng: -93.9
    }
}

function initMap() {

    (function () {
        var e = function (f, g) {
            return function () {
                return f.apply(g, arguments)
            }
        },
            d = function (i, g) {
                for (var f in g) {
                    if (c.call(g, f)) {
                        i[f] = g[f]
                    }
                }

                function h() {
                    this.constructor = i
                }
                h.prototype = g.prototype;
                i.prototype = new h();
                i.__super__ = g.prototype;
                return i
            },
            c = {}.hasOwnProperty;
        (function (f) {
            return this.AddressPicker = (function (h) {
                d(g, h);

                function g(i) {
                    if (i == null) {
                        i = {}
                    }
                    this.options = f.extend({
                        local: [],
                        datumTokenizer: function (j) {
                            return Bloodhound.tokenizers.whitespace(j.num)
                        },
                        queryTokenizer: Bloodhound.tokenizers.whitespace,
                        autocompleteService: new google.maps.places.AutocompleteService(),
                        autocompleteServiceOptions: {
                            types: ["geocode"],
                            componentRestrictions: {
                                country: "us"
                            }
                        },
                        remote: "fakeRemote"
                    }, i);
                    g.__super__.constructor.call(this, this.options);
                    this.placeService = new google.maps.places.PlacesService(document.createElement("div"))
                }
                g.prototype.search = function (k, j, i) {
                    if (/^\d{1,5}$/.test(k)) {
                        return
                    }

                    this.options.autocompleteServiceOptions.input = k;
                    return this.options.autocompleteService.getPlacePredictions(this.options.autocompleteServiceOptions, (function (l) {
                        return function (m) {
                            f.each(m, function (n, o) {

                                m[n].description = o.description.replace(", United States", "")

                            });
                            return i(m)
                        }
                    })(this))
                };
                return g
            })(Bloodhound)
        })(jQuery)
    }).call(window);

    var b = $("#q");

    if (b.length) {
        b.typeahead(null, {
            displayKey: "description",
            limit: 20,
            source: new AddressPicker()
        }).bind("typeahead:select", function () {
            //on change
            allow = false;
            cleanValidation();
            $('.map_image').css('width', '58.3333%');
            GetBounds();
        })
    }
    if (pageload == 0) {
        LoadMapUpdate();
        pageload++;
    }
};
function LoadMapUpdate() {

    Map = AffMap($("#map_canvas"), {
        gmapSettings: {
            zoom: zoom,
            minZoom: 6,
            maxZoom: 14,
            center: center
        }
    });
    // Map.addOverlay("/Images/tiles/{x}-{y}-{z}.png");

    Map.addOverlay("https://www.unitedstateszipcodes.org/tiles/all/{x}-{y}-{z}.png");
    if (typeof geojson !== "undefined") {
        Map.addGeoJSON(geojson)
    }
    if (typeof bounds !== "undefined") {
        Map.gmap.fitBounds(bounds)
    }
    if (typeof map_control_html !== "undefined") {
        Map.addControl(map_control_html, map_control_onclick)
    }
    if (typeof markers !== "undefined" && markers.length > 0) {
        for (var a = 0; a < markers.length; a++) {
            Map.codeAddress(markers[a], function (d) {
                var c = new google.maps.Marker({
                    map: Map.gmap,
                    position: d[0].geometry.location
                });
                Map.gmap.setCenter(d[0].geometry.location);
                if (windowtext != null) {
                    var e = new google.maps.InfoWindow({
                        content: windowtext
                    });
                    e.open(Map.gmap, c)
                }
            })
        }
    }
}

//Chart Map Functionality//
