using ItsBot.WordDetection;
using System;

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
            RecentCommentsIds = new HashedFixedSizeQueue<string>(RecentCommentIdBufferSize);
        }


        public void Filter(CommentResults commentResults)
        {
            if (commentResults == null)
                throw new ArgumentNullException(nameof(commentResults));

            
            // First, we need to get all ids in our buffer into a constant
            // lookup structure for quick comparisons.

            foreach(var commentChild in commentResults.Data.Children)
            {
                var comment = commentChild.Data;

                if (
                    NotRecentlyProcessed(comment) &&
                    ContainsWordMatch(comment, out var wordDetectResult))
                {
                    ProcessComment(comment);
                }
            }
        }

        private void ProcessComment(CommentChildData comment)
        {
            // TEMPORARY MEASURE!! REMOVE IN FINAL!!!
            Console.WriteLine($"{comment.Author} : {comment.Id}");
            Console.WriteLine();


            RecentCommentsIds.Enqueue(comment.Id);
        }

        private bool NotRecentlyProcessed(CommentChildData comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            return !RecentCommentsIds.Contains(comment.Id);
        }

        private bool ContainsWordMatch(CommentChildData comment, out WordDetectorResult result)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            // Assign a value to result to return.
            result = default;

            var match = WordDetector.Detect(comment.Body);

            bool containsWordMatch = match.TotalMatches > 0;

            if (containsWordMatch)
                result = match;

            return containsWordMatch;
        }


        private const int RecentCommentIdBufferSize = 500;

        private WordDetector WordDetector { get; }

        private HashedFixedSizeQueue<string> RecentCommentsIds { get; }
    }




}
