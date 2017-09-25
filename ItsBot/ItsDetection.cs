using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ItsBot
{
    internal class ItsDetector
    {
        public ItsDetector(string match) : this(new string[]{ match}){}

        public ItsDetector(IEnumerable<string> matches)
        {
            MatchList = matches ?? new string[] { };
        }

        private IEnumerable<string> MatchList { get; }

        public Dictionary<string, MatchCollection> Detect(string commentData)
        {
            var detectionResult = new Dictionary<string, MatchCollection>();

            foreach(var match in MatchList)
            {
                var itsPosMatches = Regex.Matches(commentData, $"\\b{match}\\b", RegexOptions.IgnoreCase);

                detectionResult[match] = itsPosMatches;
            }

            return detectionResult;
        }

        
    }
}
