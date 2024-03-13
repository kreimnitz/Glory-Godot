using System.IO;
using ProtoBuf;

namespace Utilities.Comms
{
    public interface IServerMessageReceivedHandler
    {
        void HandleServerMessage(Message message);
    }

    public interface IClientMessageRecievedHandler
    {
        void HandleClientMessage(Message message, int playerId);
    }

    public static class SerializationUtilities
    {
        public static byte[] ToByteArray<T>(T toSerialize)
        {
            using (var ms = new MemoryStream())
            {
                Serializer.Serialize(ms, toSerialize);
                return ms.ToArray();
            }
        }

        public static T FromByteArray<T>(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                return Serializer.Deserialize<T>(ms);
            }
        }
    }
}
