using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LeaderboardController : Leaderboard, MonoBehaviour
{
    void OnApplicationQuit()
    {
        saveFile();
    }
}

[System.Serializable()]
public struct LeaderboardEntry
{
    public string Player1Name;
    public string Player2Name;
    public int Score;
}

public class Leaderboard {
    static readonly Leaderboard _instance = new Leaderboard();

    public static Leaderboard Instance
    {
        get 
        {
            return _instance;
        }
    }

    const int MAX_ENTRY = 10;
    readonly string FILE_DIR = Application.persistentDataPath + "/";
    const string FILE_NAME = "leaderboard.bin";
    readonly string FILE_PATH;
    List<LeaderboardEntry> _leaderBoard = new List<LeaderboardEntry>();

    private Leaderboard()
    {
        FILE_PATH = FILE_DIR + "/" + FILE_NAME;
        //FILE_PATH = Path.Combine(Application.persistentDataPath, FILE_NAME);
        Debug.Log(FILE_PATH);
        readFile();
    }

    int addEntry(LeaderboardEntry entry)
    {
        int rank = 0;
        for (rank = 0; rank < _leaderBoard.Count; ++rank)
            if (entry.Score > _leaderBoard[rank].Score)
                break;

        _leaderBoard.Insert(rank, entry);
        if (_leaderBoard.Count >= MAX_ENTRY)
            _leaderBoard.RemoveRange(MAX_ENTRY, _leaderBoard.Count - MAX_ENTRY);

        return rank;
    }

    /// <summary>
    /// Add an entry into the leaderboard.
    /// </summary>
    /// <param name="entry"></param>
    /// <returns>The rank of entry</returns>
    public int[] AddEntries(LeaderboardEntry[] entries)
    {
        int[] ranks = entries
            .Select((x, i) => new { Index = i, Entry = x })
            .OrderByDescending(x => x.Entry.Score)
            .Select(x => new { Index = x.Index, Rank = addEntry(x.Entry) })
            .OrderBy(x => x.Index)
            .Select(x => x.Rank)
            .ToArray();

        saveFile();
        return ranks;

        /*
        int[] ranks = new int[entries.Length];
        var sorted = entries
            .Select((x, i) => new { Index = i, Entry = x })
            .OrderByDescending(x => x.Entry.Score)
            .ToArray();
        foreach (var i in sorted)
            ranks[i.Index] = AddEntry(i.Entry);
        
        saveFile();
        return ranks;
        */
    }

    public LeaderboardEntry[] GetEntries()
    {
        return _leaderBoard.ToArray();
    }

    public void Print()
    {
        for (int i = 0; i < _leaderBoard.Count; ++i)
            Debug.Log(string.Format("{0}: Score = {1}", i, _leaderBoard[i].Score));
    }

    protected void saveFile()
    {
        try
        {
            using (Stream stream = File.Open(FILE_PATH, FileMode.Create))
            {
                BinaryFormatter bin = new BinaryFormatter();
                bin.Serialize(stream, _leaderBoard);
            }
        }
        catch (IOException)
        {
        }
    }

    void readFile()
    {
        try
        {
            using (Stream stream = File.Open(FILE_PATH, FileMode.Open))
            {
                BinaryFormatter bin = new BinaryFormatter();
                _leaderBoard = (List<LeaderboardEntry>)bin.Deserialize(stream);
            }
        }
        catch (IOException)
        {
        }
    }
}
