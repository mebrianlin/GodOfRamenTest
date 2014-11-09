using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Helper : MonoBehaviour {

    public event NoodleEventHandler OnNoodleCooked;

    BlowFire _blowFire;

	Queue<GameObject> _rawRamen = new Queue<GameObject>();
	Queue<GameObject> _boiledRamen = new Queue<GameObject>();

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

		//ramen+1
		//destroy
//		Destroy(_rawRamen.Dequeue());

//		Vector3 boiledRamenPos = new Vector3(0,15,0);
//
//		GameObject boiledRamenObject = 
//			Instantiate(Resources.Load("Prefabs/BoiledRamen", typeof(GameObject)) as GameObject, boiledRamenPos ,  Quaternion.Euler(90, -180, 0) ) as GameObject;
//		_boiledRamen.Enqueue(boiledRamenObject);

        if (OnNoodleCooked != null)
            OnNoodleCooked();
    }

    /// <summary>
    /// Add a pack of ramen.
    /// </summary>
    /// <returns>False if there are already too much ramen.</returns>
    public bool AddNewRamen(int teamID) {
        _blowFire.AddNewRamen();

        // TODO: xiaoxin zhao
        // 1. Add a ramen object to the pot
//		Vector3 ramenPos = Vector3.zero;
//		
//		switch (teamID)
//		{
//		case 0:
//			ramenPos = new Vector3(-17,2,-0.5f);
//			break;
//		case 1:
//			ramenPos = new Vector3(17,2,-0.5f);
//			break;
//			
//		}
//		
//		GameObject ramenObject = 
//			Instantiate(Resources.Load("Prefabs/Ramen", typeof(GameObject)) as GameObject, ramenPos,  Quaternion.Euler(90, -180, 0) ) as GameObject;
//		_rawRamen.Enqueue(ramenObject);
		return true;
    }

    public void IncreaseTemperature() {
        _blowFire.IncreaseTemperature();
    }
}
