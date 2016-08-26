using KTS_Kinect_Tracking_Server.KTS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace KTS_Kinect_Tracking_Server.KTS.Network
{
    class NetworkHandler
    {

        private List<IPAddress> ValidIps;
        private Socket ServerSocket;
        private static List<StateObject> clients = new List<StateObject>();

        internal void initialize()
        {
            ValidIps = getValidNetworkInterfaces();
            ProgramSettings.validIps = ValidIps;
        }

        internal void onStarting()
        {
            try
            {
                //create IPv4 socket
                ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //send messages as quickly as possible
                ServerSocket.NoDelay = true;

                IPAddress selectedIp = ValidIps[ProgramSettings.selectedIpIndex];

                int networkPort = ProgramSettings.port;

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

        internal void onStop()
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

                //start listening for infromation ignore for now only one way communication
                // state.clientSocket.BeginReceive(state.buffer, 0, StateObject.bufferSize, SocketFlags.None, new AsyncCallback(ReciveCallback), state);

                ServerSocket.BeginAccept(new AsyncCallback(AcceptCallback), server);
            }
            catch (Exception) { }

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



        internal void sendMessages(MemoryStream memStream)
        {

            int memStreamLength = (int)memStream.Length;

            byte[] intBytes = BitConverter.GetBytes(memStreamLength - 4);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(intBytes);
            }

            byte[] message = memStream.GetBuffer();

            // set header  intbytes should be in little endian. 
            message[0] = intBytes[0];
            message[1] = intBytes[1];
            message[2] = intBytes[2];
            message[3] = intBytes[3];


            //send object! to all connected people
            lock (clients)
            {

                for (int i = 0; i < clients.Count; i++)
                {

                    try
                    {
                        clients[i].clientSocket.BeginSend(message, 0, memStreamLength, SocketFlags.None, new AsyncCallback(SendCallback), clients[i]);
                    }
                    catch
                    {
                        if (clients.Contains(clients[i]))
                        {
                            clients[i].clientSocket.Close();
                            clients[i].Message.Close();
                            clients.Remove(clients[i]);
                        }
                    }
                }
            }

            //clear memorystream
            memStream.Position = 0;
            memStream.SetLength(0);
        }


        private void SendCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            try
            {
                int bytesSent = state.clientSocket.EndSend(ar);
            }
            catch (Exception)
            {
                //removeClient(state);
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
                if (clients.Contains(client))
                {
                    client.clientSocket.Close();
                    client.Message.Close();
                    ret = clients.Remove(client);
                }
            }
            return ret;
        }



        /// <summary>
        /// State object hold connected client inforamtion such as buffer and socket
        /// </summary>
        public class StateObject
        {
            // Client  socket.
            public Socket clientSocket = null;

            //Socket buffer
            public const int bufferSize = 8048;
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
