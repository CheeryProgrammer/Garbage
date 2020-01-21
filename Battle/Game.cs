using Battle.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Battle
{
	class Game: IDisposable
	{
		private CancellationTokenSource _cancellationSource;
		private Controller _controller;
		private RenderingPlan _renderingPlan;
		private ManualResetEvent _gameOverEvent;

		internal void WaitForGameOver()
		{
			_gameOverEvent.WaitOne();
		}

		private Field _field;

		public Game(int width, int weight)
		{
			_controller = new Controller();
			_renderingPlan = new RenderingPlan();
			Console.SetWindowSize(width, weight);
			Console.SetBufferSize(width, weight);

			_cancellationSource = new CancellationTokenSource();
			_gameOverEvent = new ManualResetEvent(false);
			_field = new Field(width, weight, _renderingPlan);
			_field.AddEnemies(100);
		}

		internal void Start()
		{
			_ = StartGameLoop();
			_ = _controller.StartListening();
		}

		internal void Stop()
		{
			if (!_cancellationSource.IsCancellationRequested)
				_cancellationSource.Cancel();
			_gameOverEvent.Set();
		}

		private async Task StartGameLoop()
		{
			await Task.Run(() => GameLoop(_cancellationSource.Token));
		}

		private void GameLoop(CancellationToken cancelToken)
		{
			while (!cancelToken.IsCancellationRequested)
			{
				var delayTask = Task.Delay(16);
				UpdateState();
				ProcessInput();
				Draw();
				delayTask.Wait();
				if (!_field.IsPlayerAlive)
					break;
			}
			Stop();
		}

		private void ProcessInput()
		{
			foreach(var key in _controller.GetInput())
			{
				switch (key)
				{
					case ConsoleKey.W:
						_field.MovePlayer(Direction.Up);
						break;
					case ConsoleKey.D:
						_field.MovePlayer(Direction.Right);
						break;
					case ConsoleKey.S:
						_field.MovePlayer(Direction.Down);
						break;
					case ConsoleKey.A:
						_field.MovePlayer(Direction.Left);
						break;
					case ConsoleKey.Spacebar:
						_field.AttackMonster();
						break;
					case ConsoleKey.Escape:
						Stop();
						break;
				}
			}
		}

		private void Draw()
		{
			foreach(RenderingElement el in _renderingPlan.GetElements())
			{
				Console.SetCursorPosition(el.From.X, el.From.Y);
				Console.Write(' ');
				Console.SetCursorPosition(el.To.X, el.To.Y);
				Console.Write(el.View.GetSymbol());
			}
		}

		private void UpdateState()
		{
			_field.Update();
		}

		public void Dispose()
		{
			Stop();
			_cancellationSource.Dispose();
		}
	}
}
