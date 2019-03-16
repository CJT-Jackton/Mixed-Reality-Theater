namespace MRTheater_Server
{
    abstract class MessageBase
    {
        public abstract void Serialize(NetworkWriter writer);
        public abstract void Deserialize(NetworkReader reader);
    }
}
