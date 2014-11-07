using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConveyorBelt : MonoBehaviour {
	
	FoodFactory _factory;
	Queue<GameObject> _foodOnBelt;

	Vector3 _initialPos = new Vector3(0, 20, 0);

	void Start () {
        _factory = new FoodFactory();
		_foodOnBelt = new Queue<GameObject>();
		StartCoroutine(generateFood());
	}

	void FixedUpdate () {
		foreach (GameObject obj in _foodOnBelt)
			obj.transform.Translate(new Vector3(0, 0, 0.2f));
			//obj.transform.Translate(new Vector3(0, 0.2f, 0f));
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
}
