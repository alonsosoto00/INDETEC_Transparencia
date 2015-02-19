arbolapp.directive('fileManager', function() {
  return {
    templateUrl: 'partials/file-manager.html',
    restrict: 'E',
      controller: function($scope,$http,keys) {
        console.log($scope.selected);
    }
  };
});
