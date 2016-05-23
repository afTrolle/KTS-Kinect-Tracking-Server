using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTS_Kinect_Tracking_Server.Utilitys
{
    class UserException
    {
    }

    public class KinectInitException : Exception
    {
        public KinectInitException(string message)
        : base(message)
        {
        }

        public KinectInitException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }


    public class NetworkControllerInitException : Exception
    {
        public NetworkControllerInitException(string message)
        : base(message)
        {
        }

        public NetworkControllerInitException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
