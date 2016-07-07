using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using KTS_Kinect_Tracking_Server.Utilitys;
using System.IO;
using MessageData;
using System.Runtime.Serialization.Formatters.Binary;

namespace KTS_Kinect_Tracking_Server.Network
{
    class NetworkClass
    {
        MainClass MClass;
        private List<IPAddress> ValidIps;

        private Socket ServerSocket;

        //list with all the connected clients!
        private static List<StateObject> clients = new List<StateObject>();

        public NetworkClass(MainClass mClass)
        {
            MClass = mClass;
            ValidIps = getValidNetworkInterfaces();

            //Update UI
            GUIHandler.setAvailableNetworkIps(ValidIps);
        }


        public void startServer()
        {
            try
            {
                //create IPv4 socket
                ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //send messages as quickly as possible
                ServerSocket.NoDelay = true;

                IPAddress selectedIp = ValidIps[GUIHandler.getSelectedNetworkIpIndex()];

                int networkPort = SettingsHelper.getNetworkPort();

                IPEndPoint localEndPoint = new IPEndPoint(selectedIp, networkPort);
                ServerSocket.Bind(localEndPoint);

                //amount of waiting connections
                ServerSocket.Listen(10);
                ServerSocket.BeginAccept(new AsyncCallback(AcceptCallback), ServerSocket);

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }

        }

        public void stopServer()
        {

            lock (clients)
            {
                foreach (StateObject state in clients)
                {
                    state.clientSocket.Shutdown(SocketShutdown.Both);
                    state.clientSocket.Close();

                    state.clientSocket = null;

                }
                //remove all sockets from the list!
                clients.Clear();
            }


            ServerSocket.Close();
            ServerSocket.Dispose();

        }

        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();


        public void sendMessage(MessageClass data)
        {

            //append buffer so we can set header of the object with an int 
            memoryStream.Position = 4;


            // Serialize the messageclass to memory stream
            formatter.Serialize(memoryStream, data);


            int memStreamLength = (int)memoryStream.Length;
            // generates header
            byte[] intBytes = BitConverter.GetBytes(memStreamLength-4);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(intBytes);
            }

            byte[] message = memoryStream.GetBuffer();

            // set header  intbytes should be in little endian. 
            message[0] = intBytes[0];
            message[1] = intBytes[1];
            message[2] = intBytes[2];
            message[3] = intBytes[3];


            //send object!
            lock (clients)
            {
                //send message to all clients!
                foreach (StateObject state in clients)
                {
                    state.clientSocket.BeginSend(message, 0, memStreamLength, SocketFlags.None, new AsyncCallback(SendCallback), state);
                }
            }

            //clear memorystream
            memoryStream.Position = 0;
            memoryStream.SetLength(0);
        }


        // Callbacks!

        /// <summary>
        /// Called when a clients connectes
        /// </summary>
        /// <param name="ar"></param>
        private void AcceptCallback(IAsyncResult ar)
        {
            Socket server = (Socket)ar.AsyncState;

            try
            {
                //connected client
                Socket client = server.EndAccept(ar);

                client.NoDelay = true;

                StateObject state = new StateObject();
                state.clientSocket = client;

                //add connected client!
                addClient(state);

                //start listening for infromation
                state.clientSocket.BeginReceive(state.buffer, 0, StateObject.bufferSize, SocketFlags.None, new AsyncCallback(ReciveCallback), state);


                ServerSocket.BeginAccept(new AsyncCallback(AcceptCallback), server);
            }
            catch (Exception) { }

        }

        /// <summary>
        /// Handles reciving of messages
        /// </summary>
        /// <param name="ar"></param>
        private void ReciveCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;

            try { 
            int bytesRead = state.clientSocket.EndReceive(ar);

            if (state.clientSocket.Connected)
            {

                //check that we recived at least 1 byte of data
                if (bytesRead > 0)
                {

                    //TODO handle message receving!

                }

                // start receving more data again.
                state.clientSocket.BeginReceive(state.buffer, 0, StateObject.bufferSize, SocketFlags.None, new AsyncCallback(ReciveCallback), state);
            }
            else
            {
                // TO DO update this
                //probably lost connection! or lingertime ran out or something!
                removeAndCloseSocket(state);
            }

            } catch { }

        }


        private void SendCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            try
            {
                int bytesSent = state.clientSocket.EndSend(ar);


                // Falied to send any bytes!
                if (bytesSent == 0)
                {
                    removeAndCloseSocket(state);
                }
            }
            catch
            {
                removeAndCloseSocket(state);
            }


        }



        //adding and removing clients from the list 
        private void addClient(StateObject client)
        {
            lock (clients)
            {
                clients.Add(client);
            }
        }

        //removes stateObject from clients list
        private bool removeClient(StateObject client)
        {
            bool ret = false;
            lock (clients)
            {
                ret = clients.Remove(client);
            }
            return ret;
        }

        //shutdown socket and removes it from the list
        private void removeAndCloseSocket(StateObject state)
        {
            try
            {
              //  state.clientSocket.Shutdown(SocketShutdown.Both);
                state.clientSocket.Close();
            }
            catch (Exception) { }

            removeClient(state);
        }

        /// <summary>
        /// Generates list of IPAddress that follows IPv4 that a socket can listen too.
        /// </summary>
        /// <returns>List of ipv4 address</returns>
        private List<IPAddress> getValidNetworkInterfaces()
        {
            List<IPAddress> ValidIps = new List<IPAddress>();

            ValidIps.Add(IPAddress.Parse("127.0.0.1"));

            try
            {
                // Establish the local endpoint for the socket.
                IPHostEntry localHost = Dns.GetHostEntry(Dns.GetHostName());

                //find first ipv4 address 
                for (int i = 0; i < localHost.AddressList.Length; i++)
                {
                    if (localHost.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        ValidIps.Add(localHost.AddressList[i]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return ValidIps;
        }



        /// <summary>
        /// State object hold connected client inforamtion such as buffer and socket
        /// </summary>
        public class StateObject
        {
            // Client  socket.
            public Socket clientSocket = null;

            //Socket buffer
            public const int bufferSize = 2048;
            public byte[] buffer = new byte[bufferSize];

            // message header how many bytes will be recived
            public byte[] headerbuffer = new byte[4];
            public int headerIndex = 0;

            // storage of the message
            public int ExpectedMessageLength = 0;
            public int ReceivedMessageLength = 0;
            public MemoryStream Message = new MemoryStream();
        }


    }

}
