// Cambio Fecha
connection.on("notificarCambioFecha", (resultado) => {

    if (resultado.exitoso === true) {

        $("#idFechaProceso").text(resultado.fecha);
      
        if (WebNotifications.areSupported()) {
            if (WebNotifications.currentPermission() === WebNotifications.permissions.granted) {
                WebNotifications.new('Notificacion Tablero', 'La fecha de proceso ha sido modificada por el usuario ' + resultado.usuario + '\n', '../images/proceso.png', null, 10000);
            } else {
                WebNotifications.askForPermission();
            }
        }
    }
    else {
        toastr["error"]("No ha sido posible actualizar la fecha del proceso", 'Lo sentimos!');
    }
});

connection.start().catch(err => console.error(err.toString()));

document.getElementById("btnCambioFecha").addEventListener("click", event => {

    //Se obtienen los ids de los origenes seleccionados
    const fecha = $("#txtFechaProceso").val();
    $('#modalCambioFecha').modal('hide');
    getLoggedUser();
    connection.invoke("CambiarFechaProceso", fecha, loggedUser).catch(err => console.error(err.toString()));
    event.preventDefault();
});