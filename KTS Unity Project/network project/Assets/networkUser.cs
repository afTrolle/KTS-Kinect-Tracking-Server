using UnityEngine;
using System.Collections;

public class networkUser : MonoBehaviour {

    public string ip;
    public int port;

    private KinectConnectorhandler kinectConnectorHandler = new KinectConnectorhandler();

	// Use this for initialization
	void Start () {

        //kinectConnectorHandler.setIP(ip);
        //kinectConnectorHandler.setport(port);
      //  kinectConnectorHandler.setOnDisconnected(onDisconnected);

        Debug.Log(System.Environment.Version);

        kinectConnectorHandler.connect(ip,port);
    }

    // Update is called once per frame
    void Update () {
	
	}

    void onDisconnected()
    {

    }

    void onConnected()
    {

    }

    void onBodyDataRecieved()
    {

    }

    //TBD
    void onIdentification()
    {

    }
}
