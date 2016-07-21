using UnityEngine;
using System.Collections;
using System;
using MessageData;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class AsynchronousKTSClient
{
    /// <summary> holds important networking information (Might not needed to be a globa)</summary>
    private StateClass State;


    // Callback functions

    /*
public delegate void OnConnectionDelegate(bool status);
OnConnectionDelegate ConnectionCallback;

public delegate void onError();
public delegate void onRecviedBodyData();
public delegate void onDisconnect();

*/

    public delegate void onBodyDelegate(bodyclass[] body);
    private onBodyDelegate onBodyCallback;

    ///<summary> Connect to KTS server for body input </summary>
    ///<param name="ip">String with the ip address in ipv4  "127.0.0.1" for example</param>
    ///<param name="port">integer with port number for the server to connect too</param>
    ///<param name="callback">will be called after a try to connect to server with true if the connection was successfull or false if it falied to connect</param>
    ///<exception cref="ArgumentNullException">The ip parameter is null.</exception>
    ///<exception cref="SocketException">An error occurred when attempting to access the socket.</exception>
    ///<exception cref="ObjectDisposedException">The Socket has been closed.</exception>
    ///<exception cref="SecurityException">A caller higher in the call stack does not have permission for the requested operation.</exception>
    ///<exception cref="ArgumentOutOfRangeException">The port number is not valid.</exception>
    public void connect(string ip, int port, onBodyDelegate onBodyCallback)
    {

        try
        {

            this.onBodyCallback = onBodyCallback;

            //get ip address,
            System.Net.IPAddress ipAddress = System.Net.IPAddress.Parse(ip);  //127.0.0.1 as an example

            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

            // create "Connection/socket" 
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            client.NoDelay = true;

            Debug.Log("connecting to: " + ip + " on port: " + port);
            // Connect to the remote endpoint.
            client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);

            //  ConnectionCallback = callback;
        }
        catch (Exception e)
        {
            throw e;
        }

    }


    private void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket from the state object.
            Socket client = (Socket)ar.AsyncState;

            // Complete the connection.
            client.EndConnect(ar);

            // remove delay so messages are sent and recived as fast as possible.
            client.NoDelay = true;

            //check that we are truly connected
            if (client.Connected)
            {
                //print connection information
                Debug.Log("Connected to " + client.RemoteEndPoint.ToString());

                //create state object to sotre recive infromation and socket.
                State = new StateClass();
                State.Client = client;

                //start receiving data
                State.Client.BeginReceive(State.Buffer, 0, StateClass.BufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallaback), State);
            }

            //    ConnectionCallback(client.Connected);

        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
    }


    private void ReceiveCallaback(IAsyncResult ar)
    {
        //get state object
        StateClass state = (StateClass)ar.AsyncState;

        //get amount of bytes that has been read to buffer.
        int bytesRead = state.Client.EndReceive(ar); //buffer upper bound

        int bufferIndex = 0; //buffer lower bound

        //sanity check, check that at least 1 byte has been recived
        if (bytesRead > 0)
        {
            parseData(state, bytesRead, ref bufferIndex);
        }

    }

    /// <summary>
    /// handles reciving data.
    /// </summary>
    /// <param name="state"></param>
    /// <param name="bytesRead"></param>
    /// <param name="bufferIndex"></param>
    private void parseData(StateClass state, int bytesRead, ref int bufferIndex)
    {

        //check if we have recived header.
        if (state.ExpectedMessageLength == 0)
        {
            if (!GenerateHeaderFromBuffer(state, bytesRead, ref bufferIndex))
            {
                //if generate header fails means that we haven't enough data to generate a header.

                //wait for more information
                state.Client.BeginReceive(state.Buffer, 0, StateClass.BufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallaback), state);
                return;
            }

        }

        //calculate amount to copy 
        int length = Math.Min(state.ExpectedMessageLength - state.ReceivedMessageLength, bytesRead - bufferIndex);

        //copy from buffer to message
        state.Message.Write(state.Buffer, bufferIndex, length);
        // Array.Copy(state.Buffer, bufferIndex, state.Message, state.ReceivedMessageLength, length);

        bufferIndex += length;
        state.ReceivedMessageLength += length;

        if (state.ReceivedMessageLength < state.ExpectedMessageLength)
        {
            //wait for more data
            state.Client.BeginReceive(state.Buffer, 0, StateClass.BufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallaback), state);
            return;
        }
        else
        {

            //generate Object
            GeneratBodyData(state);

            //clear 
            state.ExpectedMessageLength = 0;
            state.ReceivedMessageLength = 0;
            state.Message.Position = 0;
            state.Message.SetLength(0);

            //check if there is more data in the buffer, beacuse then there will probably be a new header and more data.
            if (bytesRead > bufferIndex)
            {
                //recusion please
                parseData(state, bytesRead, ref  bufferIndex);
                return;
            }
            else
            {
                state.Client.BeginReceive(state.Buffer, 0, StateClass.BufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallaback), state);
                return;
            }
        }

    }


    private void GeneratBodyData(StateClass state)
    {

        BinaryFormatter formatter = new BinaryFormatter();

        // Deserialize the hashtable from the file and 
        // assign the reference to the local variable.

        state.Message.Seek(0, SeekOrigin.Begin);
        MessageClass MessageData = (MessageClass)formatter.Deserialize(state.Message);

       // Debug.Log("Message recived!");
        onBodyCallback(MessageData.body);
  

        //  print("recived body tracking data: " +i);
    }

    private bool GenerateHeaderFromBuffer(StateClass state, int bytesRead, ref int bufferIndex)
    {

        // and by RFC 1700 https://tools.ietf.org/html/rfc1700
        // the network will try to follow the most common with big-endian communications.

        int i;
        for (i = 0; bytesRead > (i + bufferIndex) && (i + state.headerIndex) < 4; i++)
        {
            state.headerbuffer[i + state.headerIndex] = state.Buffer[i + bufferIndex];
        }

        state.headerIndex += i;
        bufferIndex += i;

        if (state.headerIndex == 4)
        {

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(state.headerbuffer);
                state.ExpectedMessageLength = BitConverter.ToInt32(state.headerbuffer, 0) ;
            }
            else
            {
                state.ExpectedMessageLength = BitConverter.ToInt32(state.headerbuffer, 0);
            }

            //create buffer for message
            //state.Message = new byte[state.ExpectedMessageLength];
            state.Message.SetLength(state.ExpectedMessageLength);
            state.Message.Capacity = state.ExpectedMessageLength;

            //clear header index
            state.headerIndex = 0;

            //redudndat but easier for debuging
            state.headerbuffer[0] = 0;
            state.headerbuffer[1] = 0;
            state.headerbuffer[2] = 0;
            state.headerbuffer[3] = 0;

            return true;
        }
        else
        {
            return false;
        }

    }

    internal void setOnDisconnected(Action onDisconnected)
    {
        //   throw new NotImplementedException();
    }

}

public class StateClass
{

    public Socket Client = null;

    /// <summary>
    /// 
    /// </summary>
    public const int BufferSize = 1000;

    /// <summary>
    /// temporary buffer.
    /// </summary>
    public byte[] Buffer = new byte[BufferSize];

    public byte[] headerbuffer = new byte[4];
    public int headerIndex = 0;

    public int ExpectedMessageLength = 0;
    public int ReceivedMessageLength = 0;
    public MemoryStream Message = new MemoryStream();
}
