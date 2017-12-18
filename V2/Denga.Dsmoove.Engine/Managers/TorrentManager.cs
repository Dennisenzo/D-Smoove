using System;
using System.Collections.Generic;
using System.Text;
using Denga.Dsmoove.Engine.Data.Entities;
using Denga.Dsmoove.Engine.Files;
using Denga.Dsmoove.Engine.Peers;
using Denga.Dsmoove.Engine.Pieces;
using Denga.Dsmoove.Engine.TorrentProviders;
using Denga.Dsmoove.Engine.Trackers;

namespace Denga.Dsmoove.Engine.Managers
{
  public  class TorrentManager
    {
        private TrackerHandler trackerHandler;
        private FileHandler fileHandler;
        private PieceHandler pieceHandler;
        private PeerHandler peerHandler;

        public Torrent Torrent { get; private set; }

        public TorrentManager(TrackerHandler trackerHandler, FileHandler fileHandler, PieceHandler pieceHandler,
            PeerHandler peerHandler)
        {
            this.trackerHandler = trackerHandler;
            this.fileHandler = fileHandler;
            this.pieceHandler = pieceHandler;
            this.peerHandler = peerHandler;
        }
        public void Start(IProvideTorrent torrentProvider)
        {
            var metaData = torrentProvider.GetMetaData();
            Torrent = Torrent.FromMetaData(metaData);

            fileHandler.Start(Torrent);
            peerHandler.Start(Torrent);
            pieceHandler.Start(Torrent);
            trackerHandler.Start(Torrent);
        }
    }
}
