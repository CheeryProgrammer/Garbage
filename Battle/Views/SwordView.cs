namespace Battle.Views
{
	class SwordView: IView
	{
		private const string _views = "┘⌐┌¬";
		private readonly Direction _direction;
		public SwordView(Direction direction)
		{
			_direction = direction;
		}

		public char GetSymbol() => _views[(int)_direction];
	}
}
