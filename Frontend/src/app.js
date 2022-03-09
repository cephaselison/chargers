(function() {
    'use strict';

    angular.module('zaptec', [])
        // --------------------------------------------------------------------
        .constant('config', {
            apiUrl: 'http://localhost:5000/api'
        })
        // --------------------------------------------------------------------
        .service('apiService', [
            '$http', 'config',
            function($http, config) {
                function url() {
                    var url = config.apiUrl;
                    for(var i=0; i<arguments.length; i++) {
                        url += '/' + arguments[i];
                    }
                    return url;
                }

                return {
                    getInstallations: function() {
                        return $http.get(url('installations'))
                            .then(function(response) {
                                return response.data;
                            });
                    },

                    connect: function(chargerId) {
                        return $http.post(url('chargers', chargerId, 'connect'))
                            .then(function(response) {
                                return response.data;
                            });
                    },

                    disconnect: function(chargerId) {
                        return $http.post(url('chargers', chargerId, 'disconnect'))
                            .then(function(response) {
                                return response.data;
                            });
                    }
                };
            }
        ])
        // --------------------------------------------------------------------
        .controller('mainController', [
            '$scope', '$interval', 'apiService',
            function($scope, $interval, apiService) {
                function refreshInstallations() {
                    apiService.getInstallations()
                        .then(function(installations) {
                            $scope.installations = installations;
                        });
                }

                function connect(charger) {
                    return apiService.connect(charger.id);
                }

                function disconnect(charger) {
                    return apiService.disconnect(charger.id);
                }

                function updateCharger(charger) {
                    for(var i=0; i<$scope.installations.length; i++) {
                        if($scope.installations[i].id == charger.installationId) {
                            var chargers = $scope.installations[i].chargers;
                            for(var j=0; j<chargers.length; j++) {
                                if(chargers[j].id == charger.id) {
                                    chargers[j] = charger;
                                }
                            }
                        }
                    }
                }

                refreshInstallations();

                $scope.getCurrents = function(installation) {
                    var currents = {
                        allocated: 0,
                        charging: 0,
                        circuitBreaker: installation.circuitBreakerCurrent
                    };
                    installation.chargers.forEach(function(charger) {
                        currents.allocated += charger.allocatedCurrent;
                        if(charger.connectedVehicle) {
                            currents.charging += charger.connectedVehicle.chargeCurrent;
                        }
                    });
                    return currents;
                };

                $scope.toggleChargerConnected = function(charger) {
                    var promise = charger.connectedVehicle ? disconnect(charger) : connect(charger);
                    promise.then(function(charger) {
                        updateCharger(charger);
                    });
                };
            }
        ])
        ;
})();