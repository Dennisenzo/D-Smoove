using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Config
{
    public class Settings
    {
        public static GeneralSettings General { get { return _instance._general; } }
        public static TorrentSettings Torrent { get { return _instance._torrent; } }
        public static FileSettings File { get { return _instance._file; } }

        private GeneralSettings _general;
        private TorrentSettings _torrent;
        private FileSettings _file;

        private static Settings _instance;

        static Settings()
        {
            _instance = new Settings();
        }

        public Settings()
        {
            _general = new GeneralSettings();

            _general.PeerId = "DS123123123123123123";

            _torrent = new TorrentSettings();

            _torrent.ListeningPort = 6881;

            _file = new FileSettings();

            _file.KeepWriteLockOpen = true;
            _file.DefaultDownloadFolder = @"C:\Temp\DSmoove";

        }

        public class GeneralSettings
        {
            public string PeerId { get; set; }
        }

        public class TorrentSettings
        {
            public int ListeningPort { get; set; }
        }
        public class FileSettings
        {
            public bool KeepWriteLockOpen { get; set; }

            public string DefaultDownloadFolder { get; set; }
        }
    }
}