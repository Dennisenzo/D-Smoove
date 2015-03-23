using DSmoove.Core.Helpers;
using DSmoove.Core.PeerCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Interfaces
{
    public interface IHandlePeerUploads
    {
        void SendBitfieldCommand(BitFieldCommand command);
        void SendHaveCommand(HaveCommand command);
        void SendPieceCommand(PieceCommand command);

        AsyncSubscription<IHandlePeerUploads, RequestCommand> RequestCommandSubscription { get; }
        AsyncSubscription<IHandlePeerUploads, CancelCommand> CancelCommandSubscription { get; }
    }
}
