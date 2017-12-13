using DSmoove.Core.Helpers;
using DSmoove.Core.PeerCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Interfaces
{
    public interface IHandlePeerDownloads
    {
        void SendRequestCommand(RequestCommand command);
        void SendCancelCommand(CancelCommand command);

        AsyncSubscription<IHandlePeerDownloads, BitFieldCommand> BitFieldCommandSubscription { get; }
        AsyncSubscription<IHandlePeerDownloads, HaveCommand> HaveCommandSubscription { get; }
        AsyncSubscription<IHandlePeerDownloads, PieceCommand> PieceCommandSubscription { get; }
    }
}