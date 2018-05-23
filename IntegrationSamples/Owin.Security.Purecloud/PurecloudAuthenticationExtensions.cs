using System;
using Microsoft.Owin.Security;
using Owin.Security.Purecloud;

namespace Owin
{
    public static class PurecloudAuthenticationExtensions
    {
        /// <summary>
        /// Authenticate users using Purecloud
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder"/> passed to the configuration method</param>
        /// <param name="options">Middleware configuration options</param>
        /// <returns>The updated <see cref="IAppBuilder"/></returns>
        public static IAppBuilder UsePurecloudAuthentication(this IAppBuilder app, PurecloudAuthenticationOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            app.Use(typeof(PurecloudAuthenticationMiddleware), app, options);
            return app;
        }

        /// <summary>
        /// Authenticate users using Purecloud
        /// </summary>
        /// <param name="app">The <see cref="IAppBuilder"/> passed to the configuration method</param>
        /// <param name="appId">The appId assigned by Purecloud</param>
        /// <param name="appSecret">The appSecret assigned by Purecloud</param>
        /// <returns>The updated <see cref="IAppBuilder"/></returns>
        public static IAppBuilder UsePurecloudAuthentication(
            this IAppBuilder app,
            string appId,
            string appSecret)
        {
            return UsePurecloudAuthentication(
                app,
                new PurecloudAuthenticationOptions
                {
                    AppId = appId,
                    AppSecret = appSecret,
                });
        }
    }
}