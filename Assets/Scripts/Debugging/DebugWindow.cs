using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using ExtensionMethods;

public class DebugWindow : MonoBehaviour {

    const int MAX_OUTPUT = 3;
    const int MAX_HISTORY = 100;

    List<string> _outputs = new List<string>(MAX_OUTPUT);
    List<string> _historyCmd = new List<string>(MAX_HISTORY);
    Dictionary<string, CmdLet> _commands = new Dictionary<string, CmdLet>()
    {
        { "restart", new RestartCmdLet() },
        { "pause", new PauseCmdLet() },
        { "resume", new ResumeCmdLet() },

        { "setfloat", new SetFloatCmdLet() },
        { "setbool", new SetBoolCmdLet() },
        { "setint", new SetIntCmdLet() },
        { "setstring", new SetStringCmdLet() },
        { "getfloat", new GetFloatCmdLet() },
        { "getbool", new GetBoolCmdLet() },
        { "getint", new GetIntCmdLet() },
        { "getstring", new GetStringCmdLet() },
    };

    bool _showCursor = false;
    int _cursorPos = 0;
    int _historyPos = -1;
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

    IEnumerator toggleCursor()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            _showCursor = !_showCursor;
        }
    }

	void Start () {
        StartCoroutine("toggleCursor");
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            _isWindowShown = !_isWindowShown;
            return;
        }

        if (_isWindowShown)
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if (_cursorPos > 0)
                {
                    --_cursorPos;
                    _inputString = _inputString.Remove(_cursorPos, 1);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                //if (_inputString.Length != 0)
                //    _inputString = _inputString.Remove(_inputString.Length - 1);

            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (_cursorPos > 0)
                    --_cursorPos;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (_cursorPos < _inputString.Length)
                    ++_cursorPos;

            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                moveInHistory(-1);

            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                moveInHistory(1);

            }
            else if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                appendToOutput(_inputString);
                // put it into history
                addHistory(_inputString);

                char[] delimiters = new char[] { ' ', '\n' };
                string[] parts = _inputString.Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries);
                
                if (parts.Length > 0)
                {
                    string cmd = parts[0].ToLower();
                    if (_commands.ContainsKey(cmd))
                    {
                        string result = _commands[cmd].Execute(parts);
                        if (!string.IsNullOrEmpty(result))
                            appendToOutput(result);
                    }
                    else
                    {
                        appendToOutput("Cannot find command " + cmd);
                    }
                }
                //using System.Text.RegularExpressions;
                //string[] lines = Regex.Split(_inputString, " \t\r\n");

                // reset current input
                _inputString = "";
                _cursorPos = 0;

            }

            foreach (char c in Input.inputString)
            {
                if (!char.IsControl(c) && c != '`')
                {
                    _inputString = _inputString.Insert(_cursorPos, c.ToString());
                    ++_cursorPos;
                }
            }
        }
	}

    void DoWindow(int windowID)
    {

    }

    public Rect windowRect = new Rect(0, 0, Screen.width, 120);
    GUIStyle customLabelStyle;
    void OnGUI()
    {
        if (customLabelStyle == null)
        {
            customLabelStyle = new GUIStyle(GUI.skin.label);
            customLabelStyle.fontSize = 20;
            customLabelStyle.normal.textColor = Color.white;
        }

        //doWindow0 = GUI.Toggle(windowRect, doWindow0, "Window 0");
        if (_isWindowShown)
        {
            windowRect = GUI.Window(0, windowRect, DoWindow, "Debug Window");
            for (int i = 0; i < _outputs.Count; ++i)
            {
                GUI.Label(new Rect(10, 20*i, 1000, 50), _outputs[i], customLabelStyle);
                //GUI.TextArea(new Rect(10, 20 * i, 100, 20), _outputs[i]);
            }
            GUI.Label(new Rect(10, 20 * _outputs.Count, 1000, 50), _inputString.Insert(_cursorPos, (_showCursor ? "|" : "")), customLabelStyle);
        }
    }

    void appendToOutput(string str)
    {
        if (_outputs.Count >= MAX_OUTPUT)
            _outputs.RemoveAt(0);
        _outputs.Add(str);
    }

    void moveInHistory(int move)
    {
        int newPos = _historyPos + move;
        if (newPos < 0)
        {
            if (_historyPos == 0)
                ;// error
            _historyPos = 0;
        }
        else if (newPos >= _historyCmd.Count)
        {
            if (_historyPos >= _historyCmd.Count - 1)
                ;// error
            _historyPos = _historyCmd.Count;
        }
        else
            _historyPos = newPos;

        if (_historyPos == _historyCmd.Count)
        {
            // current tmp
            _inputString = "";
            _cursorPos = 0;
        }
        else
        {
            _inputString = _historyCmd[_historyPos];
            _cursorPos = _inputString.Length;
        }
    }

    void addHistory(string cmd)
    {
        if (_historyCmd.Count >= MAX_HISTORY)
            _historyCmd.RemoveAt(0);
        _historyCmd.Add(cmd);
        _historyPos = _historyCmd.Count;
    }


}
