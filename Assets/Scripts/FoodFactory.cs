using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum Food {
	Mushroom,
	Cai,
	Corn,
};

public class FoodFactory {

	string _prefabDir = "Prefabs/Food/";

	static Dictionary<Food, FoodInfo> _dictionary = new Dictionary<Food, FoodInfo>() {
		{ Food.Mushroom, new FoodInfo { PrefabName = "Mushroom", Type = FoodType.Vegetable, Value = 10f }},
		{ Food.Cai,      new FoodInfo { PrefabName = "Cai",      Type = FoodType.Vegetable, Value =  5f }},
		{ Food.Corn,     new FoodInfo { PrefabName = "Corn",     Type = FoodType.Vegetable, Value =  5f }},
	};

	public GameObject CreateFood()
	{
		return CreateFood(_dictionary.ElementAt(Random.Range(0, _dictionary.Count)).Key);
    }

	public GameObject CreateFood(FoodType type)
	{
		/*
		Food food = _dictionary.Where(p => p.Value.Type == type)
			.Select(p => p.Key)
			.OrderBy(x => Random.Range(0f, 1f))
			.ElementAt(0);

		*/
		Food food = _dictionary.OrderBy(x => Random.Range(0f, 1f))
			.SkipWhile(x => x.Value.Type != type)
			.Select(x => x.Key)
			.ElementAt(0);
			
		return CreateFood(food);
	}
	
	public GameObject CreateFood(Food food)
	{
		Debug.Log("Creating food..." + food.ToString());

		if (!_dictionary.ContainsKey(food))
			return null;

		FoodInfo info = _dictionary[food];
		string prefabFilePath = _prefabDir + info.PrefabName;
		GameObject foodObject = Object.Instantiate(
			Resources.Load(prefabFilePath, typeof(GameObject)) as GameObject,
			Vector3.zero,
			Quaternion.identity) as GameObject;

		FoodScript script = foodObject.GetComponent<FoodScript>();
		if (script == null)
			return null;
		else
			script.Info = info;

		return foodObject;
	}

}
