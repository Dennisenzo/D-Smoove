using Bencode;
using DSmoove.Core.Config;
using DSmoove.Core.Entities;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web;

namespace DSmoove.Core.Managers
{
    public class TrackerManager
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private Torrent _torrent;

        private Timer _trackerUpdateTimer;

        public TrackerManager(Torrent torrent)
        {
            _torrent = torrent;
            _trackerUpdateTimer = new Timer();
            _trackerUpdateTimer.AutoReset = false;
            _trackerUpdateTimer.Elapsed += Update;
        }

        public void Start()
        {
            Update(null, null);
        }

        public void Update(object sender, ElapsedEventArgs e)
        {
            log.Debug("Starting tracker update...");
            UriQueryBuilder builder = new UriQueryBuilder(_torrent.Metadata.Announce);

            var infoHash = HttpUtility.UrlEncode(_torrent.Metadata.Hash);

            builder.QueryString.Add("info_hash", infoHash);
            builder.QueryString.Add("peer_id", Settings.General.PeerId);
            builder.QueryString.Add("port", Settings.Torrent.ListeningPort.ToString());
            builder.QueryString.Add("left", _torrent.RemainingBytes.ToString());
            builder.QueryString.Add("uploaded", _torrent.UploadedBytes.ToString());
            builder.QueryString.Add("downloaded", _torrent.DownloadedBytes.ToString());

            WebClient client = new WebClient();

            var data = client.DownloadData(builder.ToString());
            //var test = client.DownloadString(builder.ToString());
            var responseDictionary = BencodeUtility.DecodeDictionary(data);

            if (responseDictionary["peers"] is byte[])
            {
                // Compact peer list.
                var peers = (byte[])responseDictionary["peers"];

                for (int i = 0; i < peers.Length; i += 6)
                {
                    var ip = new IPAddress(peers.Skip(i).Take(4).ToArray());

                    byte[] portBytes = peers.Skip(i + 4).Take(2).Reverse().ToArray();

                    ushort port = (ushort)BitConverter.ToInt16(portBytes, 0);

                    _torrent.AddAndGetPeer(ip, port);
                }
            }
            else
            {
                // Full peer list.
            }

            if (responseDictionary.ContainsKey("interval"))
            {
                int delay = Int32.Parse(responseDictionary["interval"].ToString());
                _trackerUpdateTimer.Interval = delay * 1000;
                _trackerUpdateTimer.Start();
            }
            log.Debug("Finished tracker update!");
        }
    }
}