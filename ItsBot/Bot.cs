using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ItsBot.TokenManagement;
using ItsBot.WordDetection;
using ItsBot.ApiCalls;

namespace ItsBot
{
    /// <summary>
    /// Its bot.
    /// </summary>
    public class Bot : IBot
    {
        // The search list should come from a database in the future.
        // I think I'm going to start by only searching for "it's," as correcting
        // "its" isn't as useful.
        private static readonly string[] SearchWords = { "It's" };

        private const string OAuthUrl = "https://oauth.reddit.com/";

        private const string Bearer = "Bearer";

        // ToDo: Fine tune this number.

        private const double SecondsBetweenRequests = 1.4;

        private BotCredentials Credentials { get; }

        private TokenManager TokenManager { get; }

        private ApiCaller Api { get; }

        private RateLimiter RateLimiter { get; }

        private CommentFilter CommentFilter { get; }

        public Bot(BotCredentials apiCredentials)
        {
            Credentials = apiCredentials ?? throw new ArgumentNullException(nameof(apiCredentials));
            TokenManager = new TokenManager(Credentials);
            Api = new ApiCaller(OAuthUrl, Credentials.UserAgent, () => new AuthenticationHeaderValue(Bearer, TokenManager.Token));
            RateLimiter = new RateLimiter(TimeSpan.FromSeconds(SecondsBetweenRequests));
            CommentFilter = new CommentFilter(new WordDetector(new WordDetectorSettings(SearchWords)));
        }


        private async Task<CommentResults> GetCommentsAsync()
        {
            const string Endpoint = "r/all/comments?limit=100";
            return await RateLimiter.LimitAsync(async () => await Api.GetAsync<CommentResults>(Endpoint));
        }

        public async Task RunOnceAsync()
        {
            // This will probably have its own loop in the future.
            // For now I'll leave the main loop in the console app.

            var commentsResults = await GetCommentsAsync();
            CommentFilter.Filter(commentsResults);
        }

    }

}
