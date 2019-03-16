using System;
using System.Net.Sockets;

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
                return;
            }

            byte[] data = new byte[dataLength];
            Buffer.BlockCopy(readBuffer, 0, data, 0, dataLength);

            stream.BeginRead(readBuffer, 0, socket.ReceiveBufferSize, OnReceivedData, null);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
