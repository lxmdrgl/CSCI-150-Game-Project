using UnityEngine;
using System.IO;
public static class SaveSystem
{
    private static readonly string playerPosFile = Application.persistentDataPath + "/playerPosition.json";
    private static readonly string playerStatsFile = Application.persistentDataPath + "/playerStats.json";

    // Struct to represent the position data
    [System.Serializable]
    public struct PositionData
    {
        public float x, y, z;

        public PositionData(Vector3 position)
        {
            x = position.x;
            y = position.y;
            z = position.z;
        }

        public Vector3 ToVector3() => new Vector3(x, y, z);
    }
    public static void SaveStats(PlayerStats stats)
    {
        string json = JsonUtility.ToJson(stats);
        File.WriteAllText(playerStatsFile, json);
        Debug.Log($"Position saved to {playerStatsFile}");
    }
    // Load the stats from JSON
    public static PlayerStats LoadStats()
    {
        if (File.Exists(playerStatsFile))
        {
            string json = File.ReadAllText(playerStatsFile);
            PlayerStats stats = JsonUtility.FromJson<PlayerStats>(json);
            Debug.Log("Stats loaded: " + stats.StatString());
            return stats;
        }
        else
        {
            Debug.LogWarning("Save file not found. Returning default stats.");
            return new PlayerStats();
        }
    }
    // Save the position to JSON
    public static void SavePosition(Vector3 position)
    {
        PositionData data = new PositionData(position);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(playerPosFile, json);
        Debug.Log($"Position saved to {playerPosFile}");
    }

    // Load the position from JSON
    public static Vector3 LoadPosition()
    {
        if (File.Exists(playerPosFile))
        {
            string json = File.ReadAllText(playerPosFile);
            PositionData data = JsonUtility.FromJson<PositionData>(json);
            Debug.Log("Position loaded: " + data.ToVector3());
            return data.ToVector3();
        }
        else
        {
            Debug.LogWarning("Save file not found. Returning default position (0,0,0).");
            return Vector3.zero;
        }

    }
}
