using System;
using System.Diagnostics;

namespace PokerAI
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Table table = new Table();
            table.Play();
            watch.Stop();
            Console.WriteLine(
                string.Format("Minutes :{0}\nSeconds :{1}\n Mili seconds :{2}",
                watch.Elapsed.Minutes, watch.Elapsed.Seconds, watch.Elapsed.TotalMilliseconds));
            int i = 0;
		}
	}
}
