using DSmoove.Core.Helpers;
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

        AsyncSubscription<IHandlePeerConnection> PeerConnectedSubscription { get; }
        AsyncSubscription<IHandlePeerConnection> PeerDisconnectedSubscription { get; }

        AsyncSubscription<IHandlePeerConnection, HandshakeCommand> HandshakeCommandSubscription { get; }
        AsyncSubscription<IHandlePeerConnection, PortCommand> PortCommandSubscription { get; }
        AsyncSubscription<IHandlePeerConnection, ChokeCommand> ChokeCommandSubscription { get; }
        AsyncSubscription<IHandlePeerConnection, UnchokeCommand> UnchokeCommandSubscription { get; }
        AsyncSubscription<IHandlePeerConnection, InterestedCommand> InterestedCommandSubscription { get; }
        AsyncSubscription<IHandlePeerConnection, NotInterestedCommand> NotInterestedCommandSubscription { get; }
    }
}
