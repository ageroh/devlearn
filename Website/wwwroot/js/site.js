$(function () {
    
});


function initiatePolling() {
    var interval = 2000;
    var lastKnownScoreId = 0;
    var poll = function() {
        $.get("/polling?lastKnownScoreId=" + lastKnownScoreId,
            function (result) {
                if (result) {
                    render(result);

                }
                
                setTimeout(poll, interval);
            });
    }

}

function initiateWebSocketConnection() {
    var ws = new WebSocket("ws://localhost:8181/socket");
    ws.onopen = function () {
        console.log("Connection opened");
    }
    ws.onclose = function () {
        console.log("Connection closed");
    }
    ws.onmessage = function (e) {
        render(e.data);
    } 
}

function render(result) {
    if (!result) {
        return;
    }
    var data = JSON.parse(result);
    var template = $("#js-row-template").html();
    for (var i = 0; i < data.length; i++) {
        var score = data[i];
        var event = score.Event.EventName;
        var eventId = score.EventId;
        var home = score.Home;
        var away = score.Away;
        var eventrow = $("#js-event-" + eventId);
        if (eventrow.length === 0) {
            eventrow = $(template);
            eventrow.attr("id", "js-event-" + eventId);
            $("#js-holder").append(eventrow);
        }
        eventrow.find(".js-event").html(event);
        eventrow.find(".js-home").html(home);
        eventrow.find(".js-away").html(away);
    }
}