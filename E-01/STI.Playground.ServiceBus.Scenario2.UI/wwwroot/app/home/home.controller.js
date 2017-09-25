(function (undefined) {
    'use strict';
    angular
        .module('home')
        .controller('homeController', homeController);

    homeController.$inject = ['ssmartRequesting', 'signalRClient', 'BASE_URL_SIGNALR', '$timeout', '$rootScope', '$scope', 'toastr'];
    function homeController(ssmartRequesting, signalRClient, BASE_URL_SIGNALR, $timeout, $rootScope, $scope, toastr) {
        var vm = this;
        vm.save = save;
        vm.waintCommand = false;
        vm.queryInProgress = true;
        $rootScope.$on('addressSaved', addressSavedEvent);
        $rootScope.$on('debtsCalculationStarted', debtsCalculationStartedEvent);
        $rootScope.$on('finishedCalculationDebts', finishedCalculationDebtsEvent);
        $rootScope.$on('sendingMail', sendingMailEvent);
        $rootScope.$on('emailSent', emailSentEvent);

        activate();

        function activate() {
            vm
                .states = 'AC AL AP AM BA CE DF ES GO MA MT MS MG PA PB PR PE PI RJ RN RS RO RR SC SP SE TO'
                .split(' ')
                .map(function (state) {
                    return { abbrev: state };
                });
            vm.queryInProgress = true;
            ssmartRequesting
                .get("people/a8f40773-b1e2-4f3e-b813-0052e10090f1")
                .then(
                    function (data) {
                        vm.user = data;
                        vm.queryInProgress = false;
                        console.log('get response',data);
                    },
                    function (error) {
                        vm.queryInProgress = false;
                        console.log("Error on get person");
                    }
                );
        }

        function save(newAddress) {
            vm.waintCommand = true;
            ssmartRequesting
                .put("people/a8f40773-b1e2-4f3e-b813-0052e10090f1", newAddress)
                .then(
                    function (data) {
                        if (data !== "Command sent")
                            vm.waintCommand = false;
                    },
                    function (error) {
                        console.log("Error on update address");
                        vm.waintCommand = false;
                    }
                );
        }

        function addressSavedEvent(event, data) {
            toastr.success('AddressAddress was saved successfully!', 'Success');
            $timeout(function () {
                vm.waintCommand = false;
            });
        }

        function debtsCalculationStartedEvent(event, data) {
            toastr.info('Awaiting calculations of debts', 'Information');
            $timeout(function () {
                vm.waintCommand = true;
            });
        }

        function finishedCalculationDebtsEvent(event, data) {
            toastr.success('Finished calculation of debts!', 'Success');
            $timeout(function () {
                vm.waintCommand = true;
            });
        }

        function sendingMailEvent(event, data) {
            toastr.info('Sending mail', 'Information');
            $timeout(function () {
                vm.waintCommand = true;
            });
        }

        function emailSentEvent(event, data) {
            toastr.success('Email sent', 'Success');
            $timeout(function () {
                vm.waintCommand = true;
            });
        }
    }
})();