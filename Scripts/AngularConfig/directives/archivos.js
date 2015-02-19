arbolapp.directive('archivos', function() {
  return {
    templateUrl: 'partials/archivos.html',
    restrict: 'E',
      controller: function($scope,$http,keys) {
        console.log($scope.selected);
    }
  };
});
