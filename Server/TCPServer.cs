using System;
using System.Net;
using System.Net.Sockets;

namespace MRTheater_Server
{
    class TCPServer
    {
        //public IPAddress IP;
        public int port = 8848;

        public Client[] clients = new Client[1024];

        private TcpListener serverSocket;

        public void Init()
        {
            for (int i = 0; i < clients.Length; ++i)
            {
                clients[i] = new Client();
            }

            foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Console.WriteLine("MR Theater Server");
                    Console.WriteLine(ip.ToString());
                    break;
                }
            }

            Console.Write("(" + DateTime.Now.ToString("HH:mm:ss") + "): ");
            Console.WriteLine("Server started listening from port: " + port);

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

                    Console.Write("(" + DateTime.Now.ToString("HH:mm:ss") + "): ");
                    Console.WriteLine("Connection from " + clients[i].IP);

                    return;
                }
            }

            Console.Write("(" + DateTime.Now.ToString("HH:mm:ss") + "): ");
            Console.WriteLine("Maximum amount of clients!");
        }

        public void SendMsgToAll(MessageBase msg)
        {
            for (int i = 0; i < clients.Length; ++i)
            {
                if (clients[i].socket != null && clients[i].socket.Connected)
                {
                    SendMsg(i, msg);
                }
            }
        }

        public void SendMsg(int connectionId, MessageBase msg)
        {
            short msgType = 0;

            if (msg.GetType().Name == "TextMessage")
            {
                msgType = MRTMsgType.Text;
            }
            else if (msg.GetType().Name == "AnchorMessage")
            {
                msgType = MRTMsgType.Anchor;
            }
            else if (msg.GetType().Name == "SpawnMessage")
            {
                msgType = MRTMsgType.Spawn;
            }

            NetworkWriter writer = new NetworkWriter();
            writer.StartMessage(msgType);

            msg.Serialize(writer);

            writer.FinishMessage();

            // send message to client
            clients[connectionId].stream.BeginWrite(writer.ToArray(), 0, writer.ToArray().Length, null, null);

            Console.Write("(" + DateTime.Now.ToString("HH:mm:ss") + "): ");
            Console.WriteLine("Sent message to client {0}.", connectionId);
        }

        private void SendWelcome(int connectionId)
        {
            TextMessage msg = new TextMessage();
            msg.connectId = connectionId;
            msg.payload = "Welcome to the server!";
            msg.payload += "\nNow you are connected.";

            SendMsg(connectionId, msg);
        }
    }
}
