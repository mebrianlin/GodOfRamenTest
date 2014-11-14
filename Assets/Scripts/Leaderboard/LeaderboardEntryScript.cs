using UnityEngine;
using System.Collections;
using System.Linq;

public class LeaderboardEntryScript : MonoBehaviour {

    public GameObject Background;
    public GameObject Names;
    public GameObject Score;

    Texture[] Textures;

    public bool IsFocus
    {
        set
        {
            Background.GetComponent<MeshRenderer>().materials[0].mainTexture = Textures[(value ? 0 : 1)];
        }
    }

    public LeaderboardEntry Entry
    {
        set
        {
            Names.GetComponent<TextMesh>().text = value.Player1Name + " & " + value.Player2Name;
            Score.GetComponent<TextMesh>().text = value.Score.ToString();
        }
    }

	void Awake () {
        Names.GetComponent<TextMesh>().text = "";
        Score.GetComponent<TextMesh>().text = "";

        Textures = Background.GetComponent<MeshRenderer>().materials.Select(x => x.mainTexture).ToArray();
	}

	void Update () {

	}
}
