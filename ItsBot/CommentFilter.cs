using System;
using System.Collections.Generic;
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

        public CommentFilter() { }


        public void Filter(CommentResults commentResults)
        {
            if (commentResults == null)
                throw new ArgumentNullException(nameof(commentResults));
        }

    }
}
