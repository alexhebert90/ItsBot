using ItsBot;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ItsBotTests
{
    /// <summary>
    /// Tests around the <see cref="FixedSizeQueue{T}"/> class.
    /// </summary>
    [TestClass]
    public class FixedSizeQueueTests
    {
        [TestMethod]
        public void MaxSizeIsValidated()
        {
            // Max size of "0" is useless and therefore invalid.
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => 
                new FixedSizeQueue<int>(0));

            // Max size of "-1" does not make sense.
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                new FixedSizeQueue<int>(-1));

            // One valid case, to make sure that works.
            new FixedSizeQueue<int>(100);
        }

        [TestMethod]
        public void MaxCountMaintained()
        {
            const int TestSize = 50;

            var queue = new FixedSizeQueue<int>(TestSize);

            Assert.AreEqual(queue.MaxSize, TestSize);
        }

        [TestMethod]
        public void QueueInitializesToEmpty()
        {
            var queue = new FixedSizeQueue<int>(4);

            Assert.AreEqual(queue.Count, 0);
        }

        [TestMethod]
        public void AddItemsAndValidateCount()
        {
            const int MaxSize = 3;

            // Create a queue with 3 items to simplify test.
            var queue = new FixedSizeQueue<int>(MaxSize);
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

            var queue = new FixedSizeQueue<int>(MaxSize);

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

            foreach(var item in queue.Items)
            {
                Assert.AreEqual(item, currentItemShouldBe);
                currentItemShouldBe++;
            }
        }
    }
}
