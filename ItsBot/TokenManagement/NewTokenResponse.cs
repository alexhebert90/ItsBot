using Newtonsoft.Json;

namespace ItsBot.TokenManagement
{
    /// <summary>
    /// Json class that maps to the API response that reddit sends down for a successfully retrieved OAuth token.
    /// </summary>
    internal class NewTokenResponse
    {
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; set; }
    }
}
