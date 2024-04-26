using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Utilities.Comms
{
    public class Message
    {
        private const int ChunkSize = 1024;
        private const int MessageTypeSectionSize = 4;
        private const int DataSizeSectionSize = 4;
        private const int MessageTypeSectionIndex = 0;
        private const int DataSizeSectionIndex = 4;
        private const int DataSectionIndex = 8;
        private const int HeaderSize = MessageTypeSectionSize + DataSizeSectionSize;

        public int MessageTypeId { get; }
        public byte[] Data { get; }

        public Message(int type, byte[] data)
        {
            MessageTypeId = type;
            Data = data;
        }

        public static Message ReceiveMessage(Socket socket)
        {
            var readChunk = new byte[ChunkSize];
            socket.Receive(readChunk);

            var messageTypeId = BitConverter.ToInt32(readChunk, MessageTypeSectionIndex);
            int dataSize = BitConverter.ToInt32(readChunk, DataSizeSectionIndex);

            byte[] data = new byte[dataSize];
            int firstDataCopySize = Math.Min(dataSize, ChunkSize - HeaderSize);
            Array.Copy(readChunk, DataSectionIndex, data, 0, firstDataCopySize);

            var dataToRead = dataSize - firstDataCopySize;
            while (dataToRead > 0)
            {
                socket.Receive(readChunk);
                int dataRead = dataSize - dataToRead;
                int copyLength = Math.Min(dataToRead, ChunkSize);
                Array.Copy(readChunk, 0, data, dataRead, copyLength);
                dataToRead -= ChunkSize;
            }

            return new Message(messageTypeId, data);
        }

        public static void SendMessage(Socket socket, Message message)
        {
            var byteArrays = message.Serialize();
            foreach (var byteArray in byteArrays)
            {
                socket.Send(byteArray);
            }
        }

        public List<byte[]> Serialize()
        {
            List<byte[]> chunks = new List<byte[]>();
            int firstChuckDataSectionSize = ChunkSize - HeaderSize;
            byte[] messageTypeBytes = BitConverter.GetBytes(MessageTypeId);
            byte[] dataSizeBytes = BitConverter.GetBytes(Data.Length);
            int firstDataCopySize = firstChuckDataSectionSize >= Data.Length
                ? Data.Length
                : firstChuckDataSectionSize;

            byte[] firstChunk = new byte[ChunkSize];
            Array.Copy(messageTypeBytes, 0, firstChunk, MessageTypeSectionIndex, messageTypeBytes.Length);
            Array.Copy(dataSizeBytes, 0, firstChunk, DataSizeSectionIndex, dataSizeBytes.Length);
            Array.Copy(Data, 0, firstChunk, DataSectionIndex, firstDataCopySize);
            chunks.Add(firstChunk);

            int dataToWrite = Data.Length - firstDataCopySize;
            while (dataToWrite > 0)
            {
                byte[] nextChunk = new byte[ChunkSize];
                int dataSourceIndex = firstChuckDataSectionSize + ChunkSize * (chunks.Count - 1);
                int copyLength = Math.Min(Data.Length - dataSourceIndex, ChunkSize);
                Array.Copy(Data, dataSourceIndex, nextChunk, 0, copyLength);
                chunks.Add(nextChunk);
                dataToWrite -= copyLength;
            }

            return chunks;
        }
    }
}