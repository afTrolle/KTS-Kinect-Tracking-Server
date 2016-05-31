using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using KTS_Kinect_Tracking_Server.Utilitys;
using KTS_Kinect_Tracking_Server.Properties;


namespace KTS_Kinect_Tracking_Server.Network
{
    class NetworkController
    {
        MainClass mClass;

        // list over available ips
        public List<IPAddress> validIps;

        internal void init(MainClass mClass)
        {

            this.mClass = mClass;

            validIps = getValidNetworkInterfaces();

            if (!(validIps.Count > 0))
            {
                throw new NetworkControllerInitException("No Ipv4 Network interface found");
            }

            //set
            setNetworkInterfaceOptions();

        }

        internal async Task StartServerAsync()
        {
            //  throw new NotImplementedException();

            return;
        }

        internal void StopTracking()
        {
            // throw new NotImplementedException();

        }

        internal void ApplicationClosing()
        {
            //    throw new NotImplementedException();

        }

        /*     Helper functions          */

        //finds valid ipv4 network.
        private List<IPAddress> getValidNetworkInterfaces()
        {
            List<IPAddress> ValidIps = new List<IPAddress>();

            ValidIps.Add(IPAddress.Parse("127.0.0.1"));

            try
            {
                // Establish the local endpoint for the socket.
                IPHostEntry localHost = Dns.GetHostEntry(Dns.GetHostName());

                //find first ipv4 address 
                for (int i = 0; i < localHost.AddressList.Length; i++)
                {
                    if (localHost.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        ValidIps.Add(localHost.AddressList[i]);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return ValidIps;
        }

        private void setNetworkInterfaceOptions()
        {
            for (int i = 0; i < validIps.Count; i++)
            {
                mClass.mainWindow.NetworkInterfaceComboBox.Items.Add(validIps[i].ToString());
                if (Settings.Default.NetworkInterface == validIps[i].ToString())
                {
                    mClass.mainWindow.NetworkInterfaceComboBox.SelectedIndex = i;
                }

            }

            if (mClass.mainWindow.NetworkInterfaceComboBox.SelectedIndex == -1)
            {
                mClass.mainWindow.NetworkInterfaceComboBox.SelectedIndex = 0;
            }
        }

    }
}
