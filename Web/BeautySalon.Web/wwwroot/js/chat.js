function chat() {
    updateScrollToBottom();

    var connection =
        new signalR.HubConnectionBuilder()
            .withUrl("/chat")
            .build();

    connection.start().then(function () {
        var senderUsername = document.getElementById("sender-username").innerText;
        var receiverUsername = document.getElementById("receiver-username").innerText;
        var groupName = document.getElementById("group-name").innerText;

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
        var formattedDate = formatDate();

        var chatInfo = `<div class="media"><div class="media-body ml-2"><time class="small theme-color-gold">${formattedDate}</time><h6 class="mt-0 mb-1">${username} says:</h6><p class="small mb-2 color-black"> ${escapeHtml(message)}</p></div><img id="small-pic" src="${picture}" class="mr-2 img-fluid" alt="user-pic"></div><hr />`;

        $("#messagesList").append(chatInfo);
        updateScrollToBottom();
    });

    connection.on("ReceiveMessage", function (message, username, picture, currentGroupName) {
        var groupName = document.getElementById("group-name").innerText;

        if (groupName !== currentGroupName) {
            return;
        }

        var formattedDate = formatDate();

        var chatInfo = `<div class="media"><img id="small-pic" src="${picture}" class="img-fluid mr-2" alt="user-pic">
                                                                                               <div class="media-body mr-2 text-right"><time class="small theme-color-gold">${formattedDate}</time><h6 class="mt-0">${username} says:</h6><p class="small text-right color-black"> ${escapeHtml(message)}</p></div></div><hr />`;

        $("#messagesList").append(chatInfo);
        updateScrollToBottom();
    });

    function formatDate() {
        return moment().local().format('L LT');
    }

    function updateScrollToBottom() {
        var messageBody = document.querySelector('#messagesList');
        messageBody.scrollTop = messageBody.scrollHeight - messageBody.clientHeight;
    }

    function escapeHtml(unsafe) {
        return unsafe
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;")
            .replace(/"/g, "&quot;")
            .replace(/'/g, "&#039;");
    }
}