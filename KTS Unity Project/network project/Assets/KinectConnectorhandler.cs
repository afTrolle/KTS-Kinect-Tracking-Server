using UnityEngine;
using System.Collections;
using System;
using MessageData;
using System.Net.Sockets;
using System.Net;

public class KinectConnectorhandler
{

    TcpClient tcpClient = null;


    internal void connect(string ip, int port)
    {

        tcpClient = new TcpClient();
        tcpClient.BeginConnect(ip, port, new AsyncCallback(onConnection), tcpClient);

        Debug.Log("connecting to " + ip);
     
    }


    private void onConnection(IAsyncResult ar)
    {

    }

    internal void setIP(string ip)
    {
        //  throw new NotImplementedException();
    }

    internal void setport(int port)
    {
        // throw new NotImplementedException();
    }

    internal void setOnDisconnected(Action onDisconnected)
    {
        //   throw new NotImplementedException();
    }


}
