﻿using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ItsBot.ApiCalls
{
    /// <summary>
    /// A class responsible for making calls to a particular API service.
    /// In the context of this application, it is the central pass-through point for retrieving both
    /// OAuth tokens and comments.
    /// </summary>
    internal class ApiCaller
    {
        /// <summary>
        /// Allows the client to be initialized with a root URL to prefix all subsequent requests with.
        /// Simplifies code of api calls.
        /// </summary>
        private string RootUrl { get; }

        /// <summary>
        /// Holds a single instance of a reusable .NET <see cref="HttpClient"/> used to actually make the requests.
        /// </summary>
        private HttpClient Client { get; }

        /// <summary>
        /// In cases where authorization headers need to be dynamic over the instance life,
        /// allows a function call to be passed in to use to populate the authentication.
        /// (This is used to allow the OAuth token to change for the instance as the token is flagged as expired by
        /// the <see cref="TokenManagement"/> class).
        /// </summary>
        private Func<AuthenticationHeaderValue> AuthPopulator { get; }

        /// <summary>
        /// Simple helper used to merge the root and partial uris into a single uri.
        /// </summary>
        /// <param name="partialUri"></param>
        /// <returns></returns>
        private string UriCombine(string partialUri)
        {
            if (partialUri == null)
                throw new ArgumentNullException(nameof(partialUri));

            return $"{RootUrl}{partialUri}";
        }

        /// <summary>
        /// Contains shared logic that all present and future public functions need to pass through.
        /// </summary>
        /// <param name="responseCall"></param>
        /// <returns></returns>
        private async Task<string> CallAsync(Func<Task<HttpResponseMessage>> responseCall)
        {
            // Populate the auth header in time to make the request, if the client has opted to use this method.
            if (AuthPopulator != null)
                Client.DefaultRequestHeaders.Authorization = AuthPopulator();

            var response = await responseCall();

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Private helper to call an api and collect its results.
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        private async Task<string> GetAsync(string requestUri)
        {
            var fulluri = UriCombine(requestUri);

            return await CallAsync(async () => await Client.GetAsync(fulluri));
        }

        /// <summary>
        /// Private helper to POST to an api and collect its results.
        /// </summary>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        private async Task<string> PostAsync(string requestUri)
            => await PostAsync(requestUri, null);

        /// <summary>
        /// Private helper to POST to an api with form data, and collect its results.
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="formContent"></param>
        /// <returns></returns>
        private async Task<string> PostAsync(string requestUri, FormUrlEncodedContent formContent)
        {
            var fullUri = UriCombine(requestUri);

            return await CallAsync(async () => await Client.PostAsync(fullUri, formContent));
        }

        /// <summary>
        /// Main constructor that allows for a static set of auth credentials that will not change for the lifetime of the instance.
        /// <para>See <see cref="TokenManagement"/> class as an example implementation.</para>
        /// </summary>
        /// <param name="rootUrl"></param>
        /// <param name="userAgent"></param>
        /// <param name="basicAuth"></param>
        public ApiCaller(string rootUrl, string userAgent, BasicAuthCredentials basicAuth)
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

        /// <summary>
        /// Allows instantiation of an API caller without authorization headers as part of the request.
        /// </summary>
        /// <param name="rootUrl"></param>
        /// <param name="userAgent"></param>
        public ApiCaller(string rootUrl, string userAgent) : this(rootUrl, userAgent, basicAuth: null) { }

        /// <summary>
        /// Allows instantiation of an API caller where the authorization headers are dynamically generated by the function provided.
        /// </summary>
        /// <param name="rootUrl"></param>
        /// <param name="userAgent"></param>
        /// <param name="authPopulator"></param>
        public ApiCaller(string rootUrl, string userAgent, Func<AuthenticationHeaderValue> authPopulator) : this(rootUrl, userAgent)
        {
            AuthPopulator = authPopulator;
        }
    

        /// <summary>
        /// Makes a GET api call to the endpoint provided, and transforms the results into Type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string requestUri)
            => JsonConvert.DeserializeObject<T>(await GetAsync(requestUri));

        /// <summary>
        /// Makes a POST api call to the endpoint provided, and transforms the results into Type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri"></param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string requestUri)
            => JsonConvert.DeserializeObject<T>(await PostAsync(requestUri));

        /// <summary>
        /// Makes a POST api call to the endpoint provided, using the form values provided, and transforms the results into Type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri"></param>
        /// <param name="formContent"></param>
        /// <returns></returns>
        public async Task<T> PostAsync<T>(string requestUri, FormUrlEncodedContent formContent)
            => JsonConvert.DeserializeObject<T>(await PostAsync(requestUri, formContent));
    }


}
