// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Microsoft.Owin;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Security;

namespace Owin.Security.Purecloud
{
    /// <summary>
    /// Configuration options for <see cref="PurecloudAuthenticationMiddleware"/>
    /// </summary>
    public class PurecloudAuthenticationOptions : AuthenticationOptions
    {
        /// <summary>
        /// Initializes a new <see cref="PurecloudAuthenticationOptions"/>
        /// </summary>
        public PurecloudAuthenticationOptions()
            : base(Constants.DefaultAuthenticationType)
        {
            Caption = Constants.DefaultAuthenticationType;
            CallbackPath = new PathString("/signin-Purecloud");
            AuthenticationMode = AuthenticationMode.Passive;
            BackchannelTimeout = TimeSpan.FromSeconds(60);
            SendAppSecretProof = true;
            CookieManager = new CookieManager();

            AuthorizationEndpoint = Constants.AuthorizationEndpoint;
            TokenEndpoint = Constants.TokenEndpoint;
            UserInformationEndpoint = Constants.UserInformationEndpoint;
        }

        /// <summary>
        /// Gets or sets the Purecloud-assigned appId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Gets or sets the Purecloud-assigned app secret
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// Gets or sets the URI where the client will be redirected to authenticate.
        /// </summary>
        public string AuthorizationEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the URI the middleware will access to exchange the OAuth token.
        /// </summary>
        public string TokenEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the URI the middleware will access to obtain the user information.
        /// </summary>
        public string UserInformationEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the a pinned certificate validator to use to validate the endpoints used
        /// in back channel communications belong to Purecloud.
        /// </summary>
        /// <value>
        /// The pinned certificate validator.
        /// </value>
        /// <remarks>If this property is null then the default certificate checks are performed,
        /// validating the subject name and if the signing chain is a trusted party.</remarks>
        public ICertificateValidator BackchannelCertificateValidator { get; set; }

        /// <summary>
        /// Gets or sets timeout value in milliseconds for back channel communications with Purecloud.
        /// </summary>
        /// <value>
        /// The back channel timeout in milliseconds.
        /// </value>
        public TimeSpan BackchannelTimeout { get; set; }

        /// <summary>
        /// The HttpMessageHandler used to communicate with Purecloud.
        /// This cannot be set at the same time as BackchannelCertificateValidator unless the value 
        /// can be downcast to a WebRequestHandler.
        /// </summary>
        public HttpMessageHandler BackchannelHttpHandler { get; set; }

        /// <summary>
        /// Get or sets the text that the user can display on a sign in user interface.
        /// </summary>
        public string Caption
        {
            get { return Description.Caption; }
            set { Description.Caption = value; }
        }

        /// <summary>
        /// The request path within the application's base path where the user-agent will be returned.
        /// The middleware will process this request when it arrives.
        /// Default value is "/signin-Purecloud".
        /// </summary>
        public PathString CallbackPath { get; set; }

        /// <summary>
        /// Gets or sets the name of another authentication middleware which will be responsible for actually issuing a user <see cref="System.Security.Claims.ClaimsIdentity"/>.
        /// </summary>
        public string SignInAsAuthenticationType { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IPurecloudAuthenticationProvider"/> used to handle authentication events.
        /// </summary>
        public IPurecloudAuthenticationProvider Provider { get; set; }

        /// <summary>
        /// Gets or sets the type used to secure data handled by the middleware.
        /// </summary>
        public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }

        /// <summary>
        /// Gets or sets if the appsecret_proof should be generated and sent with Purecloud API calls.
        /// This is enabled by default.
        /// </summary>
        public bool SendAppSecretProof { get; set; }

        /// <summary>
        /// An abstraction for reading and setting cookies during the authentication process.
        /// </summary>
        public ICookieManager CookieManager { get; set; }
    }
}