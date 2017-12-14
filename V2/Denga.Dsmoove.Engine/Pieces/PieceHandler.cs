using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Denga.Dsmoove.Engine.Data.Entities;

namespace Denga.Dsmoove.Engine.Pieces
{
    public class PieceHandler
    {
        public Torrent Torrent { get; private set; }

        public PieceHandler()
        {
        
        }

        public void Start(Torrent torrent)
        {
            Torrent = torrent;
            torrent.Pieces = LoadPieces();
        }


        public List<Piece> LoadPieces()
        {
            List<Piece> pieces = new List<Piece>();
            for (int i = 0; i < Torrent.MetaData.Info.Pieces.Length; i += 20)
            {
                int index = (int) Math.Floor(i / 20.0);
                Piece piece = new Piece(Torrent.Id, index * Torrent.MetaData.Info.PieceLength,
                    Torrent.MetaData.Info.PieceLength)
                {
                    Hash = Torrent.MetaData.Info.Pieces.Skip(i).Take(20).ToArray(),
                    Index = index
                };

                pieces.Add(piece);
            }

            var lastPiece = pieces.Last();
            lastPiece.Range.Length = Torrent.MetaData.Info.TotalBytes -
                                     (Torrent.MetaData.Info.PieceLength * (pieces.Count - 1));
            Torrent.BitField = new BitArray(pieces.Count, false);

            return pieces;
        }
    }
}
