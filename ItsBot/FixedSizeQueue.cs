using System;
using System.Collections.Generic;

namespace ItsBot
{
    // This will likely move to a new assembly where it can be publicly available.

    internal class FixedSizeQueue<T>
    {
        public FixedSizeQueue(int maxSize)
        {
            if (maxSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxSize), "Size of queue may not be zero or negative.");

            MaxSize = maxSize;

            // Pro-actively sets the queue start size to the requested size.
            BackingQueue = new Queue<T>(MaxSize);
        }


        public virtual void Enqueue(T item)
        {
            BackingQueue.Enqueue(item);

            if (BackingQueue.Count > MaxSize)
                Dequeue();
        }

        // The items could technically expose the original collection
        // to callers, but I'm not going to worry about that extreme edge case for
        // this project.

        /// <summary>
        /// Exposes the items in the queue to callers.
        /// </summary>
        public IEnumerable<T> Items
            => BackingQueue;

        /// <summary>
        /// Returns the current number of items in the queue.
        /// </summary>
        public int Count
            => BackingQueue.Count;


        protected virtual T Dequeue()
        {
            return BackingQueue.Dequeue();
        }

        /// <summary>
        /// Holds the backing queue behind the instance.
        /// </summary>
        private Queue<T> BackingQueue { get; }

        /// <summary>
        /// Holds the queue's maximum size.
        /// </summary>
        public int MaxSize { get; }
    }
}
