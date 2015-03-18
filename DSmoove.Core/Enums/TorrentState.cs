using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Enums
{
    public enum TorrentState
    {
        Stopped,
        Started,
        Initial,
        DownloadingMetadata,
        ConnectingToTracker,
        Downloading,
        Seeding
    }
    public enum TorrentTrigger
    {
        Start,
        Stop,
        DownloadMetaData,
        ConnectToTracker,
        Download
    }
}
