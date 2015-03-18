using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Entities
{
    public class PieceList
    {
        private List<Piece> _pieces;

        public IEnumerable<Piece> All { get { return _pieces; } }
        public IEnumerable<Piece> Waiting { get { return _pieces.Where(p => p.Status == PieceStatus.Initial || p.Status == PieceStatus.PartiallyDownloaded); } }
        public IEnumerable<Piece> Downloaded { get { return _pieces.Where(p => p.Status == PieceStatus.Downloaded); } }
        public IEnumerable<Piece> Verified { get { return _pieces.Where(p => p.Status == PieceStatus.Verified); } }

        public int Count { get; private set; }

        public PieceList(List<Piece> pieces)
        {
            _pieces = pieces;
            Count = _pieces.Count;
        }

        public Piece ByIndex(int index)
        {
            return _pieces.Single(p => p.Index == index);
        }
    }
}