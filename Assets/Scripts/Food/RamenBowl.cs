using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using ExtensionMethods;

public class RamenBowl:MonoBehaviour {

	Dictionary<Food, int> _requiredIngredients = new Dictionary<Food, int>();

	public void SetRequiredIngredients(Food[] food)
	{
		_requiredIngredients.Clear();

		foreach (var f in food) {
			if (_requiredIngredients.ContainsKey(f))
			    ++_requiredIngredients[f];
			else
			    _requiredIngredients[f] = 1;
		}
	}

	public bool AddIngredient(Food food)
	{
		if (!_requiredIngredients.ContainsKey(food))
			return false;

		if (_requiredIngredients[food] == 0)
			return false;

		if (--_requiredIngredients[food] == 0)
			_requiredIngredients.Remove(food);

		return true;
	}

	public bool IsBowlComplete() {
		return _requiredIngredients.Empty();
	}

	void Start(){

	}
}
