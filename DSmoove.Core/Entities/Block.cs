using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Entities
{
    public class Block
    {
        public int PieceIndex { get; set; }
        public long PieceOffset { get; set; }

        public bool Downloaded { get; set; }
        public byte[] Data { get; set; }
        public DataRange Range { get; set; }
    }
}