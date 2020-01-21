using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
	class Server : IDisposable
	{
		private TcpListener _listener;

		public event EventHandler<ClientRequest> OnClientConnected;

		public Server(string hostName, int port)
		{
			var hostEntry = Dns.GetHostEntry(hostName);
			var address = hostEntry.AddressList.First();
			_listener = new TcpListener(address, port);
		}

		internal void WaitForShutdown()
		{
			
		}

		internal Task Start()
		{
			return Task.Run(StartListening);
		}

		private void StartListening()
		{
			_listener.Start();
			while (true)
			{
				var clientSocket = _listener.AcceptSocket();
				ReadSocketAsync(clientSocket);
			}
		}

		private void ReadSocketAsync(Socket clientSocket)
		{
			var buffer = new byte[512];
			var receivedCount = clientSocket.Receive(buffer);
			var message = new ClientRequest(Encoding.UTF8.GetString(buffer.Take(receivedCount).ToArray()), clientSocket);
			OnClientConnected?.Invoke(this, message);
		}

		public void Dispose()
		{
			_listener.Stop();
		}
	}
}
