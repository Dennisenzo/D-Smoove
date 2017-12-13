using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Bencode;
using Denga.Dsmoove.Engine.Data.Entities;

namespace Denga.Dsmoove.Engine.Helpers
{
    public static class MetadataHelper
    {
        public static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static MetaData DecodeMetaData(byte[] torrentData)
        {
            var metaData = new MetaData();

            var parsedData = BencodeUtility.DecodeDictionary(torrentData);

            metaData.Announce = Encoding.UTF8.GetString((byte[]) parsedData["announce"]);
            metaData.CreationDate = Epoch.AddSeconds((long) parsedData["creation date"]);

            metaData.Info = new MetaData.MetadataInfo();

            var parsedInfoData = (Dictionary<string, object>) parsedData["info"];

            metaData.Info.PieceLength = (long) parsedInfoData["piece length"];
            metaData.Info.Pieces = (byte[]) parsedInfoData["pieces"];
            metaData.Info.Private = parsedInfoData.ContainsKey("private") && (long) parsedInfoData["private"] == 1;

            metaData.Info.Name = Encoding.UTF8.GetString((byte[]) parsedInfoData["name"]);

            using (SHA1Managed sha1 = new SHA1Managed())
            {
                var data = BencodeUtility.Encode(parsedData["info"]);
                metaData.Hash = sha1.ComputeHash(data.ToArray());
            }

            if (parsedInfoData.ContainsKey("files"))
            {
                //Multi file

                foreach (var fileListItem in (List<object>) parsedInfoData["files"])
                {
                    var fileDictionary = (Dictionary<string, object>) fileListItem;

                    var file = new MetaData.MetadataInfo.MetadataInfoFiles();

                    file.Length = (long) fileDictionary["length"];
                    file.Md5Sum = fileDictionary.ContainsKey("md5sum")
                        ? Encoding.UTF8.GetString((byte[]) parsedInfoData["md5sum"])
                        : null;

                    if (fileDictionary.ContainsKey("path"))
                    {
                        List<object> path = (List<object>) fileDictionary["path"];
                        file.Path = new List<string>();

                        foreach (object pathPart in path)
                        {
                            file.Path.Add(Encoding.UTF8.GetString((byte[]) pathPart));
                        }
                    }


                    metaData.Info.Files.Add(file);
                }
            }
            else
            {
                // Single file
                metaData.Info.Files.Add(new MetaData.MetadataInfo.MetadataInfoFiles());

                metaData.Info.Files[0].Length = (long) parsedInfoData["length"];
                metaData.Info.Files[0].Md5Sum = parsedInfoData.ContainsKey("md5sum")
                    ? Encoding.UTF8.GetString((byte[]) parsedInfoData["md5sum"])
                    : null;
                metaData.Info.Files[0].Path = new List<string> {metaData.Info.Name};


            }
            return metaData;

        }
    }
}