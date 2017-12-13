using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using Bencode;
using Denga.Dsmoove.Engine.Data.Entities;
using Denga.Dsmoove.Engine.Data.Repositories;
using Denga.Dsmoove.Engine.Infrastructure;
using Denga.Dsmoove.Engine.Infrastructure.Events;
using Denga.Dsmoove.Engine.Peers;
using log4net;

namespace Denga.Dsmoove.Engine.Trackers
{
    public class TrackerHandler
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Torrent Torrent { get; private set; }

        private readonly Timer _trackerUpdateTimer;

        public TrackerHandler()
        {
            _trackerUpdateTimer = new Timer();
            _trackerUpdateTimer.AutoReset = false;
            _trackerUpdateTimer.Elapsed += Update;
        }

        public void Start(Torrent torrent)
        {
            Torrent = torrent;
            Update();
        }

        private void Update(object sender, ElapsedEventArgs e)
        {
            Update();
        }

        public void Update()
        {
            log.Debug("Starting tracker update...");
            _trackerUpdateTimer.Stop();

            var infoHash = HttpUtility.UrlEncode(Torrent.MetaData.Hash);

            var queryStringBuilder = new StringBuilder();

            queryStringBuilder.Append("?info_hash=" + infoHash);
            queryStringBuilder.Append("&peer_id=" + SettingsRepository.Get().PeerId);
            queryStringBuilder.Append("&key=" + "12345");
            queryStringBuilder.Append("&port=" + SettingsRepository.Get().ListeningPort);
            queryStringBuilder.Append("&left=" + Torrent.RemainingBytes);
            queryStringBuilder.Append("&uploaded=" + Torrent.UploadedBytes);
            queryStringBuilder.Append("&downloaded=" + Torrent.DownloadedBytes);

            WebClient client = new WebClient();

            var data = client.DownloadData(Torrent.MetaData.Announce + queryStringBuilder);
            var responseDictionary = BencodeUtility.DecodeDictionary(data);

            TrackerData trackerData = new TrackerData
            {
                InfoHash = Torrent.MetaData.Hash
            };
            if (responseDictionary.ContainsKey("failure reason"))
            {
                var str = Encoding.Default.GetString(responseDictionary["failure reason"] as byte[]);
            }

            else if (responseDictionary["peers"] is byte[] peersBytes)
            {
                //Short peer list
                for (int i = 0; i < peersBytes.Length; i += 6)
                {
                    var ip = new IPAddress(peersBytes.Skip(i).Take(4).ToArray());

                    byte[] portBytes = peersBytes.Skip(i + 4).Take(2).Reverse().ToArray();

                    ushort port = (ushort) BitConverter.ToInt16(portBytes, 0);

                    Torrent.Peers.Add(new PeerData()
                    {
                        IpAddress = ip,
                        Port = port,
                        PeerId = null
                    });
                }
            }
            else if (responseDictionary["peers"] is List<object> peers)
            {
                // Full peer list
                foreach (var peer in peers)
                {
                    if (peer is Dictionary<string, object> peerInfo)
                    {
                        Torrent.Peers.Add(new PeerData()
                        {
                            IpAddress = IPAddress.Parse(Encoding.Default.GetString((byte[])peerInfo["ip"])),
                            Port = int.Parse(peerInfo["port"].ToString()),
                            PeerId = Encoding.Default.GetString((byte[])peerInfo["peer id"])
                        });
                    }
                }
            }

            log.Debug($"Torrent now has {Torrent.Peers.Count} peers!");

            int delay = 300;

            if (responseDictionary.ContainsKey("interval"))
            {
                delay = Int32.Parse(responseDictionary["interval"].ToString());
            }

            Bus.Instance.Publish(new TrackerUpdatedEvent()
            {
                TorrentId = Torrent.Id
            });

            _trackerUpdateTimer.Interval = delay * 1000;
            _trackerUpdateTimer.Start();


            //  await UpdateSubscription.TriggerAsync(trackerData, this);

            log.Debug("Finished tracker update!");
        }
    }
}