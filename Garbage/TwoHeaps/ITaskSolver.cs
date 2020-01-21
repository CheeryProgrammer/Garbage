using System.Collections.Generic;

namespace Garbage.TwoHeaps
{
	interface ITaskSolver
	{
		IReadOnlyCollection<int> First { get; }
		IReadOnlyCollection<int> Second { get; }
		int IterationsCount { get; }
		void Run();
	}
}
