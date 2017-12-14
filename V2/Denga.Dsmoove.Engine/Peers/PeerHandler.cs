using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Denga.Dsmoove.Engine.Data.Entities;
using Denga.Dsmoove.Engine.Infrastructure;
using Denga.Dsmoove.Engine.Infrastructure.Events;
using Denga.Dsmoove.Engine.Peers.Commands;
using log4net;

namespace Denga.Dsmoove.Engine.Peers
{
    public class PeerHandler
    {
        #region Properties and Fields

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public Torrent Torrent { get; private set; }

        public PeerConnectionStatus Status { get; private set; }

        public IDownloadStrategy DownloadStrategy { get; private set; }

        #endregion

        #region Constructors

        public PeerHandler(Torrent torrent)
        {
            Torrent = torrent;
            DownloadStrategy = new RarestFirstStrategy(Torrent);
        }

        #endregion

        #region Public Methods

        public void Start()
        {
            Bus.Instance.Subscribe<TrackerUpdatedEvent>(e =>
            {
                ConnectToPeers();
            });
            Bus.Instance.Subscribe<PeerConnectedEvent>(e =>
            {
              SendHandshake(e.PeerConnection);
            });
            Bus.Instance.Subscribe<PeerMessageReceivedEvent>(e =>
            {
                HandlePeerMessage(e.PeerConnection, e.MessageData);
            });
            Bus.Instance.Subscribe<ReceivedPeerCommandEvent<ChokeCommand>>(e =>
            {
                e.Source.PeerData.IsChokingUs = true;
            });
            Bus.Instance.Subscribe<ReceivedPeerCommandEvent<UnchokeCommand>>(e =>
            {
                e.Source.PeerData.IsChokingUs = false;
            });
            Bus.Instance.Subscribe<ReceivedPeerCommandEvent<InterestedCommand>>(e =>
            {
                e.Source.PeerData.IsInterestedInUs = true;
            });
            Bus.Instance.Subscribe<ReceivedPeerCommandEvent<NotInterestedCommand>>(e =>
            {
                e.Source.PeerData.IsInterestedInUs = false;
            });
            Bus.Instance.Subscribe<ReceivedPeerCommandEvent<BitFieldCommand>>(e =>
            {
                e.Source.PeerData.BitField = e.Command.Downloaded;
            });
            Bus.Instance.Subscribe<ReceivedPeerCommandEvent<HaveCommand>>(e =>
            {
                e.Source.PeerData.SetDownloaded(e.Command.PieceIndex);
            });
        }
        
        #endregion

        public void ConnectToPeers()
        {
       
            foreach (var peer in Torrent.Peers)
            {
                log.Debug($"Connecting to peer {peer.PeerId}");
                var connection = new PeerConnection(peer);
                connection.Connect();
            }
        }

        #region Peer Messages

        public void HandlePeerMessage(PeerConnection source, byte[] messageData)
        {
            if (messageData == null)
            {
                log.DebugFormat($"Received KeepAlive message from {source}");
            }
            else
            {
                log.DebugFormat($"Received message from {source.PeerData.IpAddress}:{source.PeerData.Port} with command {(PeerCommandId)messageData[0]}");
                switch ((PeerCommandId) messageData[0])
                {
                    case PeerCommandId.Choke:
                    {
                        ChokeCommand command = new ChokeCommand();
                        command.FromByteArray(messageData);
                        Bus.Instance.Publish(new ReceivedPeerCommandEvent<ChokeCommand>(source, command));
                        break;
                    }
                    case PeerCommandId.Unchoke:
                    {
                        var command = new UnchokeCommand();
                        command.FromByteArray(messageData);
                        Bus.Instance.Publish(new ReceivedPeerCommandEvent<UnchokeCommand>(source, command));

                        break;
                    }
                    case PeerCommandId.Interested:
                    {
                        var command = new InterestedCommand();
                        command.FromByteArray(messageData);
                        Bus.Instance.Publish(new ReceivedPeerCommandEvent<InterestedCommand>(source, command));

                        break;
                    }
                    case PeerCommandId.NotInterested:
                    {
                        var command = new NotInterestedCommand();
                        command.FromByteArray(messageData);
                        Bus.Instance.Publish(new ReceivedPeerCommandEvent<NotInterestedCommand>(source, command));

                        break;
                    }
                    case PeerCommandId.BitField:
                    {
                        var command = new BitFieldCommand();
                        command.FromByteArray(messageData);
                        Bus.Instance.Publish(new ReceivedPeerCommandEvent<BitFieldCommand>(source, command));

                            break;
                    }
                    case PeerCommandId.Have:
                    {
                        var command = new HaveCommand();
                        command.FromByteArray(messageData);
                        Bus.Instance.Publish(new ReceivedPeerCommandEvent<HaveCommand>(source, command));

                        break;
                    }

                    case PeerCommandId.Request:
                    {
                        var command = new RequestCommand();
                        command.FromByteArray(messageData);
                        Bus.Instance.Publish(new ReceivedPeerCommandEvent<RequestCommand>(source, command));

                        break;
                    }

                    default:
                    {
                        log.Warn("Weird data received!");
                        break;
                    }
                }
            }
        }

        private void SendHandshake(PeerConnection source)
        {
            Status = PeerConnectionStatus.Connected;

            HandshakeCommand command = new HandshakeCommand()
            {
                InfoHash = Torrent.MetaData.Hash
            };

            source.SendAsync(command.ToByteArray());
        }

        #endregion
    }
}
