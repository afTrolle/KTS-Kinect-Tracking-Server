
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Joint[] Joints { get; } = new Joint[25];

        public Person()
        {

        }


        //these should be mapt to the same as kinect sdk 2.0
        [Serializable]
        public enum TrackingConfidence { Low, High };
        //
        // Summary:
        //     Specifies the state of tracking a body or body's attribute.
        [Serializable]
        public enum TrackingState
        {
            //
            // Summary:
            //     The joint data is not tracked and no data is known about this joint.
            NotTracked = 0,
            //
            // Summary:
            //     The joint data is inferred and confidence in the position data is lower than
            //     if it were Tracked.
            Inferred = 1,
            //
            // Summary:
            //     The joint data is being tracked and the data can be trusted.
            Tracked = 2
        }

        [Serializable]
        public enum HandState { Unknown, NotTracked, Open, Closed, Lasso };

        [Serializable]
        public enum FrameEdges { None, Right = 1, Left = 2, Top = 4, Bottom = 8 }

        //
        // Summary:
        //     The types of joints of a Body.
        [Serializable]
        public enum JointType
        {
            //
            // Summary:
            //     Base of the spine.
            SpineBase = 0,
            //
            // Summary:
            //     Middle of the spine.
            SpineMid = 1,
            //
            // Summary:
            //     Neck.
            Neck = 2,
            //
            // Summary:
            //     Head.
            Head = 3,
            //
            // Summary:
            //     Left shoulder.
            ShoulderLeft = 4,
            //
            // Summary:
            //     Left elbow.
            ElbowLeft = 5,
            //
            // Summary:
            //     Left wrist.
            WristLeft = 6,
            //
            // Summary:
            //     Left hand.
            HandLeft = 7,
            //
            // Summary:
            //     Right shoulder.
            ShoulderRight = 8,
            //
            // Summary:
            //     Right elbow.
            ElbowRight = 9,
            //
            // Summary:
            //     Right wrist.
            WristRight = 10,
            //
            // Summary:
            //     Right hand.
            HandRight = 11,
            //
            // Summary:
            //     Left hip.
            HipLeft = 12,
            //
            // Summary:
            //     Left knee.
            KneeLeft = 13,
            //
            // Summary:
            //     Left ankle.
            AnkleLeft = 14,
            //
            // Summary:
            //     Left foot.
            FootLeft = 15,
            //
            // Summary:
            //     Right hip.
            HipRight = 16,
            //
            // Summary:
            //     Right knee.
            KneeRight = 17,
            //
            // Summary:
            //     Right ankle.
            AnkleRight = 18,
            //
            // Summary:
            //     Right foot.
            FootRight = 19,
            //
            // Summary:
            //     Between the shoulders on the spine.
            SpineShoulder = 20,
            //
            // Summary:
            //     Tip of the left hand.
            HandTipLeft = 21,
            //
            // Summary:
            //     Left thumb.
            ThumbLeft = 22,
            //
            // Summary:
            //     Tip of the right hand.
            HandTipRight = 23,

            //
            // Summary:
            //     Right thumb.
            ThumbRight = 24
        }

        [Serializable]
        public struct Joint
        {
            //
            // Summary:
            //     The position of the joint in camera space.
            public CameraSpacePoint Position;

            public Vector4 Orientation;
            //
            // Summary:
            //     The tracking state of the joint.
            public TrackingState TrackingState;
        }

        [Serializable]
        public struct CameraSpacePoint
        {
            //
            // Summary:
            //     The X coordinate of the point, in meters.
            public float X;
            //
            // Summary:
            //     The Y coordinate of the point, in meters.
            public float Y;
            //
            // Summary:
            //     The Z coordinate of the point, in meters.
            public float Z;
        }

        [Serializable]
        public struct Vector4
        {
            //
            // Summary:
            //     The W coordinate of the vector.
            public float W;
            //
            // Summary:
            //     The X coordinate of the vector.
            public float X;
            //
            // Summary:
            //     The Y coordinate of the vector.
            public float Y;
            //
            // Summary:
            //     The Z coordinate of the vector.
            public float Z;
        }

    }




}
