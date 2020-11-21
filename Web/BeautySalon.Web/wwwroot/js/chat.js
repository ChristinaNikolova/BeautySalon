function chat() {
    updateScrollToBottom();

    var connection =
        new signalR.HubConnectionBuilder()
            .withUrl("/chat")
            .build();

    connection.start().then(function () {
        var senderUsername = document.getElementById("sender-username").innerText;
        var receiverUsername = document.getElementById("receiver-username").innerText;

        connection
            .invoke("CreateGroup", senderUsername, receiverUsername, groupName);
    });

    $("#sendButton").click(function () {
        var chatMessage = $("#messageInput").val();
        var senderUsername = document.getElementById("sender-username").innerText;
        var receiverUsername = document.getElementById("receiver-username").innerText;
        var groupName = document.getElementById("group-name").innerText;

        var model = {
            chatMessage: chatMessage,
            senderUsername: senderUsername,
            receiverUsername: receiverUsername,
            groupName: groupName,
        };

        connection.invoke("Send", model);
        $("textarea").val("");
        $("textarea").focus();
    });

    connection.on("SendMessage", function (message, username, picture) {
        var date = new Date();
        var formattedDate = formatDate(date);

        var chatInfo = `<div class="media"><div class="media-body ml-2"><span class="small theme-color-gold">${formattedDate}</span><h6 class="mt-0 mb-1">${username} says:</h6><p class="small mb-2 color-black"> ${message}</p></div><img id="small-pic" src="${picture}" class="mr-2 img-fluid" alt="user-pic"></div><hr />`;

        $("#messagesList").append(chatInfo);
        updateScrollToBottom();
    });

    connection.on("ReceiveMessage", function (message, username, picture, currentGroupName) {
        var groupName = document.getElementById("group-name").innerText;

        if (groupName !== currentGroupName) {
            return;
        }
        var date = new Date();
        var formattedDate = formatDate(date);

        var chatInfo = `<div class="media"><img id="small-pic" src="${picture}" class="img-fluid mr-2" alt="user-pic">
                                                                                               <div class="media-body mr-2 text-right"><span class="small theme-color-gold">${formattedDate}</span><h6 class="mt-0">${username} says:</h6><p class="small text-right color-black"> ${message}</p></div></div><hr />`;

        $("#messagesList").append(chatInfo);
        updateScrollToBottom();
    });

    function formatDate(date) {
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var ampm = hours >= 12 ? 'pm' : 'am';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        minutes = minutes < 10 ? '0' + minutes : minutes;
        var strTime = hours + ':' + minutes + ' ' + ampm.toUpperCase();
        return date.getMonth() + 1 + "/" + date.getDate() + "/" + date.getFullYear() + " " + strTime;
    }

    function updateScrollToBottom() {
        var messageBody = document.querySelector('#messagesList');
        messageBody.scrollTop = messageBody.scrollHeight - messageBody.clientHeight;
    }
}