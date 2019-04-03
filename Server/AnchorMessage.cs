namespace MRTheater_Server
{
    /// <summary>
    /// The anchor message.
    /// </summary>
    class AnchorMessage : MessageBase
    {
        public int connectId;
        public Vector3 position = new Vector3();
        public Vector3 rotation = new Vector3();

        /// <summary>
        /// The method is used to populate a NetworkWriter stream from a message object.
        /// </summary>
        /// <param name="writer">Stream to write to.</param>
        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(connectId);
            writer.Write(position);
            writer.Write(rotation);
        }

        /// <summary>
        /// This method is used to populate a message object from a NetworkReader stream.
        /// </summary>
        /// <param name="reader">Stream to read from.</param>
        public override void Deserialize(NetworkReader reader)
        {
            connectId = reader.ReadInt32();
            position = reader.ReadVector3();
            rotation = reader.ReadVector3();
        }
    }
}
