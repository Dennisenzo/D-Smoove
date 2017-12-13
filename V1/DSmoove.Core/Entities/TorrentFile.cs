using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Entities
{
   public class TorrentFile
    {
       public Guid Id { get; set; }
       public string Name { get;set; }
       public DataRange Range { get; set; }

       public TorrentFile()
       {
           Id = Guid.NewGuid();
       }
    }
}
