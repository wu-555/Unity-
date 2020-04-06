using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ServerConn : MonoBehaviour
{
    public InputField InputField;
    public Text text;
    public ScrollRect scrollRect;
    private string ipAddress = "192.168.0.102";
    private int port = 7788;
    private Socket socket;
    private Thread t;
    private byte[] reciveData = new byte[1024];
    private string reciveMessage = "";
    // Start is called before the first frame update
    void Start()
    {
        ConnectedToServer();
    }

    // Update is called once per frame
    void Update()
    {
        if (reciveMessage != null && reciveMessage != "")
        {
            text.text += "\n" + reciveMessage;
            reciveMessage = "";
        }
        scrollRect.verticalNormalizedPosition = 0;
    }

    private void ConnectedToServer()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect(new IPEndPoint(IPAddress.Parse(ipAddress), port));
        Debug.Log("success");
        t = new Thread(ReciveMessage);
        t.Start();
    }

    private void SendMessage()
    {
        byte[] message = Encoding.UTF8.GetBytes(InputField.text);
        socket.Send(message);
    }

    private void ReciveMessage()
    {
        while (true)
        {
            if (socket.Connected == false)
            {
                break;
            }
            int length = socket.Receive(reciveData);
            reciveMessage = Encoding.UTF8.GetString(reciveData, 0, length);
        }
    }

    public void ClickSend()
    {
        SendMessage();
        InputField.text = "";
    }

    private void OnDestroy()
    {
        socket.Shutdown(SocketShutdown.Both);
        socket.Close();
    }
}
