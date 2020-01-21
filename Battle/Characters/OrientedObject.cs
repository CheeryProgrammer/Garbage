using Battle.Common;

namespace Battle.Characters
{
	class OrientedObject : FieldObject
	{
		public Direction Direction { get; private set; }

		public OrientedObject(Point2D position) : base(position)
		{
		}

		internal override void MoveTo(Direction direction)
		{
			Direction = direction;
			base.MoveTo(direction);
		}
	}
}
