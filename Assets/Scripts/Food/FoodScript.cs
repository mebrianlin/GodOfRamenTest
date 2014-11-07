using UnityEngine;
using System.Collections;

public enum FoodType {
	Vegetable,
	Meat,
};

public struct FoodInfo {
	public string PrefabName { get; set; }
	public FoodType Type { get; set; }
	public float Value { get; set; }
}

public class FoodScript : MonoBehaviour {

	public FoodInfo Info {
		get;
		set;
	}

	void Start () {
	
	}

	void Update () {
	
	}
}
