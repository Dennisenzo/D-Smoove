namespace Denga.Dsmoove.Engine.Peers.Commands
{
    public class UnchokeCommand : BasePeerCommand
    {
        public UnchokeCommand()
            : base(PeerCommandId.Unchoke)
        {

        }

        public override byte[] ToByteArray()
        {
            return ToByteArray(true);
        }
    }
}