using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KTS_Kinect_Tracking_Server.Utilitys
{
    /// <summary>
    /// all calls in this class must be on the ui thread! or errors will occur!
    /// </summary>
    static class GUIHandler
    {

        public static MainWindow mainWindow;


        /// <summary>
        /// takes list of ipaddresses and sets the network interface combobox with the ips. also loads the settings
        /// </summary>
        /// <param name="validIps"></param>
        public static void setAvailableNetworkIps(List<IPAddress> validIps)
        {

            //loop through all available ips and add them to the gui
            for (int i = 0; i < validIps.Count; i++)
            {

                mainWindow.NetworkInterfaceComboBox.Items.Add(validIps[i].ToString());

                //check if there is a saved ip in the interface
                if (SettingsHelper.getSelectedNetworkInterface() == validIps[i].ToString())
                {
                    mainWindow.NetworkInterfaceComboBox.SelectedIndex = i;
                }

            }

            // if no index has been found select first item  (127.0.0.1) 
            if (mainWindow.NetworkInterfaceComboBox.SelectedIndex == -1)
            {
                mainWindow.NetworkInterfaceComboBox.SelectedIndex = 0;
            }
        }


        /// Gets selected index of combobox of the avaiable network ips
        internal static int getSelectedNetworkIpIndex()
        {
            return mainWindow.NetworkInterfaceComboBox.SelectedIndex;
        }

        internal static void setTrackedNumberOfBodies(int numberofCountedBodies)
        {

            String text = "Tracked Bodies: " + numberofCountedBodies + System.Environment.NewLine + "Overal Latency: 0 ms";
            mainWindow.KinectStatsTextBox.Text = text;

        }

        internal static void setTrackedNumberOfBodies(int numberofCountedBodies, int fpscounter, Double avarageExecuutionTime)
        {
            String text = "Tracked Bodies: " + numberofCountedBodies + System.Environment.NewLine + "Latency: " + avarageExecuutionTime + " ms" +
                 System.Environment.NewLine + "Fps: " + fpscounter;
            mainWindow.KinectStatsTextBox.Text = text;
        }
    }
}
