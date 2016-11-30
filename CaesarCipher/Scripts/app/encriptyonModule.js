var app = angular.module("encriptyonModule", ["diagramModule"]);
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
    EncryptService.tryDecrypt = function (postData) {
        return $http({
            url: '/Encrypt/tryDecryp',
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
    $scope.columns = [];

    //receive encrypted text from the server
    $scope.getEncryption = function (encState) {
        $scope.buildDiagram();

        var postData = {
            userText: $scope.userText,
            rotate: $scope.rotate,
            encrypt: encState //bool variable - select encrypt or decrypt text  (true - encrypt) 
        };

        EncryptService.getEncryptionText(postData)
        .success(function (textEnc) {
            //show processed text in textarea
            $scope.resultText = textEnc.replace(/\\n/g, '\n');
        });
    }

    //attempt rashifrovat text (successful - get rotation / fail - get -1)
    $scope.tryDecrypt = function () {
        var postData = {
            userEncryptText: $scope.userText
        };

        EncryptService.tryDecrypt(postData)
        .success(function (result) {
            alert(result);
        })
        .error(function (result) {
            alert("error " + result);
        });
    }

    $scope.buildDiagram = function () {
        if ($scope.userText.length) {
            var repeatSymbols = {};
            //count the number of repetitions (only english alphabet)
            for (var i = 0; i < $scope.userText.length; i++) {
                var symbol = $scope.userText[i];
                if (/[a-zA-Z]/.test(symbol)) {
                    repeatSymbols[symbol] = (isNaN(repeatSymbols[symbol]) ? 1 : repeatSymbols[symbol] + 1);
                }
            }
            //clear data for coluns diagram and add new data
            $scope.columns.length = 0;
            for (key in repeatSymbols) {
                $scope.columns.push({ parameter: key, amount: repeatSymbols[key] });
            }
        }
        
    };

    //clear user text and data for diagram
    $scope.clearData = function () {
        $scope.userText = null;
        $scope.columns.length = 0;
    };
});