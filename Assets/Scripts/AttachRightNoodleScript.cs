﻿using UnityEngine;
using System.Collections;

public class AttachRightNoodleScript : MonoBehaviour
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
				if (col.tag == "RightTableTrigger")
						attached = true;
		}

		void OnTriggerExit (Collider col)
		{
				if (col.tag == "RightTableTrigger")
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