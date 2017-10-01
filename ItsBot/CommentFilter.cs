using ItsBot.WordDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItsBot
{
    /// <summary>
    /// This class's job is to take in a batch of comments from an api caller and to return only those that are worth recording.
    /// This class will likely need to be specific to the "pull" part of this app.
    /// </summary>
    internal class CommentFilter
    {
        // Stateful class that will make sure comments don't get duplicated!

        public CommentFilter(WordDetector wordDetector)
        {
            WordDetector = wordDetector ?? throw new ArgumentNullException(nameof(wordDetector));
            RecentCommentsIds = new FixedSizeQueue<string>(RecentCommentIdBufferSize);
        }


        public void Filter(CommentResults commentResults)
        {
            if (commentResults == null)
                throw new ArgumentNullException(nameof(commentResults));

            
            // First, we need to get all ids in our buffer into a constant
            // lookup structure for quick comparisons.
            var recentIdHash = 
                new HashSet<string>(RecentCommentsIds.Items);

            foreach(var comment in commentResults.Data.Children)
            {
                var commentId = comment.Data.Id;

                if(!recentIdHash.Contains(commentId))
                {
                    // Only continue processing the current comment if it hasn't already been processed.

                }
            }

        }

        //private bool RecentlyProcessed(CommentData comment)
        //{
        //    if (comment == null)
        //        throw new ArgumentNullException(nameof(comment));

        //    // ToDo: Use repeated hash.

        //}

        //private bool ContainsWordMatch(CommentData comment)
        //{
        //    if (comment == null)
        //        throw new ArgumentNullException(nameof(comment));

        //    return 
        //}


        private const int RecentCommentIdBufferSize = 500;

        private WordDetector WordDetector { get; }

        private FixedSizeQueue<string> RecentCommentsIds { get; }
    }




}
