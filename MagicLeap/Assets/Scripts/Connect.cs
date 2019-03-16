using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Connect : MonoBehaviour
{
    private int myConnectionId;
    private int channelId;
    private int hostId;

    // Use this for initialization
    void Start()
    {
        //GlobalConfig globalConfig = new GlobalConfig();
        //globalConfig.MaxPacketSize = 500;

        NetworkTransport.Init();

        ConnectionConfig config = new ConnectionConfig();
        channelId = config.AddChannel(QosType.Reliable);

        HostTopology topology = new HostTopology(config, 10);

        hostId = NetworkTransport.AddHost(topology, 8888);

        byte error;
        myConnectionId = NetworkTransport.Connect(hostId, "192.168.1.42", 8848, 0, out error);

        if ((NetworkError)error == NetworkError.Ok)
        {
            Debug.Log("NetworkError: " + error);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int outHostId;
        int outConnectionId;
        int outChannelId;
        byte[] buffer = new byte[1024];
        int bufferSize = 1024;
        int receiveSize;
        byte error;

        NetworkEventType evnt = NetworkTransport.Receive(out outHostId, out outConnectionId, out outChannelId, buffer, bufferSize, out receiveSize, out error);

        switch (evnt)
        {
            case NetworkEventType.Nothing: break;
            case NetworkEventType.ConnectEvent:
                if (outHostId == hostId &&
                    outConnectionId == myConnectionId &&
                    (NetworkError)error == NetworkError.Ok)
                {
                    Debug.Log("Connected");
                }
                else
                {
                    //somebody else sent a connect request to me

                }

                break;

            case NetworkEventType.DisconnectEvent:
                if (outHostId == hostId &&
                    outConnectionId == myConnectionId)
                {
                    Debug.Log("Connected, error:" + error.ToString());
                }
                else
                {
                    //one of the established connections has disconnected
                    Debug.Log("Disconnected");
                }

                break;

            case NetworkEventType.BroadcastEvent:

                break;
        }
    }

    void OnDestroy()
    {
        byte error;
        NetworkTransport.Disconnect(hostId, myConnectionId, out error);
    }

    void SendMsg(byte[] buffer, int bufferLength)
    {
        byte error;
        NetworkTransport.Send(hostId, myConnectionId, channelId, buffer, bufferLength, out error);
    }
}
