using System;
using System.Text.RegularExpressions;

namespace ItsBot.WordDetection
{
    // ToDo: Make internal once it's possible

    public class MatchResult
    {
        public int Index { get; }

        public string Value { get; }

        public int Length
            => Value.Length;

        public MatchResult(int index, string value)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), "Match result index must be a positive integer");

            Index = index;

            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public MatchResult(Match match)
            : this(index: Validate(match).Index, value: Validate(match).Value) { }

        private static Match Validate(Match match)
            => match ?? throw new ArgumentNullException(nameof(match));
    }
}
