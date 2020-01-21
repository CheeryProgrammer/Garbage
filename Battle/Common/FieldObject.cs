namespace Battle.Common
{
	class FieldObject
	{
		public Point2D Position { get; }
	
		public FieldObject(Point2D position)
		{
			Position = position;
		}

		internal virtual void MoveTo(Direction direction)
		{
			switch (direction)
			{
				case Direction.Up:
					Position.Y--;
					break;
				case Direction.Right:
					Position.X++;
					break;
				case Direction.Down:
					Position.Y++;
					break;
				case Direction.Left:
					Position.X--;
					break;
				case Direction.None:
					break;
			};
		}
	}
}
