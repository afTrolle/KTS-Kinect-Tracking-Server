using KTS_Kinect_Tracking_Server.Utilitys;
using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
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
                kinectPreviewBitmap = new WriteableBitmap(512, 424, 96, 96, PixelFormats.Gray16, null);
                mainClass.mainWindow.KinectCameraImage.Source = kinectPreviewBitmap;

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

        internal void setIrImage(System.Windows.Controls.Image irImage)
        {
            irImage.Source = kinectPreviewBitmap;
        }

        private void initCamera()
        {

            //setup ir reader so we get a view of the camera
            irReader = kSensor.InfraredFrameSource.OpenReader();
            FrameDescription fd = kSensor.InfraredFrameSource.FrameDescription;

            //data holders
            ushort[] irData = new ushort[fd.LengthInPixels];

            WriteableBitmap wbmap = new WriteableBitmap(fd.Width, fd.Height, 96, 96, PixelFormats.Gray16, null);
            Int32Rect wbmapRect = new Int32Rect(0, 0, wbmap.PixelWidth, wbmap.PixelHeight);
            int wbmapStride = wbmap.PixelWidth * wbmap.Format.BitsPerPixel / 8;

            wbmap.WritePi§xels();
            //irReader.FrameArrived += irReader_FrameArrived;
        }


        // Start tracking active
        internal async Task StartTrackingAsync()
        {

            // check if a kinect device is connected


            //start Kinect sensor
            kSensor.Open();

            // wait for 10 sec for the kinect to start
            for (int sec = 0; sec < 10; sec++)
            {

                await Task.Delay(1000);

                if (kSensor.IsOpen && kSensor.IsAvailable)
                {
                    // break waiting cycle kinect is ready to go
                    return;
                }
            }

            // This part is run if kinect has not started within 10 sec tell program to look for errors
            kSensor.Close();
            if (!isKinectConnected())
            {
                throw new Exception("Failed to start Kinect sensor, Can not detect Kinect sensor");
            }
            throw new Exception("Kinect sensor failed to start took to long for sensor to start");
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


        /* Kinect Frames Events  */

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


        /*   */

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
