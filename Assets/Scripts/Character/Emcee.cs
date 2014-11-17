using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ExtensionMethods;

public class Emcee : MonoBehaviour
{
    public GameObject[] transitions1;
    public GameObject[] transitions2;


    const int MAX_TEAM = 2;
    const int MAX_INGREDIENT = 3;
    const int COMBINATION_NUM = 3;

    int TIME_PER_ROUnD = GameSettings.GetInt("TimePerRound");

    Timer _timer;
    FoodFactory _factory;
    Leaderboard _leaderboard = Leaderboard.Instance;
    // mapping teams to their teamIDs
    Dictionary<RamenTeam, int> _teams = new Dictionary<RamenTeam, int>();
    List<ConveyorBelt> _conveyorBelts;

    public Food[] RequiredIngredient;
    Food[][] _foodsOnBeltInRounds = {
			new Food[] {Food.Cai,    Food.Shrimp,  Food.Eggs, Food.None },
			new Food[] {Food.Cai,    Food.Eggs,    Food.Meat, Food.Carrot,   Food.Chicken, Food.None},
			new Food[] {Food.Carrot, Food.Chicken, Food.Pea,  Food.Mushroom, Food.Tomato,  Food.Carrot, Food.Cai, Food.Shrimp, },
		};
    public Food[][] RequiredIngredientCombinations = new Food[][]{
			new Food[]{Food.Shrimp},
			new Food[]{Food.Cai, Food.Eggs, Food.Meat, },
			new Food[]{Food.Carrot, Food.Chicken, Food.Mushroom, Food.Pea, Food.Tomato}
		};




    int _round = 0;
    const int TOTAL_ROUND = 3;
    int[] _teamScores = new int[MAX_TEAM];
    LeaderboardInsertResult[] _insertResults = new LeaderboardInsertResult[MAX_TEAM];

    public void Reset()
    {
        for (int i = 0; i < _teamScores.Length; ++i)
            _teamScores[i] = 0;
        for (int i = 0; i < _insertResults.Length; ++i)
            _insertResults[i] = LeaderboardInsertResult.Default;
    }

    void Awake()
    {
        GameSettings.OnBoolValueChange += GameSettings_OnBoolValueChange;   
    }

    void GameSettings_OnBoolValueChange(string name, bool b)
    {
        if (name == "Pause")
        {
            if (b)
                _timer.StopTimer();
            else
                _timer.StartTimer();
        }
    }

    void Start()
    {
        _timer = GetComponentInChildren<Timer>();
        _timer.OnTimeElpased += timeUp;
        _timer.Interval = TIME_PER_ROUnD;
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

        Reset();
        RequiredIngredient = RequiredIngredientCombinations[_round];


        StartCoroutine("generateFood");
        StartCoroutine(WaitAndPlayTransitionAnimation());

    }

    void Update()
    {
    }

    public void Restart()
    {
        Application.LoadLevel("Main");
    }

    public Food randomFoodInArray(Food[] foods)
    {
        if (foods.Length == 0)
            return Food.None;
        return foods[Random.Range(0, foods.Length)];
    }

    IEnumerator generateFood()
    {
        while (true)
        {
            Food food = randomFoodInArray(_foodsOnBeltInRounds[_round]);
            foreach (var c in _conveyorBelts)
                c.GenerateFood(food);

            float elapsedTime = 0;
            while (elapsedTime < 9.5f  / _conveyorBelts[0].Speed)
            {
                elapsedTime += 1;
                yield return new WaitForFixedUpdate();
            }
        }
    }

    /// <summary>
    /// Get a team's ID. If it is a new team, create a new ID for it.
    /// </summary>
    /// <param name="team"></param>
    /// <returns>The team's ID. -1 too many teams.</returns>
    public int GetTeamId(RamenTeam team)
    {
        if (_teams.ContainsKey(team))
            return _teams[team];

        int numOfTeam = _teams.Count;
        if (numOfTeam >= MAX_TEAM)
            return -1;

        _teams.Add(team, numOfTeam);
        return _teams[team];
    }

