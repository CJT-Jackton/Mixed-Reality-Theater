namespace MRTheater_Server
{
    /// <summary>
    /// Network message classes should be derived from this class.
    /// </summary>
    abstract class MessageBase
    {
        /// <summary>
        /// The method is used to populate a NetworkWriter stream from a message object.
        /// </summary>
        /// <param name="writer">Stream to write to.</param>
        public abstract void Serialize(NetworkWriter writer);

        /// <summary>
        /// This method is used to populate a message object from a NetworkReader stream.
        /// </summary>
        /// <param name="reader">Stream to read from.</param>
        public abstract void Deserialize(NetworkReader reader);
    }
}
