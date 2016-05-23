using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using KTS_Kinect_Tracking_Server.Utilitys;
using KTS_Kinect_Tracking_Server.Properties;

namespace KTS_Kinect_Tracking_Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //Main class ties everything together
        MainClass mClass = new MainClass();

        //make user that the init functions is only run once

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
            if (!ApplicationState.isApplicationInitialized)
            {

                setUiFromApplicationSettings();

                mClass.onApplicationInitialization(this);

                ApplicationState.isApplicationInitialized = true;
                ApplicationState.state = ApplicationState.IDLE;
            }

        }

        private void onApplcationClosing(object sender, CancelEventArgs e)
        {
            mClass.onApplicationExit();
        }

        /************************  UI Event Handlers  ***********************************/

        /*    Networking interface UI   */
        // Called when User Select Network Interface
        private void onNetworkInterfaceChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        /*    Network Port UI   */

        // Only allows numbers as input for port 
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        // Called everytime the network port has changed
        private void onPortChange(object sender, TextChangedEventArgs e)
        {
        }

        /*    Kinect Camera UI   */

        // Called when time to change kinect camera preview (infrared , depth or color camera)
        private void onKinectCameraChanged(object sender, SelectionChangedEventArgs e)
        {


        }

        // Called if we should show preview or not show the preview.
        private void onKinectPreviewToggled(object sender, RoutedEventArgs e)
        {


        }

        /*    Logging UI   */

        // Called to set where the logs path should save
        private void onSetPathButtonClicked(object sender, RoutedEventArgs e)
        {

        }

        // Called to set wether or not to save logs to file
        private void onLoggingToggleButtonClicked(object sender, RoutedEventArgs e)
        {

        }


        private async void onStartStopClicked(object sender, RoutedEventArgs e)
        {

            Button StartStopButton = (Button)sender;
            // if the application ()
            if (ApplicationState.state == ApplicationState.RUNNING)
            {

                try
                {

                    // TODO call stop functions


                }
                catch (Exception)
                {

                }



                return;
            }
            else if (ApplicationState.state == ApplicationState.IDLE)
            {

                try
                {
                    StartStopButton.Content = "Starting";

                    setUIaccessState(false);

                    ApplicationState.state = ApplicationState.STARTING;
                    await mClass.onApplicationStartAsync();
                    ApplicationState.state = ApplicationState.RUNNING;
                    StartStopButton.Content = "Stop";

                }
                catch (Exception)
                {
                    // TODO prompt error message 
                    ApplicationState.state = ApplicationState.IDLE;
                    setUIaccessState(true);
                    StartStopButton.Content = "start ";
                }

                return;
            }
        }


        /************************  More stuff here  ***********************************/


        private void setUiFromApplicationSettings()
        {

            NetworkPortTextBox.Text = Settings.Default.Port.ToString();

            KinectPreviewToggleButton.IsEnabled = Settings.Default.isKinectCameraEnabled;

            LogsPathTextBlock.Text = Settings.Default.LogDirectorty ;
            LoggingToggleButton.IsEnabled = Settings.Default.isLogingEnabled;

            string status = Settings.Default.KinectCamera;

            Console.WriteLine(status);

            // better implmentaion could be made
            if (status.Equals("Color Camera"))
            {
                KinectComboBoxItemCL.IsSelected = true;
            } else if (status.Equals("Infrared Camera"))
            {
                KinectComboBoxItemIR.IsSelected = true;
            }
            else if (status.Equals( "Depth Camera"))
            {
                KinectComboBoxItemDP.IsSelected = true;
            }

        }


        // stop users from editng settings when program is running
        private void setUIaccessState(bool state)
        {
            NetworkPortTextBox.IsEnabled = state;
            NetworkInterfaceComboBox.IsEnabled = state;

            SetPathButton.IsEnabled = state;
            LoggingToggleButton.IsEnabled = state;
        }
    }

}
