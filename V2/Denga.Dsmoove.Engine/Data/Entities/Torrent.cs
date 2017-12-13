using System;
using System.Collections.Generic;
using Denga.Dsmoove.Engine.Peers;
using Denga.Dsmoove.Engine.Trackers;
using LiteDB;

namespace Denga.Dsmoove.Engine.Data.Entities
{
    public class Torrent : BaseEntity
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public MetaData MetaData { get; set; }
        public long RemainingBytes { get; set; }
        public long UploadedBytes { get; set; }
        public long DownloadedBytes { get; set; }

        [BsonIgnore]
        public List<PeerData> Peers { get; set; } = new List<PeerData>();


        public List<TrackerData> Trackers { get; set; } = new List<TrackerData>();

        public static Torrent FromMetaData(MetaData metaData)
        {
            var torrent = new Torrent()
            {
                MetaData = metaData,
                Name = metaData.Info.Name,
                CreatedAt = DateTime.Now
            };

            return torrent;
        }
    }

    public class MetaData
    {
        public MetadataInfo Info { get; set; }
        public string Announce { get; set; }
        public DateTime CreationDate { get; set; }
        public byte[] Hash { get; set; }

        public class MetadataInfo
        {
            public long PieceLength { get; set; }
            public byte[] Pieces { get; set; }
            public bool Private { get; set; }
            public string Name { get; set; }
            public List<MetadataInfoFiles> Files { get; set; } = new List<MetadataInfoFiles>();

            public class MetadataInfoFiles
            {
                public long Length { get; set; }
                public string Md5Sum { get; set; }
                public List<string> Path { get; set; } = new List<string>();
            }
        }
    }
}