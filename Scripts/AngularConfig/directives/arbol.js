arbolapp.directive('arbol', function() {
    return {
        templateUrl: 'partials/arbol.html',
        restrict: 'E',
        controller: function($scope, $http, keys) {
            $scope.debug = {};
            $scope.selected = {
                id: null
            };
            $scope.editing = false;
            // Parameters
            $scope.parameters = {
                dragEnabled: true,
                emptyPlaceholderEnabled: false,
                maxDepth: 10,
                dragDelay: 0,
                dragDistance: 0,
                lockX: false,
                lockY: false,
                boundTo: '',
                spacing: 20,
                coverage: 50,
                cancelKey: 'esc',
                copyKey: 'shift',
                selectKey: 'ctrl',
                enableExpandOnHover: true,
                expandOnHover: 500,
            };

            $scope.keys = keys;
            var contador = 0;

            $scope.fnMuestraInput = function(scope) {
                var indexNode = $scope.fnGetIndexNode(scope.$modelValue.id);
                if (indexNode != -1) {
                    var txt = document.getElementById('txtNodo' + indexNode);
                    var lbl = document.getElementById('lblNodo' + indexNode);
                    if (txt != null && lbl != null) {
                        txt.value = lbl.innerText;
                        lbl.style.display = 'none';
                        txt.style.display = 'block';
                        txt.focus();
                        txt.setSelectionRange(0, txt.value.length);
                    }
                }
            }

            $scope.fnOnBlurTxtNodo = function(scope) {
                var indexNode = $scope.fnGetIndexNode(scope.$modelValue.id);
                if (indexNode != -1) {
                    var txt = document.getElementById('txtNodo' + indexNode);
                    var lbl = document.getElementById('lblNodo' + indexNode);
                    if (txt != null && lbl != null) {
                        if (txt.value != null && txt.value.trim() != "") {
                            scope.$modelValue.title = txt.value;
                            lbl.innerText = txt.value;
                            $scope.updateNodeFromServer(scope);
                        }
                        lbl.style.display = 'block';
                        txt.style.display = 'none';
                    }
                }
            }

            $scope.fnMuestraLabelF2 = function(event, scope) {
                //alert("Se presionó la tecla " + event.keyCode);
            }

            $scope.fnGetIndexNode = function(nodeID) {
                //Inicializamos el contador
                contador = 0;
                //Comenzamos a Iterar
                for (var i = 0; i < $scope.data.length; i++) {
                    //Incrementamos el Contador
                    contador += 1;
                    //Obtenemos el Id del nodo que estamos evaluando
                    var nodoId = $scope.data[i].id;
                    //Si el ID que estamos buscando, regresamos true
                    if (nodoId == nodeID) {
                        encontrado = true;
                        break;
                    }
                    //De lo contrario, buscamos en los nodos hijos del nodo (si los tuviera)
                    else {
                        var encontrado = $scope.fnGetIndexNestedNode(nodeID, $scope.data[i].nodes);
                        if (encontrado)
                            break;
                    }
                }
                return encontrado ? contador : -1;
            }

            $scope.fnGetIndexNestedNode = function(nodeID, nodes) {
                for (var i = 0; i < nodes.length; i++) {
                    //Incrementamos el Contador
                    contador += 1;
                    //Obtenemos el Id del nodo que estamos evaluando
                    var nodoId = nodes[i].id;
                    //Si el ID que estamos buscando, regresamos true
                    if (nodoId == nodeID)
                        return true;
                    //De lo contrario, buscamos en los nodos hijos del nodo (si los tuviera)
                    else {
                        var encontrado = $scope.fnGetIndexNestedNode(nodeID, nodes[i].nodes);
                        if (encontrado)
                            return true;
                    }
                }
                return false;
            }

            $scope.removeThisNode = function(scope) {
                //Eliminamos el nodo del Arbol
                var removed = scope.remove();
                //Si se pudo eliminar el nodo del Arbol
                if (removed.$$state.status == 1) {
                    $scope.editing = false;
                    //Lo eliminamos tambien de la BD
                    $scope.removeNodeFromServer(scope.$modelValue.id);
                    //Y Recargamos el Arbol
                    $scope.llenaArbol();
                }
            };

            $scope.removeNodeFromServer = function(nodoId) {
                if(nodoId == "newnode"){
                    $scope.llenaArbol();
                }else if(nodoId != null) {
                    $http({
                        method: 'POST',
                        url: 'Admin.aspx/fn_EliminaNodo',
                        data: '{nodoId: "' + nodoId + '"}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                    }).success(function(data, status) {}).error(function(data, status) {
                        alert("No se pudo eliminar el nodo");
                    });
                }
                console.log(nodoId);
            };

            $scope.updateNodeFromServer = function(scope) {
                if (scope.$modelValue != null) {
                    var nodo = {};
                    nodo.CT_NodoId = scope.$modelValue.id;
                    if (nodo.CT_NodoId == "newnode") {
                        return;
                    }
                    nodo.CT_Descripcion = scope.$modelValue.title;
                    nodo.CT_NodoPadreId = scope.depth() == 1 ? null : scope.$parentNodesScope.$parent.$modelValue.id;
                    //Actualizamos el nodo en la BD
                    $http({
                        method: 'POST',
                        url: 'Admin.aspx/fn_ActualizaDescripcionNodo',
                        // data: {nodoJSON: JSON.stringify(nodo)},
                        data: '{nodoId:"' + nodo.CT_NodoId + '", descripcion: "' + nodo.CT_Descripcion + '"}',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                    }).success(function(data, status) {
                        $scope.llenaArbol();
                    }).error(function(data, status) {
                        alert("No se pudo actualizar el nodo");
                        $scope.llenaArbol();
                    });
                }
            }


            var keepGoing;
            var padre = null;
            var ordenNodo = 0;
            var ordenNodoAux = 0;

            var dameData = function(nodo) {
                for (var c = 0; c < $scope.data.length; c++) {
                    var elemento = $scope.data[c];
                    var nivel = 0;
                    console.log(nivel, elemento.id);
                    if (elemento.id == nodo) {
                        return {
                            padre: null,
                            orden: c + 1
                        };
                    } else if (elemento.nodes.length) {
                        var data = dameDataAux(nodo, elemento, nivel + 1);
                        if (data !== undefined)
                            return data;
                    }
                }
                return "no lo encontre";
            };

            var dameDataAux = function(hijo, iterador, nivel) {
                for (var c = 0; c < iterador.nodes.length; c++) {
                    var elemento = iterador.nodes[c];
                    console.log(nivel, elemento.id);
                    if (elemento.id == hijo) {
                        return {
                            padre: iterador.id,
                            orden: c + 1
                        };
                    } else if (elemento.nodes.length) {
                        var data = dameDataAux(hijo, elemento, nivel + 1);
                        if (data !== undefined)
                            return data;
                    }
                }
            };

            $scope.updateNodePositionFromServer = function(scope) {
                if (scope != null) {
                    var nodoPos = {};

                    nodoPos.CT_NodoId = scope.$modelValue.id;
                    nodoPos.CT_NodoPadreId = dameData(nodoPos.CT_NodoId).padre;
                    nodoPos.CT_Orden = dameData(nodoPos.CT_NodoId).orden;

                    // alert(ordenNodo + '   '+ordenNodoAux);
                    console.log(nodoPos);
                    //Actualizamos el nodo en la BD
                    $http({
                        method: 'POST',
                        url: 'Admin.aspx/fn_actualizaPosicionNodo',
                        data: {
                            nodoJSON: JSON.stringify(nodoPos)
                        },
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                    }).success(function(data, status) {
                        $scope.llenaArbol();
                    }).error(function(data, status) {
                        alert("No se pudo actualizar el nodo");
                        $scope.llenaArbol();
                    });
                }
            }

            $scope.getRanges = function() {
                $http({
                    method: 'POST',
                    url: 'Admin.aspx/fn_GetControlMaestroTiposRangos',
                    data: '',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                }).success(function(data, status) {
                    $scope.rangos = angular.fromJson(data.d);
                    console.log($scope.rangos);

                }).error(function(data, status) {
                    //alert("No hay descripcion..");
                    //$scope.getRanges();
                });
            };

            $scope.showData = function(id) {

                var nodo = {};

                nodo.nodoId = id;
                nodo.mostrarFiltrados = false;

                $scope.selected = {
                    id: id,
                    filesNodo: 0,
                    textoNodo: "cargando..."
                };

                $http({
                    method: 'POST',
                    url: 'Admin.aspx/fn_GetListadoArchivosNodo',
                    // data: JSON.stringify(nodo),
                    data: '{nodoId:"'+id+'", mostrarFiltrados:'+ false +'}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                }).success(function(data, status) {
                    $scope.selected.filesNodo = angular.fromJson(data.d);
                    // console.log($scope.selected.filesNodo);
                }).error(function(data, status) {
                    //alert("No hay archivos..");
                });

                $http({
                    method: 'POST',
                    url: 'Admin.aspx/fn_GetDatosNodo',
                    data: '{nodoId:"' + id + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                }).success(function(data, status) {
                    $scope.selected.textoNodo = angular.fromJson(data.d);
                    console.log($scope.selected.textoNodo);
                }).error(function(data, status) {
                    //alert("No hay descripcion..");
                });
                // console.log($scope.selected);
            }


            $scope.toggle = function(scope) {
                scope.toggle();
            };

            $scope.moveLastToTheBeginning = function() {
                var a = $scope.data.pop();
                $scope.data.splice(0, 0, a);
            };

            $scope.updateTextoNodo = function(update) {

                if (update) return; // son iguales, no es necesario actualizar

                var id = $scope.selected.id,
                description = $scope.selected.textoNodo;

                $http({
                    method: 'POST',
                    url: 'Admin.aspx/fn_ActualizaTextoNodo',
                    data: '{nodoId:"'+description.CT_NodoId+'", texto:"'+ description.CT_Texto +'"}',
                }).success(function(data, status) {
                    // success
                    // console.log("exito");
                }).error(function(data, status) {
                    alert("No se pudo actualizar el texto");
                });
            };

            $scope.updatePrefijoNodo = function (update) {

                if (update) return; // son iguales, no es necesario actualizar

                var id = $scope.selected.id,
                description = $scope.selected.textoNodo;

                $http({
                    method: 'POST',
                    url: 'Admin.aspx/fn_ActualizaPrefijoNodo',
                    data: '{nodoId:"' + description.CT_NodoId + '", prefijo:"' + description.MR_Prefijo + '"}',
                }).success(function (data, status) {
                    // success
                    // console.log("exito");
                }).error(function (data, status) {
                    alert("No se pudo actualizar el prefijo");
                });
            };

            $scope.updateAplicaNodo = function () {
                var id = $scope.selected.id,
                description = $scope.selected.textoNodo;

                $http({
                    method: 'POST',
                    url: 'Admin.aspx/fn_ActualizaAplicaNodo',
                    data: '{nodoId:"' + description.CT_NodoId + '", aplica:"' + description.CT_Aplica + '"}',
                }).success(function (data, status) {
                    // success
                    // console.log("exito");
                }).error(function (data, status) {
                    alert("No se pudo aplicar el cambio");
                });
            };

            $scope.newSubItem = function(scope) {
                // this.expandAll() // solucion por ahora
                var nodeData = scope.$modelValue;

                $scope.editing = true;
                // $scope.debug.newnode = nodeData;
                // console.log(scope.$modelValue);

                nodeData.nodes.push({
                    id: "newnode",
                    title: ' ',
                    nodes: []
                });

                window.setTimeout(function() {
                    var label = document.getElementById('newnode').childNodes[3];
                    var text = document.getElementById('newnode').childNodes[5];

                    // console.log("this:",document.getElementById('newnode').childNodes);

                    text.addEventListener("blur", function(e) {
                        if (text.value == '' || text.value == null) {
                            e.preventDefault();
                            window.setTimeout(function() {
                                label.style.display = 'none';
                                text.style.display = 'block';
                                text.focus();
                            }, 50);
                        } else {
                            var nuevoNodo = {};
                            nuevoNodo.CT_NodoPadreId = nodeData.id;
                            nuevoNodo.CT_Descripcion = text.value;
                            nuevoNodo.CT_Orden = nodeData.nodes.length;
                            console.log(nuevoNodo);
                            $http({
                                method: 'POST',
                                url: 'Admin.aspx/fn_GuardaNuevoNodo',
                                data: {
                                    nodoJSON: JSON.stringify(nuevoNodo)
                                },
                            }).success(function(data, status) {
                                $scope.llenaArbol();
                            }).error(function(data, status) {
                                $scope.llenaArbol();
                            });
                        }
                    }, true);

                    label.style.display = 'none';
                    text.style.display = 'block';
                    text.focus();
                }, 100);
            };

            $scope.newItem = function(scope) {

                $scope.editing = true;

                $scope.data.push({
                    id: "newnode",
                    title: ' ',
                    nodes: []
                });

                window.setTimeout(function() {
                    var label = document.getElementById('newnode').childNodes[3];
                    var text = document.getElementById('newnode').childNodes[5];

                    text.addEventListener("blur", function(e) {
                        if (text.value == '' || text.value == null) {
                            e.preventDefault();
                            window.setTimeout(function() {
                                label.style.display = 'none';
                                text.style.display = 'block';
                                text.focus();
                            }, 50);
                        } else {
                            var nuevoNodo = {};
                            nuevoNodo.CT_NodoPadreId = null;
                            nuevoNodo.CT_Descripcion = text.value;
                            nuevoNodo.CT_Orden = $scope.data.length;
                            console.log(nuevoNodo);
                            $scope.editing = false;
                            $http({
                                method: 'POST',
                                url: 'Admin.aspx/fn_GuardaNuevoNodo',
                                data: {
                                    nodoJSON: JSON.stringify(nuevoNodo)
                                },
                            }).success(function(data, status) {
                                $scope.llenaArbol();
                            }).error(function(data, status) {
                                $scope.llenaArbol();
                            });
                        }
                    }, true);

                    label.style.display = 'none';
                    text.style.display = 'block';
                    text.focus();
                }, 100);
            };

            $scope.collapseAll = function() {
                $scope.$broadcast('collapseAll');
            };

            $scope.expandAll = function() {
                $scope.$broadcast('expandAll');
            };

            $scope.llenaArbol = function() {
                $http({
                    method: 'POST',
                    url: 'Admin.aspx/fn_FormaArbol',
                    data: ''
                }).success(function(data, status) {
                    $scope.editing = false;

                    var arrayArbol = data.d.replace(/"Id":/g, '"id":');
                    arrayArbol = arrayArbol.replace(/"Title":/g, '"title":');
                    arrayArbol = arrayArbol.replace(/"Nodes":/g, '"nodes":');

                    $scope.data = angular.fromJson(arrayArbol);
                    console.log(arrayArbol);
                }).error(function(data, status) {
                    console.log(data);
                    alert("Error al cargar el Árbol");
                })
            };

            //LLenamos el Arbol
            $scope.llenaArbol();
            //Cargamos el combo de rangos
            $scope.getRanges();
        }
    };
});
