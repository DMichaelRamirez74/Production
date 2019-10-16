
using Fingerprints.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Ninject;
using Owin;
using Fingerprints.Hubs.ExecutiveHubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Fingerprints.Hubs.YakkrHub;

[assembly: OwinStartup(typeof(Fingerprints.Startup))]
namespace Fingerprints
{
    //public partial class Startup
    //{
    //   public void Configuration(IAppBuilder app)
    //    {


    //      ConfigureAuth(app);
    //        app.MapSignalR();
    //    }

    //}


    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

            var kernel = new StandardKernel();
            var resolver = new NinjectSignalRDependencyResolver(kernel);

            kernel.Bind<IExecutiveDashboadTicker>().To<ExecutiveDashboardTicker>().InSingletonScope();//Bind to Executive Ticker object  // Make it a singleton object.

            kernel.Bind(typeof(IHubConnectionContext<dynamic>)).ToMethod(context =>
                    resolver.Resolve<IConnectionManager>().GetHubContext<ExecutiveDashboardHub>().Clients)
                    .WhenInjectedInto<IExecutiveDashboadTicker>();



            kernel.Bind<IYakkrTicker>().To<YakkrTicker>().InSingletonScope();

            kernel.Bind(typeof(IHubConnectionContext<dynamic>)).ToMethod(context => 
            resolver.Resolve<IConnectionManager>().GetHubContext<YakkrHub>().Clients
            ).WhenInjectedInto<IYakkrTicker>();

            var config = new HubConfiguration();
            config.Resolver = resolver;

          
            ConfigureAuth(app);

            ConfigureSignalR(app, config);

          
        }
    }

   
}