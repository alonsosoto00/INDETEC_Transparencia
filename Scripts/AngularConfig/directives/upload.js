/*arbolapp.directive('uploader', [
    function () {
        return {
            restrict: 'E',
            scope: {
                action: '='
            },
            controller: ['$scope',
                function ($scope) {
                    $scope.uploaded = "";
                    $scope.$on('filesUpdated', function (event, args) {
                        var formData = new FormData();
                        for (var i = 0; i < $scope.fileread.length; i++) {
                            var file = $scope.fileread[i];
                            formData.append('files[]', file, file.name);
                        }
                        var xhr = new XMLHttpRequest();
                        xhr.open('POST', 'Default.aspx/fn_CargaArchivos', true);
                        xhr.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
                        xhr.addEventListener("progress", updateProgress, false);
                        function updateProgress(evt) {
                            if (evt.lengthComputable) {
                                $scope.$apply(function () {
                                    $scope.percentComplete = evt.loaded / evt.total;
                                });
                            } else {
                                $scope.$apply(function () {
                                    $scope.percentComplete = 50;
                                });
                            }
                        }
                        xhr.onload = function () {
                            if (xhr.status === 200) {
                                var response = JSON.parse(xhr.response).files;
                                var toEmit = [];
                                for (var i = 0; i < response.length; i++) {
                                    var nombre = response[i].split("/");
                                    toEmit = {
                                        'url': response[i],
                                        'nombre': nombre[nombre.length - 1]
                                    };
                                }
                                $scope.$emit('pushFile', toEmit);
                            } else {
                                console.log('An error occurred!');
                            }
                        };
                        xhr.send(formData);
                    });
                }
            ],
            link: function (scope, elem, attrs, ctrl) {
                angular.element(elem[0].querySelector('input[type="file"]')).bind("change", function (changeEvent) {
                    scope.$apply(function () {
                        scope.$emit("message", "Se esta subiendo");
                        scope.fileread = changeEvent.target.files;
                    });
                    scope.$emit('filesUpdated');
                });
                scope.$on('pushFile', function (event, args) {
                    scope.$apply(function () {
                        scope.uploaded = args;
                        scope.$emit('uploadedFiles', scope.uploaded);
                    });
                });
            },
            templateUrl: 'partials/uploader.html'
        };
    }])*/