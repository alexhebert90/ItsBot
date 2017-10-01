using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ItsBot.WordDetection
{
    // ToDo: Make internal once it's possible.

    public class MatchResultCollection
    {
        public IReadOnlyList<MatchResult> Collection { get; }

        public MatchResultCollection() : 
            this(Enumerable.Empty<Match>()) { }

        public MatchResultCollection(IEnumerable<Match> matchCollection)
        {
            if (matchCollection == null)
                throw new ArgumentNullException(nameof(matchCollection));

            var collection = new List<MatchResult>();

            foreach(var match in matchCollection)
            {
                collection.Add(new MatchResult(match));
            }

            Collection = collection.AsReadOnly();
        }

        public MatchResultCollection(MatchCollection matchCollection) :
            this(matchCollection.OfType<Match>())
        { }
    }
}
