using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using ExtensionMethods;

public class RamenTeam : MonoBehaviour
{
    public GameObject delicious;
    public GameObject samplePos;
    public GameObject[] transitions;

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


    //instructor
    Vector3 instructorOriginPos;
    bool instructorIsMoving = false;

    string[] randomNames = new string[] {
        "Xin", "Eric", "Chuan", "Jialin", "Brian", "Martin"
    };

    void Awake()
    {
        if (GameSettings.GetBool("DebugMode"))
        {
            int length = Random.Range(3, 5);
            string s1 = "", s2 = "";
            for (int i = 0; i < length; ++i)
            {
                s1 += (char)((int)'A' + Random.Range(0, 26));
                s2 += (char)((int)'A' + Random.Range(0, 26));
            }
        }
        else
        {
            // set default player names
            string s1 = randomNames[Random.Range(0, randomNames.Length)];
            string s2 = randomNames[Random.Range(0, randomNames.Length)];
            SetPlayerNames(s1, s2);
        }

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

        instructorOriginPos = delicious.transform.position;
    }

    void apprentice_OnNoodleReady()
    {
        _helper.AddNewRamen(_id);
    }

    void helper_OnNoodleCooked()
    {
        if (_ramenBowl.Count < 5)
        {
            Vector3 boiledRamenPos = _ingredientTargetPos - new Vector3(0, _ramenBowl.Count * 8, 0); ;

            string prefabPath = "Prefabs/RamenIngredient" + _emcee.GetRoundNum().ToString();
            GameObject boiledRamenObject =
                Instantiate(Resources.Load(prefabPath, typeof(GameObject)) as GameObject,
                            boiledRamenPos, Quaternion.Euler(90, -180, 0)) as GameObject;
            RamenBowl bowl = boiledRamenObject.GetComponent<RamenBowl>();
            if (bowl == null)
                Debug.LogError("Cannot find RAMENNNN AAAAAAAAAAAAAAA");
            bowl.SetRequiredIngredients(_emcee.RequiredIngredient);
            _ramenBowl.Add(boiledRamenObject);
        }
        else
        {
            Debug.Log("Bar is full. No place to put boiled ramen!");
        }
    }

    IEnumerator movingIngredients()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();

