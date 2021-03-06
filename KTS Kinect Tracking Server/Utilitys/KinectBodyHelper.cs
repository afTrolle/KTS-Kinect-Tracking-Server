﻿using Microsoft.Kinect;
using System;
using MessageData;

namespace KTS_Kinect_Tracking_Server.Utilitys
{
    class KinectBodyHelper
    {

        /*

        public static void setPersonVariables(MessageData.bodyclass person, Body body)
        {
            person.TrackingID = body.TrackingId;
            person.IsTracked = body.IsTracked;
            person.IsRestricted = body.IsRestricted;

            person.HandLeftConfidence = (MessageData.bodyclass.TrackingConfidence)body.HandLeftConfidence;
            person.HandRightConfidence = (MessageData.bodyclass.TrackingConfidence)body.HandRightConfidence;

            person.HandRightState = (MessageData.bodyclass.HandState)body.HandRightState;
            person.HandLeftState = (MessageData.bodyclass.HandState)body.HandLeftState;

            person.ClippedEdges = (MessageData.bodyclass.FrameEdges)body.ClippedEdges;

            foreach (var item in body.Joints)
            {
                int index = (int)item.Key;
                Microsoft.Kinect.Joint bodyJoint = item.Value;
                Microsoft.Kinect.JointOrientation bodyJointOrientation = body.JointOrientations[item.Key];

                person.Joints[index].TrackingState = (MessageData.bodyclass.TrackingState)bodyJoint.TrackingState;

                person.Joints[index].Position.X = bodyJoint.Position.X;
                person.Joints[index].Position.Y = bodyJoint.Position.Y;
                person.Joints[index].Position.Z = bodyJoint.Position.Z;

                person.Joints[index].Orientation.W = bodyJointOrientation.Orientation.W;
                person.Joints[index].Orientation.X = bodyJointOrientation.Orientation.X;
                person.Joints[index].Orientation.Y = bodyJointOrientation.Orientation.Y;
                person.Joints[index].Orientation.Z = bodyJointOrientation.Orientation.Z;

            }
        }
        */

        internal static void Initialization()
        {
            tempBodyHolder = new bodyclass[5];
            for (int i = 0; i < 5; i++)
            {
                tempBodyHolder[i] = new bodyclass();
            }
        }

        static bodyclass[] tempBodyHolder;

        //this is a bit naive beacuse this take a bit of computation would be better if we 
        internal static bodyclass[] getSerilazableBodyData(Body[] bodies)
        {

            for (int i = 0; i < 5; i++)
            {
                tempBodyHolder[i].TrackingID = bodies[i].TrackingId;

                if (bodies[i].IsTracked)
                {
                    tempBodyHolder[i].TrackingID = bodies[i].TrackingId;
                    tempBodyHolder[i].IsTracked = bodies[i].IsTracked;
                    tempBodyHolder[i].IsRestricted = bodies[i].IsRestricted;

                    tempBodyHolder[i].HandLeftConfidence = (bodyclass.TrackingConfidence)bodies[i].HandLeftConfidence;
                    tempBodyHolder[i].HandRightConfidence = (bodyclass.TrackingConfidence)bodies[i].HandRightConfidence;

                    tempBodyHolder[i].HandLeftState = (bodyclass.HandState)bodies[i].HandLeftState;
                    tempBodyHolder[i].HandRightState = (bodyclass.HandState)bodies[i].HandRightState;

                    tempBodyHolder[i].ClippedEdges = (bodyclass.FrameEdges)bodies[i].ClippedEdges;

                    foreach (var item in bodies[i].Joints)
                    {
                        int index = (int)item.Key;
                        Microsoft.Kinect.Joint bodyJoint = item.Value;

                        tempBodyHolder[i].Joints[index].TrackingState = (MessageData.bodyclass.TrackingState)bodyJoint.TrackingState;

                        tempBodyHolder[i].Joints[index].Position.X = bodyJoint.Position.X;
                        tempBodyHolder[i].Joints[index].Position.Y = bodyJoint.Position.Y;
                        tempBodyHolder[i].Joints[index].Position.Z = bodyJoint.Position.Z;


                        Microsoft.Kinect.JointOrientation bodyJointOrientation = bodies[i].JointOrientations[item.Key];

                        tempBodyHolder[i].Joints[index].Orientation.W = bodyJointOrientation.Orientation.W;
                        tempBodyHolder[i].Joints[index].Orientation.X = bodyJointOrientation.Orientation.X;
                        tempBodyHolder[i].Joints[index].Orientation.Y = bodyJointOrientation.Orientation.Y;
                        tempBodyHolder[i].Joints[index].Orientation.Z = bodyJointOrientation.Orientation.Z;

                    }
                } 
                
            }
            return tempBodyHolder;
        }

        //probably one or two people will be tracked. not 5 people
        //creates array only with tracked data!
        internal static bodyclass[] getSerilazableBodyDataOnlyTracked(Body[] bodies)
        {
            int BodiesTracked = 0;

            foreach (Body body in bodies)
            {
                if (body.IsTracked)
                {
                    BodiesTracked++;
                }
            }

            bodyclass[] retBodies = new bodyclass[BodiesTracked];

            int index = 0;

            for (int i = 0; bodies.Length > i; i++)
            {
                if (bodies[i].IsTracked)
                {
                    retBodies[index] = new bodyclass();
                    retBodies[index].TrackingID = bodies[i].TrackingId;
                    retBodies[index].IsTracked = bodies[i].IsTracked;
                    retBodies[index].IsRestricted = bodies[i].IsRestricted;

                    retBodies[index].HandLeftConfidence = (bodyclass.TrackingConfidence)bodies[i].HandLeftConfidence;
                    retBodies[index].HandRightConfidence = (bodyclass.TrackingConfidence)bodies[i].HandRightConfidence;

                    retBodies[index].HandLeftState = (bodyclass.HandState)bodies[i].HandLeftState;
                    retBodies[index].HandRightState = (bodyclass.HandState)bodies[i].HandRightState;

                    retBodies[index].ClippedEdges = (bodyclass.FrameEdges)bodies[i].ClippedEdges;

                    foreach (var item in bodies[i].Joints)
                    {
                        int index2 = (int)item.Key;
                        Microsoft.Kinect.Joint bodyJoint = item.Value;

                        retBodies[index].Joints[index2].TrackingState = (MessageData.bodyclass.TrackingState)bodyJoint.TrackingState;

                        retBodies[index].Joints[index2].Position.X = bodyJoint.Position.X;
                        retBodies[index].Joints[index2].Position.Y = bodyJoint.Position.Y;
                        retBodies[index].Joints[index2].Position.Z = bodyJoint.Position.Z;

                        Microsoft.Kinect.JointOrientation bodyJointOrientation = bodies[i].JointOrientations[item.Key];

                        retBodies[index].Joints[index2].Orientation.W = bodyJointOrientation.Orientation.W;
                        retBodies[index].Joints[index2].Orientation.X = bodyJointOrientation.Orientation.X;
                        retBodies[index].Joints[index2].Orientation.Y = bodyJointOrientation.Orientation.Y;
                        retBodies[index].Joints[index2].Orientation.Z = bodyJointOrientation.Orientation.Z;


                    }
                    index++;
                }

            }
            return retBodies;
        }
    }
}
