using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.Twitter;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Microsoft.AspNet.SignalR;

namespace Fingerprints
{
    public  partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Login/Loginagency")
            });


            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // App.Secrets is application specific and holds values in CodePasteKeys.json
            // Values are NOT included in repro – auto-created on first load
            //if (!string.IsNullOrEmpty(App.Secrets.GoogleClientId))
            //{
            //    app.UseGoogleAuthentication(
            //        clientId: App.Secrets.GoogleClientId,
            //        clientSecret: App.Secrets.GoogleClientSecret);
            //}

            //if (!string.IsNullOrEmpty(App.Secrets.TwitterConsumerKey))
            //{
            //    app.UseTwitterAuthentication(
            //        consumerKey: App.Secrets.TwitterConsumerKey,
            //        consumerSecret: App.Secrets.TwitterConsumerSecret);
            //}

            //if (!string.IsNullOrEmpty(App.Secrets.GitHubClientId))
            //{
            //    app.UseGitHubAuthentication(
            //        clientId: App.Secrets.GitHubClientId,
            //        clientSecret: App.Secrets.GitHubClientSecret);
            //}

            System.Web.Helpers.AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            
        }

        public static void ConfigureSignalR(IAppBuilder app, HubConfiguration config)
        {
            // For more information on how to configure your application using OWIN startup, visit http://go.microsoft.com/fwlink/?LinkID=316888

            app.MapSignalR(config);
        }

        public static void ConfigureSignalR2(IAppBuilder app, HubConfiguration config)
        {
            // For more information on how to configure your application using OWIN startup, visit http://go.microsoft.com/fwlink/?LinkID=316888

            app.MapSignalR(config);
        }

    }
}
