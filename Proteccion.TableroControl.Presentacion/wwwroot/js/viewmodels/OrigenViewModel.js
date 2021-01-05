/// <reference path="../../lib/knockout/knockout-3.4.2.js" />

function Parametro(config) {
    var self = this;
    var inicializar = !config;

    self.id = ko.observable(!inicializar ? config.id : "");
    self.nombre = ko.observable(!inicializar ? config.nombre : "");
    self.valor = ko.observable(!inicializar ? config.valor : "");
    self.dinamico = ko.observable(!inicializar ? config.dinamico : false);
    self.tipoDato = ko.observable(!inicializar ? config.tipoDato : "");
    self.orden = ko.observable(!inicializar ? config.orden : 0);
    self.esFechaProceso = ko.observable(!inicializar ? config.esFechaProceso : false);
}
function Campo(config) {

    var self = this;
    var inicializar = !config;
    self.id = ko.observable(!inicializar ? config.id : "");
    self.nombre = ko.observable(!inicializar ? config.nombre : "");
    self.tipoDato = ko.observable(!inicializar ? config.tipoDato : "");
    self.nombreExcel = ko.observable(!inicializar ? config.nombreExcel : "");
    self.posicionInicial = ko.observable(!inicializar ? config.posicionInicial: 0);
    self.longitudCampo = ko.observable(!inicializar ? config.longitudCampo: 0);
    self.eliminarCamposBlanco = ko.observable(!inicializar ? config.eliminarCamposBlanco : false);
    self.orden = ko.observable(!inicializar ? config.orden : 0);
}


