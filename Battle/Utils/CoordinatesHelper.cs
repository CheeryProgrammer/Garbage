using Battle.Common;
using System;

namespace Battle.Utils
{
	static class CoordinatesHelper
	{
		private static Random Rand = new Random();

		public static Point2D GenerateRandomPosition(int maxX, int maxY)
		{
			return new Point2D { X = Rand.Next(maxX) + 1, Y = Rand.Next(maxY) + 1 };
		}

		internal static Direction GetRandomDirection()
		{
			return (Direction)Rand.Next(4);
		}
	}
}
