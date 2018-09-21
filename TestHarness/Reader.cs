using RW.EventfulConcurrentQueue;
using System;

namespace TestHarness
{
    public sealed class Reader
    {
        public bool Running { get; private set; }
        private EventfulConcurrentQueue<Item> queue { get; set; }

        public Reader(EventfulConcurrentQueue<Item> queue) => this.queue = queue;

        public void Start()
        {
            Running = true;
            queue.ItemEnqueued += Queue_ItemEnqueued;
        }

        public void Stop()
        {
            queue.ItemEnqueued -= Queue_ItemEnqueued;
            Running = false;
        }

        private void Queue_ItemEnqueued(object sender, EventArgs e)
        {
            if (queue.TryDequeue(out Item item))
            {
                Console.WriteLine($"Item found: {item.Stamp} - {item.Value}");
            }
        }
    }
}
