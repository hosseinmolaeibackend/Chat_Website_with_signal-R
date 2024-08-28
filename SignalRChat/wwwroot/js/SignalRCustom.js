    
var myModal = $("#myModal");
var myInput = $("#myInput");


$('#myModal').on('shown.bs.modal', function () {
        $('#myInput').focus();
    });
function appendGroup(groupName, token) {
        if (groupName === 'Errorr') {
            alert("Errorr");
        } else {
            $(".rooms ul").append(`
            <li onclick="JoinInGroup("${token}")">
                ${groupName}
                <img src='img/Default.jpg' />
                <span class="badge badge-light">4</span>
            </li>
            `);
            $("#exampleModal").modal({ show: false });
        }
    }


function joines(group, chats) {
    $(".footer").css("display", "block");
    $(".header").css("display", "block");
    $(".header h2").html(group.groupTitle);
    currentGroupId = group.id;
    console.log(currentGroupId);
    for (var i in chats) {
        var chat = chats[i];
        if (userId === chat.userId) {
            console.log("پیام من:", chat.chatBody);
            $(".chats").append(`
                                    <div class="chat-me">
                                        <div class="chat">
                                                <span>${chat.userName}</span>
                                            <p>${chat.chatBody}</p>
                                                    <span>${chat.createDate}</span>
                                        </div>
                                    </div>
                                `);
        } else {
            console.log("پیام شما:", chat.chatBody);
            $(".chats").append(`
                                    <div class="chat-you">
                                        <div class="chat">
                                                <span>${chat.userName}</span>
                                            <p>${chat.chatBody}</p>
                                                    <span>${chat.createDate}</span>
                                        </div>
                                    </div>
                                `);
        }
    }
    console.log(chats);
}
function insertGroup(event) {
        event.preventDefault();
        console.log(event);
        var groupname = event.target[1].value;
        console.log(groupname);
        var formData = new FormData();
        formData.append("GroupName", groupname);
        $.ajax({
            url: "/home/CreateGroup",
            type: "post",
            data: formData,
            enctype: "multipart/from-data",
            processData: false,
            contentType: false
        });
    }
function search() {
        var text = $("#search_input").val();
        if (text) {
            $("#search_result").show();
            $("#user_group").hide();
            $.ajax({
                url: "/home/search?title=" + text,
                type: "get"
            }).done(function (data) {
                $("#search_result ul").html("");
                console.log(data);
                for (var i in data) {
                    if (data[i].isUser) {
                        $("#search_result ul").append(`
                                             <li onclick="JoinInGroup('${data[i].token}')">
                                             ${data[i].tiltle}
                                     <img src='img/Default.jpg' />
                                     <span>17:20 1400/01/15</span>
                                    </li>
                            `);
                    }
                    else {
                        $("#search_result ul").append(`
                                                     <li onclick="JoinInGroup('${data[i].token}')">
                                                             ${data[i].tiltle}
                                             <img src='img/Default.jpg' />
                                             <span>17:20 1400/01/15</span>
                                            </li>
                                    `);
                    }

                }
            });
        }
        else {
            $("#search_result").hide();
            $("#user_group").show();
        }
}

function receive(chat) {
    $("#messagetext").val('');
    if (userId === chat.userId) {
        console.log("پیام من:", chat.chatBody);
        $(".chats").append(`
                            <div class="chat-me">
                                <div class="chat">
                                <span>${chat.userName}</span>
                                    <p>${chat.chatBody}</p>
                                            <span>${chat.createDate}</span>
                                </div>
                            </div>
                        `);
    } else {
        console.log("پیام شما:", chat.chatBody);
        $(".chats").append(`
                            <div class="chat-you mb-3">
                                <div class="chat">
                                        <span>${chat.userName}</span>
                                    <p>${chat.chatBody}</p>
                                            <span>${chat.createDate}</span>
                                </div>
                            </div>
                        `);
    }

}

function SendMessage(event) {
    event.preventDefault();
    var text = $("#messagetext").val();
    if (text != "") {
        connection.invoke("SendMessage", text, currentGroupId);
    }
    else {
        alert("errorr");
    }
}



function JoinInGroup(token) {
    connection.invoke("JoinGroup", token, currentGroupId);
}





