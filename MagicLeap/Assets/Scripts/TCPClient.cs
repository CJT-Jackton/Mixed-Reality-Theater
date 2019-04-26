using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

class TCPClient : MonoBehaviour
{
    public TcpClient client;
    public NetworkStream stream;

    //public DebugLog debugLog;
    public SpawnManager spawnManager;

    public string ServerDomain = "mrtheaterserver.webredirect.org";
    public string IpAddress = "127.0.0.1";
    public int port = 8848;

    public bool isConnected;

    private byte[] asyncBuffer;
    private int timeout = 5000;

    public void Connect()
    {
        Debug.Log("Attempting establish connection to server.");

        client = new TcpClient();

        client.SendBufferSize = 4096;
        client.ReceiveBufferSize = 4096;

        asyncBuffer = new byte[8192];

        IPAddress[] remoteHost = Dns.GetHostAddresses(ServerDomain);
        IAsyncResult result = client.BeginConnect(remoteHost, port, null, null);
        
        try
        {
            client.EndConnect(result);
        }
        catch (Exception)
        {
            isConnected = false;
        }

        if (!client.Connected)
        {
            Invoke("Reconnect", 5.0f);
        }
        else
        {
            stream = client.GetStream();
            stream.BeginRead(asyncBuffer, 0, asyncBuffer.Length, OnReceivedData, null);

            isConnected = true;

            Debug.Log("Successfully connected to the server.");
        }
    }

    public void Reconnect()
    {
        Debug.Log("Attempting re-establish connection to server.");

        client = new TcpClient();

        client.SendBufferSize = 4096;
        client.ReceiveBufferSize = 4096;

        asyncBuffer = new byte[8192];

        IPAddress[] remoteHost = Dns.GetHostAddresses(ServerDomain);
        IAsyncResult result = client.BeginConnect(remoteHost, port, null, null);

        try
        {
            client.EndConnect(result);
        }
        catch (Exception)
        {
            isConnected = false;
        }

        if (!client.Connected)
        {
            Invoke("Reconnect", 5.0f);
        }
        else
        {
            stream = client.GetStream();
            stream.BeginRead(asyncBuffer, 0, asyncBuffer.Length, OnReceivedData, null);

            isConnected = true;

            Debug.Log("Successfully connected to the server.");
        }
    }

    private void OnConnected(IAsyncResult result)
    {
        try
        {
            CancelInvoke("Reconnect");
            client.EndConnect(result);

            if (!client.Connected)
            {
                Debug.Log("Failed to connect to server.");
                return;
            }

            stream = client.GetStream();
            stream.BeginRead(asyncBuffer, 0, asyncBuffer.Length, OnReceivedData, null);

            isConnected = true;

            Debug.Log("Successfully connected to the server.");
        }
        catch (Exception)
        {
            isConnected = false;
            return;
        }
    }

    private void OnReceivedData(IAsyncResult result)
    {
        try
        {
            int length = stream.EndRead(result);
            byte[] data = new byte[length];

            Buffer.BlockCopy(asyncBuffer, 0, data, 0, length);

            if (length <= 0)
            {
                Debug.Log("Disconnected from server.");
                return;
            }

            NetworkReader networkReader = new NetworkReader(data);

            byte[] readerMsgSizeData = networkReader.ReadBytes(2);
            short readerMsgSize = (short)((readerMsgSizeData[1] << 8) + readerMsgSizeData[0]);
            //Debug.Log(readerMsgSize);

            byte[] readerMsgTypeData = networkReader.ReadBytes(2);
            short readerMsgType = (short)((readerMsgTypeData[1] << 8) + readerMsgTypeData[0]);
            //Debug.Log(readerMsgType);

            if (readerMsgType == MRTMsgType.Spawn)
            {
                SpawnMessage msg = new SpawnMessage();
                msg.Deserialize(networkReader);

                spawnManager.Spawn(msg);
            }
            else if (readerMsgType == MRTMsgType.DestroyAllStatic)
            {
                spawnManager.DestroyAll();
            }
            else if (readerMsgType == MRTMsgType.Text)
            {
                TextMessage msg = new TextMessage();
                msg.Deserialize(networkReader);

                Debug.Log(msg.payload);

                //debugLog.Log(msg.payload);
            }

            stream.BeginRead(asyncBuffer, 0, asyncBuffer.Length, OnReceivedData, null);
        }
        catch (Exception)
        {
            Debug.Log("Disconnected from server.");
        }
    }

    public void SendMsg(MessageBase msg)
    {
        short msgType = 0;

        if (msg.GetType().Name == "TextMessage")
        {
            msgType = MRTMsgType.Text;
        }
        else if (msg.GetType().Name == "AnchorMessage")
        {
            msgType = MRTMsgType.DirectorSpawn;
        }
        else if (msg.GetType().Name == "SpawnMessage")
        {
            msgType = MRTMsgType.Spawn;
        }

        NetworkWriter writer = new NetworkWriter();
        writer.StartMessage(msgType);

        msg.Serialize(writer);

        writer.FinishMessage();

        // send message to server
        stream.Write(writer.ToArray(), 0, writer.ToArray().Length);
    }
}
