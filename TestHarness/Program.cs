using RW.EventfulConcurrentQueue;
using System;

namespace TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            var queue = new EventfulConcurrentQueue<Item>();
            var feeder1 = new Feeder(queue, 1);
            var feeder2 = new Feeder(queue, 2);
            var feeder3 = new Feeder(queue, 3);

            var reader = new Reader(queue);

            reader.Start();
            feeder1.Start();
            feeder2.Start();
            feeder3.Start();
            Console.WriteLine("Started - Press ENTER to start stopping...");
            Console.ReadLine();
            feeder1.Stop();
            feeder2.Stop();
            feeder3.Stop();
            reader.Stop();
            while (feeder1.Running && feeder2.Running && feeder3.Running && reader.Running)
            {
                System.Threading.Thread.Sleep(50);
                Console.WriteLine("Waiting for feeder and reader to stop");
            }
            Console.WriteLine($"Items remaining: {queue.Count}");
            Console.WriteLine("Stopped - Press ENTER to close...");
            Console.ReadLine();
        }
    }
}
