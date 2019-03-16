using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Serializer : MonoBehaviour
{
    private NetworkServerSimple server;
    private NetworkClient client;

    const short k_myMsg = 178;

    // Use this for initialization
    void Start()
    {
        StartServer();
        StartClient();
    }

    // Update is called once per frame
    void Update()
    {
        if (server != null)
        {
            server.Update();
        }
    }

    void StartServer()
    {
        server = new NetworkServerSimple();
        server.RegisterHandler(k_myMsg, OnMyMessage);

        if (server.Listen(8848))
        {
            Debug.Log("Start listening port 8848");
        }
    }

    void StartClient()
    {
        client = new NetworkClient();
        client.RegisterHandler(MsgType.Connect, OnClientConnected);
        client.Connect("127.0.0.1", 8848);
    }

    void OnMyMessage(NetworkMessage networkMsg)
    {
        Debug.Log("Got message, size=" + networkMsg.reader.Length);
        var someValue = networkMsg.reader.ReadInt32();
        var someString = networkMsg.reader.ReadString();
        Debug.Log("Message value =" + someValue + " Message string =\'" + someString + "\'");
    }

    void OnClientConnected(NetworkMessage networkMsg)
    {
        Debug.Log("Client connected.");
        SendMessage();
    }

    void SendMessage()
    {
        NetworkWriter writer = new NetworkWriter();
        writer.StartMessage(k_myMsg);
        writer.Write(42);
        writer.Write("Are you ok?");
        writer.FinishMessage();

        client.SendWriter(writer, 0);
    }
}
