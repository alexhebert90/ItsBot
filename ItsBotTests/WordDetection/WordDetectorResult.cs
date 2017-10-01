using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using ItsBot.WordDetection;
using System.Text.RegularExpressions;

namespace ItsBotTests.WordDetection
{
    /// <summary>
    /// Tests around the <see cref="WordDetectorResult"/> class.
    /// </summary>
    [TestClass]
    public class WordDetectorResultTests
    {
        /// <summary>
        /// The most straightforward test I can imagine.
        /// </summary>
        [TestMethod]
        public void ConstructorRuns()
        {
            new WordDetectorResult();
        }

        /// <summary>
        /// Validating <see cref="WordDetectorResult.AddResult(string, MatchResultCollection)"/>
        /// parameters are verified properly.
        /// </summary>
        [TestMethod]
        public void AddResultParametersNotNull()
        {
            var result = new WordDetectorResult();

            Assert.ThrowsException<ArgumentNullException>(() =>
                result.AddResult(matchWord: null, matches: new MatchResultCollection()));

            Assert.ThrowsException<ArgumentNullException>(() =>
                result.AddResult(matchWord: "not null", matches: null));
        }


        [TestMethod]
        public void VerifyMatchWordCount()
        {
            var result = new WordDetectorResult();

            // Making sure the count increases with each added word.
            // Try for an arbitrary number of cycles.
            for(int x = 1; x <=15; x++)
            {
                result.AddResult(x.ToString(), new MatchResultCollection());
                Assert.AreEqual(x, result.MatchWordCount);
            }
        }

        [TestMethod]
        public void VerifyMatchWordCountCaseInsensitive()
        {
            var result = new WordDetectorResult();

            int count = 1;
            for(char let = 'a'; let <= 'z'; let++)
            {
                // Though we're adding two results, the total count should only increase by 1.
                result.AddResult(let.ToString().ToLower(), new MatchResultCollection());
                result.AddResult(let.ToString().ToUpper(), new MatchResultCollection());

                Assert.AreEqual(count, result.MatchWordCount);
                count++;
            }
        }

        // ToDo: Add more tests.
    }
}
