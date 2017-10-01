using System;

namespace ItsBot
{
    public class BotCredentials
    {
        public string ClientId { get; }
        public string ClientSecret { get; }

        public string UserAgent { get; }

        public BotCredentials(string clientId, string clientSecret, string userAgent)
        {
            ClientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
            ClientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
            UserAgent = userAgent ?? throw new ArgumentNullException(nameof(userAgent));
        }
    }

}
