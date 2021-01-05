connection.on("notificarEjecucion", (validacion) => {
    let id = validacion.idValidacion;

    if (validacion.exitoso) {
        $("#imagen_" + id).attr("src", "../images/ok.png");
        $("#celda_" + id).addClass("celda-exito");
        $("#detalle_" + id).hide();
    }
    else {
        $("#imagen_" + id).attr("src", "../images/error.png");
        $("#celda_" + id).addClass("celda-error");
        $("#detalle_" + id).show();
    }

    $("#resultado_" + id).text(validacion.estado);
    $("#resultado_" + id).show();
});

connection.on("notificarFinEjecucion", () => {
    if (WebNotifications.areSupported()) {
        if (WebNotifications.currentPermission() === WebNotifications.permissions.granted) {
            WebNotifications.new('Notificacion Tablero', 'Ha finalizado el proceso de validación!\n', '../images/proceso.png', null, 10000);
        } else {
            WebNotifications.askForPermission();
        }
    }
});

document.getElementById("btnEjecutar").addEventListener("click", event => {

    //Se obtienen los ids de los origenes seleccionados
    const ids = $('input.validacionCheck:checked').map(function () { return $(this).data("id"); }).get();
    const equipo = $("#IdEquipo option:selected").text();
    const tipo = $("#IdTipoValidacion option:selected").text();

    ids.forEach((id) => {
        $("#imagen_" + id).attr("src", "../images/loader.gif");
        $("#imagen_" + id).show();
        $("#resultado_" + id).hide();
    });

    getLoggedUser();

    connection.invoke("EjecutarValidaciones", ids, equipo, tipo, loggedUser).catch(err => console.error(err.toString()));
    event.preventDefault();
});
