using UnityEngine;
using System.Collections;

public class KeyboardSimulator : MonoBehaviour {

    const int MAX_TEAM = 2;
    public RamenTeam[] Teams;

	public GameObject[] Hands;

    Apprentice[] _apprentices;
    Helper[] _helpers;

    KeyCode[] _addRamenCode       = new KeyCode[MAX_TEAM]   { KeyCode.LeftShift,    KeyCode.RightShift };
    KeyCode[] _increaseTempCode   = new KeyCode[MAX_TEAM]   { KeyCode.LeftControl,  KeyCode.RightControl };
    KeyCode[] _grabIngredientCode = new KeyCode[MAX_TEAM]   { KeyCode.LeftAlt,      KeyCode.RightAlt };
	KeyCode[] _moveHandUpCode     = new KeyCode[MAX_TEAM*2] { KeyCode.W, KeyCode.I, KeyCode.UpArrow,    KeyCode.Keypad8 };
	KeyCode[] _moveHandDownCode   = new KeyCode[MAX_TEAM*2] { KeyCode.S, KeyCode.K, KeyCode.DownArrow,  KeyCode.Keypad2 };
    KeyCode[] _moveHandLeftCode   = new KeyCode[MAX_TEAM*2] { KeyCode.A, KeyCode.J, KeyCode.LeftArrow,  KeyCode.Keypad4 };
    KeyCode[] _moveHandRightCode  = new KeyCode[MAX_TEAM*2] { KeyCode.D, KeyCode.L, KeyCode.RightArrow, KeyCode.Keypad6 };


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
        if (GameSettings.GetBool("UseKeyboard"))
        {
            for (int i = 0; i < MAX_TEAM && i < Teams.Length; ++i)
            {
                if (Input.GetKeyDown(_addRamenCode[i]))
                    _helpers[i].AddNewRamen(i);

                if (Input.GetKey(_increaseTempCode[i]))
                    _helpers[i].IncreaseTemperature();

                if (Input.GetKeyDown(_grabIngredientCode[i]))
                    Teams[i].GrabIngredient();
            }

            for (int i = 0; i < 2 * MAX_TEAM && i < 2 * Teams.Length; ++i)
            {
                if (Input.GetKey(_moveHandUpCode[i]) && i < Hands.Length)
                    Hands[i].transform.Translate(Vector3.up * 0.2f);
                if (Input.GetKey(_moveHandDownCode[i]) && i < Hands.Length)
                    Hands[i].transform.Translate(Vector3.down * 0.2f);
                if (Input.GetKey(_moveHandLeftCode[i]) && i < Hands.Length)
                    Hands[i].transform.Translate(Vector3.left * 0.2f);
                if (Input.GetKey(_moveHandRightCode[i]) && i < Hands.Length)
                    Hands[i].transform.Translate(Vector3.right * 0.2f);
            }
        }
	}
}
