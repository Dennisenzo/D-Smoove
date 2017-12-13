using DSmoove.Core.Connections;
using DSmoove.Core.Entities;
using EasyMemoryRepository;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Managers
{
    public class FileManager
    {
        private List<FileConnection> _fileConnections;

        [Inject]
        public MemoryRepository MemoryRepository { get; set; }

        public FileManager()
        {
            _fileConnections = new List<FileConnection>();
        }

        public async Task<List<TorrentFile>> LoadFilesAsync(Guid torrentId)
        {
            var torrent = MemoryRepository.SingleOrDefault<Torrent>(t => t.Id == torrentId);

            Task<List<TorrentFile>> task = Task.Factory.StartNew<List<TorrentFile>>(() =>
                {
                    List<TorrentFile> torrentFiles = new List<TorrentFile>();
                    long offset = 0;

                    foreach (var fileInfo in torrent.Metadata.Info.Files)
                    {
                        TorrentFile file = new TorrentFile()
                        {
                            Range = new DataRange(offset, fileInfo.Length),
                            Name = string.Join("", fileInfo.Path.ToArray()),
                        };

                        torrentFiles.Add(file);

                        offset += file.Range.Length;
                    }

                    return torrentFiles;
                });

            return await task;
        }

        public async Task PrepareFilesAsync(Guid torrentId, string folder)
        {
            var torrent = MemoryRepository.SingleOrDefault<Torrent>(t => t.Id == torrentId);

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            foreach (var file in torrent.Files)
            {
                var path = Path.Combine(folder, file.Name);

                FileConnection connection = new FileConnection(file.Id, path, file.Range.Length);

                _fileConnections.Add(connection);

                await connection.OpenFileAsync();
            }
        }

        public void WritePiece(Piece piece)
        {
            var torrent = MemoryRepository.SingleOrDefault<Torrent>(t => t.Id == piece.TorrentId);

            TorrentFile file = torrent.Files.First();
            int fileIndex = 0;

            // Find the first file.
            while (file.Range.FirstByte > piece.Range.FirstByte)
            {
                fileIndex++;
                file = torrent.Files[fileIndex];
            }

            if (file.Range.LastByte >= piece.Range.LastByte)
            {
                // Only file to update.
                var connection = _fileConnections.Single(f => f.FileId == file.Id);

                int offset = (int)(piece.Range.FirstByte - file.Range.FirstByte);

                connection.Write(new FileWriteCommand(piece.GetPieceData(), offset));
            }
            else
            {
                // This piece spans multiple files.
                long dataWritten = 0;

                while (file.Range.LastByte < piece.Range.LastByte)
                {
                    var connection = _fileConnections.Single(f => f.FileId == file.Id);

                    int offset = (int)(piece.Range.FirstByte - file.Range.FirstByte);
                    long writeSize = file.Range.LastByte - (file.Range.FirstByte + offset);

                    byte[] data = new byte[writeSize];

                    Array.Copy(piece.GetPieceData(), dataWritten, data, 0, writeSize);
                    piece.GetPieceData().CopyTo(data, 0);

                    connection.Write(new FileWriteCommand(data, offset));

                    dataWritten += writeSize;
                    fileIndex++;
                    file = torrent.Files[fileIndex];
                }
            }

        }

    }
}