namespace MRTheater_Server
{
    /// <summary>
    /// An enumeration of message types.
    /// </summary>
    class MRTMsgType
    {
        /// <summary>
        /// Plain text message.
        /// </summary>
        public static short Text = 50;

        /// <summary>
        /// Spawn game object.
        /// </summary>
        public static short Spawn = 51;

        /// <summary>
        /// The spatial anchor.
        /// </summary>
        public static short Anchor = 52;
    }
}
