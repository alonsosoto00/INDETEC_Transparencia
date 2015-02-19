function showActions(padre) {
    // console.log(padre.childNodes[1].childNodes[0].childNodes[7]);

    padre.childNodes[1].childNodes[0].childNodes[7].style.visibility = "visible";
    padre.childNodes[1].childNodes[0].childNodes[9].style.visibility = "visible";
}

function hideActions(padre) {
    padre.childNodes[1].childNodes[0].childNodes[7].style.visibility = "hidden";
    padre.childNodes[1].childNodes[0].childNodes[9].style.visibility = "hidden";

}

arbolapp.controller('MyContoller', ['$scope', '$upload',
    function ($scope, $upload) {
        //$scope.$watch('files', function () {
        //    $scope.upload($scope.files);
        //});

        $scope.nuevo = {
            descripcion: "",
            fecha_inicio: "",
            fecha_fin: ""
        };


        $scope.next = function () {
            document.getElementById("divupload").style.display = "inline";
            document.getElementById("divdatos").style.display = "none";
            document.getElementById("back").style.display = "inline";
            document.getElementById("next").style.display = "none";
        }
        $scope.back = function () {
            document.getElementById("divupload").style.display = "none";
            document.getElementById("divdatos").style.display = "inline";
            document.getElementById("back").style.display = "none";
            document.getElementById("next").style.display = "inline";
        }

        $scope.upload = function (files) {
            var ini = "";
            var fin = "";
            $scope.nuevo.files = files;
            try {
                var i = $scope.nuevo.fecha_inicio;
                ini = i.getFullYear() + "-" + (i.getMonth() + 1) + "-" + i.getDate();
            } catch (error) {}
            try {
                var f = $scope.nuevo.fecha_fin;
                fin = f.getFullYear() + "-" + (f.getMonth() + 1) + "-" + f.getDate();
            } catch (error) {}
            if (files && files.length) {
                for (var i = 0; i < files.length; i++) {
                    var file = $scope.nuevo.files[i];
                    file.status = "Iniciando..."
                    $upload.upload({
                        url: 'UploadHandler.ashx',
                        data: '{CTR_MR_MapeoId: "' + $scope.selected.id + '",CTR_DescripcionArchivo: "' + $scope.nuevo.descripcion + '",CTR_FechaInicio: "' + ini + '",CTR_FechaFin: "' + fin + '"}',
                        file: file
                    }).progress(function (evt) {
                        var progressPercentage = parseInt(100.0 * evt.loaded / evt.total);
                        file.status = progressPercentage + '% ';
                        console.log('progress: ' + progressPercentage + '% ' + evt.config.file.name);
                    }).success(function (data, status, headers, config) {
                        file.status = 'Cargado';
                        console.log('file ' + config.file.name + 'uploaded. Response: ' + data);
                    }).error(function (data, status, headers, config) {
                        file.status = 'Fallo';
                    });
                }
            }
        };
    }
]);
