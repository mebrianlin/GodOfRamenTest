using UnityEngine;
using System.Collections;

public class StartScreenScript : MonoBehaviour
{

		// Use this for initialization
		private bool leftButtonClicked = false;
		private bool rightButtonClicked = false;
		private GameObject leftButton;
		private GameObject leftButtonDown;
		private GameObject rightButton;
		private GameObject rightButtonDown;
		void Start ()
		{
	
				leftButton = this.transform.Find ("left").gameObject;
				leftButtonDown = this.transform.Find ("left_2").gameObject;
				rightButton = this.transform.Find ("right").gameObject;
				rightButtonDown = this.transform.Find ("right_2").gameObject;
				rightButtonDown.SetActive (false);
				leftButtonDown.SetActive (false);
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (leftButtonClicked && rightButtonClicked)
						Application.LoadLevel (Application.loadedLevel + 1);

				if (Input.GetMouseButtonDown (0)) {
						leftButtonClicked = true;
						leftButton.SetActive (false);
						leftButtonDown.SetActive (true);
				}
				if (Input.GetMouseButtonDown (1)) {
						rightButtonClicked = true;
						rightButton.SetActive (false);
						rightButtonDown.SetActive (true);
				}
		}
}
