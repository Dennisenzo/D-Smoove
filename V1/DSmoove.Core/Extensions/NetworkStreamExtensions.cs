using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DSmoove.Core.Extensions
{
    public static class NetworkStreamExtensions
    {
        public static async Task<int> ReadFullBufferAsync(this NetworkStream networkStream, byte[] buffer)
        {
            int bytesRead = 0;

            while (bytesRead < buffer.Length)
            {
                bytesRead += await networkStream.ReadAsync(buffer, bytesRead, buffer.Length - bytesRead);
            }

            return bytesRead;
        }
    }
}