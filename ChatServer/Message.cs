using System.Net.Sockets;

namespace ChatServer
{
	internal class ClientRequest
	{
		public string Id { get; }
		public Socket Socket { get; }

		public ClientRequest(string senderId, Socket socket)
		{
			Id = senderId;
			Socket = socket;
		}
	}
}