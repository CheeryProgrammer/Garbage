using System;
using System.Linq;

namespace Garbage
{
	internal class OneZeroDemo
	{
		internal static void Run()
		{
			var rnd = new Random();
			var numbers = Enumerable.Range(0, 20).Select(_ => rnd.Next(0, 2)).Concat(new int[]{ 0,0,0,0,0,0}).ToArray();
			var numbersString = string.Concat(numbers);
			Console.WriteLine(numbersString);

			var maxZerosLength = numbers.Aggregate(new { Max = 0, Current = 0 }, (state, n) => new { Max = Math.Max(state.Max, state.Current), Current = n == 0 ? state.Current + 1 : 0 }, s => Math.Max(s.Max, s.Current));
			Console.WriteLine($"Max 0-length: {maxZerosLength}");
		}
	}
}