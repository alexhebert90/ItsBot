using System;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ItsBot
{
    internal class TokenManager
    {
        private const string ENDPOINT = "access_token";

        private readonly KeyValuePair<string, string> _grantType =
            new KeyValuePair<string, string>("grant_type", "client_credentials");

        private const string USER_AGENT = "ItsBot-TokenAgent";

        // ToDo: DeviceId

        private ApiCredentials Credentials { get; }

        private ApiCaller Api { get; }

        public TokenManager(ApiCredentials apiCredentials)
        {
            Credentials = apiCredentials ?? throw new ArgumentNullException(nameof(apiCredentials));

            // Set up credentials to use with the api instance.
            BasicAuthCreds basicAuthCreds = new BasicAuthCreds(
                username: Credentials.ClientId,
                password: Credentials.ClientSecret);

            Api = new ApiCaller(USER_AGENT, basicAuthCreds);
        }

        public string Token => GetToken();

        private string GetToken()
        {
            // ToDo: Manage not getting a new key with every request.
            return NewToken();
        }


        private string NewToken()
        {
            var formData = new FormUrlEncodedContent(new[]
            {
                _grantType
            });

            var apiResponse = Api.CallAsync<NewTokenResponse>(ENDPOINT, formData).Result;

            return apiResponse.AccessToken;
        }
    }

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
