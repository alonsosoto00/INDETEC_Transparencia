arbolapp.directive('descripcion', function() {
  return {
    templateUrl: 'partials/descripcion.html',
    restrict: 'E',
      controller: function($scope,$http,keys) {
        console.log($scope.selected);
    }
  };
});
