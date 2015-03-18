using DSmoove.Core.Config;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DSmoove.Core.Connections
{
    public class IncomingConnection
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private Task _listenerTask;

        public event NewConnectionAsync NewConnectionEvent;
        public delegate Task NewConnectionAsync(TcpClient client);

        private CancellationToken _cancellationToken;

        public IncomingConnection()
        {
            _listenerTask = new Task(() => StartListeningAsync());
            _cancellationToken = new CancellationToken();
        }

        public void StartListening()
        {
            _listenerTask.Start();
        }

        public async void StartListeningAsync()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, Settings.Torrent.ListeningPort);
            listener.Start();
            while (!_cancellationToken.IsCancellationRequested)
            {
                var tcpClient = await listener.AcceptTcpClientAsync();

                if (NewConnectionEvent != null)
                {
                    IPEndPoint endpoint = (IPEndPoint)tcpClient.Client.RemoteEndPoint;

                    log.DebugFormat("New incoming connection from {0}:{1}", endpoint.Address, endpoint.Port);

                    await NewConnectionEvent(tcpClient);
                }
            }
        }
    }
}