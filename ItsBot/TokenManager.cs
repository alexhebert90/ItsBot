using System;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ItsBot
{
    internal class TokenManager
    {
        private const string API_URL = "https://www.reddit.com/api/v1/";

        private const string ENDPOINT = "access_token";

        private readonly KeyValuePair<string, string> _grantType =
            new KeyValuePair<string, string>("grant_type", "client_credentials");

        /// <summary>
        /// The amount of time before a token is actually set to expire before fetching a new one, in minutes.
        /// (Arbitrary selection of 1.5 minutes).
        /// </summary>
        private const double EXPIRATION_BUFFER_MINUTES = 1.5;

        // ToDo: DeviceId

        private ApiCredentials Credentials { get; }

        private ApiCaller Api { get; }


        /// <summary>
        /// Holds the data from the last valid token retrieval, if one has already occured.
        /// </summary>
        private SuccessfulTokenResult LastValidNewToken { get; set; }


        public TokenManager(ApiCredentials apiCredentials)
        {
            Credentials = apiCredentials ?? throw new ArgumentNullException(nameof(apiCredentials));

            // Set up credentials to use with the api instance.
            BasicAuthCredentials basicAuthCreds = new BasicAuthCredentials(
                username: Credentials.ClientId,
                password: Credentials.ClientSecret);

            Api = new ApiCaller(API_URL, Credentials.UserAgent, basicAuthCreds);
        }

        public string Token => GetToken();

        private string GetToken()
        {
            // I'm assuming for now that ResetToken() will always
            // succeed in getting a new token.

            
            if(LastValidNewToken == null)
            {
                // If a token has never been fetched
                ResetToken();
            }
            else if(ExpiresSoon(LastValidNewToken))
            {
                // If there is an existing token but it's about to expire
                ResetToken();
            }


            // We'll assume that covers it all for now and that hitting this point means
            // there is a valid token.
            return LastValidNewToken.TokenResponse.AccessToken;
        }


        private static bool ExpiresSoon(SuccessfulTokenResult token)
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));


            var exactTokenExpiresAt = token.TokenExpiresAt;

            if(DateTime.UtcNow >= exactTokenExpiresAt - TimeSpan.FromMinutes(EXPIRATION_BUFFER_MINUTES))
            {
                return true;
            }

            return false;
        }

        private void ResetToken()
        {
            var formData = new FormUrlEncodedContent(new[]
            {
                _grantType
            });


            var apiResponse = 
                Api.PostAsync<NewTokenResponse>(ENDPOINT, formData).Result;

            // ToDo: Don't leave this like this. I just don't know what to do yet
            // If any of this somehow fails.
            if (apiResponse == null)
                throw new InvalidOperationException();


            LastValidNewToken = new SuccessfulTokenResult(apiResponse);
        } 
    }

    internal class SuccessfulTokenResult
    {
        public NewTokenResponse TokenResponse { get; }

        public DateTime TimeGotten { get; }

        public DateTime TokenExpiresAt { get; }

        public SuccessfulTokenResult(
            NewTokenResponse newTokenResponse)
        {
            TokenResponse = newTokenResponse ?? throw new ArgumentNullException(nameof(newTokenResponse));
            TimeGotten = DateTime.UtcNow;
            TokenExpiresAt = TimeGotten + TimeSpan.FromMinutes(TokenResponse.ExpiresIn);
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
