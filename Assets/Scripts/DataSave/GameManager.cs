using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Player player;
    public string MainMenuSceneName;
    private int slot;
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
                player.playerStats = data.stats;
                Debug.Log($"Loaded player at position {player.Position} from slot {slot}");
            }
            else
            {
                Debug.LogWarning($"No save data found for slot {slot}. Loading defaults.");
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
        PlayerStats stats = player.playerStats;
        float playTime = Time.timeSinceLevelLoad;  // Example: Time spent in the scene
        List<string> unlockedCharacters = new List<string> { "Knight", "Mage" };  // Example list

        SaveSystem.SaveGame(slot, position, stats, playTime+=data.playTime, unlockedCharacters);
        Debug.Log($"Game saved to slot {slot}");

        SceneManager.LoadScene(MainMenuSceneName);
    }
}
