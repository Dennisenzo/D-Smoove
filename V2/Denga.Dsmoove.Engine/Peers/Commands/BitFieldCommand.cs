using System.Collections;
using System.Linq;

namespace Denga.Dsmoove.Engine.Peers.Commands
{
    public class BitFieldCommand : BasePeerCommand
    {
        public BitArray Downloaded { get; set; }

        public BitFieldCommand()
            : base(PeerCommandId.BitField)
        {

        }

        public override byte[] ToByteArray()
        {
            byte[] data = new byte[Downloaded.Length / 8];
            Downloaded.CopyTo(data, 0);
            return ToByteArray(true, data);
        }

        public override void FromByteArray(byte[] data)
        {
            base.FromByteArray(data);

            Downloaded = new BitArray(data.Skip(1).ToArray());
        }
    }
}