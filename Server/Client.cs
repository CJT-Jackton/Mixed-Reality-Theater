using System;
using System.Net.Sockets;

namespace MRTheater_Server
{
    class Client
    {
        public int myConnectId;
        public string IP;

        public TcpClient socket;
        public NetworkStream stream;

        private byte[] readBuffer;

        public void Start()
        {
            socket.SendBufferSize = 4096;
            socket.ReceiveBufferSize = 4096;

            stream = socket.GetStream();

            readBuffer = new byte[4096];

            stream.BeginRead(readBuffer, 0, socket.ReceiveBufferSize, OnReceivedData, null);
        }

        private void OnReceivedData(IAsyncResult result)
        {
            try
            {
                int dataLength = stream.EndRead(result);

                if (dataLength <= 0)
                {
                    CloseConnection();
                    return;
                }

                byte[] data = new byte[dataLength];
                Buffer.BlockCopy(readBuffer, 0, data, 0, dataLength);

                NetworkReader networkReader = new NetworkReader(data);

                byte[] readerMsgSizeData = networkReader.ReadBytes(2);
                short readerMsgSize = (short)((readerMsgSizeData[1] << 8) + readerMsgSizeData[0]);
                //Debug.Log(readerMsgSize);

                byte[] readerMsgTypeData = networkReader.ReadBytes(2);
                short readerMsgType = (short)((readerMsgTypeData[1] << 8) + readerMsgTypeData[0]);
                //Debug.Log(readerMsgType);

                if (readerMsgType == MRTMsgType.Anchor)
                {
                    AnchorMessage msg = new AnchorMessage();
                    msg.Deserialize(networkReader);

                    Console.Write("(" + DateTime.Now.ToString("HH:mm:ss") + "): ");
                    Console.WriteLine("Received anchor");
                    Console.WriteLine("Position: " + msg.position.ToString("F4"));
                    Console.WriteLine("Rotation: " + msg.rotation.ToString("F4"));
                }
                else if (readerMsgType == MRTMsgType.Text)
                {
                    TextMessage msg = new TextMessage();
                    msg.Deserialize(networkReader);

                    Console.Write("(" + DateTime.Now.ToString("HH:mm:ss") + "): ");
                    Console.WriteLine("Received text");
                    Console.WriteLine(msg.payload);
                }

                stream.BeginRead(readBuffer, 0, socket.ReceiveBufferSize, OnReceivedData, null);
            }
            catch (Exception)
            {
                CloseConnection();
                throw;
            }
        }

        private void CloseConnection()
        {
            socket.Close();
            Console.Write("(" + DateTime.Now.ToString("HH:mm:ss") + "): ");
            Console.WriteLine("Connection terminated from {0}", IP);
        }
    }
}
