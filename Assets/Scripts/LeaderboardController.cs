﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LeaderboardController : MonoBehaviour
{
    void OnApplicationQuit()
    {
        Leaderboard.Instance.Save();
    }
}

[System.Serializable()]
public struct LeaderboardEntry
{
    public string Player1Name;
    public string Player2Name;
    public int Score;
}

public class LeaderboardEntryComparer : IComparer<LeaderboardEntry>
{
    public int Compare(LeaderboardEntry lhs, LeaderboardEntry rhs)
    {
        return rhs.Score - lhs.Score;
    }
}

public sealed class Leaderboard {
    static readonly Leaderboard _instance = new Leaderboard();

    public static Leaderboard Instance
    {
        get 
        {
            return _instance;
        }
    }

    const int MAX_ENTRY = 100;
    readonly string FILE_DIR = Application.persistentDataPath;
    const string FILE_NAME = "leaderboard.bin";
    readonly string _filePath;
    List<LeaderboardEntry> _leaderBoard = new List<LeaderboardEntry>();

    //IComparer<LeaderboardEntry> c = new LeaderboardEntryComparer();
    //SortedList<LeaderboardEntry, int> _sortedLeaderBoard = 
    //    new SortedList<LeaderboardEntry, int>(new LeaderboardEntryComparer());

    private Leaderboard()
    {
        _filePath = Path.Combine(FILE_DIR, FILE_NAME);
        readFile();
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
    public int[] AddEntries(LeaderboardEntry[] entries)
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
        
        int[] ranks = new int[entries.Length];
        var sorted = entries
            .Select((x, i) => new { Index = i, Entry = x })
            .OrderByDescending(x => x.Entry.Score)
            .ToArray();

        int lastIndex = 0;
        foreach (var i in sorted)
            lastIndex = ranks[i.Index] = addEntry(i.Entry, lastIndex);
        
        saveFile();
        return ranks;
        
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