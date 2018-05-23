using IntegrationSamples;
using IntegrationSamples.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

[assembly: OwinStartup(typeof(Startup), "Configuration")]

namespace IntegrationSamples
{
    public class Startup
    {
        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddDataProtection();
        //    services.Configure<ExternalAuthenticationOptions>(options =>
        //    {
        //        options.SignInAsAuthenticationType = CookieAuthenticationDefaults.AuthenticationType;
        //    });
        //}

        public void Configuration(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie

            //var authenticationOptions = new Owin.Security.Purecloud.PurecloudAuthenticationOptions
            //{
            //    AppId = AppSettings.ClientID,
            //    AppSecret = AppSettings.ClientSecret,
            //    SignInAsAuthenticationType = "Purecloud",
            //};

            //app.UseCookieAuthentication(new CookieAuthenticationOptions
            //{
            //    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
            //    LoginPath = new PathString("/Account/ExternalLogin"),
            //    //Provider = new CookieAuthenticationProvider
            //    //{
            //    //    // Enables the application to validate the security stamp when the user logs in.
            //    //    // This is a security feature which is used when you change a password or add an external login to your account.  
            //    //    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
            //    //        validateInterval: TimeSpan.FromMinutes(30),
            //    //        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
            //    //}
            //});

            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            //app.UsePurecloudAuthentication(authenticationOptions);            

            //app.UseTwoFactorRememberBrowserCookie(
            //DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
            app.MapSignalR();
            //app.SetDefaultSignInAsAuthenticationType("External");
            //app.UseOAuthAuthorizationServer("PureCloud", options=>
            //{
                
            //});
        }
    }
}