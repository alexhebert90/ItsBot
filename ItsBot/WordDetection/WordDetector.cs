using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ItsBot.WordDetection
{
    internal class WordDetector
    {
        private WordDetectorSettings Settings { get; }

        public WordDetector(WordDetectorSettings settings)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public Dictionary<string, MatchCollection> Detect(string commentData)
        {
            var detectionResult = new Dictionary<string, MatchCollection>();

            foreach(var match in Settings.MatchCollection)
            {
                var itsPosMatches = Regex.Matches(commentData, $"\\b{match}\\b", RegexOptions.IgnoreCase);

                detectionResult[match] = itsPosMatches;
            }

            return detectionResult;
        }       
    }
}
