using KTS_Kinect_Tracking_Server.Kinect;
using KTS_Kinect_Tracking_Server.Network;
using KTS_Kinect_Tracking_Server.Utilitys;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTS_Kinect_Tracking_Server
{
    class MainClass
    {
        public MainWindow mainWindow;
        public KinectController kinectControl = new KinectController();
        public NetworkController networkControl = new NetworkController();


        // Called when Application window was loaded but not yet been shown.
        public void onApplicationInitialization(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;

            //TODO Clean up this
            try
            {
                // Prep Kinect SDK
                kinectControl.init(this);
                // Prep Network
                networkControl.init();
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

        public void onBodyTrackingUpdated(Body[] bodies)
        {
            //TODO fix this
        }

    }
}
