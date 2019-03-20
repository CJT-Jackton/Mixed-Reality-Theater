using UnityEngine;
using UnityEngine.Networking;

class AnchorMessage : MessageBase
{
    public int connectId;
    public Vector3 position = new Vector3();
    public Vector3 rotation = new Vector3();

    public override void Serialize(NetworkWriter writer)
    {
        writer.Write(connectId);
        writer.Write(position);
        writer.Write(rotation);
    }

    public override void Deserialize(NetworkReader reader)
    {
        connectId = reader.ReadInt32();
        position = reader.ReadVector3();
        rotation = reader.ReadVector3();
    }
}
