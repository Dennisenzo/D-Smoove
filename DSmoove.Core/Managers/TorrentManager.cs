using Bencode;
using DSmoove.Core.Config;
using DSmoove.Core.Entities;
using DSmoove.Core.Enums;
using DSmoove.Core.Interfaces;
using DSmoove.Core.Providers;
using EasyMemoryRepository;
using log4net;
using Ninject;
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

        public TorrentState LastState { get; private set; }

        private IProvideTorrent _torrentProvider;

        [Inject]
        public TransferManager TransferManager { get; set; }

        [Inject]
        public FileManager FileManager { get; set; }

        [Inject]
        public TrackerManager TrackerManager { get; set; }

        [Inject]
        public PieceManager PieceManager { get; set; }

        [Inject]
        public MemoryRepository MemoryRepository { get; set; }

        public TorrentManager()
        {
            log.Info("Starting new torrent manager.");
            ConfigureStateMachine(TorrentState.Stopped);

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

        public void Load(IProvideTorrent torrentProvider)
        {
            _torrentProvider = torrentProvider;
            Start();
        }

        private void Download()
        {
            log.Info("Starting torrent download.");
            TransferManager.Start();
        }

        private void ConnectToTracker()
        {
            log.Info("Connecting to tracker.");
            TrackerManager.Start();

            _stateMachine.Fire(TorrentTrigger.Download);
        }


        private async void DownloadMetadata()
        {
            log.Info("Downloading Metadata.");

            Torrent torrent = new Torrent();

            MemoryRepository.Add<Torrent>(torrent);

            torrent.Metadata = await _torrentProvider.GetMetadataAsync();
            torrent.Files = await FileManager.LoadFilesAsync();
            torrent.Pieces = new PieceList(await PieceManager.LoadPiecesAsync());

            await FileManager.PrepareFilesAsync(Settings.File.DefaultDownloadFolder);

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