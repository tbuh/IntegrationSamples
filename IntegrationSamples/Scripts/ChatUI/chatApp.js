var getChatMessage;
var setChatMessage;

$(document).ready(function () {

    var isBotActivated = false;
    var isBotWeatherActivated = false;
    var chatCommands = [
        'You can use different commands:',
        '<ul>',
        '<li>clear</li>',
        '<li>close</li>',
        '<li>bot</li>',
        '<li>weather</li>',
        '</ul>',
        'Just type them in input below'
    ].join('');

    $(".openpopSAP").click(function (e) {
        setChatMessage("SAP: sending request...");
        chatUI.trigger('is-typing');

        var data = null;

        var xhr = new XMLHttpRequest();
        xhr.withCredentials = false;

        xhr.addEventListener("readystatechange", function () {
            if (this.readyState === this.DONE) {
                setChatMessage("SAP: " + this.responseText.split(":").join("\n").split(",").join("\n"));
            }
        });
        
        //setting request method
        //API endpoint for API sandbox 
        xhr.open("GET", "https://sandbox.api.sap.com/concur/api/v3.0/common/users?employeeID=ConcurConsultant");

        //API endpoint with optional query parameters
        //xhr.open("GET", "https://sandbox.api.sap.com/sapb1/b1s/v2/Orders({DocEntry})?$select=array");

        //Available API Endpoints
        //https://{host}:{port}/b1s/v2

        //adding request headers
        //xhr.setRequestHeader("demoDB", "string");
        xhr.setRequestHeader("Content-Type", "application/json");
        xhr.setRequestHeader("Accept", "application/json");
        //API Key for API Sandbox
        xhr.setRequestHeader("APIKey", "Qv3z8J83dK866pT8ZNbTVxsJ3FPFGcdB");


        //Available Security Schemes for productive API Endpoints
        //Basic Authentication

        //Basic Auth : provide username:password in Base64 encoded in Authorization header
        //xhr.setRequestHeader("Authorization", "Basic <Base64 encoded value>");

        //sending request
        xhr.send(data);

    });

    $(".openpopBot").click(function (e) {
        setChatMessage("Please ask BOT...");
        isBotActivated = true;
    });

    $(".openpopWeather").click(function (e) {
        setChatMessage("Please send me the location you are interested in.");
        isBotWeatherActivated = true;
    });

    var fbUserId = "";
    var chat = $.connection.chat;
    // Create a function that the hub can call back to display messages.
    chat.client.addNewMessageToPage = function (name, message) {
        fbUserId = name;
        setChatMessage(name + ": " + message);
    };

    chat.client.setSAP = function (message) {
        setChatMessage("SAP: " + message);
    };

    chat.client.addBotMessageToPage = function (name, message) {
        isBotActivated = false;
        setChatMessage(name + ": " + message);
    };

    chat.client.addBotWeatherToPage = function (message) {
        isBotWeatherActivated = false;
        setChatMessage(message);
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
            else if (isBotWeatherActivated) {
                chat.server.getWeatherFor(agentMessage);
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
            chatUI.trigger('add-phrase', chatCommands);
        } else if (msg === 'bot') {
            isBotActivated = true;
            setChatMessage('Please ask BOT...');
        }
        else if (msg === 'weather') {
            isBotWeatherActivated = true;
            setChatMessage('Please send me the location you are interested in.');
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
        if (msg !== 'close' && msg !== 'clear' && msg !== 'bot' && msg !== 'weather')
            getChatMessage(msg);
    };

    chatUI.trigger('add-phrase', chatCommands);

    chatUI.on('user-send-message', chatMessage);

    chatUI.on('chat-closed', function (data) {
        console.log('chat-closed', data);
        $chatIcon.show();
        chat.server.closeChat();
    });
});