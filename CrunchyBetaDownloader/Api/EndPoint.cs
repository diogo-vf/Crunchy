namespace CrunchyBetaDownloader.Api
{
    public static class EndPoint
    {
        public static string Token { get; } = "https://beta-api.crunchyroll.com/auth/v1/token";
        public static string Search { get; } = "https://beta-api.crunchyroll.com/content/v1/search";
        public static string Profile { get; } = "https://beta-api.crunchyroll.com/accounts/v1/me/profile";
        public static string Index { get; } = "https://beta-api.crunchyroll.com/index/v2";
        public static string Objects { get; } = "https://beta-api.crunchyroll.com/cms/v2{0}/objects/{1}";
        public static string Seasons { get; } = "https://beta-api.crunchyroll.com/cms/v2{0}/seasons";
        public static string Episodes { get; } = "https://beta-api.crunchyroll.com/cms/v2{0}/episodes";
    }
}