using DSmoove.Core.Enums;
using DSmoove.Core.Interfaces;
using Stateless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Managers
{
    public class DownloadManager : BaseDataManager
    {
        private StateMachine<DownloadState, DownloadTrigger> _stateMachine;
        private IProvidePeers _peerProvider;

        public DownloadManager(IProvidePeers peerProvider)
        {
            _peerProvider = peerProvider;
        }

        public void Start()
        {
         //   _stateMachine.Fire(DownloadTrigger.Start);
        }

        public void Stop()
        {
          //  _stateMachine.Fire(DownloadTrigger.Start);
        }
    }
}