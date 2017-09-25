(function () {
    angular
        .module('notifications', [
            'sti.signalr'
        ]);

    angular
        .module('notifications')
        .run(runNotifications);

    runNotifications.$inject = ['$rootScope', '$timeout', 'signalRClient', 'signalRStates', 'BASE_URL_SIGNALR'];
    function runNotifications($rootScope, $timeout, signalRClient, signalRStates, BASE_URL_SIGNALR) {
        var hub = signalRClient(BASE_URL_SIGNALR, "notificationHub");
        hub.connect();
        hub.onStateChanged(function (state) {});
        hub.onDisconnected(function () {
            $timeout(function () {
                hub.connect();
            }, 15000);
        });

        hub.on("processStarted", function (message) {
            $rootScope.$broadcast(message.name, { id: message.id, type: "started" });
        });

        hub.on("notifyProgress", function (message) {
            $rootScope.$broadcast(message.name, { id: message.id, type: "progress", progress: message.percentage });
        });

        hub.on("processFinished", function (message) {
            $rootScope.$broadcast(message.name, { id: message.id, type: "finished", adtionalInfo: message.adtionalInfo });
        });

        hub.on("processSent", function (message) {
            $rootScope.$broadcast(message.name, { id: message.id, type: "sent", adtionalInfo: message.adtionalInfo });
        });
    }
})();