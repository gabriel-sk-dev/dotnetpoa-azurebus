(function (undefined) {
    'use strict';

    var app = angular
                .module('azureBusApp', [
                    'ngMaterial',
                    'ngAnimate', 
                    'toastr',
                    'ui.router',
                    'notifications',
                    'home',
                    'common.request',
                    'sti.signalr'
                ]);

    //app.constant('BASE_URL_SIGNALR', 'http://sti-signalr-demo.azurewebsites.net');
    app.constant('BASE_URL_SIGNALR', 'http://localhost:2627');
    app.config(configApp);

    configApp.$inject = ['$stateProvider', '$urlRouterProvider'];
    function configApp($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('home',
            {
                templateUrl: "app/home/main.html",
                controllerAs: "vm",
                controller: "homeController",
                url: '/'
            });
        $urlRouterProvider.otherwise("/");
    }

})();