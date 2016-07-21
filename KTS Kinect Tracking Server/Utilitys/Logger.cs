using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace KTS_Kinect_Tracking_Server.Utilitys
{
    static class Logger
    {

        //refernece
        private static Stopwatch watch = new Stopwatch();
        private static int counter = 0;
        static public void start()
        {
            watch.Start();
        }

        static public void stop()
        {
            watch.Stop();
            counter++;
        }

        static public void clear()
        {
            counter = 0;
            watch.Restart();
        }

        static public Double getAvarage()
        {
            return System.Math.Round(((Double)watch.ElapsedMilliseconds / counter), 2); ;
        }

    }
}
