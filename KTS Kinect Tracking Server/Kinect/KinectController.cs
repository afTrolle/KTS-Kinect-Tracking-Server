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

        // camera image Buffer
        WriteableBitmap kinectPreviewBitmap;


        // inizalation of kinect sensor
        internal void init(MainClass mainClass)
        {
            //get Kinect Sensor
            try
            {
                kSensor = KinectSensor.GetDefault();

                kSensor.PropertyChanged += KSensor_PropertyChanged;
                bodies = new Body[6]; //kinect camera can max track 6 people


            }
            catch (TypeInitializationException)
            {
                throw new KinectInitException("Kinect SDK 2.0 not installed");
            }



            // setup body tracking / reader
            bodyReader = kSensor.BodyFrameSource.OpenReader();
           
            bodyReader.FrameArrived += bodyReader_FrameArrived;

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


        internal void StartTracking()
        {

            if (!isKinectConnected())
            {
                throw new KinectInitException("Kinect not connected or recognized");
            }

            // start kinect sensor (initialises ksensors vars)
            kSensor.Open();

        }


        internal void StopTracking()
        {
            //   throw new NotImplementedException();
        }


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



        // Release buffers and tell kinect too shutdown
        internal void ApplicationClosing()
        {
            //         throw new NotImplementedException();
        }

    }
}
