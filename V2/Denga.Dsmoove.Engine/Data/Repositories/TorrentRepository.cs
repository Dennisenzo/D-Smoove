using System;
using System.Collections.Generic;
using System.Text;
using Denga.Dsmoove.Engine.Data.Entities;
using LiteDB;

namespace Denga.Dsmoove.Engine.Data.Repositories
{
   public class TorrentRepository : BaseRepository<Torrent>
    {
        public TorrentRepository() : base()
        {
        }

        protected override void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}
