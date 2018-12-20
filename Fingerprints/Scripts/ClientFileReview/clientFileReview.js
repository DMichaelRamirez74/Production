
$(document).ready(function () {
    var isHost = false;
    var isEmpty = false;
    var isClose = false;
    var isOpen = false;
    var HostUserName = '';
    var HostUserColor = '';
    var HostUserId = '';
    var HostUserRole = '';
    var IsHost = false;
    var IsValidMonth = false;
    var CurrentMemberName = '';
    var currentMemberColor = '';


    $(document).ajaxStart(function () {

        $('#spinner').show();

    });

    $(document).ajaxStop(function () {

        $('#spinner').hide();

    });


    $(document).on("click", "#goto_timeline", function (e) {

        var _cid = $(this).data("clientid");
        if (_cid) {
            $('#spinner').show();
            window.location.href = window.timelineUrl.concat(_cid);
        }
    });


    $(window).bind("load resize", function () {

        topOffset = 50;
        width = (this.window.innerWidth > 0) ? this.window.innerWidth : this.screen.width;
        if (width < 768) {
            topOffset = 100; // 2-row-menu
        }

        height = ((this.window.innerHeight > 0) ? this.window.innerHeight : this.screen.height) - 1;
       
        height = height - topOffset;
        if (height < 1) height = 1;
        if (height > topOffset) {
            var fileHeight = height - 50;
          
            $('.bg-min-height').css('min-height', (fileHeight) + 'px');
        }
    });

    $('.nano-open').nanoScroller();
    $('.nano-close').nanoScroller();

    $('#reportmodal').on('shown.bs.modal', function (e) {
        $(".nano").nanoScroller();
    });

    $('.month-block-al').removeClass('month-active');

    var currentMonth = parseInt(new Date().getMonth()) + 1;

    $('.month-' + currentMonth).addClass('month-active');

    var checkMonth = $('.month-active').children('div').children('h2').html().toUpperCase();

    //Get the class and centers from the db//
    $.ajax({
        url: "/AgencyUser/GetClassess",
        datatype: 'json',
        type: 'post',
        data: { CurrentMonth: checkMonth },
        success: function (data) {

            //if (data.centers.length > 0) {
            //    var center = '<option value="0">Select Center</option>';

            //    $.each(data.centers, function (i, element1) {
            //        center += '<option value=' + element1.CenterId + '>' + element1.CenterName + '</option>';

            //    });
            //    var classsection = '<option value="0">Select Classroom</option>';
            //    $('.centerSelect').html(center);
            //    $('.classroomSelect').html(classsection);

            //}
            if (data.clientsList.length > 0) {
                var center = '';
                var classroom = '';
                var clientsSection = $('.clients-section');
                var appendSection = '';
                $('.content-inside').html('');
                $.each(data.clientsList, function (j, element2) {
                    clientsSection.children('div').find('.client-name').attr('client-id', element2.ClientId);
                    clientsSection.children('.clients-div').addClass('client_' + element2.ClientId);

                    clientsSection.children('div').find('.client-name').html(element2.ClientName);
                    clientsSection.children('div').find('.client-name').attr('title', element2.ClientName);
                    if (data.clientStatus !== null && data.clientStatus.length > 0) {

                        $.each(data.clientStatus, function (k, element3) {
                            if (element3.ClientId === element2.ClientId) {
                                clientsSection.children('.client_' + element2.ClientId).find('.' + element3.AttendanceMonth + '-status').parent('.Status-title').removeClass('Status-title-Open Status-title-Empty Status-title-Closed');
                                clientsSection.children('.client_' + element2.ClientId).find('.' + element3.AttendanceMonth + '-status').html(element3.Status).parent('.Status-title').addClass('Status-title-' + element3.Status);
                            }
                        });
                    }

                    appendSection += clientsSection.html();
                    clientsSection.children('.client_' + element2.ClientId).find('.Status-title').removeClass('Status-title-Open Status-title-Empty Status-title-Closed');
                    clientsSection.children('.client_' + element2.ClientId).find('.Status-title').addClass('Status-title-Empty');
                    clientsSection.children('.client_' + element2.ClientId).find('.Status-title').children('p').html('Empty');
                    clientsSection.children('.clients-div').removeClass('client_' + element2.ClientId);
                });

                $('.content-inside').html(appendSection);
                $('.nano').nanoScroller();
            }

            else {
                $('.content-inside').html('');

            }
        }

    });

    $('.centerSelect').on('change', function () {


        var centerId = parseInt($(this).val());
        if (centerId === 0) {
            var class_option = '<option value="0">Select Classroom</option>';
            $('.classroomSelect').html(class_option);
            return false;
        }
        $.ajax({
            url: "/AgencyUser/GetClassByCenterID",
            datatype: 'json',
            type: 'post',
            data: { CenterID: centerId },
            success: function (data) {
                if (data.length > 0) {
                    var classroom = '<option value="0">--Select Classroom--</option>';

                    $.each(data, function (i, element1) {
                        classroom += '<option value=' + element1.ClassRoomId + '>' + element1.ClassRoomName + '</option>';
                    });

                    $('.classroomSelect').html(classroom);
                    if (data.length === 1) {
                        $('.classroomSelect').val(data[0].ClassRoomId);
                    }
                }
            },
            error: function (data) {

            }
        });
    });

    // On click of Search button//
    $('#searchClient').on('click', function () {
        var CenterId = parseInt($('.centerSelect').val());
        var ClassRoomId = parseInt($('.classroomSelect').val());
        cleanValidation();
        if ((CenterId === 0) && (ClassRoomId !== 0)) {
            customAlert('Please Select Center Name');
            plainValidation('.centerSelect');
            return false;
        }
        if ((ClassRoomId === 0) && (CenterId !== 0)) {
            customAlert('Please Select Classroom Name');
            plainValidation('.classroomSelect');
            return false;
        }

        $.ajax({
            url: '/AgencyUser/GetClients',
            datatype: 'json',
            type: 'post',
            data: { CenterId: CenterId, ClassRoomId: ClassRoomId },
            success: function (data) {
                if (data.clientDetails !== null && data.clientDetails.length > 0) {
                    var center = '';
                    var classroom = '';
                    var clientsSection = $('.clients-section');
                    var appendSection = '';
                    $('.content-inside').html('');
                    $.each(data.clientDetails, function (j, element3) {
                        clientsSection.children('div').find('.client-name').attr('client-id', element3.ClientId);
                        clientsSection.children('.clients-div').addClass('client_' + element3.ClientId);

                        clientsSection.children('div').find('.client-name').html(element3.ClientName);

                        if (data.clientStatus !== null && data.clientStatus.length > 0) {

                            $.each(data.clientStatus, function (k, element4) {

                                if (element4.ClientId === element3.ClientId) {
                                    clientsSection.children('.client_' + element3.ClientId).find('.' + element4.AttendanceMonth + '-status').parent('.Status-title').removeClass('Status-title-Open Status-title-Empty Status-title-Closed');
                                    clientsSection.children('.client_' + element3.ClientId).find('.' + element4.AttendanceMonth + '-status').html(element4.Status).parent('.Status-title').addClass('Status-title-' + element4.Status);
                                }
                            });
                        }


                        appendSection += clientsSection.html();
                        clientsSection.children('.client_' + element3.ClientId).find('.Status-title').removeClass('Status-title-Open Status-title-Empty Status-title-Closed');
                        clientsSection.children('.client_' + element3.ClientId).find('.Status-title').addClass('Status-title-Empty');
                        clientsSection.children('.client_' + element3.ClientId).find('.Status-title').children('p').html('Empty');
                        clientsSection.children('.clients-div').removeClass('client_' + element3.ClientId);
                    });
                    $('.month-block-al').removeClass('month-active');
                    $('.month-' + currentMonth).addClass('month-active');
                    $('.content-inside').html(appendSection);
                }

                else {
                    $('.content-inside').html('<div style="padding:40px;margin:auto 0;font-size: large;text-align: center;">No data found</div>');
                }
            }
        });


    });

    //On click of  Status of the Month of the client//
    $('.content-inside').on('click', '.Status-title', function () {

        var item = $('.members-block');

        var statusMonth = parseInt($(this).children('p').attr('month'));
        var currentMonth1 = parseInt(new Date().getMonth()) + 1;
       
        isEmpty = $(this).hasClass('Status-title-Empty');
        isClose = $(this).hasClass('Status-title-Closed');
        isOpen = $(this).hasClass('Status-title-Open');
        IsValidMonth = (statusMonth === currentMonth1);
        if (statusMonth > currentMonth1) {
            customAlert('You are allowed to access only the current and previous month data.');
            return false;
        }

        if ((statusMonth === currentMonth1) && (isClose === true)) {
            item.children('div').find('.meeting-content-desc').removeClass('meeting-content-desc');
            item.children('div').find('.start-meeting').addClass('meeting-content-desc-start');
            item.children('div').find('.end-meeting').removeClass('meeting-content-desc-start');
            item.children('div').find('.end-meeting').addClass('meeting-content-desc-start');


        }
        else if ((statusMonth === currentMonth1) && (isOpen === true)) {
            item.children('div').find('.meeting-content-desc').removeClass('meeting-content-desc');
            item.children('div').find('.start-meeting').addClass('meeting-content-desc-start');
            item.children('div').find('.end-meeting').removeClass('meeting-content-desc-start');
            item.children('div').find('.end-meeting').addClass('meeting-content-desc');


        }
        else if ((statusMonth === currentMonth1) && (isEmpty === true)) {
            item.children('div').find('.start-meeting').removeClass('meeting-content-desc-start');
            item.children('div').find('.end-meeting').removeClass('meeting-content-desc-start');
            item.children('div').find('.end-meeting').addClass('meeting-content-desc');
            item.children('div').find('.start-meeting').addClass('meeting-content-desc');



        }
        else {
            item.children('div').find('.start-meeting').removeClass('meeting-content-desc');
            item.children('div').find('.end-meeting').removeClass('meeting-content-desc');
            item.children('div').find('.start-meeting').addClass('meeting-content-desc-start');
            item.children('div').find('.end-meeting').addClass('meeting-content-desc-start');


        }

        $('.month-block-al').removeClass('month-active');
        $('.month-' + statusMonth).addClass('month-active');
        $('.client-content-desc').removeClass('client-active');
        $(this).parents('.clients-div').children('div').find('.client-content-desc').addClass('client-active');
        var clientId = parseInt($('.client-active').children('p').attr('client-id'));
        var reviewMonth = $('.month-active').children('div').children('h2').html().toUpperCase();


        $.ajax({
            url: '/AgencyUser/CheckIsHost',
            datatype: 'json',
            type: 'post',
            data: { currentMonth: reviewMonth, ClientId: clientId },
            success: function (data) {

                HostUserName = data.HostInfo.FullName;
                HostUserId = data.HostInfo.UserId;
                HostUserColor = data.HostInfo.UserColor;
                isHost = data.HostInfo.IsHost;
                getMembers(reviewMonth, clientId);
                if (isHost) {

                    GetClientProfile(clientId, statusMonth);
                    //if(!isEmpty)
                    //{
                    bindOpenCloseItems(reviewMonth, clientId);
                    //}
                }
                else if ((!isHost) && (isEmpty)) {
                    GetClientProfile(clientId, statusMonth);
                    bindOpenCloseItems(reviewMonth, clientId);
                    $('.add-items').addClass('hidden');
                    $('.open-items').addClass('hidden');
                    $('.closed-items').addClass('hidden');
                }
                else if ((!isHost)) {
                    $('.members-block').addClass('hidden');
                    GetClientProfile(clientId, statusMonth);
                    bindOpenCloseItems(reviewMonth, clientId);
                }

                if (data.member !== null) {
                    CurrentMemberName = data.member.FullName;
                    currentMemberColor = data.member.UserColor;

                }
            }
        });



    });

    //On Click of Start Meeting button//
    $('.start-meeting').on('click', function () {

        if ((!isEmpty) || (!IsValidMonth)) {
            return false;
        }
        StartMeeting();
    });

    //On Click of End Meeting Button//
    $('.end-meeting').on("click", function () {
        if (!isOpen) {
            return false;
        }
        EndMeeting();
    });

    //On Click of Add Items Button//
    $('#addItemsbtn').click(function () {
        $('.review-head').html('Add Review');
        $('#reviewtext1').val('');
        $('#reviewtext1').prop('disabled', false);
        $('.close-note').addClass('hidden');
        $('#updateReview').addClass('hidden');
        $('#closeReview').addClass('hidden');
        $('#addReview').removeClass('hidden');
    });

    //On Click of Edit Review icon in the Open/Closed Items List//
    $('body').on('click', '.edit-review', function () {

        var isedit = $(this).attr('isEdit');
        var notes_id = parseInt($(this).attr('notes-id'));

        var IsClosed = $(this).attr('isClosed');
        var notesOpen = $(this).parents('tr').find('.review-notes').first().contents().eq(0).text();
        var notesClose = $(this).parents('tr').find('.review-notes').first().contents().eq(2).text();
        if (isedit === 'false') {
            customAlert('Your are not having access to edit this note.');
            return false;
        }
        if (IsClosed === 'false') {
            $('#updateReview').attr('notes-id', notes_id);
            $('#closeReview').attr('notes-id', notes_id);
            $('#reviewtext1').val(notesOpen);
            $('#reviewtext2').val(notesClose);
            $('#reviewtext1').prop('disabled', false);
            $('#reviewtext2').prop('disabled', false);
            $('.review-head').html('Update Note');
            $('.close-note').removeClass('hidden');
            $('#updateReview').removeClass('hidden');
            $('#closeReview').removeClass('hidden');
            $('#addReview').addClass('hidden');
        }
        else if (IsClosed === 'true') {
            $('#reviewtext1').val(notesOpen);
            $('#reviewtext2').val(notesClose);
            $('.review-head').html('Review Notes');
            $('.close-note').removeClass('hidden');
            $('#updateReview').addClass('hidden');
            $('#closeReview').addClass('hidden');
            $('#addReview').addClass('hidden');
            $('#reviewtext1').prop('disabled', true);
            $('#reviewtext2').prop('disabled', true);
        }
        $('#additemsModal').modal('show');
    });

    //Function for Load the Development Memebers in view Page//
    function getMembers(reviewmonth, clientID) {
        $.ajax({
            url: '/AgencyUser/GetMembers',
            datatype: 'json',
            type: 'post',
            data: { ReviewMonth: reviewmonth, ClientId: clientID },
            success: function (data) {
                if (data.membersList !== null && data.membersList.length > 0) {
                    var membersDiv = $('.membrslist');
                    var appdendDiv = '';
                    $('.append-members').html('');
                    var presentCount = 0;
                    var contributor = 0;
                    var membersCount = data.membersList.length;
                    var report_members = '';
                    $.each(data.membersList, function (i, element) {
                        membersDiv.children('div').find('.member-content-desc > p').html(element.FullName);
                        membersDiv.children('div').find('.member-survey-3d').attr('user-id', element.UserId);
                        membersDiv.children('div').find('.member-survey-3d').attr('role-id', element.RoleId);
                        membersDiv.children('div').find('.bar-color-common').css('background-color', element.UserColor);
                        if (element.IsPresent) {
                            if (isEmpty && !IsValidMonth) {
                                membersDiv.children('div').find('.member-survey-3d').attr('isPresent', false);
                                membersDiv.children('div').find('.member-survey-3d').attr('isEdit', false);
                                membersDiv.children('div').find('.member-survey-3d').children('.tick-3d').addClass('hidden');
                            }
                            else {
                                membersDiv.children('div').find('.member-survey-3d').attr('isPresent', element.IsPresent);
                                membersDiv.children('div').find('.member-survey-3d').attr('isEdit', element.IsEdit);
                                membersDiv.children('div').find('.member-survey-3d').children('.tick-3d').removeClass('hidden');
                                presentCount += 1;
                            }

                            report_members += '<li class="present">' + element.FullName + '</li>';
                        }
                        else {
                            membersDiv.children('div').find('.member-survey-3d').attr('isPresent', false);
                            membersDiv.children('div').find('.member-survey-3d').attr('isEdit', element.IsEdit);
                            membersDiv.children('div').find('.member-survey-3d').children('.tick-3d').addClass('hidden');
                            report_members += (element.IsContributor) ? '<li class="contributor">' + element.FullName + '</li>' : '<li class="absent">' + element.FullName + '</li>';

                        }
                        appdendDiv += membersDiv.html();
                    });

                    $('.append-members').html(appdendDiv);
                    $('.append-members').find('.member-survey-3d').addClass('check-attendance');
                    $('.members-count').html(membersCount);

                    contributor = (data.contributorCount === null) ? 0 : data.contributorCount;
                    $('.member-contributor').html(contributor);
                    $('.members-present').html(presentCount);

                    $('.report-modal-clientname').html($('.client-active').children('p').html());
                    $('.report-modal-month').html($('.month-active').children('div').children('h2').html().toUpperCase());
                    $('.report-modal-hostname').html(HostUserName);
                    $('.report-modal-devmembers').html(report_members);


                    $('.report-modal-presentcount').html(presentCount);
                    $('.dev-mem-count').html('(' + membersCount + ')');
                    $('.report-modal-contributorcount').html(contributor);
                    $('.report-modal-status').html('Closed');

                    if (isHost) {
                        $('.members-block').removeClass('hidden');
                        $('html, body').animate({
                            scrollTop: $(".members-block").offset().top
                        }, 1000);
                        $('.nano1').nanoScroller();
                        $('.nano2').nanoScroller();
                    }


                }


            }
        });
    }

    //Bind the Open and Closed items in the View Page//
    function bindOpenCloseItems(ReviewMonth, ClientId) {

        $.ajax({

            url: '/AgencyUser/GetReviewNotes',
            datatype: 'json',
            type: 'post',
            data: { MonthReviewed: ReviewMonth, ClientID: ClientId },
            success: function (data) {

                var openTable = $('.open-table');
                var bindOpenTable = '';
                var closeTable = $('.closed-table');
                var bindCloseTable = '';
                var closecount = 0;
                var opencount = 0;
                var opentable2 = '';

                var reportPopUpdiv = $('.report-items-popup');
                var bindOpenReport = '';
                var bindCloseReport = '';

                var img_editicon = "/images/edit_icon.png";
                var img_eye = "/images/eye.png";



                if (data.OpenNotesList !== null && data.OpenNotesList.length > 0) {


                    $.each(data.OpenNotesList, function (i, element) {
                        //   debugger;
                        openTable.find('.review-date').html(element.ReviewDate);
                        var Notes1 = (element.CloseNotes !== "") ? element.OpenNotes + '<span class="comma-span">,</span>' + element.CloseNotes : element.OpenNotes + '<span class="comma-span"></span>' + element.CloseNotes;
                        openTable.find('.review-notes').html(Notes1);
                        openTable.find('.review-staff-name').html(element.ReviewedStaffName);
                        openTable.find('.review-status').html('Open');
                        openTable.find('.bar-color-common').css('background-color', element.StaffUniqueColor);
                        openTable.find('.edit-review').attr('isEdit', element.IsEdit);
                        openTable.find('.edit-review').attr('isClosed', isClose);
                        openTable.find('.edit-review').attr('notes-id', element.NotesId);
                        opencount += 1;
                        if (isClose || (!IsValidMonth)) {

                            openTable.find('.image-icon').attr('src', img_eye);
                            $(".open-edit-note").html("view");
                        }
                        else {
                            openTable.find('.image-icon').attr('src', img_editicon);
                            $(".open-edit-note").html("Edit");
                        }
                        bindOpenTable += openTable.find('tbody').html();



                        reportPopUpdiv.find('.rep-pop-reviewdate').html(element.ReviewDate);
                        var openReportNotes = (element.CloseNotes !== "") ? element.OpenNotes + ' , ' + element.CloseNotes : element.OpenNotes;
                        reportPopUpdiv.find('.rep-pop-notes').html(openReportNotes);
                        reportPopUpdiv.find('.rep-pop-mem-name').html(element.ReviewedStaffName);
                        bindOpenReport += reportPopUpdiv.html();

                    });

                    if (opencount > 0) {
                        $('.no-open-data').addClass('hidden');

                        $('.open-table-body').html(bindOpenTable);
                    }
                    else {
                        $('.open-items').removeClass('hidden');
                        $('.no-open-data').removeClass('hidden');
                    }
                }

                else {
                    $('.open-items').removeClass('hidden');
                    $('.open-table-body').html('<tr class="hidden no-open-data"> <td colspan="6" style="padding:40px;">No data found</td> </tr>');
                    $('.no-open-data').removeClass('hidden');

                }
                if (data.ClosedNotesList !== null && data.ClosedNotesList.length > 0) {
                    $.each(data.ClosedNotesList, function (j, element2) {


                        var Notes2 = '';
                        var closedReportNotes = '';

                        Notes2 = (element2.CloseNotes !== '') ? element2.OpenNotes + '<span class="comma-span">, </span>' + element2.CloseNotes : element2.OpenNotes + '<span class="comma-span"></span>' + element2.CloseNotes;
                        closeTable.find('.review-date').html(element2.ReviewDate);
                        closeTable.find('.review-notes').html(Notes2);
                        closeTable.find('.review-staff-name').html(element2.ReviewedStaffName);
                        closeTable.find('.review-status').html('Closed');
                        closeTable.find('.bar-color-common').css('background-color', element2.StaffUniqueColor);
                        closeTable.find('.edit-review').attr('isEdit', element2.IsEdit);
                        closeTable.find('.edit-review').attr('isClosed', true);
                        closeTable.find('.edit-review').attr('notes-id', element2.NotesId);
                        closecount += 1;
                        bindCloseTable += closeTable.find('tbody').html();

                        reportPopUpdiv.find('.rep-pop-reviewdate').html(element2.ReviewDate);
                        var closeReportNotes = (element2.CloseNotes !== "") ? element2.OpenNotes + ' , ' + element2.CloseNotes : element2.OpenNotes;
                        reportPopUpdiv.find('.rep-pop-notes').html(closeReportNotes);
                        reportPopUpdiv.find('.rep-pop-mem-name').html(element2.ReviewedStaffName);
                        bindCloseReport += reportPopUpdiv.html();
                    });
                    if (closecount > 0) {
                        $('.no-closed-data').addClass('hidden');
                        $('.closed-table-body').html(bindCloseTable);
                    }
                    else {
                        $('.closed-items').removeClass('hidden');
                        $('.no-closed-data').removeClass('hidden');
                    }
                }

                else {
                    $('.closed-items').removeClass('hidden');
                    $('.closed-table-body').html('<tr class="hidden no-closed-data"> <td colspan="6" style="padding:40px;">No data found</td> </tr>');
                    $('.no-closed-data').removeClass('hidden');
                }

                //bind data to report//
                if (isClose) {
                    var no_data_div = '<div class="col-xs-12 no-data-report-modal">No data found</div>';
                    if (opencount > 0) {

                        $('.rep-pop-openitems-div').html(bindOpenReport);

                    }
                    else {
                        $('.rep-pop-openitems-div').html(no_data_div);
                        $('.no-open-report').removeClass('hidden');
                    }
                    if (closecount > 0) {
                        $('.rep-pop-closeditems-div').html(bindCloseReport);
                    }
                    else {
                        $('.rep-pop-closeditems-div').html(no_data_div);
                    }
                    $('#reportmodal').modal('show');
                    var lft_hgt = $('.dev_mem_lft').css('height');
                    lft_hgt = lft_hgt.split('px')[0];
                    var rgt_hgt = $('.dev_mem_rgt').css('height');
                    rgt_hgt = rgt_hgt.split('px')[0];

                    $('.dev_mem_lft').css('height', rgt_hgt + 'px');
                }


            }
        });

        $('.open-items').removeClass('hidden');


        $('.closed-items').removeClass('hidden');

        if (isClose || isEmpty || !IsValidMonth) {
            $('.add-items').addClass('hidden');
        }
        else {
            $('.add-items').removeClass('hidden');
        }

    }

    //On click of the 3d bar of the Members//
    $('body').on('click', '.check-attendance', function () {

        var revMonth = parseInt($('.month-active').attr('month'));
        var isedit = $(this).attr('isEdit');
        var curMonth = parseInt(new Date().getMonth() + 1);
        if ((!isEmpty) || (!IsValidMonth)) {
            return false;
        }
        else if (isedit === 'false') {
            return false;
        }

        var unChecked = $(this).children('.tick-3d').hasClass('hidden');
        var presentElement = $('.members-present');
        var membersPresent = parseInt(presentElement.html());
        var TotalMembers = parseInt($('.members-count').html());

        if (unChecked) {
            $(this).children('.tick-3d').removeClass('hidden');
            membersPresent += 1;
            presentElement.html(membersPresent);
            $(this).attr('isPresent', true);
        }
        else {
            $(this).children('.tick-3d').addClass('hidden');
            membersPresent -= 1;
            presentElement.html(membersPresent);
            $(this).attr('isPresent', false);
        }
    });

    //On Click of Add Review Button//
    $('#addReview').click(function () {
        var review1 = $('#reviewtext1').val().trim();

        var reviewMonth = $('.month-active').children('div').children('h2').html().toUpperCase();
        var clientReviewed = parseInt($('.client-active').children('p').attr('client-id'));
        if (review1 === "") {
            customAlert('Please enter opening note.');
            $('#reviewtext1').focus();
            return false;
        }
        var notes = {
            'OpenNotes': review1,
            'CloseNotes': '',
            'ReviewedStaffName': CurrentMemberName,
            'ReviewMonth': reviewMonth,
            'ReviewStatus': true,
            'ClientId': clientReviewed
        };
        var notesString = JSON.stringify(notes);
        $.ajax({

            url: '/AgencyUser/InsertReviewNotes',
            type: 'post',
            datatype: 'json',
            data: { reviewNotes: notesString },
            success: function (data) {
                if (data) {
                    $('#additemsModal').modal('hide');
                    customAlert('Note added successfully.');
                    bindOpenCloseItems(reviewMonth, clientReviewed);
                }

            }
        });

    });

    //On click of Update Review Button//
    $('#updateReview').click(function () {

        var openNote = $('#reviewtext1').val().trim();
        var closingNote = $('#reviewtext2').val().trim();
        var NotesID = parseInt($(this).attr('notes-id'));
        var clientId = parseInt($('.client-active').children('.client-name').attr('client-id'));
        var activeMonth = $('.month-active').children('div').children('h2').html().toUpperCase();
        var showStatus = 'Note updated successfully';
        if (openNote === '') {
            customAlert('Please enter opening note.');
            $('#reviewtext1').focus();
            return false;
        }
        var reviewNotes = {
            'NotesId': NotesID,
            'OpenNotes': openNote,
            'CloseNotes': closingNote,
            'ReviewStatus': true
        };
        var NotesString = JSON.stringify(reviewNotes);
        $('#additemsModal').modal('hide');
        updateReviewNotes(NotesString, activeMonth, clientId, showStatus);

    });

    //On click of Update Review Button//
    $('#closeReview').click(function () {

        var openNote = $('#reviewtext1').val().trim();
        var closingNote = $('#reviewtext2').val().trim();
        var NotesID = parseInt($(this).attr('notes-id'));
        var clientId = parseInt($('.client-active').children('.client-name').attr('client-id'));
        var activeMonth = $('.month-active').children('div').children('h2').html().toUpperCase();
        var closestatus = 'Note closed successfully';
        if (openNote === '') {
            customAlert('Please enter opening note.');
            return false;
        }
        var reviewNotes = {
            'NotesId': NotesID,
            'OpenNotes': openNote,
            'CloseNotes': closingNote,
            'ReviewStatus': false
        };
        var NotesString2 = JSON.stringify(reviewNotes);
        updateReviewNotes(NotesString2, activeMonth, clientId, closestatus);
    });

    //function For Update Review Notes//
    function updateReviewNotes(NotesModel, monthactive, clientid, status_show) {
        $.ajax({
            datatype: 'json',
            type: 'post',
            url: '/AgencyUser/UpdateReviewNotes',
            data: { ReviewNotes: NotesModel },
            success: function (data) {
                if (data) {
                    $('#additemsModal').modal('hide');
                    customAlert(status_show);
                    bindOpenCloseItems(monthactive, clientid);
                }
                else {

                    customAlert('Some error occured.Please try again later.');
                }
            }
        });
    }

    //function For Start Meeting//
    function StartMeeting() {
        var arr = [];
        var clientId = parseInt($('.client-active').children('p').attr('client-id'));
        var month = $('.month-active').children('div').children('h2').html();
        var status = true;
        $('.check-attendance').each(function () {
            var userid = $(this).attr('user-id');
            var roleid = $(this).attr('role-id');
            var isPresent = $(this).attr('isPresent');

            var DevelopmentMembers = {
                'UserId': userid,
                'RoleId': roleid,
                'isPresent': isPresent,
                'ClientId': clientId,
                'ReviewMonth': month,
                'Status': status
            };
            arr.push(DevelopmentMembers);

        });
        var convArray = JSON.stringify(arr);

        InsertMembersAttendance(convArray, true, month, clientId);


    }

    //function For End Meeting//
    function EndMeeting() {
        var arr1 = [];
        var clientId1 = parseInt($('.client-active').children('p').attr('client-id'));
        var month1 = $('.month-active').children('div').children('h2').html();
        var status1 = false;
        $('.check-attendance').each(function () {
            var userid1 = $(this).attr('user-id');
            var roleid1 = $(this).attr('role-id');
            var isPresent1 = $(this).attr('isPresent');

            var DevelopmentMembers1 = {
                'UserId': userid1,
                'RoleId': roleid1,
                'isPresent': isPresent1,
                'ClientId': clientId1,
                'ReviewMonth': month1,
                'Status': status1
            };
            arr1.push(DevelopmentMembers1);

        });
        var convArray1 = JSON.stringify(arr1);

        InsertMembersAttendance(convArray1, false, month1, clientId1);

        $('.edit-review').attr('isClosed', true);


    }

    //function for Insert attendance for review members//
    function InsertMembersAttendance(convArr, clientreviewstatus, selectMonth, clientIdSelect) {
        $.ajax({
            url: '/AgencyUser/InsertMembersAttendance',
            datatype: 'json',
            type: 'post',
            data: { membersList: convArr },
            success: function (data) {
                if (data) {
                    var showStatus = (clientreviewstatus) ? 'Open' : 'Closed';
                    var monthDiv = '.' + selectMonth.toLowerCase() + '-status';
                    $('.client_' + clientIdSelect).find(monthDiv).html(showStatus);
                    $('.client_' + clientIdSelect).find(monthDiv).parent('.Status-title').removeClass('Status-title-Empty Status-title-Closed Status-title-Open');
                    $('.client_' + clientIdSelect).find(monthDiv).parent('.Status-title').addClass('Status-title-' + showStatus);
                    if (clientreviewstatus) {
                        isEmpty = false;
                        isClose = false;
                        isOpen = true;
                    }
                    else {
                        isEmpty = false;
                        isClose = true;
                        isOpen = false;
                    }

                    $('.members-block').addClass('hidden');
                    $('.client-profile').removeClass('hidden');
                    bindOpenCloseItems(selectMonth, clientIdSelect);
                }
            }
        });
    }

    //function to get the client profile//
    function GetClientProfile(ClientId, monthrev) {

        var todayDate = new Date();
        var monthtoreview = monthrev + "/" + '1' + "/" + todayDate.getFullYear();
        $.ajax({
            url: '/AgencyUser/GetClientProfile',
            datatype: 'json',
            type: 'post',
            data: { ClientId: ClientId, dateReview: monthtoreview },
            success: function (data) {

                if (isEmpty) {
                    $('.profile-content-desc-pic').removeClass('Status-desc-pic-Empty Status-desc-pic-Closed Status-desc-pic-Open');
                    $('.profile-content-desc-pic').addClass('Status-desc-pic-Empty');
                    $('.profile-content-desc-pic').children('p').html('Empty');
                }
                if (isClose) {
                    $('.profile-content-desc-pic').removeClass('Status-desc-pic-Empty Status-desc-pic-Closed Status-desc-pic-Open');

                    $('.profile-content-desc-pic').addClass('Status-desc-pic-Closed');
                    $('.profile-content-desc-pic').children('p').html('Closed');
                }
                if (isOpen) {
                    $('.profile-content-desc-pic').removeClass('Status-desc-pic-Empty Status-desc-pic-Closed Status-desc-pic-Open');

                    $('.profile-content-desc-pic').addClass('Status-desc-pic-Open');
                    $('.profile-content-desc-pic').children('p').html('Open');
                }

                // $("#goto_timeline").prop("href", window.timelineUrl.concat(data[0].ChildId));
                $("#goto_timeline").data("clientid", data[0].ChildId);
                if (data[0].IsPregnantMother) {
                    $('.child-div').addClass('hidden');
                    $('.preg-div').removeClass('hidden');
                    $('.child-dob').html(data[0].DOB + '</br> ' + data[0].ChildAge);
                    $('.start-date').html(data[0].StartDate);
                    $('.trimister').html(data[0].Trimester);
                    $('.child-name').html(data[0].ChildName);
                    $('.total-case-note').html(data[0].TotalCasenote);
                    $('.last-date-caseNote').html(data[0].LastdateofCasenote);
                    $('.doctor').html(data[0].Doctor);
                    var addr = data[0].Address.split('+');
                    var convaddr = '';
                    $.each(addr, function (i, element) {
                        convaddr += element + '</br>';
                    });
                    $('.client-address').css('text-transform', 'captalize');
                    $('.client-address').html(convaddr);
                    var imagesrc = (data[0].Profilepic === "") ? ("/Images/prof-image.png") : ("data:image/jpg;base64," + data[0].Profilepic);
                    $('#ProfileImage').attr('src', imagesrc);
                    $("#ProfileImage").css("display", "block");

                }
                else {
                    $('.child-div').removeClass('hidden');
                    $('.preg-div').addClass('hidden');

                    $('.child-dob').html(data[0].DOB + '</br> ' + data[0].ChildAge);
                    $('.child-bmi').html(data[0].BMI);
                    $('.child-language').html(data[0].Language);
                    $('.child-attendance').html(parseFloat(data[0].Attendance).toFixed(2) + '%');
                    $('.start-date').html(data[0].StartDate);
                    $('.enrolled-days').html(data[0].TotalEnrolled + ' ' + 'days');
                    $('.iep').html(data[0].IEP);
                    $('.behavior-plan').html(data[0].BehaviorPlan);
                    $('.allergies').html(data[0].Allergies);
                    $('.doctor').html(data[0].Doctor);
                    $('.dentist').html(data[0].Dentist);
                    $('.miss-screenings').html(data[0].MissingScreenings);
                    $('.trans-requested').html(data[0].TransferRequested);

                    if (data[0].TransportationProvided === "Yes") {
                        $('.trans-provided').html(data[0].TransportationProvided);
                        $('.trans-prov-div').removeClass('hidden');
                    }
                    else {
                        $('.trans-prov-div').addClass('hidden');
                    }

                    if (data[0].Trimester === null || data[0].Trimester === "") {
                        $('.trimister-div').addClass('hidden');
                        $('.trimister').html(data[0].Trimester);
                    }
                    else {
                        $('.trimister-div').removeClass('hidden');
                        $('.trimister').html(data[0].Trimester);
                    }
                    $('.child-name').html(data[0].ChildName);
                    $('.mother-name').html(data[0].Mother);
                    $('.parent-type').html(data[0].Parent);
                    $('.parent-dob').html(data[0].ParentDOB);
                    $('.is-Employed').html(data[0].Employed);
                    $('.job-training').html(data[0].JobTraining);
                    $('.total-case-note').html(data[0].TotalCasenote);
                    $('.last-date-caseNote').html(data[0].LastdateofCasenote);

                    var addr = data[0].Address.split('+');
                    var convaddr = '';
                    $.each(addr, function (i, element) {
                        convaddr += element + '</br>';
                    });
                    $('.client-address').css('text-transform', 'captalize');
                    $('.client-address').html(convaddr);
                    $('.client-case-notes').html('');
                    if (isEmpty) {
                        $('.prof-empty-status').addClass('Status-title-Empty');
                    }
                    else if (isClose) {
                        $('.prof-closed-status').addClass('Status-title-Closed');

                    }
                    else {
                        $('.prof-open-status').addClass('Status-title-Open');

                    }

                    var imagesrc = (data[0].Profilepic === "") ? "/Images/prof-image.png" : "data:image/jpg;base64," + data[0].Profilepic;
                    $('#ProfileImage').attr('src', imagesrc);
                    $("#ProfileImage").css("display", "block");

                    if (data[0].ParenFemaleName === "" || data[0].ParenFemaleName === null) {
                        $('.mother-figure').addClass('hidden');
                    }
                    else {
                        $('.mother-figure').removeClass('hidden');
                        $('.mother-fig-dob').html(data[0].MotherDOB);
                        $('.mother-fig-job').html(data[0].MotherJobTraining);
                        $('.mother-fig-employed').html(data[0].MotherIsEmployed);
                        $('.parent-female-name').html(data[0].ParenFemaleName);
                    }

                    if (data[0].ParentMaleName === "" || data[0].ParentMaleName === null) {
                        $('.father-figure').addClass('hidden');
                    }
                    else {

                        $('.father-figure').removeClass('hidden');
                        $('.father-fig-dob').html(data[0].FatherDOB);
                        $('.father-fig-job').html(data[0].FatherJobTraining);
                        $('.father-fig-employed').html(data[0].FatherIsEmployed);
                        $('.father-fig-name').html(data[0].ParentMaleName);
                        $('.preg-mother-address').html(data[0].PregnantAddress);
                    }
                }
                $('.client-profile').removeClass('hidden');
                if (!isHost) {
                    $('html, body').animate({
                        scrollTop: $(".client-profile").offset().top
                    }, 1000);
                }

            }
        });
    }


});