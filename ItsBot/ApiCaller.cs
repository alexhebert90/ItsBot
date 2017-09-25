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
        


        private string RootUrl { get; }

        private HttpClient Client { get; }

        // Can be null.
        private Func<AuthenticationHeaderValue> AuthPopulator { get; }

        private string UriCombine(string partialUri)
        {
            if (partialUri == null)
                throw new ArgumentNullException(nameof(partialUri));

            return $"{RootUrl}{partialUri}";
        }

        public ApiCaller(string rootUrl, string userAgent, BasicAuthCreds basicAuth)
        {
            RootUrl = rootUrl ?? throw new ArgumentNullException(nameof(rootUrl));

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


        public ApiCaller(string rootUrl, string userAgent) : this(rootUrl, userAgent, basicAuth: null) { }

        public ApiCaller(string rootUrl, string userAgent, Func<AuthenticationHeaderValue> authPopulator) : this(rootUrl, userAgent)
        {
            AuthPopulator = authPopulator;
        }
    
        public async Task<string> GetAsync(string requestUri)
        {
            var fulluri = UriCombine(requestUri);

            return await CallAsync(async () => await Client.GetAsync(fulluri));
        }

        public async Task<T> GetAsync<T>(string requestUri)
            => JsonConvert.DeserializeObject<T>(await GetAsync(requestUri));

        public async Task<string> PostAsync(string requestUri)
            => await PostAsync(requestUri, null);

        public async Task<string> PostAsync(string requestUri, FormUrlEncodedContent formContent)
        {
            var fullUri = UriCombine(requestUri);

            return await CallAsync(async () => await Client.PostAsync(fullUri, formContent));
        }

        private async Task<string> CallAsync(Func<Task<HttpResponseMessage>> responseCall)
        {
            // Populate the auth header in time to make the request, if the client has opted to use this method.
            if (AuthPopulator != null)
                Client.DefaultRequestHeaders.Authorization = AuthPopulator();

            var response = await responseCall();

            return await response.Content.ReadAsStringAsync();
        }

        // ToDo: Add in ability to "GET."
        //private async 

        public async Task<T> PostAsync<T>(string requestUri, FormUrlEncodedContent formContent)
            => JsonConvert.DeserializeObject<T>(await PostAsync(requestUri, formContent));
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
