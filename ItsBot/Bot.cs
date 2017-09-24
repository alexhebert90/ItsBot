using System;

namespace ItsBot
{
    /// <summary>
    /// Its bot.
    /// </summary>
    public class Bot
    {
        private ApiCredentials Credentials { get; }

        private TokenManager TokenManager { get; }

        private ApiCaller ApiCaller { get; }

        public Bot(ApiCredentials apiCredentials)
        {
            Credentials = apiCredentials ?? throw new ArgumentNullException(nameof(apiCredentials));
            TokenManager = new TokenManager(Credentials);
        }

        public string Test()
        {
            return TokenManager.Token;
        }
    }

    public class ApiCredentials
    {
		public string ClientId { get; }
		public string ClientSecret { get; }

		public ApiCredentials(string clientId, string clientSecret)
		{
			ClientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
			ClientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
		}
    }

}
