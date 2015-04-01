using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Entities
{
    public class Piece
    {
        public int Index { get; set; }
        public byte[] Hash { get; set; }

        public int Availability { get; set; }

        public List<Block> Blocks { get; set; }

        public DataRange Range { get; set; }

        public bool Verified { get; private set; }

        public PieceStatus Status { get; set; }

        public Guid TorrentId { get; private set; }

        public Piece(Guid torrentId, long firstByte, long length)
        {
            TorrentId = torrentId;

            Blocks = new List<Block>();
            Range = new DataRange(firstByte, length);
            BuildBlocks();
            Status = PieceStatus.Initial;
        }

        private void BuildBlocks()
        {
            int blockSize = 1024 * 16;
            int blocksWritten = 0;
            while (blocksWritten < Range.Length)
            {
                Block block = new Block();
                block.Range = new DataRange(Range.FirstByte + blocksWritten, blockSize);
                block.Data = new byte[blockSize];

                block.PieceIndex = Index;
                block.PieceOffset = blocksWritten;
                Blocks.Add(block);

                blocksWritten += blockSize;
            }
            var lastBlock = Blocks.Last();

            if (lastBlock.Range.LastByte > Range.LastByte)
            {
                long difference = lastBlock.Range.LastByte - Range.LastByte;
                lastBlock.Range.Length -= difference;

            }
        }

        public bool Verify(byte[] hash)
        {
            Verified = false;

            if (hash.Length != Hash.Length)
            {
                return false;
            }

            for (int i = 0; i < hash.Length; i++)
            {
                if (hash[i] != Hash[i])
                {
                    return false;
                }
            }
            Verified = true;
            return true;
        }

        public byte[] GetPieceData()
        {
            var blockData = Blocks.Select(b => b.Data).ToList();
            byte[] pieceData = new byte[Blocks.Sum(b => b.Range.Length)];

            int firstByte = 0;

            for (int i = 0; i < blockData.Count; i++)
            {
                var data = blockData[i];
                data.CopyTo(pieceData, firstByte);

                firstByte += data.Length;
            }

            return pieceData;
        }
    }

    public enum PieceStatus
    {
        Initial,
        Downloading,
        PartiallyDownloaded,
        Queued,
        Downloaded,
        Verified,
        Completed
    }
}