using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.PeerCommands
{
    public class NotInterestedCommand : BasePeerCommand
    {
        public NotInterestedCommand()
            : base(PeerCommandId.NotInterested)
        {

        }

        public override byte[] ToByteArray()
        {
            return ToByteArray(true);
        }
    }
}