using DSmoove.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Interfaces
{
    public interface IProvidePeerConnections
    {
        AsyncSubscription<IHandlePeerConnection> PeerAddedSubscription { get; }
        AsyncSubscription<IHandlePeerDownloads, IProvidePeerConnections> PeerReadyForDownloadSubscription { get; }
        AsyncSubscription<IHandlePeerUploads, IProvidePeerConnections> PeerReadyForUploadSubscription { get; }

        IEnumerable<IHandlePeerDownloads> DownloadHandlers { get; }
    }
}