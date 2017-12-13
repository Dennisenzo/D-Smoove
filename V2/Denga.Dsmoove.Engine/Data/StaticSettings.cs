using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Denga.Dsmoove.Engine.Data
{
    public class StaticSettings
    {
        public static string BasePath { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "D-Smoove");

        public static string BaseDataPath { get; set; } = Path.Combine(BasePath, "Data");
        public static string BaseDownloadPath { get; set; } = @"C:\temp\dsmoove";


    }
}