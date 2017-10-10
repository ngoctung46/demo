using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            /*Example 1 : Multi thread
            Thread t = new Thread(PrintNumbers); // Create new thread
            t.Start(); // Start newly created thread
            PrintNumbers(); // Start from main thread
            */

            /*Example 2: Make theard wait for another thread finish before executing
            Thread t = new Thread(PrintNumbersWithDelay);
            t.Start(); // start a thread
            t.Join(); // wait for the completion
            PrintNumbersWithDelay(); */

            /*Example 3: Abort a thread
            Thread t = new Thread(PrintNumbersWithDelay);
            t.Start();
            Thread.Sleep(TimeSpan.FromSeconds(6)); // Make main thread sleep for 6 second and abort the theard t after that
            t.Abort();
            Console.WriteLine($"Thread {t.ManagedThreadId} has been aborted by Thread {Thread.CurrentThread.ManagedThreadId}");*/

            /*Example 4: Get Thread Info
            Thread t1 = new Thread(() => PrintNumbersWithStatus(1000));
            Thread t2 = new Thread(() => PrintNumbersWithStatus(3000));
            Console.WriteLine($"t1 State: { t1.ThreadState.ToString()}");
            Console.WriteLine($"t2 State: { t2.ThreadState.ToString()}");
            t2.Start();
            Console.WriteLine("t2 started");
            t1.Start();
            Console.WriteLine("t1 started");
            Thread.Sleep(6000); // wait 6 second
            t1.Abort();
            Console.WriteLine($"t1 State: { t1.ThreadState.ToString()}");
            Console.WriteLine($"t2 State: { t2.ThreadState.ToString()}");*/

            /*Example 5: Theard priority
            Console.WriteLine($"Current thread priority: {Thread.CurrentThread.Priority}");
            Console.WriteLine("Running on all cores available");
            RunThreads();
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Console.WriteLine("Running on a single core");
            Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(1); // Set to single core cpu
            RunThreads();*/

            /*Example 6: Foreground and background thread
            var sampleForeground = new ThreadSample1(10);
            var sampleBackground = new ThreadSample1(20);

            var threadOne = new Thread(sampleForeground.CountNumbers);
            threadOne.Name = "Foreground thread";
            var threadTwo = new Thread(sampleBackground.CountNumbers);
            threadTwo.Name = "Background thread";
            threadTwo.IsBackground = true;

            threadOne.Start();
            threadTwo.Start(); */

            /*Example 7: Thread Safe
            Console.WriteLine("Incorrect counter:");
            var c = new Counter();
            var t1 = new Thread(() => TestCounter(c));
            var t2 = new Thread(() => TestCounter(c));
            var t3 = new Thread(() => TestCounter(c));
            t1.Start();
            t2.Start();
            t3.Start();
            t1.Join();
            t2.Join();
            t3.Join();
            Console.WriteLine($"Total Count: {c.Count}");
            Console.WriteLine("-------------------------");
            Console.WriteLine("Correct Counter");
            var c1 = new CounterWithLock();
            t1 = new Thread(() => TestCounter(c1));
            t2 = new Thread(() => TestCounter(c1));
            t3 = new Thread(() => TestCounter(c1));
            t1.Start();
            t2.Start();
            t3.Start();
            t1.Join();
            t2.Join();
            t3.Join();
            Console.WriteLine($"Total count: {c1.Count}"); */
        }

        // Example 1 Method
        private static void PrintNumbers()
        {
            Console.WriteLine($"Starting From Thread: {Thread.CurrentThread.ManagedThreadId}");
            for (int i = 1; i < 10; i++)
            {
                Console.WriteLine($"Thread Id { Thread.CurrentThread.ManagedThreadId} - Value: {i}");
            }
        }

        // Example 2 Method
        private static void PrintNumbersWithDelay()
        {
            Console.WriteLine($"Starting From Theard: {Thread.CurrentThread.ManagedThreadId}");
            for (int i = 1; i < 10; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine($"Thread Id { Thread.CurrentThread.ManagedThreadId} - Value: {i}");
            }
        }

        // Example 4 Method
        private static void PrintNumbersWithStatus(int time)
        {
            Console.WriteLine($"Starting From Theard: {Thread.CurrentThread.ManagedThreadId} - State {Thread.CurrentThread.ThreadState.ToString()}");
            for (int i = 1; i < 10; i++)
            {
                Thread.Sleep(time);
                Console.WriteLine($"Thread Id { Thread.CurrentThread.ManagedThreadId} - Value: {i}");
            }
        }

        // Example 5 Method
        private static void RunThreads()
        {
            var sample = new ThreadSample();
            var threadOne = new Thread(sample.CountNumbers);
            threadOne.Name = "Thread One";
            var threadTwo = new Thread(sample.CountNumbers);
            threadTwo.Name = "Thread Two";

            threadOne.Priority = ThreadPriority.Highest;
            threadTwo.Priority = ThreadPriority.Lowest;
            threadOne.Start();
            threadTwo.Start();
            Thread.Sleep(TimeSpan.FromSeconds(2));
            sample.Stop();
        }

        // Example 7
        private static void TestCounter(CounterBase c)
        {
            for (int i = 0; i < 100000; i++)
            {
                c.Increment();
                c.Decrement();
            }
        }
    }

    // Example 5 class
    internal class ThreadSample
    {
        private bool _isStopped = false;

        public void Stop() => _isStopped = true;

        public void CountNumbers()
        {
            long counter = 0;
            while (!_isStopped)
            {
                counter++;
            }
            Console.WriteLine($"{Thread.CurrentThread.Name} with {Thread.CurrentThread.Priority,11 } priority has a count = {counter,13:N0}");
        }
    }

    // Example 6
    internal class ThreadSample1
    {
        private readonly int _iteration;

        public ThreadSample1(int iteration) => _iteration = iteration;

        public void CountNumbers()
        {
            for (int i = 0; i < _iteration; i++)
            {
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
                Console.WriteLine($"{Thread.CurrentThread.Name} prints { i }");
            }
        }
    }

    internal class CounterWithLock : CounterBase
    {
        private readonly object _syncRoot = new Object();
        public int Count { get; set; }

        public override void Increment()
        {
            lock (_syncRoot)
            {
                Count++;
            }
        }

        public override void Decrement()
        {
            lock (_syncRoot)
            {
                Count--;
            }
        }
    }

    internal class Counter : CounterBase
    {
        public int Count { get; private set; }

        public override void Increment()
        {
            Count++;
        }

        public override void Decrement()
        {
            Count--;
        }
    }

    internal abstract class CounterBase
    {
        public abstract void Increment();

        public abstract void Decrement();
    }
}