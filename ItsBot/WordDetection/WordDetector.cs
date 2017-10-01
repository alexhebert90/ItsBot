using System;
using System.Text.RegularExpressions;

namespace ItsBot.WordDetection
{
    /// <summary>
    /// Class designed to detect full words within a provided collection of words.
    /// </summary>
    internal class WordDetector
    {
        private WordDetectorSettings Settings { get; }

        /// <summary>
        /// Creates a new <see cref="WordDetector"/> instance using the provided settings.
        /// </summary>
        /// <param name="settings"></param>
        public WordDetector(WordDetectorSettings settings)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <summary>
        /// Runs detection and returns a dictionary key for every word provided in the settings.
        /// </summary>
        /// <param name="commentData"></param>
        /// <returns></returns>
        public WordDetectorResult Detect(string commentData)
        {
            if (commentData == null)
                throw new ArgumentNullException(nameof(commentData));

            var detectionResult = new WordDetectorResult();

            foreach (var match in Settings.MatchCollection)
            {
                var currentMatchWordMatches = Regex.Matches(commentData, $"\\b{match}\\b", RegexOptions.IgnoreCase);
                detectionResult.AddResult(match, new MatchResultCollection(currentMatchWordMatches));
            }

            return detectionResult;
        }       
    }
}
