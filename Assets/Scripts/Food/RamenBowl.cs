using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using ExtensionMethods;

public class RamenBowl:MonoBehaviour {

	public Texture ramenAllBlack;
	public Texture ramenCai;
	public Texture ramenMeat;
	public Texture ramenEgg;
	public Texture ramenCaiMeat;
	public Texture ramenCaiEgg;
	public Texture ramenMeatEgg;
	public Texture ramenFinished;

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

	//hardcoded T_T Sorry
	public void ChangeRamenTexture(int round){
//		Debug.Log("Required Ingredient Number: " + _requiredIngredients.Count);

		if(round == 0){
			switch (_requiredIngredients.Count){
			case 0:
				GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramenFinished);
				break;
			case 1:
				GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramenEgg);
				break;
			}

		}else if(round == 1){
			switch (_requiredIngredients.Count){
			case 0:
				GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramenFinished);
				break;
			case 1:
				//Debug.Log("Need 1 more!");
				if(_requiredIngredients.ContainsKey(Food.Cai))
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramenMeatEgg);
				else if(_requiredIngredients.ContainsKey(Food.Meat))
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramenCaiEgg);
				else if(_requiredIngredients.ContainsKey(Food.Eggs))
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramenCaiMeat);
				break;
			case 2:
				//Debug.Log("Need 2 more!");
				if(!_requiredIngredients.ContainsKey(Food.Cai))
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramenCai);
				else if(!_requiredIngredients.ContainsKey(Food.Meat))
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramenMeat);
				else if(!_requiredIngredients.ContainsKey(Food.Eggs))
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramenEgg);
				break;
			case 3:
				GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramenAllBlack);
				break;
			}
		}else if(round == 2){
			//
		}
	}

	void Start(){

	}

	void Update(){

	}
}
