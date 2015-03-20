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
            _connection = new ConnectionManager();
            _upload = new UploadManager(_connection);
            _download = new DownloadManager(_connection);
        }

        public void Start()
        {
            _connection.Start();
            _download.Start();
            _upload.Start();
        }
    }
}
