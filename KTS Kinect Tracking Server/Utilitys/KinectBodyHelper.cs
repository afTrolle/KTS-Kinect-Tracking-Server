using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KTS_Kinect_Tracking_Server.Utilitys
{
    class KinectBodyHelper
    {

        public static  void setPersonVariables( Person person ,Body body)
        {
            person.TrackingID = body.TrackingId;
            person.IsTracked = body.IsTracked;
            person.IsRestricted = body.IsRestricted;

            person.HandLeftConfidence = (KTS_Kinect_Tracking_Server.Utilitys.Person.TrackingConfidence)body.HandLeftConfidence;
            person.HandRightConfidence = (KTS_Kinect_Tracking_Server.Utilitys.Person.TrackingConfidence)body.HandRightConfidence;

            person.HandRightState = (KTS_Kinect_Tracking_Server.Utilitys.Person.HandState)body.HandRightState;
            person.HandLeftState = (KTS_Kinect_Tracking_Server.Utilitys.Person.HandState)body.HandLeftState;

            person.ClippedEdges = (KTS_Kinect_Tracking_Server.Utilitys.Person.FrameEdges)body.ClippedEdges;

            foreach (var item in body.Joints)
            {
                int index = (int)item.Key;
                Microsoft.Kinect.Joint bodyJoint = item.Value;
                Microsoft.Kinect.JointOrientation bodyJointOrientation = body.JointOrientations[item.Key];

                person.Joints[index].TrackingState = (KTS_Kinect_Tracking_Server.Utilitys.Person.TrackingState)bodyJoint.TrackingState;

                person.Joints[index].Position.X = bodyJoint.Position.X;
                person.Joints[index].Position.Y = bodyJoint.Position.Y;
                person.Joints[index].Position.Z = bodyJoint.Position.Z;

                person.Joints[index].Orientation.W = bodyJointOrientation.Orientation.W;
                person.Joints[index].Orientation.X = bodyJointOrientation.Orientation.X;
                person.Joints[index].Orientation.Y = bodyJointOrientation.Orientation.Y;
                person.Joints[index].Orientation.Z = bodyJointOrientation.Orientation.Z;

            }
        }
    }
}
