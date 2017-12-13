using DSmoove.Core.Entities;
using EasyMemoryRepository;
using Ninject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Managers
{
    public class PieceManager
    {
        [Inject]
        public MemoryRepository MemoryRepository { get; set; }

        public async Task<List<Piece>> LoadPiecesAsync(Guid torrentId)
        {
            var torrent = MemoryRepository.SingleOrDefault<Torrent>(t => t.Id == torrentId);

            Task<List<Piece>> task = Task.Factory.StartNew<List<Piece>>(() =>
            {
                List<Piece> pieces = new List<Piece>();
                for (int i = 0; i < torrent.Metadata.Info.Pieces.Length; i += 20)
                {
                    int index = (int)Math.Floor(i / 20.0);
                    Piece piece = new Piece(torrent.Id, index * torrent.Metadata.Info.PieceLength, torrent.Metadata.Info.PieceLength);
                    piece.Hash = torrent.Metadata.Info.Pieces.Skip(i).Take(20).ToArray();
                    piece.Index = index;

                    pieces.Add(piece);
                }

                var lastPiece = pieces.Last();
                lastPiece.Range.Length = torrent.Metadata.TotalBytes - (torrent.Metadata.Info.PieceLength * (pieces.Count - 1));
                torrent.BitField = new BitArray(pieces.Count, false);

                return pieces;
            }
            );

            return await task;
        }

        public Piece GetNextPieceForDownload()
        {
            return null;//  return  _torrent.Pieces.Waiting.OrderByDescending(p => p.Availability).First();
        }
    }
}