using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ExtensionMethods;

public class Emcee : MonoBehaviour {

	const int MAX_TEAM = 2;
    const int MAX_INGREDIENT = 3;
    
    FoodFactory _factory;
    // mapping teams to their teamIDs
    Dictionary<RamenTeam, int> _teams = new Dictionary<RamenTeam, int>();
    List<ConveyorBelt> _conveyorBelts;

    float _generateFoodSpeed = 1.0f;

	public Food[] RequiredIngredient {
		get { return new Food[MAX_INGREDIENT]{ Food.Cai, Food.Mushroom, Food.Chicken}; }
		private set {}
	}

    int[] _teamScores = new int[MAX_TEAM];

	void Start () {
        for (int i = 0; i < _teamScores.Length; ++i)
            _teamScores[i] = 0;

        _factory = new FoodFactory();

        // GameObject[] conveyorBeltObjs = gameObject.FindObjectsWithTagInChildren("ConveyorBelt").OrderBy(obj => obj.transform.position.x).ToArray();
        // GameObject[] conveyorBeltObjs = GameObject.FindGameObjectsWithTag("ConveyorBelt");

        _conveyorBelts = this.transform.Cast<Transform>()
            .Select(x => x.gameObject)
            .Where(obj => obj.tag == "ConveyorBelt")
            .OrderByDescending(obj => obj.transform.position.x)
            .Select(obj => obj.GetComponent<ConveyorBelt>())
            .ToList();
        if (_conveyorBelts.Count != MAX_TEAM)
            Debug.LogError(string.Format("Number of ConveroyBelt({0}) does not equal to MAX_TEAM({1}).", _conveyorBelts.Count, MAX_TEAM));

        StartCoroutine(generateFood());
	}
	
	void Update () {
	
	}
    
    IEnumerator generateFood()
    {
        while (true)
        {
            Food food = _factory.GetRandomFood();
            foreach (var c in _conveyorBelts)
                c.GenerateFood(food);
            yield return new WaitForSeconds(_generateFoodSpeed);
        }
    }

    /// <summary>
    /// Get a team's ID. If it is a new team, create a new ID for it.
    /// </summary>
    /// <param name="team"></param>
    /// <returns>The team's ID. -1 too many teams.</returns>
    public int GetTeamId(RamenTeam team) {
        if (_teams.ContainsKey(team))
            return _teams[team];

        int numOfTeam = _teams.Count;
        if (numOfTeam >= MAX_TEAM)
            return -1;

        _teams.Add(team, numOfTeam);
        return _teams[team];
    }

    public GameObject GrabIngredient(RamenTeam team) {
        if (!_teams.ContainsKey(team)) {
            Debug.LogError("Cannot find the team.");
            return null;
        }
        int teamId = _teams[team];
        
        GameObject food = null;
        for (int i = 0; i < _conveyorBelts.Count; ++i) {
            GameObject tmp = _conveyorBelts[i].GrabIngredient();
            if (i == teamId)
                food = tmp;
            else
                Destroy(tmp);
        }

        return food;
    }

    public bool CompleteRamen(RamenTeam team) {
        if (_teams.ContainsKey(team))
            return false;

        int teamId = _teams[team];
        if (teamId >= MAX_TEAM)
            return false;

        ++_teamScores[teamId];
        // TODO: Brian
        switch (teamId)
        {
            default:
                break;

            case 0:
                break;

            case 1:
                break;
        }

        return true;
    }

    void OnGUI() {
        string output = "";
        for (int i = 0; i < _teamScores.Length; ++i)
            output += string.Format("Team {0}: {1} bowls           ", i, _teamScores[i].ToString());
        GUI.Label(new Rect(10, 10, 250, 250), output);
    }
}
