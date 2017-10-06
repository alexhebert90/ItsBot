using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ItsBot.Tests
{
    /// <summary>
    /// Tests against the <see cref="HashedFixedSizeQueue{T}"/> class.
    /// </summary>
    [TestClass]
    public class HashedFixedSizeQueueTests
    {
        #region Hash Tests

        [TestMethod]
        public void ContainsWorksAsExpected()
        {
            var queue = new HashedFixedSizeQueue<string>(3);

            const string Word1 = "Out of ideas.";
            const string Word2 = "Life";
            const string Word3 = "Tired.";
            const string Word4 = " <pointless text>";
            const string Word5 = "... is this enough?";

            queue.Enqueue(Word1);
            Assert.IsTrue(queue.Contains(Word1));

            queue.Enqueue(Word2);
            Assert.IsTrue(queue.Contains(Word2));

            queue.Enqueue(Word3);
            Assert.IsTrue(queue.Contains(Word3));

            // Making sure somehow nothing got modified...
            Assert.IsTrue(queue.Contains(Word1));
            Assert.IsTrue(queue.Contains(Word2));
            Assert.IsTrue(queue.Contains(Word3));

            // Here, things should start getting deleted.

            queue.Enqueue(Word4);
            Assert.IsTrue(queue.Contains(Word4));
            Assert.IsFalse(queue.Contains(Word1));

            queue.Enqueue(Word5);
            Assert.IsTrue(queue.Contains(Word5));
            Assert.IsFalse(queue.Contains(Word2));

            // Wrap it up. Yeah, it's a little redundant, but also extra safe.
            Assert.IsTrue(queue.Contains(Word3));
            Assert.IsTrue(queue.Contains(Word4));
            Assert.IsTrue(queue.Contains(Word5));

            Assert.IsFalse(queue.Contains(Word1));
            Assert.IsFalse(queue.Contains(Word2));
        }




        #endregion Hash Tests


        // Not proud of this, but all of the tests below
        // are dupes from the FixedSizeQueue tests.

        // I'll have to circle back on sharing the logic; I couldn't
        // easily come up with a plan. Generic constructor constraints honestly
        // would help a ton.

        #region Base Class Tests

        [TestMethod]
        public void MaxSizeIsValidated()
        {
            // Max size of "0" is useless and therefore invalid.
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                new HashedFixedSizeQueue<int>(0));

            // Max size of "-1" does not make sense.
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                new HashedFixedSizeQueue<int>(-1));

            // One valid case, to make sure that works.
            new HashedFixedSizeQueue<int>(100);
        }

        [TestMethod]
        public void MaxCountMaintained()
        {
            const int TestSize = 50;

            var queue = new HashedFixedSizeQueue<int>(TestSize);

            Assert.AreEqual(queue.MaxSize, TestSize);
        }

        [TestMethod]
        public void QueueInitializesToEmpty()
        {
            var queue = new HashedFixedSizeQueue<int>(4);

            Assert.AreEqual(queue.Count, 0);
        }

        [TestMethod]
        public void AddItemsAndValidateCount()
        {
            const int MaxSize = 3;

            // Create a queue with 3 items to simplify test.
            var queue = new HashedFixedSizeQueue<int>(MaxSize);
            Assert.AreEqual(queue.Count, 0);

            queue.Enqueue(1);
            Assert.AreEqual(queue.Count, 1);

            queue.Enqueue(2);
            Assert.AreEqual(queue.Count, 2);

            queue.Enqueue(3);
            Assert.AreEqual(queue.Count, 3);

            // We've passed the max limit at this point.

            queue.Enqueue(4);
            Assert.AreEqual(queue.Count, MaxSize);

            queue.Enqueue(5);
            Assert.AreEqual(queue.Count, MaxSize);
        }

        [TestMethod]
        public void ValidateItemOrdering()
        {
            const int MaxSize = 5;

            var queue = new HashedFixedSizeQueue<int>(MaxSize);

            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            queue.Enqueue(4);
            queue.Enqueue(5);

            // Just because it's easy, validate the count is accurate before continuing.
            Assert.AreEqual(queue.Count, MaxSize);

            queue.Enqueue(6);
            queue.Enqueue(7);

            // Make sure there are still only 5 elements.
            Assert.AreEqual(queue.Count, MaxSize);

            // Now "1" and "2" should have been popped off,
            // so the next item should be "3."

            int currentItemShouldBe = 3;

            foreach (var item in queue.Items)
            {
                Assert.AreEqual(item, currentItemShouldBe);
                currentItemShouldBe++;
            }
        }

        #endregion Base Class Tests


    }
}
