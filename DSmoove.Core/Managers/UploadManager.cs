using DSmoove.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Managers
{
    public class UploadManager 
    {
        private IProvidePeerConnections _peerProvider;

        public UploadManager(IProvidePeerConnections peerProvider)
        {
            _peerProvider = peerProvider;
        }

        internal void Start()
        {
          //  throw new NotImplementedException();
        }
    }
}