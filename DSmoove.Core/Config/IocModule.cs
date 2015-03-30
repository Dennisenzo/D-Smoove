using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using DSmoove.Core.Managers;
using EasyMemoryRepository;

namespace DSmoove.Core.Config
{
    public class IocModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ConnectionManager>().ToSelf();
            Bind<DownloadManager>().ToSelf();
            Bind<FileManager>().ToSelf();
            Bind<MetadataManager>().ToSelf();
            Bind<PieceManager>().ToSelf();
            Bind<TorrentManager>().ToSelf();
            Bind<TrackerManager>().ToSelf();
            Bind<TransferManager>().ToSelf();
            Bind<MemoryRepository>().ToSelf();
        }
    }
}
