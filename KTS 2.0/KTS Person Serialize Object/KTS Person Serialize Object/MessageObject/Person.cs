using System;
using System.Collections.Generic;
using System.Text;
using ProtoBuf;

namespace KTS_Person_Serialize_Object.MessageObject
{
    [ProtoContract]
    public class Person
    {
        [ProtoMember(1)]
        public bool IsTracked;
        [ProtoMember(2)]
        public bool IsRestricted;
        [ProtoMember(3)]
        public ulong TrackingID;

        [ProtoMember(4)]
        public TrackingConfidence HandLeftConfidence;
        [ProtoMember(5)]
        public TrackingConfidence HandRightConfidence;

        [ProtoMember(6)]
        public HandState HandLeftState;
        [ProtoMember(7)]
        public HandState HandRightState;
        [ProtoMember(8)]
        public FrameEdges ClippedEdges;

        // Index by enum JointType
        [ProtoMember(9)]
        public Joint[] Joints = new Joint[25];

    }



    // structs used for the joint parts
    [ProtoContract]
    public struct Joint
    {
        [ProtoMember(1)]
        public CameraSpacePoint Position;
        [ProtoMember(2)]
        public Vector4 Orientation;
        [ProtoMember(3)]
        public TrackingState TrackingState;

    }

    [ProtoContract]
    public struct CameraSpacePoint
    {
        [ProtoMember(1)]
        public float X;
        [ProtoMember(2)]
        public float Y;
        [ProtoMember(3)]
        public float Z;
    }

    [ProtoContract]
    public struct Vector4
    {
        [ProtoMember(1)]
        public float W;
        [ProtoMember(2)]
        public float X;
        [ProtoMember(3)]
        public float Y;
        [ProtoMember(4)]
        public float Z;
    }

    //Enumerators used for indicating tracking state and which joint is tracked
    public enum TrackingConfidence : Int32 { Low = 0, High = 1 };

    public enum TrackingState : Int32
    {
        NotTracked = 0, Inferred = 1, Tracked = 2
    }

    public enum HandState : Int32 { Unknown = 0, NotTracked = 1, Open = 2, Closed = 3, Lasso = 4 };

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
