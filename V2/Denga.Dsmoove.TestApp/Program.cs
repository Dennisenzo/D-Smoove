using System;
using System.IO;
using System.Reflection;
using Denga.Dsmoove.Engine.Data.Entities;
using Denga.Dsmoove.Engine.Data.Repositories;
using Denga.Dsmoove.Engine.Files;
using Denga.Dsmoove.Engine.Peers;
using Denga.Dsmoove.Engine.TorrentProviders;
using Denga.Dsmoove.Engine.Trackers;
using log4net;
using log4net.Config;

namespace Denga.Dsmoove.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            var settings = new Settings
            {
            
            };

           //    var torrentFile = @"C:\Users\dhell\Downloads\debian-mac-9.3.0-amd64-netinst.iso.torrent";

            // var provider = new FileTorrentProvider(torrentFile);
            //   var metadata = fileTorrent.GetMetaData();

            //var torrent = Torrent.FromMetaData(metadata);

            //   using (var repo = new TorrentRepository())
            //   {
            //        repo.Save(torrent);
            //    }

            var provider =
                new UriTorrentProvider(
                    new Uri("https://torrents.linuxmint.com/torrents/linuxmint-17-mate-nocodecs-64bit-v2.iso.torrent"));
            var metaData = provider.GetMetaData();
            var torrent = Torrent.FromMetaData(metaData);

            var trackerHandler = new TrackerHandler();
            var peerHandler = new PeerHandler(torrent);
            var fileHandler = new FileHandler(torrent);

            fileHandler.Start();
            peerHandler.Start();
            trackerHandler.Start(torrent);
            
            Console.ReadKey(false);
        }
    }
}