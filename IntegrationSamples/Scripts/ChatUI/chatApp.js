var getChatMessage;
var setChatMessage;

$(document).ready(function () {

    var isBotActivated = false;
    $(".openpopBot").click(function (e) {
        setChatMessage("Please ask BOT...");
        isBotActivated = true;
    });

    var fbUserId = "";
    var chat = $.connection.chat;
    // Create a function that the hub can call back to display messages.
    chat.client.addNewMessageToPage = function (name, message) {
        fbUserId = name;
        setChatMessage("FB User:" + message);
    };

    chat.client.addBotMessageToPage = function (name, message) {
        isBotActivated = false;
        setChatMessage(name + ": " + message);
    };

    var chatUI = ChatUI({
        title: $('#AgentName').val(),
        avatar: './john-doe.jpg',
        subtitle: ''
    }).render('#chat');

    var $chatIcon = $('#chat-icon');

    $.connection.hub.start().done(function () {
        getChatMessage = function (agentMessage) {
            // Call the Send method on the hub.
            if (isBotActivated) {
                chat.server.botSupport(agentMessage);
            }
            else {
                chat.server.send(agentMessage);
            }
            isBotActivated = false;
        }
    });

    $chatIcon.click(function () {
        chatUI.trigger('open-chat');
        $chatIcon.hide();
        chat.server.openChat($('#AgentId').val());
    });


    setChatMessage = function (msg) {
        chatUI.trigger('add-phrase', msg);
    };

    var timeoutId = null;
    var chatMessage = function (msg) {
        if (msg === 'close') {
            chatUI.trigger('close-chat');
        } else if (msg === 'clear') {
            chatUI.trigger('clear-dialog');
        } else if (msg === 'bot') {
            isBotActivated = true;
            setChatMessage('Please ask BOT...');
        }
        //else if (!timeoutId) {
        //    var waitTime = Math.floor(Math.random() * 2000) + 600;
        //    setTimeout(function () {
        //        chatUI.trigger('is-typing');
        //    }, 500);
        //    timeoutId = setTimeout(function () {
        //        timeoutId = null;
        //    }, waitTime);
        //}
        if (msg !== 'close' && msg !== 'clear' && msg !== 'bot')
            getChatMessage(msg);
    };

    chatUI.trigger('add-phrase', [
        'You can use different commands:',
        '<ul>',
        '<li>clear</li>',
        '<li>close</li>',
        '<li>bot</li>',
        '</ul>',
        'Just type them in input below'
    ].join(''));

    chatUI.on('user-send-message', chatMessage);

    chatUI.on('chat-closed', function (data) {
        console.log('chat-closed', data);
        $chatIcon.show();
        chat.server.closeChat();
    });
});