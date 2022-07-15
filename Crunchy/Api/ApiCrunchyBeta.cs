using System.Text;
using System.Text.RegularExpressions;
using Crunchy.Api.ResponsesClasses;
using Crunchy.Api.utils;
using Newtonsoft.Json;

namespace Crunchy.Api;

public class ApiCrunchyBeta
{
    public ApiCrunchyBeta()
    {
        Client = new HttpClient();
    }

    public async Task<ProfileResponse?> Login(string? username, string? password)
    {
        Dictionary<string, string?> content = new()
        {
            ["username"] = username,
            ["password"] = password,
            ["grant_type"] = "password",
            ["scope"] = "offline_access"
        };

        string tokenResponseJson = await Request(RequestType.Post, AccessToken, content, EndPoint.Token);
        TokenResponse? tokenResponse = ConvertStringJsonToResponse<TokenResponse>(tokenResponseJson);

        if (tokenResponse is null) throw new Exception("Fail to connect user");
        tokenResponse.ExpiresAt =
            DateTime.Now.AddSeconds(TimeSpan.FromSeconds(tokenResponse.ExpiresIn - 5 ?? 0).TotalSeconds);

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
            ["start"] = (4 * page).ToString()
        };

        string searchResponseJson = await Request(RequestType.Get, tokenResponse?.AccessToken, content,
            EndPoint.Search);
        SearchResponse? a = ConvertStringJsonToResponse<SearchResponse>(searchResponseJson);
        return a?.Feed(tokenResponse);
    }

    public async Task<SeasonsResponse?> GetSeasonFromSerieId(IndexResponse? indexResponse, string seriesId,
        string locale)
    {
        Response? tokenResponse = await RefreshToken(indexResponse);
        string uri = string.Format(EndPoint.Seasons, indexResponse?.Bucket);

        Dictionary<string, string?> content = new()
        {
            ["series_id"] = seriesId,
            ["locale"] = locale,
            ["Policy"] = indexResponse?.Policy,
            ["Signature"] = indexResponse?.Signature,
            ["Key-Pair-Id"] = indexResponse?.KeyPairId
        };

        string seasonResponseJson = await Request(RequestType.Get, AccessToken, content, uri, true);
        SeasonsResponse? seasonsResponse =
            ConvertStringJsonToResponse<SeasonsResponse>(seasonResponseJson);

        return seasonsResponse?.Feed(tokenResponse);
    }

    public async Task<EpisodesResponse?> GetEpisodesFromSeasonId(IndexResponse? indexResponse, string seasonId,
        string locale)
    {
        Response? tokenResponse = await RefreshToken(indexResponse);
        string uri = string.Format(EndPoint.Episodes, indexResponse?.Bucket);

        Dictionary<string, string?> content = new()
        {
            ["season_id"] = seasonId,
            ["locale"] = locale,
            ["Policy"] = indexResponse?.Policy,
            ["Signature"] = indexResponse?.Signature,
            ["Key-Pair-Id"] = indexResponse?.KeyPairId
        };

        string episodesResponseJson = await Request(RequestType.Get, AccessToken, content, uri, true);
        EpisodesResponse? episodesResponse =
            ConvertStringJsonToResponse<EpisodesResponse>(episodesResponseJson);
        return episodesResponse?.Feed(tokenResponse);
    }

    public async Task<IndexResponse?> Index(Response? response)
    {
        Response? tokenResponse = await RefreshToken(response);
        string indexResponseJson = await Request(RequestType.Get, tokenResponse?.AccessToken, null,
            EndPoint.Index);

        return ConvertStringJsonToResponse<IndexResponse>(indexResponseJson)?.Feed<IndexResponse>(tokenResponse);
    }

    public async Task<ObjectsResponse?> GetObject(IndexResponse? indexResponse, string url, string locale)
    {
        string id = new Regex(@"\/[A-Z]\w+").Match(url).Value.Replace("/", string.Empty);
        string uri = string.Format(EndPoint.Objects, indexResponse?.Bucket, id);
        Dictionary<string, string?> content = new()
        {
            ["locale"] = locale,
            ["Policy"] = indexResponse?.Policy,
            ["Signature"] = indexResponse?.Signature,
            ["Key-Pair-Id"] = indexResponse?.KeyPairId
        };
        string objectResponseJson = await Request(RequestType.Get, indexResponse?.AccessToken, content, uri, true);

        return ConvertStringJsonToResponse<ObjectsResponse>(objectResponseJson)
            ?.Feed<ObjectsResponse>(indexResponse);
    }

    public async Task<VideoStreams?> CallPlayback(string playback, ObjectsResponse? objectsResponse = null)
    {
        string objectResponseJson = await Request(RequestType.Get, null, null, playback);

        return ConvertStringJsonToResponse<VideoStreams>(objectResponseJson)
            ?.Feed<VideoStreams>(objectsResponse);
    }

    private static string? ConvertIEnumerableToUrl(IEnumerable<KeyValuePair<string, string?>>? content)
    {
        if (content is null) return null;

        StringBuilder builder = new();
        foreach ((string key, string? value) in content)
        {
            if (builder.Length > 0) builder.Append('&');

            builder.Append(Encode(key));
            builder.Append('=');
            builder.Append(Encode(value));
        }

        return builder.ToString();
    }

    private static string Encode(string? data)
    {
        // Escape spaces as '+'.
        return string.IsNullOrEmpty(data) ? string.Empty : Uri.EscapeDataString(data).Replace("%20", "+");
    }

    private async Task<Response?> RefreshToken(Response? response)
    {
        if (response is null || string.IsNullOrEmpty(response.RefreshToken))
            throw new Exception("RefreshToken: invalid data");
        if (response.ExpiresAt > DateTime.Now) return response;

        Dictionary<string, string?> content = new()
        {
            ["refresh_token"] = response.RefreshToken,
            ["grant_type"] = "refresh_token",
            ["scope"] = "offline_access"
        };
        string json = await Request(RequestType.Post, AccessToken, content, EndPoint.Token);
        TokenResponse? tokenResponse = ConvertStringJsonToResponse<TokenResponse>(json);

        if (tokenResponse is null) throw new Exception("Fail to connect user");
        tokenResponse.ExpiresAt =
            DateTime.Now.AddSeconds(TimeSpan.FromSeconds(tokenResponse.ExpiresIn ?? 0).TotalSeconds);

        return tokenResponse;
    }

    private async Task<string> Request(RequestType type, string? accessToken,
        IEnumerable<KeyValuePair<string, string?>>? content, string uri, bool contentOnUrl = false)
    {
        (HttpMethod httpMethod, string authorizationType) = type switch
        {
            RequestType.Post => (HttpMethod.Post, "Basic"),
            RequestType.Get => (HttpMethod.Get, "Bearer"),
            _ => throw new Exception($"{nameof(type)} Out of enum")
        };
        if (contentOnUrl)
            uri += $"?{ConvertIEnumerableToUrl(content)}";
        HttpResponseMessage response;
        using (HttpRequestMessage request = new(httpMethod, uri))
        {
            request.Headers.Add("Authorization", $"{authorizationType} {accessToken}");
            if (!contentOnUrl)
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
}