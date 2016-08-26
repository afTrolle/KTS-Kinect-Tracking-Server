using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KTS_Kinect_Tracking_Server.KTS;
using KTS_Kinect_Tracking_Server.KTS.Gui;
using KTS_Kinect_Tracking_Server.KTS.Utils;

namespace KTS_Kinect_Tracking_Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += onApplicationInitialization;
            this.Closing += onApplcationClosing;
        }

        /************************ events for starting and ending application ***********************************/

        // Called before window is shown (load settings)
        private void onApplicationInitialization(object sender, RoutedEventArgs e)
        {
            // guard so that inizaltation occurs only once
            if (!Main.isApplicationInitialized)
            {
                WindowHelper.mainWindow = this;
                ProgramSettings.mainWindow = this;
                Main.applicationInitialization();
            }
        }

        // Called before winodws is closed, aka closing the program
        private void onApplcationClosing(object sender, CancelEventArgs e)
        {
            Main.applcationClosing();
        }

        /************************  UI Event Handlers  ***********************************/

        /*    Networking interface UI   */
        // Called when User Select Network Interface
        private void onNetworkInterfaceChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Main.isApplicationInitialized)
            {
                string networkInterface = (string)NetworkInterfaceComboBox.SelectedItem;

                if (networkInterface != null && networkInterface != "")
                {
                    Main.networkInterfaceChanged(networkInterface);
                }
            }
        }

        // Only allows numbers as input for port 
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }


        // Called everytime the network port has changed
        private void onPortChange(object sender, TextChangedEventArgs e)
        {
            // this might be a bit foolish using to much I/O beacuse person can be typing however there isn't loads of interaction
            // however means that while your I/o operation is on going means that the ui is locked
            if (Main.isApplicationInitialized)
            {

                string port = NetworkPortTextBox.Text;
                int portNumber;
                bool parsed = Int32.TryParse(port, out portNumber);

                if (parsed)
                {
                    Main.portChanged(portNumber);
                }

            }
        }

        /*    Kinect Camera UI   */

        // Called when time to change kinect camera preview (infrared , depth or color camera)
        private void onKinectCameraChanged(object sender, SelectionChangedEventArgs e)
        {

            if (Main.isApplicationInitialized)
            {
                Main.kinectCameraChanged(e.AddedItems);
            }
        }

        // Called if we should show preview or not show the preview.
        private void onKinectPreviewToggled(object sender, RoutedEventArgs e)
        {
            if (Main.isApplicationInitialized)
            {
                if (KinectPreviewToggleButton.IsChecked == true)
                {
                    Main.onKinectPreviewEnabled(true);
                }
                else
                {
                    Main.onKinectPreviewEnabled(false);
                }
            }
        }

        /*    Logging UI   */
        // Called to set where the logs path should save
        private void onSetPathButtonClicked(object sender, RoutedEventArgs e)
        {
            Main.onPathButtonClicked();
        }

        // Called to set wether or not to save logs to file
        private void onLoggingToggleButtonClicked(object sender, RoutedEventArgs e)
        {
            if (Main.isApplicationInitialized)
            {
               
                if (LoggingToggleButton.IsChecked == true)
                {
                    Main.onLoggingEnabled(true);
                }
                else
                {
                    Main.onLoggingEnabled(false);
                }
            }
        }

        private void onStartStopClicked(object sender, RoutedEventArgs e)
        {
            Main.onStartStopClicked();
        }

    }
}
