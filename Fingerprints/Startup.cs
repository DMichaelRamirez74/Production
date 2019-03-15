
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Fingerprints.Startup))]
namespace Fingerprints
{
    public  class Startup
    {
       public void Configuration(IAppBuilder app)
        {
           // ConfigureAuth(app);

            app.MapSignalR();
        }

    }
}