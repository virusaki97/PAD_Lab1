using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            ushort port;
            Console.WriteLine("Subscriber");
            Console.Write("Enter subscriber port: ");
            port = Convert.ToUInt16(Console.ReadLine());

            Subscriber subscriber = new Subscriber(port);
            Console.WriteLine("Subscriber started... Ctrl+C - Stop");


            while (true)
            {
                subscriber.Receive();
            }
        }
    }
}
