using ItsBot.WordDetection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ItsBot.Tests.WordDetection
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

            Assert.AreEqual(result.MatchWordCount, 4);
        }

        /// <summary>
        /// Make sure that the output is tied to the keys supplied in the settings.
        /// </summary>
        [TestMethod]
        public void ResultKeysMatchInput()
        {
            const string Key1 = "Lopsided";
            const string Key2 = "Forever here";
            const string Key3 = "audio.";

            const string KeyNotInInstance = "I'm Not Used";

            var detector = new WordDetector(
                new WordDetectorSettings(
                    new string[] { Key1, Key2, Key3 }));

            var result = detector.Detect("This text doesn't matter, yet again.");

            // We expect non-null results for all keys we passed into the instance.
            Assert.IsNotNull(result.GetMatchesFor(Key1));
            Assert.IsNotNull(result.GetMatchesFor(Key2));
            Assert.IsNotNull(result.GetMatchesFor(Key3));

            // But for a key that was not passed into the instance, we do expect null.
            Assert.IsNull(result.GetMatchesFor(KeyNotInInstance));
        }


        // ToDo: This test no longer makes sense and will be validated by validating the
        // WordDetectorResult constructor arguments.

        ///// <summary>
        ///// Just verifying that every match object gotten back is not assigned to a null reference.
        ///// </summary>
        //[TestMethod]
        //public void MatchCollectionsNeverNull()
        //{
        //    var detector = new WordDetector(
        //        new WordDetectorSettings(
        //            new string[] { "anywhere", "bat", "cat", "dog", "egg" }));

        //    var result = detector.Detect("I'll put cat and bat in here just for fun.");

        //    Assert.IsFalse(result.Values.Any(v => v == null));
        //}

        [TestMethod]
        public void SingleWordTest()
        {
            const string SearchTerm = "life";

            var detector = new WordDetector(
                new WordDetectorSettings(SearchTerm));

            // Only whole words should be matched case insensitively, so this text should contain
            // 3 matches.
            const string TestSentence = 
                "Life can be a challenge, which is why LifeTime is now offering" +
                " a new life package deal to enhance your life. What a time to be alive.";

            var result = detector.Detect(TestSentence).GetMatchesFor(SearchTerm).Collection;

            // Verify total count.
            Assert.AreEqual(result.Count, 3);

            // Yes, I did count these indexes by hand. Felt pretty stupid while doing it, too,
            // but by the end of the first line I realized it was too late to give up.
            Assert.AreEqual(result[0].Index, 0);
            Assert.AreEqual(result[0].Value, "Life");

            Assert.AreEqual(result[1].Index, 69);
            Assert.AreEqual(result[1].Value, "life");

            Assert.AreEqual(result[2].Index, 103);
            Assert.AreEqual(result[2].Value, "life");
        }

        [TestMethod]
        public void MultiWordAdvanced()
        {
            const string Search1 = "Pumpkin";
            const string Search2 = "Pineapple";

            var detector = new WordDetector(
                new WordDetectorSettings(new string[] { Search1, Search2 }));

            const string TestSentence =
                "Pumpkin Pineapple pumpkin pineapple pUmpkin pineApple " +
                "pumpkin pIneapple pumpkin. Pineapple. But seriously, pumpkin.";

            var result = detector.Detect(TestSentence);

            var pumpkin = result.GetMatchesFor(Search1).Collection;
            var pineapple = result.GetMatchesFor(Search2).Collection;

            // I also did these indexes by hand and using math...impressed yet?

            // Pumpkin
            Assert.AreEqual(pumpkin.Count, 6);

            Assert.AreEqual(pumpkin[0].Index, 0);
            Assert.AreEqual(pumpkin[0].Value, "Pumpkin");

            Assert.AreEqual(pumpkin[1].Index, 18);
            Assert.AreEqual(pumpkin[1].Value, "pumpkin");

            Assert.AreEqual(pumpkin[2].Index, 36);
            Assert.AreEqual(pumpkin[2].Value, "pUmpkin");

            Assert.AreEqual(pumpkin[3].Index, 54);
            Assert.AreEqual(pumpkin[3].Value, "pumpkin");

            Assert.AreEqual(pumpkin[4].Index, 72);
            Assert.AreEqual(pumpkin[4].Value, "pumpkin");

            Assert.AreEqual(pumpkin[5].Index, 107);
            Assert.AreEqual(pumpkin[5].Value, "pumpkin");

            //==============================
            // Pineapple

            Assert.AreEqual(pineapple.Count, 5);

            Assert.AreEqual(pineapple[0].Index, 8);
            Assert.AreEqual(pineapple[0].Value, "Pineapple");
                            
            Assert.AreEqual(pineapple[1].Index, 26);
            Assert.AreEqual(pineapple[1].Value, "pineapple");
                            
            Assert.AreEqual(pineapple[2].Index, 44);
            Assert.AreEqual(pineapple[2].Value, "pineApple");
                            
            Assert.AreEqual(pineapple[3].Index, 62);
            Assert.AreEqual(pineapple[3].Value, "pIneapple");
                            
            Assert.AreEqual(pineapple[4].Index, 81);
            Assert.AreEqual(pineapple[4].Value, "Pineapple");                         
        }
    }
}
