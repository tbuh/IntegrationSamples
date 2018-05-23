//using Microsoft.Owin.Security;
//using System.Threading.Tasks;
//using System.Web;
//using System.Web.Mvc;
//using System.Linq;

//namespace IntegrationSamples.Controllers
//{
//    public class AccountController : Controller
//    {
//        // GET: Account
//        public ActionResult Login()
//        {
//            return View();
//        }

//        [HttpGet]
//        [AllowAnonymous]
//        public ActionResult ExternalLogin(string returnUrl)
//        {
//            ControllerContext.HttpContext.Session.RemoveAll();
//            // Request a redirect to the external login provider
//            return new ChallengeResult("Purecloud", Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
//        }

//        [AllowAnonymous]
//        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
//        {
//            return RedirectToLocal(returnUrl);
            
//            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
//           // var loginInfo = AuthenticationManager.User;            

//            if (loginInfo == null)
//            {
//                return RedirectToAction("Login");
//            }
//            else
//            {
//               // var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
//                AuthenticationManager.SignIn();
//                //AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, loginInfo.Identities.ToArray());
//                return RedirectToLocal(returnUrl);
//            }                
//            //// Sign in the user with this external login provider if the user already has a login
//            //var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
//            //switch (result)
//            //{
//            //    case SignInStatus.Success:
//            //        return RedirectToLocal(returnUrl);
//            //    case SignInStatus.LockedOut:
//            //        return View("Lockout");
//            //    case SignInStatus.RequiresVerification:
//            //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
//            //    case SignInStatus.Failure:
//            //    default:
//            //        // If the user does not have an account, then prompt the user to create an account
//            //        ViewBag.ReturnUrl = returnUrl;
//            //        ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
//            //        return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
//            //}
//        }


//        #region Helpers
//        // Used for XSRF protection when adding external logins
//        private const string XsrfKey = "XsrfId";

//        private IAuthenticationManager AuthenticationManager
//        {
//            get
//            {
//                return HttpContext.GetOwinContext().Authentication;
//            }
//        }

//        private ActionResult RedirectToLocal(string returnUrl)
//        {
//            if (Url.IsLocalUrl(returnUrl))
//            {
//                return Redirect(returnUrl);
//            }
//            return RedirectToAction("Social", "Home");
//        }

//        internal class ChallengeResult : HttpUnauthorizedResult
//        {
//            public ChallengeResult(string provider, string redirectUri)
//                : this(provider, redirectUri, null)
//            {
//            }

//            public ChallengeResult(string provider, string redirectUri, string userId)
//            {
//                LoginProvider = provider;
//                RedirectUri = redirectUri;
//                UserId = userId;
//            }

//            public string LoginProvider { get; set; }
//            public string RedirectUri { get; set; }
//            public string UserId { get; set; }

//            public override void ExecuteResult(ControllerContext context)
//            {
//                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
//                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
//            }
//        }

//        #endregion
//    }
//}