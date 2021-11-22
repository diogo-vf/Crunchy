﻿namespace CrunchyBetaDownloader.Api.Interfaces
{
    public interface IToken
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }
        public string TokenType { get; set; }
        public string Scope { get; set; }
        public string Country { get; set; }
    }
}