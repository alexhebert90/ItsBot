using System.Collections.Generic;

namespace ItsBot
{
    // These POCOs relate to the responses gotten back from Reddit when fetching comments.

    internal class CommentResults
    {
        public CommentData Data { get; set; }
    }

    internal class CommentData
    {
        public List<CommentDataChildren> Children { get; set; }

        public string After { get; set; }

        public string Before { get; set; }
    }

    internal class CommentDataChildren
    {
        public string Kind { get; set; }

        public CommentChildData Data { get; set; }
    }

    internal class CommentChildData
    {
        public string Id { get; set; }

        public string Author { get; set; }

        public string Body { get; set; }

        public string SubReddit { get; set; }

        public long Created_Utc { get; set; }
    }
}
