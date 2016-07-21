using UnityEngine;
using System.Collections;
using MessageData;

public class networkUser : MonoBehaviour
{

    public string ip;
    public int port;

    public OVRCameraRig player;
    private Vector3 TrackingOrginalPos;
    private Vector3 playerOrginalPos;

    private AsynchronousKTSClient KTSClient = new AsynchronousKTSClient();

    private PlayerPositionHolder posHolder = new PlayerPositionHolder();

    // Use this for initialization
    void Start()
    {

        // kinectConnectorHandler.setIP(ip);
        // kinectConnectorHandler.setport(port);
        //  kinectConnectorHandler.setOnDisconnected(onDisconnected);

        Debug.Log(System.Environment.Version);

        KTSClient.connect(ip, port, onBodyUpdate);

        playerOrginalPos = player.transform.position;
    }

    void Update()
    {

        Vector3 newpos = posHolder.getPosition();
        if (newpos.x != 0f && newpos.y != 0f && newpos.z != 0f)
        {
            player.transform.transform.position = playerOrginalPos + newpos - TrackingOrginalPos;
        }
    }

    void onConnected(bool status)
    {
        //if we where successful to connect to server

    }

    ulong activeTrackingID = 0;

    void onBodyUpdate(bodyclass[] bodies)
    {

        bool postUpdated = false;

        foreach (bodyclass body in bodies)
        {

            if (body.IsTracked)
            {
                //Debug.Log("Tracked");
                //haven't found a person to track yet
                if (activeTrackingID == 0)
                {
                    postUpdated = true;
                    activeTrackingID = body.TrackingID;

                    //take inintal player position 
                    //

                    if (bodyclass.TrackingState.Tracked == body.Joints[(int)bodyclass.JointType.Neck].TrackingState)
                    {
                        posHolder.setPoisition(body.Joints[(int)bodyclass.JointType.Neck].Position.X, body.Joints[(int)bodyclass.JointType.Neck].Position.Y,-body.Joints[(int)bodyclass.JointType.Neck].Position.Z);
                        TrackingOrginalPos = posHolder.getPosition();
                    }
                }
                else if (activeTrackingID == body.TrackingID)
                {
                    postUpdated = true;
                    //person is selected and tracked!

                    if (bodyclass.TrackingState.Tracked == body.Joints[(int)bodyclass.JointType.Neck].TrackingState)
                    {
                        posHolder.setPoisition(-body.Joints[(int)bodyclass.JointType.Neck].Position.X, body.Joints[(int)bodyclass.JointType.Neck].Position.Y, -body.Joints[(int)bodyclass.JointType.Neck].Position.Z);
                    }

                }
            }

        }

        //removes trackin id!
        if (!postUpdated)
        {
            activeTrackingID = 0;
        }


    }


    public class PlayerPositionHolder
    {
        UnityEngine.Object PositionLock = new UnityEngine.Object();

        float refX = 0;
        float refY = 0;
        float refZ = 0;

        float x = 0;
        float y = 0;
        float z = 0;

        public void setPoisition(float x, float y, float z)
        {
            lock (PositionLock)
            {
                if (refX == 0 && refY == 0 && refZ == 0)
                {
                    refX = x;
                    refY = y;
                    refZ = z;
                }

                this.x = x;
                this.y = y;
                this.z = z;

            }
        }

        public Vector3 getPosition()
        {
            lock (PositionLock)
            {
                return new Vector3(x - refX, y - refY, -(z - refZ));
            }
        }

    }

}