            while (!_ingredients.Empty())
            {
                //if (!_ingredients.Empty()) {
                GameObject top = _ingredients.Peek();

                if ((top.transform.position - _ingredientTargetPos).sqrMagnitude < 0.5f)
                {
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

    public void GrabIngredient()
    {
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
        bool isIngredientCorrect = false;
        for (int i = 0; i < _ramenBowl.Count; )
        {
            GameObject g = _ramenBowl[i];
            var r = g.GetComponent<RamenBowl>();
            if (r.AddIngredient(food))
            {
                isIngredientCorrect = true;

                // if a bowl of ramen is completed (ingredients + noodles)
                r.ChangeRamenTexture(_emcee.GetRoundNum());
                if (r.IsBowlComplete())
                {

                    ++_numRamen;
                    _scoreText.text = _numRamen.ToString();
                    // TODO:
                    // destroy the complete ramen
                    _ramenBowl.RemoveAt(i);
                    //Destroy(g);
                    //StartCoroutine(WaitAndDestroy(g));

                    SoundManager.instance.PlayDelicious();
                    StartCoroutine(InstructorMoving());
                    Destroy(g);
                    foreach (var ramenB in _ramenBowl)
                    {
                        ramenB.transform.position += new Vector3(0f, 8f, 0f);
                    }
                    _emcee.CompleteRamen(this);
                    delicious.transform.position = instructorOriginPos;
                }
                break;
            }
            else
                ++i;
        }
        if (!isIngredientCorrect)
            SoundManager.instance.PlayWarning();
    }

    IEnumerator WaitAndDestroy(GameObject g)
    {

        Vector3 originPos = instructorOriginPos;
        Vector3 targetPos = originPos + new Vector3(0, 47.5f, 0);

        float moveTime = 0.7f;
        float currentTime = moveTime;

        while (currentTime >= 0)
        {
            currentTime -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            delicious.transform.position = Vector3.Lerp(originPos, targetPos, 1 - currentTime / moveTime);
        }

        delicious.GetComponent<Animator>().SetBool("praise", true);

        yield return new WaitForSeconds(1f);

        while (currentTime >= -0.7f && currentTime < 0)
        {
            currentTime -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            delicious.transform.position = Vector3.Lerp(targetPos, originPos, -currentTime / moveTime);
        }

        Destroy(g);
        delicious.GetComponent<Animator>().SetBool("praise", false);
        foreach (var ramenB in _ramenBowl)
        {
            ramenB.transform.position += new Vector3(0f, 8f, 0f);
        }
    }


    IEnumerator InstructorMoving()
    {
        Vector3 originPos = instructorOriginPos;
        Vector3 targetPos = originPos + new Vector3(0, 47.5f, 0);

        float moveTime = 0.7f;
        float currentTime = moveTime;

        while (currentTime >= 0)
        {
            currentTime -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            delicious.transform.position = Vector3.Lerp(originPos, targetPos, 1 - currentTime / moveTime);
        }

        delicious.GetComponent<Animator>().SetBool("praise", true);

        yield return new WaitForSeconds(1f);

        while (currentTime >= -0.7f && currentTime < 0)
        {
            currentTime -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            delicious.transform.position = Vector3.Lerp(targetPos, originPos, -currentTime / moveTime);
        }
        delicious.GetComponent<Animator>().SetBool("praise", false);

    }

    public void ShowLeaderboard(LeaderboardInsertResult rank)
    {
        _scoreText.text = "";
        _leaderboard.Show(rank);
    }

    public void HideLeaderboard()
    {
        _leaderboard.Hide();
    }

    

    public void ShowSampleRamen()
    {
        int round = _emcee.GetRoundNum();
        for (int i = 0; i < transitions.Length; i++)
        {
            transitions[i].SetActive(i == round);
        }
    }

    public void HideSampleRamen()
    {
        int round = _emcee.GetRoundNum();
        if (0 <= round && round < transitions.Length)
            StartCoroutine(moveTransitionToSample(transitions[round]));
    }

    IEnumerator moveTransitionToSample(GameObject transition)
    {
        int step = 20;
        float distance = (transition.transform.position - samplePos.transform.position).magnitude / step;
        float scaleStep = (transition.transform.localScale - samplePos.transform.localScale).magnitude / step;
        while (transition.transform.position != samplePos.transform.position)
        {
            transition.transform.position = Vector3.MoveTowards(transition.transform.position, samplePos.transform.position, distance);
            transition.transform.localScale = Vector3.MoveTowards(transition.transform.localScale, samplePos.transform.localScale, scaleStep);
            yield return new WaitForFixedUpdate();
        }
    }

    public void ChangeIngredientAfterOneRound(int round)
    {
        delicious.transform.position = instructorOriginPos;
        delicious.GetComponent<Animator>().SetBool("praise", false);


        while (!_ingredients.Empty())
        {
            GameObject top = _ingredients.Peek();
            _ingredients.Dequeue();
            Destroy(top);
        }
        //_ingredients.Clear();

        int currentBoiledRamenNum = _ramenBowl.Count;
        for (int i = 0; i < _ramenBowl.Count; i++)
        {
            GameObject g = _ramenBowl[i];
            Destroy(g);
        }

        _ramenBowl.Clear();

        string ingredientPrefabPath = "Prefabs/RamenIngredient" + round.ToString();

        for (int i = 0; i < currentBoiledRamenNum; i++)
        {
            Vector3 boiledRamenPos = _ingredientTargetPos - new Vector3(0, i * 8, 0); ;
            GameObject g = Instantiate(Resources.Load(ingredientPrefabPath, typeof(GameObject)) as GameObject,
                            boiledRamenPos, Quaternion.Euler(90, -180, 0)) as GameObject;
            g.GetComponent<RamenBowl>().SetRequiredIngredients(_emcee.RequiredIngredient);
            _ramenBowl.Add(g);
        }

        //change sample
        GameObject sampleRamen = this.gameObject.FindObjectWithTagInChildren("SampleRamen");
        if (sampleRamen != null)
        {
            Destroy(sampleRamen);
        }
        string samplePrefabPath = "Prefabs/FinishedRamenSample" + round.ToString();
        GameObject s = Instantiate(Resources.Load(samplePrefabPath, typeof(GameObject)) as GameObject,
                                   samplePos.transform.position, Quaternion.identity) as GameObject;
        s.transform.parent = this.gameObject.transform;
        s.SetActive(false);


    }


    public void Reset()
    {

    }


    GUIStyle customTextStyle;
    string p1 = "Your Name";
    string p2 = "Your Name";
    void OnGUI()
    {
        /*
        if (customTextStyle == null)
        {
            customTextStyle = new GUIStyle(GUI.skin.textField);
            customTextStyle.fontSize = 36;
            customTextStyle.normal.textColor = Color.white;
            customTextStyle.alignment = TextAnchor.MiddleLeft;
        }
        float x = transform.position.x / 100 * Screen.width / 2 + Screen.width / 8;
        float y = Screen.height / 2;
        p1 = GUI.TextField(new Rect(x, y - 30, Screen.width / 4, 50), p1, customTextStyle);
        p2 = GUI.TextField(new Rect(x, y + 30, Screen.width / 4, 50), p2, customTextStyle);
        */
    }

}
