using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KTS_Person_Serialize_Object.MessageObject;


namespace Assets.Scripts.KTS_Library
{
    class TrackingUtils
    {
     
        // possible solution
        //   https://social.msdn.microsoft.com/Forums/en-US/a5c8e59d-9131-4014-9439-4e53583aef47/kinect-v2-body-orientation-detection-in-unity3d?forum=kinectv2sdk
        internal static Vector3 _offset;
        // Offset to set the tracking by
        public Vector3 Offset
        {
            get { lock (PositionLock) { return _offset; } }
            set { lock (PositionLock) { _offset = value; } }
        }

        private UnityEngine.Object PositionLock = new UnityEngine.Object();
        private Dictionary<JointType, trackingPoint> trackingPoints = new Dictionary<JointType, trackingPoint>();


        public void updateTrackingPoint(KTS_Person_Serialize_Object.MessageObject.Joint joint, JointType type)
        {

            lock (PositionLock)
            {
                trackingPoint point = null;

                //try to get tracking point
                if (trackingPoints.TryGetValue(type, out point))
                {
                    point.updatePosition(joint);
                }
                else
                {
                    //if it fails create tracking point and update data
                    point = new trackingPoint();
                    trackingPoints.Add(type, point);
                    point.updatePosition(joint);
                }
            }
        }


        public Vector3 getCalibratedJointPosistion(JointType type)
        {
            lock (PositionLock)
            {
                trackingPoint point = null;

                //try to get tracking point
                if (trackingPoints.TryGetValue(type, out point))
                {
                    return point.getCalibratedPosition();
                }
                return new Vector3(0, 0, 0);
            }
        }



        public void setReferncePoint(KTS_Person_Serialize_Object.MessageObject.Joint joint, JointType type)
        {
            lock (PositionLock)
            {
                trackingPoint point = null;

                //try to get tracking point
                if (trackingPoints.TryGetValue(type, out point))
                {
                    point.setReferncePosition(joint);
                }
                else
                {
                    //if it fails create tracking point and update data
                    point = new trackingPoint();
                    trackingPoints.Add(type, point);
                    point.setReferncePosition(joint);
                }
            }

        }

        internal Quaternion getJointOrientation(JointType type)
        {
            lock (PositionLock)
            {
                trackingPoint point = null;

                //try to get tracking point
                if (trackingPoints.TryGetValue(type, out point))
                {
                    return point.getOrientaion();
                }

                return new Quaternion(0, 0, 0,0);
            }
        }
    }

    public class trackingPoint
    {
        //which type of joint
        private Vector3 Originposisition;
        private Vector3 calibratedPosition;

        private UnityEngine.Vector4 Orientation;


        internal Vector3 getCalibratedPosition()
        {
            return calibratedPosition;
        }

        internal void updatePosition(KTS_Person_Serialize_Object.MessageObject.Joint joint)
        {
            Orientation.w = joint.Orientation.W;
            Orientation.x = joint.Orientation.X;
            Orientation.y = joint.Orientation.Y;
            Orientation.z = joint.Orientation.Z;

            //using the calibartion position temporarly for calulcating real value
            calibratedPosition.x = joint.Position.X;
            calibratedPosition.y = joint.Position.Y;
            calibratedPosition.z = -joint.Position.Z;

            //needs to use _offset or race condition will happen
            calibratedPosition = calibratedPosition - Originposisition + TrackingUtils._offset;
        }

        internal void setReferncePosition(KTS_Person_Serialize_Object.MessageObject.Joint joint)
        {
            Originposisition.x = joint.Position.X;
            Originposisition.y = joint.Position.Y;
            Originposisition.z = -joint.Position.Z;
        }

        internal Quaternion getOrientaion()
        {
            /*
            Binormal(X) – perpendicular to bone and normal
            Bone direction(Y) - always matches skeleton
            Normal(Z) – joint roll, perpendicular to the bone
            So if you want to extract the normal, transform vector (0, 0, 1) using the quaternion.
            */

           return new Quaternion(-Orientation.x,-Orientation.y, Orientation.z, Orientation.w);
         //    return new Quaternion(0, 0, Orientation.z, Orientation.w); //roll
            // return new Quaternion(-Orientation.x, 0, 0, Orientation.w); pitch
            //  return new Quaternion(0,Orientation.y , 0, Orientation.w); //pitch
           // return new Quaternion(0, -Orientation.y, 0, 20); //pitch
            // Orientation.x Pitch
            //   Orientation.z Roll
        }
    }

}
