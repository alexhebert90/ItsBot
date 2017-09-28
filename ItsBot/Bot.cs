using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ItsBot
{
    /// <summary>
    /// Its bot.
    /// </summary>
    public class Bot
    {
        internal const string ITS = "its";
        internal const string IT_S = "it's";

        private const string OATH_URL = "https://oauth.reddit.com/";

        private const string BEARER = "Bearer";

        // ToDo: Fine tune this number.

        private const double SECONDS_BETWEEN_REQUESTS = 1.4;

        private ApiCredentials Credentials { get; }

        private TokenManager TokenManager { get; }

        private ApiCaller Api { get; }

        private ItsDetector ItsDetector { get; }

        private RateLimiter RateLimiter { get; }

        public Bot(ApiCredentials apiCredentials)
        {
            Credentials = apiCredentials ?? throw new ArgumentNullException(nameof(apiCredentials));
            TokenManager = new TokenManager(Credentials);
            Api = new ApiCaller(OATH_URL, Credentials.UserAgent, () => new AuthenticationHeaderValue(BEARER, TokenManager.Token));
            ItsDetector = new ItsDetector(new string[] { ITS, IT_S });
            RateLimiter = new RateLimiter(TimeSpan.FromSeconds(SECONDS_BETWEEN_REQUESTS));
        }


        private async Task<CommentResults> GetCommentsAsync()
        {
            const string ENDPOINT = "r/all/comments?limit=100";
            return await RateLimiter.LimitAsync(async () => await Api.GetAsync<CommentResults>(ENDPOINT));
        }

        public async Task<FilteredCommentMatchCollection> GetFilteredCommentsAsync()
        {
            var commentsResult = await GetCommentsAsync();
            return FindMatchesInChildren(commentsResult);
        }

        private FilteredCommentMatchCollection FindMatchesInChildren(CommentResults results)
        {
            var commentAndMatches = new List<CommentAndMatches>();

            var comments = results.Data.Children;

            foreach(var comment in comments)
            {
                var matchResult = ItsDetector.Detect(comment.Data.Body);

                commentAndMatches.Add(new CommentAndMatches(comment.Data, matchResult));
            }

            return new FilteredCommentMatchCollection(commentAndMatches);
        }
    }



    public class FilteredCommentMatchCollection
    {
        public IReadOnlyList<CommentAndMatches> Collection { get; }
        
        public FilteredCommentMatchCollection(IEnumerable<CommentAndMatches> commentAndMatches)
        {
            commentAndMatches = commentAndMatches ?? new List<CommentAndMatches>();

            Collection =
                commentAndMatches.Where(c => c.ItsMatches.Count > 0 || c.It_sMatches.Count > 0).
                ToList();
        }
    }




    public class CommentAndMatches
    {
        public CommentChildData Comment { get; }

        public MatchCollection ItsMatches { get; }

        public MatchCollection It_sMatches { get; }

        public CommentAndMatches(CommentChildData comment, Dictionary<string, MatchCollection> collection)
        {
            Comment = comment ?? throw new ArgumentNullException(nameof(comment));

            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            ItsMatches = collection[Bot.ITS] ?? throw new ArgumentNullException(nameof(collection));
            It_sMatches = collection[Bot.IT_S] ?? throw new ArgumentNullException(nameof(collection));
        }
    }




    public class ApiCredentials
    {
		public string ClientId { get; }
		public string ClientSecret { get; }

        public string UserAgent { get; }

		public ApiCredentials(string clientId, string clientSecret, string userAgent)
		{
			ClientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
			ClientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
            UserAgent = userAgent ?? throw new ArgumentNullException(nameof(userAgent));
		}
    }

    public class CommentResults
    {
        public CommentData Data { get; set; }
    }

    public class CommentData
    {
        public List<CommentDataChildren> Children { get; set; }

        public string After { get; set; }

        public string Before { get; set; }
    }

    public class CommentDataChildren
    {
        public string Kind { get; set; }

        public CommentChildData Data { get; set; }
    }

    public class CommentChildData
    {
        public string Author { get; set; }

        public string Body { get; set; }

        public string SubReddit { get; set; }

        public long Created_Utc { get; set; }
    }



}
