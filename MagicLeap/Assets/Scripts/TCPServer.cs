using System;
using System.Net;
using System.Net.Sockets;

class TCPServer
{
    public int port = 8848;

    public Client[] clients = new Client[1024];

    private TcpListener serverSocket;

    public void Init()
    {
        serverSocket = new TcpListener(IPAddress.Any, port);
        serverSocket.Start();
        serverSocket.BeginAcceptTcpClient(OnClientConnected, null);
    }

    private void OnClientConnected(IAsyncResult result)
    {
        TcpClient client = serverSocket.EndAcceptTcpClient(result);
        serverSocket.BeginAcceptTcpClient(OnClientConnected, null);

        for (int i = 0; i < clients.Length; ++i)
        {
            if (clients[i].socket == null)
            {
                clients[i].socket = client;
                clients[i].myConnectId = i;
                clients[i].IP = client.Client.RemoteEndPoint.ToString();

                clients[i].Start();

                Console.WriteLine("Connection from " + clients[i].IP);

                return;
            }
        }

        Console.WriteLine("Maximum amount of clients!");
    }
}
