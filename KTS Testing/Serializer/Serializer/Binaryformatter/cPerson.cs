using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace Serializer.Binaryformatter.customFormatter
{
    [Serializable]
    class cPerson : ISerializable
    {


        public bool IsTracked ;
        public bool IsRestricted  ;
        public ulong TrackingID ;

        public TrackingConfidence HandLeftConfidence ;
        public TrackingConfidence HandRightConfidence ;
        public HandState HandLeftState;
        public HandState HandRightState;

        public FrameEdges ClippedEdges;

        // Index by enum JointType
        public Joint[] Joints = new Joint[25];


        //deserlization constructor 
        protected cPerson(SerializationInfo info, StreamingContext context)
        {
            IsTracked = info.GetBoolean("a");
            IsRestricted = info.GetBoolean("b");

            TrackingID = (ulong)info.GetUInt64("c");

            HandLeftConfidence = (TrackingConfidence)info.GetByte("d");
            HandRightConfidence = (TrackingConfidence)info.GetByte("e");

            HandLeftState = (HandState)info.GetByte("f");
            HandRightState = (HandState)info.GetByte("g");

            ClippedEdges = (FrameEdges)info.GetByte("h");

            Joints[0].Position.X = info.GetSingle("A1");
            Joints[0].Position.Y = info.GetSingle("A2");
            Joints[0].Position.Z = info.GetSingle("A3");

            Joints[0].Orientation.W = info.GetSingle("A4");
            Joints[0].Orientation.X = info.GetSingle("A5");
            Joints[0].Orientation.Y = info.GetSingle("A6");
            Joints[0].Orientation.Z = info.GetSingle("A7");

            Joints[0].TrackingState = (TrackingState)info.GetByte("A8");

            Joints[1].Position.X = info.GetSingle("B1");
            Joints[1].Position.Y = info.GetSingle("B2");
            Joints[1].Position.Z = info.GetSingle("B3");

            Joints[1].Orientation.W = info.GetSingle("B4");
            Joints[1].Orientation.X = info.GetSingle("B5");
            Joints[1].Orientation.Y = info.GetSingle("B6");
            Joints[1].Orientation.Z = info.GetSingle("B7");

            Joints[1].TrackingState = (TrackingState)info.GetByte("B8");

            Joints[2].Position.X = info.GetSingle("C1");
            Joints[2].Position.Y = info.GetSingle("C2");
            Joints[2].Position.Z = info.GetSingle("C3");

            Joints[2].Orientation.W = info.GetSingle("C4");
            Joints[2].Orientation.X = info.GetSingle("C5");
            Joints[2].Orientation.Y = info.GetSingle("C6");
            Joints[2].Orientation.Z = info.GetSingle("C7");

            Joints[2].TrackingState = (TrackingState)info.GetByte("C8");

            Joints[3].Position.X = info.GetSingle("D1");
            Joints[3].Position.Y = info.GetSingle("D2");
            Joints[3].Position.Z = info.GetSingle("D3");

            Joints[3].Orientation.W = info.GetSingle("D4");
            Joints[3].Orientation.X = info.GetSingle("D5");
            Joints[3].Orientation.Y = info.GetSingle("D6");
            Joints[3].Orientation.Z = info.GetSingle("D7");

            Joints[3].TrackingState = (TrackingState)info.GetByte("D8");

            Joints[4].Position.X = info.GetSingle("E1");
            Joints[4].Position.Y = info.GetSingle("E2");
            Joints[4].Position.Z = info.GetSingle("E3");

            Joints[4].Orientation.W = info.GetSingle("E4");
            Joints[4].Orientation.X = info.GetSingle("E5");
            Joints[4].Orientation.Y = info.GetSingle("E6");
            Joints[4].Orientation.Z = info.GetSingle("E7");

            Joints[4].TrackingState = (TrackingState)info.GetByte("E8");


            Joints[5].Position.X = info.GetSingle("F1");
            Joints[5].Position.Y = info.GetSingle("F2");
            Joints[5].Position.Z = info.GetSingle("F3");

            Joints[5].Orientation.W = info.GetSingle("F4");
            Joints[5].Orientation.X = info.GetSingle("F5");
            Joints[5].Orientation.Y = info.GetSingle("F6");
            Joints[5].Orientation.Z = info.GetSingle("F7");

            Joints[5].TrackingState = (TrackingState)info.GetByte("F8");

            Joints[6].Position.X = info.GetSingle("G1");
            Joints[6].Position.Y = info.GetSingle("G2");
            Joints[6].Position.Z = info.GetSingle("G3");

            Joints[6].Orientation.W = info.GetSingle("G4");
            Joints[6].Orientation.X = info.GetSingle("G5");
            Joints[6].Orientation.Y = info.GetSingle("G6");
            Joints[6].Orientation.Z = info.GetSingle("G7");

            Joints[6].TrackingState = (TrackingState)info.GetByte("G8");

            Joints[7].Position.X = info.GetSingle("H1");
            Joints[7].Position.Y = info.GetSingle("H2");
            Joints[7].Position.Z = info.GetSingle("H3");

            Joints[7].Orientation.W = info.GetSingle("H4");
            Joints[7].Orientation.X = info.GetSingle("H5");
            Joints[7].Orientation.Y = info.GetSingle("H6");
            Joints[7].Orientation.Z = info.GetSingle("H7");

            Joints[7].TrackingState = (TrackingState)info.GetByte("H8");

            Joints[8].Position.X = info.GetSingle("I1");
            Joints[8].Position.Y = info.GetSingle("I2");
            Joints[8].Position.Z = info.GetSingle("I3");

            Joints[8].Orientation.W = info.GetSingle("I4");
            Joints[8].Orientation.X = info.GetSingle("I5");
            Joints[8].Orientation.Y = info.GetSingle("I6");
            Joints[8].Orientation.Z = info.GetSingle("I7");

            Joints[8].TrackingState = (TrackingState)info.GetByte("I8");

            Joints[9].Position.X = info.GetSingle("J1");
            Joints[9].Position.Y = info.GetSingle("J2");
            Joints[9].Position.Z = info.GetSingle("J3");

            Joints[9].Orientation.W = info.GetSingle("J4");
            Joints[9].Orientation.X = info.GetSingle("J5");
            Joints[9].Orientation.Y = info.GetSingle("J6");
            Joints[9].Orientation.Z = info.GetSingle("J7");

            Joints[9].TrackingState = (TrackingState)info.GetByte("J8");

            Joints[10].Position.X = info.GetSingle("K1");
            Joints[10].Position.Y = info.GetSingle("K2");
            Joints[10].Position.Z = info.GetSingle("K3");

            Joints[10].Orientation.W = info.GetSingle("K4");
            Joints[10].Orientation.X = info.GetSingle("K5");
            Joints[10].Orientation.Y = info.GetSingle("K6");
            Joints[10].Orientation.Z = info.GetSingle("K7");

            Joints[10].TrackingState = (TrackingState)info.GetByte("K8");

            Joints[11].Position.X = info.GetSingle("L1");
            Joints[11].Position.Y = info.GetSingle("L2");
            Joints[11].Position.Z = info.GetSingle("L3");

            Joints[11].Orientation.W = info.GetSingle("L4");
            Joints[11].Orientation.X = info.GetSingle("L5");
            Joints[11].Orientation.Y = info.GetSingle("L6");
            Joints[11].Orientation.Z = info.GetSingle("L7");

            Joints[11].TrackingState = (TrackingState)info.GetByte("L8");

            Joints[12].Position.X = info.GetSingle("M1");
            Joints[12].Position.Y = info.GetSingle("M2");
            Joints[12].Position.Z = info.GetSingle("M3");

            Joints[12].Orientation.W = info.GetSingle("M4");
            Joints[12].Orientation.X = info.GetSingle("M5");
            Joints[12].Orientation.Y = info.GetSingle("M6");
            Joints[12].Orientation.Z = info.GetSingle("M7");

            Joints[12].TrackingState = (TrackingState)info.GetByte("M8");

            Joints[13].Position.X = info.GetSingle("N1");
            Joints[13].Position.Y = info.GetSingle("N2");
            Joints[13].Position.Z = info.GetSingle("N3");

            Joints[13].Orientation.W = info.GetSingle("N4");
            Joints[13].Orientation.X = info.GetSingle("N5");
            Joints[13].Orientation.Y = info.GetSingle("N6");
            Joints[13].Orientation.Z = info.GetSingle("N7");

            Joints[13].TrackingState = (TrackingState)info.GetByte("N8");

            Joints[14].Position.X = info.GetSingle("O1");
            Joints[14].Position.Y = info.GetSingle("O2");
            Joints[14].Position.Z = info.GetSingle("O3");

            Joints[14].Orientation.W = info.GetSingle("O4");
            Joints[14].Orientation.X = info.GetSingle("O5");
            Joints[14].Orientation.Y = info.GetSingle("O6");
            Joints[14].Orientation.Z = info.GetSingle("O7");

            Joints[14].TrackingState = (TrackingState)info.GetByte("O8");

            Joints[15].Position.X = info.GetSingle("P1");
            Joints[15].Position.Y = info.GetSingle("P2");
            Joints[15].Position.Z = info.GetSingle("P3");

            Joints[15].Orientation.W = info.GetSingle("P4");
            Joints[15].Orientation.X = info.GetSingle("P5");
            Joints[15].Orientation.Y = info.GetSingle("P6");
            Joints[15].Orientation.Z = info.GetSingle("P7");

            Joints[15].TrackingState = (TrackingState)info.GetByte("P8");

            Joints[16].Position.X = info.GetSingle("Q1");
            Joints[16].Position.Y = info.GetSingle("Q2");
            Joints[16].Position.Z = info.GetSingle("Q3");

            Joints[16].Orientation.W = info.GetSingle("Q4");
            Joints[16].Orientation.X = info.GetSingle("Q5");
            Joints[16].Orientation.Y = info.GetSingle("Q6");
            Joints[16].Orientation.Z = info.GetSingle("Q7");

            Joints[16].TrackingState = (TrackingState)info.GetByte("Q8");

            Joints[17].Position.X = info.GetSingle("R1");
            Joints[17].Position.Y = info.GetSingle("R2");
            Joints[17].Position.Z = info.GetSingle("R3");

            Joints[17].Orientation.W = info.GetSingle("R4");
            Joints[17].Orientation.X = info.GetSingle("R5");
            Joints[17].Orientation.Y = info.GetSingle("R6");
            Joints[17].Orientation.Z = info.GetSingle("R7");

            Joints[17].TrackingState = (TrackingState)info.GetByte("R8");

            Joints[18].Position.X = info.GetSingle("S1");
            Joints[18].Position.Y = info.GetSingle("S2");
            Joints[18].Position.Z = info.GetSingle("S3");

            Joints[18].Orientation.W = info.GetSingle("S4");
            Joints[18].Orientation.X = info.GetSingle("S5");
            Joints[18].Orientation.Y = info.GetSingle("S6");
            Joints[18].Orientation.Z = info.GetSingle("S7");

            Joints[18].TrackingState = (TrackingState)info.GetByte("S8");

            Joints[19].Position.X = info.GetSingle("T1");
            Joints[19].Position.Y = info.GetSingle("T2");
            Joints[19].Position.Z = info.GetSingle("T3");

            Joints[19].Orientation.W = info.GetSingle("T4");
            Joints[19].Orientation.X = info.GetSingle("T5");
            Joints[19].Orientation.Y = info.GetSingle("T6");
            Joints[19].Orientation.Z = info.GetSingle("T7");

            Joints[19].TrackingState = (TrackingState)info.GetByte("T8");

            Joints[20].Position.X = info.GetSingle("U1");
            Joints[20].Position.Y = info.GetSingle("U2");
            Joints[20].Position.Z = info.GetSingle("U3");

            Joints[20].Orientation.W = info.GetSingle("U4");
            Joints[20].Orientation.X = info.GetSingle("U5");
            Joints[20].Orientation.Y = info.GetSingle("U6");
            Joints[20].Orientation.Z = info.GetSingle("U7");

            Joints[20].TrackingState = (TrackingState)info.GetByte("U8");

            Joints[21].Position.X = info.GetSingle("V1");
            Joints[21].Position.Y = info.GetSingle("V2");
            Joints[21].Position.Z = info.GetSingle("V3");

            Joints[21].Orientation.W = info.GetSingle("V4");
            Joints[21].Orientation.X = info.GetSingle("V5");
            Joints[21].Orientation.Y = info.GetSingle("V6");
            Joints[21].Orientation.Z = info.GetSingle("V7");

            Joints[21].TrackingState = (TrackingState)info.GetByte("V8");

            Joints[22].Position.X = info.GetSingle("W1");
            Joints[22].Position.Y = info.GetSingle("W2");
            Joints[22].Position.Z = info.GetSingle("W3");

            Joints[22].Orientation.W = info.GetSingle("W4");
            Joints[22].Orientation.X = info.GetSingle("W5");
            Joints[22].Orientation.Y = info.GetSingle("W6");
            Joints[22].Orientation.Z = info.GetSingle("W7");

            Joints[22].TrackingState = (TrackingState)info.GetByte("W8");

            Joints[23].Position.X = info.GetSingle("X1");
            Joints[23].Position.Y = info.GetSingle("X2");
            Joints[23].Position.Z = info.GetSingle("X3");

            Joints[23].Orientation.W = info.GetSingle("X4");
            Joints[23].Orientation.X = info.GetSingle("X5");
            Joints[23].Orientation.Y = info.GetSingle("X6");
            Joints[23].Orientation.Z = info.GetSingle("X7");

            Joints[23].TrackingState = (TrackingState)info.GetByte("X8");

            Joints[24].Position.X = info.GetSingle("Y1");
            Joints[24].Position.Y = info.GetSingle("Y2");
            Joints[24].Position.Z = info.GetSingle("Y3");

            Joints[24].Orientation.W = info.GetSingle("Y4");
            Joints[24].Orientation.X = info.GetSingle("Y5");
            Joints[24].Orientation.Y = info.GetSingle("Y6");
            Joints[24].Orientation.Z = info.GetSingle("Y7");

            Joints[24].TrackingState = (TrackingState)info.GetByte("Y8");

            /*
            getJointVars("A", Joints[0], info);
            getJointVars("B", Joints[1], info);
            getJointVars("C", Joints[2], info);
            getJointVars("D", Joints[3], info);
            getJointVars("E", Joints[4], info);
            getJointVars("F", Joints[5], info);
            getJointVars("G", Joints[6], info);
            getJointVars("H", Joints[7], info);
            getJointVars("I", Joints[8], info);
            getJointVars("J", Joints[9], info);
            getJointVars("K", Joints[10], info);
            getJointVars("L", Joints[11], info);
            getJointVars("M", Joints[12], info);
            getJointVars("N", Joints[13], info);
            getJointVars("O", Joints[14], info);
            getJointVars("P", Joints[15], info);
            getJointVars("Q", Joints[16], info);
            getJointVars("R", Joints[17], info);
            getJointVars("S", Joints[18], info);
            getJointVars("T", Joints[19], info);
            getJointVars("U", Joints[20], info);
            getJointVars("V", Joints[21], info);
            getJointVars("W", Joints[22], info);
            getJointVars("X", Joints[23], info);
            getJointVars("Y", Joints[24], info);
            */

        }

        public cPerson()
        {
        }

        private void getJointVars(string v, Joint joint, SerializationInfo info)
        {
            joint.Position.X = info.GetSingle(v + "1");
            joint.Position.Y = info.GetSingle(v + "2");
            joint.Position.Z = info.GetSingle(v + "3");

            joint.Orientation.W = info.GetSingle(v + "4");
            joint.Orientation.X = info.GetSingle(v + "5");
            joint.Orientation.Y = info.GetSingle(v + "6");
            joint.Orientation.Z = info.GetSingle(v + "7");

            joint.TrackingState = (TrackingState)info.GetByte(v + "8");
        }

        //SerializationFormatter
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("a", IsTracked);
            info.AddValue("b", IsRestricted);
            info.AddValue("c", TrackingID);

            info.AddValue("d", (byte)HandLeftConfidence);
            info.AddValue("e", (byte)HandRightConfidence);

            info.AddValue("f", (byte)HandLeftState);
            info.AddValue("g", (byte)HandRightState);

            info.AddValue("h", (byte)ClippedEdges);

            setJointVars("A", Joints[0], info);
            setJointVars("B", Joints[1], info);
            setJointVars("C", Joints[2], info);
            setJointVars("D", Joints[3], info);
            setJointVars("E", Joints[4], info);
            setJointVars("F", Joints[5], info);
            setJointVars("G", Joints[6], info);
            setJointVars("H", Joints[7], info);
            setJointVars("I", Joints[8], info);
            setJointVars("J", Joints[9], info);
            setJointVars("K", Joints[10], info);
            setJointVars("L", Joints[11], info);
            setJointVars("M", Joints[12], info);
            setJointVars("N", Joints[13], info);
            setJointVars("O", Joints[14], info);
            setJointVars("P", Joints[15], info);
            setJointVars("Q", Joints[16], info);
            setJointVars("R", Joints[17], info);
            setJointVars("S", Joints[18], info);
            setJointVars("T", Joints[19], info);
            setJointVars("U", Joints[20], info);
            setJointVars("V", Joints[21], info);
            setJointVars("W", Joints[22], info);
            setJointVars("X", Joints[23], info);
            setJointVars("Y", Joints[24], info);
        }

        private void setJointVars(string s, Joint jo, SerializationInfo info)
        {
            info.AddValue(s + "1", jo.Position.X);
            info.AddValue(s + "2", jo.Position.Y);
            info.AddValue(s + "3", jo.Position.Z);

            info.AddValue(s + "4", jo.Orientation.W);
            info.AddValue(s + "5", jo.Orientation.X);
            info.AddValue(s + "6", jo.Orientation.Y);
            info.AddValue(s + "7", jo.Orientation.Z);

            info.AddValue(s + "8", (int)jo.TrackingState);
        }

    }
    // structs used for the joint parts
 
    public struct Joint
    {
        public CameraSpacePoint Position;
        public Vector4 Orientation;
        public TrackingState TrackingState;
    }

    public struct CameraSpacePoint
    {
        public float X;
        public float Y;
        public float Z;
    }
    
    public struct Vector4
    {
        public float W;
        public float X;
        public float Y;
        public float Z;
    }

    //Enumerators used for indicating tracking state and which joint is tracked

    public enum TrackingConfidence : byte { Low = 0, High = 1 };
   
    public enum TrackingState : byte
    {
        NotTracked = 0, Inferred = 1, Tracked = 2
    }
   
    public enum HandState : byte { Unknown = 0, NotTracked = 1, Open = 2, Closed = 3, Lasso = 4 };
   
    [FlagsAttribute]
    public enum FrameEdges : byte { None = 0, Right = 1, Left = 2, Top = 4, Bottom = 8 }
  
    public enum JointType : byte
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

}
