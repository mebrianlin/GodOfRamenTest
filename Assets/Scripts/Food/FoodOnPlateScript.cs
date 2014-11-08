using UnityEngine;
using System.Collections;
using System.Linq;

public class FoodOnPlateScript : MonoBehaviour {

    GameObject _food;
    FoodScript _foodScript;

    public FoodInfo Info
    {
        get
        {
            updateChildren();
            _foodScript = _food.GetComponent<FoodScript>();
            return _foodScript == null ? FoodInfo.None : _foodScript.Info;
        }
        set
        {
            updateChildren();
            _foodScript = _food.GetComponent<FoodScript>();
            if (_foodScript != null)
                _foodScript.Info = value;
        }
    }

    public GameObject Food
    {
        get
        {
            updateChildren();
            return _food;
        }
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

        updateChildren();
	}

	void Update () {
	
	}

    void updateChildren()
    {
        var childArray = this.transform.Cast<Transform>()
            .Select(x => x.gameObject)
            .ToArray();

        _food = null;

        foreach (var c in childArray)
        {
            if (c.tag == "Food")
                _food = c;
        }
    }
}
