using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Entities
{
    public class AvailablePiece
    {
        public int Index { get; set; }
        public int Availability { get; set; }

        public AvailablePiece(int index, int availability = 0)
        {
            Index = index;
            Availability = availability;
        }
        public void Increase()
        {
            Availability++;
        }

        public void Decrease()
        {
            Availability--;
        }
    }
}
