namespace Denga.Dsmoove.Engine.Peers.Commands
{
    public class PortCommand : BasePeerCommand
    {
        public PortCommand() : base(PeerCommandId.Port)
        {
        }

        public override byte[] ToByteArray()
        {
            throw new System.NotImplementedException();
        }
    }
}