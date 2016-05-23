using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTS_Kinect_Tracking_Server.Utilitys
{
   static class ApplicationState
    {
        // wether server is running or not.
        public static byte state { get; set; }

       public static bool isApplicationInitialized { get; set; } = false;

        public const byte STARTING = 1;
        public const byte RUNNING = 2;
        public const byte STOPPING = 3;
        public const byte IDLE = 0;
         
    }
}
