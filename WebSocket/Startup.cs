using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebSocket.Startup))]
namespace WebSocket
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
