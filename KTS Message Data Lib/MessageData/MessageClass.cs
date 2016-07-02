using System;

namespace MessageData
{

    //Basic Serialization, for performance increases we could create a Custom Serialization instead for increased performance.
    //https://msdn.microsoft.com/en-us/library/4abbf6k0(v=vs.80).aspx
    [Serializable]
    public class MessageClass 
    {
        //instruction
        public int ins;

        //not really used but can be used for further functionality.
        public string mes;

        // max of  6 bodies can be tracked.
        public bodyclass[] body; 
    }
}
