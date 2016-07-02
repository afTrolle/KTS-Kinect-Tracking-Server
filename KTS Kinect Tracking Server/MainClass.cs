using KTS_Kinect_Tracking_Server.Kinect;
using KTS_Kinect_Tracking_Server.Network;
using KTS_Kinect_Tracking_Server.Utilitys;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using static KTS_Kinect_Tracking_Server.Network.NetworkController;

namespace KTS_Kinect_Tracking_Server
{
    class MainClass
    {
        public MainWindow mainWindow;
        public KinectController kinectControl = new KinectController();
        public NetworkController networkControl = new NetworkController();


        private Person[] Stefans = new Person[8];


        // Called when Application window was loaded but not yet been shown.
        public void onApplicationInitialization(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;

            for (int i = 0; i < 8; i++)
            {
                Stefans[i] = new Person();
            }

            //TODO Clean up this
            try
            {
                // Prep Kinect SDK
                kinectControl.init(this);
                // Prep Network
                networkControl.init(this);
            }
            catch (KinectInitException e)
            {

            }
            catch (NetworkControllerInitException e)
            {

            }

            // Load Settings if it doesn't conflict with kinect and networking options
        }


        internal async Task onApplicationStartAsync()
        {
            try
            {
                await kinectControl.StartTrackingAsync();
                // TODO add network start function
            }
            catch (Exception e)
            {
                throw e;
            }

            try
            {
                await networkControl.StartServerAsync();
            }
            catch (Exception e)
            {
                kinectControl.StopTracking();
                throw e;
            }

        }


        // Called when server should stop hosting and tracking a person.
        public async Task onApplicationStopAsync()
        {
            kinectControl.StopTracking();

            networkControl.StopTracking();
        }


        // Called when application should quit running
        internal void onApplicationExit()
        {
            networkControl.ApplicationClosing();
            kinectControl.onApplicationExit();
        }


        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();

        public void onBodyTrackingUpdated(Body[] bodies)
        {

            //TODO fix this
            for (int i = 0; i < 6; i++)
            {
                if (bodies[i] != null)
                {
                    KinectBodyHelper.setPersonVariables(Stefans[i], bodies[i]);
                }
            }

            //append buffer so we can set size of the object with an int 
            stream.Position = 4;

            // Serialize the customer object graph.
            formatter.Serialize(stream, Stefans);

            //todo SEND BYTE ARRAY!
            foreach (StateObject client in networkControl.ClientStates)
            {
                if (client.connection.Connected)
                {
                    networkControl.sendBodyData(client, stream);
                }
            }

            //clear Stream
            stream.SetLength(0);
        }

    }

}
