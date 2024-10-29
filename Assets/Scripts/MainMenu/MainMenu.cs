using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class MainMenu : MonoBehaviour
{
    public string GameSceneName;
    public TMP_Text[] saveSlotTexts;
    private SaveSystem.SaveData data;

    private async void Awake()
    {
        // Initialize unity services
        try
        {
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Services Initialized.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to initialize Unity Services: {e.Message}");
        }

        string playerName = PlayerPrefs.GetString("PlayerName", "Player");
        Debug.Log("Loaded Player Name: " + playerName);  // Verify the name is loaded correctly

        int slot = PlayerPrefs.GetInt("SaveSlot", -1);  // Use -1 for invalid/default case
        if (SaveSystem.SaveExists(slot))
        {
            data = SaveSystem.LoadGame(slot);
        }
        else
        {
            slot = 1;  // Force it to slot 1 if nothing valid is found
            data = SaveSystem.InitializeDefaultSave(slot);
            Debug.Log("Creating Initial Save in Slot 1");
            PlayerPrefs.SetInt("SaveSlot", slot);  // Save the default slot
        }
        DisplaySaveSlots();
    }

    private void DisplaySaveSlots()
    {
        for (int i = 1; i <= 3; i++)
        {
            SaveSystem.SaveData tempData = SaveSystem.LoadGame(i);
            if (tempData != null)
            {
                int roundedTimePlayed = Mathf.FloorToInt(tempData.playTime); 

                // Assuming saveSlotTexts[i-1] corresponds to the TextMeshPro element for save slot i
                saveSlotTexts[i - 1].text = $"Slot {i}:\n" +
                                             $"Time Played: {roundedTimePlayed} seconds\n" +
                                             $"Characters Unlocked: {string.Join(", ", tempData.unlockedCharacters)}";
            }
            else
            {
                saveSlotTexts[i - 1].text = $"Create Save";
            }
        }
    }
    public void Play()
    {
        int slot = PlayerPrefs.GetInt("SaveSlot", -1);  // Default to slot 1 if not found
        Debug.Log($"Selected Slot: {slot}");

        if (slot > 0 && data != null)
        {
            SceneManager.LoadScene(GameSceneName);
        }
        else
        {
            Debug.Log("Cannot Find Valid Save File");
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void LoadSaveSlot(int slot)
    {
        if (SaveSystem.SaveExists(slot))
        {
            data = SaveSystem.LoadGame(slot);
            PlayerPrefs.SetInt("SaveSlot", slot);  // Save the correct slot
            Debug.Log($"Save Slot Set To: {slot}");
        }
        else
        {
            PlayerPrefs.SetInt("SaveSlot", slot);  // Save the correct slot
            data = SaveSystem.InitializeDefaultSave(slot);
            DisplaySaveSlots();
            Debug.Log("Creating Default Save For Slot: "+slot);
        }
    }

    public void DeleteSaveSlot(int slot)
    {
        if(SaveSystem.SaveExists(slot))
        {
            SaveSystem.DeleteSave(slot);
            DisplaySaveSlots();
        }
        else
        {
            Debug.Log("Save Does Not Exist");
        }
    }
}
