using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConveyorBelt : MonoBehaviour {

    public float GenerateFoodSpeed;


	FoodFactory _factory;
	Queue<GameObject> _foodOnBelt;
    GameObject _activeObject;

    bool ChuanGeMode = false;
    bool 川哥 = false;

	Vector3 _initialPos = new Vector3(0, 20, 0);

	void Start () {
        _factory = new FoodFactory();
		_foodOnBelt = new Queue<GameObject>();
		StartCoroutine(generateFood());
	}

	void FixedUpdate () {
        _activeObject = null;

		foreach (GameObject obj in _foodOnBelt) {
			obj.transform.Translate(new Vector3(0, 0, 0.2f));

			FoodOnPlateScript foodOnPlate = obj.GetComponent<FoodOnPlateScript>();
            if (foodOnPlate != null && obj.transform.position.y > 0 && obj.transform.position.y < 10)
            {
                foodOnPlate.InFocus = true;
                _activeObject = obj;
            }
            else
                foodOnPlate.InFocus = false;

		}
		while (_foodOnBelt.Count > 0 && _foodOnBelt.Peek().transform.position.y < -80f)
			Destroy(_foodOnBelt.Dequeue());

	}

	IEnumerator generateFood() {
		while (true) {
			yield return new WaitForSeconds(1.0f);
			//_factory.CreateFood(FoodType.Vegetable);
			GameObject food = _factory.CreateFood();
			food.transform.position = _initialPos;
			_foodOnBelt.Enqueue(food);
		}
	}

    public GameObject GrabIngredient() {
        // TODO: Brian
        if (_activeObject == null)
            return null;

        FoodOnPlateScript script = _activeObject.GetComponent<FoodOnPlateScript>();
        GameObject food = script.Food;


        if (川哥) // Chuan's suggestion
        {
            
        }
        else // Eric's suggestion
        {
            // the food has been grabbed
            if (food == null)
                return null;
            food.transform.parent = null;
        }


        return food;
    }
}
