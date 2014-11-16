using UnityEngine;
using System.Collections;
using Phidgets;
using Phidgets.Events;
public class phidgetTest : MonoBehaviour
{
    public int serialNum = -1;
    public event SpatialDataEventHandler SpatialData;

    Spatial spatial;

    // Use this for initialization
    void Start()
    {
        try
        {
            //Declare an spatial object
            spatial = new Spatial();
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }

        if (spatial != null)
        {
            //Hook the basic event handlers
            spatial.Attach += new AttachEventHandler(accel_Attach);
            spatial.Detach += new DetachEventHandler(accel_Detach);
            spatial.Error += new ErrorEventHandler(accel_Error);

            //hook the phidget specific event handlers
            spatial.SpatialData += new SpatialDataEventHandler(spatial_SpatialData);

            //open the acclerometer object for device connections
            spatial.open(serialNum);

            //get the program to wait for an spatial device to be attached
            Debug.Log("Waiting for spatial to be attached....");
            spatial.waitForAttachment();

            //Set the data rate so the events aren't crazy
            spatial.DataRate = 400; //multiple of 8
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //spatial data handler - all spatial data tied together.
    void spatial_SpatialData(object sender, SpatialDataEventArgs e)
    {
        // Debug.Log("SpatialData event time:" + e.spatialData[0].Timestamp.TotalSeconds.ToString());
        // 
        // if (e.spatialData[0].AngularRate.Length > 0)
        //     Debug.Log(" Angular Rate: " + e.spatialData[0].AngularRate[0] + ", " + e.spatialData[0].AngularRate[1] + ", " + e.spatialData[0].AngularRate[2]);
        // if (e.spatialData[0].MagneticField.Length > 0)
        //     Debug.Log(" Magnetic Field: " + e.spatialData[0].MagneticField[0] + ", " + e.spatialData[0].MagneticField[1] + ", " + e.spatialData[0].MagneticField[2]);

        // callback
        if (SpatialData != null)
            SpatialData(sender, e);
    }

    //Attach event handler...Display the serial number of the attached 
    //spatial to the console
    void accel_Attach(object sender, AttachEventArgs e)
    {
        Debug.Log("Spatial {0} attached!" +
                e.Device.SerialNumber.ToString());
    }

    //Detach event handler...Display the serial number of the detached spatial
    //to the console
    void accel_Detach(object sender, DetachEventArgs e)
    {
        Debug.Log("Spatial {0} detached!" +
                e.Device.SerialNumber.ToString());
    }

    //Error event handler...Display the description of the error to the console
    void accel_Error(object sender, ErrorEventArgs e)
    {
        Debug.Log(e.Description);
    }

    void OnApplicationQuit()
    {
        if (spatial != null)
            spatial.close();
    }
}
