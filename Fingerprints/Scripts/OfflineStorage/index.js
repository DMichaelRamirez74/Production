//initial page load
$(document).ready(function () {
    $('#btnUpdateUser').hide();
    GetAllUser("");

});

//to add ne user
$(document.body).on("click", "#btnNewUser", function () {
    if (validate()) {
        var data = '{"Name":"' + $('#name').val() + '","Email":"' + $('#email').val() + '","Technology":"' + $('#tech').val() + '"}';
        data = JSON.parse(data);
        clear();
        DataBaseManager.AddnewUser(data, GetAllUser);
    } else {
        alertCall("Please enter all the values");
    }
});

//clear button call
$(document.body).on("click", "#btnClear", function () {
    clear();
});

//update id call
$(document.body).on("click", ".userid", function () {
    getDataToUpdate(this);
});


//to add ne user
$(document.body).on("click", "#btnUpdateUser", function () {
    if (validate()) {
        var data = '{"UserID":' + parseInt($('#userid').val()) + ',"Name":"' + $('#name').val() + '","Email":"' + $('#email').val() + '","Technology":"' + $('#tech').val() + '"}';
        data = JSON.parse(data);
        clear();
        DataBaseManager.UpdateUser(data, GetAllUser);
    } else {
        alertCall("Please enter all the values");
    }
});


//function to delete the user
function deleteUser(data,row) {
    bootbox.confirm("Are you sure you want to delete " + data + "?", function (result) {
        if (result == true) { $(row).parent().parent().remove(); DataBaseManager.DeleteUser(data, alertCall); }
    });
}

//function to get all the users
function GetAllUser(data) {
    if (data != "") { alertCall(data); }
    DataBaseManager.GetAllUser(listAllUsers);
}

//function to list all the users to table
function listAllUsers(data) {
    $('#UserDetails tbody tr').remove();
    for (var i = 0; i < data.length; i++) {
        var tr = "<tr>";
        tr = tr + "<td><div class='userid' style='cursor: pointer;'>" + data[i].UserID + "</div></td>";
        tr = tr + "<td class='useremail'>" + data[i].Email + "</td>";
        tr = tr + "<td class='username'>" + data[i].Name + "</td>";
        tr = tr + "<td class='usertech'>" + data[i].Technology + "</td>";
        tr = tr + "<td>" + "<button onclick='deleteUser(" + data[i].UserID + ",this)'>Delete</button>" + "</td>";
        tr = tr + "</tr>";
        $('#UserDetails tbody').append(tr);
    }
}

//callback functions
function alertCall(data) {
    bootbox.alert(data);
}

//validate all the text boxes
function validate() {
    if ($('#name').val().trim().length == 0 || $('#email').val().trim().length == 0 || $('#tech').val().trim().length == 0) {
        return false;
    } else {
        return true;
    }
}

//claer all the fields
function clear() {
    $('#name').val("");
    $('#email').val("");
    $('#tech').val("");
    $('#userid').val(0);
    $('#btnNewUser').show();
    $('#btnUpdateUser').hide();
    $("#email").removeAttr('disabled');
}

//get value to textboxes from table row for update
function getDataToUpdate(row) {
    debugger;
    var tr = $(row).parent().parent();
    $('#name').val($(tr).find('.username').text().trim());
    $('#email').val($(tr).find('.useremail').text().trim());
    $('#tech').val($(tr).find('.usertech').text().trim());
    $('#userid').val(parseInt($(tr).find('.userid').text().trim()));
    $('#btnNewUser').hide();
    $('#btnUpdateUser').show();
    $("#email").attr("disabled", "disabled");
}
