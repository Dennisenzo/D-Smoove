﻿using System;
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

        private byte[] data { get; set; }
        public byte[] Data => data ?? (data = new byte[Range.Length]);


        public DataRange Range { get; set; }

        public BlockStatus Status { get; set; }
    }

    public enum BlockStatus
    {
        Empty,
        Partially,

    }
}