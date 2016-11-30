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
    $scope.thoughts = [
        "Type the encrypted text, and I will try decrypt it.",
        "Unfortunately i don't know!",
        "Please wait I am thinking.",
        "I know! It is "
    ];
    $scope.think = $scope.thoughts[0];
    $scope.waiting = false;

    //receive encrypted text from the server
    $scope.getEncryption = function (encState) {
        if (!$scope.userText || !$scope.rotate) {
            $scope.think = "Type the encrypted text, and rotate number maste be from 0 to 26.";
            return;
        }
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
        if ($scope.userText) {
            $scope.waiting = true;
            $scope.think = $scope.thoughts[2];
            var postData = {
                userEncryptText: $scope.userText
            };

            EncryptService.tryDecrypt(postData)
            .success(function (result) {
                if (result >= 0) {
                    $scope.think = $scope.thoughts[3] + " " + result + "!";
                }
                else {
                    $scope.think = $scope.thoughts[1];
                }
                $scope.waiting = false;
            })
            .error(function (result) {
                $scope.think = "Error! I am sory!";
                $scope.waiting = false;
            });
        }
        else {
            $scope.think = "You did not type text."
        }
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
        $scope.think = $scope.thoughts[0];
    };
});