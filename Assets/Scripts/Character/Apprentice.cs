using UnityEngine;
using System.Collections;

public class Apprentice : MonoBehaviour {

    public event NoodleEventHandler OnNoodleReady;

    ClothRendererTest _noodleController;

	void Start () {
        _noodleController = GetComponentInChildren<ClothRendererTest>();
        if (_noodleController == null)
            Debug.LogError("Cannot find ClothRendererTest in children.");
        _noodleController.OnNoodleReady += finishOneBunchOfNoodle;
	}
	
	void Update () {
	    

	}

    void finishOneBunchOfNoodle() {

        if (OnNoodleReady != null)
            OnNoodleReady();
    }
}
