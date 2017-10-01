using ItsBot.WordDetection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ItsBotTests.WordDetection
{
    /// <summary>
    /// Tests around the <see cref="WordDetector"/> class.
    /// </summary>
    [TestClass]
    public class WordDectectorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Settings object should not be null.")]
        public void SettingsNotNull()
        {
            new WordDetector(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Search text should not be null.")]
        public void SearchTextNotNull()
        {
            new WordDetector(new WordDetectorSettings("Search")).Detect(null);
        }

        /// <summary>
        /// Make sure a key exists for every unique search word supplied.
        /// </summary>
        [TestMethod]
        public void KeyForEveryUniqueWord()
        {
            // I'm not sure how I feel about this test, since
            // technically the WordDetectorSettings class is what removes redundant
            // keys... It is still useful but may be out of the correct scope.

            var detector = new WordDetector(
                new WordDetectorSettings(
                    new string[] { "A", "b", "apple", "banana", "apple" }));

            var result = detector.Detect("Unimportant text to search.");

            Assert.AreEqual(result.Count, 4);
        }

        /// <summary>
        /// Make sure that the output is tied to the keys supplied in the settings.
        /// </summary>
        [TestMethod]
        public void ResultKeysMatchInput()
        {
            const string KEY1 = "Lopsided";
            const string KEY2 = "Forever here";
            const string KEY3 = "audio.";

            var detector = new WordDetector(
                new WordDetectorSettings(
                    new string[] { KEY1, KEY2, KEY3 }));

            var result = detector.Detect("This text doesn't matter, yet again.");

            // The test will fail under this point if the point I'm trying to prove isn't accurate.
            var a = result[KEY1];
            var b = result[KEY2];
            var c = result[KEY3];
        }

        /// <summary>
        /// Just verifying that every match object gotten back is not assigned to a null reference.
        /// </summary>
        [TestMethod]
        public void MatchCollectionsNeverNull()
        {
            var detector = new WordDetector(
                new WordDetectorSettings(
                    new string[] { "anywhere", "bat", "cat", "dog", "egg" }));

            var result = detector.Detect("I'll put cat and bat in here just for fun.");

            Assert.IsFalse(result.Values.Any(v => v == null));
        }
    }
}