function OrigenViewModel(model) {

    //Se guarda el contexto del viewmodel
    var self = this;
    //Información basica
    self.idOrigenDato = ko.observable(model.idOrigenDato);
    self.tipoOrigen = ko.observable(model.idTipoOrigen);
    self.nombre = ko.observable(model.nombre);
    self.descripcion = ko.observable(model.descripcion);
    self.activo = ko.observable(model.activo);

    //Origen Excel, CSV y longitud fija
    self.rutaArchivo = ko.observable(model.rutaArchivo);
    self.nombreArchivo = ko.observable(model.nombreArchivo);
    self.filaEncabezado = ko.observable(model.filaEncabezado);
    self.eliminarFila = ko.observable(model.eliminarFila);
    self.concatenarFecha = ko.observable(model.concatenarFecha);
    self.separador = ko.observable(model.separador);
    self.esOrigenSftp = ko.observable(false);
    self.rutaOrigenSftp = ko.observable(model.rutaOrigenSftp);
    self.rutaDestinoSftp = ko.observable(model.rutaDestinoSftp);

    self.lineaInicioLectura = ko.observable(model.lineaInicioLectura);
    self.columnaInicioLectura = ko.observable(model.columnaInicioLectura);

    //Origen BD
    self.procedimiento = ko.observable(model.procedimiento);
    self.esquemaProcedimiento = ko.observable(model.esquemaProcedimiento);
    self.nombreParametro = ko.observable("");
    self.valorParametro = ko.observable("");
    self.tipoCampoParametro = ko.observable("");
    self.dinamico = ko.observable(false);
    self.esFechaProceso = ko.observable(false);

    //Configuracion destino
    self.nombreTabla = ko.observable(model.nombreTabla);
    self.esquemaTabla = ko.observable(model.esquemaTabla);
    self.nombreCampo = ko.observable("");
    self.tipoCampo = ko.observable("");
    self.nombreExcel = ko.observable("");
    self.modoLectura = ko.observable(false);
    self.itemEliminar = ko.observable();
    self.eliminarCampo = ko.observable(false);


    self.posicionInicial = ko.observable(0);
    self.longitudCampo = ko.observable(0);
    self.eliminarCamposBlanco = ko.observable(false);


    self.habilitarParametros = ko.pureComputed(function () {
        return self.esFechaProceso() === false;
    }, self);

    self.soloLectura = ko.pureComputed(function () {
        return accion === "Detalle" || self.modoLectura();
    }, self);

    self.habilitarTipo = ko.pureComputed(function () {
        return accion === "Editar";
    }, self);

    self.habilitarExcel = ko.pureComputed(function () {
        return self.tipoOrigen() === "3" || self.tipoOrigen() === "16" || self.tipoOrigen() === "20" || self.tipoOrigen() === "22";
    }, self);    

    self.habilitarLongitudFija = ko.pureComputed(function () {
        return self.tipoOrigen() === "20" || self.tipoOrigen() === "22";
    }, self);  

    self.habilitarSeparador = ko.pureComputed(function () {
        return self.tipoOrigen() === "16" || self.tipoOrigen() === "22";
    }, self);  

    self.habilitarBD = ko.pureComputed(function () {
        return self.tipoOrigen() === "4";
    }, self);

    self.habilitarBotonCampo = ko.pureComputed(function () {

        if (self.tipoOrigen() === "3") {
            return self.nombreCampo() !== "" && self.nombreExcel() !== "";
        }
        else {
            return self.nombreCampo() !== "";
        }
    }, self);

    self.habilitarBotonParametro = ko.pureComputed(function () {
        return (self.nombreParametro() !== "" && self.valorParametro() !== "") || self.esFechaProceso();
    }, self);

    self.parametros = ko.observableArray(ko.utils.arrayMap(model.parametros, function (parametro) {
        return new Parametro(parametro);
    }));

    self.habilitarCampoDecimal = ko.pureComputed(function () {
        return self.tipoOrigen() === "20" && self.tipoCampo() === "FLOAT";
    }, self);

    self.campos = ko.observableArray(ko.utils.arrayMap(model.campos, function (campo) {
        return new Campo(campo);
    }));

    self.addParametro = function () {

        let elemento = {
            nombre: self.nombreParametro(),
            valor: self.valorParametro(),
            dinamico: self.dinamico(),
            tipoDato: self.tipoCampoParametro(),
            esFechaProceso: self.esFechaProceso()
        };
        self.parametros.push(new Parametro(elemento));
        self.nombreParametro("");
        self.valorParametro("");
        self.tipoCampoParametro("INT");
        self.dinamico(false);
        self.esFechaProceso(false);
        $("#modalCrudFiltros").modal("hide");
    };

    self.validarCampos = function () {
        if (self.longitudCampo() < 1 && self.tipoOrigen() === "20") {
            alertify.error("La longitud para el campo registrada no es válida");
            return true;
        }
    }

    self.addCampo = function () {
        let elemento = {
            nombre: self.nombreCampo(),
            tipoDato: self.tipoCampo(),
            nombreExcel: self.nombreExcel(),
            posicionInicial: self.posicionInicial(),
            longitudCampo: self.longitudCampo(),
            eliminarCamposBlanco: self.eliminarCamposBlanco()

        };
        if (self.validarCampos()) {
            return false;
        }
        else {
            self.campos.push(new Campo(elemento));
            self.nombreCampo("");
            self.tipoCampo("INT");
            self.nombreExcel("");
            self.posicionInicial(0);
            self.longitudCampo(0);
            self.eliminarCamposBlanco(false);

            $("#modalCrudCampos").modal("hide");
        }

    };

    self.pedirConfirmacionCampo = function (item) {
        $("#modalEliminar").modal("show");
        self.itemEliminar(item);
        self.eliminarCampo(true);
    };

    self.pedirConfirmacionParametro = function (item) {
        $("#modalEliminar").modal("show");
        self.itemEliminar(item);
        self.eliminarCampo(false);
    };

    self.eliminarDato = function () {

        let item = self.itemEliminar();
        if (self.eliminarCampo() === true) {
            self.campos.remove(item);
        } else {
            self.parametros.remove(item);
        }

        $("#modalEliminar").modal("hide");
    };

    self.addFechaProceso = function () {
        if (self.esFechaProceso()) {
            self.nombreParametro("FechaProceso");
            self.tipoCampoParametro("DATETIME");
            self.valorParametro("-");
        } else {
            self.nombreParametro("");
            self.tipoCampoParametro("INT");
            self.valorParametro("");
        }

    };

    self.guardar = function () {

        var continuar = true;

        $(".requerido").each(function () {
            if (continuar) {
                continuar = $(this).requeridoKO();
            } else {
                $(this).requeridoKO();
            }
        });

        if (!continuar) return false;

        //Si el origen es Excel se valida la extension del archivo
        if (self.tipoOrigen() === "3" || self.tipoOrigen() === "16") {

            var extensiones_permitidas = [".xls", ".xlsx"];

            if (self.tipoOrigen() === "16") {
                extensiones_permitidas = [".csv"];
            }

            var extension = (self.nombreArchivo().substring(self.nombreArchivo().lastIndexOf("."))).toLowerCase();

            var permitida = false;
            for (var i = 0; i < extensiones_permitidas.length; i++) {
                if (extensiones_permitidas[i] === extension) {
                    permitida = true;
                    break;
                }
            }

            if (!permitida) {
                continuar = false;
                alertify.error("La extensión del archivo no es valida");
            }
        }

        if (!continuar) return false;

        if (self.campos().length === 0) {
            continuar = false;
            alertify.error("Debes ingresar por lo menos un campo como origen");
        }

        if (!continuar) return false;

        //Se enumeran los campos
        let numerador = 1;
        self.campos().forEach((campo) => {
            campo.orden(numerador++);
        });

        //Se enumeran los campos
        numerador = 1;
        self.parametros().forEach((parametro) => {
            parametro.orden(numerador++);
        });

        let datos = {
            idOrigenDato: self.idOrigenDato(),
            nombre: self.nombre(),
            descripcion: self.descripcion(),
            idTipoOrigen: self.tipoOrigen(),
            procedimiento: self.procedimiento(),
            esquemaProcedimiento: self.esquemaProcedimiento(),
            esquemaTabla: self.esquemaTabla(),
            nombreTabla: self.nombreTabla(),
            rutaArchivo: self.rutaArchivo(),
            nombreArchivo: self.nombreArchivo(),
            filaEncabezado: self.filaEncabezado(),
            eliminarFila: self.eliminarFila(),
            concatenarFecha: self.concatenarFecha(),
            separador: self.separador(),
            activo: self.activo(),
            campos: self.campos(),
            parametros: self.parametros(),
            esOrigenSftp: self.esOrigenSftp(),
            rutaOrigenSftp: self.rutaOrigenSftp(),
            rutaDestinoSftp: self.rutaDestinoSftp(),
            lineaInicioLectura: self.lineaInicioLectura(),
            columnaInicioLectura: self.columnaInicioLectura()
        };
        $.ajax({
            type: "POST",
            url: urlPost,
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            data: datos,
            success: function (data) {

                if (data.resultado === true) {

                    if (data.validacion === 1) {
                        toastr["error"]("El nombre de la tabla no esta disponible", "Lo sentimos!");
                        return;
                    }

                    if (data.validacion === 2) {
                        toastr["error"]("El procedimiento almacenado no existe en la BD", "Lo sentimos!");
                        return;
                    }

                    if (data.validacion === 3) {
                        toastr["error"]("Ya existe un origen de datos con el mismo nombre", "Lo sentimos!");
                        return;
                    }

                    switch (accion) {
                        case "Nuevo":
                            self.modoLectura(true);
                            toastr["success"]("El origen de datos se ha guardado correctamente", "Muy Bien!");
                            break;
                        case "Editar":
                            toastr["success"]("El origen de datos se ha actualizado correctamente", "Muy Bien!");
                            break;
                        default:
                            toastr["error"]("Algo ha fallado", "Lo sentimos!");
                            break;
                    }
                }
                else {
                    toastr["error"]("Ha ocurrido un error guardando el origen de datos", "Lo sentimos!");
                }
            }
        });
    };
}

$(document).ready(function () {

    //Se aplica el viewModel en la vista
    var origenViewModel = new OrigenViewModel(origenData);
    ko.applyBindings(origenViewModel);

});