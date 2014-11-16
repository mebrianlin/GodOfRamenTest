using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DebugWindow : MonoBehaviour {

    Dictionary<string, string> _commands = new Dictionary<string, string>();
    bool _isWindowShown = false;
    string _inputString = "";

    public void Show()
    {
        _isWindowShown = true;
    }

    public void Hide()
    {
        _isWindowShown = false;
    }

	void Start () {
	
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            _isWindowShown = !_isWindowShown;
        }
        _inputString = Input.inputString;
	}

    void DoWindow(int windowID)
    {

    }

    void OnGUI()
    {
        //doWindow0 = GUI.Toggle(windowRect, doWindow0, "Window 0");
        if (_isWindowShown)
        {
            windowRect = GUI.Window(0, windowRect, DoWindow, "Basic Window");
            if (GUI.Button(new Rect(10, 20, 100, 20), _inputString))
                print("Got a click");
        }

    }

    public Rect windowRect = new Rect(0, 0, Screen.width, 120);
}


public static class GameSettings
{
    static Dictionary<string, float> _floatTable = new Dictionary<string, float>()
    {
    };
    static Dictionary<string, bool> _boolTable = new Dictionary<string, bool>()
    {
        { "DebugMode", true },
        { "UseKeyboard", true },
        { "UsePhidget", false },
    };
    static Dictionary<string, int> _intTable = new Dictionary<string, int>()
    { 
        { "TimePerRound", 30 },
        { "MaxNoodleHitCount", 4 },
    };
    static Dictionary<string, string> _stringTable = new Dictionary<string, string>();

    public static bool GetBool(string s)
    {
        if (_boolTable.ContainsKey(s))
            return _boolTable[s];

        Debug.LogError("Cannot find variable \"" + s + "\"");
        return false;
    }

    public static void SetBool(string s, bool b)
    {
        _boolTable[s] = b;
    }

    public static int GetInt(string s)
    {
        if (_intTable.ContainsKey(s))
            return _intTable[s];

        Debug.LogError("Cannot find variable \"" + s + "\"");
        return 0;
    }

    public static void Setint(string s, int i)
    {
        _intTable[s] = i;
    }

    public static float GetFloat(string s)
    {
        if (_floatTable.ContainsKey(s))
            return _floatTable[s];

        Debug.LogError("Cannot find variable \"" + s + "\"");
        return 0;
    }

    public static void SetFloat(string s, float f)
    {
        _floatTable[s] = f;
    }
}

