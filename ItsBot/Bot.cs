using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ItsBot.TokenManagement;
using ItsBot.WordDetection;
using ItsBot.ApiCalls;

namespace ItsBot
{
    /// <summary>
    /// Its bot.
    /// </summary>
    public class Bot
    {
        internal const string Its = "its";
        internal const string It_s = "it's";

        private const string OAuthUrl = "https://oauth.reddit.com/";

        private const string Bearer = "Bearer";

        // ToDo: Fine tune this number.

        private const double SecondsBetweenRequests = 1.4;

        private BotCredentials Credentials { get; }

        private TokenManager TokenManager { get; }

        private ApiCaller Api { get; }

        private WordDetector ItsDetector { get; }

        private RateLimiter RateLimiter { get; }

        public Bot(BotCredentials apiCredentials)
        {
            Credentials = apiCredentials ?? throw new ArgumentNullException(nameof(apiCredentials));
            TokenManager = new TokenManager(Credentials);
            Api = new ApiCaller(OAuthUrl, Credentials.UserAgent, () => new AuthenticationHeaderValue(Bearer, TokenManager.Token));
            ItsDetector = new WordDetector(new WordDetectorSettings(new string[] { Its, It_s }));
            RateLimiter = new RateLimiter(TimeSpan.FromSeconds(SecondsBetweenRequests));
        }


        private async Task<CommentResults> GetCommentsAsync()
        {
            const string Endpoint = "r/all/comments?limit=100";
            return await RateLimiter.LimitAsync(async () => await Api.GetAsync<CommentResults>(Endpoint));
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
                commentAndMatches.Where(c => c.ItsMatches.Collection.Count > 0 || c.It_sMatches.Collection.Count > 0).
                ToList();
        }
    }




    public class CommentAndMatches
    {
        public CommentChildData Comment { get; }

        public MatchResultCollection ItsMatches { get; }

        public MatchResultCollection It_sMatches { get; }

        public CommentAndMatches(CommentChildData comment, WordDetectorResult collection)
        {
            Comment = comment ?? throw new ArgumentNullException(nameof(comment));

            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            ItsMatches = collection.GetMatchesFor(Bot.Its) ?? throw new ArgumentNullException(nameof(collection));
            It_sMatches = collection.GetMatchesFor(Bot.It_s) ?? throw new ArgumentNullException(nameof(collection));
        }
    }







}
