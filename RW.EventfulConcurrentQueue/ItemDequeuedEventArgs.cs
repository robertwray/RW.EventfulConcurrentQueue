using System;

namespace RW.EventfulConcurrentQueue
{
    public sealed class ItemDequeuedEventArgs<T> : EventArgs
    {
        public T Item { get; set; }
    }
}
