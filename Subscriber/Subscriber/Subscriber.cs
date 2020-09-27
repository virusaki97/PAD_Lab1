using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber
{
    class Subscriber
    {
        private UdpClient client = new UdpClient(); // clientul udp
        private IPEndPoint localEp; // adresa pe care va fi plasat subscriberu (ip + port)
        private IPAddress multicastaddress = IPAddress.Parse("224.0.0.224"); // adresa de multicast

        public Subscriber(ushort port)
        {
            localEp =  new IPEndPoint(IPAddress.Any, port); // adresa locala pe care e subscriberu
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            client.ExclusiveAddressUse = false; // nu e adresa unica (multicast)
            client.Client.Bind(localEp); // binduim clientul pe adresa noastra
            client.JoinMulticastGroup(multicastaddress); // ne unim cu alte adrese multicast
        }

        public void Receive()
        {
            Byte[] data = client.Receive(ref localEp);
            string strData = Encoding.Unicode.GetString(data);
            Console.WriteLine($"Message Received: {strData}");
        }
    }
}
