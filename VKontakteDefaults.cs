namespace Authentication.VKontakte.Core
{
    public static class VKontakteDefaults
    {
        public const string ClaimsIssuer = "VKontakte";
        public const string AuthenticationScheme = "VKontakte";
        public static readonly string DisplayName = "VKontakte";
        public static readonly string AuthorizationEndpoint = "https://oauth.vk.com/authorize";
        public static readonly string TokenEndpoint = "https://oauth.vk.com/access_token";
        public static readonly string UserInformationEndpoint = "https://api.vk.com/method/users.get.json";
        public static readonly string CallbackPath = "/signin-vkontakte";
    }
}
