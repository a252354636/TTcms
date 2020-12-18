﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;

namespace TTcms.Owin.Security.SSO
{
    /// <summary>
    /// Default <see cref="ISSOAccountAuthenticationProvider"/> implementation.
    /// </summary>
    public class SSOAccountAuthenticationProvider : ISSOAccountAuthenticationProvider
    {
        /// <summary>
        /// Initializes a new <see cref="SSOAccountAuthenticationProvider"/>
        /// </summary>
        public SSOAccountAuthenticationProvider()
        {
            OnAuthenticated = context => Task.FromResult<object>(null);
            OnReturnEndpoint = context => Task.FromResult<object>(null);
            OnApplyRedirect = context =>
                context.Response.Redirect(context.RedirectUri);
        }

        /// <summary>
        /// Gets or sets the function that is invoked when the Authenticated method is invoked.
        /// </summary>
        public Func<SSOAccountAuthenticatedContext, Task> OnAuthenticated { get; set; }

        /// <summary>
        /// Gets or sets the function that is invoked when the ReturnEndpoint method is invoked.
        /// </summary>
        public Func<SSOReturnEndpointContext, Task> OnReturnEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the delegate that is invoked when the ApplyRedirect method is invoked.
        /// </summary>
        public Action<SSOAccountApplyRedirectContext> OnApplyRedirect { get; set; }

        /// <summary>
        /// Invoked whenever Microsoft succesfully authenticates a user
        /// </summary>
        /// <param name="context">Contains information about the login session as well as the user <see cref="System.Security.Claims.ClaimsIdentity"/>.</param>
        /// <returns>A <see cref="Task"/> representing the completed operation.</returns>
        public virtual Task Authenticated(SSOAccountAuthenticatedContext context)
        {
            return OnAuthenticated(context);
        }

        /// <summary>
        /// Invoked prior to the <see cref="System.Security.Claims.ClaimsIdentity"/> being saved in a local cookie and the browser being redirected to the originally requested URL.
        /// </summary>
        /// <param name="context">Contains information about the login session as well as the user <see cref="System.Security.Claims.ClaimsIdentity"/></param>
        /// <returns>A <see cref="Task"/> representing the completed operation.</returns>
        public virtual Task ReturnEndpoint(SSOReturnEndpointContext context)
        {
            return OnReturnEndpoint(context);
        }

        /// <summary>
        /// Called when a Challenge causes a redirect to authorize endpoint in the Microsoft account middleware
        /// </summary>
        /// <param name="context">Contains redirect URI and <see cref="AuthenticationProperties"/> of the challenge </param>
        public virtual void ApplyRedirect(SSOAccountApplyRedirectContext context)
        {
            OnApplyRedirect(context);
        }
    }
}
