using ItsBot;
using ItsBot.WordDetection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ItsBotTests
{
    /// <summary>
    /// Tests around the <see cref="CommentFilter"/> class.
    /// </summary>
    [TestClass]
    public class CommentFilterTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Word detector should not be null.")]
        public void WordDetectorNotNull()
        {
            new CommentFilter(wordDetector: null);
        }


        [TestMethod]
        public void FilterInputNotNull()
        {
            var filter = new CommentFilter(
                new WordDetector(
                    new WordDetectorSettings("Match")));

            // Testing this in line so that I am confident that the piece I want
            // to test is the piece throwing the exception.
            Assert.ThrowsException<ArgumentNullException>(() => 
                filter.Filter(null), "Passed in comment data should not be allowed to be null.");
        }
    }
}
