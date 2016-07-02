using System;

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

        public enum TrackingConfidence { Low, High };

        public enum TrackingState
        {
            NotTracked = 0, Inferred = 1, Tracked = 2
        }

        public enum HandState { Unknown, NotTracked, Open, Closed, Lasso };

        public enum FrameEdges { None, Right = 1, Left = 2, Top = 4, Bottom = 8 }

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


}
