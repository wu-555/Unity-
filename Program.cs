using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace 聊天室服务器
{
    class Program
    {
        public static List<Client> clientList = new List<Client>();

        public static void BoradCastMessage(string message)
        {
            var notConnClient = new List<Client>();
            foreach (var client in clientList)
            {
                if (client.ClientConnected)
                {
                    client.SendMessage(message);
                }
                else
                {
                    notConnClient.Add(client);
                }
            }
            foreach (var temp in notConnClient)
            {
                clientList.Remove(temp);
            }
        }

        static void Main(string[] args)
        {

            Socket tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            tcpSocket.Bind(new IPEndPoint(IPAddress.Parse("192.168.0.102"),7788));
            tcpSocket.Listen(100);
            while (true)
            {
                Socket socket = tcpSocket.Accept();
                Client client = new Client(socket);
                clientList.Add(client);
            }
        }
    }
}
