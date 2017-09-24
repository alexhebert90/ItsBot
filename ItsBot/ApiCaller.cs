using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ItsBot
{
    internal class ApiCaller
    {
        /// <summary>
        /// This should be the root url used for all api endpoints.
        /// </summary>
        private const string API_URL = "https://www.reddit.com/api/v1/";

        private HttpClient Client { get; }

        private static string FullUri(string partialUri)
            => $"{API_URL}{partialUri}";

        public ApiCaller(string userAgent, BasicAuthCreds basicAuth)
        {
            // Since the use case for this is pretty specific, I'm going to require a
            // user agent to be provided and set.
            if (userAgent == null)
                throw new ArgumentNullException(nameof(userAgent));

            Client = new HttpClient();

            // Remove any default headers.
            Client.DefaultRequestHeaders.Clear();

            // Add in the user agent.
            Client.DefaultRequestHeaders.Add("User-Agent", userAgent);

            // If basic auth credentials have been provided, add them to the header values.
            if (basicAuth != null)
                Client.DefaultRequestHeaders.Authorization = basicAuth.AsAuthHeader();
        }


        public ApiCaller(string userAgent) : this(userAgent, null) { }
    

        public async Task<string> CallAsync(string requestUri)
            => await CallAsync(requestUri, null);

        public async Task<string> CallAsync(string requestUri, FormUrlEncodedContent formContent)
        {
            if (requestUri == null)
                throw new ArgumentNullException(nameof(requestUri));

            var result = await Client.PostAsync(FullUri(requestUri), formContent);

            // ToDo: Examine status code and verify it was successful.

            return await result.Content.ReadAsStringAsync();
        }

        public async Task<T> CallAsync<T>(string requestUri, FormUrlEncodedContent formContent)
            => JsonConvert.DeserializeObject<T>(await CallAsync(requestUri, formContent));
    }

    internal class BasicAuthCreds
    {
        private const string BASIC = "Basic";

        private string UserName { get; }

        private string Password { get; }

        public BasicAuthCreds(string username, string password)
        {
            UserName = username ?? throw new ArgumentNullException(nameof(username));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }

        private string AsEncodedHeader()
        {
            var bytes = Encoding.ASCII.GetBytes($"{UserName}:{Password}");

            var base64 = Convert.ToBase64String(bytes);

            return base64;
        }

        public AuthenticationHeaderValue AsAuthHeader()
        {
            return new AuthenticationHeaderValue(BASIC, AsEncodedHeader());
        }
           
    }
}
