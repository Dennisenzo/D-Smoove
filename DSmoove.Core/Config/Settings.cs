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
        public static ConnectionSettings Connection { get { return _instance._connection; } }
        public static FileSettings File { get { return _instance._file; } }

        private GeneralSettings _general;
        private ConnectionSettings _connection;
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

            _connection = new ConnectionSettings();

            _connection.ListeningPort = 6881;
            _connection.MaxPeerConnections = 10;

            _file = new FileSettings();

            _file.KeepWriteLockOpen = true;
            _file.DefaultDownloadFolder = @"C:\Temp\DSmoove";

        }

        public class GeneralSettings
        {
            internal GeneralSettings() { }

            public string PeerId { get; set; }
        }

        public class ConnectionSettings
        {
            internal ConnectionSettings() { }

            public int ListeningPort { get; set; }
            public int MaxPeerConnections { get; set; }
        }


        public class FileSettings
        {
            internal FileSettings() { }

            public bool KeepWriteLockOpen { get; set; }
            public string DefaultDownloadFolder { get; set; }
        }
    }
}