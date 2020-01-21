using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
	class Program
	{
		private static List<ChatClient> _clients;

		static void Main(string[] args)
		{
			_clients = new List<ChatClient>();

			using(var server = new Server("localhost", 7777))
			{
				server.OnClientConnected += RegisterClient;
				server.Start();
				
				server.WaitForShutdown();

				Console.WriteLine("Server is running. Press any key to shutdown the server.");
				Console.ReadKey();
			}
		}

		private static void RegisterClient(object sender, ClientRequest req)
		{
			var client = new ChatClient(req.Id, req.Socket);
			client.OnMessageReceived += (senderClient, message) => SendToAll(((ChatClient)senderClient).Id, message);
			_clients.Add(client);
			Console.WriteLine($"Registered: {req.Id}");
		}

		private static void SendToAll(string senderId, string message)
		{
			foreach (var client in _clients)
			{
				if(client.Id != senderId)
				{
					_ = client.SendAsync(senderId, message);
				}
			};
		}
	}
}
