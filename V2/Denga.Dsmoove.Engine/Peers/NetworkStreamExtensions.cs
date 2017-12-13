using System.Net.Sockets;

namespace Denga.Dsmoove.Engine.Peers
{
    public static class NetworkStreamExtensions
    {
        public static int ReadFullBuffer(this NetworkStream networkStream, byte[] buffer)
        {
            int bytesRead = 0;

            while (bytesRead < buffer.Length)
            {
                bytesRead += networkStream.Read(buffer, bytesRead, buffer.Length - bytesRead);
            }

            return bytesRead;
        }
    }
}