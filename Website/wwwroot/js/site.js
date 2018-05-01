(function () {
    var container = $("#js-push-holder");
    var ws = new WebSocket("ws://localhost:8181/scores");
    ws.onopen = function () {
        console.log("Connection opened");
    }
    ws.onclose = function () {
        console.log("Connection closed");
    }
    ws.onmessage = function (e) {
        render(container, JSON.parse(e.data));
    }
})();

(function () {
    var container = $("#js-poll-holder");
    var interval = 5000;
    var lastKnownScoreId = 0;
    var poll = function () {
        $.get("/scores?lastKnownScoreId=" + lastKnownScoreId,
            function (result) {
                if (result && result.length > 0) {
                    render(container, result);
                    lastKnownScoreId = result[0].id;
                }
                setTimeout(poll, interval);
            });
    }
    poll();
})();



function render(container, data) {
    var template = $("#js-row-template").html();
    for (var i = 0; i < data.length; i++) {
        var score = data[i];
        var eventname = score.event.eventname;
        var eventid = score.eventid;
        var home = score.home;
        var away = score.away;
        var eventrow = container.find(".js-event-" + eventid);
        if (eventrow.length === 0) {
            eventrow = $(template);
            eventrow.addClass("js-event-" + eventid);
            eventrow.data("sid", eventid);
            container.append(eventrow);
        }
        eventrow.find(".js-event-name").html(eventname);
        eventrow.find(".js-home").html(home);
        eventrow.find(".js-away").html(away);
    }
    
    var events  = container.children(".js-event-container");
    events.detach().sort(function(a, b) {
            var astts = parseInt($(a).data("sid"));
            var bstts = parseInt($(b).data("sid"));
            return (astts > bstts) ? (astts > bstts) ? 1 : 0 : -1;
        });
    container.append(events);
}