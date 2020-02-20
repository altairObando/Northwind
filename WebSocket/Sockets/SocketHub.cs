using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using WebSocket.Models;

namespace WebSocket.Sockets
{
    /// <summary>
    /// Esta es la clase que sera instanciada desde el lado del cliente
    /// y la cual sera el contexto principal para acceder a una opcion del servidor
    /// desde el lado del cliente del socket.
    /// </summary>
    [HubName("socketHub")] // Con este nombre debe instanciarse desde el lado del cliente.
    public class SocketHub : Hub
    {
        /// <summary>
        /// Esta instancia unica de solo lectura sera la que permita la conexion en tiempo real
        /// con el contexto del socket hacia la applicacion web. sin esta instancia seria mas complicado acceder a 
        /// a la capa de datos.
        /// </summary>
        private readonly SocketTask _socketTask;
        public SocketHub() : this(SocketTask.Instance) { }
        public SocketHub(SocketTask instance) => this._socketTask = instance;

        public async Task<List<Notificacion>> GetNotificaciones(bool transact)
        {
            return await this._socketTask.GetNotificaciones(transact);
        }
    }
}