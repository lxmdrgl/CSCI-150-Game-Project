using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class DeathScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject deathScreenCanvasGO;
    [SerializeField] private string fileName;

    private FileDataHandler dataHandler;
    private string selectedProfileId = "";

    public string MainMenuSceneName;

    public string GameplaySceneName;
    
    
    private void Awake()
    {
        
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);

        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileId();
    }


    public void DeathQuit()
    {
        
        DataPersistenceManager.instance.DeleteProfileData(selectedProfileId);
        /*
        DataPersistenceManager.instance.ChangeSelectedProfileId(selectedProfileId);

        if (!DataPersistenceManager.instance.HasGameData())
        {
            DataPersistenceManager.instance.NewGame();
            DataPersistenceManager.instance.SaveGame();
        }*/

        deathScreenCanvasGO.SetActive(false);

        SceneManager.LoadSceneAsync(MainMenuSceneName);
    }

    public void DeathRestart()
    {
        DataPersistenceManager.instance.NewGame();
        DataPersistenceManager.instance.SaveGame();

        deathScreenCanvasGO.SetActive(false);

        SceneManager.LoadScene(GameplaySceneName);
    }
}
