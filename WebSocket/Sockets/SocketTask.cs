using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebSocket.Models;
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
        private ApplicationDbContext _db = new ApplicationDbContext();
        /// Constructor de la clase, el cual permite acceder a los usuarios conectados
        /// al socket, usando la propiedad clientes se puede acceder a estos.
        private SocketTask(IHubConnectionContext<dynamic> Clients) => Clientes = Clients;
        public IHubConnectionContext<dynamic> Clientes { get; set; }
        /// Instancia que sirve de canal entre el socket y la appweb
        public static SocketTask Instance => _instance.Value;
        // Campos para Thread Safe
        private readonly object _actualizandoCamposLock = new object();
        private volatile bool _actualizandoCampos = false; 
        /// 
        /// A partir de aqui puedes establecer los metodos que se conectaran
        /// directamente a la capa de datos, estos metodos deben de ser asyncro
        /// con el objetivo de aligerar la carga del procesador/servidor.
        /// REVISAR APPLICATIONDBCONTEXT

            /// Este metodo permite obtener todas las notificaciones en el sistema
            /// ordenandolas de forma descendente.
        public async Task<List<Notificacion>> GetNotificaciones(bool transact)
        {
            if(!transact)
            {
                var not = await _db.Notificaciones
                .OrderByDescending(d => d.FechaCreacion)
                .ToListAsync();
                return not;
            }
            else
            {
                var query = "SELECT * FROM [dbo].[Notificaciones] Order By FechaCreacion DESC";
                var not = await _db.Database.SqlQuery<Notificacion>(query).ToListAsync();
                return not;
            }
        }

        public void UpdateNotificacion(Guid id)
        {
            lock(_actualizandoCamposLock)
            {
                if(!_actualizandoCampos)
                {
                    _actualizandoCampos = true;
                    var not =  _db.Notificaciones.Find(id);
                    not.Seen = true;
                    _db.Entry(not).State = EntityState.Modified;

                    _actualizandoCampos = false;

                    if (_db.SaveChanges() > 0)
                        Clientes.All.marcarNotificacionVista(id.ToString());
                }
            }
        }
    }
}