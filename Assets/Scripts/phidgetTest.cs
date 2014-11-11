using UnityEngine;
using System.Collections;
using Phidgets;
using Phidgets.Events;
public class phidgetTest : MonoBehaviour
{
		Spatial spatial = new Spatial ();
		public Helper _helper;

	// Use this for initialization
		void Start ()
		{
				//Declare an spatial object
				spatial = new Spatial ();

				//Hook the basic event handlers
				spatial.Attach += new AttachEventHandler (accel_Attach);
				spatial.Detach += new DetachEventHandler (accel_Detach);
				spatial.Error += new ErrorEventHandler (accel_Error);

				//hook the phidget specific event handlers
				spatial.SpatialData += new SpatialDataEventHandler (spatial_SpatialData);

				//open the acclerometer object for device connections
				spatial.open ();

				//get the program to wait for an spatial device to be attached
				Debug.Log ("Waiting for spatial to be attached....");
				spatial.waitForAttachment ();

				//Set the data rate so the events aren't crazy
				spatial.DataRate = 400; //multiple of 8
		}
	
		// Update is called once per frame
		void Update ()
		{
			
		}

		//spatial data handler - all spatial data tied together.
		void spatial_SpatialData (object sender, SpatialDataEventArgs e)
		{
				Debug.Log ("SpatialData event time:" + e.spatialData [0].Timestamp.TotalSeconds.ToString ());
//				if (e.spatialData [0].Acceleration.Length > 0)
//						Debug.Log (" Acceleration: " + e.spatialData [0].Acceleration [0] + ", " + e.spatialData [0].Acceleration [1] + ", " + e.spatialData [0].Acceleration [2]);
				Vector3 acceleration = new Vector3 ((float)e.spatialData [0].Acceleration [0], (float)e.spatialData [0].Acceleration [1], (float)e.spatialData [0].Acceleration [2]);
				
				//Helper calls increaseTemp
				Debug.Log ("Acceleration Magnitude: " + acceleration.magnitude);
				_helper.IncreaseTemperature(Mathf.Pow(acceleration.magnitude-1,2)*.1f);


				if (e.spatialData [0].AngularRate.Length > 0)
						Debug.Log (" Angular Rate: " + e.spatialData [0].AngularRate [0] + ", " + e.spatialData [0].AngularRate [1] + ", " + e.spatialData [0].AngularRate [2]);
				if (e.spatialData [0].MagneticField.Length > 0)
						Debug.Log (" Magnetic Field: " + e.spatialData [0].MagneticField [0] + ", " + e.spatialData [0].MagneticField [1] + ", " + e.spatialData [0].MagneticField [2]);
		}
	
		//Attach event handler...Display the serial number of the attached 
		//spatial to the console
		void accel_Attach (object sender, AttachEventArgs e)
		{
				Debug.Log ("Spatial {0} attached!" +
						e.Device.SerialNumber.ToString ());
		}
	
		//Detach event handler...Display the serial number of the detached spatial
		//to the console
		void accel_Detach (object sender, DetachEventArgs e)
		{
				Debug.Log ("Spatial {0} detached!" +
						e.Device.SerialNumber.ToString ());
		}
	
		//Error event handler...Display the description of the error to the console
		void accel_Error (object sender, ErrorEventArgs e)
		{
				Debug.Log (e.Description);
		}
}
