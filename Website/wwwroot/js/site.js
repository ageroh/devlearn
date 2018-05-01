var mode = $("#js-holder").data("mode");

if (mode === "poll") {
    initiatePolling();
} else {
    initiateWebSocketConnection();
}


function initiatePolling() {
    var interval = 2000;
    var lastKnownScoreId = 0;
    var poll = function() {
        $.get("/scores?lastKnownScoreId=" + lastKnownScoreId,
            function(result) {
                if (result && result.length > 0) {
                    render(result);
                    console.log(result);
                    lastKnownScoreId = result[0].id;
                }
                setTimeout(poll, interval);
            });
    }
    poll();
}

function initiateWebSocketConnection() {
    var ws = new WebSocket("ws://localhost:8181/scores");
    ws.onopen = function () {
        console.log("Connection opened");
    }
    ws.onclose = function () {
        console.log("Connection closed");
    }
    ws.onmessage = function (e) {
        render(JSON.parse(e.data));
    }
}

function render(data) {
    var template = $("#js-row-template").html();
    for (var i = 0; i < data.length; i++) {
        var score = data[i];
        var event = score.event.eventname;
        var eventId = score.eventid;
        var home = score.home;
        var away = score.away;
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