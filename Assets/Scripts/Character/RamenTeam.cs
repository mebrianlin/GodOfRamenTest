using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using ExtensionMethods;

public class RamenTeam : MonoBehaviour {

    int _id;
	Vector3 _ingredientTargetPos;
    Helper _helper;
    Apprentice _apprentice;
    ConveyorBelt _conveyorBelt;
	Emcee _emcee;

    int _numRamen;
	Queue<GameObject> _ingredients = new Queue<GameObject>();

	void Start () {
        _apprentice = GetComponentInChildren<Apprentice>();
        _helper = GetComponentInChildren<Helper>();
        GameObject obj = GameObject.FindGameObjectWithTag("ConveyorBelt");
        if (obj == null)
            Debug.LogError("Cannot find ConveyorBelt object.");
        _conveyorBelt = obj.GetComponent<ConveyorBelt>();

		obj = GameObject.FindGameObjectWithTag("Emcee");
		if (obj == null)
			Debug.LogError("Cannot find Emcee object.");
		_emcee = obj.GetComponent<Emcee>();

        if (_apprentice == null)
            Debug.LogError("Cannot find Apprentice.");
        if (_helper == null)
            Debug.LogError("Cannot find Helper.");
		_apprentice.OnNoodleReady += apprentice_OnNoodleReady;

		_id = _emcee.GetTeamId(this);
		if (_id == 0)
			_ingredientTargetPos = new Vector3(15, 0, 0);
		else
			_ingredientTargetPos = new Vector3(-15, 0, 0);

		StartCoroutine(movingIngredients());
	}
	
    void FixedUpdate()
	{
		
	}
	
	void apprentice_OnNoodleReady()
    {
		_helper.AddNewRamen(_id);
    }

	IEnumerator movingIngredients() {
		while (true) {
			yield return new WaitForFixedUpdate();
			
            while (!_ingredients.Empty()) {
            //if (!_ingredients.Empty()) {
			    GameObject top = _ingredients.Peek();

                if ((top.transform.position - _ingredientTargetPos).sqrMagnitude < 0.5f) {
                    _ingredients.Dequeue();
                    Destroy(top);
                }
                else
                    break;
            }
			
			foreach (var i in _ingredients)
				i.transform.position = Vector3.Lerp(i.transform.position, _ingredientTargetPos, 0.05f);
		}
	}

    public void GrabIngredient() {
        GameObject ingredient = _conveyorBelt.GrabIngredient();

        // did not get the ingredient
        if (ingredient == null)
            return;

		FoodScript script = ingredient.GetComponent<FoodScript>();
        if (script == null)
            return;

        FoodInfo info = script.Info;
        FoodType type = info.Type;
        float value = info.Value;

		_ingredients.Enqueue(ingredient);

        // TODO: xiaoxin zhao

        // if a bowl of ramen is completed (ingredients + noodles)
        if (false)
        {
            ++_numRamen;
            // add score
            // destroy the complete ramen
	

			_emcee.CompleteRamen(this);
        }
    }
}
