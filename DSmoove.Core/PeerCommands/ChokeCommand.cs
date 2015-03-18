using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.PeerCommands
{
    public class ChokeCommand : BasePeerCommand
    {
        public ChokeCommand()
            : base(0x00)
        {

        }

        public override byte[] ToByteArray()
        {
            return ToByteArray(true);
        }
    }
}