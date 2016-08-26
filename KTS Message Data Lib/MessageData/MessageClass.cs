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

                string s = i.ToString();

                bodies[i].IsTracked = info.GetBoolean(s + "A");
                bodies[i].IsRestricted = info.GetBoolean(s + "B");
                bodies[i].TrackingID = info.GetUInt64(s + "C");

                bodies[i].HandLeftConfidence = (TrackingConfidence)info.GetInt32(s + "D");
                bodies[i].HandRightConfidence = (TrackingConfidence)info.GetInt32(s + "E");

                bodies[i].HandLeftState = (HandState)info.GetInt32(s + "F");
                bodies[i].HandRightState = (HandState)info.GetInt32(s + "G");

                bodies[i].ClippedEdges = (FrameEdges)info.GetInt32(s + "H");

                setJoint(bodies[i].Joints[0], (s + "a"), info);
                setJoint(bodies[i].Joints[1], (s + "b"), info);
                setJoint(bodies[i].Joints[2], (s + "c"), info);
                setJoint(bodies[i].Joints[3], (s + "d"), info);
                setJoint(bodies[i].Joints[4], (s + "e"), info);
                setJoint(bodies[i].Joints[5], (s + "f"), info);
                setJoint(bodies[i].Joints[6], (s + "g"), info);
                setJoint(bodies[i].Joints[7], (s + "h"), info);
                setJoint(bodies[i].Joints[8], (s + "i"), info);
                setJoint(bodies[i].Joints[9], (s + "j"), info);
                setJoint(bodies[i].Joints[10], (s + "k"), info);
                setJoint(bodies[i].Joints[11], (s + "l"), info);
                setJoint(bodies[i].Joints[12], (s + "m"), info);
                setJoint(bodies[i].Joints[13], (s + "n"), info);
                setJoint(bodies[i].Joints[14], (s + "o"), info);
                setJoint(bodies[i].Joints[15], (s + "p"), info);
                setJoint(bodies[i].Joints[16], (s + "q"), info);
                setJoint(bodies[i].Joints[17], (s + "r"), info);
                setJoint(bodies[i].Joints[18], (s + "s"), info);
                setJoint(bodies[i].Joints[19], (s + "t"), info);
                setJoint(bodies[i].Joints[20], (s + "u"), info);
                setJoint(bodies[i].Joints[21], (s + "v"), info);
                setJoint(bodies[i].Joints[22], (s + "w"), info);
                setJoint(bodies[i].Joints[23], (s + "x"), info);
                setJoint(bodies[i].Joints[24], (s + "y"), info);
            }
        }

        private void setJoint(bodyclass.Joint joint, string v, SerializationInfo info)
        {

            joint.Position.X = info.GetSingle(v + "1");
            joint.Position.Y = info.GetSingle(v + "2");
            joint.Position.Z = info.GetSingle(v + "3");
            joint.Orientation.W = info.GetSingle(v + "4");
            joint.Orientation.X = info.GetSingle(v + "5");
            joint.Orientation.Y = info.GetSingle(v + "6");
            joint.Orientation.Z = info.GetSingle(v + "7");
            joint.TrackingState = (TrackingState)info.GetInt32(v + "8");
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
                string s = i.ToString();

                info.AddValue((s + "A"), bodies[i].IsTracked);
                info.AddValue((s + "B"), bodies[i].IsRestricted);
                info.AddValue((s + "C"), bodies[i].TrackingID);

                info.AddValue((s + "D"), (int)bodies[i].HandLeftConfidence);
                info.AddValue((s + "E"), (int)bodies[i].HandRightConfidence);

                info.AddValue((s + "F"), (int)bodies[i].HandLeftState);
                info.AddValue((s + "G"), (int)bodies[i].HandRightState);

                info.AddValue((s + "H"), (int)bodies[i].ClippedEdges);

                /*
                public float X;
                public float Y;
                public float Z;

                and        
                public float W;
                public float X;
                public float Y;
                public float Z;

                8 * 25 * 5 =  1000 joint variables
                */

                info.AddValue((s + "a1"), bodies[i].Joints[0].Position.X);
                info.AddValue((s + "a2"), bodies[i].Joints[0].Position.Y);
                info.AddValue((s + "a3"), bodies[i].Joints[0].Position.Z);
                info.AddValue((s + "a4"), bodies[i].Joints[0].Orientation.W);
                info.AddValue((s + "a5"), bodies[i].Joints[0].Orientation.X);
                info.AddValue((s + "a6"), bodies[i].Joints[0].Orientation.Y);
                info.AddValue((s + "a7"), bodies[i].Joints[0].Orientation.Z);
                info.AddValue((s + "a8"), (int)bodies[i].Joints[0].TrackingState);

                info.AddValue((s + "b1"), bodies[i].Joints[1].Position.X);
                info.AddValue((s + "b2"), bodies[i].Joints[1].Position.Y);
                info.AddValue((s + "b3"), bodies[i].Joints[1].Position.Z);
                info.AddValue((s + "b4"), bodies[i].Joints[1].Orientation.W);
                info.AddValue((s + "b5"), bodies[i].Joints[1].Orientation.X);
                info.AddValue((s + "b6"), bodies[i].Joints[1].Orientation.Y);
                info.AddValue((s + "b7"), bodies[i].Joints[1].Orientation.Z);
                info.AddValue((s + "b8"), (int)bodies[i].Joints[1].TrackingState);

                info.AddValue((s + "c1"), bodies[i].Joints[2].Position.X);
                info.AddValue((s + "c2"), bodies[i].Joints[2].Position.Y);
                info.AddValue((s + "c3"), bodies[i].Joints[2].Position.Z);
                info.AddValue((s + "c4"), bodies[i].Joints[2].Orientation.W);
                info.AddValue((s + "c5"), bodies[i].Joints[2].Orientation.X);
                info.AddValue((s + "c6"), bodies[i].Joints[2].Orientation.Y);
                info.AddValue((s + "c7"), bodies[i].Joints[2].Orientation.Z);
                info.AddValue((s + "c8"), (int)bodies[i].Joints[2].TrackingState);

                info.AddValue((s + "d1"), bodies[i].Joints[3].Position.X);
                info.AddValue((s + "d2"), bodies[i].Joints[3].Position.Y);
                info.AddValue((s + "d3"), bodies[i].Joints[3].Position.Z);
                info.AddValue((s + "d4"), bodies[i].Joints[3].Orientation.W);
                info.AddValue((s + "d5"), bodies[i].Joints[3].Orientation.X);
                info.AddValue((s + "d6"), bodies[i].Joints[3].Orientation.Y);
                info.AddValue((s + "d7"), bodies[i].Joints[3].Orientation.Z);
                info.AddValue((s + "d8"), (int)bodies[i].Joints[3].TrackingState);

                info.AddValue((s + "e1"), bodies[i].Joints[4].Position.X);
                info.AddValue((s + "e2"), bodies[i].Joints[4].Position.Y);
                info.AddValue((s + "e3"), bodies[i].Joints[4].Position.Z);
                info.AddValue((s + "e4"), bodies[i].Joints[4].Orientation.W);
                info.AddValue((s + "e5"), bodies[i].Joints[4].Orientation.X);
                info.AddValue((s + "e6"), bodies[i].Joints[4].Orientation.Y);
                info.AddValue((s + "e7"), bodies[i].Joints[4].Orientation.Z);
                info.AddValue((s + "e8"), (int)bodies[i].Joints[4].TrackingState);

                info.AddValue((s + "f1"), bodies[i].Joints[5].Position.X);
                info.AddValue((s + "f2"), bodies[i].Joints[5].Position.Y);
                info.AddValue((s + "f3"), bodies[i].Joints[5].Position.Z);
                info.AddValue((s + "f4"), bodies[i].Joints[5].Orientation.W);
                info.AddValue((s + "f5"), bodies[i].Joints[5].Orientation.X);
                info.AddValue((s + "f6"), bodies[i].Joints[5].Orientation.Y);
                info.AddValue((s + "f7"), bodies[i].Joints[5].Orientation.Z);
                info.AddValue((s + "f8"), (int)bodies[i].Joints[5].TrackingState);

                info.AddValue((s + "g1"), bodies[i].Joints[6].Position.X);
                info.AddValue((s + "g2"), bodies[i].Joints[6].Position.Y);
                info.AddValue((s + "g3"), bodies[i].Joints[6].Position.Z);
                info.AddValue((s + "g4"), bodies[i].Joints[6].Orientation.W);
                info.AddValue((s + "g5"), bodies[i].Joints[6].Orientation.X);
                info.AddValue((s + "g6"), bodies[i].Joints[6].Orientation.Y);
                info.AddValue((s + "g7"), bodies[i].Joints[6].Orientation.Z);
                info.AddValue((s + "g8"), (int)bodies[i].Joints[6].TrackingState);

                info.AddValue((s + "h1"), bodies[i].Joints[7].Position.X);
                info.AddValue((s + "h2"), bodies[i].Joints[7].Position.Y);
                info.AddValue((s + "h3"), bodies[i].Joints[7].Position.Z);
                info.AddValue((s + "h4"), bodies[i].Joints[7].Orientation.W);
                info.AddValue((s + "h5"), bodies[i].Joints[7].Orientation.X);
                info.AddValue((s + "h6"), bodies[i].Joints[7].Orientation.Y);
                info.AddValue((s + "h7"), bodies[i].Joints[7].Orientation.Z);
                info.AddValue((s + "h8"), (int)bodies[i].Joints[7].TrackingState);

                info.AddValue((s + "i1"), bodies[i].Joints[8].Position.X);
                info.AddValue((s + "i2"), bodies[i].Joints[8].Position.Y);
                info.AddValue((s + "i3"), bodies[i].Joints[8].Position.Z);
                info.AddValue((s + "i4"), bodies[i].Joints[8].Orientation.W);
                info.AddValue((s + "i5"), bodies[i].Joints[8].Orientation.X);
                info.AddValue((s + "i6"), bodies[i].Joints[8].Orientation.Y);
                info.AddValue((s + "i7"), bodies[i].Joints[8].Orientation.Z);
                info.AddValue((s + "i8"), (int)bodies[i].Joints[8].TrackingState);

                info.AddValue((s + "j1"), bodies[i].Joints[9].Position.X);
                info.AddValue((s + "j2"), bodies[i].Joints[9].Position.Y);
                info.AddValue((s + "j3"), bodies[i].Joints[9].Position.Z);
                info.AddValue((s + "j4"), bodies[i].Joints[9].Orientation.W);
                info.AddValue((s + "j5"), bodies[i].Joints[9].Orientation.X);
                info.AddValue((s + "j6"), bodies[i].Joints[9].Orientation.Y);
                info.AddValue((s + "j7"), bodies[i].Joints[9].Orientation.Z);
                info.AddValue((s + "j8"), (int)bodies[i].Joints[9].TrackingState);

                info.AddValue((s + "k1"), bodies[i].Joints[10].Position.X);
                info.AddValue((s + "k2"), bodies[i].Joints[10].Position.Y);
                info.AddValue((s + "k3"), bodies[i].Joints[10].Position.Z);
                info.AddValue((s + "k4"), bodies[i].Joints[10].Orientation.W);
                info.AddValue((s + "k5"), bodies[i].Joints[10].Orientation.X);
                info.AddValue((s + "k6"), bodies[i].Joints[10].Orientation.Y);
                info.AddValue((s + "k7"), bodies[i].Joints[10].Orientation.Z);
                info.AddValue((s + "k8"), (int)bodies[i].Joints[10].TrackingState);

                info.AddValue((s + "l1"), bodies[i].Joints[11].Position.X);
                info.AddValue((s + "l2"), bodies[i].Joints[11].Position.Y);
                info.AddValue((s + "l3"), bodies[i].Joints[11].Position.Z);
                info.AddValue((s + "l4"), bodies[i].Joints[11].Orientation.W);
                info.AddValue((s + "l5"), bodies[i].Joints[11].Orientation.X);
                info.AddValue((s + "l6"), bodies[i].Joints[11].Orientation.Y);
                info.AddValue((s + "l7"), bodies[i].Joints[11].Orientation.Z);
                info.AddValue((s + "l8"), (int)bodies[i].Joints[11].TrackingState);

                info.AddValue((s + "m1"), bodies[i].Joints[12].Position.X);
                info.AddValue((s + "m2"), bodies[i].Joints[12].Position.Y);
                info.AddValue((s + "m3"), bodies[i].Joints[12].Position.Z);
                info.AddValue((s + "m4"), bodies[i].Joints[12].Orientation.W);
                info.AddValue((s + "m5"), bodies[i].Joints[12].Orientation.X);
                info.AddValue((s + "m6"), bodies[i].Joints[12].Orientation.Y);
                info.AddValue((s + "m7"), bodies[i].Joints[12].Orientation.Z);
                info.AddValue((s + "m8"), (int)bodies[i].Joints[12].TrackingState);

                info.AddValue((s + "n1"), bodies[i].Joints[13].Position.X);
                info.AddValue((s + "n2"), bodies[i].Joints[13].Position.Y);
                info.AddValue((s + "n3"), bodies[i].Joints[13].Position.Z);
                info.AddValue((s + "n4"), bodies[i].Joints[13].Orientation.W);
                info.AddValue((s + "n5"), bodies[i].Joints[13].Orientation.X);
                info.AddValue((s + "n6"), bodies[i].Joints[13].Orientation.Y);
                info.AddValue((s + "n7"), bodies[i].Joints[13].Orientation.Z);
                info.AddValue((s + "n8"), (int)bodies[i].Joints[13].TrackingState);

                info.AddValue((s + "o1"), bodies[i].Joints[14].Position.X);
                info.AddValue((s + "o2"), bodies[i].Joints[14].Position.Y);
                info.AddValue((s + "o3"), bodies[i].Joints[14].Position.Z);
                info.AddValue((s + "o4"), bodies[i].Joints[14].Orientation.W);
                info.AddValue((s + "o5"), bodies[i].Joints[14].Orientation.X);
                info.AddValue((s + "o6"), bodies[i].Joints[14].Orientation.Y);
                info.AddValue((s + "o7"), bodies[i].Joints[14].Orientation.Z);
                info.AddValue((s + "o8"), (int)bodies[i].Joints[14].TrackingState);

                info.AddValue((s + "p1"), bodies[i].Joints[15].Position.X);
                info.AddValue((s + "p2"), bodies[i].Joints[15].Position.Y);
                info.AddValue((s + "p3"), bodies[i].Joints[15].Position.Z);
                info.AddValue((s + "p4"), bodies[i].Joints[15].Orientation.W);
                info.AddValue((s + "p5"), bodies[i].Joints[15].Orientation.X);
                info.AddValue((s + "p6"), bodies[i].Joints[15].Orientation.Y);
                info.AddValue((s + "p7"), bodies[i].Joints[15].Orientation.Z);
                info.AddValue((s + "p8"), (int)bodies[i].Joints[15].TrackingState);

                info.AddValue((s + "q1"), bodies[i].Joints[16].Position.X);
                info.AddValue((s + "q2"), bodies[i].Joints[16].Position.Y);
                info.AddValue((s + "q3"), bodies[i].Joints[16].Position.Z);
                info.AddValue((s + "q4"), bodies[i].Joints[16].Orientation.W);
                info.AddValue((s + "q5"), bodies[i].Joints[16].Orientation.X);
                info.AddValue((s + "q6"), bodies[i].Joints[16].Orientation.Y);
                info.AddValue((s + "q7"), bodies[i].Joints[16].Orientation.Z);
                info.AddValue((s + "q8"), (int)bodies[i].Joints[16].TrackingState);

                info.AddValue((s + "r1"), bodies[i].Joints[17].Position.X);
                info.AddValue((s + "r2"), bodies[i].Joints[17].Position.Y);
                info.AddValue((s + "r3"), bodies[i].Joints[17].Position.Z);
                info.AddValue((s + "r4"), bodies[i].Joints[17].Orientation.W);
                info.AddValue((s + "r5"), bodies[i].Joints[17].Orientation.X);
                info.AddValue((s + "r6"), bodies[i].Joints[17].Orientation.Y);
                info.AddValue((s + "r7"), bodies[i].Joints[17].Orientation.Z);
                info.AddValue((s + "r8"), (int)bodies[i].Joints[17].TrackingState);

                info.AddValue((s + "s1"), bodies[i].Joints[18].Position.X);
                info.AddValue((s + "s2"), bodies[i].Joints[18].Position.Y);
                info.AddValue((s + "s3"), bodies[i].Joints[18].Position.Z);
                info.AddValue((s + "s4"), bodies[i].Joints[18].Orientation.W);
                info.AddValue((s + "s5"), bodies[i].Joints[18].Orientation.X);
                info.AddValue((s + "s6"), bodies[i].Joints[18].Orientation.Y);
                info.AddValue((s + "s7"), bodies[i].Joints[18].Orientation.Z);
                info.AddValue((s + "s8"), (int)bodies[i].Joints[18].TrackingState);

                info.AddValue((s + "t1"), bodies[i].Joints[19].Position.X);
                info.AddValue((s + "t2"), bodies[i].Joints[19].Position.Y);
                info.AddValue((s + "t3"), bodies[i].Joints[19].Position.Z);
                info.AddValue((s + "t4"), bodies[i].Joints[19].Orientation.W);
                info.AddValue((s + "t5"), bodies[i].Joints[19].Orientation.X);
                info.AddValue((s + "t6"), bodies[i].Joints[19].Orientation.Y);
                info.AddValue((s + "t7"), bodies[i].Joints[19].Orientation.Z);
                info.AddValue((s + "t8"), (int)bodies[i].Joints[19].TrackingState);

                info.AddValue((s + "u1"), bodies[i].Joints[20].Position.X);
                info.AddValue((s + "u2"), bodies[i].Joints[20].Position.Y);
                info.AddValue((s + "u3"), bodies[i].Joints[20].Position.Z);
                info.AddValue((s + "u4"), bodies[i].Joints[20].Orientation.W);
                info.AddValue((s + "u5"), bodies[i].Joints[20].Orientation.X);
                info.AddValue((s + "u6"), bodies[i].Joints[20].Orientation.Y);
                info.AddValue((s + "u7"), bodies[i].Joints[20].Orientation.Z);
                info.AddValue((s + "u8"), (int)bodies[i].Joints[20].TrackingState);

                info.AddValue((s + "v1"), bodies[i].Joints[21].Position.X);
                info.AddValue((s + "v2"), bodies[i].Joints[21].Position.Y);
                info.AddValue((s + "v3"), bodies[i].Joints[21].Position.Z);
                info.AddValue((s + "v4"), bodies[i].Joints[21].Orientation.W);
                info.AddValue((s + "v5"), bodies[i].Joints[21].Orientation.X);
                info.AddValue((s + "v6"), bodies[i].Joints[21].Orientation.Y);
                info.AddValue((s + "v7"), bodies[i].Joints[21].Orientation.Z);
                info.AddValue((s + "v8"), (int)bodies[i].Joints[21].TrackingState);

                info.AddValue((s + "w1"), bodies[i].Joints[22].Position.X);
                info.AddValue((s + "w2"), bodies[i].Joints[22].Position.Y);
                info.AddValue((s + "w3"), bodies[i].Joints[22].Position.Z);
                info.AddValue((s + "w4"), bodies[i].Joints[22].Orientation.W);
                info.AddValue((s + "w5"), bodies[i].Joints[22].Orientation.X);
                info.AddValue((s + "w6"), bodies[i].Joints[22].Orientation.Y);
                info.AddValue((s + "w7"), bodies[i].Joints[22].Orientation.Z);
                info.AddValue((s + "w8"), (int)bodies[i].Joints[22].TrackingState);

                info.AddValue((s + "x1"), bodies[i].Joints[23].Position.X);
                info.AddValue((s + "x2"), bodies[i].Joints[23].Position.Y);
                info.AddValue((s + "x3"), bodies[i].Joints[23].Position.Z);
                info.AddValue((s + "x4"), bodies[i].Joints[23].Orientation.W);
                info.AddValue((s + "x5"), bodies[i].Joints[23].Orientation.X);
                info.AddValue((s + "x6"), bodies[i].Joints[23].Orientation.Y);
                info.AddValue((s + "x7"), bodies[i].Joints[23].Orientation.Z);
                info.AddValue((s + "x8"), (int)bodies[i].Joints[23].TrackingState);

                info.AddValue((s + "y1"), bodies[i].Joints[24].Position.X);
                info.AddValue((s + "y2"), bodies[i].Joints[24].Position.Y);
                info.AddValue((s + "y3"), bodies[i].Joints[24].Position.Z);
                info.AddValue((s + "y4"), bodies[i].Joints[24].Orientation.W);
                info.AddValue((s + "y5"), bodies[i].Joints[24].Orientation.X);
                info.AddValue((s + "y6"), bodies[i].Joints[24].Orientation.Y);
                info.AddValue((s + "y7"), bodies[i].Joints[24].Orientation.Z);
                info.AddValue((s + "y8"), (int)bodies[i].Joints[24].TrackingState);

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
