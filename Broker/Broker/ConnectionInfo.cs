using System.Net.Sockets;

namespace Broker
{
        class ConnectionInfo
        {
            public const int BUFF_SIZE = 1024;
            public byte[] data = new byte[BUFF_SIZE];
            public Socket socket;
        }
}