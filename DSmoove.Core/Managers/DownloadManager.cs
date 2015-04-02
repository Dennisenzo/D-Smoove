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
    public class DownloadManager 
    {
        private StateMachine<DownloadState, DownloadTrigger> _stateMachine;

        [Inject]
        public IProvidePeerConnections PeerProvider { get; set; }

        public DownloadManager()
        {
            ConfigureStateMachine();
        }

        private void ConfigureStateMachine()
        {
            _stateMachine = new StateMachine<DownloadState, DownloadTrigger>(DownloadState.Stopped);
            _stateMachine.Configure(DownloadState.Stopped)
                .OnEntry(() => Stop())
                .Permit(DownloadTrigger.Start, DownloadState.Started);

            _stateMachine.Configure(DownloadState.Started)
                .OnEntry(() => Start())
                .Permit(DownloadTrigger.Stop, DownloadState.Stopped);
        }

        public void StartManager()
        {
               _stateMachine.Fire(DownloadTrigger.Start);
        }

        public void StopManager()
        {
              _stateMachine.Fire(DownloadTrigger.Start);
        }

        private void Start()
        {
            PeerProvider.PeerReadyForDownloadSubscription.Subscribe(PeerConnected);
        }

        private void Stop()

        { }

        private void PeerConnected(IHandlePeerDownloads peer, IProvidePeerConnections source)
        {
            peer.BitFieldCommandSubscription.Subscribe(BitfieldReceived);
            peer.HaveCommandSubscription.Subscribe(HaveReceived);
            peer.PieceCommandSubscription.Subscribe(PieceReceived);
        }

        private void PieceReceived(IHandlePeerDownloads arg1, PeerCommands.PieceCommand arg2)
        {
            
        }

        private void HaveReceived(IHandlePeerDownloads arg1, PeerCommands.HaveCommand arg2)
        {
         
        }

        private void BitfieldReceived(IHandlePeerDownloads arg1, PeerCommands.BitFieldCommand arg2)
        {
         
        }
    }
}