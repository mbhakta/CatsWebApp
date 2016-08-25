catsWebApp.factory("catsWebAppService", ["$http", function ($http) {

    var service = {};

    service.generateResults = function () {
        return $http.get("/Home/GenerateResults");
    };
    return service;
}]);