catsWebApp.controller("catsWebAppController", function ($scope, catsWebAppService) {
  
    // Extract the results from the C# controller and send it to UI for rendering
    function generateResults() {
        catsWebAppService.generateResults()
            .success(function (data) {
                $scope.data = data; 
            })
            .error(function (error) {
                $scope.status = "Unable to get data : " + error.message;
                console.log($scope.status);
            });
    }

    // Render on page init
    generateResults();
});

