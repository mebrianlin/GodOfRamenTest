using UnityEngine;
using System.Collections;

public class Apprentice : MonoBehaviour {

    public event NoodleEventHandler OnNoodleReady;

	void Start () {
	
	}
	
	void Update () {
	    

	}

    void finishOneBunchOfNoodle() {

        if (OnNoodleReady != null)
            OnNoodleReady();
    }
}
