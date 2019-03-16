﻿using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

class TCPClient
{
    public TcpClient client;
    public NetworkStream stream;
    
    public DebugLog debugLog;
    public SpawnManager spawnManager;

    public string IpAddress = "127.0.0.1";
    public int port = 8848;

    public bool isConnected;

    private byte[] asyncBuffer;

    public void Connect()
    {
        Debug.Log("Attempting establish connection to server.");

        client = new TcpClient();

        client.SendBufferSize = 4096;
        client.ReceiveBufferSize = 4096;

        asyncBuffer = new byte[8192];

        client.BeginConnect(IpAddress, port, OnConnected, null);

        //IPAddress[] remoteHost = Dns.GetHostAddresses("host.contoso.com");
        //client.BeginConnect(remoteHost, 8848, OnConnected, null);
    }

    private void OnConnected(IAsyncResult result)
    {
        try
        {
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
                Application.Quit();
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

                //Debug.Log(msg.connectId);
                //Debug.Log(msg.prefabId);
                Debug.Log(msg.position.ToString("F4"));
                Debug.Log(msg.payload);

                spawnManager.Spawn(msg);
            }
            else if (readerMsgType == MRTMsgType.Text)
            {
                TextMessage msg = new TextMessage();
                msg.Deserialize(networkReader);
                
                Debug.Log(msg.payload);

                debugLog.Log(msg.payload);
            }

            stream.BeginRead(asyncBuffer, 0, asyncBuffer.Length, OnReceivedData, null);
        }
        catch (Exception)
        {
            Debug.Log("Disconnected from server.");
            Application.Quit();
            return;
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
            msgType = MRTMsgType.Anchor;
        }
        else if(msg.GetType().Name == "SpawnMessage")
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