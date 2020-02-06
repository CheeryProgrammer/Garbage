using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Labs
{
	[SimpleJob(RuntimeMoniker.Net472, baseline: true)]
	[RPlotExporter]
	[MemoryDiagnoser]
	public class BenchmarkContainer
	{
		private static readonly int[] AllDigits = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		public int[][] _matrix;

		public BenchmarkContainer()
		{
			_matrix = new int[3][]
			{
				new int[] { 786, 2, 123, 5 },
				new int[] { 784, 3, 123, 6 },
				new int[] { 58 , 4, 1 , 7 },
			};
		}

		[Benchmark]
		public int[][] RemoveColumns()
		{
			var maxCols = _matrix.Max(row => row.Length);

			var columns = new List<int?[]>();
			for (int colIndex = 0; colIndex < maxCols; colIndex++)
			{
				var column = _matrix.Select(row => colIndex < row.Length ? row[colIndex] : (int?)null);

				if (!HasCommonDigit(column))
					columns.Add(column.ToArray());
			}

			var rowsCount = _matrix.Length;

			int[][] result = new int[rowsCount][];

			for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
			{
				result[rowIndex] = columns.Select(col => col[rowIndex])
					.TakeWhile(element => element.HasValue)
					.Select(element => element.Value)
					.ToArray();
			}

			return result;
		}

		#region Helpers methods
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		bool HasCommonDigit(IEnumerable<int?> column)
		{
			HashSet<int> allDigits = new HashSet<int>(AllDigits);
			return column.Where(num => num.HasValue)
				.All(num =>
				{
					var digits = GetDigits(num.Value);
					allDigits.IntersectWith(digits);
					return allDigits.Any();
				});
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		HashSet<int> GetDigits(int number)
		{
			var digits = new HashSet<int>();
			while (number > 0)
			{
				digits.Add(number % 10);
				number /= 10;
			}
			return digits;
		}
		#endregion

		[Benchmark]
		public int[][] Task()
		{
			List<int> colsToStay = FindColsToStay(_matrix);
			int[][] newInt = new int[_matrix.Length][].Select(x => new int[colsToStay.Count]).ToArray();
			int k = 0;
			for (int j = 0; j < colsToStay.Count; j++)
			{
				for (int i = 0; i < _matrix.Length; i++)
					newInt[i][k] = _matrix[i][colsToStay[j]];
				k++;
			}
			return newInt;
		}

		public static List<int> FindColsToStay(int[][] matrix)
		{
			int i = 0;
			List<int> colsToStay = new List<int>();
			for (int j = 0; j < matrix[i].Length; j++)
			{
				List<int> sameNumbers = new List<int>();
				sameNumbers = sameNumbers.Concat(NumbersList(matrix[i][j])).ToList();
				i++;
				for (; i < matrix.Length; i++)
				{
					sameNumbers = sameNumbers.Intersect(NumbersList(matrix[i][j])).ToList();
					if (sameNumbers.Count == 0)
					{
						colsToStay.Add(j);
						break;
					}
				}
				i = 0;
			}
			return colsToStay;
		}

		private static List<int> NumbersList(int num)
		{
			List<int> result = new List<int>();
			while (num != 0)
			{
				result.Add(num % 10);
				num /= 10;
			}
			return result;
		}
	}
}
