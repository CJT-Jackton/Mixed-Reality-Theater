using UnityEngine;
using UnityEngine.UI;

public class ClientManager : MonoBehaviour
{
    public string ServerDomain = "mrtheaterserver.webredirect.org";
    //public string IpAddress = "127.0.0.1";

    private TCPClient client;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);

        //client = new TCPClient();
        client = gameObject.AddComponent<TCPClient>();
        client.ServerDomain = ServerDomain;
        client.spawnManager = GetComponent<SpawnManager>();
        client.Connect();
    }

    private void OnApplicationQuit()
    {
        client.client.Close();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UploadAnchor(DirectorSpawnMessage msg)
    {
        client.SendMsg(msg);
    }
}
