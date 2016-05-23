using KTS_Kinect_Tracking_Server.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTS_Kinect_Tracking_Server.Utilitys
{
    static class SettingsHelper
    {

        // get which camera should be used
        public static String getSelectKinectCamera()
        {
            return Settings.Default.KinectCamera;
        }

        // set wether or not the camera should be enabled
        public static void setKinectCameraPreviewEnabled(bool status)
        {
            Settings.Default.isKinectCameraEnabled = status;
            Settings.Default.Save();
        }

        // Load if the camera should be enabled
        public static bool isKinectCameraPreviewEnabled()
        {
            return Settings.Default.isKinectCameraEnabled;
        }


        // set if logs should be printed to file or not
        public static void setLoggingEnabled(bool status)
        {
            Settings.Default.isLogingEnabled = status;
            Settings.Default.Save();
        }

        // get if logs are enabled or not
        public static bool isLoggingEnabled()
        {
            return Settings.Default.isLogingEnabled;
        }

    }
}
