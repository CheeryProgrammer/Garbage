namespace Battle.Views
{
	internal class EnemyView: IView
	{
		private const string _views = "^>v<o";
		private Direction _direction;

		public EnemyView(Direction direction)
		{
			_direction = direction;
		}

		public char GetSymbol() => _views[(int)_direction];
	}
}