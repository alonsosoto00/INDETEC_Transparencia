var arbolapp = angular.module('treeApp', ['ui.tree', 'ui.router', 'angularFileUpload'])
  .factory('httpRequestInterceptor',function(){
    return{
      request:function(config){
        config.headers = {'Content-Type': 'application/json; charset=utf-8'}
        return config;
      }
    }
  })
