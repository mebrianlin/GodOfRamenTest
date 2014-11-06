using System.Collections;
using System.Collections.Generic;

public class FoodFactory {

    public enum FoodType
    {
        Meat,
        Vegetable,
        Sauce
    }

    public sealed class Food
    {
        public Food(string name, FoodType type)
        {
            this.Name = name;
            this.Type = type;
        }

        public string Name
        {
            get;
            private set;
        }

        public FoodType Type
        {
            get;
            private set;
        }


    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public Food CreateFood()
    {
        return new Food("mushroom", FoodType.Vegetable);
    }

    public Food CreateFood(FoodType type)
    {
        return new Food("mushroom", FoodType.Vegetable);
    }

}
