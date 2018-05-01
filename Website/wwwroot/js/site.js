$(function () {

    var ws = new WebSocket('ws://localhost:8181/socket');
    ws.onopen = function (e) {
        console.log('Connection opened');
    }

    ws.onclose = function (e) {
        console.log('Connection closed');
    }

    ws.onmessage = function (e) {
        var data = JSON.parse(e.data);

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
});