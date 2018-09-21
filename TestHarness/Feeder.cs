using RW.EventfulConcurrentQueue;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestHarness
{
    public sealed class Feeder
    {
        public bool Running { get; private set; }
        public int FeederId { get; set; }
        private Task task { get; set; }
        private EventfulConcurrentQueue<Item> Queue { get; set; }

        public Feeder(EventfulConcurrentQueue<Item> queue, int feederId)
        {
            Queue = queue;
            FeederId = feederId;
        }
        public void Stop()
        {
            Running = false;
            Queue.ItemDequeued -= Queue_ItemDequeued;
        }

        public void Start()
        {
            Running = true;
            task = new Task(DoWork);
            task.Start();
            Queue.ItemDequeued += Queue_ItemDequeued;
        }

        public void Queue_ItemDequeued(object sender, ItemDequeuedEventArgs<Item> e)
        {
            if (e.Item.FeederId == FeederId)
            {
                Console.WriteLine($"Item fed by me has been removed from queue: {e.Item.Value}");
            }
        }

        private void DoWork()
        {
            var id = 0;
            while (Running)
            {
                Queue.Enqueue(new Item { FeederId = FeederId, Stamp = DateTime.Now, Value = $"Feeder {FeederId} Item id {id}" });
                Thread.Sleep(FeederId * 500);
                id++;
            }
        }
    }
}
