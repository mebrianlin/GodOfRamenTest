using UnityEngine;
using System.Collections;

public class Emcee : MonoBehaviour {

    static int _teamId = 0;


	void Start () {
	
	}
	
	void Update () {
	
	}

    /// <summary>
    /// Register a team to the emcee.
    /// </summary>
    /// <param name="team"></param>
    /// <returns>The team's ID, which the team should should keep track of.</returns>
    public int RegisterTeam(RamenTeam team) {

        return _teamId++;
    }
}
