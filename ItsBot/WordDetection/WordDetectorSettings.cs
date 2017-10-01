using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ItsBot.WordDetection
{
    // ToDo: I'm starting to doubt whether this class is even needed. 
    // I might merge it in with WordDetector and move all of its tests there as well, then 
    // move WordDetector up to the root level.

    // I made it because I thought I'd have to share the settings between several classes, 
    // but now I'm not really sure
    // why I thought that...

    //======================

    /// <summary>
    /// Encapsulates the settings for a word detector so that its settings may be shared by calling classes.
    /// </summary>
    internal class WordDetectorSettings
    {
        public ImmutableHashSet<string> MatchCollection { get; }

        public WordDetectorSettings(string match) : this(new string[] { match }) { }

        public WordDetectorSettings(IEnumerable<string> matchList)
        {
            // Prevent null arguments.
            if (matchList == null)
                throw new ArgumentNullException(nameof(matchList));

            var set = new HashSet<string>();

            // Validate that each provided input is not null.
            foreach (string match in matchList)
            {
                if (string.IsNullOrWhiteSpace(match))
                    throw new InvalidWordMatchException(match);

                set.Add(match);
            }

            // Validate that the sequence contains elements (we shouldn't be able to match on no words...
            if (!set.Any())
                throw new EmptyWordCollectionException();

            // Store matches as immutable collection.
            MatchCollection = set.ToImmutableHashSet();
        }


        public class EmptyWordCollectionException : Exception
        {
            public EmptyWordCollectionException() : base("Word collection may not be empty!") { }
        }

        public class InvalidWordMatchException : Exception
        {
            public InvalidWordMatchException(string match) : base($"Match: {match} is not a valid word match argument.") { }
        }
    }
}
