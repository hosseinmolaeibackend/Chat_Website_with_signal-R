    
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
            <li onclick="JoinInGroup('${token}')">
                ${groupName}
                <img src='img/Default.jpg' />
                <span>17:20 1400/01/15</span>
            </li>
            `);
            $("#exampleModal").modal({ show: false });
        }
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


