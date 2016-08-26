using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using System.Windows.Media.Imaging;
using System.Windows;
using KTS_Kinect_Tracking_Server.KTS.Gui;
using KTS_Kinect_Tracking_Server.KTS.Utils;
using static KTS_Kinect_Tracking_Server.KTS.Utils.ProgramSettings;
using System.Windows.Media;

namespace KTS_Kinect_Tracking_Server.KTS.Kinect
{
    class KinectHandler
    {
        // main kinect sensor variable
        KinectSensor kSensor;

        // Body tracking data
        BodyFrameReader bodyReader;
        Body[] bodies;

        //image readers
        InfraredFrameReader irReader = null;
        ColorFrameReader colorReader = null;
        DepthFrameReader depthReader = null;

        // Ui kinect camera output 
        WriteableBitmap kinectPreviewBitmap;
        Int32Rect ImageSize;
        long stride;
        ushort[] imageData;
        byte[] colorImageData;
        byte[] depthPixels;


        public void initialize()
        {
            try
            {
                kSensor = KinectSensor.GetDefault();

                // setup body tracking server
                bodies = new Body[6]; //kinect camera can max track 6 people
            }
            catch (TypeInitializationException)
            {
                throw new Exception("Kinect SDK 2.0 not installed");
            }
        }

        internal async Task onStarting()
        {

            bodyReader = kSensor.BodyFrameSource.OpenReader();
            bodyReader.FrameArrived += bodyReader_FrameArrived;

            //start Kinect sensor
            kSensor.Open();

            // wait for 10 sec for the kinect to start
            for (int sec = 0; sec < 10; sec++)
            {
                await Task.Delay(1000);

                // break waiting cycle kinect is ready to go
                if (kSensor.IsOpen && kSensor.IsAvailable)
                {
                    //check if camera preview is enabled
                    if (ProgramSettings.isKinectCameraPreivewEnabled)
                    {
                        WindowHelper.setKinectCameraCanvasVisibility(true);

                        setupKinectCamera();
                    }

                    return;
                }
            }

        }


        private void bodyReader_FrameArrived(object sender, BodyFrameArrivedEventArgs args)
        {
            // throw new NotImplementedException();
            using (BodyFrame bodyFrame = args.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    bodyFrame.GetAndRefreshBodyData(bodies);
                    // updated body data
                    Main.onBodyTrackingUpdated(bodies);
                }
            }
        }

        // Stop capturing/tracking data.
        internal void onStopping()
        {
            if (ProgramSettings.isKinectCameraPreivewEnabled)
            {
                clearKinectCamera();
            }

            WindowHelper.setKinectCameraCanvasVisibility(false);
            bodyReader.FrameArrived -= bodyReader_FrameArrived;
            bodyReader.Dispose();

            kSensor.Close();
        }

        /***************** Preivew showing camera capture ***********************/

        private void setupKinectCamera()
        {

            int camera = ProgramSettings.SelectedKinectCameraIndex;
            if (ProgramSettings.isKinectCameraPreivewEnabled)
            {

                if (camera == (int)KinectCameraPreviewType.Color)
                {
                    setupColorView();
                }
                else if (camera == (int)KinectCameraPreviewType.Infrared)
                {
                    setupInfraredView();
                }
                else if (camera == (int)KinectCameraPreviewType.Depth)
                {
                    setupDeptView();
                }
            }
        }

        private void clearKinectCamera()
        {
            int camera = ProgramSettings.SelectedKinectCameraIndex;

            if (camera == (int)KinectCameraPreviewType.Color)
            {
                clearColorView();
            }
            else if (camera == (int)KinectCameraPreviewType.Infrared)
            {
                clearInfraredView();
            }
            else if (camera == (int)KinectCameraPreviewType.Depth)
            {
                clearDeptView();
            }
        }


        private void setupColorView()
        {
            colorReader = kSensor.ColorFrameSource.OpenReader();
            //setSensorRect(kSensor.ColorFrameSource.FrameDescription);
            ImageSize = new Int32Rect(0, 0, 1920, 1080);
            stride = 4 * kSensor.ColorFrameSource.FrameDescription.Width;
            colorImageData = new byte[kSensor.ColorFrameSource.FrameDescription.LengthInPixels * 4];

            kinectPreviewBitmap = new WriteableBitmap(1920, 1080, 96, 96, PixelFormats.Bgra32, null);

            WindowHelper.mainWindow.KinectCameraImage.Source = kinectPreviewBitmap;

            colorReader.FrameArrived += ColorReader_FrameArrived;

        }

        private void clearColorView()
        {
            WindowHelper.mainWindow.KinectCameraImage.Source = null;

            colorReader.FrameArrived -= ColorReader_FrameArrived;
            colorReader.Dispose();
            colorReader = null;
        }


        private void setupInfraredView()
        {
            irReader = kSensor.InfraredFrameSource.OpenReader();
            FrameDescription fd = (kSensor.InfraredFrameSource.FrameDescription);
            ImageSize = new Int32Rect(0, 0, fd.Width, fd.Height);
            stride = fd.BytesPerPixel * fd.Width;
            imageData = new ushort[fd.LengthInPixels];

            kinectPreviewBitmap = new WriteableBitmap(512, 424, 96, 96, PixelFormats.Gray16, null);
            WindowHelper.mainWindow.KinectCameraImage.Source = kinectPreviewBitmap;

            irReader.FrameArrived += irReader_FrameArrived;
        }

