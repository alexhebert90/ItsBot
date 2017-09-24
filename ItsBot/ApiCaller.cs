using System;
using System.Net.Http;
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

        public ApiCaller(string userAgent)
        {
            // Since the use case for this is pretty specific, I'm going to require a
            // user agent to be provided and set.
            if(userAgent == null)
                throw new ArgumentNullException(nameof(userAgent));

            Client = new HttpClient();

            // Remove any default headers.
            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.Add("User-Agent", userAgent);

        }

         

        public async Task<string> CallAsync(string requestUri)
            => await CallAsync(requestUri, null);

        public async Task<string> CallAsync(string requestUri, FormUrlEncodedContent formContent)
        {
            var result = await Client.PostAsync(requestUri, formContent);

            return result.Content.ToString();

        }
    }
}
