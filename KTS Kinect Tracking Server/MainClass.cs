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

        // if server and kinect camera is running
        private bool isApplicationRunning = false;

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
                //    networkControl.init();
            }
            catch (KinectInitException e)
            {

            }
            catch (NetworkControllerInitException e)
            {

            }

            // Load Settings if it doesn't conflict with kinect and networking options
        }

        // Called  when Application should start tracking
        public void StartTracking()
        {


        }

        // Called when server should stop hosting and tracking a person.
        public void StopTracking()
        {
            kinectControl.StopTracking();

            networkControl.StopTracking();
        }



        internal void ApplicationClosing()
        {
            networkControl.ApplicationClosing();
            kinectControl.ApplicationClosing();
        }

        public void onBodyTrackingUpdated(Body[] bodies)
        {
            //TODO fix this
        }



        internal bool onApplicationStart()
        {

            try
            {
                kinectControl.StartTracking();
            }
            catch (KinectInitException e)
            {
                return false;
            }


            try
            {
                networkControl.StartServer();
            }
            catch (NetworkControllerInitException e)
            {
                kinectControl.StopTracking();
                return false;
            }


            isApplicationRunning = true;

            return true;
        }

        internal bool onApplicationStop()
        {
            isApplicationRunning = false;
            //throw new NotImplementedException();
            return true;
        }

        internal bool getisApplicationRunning()
        {
            return isApplicationRunning;
        }

        internal void setisApplicationRunning(bool status)
        {
            isApplicationRunning = status;

            //maybe add delegate
           
        }

        BackgroundWorker test;
    }
}
