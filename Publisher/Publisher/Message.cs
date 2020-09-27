using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    class Message
    {
        public static byte[] CreateMessage(string message, ushort port)
        {
            byte[] packet = new byte[message.Length + 2 + 2]; // se aloca memorie pentru packet ( + 2 byti - port, + 2 byti - lungimea mesajului);
            byte[] packetLength = BitConverter.GetBytes((ushort)message.Length); // se calculeaza lungimea mesajului

            Array.Copy(BitConverter.GetBytes((ushort)port), packet, 2); // copiem portu in array binar
            Array.Copy(packetLength, 0, packet, 2, 2); // copiem dim. packetului in array binar
            Array.Copy(Encoding.ASCII.GetBytes(message), 0, packet, 4, message.Length); // copiem mesajul in array binar

            return packet;
        }
    }
}
