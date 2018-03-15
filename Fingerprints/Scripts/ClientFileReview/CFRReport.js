$(document).ready(function () {

    GetCFRReport(0);

    GetCenters();

    

    function GetCenters() {
        $.ajax({

            url: '/AgencyUser/GetCenters',
            type: 'post',
            datatype: 'json',
            success: function (data) {
                console.log(data);
                $('.cfr-center-select').html('');
                var bindOption = '<option value=0>Center</option>';

                if (data !== null && data.length > 0) {
                    $.each(data, function (i, element) {
                        bindOption += '<option value=' + element.CenterId + '>' + element.CenterName + '</option>';
                    });
                    $('.cfr-center-select').html(bindOption);
                }
            }
        });
    }


    function GetCFRReport(centerId) {
        $.ajax({

            url: '/AgencyUser/GetCFRReport',
            type: 'post',
            datatype: 'json',
            data: { centerID: centerId },
            success: function (data) {
                var tableToappend = $('.appendTable');
                var bindTable = '';
                var attMonth = '';
                var presenter = '';
                var contributor = '';
                var absent = '';
                var useridcount = 0;
                $('.cfr-table-bind').html('');

                if (data.anaList !== null && data.anaList.length > 0) {
                    $.each(data.anaList, function (i, element1) {
                        if (element1.length > 0) {
                            tableToappend.find('.cfr-presenter').html('N/A');
                            tableToappend.find('.cfr-contributor').html('N/A');
                            tableToappend.find('.cfr-absent').html('N/A');
                            $.each(element1, function (j, element2) {
                                
                                if (element2.TotalFamilies > 0) {

                                    attMonth = element2.AttendanceMonth;
                                    tableToappend.find('.cfr-staffname').html(element2.FSWUserName);
                                    tableToappend.find('.cfr-totfamilies').html(element2.TotalFamilies);
                                    if (attMonth !== 'N/A') {
                                        tableToappend.find('.cfr-presenter-' + attMonth).html(element2.PresentCount);
                                        tableToappend.find('.cfr-contributor-' + attMonth).html(element2.ContributorCount);
                                        tableToappend.find('.cfr-absent-' + attMonth).html(element2.AbsentMembersCount);
                                    }


                                }
                            });
                            bindTable += tableToappend.find('tbody').html();
                        }

                    });
                    $('.cfr-table-bind').html('');
                    if (bindTable !== '') {
                        $('.cfr-table-bind').html(bindTable);

                    }

                    else {
                        var noData1 = '<td colspan="15" style="padding:40px;text-align: center;font-size: 17px;">No data found</td>';


                        $('.cfr-table-bind').html(noData1);
                    }
                }


                else {
                    $('.cfr-table-bind').html('');
                    var noData = '<td colspan="15" style="padding:40px;text-align: center;font-size: 17px;">No data found</td>';


                    $('.cfr-table-bind').html(noData);
                }
            
            }

        });
    };

    $('.cfr-center-select').on('change', function () {
        var centerId = parseInt($(this).val());
        GetCFRReport(centerId);
    });

});