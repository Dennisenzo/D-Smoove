using DSmoove.Core.Entities;
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
        private Torrent _torrent;

        public PieceManager(Torrent torrent)
        {
            _torrent = torrent;
        }

        public async Task<List<Piece>> LoadPiecesAsync()
        {
            Task<List<Piece>> task = Task.Factory.StartNew<List<Piece>>(() =>
            {
                List<Piece> pieces = new List<Piece>();
                for (int i = 0; i < _torrent.Metadata.Info.Pieces.Length; i += 20)
                {
                    int index = (int)Math.Floor(i / 20.0);
                    Piece piece = new Piece(index * _torrent.Metadata.Info.PieceLength, _torrent.Metadata.Info.PieceLength);
                    piece.Hash = _torrent.Metadata.Info.Pieces.Skip(i).Take(20).ToArray();
                    piece.Index = index;

                    pieces.Add(piece);
                }

                var lastPiece = pieces.Last();
                lastPiece.Range.Length = _torrent.Metadata.TotalBytes - (_torrent.Metadata.Info.PieceLength * (pieces.Count - 1));
                _torrent.BitField = new BitArray(pieces.Count, false);

                return pieces;
            }
            );

            return await task;
        }

        public Piece GetNextPieceForDownload()
        {
          return  _torrent.Pieces.Waiting.OrderByDescending(p => p.Availability).First();
        }
    }
}