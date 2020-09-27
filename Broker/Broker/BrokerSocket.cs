using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Broker
{
    class BrokerSocket
    {
        private const int CONNECTIONS_NR = 1; // 1 publisher
        private Socket brokerSocket;
        private string multicastIP = "224.0.0.224"; // adresa de multicast

        
        public BrokerSocket()
        {
            brokerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // crearea socketului
        }

        public void Bind(int port)
        {
            brokerSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), port)); // binduim socketu 
        }

        public void Listen()
        {
            brokerSocket.Listen(CONNECTIONS_NR); // incepem sa ascultam 
        }

        public void Accept()
        {
            brokerSocket.BeginAccept(AcceptedCallback, null); // incepem sa acceptam conexiuni
        }

        private void AcceptedCallback(IAsyncResult result) // se apeleaza cind se accepta un pachet
        {
            try
            {
                ConnectionInfo connection = new ConnectionInfo(); // luam informatia de la conexiune

                connection.socket = brokerSocket.EndAccept(result); // terminam transmiterea datelor
                connection.data = new byte[ConnectionInfo.BUFF_SIZE]; // alocam memorie pentru pachetul care a venit
                connection.socket.BeginReceive(connection.data, 0, connection.data.Length, SocketFlags.None, ReceiveCallback, connection);
                // extragerea datelor - se apeleaza ReceiveCallback
            }
            catch (Exception e)
            {
                Console.WriteLine($"Can't accept, {e.Message}");
            }
            finally
            {
                Accept();
            }

        }

        private void ReceiveCallback(IAsyncResult result)
        {
            ConnectionInfo connection = result.AsyncState as ConnectionInfo;

            try
            {
                Socket publisherSocket = connection.socket; // socketul de la publisher
                SocketError response;
                int buffSize = publisherSocket.EndReceive(result, out response); // terminam primirea datelor

                if (response == SocketError.Success)
                {
                    byte[] packet = new byte[buffSize];  // alocam memorie pentru packet
                    Array.Copy(connection.data, packet, packet.Length); // copiem datele din packetul transmis

                    var data = PacketHandler.Handle(packet); // aflam portul si mesajul
                    RedirectData(data.message, data.portNumber); // rederictionam datele
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Can't receive data from Client, {e.Message}");
            }
            finally
            {
                try
                {
                    connection.socket.BeginReceive(connection.data, 0, connection.data.Length, SocketFlags.None, ReceiveCallback, connection);
                    //incepem sa primi datele in continuare
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{e.Message}");
                    connection.socket.Close();
                }
            }
        }

        private void RedirectData(string message, ushort port)
        {
            UdpClient udpclient = new UdpClient(); // cream udpClient
            IPAddress multicastaddress = IPAddress.Parse(multicastIP); // cream adresa multicast
            IPEndPoint remoteep = new IPEndPoint(multicastaddress, port); // cream adresa + port la care se vor transmite mesajele
            byte[] buffer = null;

            udpclient.JoinMulticastGroup(multicastaddress); // unim grupa de multicast

            buffer = Encoding.Unicode.GetBytes(message); // transformam in binar mesajul
            udpclient.Send(buffer, buffer.Length, remoteep); // transmitem datele la subscriber
            Console.WriteLine($"Sent {message} to port - {port}" );
        }
    }
}
