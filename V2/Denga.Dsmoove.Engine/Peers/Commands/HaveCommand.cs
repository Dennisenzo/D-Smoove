using System;
using System.Linq;

namespace Denga.Dsmoove.Engine.Peers.Commands
{
    public class HaveCommand : BasePeerCommand
    {
        public int PieceIndex { get; set; }
        public HaveCommand()
            : base(PeerCommandId.Have)
        {

        }

        public override void FromByteArray(byte[] data)
        {
            base.FromByteArray(data);
            PieceIndex = BitConverter.ToInt32(data.Skip(1).Take(4).Reverse().ToArray(), 0);
        }

        public override byte[] ToByteArray()
        {
            return base.ToByteArray(true, PieceIndex);
        }
    }
}