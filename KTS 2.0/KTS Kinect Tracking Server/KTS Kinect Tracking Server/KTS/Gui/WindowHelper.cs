using KTS_Kinect_Tracking_Server.KTS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KTS_Kinect_Tracking_Server.KTS.Gui
{
    //simple functions
    static class WindowHelper
    {
        public static MainWindow mainWindow;

        internal static void setUI()
        {
            loadNetworkInterface();

            mainWindow.NetworkPortTextBox.Text = ProgramSettings.port.ToString();

            mainWindow.KinectCameraComboBox.SelectedIndex = ProgramSettings.SelectedKinectCameraIndex;
            mainWindow.KinectPreviewToggleButton.IsChecked = ProgramSettings.isKinectCameraPreivewEnabled;

            mainWindow.LogsPathTextBlock.Text = ProgramSettings.logPath;
            mainWindow.LoggingToggleButton.IsChecked = ProgramSettings.isLoggingEnabled;
        }

        private static void loadNetworkInterface()
        {

            //loop through all available ips and add them to the gui
            for (int i = 0; i < ProgramSettings.validIps.Count; i++)
            {
                mainWindow.NetworkInterfaceComboBox.Items.Add(ProgramSettings.validIps[i].ToString());
            }

            mainWindow.NetworkInterfaceComboBox.SelectedIndex = ProgramSettings.selectedIpIndex;

        }

        //lock ui and set start button to indicate that program is starting!
        internal static void onStarting()
        {
            mainWindow.StartStopButton.Content = "Starting";
            mainWindow.StartStopButton.IsEnabled = false;

            setInterfaceInteraction(false);
        }

        /// <summary>
        /// Enable input to the settings of the program
        /// </summary>
        /// <param name="status"></param>
        private static void setInterfaceInteraction(bool status)
        {
            mainWindow.NetworkPortTextBox.IsEnabled = status;
            mainWindow.NetworkInterfaceComboBox.IsEnabled = status;

            mainWindow.KinectCameraComboBox.IsEnabled = status;

            mainWindow.LoggingToggleButton.IsEnabled = status;
            mainWindow.SetPathButton.IsEnabled = status;
        }

        //makes the kinect camera bitmap visible or not
        internal static void setKinectCameraCanvasVisibility(bool v)
        {
            if (v)
            {
                mainWindow.KinectCameraImage.Visibility = Visibility.Visible;
            }
            else
            {
                mainWindow.KinectCameraImage.Visibility = Visibility.Hidden;
            }

        }

        internal static void onFaliedStart(Exception e)
        {

            mainWindow.StartStopButton.IsEnabled = true;
            mainWindow.StartStopButton.Content = "Start";
        }

        internal static void onStartSuccess()
        {

            mainWindow.StartStopButton.IsEnabled = true;
            mainWindow.StartStopButton.Content = "Stop";

        }


        internal static void onStop()
        {
            mainWindow.StartStopButton.Content = "Start";
            mainWindow.StartStopButton.IsEnabled = true;
            setInterfaceInteraction(true);
        }
    }
}
