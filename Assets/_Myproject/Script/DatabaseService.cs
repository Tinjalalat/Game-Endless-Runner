using SQLite;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Table("leaderboard")]

public class DbLeaderboardEntry
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Column("player_name")]
    public string PlayerName { get; set; }

    [Column("score")]
    public int Score { get; set; }

    [Column("distance")]
    public float Distance { get; set; }

    [Column("health")]
    public float Health { get; set; }

    [Column("date")]
    public string Date { get; set; }
}

public class DatabaseService
{
    private SQLiteConnection _connection;
    private string _dbPath;

    public DatabaseService()
    {
        string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        _dbPath = Path.Combine(desktopPath, "leaderboard.db");


        _connection = new SQLiteConnection(_dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        _connection.CreateTable<DbLeaderboardEntry>();
        Debug.Log($"Database initialized at: {_dbPath}");
    }

    public void AddEntry(DbLeaderboardEntry entry)
    {
        _connection.Insert(entry);
    }

    public List<DbLeaderboardEntry> GetAllEntries()
    {
        var entries = _connection.Table<DbLeaderboardEntry>().ToList();
        entries.Sort((a, b) =>
        {
            if (a.Health != b.Health)
                return b.Health.CompareTo(a.Health);
            else
                return a.Distance.CompareTo(b.Distance);
        });
        return entries;
    }


    public List<DbLeaderboardEntry> GetTopEntries(int limit = 5)
    {
        var all = GetAllEntries();
        if (all.Count > limit)
            all = all.GetRange(0, limit);
        return all;
    }

    public void ClearAll()
    {
        _connection.DeleteAll<DbLeaderboardEntry>();
    }

    public void ExportToJson(string jsonFilePath)
    {
        var dbEntries = GetAllEntries();
        var exportList = new List<LeaderboardEntry>();
        foreach (var db in dbEntries)
        {
            exportList.Add(new LeaderboardEntry(db.PlayerName, db.Score, db.Distance, db.Health, db.Date));

        }
        var wrapper = new LeaderboardData { performance = exportList };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(jsonFilePath, json);
        Debug.Log($"Exported {exportList.Count} entries to JSON: {jsonFilePath}");
    }

    public void ImportFromJson(string jsonFilePath)
    {
        if (File.Exists(jsonFilePath))
        {
            string json = File.ReadAllText(jsonFilePath);
            var data = JsonUtility.FromJson<LeaderboardData>(json);
            if (data != null && data.performance !=null)
            {
                ClearAll();
                foreach(var entry in data.performance)
                {
                    var dbEntry = new DbLeaderboardEntry
                    {
                        PlayerName = entry.playerName,
                        Score = entry.score,
                        Distance = entry.distance,
                        Health = entry.health,
                        Date = entry.date
                    };
                    AddEntry(dbEntry);
                }
                Debug.Log($"Imported {data.performance.Count} entries from JSON.");
            }
        }
        else
        {
            Debug.LogWarning("JSON file not found for import.");
        }
    }
}