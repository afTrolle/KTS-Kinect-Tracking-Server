using KTS_Kinect_Tracking_Server.Utilitys;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;


namespace KTS_Kinect_Tracking_Server.Kinect
{
    class KinectController
    {
        // main kinect sensor variable
        KinectSensor kSensor;

        // Body tracking data
        BodyFrameReader bodyReader;
        Body[] bodies;

        //image readers
        InfraredFrameReader irReader;
        ColorFrameReader colorReader;
        DepthFrameReader depthReader;

        MultiSourceFrameReader test;

        // camera image Buffer
        WriteableBitmap kinectPreviewBitmap;

        /* Application Running cycle   */

        // inizalation of kinect sensor
        internal void init(MainClass mainClass)
        {
            //get Kinect Sensor
            try
            {
                kSensor = KinectSensor.GetDefault();

                kSensor.PropertyChanged += KSensor_PropertyChanged;

                bodies = new Body[6]; //kinect camera can max track 6 people


                // TODO get UI Components that will be drawn too. 


                // TODO get user settings and set event handlers.


                // setup body tracking / reader
                bodyReader = kSensor.BodyFrameSource.OpenReader();

                bodyReader.FrameArrived += bodyReader_FrameArrived;

            }
            catch (TypeInitializationException)
            {
                throw new KinectInitException("Kinect SDK 2.0 not installed");
            }

        }


        // Start tracking active
        internal async Task StartTrackingAsync()
        {

            // check if a kinect device is connected
        /*    if (!isKinectConnected())
            {
                throw new Exception("Kinect sensor is not connected");
            }
         */

            //start Kinect sensor
            kSensor.Open();

            // wait for 10 sec for the kinect to start
            for (int sec = 0; sec < 10; sec++)
            {

                await Task.Delay(1000);

                if (kSensor.IsOpen && kSensor.IsAvailable)
                {
                    // break waiting cycle
                    return;
                }
            }

            kSensor.Close();
            throw new Exception("Kinect Failed to start");
        }

        // Stop Tracking 
        internal void StopTracking()
        {
            //   throw new NotImplementedException();
        }

        // program is about to be shutdown realse memory.
        internal void onApplicationExit()
        {
            //         throw new NotImplementedException();
        }



        /* Kinect Events  */

        // called when skeleton tracking has been updated
        private void bodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs args)
        {

            using (BodyFrame bodyFrame = args.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    bodyFrame.GetAndRefreshBodyData(bodies);
                    // updated body data

                }
            }
        }


        private void KSensor_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (e.PropertyName.Equals("IsAvailable"))
            {

                if (kSensor.IsAvailable)
                {

                }
                else
                {

                }

            }
            else if (e.PropertyName.Equals("IsOpen"))
            {

                if (kSensor.IsOpen)
                {

                }
                else
                {

                }

            }

        }




        /* Helper Functions  */ 

        // checks device manager if kinect is connected could be made better but works.
        private bool isKinectConnected()
        {
            ManagementObjectCollection collection;
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_PnPEntity WHERE Description LIKE '%Kinect%' "))
                collection = searcher.Get();
            if (collection != null)
            {
                if (collection.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }


    }
}
