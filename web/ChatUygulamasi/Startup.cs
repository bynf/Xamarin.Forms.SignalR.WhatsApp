using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(ChatUygulamasi.Startup))]

namespace ChatUygulamasi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}