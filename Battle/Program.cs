using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Battle
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.CursorVisible = false;

			using (var game = new Game(80, 30))
			{
				game.Start();
				game.WaitForGameOver();
			}

		}
	}
}
