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

        void SubscribeToBitFieldCommand(Action<IHandlePeerDownloads, BitFieldCommand> action);
        void SubscribeToHaveCommand(Action<IHandlePeerDownloads, HaveCommand> action);
        void SubscribeToPieceCommand(Action<IHandlePeerDownloads, PieceCommand> action);
    }
}