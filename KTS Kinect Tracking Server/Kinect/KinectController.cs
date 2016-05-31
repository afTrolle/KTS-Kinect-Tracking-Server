using KTS_Kinect_Tracking_Server.Properties;
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
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace KTS_Kinect_Tracking_Server.Kinect
{
    class KinectController
    {
        MainClass mainClass;

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



        // inizalation of kinect sensor
        internal void init(MainClass mainClass)
        {
            this.mainClass = mainClass;
            //get Kinect Sensor
            try
            {
                kSensor = KinectSensor.GetDefault();
                // setup body tracking server
                bodies = new Body[6]; //kinect camera can max track 6 people

            }
            catch (TypeInitializationException)
            {
                throw new KinectInitException("Kinect SDK 2.0 not installed");
            }

        }

        //set what preview that should be showing
        private void setPreviewCamera()
        {

            bool active = Settings.Default.isKinectCameraEnabled;
            string status = Settings.Default.KinectCamera;

            ClearPreviewCAmeras();

            if (active == false && (ApplicationState.state == ApplicationState.RUNNING || ApplicationState.state == ApplicationState.STARTING))
            {
                mainClass.mainWindow.KinectCameraImage.Visibility = Visibility.Hidden;
                return;
            }
            else
            {
                mainClass.mainWindow.KinectCameraImage.Visibility = Visibility.Visible;
            }

            // setup camera preview
            if (status.Equals("Color Camera"))
            {

                colorReader = kSensor.ColorFrameSource.OpenReader();
                //setSensorRect(kSensor.ColorFrameSource.FrameDescription);
                ImageSize = new Int32Rect(0, 0, 1920, 1080);
                stride = 4 * kSensor.ColorFrameSource.FrameDescription.Width;
                colorImageData = new byte[kSensor.ColorFrameSource.FrameDescription.LengthInPixels * 4];

                kinectPreviewBitmap = new WriteableBitmap(1920, 1080, 96, 96, PixelFormats.Bgra32, null);
                mainClass.mainWindow.KinectCameraImage.Source = kinectPreviewBitmap;

                colorReader.FrameArrived += ColorReader_FrameArrived;
            }
            else if (status.Equals("Infrared Camera"))
            {

                irReader = kSensor.InfraredFrameSource.OpenReader();
                setSensorRect(kSensor.InfraredFrameSource.FrameDescription);

                kinectPreviewBitmap = new WriteableBitmap(512, 424, 96, 96, PixelFormats.Gray16, null);
                mainClass.mainWindow.KinectCameraImage.Source = kinectPreviewBitmap;

                irReader.FrameArrived += irReader_FrameArrived;

            }
            else if (status.Equals("Depth Camera"))
            {

                depthReader = kSensor.DepthFrameSource.OpenReader();
                //setSensorRect(kSensor.DepthFrameSource.FrameDescription);
                ImageSize = new Int32Rect(0, 0, 512, 424);
                stride = 3 * kSensor.DepthFrameSource.FrameDescription.Width;
                imageData = new ushort[kSensor.DepthFrameSource.FrameDescription.LengthInPixels];
                depthPixels = new byte[3 * kSensor.DepthFrameSource.FrameDescription.LengthInPixels];
                kinectPreviewBitmap = new WriteableBitmap(512, 424, 96, 96, PixelFormats.Bgr24, null);
                mainClass.mainWindow.KinectCameraImage.Source = kinectPreviewBitmap;

                depthReader.FrameArrived += DepthReader_FrameArrived;
            }
        }

        private void ClearPreviewCAmeras()
        {
            // clear old preview.

            if (irReader != null)
            {
                irReader.FrameArrived -= irReader_FrameArrived;
                irReader.Dispose();
                irReader = null;
            }
            else if (depthReader != null)
            {
                depthReader.FrameArrived -= DepthReader_FrameArrived;
                depthReader.Dispose();
                depthReader = null;
            }
            else if (colorReader != null)
            {
                colorReader.FrameArrived -= ColorReader_FrameArrived;
                colorReader.Dispose();
                colorReader = null;
            }

        }

        internal void updatePreviewState()
        {
            setPreviewCamera();
        }

        private void setSensorRect(FrameDescription fd)
        {
            ImageSize = new Int32Rect(0, 0, fd.Width, fd.Height);
            stride = fd.BytesPerPixel * fd.Width;
            imageData = new ushort[fd.LengthInPixels];
        }

        // HOW to draw image kinectPreviewBitmap.WritePixels(ImageSize, imageData, (int)stride, 0);


        // Start tracking active
        internal async Task StartTrackingAsync()
        {

            // check if a kinect device is connected
            setPreviewCamera();

            bodyReader = kSensor.BodyFrameSource.OpenReader();
            bodyReader.FrameArrived += bodyReader_FrameArrived;

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
            mainClass.mainWindow.KinectCameraImage.Visibility = Visibility.Hidden;
            bodyReader.FrameArrived -= bodyReader_FrameArrived;
            bodyReader.Dispose();
            ClearPreviewCAmeras();

            kSensor.Close();
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
                    
                    mainClass.onBodyTrackingUpdated(bodies);

                    bodyFrame.Dispose();
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
                    irFrame.Dispose();
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
                    DepthFrame.Dispose();
                }
            }

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
                    //blue
                    depthPixels[colorPixelIndex++] = 0;

                    byte color = (byte)(avarage * 510);
                    // avarage * 127.5
                    //0.5 * 255 = något;
                    // Write out green byte
                    depthPixels[colorPixelIndex++] = color;

                    // Write out red byte                        
                    depthPixels[colorPixelIndex++] = (byte)(255 - color);

                }
                else if (avarage > 0.5 && avarage < 1.0)
                {
                    //0.5 * 255 = 0
                    //0.5 * 255 = 0 
                    // 1 * 255 = 255
                    byte color = (byte)((avarage - 0.5) * 510);

                    //blue
                    depthPixels[colorPixelIndex++] = color;

                    // avarage * 127.5
                    //0.5 * 255 = något;
                    // Write out green byte
                    depthPixels[colorPixelIndex++] = (byte)(255 - color);

                    // Write out red byte                        
                    depthPixels[colorPixelIndex++] = 0;

                }
                else
                {
                    depthPixels[colorPixelIndex++] = 255;

                    // avarage * 127.5
                    //0.5 * 255 = något;
                    // Write out green byte
                    depthPixels[colorPixelIndex++] = 0;

                    // Write out red byte                        
                    depthPixels[colorPixelIndex++] = 0;

                }

                /*
                // To convert to a byte, we're discarding the most-significant
                // rather than least-significant bits.
                // We're preserving detail, although the intensity will "wrap."
                // Values outside the reliable depth range are mapped to 0 (black).

                // Note: Using conditionals in this loop could degrade performance.
                // Consider using a lookup table instead when writing production code.
                // See the KinectDepthViewer class used by the KinectExplorer sample
                // for a lookup table example.
                byte intensity = (byte)(depth >= minDepth && depth <= maxDepth ? depth : 0);

                // Write out blue byte
                depthPixels[colorPixelIndex++] = intensity;

                // Write out green byte
                depthPixels[colorPixelIndex++] = intensity;

                // Write out red byte                        
                depthPixels[colorPixelIndex++] = intensity;
                */
            }

        }

        // colorpreviewhandler
        private void ColorReader_FrameArrived(object sender, ColorFrameArrivedEventArgs args)
        {

            using (ColorFrame ColorFrame = args.FrameReference.AcquireFrame())
            {
                if (ColorFrame != null)
                {
                    ColorFrame.CopyConvertedFrameDataToArray(colorImageData, ColorImageFormat.Bgra);
                    kinectPreviewBitmap.WritePixels(ImageSize, colorImageData, (int)stride, 0);
                    ColorFrame.Dispose();
                }
            }
            //throw new NotImplementedException();
            //     e.FrameReference.AcquireFrame().CopyRawFrameDataToArray(imageData);

            //   ColorImageFormat color =  e.FrameReference.AcquireFrame().RawColorImageFormat;
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
