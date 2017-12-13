namespace Denga.Dsmoove.Engine.Peers.Commands
{
    public class InterestedCommand : BasePeerCommand
    {
        public InterestedCommand()
            : base(PeerCommandId.Interested)
        {

        }

        public override byte[] ToByteArray()
        {
            return ToByteArray(true);
        }
    }
}