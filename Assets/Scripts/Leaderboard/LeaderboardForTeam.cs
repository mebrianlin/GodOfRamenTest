﻿using UnityEngine;
using System.Collections;

public class LeaderboardForTeam : MonoBehaviour {

    const int NUM_SHOWN = 10;

	void Start () {
	
	}

	void Update () {
	
	}

    public void Show(int highlightIndex)
    {
        Leaderboard board = Leaderboard.Instance;
        LeaderboardEntry[] entry = board.GetEntries();

        string prefabFilePath = "Prefabs/LeaderboardEntry";
        for (int i = 0; i < NUM_SHOWN; ++i)
        {
            GameObject entryObject = UnityEngine.Object.Instantiate(
                   Resources.Load(prefabFilePath, typeof(GameObject)) as GameObject) as GameObject;
            entryObject.transform.parent = this.transform;

            LeaderboardEntryScript script = entryObject.GetComponent<LeaderboardEntryScript>();

            if (i == NUM_SHOWN - 1 && highlightIndex >= NUM_SHOWN)
            {
                script.Entry = entry[highlightIndex];
                script.IsFocus = true;
            }
            else
            {
                script.Entry = entry[i];
                script.IsFocus = (i == highlightIndex);
            }
            //entryObject.transform.position = new Vector3(0, -3 * i, 0);
            entryObject.transform.localPosition = new Vector3(0, -3 * i, 0);
        }
    }

    public void Hide()
    {
        throw new System.NotImplementedException("Hide function not implemented yet");
    }
}
