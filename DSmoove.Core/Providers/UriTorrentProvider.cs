using DSmoove.Core.Entities;
using DSmoove.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Providers
{
    public class UriTorrentProvider : IProvideTorrent
    {
        private Uri _uri;

        public UriTorrentProvider(Uri uri)
        {
            _uri = uri;
        }
        public async Task<Metadata> GetMetadataAsync()
        {
            var torrentData = await GetTorrentData();

            Metadata metadata = await MetadataDecoder.DecodeMetadataAsync(torrentData);

            return metadata;
        }

        private async Task<byte[]> GetTorrentData()
        {
            WebClient webClient = new WebClient();

            byte[] torrentData = await webClient.DownloadDataTaskAsync(_uri);

            return torrentData;
        }
    }
}
