using System;

namespace Battle.Common
{
	class Point2D
	{
		public int X { get; set; }
		public int Y { get; set; }

		internal Point2D Clone()
		{
			return new Point2D { X = X, Y = Y };
		}

		public override bool Equals(object obj)
		{
			if (base.Equals(obj))
				return true;

			Point2D otherPoint = obj as Point2D;
			return otherPoint != null && X == otherPoint.X && Y == otherPoint.Y;
		}

		internal static Point2D CreateNear(Point2D position, Direction direction)
		{
			var neighbour = position.Clone();

			switch (direction)
			{
				case Direction.Up:
					neighbour.Y--;
					break;
				case Direction.Right:
					neighbour.X++;
					break;
				case Direction.Down:
					neighbour.Y++;
					break;
				case Direction.Left:
					neighbour.X--;
					break;
			}

			return neighbour;
		}
	}
}
