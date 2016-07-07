using UnityEngine;
using System.Collections;

public class networkUser : MonoBehaviour
{

    public string ip;
    public int port;

    private AsynchronousKTSClient KTSClient = new AsynchronousKTSClient();

    // Use this for initialization
    void Start()
    {

        //kinectConnectorHandler.setIP(ip);
        //kinectConnectorHandler.setport(port);
        //  kinectConnectorHandler.setOnDisconnected(onDisconnected);

        Debug.Log(System.Environment.Version);

        KTSClient.connect(ip,port);
    }

    void onConnected(bool status)
    {
        //if we where successful to connect to server
        if (status)
        {

        }
        else
        {

        }

    }

}
