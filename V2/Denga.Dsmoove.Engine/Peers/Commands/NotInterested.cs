namespace Denga.Dsmoove.Engine.Peers.Commands
{
    public class NotInterestedCommand : BasePeerCommand
    {
        public NotInterestedCommand()
            : base(PeerCommandId.NotInterested)
        {

        }

        public override byte[] ToByteArray()
        {
            return ToByteArray(true);
        }
    }
}