(function () {
    'use strict';
    angular
        .module('sti.signalr')
        .factory('signalRClient', signalRClient);

    signalRClient.$inject = [];
    function signalRClient() {

        function createHub(address, hubName) {

            var _connection = $.hubConnection(address);
            var _proxy = _connection.createHubProxy(hubName);
            var connected = false;

            return {
                on: (eventName, callback) => {
                    _proxy.on(eventName, callback);
                },

                onStateChanged: (callback) => {
                    _connection.stateChanged(callback);
                },

                onError: (callback) => {
                    _connection.error(callback);
                    logger.error(hubName + ': Error');
                },

                connect: () => setTimeout(() => {
                    _connection.start()
                        .done(() => { connected = true; })
                        .fail(() => { connected = false; });
                }),

                disconnect: () => {
                    _connection.stop();
                },

                onDisconnected: (callback) => {
                    _connection.disconnected(callback);
                },

                send: (id, methodName) => {
                    _connection
                        .start()
                        .done(() => {
                             console.log('enviado');
                             _proxy.invoke("processSent", { "tenant": "atiradores", "id": id, "name": methodName });
                         });
                },

                reconnect: function () {
                    this.disconnect();
                    this.connect();
                }
            };
        }

        return createHub;
    }

    angular.module('sti.signalr')
        .value('signalRStates', {
            connecting: $.signalR.connectionState.connecting,
            connected: $.signalR.connectionState.connected,
            reconnecting: $.signalR.connectionState.reconnecting,
            disconnected: $.signalR.connectionState.disconnected
        });

})();