namespace Owin.Security.Purecloud
{
    public static class Constants
    {
        public const string DefaultAuthenticationType = "Purecloud";
        internal const string DefaultEnvironment = "mypurecloud.ie";
        internal const string ReplaceEnvironment = "{{environment}}";
        public const string AuthorizationEndpoint = "https://login.mypurecloud.ie/oauth/authorize";
        public const string TokenEndpoint = "https://login.mypurecloud.ie/oauth/token";
        public const string UserInformationEndpoint = "https://api.mypurecloud.ie/api/v2/users/me";
    }
}