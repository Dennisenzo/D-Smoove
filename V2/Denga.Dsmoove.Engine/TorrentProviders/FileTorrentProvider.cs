using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Denga.Dsmoove.Engine.Data.Entities;
using Denga.Dsmoove.Engine.Helpers;

namespace Denga.Dsmoove.Engine.TorrentProviders
{
    public class FileTorrentProvider : IProvideTorrent
    {
        private string file;

        public FileTorrentProvider(string file)
        {
            this.file = file;
        }
        public MetaData GetMetaData()
        {
            byte[] torrentData = File.ReadAllBytes(file);

            return MetadataHelper.DecodeMetaData(torrentData);
        }
    }
}
