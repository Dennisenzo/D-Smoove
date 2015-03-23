using Bencode;
using DSmoove.Core.Config;
using DSmoove.Core.Entities;
using DSmoove.Core.Enums;
using DSmoove.Core.Interfaces;
using DSmoove.Core.Providers;
using log4net;
using Stateless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Managers
{
    public class TorrentManager
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private StateMachine<TorrentState, TorrentTrigger> _stateMachine;

        public Torrent Torrent { get; private set; }

        public TorrentState LastState { get; private set; }

        private IProvideTorrent _torrentProvider;

       private TransferManager _transferManager;
        private FileManager _fileManager;
        private TrackerManager _trackerManager;
        private PieceManager _pieceManager;

        public TorrentManager(IProvideTorrent torrentProvider)
        {
            log.Info("Starting new torrent manager.");
            ConfigureStateMachine(TorrentState.Stopped);
            _torrentProvider = torrentProvider;
            Torrent = new Torrent();
            _fileManager = new FileManager(Torrent);
          //  _peerManager = new OldTransferManager(Torrent, _fileManager);
            _trackerManager = new TrackerManager(Torrent);
            _transferManager = new TransferManager(_trackerManager);
            _pieceManager = new PieceManager(Torrent);
            LastState = TorrentState.DownloadingMetadata;
        }

        private void ConfigureStateMachine(TorrentState initialState)
        {
            _stateMachine = new StateMachine<TorrentState, TorrentTrigger>(initialState);
            _stateMachine.Configure(TorrentState.Stopped)
                .PermitDynamic(TorrentTrigger.Start, GetLastState);

            _stateMachine.Configure(TorrentState.DownloadingMetadata)
                .SubstateOf(TorrentState.Started)
                .OnEntry(() => DownloadMetadata())
                .OnExit(() => SetLastState(TorrentState.DownloadingMetadata))
                .Permit(TorrentTrigger.Stop, TorrentState.Stopped)
                .Permit(TorrentTrigger.ConnectToTracker, TorrentState.ConnectingToTracker);

            _stateMachine.Configure(TorrentState.ConnectingToTracker)
                .SubstateOf(TorrentState.Started)
                .OnEntry(() => ConnectToTracker())
                .OnExit(() => SetLastState(TorrentState.ConnectingToTracker))
                .Permit(TorrentTrigger.Stop, TorrentState.Stopped)
                .Permit(TorrentTrigger.Download, TorrentState.Downloading);

            _stateMachine.Configure(TorrentState.Downloading)
                .SubstateOf(TorrentState.Started)
                .OnEntry(() => Download())
                .OnExit(() => SetLastState(TorrentState.Downloading))
                .Permit(TorrentTrigger.Stop, TorrentState.Stopped);

            _stateMachine.Configure(TorrentState.Seeding)
                .SubstateOf(TorrentState.Started)
                .OnExit(() => SetLastState(TorrentState.Seeding))
                .Permit(TorrentTrigger.Stop, TorrentState.Stopped);
        }

        private void Download()
        {
            log.Info("Starting torrent download.");
            //_peerManager.Start();
        }

        private void ConnectToTracker()
        {
            log.Info("Connecting to tracker.");
            _trackerManager.Start();

            _stateMachine.Fire(TorrentTrigger.Download);
        }


        private async void DownloadMetadata()
        {
            log.Info("Downloading Metadata.");
            Torrent.Metadata = await _torrentProvider.GetMetadataAsync();
            Torrent.Files = await _fileManager.LoadFilesAsync();
            Torrent.Pieces = new PieceList(await _pieceManager.LoadPiecesAsync());
            
            await _fileManager.PrepareFilesAsync(Settings.File.DefaultDownloadFolder);

            _stateMachine.Fire(TorrentTrigger.ConnectToTracker);
        }

        private TorrentState GetLastState()
        {
            return LastState;
        }

        private void SetLastState(TorrentState state)
        {
            LastState = state;
        }

        #region Public Methods

        public void Start()
        {
            _stateMachine.Fire(TorrentTrigger.Start);
        }

        public void Stop()
        {
            _stateMachine.Fire(TorrentTrigger.Stop);
        }

        #endregion
    }
}