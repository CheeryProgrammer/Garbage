using Battle.Common;
using System;

namespace Battle.Characters
{
	class Enemy : OrientedObject
	{
		private static Random Rand = new Random();

		public bool CanMove
		{
			get
			{
				return DateTime.Now > _lastMovementTime.AddMilliseconds(_restTimeMs);
			}
		}

		private DateTime _lastMovementTime;
		private int _restTimeMs;

		public Enemy(Point2D position) : base(position)
		{
			_restTimeMs = Rand.Next(300, 1500);
			_lastMovementTime = DateTime.Now;
		}

		internal override void MoveTo(Direction direction)
		{
			if (CanMove)
			{
				_lastMovementTime = DateTime.Now;
				base.MoveTo(direction);
			}
		}
	}
}
