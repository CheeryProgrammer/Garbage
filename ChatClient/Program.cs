using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient
{
	class Program
	{
		static void Main(string[] args)
		{
			var address = Dns.GetHostEntry("localhost").AddressList.First();
			Socket sender = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.IP);
			
			sender.Connect(address, 7777);
			
			var clientId = Guid.NewGuid();
			sender.Send(Encoding.UTF8.GetBytes(clientId.ToString()), SocketFlags.None);

			_ = StartListeningAsync(sender);

			Console.WriteLine($"Connected! Your id is {clientId}");
			
			string message = string.Empty;
			while(!string.IsNullOrWhiteSpace(message = Console.ReadLine()))
				sender.Send(Encoding.UTF8.GetBytes(message), SocketFlags.None);

			sender.Close();
		}

		private static async Task StartListeningAsync(Socket socket)
		{
			await Task.Run(() =>
			{
				string msg = string.Empty;
				while (socket.Connected)
				{
					var buffer = new byte[256];
					int readCount = socket.Receive(buffer);
					msg += Encoding.UTF8.GetString(buffer.Take(readCount).ToArray());
					if (socket.Available == 0)
					{
						Console.WriteLine(msg);
						msg = string.Empty;
					}
				};
			});
		}
	}
}
