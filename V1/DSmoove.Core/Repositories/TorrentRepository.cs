using DSmoove.Core.Entities;
using EasyMemoryRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Repositories
{
    public class TorrentRepository
    {
        public MemoryRepository MemoryRepository { get; set; }

        public void Add(Torrent torrent)
        {
            MemoryRepository.Add<Torrent>(torrent);
        }

        public Torrent GetById(Guid id)
        {
            return MemoryRepository.SingleOrDefault<Torrent>(t => t.Id == id);
        }
    }
}