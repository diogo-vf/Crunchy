using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using CrunchyBetaDownloader.Api.Interfaces;
using CrunchyBetaDownloader.Configs;

namespace CrunchyBetaDownloader
{
    public class ApiCrunchyBeta
    {
        private static readonly string AccessToken = "aHJobzlxM2F3dnNrMjJ1LXRzNWE6cHROOURteXRBU2Z6QjZvbXVsSzh6cUxzYTczVE1TY1k=";
        private static readonly string IndexEndpoint = "https://beta-api.crunchyroll.com/index/v2";
        private static readonly string ProfileEndpoint = "https://beta-api.crunchyroll.com/accounts/v1/me/profile";
        private static readonly string TokenEndpoint = "https://beta-api.crunchyroll.com/auth/v1/token";
        private static readonly string SearchEndpoint = "https://beta-api.crunchyroll.com/content/v1/search";
        private static readonly string StreamsEndpoint = "https://beta-api.crunchyroll.com/cms/v2{}/videos/{}/streams";
        private static readonly string SeasonsEndpoint = "https://beta-api.crunchyroll.com/cms/v2{}/seasons";
        private static readonly string EpisodesEndpoint = "https://beta-api.crunchyroll.com/cms/v2{}/episodes";
        
        private enum HttpMethod
        {
            Get,
            Post
        }

        #region private properties
        
        private const string BaseUrl = "https://crunchyroll.com";

        private const string UserAgent =
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.61 Safari/537.36'";

        private HttpClient Client { get; init; }
        private CookieContainer CookieContainer { get; init; }
        private HttpClientHandler Handler { get; init; }
        private Uri _lastUri;
        
        #endregion private properties

        #region public properties
        
        public string? Auth { get; private set; }
        public bool IsPremium { get; private set; }
        #endregion public properties
        
        private Cookie? GetCookie(Uri uri, string name) =>
            CookieContainer.GetCookies(uri).FirstOrDefault(x => x.Name == name);
        
        public Cookie? GetCookie(string name) => GetCookie(_lastUri, name);

        public ApiCrunchyBeta()
        {
            _lastUri = new Uri(BaseUrl);
            
            CookieContainer = new CookieContainer();
            Handler = new HttpClientHandler
            {
                CookieContainer = CookieContainer
            };
            Client = new HttpClient(Handler);
            Client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            Client.DefaultRequestHeaders.Add("Authorization", $"Basic {AccessToken}");
        }

        public async Task<string?> Token(string username, string password)
        {
            JsonObject json = new()
            {
                ["username"] = username,
                ["password"] = password,
                ["grant_type"] = "password",
                ["scope"] = "offline_access"
            };
            HttpResponseMessage response = await Client.PostAsync(TokenEndpoint, new StringContent(json.ToString()));

            return await response.Content.ReadAsStringAsync();
        }
    }
}