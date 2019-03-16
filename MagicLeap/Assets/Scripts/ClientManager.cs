using UnityEngine;
using UnityEngine.UI;

public class ClientManager : MonoBehaviour
{
    public string IpAddress = "127.0.0.1";

    private TCPClient client;

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);

        client = new TCPClient();
        client.IpAddress = IpAddress;
        client.debugLog = GetComponent<DebugLog>();
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

    public void UploadAnchor(Transform transform)
    {
        AnchorMessage msg = new AnchorMessage();
        msg.connectId = 0;
        msg.position = transform.position;
        msg.rotation = transform.rotation.eulerAngles;

        client.SendMsg(msg);
    }
}
