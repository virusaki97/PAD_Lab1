using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    class Program
    {
        private static PublisherSocket socket = new PublisherSocket();
        static void Main(string[] args)
        {
            ushort forwardPort;
            socket.Connect("127.0.0.1", 9000); // conectare la broker
            Console.WriteLine("Publisher");

            string text = "";
            while (true) 
            {
                if (socket.isConnected)
                {
                    string tempPort = "";
                    Console.Write("Enter Forward port: "); // porturile la care sa transmita brokerul mesajele
                    tempPort = Console.ReadLine();

                    if (String.IsNullOrWhiteSpace(tempPort))
                        continue;

                    forwardPort = Convert.ToUInt16(tempPort);

                    Console.WriteLine("Please enter a message");
                    text = Console.ReadLine();
                    if (String.IsNullOrWhiteSpace(text))
                    {
                        continue;
                    }

                    byte[] message = Message.CreateMessage(text, forwardPort); // aici pregatim datele in format binar

                    socket.Send(message); // transmite mesaj la broker.
                }
            }
        }
    }
}
