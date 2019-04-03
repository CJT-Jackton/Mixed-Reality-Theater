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

                    int id;
                    Console.Write("Prefab Id: ");

                    while ((input = Console.ReadLine()) != null)
                    {
                        if (Int32.TryParse(input, out id))
                        {
                            msg.prefabId = id;
                            break;
                        }
                    }

                    Console.Write("Position: ");

                    while ((input = Console.ReadLine()) != null)
                    {
                        Vector3 vec;

                        if (Vector3.TryParse(input, out vec))
                        {
                            msg.position = vec;
                            break;
                        }
                    }

                    Console.Write("Rotation: ");

                    while ((input = Console.ReadLine()) != null)
                    {
                        Vector3 vec;

                        if (Vector3.TryParse(input, out vec))
                        {
                            msg.rotation = vec;
                            break;
                        }
                    }

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
