using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSocket.Models.Northwind;

namespace WebSocket.Sockets
{
    public class SocketTask
    {
        /// <summary>
        /// Instancia unica / singleton de la clase socket
        /// </summary>
        private readonly static Lazy<SocketTask> _instance = new Lazy<SocketTask>(() => new SocketTask(GlobalHost.ConnectionManager.GetHubContext<SocketHub>().Clients));
        // Aqui puedes instanciar tu base de datos, como ejemplo uso la de usuarios.
        private Context _db = new Context();
        /// Constructor de la clase, el cual permite acceder a los usuarios conectados
        /// al socket, usando la propiedad clientes se puede acceder a estos.
        private SocketTask(IHubConnectionContext<dynamic> Clients) => Clientes = Clients;
        public IHubConnectionContext<dynamic> Clientes { get; set; }
        /// Instancia que sirve de canal entre el socket y la appweb
        public static SocketTask Instance => _instance.Value;
        private readonly object _actualizandoCamposLock = new object();
        private volatile bool _actualizandoCampos = false;
        /// 
        /// A partir de aqui puedes establecer los metodos que se conectaran
        /// directamente a la capa de datos, estos metodos deben de ser asyncro
        /// con el objetivo de aligerar la carga del procesador/servidor.
        /// 

    }
}