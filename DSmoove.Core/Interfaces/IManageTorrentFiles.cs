using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Interfaces
{
    public interface IManageTorrentFiles
    {
        Task PrepareTorrentFilesAsync(Guid torrentId);

        Task WritePiece(Guid torrentId);
    }
}