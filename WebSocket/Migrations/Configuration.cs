namespace WebSocket.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WebSocket.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "WebSocket.Models.ApplicationDbContext";
        }

        protected override void Seed(WebSocket.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            context.Notificaciones.AddOrUpdate
                (
                d => d.Descripcion,
                new Models.Notificacion { Tipo = Models.TipoNotificacion.info, Descripcion = "Probando nueva notificacion", FechaCreacion = DateTime.Now, Seen = false, Url="/Home/Index" },
                new Models.Notificacion { Tipo = Models.TipoNotificacion.info, Descripcion = "Probando nueva notificacion", FechaCreacion = DateTime.Now, Seen = false, Url = "/Home/Index" },
                new Models.Notificacion { Tipo = Models.TipoNotificacion.question, Descripcion = "Probando nueva notificacion", FechaCreacion = DateTime.Now, Seen = false, Url = "/Home/Index" },
                new Models.Notificacion { Tipo = Models.TipoNotificacion.success, Descripcion = "Probando nueva notificacion", FechaCreacion = DateTime.Now, Seen = false, Url = "/Home/Index" }
                );
            context.SaveChanges();
        }
    }
}
