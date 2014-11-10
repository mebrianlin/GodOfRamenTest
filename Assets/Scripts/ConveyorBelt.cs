using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConveyorBelt : MonoBehaviour {

	FoodFactory _factory;
	Queue<GameObject> _foodOnBelt;
    GameObject _activeObject;

    bool ChuanGeMode = false;
    bool 川哥 = false;

	Vector3 _initialPos = new Vector3(-50, -11, 7);
	Vector3 _conveyorSpeed = new Vector3(0.15f, 0, 0);
    float _generateFoodSpeed = 1.0f;

	void Start () {
        _factory = new FoodFactory();
		_foodOnBelt = new Queue<GameObject>();

        _generateFoodSpeed = 9.5f * Time.fixedDeltaTime / _conveyorSpeed.x;
		StartCoroutine(generateFood());
	}

	void FixedUpdate () {
        _activeObject = null;

		foreach (GameObject obj in _foodOnBelt) {
			obj.transform.position += _conveyorSpeed;

			FoodOnPlateScript foodOnPlate = obj.GetComponent<FoodOnPlateScript>();
            if (foodOnPlate != null && obj.transform.position.x > -5 && obj.transform.position.x < 5)
            {
                foodOnPlate.InFocus = true;
                _activeObject = obj;
            }
            else
                foodOnPlate.InFocus = false;

		}
        while (_foodOnBelt.Count > 0 && _foodOnBelt.Peek().transform.position.x > 50)
			Destroy(_foodOnBelt.Dequeue());

	}

	IEnumerator generateFood() {
		while (true) {
            yield return new WaitForSeconds(_generateFoodSpeed);
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

        if (script.Info.Type == FoodType.None)
            return null;

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
