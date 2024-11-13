using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Unity.VisualScripting;
using Game.CoreSystem.StatsSystem;
using Game.Weapons.Components;
using Game.Weapons; 

namespace Game.CoreSystem
{
public static class SaveSystem
{
    private static readonly string savePath = Application.persistentDataPath + "/saveSlot{0}.json";

    [System.Serializable]
    public class SaveData
    {
        public SaveSystem.PositionData position;
        public float Currenthealth;
        public float MaxHealth;
        public float playTime;
        public List<string> unlockedCharacters;
        public List<EnemyData> enemies;  // New list to save enemy data
        public List<WeaponData> weaponInventory;  // New list to save weapon inventory data

    public SaveData(Vector3 position, float Currenthealth, float MaxHealth, float playTime, List<string> unlockedCharacters, List<EnemyData> enemies,List<WeaponData> weaponInventory)
    {
        this.position = new SaveSystem.PositionData(position);
        this.Currenthealth = Currenthealth;
        this.MaxHealth = MaxHealth;
        this.playTime = playTime;
        this.unlockedCharacters = unlockedCharacters;
        this.enemies = enemies;
        this.weaponInventory = weaponInventory;
    }
    }
    [System.Serializable]
    public struct EnemyData
    {
        public PositionData position;
        public bool isAlive;

        public EnemyData(Vector3 position, bool isAlive)
        {
            this.position = new PositionData(position);
            this.isAlive = isAlive;
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

    public static void SaveGame(int slot, Vector3 position, float Currenthealth, float MaxHealth, float playTime, List<string> unlockedCharacters,List<EnemyData> enemies, List<WeaponData> weaponInventory)
    {
        SaveData data = new SaveData(position, Currenthealth, MaxHealth, playTime, unlockedCharacters,enemies, weaponInventory);
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
        Vector3 defaultPosition = Vector3.zero;  // Default player starting position
        float defaultPlayTime = 0f;  // Default playtime
        List<string> defaultUnlockedCharacters = new List<string> { "Knight" };
        List<WeaponData> defaultWeaponInventory = new List<WeaponData>();
        float defaultHealth = 100f;
        
        // Automatically capture the default positions and statuses of enemies
        List<EnemyData> defaultEnemies = new List<EnemyData>();
        foreach (Entity enemy in Object.FindObjectsByType<Entity>(FindObjectsSortMode.None))
        {
            if (enemy != null)
            {
                defaultEnemies.Add(new EnemyData(enemy.transform.position, true));
            }
        }

        SaveData defaultData = new SaveData(defaultPosition, defaultHealth,defaultHealth, defaultPlayTime, defaultUnlockedCharacters, defaultEnemies, defaultWeaponInventory);
        SaveGame(slot, defaultPosition, defaultHealth,defaultHealth, defaultPlayTime, defaultUnlockedCharacters,defaultEnemies, defaultWeaponInventory);  // Save the default data

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
}