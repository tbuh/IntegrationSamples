// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net.Http;
using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Infrastructure;

namespace Owin.Security.Purecloud
{
    /// <summary>
    /// OWIN middleware for authenticating users using Purecloud
    /// </summary>    
    public class PurecloudAuthenticationMiddleware : AuthenticationMiddleware<PurecloudAuthenticationOptions>
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Initializes a <see cref="PurecloudAuthenticationMiddleware"/>
        /// </summary>
        /// <param name="next">The next middleware in the OWIN pipeline to invoke</param>
        /// <param name="app">The OWIN application</param>
        /// <param name="options">Configuration options for the middleware</param>
        public PurecloudAuthenticationMiddleware(
            OwinMiddleware next,
            IAppBuilder app,
            PurecloudAuthenticationOptions options)
            : base(next, options)
        {
            if (string.IsNullOrWhiteSpace(Options.AppId))
            {
                throw new ArgumentException("OptionMustBeProvided AppId");
            }
            if (string.IsNullOrWhiteSpace(Options.AppSecret))
            {
                throw new ArgumentException("OptionMustBeProvided AppSecret");
            }

            _logger = app.CreateLogger<PurecloudAuthenticationMiddleware>();

            if (Options.Provider == null)
            {
                Options.Provider = new PurecloudAuthenticationProvider();
            }
            if (Options.StateDataFormat == null)
            {
                IDataProtector dataProtector = app.CreateDataProtector(
                    typeof(PurecloudAuthenticationMiddleware).FullName,
                    Options.AuthenticationType, "v1");
                Options.StateDataFormat = new PropertiesDataFormat(dataProtector);
            }
            if (String.IsNullOrEmpty(Options.SignInAsAuthenticationType))
            {
                //Options.SignInAsAuthenticationType = app.GetDefaultSignInAsAuthenticationType();
                throw new ArgumentException("OptionMustBeProvided SignInAsAuthenticationType");
            }

            _httpClient = new HttpClient(ResolveHttpMessageHandler(Options));
            _httpClient.Timeout = Options.BackchannelTimeout;
            _httpClient.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB
        }

        /// <summary>
        /// Provides the <see cref="AuthenticationHandler"/> object for processing authentication-related requests.
        /// </summary>
        /// <returns>An <see cref="AuthenticationHandler"/> configured with the <see cref="PurecloudAuthenticationOptions"/> supplied to the constructor.</returns>
        protected override AuthenticationHandler<PurecloudAuthenticationOptions> CreateHandler()
        {
            return new PurecloudAuthenticationHandler(_httpClient, _logger);
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Managed by caller")]
        private static HttpMessageHandler ResolveHttpMessageHandler(PurecloudAuthenticationOptions options)
        {
            HttpMessageHandler handler = options.BackchannelHttpHandler ?? new WebRequestHandler();

            // If they provided a validator, apply it or fail.
            if (options.BackchannelCertificateValidator != null)
            {
                // Set the cert validate callback
                var webRequestHandler = handler as WebRequestHandler;
                if (webRequestHandler == null)
                {
                    throw new InvalidOperationException();
                }
                webRequestHandler.ServerCertificateValidationCallback = options.BackchannelCertificateValidator.Validate;
            }

            return handler;
        }
    }
}