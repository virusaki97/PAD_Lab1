using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker
{
    class PacketHandler
    {
        public static (string message, ushort portNumber) Handle(byte[] packet)
        {
            ushort portNumber = BitConverter.ToUInt16(packet, 0);
            ushort packetLength = BitConverter.ToUInt16(packet, 2);
            byte[] data = new byte[packetLength];
            Array.Copy(packet, 4, data, 0, packetLength);
            string message = Encoding.Default.GetString(data);

            Console.WriteLine($"Packet received: Length: {packetLength + 2}  ");

            return (message, portNumber);
        }
    }
}
