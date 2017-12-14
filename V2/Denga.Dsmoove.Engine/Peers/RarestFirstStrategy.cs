using System;
using Denga.Dsmoove.Engine.Data.Entities;
using Denga.Dsmoove.Engine.Pieces;

namespace Denga.Dsmoove.Engine.Peers
{
    public class RarestFirstStrategy :IDownloadStrategy
    {
        public Torrent Torrent { get; }
        public RarestFirstStrategy(Torrent torrent) => Torrent = torrent;

        public Piece GetNextPiece(PeerData peer)
        {

        }
    }
}