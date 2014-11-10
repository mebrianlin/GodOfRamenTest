using UnityEngine;
using System.Collections;

public class IntroControl : MonoBehaviour {

	public MovieTexture openingMov;
	
	// Use this for initialization
	void Start () {
		renderer.material.mainTexture = openingMov;
		openingMov.loop = false;
		openingMov.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			openingMov.Stop();
			Application.LoadLevel(Application.loadedLevel+1);
		}

		if (!openingMov.isPlaying) {
			openingMov.Stop();
			Application.LoadLevel(Application.loadedLevel+1);
		}
	}
}
