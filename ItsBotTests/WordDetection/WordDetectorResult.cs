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

        [TestMethod]
        public void AddResultParametersNotNull()
        {
            var result = new WordDetectorResult();

            Assert.ThrowsException<ArgumentNullException>(() =>
                result.AddResult(matchWord: null, matches: new MatchResultCollection()));

            Assert.ThrowsException<ArgumentNullException>(() =>
                result.AddResult(matchWord: "not null", matches: null));
        }

    }
}
