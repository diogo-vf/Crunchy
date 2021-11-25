using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using CrunchyBetaDownloader.Api.ResponsesClasses;
using CrunchyBetaDownloader.Api.utils;

namespace CrunchyBetaDownloader.Api
{
    public class ApiCrunchyBeta
    {
        private enum RequestType { Get, Post }

        #region private properties

        private const string BaseUrl = "https://crunchyroll.com";
        private const string AccessToken = "aHJobzlxM2F3dnNrMjJ1LXRzNWE6cHROOURteXRBU2Z6QjZvbXVsSzh6cUxzYTczVE1TY1k=";

        // private const string UserAgent = "Crunchyroll/3.10.0 Android/6.0 okhttp/4.9.1";
        // private const string UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.61 Safari/537.36'";

        private HttpClient Client { get; init; }
        private CookieContainer CookieContainer { get; init; }
        private HttpClientHandler Handler { get; init; }
        private Uri _lastUri;

        #endregion private properties

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
        }

        public async Task<ProfileResponse?> Login(string username, string password)
        {
            Dictionary<string, string> content = new()
            {
                ["username"] = username,
                ["password"] = password,
                ["grant_type"] = "password",
                ["scope"] = "offline_access"
            };

            string tokenResponseJson = await Request(RequestType.Post, AccessToken, content, EndPoint.Token); //await Request(RequestType.Post, AccessToken, content, EndPoint.Token);
            TokenResponse? tokenResponse =
                ConvertStringJsonToResponse<TokenResponse>(tokenResponseJson);
            
            if (tokenResponse is null) throw new Exception("Fail to connect user");
            
            string profileResponseJson = await Request(RequestType.Get, tokenResponse.AccessToken, null, EndPoint.Profile);
            ProfileResponse? profileResponse =
                JsonSerializer.Deserialize<ProfileResponse>(profileResponseJson);

            return profileResponse?.Feed(tokenResponse);
        }

        public async Task<TokenResponse?> Search(Response? response, string query)
        {
            TokenResponse? tokenResponse = await RefreshToken(response);
            // Dictionary<string, string> content = new()
            // {
            //     ["q"] =  query,
            //     ["n"] = "6",
            //     ["locale"] = 
            // };
            // GetRequest()
            //
            return tokenResponse;
        }

        public async Task<(TokenResponse? tokenResponse, IndexResponse? response)> Index(Response? response)
        {
            TokenResponse? tokenResponse = await RefreshToken(response);
            IndexResponse? indexResponse = ConvertStringJsonToResponse<IndexResponse>(
                await Request(RequestType.Get, tokenResponse?.AccessToken, null, "https://beta-api.crunchyroll.com/index/v2"));

            return (tokenResponse, indexResponse);
        }

        private async Task<TokenResponse?> RefreshToken(Response? response)
        {
            if (string.IsNullOrEmpty(response?.RefreshToken)) throw new Exception("RefreshToken: no token Response");
            Dictionary<string, string> content = new()
            {
                ["refresh_token"] = response.RefreshToken,
                ["grant_type"] = "refresh_token",
                ["scope"] = "offline_access"
            };
            string json = await Request(RequestType.Post, AccessToken, content, EndPoint.Token);

            return ConvertStringJsonToResponse<TokenResponse>(json);
        }
        
        private async Task<string> Request(RequestType type, string? accessToken, IEnumerable<KeyValuePair<string, string>>? content, string uri)
        {
            (HttpMethod httpMethod, string authorization) = type switch
            {
                RequestType.Post => (HttpMethod.Post, "Basic"),
                RequestType.Get => (HttpMethod.Get, "Bearer"),
                _ => throw new Exception($"{nameof(type)} Out of enum")
            };
            
            HttpResponseMessage response;
            using (HttpRequestMessage request = new(httpMethod, uri))
            {
                request.Headers.Add("Authorization", $"{authorization} {accessToken}");
                request.Content = content is null ? null : new FormUrlEncodedContent(content);
                response = await Client.SendAsync(request);
            }
            string json = await response.Content.ReadAsStringAsync();

            return json;
        }

        private static T? ConvertStringJsonToResponse<T>(string json) where T : Response
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}