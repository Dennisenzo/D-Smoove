using DSmoove.Core.Helpers;
using DSmoove.Core.PeerCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Interfaces
{
    public interface IProvidePeerMessages
    {
        AsyncSubscription<IProvidePeerMessages, byte[]> PeerMessageSubscription { get; }
        AsyncSubscription<IProvidePeerMessages, byte[]> PeerHandshakeSubscription { get; }
    }
}