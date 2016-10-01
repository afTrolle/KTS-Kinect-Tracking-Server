using UnityEngine;
using System.Collections;
using KTS_Person_Serialize_Object.MessageObject;
using System;
using Assets.Scripts.KTS_Library;

public class ExampleScript : MonoBehaviour
{

    /*
     * TODO Bug testa lämna kameran och kommer tillbaka! något blir fel!
     * Plocka ut tracking id ur koden!
     * note blir fel droppar inte personen där den lämnar tracking fältet.
     * 
     * Lägg till rotation till händerna! proably tracks bottom of the hands also!
     * 
     */


    KTSLibrary KTSLib = new KTSLibrary();

    PlayerPositionHolder playerPos = new PlayerPositionHolder();

    public Vector3 test;

    public string ip;
    public int port;

    public OVRCameraRig player;
    public Transform RightHand;



    // Use this for initialization
    void Start()
    {

        KTSLib.connect(ip, port, onBodyCallback);

        //playerPos.setTrackingOffset(player.transform.position);
    }

    /// <summary>
    /// This callback is called on Not the UI THREAD! please read on multithreaded applications
    /// </summary>
    /// <param name="body"></param>
    private void onBodyCallback(Person[] body)
    {

        // loop through all the bodies
        for (int i = 0; i < body.Length; i++)
        {

            if (playerPos.getActivePlayerID() == 0 && body[i].IsTracked == true)
            {
                playerPos.setTrackingUpdate(body[i].TrackingID, body[i]);
                return;
            }
            else if (body[i].TrackingID == playerPos.getActivePlayerID() && body[i].IsTracked == true)
            {
                playerPos.setTrackingUpdate(body[i]);
                return;
            }
        }
    }



    // Update is called once per frame
    void Update()
    {

        playerPos.updateTime();

        Vector3 temp = playerPos.getCalibratedPosition(JointType.Neck);
        if (temp.x != 0 && temp.y != 0 && temp.z != 0)
            player.transform.transform.position = temp;

        temp = playerPos.getCalibratedPosition(JointType.HandRight);
        if (temp.x != 0 && temp.y != 0 && temp.z != 0)
            RightHand.transform.position = temp;

        Quaternion test =  playerPos.getOrientation(JointType.HandRight);
        //if (temp.x != 0 && temp.y != 0 && temp.z != 0)
        RightHand.transform.localRotation = test;
    }

}



public class PlayerPositionHolder
{

    // Program currently only tracks one person for  3 seconds the pics a new target!
    private float timeLeft = 2.0f;

    private ulong PersonTrackingID = 0;
    private UnityEngine.Object _lock = new UnityEngine.Object();

    TrackingUtils jointHolder = new TrackingUtils();

    internal void setTrackingOffset(Vector3 position)
    {
        jointHolder.Offset = position;
    }

    internal ulong getActivePlayerID()
    {
        lock (_lock)
        {
            return PersonTrackingID;
        }
    }



    internal void setTrackingUpdate(ulong trackingID, Person person)
    {
        lock (_lock)
        {
            PersonTrackingID = trackingID;
        }
    }

    internal void setTrackingUpdate(Person person)
    {

        lock (_lock)
        {
            //cheating a bit here  TrackingState.Tracked == 2 and TrackingState.Inferd == 1 while TrackingState.NotTracked = 0
            if (person.Joints[(int)JointType.Neck].TrackingState > 0)
                jointHolder.updateTrackingPoint(person.Joints[(int)JointType.Neck], JointType.Neck);
            if (person.Joints[(int)JointType.HandRight].TrackingState > 0)
                jointHolder.updateTrackingPoint(person.Joints[(int)JointType.HandRight], JointType.HandRight);
            if (person.Joints[(int)JointType.HandLeft].TrackingState > 0)
                jointHolder.updateTrackingPoint(person.Joints[(int)JointType.HandLeft], JointType.HandLeft);

            //reset timer!
            timeLeft = 3.0f;

        }

    }


    internal void updateTime()
    {
        lock (_lock)
        {
            timeLeft -= Time.deltaTime;

            //check if it has passed to long since last update
            if (timeLeft < 0)
            {
                PersonTrackingID = 0;
                jointHolder = new TrackingUtils();
                timeLeft = 3.0f;
            }
        }
    }

    internal Vector3 getCalibratedPosition(JointType type)
    {
        lock (_lock)
        {
            return jointHolder.getCalibratedJointPosistion(type);
        }
    }


    internal Quaternion getOrientation(JointType type)
    {
        lock (_lock)
        {
            return jointHolder.getJointOrientation(type);
        }
    }


}
