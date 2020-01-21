namespace Battle.Views
{
	class PlayerView: IView
	{
		private const string _views = "┼╟╫╢o";
		private Direction _direction;

		public PlayerView(Direction direction)
		{
			_direction = direction;
		}

		public char GetSymbol() => _views[(int)_direction];
	}
}
