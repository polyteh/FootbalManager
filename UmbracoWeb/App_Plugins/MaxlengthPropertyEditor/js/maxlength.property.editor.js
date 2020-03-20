angular.module('umbraco').controller('MaxlengthPropertyEditorController', function($scope, assetsService) {
    if($scope.model.value == undefined) {
        $scope.model.count = ($scope.model.config.nochars * 1);
    } else {
        $scope.model.count = ($scope.model.config.nochars * 1) - $scope.model.value.length;
    }

    $scope.model.change = function() {
        if($scope.model.value == undefined) {
            $scope.model.count = ($scope.model.config.nochars * 1);
        } else {
            $scope.model.count = ($scope.model.config.nochars * 1) - $scope.model.value.length;
        }
        if($scope.model.count < 0) {
            $scope.model.value = $scope.model.value.substring(0, ($scope.model.config.nochars * 1));
        }
    }
});