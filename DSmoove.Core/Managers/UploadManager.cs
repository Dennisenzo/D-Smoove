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
        private IProvidePeers _peerProvider;

        public UploadManager(IProvidePeers peerProvider)
        {
            _peerProvider = peerProvider;
        }

        internal void Start()
        {
          //  throw new NotImplementedException();
        }
    }
}