    public GameObject GrabIngredient(RamenTeam team)
    {
        if (!_teams.ContainsKey(team))
        {
            Debug.LogError("Cannot find the team.");
            return null;
        }
        int teamId = _teams[team];
        if (teamId >= MAX_TEAM)
            return null;

        GameObject food = null;
        for (int i = 0; i < _conveyorBelts.Count; ++i)
        {
            GameObject tmp = _conveyorBelts[i].GrabIngredient();
            if (i == teamId)
                food = tmp;
            else
                Destroy(tmp);
        }

        return food;
    }

    public bool CompleteRamen(RamenTeam team)
    {
        if (!_teams.ContainsKey(team))
        {
            Debug.LogError("Cannot find the team.");
            return false;
        }

        int teamId = _teams[team];
        if (teamId >= MAX_TEAM)
            return false;

        ++_teamScores[teamId];

        return true;
    }

    public LeaderboardInsertResult GetTeamRank(RamenTeam team)
    {
        if (!_teams.ContainsKey(team))
        {
            Debug.LogError("Cannot find the team.");
            return LeaderboardInsertResult.Default;
        }

        int teamId = _teams[team];
        if (teamId >= MAX_TEAM)
            return LeaderboardInsertResult.Default;

        return _insertResults[teamId];
    }

    void timeUp(GameObject sender)
    {
        ++_round;
        if (_round >= TOTAL_ROUND)
        {
            _round = 0;
            endGame();
        }
        else
        {
            foreach (var conveyorBelt in _conveyorBelts)
                conveyorBelt.ChangeSpeed(_round);

            StartCoroutine(WaitAndPlayTransitionAnimation());
        }
    }

    void endGame()
    {

        if (GameSettings.GetBool("DebugMode"))
            _teamScores = new int[] { Random.Range(1, 100), Random.Range(1, 100) };

        // if the game has ended
        LeaderboardEntry[] entries = _teams
            .OrderBy(x => x.Value) // order by team id
            .Select((x, i) => new LeaderboardEntry { Player1Name = x.Key.Player1Name, Player2Name = x.Key.Player2Name, Score = _teamScores[i] })
            .ToArray();
        //LeaderboardEntry[] entries = _teamScores
        //     .Select(x => new LeaderboardEntry { Player1Name = "", Player2Name = "", Score = x })
        //    .ToArray();
        _insertResults = _leaderboard.AddEntries(entries);

        foreach (var t in _teams)
            t.Key.ShowLeaderboard(_insertResults[t.Value]);
    }

    void ChangeNewBowlOfRamen()
    {
        Debug.Log("Round = " + _round + ". Change new bowl of Ramen");
        if (_round <= RequiredIngredientCombinations.GetLength(0))
        {
            RequiredIngredient = RequiredIngredientCombinations[_round];
        }
        foreach (var t in _teams)
        {
            t.Key.ChangeIngredientAfterOneRound(_round);
        }
    }

    public int GetRoundNum()
    {
        return _round;
    }


    IEnumerator WaitAndPlayTransitionAnimation()
    {
        //StopCoroutine("generateFood");
        GameSettings.SetBool("Pause", true);

        float audioLength = SoundManager.instance.PlayerTransitionSound(_round);
        
        foreach (var team in _teams)
        {
            team.Key.ShowSampleRamen();
        }

        float start = Time.realtimeSinceStartup;
        //while (Time.realtimeSinceStartup < start + 5f) {
        while (Time.realtimeSinceStartup < start + audioLength)
        {
            yield return null;
        }

        foreach (var team in _teams)
            team.Key.HideSampleRamen();
        /*
        for (int i = 0; i < TOTAL_ROUND; i++)
        {
            transitions1[i].SetActive(false);
            transitions2[i].SetActive(false);
        }*/

        GameSettings.SetBool("Pause", false);

        //StartCoroutine("generateFood");

        if (_round >= TOTAL_ROUND)
        {
            _round = 0;
            endGame();
        }
        else
        {
            if (_round != 0)
            {
                //foreach (var belt in _conveyorBelts)
                //    belt.Reset();
            }
            ChangeNewBowlOfRamen();
            _timer.StartTimer();
        }
    }
}
