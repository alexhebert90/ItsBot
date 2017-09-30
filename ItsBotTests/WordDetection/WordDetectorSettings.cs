using ItsBot.WordDetection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ItsBotTests.WordDetection
{
    /// <summary>
    /// Tests tied to the <see cref="WordDetectorSettings"/> class.
    /// </summary>
    [TestClass]
    public class WordDetectorSettingsTests
    {
        /// <summary>
        /// Validates that null collections result in a <see cref="ArgumentNullException"/> at the constructor level.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Null collection inputs should not be allowed.")]
        public void WordCollectionNotNull()
        {
            new WordDetectorSettings(matchList: null);
        }

        /// <summary>
        /// Validates that an empty collection is not valid input to the constructor.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(WordDetectorSettings.EmptyWordCollectionException), "An empty word collection should not be allowed.")]
        public void WordCollectionsNotEmpty()
        {
            new WordDetectorSettings(new string[] { });
        }

        /// <summary>
        /// Tests variations of inputs I've arbitrarily deemed to be "invalid."
        /// </summary>
        [TestMethod]
        public void InvalidWordsCaught()
        {
            // Whitespace bad.
            Assert.ThrowsException<WordDetectorSettings.InvalidWordMatchException>(() =>
                new WordDetectorSettings("    "));

            // Empty bad.
            Assert.ThrowsException<WordDetectorSettings.InvalidWordMatchException>(() =>
                new WordDetectorSettings(""));

            // Null bad.
            Assert.ThrowsException<WordDetectorSettings.InvalidWordMatchException>(() =>
                new WordDetectorSettings(match: null));

            // Mix it up a little
            Assert.ThrowsException<WordDetectorSettings.InvalidWordMatchException>(() =>
                new WordDetectorSettings(new string[] {"valid", "valid2", "  " }));

            Assert.ThrowsException<WordDetectorSettings.InvalidWordMatchException>(() =>
                new WordDetectorSettings(new string[] { "valid", null, "valid2" }));

            Assert.ThrowsException<WordDetectorSettings.InvalidWordMatchException>(() =>
                new WordDetectorSettings(new string[] { "", null, "valid2" }));
        }

        /// <summary>
        /// Tests the constructor does not break down for what I deem "valid" inputs.
        /// </summary>
        [TestMethod]
        public void ValidWordsAreValid()
        {
            // Not much meaning behind these tests besides validating just an actual minimum of a functioning constructor.
            new WordDetectorSettings("I'm Valid.");

            new WordDetectorSettings("Translucent.");

            new WordDetectorSettings("<text>");

            new WordDetectorSettings(new string[] { "One", "Two", "Three" });
        }

        /// <summary>
        /// Makes sure valid words make it into the instance.
        /// </summary>
        [TestMethod]
        public void ValidWordsPreservedIntoInstance()
        {
            // C# 7 Local functions coming in handy...
            void Test(WordDetectorSettings settings, int count)
            {
                Assert.AreEqual(settings.MatchCollection.Count, count);
            }

            // One using single constructor.
            Test(new WordDetectorSettings("One"), 1);
            // One using enumerable constructor.
            Test(new WordDetectorSettings(new string[] { "One" }), 1);
            // Three distinct.
            Test(new WordDetectorSettings(new string[] { "One", "Two", "Three" }), 3);
            // Two distinct in collection of three..
            Test(new WordDetectorSettings(new string[] { "One", "Two", "Two" }), 2);
        }
    }
}
