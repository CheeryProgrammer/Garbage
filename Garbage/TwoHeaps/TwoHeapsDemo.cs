using System;
using System.Collections.Generic;
using System.Linq;

namespace Garbage.TwoHeaps
{
	class TwoHeapsDemo
	{
		private static Random Rand = new Random();
		private static ConsoleColor DefaultForegroundColor = Console.ForegroundColor;

		internal void Run()
		{
			//List<int> numbers = new List<int> { 1, 2, 2, 3 };
			foreach (List<int> testCase in GetTestCases())
			{
				/*ITaskSolver solver = new FullBruteForce(testCase);
				solver.Run();
				PrintResults(solver);*/

				ITaskSolver solver = new BruteForceOptimizedSolver(testCase);
				solver.Run();
				PrintResults(solver);

				solver = new BruteForceOptimizedSolverPlus(testCase);
				solver.Run();
				PrintResults(solver);

				Console.WriteLine("____________________________________________________________\n");
			}
		}

		private static IEnumerable<List<int>> GetTestCases()
		{
			/*yield return new List<int> { 1, 2, 2, 3 };
			yield return new List<int> { 8, 16, 59, 37, 15, 28, 41, 32, 9, 10 };
			yield return new List<int> { 10, 10, 10, 10, 22, 18 };
			yield return new List<int> { 10, 10, 10, 10, 18, 22 };*/
			//yield return new List<int> { -92, -59, -44, -17, 17, 35, 56 };
			for (int i = 0; i < 1; i++)
			{
				yield return Enumerable.Range(0, 40)
					.Select(el => Rand.Next(-100000, 100000))
					.ToList();
			}
		}

		private static void PrintResults(ITaskSolver solver)
		{
			Console.WriteLine($"Iterations: {solver.IterationsCount}");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"Difference: {Math.Abs(solver.First.Sum() - solver.Second.Sum())}");
			Console.ForegroundColor = DefaultForegroundColor;
			Console.WriteLine($"First heap: sum({solver.First.Sum()}); values {{{string.Join(", ", solver.First)}}}");
			Console.WriteLine($"Second heap: sum({solver.Second.Sum()}); values {{{string.Join(", ", solver.Second)}}}");
			Console.WriteLine();
		}
	}
}
