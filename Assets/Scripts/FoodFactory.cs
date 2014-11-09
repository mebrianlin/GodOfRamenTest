using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum Food {
	Cai,
	Carrot,
	Chicken,
	Eggs,
	Fish,
	Meat,
	Mushroom,
	Pea,
	Shrimp,
	Tomato,
	None,
};

public enum FoodType
{
    Vegetable,
    Meat,
    None,
};

public struct FoodInfo
{
    public string PrefabName { get; set; }
    public FoodType Type { get; set; }
    public float Value { get; set; }
	public Food Food { get; set; }

    public static FoodInfo None = new FoodInfo { PrefabName = "", Type = FoodType.None, Value = 0f, Food = Food.None };
}

public class FoodFactory {

	string _prefabDir = "Prefabs/Food/";

	static Dictionary<Food, FoodInfo> _dictionary = new Dictionary<Food, FoodInfo>() {
		{ Food.None,     new FoodInfo { PrefabName = "None",     Type = FoodType.Vegetable, Value =  0f, Food = Food.None      } },
		{ Food.Cai,      new FoodInfo { PrefabName = "Cai",      Type = FoodType.Vegetable, Value =  5f, Food = Food.Cai      } },
		{ Food.Carrot,   new FoodInfo { PrefabName = "Carrot",   Type = FoodType.Vegetable, Value =  5f, Food = Food.Carrot   } },
		{ Food.Chicken,  new FoodInfo { PrefabName = "Chicken",  Type = FoodType.Meat,      Value =  5f, Food = Food.Chicken  } },
		{ Food.Eggs,  	 new FoodInfo { PrefabName = "Eggs",  	 Type = FoodType.Meat,      Value =  5f, Food = Food.Eggs     } },
		{ Food.Fish,  	 new FoodInfo { PrefabName = "Fish",  	 Type = FoodType.Meat,      Value =  5f, Food = Food.Fish     } },
		{ Food.Meat,  	 new FoodInfo { PrefabName = "Meat",  	 Type = FoodType.Meat,      Value =  5f, Food = Food.Meat     } },
		{ Food.Mushroom, new FoodInfo { PrefabName = "Mushroom", Type = FoodType.Vegetable, Value = 10f, Food = Food.Mushroom } },
		{ Food.Pea,      new FoodInfo { PrefabName = "Pea",      Type = FoodType.Vegetable, Value =  5f, Food = Food.Pea      } },
		{ Food.Shrimp,   new FoodInfo { PrefabName = "Shrimp",	 Type = FoodType.Meat,      Value = 10f, Food = Food.Shrimp   } },
		{ Food.Tomato,   new FoodInfo { PrefabName = "Tomato",	 Type = FoodType.Vegetable, Value =  5f, Food = Food.Tomato   } },
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
		//Debug.Log("Creating food..." + food.ToString());

		if (!_dictionary.ContainsKey(food))
			return null;

		FoodInfo info = _dictionary[food];
		string prefabFilePath = _prefabDir + info.PrefabName;
		GameObject foodObject = Object.Instantiate(
			Resources.Load(prefabFilePath, typeof(GameObject)) as GameObject) as GameObject;

		FoodOnPlateScript script = foodObject.GetComponent<FoodOnPlateScript>();
		if (script == null)
			return null;
		else {
			script.Info = info;
			script.InFocus = false;
		}

		return foodObject;
	}

}
