using System;
using System.Collections.Generic;
using System.Text;

#pragma warning disable CS0649
namespace Serializer.Binaryformatter
{
    [Serializable]
    class bPerson
    {

        public bool IsTracked = false;
        public bool IsRestricted = false;
        public ulong TrackingID = 0;

        public TrackingConfidence HandLeftConfidence = TrackingConfidence.Low;
        public TrackingConfidence HandRightConfidence = TrackingConfidence.Low;
        public HandState HandLeftState;
        public HandState HandRightState;

        public FrameEdges ClippedEdges;

        // Index by enum JointType
        public Joint[] Joints = new Joint[25];

        public bPerson()
        {
            for (int i = 0; i < Joints.Length; i++)
            {
                Joints[i] = new Joint();
            }
        }
    }

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

    //Enumerators used for indicating tracking state and which joint is tracked
    [Serializable]
    public enum TrackingConfidence : byte { Low = 0, High = 1 };
    [Serializable]
    public enum TrackingState : byte
    {
        NotTracked = 0, Inferred = 1, Tracked = 2
    }
    [Serializable]
    public enum HandState : byte { Unknown = 0, NotTracked = 1, Open = 2, Closed = 3, Lasso = 4 };
    [Serializable]
    [FlagsAttribute]
    public enum FrameEdges : byte { None = 0, Right = 1, Left = 2, Top = 4, Bottom = 8 }
    [Serializable]
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
