using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public int score;
    public float distance;
    public float health;
    public string date;

    public LeaderboardEntry(string playerName, int score, float distance, float health, string date)
    {
        this.playerName = playerName;
        this.score = score;
        this.distance = distance;
        this.health = health;
        this.date = date;
    }
}

[System.Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> performance = new List<LeaderboardEntry>();
}

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;
    public LeaderboardData leaderboardData = new LeaderboardData();
    private string jsonExportPath;
    private string jsonFilePath;   // Declared missing field
    private DatabaseService db;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        db = new DatabaseService();

        string folderPath = Path.Combine(Application.dataPath, "Data");
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
        jsonExportPath = Path.Combine(folderPath, "Leaderboardexport.json");

        jsonFilePath = Path.Combine(Application.persistentDataPath, "leaderboard.json");
    }

    public void AddEntry(string playerName, int score, float distance, float health)
    {
        string date = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        var dbEntry = new DbLeaderboardEntry
        {
            PlayerName = playerName,
            Score = score,
            Distance = distance,
            Health = health,
            Date = date
        };
        db.AddEntry(dbEntry);
        Debug.Log($"Leaderboard added to DB: {playerName}, Score {score}, Health {health:F1}, Distance {distance:F1}m");

        TrimDatabaseToTop5();
        ExportToJson();
    }

    private void TrimDatabaseToTop5()
    {
        var all = db.GetAllEntries();
        // Only trim if we have more than 5 entries
        if (all.Count <= 5) return;

        // Get top 5 (assuming db.GetAllEntries returns in descending score order, otherwise sort here)
        var top5 = all.GetRange(0, 5);
        db.ClearAll();
        foreach (var entry in top5)
        {
            db.AddEntry(entry);
        }
        Debug.Log("Database trimmed to top 5 entries.");
    }

    public List<LeaderboardEntry> GetEntries()
    {
        var dbEntries = db.GetTopEntries(5);
        // Create a new list to hold the converted entries
        List<LeaderboardEntry> result = new List<LeaderboardEntry>();
        foreach (var entry in dbEntries)  // renamed from 'db' to 'entry' to avoid conflict
        {
            result.Add(new LeaderboardEntry(entry.PlayerName, entry.Score, entry.Distance, entry.Health, entry.Date));
        }
        return result;
    }

    public void ClearLeaderboard()
    {
        db.ClearAll();
        Debug.Log("Leaderboard cleared.");
    }

    public void ExportToJson()
    {
        db.ExportToJson(jsonExportPath);
    }
}