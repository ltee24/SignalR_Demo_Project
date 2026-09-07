var connection = new signalR.HubConnectionBuilder().withUrl("/hubs/chat").withAutomaticReconnect([0, 1000, 5000, null]).build();


let selectedRoomId = null;
let selectedUserId = null;
let selectedUserName = null;

console.log("welcome");

connection.on("RecieveCreateRoomMessage", function (roomName, userName) {
    showMessage(`${userName} created room ${roomName}`);

})
connection.on("RecieveJoinRoomMessage", function (roomName, memberName) {
    showMessage(`${memberName} joined room ${roomName}`)
})

connection.on("ReceievePrivateMessage", function (senderName, receiverName, message) {
    showMessage(`${senderName}:${message}`)
})

connection.on("ReceiveRoomMessage", function (senderName, message) {
    showMessage(`${senderName}:${message}`)
})
document.addEventListener('DOMContentLoaded', (event) => {
    fillRoomDropDown();
    fillUserDropDown();


    document.getElementById('roomSelect').addEventListener('change', function () {
        document.getElementById('joinRoomBtn').disabled = this.value === '';
    });

    document.getElementById('leaveRoomSelect').addEventListener('change', function () {
        document.getElementById('leaveRoomBtn').disabled = this.value === '';
    });

    document.getElementById('deleteRoomSelect').addEventListener('change', function () {
        document.getElementById('deleteRoomBtn').disabled = this.value === '';
    });

    document.getElementById('userSelect').addEventListener('change', function () {
        document.getElementById('userSelectBtn').disabled = this.value === '';
    });
})

document.getElementById("sendMessageForm").addEventListener("submit", function (e) {
    e.preventDefault();

    let messageInput = document.getElementById("messageInput")
    let message = messageInput.value;
    console.log(message);
    console.log(selectedUserId);
    console.log(selectedUserName);

    $.ajax({
        url: '/ChatMessages/SaveChatMessages',
        dataType: "json",
        type: "POST",
        contentType: 'application/json;charset=utf-8',
        data: JSON.stringify({ roomId: selectedRoomId, recieverId: selectedUserId, message: message }),
        async: true,
        processData: false,
        cache: false,
        success: function (json) {
            console.log("Save successfully:", json);
            if (selectedRoomId != null) {
                connection.send("SendRoomMessage", selectedRoomId, message);
            }
            else if (selectedUserId != null) {
                connection.send("sendPrivateMessage", selectedUserId, selectedUserName, message);
            }
            messageInput.value = '';
        },
        error: function (xhr) {
            console.log("Status:", xhr.status);
            console.log("Response:", xhr.responseText);
        }

    });

  

    

})
function addNewRoom() {
    let createRoomName = document.getElementById('newRoomName');
    var roomName = createRoomName.value;
    console.log(roomName);
    if (roomName == null && roomName == '') {
        return;
    }

    $.ajax({
        url: '/ChatRooms/CreateRoom',
        dataType: "json",
        type: "POST",
        contentType: 'application/json;charset=utf-8',
        data: JSON.stringify({ id: 0, name: roomName }),
        async: true,
        processData: false,
        cache: false,
        success: function (json) {
            connection.send("SendAddRoomMessage", json.id, json.name);
            createRoomName.value = '';
        }

    })
}
function joinRoom() {
    let room = document.getElementById('roomSelect');
    var roomId = room.value;
    console.log(roomId);

    $.ajax({
        url: '/RoomMember/JoinRoom',
        dataType: "json",
        type: "POST",
        contentType: 'application/json;charset=utf-8',
        data: JSON.stringify({ roomId: roomId }),
        async: true,
        processData: false,
        cache: false,
        success: function (json) {
            console.log(json);
            connection.send("SendJoinRoomMessage", json.chatRoomId, json.memberName);
            room.value = '';
        }
    })
}


