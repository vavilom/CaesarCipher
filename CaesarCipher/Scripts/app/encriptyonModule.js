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
});