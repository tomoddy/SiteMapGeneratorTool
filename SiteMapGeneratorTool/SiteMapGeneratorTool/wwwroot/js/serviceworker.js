self.addEventListener('fetch', function (event) { });

self.addEventListener('push', function (e) {
    var payload = e.data.text().split(",");
    console.log(payload);

    var options = {
        body: payload[0],
        icon: "../static/favicon.ico",
        data: {
            dateOfArrival: Date.now(),
            guid: payload[1]
        },
        actions: [
            {
                action: "explore", title: "View"
            },
            {
                action: "close", title: "Ignore"
            },
        ]
    };
    e.waitUntil(
        self.registration.showNotification("Web Crawl Complete", options)
    );
});

self.addEventListener('notificationclick', function (e) {
    var notification = e.notification;
    var action = e.action;

    if (action == "explore") {
        clients.openWindow(`/results?guid=${e.notification.data.guid}`);
    }
    else {
        notification.close();
    }
});