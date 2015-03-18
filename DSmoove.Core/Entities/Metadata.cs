using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Entities
{
    public class Metadata
    {
        public string Announce { get; set; }

        public DateTime CreationDate { get; set; }

        public MetadataInfo Info { get; set; }

        public byte[] Hash { get; set; }

        public long TotalBytes { get { return Info.Files.Sum(f => f.Length); } }

        public bool IsMultiFile
        {
            get
            {
                return Info.Files.Count > 1;
            }
        }

        public Metadata()
        {
            Info = new MetadataInfo();
        }

        public class MetadataInfo
        {
            public string Name { get; set; }

            public long PieceLength { get; set; }

            public bool Private { get; set; }

            public byte[] Pieces { get; set; }

            public List<MetadataInfoFiles> Files { get; set; }


            public MetadataInfo()
            {
                Files = new List<MetadataInfoFiles>();
            }


            public class MetadataInfoFiles
            {
                public long Length { get; set; }

                public string Md5Sum { get; set; }

                public List<string> Path { get; set; }

                public MetadataInfoFiles()
                {
                    Path = new List<string>();
                }
            }
        }
    }
}