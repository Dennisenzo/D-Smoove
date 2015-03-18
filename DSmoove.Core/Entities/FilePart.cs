using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Entities
{
    public class FilePart
    {
        public Guid FileId { get; set; }
        public long Offset { get; set; }
        public int Length { get; set; }
    }
}
