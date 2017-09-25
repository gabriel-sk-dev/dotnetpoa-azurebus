(function () {
    'use strict';

    angular
        .module('common.request')
        .provider('ssmartRequesting', ssmartRequestingProvider);

    function ssmartRequestingProvider() {        

        return {
            $get: requestService
        };

        function requestService($http, $q, BASE_URL_API) {
            return {
                'getBaseUrl': getBaseUrl,
                'get': get,
                'post': post,
                'put': put,
                'delete': _delete,
                'patch': patch
            };

            function getBaseUrl() {
                var baseUrl = "http://" + BASE_URL_API;
                return baseUrl;
            }

            function get(url, config) {
                var completeUrl;
                if (config && config.url)
                    completeUrl = config.url;
                else
                    completeUrl = getBaseUrl() + url;
                var promise = $http.get(completeUrl);

                if (config && config.returnStatusCode)
                    return promise;

                var deferred = $q.defer();
                promise.then(function (response) {
                    if (response.status === 204) {
                        console.log("Result: 204 - No content - [modificado para sucesso - está em teste]");
                        deferred.resolve(null); /*deferred.reject({ error: "No Content"});*/
                    }
                    deferred.resolve(response.data);
                }, function (error) {
                    deferred.reject(error.data);
                });

                return deferred.promise;
            }

            function post(url, data, config) {
                var completeUrl;
                if (config && config.url)
                    completeUrl = config.url;
                else
                    completeUrl = getBaseUrl() + url;

                var promise = $http.post(completeUrl, JSON.stringifyOnce(data), {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                });

                if (config && config.returnStatusCode)
                    return promise;
                
                var deferred = $q.defer();
                promise.then(function (response) {
                    deferred.resolve(response.data);
                }, function (error) {
                    deferred.reject(error.data);
                });

                return deferred.promise;
            }

            function put(url, data, config) {
                var completeUrl = getBaseUrl() + url;
                var promise = $http.put(completeUrl, JSON.stringifyOnce(data), {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                });

                if (config && config.returnStatusCode)
                    return promise;

                var deferred = $q.defer();
                promise.then(function (response) {
                    deferred.resolve(response.data);
                }, function (error) {
                    deferred.reject(error.data);
                });

                return deferred.promise;
            }

            function _delete(url, config) {
                var completeUrl = getBaseUrl() + url;
                var promise = $http.delete(completeUrl);

                if (config && config.returnStatusCode)
                    return promise;

                var deferred = $q.defer();
                promise.then(function (response) {
                    deferred.resolve(response.data);
                }, function (error) {
                    deferred.reject(error.data);
                });

                return deferred.promise;
            }

            function patch(url, data, config) {
                var completeUrl = getBaseUrl() + url;
                var promise = $http.patch(completeUrl, JSON.stringifyOnce(data), {
                    headers: {
                        'Content-Type': 'application/json'
                    }
                });

                if (config && config.returnStatusCode)
                    return promise;

                var deferred = $q.defer();
                promise.then(function (response) {
                    deferred.resolve(response.data);
                }, function (error) {
                    deferred.reject(error.data);
                });

                return deferred.promise;
            }
        }
    }

    /* A função JSON.stringify foi re-escripta para usar o JSON.stringifyOnce para evitar o erro de conversão circular */
    JSON.stringifyOnce = function (obj, replacer, indent) {
        var printedObjects = [];
        var printedObjectKeys = [];

        function printOnceReplacer(key, value) {
            if (printedObjects.length > 2000) { // browsers will not print more than 20K, I don't see the point to allow 2K.. algorithm will not be fast anyway if we have too many objects
                return 'object too long';
            }
            var printedObjIndex = false;
            printedObjects.forEach(function (obj, index) {
                if (obj === value) {
                    printedObjIndex = index;
                }
            });

            if (key == '') { //root element
                printedObjects.push(obj);
                printedObjectKeys.push("root");
                return value;
            }

            else if (printedObjIndex + "" != "false" && typeof (value) == "object") {
                if (printedObjectKeys[printedObjIndex] == "root") {
                    return "(pointer to root)";
                } else {
                    return "(see " + ((!!value && !!value.constructor) ? value.constructor.name.toLowerCase() : typeof (value)) + " with key " + printedObjectKeys[printedObjIndex] + ")";
                }
            } else {

                var qualifiedKey = key || "(empty key)";
                printedObjects.push(value);
                printedObjectKeys.push(qualifiedKey);
                if (replacer) {
                    return replacer(key, value);
                } else {
                    return value;
                }
            }
        }
        return JSON.stringify(obj, printOnceReplacer, indent);
    };

})();
