using System.Collections.Generic;

namespace ItsBot
{
    // These POCOs relate to the responses gotten back from Reddit when fetching comments.

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
