using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace MessageData
{

    //Basic Serialization, for performance increases we could create a Custom Serialization instead for increased performance.
    //https://msdn.microsoft.com/en-us/library/4abbf6k0(v=vs.80).aspx
    [Serializable]
    public class MessageClass : ISerializable
    {
        //instruction
        public Int32 instruction;

        //not really used but can be used for further functionality.
        public string message;

        // max of  6 bodies can be tracked.
        public Int32 bodyCount;
        public bodyclass[] bodies;


        public MessageClass()
        {

        }

        protected MessageClass(SerializationInfo info, StreamingContext context)
        {
            instruction = info.GetInt32("A");
            message = info.GetString("B");
            bodyCount = info.GetInt32("C");

            bodies = new bodyclass[bodyCount];

            for (int i = 0; i < bodyCount; i++)
            {
                bodies[i] = new bodyclass();
                bodies[i].IsTracked = info.GetBoolean(i.ToString() + "A");



            }
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {

            info.AddValue("A", instruction);
            info.AddValue("B", message);
            info.AddValue("C", bodies.Length);

            for (int i = 0; i < bodies.Length; i++)
            {
                // i max is 5 so ther shouldn't be a problem.

                info.AddValue((i.ToString() + "A"), bodies[i].IsTracked);
                info.AddValue((i.ToString() + "B"), bodies[i].IsRestricted);
                info.AddValue((i.ToString() + "C"), bodies[i].TrackingID);

                info.AddValue((i.ToString() + "D"), bodies[i].HandLeftConfidence);
                info.AddValue((i.ToString() + "E"), bodies[i].HandRightConfidence);

                info.AddValue((i.ToString() + "F"), bodies[i].HandLeftState);
                info.AddValue((i.ToString() + "G"), bodies[i].HandRightState);

                info.AddValue((i.ToString() + "H"), bodies[i].ClippedEdges);

                /*
                public float X;
                public float Y;
                public float Z;
                */
                info.AddValue((i.ToString() + "a"), bodies[i].Joints[0].Position.X);
                info.AddValue((i.ToString() + "a"), bodies[i].Joints[0].Position.Y);
                info.AddValue((i.ToString() + "a"), bodies[i].Joints[0].Position.Z);
                info.AddValue((i.ToString() + "a"), bodies[i].Joints[0].Orientation.W);
                info.AddValue((i.ToString() + "a"), bodies[i].Joints[0].Orientation.X);
                info.AddValue((i.ToString() + "a"), bodies[i].Joints[0].Orientation.Y);
                info.AddValue((i.ToString() + "a"), bodies[i].Joints[0].Orientation.Z);
                info.AddValue((i.ToString() + "a"), bodies[i].Joints[0].TrackingState);

                info.AddValue((i.ToString() + "b"), bodies[i].Joints[1].Position.X);
                info.AddValue((i.ToString() + "b"), bodies[i].Joints[1].Position.Y);
                info.AddValue((i.ToString() + "b"), bodies[i].Joints[1].Position.Z);
                info.AddValue((i.ToString() + "b"), bodies[i].Joints[1].Orientation.W);
                info.AddValue((i.ToString() + "b"), bodies[i].Joints[1].Orientation.X);
                info.AddValue((i.ToString() + "b"), bodies[i].Joints[1].Orientation.Y);
                info.AddValue((i.ToString() + "b"), bodies[i].Joints[1].Orientation.Z);


                info.AddValue((i.ToString() + "c"), bodies[i].Joints[2]);

                info.AddValue((i.ToString() + "d"), bodies[i].Joints[3]);

                info.AddValue((i.ToString() + "e"), bodies[i].Joints[4]);

                info.AddValue((i.ToString() + "f"), bodies[i].Joints[5]);

                info.AddValue((i.ToString() + "g"), bodies[i].Joints[6]);

                info.AddValue((i.ToString() + "h"), bodies[i].Joints[7]);

                info.AddValue((i.ToString() + "i"), bodies[i].Joints[8]);

                info.AddValue((i.ToString() + "j"), bodies[i].Joints[9]);

                info.AddValue((i.ToString() + "k"), bodies[i].Joints[10]);

                info.AddValue((i.ToString() + "l"), bodies[i].Joints[11]);

                info.AddValue((i.ToString() + "m"), bodies[i].Joints[12]);

                info.AddValue((i.ToString() + "n"), bodies[i].Joints[13]);

                info.AddValue((i.ToString() + "o"), bodies[i].Joints[14]);

                info.AddValue((i.ToString() + "p"), bodies[i].Joints[15]);

                info.AddValue((i.ToString() + "q"), bodies[i].Joints[16]);

                info.AddValue((i.ToString() + "r"), bodies[i].Joints[17]);

                info.AddValue((i.ToString() + "s"), bodies[i].Joints[18]);

                info.AddValue((i.ToString() + "t"), bodies[i].Joints[19]);

                info.AddValue((i.ToString() + "u"), bodies[i].Joints[20]);

                info.AddValue((i.ToString() + "v"), bodies[i].Joints[21]);

                info.AddValue((i.ToString() + "w"), bodies[i].Joints[22]);

                info.AddValue((i.ToString() + "x"), bodies[i].Joints[23]);

                info.AddValue((i.ToString() + "y"), bodies[i].Joints[24]);
            }
            /*
                    public bool IsTracked;
                    public bool IsRestricted;
                    public ulong TrackingID;

                    public TrackingConfidence HandLeftConfidence;
                    public TrackingConfidence HandRightConfidence;

                    public HandState HandLeftState;
                    public HandState HandRightState;

                    public FrameEdges ClippedEdges;

                }
                */

            /*
             * 
             *     public enum JointType : Int32
    {
        SpineBase = 0,
        SpineMid = 1,
        Neck = 2,
        Head = 3,
        ShoulderLeft = 4,
        ElbowLeft = 5,
        WristLeft = 6,
        HandLeft = 7,
        ShoulderRight = 8,
        ElbowRight = 9,
        WristRight = 10,
        HandRight = 11,
        HipLeft = 12,
        KneeLeft = 13,
        AnkleLeft = 14,
        FootLeft = 15,
        HipRight = 16,
        KneeRight = 17,
        AnkleRight = 18,
        FootRight = 19,
        SpineShoulder = 20,
        HandTipLeft = 21,
        ThumbLeft = 22,
        HandTipRight = 23,
        ThumbRight = 24
    }

             * 
             * 
             * 
             * */

        }
    }
}
