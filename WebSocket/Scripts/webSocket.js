'use strict';

// Declarando el web socket
var wss = $.connection.socketHub;
/// Funcion para inicializar el websocket, despues de su ejecucion
/// esta quedara oculta en cache para agregarle un poquito de seguridad.
$(function () {
    function init() {
        // Llama al metodo GetNotificaciones en el servidor
        wss.server.getNotificaciones()
            .done(function (result) { // al terminar el metodo se ejecuta la funcion done, para trabajar con el resultado.
                console.log(result);
            });
    }
    // Es un callback que sera ejecutado a peticion del servidor
    wss.client.marcarVisto = function (notificacion) {
        // Aqui le quitamos el color de 'no visto' a una notificacion.
    }
    // Arrancamos el web socket
    $.connection.hub.start().done(init);
});