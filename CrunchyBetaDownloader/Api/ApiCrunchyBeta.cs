using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CrunchyBetaDownloader.Api.ResponsesClasses;
using CrunchyBetaDownloader.Api.utils;
using Newtonsoft.Json;

namespace CrunchyBetaDownloader.Api
{
    public class ApiCrunchyBeta
    {
        private enum RequestType
        {
            Get,
            Post
        }

        #region private properties

        private const string AccessToken = "aHJobzlxM2F3dnNrMjJ1LXRzNWE6cHROOURteXRBU2Z6QjZvbXVsSzh6cUxzYTczVE1TY1k=";

        // private const string UserAgent = "Crunchyroll/3.10.0 Android/6.0 okhttp/4.9.1";
        // private const string UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.61 Safari/537.36'";

        private HttpClient Client { get; }

        #endregion private properties

        public ApiCrunchyBeta()
        {
            Client = new HttpClient();
        }

        public async Task<ProfileResponse?> Login(string username, string password)
        {
            Dictionary<string, string?> content = new()
            {
                ["username"] = username,
                ["password"] = password,
                ["grant_type"] = "password",
                ["scope"] = "offline_access"
            };

            string tokenResponseJson = await Request(RequestType.Post, AccessToken, content, EndPoint.Token);
            TokenResponse? tokenResponse =
                ConvertStringJsonToResponse<TokenResponse>(tokenResponseJson);
            
            if (tokenResponse is null) throw new Exception("Fail to connect user");
            tokenResponse.ExpireAt = DateTime.Now.Add(TimeSpan.FromSeconds(tokenResponse.ExpiresIn ?? 0));

            string profileResponseJson =
                await Request(RequestType.Get, tokenResponse.AccessToken, null, EndPoint.Profile);
            return ConvertStringJsonToResponse<ProfileResponse>(profileResponseJson)?.Feed(tokenResponse);
        }

        //TODO fix the error when it's useful
        public async Task<SearchResponse?> Search(Response? response, string query, int page = 0)
        {
            Response? tokenResponse = await RefreshToken(response);

            Dictionary<string, string?> content = new()
            {
                ["q"] = query,
                ["n"] = "4",
                ["start"] = (4 * page).ToString(),
            };

            string searchResponseJson = await Request(RequestType.Get, tokenResponse?.AccessToken, content,
                EndPoint.Search);
            SearchResponse? a = ConvertStringJsonToResponse<SearchResponse>(searchResponseJson);
            return a?.Feed(tokenResponse);
        }

        public async Task<Response?> Season(IndexResponse response, string seasonId)
        {
            Dictionary<string, string?> content = new()
            {
                ["series_id"] = seasonId,
                ["Policy"] = response.Policy,
                ["Signature"] = response.Signature,
                ["Key-Pair-Id"] = response.KeyPairId
            };

            string tokenResponseJson = await Request(RequestType.Post, AccessToken, content, EndPoint.Token);
            Response? tokenResponse =
                ConvertStringJsonToResponse<TokenResponse>(tokenResponseJson);
            return tokenResponse;
        }
        public async Task<IndexResponse?> Index(Response? response)
        {
            Response? tokenResponse = await RefreshToken(response);
            string indexResponseJson = await Request(RequestType.Get, tokenResponse?.AccessToken, null,
                EndPoint.Index);

            return ConvertStringJsonToResponse<IndexResponse>(indexResponseJson)?.Feed<IndexResponse>(tokenResponse);
        }

        private async Task<Response?> RefreshToken(Response? response)
        {
            if ( response is null || string.IsNullOrEmpty(response.RefreshToken)) throw new Exception("RefreshToken: invalid data");
            if (response.ExpireAt > DateTime.Now) return response;
            
            Dictionary<string, string?> content = new()
            {
                ["refresh_token"] = response.RefreshToken,
                ["grant_type"] = "refresh_token",
                ["scope"] = "offline_access"
            };
            string json = await Request(RequestType.Post, AccessToken, content, EndPoint.Token);
            TokenResponse? tokenResponse = ConvertStringJsonToResponse<TokenResponse>(json);
            
            if (tokenResponse is null) throw new Exception("Fail to connect user");
            tokenResponse.ExpireAt = DateTime.Now.Add(TimeSpan.FromSeconds(tokenResponse.ExpiresIn ?? 0));

            return tokenResponse;
        }

        private async Task<string> Request(RequestType type, string? accessToken,
            IEnumerable<KeyValuePair<string, string?>>? content, string uri)
        {
            (HttpMethod httpMethod, string authorizationType) = type switch
            {
                RequestType.Post => (HttpMethod.Post, "Basic"),
                RequestType.Get => (HttpMethod.Get, "Bearer"),
                _ => throw new Exception($"{nameof(type)} Out of enum")
            };

            HttpResponseMessage response;
            using (HttpRequestMessage request = new(httpMethod, uri))
            {
                request.Headers.Add("Authorization", $"{authorizationType} {accessToken}");
                request.Content = content is null ? null : new FormUrlEncodedContent(content);
                response = await Client.SendAsync(request);
            }

            return await response.Content.ReadAsStringAsync();
        }

        private static T? ConvertStringJsonToResponse<T>(string json) where T : Response
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
#if DEBUG
                Console.WriteLine(e.Message);
#endif
                return default;
            }
        }
    }
}