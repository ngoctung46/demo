using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            /* Example 1: Basic async operation
            Task t = AsynchronyWithAwait();
            t.Wait(); */

            /* Example 2: Using async with lambda
            Task t = AsyncProcessing();
            t.Wait(); */

            /*ex 3 */
            Task t = AsynchronyWithAwait();
            t.Wait();
        }

        private static async Task AsynchronyWithAwait()
        {
            try
            {
                string result = await GetInfoAsync("Async Task 1");
                Console.WriteLine(result);
                //result = await GetInfoAsync("Async Task 2");
                //Console.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static async Task AsyncProcessing()
        {
            Func<string, Task<string>> asyncLambda = async name =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2));
                return $"Task {name} is running on a thread id {Thread.CurrentThread.ManagedThreadId}." +
                       $"Is thread pool thread: {Thread.CurrentThread.IsThreadPoolThread}";
            };
            string result = await asyncLambda("async Lambda");
            Console.WriteLine(result);
        }

        private static async Task<string> GetInfoAsync(string name)
        {
            await Task.Delay((TimeSpan.FromSeconds(2)));
            return $"Task {name} is running on a thread id {Thread.CurrentThread.ManagedThreadId}." +
                   $"Is thread pool thread: {Thread.CurrentThread.IsThreadPoolThread}";
        }
    }
}