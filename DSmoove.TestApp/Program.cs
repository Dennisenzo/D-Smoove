using DSmoove.Core;
using DSmoove.Core.Config;
using DSmoove.Core.Managers;
using DSmoove.Core.Providers;
using log4net;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DSmoove.TestApp
{
    public class Program
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Main(string[] args)
        {
            UriTorrentProvider provider = new UriTorrentProvider(new Uri("http://www.nyaa.se/?page=download&tid=665880"));
            TorrentManager torrentJob = new TorrentManager();
            
            IKernel kernel = new StandardKernel(new IocModule());
            var torrentManager = kernel.Get<TorrentManager>();
            
            torrentJob.Load(provider);

            Console.ReadKey();

            torrentJob.Stop();
        }
    }
}

