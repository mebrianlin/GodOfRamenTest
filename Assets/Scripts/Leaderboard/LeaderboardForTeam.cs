﻿using UnityEngine;
using System.Collections;

using ExtensionMethods;

public class LeaderboardForTeam : MonoBehaviour {

    const int NUM_SHOWN = 6;
    float itemHeight = 0f;

	void Start () {
	    
	}

	void Update () {
	
	}

    public void Show(LeaderboardInsertResult insertResult)
    {
        int highlightIndex = insertResult.Index;

        Leaderboard board = Leaderboard.Instance;
        LeaderboardEntry[] entry = board.GetEntries();
        if (highlightIndex >= entry.Length)
            highlightIndex = entry.Length - 1;


        string prefabFilePath = "Prefabs/LeaderboardEntry";
        for (int i = 0; i < NUM_SHOWN && i < entry.Length; ++i)
        {
            GameObject entryObject = UnityEngine.Object.Instantiate(
                   Resources.Load(prefabFilePath, typeof(GameObject))) as GameObject;
            entryObject.transform.parent = this.transform;

            if (itemHeight == 0)
            {
                foreach (var t in entryObject.GetComponentsInChildren<TextMesh>())
                    itemHeight = Mathf.Max(itemHeight, t.renderer.bounds.extents.y * 2);
            }

            LeaderboardEntryScript script = entryObject.GetComponent<LeaderboardEntryScript>();

            if (i == NUM_SHOWN - 1 && highlightIndex >= NUM_SHOWN)
            {
                Debug.Log(highlightIndex);
                script.Entry = entry[highlightIndex];
                script.IsFocus = true;
            }
            else
            {
                script.Entry = entry[i];
                script.IsFocus = (i == highlightIndex);
            }
            //entryObject.transform.position = new Vector3(0, -3 * i, 0);
            entryObject.transform.localPosition = new Vector3(0, -itemHeight * i, -1);
        }

        GameObject creditScene = this.gameObject.FindObjectWithTagInChildren("Leaderboard");
        creditScene.GetComponent<MeshRenderer>().enabled = true;
        CreditControl creditControl = creditScene.GetComponent<CreditControl>();
        creditControl.PlayCredit();
        //this.gameObject.FindObjectWithTagInChildren("Leaderboard").GetComponent<SpriteRenderer>().enabled = true;


    }

    public void Hide()
    {
        throw new System.NotImplementedException("Hide function not implemented yet");
    }
}
