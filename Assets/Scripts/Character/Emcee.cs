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
    /// Register a team to the emcee.
    /// </summary>
    /// <param name="team"></param>
    /// <returns>False if the team has been registered previously.</returns>
    public bool RegisterTeam(RamenTeam team) {
        if (_teams.ContainsKey(team))
            return false;

        int numOfTeam = _teams.Count;
        if (numOfTeam >= MAX_TEAM)
            return false;

        _teams.Add(team, numOfTeam);
        return true;
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
