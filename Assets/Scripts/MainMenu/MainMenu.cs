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
    private SaveSystem.SaveData data;

    public void Awake()
    {
        slot = PlayerPrefs.GetInt("SaveSlot", 1);
        if (SaveSystem.SaveExists(slot))
        {
            Debug.Log("Loading Last Save");
            data = SaveSystem.LoadGame(slot);
        }
        else
        {
            data = SaveSystem.InitializeDefaultSave(slot);
            Debug.Log("Creating Initial Save");
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
                saveSlotTexts[i - 1].text = $"Slot {i}: Empty";
            }
        }
    }
    public void Play()
    {
        if(slot > 0 && data!=null)
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
            // Save the slot number to be accessed after scene load
            data = SaveSystem.LoadGame(slot);
            PlayerPrefs.SetInt("SaveSlot", slot);
            Debug.Log("Save Slot Set To: " + slot);
        }
        else
        {
            data = SaveSystem.InitializeDefaultSave(slot);
            DisplaySaveSlots();
            Debug.Log("Creating Default Save");
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
