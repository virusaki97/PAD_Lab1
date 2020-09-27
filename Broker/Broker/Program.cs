using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Broker
{
    class Program
    {
  
        static void Main(string[] args)
        {
            Console.WriteLine("Broker");
            BrokerSocket socket = new BrokerSocket();
            ushort port = 9000;
            socket.Bind(port); // va asculta la portu 9000
            socket.Listen(); // incepe sa asculte
            socket.Accept(); // accepta conexiuni
            Console.WriteLine($"Broker is listening on port {port}");

            Console.ReadLine();
        }
    }
}
