using KTS_Kinect_Tracking_Server.Kinect;
using KTS_Kinect_Tracking_Server.Network;
using KTS_Kinect_Tracking_Server.Utilitys;
using MessageData;
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

        public NetworkClass networkControl;


        // Called when Application window was loaded but not yet been shown.
        public void onApplicationInitialization(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;

            KinectBodyHelper.Initialization();

            //TODO Clean up this
            try
            {
                // Prep Kinect SDK
                kinectControl.init(this);
                // Prep Network
                networkControl = new NetworkClass(this);
            }
            catch (KinectInitException)
            {

            }
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
                networkControl.startServer();
            }
            catch (Exception e)
            {
                kinectControl.StopTracking();
                throw e;
            }

            watch.Start();



        }

        // Called when server should stop hosting and tracking a person.
        public void onApplicationStop()
        {
            kinectControl.StopTracking();

            networkControl.stopServer();
            watch.Stop();
        }


        // Called when application should quit running
        internal void onApplicationExit()
        {
            networkControl.stopServer();
            kinectControl.onApplicationExit();
        }


        MessageClass Message = new MessageClass();

        Stopwatch watch = new Stopwatch();
        int fpscounter = 0;

        public void onBodyTrackingUpdated(Body[] bodies)
        {

            Logger.start();
            //TODO fix this

            bodyclass[] serilazableBodyData = KinectBodyHelper.getSerilazableBodyDataOnlyTracked(bodies);

            Message.instruction = 1;
            Message.bodyCount = serilazableBodyData.Length;
            Message.bodies = serilazableBodyData;

            networkControl.sendMessage(Message);

            Logger.stop();
            fpscounter++;

            if (watch.ElapsedMilliseconds > 1000)
            {
                Double avarageExecuutionTime = Logger.getAvarage();
                int numberofCountedBodies = 0;
                foreach (Body body in bodies)
                {
                    if (body.IsTracked)
                        numberofCountedBodies++;
                }

                GUIHandler.setTrackedNumberOfBodies(numberofCountedBodies, fpscounter, avarageExecuutionTime);
                fpscounter = 0;
                Logger.clear();
                watch.Restart();
            }


        }



        //tempororay fix



    }

}
