using UnityEngine;
using System.Collections.Generic;


public static class GameSettings
{
    public delegate void IntValueChanged(string name, int i);
    public delegate void BoolValueChanged(string name, bool b);
    public delegate void FloatValueChanged(string name, float f);
    public delegate void StringValueChanged(string name, string s);

    public static event IntValueChanged OnIntValueChange;
    public static event BoolValueChanged OnBoolValueChange;
    public static event FloatValueChanged OnFloatValueChange;
    public static event StringValueChanged OnStringValueChange;

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
        { "TimePerRound", 10 },
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
        if (OnBoolValueChange != null)
            OnBoolValueChange(s, b);
    }

    public static int GetInt(string s)
    {
        if (_intTable.ContainsKey(s))
            return _intTable[s];

        Debug.LogError("Cannot find variable \"" + s + "\"");
        return 0;
    }

    public static void SetInt(string s, int i)
    {
        _intTable[s] = i;
        if (OnIntValueChange != null)
            OnIntValueChange(s, i);
    }

    public static float GetFloat(string s)
    {
        if (_floatTable.ContainsKey(s))
            return _floatTable[s];

        throw new KeyNotFoundException("Cannot find variable \"" + s + "\"");
        Debug.LogError("Cannot find variable \"" + s + "\"");
        return 0;
    }

    public static void SetFloat(string s, float f)
    {
        _floatTable[s] = f;
        if (OnFloatValueChange != null)
            OnFloatValueChange(s, f);
    }

    public static string GetString(string s)
    {
        if (_stringTable.ContainsKey(s))
            return _stringTable[s];

        Debug.LogError("Cannot find variable \"" + s + "\"");
        return "";
    }

    public static void SetString(string s, string str)
    {
        _stringTable[s] = str;
        if (OnStringValueChange != null)
            OnStringValueChange(s, str);
    }
}

