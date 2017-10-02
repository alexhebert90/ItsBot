using System.Collections.Generic;

namespace ItsBot
{
    /// <summary>
    /// Fixed size queue implementation that is backed by a <see cref="HashSet{T}"/>.
    /// This has the benefit of providing constant lookup times at the expense of slightly
    /// more memory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class HashedFixedSizeQueue<T> : FixedSizeQueue<T>
    {
        private HashSet<T> BackingHash { get; }

        public HashedFixedSizeQueue(int maxSize) : 
            base(maxSize)
        {
            // I guess you can't allocate a size at the constructor level. Bummer.
            BackingHash = new HashSet<T>();
        }

        public override void Enqueue(T item)
        {
            base.Enqueue(item);
            BackingHash.Add(item);
        }

        public bool Contains(T item)
            => BackingHash.Contains(item);

        protected override T Dequeue()
        {
            T outObj = base.Dequeue();
            BackingHash.Remove(outObj);
            return outObj;
        }

    }
}
