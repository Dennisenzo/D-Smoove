namespace Denga.Dsmoove.Engine.Peers.Commands
{
    public class ChokeCommand : BasePeerCommand
    {
        public ChokeCommand()
            : base(PeerCommandId.Choke)
        {

        }

        public override byte[] ToByteArray()
        {
            return ToByteArray(true);
        }
    }
}