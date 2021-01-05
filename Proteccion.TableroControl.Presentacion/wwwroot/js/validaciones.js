(function ($) {

    $.fn.soloTexto = function (key) {

        if (is.firefox()) {
            if (is.teclaPermitidaFirefox(key.key)) {
                var expresionSoloLetras = /^[A-Za-z\s]+$/;
                return expresionSoloLetras.test(key.key);
            }
        } else {
            if ((key.charCode !== 32) && (key.charCode < 65) || (key.charCode > 90) && (key.charCode < 97) || (key.charCode > 122)) {
                if (key.charCode !== 209 && key.charCode !== 241 && key.charCode !== 225 && key.charCode !== 233 && key.charCode !== 237 && key.charCode !== 243 && key.charCode !== 250 &&
                    key.charCode !== 193 && key.charCode !== 201 && key.charCode !== 205 && key.charCode !== 211 && key.charCode !== 218) {
                    return false;
                }
            }
        }
        return true;
    };

    $.fn.soloTextoNoEspacios = function (key) {

        if (is.firefox()) {
            if (is.teclaPermitidaFirefox(key.key)) {
                var expresionSoloLetras = /^[A-Za-z\s]+$/;
                return expresionSoloLetras.test(key.key);
            }
        } else {

            //solo letras A-Z y a-z
            return (key.charCode >= 65 && key.charCode <= 90) || (key.charCode >= 97 && key.charCode <= 122);
        }
    };

    $.fn.soloNumeros = function (key) {

        if (is.firefox() && is.teclaPermitidaFirefox(key.key)) {
            if (is.not.number(parseInt(key.key))) {
                return false;
            }
        } else {
            if (key.charCode < 48 || key.charCode > 57) {
                return false;
            }
        }
        return true;
    };

    $.fn.requerido = function () {

        if ($(this).val() !== undefined && $(this).val().length === 0 && $(this).attr("id") !== undefined) {
            $(this).addClass("validation-error");
            setTimeout("quitarMarca()", 5000);
            alertify.dismissAll();
            alertify.error("Hay campos obligatorios sin diligenciar");

            return false;
        } else {
            return true;
        }
    };

    $.fn.requeridoKO = function () {

        if ($(this).val() === "") {
            $(this).addClass("validation-error");
            setTimeout("quitarMarca()", 5000);
            alertify.dismissAll();
            alertify.error("Hay campos obligatorios sin diligenciar");

            return false;
        } else {
            return true;
        }
    };

    $.fn.validarFormatoEmail = function () {
        var emailReg = /\S+@\S+\.\S+/;
        if (!emailReg.test((this).val())) {
            alertify.dismissAll();
            alertify.error(mensajeCorreoInvalido);
            (this).addClass("validation-error");
            setTimeout("quitarMarca()", 5000);
            (this).focus();
            return false;
        } else
            return true;
    };

}(jQuery));

