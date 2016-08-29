using UnityEngine;
using System.Collections;
using KTS_Person_Serialize_Object.MessageObject;
using System;

public class ExampleScript : MonoBehaviour
{

    KTSLibrary KTSLib = new KTSLibrary();
    PlayerPositionHolder playerPos = new PlayerPositionHolder();
    PlayerPositionHolder leftHand = new PlayerPositionHolder();

    public string ip;
    public int port;

    public OVRCameraRig player;


    // Use this for initialization
    void Start()
    {

        KTSLib.connect(ip, port, onBodyCallback);
        playerPos.setPlayerOrginialPosition(player.transform.position);

    }

    private void onBodyCallback(Person[] body)
    {

        // loop through all the bodies
        for (int i = 0; i < body.Length; i++)
        {

            if (playerPos.getCurrentTrackingID() == 0 && body[i].IsTracked == true)
            {

                KTS_Person_Serialize_Object.MessageObject.Joint neckJoint = body[i].Joints[(int)JointType.Head];
                if (neckJoint.TrackingState == TrackingState.Tracked || neckJoint.TrackingState == TrackingState.Inferred)
                {
                    playerPos.setTrackingOrginalPosition(neckJoint.Position.X, neckJoint.Position.Y, neckJoint.Position.Z, body[i].TrackingID);
                    playerPos.setPosition(neckJoint.Position.X, neckJoint.Position.Y, neckJoint.Position.Z);
                }
                return;
            }
            else if (body[i].IsTracked == true && playerPos.getCurrentTrackingID() == body[i].TrackingID)
            {
                KTS_Person_Serialize_Object.MessageObject.Joint neckJoint = body[i].Joints[(int)JointType.Neck];
                if (neckJoint.TrackingState == TrackingState.Tracked)
                {
                    playerPos.setPosition(neckJoint.Position.X, neckJoint.Position.Y, neckJoint.Position.Z);
                }
                return;
            }

        }
    }



    // Update is called once per frame
    void Update()
    {
        player.transform.transform.position = playerPos.getPosition();
    }
}




public class PlayerPositionHolder
{
    UnityEngine.Object PositionLock = new UnityEngine.Object();

    //First Position reading when tracking
    private Vector3 TrackingOrginalPos = new Vector3();
    // first position of game character 
    private Vector3 playerOrginalPos;

    // latest position of tracking device
    private Vector3 latestTrackingPos = new Vector3();

    //keeps times since last update
    private float timeLeft = 2.0f;


    private ulong currentTrackingID = 0;

    public void setPlayerOrginialPosition(Vector3 playerPos)
    {
        lock (PositionLock)
        {
            playerOrginalPos = playerPos;
        }
    }

    public void setTrackingOrginalPosition(float x, float y, float z, ulong currentTrackingID)
    {
        lock (PositionLock)
        {
            timeLeft = 5.0f;
            this.currentTrackingID = currentTrackingID;
            TrackingOrginalPos.x = x;
            TrackingOrginalPos.y = y;
            TrackingOrginalPos.z = -z;
        }
    }

    public void setPosition(float x, float y, float z)
    {
        lock (PositionLock)
        {
            timeLeft = 2.0f;

            latestTrackingPos.x = x;
            latestTrackingPos.y = y;
            latestTrackingPos.z = -z;
        }
    }

    public ulong getCurrentTrackingID()
    {
        lock (PositionLock)
        {
            return currentTrackingID;
        }
    }

    public Vector3 getPosition()
    {
        lock (PositionLock)
        {

            timeLeft -= Time.deltaTime;

            if (timeLeft < 0)
            {
                timeLeft = 0;
                currentTrackingID = 0;
            }

            return (latestTrackingPos - TrackingOrginalPos + playerOrginalPos);
        }
    }


}
