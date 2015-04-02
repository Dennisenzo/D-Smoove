using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.PeerCommands
{
    public abstract class BasePeerCommand
    {
        public PeerCommandId MessageId { get; protected set; }

        private ASCIIEncoding _encoding;

        public abstract byte[] ToByteArray();

        public virtual void FromByteArray(byte[] data)
        {
            if (data.Length >= 1)
            {
                MessageId = (PeerCommandId)data[0];
            }
        }

        public BasePeerCommand(PeerCommandId messageId)
        {
            MessageId = messageId;

            _encoding = new ASCIIEncoding();
        }

        protected virtual byte[] ToByteArray(bool addDefaultValues, params object[] items)
        {
            List<byte> tempData = new List<byte>();



            if (addDefaultValues)
            {
                tempData.Add((byte)MessageId);
            }

            foreach (var item in items)
            {
                if (item is string)
                {
                    tempData.AddRange(_encoding.GetBytes(item as string));
                }
                else if (item is int)
                {
                    tempData.AddRange(BitConverter.GetBytes((int)item).Reverse());
                }
                else if (item is byte[])
                {
                    tempData.AddRange(item as byte[]);
                }
                else if (item is byte)
                {
                    tempData.Add((byte)item);
                }
            }

            int totalLength = tempData.Count;
            byte[] data = null;
            byte[] length = null;

            if (addDefaultValues)
            {
                data = new byte[totalLength + 4];
                length = BitConverter.GetBytes(totalLength);

                length.CopyTo(data, 0);
                tempData.CopyTo(data, 3);
            }
            else
            {
                data = new byte[totalLength];

                tempData.CopyTo(data, 0);
            }

            return data;
        }
    }

    public enum PeerCommandId : byte
    {
        Handshake = 0x99,
        Choke = 0x00,
        Unchoke = 0x01,
        Interested = 0x02,
        NotInterested = 0x03,
        Have = 0x04,
        BitField = 0x05,
        Request = 0x06,
        Piece = 0x07,
        Cancel = 0x08,
        Port = 0x09
    }
}