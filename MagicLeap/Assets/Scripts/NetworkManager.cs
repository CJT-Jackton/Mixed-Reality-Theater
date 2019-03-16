using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);
        TCPServer server = new TCPServer();
        server.Init();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
