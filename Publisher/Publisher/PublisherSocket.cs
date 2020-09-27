using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Publisher
{
    class PublisherSocket
    {
        private Socket publisherSocket;
        private byte[] buffer;
        private const int BUFF_SIZE = 1024;
        private string ip;
        private int port;
        public bool isConnected = false;

        public PublisherSocket()
        {
            publisherSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string ipAddres, int port)
        {
            ip = ipAddres;
            this.port = port;
            publisherSocket.BeginConnect(new IPEndPoint(IPAddress.Parse(ipAddres), port), ConnectCallback, null);
        }

        // se incearca de a se conencta la broker din nou
        private void Reconnect()
        {
            this.isConnected = false;
            publisherSocket.Close();
            publisherSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            publisherSocket.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), ConnectCallback, null);
        }

        public void Send(byte[] data)
        {
            try
            {
                publisherSocket.Send(data);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not Send data, {e.Message}");
                Console.WriteLine($"Reconnecting");
                this.Reconnect();
            }
        }

        // codul ce se porneste la conectare cu brokeru
        private void ConnectCallback(IAsyncResult result)
        {
            if (publisherSocket.Connected)
            {
                try
                {
                    isConnected = true;
                    Console.WriteLine("Connection estabilished");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Can't estabilish a connection, {e.Message}");
                }
            }
            else
            {
                Console.WriteLine("Publisher Socket could not connect to Broker, trying to reconnect in 5 seconds");
                Thread.Sleep(5000);
                this.Reconnect();
            }
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            
        }
    }
}
