using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Game.CoreSystem.StatsSystem;
using Unity.VisualScripting;
using Game.Weapons; 

namespace Game.CoreSystem
{
public class GameManager : MonoBehaviour
{
    public Player player;
    public string MainMenuSceneName;
    private int slot;
    public Stats stats;
    private SaveSystem.SaveData data;
    private void Awake()
    {
        slot = PlayerPrefs.GetInt("SaveSlot", -1);

        if (slot >= 0)
        {
            // Load player data for the given slot
            data = SaveSystem.LoadGame(slot);
            
            if (data != null)
            {
                player.Position = data.position.ToVector3();
                if (stats == null)
                {
                    Debug.LogError("Stats component is missing on the player.");
                }
                // Ensure that the current health value is applied after initializing the stat
                stats.Health.setHP(data.MaxHealth, data.Currenthealth);
                Debug.Log(stats.Health.CurrentValue);
                Debug.Log($"Loaded player at position {player.Position} from slot {slot}");

                // Load weapon inventory
                WeaponInventory weaponInventory = player.GetComponentInChildren<WeaponInventory>();
                Debug.Log("Loading Weapons...");
                int j = 0;
                if(data.weaponInventory[j])
                {
                    while(data.weaponInventory[j])
                    {
                        Debug.Log("Weapon " + j + " :");
                        Debug.Log(data.weaponInventory[j]);
                        weaponInventory.TrySetWeapon(data.weaponInventory[j], j, out _);
                        j++;
                    }                       
                }

                // Load enemy data
                List<Entity> enemies = new List<Entity>(Object.FindObjectsByType<Entity>(FindObjectsSortMode.None));

                for (int i = 0; i < data.enemies.Count; i++)
                {
                    if (i < enemies.Count)
                    {
                        Entity enemy = enemies[i];
                        SaveSystem.EnemyData enemyData = data.enemies[i];
                        enemy.transform.position = enemyData.position.ToVector3();
                        enemy.gameObject.SetActive(enemyData.isAlive); // Disable if dead
                    }
                }

            }
            else
            {
                slot = 1;
                PlayerPrefs.SetInt("SaveSlot", slot);  // Save the default slot
                data = SaveSystem.InitializeDefaultSave(slot);
                Debug.Log("No Saves Found Creating A Default Save In Slot One");
            }
        }
        else
        {
            Debug.LogWarning("No valid save slot selected.");
        }
    }

    public void SaveQuit()
    {
        Vector3 position = player.Position;
        Stats stats = player.GetComponentInChildren<Stats>();
        float playTime = Time.timeSinceLevelLoad;  // Example: Time spent in the scene
        List<string> unlockedCharacters = new List<string> { "Knight", "Mage" };  // Example list
        float maxHealth = stats.Health.getMaxVal();
        float currentHealth = stats.Health.getCurrentVal();
        // Capture weapon inventory
        WeaponInventory weaponInventory = player.GetComponentInChildren<WeaponInventory>();
        List<WeaponData> weaponDataList = new List<WeaponData>();
        foreach (WeaponData weapon in weaponInventory.weaponData)
        {
            weaponDataList.Add(weapon);
        }

        // Capture the current state of all enemies
        List<SaveSystem.EnemyData> enemiesData = new List<SaveSystem.EnemyData>();
        foreach (Entity enemy in Object.FindObjectsByType<Entity>(FindObjectsSortMode.None))
        {
            bool isAlive = enemy.gameObject.activeInHierarchy;
            enemiesData.Add(new SaveSystem.EnemyData(enemy.transform.position, isAlive));
        }


        SaveSystem.SaveGame(slot, position, currentHealth,maxHealth, playTime+=data.playTime, unlockedCharacters, enemiesData, weaponDataList);
        Debug.Log($"Game saved to slot {slot}");

        Time.timeScale = 1f;

        SceneManager.LoadScene(MainMenuSceneName);
    }
}
}