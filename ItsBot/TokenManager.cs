using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace ItsBot
{
    internal class TokenManager
    {
        private const string ENDPOINT = "access_token";

        private readonly KeyValuePair<string, string> _grantType =
            new KeyValuePair<string, string>("grant_type", "installed_client");

        private const string USER_AGENT = "ItsBot-TokenAgent";

        // ToDo: DeviceId

        private ApiCredentials Credentials { get; }

        private ApiCaller Api { get; }

        public TokenManager(ApiCredentials apiCredentials)
        {
            Credentials = apiCredentials ?? throw new ArgumentNullException(nameof(apiCredentials));
            Api = new ApiCaller(USER_AGENT);
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

            var apiResponse = Api.CallAsync(ENDPOINT, formData).Result;

            return apiResponse;
        }
    }
}
