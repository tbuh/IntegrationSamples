using System.Threading.Tasks;

namespace Owin.Security.Purecloud
{
    /// <summary>
    /// Specifies callback methods which the <see cref="PurecloudAuthenticationMiddleware"></see> invokes to enable developer control over the authentication process. />
    /// </summary>
    public interface IPurecloudAuthenticationProvider
    {
        /// <summary>
        /// Invoked whenever Purecloud succesfully authenticates a user
        /// </summary>
        /// <param name="context">Contains information about the login session as well as the user <see cref="System.Security.Claims.ClaimsIdentity"/>.</param>
        /// <returns>A <see cref="Task"/> representing the completed operation.</returns>
        Task Authenticated(PurecloudAuthenticatedContext context);

        /// <summary>
        /// Invoked prior to the <see cref="System.Security.Claims.ClaimsIdentity"/> being saved in a local cookie and the browser being redirected to the originally requested URL.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>A <see cref="Task"/> representing the completed operation.</returns>
        Task ReturnEndpoint(PurecloudReturnEndpointContext context);

        /// <summary>
        /// Called when a Challenge causes a redirect to authorize endpoint in the Purecloud middleware
        /// </summary>
        /// <param name="context">Contains redirect URI and <see cref="AuthenticationProperties"/> of the challenge </param>
        void ApplyRedirect(PurecloudApplyRedirectContext context);
    }
}