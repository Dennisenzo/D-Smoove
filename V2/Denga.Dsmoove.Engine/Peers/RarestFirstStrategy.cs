using System;
using System.Collections.Generic;
using System.Linq;
using Denga.Dsmoove.Engine.Data.Entities;
using Denga.Dsmoove.Engine.Pieces;

namespace Denga.Dsmoove.Engine.Peers
{
    public class RarestFirstStrategy :IDownloadStrategy
    {
        public Torrent Torrent { get; }
        public RarestFirstStrategy(Torrent torrent) => Torrent = torrent;

        public Piece GetNextPiece(PeerData peer)
        {
            var rarity = new List<Tuple<Piece, int>>();
            for (int i = 0; i < Torrent.BitField.Count; i++)
            {
                var numberOfPeers = Torrent.Peers.Count(p => p.BitField[i]);
                rarity.Add(new Tuple<Piece, int>(Torrent.Pieces[i], numberOfPeers));
            }

           return rarity.Where(r => r.Item2 == rarity.Min(r2 => r2.Item2)).Select(r=>r.Item1).Random();
        }

        public bool AreWeInterested(PeerData peer)
        {
           for (int i = 0 ;i < peer.BitField.Count;i++)
            {
                bool bit = peer.BitField.Get(i);
                if (bit && Torrent.BitField[i] == false)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public static class ListExtensions
    {
        private static readonly Random random = new Random();
        public static T Random<T>(this IEnumerable<T> items)
        {
            var itemsList = items.ToList();
            var nextRandom = random.Next(itemsList.Count);
            return itemsList[nextRandom];
        }
    }
}