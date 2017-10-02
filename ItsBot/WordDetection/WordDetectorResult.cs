using System;
using System.Collections.Generic;
using System.Linq;

namespace ItsBot.WordDetection
{
    internal class WordDetectorResult
    {
        private Dictionary<string, MatchResultCollection> BackingCollection { get; }

        public WordDetectorResult()
        {
            BackingCollection = new Dictionary<string, MatchResultCollection>();
        }

        public void AddResult(string matchWord, MatchResultCollection matches)
        {
            if (matchWord == null)
                throw new ArgumentNullException(nameof(matchWord));

            if (matches == null)
                throw new ArgumentNullException(nameof(matches));

            BackingCollection[CaseInsensitiveKey(matchWord)] = matches;
        }

        /// <summary>
        /// Returns the number of unique word matches total for this result.
        /// </summary>
        public int MatchWordCount =>
            BackingCollection.Count;


        /// <summary>
        /// Returns the total number of matches across all match words.
        /// </summary>
        public int TotalMatches
            => BackingCollection.Values.Sum(i => i.Collection.Count);


        /// <summary>
        /// Returns matches for a specified match string.
        /// <para>Returns null if key does not exist.</para>
        /// </summary>
        /// <param name="matchWord"></param>
        /// <returns></returns>
        public MatchResultCollection GetMatchesFor(string matchWord)
        {
            if (matchWord == null)
                throw new ArgumentNullException(nameof(matchWord));

            // Return null if no matches found.
            BackingCollection.TryGetValue(CaseInsensitiveKey(matchWord), out var result);
            return result;
        }

        /// <summary>
        /// Helper to guarantee key manipulations run through the same logic.
        /// Used to make case insensitive keys.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string CaseInsensitiveKey(string key)
            => key.ToUpper();
    }
}
