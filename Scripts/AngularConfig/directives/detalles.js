arbolapp.directive('detalles', function() {
  return {
    templateUrl: 'partials/detalles.html',
    restrict: 'E',
      controller: function($scope,$http,keys) {
        console.log($scope.selected);
    }
  };
});