using System;
using System.Threading;
using NetGL;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var w = new Window("aaa");

            w.KeyUp += (s, e) =>
            {
                Console.WriteLine(e.Key);
                //Thread.Sleep(1000);
            };
            w.KeyDown += (s, e) =>
            {
                Console.WriteLine(e.Key);
                //Thread.Sleep(1000);
            };

            w.Show();
        }
    }
}
