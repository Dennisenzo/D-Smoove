using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using Ninject.Extensions.NamedScope;
using Ninject.Extensions.ContextPreservation;
using DSmoove.Core.Managers;
using EasyMemoryRepository;
using DSmoove.Core.Interfaces;

namespace DSmoove.Core.Config
{
    public class IocModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ConnectionManager, IProvidePeerConnections>().To<ConnectionManager>().InCallScope();
            Bind<DownloadManager>().ToSelf().InCallScope();
            Bind<FileManager>().ToSelf().InCallScope();
            Bind<MetadataManager>().ToSelf().InCallScope();
            Bind<PieceManager>().ToSelf().InCallScope();
            Bind<TorrentManager>().ToSelf().InCallScope();
            Bind<TrackerManager, IProvideTrackerUpdates>().To<TrackerManager>().InCallScope();
            Bind<TransferManager>().ToSelf().InCallScope();
            Bind<MemoryRepository>().ToSelf().InSingletonScope();

        }
    }
}