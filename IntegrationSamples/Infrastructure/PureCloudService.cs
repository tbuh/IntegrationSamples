using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using PureCloudPlatform.Client.V2.Api;
using PureCloudPlatform.Client.V2.Extensions;

namespace IntegrationSamples.Infrastructure
{
    public class PureCloudService
    {
        private ConversationsApi _conversationsApi;
        private static AuthTokenInfo _authTokenInfo;

        public PureCloudService()
        {
            _conversationsApi = new ConversationsApi();
        }

        public void AddMessage()
        {
          //  _conversationsApi.2
        }

        public static void Init()
        {
            try
            {
                //string token = null;
                //Task.Run(async () => { _authTokenInfo = await GetToken(); });
                //PureCloudPlatform.Client.V2.Client.ApiClient api = new PureCloudPlatform.Client.V2.Client.ApiClient(AppSettings.PureCloudUri);
                //_authTokenInfo = api.PostToken(AppSettings.ClientID, AppSettings.ClientSecret, AppSettings.RedirectUri);
            }
            catch (Exception ex)
            {


            }
        }

        private static async Task<AuthTokenInfo> GetToken()
        {
            var client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("redirect_uri", AppSettings.RedirectUri)
            });

            var basicAuth = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes($"{AppSettings.ClientID}:{AppSettings.ClientSecret}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);
            var response = await client.PostAsync("https://login.mypurecloud.ie/oauth/token", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseJObj = JObject.Parse(responseContent);

            var expires = responseJObj.GetValue("expires_in").ToString();

            var tokenInfo = new AuthTokenInfo
            {
                AccessToken = responseJObj.GetValue("access_token").ToString(),
                TokenType = responseJObj.GetValue("token_type").ToString(),
            };

            int expiresValue;
            if (Int32.TryParse(expires, NumberStyles.Integer, CultureInfo.InvariantCulture, out expiresValue))
            {
                tokenInfo.ExpiresIn = expiresValue;
            }

            return tokenInfo;
        }
    }
}