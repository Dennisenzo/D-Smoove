using DSmoove.Core.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.PeerCommands
{
    // handshake: <pstrlen><pstr><reserved><info_hash><peer_id>
    // pstrlen: string length of <pstr>, as a single raw byte
    // pstr: string identifier of the protocol
    // reserved: eight (8) reserved bytes. All current implementations use all zeroes. Each bit in these bytes can be used to change the behavior of the protocol. An email from Bram suggests that trailing bits should be used first, so that leading bits may be used to change the meaning of trailing bits.
    // info_hash: 20-byte SHA1 hash of the info key in the metainfo file. This is the same info_hash that is transmitted in tracker requests.
    // peer_id: 20-byte string used as a unique ID for the client. This is usually the same peer_id that is transmitted in tracker requests (but not always e.g. an anonymity option in Azureus).


    public class HandshakeCommand : BasePeerCommand
    {
        public int MessageLength { get; set; }
        public string Protocol { get; set; }
        public byte[] InfoHash { get; set; }
        public string PeerId { get; set; }
        public byte[] Flags { get; set; }

        public HandshakeCommand()
            : base(PeerCommandId.Handshake)
        {
            Protocol = "BitTorrent protocol";
            MessageLength = Protocol.Length;
            PeerId = Settings.General.PeerId;
            Flags = new byte[8];
            InfoHash = new byte[20];
        }

        public override byte[] ToByteArray()
        {
            return ToByteArray(false, (byte)MessageLength, Protocol, Flags, InfoHash, PeerId);
        }

        public override void FromByteArray(byte[] data)
        {
            MessageLength = data.Length - 48;
            Protocol = Encoding.ASCII.GetString(data, 0, MessageLength);
            Array.Copy(data, MessageLength, Flags, 0, 8);
            Array.Copy(data, MessageLength + 8, InfoHash, 0, 20);
            PeerId = Encoding.ASCII.GetString(data, MessageLength + 28, 20);
        }
    }
}