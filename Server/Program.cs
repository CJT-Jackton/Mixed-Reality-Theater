using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRTheater_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            TCPServer server = new TCPServer();
            server.Init();

            string input;

            while ((input = Console.ReadLine()) != null)
            {
                input = input.ToLower();

                if (input == "quit")
                {
                    return;
                }
                else if (input == "spawn")
                {
                    SpawnMessage msg = new SpawnMessage();
                    msg.prefabId = 0;
                    msg.position = new Vector3(0.4f, 1.2f, -1.1f);
                    msg.rotation = new Vector3(0.0f, 0.0f, 0.0f);
                    msg.payload = "Spawn barrel.";

                    server.SendMsgToAll(msg);
                }
                else if (input == "message")
                {
                    string message = Console.ReadLine();

                    TextMessage msg = new TextMessage();
                    msg.payload = message;

                    server.SendMsgToAll(msg);
                }
            }

            //Console.ReadLine();
        }
    }
}
