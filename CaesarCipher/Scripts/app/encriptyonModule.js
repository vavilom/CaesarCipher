var app = angular.module("encriptyonModule", []);
app.factory('EncryptService', ['$http', function ($http) {
    //send text on the server, and get encrypt/decrypt result
    var EncryptService = {};
    EncryptService.getEncryptionText = function (postData) {
        return $http({
            url: '/Encrypt/GetEncryption',
            method: "POST",
            params: postData
        });
    };
    return EncryptService;
}]);
app.controller("encriptyonCtrl", function ($scope, EncryptService) {
    $scope.repeatSymbols = {};
    //view data
    $scope.rotate = 0;
    $scope.userText = "";
    $scope.resultText = "";

    //receive encrypted text from the server
    $scope.getEncryption = function (encState) {
        var postData = {
            userText: $scope.userText,
            rotate: $scope.rotate,
            encrypt: encState //bool variable - select encrypt or decrypt text  (true - encrypt) 
        };
        EncryptService.getEncryptionText(postData)
        .success(function (textEnc) {
            console.log(textEnc);
            //show processed text in textarea
            $scope.resultText = textEnc.substring(1, textEnc.length - 1).replace(/\\n/g, '\n');
        });
    }

    $scope.buildDiagram = function () {
        $scope.repeatSymbols = {};

        for (var i = 0; i < $scope.userText.length; i++) {
            var symbol = $scope.userText[i];
            if (symbol == ' ') continue;
            $scope.repeatSymbols[symbol] = (isNaN($scope.repeatSymbols[symbol]) ? 1 : $scope.repeatSymbols[symbol] + 1);
        }

    };

    $scope.test = function () {
        $scope.symbols['b'] += 5;
        $scope.symbols['b']++;
        $scope.symbols['c'] = (isNaN($scope.symbols['c']) ? 1 : $scope.symbols['c'] + 1);
        $scope.symbols['c']++;
        console.log($scope.symbols);
    };
});