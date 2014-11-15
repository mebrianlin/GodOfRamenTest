using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ExtensionMethods;

public class Emcee : MonoBehaviour {

	const int MAX_TEAM = 2;
    const int MAX_INGREDIENT = 3;

    Timer _timer;
    FoodFactory _factory;
    Leaderboard _leaderboard = Leaderboard.Instance;
    // mapping teams to their teamIDs
    Dictionary<RamenTeam, int> _teams = new Dictionary<RamenTeam, int>();
    List<ConveyorBelt> _conveyorBelts;

    float _generateFoodSpeed = 1.0f;

	public Food[] RequiredIngredient {
		get { return new Food[MAX_INGREDIENT]{ Food.Eggs, Food.Meat, Food.Cai}; }
		private set {}
	}


    int _round = 0;
    const int TOTAL_ROUND = 3;
    int[] _teamScores = new int[MAX_TEAM];
    int[] _ranks = new int[MAX_TEAM];

    public void Reset() {
        for (int i = 0; i < _teamScores.Length; ++i)
            _teamScores[i] = 0;
        for (int i = 0; i < _ranks.Length; ++i)
            _ranks[i] = -1;
    }

	void Start () {

        _timer = GetComponentInChildren<Timer>();
        _timer.OnTimeElpased += timeUp;
        _timer.Interval = 60;
        _timer.StartTimer();

        _factory = new FoodFactory();

        // GameObject[] conveyorBeltObjs = gameObject.FindObjectsWithTagInChildren("ConveyorBelt").OrderBy(obj => obj.transform.position.x).ToArray();
        // GameObject[] conveyorBeltObjs = GameObject.FindGameObjectsWithTag("ConveyorBelt");

        _conveyorBelts = this.transform.Cast<Transform>()
            .Select(x => x.gameObject)
            .Where(obj => obj.tag == "ConveyorBelt")
            .OrderBy(obj => obj.transform.position.x)
            .Select(obj => obj.GetComponent<ConveyorBelt>())
            .ToList();
        if (_conveyorBelts.Count != MAX_TEAM)
            Debug.LogError(string.Format("Number of ConveroyBelt({0}) does not equal to MAX_TEAM({1}).", _conveyorBelts.Count, MAX_TEAM));


        _generateFoodSpeed = 9.5f * Time.fixedDeltaTime / _conveyorBelts[0].Speed.x;

        Reset();

        StartCoroutine(generateFood());

#if __DEBUG
        StartCoroutine(test());
#endif
	}

    IEnumerator test()
    {
        yield return new WaitForSeconds(1f);
        _teamScores = new int[2] { 20, 25 };
        endGame();
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
        if (teamId >= MAX_TEAM)
            return null;

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
        if (!_teams.ContainsKey(team)) {
            Debug.LogError("Cannot find the team.");
            return false;
        }

        int teamId = _teams[team];
        if (teamId >= MAX_TEAM)
            return false;

        ++_teamScores[teamId];

        return true;
    }

    public int GetTeamRank(RamenTeam team)
    {
        if (!_teams.ContainsKey(team)) {
            Debug.LogError("Cannot find the team.");
            return -1;
        }

        int teamId = _teams[team];
        if (teamId >= MAX_TEAM)
            return -1;

        return _ranks[teamId];
    }

    void timeUp(GameObject sender) {
        ++_round;
        if (_round >= TOTAL_ROUND) {
            _round = 0;
            endGame();
        }
        else
            throw new System.NotImplementedException("Change to a new bowl of ramen");
    }

    void endGame() {
        // if the game has ended
        LeaderboardEntry[] entries = _teamScores
            .Select(x => new LeaderboardEntry { Player1Name = "", Player2Name = "", Score = x })
            .ToArray();
        _ranks = _leaderboard.AddEntries(entries);

        foreach (var t in _teams)
            t.Key.ShowLeaderboard(_ranks[t.Value]);
    }
}
