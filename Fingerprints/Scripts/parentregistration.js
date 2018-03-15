
var enc_Eve_Id = "";
$(function () {


    //checks the reg count is greater than avail count//
    var getCurrentCount = $('#Ev_Curcount').val();
    var getTotalCount = $('#Ev_Totcount').val();
    var getAvailSeats = getTotalCount - getCurrentCount;
    if (getTotalCount > getCurrentCount) {
        $('#availSlotsSpan').html(getAvailSeats);
        $('#householdRegisterSpan').html(getCurrentCount);
        $('#myModalNoSlot').modal('show');
    }
    //checks the reg count is greater than avail count//

    enc_Eve_Id = $('#enc_eventId').val();

    $('#addRegistration').on('click', function () {
        cleanValidation();
        if ($('input[name=householdChx]:checked').length == 0) {

            customAlert('Please select household member');
            return false;
        }

        registerEvent(true, 1);
    });

    $('#addNewHousehold').on('click', function () {
        var externalName = $('#externalName').val();
        var genderSelect = $('#external_select').val();
        var genderType = (genderSelect == '1') ? 'Male' : (genderSelect == '2') ? 'Female' : 'Other'
        if (externalName == '') {
            customAlert('Please enter Name');
            plainValidation('#externalName');
            return false;
        }
        getAvailableslots();
        var ulLength = $('#externalUl').children('li').length;
        var listElement = '<li class="li' + ulLength + '" en_id="0"><span class="ext_name_span">' + externalName + '</span><span class="ext_gender_span">' + genderType + '</span><p><img src="/images/rd_close.png" class="delete_external" pos=' + ulLength + ' style="cursor:pointer;" onclick="deleteExternal(this);"></p></li>';
        $('#externalUl').append(listElement);
        $('#externalName').val('');
        $('#external_select').val('1');

        $(".add_pp").addClass("add_pp1");
        $(".sub").css({ "display": "none" });
        $(".add").css({ "display": "inline-block" });
        $(".list_midd_sec ul").css({ "display": "none" });
    });

    $('#cancelRegistration').on('click', function () {
        var event_id = $('#enc_eventId').val();

        registerEvent(false, 2);
    });

    $('.house-checkbox').on('change', function () {


        getAvailableslots();
    });
});



function deleteExternal(val) {

    var pos = $(val).attr('pos');
    $('#externalUl').find('.li' + pos + '').hide();
}

function registerEvent(Stauts, regMode) {
    var events = {
        'Enc_EventId': enc_Eve_Id,
        'MilesDriven': 0,
        'TimeTaken': 0,
        'Enc_ClientId': $('#masterClient').attr('cli_id'),
        'Signature': "",
        'IsUpdate': $('#isUpdate').val().trim()
    };

    var arr = [];


    $('#householdList').find('li').each(function (i, hou) {
        var isChecked = $(hou).find('input[name=householdChx]').is(':checked');
        var name = $(hou).children('span').html();
        var gender = $(hou).children('span').attr('gender');
        var en_id = $(hou).children('span').attr('cli_id');
        var en_rsv_id = $(hou).find('input[name=householdChx]').attr('en_id');
        var isReg = $(hou).find('input[name=householdChx]').attr('isReg');
        var parent = '';
        if (isChecked) {



            parent = {

                'FullName': name,
                'Enc_ClientId': en_id,
                'Gender': gender,
                'Enc_RSVPId': en_rsv_id,
                'Status': Stauts,
                'IsRegistered': (isReg === 'True') ? true : false
            };

            arr.push(parent);
        }
        else {

            if (isReg == 'True') {
                parent = {

                    'FullName': name,
                    'Enc_ClientId': en_id,
                    'Gender': gender,
                    'Enc_RSVPId': en_rsv_id,
                    'Status': false,
                    'IsRegistered': (isReg === 'True') ? true : false
                };
                arr.push(parent);
            }
        }
    });

    if ($('#externalUl').find('li').length > 0) {
        $('#externalUl').find('li').each(function (j, el) {
           
            var extname = $(el).children('.ext_name_span').html();
            var extgender = $(el).children('.ext_gender_span').html();
            var enc_rs_id = $(el).attr('en_id');
            var isReg = $(el).attr('isReg');
            var parent = {};
            parent.FullName = extname;
            parent.Enc_ClientId = '0';
            parent.Gender = extgender;
            var isUpdate = ($('#isUpdate').val() == 'True');
            var parent2 = '';

            if (isUpdate && ($(el).css('display') == 'none')) {
                parent2 = {

                    'FullName': extname,
                    'Enc_ClientId': null,
                    'Gender': extgender,
                    'Enc_RSVPId': enc_rs_id,
                    'Status': false,
                    'IsRegistered': (isReg === 'True') ? true : false
                };
                arr.push(parent2);
            }

            else if (!($(el).css('display') == 'none')) {
                parent2 = {

                    'FullName': extname,
                    'Enc_ClientId': null,
                    'Gender': extgender,
                    'Enc_RSVPId': enc_rs_id,
                    'Status': Stauts,
                    'IsRegistered': (isReg === 'True') ? true : false
                };
                arr.push(parent2);
            }

            
        });
    }

    var parentModal = {
        'Events': events,
        'HouseholdList': arr
    };
    parentModal.Events = events;


    var modelString = JSON.stringify(parentModal);
    $.ajax({
        url: '/Events/RegisterEvent',
        datatype: 'json',
        type: 'post',
        data: { parentEventString: modelString, mode: regMode },
        success: function (data) {

            if (regMode === 1) {
                if (data.RegStatus) {
                    customAlert('Data saved successfully');
                    location.href = '/Events/ParentEventSelection';
                }
                else if (data.RegStatus === false) {

                    if (data.AvailableSlots === 0) {
                        $('#availSlotsanchor').html(data.AvailableSlots).css({ 'background': '#e74c3c' });
                    }
                    else {
                        $('#availSlotsanchor').html(data.AvailableSlots).css({ 'background': '#27ae60' });
                    }
                    $('#noSlotCheckModal').find('#availSlotsSpan').html(data.AvailableSlots);
                    $('#noSlotCheckModal').find('#householdRegisterSpan').html(data.NewHousholds);
                    $('#noSlotCheckModal').modal('show');
                }


            }
            else if (regMode === 2) {
                if (data.RegStatus) {
                    customAlert('Data saved successfully');
                    location.href = '/Events/ParentEventSelection';
                }
                else {
                    customAlert('Some error occured.Please,try again later.');
                }
            }

        },
        error: function (data) {
            customAlert('Some error occured.Please,try again later.');
        }


    });
}

function cancelEventRegistration(eveId) {
    $.ajax({
        url: '/Events/CancelEventRegistration',
        type: 'post',
        datatype: 'json',
        data: { enc_EventId: eveId },
        success: function (data) {
            if (data) {
                if ($('#isMode').val() === "1") {
                    location.href = '/Events/ParentEventSelection';

                }
                else {
                    location.href = '/Parent/SchoolCalendar';

                }
            }
            else {
                customAlert('Error occured.Please try again later');
            }
        },
        error: function (data) {
            customAlert('Error occured.Please try again later');

        }
    })
}

//imgCapture
function clearCanvas(canvas, context) {
    context.clearRect(0, 0, canvas.width, canvas.height);
}

function getAvailableslots() {
    $.ajax({
        url: '/Events/GetRemainingSeats',
        datatype: 'json',
        type: 'post',
        data: { enc_Id: enc_Eve_Id },
        success: function (data) {

        },
        error: function (data) {

        }
    });
}