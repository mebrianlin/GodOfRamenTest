using UnityEngine;
using System.Collections;

public class AttachNoodleScript : MonoBehaviour
{
		public bool attached = false;
		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void OnTriggerEnter (Collider col)
		{
				attached = true;
		}

		void OnTriggerExit (Collider col)
		{
				attached = false;
		}

		public void unAttach ()
		{
				attached = false;
		}

		public bool isAttached ()
		{
				return attached;
		}

}
