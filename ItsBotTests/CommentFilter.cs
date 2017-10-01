using ItsBot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ItsBotTests
{
    [TestClass]
    public class CommentFilterTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Passed in comment data should not be allowed to be null.")]
        public void FilterInputNotNull()
        {
            new CommentFilter().Filter(null);
        }
    }
}
