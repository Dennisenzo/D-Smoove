using log4net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Connections
{
    public class FileConnection
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Guid FileId { get; private set; }

        private string _file;
        private long _size;
        private FileStream _fileStream;

        private ConcurrentQueue<FileWriteCommand> _commandQueue;

        private Task _dequeueTask;

        public FileConnection(Guid fileId, string file, long size)
        {
            FileId = fileId;
            _file = file;
            _size = size;
            _commandQueue = new ConcurrentQueue<FileWriteCommand>();

            _dequeueTask = new Task(() => Dequeue(), TaskCreationOptions.LongRunning);
            _dequeueTask.Start();
        }

        public Task OpenFileAsync()
        {
            Task task = Task.Factory.StartNew(() => OpenFile());
            return task;
        }

        public void OpenFile()
        {
            if (!File.Exists(_file))
            {
                _fileStream = new FileStream(_file, FileMode.CreateNew, FileAccess.ReadWrite);
                byte[] emptyData = new byte[_size];
                Write(new FileWriteCommand(emptyData, 0));
            }
            else
            {
                _fileStream = new FileStream(_file, FileMode.Open, FileAccess.ReadWrite);
            }
        }

        public void Write(FileWriteCommand command)
        {
            _commandQueue.Enqueue(command);
        }

        public async void Dequeue()
        {
            FileWriteCommand command;

            while (true)
            {
                while (_commandQueue.TryDequeue(out command))
                {
                    _fileStream.Position = command.Offset;
                    await _fileStream.WriteAsync(command.Data, 0, command.Data.Length);
                }

                await Task.Delay(5000);
            }
        }
    }

    public class FileWriteCommand
    {
        public byte[] Data { get; private set; }
        public int Offset { get; private set; }

        public FileWriteCommand(byte[] data, int offset)
        {
            Data = data;
            Offset = offset;
        }
    }
}