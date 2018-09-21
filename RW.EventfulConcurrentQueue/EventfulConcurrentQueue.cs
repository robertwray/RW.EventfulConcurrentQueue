using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RW.EventfulConcurrentQueue
{
    public sealed class EventfulConcurrentQueue<T>
    {
        private ConcurrentQueue<T> _queue;

        public EventfulConcurrentQueue()
        {
            _queue = new ConcurrentQueue<T>();
        }
        public EventfulConcurrentQueue(ConcurrentQueue<T> queue)
        {
            _queue = queue;
        }

        public void Enqueue(T item)
        {
            _queue.Enqueue(item);
            OnItemEnqueued();
        }

        public bool TryDequeue(out T result)
        {
            var success = _queue.TryDequeue(out result);

            if (success)
            {
                OnItemDequeued(result);
            }
            return success;
        }

        public event EventHandler ItemEnqueued;
        public event EventHandler<ItemDequeuedEventArgs<T>> ItemDequeued;

        void OnItemEnqueued()
        {
            ItemEnqueued?.Invoke(this, EventArgs.Empty);
        }

        void OnItemDequeued(T item)
        {
            ItemDequeued?.Invoke(this, new ItemDequeuedEventArgs<T> { Item = item });
        }

        /* Re-implementation pass through  */
        public bool IsEmpty => _queue.IsEmpty;
        public int Count => _queue.Count;
        public void CopyTo(T[] array, int index) => _queue.CopyTo(array, index);
        public IEnumerator<T> GetEnumerator() => _queue.GetEnumerator();
        public T[] ToArray() => _queue.ToArray();
        public bool TryPeek(out T result) => _queue.TryPeek(out result);
    }
}