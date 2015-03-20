using DSmoove.Core.PeerCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Interfaces
{
    public interface IHandlePeerConnection
    {
        void SendHandshakeCommand(HandshakeCommand command);
        void SendPortCommand(PortCommand command);
        void SendChokeCommand(ChokeCommand command);
        void SendUnchokeCommand(UnchokeCommand command);
        void SendInterestedCommand(InterestedCommand command);
        void SendNotInterestedCommand(NotInterestedCommand command);

        void SubscribeToHandshakeCommand(Action<IHandlePeerConnection, HandshakeCommand> action);
        void SubscribeToPortCommand(Action<IHandlePeerConnection, PortCommand> action);
        void SubscribeToChokeCommand(Action<IHandlePeerConnection, ChokeCommand> action);
        void SubscribeToUnchokeCommand(Action<IHandlePeerConnection, UnchokeCommand> action);
        void SubscribeToInterestedCommand(Action<IHandlePeerConnection, InterestedCommand> action);
        void SubscribeToNotInterestedCommand(Action<IHandlePeerConnection, NotInterestedCommand> action);
    }
}
