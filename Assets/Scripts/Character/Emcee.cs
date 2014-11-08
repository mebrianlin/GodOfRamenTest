using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Emcee : MonoBehaviour {

    const int MAX_TEAM = 2;
    Dictionary<RamenTeam, int> _teams = new Dictionary<RamenTeam, int>();


	void Start () {
	
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

        // TODO: Brian

        return true;
    }
}
