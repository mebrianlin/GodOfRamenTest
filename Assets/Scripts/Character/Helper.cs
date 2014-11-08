using UnityEngine;
using System.Collections;

public class Helper : MonoBehaviour {

    public event NoodleEventHandler OnNoodleCooked;

    BlowFire _blowFire;

	void Start () {
        _blowFire = GetComponentInChildren<BlowFire>();
        if (_blowFire == null)
            Debug.LogError("Cannot find BlowFire script.");
        _blowFire.OnNoodleCooked += onNoodleCooked;
	}
	
	void Update () {
	
	}


    void onNoodleCooked()
    {
        // TODO: xiaoxin zhao
        // 1. remove the ramen object from the pot

        if (OnNoodleCooked != null)
            OnNoodleCooked();
    }

    /// <summary>
    /// Add a pack of ramen.
    /// </summary>
    /// <returns>False if there are already too much ramen.</returns>
    public bool AddNewRamen() {
        _blowFire.AddNewRamen();

        // TODO: xiaoxin zhao
        // 1. Add a ramen object to the pot
        return true;
    }

    public void IncreaseTemperature() {
        _blowFire.IncreaseTemperature();
    }
}
