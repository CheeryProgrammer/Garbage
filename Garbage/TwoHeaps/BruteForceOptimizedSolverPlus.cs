﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Garbage.TwoHeaps
{
	class BruteForceOptimizedSolverPlus : ITaskSolver
	{
		public IReadOnlyCollection<int> First => _firstHeap;
		public IReadOnlyCollection<int> Second => _secondHeap;

		private readonly List<int> _numbers;
		private int _halfSum;
		private int _minimalDiff;
		private List<int> _firstHeap;
		private List<int> _secondHeap;
		private int _callsCount;

		public BruteForceOptimizedSolverPlus(List<int> numbers)
		{
			_numbers = numbers;
			_numbers.Sort();
		}

		public int IterationsCount => _callsCount;

		public void Run()
		{
			_minimalDiff = Math.Abs(_numbers.Sum());
			_halfSum = _minimalDiff / 2;

			FindWithFullBruteForce(_numbers, 0, 0);

			_firstHeap = _firstHeap ?? _numbers;

			_secondHeap = new List<int>(_numbers);

			foreach (int num in _firstHeap)
				_secondHeap.Remove(num);
		}

		private void FindWithFullBruteForce(List<int> firstHeap, int sum, int startIndex)
		{
			_callsCount++;

			int prevDiff = int.MaxValue;

			for (int i = startIndex; i < firstHeap.Count; i++)
			{
				List<int> newFirstHeap = firstHeap.Take(i)
					.Concat(firstHeap.Skip(i + 1).Take(firstHeap.Count))
					.ToList();
				int secondHeapSum = sum + firstHeap[i];
				int diff = Math.Abs(newFirstHeap.Sum() - secondHeapSum);
				
				if (diff > prevDiff)
					continue;

				prevDiff = diff;

				if (diff < _minimalDiff)
				{
					_minimalDiff = diff;
					_firstHeap = newFirstHeap;
					if (diff == 0)
						return;
				}

				FindWithFullBruteForce(newFirstHeap, secondHeapSum, i);
			};
		}
	}
}
