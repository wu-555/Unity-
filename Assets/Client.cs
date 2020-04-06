using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace 聊天室服务器
{
    class Client
    {
        //返回Client的连接状态
        public bool ClientConnected {
            get {
                return socket.Connected;
            }
        }

        private Socket socket;
        private byte[] reciveData = new byte[1024];
        private Thread t;
        public Client(Socket s)
        {
            this.socket = s;
            t = new Thread(ReciveMessage);//创建线程接收信息
            t.Start();
        }

        //接收信息
        public void ReciveMessage()
        {
            while (true)//建立死循环不断地接收消息
            {
                if (socket.Poll(10, SelectMode.SelectRead))
                {
                    socket.Close();
                    break;
                }
                int length = socket.Receive(reciveData);
                string reciveMessage = Encoding.UTF8.GetString(reciveData, 0, length);
                Program.BoradCastMessage(reciveMessage);
                Console.WriteLine(reciveMessage);
            }
        }

        public void SendMessage(string message)
        {
            socket.Send(Encoding.UTF8.GetBytes(message));
        }
    }
}
