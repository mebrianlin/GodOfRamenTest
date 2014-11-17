using UnityEngine;
using System.Collections;

public class KeyboardSimulator : MonoBehaviour {

    const int MAX_TEAM = 2;
    public RamenTeam[] Teams;

	public GameObject[] Hands;

    Apprentice[] _apprentices;
    Helper[] _helpers;
    Emcee _emcee;

    bool _leftAltDown = false;
    bool _rightAltDown = false;
    bool _paused = false;

    KeyCode[] _addRamenCode       = new KeyCode[MAX_TEAM]   { KeyCode.LeftShift,    KeyCode.RightShift };
    KeyCode[] _increaseTempCode   = new KeyCode[MAX_TEAM]   { KeyCode.LeftControl,  KeyCode.RightControl };
    KeyCode[] _grabIngredientCode = new KeyCode[MAX_TEAM]   { KeyCode.LeftAlt,      KeyCode.RightAlt };
	KeyCode[] _moveHandUpCode     = new KeyCode[MAX_TEAM*2] { KeyCode.W, KeyCode.I, KeyCode.UpArrow,    KeyCode.Keypad8 };
	KeyCode[] _moveHandDownCode   = new KeyCode[MAX_TEAM*2] { KeyCode.S, KeyCode.K, KeyCode.DownArrow,  KeyCode.Keypad2 };
    KeyCode[] _moveHandLeftCode   = new KeyCode[MAX_TEAM*2] { KeyCode.A, KeyCode.J, KeyCode.LeftArrow,  KeyCode.Keypad4 };
    KeyCode[] _moveHandRightCode  = new KeyCode[MAX_TEAM*2] { KeyCode.D, KeyCode.L, KeyCode.RightArrow, KeyCode.Keypad6 };


    void Awake()
    {
        GameSettings.OnBoolValueChange += GameSettings_OnBoolValueChange;
    }

    void GameSettings_OnBoolValueChange(string name, bool b)
    {
        if (name == "Pause")
            _paused = b;
    }

	void Start () {
        if (Teams.Length > MAX_TEAM)
            Debug.LogError("Too many teams");

        _apprentices = new Apprentice[Teams.Length];
        _helpers = new Helper[Teams.Length];
        _emcee = GameObject.FindGameObjectWithTag("Emcee").GetComponent<Emcee>();

        for (int i = 0; i < Teams.Length; ++i)
        {
            RamenTeam team = Teams[i];
            _apprentices[i] = team.GetComponentInChildren<Apprentice>();
            _helpers[i] = team.GetComponentInChildren<Helper>();
        }

	}

    void Update() {
        if (_paused)
            return;

        if (GameSettings.GetBool("UseKeyboard"))
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt))
                _leftAltDown = true;
            if (Input.GetKeyUp(KeyCode.LeftAlt))
                _leftAltDown = false;
            if (Input.GetKeyDown(KeyCode.RightAlt))
                _rightAltDown = true;
            if (Input.GetKeyUp(KeyCode.RightAlt))
                _rightAltDown = false;

            for (int i = 0; i < MAX_TEAM && i < Teams.Length; ++i)
            {
                if (Input.GetKeyDown(_addRamenCode[i]))
                    _helpers[i].AddNewRamen(i);

                if (Input.GetKey(_increaseTempCode[i]))
                    _helpers[i].IncreaseTemperature();

                if (Input.GetKeyDown(_grabIngredientCode[i]))
                    Teams[i].GrabIngredient();
            }

			if(Input.GetMouseButtonDown(0)){
				Teams[0].GrabIngredient();
			}

			if(Input.GetMouseButtonDown(1)){
				Teams[1].GrabIngredient();
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

            bool altDown = _leftAltDown || _rightAltDown;
            if (Input.GetKey(KeyCode.R) && altDown)
            {
                _emcee.Restart();
            }
        }
	}
}
