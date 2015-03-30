using Bencode;
using DSmoove.Core.Config;
using DSmoove.Core.Entities;
using DSmoove.Core.Helpers;
using DSmoove.Core.Interfaces;
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

        public AsyncSubscription<TrackerData, TrackerManager> UpdateSubscription { get; private set; }

        public TrackerManager()
        {
            _trackerUpdateTimer = new Timer();
            _trackerUpdateTimer.AutoReset = false;
            _trackerUpdateTimer.Elapsed += Update;

            UpdateSubscription = new AsyncSubscription<TrackerData, TrackerManager>();
        }

        public void Start()
        {
           Update(null, null);
        }

        private void Update(object sender, ElapsedEventArgs e)
        {
            Task updateTask = UpdateAsync();
            Task.WaitAll(updateTask);
        }

        public async Task UpdateAsync()
        {
            log.Debug("Starting tracker update...");
            _trackerUpdateTimer.Stop();
            UriQueryBuilder builder = new UriQueryBuilder(_torrent.Metadata.Announce);

            var infoHash = HttpUtility.UrlEncode(_torrent.Metadata.Hash);

            builder.QueryString.Add("info_hash", infoHash);
            builder.QueryString.Add("peer_id", Settings.General.PeerId);
            builder.QueryString.Add("port", Settings.Connection.ListeningPort.ToString());
            builder.QueryString.Add("left", _torrent.RemainingBytes.ToString());
            builder.QueryString.Add("uploaded", _torrent.UploadedBytes.ToString());
            builder.QueryString.Add("downloaded", _torrent.DownloadedBytes.ToString());

            WebClient client = new WebClient();

            var data = client.DownloadData(builder.ToString());
            var responseDictionary = BencodeUtility.DecodeDictionary(data);

            TrackerData trackerData = new TrackerData();
            trackerData.InfoHash = _torrent.Metadata.Hash;

            if (responseDictionary["peers"] is byte[])
            {
                // Compact peer list.
                var peers = (byte[])responseDictionary["peers"];

                for (int i = 0; i < peers.Length; i += 6)
                {
                    var ip = new IPAddress(peers.Skip(i).Take(4).ToArray());

                    byte[] portBytes = peers.Skip(i + 4).Take(2).Reverse().ToArray();

                    ushort port = (ushort)BitConverter.ToInt16(portBytes, 0);

                    trackerData.Peers.Add(new TrackerData.PeerData()
                        {
                            IPAddress = ip,
                            Port = port,
                            PeerId = null
                        });
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

            await UpdateSubscription.TriggerAsync(trackerData, this);

            log.Debug("Finished tracker update!");
        }

    }
}