using CrunchyBetaDownloader.Api.ResponsesClasses;

namespace CrunchyBetaDownloader.Api.utils;

public static class ResponseExtension
{
    public static T Feed<T>(this T response, Response? feederResponse) where T : Response
    {
        if (feederResponse is null) return response;
        response.RefreshToken = feederResponse.RefreshToken;
        response.AccessToken = feederResponse.AccessToken;
        response.ExpiresIn = feederResponse.ExpiresIn;
        response.ExpiresAt = feederResponse.ExpiresAt;
        response.TokenType = feederResponse.TokenType;
        response.Scope = feederResponse.Scope;
        response.Country = feederResponse.Country;
        return response;
    }
}