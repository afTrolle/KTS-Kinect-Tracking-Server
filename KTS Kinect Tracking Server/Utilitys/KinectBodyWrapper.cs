﻿
using System;

namespace KTS_Kinect_Tracking_Server.Utilitys
{

    [Serializable]
    public class Person
    {

        public bool IsTracked;
        public bool IsRestricted;
        public ulong TrackingID;

        public TrackingConfidence HandLeftConfidence;
        public TrackingConfidence HandRightConfidence;

        public HandState HandLeftState;
        public HandState HandRightState;

        public FrameEdges ClippedEdges;

        // improvments make hastable / dictonary to save memory
        //index by enum JointType
        public Joint[] Joints = new Joint[25];

        public Person()
        {

        }



        [Serializable]
        public enum TrackingConfidence { Low, High };

        [Serializable]
        public enum TrackingState
        {
            NotTracked = 0, Inferred = 1, Tracked = 2
        }

        [Serializable]
        public enum HandState { Unknown, NotTracked, Open, Closed, Lasso };

        [Serializable]
        public enum FrameEdges { None, Right = 1, Left = 2, Top = 4, Bottom = 8 }

        [Serializable]
        public enum JointType
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




}
