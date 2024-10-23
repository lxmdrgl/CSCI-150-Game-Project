using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Authentication;

public class MainMenu : MonoBehaviour
{
    public string GameSceneName;
    private int slot;
    public TMP_Text[] saveSlotTexts;

    public void Awake()
    {
        slot = PlayerPrefs.GetInt("SaveSlot", 1);
        DisplaySaveSlots();
    }
    private void DisplaySaveSlots()
    {
        for (int i = 1; i <= 3; i++)
        {
            SaveSystem.SaveData data = SaveSystem.LoadGame(i);
            if (data != null)
            {
                int roundedTimePlayed = Mathf.FloorToInt(data.playTime); 

                // Assuming saveSlotTexts[i-1] corresponds to the TextMeshPro element for save slot i
                saveSlotTexts[i - 1].text = $"Slot {i}:\n" +
                                             $"Time Played: {roundedTimePlayed} seconds\n" +
                                             $"Characters Unlocked: {string.Join(", ", data.unlockedCharacters)}";
            }
            else
            {
                saveSlotTexts[i - 1].text = $"Slot {i}: Empty";
            }
        }
    }
    public void Play()
    {
        if(slot > 0)
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
        // Save the slot number to be accessed after scene load
        PlayerPrefs.SetInt("SaveSlot", slot);
        Debug.Log("Save Slot Set To: " + slot);
    }
}
