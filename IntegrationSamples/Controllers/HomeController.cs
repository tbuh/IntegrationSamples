using IntegrationSamples.Infrastructure;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IntegrationSamples.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private const string XmlSchemaString = "http://www.w3.org/2001/XMLSchema#string";
        //public ActionResult MyCloud()
        //{
        //    var users = new GamificationService().GetTop5Scores();
        //    return View(users);
        //}

        public ActionResult TestView()
        {
            return View();
        }

        //public ActionResult Social()
        //{
        //    return View();
        //}

        public ActionResult Social()
        {
            return View();
        }

        public async Task<ActionResult> MyCloud()
        {
            return await LoginOrRunAction(() =>
            {
                ViewBag.Message = "Your Social page.";
                var users = new GamificationService().GetTop5Scores();
                return View(users);
            });
        }

        //public ActionResult MyCloud()
        //{

        //        ViewBag.Message = "Your Social page.";
        //        var users = new GamificationService().GetTop5Scores();
        //        return View(users);

        //}

        #region security
        private async Task<ActionResult> LoginOrRunAction(Func<ActionResult> action)
        {
            var authToken = GetAuthTokenFromSession();

            if (Request.QueryString["code"] != null)
            {
                try
                {
                    var code = Request.QueryString["code"];
                    authToken = await GetTokenFromCode(code);
                    ViewBag.AccessToken = authToken;

                    var user = await GetUserInfo(authToken);
                    ViewBag.Id = user.Value<string>("id");
                    ViewBag.Username = user.Value<string>("name");
                    new ChatUserService().Create(ViewBag.Id, ViewBag.Username);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.ToString();
                    Console.WriteLine(ex);
                    Console.WriteLine(ex.StackTrace);
                }
            }

            if (string.IsNullOrEmpty(authToken))
            {
                return Redirect($"https://login.mypurecloud.ie/oauth/authorize?client_id={AppSettings.ClientID}" +
                                $"&response_type=code&redirect_uri={HttpUtility.UrlEncode(AppSettings.RedirectUri)}");
            }
            return action();
        }


        private async Task<string> GetTokenFromCode(string code)
        {
            var client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", AppSettings.RedirectUri)
            });

            //var basicAuth = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes("6220730c-d0e4-4fde-af3d-5ffgdca94e22:BC2GlOXcBXof56PSR8CA0TB6tHdlj3DLPEQ8hwf87kI"));
            var basicAuth = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes("8ca23c30-d6c1-484b-9ff1-7019d54ec5ec:wyKxWGzyRBxZHEDnnTNV4SRFM7wnWMXQryWhAlqKZAQ"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
            var response = await client.PostAsync("https://login.mypurecloud.ie/oauth/token", content);
            var token = JObject.Parse(await response.Content.ReadAsStringAsync())["access_token"].ToString();
            return token;
        }

        private async Task<JObject> GetUserInfo(string accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.GetAsync("https://api.mypurecloud.ie/api/v2/users/me");
            return JObject.Parse(await response.Content.ReadAsStringAsync());
        }

        private string GetAuthTokenFromSession()
        {
            return (string)this.HttpContext.Session["authToken"];
        }

        private void SetAuthTokenToSession(string authToken)
        {
            this.HttpContext.Session.Add("authToken", authToken);
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        #endregion
    }
}