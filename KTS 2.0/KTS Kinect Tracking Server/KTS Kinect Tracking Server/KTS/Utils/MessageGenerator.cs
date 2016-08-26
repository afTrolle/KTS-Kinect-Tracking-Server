using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KTS_Person_Serialize_Object;
using KTS_Person_Serialize_Object.MessageObject;
using Microsoft.Kinect;

namespace KTS_Kinect_Tracking_Server.KTS.Utils
{
    class MessageGenerator
    {
        internal static KTSMessage generateMessage(int instruction, string extraMessage, Body[] bodies)
        {

            KTSMessage messageToReturn = new KTSMessage();
            messageToReturn.Insturction = instruction;
            messageToReturn.Extra = extraMessage;

            int numTracked = 0;

            for (int i = 0; i < bodies.Length; i++)
            {
                if (bodies[i].IsTracked)
                    numTracked++;
            }

            if (numTracked == 0)
            {
                return null;
            }

            Person[] peoples = new Person[numTracked];


            int personIndex = 0 ; 

            for (int i = 0; i < bodies.Length; i++)
            {
                if (bodies[i].IsTracked)
                {
                    peoples[personIndex] = generatePerson(bodies[i]);
                    personIndex++;
                }
            }

            messageToReturn.People = peoples;

            return messageToReturn;
        }

        private static Person generatePerson(Body body)
        {
            Person person = new Person();
            person.TrackingID = body.TrackingId;
            person.IsTracked = body.IsTracked;
            person.IsRestricted = body.IsRestricted;


            person.HandLeftConfidence = (KTS_Person_Serialize_Object.MessageObject.TrackingConfidence) body.HandLeftConfidence;
            person.HandRightConfidence = (KTS_Person_Serialize_Object.MessageObject.TrackingConfidence) body.HandRightConfidence;

            person.HandLeftState = (KTS_Person_Serialize_Object.MessageObject.HandState) body.HandLeftState;
            person.HandRightState = (KTS_Person_Serialize_Object.MessageObject.HandState) body.HandRightState;

            person.ClippedEdges = (KTS_Person_Serialize_Object.MessageObject.FrameEdges)body.ClippedEdges;

            foreach (var item in body.Joints)
            {
                int index = (int)item.Key;
                Microsoft.Kinect.Joint bodyJoint = item.Value;

                person.Joints[index].TrackingState = (KTS_Person_Serialize_Object.MessageObject.TrackingState) bodyJoint.TrackingState;

                person.Joints[index].Position.X = bodyJoint.Position.X;
                person.Joints[index].Position.Y = bodyJoint.Position.Y;
                person.Joints[index].Position.Z = bodyJoint.Position.Z;

                Microsoft.Kinect.JointOrientation bodyJointOrientation = body.JointOrientations[item.Key];

                person.Joints[index].Orientation.W = bodyJointOrientation.Orientation.W;
                person.Joints[index].Orientation.X = bodyJointOrientation.Orientation.X;
                person.Joints[index].Orientation.Y = bodyJointOrientation.Orientation.Y;
                person.Joints[index].Orientation.Z = bodyJointOrientation.Orientation.Z;

            }

            return person;
        }
    }
}
