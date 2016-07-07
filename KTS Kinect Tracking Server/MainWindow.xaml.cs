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

            //set refernce to window.
            GUIHandler.mainWindow = this;

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

                ApplicationState.state = ApplicationState.IDLE;
                ApplicationState.isApplicationInitialized = true;
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


            if (ApplicationState.isApplicationInitialized)
            {
                string netInterface = (string) NetworkInterfaceComboBox.SelectedItem;

                if (netInterface != null && netInterface != "")
                {
                    Settings.Default.NetworkInterface = netInterface;
                    Settings.Default.Save();
                }
            }
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
            // this might be a bit foolish using to much I/O beacuse person can be typing however there isn't loads of interaction
            // however means that while your I/o operation is on going means that the ui is locked
            if (ApplicationState.isApplicationInitialized)
            {
                string port = NetworkPortTextBox.Text;
                int portnum;
                bool parsed = Int32.TryParse(port, out portnum);
                if (parsed)
                {
                    Settings.Default.Port = portnum;
                    Settings.Default.Save();
                }

            }
        }

        /*    Kinect Camera UI   */

        // Called when time to change kinect camera preview (infrared , depth or color camera)
        private void onKinectCameraChanged(object sender, SelectionChangedEventArgs e)
        {

            if (ApplicationState.isApplicationInitialized)
            {

                if (KinectComboBoxItemCL.IsSelected)
                {
                    Settings.Default.KinectCamera = "Color Camera";
                }
                else if (KinectComboBoxItemIR.IsSelected)
                {
                    Settings.Default.KinectCamera = "Infrared Camera";
                }
                else if (KinectComboBoxItemDP.IsSelected)
                {
                    Settings.Default.KinectCamera = "Depth Camera";
                }

                mClass.kinectControl.updatePreviewState();

                Settings.Default.Save();
            }
        }

        // Called if we should show preview or not show the preview.
        private void onKinectPreviewToggled(object sender, RoutedEventArgs e)
        {
            if (ApplicationState.isApplicationInitialized)
            {
                if (KinectPreviewToggleButton.IsChecked == true)
                {
                    Settings.Default.isKinectCameraEnabled = true;
                }
                else
                {
                    Settings.Default.isKinectCameraEnabled = false;
                }

                mClass.kinectControl.updatePreviewState();
                Settings.Default.Save();
            }
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
            // if the application  start apllication or stop application
            if (ApplicationState.state == ApplicationState.RUNNING)
            {

                try
                {
                    // TODO call stop functions
                    await mClass.onApplicationStopAsync();
                    StartStopButton.Content = "Start";
                    ApplicationState.state = ApplicationState.IDLE;
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
                catch (Exception ex)
                {
                    // TODO prompt error message 

                    if (ex.Message != "")
                    {
                        showWarning(ex.Message, "Warning");
                    }

                    ApplicationState.state = ApplicationState.IDLE;
                    setUIaccessState(true);
                    StartStopButton.Content = "start ";
                }

                return;
            }
        }


        /************************  More stuff here  ***********************************/

        //shows message box 
        private void showWarning(string message, string title)
        {
            MessageBoxResult result = MessageBox.Show(this, message, title, MessageBoxButton.OK, MessageBoxImage.Warning);

        }
        private void setUiFromApplicationSettings()
        {

            NetworkPortTextBox.Text = Settings.Default.Port.ToString();

            KinectPreviewToggleButton.IsChecked = Settings.Default.isKinectCameraEnabled;

            LogsPathTextBlock.Text = Settings.Default.LogDirectorty;
            LoggingToggleButton.IsChecked = Settings.Default.isLogingEnabled;

            string status = Settings.Default.KinectCamera;

            Console.WriteLine(status);

            // better implmentaion could be made
            if (status.Equals("Color Camera"))
            {
                KinectComboBoxItemCL.IsSelected = true;
            }
            else if (status.Equals("Infrared Camera"))
            {
                KinectComboBoxItemIR.IsSelected = true;
            }
            else if (status.Equals("Depth Camera"))
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
