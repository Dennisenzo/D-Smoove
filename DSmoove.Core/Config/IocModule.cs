using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;

namespace DSmoove.Core.Config
{
    public class IocModule : NinjectModule
    {
        public override void Load()
        {
         //   Bind<IProvideTorrent>().To<UriTorrentProvider>();
        }
    }
}
