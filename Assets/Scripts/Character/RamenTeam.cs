#define __DEBUG
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using ExtensionMethods;

public class RamenTeam : MonoBehaviour {
	public GameObject delicious;
	public GameObject samplePos;

    public string Player1Name
    {
        get;
        private set;
    }

    public string Player2Name
    {
        get;
        private set;
    }

    int _id;
	Vector3 _ingredientTargetPos;
    Helper _helper;
    Apprentice _apprentice;
	Emcee _emcee;
    LeaderboardForTeam _leaderboard;

    int _numRamen;
	Queue<GameObject> _ingredients = new Queue<GameObject>();
	List<GameObject> _ramenBowl = new List<GameObject>();

    TextMesh _scoreText;
	
	void Start () {
        // set default player names
#if __DEBUG
        int length = Random.Range(3, 10);
        string s1 = "", s2 = "";
        for (int i = 0; i < length; ++i)
        {
            s1 += (char)((int)'A' + Random.Range(0, 51));
            s2 += (char)((int)'A' + Random.Range(0, 51));
        }
         SetPlayerNames(s1, s2);
#else
        SetPlayerNames("", "");
#endif

        _apprentice = GetComponentInChildren<Apprentice>();
        _helper = GetComponentInChildren<Helper>();
        _leaderboard = GetComponentInChildren<LeaderboardForTeam>();

        GameObject obj = GameObject.FindGameObjectWithTag("Emcee");
		if (obj == null)
			Debug.LogError("Cannot find Emcee object.");
		_emcee = obj.GetComponent<Emcee>();
        _id = _emcee.GetTeamId(this);

        obj = this.gameObject.FindObjectWithTagInChildren("ScoreText");
        if (obj == null)
            Debug.LogError("Cannot find ScoreText.");
        _scoreText = obj.GetComponent<TextMesh>();

		_ingredientTargetPos = this.gameObject.FindObjectWithTagInChildren("BowlPosition").transform.position;

        if (_apprentice == null)
            Debug.LogError("Cannot find Apprentice.");
        if (_helper == null)
            Debug.LogError("Cannot find Helper.");
		_apprentice.OnNoodleReady += apprentice_OnNoodleReady;
		_helper.OnNoodleCooked += helper_OnNoodleCooked;


		StartCoroutine(movingIngredients());
	}
	
	void apprentice_OnNoodleReady()
    {
		_helper.AddNewRamen(_id);
    }

	void helper_OnNoodleCooked()
	{
		Vector3 boiledRamenPos = _ingredientTargetPos - new Vector3(_ramenBowl.Count*8, 0, 0 );;

		GameObject boiledRamenObject = 
			Instantiate(Resources.Load("Prefabs/RamenIngredient", typeof(GameObject)) as GameObject, 
			            boiledRamenPos ,  Quaternion.Euler(90, -180, 0) ) as GameObject;
		RamenBowl bowl = boiledRamenObject.GetComponent<RamenBowl>();
		if (bowl == null)
			Debug.LogError("Cannot find RAMENNNN AAAAAAAAAAAAAAA");
		bowl.SetRequiredIngredients(_emcee.RequiredIngredient);
		_ramenBowl.Add(boiledRamenObject);
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

    public void SetPlayerNames(string name1, string name2)
    {
        this.Player1Name = name1;
        this.Player2Name = name2;
    }

    public void GrabIngredient() {
        GameObject ingredient = _emcee.GrabIngredient(this);

        // did not get the ingredient
        if (ingredient == null)
            return;

		FoodScript script = ingredient.GetComponent<FoodScript>();
        if (script == null)
            return;

        FoodInfo info = script.Info;
        FoodType type = info.Type;
        float value = info.Value;
		Food food = info.Food;

		_ingredients.Enqueue(ingredient);

        // TODO: xiaoxin zhao

		for (int i = 0; i < _ramenBowl.Count; ) {
			GameObject g = _ramenBowl[i];			
			var r = g.GetComponent<RamenBowl>();
			if (r.AddIngredient(food)) {
				// if a bowl of ramen is completed (ingredients + noodles)
				r.ChangeRamenTexture(_emcee.GetRoundNum());
				if (r.IsBowlComplete()) {

					++_numRamen;
                    _scoreText.text = _numRamen.ToString();
					// TODO:
					// destroy the complete ramen
					_ramenBowl.RemoveAt(i);
					//Destroy(g);
					StartCoroutine(WaitAndDestroy(g));


					_emcee.CompleteRamen(this);
				}
				break;
			}
			else
				++i;
		}
    }

	IEnumerator WaitAndDestroy(GameObject g){

		Vector3 originPos = delicious.transform.position;
		Vector3 targetPos = originPos + new Vector3(0, 47.5f, 0);

		float moveTime = 1f;
		float currentTime = moveTime;
	
		while (currentTime>=0) {
			currentTime -= 0.01f;
			yield return new WaitForSeconds(0.01f);
			delicious.transform.position = Vector3.Lerp( originPos,targetPos, 1-currentTime/moveTime);
		}

		delicious.GetComponent<Animator>().SetBool("praise", true);

		yield return new WaitForSeconds(1f);

		while (currentTime>=-1 && currentTime<0) {
			currentTime -= 0.01f;
			yield return new WaitForSeconds(0.01f);
			delicious.transform.position = Vector3.Lerp( targetPos, originPos,-currentTime/moveTime);
		}

		Destroy(g);
		delicious.GetComponent<Animator>().SetBool("praise", false);
		foreach(var ramenB in _ramenBowl){
			ramenB.transform.position += new Vector3(8f,0f,0f);
		}
	}

    public void ShowLeaderboard(LeaderboardInsertResult rank) {
        _leaderboard.Show(rank);
    }

    public void HideLeaderboard() {
        _leaderboard.Hide();
    }

	public void ChangeIngredientAfterOneRound( int round){

		_ingredients.Clear();
		int currentBoiledRamenNum = _ramenBowl.Count;

		for(int i = 0 ; i< _ramenBowl.Count; i++){
			GameObject g = _ramenBowl[i];
			_ramenBowl.RemoveAt(i);
			Destroy(g);
		}

		string ingredientPrefabPath = "Prefabs/RamenIngredient" + round.ToString();

		for(int i = 0; i< currentBoiledRamenNum; i++){
			Vector3 boiledRamenPos = _ingredientTargetPos - new Vector3(i*8, 0, 0 );;
			GameObject g = Instantiate(Resources.Load(ingredientPrefabPath, typeof(GameObject)) as GameObject, 
			                boiledRamenPos ,  Quaternion.Euler(90, -180, 0) ) as GameObject;
			g.GetComponent<RamenBowl>().SetRequiredIngredients (_emcee.RequiredIngredient);
			_ramenBowl.Add(g);
		}

		//change sample
		GameObject sampleRamen = this.gameObject.FindObjectWithTagInChildren("SampleRamen");
		if(sampleRamen!= null){
			Destroy(sampleRamen);
		}
		string samplePrefabPath = "Prefabs/FinishedRamenSample" + round.ToString();
		GameObject s = Instantiate(Resources.Load(samplePrefabPath, typeof(GameObject)) as GameObject,
		                           samplePos.transform.position, Quaternion.identity) as GameObject;
		s.transform.parent = this.gameObject.transform;

	}
}
