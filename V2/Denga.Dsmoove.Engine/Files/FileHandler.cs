using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Denga.Dsmoove.Engine.Data;
using Denga.Dsmoove.Engine.Data.Entities;

namespace Denga.Dsmoove.Engine.Files
{
    public class FileHandler
    {
        public Torrent Torrent { get; private set; }

        public void Start(Torrent torrent)
        {
            Torrent = torrent;

            Directory.CreateDirectory(StaticSettings.BaseDownloadPath);

            foreach (var file in Torrent.MetaData.Info.Files)
            {
                var folder = file.Path.Count > 1 ? string.Join("/",file.Path.GetRange(0, file.Path.Count - 1)) :"";
                var absoluteFolder = Path.Combine(StaticSettings.BaseDownloadPath, folder);
                Directory.CreateDirectory(absoluteFolder);
                var absoluteFile = Path.Combine(absoluteFolder, file.Path.Last());
                using (var fileStream = File.Create(absoluteFile))
                {
                    fileStream.SetLength(file.Length);
                }
            }
        }
    }
}