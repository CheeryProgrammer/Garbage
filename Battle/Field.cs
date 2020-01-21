using Battle.Characters;
using Battle.Common;
using Battle.Utils;
using Battle.Views;
using System.Collections.Generic;
using System.Linq;

namespace Battle
{
	class Field
	{
		public bool IsPlayerAlive { get; private set; }

		private readonly int _width;
		private readonly int _height;
		private readonly RenderingPlan _renderingPlan;
		private List<Enemy> _enemies;
		private Player _player;

		public Field(int width, int height, RenderingPlan renderingPlan)
		{
			_width = width;
			_height = height;
			_renderingPlan = renderingPlan;
			_player = new Player(new Point2D { X = 2, Y = 2 });
			IsPlayerAlive = true;
		}

		public void AddEnemies(int count)
		{
			_enemies = new List<Enemy>(count);
			for (int i = 0; i < count; i++)
			{
				var position = CoordinatesHelper.GenerateRandomPosition(_width - 1, _height - 1);
				_enemies.Add(new Enemy(position));
			}
		}

		internal void Update()
		{
			for (int i = 0; i < _enemies.Count; i++)
			{
				Point2D from = _enemies[i].Position.Clone();
				Point2D to = from;
				if(_enemies[i].CanMove)
				{
					var moveDirection = CoordinatesHelper.GetRandomDirection();
					_enemies[i].MoveTo(moveDirection);

					CorrectPosition(_enemies[i]);

					to = _enemies[i].Position.Clone();

					if (TryKillPlayer(_enemies[i]))
						IsPlayerAlive = false;
				}
				_renderingPlan.Add(from, to, new EnemyView(_enemies[i].Direction));
			}
			_renderingPlan.Add(_player.Position, _player.Position, new PlayerView(_player.Direction));
		}

		private bool TryKillPlayer(Enemy enemy)
		{
			return _player.Position.Equals(enemy.Position);
		}

		internal void AttackMonster()
		{
			Direction direction = _player.Direction;
			var attackPosition = Point2D.CreateNear(_player.Position, direction);
			_enemies.RemoveAll(e => e.Position.Equals(attackPosition));
			_renderingPlan.Add(attackPosition, attackPosition, new SwordView(direction));
			_renderingPlan.AddTemporarily(attackPosition, new SwordView(direction));
		}

		internal void MovePlayer(Direction direction)
		{
			var from = _player.Position.Clone();
			_player.MoveTo(direction);
			CorrectPosition(_player);
			var to = _player.Position.Clone();
			IsPlayerAlive = IsPlayerAlive && !_enemies.Any(TryKillPlayer);
			_renderingPlan.Add(from, to, new PlayerView(_player.Direction));
		}

		private void CorrectPosition(FieldObject obj)
		{
			if (obj.Position.X >= _width - 1)
				obj.Position.X--;
			if (obj.Position.X <= 0)
				obj.Position.X++;
			if (obj.Position.Y >= _height - 1)
				obj.Position.Y--;
			if (obj.Position.Y <= 0)
				obj.Position.Y++;
		}
	}
}
