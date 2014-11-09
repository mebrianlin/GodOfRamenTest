using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Emcee : MonoBehaviour {

	const int MAX_TEAM = 2;
	const int MAX_INGREDIENT = 3;
    Dictionary<RamenTeam, int> _teams = new Dictionary<RamenTeam, int>();

	public Food[] RequiredIngredient {
		get { return new Food[MAX_INGREDIENT]{ Food.Cai, Food.Mushroom, Food.Chicken}; }
		private set {}
	}

    int[] _teamScores = new int[MAX_TEAM];

	void Start () {
        for (int i = 0; i < _teamScores.Length; ++i)
            _teamScores[i] = 0;
	}
	
	void Update () {
	
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
