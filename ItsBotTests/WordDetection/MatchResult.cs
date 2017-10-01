using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ItsBot.WordDetection;
using System.Text.RegularExpressions;

namespace ItsBotTests.WordDetection
{
    /// <summary>
    /// Tests around the <see cref="MatchResult"/> class.
    /// </summary>
    [TestClass]
    public class MatchResultTests
    {
        [TestMethod]
        public void ConstructorIndexValidatesRange()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                new MatchResult(-1, "Value"), "Negative index should not be allowed.");

            // But 0 should be allowed.
            new MatchResult(0, "Value");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Value should not be allowed to be null")]
        public void ConstructorValueNotNull()
        {
            new MatchResult(0, value: null);
        }

        [TestMethod]
        public void IndexAndValueMaintainedInConstructor()
        {
            const int Index = 3421;
            const string Value = "RandomText";

            var result = new MatchResult(Index, Value);

            Assert.AreEqual(result.Index, Index);
            Assert.AreEqual(result.Value, Value);
            Assert.AreEqual(result.Length, 10);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "This constructor should validate that its parameter is not null.")]
        public void MatchConstructorValidatesNull()
        {
            new MatchResult(match: null);
        }

        [TestMethod]
        public void MatchConstructorMapsData()
        {
            var regex = new Regex("bar");
            var match = regex.Match("foobar");

            var result = new MatchResult(match: match);

            Assert.AreEqual(result.Index, 3);
            Assert.AreEqual(result.Value, "bar");
            Assert.AreEqual(result.Length, 3);
        }

        [TestMethod]
        public void LengthBasedOnValueLength()
        {
            const string Val1 = "RandomWord";
            const string Val2 = "OtherWord";
            const string Val3 = "34kjasdfj fd";
            const string Val4 = "this is a thing";

            Assert.AreEqual(new MatchResult(0, Val1).Length, Val1.Length);
            Assert.AreEqual(new MatchResult(0, Val2).Length, Val2.Length);
            Assert.AreEqual(new MatchResult(0, Val3).Length, Val3.Length);
            Assert.AreEqual(new MatchResult(0, Val4).Length, Val4.Length);
        }
    }
}
