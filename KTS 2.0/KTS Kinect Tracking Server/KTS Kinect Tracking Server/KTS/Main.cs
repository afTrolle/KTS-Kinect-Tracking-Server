using KTS_Kinect_Tracking_Server.KTS.Gui;
using KTS_Kinect_Tracking_Server.KTS.Kinect;
using KTS_Kinect_Tracking_Server.KTS.Network;
using KTS_Kinect_Tracking_Server.KTS.Utils;
using KTS_Person_Serialize_Object;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using System.IO;

namespace KTS_Kinect_Tracking_Server.KTS
{
    static class Main
    {

        // Program state trackers!
        public static bool isApplicationInitialized = false;
        public static ProgramState programState = ProgramState.IDLE;

        // Main logic components
        private static NetworkHandler networkHandler = new NetworkHandler();
        private static KinectHandler kinectHandler = new KinectHandler();

        /******************  Application handlers  *************************/

        internal static void applicationInitialization()
        {

            //check network interfaces
            networkHandler.initialize();
            //load saved values
            ProgramSettings.loadSettings();
            //set UI
            WindowHelper.setUI();

            kinectHandler.initialize();

            isApplicationInitialized = true;

           
        }

        internal static void applcationClosing()
        {
            //throw new NotImplementedException();
            ProgramSettings.saveSettings();
        }


        internal async static void onStartStopClicked()
        {

            if (programState == ProgramState.IDLE)
            {
                programState = ProgramState.STARTING;
                WindowHelper.onStarting();
                try
                {
                    await kinectHandler.onStarting();
                    networkHandler.onStarting();
                }
                catch (Exception e)
                {
                    WindowHelper.onFaliedStart(e);
                    programState = ProgramState.IDLE;
                    return;
                }

                WindowHelper.onStartSuccess();
                programState = ProgramState.RUNNING;

            }
            else if (programState == ProgramState.RUNNING)
            {

                WindowHelper.onStop();
                kinectHandler.onStopping();
                networkHandler.onStop();
                programState = ProgramState.IDLE;
            }
        }

        /******************  Networking handlers  *************************/

        internal static void networkInterfaceChanged(string networkInterface)
        {
            ProgramSettings.setNetworkInterface(networkInterface);
        }

        internal static void portChanged(int portNumber)
        {
            ProgramSettings.port = portNumber;
        }


        /******************  Kinect handlers  *************************/

        internal static void kinectCameraChanged(IList addedItems)
        {
            ProgramSettings.setKinectCamera(addedItems);
        }

        internal static void onKinectPreviewEnabled(bool state)
        {
            ProgramSettings.isKinectCameraPreivewEnabled = state;

            if (state)
            {
                kinectHandler.enablePreivew();
            }
            else
            {
                kinectHandler.disablePreivew();
            }

        }

        private static MemoryStream memStream = new MemoryStream();
        private static KTS_Person_Serialize_Object.MessageObject.KTSMessage message;
      
        /// <summary>
        /// Called on new body data has been recived
        /// </summary>
        internal static void onBodyTrackingUpdated(Body[] bodies)
        {

            message = MessageGenerator.generateMessage(1, "", bodies);

            if (message != null)
            {
                memStream.Position = 4;
                KTS_Person_Serialize_Object.KTSSerializer.SerializeKTSMessage(message, memStream);
                networkHandler.sendMessages(memStream);
            }
 
        }

        /******************  Logging handlers  *************************/

        internal static void onPathButtonClicked()
        {
            // TODO Handle setting path
        }

        internal static void onLoggingEnabled(bool v)
        {
            ProgramSettings.isLoggingEnabled = v;
            //TODO update state
        }

    }
}
