using ItsBot.WordDetection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ItsBotTests.WordDetection
{
    /// <summary>
    /// Tests around the <see cref="MatchResultCollection"/> class.
    /// </summary>
    [TestClass]
    public class MatchResultCollectionTests
    {
        [TestMethod]
        public void EmptyConsructor()
        {
            new MatchResultCollection();
        }

        [TestMethod]
        public void EmptyConstructorPopulatesEmptyCollection()
        {
            var newCollection = new MatchResultCollection();

            Assert.IsNotNull(newCollection.Collection);
            Assert.AreEqual(0, newCollection.Collection.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Enumeration constructor should not accept null.")]
        public void EnumeratedConstructorNotNull()
        {
            new MatchResultCollection(matchCollection: (IEnumerable<Match>)null);
        }

        [TestMethod]
        public void MatchCollectionConstructor()
        {
            var regex = new Regex("bar");
            var matches = regex.Matches("bar bar bar bar");

            var collection = new MatchResultCollection(matches);
            Assert.AreEqual(collection.Collection.Count, 4);
        }

        [TestMethod]
        public void CollectionImmutable()
        {
            var collection = new MatchResultCollection();
        }
    }
}
