using Denga.Dsmoove.Engine.Data.Entities;

namespace Denga.Dsmoove.Engine.TorrentProviders
{
    public interface IProvideTorrent
    {
        MetaData GetMetaData();
    }
}