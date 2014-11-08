using UnityEngine;
using System.Collections;

public class KeyboardSimulator : MonoBehaviour {

    const int MAX_TEAM = 2;
    public RamenTeam[] Teams;
    
    Apprentice[] _apprentices;
    Helper[] _helpers;

    KeyCode[] _addRamenCode = new KeyCode[MAX_TEAM] { KeyCode.LeftShift, KeyCode.RightShift };
    KeyCode[] _increaseTempCode = new KeyCode[MAX_TEAM] { KeyCode.LeftControl, KeyCode.RightControl };
    KeyCode[] _grabIngredientCode = new KeyCode[MAX_TEAM] { KeyCode.LeftAlt, KeyCode.RightAlt };

	void Start () {
        if (Teams.Length > MAX_TEAM)
            Debug.LogError("Too many teams");

        _apprentices = new Apprentice[Teams.Length];
        _helpers = new Helper[Teams.Length];

        for (int i = 0; i < Teams.Length; ++i)
        {
            RamenTeam team = Teams[i];
            _apprentices[i] = team.GetComponentInChildren<Apprentice>();
            _helpers[i] = team.GetComponentInChildren<Helper>();
        }
	}
	
	void Update () {
        for (int i = 0; i < MAX_TEAM && i < Teams.Length; ++i)
        {
            if (Input.GetKeyDown(_addRamenCode[i]))
                _helpers[i].AddNewRamen();

            if (Input.GetKey(_increaseTempCode[i]))
                _helpers[i].IncreaseTemperature();

            if (Input.GetKeyDown(_grabIngredientCode[i]))
                Teams[i].GrabIngredient();
        }
	}
}
