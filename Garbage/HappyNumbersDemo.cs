using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garbage
{
	class HappyNumbersDemo
	{
		public static void Run()
		{
			var maxTestNumber = 10000;

			for (int i = 0; i < 100; i++)
			{
				Console.WriteLine($"{i} -> {GetMaxHappyNumber(i)}");
			}



			for (int i = 0; i < maxTestNumber; i++)
			{
				int maxHappyNumber = GetMaxHappyNumber(i - 1);
				int actual = CountHappyNumbers(maxHappyNumber);
				var expected = CountHappyNumbers_Test(i);
				if (actual != expected)
				{
					Console.WriteLine($"Test failed on input: {i}");
					break;
				}

				if (i == (maxTestNumber - 1))
				{
					Console.WriteLine($"Test is passed");
				}
			}
		}
		private static int GetMaxHappyNumber(int input)
		{
			var stack = new Stack<int>();
			var theHighestRankOfLessThan4Digit = -1;
			bool carryFlag = false;
			while (input > 0)
			{
				int digit = input % 10;

				if (carryFlag)
				{
					digit--;
					carryFlag = false;
				}

				if (digit < 4)
				{
					carryFlag = true;

					if (input < 10)
						digit = 0;

					theHighestRankOfLessThan4Digit = stack.Count;
				}

				stack.Push(digit);
				input /= 10;
			}

			if (stack.Count > 0 && stack.Peek() == 0)
				stack.Pop();

			int maxHappyNumber = 0;
			while (stack.Count > 0)
			{
				int nextDigit = stack.Pop();

				if (theHighestRankOfLessThan4Digit == stack.Count)
				{
					for (int i = 0; i <= stack.Count; i++)
					{
						maxHappyNumber += 7;
						if (i < stack.Count - 1)
							maxHappyNumber *= 10;
					}
					break;
				}

				maxHappyNumber += nextDigit >= 7 ? 7 : 4;

				if (stack.Count > 0)
					maxHappyNumber *= 10;
			}

			return maxHappyNumber;
		}

		static int CountHappyNumbers(int maxHappyNumber)
		{
			var count = 0;
			var rank = 1; // multiplier

			while (maxHappyNumber > 0)
			{
				int digit = maxHappyNumber % 10;
				count += (digit == 7 ? 2 : 1) * rank;
				rank <<= 1;
				maxHappyNumber /= 10;
			}

			return count;
		}

		private static int CountHappyNumbers_Test(int input)
		{
			int count = 0;
			for (int i = 0; i < input; i++)
			{
				if (i.ToString().All(c => c == '4' || c == '7'))
					count++;
			}
			return count;
		}
	}

	struct Point
	{
		public int X { get; set; }
		public int Y { get; set; }
	}
}
