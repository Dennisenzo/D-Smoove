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

        void SubscribeToRequestCommand(Action<IHandlePeerUploads, RequestCommand> action);
        void SubscribeToCancelCommand(Action<IHandlePeerUploads, CancelCommand> action);
    }
}
