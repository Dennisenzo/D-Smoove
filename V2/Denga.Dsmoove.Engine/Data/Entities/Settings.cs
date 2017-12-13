using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LiteDB;

namespace Denga.Dsmoove.Engine.Data.Entities
{
    public class Settings: BaseEntity
    {
        public string BaseDownloadFolder { get; set; }
        public string PeerId { get; set; } = "12345678901112131415";
        public int ListeningPort { get; internal set; }

        public Settings()
        {
            Id = 1;
        }
    }
}