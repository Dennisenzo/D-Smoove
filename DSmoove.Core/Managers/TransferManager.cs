using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Managers
{
    public class TransferManager
    {
        private UploadManager _upload;
        private DownloadManager _download;
        private ConnectionManager _connection;

        private TransferManager()
        {
            _upload = new UploadManager();
            _download = new DownloadManager();
            _connection = new ConnectionManager();
        }
    }
}
