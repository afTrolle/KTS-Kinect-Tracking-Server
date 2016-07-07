using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System;
using System.IO;

public class netoworking : MonoBehaviour
{


    public string ip;
    public int port;

    // Use this for initialization
    void Start()
    {

        connect(ip, port);
    }

    // Update is called once per frame
    void Update()
    {

    }



    private Socket connect(string ip, int port)
    {

        try
        {

            System.Net.IPAddress ipaddress = System.Net.IPAddress.Parse(ip);  //127.0.0.1 as an example
            Debug.Log("connecting to " + ip);
            IPEndPoint serverAddress = new IPEndPoint(ipaddress, port);
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            client.BeginConnect(serverAddress, new AsyncCallback(ConnectCallback), client);

            return client;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return null;
        }
    }


    private void ConnectCallback(IAsyncResult ar)
    {
        /*
        try
        {
            Socket client = (Socket)ar.AsyncState;
            client.EndConnect(ar);

            StateObject state = new StateObject();
            state.connection = client;
            Debug.Log("connected to " + ip);
            System.Console.WriteLine("connecting to ip");
            client.BeginReceive(state.buffer, 0, StateObject.bufferSize, SocketFlags.None, new AsyncCallback(ReadCallback), state);

        }
        catch (Exception e)
        {
            Console.Out.WriteLine(e.Message);
        }
        */
    }

    private void ReadCallback(IAsyncResult ar)
    {
     /*   Debug.Log("message read!");
        StateObject state = (StateObject)ar.AsyncState;
        Socket client = state.connection;

        // Read data from the client socket.
        int bytesRead = client.EndReceive(ar);

        //check if we recived more than 0 bytes
        if (bytesRead > 0)
        {
            processReciveBuffer(state, bytesRead);
        }

        client.BeginReceive(state.buffer, state.bufferindex, StateObject.bufferSize - state.bufferindex, SocketFlags.None, new AsyncCallback(ReadCallback), state);
         */
    }

}
