var diagramApp = angular.module("diagramModule", ['ngAnimate']);
diagramApp.directive("diagram", function () {
    return {
        link: function (scope, element, attributes) {
            scope.parameters = scope[attributes["diagram"]];
        },
        restrict: "A",
        template: function () {
            return angular.element(document.querySelector("#template")).html();
        }
    }
})
diagramApp.controller("diagramCtrl", function ($scope) {

    $scope.colors = ['teal', 'salmon', 'peach', 'lime', 'grape'];
    $scope.parameters = [];

    //get max value (max - 100% for height column)
    $scope.getMaxAmount = function () {
        var maxCount = 0;
        //serch max maount
        for (var i = 0; i < $scope.parameters.length; i++) {
            var amount = $scope.parameters[i].amount;
            if (amount > maxCount) { maxCount = amount };
        }
        return maxCount;
    };

    //calculation height for column depending on amount
    $scope.calcHeight = function (amount) {
        var maxAmount = $scope.getMaxAmount();
        var result = amount / (maxAmount / 100);
        //if value very small, return 16% height. For visible minimum column.
        return (result < 16) ? 16 : result;
    };

    //get color for new parameter 
    $scope.getColor = function (index) {
        //depending on the index of columns, compute index of colors.
        if (index >= 5) {
            index = index - (5 * Math.floor(index / 5));
        }
        return $scope.colors[index];
    };
});
