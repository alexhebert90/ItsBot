using System;
using System.Net.Http;
using System.Collections.Generic;

namespace ItsBot.TokenManagement
{
    /// <summary>
    /// Class responsible for retrieving new api tokens and managing when it is time to retrieve a new one.
    /// </summary>
    internal class TokenManager
    {
        /// <summary>
        /// The root url for obtaining OAuth tokens from reddit.
        /// </summary>
        private const string API_URL = "https://www.reddit.com/api/v1/";

        /// <summary>
        /// The actual endpoint off of the root url we will be calling to get a token.
        /// </summary>
        private const string ENDPOINT = "access_token";

        /// <summary>
        /// Contains a collection of form values to send with every request.
        /// </summary>
        private readonly KeyValuePair<string, string> _grantType =
            new KeyValuePair<string, string>("grant_type", "client_credentials");

        /// <summary>
        /// The amount of time before a token is actually set to expire before fetching a new one, in minutes.
        /// (Arbitrary selection of 1.5 minutes).
        /// So if the token expires in 60 minutes, retrieve a new one at 58.5 minutes, in other words.
        /// </summary>
        private const double EXPIRATION_BUFFER_MINUTES = 1.5;

        // ToDo: DeviceId

        /// <summary>
        /// Holds the instance of credentials that will be used as the authorization fields to make requests.
        /// </summary>
        private ApiCredentials Credentials { get; }

        /// <summary>
        /// Holds an instance to a class that will actually perform api calls that the manager requires.
        /// </summary>
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

}
