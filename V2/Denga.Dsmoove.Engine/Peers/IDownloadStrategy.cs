using Denga.Dsmoove.Engine.Pieces;

namespace Denga.Dsmoove.Engine.Peers
{
    public interface IDownloadStrategy
    {
        Piece GetNextPiece(PeerData peer);

        bool AreWeInterested(PeerData peer);
    }
}