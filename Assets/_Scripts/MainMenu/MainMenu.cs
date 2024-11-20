using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using Unity.VisualScripting;


public class MainMenu : MonoBehaviour
{
    [Header("Menu Navigation")]
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;
    [SerializeField] private TextMeshProUGUI savesBtnText;
    public string GameSceneName;

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

        savesBtnText.text = "Save Slot: " + DataPersistenceManager.instance.GetSelectedProfileId();
    }
    public void Play()
    {
        if (DataPersistenceManager.instance.HasGameData())
        {
            // Save current data and load the game
            DataPersistenceManager.instance.SaveGame();
            SceneManager.LoadSceneAsync(GameSceneName);
        }
        else
        {
            // Create new data for the selected profile
            DataPersistenceManager.instance.NewGame();
            DataPersistenceManager.instance.SaveGame();
            SceneManager.LoadSceneAsync(GameSceneName);
        }
    }
    public void OnSavesClicked()
    {
        saveSlotsMenu.ActivateMenu();
        this.DeactivateMenu();
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
    }
    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }


}