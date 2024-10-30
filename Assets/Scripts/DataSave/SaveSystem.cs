using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Unity.VisualScripting;
public static class SaveSystem
{
    private static readonly string savePath = Application.persistentDataPath + "/saveSlot{0}.json";

    [System.Serializable]
    public class SaveData
    {
        public SaveSystem.PositionData position;
        public PlayerStats stats;
        public float playTime;
        public List<string> unlockedCharacters;

        public SaveData(Vector3 position, PlayerStats stats, float playTime, List<string> unlockedCharacters)
        {
            this.position = new SaveSystem.PositionData(position);
            this.stats = stats;
            this.playTime = playTime;
            this.unlockedCharacters = unlockedCharacters;
        }
    }

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

    public static void SaveGame(int slot, Vector3 position, PlayerStats stats, float playTime, List<string> unlockedCharacters)
    {
        SaveData data = new SaveData(position, stats, playTime, unlockedCharacters);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(string.Format(savePath, slot), json);
        Debug.Log($"Game saved to slot {slot}");
        Debug.Log($"Game saved at" + savePath);
    }

    public static SaveData LoadGame(int slot)
    {
        string path = string.Format(savePath, slot);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            Debug.Log($"Save loaded from slot {slot}");
            return data;
        }
        else
        {
            Debug.Log($"Save slot {slot} not found.");
            return null;
        }
    }

    public static bool SaveExists(int slot)
    {
        return File.Exists(string.Format(savePath, slot));
    }
    
    public static SaveData InitializeDefaultSave(int slot)
    {
        Vector3 defaultPosition = Vector3.zero;  // Default starting position
        PlayerStats defaultStats = new PlayerStats();  // Default player stats
        float defaultPlayTime = 0f;  // Default playtime
        List<string> defaultUnlockedCharacters = new List<string>();  // No characters unlocked by default
        defaultUnlockedCharacters.Add("Knight");

        SaveData defaultData = new SaveData(defaultPosition, defaultStats, defaultPlayTime, defaultUnlockedCharacters);
        SaveGame(slot, defaultPosition, defaultStats, defaultPlayTime, defaultUnlockedCharacters);  // Save the default data

        return defaultData;
    }

    public static void DeleteSave(int slot)
    {
        string path = string.Format(savePath, slot);

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"Save slot {slot} has been deleted.");
        }
        else
        {
            Debug.LogWarning($"Save slot {slot} does not exist and cannot be deleted.");
        }
    }
}
