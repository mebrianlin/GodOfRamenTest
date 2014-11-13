using UnityEngine;
using System.Collections;

public class DebugWindow : MonoBehaviour {

    public void Show()
    {
        doWindow = true;
    }

    public void Hide()
    {
        doWindow = false;
    }

	void Start () {
	
	}
	
	void Update () {
	
	}

    public bool doWindow = true;

    void DoWindow(int windowID)
    {

    }

    void OnGUI()
    {
        //doWindow0 = GUI.Toggle(windowRect, doWindow0, "Window 0");
        if (doWindow)
        {
            windowRect = GUI.Window(0, windowRect, DoWindow, "Basic Window");
            if (GUI.Button(new Rect(10, 20, 100, 20), "Hello World"))
                print("Got a click");
        }

    }

    public Rect windowRect = new Rect(0, 0, Screen.width, 120);
}
