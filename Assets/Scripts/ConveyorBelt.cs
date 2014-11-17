using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConveyorBelt : MonoBehaviour {

    public Transform LeftMask;
    public Transform RightMask;

	FoodFactory _factory;
	Queue<GameObject> _foodOnBelt;
    GameObject _activeObject;

    Vector3 _leftMaskPos;
    Vector3 _rightMaskPos;
    float _foodSize = 1f;

    bool ChuanGeMode = false;
    bool 川哥 = false;

    Vector3[] _speeds = {
        new Vector3(0.3f, 0, 0),
        new Vector3(0.4f, 0, 0),
        new Vector3(0.5f, 0, 0),
    };

    public float Speed
    {
        get { return _speed.x; }
    }

    /*
    public float NextGenerateTime
    {
        get { return _foodSize / Speed * Time.fixedDeltaTime; }
    }
    */

    Vector3 _speed = new Vector3(0.3f, 0, 0);
    Vector3 _targetSpeed = new Vector3(0.3f, 0, 0);

	void Start () {
        _factory = new FoodFactory();
		_foodOnBelt = new Queue<GameObject>();

        this._speed = _speeds[0];
        this._targetSpeed = this._speed;

        _leftMaskPos = LeftMask.renderer.bounds.center - new Vector3(LeftMask.renderer.bounds.extents.x, 0, -0.1f);
        _rightMaskPos = RightMask.renderer.bounds.center + new Vector3(RightMask.renderer.bounds.extents.x, 0, -0.1f);

        Reset();
        /*
        // manually create empty belt, so the belt is initially populated
        // populate it from right to left
        GameObject food = _factory.CreateFood(Food.None);
        _foodSize = food.renderer.bounds.extents.x * 2;

        int numOfEmptyBelt = (int)Mathf.Ceil((_rightMaskPos.x - _leftMaskPos.x) / _foodSize);
        Vector3 pos = _leftMaskPos + new Vector3(_foodSize * numOfEmptyBelt, 0, 0);

        food.transform.position = pos;
        _foodOnBelt.Enqueue(food);
        food.transform.parent = this.transform;
        
        for (; pos.x >= _leftMaskPos.x; ) {
            food = _factory.CreateFood(Food.None);
            food.transform.position = pos;
            _foodOnBelt.Enqueue(food);
            food.transform.parent = this.transform;
            pos -= new Vector3(_foodSize, 0, 0);
        }*/
	}

	void FixedUpdate () {
        _speed = _targetSpeed;

        _activeObject = null;

		foreach (GameObject obj in _foodOnBelt) {
			obj.transform.position += _speed;

			FoodOnPlateScript foodOnPlate = obj.GetComponent<FoodOnPlateScript>();
            if (foodOnPlate != null && obj.transform.position.x > transform.position.x - 5 && obj.transform.position.x < transform.position.x + 5)
            {
                foodOnPlate.InFocus = true;
                _activeObject = obj;
            }
            else
                foodOnPlate.InFocus = false;

		}
        while (_foodOnBelt.Count > 0 && _foodOnBelt.Peek().transform.position.x > _rightMaskPos.x + _foodSize)
			Destroy(_foodOnBelt.Dequeue());

	}

    public void Reset()
    {
        while (_foodOnBelt.Count > 0)
        {
            Destroy(_foodOnBelt.Dequeue());
        }

        // manually create empty belt, so the belt is initially populated
        // populate it from right to left
        GameObject food = _factory.CreateFood(Food.None);
        _foodSize = food.renderer.bounds.extents.x * 2;

        int numOfEmptyBelt = (int)Mathf.Ceil((_rightMaskPos.x - _leftMaskPos.x) / _foodSize);
        Vector3 pos = _leftMaskPos + new Vector3(_foodSize * numOfEmptyBelt, 0, 0);
        
        food.transform.position = pos;
        _foodOnBelt.Enqueue(food);
        food.transform.parent = this.transform;

        for (; pos.x >= _leftMaskPos.x; )
        {
            food = _factory.CreateFood(Food.None);
            food.transform.position = pos;
            _foodOnBelt.Enqueue(food);
            food.transform.parent = this.transform;
            pos -= new Vector3(_foodSize, 0, 0);
        }
    }

    public void GenerateFood(Food food)
    {
        GameObject foodObj = _factory.CreateFood(food);
        foodObj.transform.position = _leftMaskPos;
        _foodOnBelt.Enqueue(foodObj);
        foodObj.transform.parent = this.transform;
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

    public void ChangeSpeed(int level)
    {
        if (0 <= level && level < _speeds.Length)
            _targetSpeed = _speeds[level];
        _speed = _targetSpeed;
    }
}
