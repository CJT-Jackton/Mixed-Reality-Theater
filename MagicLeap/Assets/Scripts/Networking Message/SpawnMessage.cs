using UnityEngine;
using UnityEngine.Networking;

public class SpawnMessage : MessageBase
{
    public int connectId;
    public int instanceId;
    public int prefabId;
    public Vector3 position = new Vector3();
    public Vector3 rotation = new Vector3();
    public string payload;

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(connectId);
        writer.Write(instanceId);
        writer.Write(prefabId);
        writer.Write(position);
        writer.Write(rotation);
        writer.Write(payload);
    }

    public override void Deserialize(NetworkReader reader)
    {
        connectId = reader.ReadInt32();
        instanceId = reader.ReadInt32();
        prefabId = reader.ReadInt32();
        position = reader.ReadVector3();
        rotation = reader.ReadVector3();
        payload = reader.ReadString();
    }
}

