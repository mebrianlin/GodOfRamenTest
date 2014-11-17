using UnityEngine;
using System.Collections;

public class StartScreenScript : MonoBehaviour
{

		// Use this for initialization
		private bool leftButtonClicked = false;
		private bool rightButtonClicked = false;

		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (leftButtonClicked && rightButtonClicked)
						Application.LoadLevel (Application.loadedLevel + 1);

				if (Input.GetMouseButtonDown (0))
						leftButtonClicked = true;

				if (Input.GetMouseButtonDown (1))
						rightButtonClicked = true;
		}
}
