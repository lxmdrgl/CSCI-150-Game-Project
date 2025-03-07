using UnityEngine;
using UnityEngine.SceneManagement;
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
    public string generatorScene;

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

        SceneManager.sceneLoaded += OnSceneLoaded;

        string currentSave = DataPersistenceManager.instance.GetSelectedProfileId();

        if(currentSave != "1" && currentSave != "2" && currentSave != "3")
        {
            currentSave = "1";
            DataPersistenceManager.instance.ChangeSelectedProfileId(currentSave);
            if (!DataPersistenceManager.instance.HasGameData())
            {
                DataPersistenceManager.instance.NewGame();
                DataPersistenceManager.instance.SaveGame();
            }
        }   

        savesBtnText.text = "Save Slot: " + currentSave;
    }
    public void Play()
    {
        PlayerPrefs.SetInt("playerCount",1);
        SceneManager.LoadSceneAsync(GameSceneName);

        /*
        if (DataPersistenceManager.instance.HasGameData())
        {
            // Save current data and load the game
            DataPersistenceManager.instance.SaveGame();
            SceneManager.LoadSceneAsync(GameSceneName);
        }
        else
        {
            string currentSave = DataPersistenceManager.instance.GetSelectedProfileId();
            if(currentSave != "1" || currentSave != "2" || currentSave != "3")
            {
                currentSave = "1";
                DataPersistenceManager.instance.ChangeSelectedProfileId(currentSave);
                if (!DataPersistenceManager.instance.HasGameData())
                {
                    DataPersistenceManager.instance.NewGame();
                    DataPersistenceManager.instance.SaveGame();
                }
            }   

            SceneManager.LoadSceneAsync(GameSceneName);
        }
        */
    }

    public void PlayLocalMultiplayer()
    {
        PlayerPrefs.SetInt("playerCount",2);
        SceneManager.LoadSceneAsync(generatorScene);

        /*
        if (DataPersistenceManager.instance.HasGameData())
        {
            // Save current data and load the game
            DataPersistenceManager.instance.SaveGame();
            SceneManager.LoadSceneAsync(generatorScene);
        }
        else
        {
            string currentSave = DataPersistenceManager.instance.GetSelectedProfileId();
            if(currentSave != "1" || currentSave != "2" || currentSave != "3")
            {
                currentSave = "1";
                DataPersistenceManager.instance.ChangeSelectedProfileId(currentSave);
                if (!DataPersistenceManager.instance.HasGameData())
                {
                    DataPersistenceManager.instance.NewGame();
                    DataPersistenceManager.instance.SaveGame();
                }
            }   

            SceneManager.LoadSceneAsync(generatorScene);
        }
        */
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

    private void OnDestroy()
    {
        // Unsubscribe from sceneLoaded event to avoid memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the current scene is the Main Menu
        if (scene.name == "MainMenu") // Replace "MainMenu" with your actual scene name
        {
            string currentSave = DataPersistenceManager.instance.GetSelectedProfileId();

            // Ensure a valid save slot is set
            if (currentSave != "1" && currentSave != "2" && currentSave != "3")
            {
                currentSave = "1";
                DataPersistenceManager.instance.ChangeSelectedProfileId(currentSave);

                if (!DataPersistenceManager.instance.HasGameData())
                {
                    DataPersistenceManager.instance.NewGame();
                    DataPersistenceManager.instance.SaveGame();
                }
            }

            // Set the save slots button text
            savesBtnText.text = "Save Slot: " + currentSave;

        }
    }
}