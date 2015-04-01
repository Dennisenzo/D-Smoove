using DSmoove.Core.Enums;
using DSmoove.Core.Interfaces;
using Ninject;
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

        [Inject]
        public IProvidePeerConnections PeerProvider { get; set; }

        public DownloadManager()
        {
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