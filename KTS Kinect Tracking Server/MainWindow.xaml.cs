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

namespace KTS_Kinect_Tracking_Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //Main class ties everything together
        MainClass mClass = new MainClass();

        //
        private BackgroundWorker bw = new BackgroundWorker();


        //make user that the init functions is only run once
        private bool isApplicationInitialized = false;

      

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
            if (!isApplicationInitialized)
            {
                isApplicationInitialized = true;
                mClass.onApplicationInitialization(this);
            }


            bw.WorkerSupportsCancellation = false;
            bw.WorkerReportsProgress = false;


            bw.DoWork += Bw_DoWork;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            bw.RunWorkerAsync();

        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void onApplcationClosing(object sender, CancelEventArgs e)
        {
            mClass.ApplicationClosing();
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



        private void onStartStopClicked(object sender, RoutedEventArgs e)
        {

            Button StartStopButton = (Button)sender;
            
            // if the application ()
            if (mClass.getisApplicationRunning())
            {

                // start kinect camera and networking
                if (mClass.onApplicationStop())
                {
                    StartStopButton.Content = "Start server";
                }

            }
            else
            {

                // stop kinect camera and networking
                if (mClass.onApplicationStart())
                {
                    StartStopButton.Content = "Stop server";
                }

            }
        }


        /************************  More stuff here  ***********************************/

    }


}
