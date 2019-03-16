namespace MRTheater_Server
{
    class TextMessage : MessageBase
    {
        public int connectId;
        public string payload;

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(connectId);
            writer.Write(payload);
        }

        public override void Deserialize(NetworkReader reader)
        {
            connectId = reader.ReadInt32();
            payload = reader.ReadString();
        }
    }
}
