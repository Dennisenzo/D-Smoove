using Bencode;
using DSmoove.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Helpers
{
    public static class MetadataDecoder
    {
        public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static async Task<Metadata> DecodeMetadataAsync(byte[] torrentData)
        {
            Task<Metadata> task = Task.Factory.StartNew<Metadata>(() => DecodeMetadata(torrentData));

            return await task;
        }

        public static Metadata DecodeMetadata(byte[] torrentData)
        {
            Metadata metadata = new Metadata();

            var parsedData = BencodeUtility.DecodeDictionary(torrentData);

            metadata.Announce = Encoding.UTF8.GetString((byte[])parsedData["announce"]);
            metadata.CreationDate = Epoch.AddSeconds((long)parsedData["creation date"]);

            metadata.Info = new Metadata.MetadataInfo();

            var parsedInfoData = (Dictionary<string, object>)parsedData["info"];

            metadata.Info.PieceLength = (long)parsedInfoData["piece length"];
            metadata.Info.Pieces = (byte[])parsedInfoData["pieces"];
            metadata.Info.Private = parsedInfoData.ContainsKey("private") ? (long)parsedInfoData["private"] == 1 : false;

            metadata.Info.Name = Encoding.UTF8.GetString((byte[])parsedInfoData["name"]);

            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var data = BencodeUtility.Encode(parsedData["info"]);
                metadata.Hash = sha1.ComputeHash(data.ToArray());
            }

            if (parsedInfoData.ContainsKey("files"))
            {
                //Multi file

                foreach (var fileListItem in (List<object>)parsedInfoData["files"])
                {
                    var fileDictionary = (Dictionary<string, object>)fileListItem;

                    var file = new Metadata.MetadataInfo.MetadataInfoFiles();

                    file.Length = (long)fileDictionary["length"];
                    file.Md5Sum = fileDictionary.ContainsKey("md5sum") ? Encoding.UTF8.GetString((byte[])parsedInfoData["md5sum"]) : null;

                    if (fileDictionary.ContainsKey("path"))
                    {
                        List<object> path = (List<object>)fileDictionary["path"];
                        file.Path = new List<string>();

                        foreach (object pathPart in path)
                        {
                            file.Path.Add(Encoding.UTF8.GetString((byte[])pathPart));
                        }
                    }


                    metadata.Info.Files.Add(file);
                }
            }
            else
            {
                // Single file
                metadata.Info.Files.Add(new Metadata.MetadataInfo.MetadataInfoFiles());

                metadata.Info.Files[0].Length = (long)parsedInfoData["length"];
                metadata.Info.Files[0].Md5Sum = parsedInfoData.ContainsKey("md5sum") ? Encoding.UTF8.GetString((byte[])parsedInfoData["md5sum"]) : null;
                metadata.Info.Files[0].Path = new List<string> { metadata.Info.Name };


            }
            return metadata;

        }
    }
}