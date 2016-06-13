﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using KTS_Kinect_Tracking_Server.Utilitys;
using KTS_Kinect_Tracking_Server.Properties;
using Microsoft.Kinect;
using System.IO;

namespace KTS_Kinect_Tracking_Server.Network
{
    class NetworkController
    {
        MainClass mClass;

        // list over available ips
        public List<IPAddress> validIps;

        private Socket ServerSocket;

        internal void init(MainClass mClass)
        {

            this.mClass = mClass;

            validIps = getValidNetworkInterfaces();

            if (!(validIps.Count > 0))
            {
                throw new NetworkControllerInitException("No Ipv4 Network interface found");
            }

            //set
            setNetworkInterfaceOptions();

        }

        internal async Task StartServerAsync()
        {

            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            ServerSocket.NoDelay = true;

            IPAddress selectedIp = validIps[mClass.mainWindow.NetworkInterfaceComboBox.SelectedIndex];
            int networkport = Settings.Default.Port;
            IPEndPoint localEndPoint = new IPEndPoint(selectedIp, networkport);
            ServerSocket.Bind(localEndPoint);

            try
            {

                ServerSocket.Listen(100);
                ServerSocket.BeginAccept(AcceptCallback, ServerSocket);

            }
            catch (Exception e)
            {

            }

            return;
        }


        internal void StopTracking()
        {
            // throw new NotImplementedException();
            ServerSocket.Close();
            ServerSocket.Dispose();
        }

        internal void ApplicationClosing()
        {
            //    throw new NotImplementedException();
        }

        /*     Helper functions          */

        //finds valid ipv4 network.
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

        private void setNetworkInterfaceOptions()
        {
            for (int i = 0; i < validIps.Count; i++)
            {

                mClass.mainWindow.NetworkInterfaceComboBox.Items.Add(validIps[i].ToString());
                if (Settings.Default.NetworkInterface == validIps[i].ToString())
                {
                    mClass.mainWindow.NetworkInterfaceComboBox.SelectedIndex = i;
                }

            }


            if (mClass.mainWindow.NetworkInterfaceComboBox.SelectedIndex == -1)
            {
                mClass.mainWindow.NetworkInterfaceComboBox.SelectedIndex = 0;
            }
        }

        /*     Network Callbacks         */

        private void AcceptCallback(IAsyncResult ar)
        {

            try
            {
                // Get the socket that handles the client request.
                Socket listener = (Socket)ar.AsyncState;

                if (listener.Connected == true)
                {
                    //get connected client socket
                    Socket handler = listener.EndAccept(ar);

                    // create object handling client
                    StateObject state = new StateObject();
                    state.connection = handler;
                    // start listening for message to recive!
                    handler.BeginReceive(state.buffer, 0, StateObject.bufferSize, SocketFlags.None, ReadCallback, state);

                    // Accept new connections to the server!
                    listener.BeginAccept(AcceptCallback, listener);
                }

            }
            catch (Exception e)
            {
                // catch exeception
                Console.WriteLine(e.Message);
            }

        }


        private void ReadCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.connection;

            // Read data from the client socket.
            int bytesRead = handler.EndReceive(ar);

            //check if we recived more than 0 bytes
            if (bytesRead > 0)
            {


                int offset = 0;

                if (state.expectedMessageLength == 0 && bytesRead > 3)
                {
                    // first 4 bytes of a message is the total length of the message
                    state.expectedMessageLength = BitConverter.ToInt32(state.buffer, 0);
                    state.message = new byte[state.expectedMessageLength];
                   
                    offset = 4;
                }

                // Copy, 
                Array.Copy(state.buffer, offset, state.message, state.receivedMessageLength, Math.Min(bytesRead - offset, state.expectedMessageLength - state.receivedMessageLength));
                state.receivedMessageLength += (bytesRead - offset);

                if (state.receivedMessageLength > state.expectedMessageLength)
                {
                    // Here is message REcived 

                   

                    // Clear buffers
                    state.expectedMessageLength = 0;
                    state.receivedMessageLength = 0; //redudnat 
                    state.message = null;

                }


            }
            // wait for more data
            handler.BeginReceive(state.buffer, 0, StateObject.bufferSize, SocketFlags.None, ReadCallback, state);
        }


        // State object for reading client data asynchronously
        public class StateObject
        {
            // Client  socket.
            public Socket connection = null;

            public const int bufferSize = 2048;
            public byte[] buffer = new byte[bufferSize];
            public int expectedMessageLength = 0;
            public int receivedMessageLength = 0;
            public byte[] message = null;
        }

    }

    [Serializable]
    public class MessageObject
    {
        public Instruction MessageInstruction;

        public Status status;

        public Person[] Person;

        [Serializable]
        public enum Status { Sucess, Failed, }
        [Serializable]
        public enum Instruction { onPositionUpdate, onIdentification, onError }
    }

}
