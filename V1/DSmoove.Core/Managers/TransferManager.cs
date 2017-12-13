using DSmoove.Core.Interfaces;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Managers
{
    public class TransferManager
    {
        [Inject]
        public UploadManager UploadManager { get; set; }

        [Inject]
        public DownloadManager DownloadManager { get; set; }

        [Inject]
        public ConnectionManager ConnectionManager { get; set; }

        public TransferManager()
        {
        }

        public void Start()
        {
            ConnectionManager.Start();
            UploadManager.Start();
            DownloadManager.StartManager();
        }
    }
}