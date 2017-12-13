namespace Denga.Dsmoove.Engine.Peers.Commands
{
    public class CancelCommand : BasePeerCommand
    {
        public CancelCommand() : base(PeerCommandId.Cancel)
        {
        }

        public override byte[] ToByteArray()
        {
            throw new System.NotImplementedException();
        }
    }
}