function selectRoom() {
    let room = document.getElementById('selectRoomSelect');
    selectedRoomId = room.value;
    selectedUserId = null;
    roomName = room.options[room.selectedIndex].text;
    showMessage(`${roomName} Chat`);

    $.ajax({
        url: '/ChatMessages/GetRoomMessages',
        dataType: "json",
        type: "GET",
        contentType: 'application/json;charset=utf-8',
        data: { roomId: selectedRoomId },
        async: true,
        success: function (json) {
            json.forEach(function (message) {
                showMessage(`${message.sender}:${message.message}`);
            });
        }

    })

}

function selectUser() {
    let user = document.getElementById('userSelect');
    selectedUserId = user.value;
    selectedRoomId = null;
    selectedUserName = user.options[user.selectedIndex].text;
    showMessage(`${selectedUserName} Chat`);

    $.ajax({
        url: '/ChatMessages/GetPrivateMessages',
        dataType: "json",
        type: "GET",
        contentType: 'application/json;charset=utf-8',
        data: { recieverId: selectedUserId },
        async: true,
        success: function (json) {
            json.forEach(function (message) {
                showMessage(`${message.sender}:${message.message}`);
            });
        },
        error: function (xhr) {
            console.log("Status:", xhr.status);
            console.log("Response:", xhr.responseText);
        }

    })
}






function showMessage(msg) {
    let ui = document.getElementById('messagesList');
    let li = document.createElement("li");
    li.innerHTML = msg;
    ui.appendChild(li);
}

function fillRoomDropDown() {

    $.getJSON('/ChatRooms/GetRooms')
        .done(function (json) {
            console.log(json);
            var deleteRoomSelect = document.getElementById("deleteRoomSelect");
            var roomSelect = document.getElementById("roomSelect");
            var leaveRoomSelect = document.getElementById("leaveRoomSelect");

            deleteRoomSelect.innerHTML = "";
            roomSelect.innerHTML = "";
            leaveRoomSelect.innerHTML = "";


            addPlaceholder(roomSelect, "Select room");
            addPlaceholder(leaveRoomSelect, "Select room");
            addPlaceholder(deleteRoomSelect, "Select room");



            json.forEach(function (item) {
                // var newOption = document.createElement("option");

                // newOption.text = item.name;
                // newOption.value = item.id;
                // deleteRoomSelect.add(newOption);
                // console.log("deleteroomselect:",deleteRoomSelect);


                // var newOption1 = document.createElement("option");

                // newOption1.text = item.name;
                // newOption1.value = item.id;
                // roomSelect.add(newOption1);
                // console.log(roomSelect);
                addRoomsList(roomSelect);
                addRoomsList(leaveRoomSelect);
                addRoomsList(deleteRoomSelect);
                function addRoomsList(roomField) {
                    var roomOption = document.createElement("option");

                    roomOption.text = item.name;
                    roomOption.value = item.id;
                    roomField.add(roomOption);
                }



            });

        })
        .fail(function (jqxhr, textStatus, error) {

            var err = textStatus + ", " + error;
            console.log("Request Failed: " + jqxhr.detail);
        });

}

function fillUserRoomDropdown() {
    $.getJSON('/RoomMember/')
}

function fillUserDropDown() {
    $.getJSON('/ChatRooms/GetUsers')
        .done(function (json) {
            json.forEach(function (item) {
                console.log(item.userName);
            });
            var userSelect = document.getElementById("userSelect");

            userSelect.innerHTML = '';
            addPlaceholder(userSelect, "Select User");

            json.forEach(function (item) {
                var newOption = document.createElement("option");
                newOption.text = item.userName;
                newOption.value = item.id;
                userSelect.add(newOption);
            });
        })


}

function addPlaceholder(select, text) {
    var placeholder = document.createElement("option");
    placeholder.text = text;
    placeholder.value = "";
    placeholder.disabled = true;
    placeholder.selected = true;
    select.add(placeholder);
}



connection.start();