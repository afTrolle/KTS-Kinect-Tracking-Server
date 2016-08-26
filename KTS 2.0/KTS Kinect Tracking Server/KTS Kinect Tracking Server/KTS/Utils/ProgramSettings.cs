using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using KTS_Kinect_Tracking_Server.Properties;

namespace KTS_Kinect_Tracking_Server.KTS.Utils
{
    static class ProgramSettings
    {
        internal static MainWindow mainWindow;

        // this might be a waste of memory but should be marginal
        // these variables should always what the ui is reflecting. 

        /// <summary>
        ///  This is set inside NetworkHandler! Supposed to reperesnt available network interfaces. might be overkill.
        /// </summary>
        public static List<IPAddress> validIps { get; set; }
        public static int selectedIpIndex { get; set; }
        public static int port { get; set; }

        public static bool isKinectCameraPreivewEnabled { get; set; }
        public static int SelectedKinectCameraIndex { get; set; }

        public static bool isLoggingEnabled { get; set; }
        public static string logPath { get; set; }


        //enum for knowing what type of camera ,  based from index of ui items
       public enum KinectCameraPreviewType {Color = 0, Infrared = 1, Depth = 2};


        // If there is an ip selected from previous session set that as selected ip.
        private static int setSelectedIPIndex(string tempIp)
        {
            for (int i = 0; i < validIps.Count; i++)
            {

                if (tempIp.Equals(validIps[i].ToString()))
                {
                    return i;
                }

            }
            //default first item.
            return 0;
        }

        internal static void loadSettings()
        {
            //   throw new NotImplementedException();
            selectedIpIndex = setSelectedIPIndex(Settings.Default.selectedIp);
            port = Settings.Default.port;

            isKinectCameraPreivewEnabled = Settings.Default.isPreviewEnabled;
            SelectedKinectCameraIndex = Settings.Default.CameraIndex;

            isLoggingEnabled = Settings.Default.isLoggingEnabled;
            logPath = Settings.Default.LogPath;
        }


        internal static void saveSettings()
        {

            Settings.Default.selectedIp = validIps[selectedIpIndex].ToString();
            Settings.Default.port = port;


            Settings.Default.isPreviewEnabled = isKinectCameraPreivewEnabled;
            Settings.Default.CameraIndex = SelectedKinectCameraIndex;

            Settings.Default.isLoggingEnabled = isLoggingEnabled;
            Settings.Default.LogPath = logPath;

            Settings.Default.Save();

            //   throw new NotImplementedException();
        }

        internal static void setNetworkInterface(string networkInterface)
        {
            selectedIpIndex = setSelectedIPIndex(networkInterface);
        }

        //only one time can be selected
        internal static void setKinectCamera(IList addedItems)
        {
          SelectedKinectCameraIndex =  mainWindow.KinectCameraComboBox.Items.IndexOf(addedItems[0]);
        }
    }
}
