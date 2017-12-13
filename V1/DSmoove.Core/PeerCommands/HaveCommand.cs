using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.PeerCommands
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