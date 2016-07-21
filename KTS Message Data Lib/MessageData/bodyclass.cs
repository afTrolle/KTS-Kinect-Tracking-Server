using System;
using System.Runtime.CompilerServices;

namespace MessageData
{
    //this class is based on kinect sdk 2.0 completley 
    [Serializable]
    public class bodyclass
    {

        public bool IsTracked;
        public bool IsRestricted;
        public ulong TrackingID;

        public TrackingConfidence HandLeftConfidence;
        public TrackingConfidence HandRightConfidence;

        public HandState HandLeftState;
        public HandState HandRightState;

        public FrameEdges ClippedEdges;

        // Index by enum JointType
        public Joint[] Joints = new Joint[25];

        //Enumerators used for indicating tracking state and which joint is tracked
    

        // structs used for the joint parts
        [Serializable]
        public struct Joint
        {
            public CameraSpacePoint Position;
            public Vector4 Orientation;
            public TrackingState TrackingState;

        }

        [Serializable]
        public struct CameraSpacePoint
        {
            public float X;
            public float Y;
            public float Z;
        }

        [Serializable]
        public struct Vector4
        {
            public float W;
            public float X;
            public float Y;
            public float Z;
        }


    }

    public enum TrackingConfidence : Int32 { Low = 0, High = 1 };

    public enum TrackingState : Int32
    {
        NotTracked = 0, Inferred = 1, Tracked = 2
    }

    public enum HandState : Int32 { Unknown = 0, NotTracked = 1, Open = 2, Closed = 3, Lasso =4 };

    [FlagsAttribute]
    public enum FrameEdges : Int32 { None = 0, Right = 1, Left = 2, Top = 4, Bottom = 8 }

    public enum JointType : Int32
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
