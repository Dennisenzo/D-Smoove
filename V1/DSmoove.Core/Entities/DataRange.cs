using DSmoove.Core.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Entities
{
    public class DataRange
    {
        public long FirstByte { get; set; }
        public long LastByte { get { return FirstByte + Length - 1; } }

        public long Length { get; set; }

        public DataRange(long firstByte, long length)
        {
            FirstByte = firstByte;
            Length = length;
        }

        public bool IsInRangeOf(DataRange otherRange)
        {
            if (FirstByte >= otherRange.FirstByte && FirstByte <= LastByte)
            {
                return true;
            }
            else if (LastByte >= otherRange.FirstByte && LastByte <= LastByte)
            {
                return true;
            }
            else if (FirstByte < otherRange.FirstByte && LastByte > otherRange.LastByte)
            {
                return true;
            }

            return false;
        }
    }
}