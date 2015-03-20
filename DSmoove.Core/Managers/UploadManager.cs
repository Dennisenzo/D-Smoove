using DSmoove.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Managers
{
    public class UploadManager : BaseDataManager
    {
        private IHandlePeerConnections _connectionHandler;

        public UploadManager(IHandlePeerConnections connectionHandler)
        {
            _connectionHandler = connectionHandler;
        }

        internal void Start()
        {
            throw new NotImplementedException();
        }
    }
}