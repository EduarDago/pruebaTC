let validaciones = {};

connection.on("notificarImportacion", (resultado) => {
    let id = resultado.idOrigenDato;

    if (resultado.estado === "Sin inconvenientes") {
        $("#imagen_" + id).attr("src", "../images/ok.png");
        $("#btnInconsistencia_" + id).hide();
    }
    else {
        $("#imagen_" + id).attr("src", "../images/error.png");
        if (resultado.errores !== null) {
            $("#btnInconsistencia_" + id).show();
            validaciones = resultado.validaciones;
            $("#listaerrores_" + id).text(resultado.errores);
        } else {
            $("#btnInconsistencia_" + id).hide();
        }
    }

    $("#resultado_" + id).text(resultado.estado);
    $("#cantidad_" + id).text(resultado.cantidadRegistros);
    $("#resultado_" + id).show();

});

connection.on("notificarFinImportacion", () => {
    if (WebNotifications.areSupported()) {
        if (WebNotifications.currentPermission() === WebNotifications.permissions.granted) {
            WebNotifications.new('Notificacion Tablero', 'Ha finalizado el proceso de importación!\n', '../images/proceso.png', null, 10000);
        } else {
            WebNotifications.askForPermission();
        }
    }
});

connection.on("notificarErrorImportacion", (resultado) => {
    $("#divError").show();
    $("#mensajeError").text(resultado.estado);
});

document.getElementById("btnEjecutar").addEventListener("click", event => {

    //Se obtienen los ids de los origenes seleccionados
    const ids = $('input.origenCheck:checked').map(function () { return $(this).data("id"); }).get();
    const idsValidacion = $('input.checkValidar:checked').map(function () { return $(this).data("id"); }).get();

    if (!(ids.length > 0)) {
        toastr["error"]("Debe seleccionar por lo menos 1 origen de datos", "Lo sentimos!");
        return;
    }

    ids.forEach((id) => {
        $("#imagen_" + id).attr("src", "../images/loader.gif");
        $("#imagen_" + id).show();
        $("#resultado_" + id).hide();
        $("#btnInconsistencia_" + id).hide();
    });

    getLoggedUser();
    connection.invoke("EjecutarImportaciones", ids, idsValidacion, loggedUser).catch(err => console.error(err.toString()));
    event.preventDefault();
});
