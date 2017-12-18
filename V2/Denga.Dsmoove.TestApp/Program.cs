using System;
using System.IO;
using System.Net.Mime;
using System.Reflection;
using Denga.Dsmoove.Engine.Data.Entities;
using Denga.Dsmoove.Engine.Data.Repositories;
using Denga.Dsmoove.Engine.Files;
using Denga.Dsmoove.Engine.Peers;
using Denga.Dsmoove.Engine.Pieces;
using Denga.Dsmoove.Engine.TorrentProviders;
using Denga.Dsmoove.Engine.Trackers;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using Microsoft.Extensions.DependencyInjection;
using Ninject;

namespace Denga.Dsmoove.TestApp
{
    public class Host
    {
        public static void Main()
        {
           
            var serviceCollection = new ServiceCollection();
            var serviceProvider = ConfigureServices(serviceCollection);
            var program = new Program(serviceProvider);
            program.Run();

        }

        private static IServiceProvider ConfigureServices(IServiceCollection serviceCollection)
        {
            IKernel kernel = new StandardKernel();

            kernel.Bind<IServiceProvider>().ToConstant(kernel);

            return kernel.Get<IServiceProvider>();
        }

        //{
        //    var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
        //    XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        //    ILoggerFactory loggerFactory =
        //    serviceCollection.AddSingleton<ILoggerFactory>(logRepository);
    }


    public class Program
    {

       

        public Program(IServiceProvider provider)
        {
          //  provider.
        //    ConfigureServices(serviceCollection);
            //    var torrentFile = @"C:\Users\dhell\Downloads\debian-mac-9.3.0-amd64-netinst.iso.torrent";

            // var provider = new FileTorrentProvider(torrentFile);
            //   var metadata = fileTorrent.GetMetaData();

            //var torrent = Torrent.FromMetaData(metadata);

            //   using (var repo = new TorrentRepository())
            //   {
            //        repo.Save(torrent);
            //    }





        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {

        }

        public void Run()
        {
            var provider =
                new UriTorrentProvider(
                    new Uri("https://torrents.linuxmint.com/torrents/linuxmint-17.2-cinnamon-64bit.iso.torrent"));
            var metaData = provider.GetMetaData();
            var torrent = Torrent.FromMetaData(metaData);
            

            
            Console.ReadKey(false);
        }
    }
}
