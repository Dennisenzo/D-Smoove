using System;
using System.Linq;

namespace Denga.Dsmoove.Engine.Peers.Commands
{
    public class RequestCommand : BasePeerCommand
    {
        public int Index { get; set; }
        public long Begin { get; set; }
        public long Length { get; set; }

        public RequestCommand()
            : base(PeerCommandId.Request)
        {

        }

        public override byte[] ToByteArray()
        {
            return ToByteArray(true, Index, Begin, Length);
        }

        public override void FromByteArray(byte[] data)
        {
            base.FromByteArray(data);
            Index = BitConverter.ToInt32(data.Skip(1).Take(4).Reverse().ToArray(), 0);
            Begin = BitConverter.ToInt32(data.Skip(5).Take(4).Reverse().ToArray(), 0);
            Length = BitConverter.ToInt32(data.Skip(9).Take(4).Reverse().ToArray(), 0);

        }
    }
}
