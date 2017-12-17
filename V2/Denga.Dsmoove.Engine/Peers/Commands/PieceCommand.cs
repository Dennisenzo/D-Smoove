namespace Denga.Dsmoove.Engine.Peers.Commands
{
    public class PieceCommand : BasePeerCommand
    {
        public PieceCommand() : base(PeerCommandId.Piece)
        {
        }

        public override byte[] ToByteArray() 
        {
return new byte[0];
        }
    }
}