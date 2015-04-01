using DSmoove.Core.PeerCommands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core
{
    public class CommandQueue : ConcurrentQueue<BasePeerCommand>
    {
        public void Enqueue(BasePeerCommand item)
        {
        }
    }
}
