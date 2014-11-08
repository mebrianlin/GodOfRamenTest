﻿using UnityEngine;
using System.Collections;

public class RamenTeam : MonoBehaviour {

    int _id;
    Helper _helper;
    Apprentice _apprentice;
    ConveyorBelt _conveyorBelt;
	Emcee _emcee;

    int _numRamen;

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

		_id = _emcee.GetTeamId(this);
        _apprentice.OnNoodleReady += apprentice_OnNoodleReady;
	}

    void Update()
    {

    }

    void apprentice_OnNoodleReady()
    {
		_helper.AddNewRamen(_id);
    }

    public void GrabIngredient() {
        GameObject ingredient = _conveyorBelt.GrabIngredient();

        // did not get the ingredient
        if (ingredient == null)
            return;

        FoodOnPlateScript script = ingredient.GetComponent<FoodOnPlateScript>();
        if (script == null)
            return;
		
		Destroy(ingredient);

        FoodInfo info = script.Info;
        FoodType type = info.Type;
        float value = info.Value;

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
