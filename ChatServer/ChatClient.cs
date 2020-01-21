using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
	public class ChatClient
	{
		private string id;
		private Socket socket;

		public string Id { get => id; }

		public event EventHandler<string> OnMessageReceived;

		public ChatClient(string id, Socket socket)
		{
			this.id = id;
			this.socket = socket;

			_ = StartReadingChat();
		}

		private async Task StartReadingChat()
		{
			await Task.Run(() =>
			{
				while (socket.Connected)
				{
					string msg = string.Empty;
					var buffer = new byte[256];
					int readCount = socket.Receive(buffer);
					msg += Encoding.UTF8.GetString(buffer.Take(readCount).ToArray());
					if (socket.Available == 0)
					{
						OnMessageReceived?.Invoke(this, msg);
						Console.WriteLine(msg);
					}
				};
				Console.WriteLine($"{id} disconnected!");
			});
		}

		internal async Task SendAsync(string fromId, string message)
		{
			var bytes = new ArraySegment<byte>(Encoding.UTF8.GetBytes($"{fromId}: {message}"));
			await socket.SendAsync(bytes, SocketFlags.None);
		}
	}
}