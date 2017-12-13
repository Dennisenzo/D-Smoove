using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Denga.Dsmoove.Engine.Data.Entities;
using Denga.Dsmoove.Engine.Helpers;

namespace Denga.Dsmoove.Engine.TorrentProviders
{
    public class UriTorrentProvider : IProvideTorrent
    {
        private readonly Uri uri;

        public UriTorrentProvider(Uri uri)
        {
            this.uri = uri;
        }
        public MetaData GetMetaData()
        {
            var torrentData = GetTorrentData();

            MetaData metadata = MetadataHelper.DecodeMetaData(torrentData);

            return metadata;
        }

        private byte[] GetTorrentData()
        {
            WebClient webClient = new WebClient();

            byte[] torrentData = webClient.DownloadData(uri);

            return torrentData;
        }
    }
}
