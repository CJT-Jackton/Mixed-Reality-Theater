namespace MRTheater_Server
{
    /// <summary>
    /// The plain text message.
    /// </summary>
    class TextMessage : MessageBase
    {
        public int connectId;
        public string payload;

        /// <summary>
        /// The method is used to populate a NetworkWriter stream from a message object.
        /// </summary>
        /// <param name="writer">Stream to write to.</param>
        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(connectId);
            writer.Write(payload);
        }

        /// <summary>
        /// This method is used to populate a message object from a NetworkReader stream.
        /// </summary>
        /// <param name="reader">Stream to read from.</param>
        public override void Deserialize(NetworkReader reader)
        {
            connectId = reader.ReadInt32();
            payload = reader.ReadString();
        }
    }
}
