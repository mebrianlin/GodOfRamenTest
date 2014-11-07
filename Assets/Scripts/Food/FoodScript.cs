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

    bool _focus = true;
    public bool InFocus {
        get
        {
            return _focus;
        }
        set
        {
            if (_focus != value)
            {
                _focus = value;

                foreach (var renderer in GetComponentsInChildren<Renderer>())
                {
                    Color color = renderer.material.color;
                    color.a = value ? 1.0f : 0.6f;
                    renderer.material.color = color;
                }

            }
        }
    }

	void Start () {
        this.InFocus = false;
	}

	void Update () {
	
	}
}