        private void clearInfraredView()
        {
            irReader.FrameArrived -= irReader_FrameArrived;
            irReader.Dispose();
            irReader = null;
            imageData = null;
            WindowHelper.mainWindow.KinectCameraImage.Source = null;
            kinectPreviewBitmap = null;
        }

        private void setupDeptView()
        {
            depthReader = kSensor.DepthFrameSource.OpenReader();
            //setSensorRect(kSensor.DepthFrameSource.FrameDescription);
            ImageSize = new Int32Rect(0, 0, 512, 424);
            stride = 3 * kSensor.DepthFrameSource.FrameDescription.Width;
            imageData = new ushort[kSensor.DepthFrameSource.FrameDescription.LengthInPixels];
            depthPixels = new byte[3 * kSensor.DepthFrameSource.FrameDescription.LengthInPixels];
            kinectPreviewBitmap = new WriteableBitmap(512, 424, 96, 96, PixelFormats.Bgr24, null);
            WindowHelper.mainWindow.KinectCameraImage.Source = kinectPreviewBitmap;

            depthReader.FrameArrived += DepthReader_FrameArrived;
        }
        private void clearDeptView()
        {
            WindowHelper.mainWindow.KinectCameraImage.Source = null;
            depthReader.FrameArrived -= DepthReader_FrameArrived;
            depthReader.Dispose();
            imageData = null;
            depthPixels = null;
            kinectPreviewBitmap = null;
        }
        //generates depth image however might be better to create hashtable and use that instead
        private void generateDepthImage(DepthFrame depthFrame)
        {
            // Get the min and max reliable depth for the current frame
            float minDepth = depthFrame.DepthMinReliableDistance;
            float maxDepth = depthFrame.DepthMaxReliableDistance;
            float depthSpan = maxDepth - minDepth;

            float colorPoint = depthSpan / 510;
            /* Should implement hash table for color look up or at least create task for this */
            // Convert the depth to RGB
            int colorPixelIndex = 0;
            for (int i = 0; i < imageData.Length; ++i)
            {
                // Get the depth for this pixel
                ushort depth = imageData[i];

                float avarage = (depth / depthSpan);

                if (avarage < 0.5)
                {

                    depthPixels[colorPixelIndex++] = 0;
                    byte color = (byte)(avarage * 510);
                    depthPixels[colorPixelIndex++] = color;
                    depthPixels[colorPixelIndex++] = (byte)(255 - color);

                }
                else if (avarage > 0.5 && avarage < 1.0)
                {
                    byte color = (byte)((avarage - 0.5) * 510);

                    depthPixels[colorPixelIndex++] = color;
                    depthPixels[colorPixelIndex++] = (byte)(255 - color);
                    depthPixels[colorPixelIndex++] = 0;
                }
                else
                {
                    depthPixels[colorPixelIndex++] = 255;
                    depthPixels[colorPixelIndex++] = 0;
                    depthPixels[colorPixelIndex++] = 0;
                }
            }
        }



        public void enablePreivew()
        {

            if (Main.programState == ProgramState.RUNNING)
            {
                WindowHelper.setKinectCameraCanvasVisibility(true);
                setupKinectCamera();
            }

        }

        public void disablePreivew()
        {
            clearKinectCamera();

        }

        /*********************  Kinect sdk callback functions ***************************************/

        private void ColorReader_FrameArrived(object sender, ColorFrameArrivedEventArgs args)
        {
            using (ColorFrame ColorFrame = args.FrameReference.AcquireFrame())
            {
                if (ColorFrame != null)
                {
                    ColorFrame.CopyConvertedFrameDataToArray(colorImageData, ColorImageFormat.Bgra);
                    kinectPreviewBitmap.WritePixels(ImageSize, colorImageData, (int)stride, 0);
                   // ColorFrame.Dispose();
                }
            }
        }

        private void DepthReader_FrameArrived(object sender, DepthFrameArrivedEventArgs args)
        {
            using (DepthFrame DepthFrame = args.FrameReference.AcquireFrame())
            {
                if (DepthFrame != null)
                {
                    DepthFrame.CopyFrameDataToArray(imageData);

                    uint derp = DepthFrame.FrameDescription.BytesPerPixel;
                    generateDepthImage(DepthFrame);

                    kinectPreviewBitmap.WritePixels(ImageSize, depthPixels, (int)stride, 0);
                 //   DepthFrame.Dispose();
                }
            }
        }


        // IR reader
        private void irReader_FrameArrived(object sender, InfraredFrameArrivedEventArgs args)
        {
            using (InfraredFrame irFrame = args.FrameReference.AcquireFrame())
            {
                if (irFrame != null)
                {
                    irFrame.CopyFrameDataToArray(imageData);
                    kinectPreviewBitmap.WritePixels(ImageSize, imageData, (int)stride, 0);
                 //   irFrame.Dispose();
                }
            }
        }

    }
}
