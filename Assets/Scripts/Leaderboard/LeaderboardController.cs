using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LeaderboardController : MonoBehaviour
{
    static bool _saved = false;
    const int NUM_SHOWN = 10;

    void Start()
    {
        /*
        Leaderboard.Instance.AddEntries(new LeaderboardEntry[]
        {
            new LeaderboardEntry { Player1Name = "a", Player2Name = "b", Score = 20 },
            new LeaderboardEntry { Player1Name = "a", Player2Name = "b", Score = 21 },
            new LeaderboardEntry { Player1Name = "a", Player2Name = "b", Score = 22 },
            new LeaderboardEntry { Player1Name = "a", Player2Name = "b", Score = 23 },
            new LeaderboardEntry { Player1Name = "a", Player2Name = "b", Score = 24 },
            new LeaderboardEntry { Player1Name = "a", Player2Name = "b", Score = 25 },
            new LeaderboardEntry { Player1Name = "a", Player2Name = "b", Score = 26 },
            new LeaderboardEntry { Player1Name = "a", Player2Name = "b", Score = 27 },
            new LeaderboardEntry { Player1Name = "a", Player2Name = "b", Score = 28 },
            new LeaderboardEntry { Player1Name = "a", Player2Name = "b", Score = 29 },
        });
        int[] ranks = Leaderboard.Instance.AddEntries(new LeaderboardEntry[]
        {
            new LeaderboardEntry { Player1Name = "a", Player2Name = "b", Score = 30 },
            new LeaderboardEntry { Player1Name = "a", Player2Name = "b", Score = 25 },
        });*/
    }

    void OnApplicationQuit()
    {
        if (!_saved)
        {
            _saved = true;
            Leaderboard.Instance.Save();
        }
    }
}

[System.Serializable()]
public struct LeaderboardEntry
{
    public string Player1Name;
    public string Player2Name;
    public int Score;
}

public struct LeaderboardInsertResult
{
    public int Index;
    public int Rank;
    public static LeaderboardInsertResult Default = new LeaderboardInsertResult { Index = -1, Rank = -1 };
}

public class LeaderboardEntryComparer : IComparer<LeaderboardEntry>
{
    public int Compare(LeaderboardEntry lhs, LeaderboardEntry rhs)
    {
        return rhs.Score - lhs.Score;
    }
}

public sealed class Leaderboard {
    static Leaderboard _instance;

    public static Leaderboard Instance
    {
        get 
        {
			if (string.IsNullOrEmpty(FILE_DIR)) {
				FILE_DIR = Application.persistentDataPath;;
				_filePath = Path.Combine(FILE_DIR, FILE_NAME);
			}
			if (_instance == null)
				_instance = new Leaderboard();

            return _instance;
        }
    }

	const int MAX_ENTRY = 100;
	static string FILE_DIR;
    const string FILE_NAME = "leaderboard.bin";
    static string _filePath;
    List<LeaderboardEntry> _leaderBoard = new List<LeaderboardEntry>();

    //IComparer<LeaderboardEntry> c = new LeaderboardEntryComparer();
    //SortedList<LeaderboardEntry, int> _sortedLeaderBoard = 
    //    new SortedList<LeaderboardEntry, int>(new LeaderboardEntryComparer());

    private Leaderboard()
    {
        readFile();
        Debug.Log(_filePath);
    }

    int addEntry(LeaderboardEntry entry, int hint = 0)
    {
        int rank = 0;
        if (hint < _leaderBoard.Count && _leaderBoard[hint].Score > entry.Score)
            rank = hint;

        for (; rank < _leaderBoard.Count; ++rank)
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
    public LeaderboardInsertResult[] AddEntries(LeaderboardEntry[] entries)
    {
        /*
        int[] ranks = entries
            .Select((x, i) => new { Index = i, Entry = x })
            .OrderByDescending(x => x.Entry.Score)
            .Select(x => new { Index = x.Index, Rank = addEntry(x.Entry) })
            .OrderBy(x => x.Index)
            .Select(x => x.Rank)
            .ToArray();

        return ranks;
        */

        LeaderboardInsertResult[] result = new LeaderboardInsertResult[entries.Length];

        int[] positions = new int[entries.Length];
        var sortedEntries = entries
            .Select((x, i) => new { Index = i, Entry = x })
            .OrderByDescending(x => x.Entry.Score)
            .ToArray();

        int lastIndex = 0;
        // add the entries first
        foreach (var i in sortedEntries)
            lastIndex = result[i.Index].Index = addEntry(i.Entry, lastIndex);

        // then get the ranks
        int[] ranks = new int[_leaderBoard.Count];
        ranks[0] = 0;
        for (int i = 1; i < _leaderBoard.Count; ++i)
            ranks[i] = ranks[i - 1] + (_leaderBoard[i].Score == _leaderBoard[i - 1].Score ? 0 : 1);

        foreach (var i in sortedEntries)
            result[i.Index].Rank = ranks[i.Index];

        saveFile();
        return result;
        
    }

    public LeaderboardEntry[] GetEntries()
    {
        return _leaderBoard.ToArray();
    }

    public void Clear()
    {
        _leaderBoard.Clear();
    }

    public void Print()
    {
        for (int i = 0; i < _leaderBoard.Count; ++i)
            Debug.Log(string.Format("{0}: Score = {1}", i, _leaderBoard[i].Score));
    }

    public void Save() {
        saveFile();
    }

    void saveFile()
    {
        try
        {
            using (Stream stream = File.Open(_filePath, FileMode.Create))
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
            using (Stream stream = File.Open(_filePath, FileMode.Open))
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
