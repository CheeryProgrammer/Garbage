using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Battle
{
	class Controller
	{
		private ConcurrentQueue<ConsoleKey> _inputQueue;

		public Controller()
		{
			_inputQueue = new ConcurrentQueue<ConsoleKey>();
		}

		public IEnumerable<ConsoleKey> GetInput()
		{
			while (_inputQueue.TryDequeue(out ConsoleKey input))
				yield return input;
		}

		public Task StartListening()
		{
			return Task.Run(InputLoop);
		}

		private void InputLoop()
		{
			ConsoleKeyInfo keyInfo;
			do
			{
				keyInfo = Console.ReadKey(true);
				_inputQueue.Enqueue(keyInfo.Key);
			} while (!keyInfo.Key.Equals(ConsoleKey.Escape));
		}
	}
}
