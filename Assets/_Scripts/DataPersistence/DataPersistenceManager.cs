using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.PlayerLoop;
public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] public bool disableDataPersistence = false;
    [SerializeField] private string saveDataFileName;
    private FileDataHandler dataHandler;
    private FileDataHandler saveDataHandler;
    private GameData gameData;
    private SaveData saveData;
    private List<IDataPersistence> dataPersistenceObjects;
    public static DataPersistenceManager instance {get; private set;}
    private string selectedProfileId = "";
    private void Awake()
    {
        this.gameObject.transform.SetSiblingIndex(0);

        if (instance != null)
        {
            Debug.LogError("Found more than one data persistence manager in the scene, Destroying the newest one");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.saveDataHandler = new FileDataHandler(Application.persistentDataPath, saveDataFileName);

        InitializeSelectedProfileId();
    }
    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded+=OnSceneLoaded;
    }
    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded-=OnSceneLoaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    public void NewGame()
    {
        this.gameData = new GameData();
        dataHandler.Save( gameData, selectedProfileId);
    }
    public void NewSaveSlot()
    {
        this.saveData = new SaveData();
        saveDataHandler.Save( saveData, selectedProfileId);
    }
    public void LoadGame()
    {
        if(disableDataPersistence)
        {
            return;
        }

        this.gameData = dataHandler.Load<GameData>(selectedProfileId);
        this.saveData = saveDataHandler.Load<SaveData>(selectedProfileId);
        
        // Load data from file
        if(this.gameData == null)
        {
            Debug.Log("No data found, Cannot load");
            return;
        }
        // push data to scripts
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }
    public void SaveGame()
    {
        if(disableDataPersistence)
        {
            return;
        }

        Debug.Log("Saving Game");
        if(this.gameData == null)
        {
            Debug.LogWarning("No Data Found, Cannot save");
            return;
        }
        // pass data to other scripts
        // Save data using file handler
        foreach(IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }

        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        dataHandler.Save(gameData, selectedProfileId);
        saveData.playTime+=gameData.currentLevelTime;
        saveDataHandler.Save(saveData, selectedProfileId);
    }
    public void ChangeSelectedProfileId(string newProfileId)
    {
        this.selectedProfileId = newProfileId;

        LoadGame();
    }
    public void DeleteProfileData(string profileId)
    {
        dataHandler.Delete(profileId);

        InitializeSelectedProfileId();
        LoadGame();
    }
    public void DeleteSaveSlotData(string profileId)
    {
        dataHandler.Delete(profileId);
        saveDataHandler.Delete(profileId);

        InitializeSelectedProfileId();
        LoadGame();
    }
    public void RestartGame(string profileId, string GameplaySceneName)
    {
        dataHandler.Delete(profileId);
        selectedProfileId = profileId;
        NewGame();
    }
    private void InitializeSelectedProfileId()
    {
        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();
    }
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>();       
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
    public float getPlayTime()
    {
        return saveData.playTime;
    }
    public bool HasGameData()
    {
        return gameData != null;
    }
    public bool HasSaveData()
    {
        return saveData != null;
    }
    public Dictionary<string,GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }
    public Dictionary<string,SaveData> GetAllProfilesSaveData()
    {
        return saveDataHandler.LoadAllProfilesSaveData();
    }
    public string GetSelectedProfileId()
    {
        return selectedProfileId;
    }
}
