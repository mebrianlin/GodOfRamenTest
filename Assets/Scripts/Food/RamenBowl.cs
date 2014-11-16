using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using ExtensionMethods;

public class RamenBowl:MonoBehaviour {

	public Texture[] ramen1;//black,finish
	public Texture[] ramen5;

	//ramen3
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
				Debug.Log(ramen1.Length);
				GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen1[1]);
				break;
			case 1:
				GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen1[0]);
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
			switch (_requiredIngredients.Count){
			case 0:
				GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[31]);
				break;
			case 1:
				if(_requiredIngredients.ContainsKey(Food.Carrot)){//2345
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[30]);
				}else if(_requiredIngredients.ContainsKey(Food.Chicken)){//1345
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[29]);
				}else if(_requiredIngredients.ContainsKey(Food.Pea)){//1245
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[28]);
				}else if(_requiredIngredients.ContainsKey(Food.Mushroom)){//1235
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[27]);
				}else if(_requiredIngredients.ContainsKey(Food.Tomato)){//1234
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[26]);
				}
				break;
			case 2:
				if(_requiredIngredients.ContainsKey(Food.Carrot) && _requiredIngredients.ContainsKey(Food.Chicken)){//345
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[25]);
				}else if(_requiredIngredients.ContainsKey(Food.Carrot) && _requiredIngredients.ContainsKey(Food.Pea)){//245
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[24]);
				}else if(_requiredIngredients.ContainsKey(Food.Carrot) && _requiredIngredients.ContainsKey(Food.Mushroom)){//235
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[23]);
				}else if(_requiredIngredients.ContainsKey(Food.Carrot) && _requiredIngredients.ContainsKey(Food.Tomato)){//234
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[22]);
				}else if(_requiredIngredients.ContainsKey(Food.Chicken) && _requiredIngredients.ContainsKey(Food.Pea)){//145
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[21]);
				}else if(_requiredIngredients.ContainsKey(Food.Chicken) && _requiredIngredients.ContainsKey(Food.Mushroom)){//135
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[20]);
				}else if(_requiredIngredients.ContainsKey(Food.Chicken) && _requiredIngredients.ContainsKey(Food.Tomato)){//134
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[19]);
				}else if(_requiredIngredients.ContainsKey(Food.Pea) && _requiredIngredients.ContainsKey(Food.Mushroom)){//125
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[18]);
				}else if(_requiredIngredients.ContainsKey(Food.Pea) && _requiredIngredients.ContainsKey(Food.Tomato)){//124
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[17]);
				}else if(_requiredIngredients.ContainsKey(Food.Mushroom) && _requiredIngredients.ContainsKey(Food.Tomato)){//123
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[16]);
				}
				break;
			case 3:
				if(!_requiredIngredients.ContainsKey(Food.Carrot) && !_requiredIngredients.ContainsKey(Food.Chicken)){//12
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[6]);
				}else if(!_requiredIngredients.ContainsKey(Food.Carrot) && !_requiredIngredients.ContainsKey(Food.Pea)){//13
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[7]);
				}else if(!_requiredIngredients.ContainsKey(Food.Carrot) && !_requiredIngredients.ContainsKey(Food.Mushroom)){//14
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[8]);
				}else if(!_requiredIngredients.ContainsKey(Food.Carrot) && !_requiredIngredients.ContainsKey(Food.Tomato)){//15
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[9]);
				}else if(!_requiredIngredients.ContainsKey(Food.Chicken) && !_requiredIngredients.ContainsKey(Food.Pea)){//23
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[10]);
				}else if(!_requiredIngredients.ContainsKey(Food.Chicken) && !_requiredIngredients.ContainsKey(Food.Mushroom)){//24
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[11]);
				}else if(!_requiredIngredients.ContainsKey(Food.Chicken) && !_requiredIngredients.ContainsKey(Food.Tomato)){//25
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[12]);
				}else if(!_requiredIngredients.ContainsKey(Food.Pea) && !_requiredIngredients.ContainsKey(Food.Mushroom)){//34
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[13]);
				}else if(!_requiredIngredients.ContainsKey(Food.Pea) && !_requiredIngredients.ContainsKey(Food.Tomato)){//35
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[14]);
				}else if(!_requiredIngredients.ContainsKey(Food.Mushroom) && !_requiredIngredients.ContainsKey(Food.Tomato)){//45
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[15]);
				}
				break;
			case 4:
				if(!_requiredIngredients.ContainsKey(Food.Carrot)){//1
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[1]);
				}else if(!_requiredIngredients.ContainsKey(Food.Chicken)){//2
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[2]);
				}else if(!_requiredIngredients.ContainsKey(Food.Pea)){//3
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[3]);
				}else if(!_requiredIngredients.ContainsKey(Food.Mushroom)){//4
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[4]);
				}else if(!_requiredIngredients.ContainsKey(Food.Tomato)){//5
					GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[5]);
				}
				break;
			case 5:
				GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", ramen5[0]);
				break;
			}
		}
	}

	void Start(){

	}

	void Update(){

	}
}
