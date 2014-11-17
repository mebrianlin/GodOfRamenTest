using UnityEngine;

public abstract class CmdLet
{
    int _numParam;

    public CmdLet(int numParam)
    {
        _numParam = numParam;
    }

    public string Execute(string[] parameters)
    {
        if (parameters.Length == _numParam + 1)
            return execute(parameters);
        else
            ;
        return "";
    }

    protected abstract string execute(string[] parameters);
}

public abstract class BuiltInCmdLet : CmdLet
{
    public BuiltInCmdLet()
        : base(0)
    { }
}

public class RestartCmdLet : BuiltInCmdLet
{
    protected override string execute(string[] parameters)
    {
        Application.LoadLevel(Application.loadedLevelName);
        return "";
    }
}

public class PauseCmdLet : BuiltInCmdLet
{
    protected override string execute(string[] parameters)
    {
        GameSettings.SetBool("Pause", true);
        return "";
    }
}

public class ResumeCmdLet : BuiltInCmdLet
{
    protected override string execute(string[] parameters)
    {
        GameSettings.SetBool("Pause", false);
        return "";
    }
}

public class GetFloatCmdLet : CmdLet
{
    public GetFloatCmdLet()
        : base(1)
    { }

    protected override string execute(string[] parameters)
    {
        return GameSettings.GetFloat(parameters[1]).ToString();
    }
}

public class SetFloatCmdLet : CmdLet
{
    public SetFloatCmdLet()
        : base(2)
    { }

    protected override string execute(string[] parameters)
    {
        // GameSettings.SetFloat(parameters[1], float.Parse(parameters[2], System.Globalization.CultureInfo.InvariantCulture.NumberFormat));
        try
        {
            float f = System.Convert.ToSingle(parameters[2]);
            GameSettings.SetFloat(parameters[1], f);
        }
        catch (System.Exception e)
        {
            //
        }
        return "";
    }
}

public class GetIntCmdLet : CmdLet
{
    public GetIntCmdLet()
        : base(1)
    {
    }

    protected override string execute(string[] parameters)
    {
        return GameSettings.GetInt(parameters[1]).ToString();
    }
}

public class SetIntCmdLet : CmdLet
{
    public SetIntCmdLet()
        : base(2)
    { }

    protected override string execute(string[] parameters)
    {
        try
        {
            int i = System.Convert.ToInt32(parameters[2]);
            GameSettings.SetInt(parameters[1], i);
        }
        catch (System.Exception e)
        {
            //
        }
        return "";
    }
}

public class GetBoolCmdLet : CmdLet
{
    public GetBoolCmdLet()
        : base(1)
    { }

    protected override string execute(string[] parameters)
    {
        return GameSettings.GetBool(parameters[1]).ToString();
    }
}

public class SetBoolCmdLet : CmdLet
{
    public SetBoolCmdLet()
        : base(2)
    { }

    protected override string execute(string[] parameters)
    {
        try
        {
            bool b = System.Convert.ToBoolean(parameters[2]);
            GameSettings.SetBool(parameters[1], b);
        }
        catch (System.Exception e)
        {
            //
        }
        return "";
    }
}

public class GetStringCmdLet : CmdLet
{
    public GetStringCmdLet()
        : base(1)
    { }

    protected override string execute(string[] parameters)
    {
        return GameSettings.GetString(parameters[1]).ToString();
    }
}

public class SetStringCmdLet : CmdLet
{
    public SetStringCmdLet()
        : base(2)
    { }

    protected override string execute(string[] parameters)
    {
        GameSettings.SetString(parameters[1], parameters[2]);
        return "";
    }
}