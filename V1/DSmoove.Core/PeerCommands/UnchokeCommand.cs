using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.PeerCommands
{
    public class UnchokeCommand : BasePeerCommand
    {
        public UnchokeCommand()
            : base(PeerCommandId.Unchoke)
        {

        }

        public override byte[] ToByteArray()
        {
            return ToByteArray(true);
        }
    }
}